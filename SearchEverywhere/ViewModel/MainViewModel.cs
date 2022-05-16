using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using SearchEverywhere.Model;
using SearchEverywhere.Model.Message;
using SearchEverywhere.Utility;
using SearchEverywhere.View;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SearchEverywhere.ViewModel;

public class MainViewModel : ObservableRecipient
{
    private const uint SW_RESTORE = 0x09;
    private static readonly Everything.Everything everything = new();
    private readonly ConfigurationUtility config = Ioc.Default.GetService<ConfigurationUtility>();
    private readonly IView iview;
    private readonly PreviewUtility previewUtility = new();
    private readonly ProcessUtility processUtility;
    private readonly Timer searchPendingTimer;
    private ListItemModel currentApp;
    private MainUiElementModel currentUIVisibility = new();
    private bool firstButtonChecked = true;
    private string keyword;
    private ObservableCollection<ListItemModel> runningAppsList = new();

    private ObservableCollection<ListItemModel> searchResultList = new();
    private Dictionary<int, bool> selectedItem = new() {{0, false}, {1, true}, {2, false}, {3, false}};
    private int selectIndex;

    public MainViewModel(IView iView)
    {
        CheckNeedWizard();
        var permission = new CheckPermissionUtility();
        if (!permission.IsAdministrator())
            permission.ElevatePermission();
        processUtility = new ProcessUtility();
        processUtility.TrackNewProcess();
        searchPendingTimer = new Timer(CheckSearchKeyword, null, 200, 0);
        everything.InitSearch();
        ChangePageCommand = new RelayCommand<string>(x =>
        {
            switch (Convert.ToInt32(x))
            {
                case 0:
                    SwitchPage(MainUiElement.SearchView);
                    break;
                case 1:
                    SwitchPage(MainUiElement.WizardView);
                    break;
                case 2:
                    SwitchPage(MainUiElement.AboutView);
                    break;
                case 3:
                    SwitchPage(MainUiElement.SettingView);
                    break;
            }
        });
        InputTabCommand = new RelayCommand(() =>
        {
            var index = SelectedItem.First(i => i.Value).Key;
            if (index == 2)
            {
                SelectedItem[2] = false;
                SelectedItem[0] = true;
            }
            else
            {
                SelectedItem[index++] = false;
                SelectedItem[index] = true;
            }

            StartAllSearch(keyword);
            OnPropertyChanged("SelectedItem");
        });
        EnterCommand = new RelayCommand(() =>
        {
            if (CurrentApp == null)
                return;
            var style = GetWindowLong(currentApp.Hwnd, -16);
            if ((style & 0x20000000) == 0x20000000) ShowWindow(currentApp.Hwnd, SW_RESTORE);
            SetForegroundWindow(currentApp.Hwnd);
        });
        UpCommand = new RelayCommand(() =>
        {
            if (SelectIndex == 0)
                SelectIndex = SearchResultList.Count - 1;
            else
                SelectIndex--;
        });
        DownCommand = new RelayCommand(() =>
        {
            if (SelectIndex == SearchResultList.Count - 1)
                SelectIndex = 0;
            else
                SelectIndex++;
        });
        PreviewCommand = new RelayCommand(() =>
        {
            if (CurrentApp != null)
                previewUtility.TryToPreview(CurrentApp.Path);
        });
        FullscreenCommand = new RelayCommand(() =>
            {
                WeakReferenceMessenger.Default.Send("false", "IsSmallWindowToken");
                iView.ShowWindow();
            }
        );
        WeakReferenceMessenger.Default.Register<MainViewModel, List<ListItemModel>, string>(this, "InitAppListToken",
            InitAppListHandler);
        WeakReferenceMessenger.Default.Register<MainViewModel, RefreshProcessModel, string>(this, "RefreshApplistToken",
            RefreshAppListHandler);
        WeakReferenceMessenger.Default.Register<MainViewModel, ChangeMainUiVisibilityModel, string>(this,
            "SwitchPageToken",
            (r, msg) =>
            {
                config.appSettings.FirstUse = false;
                SwitchPage(msg.Element);
            });
        WeakReferenceMessenger.Default.Register<MainViewModel, string, string>(this, "GoToSearchToken",
            (r, msg) => { SwitchPage(MainUiElement.SearchView); });
        WeakReferenceMessenger.Default.Register<MainViewModel, string, string>(this, "CheckFirstButtonToken",
            (r, msg) => { FirstButtonChecked = true; });
        ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
    }

    public bool FirstButtonChecked
    {
        get => firstButtonChecked;
        set
        {
            SetProperty(ref firstButtonChecked, value);
            OnPropertyChanged();
        }
    }

    public MainUiElementModel CurrentUIVisibility
    {
        get => currentUIVisibility;
        set
        {
            SetProperty(ref currentUIVisibility, value);
            OnPropertyChanged();
        }
    }


    public ICommand FullscreenCommand { get; }


    public int SelectIndex
    {
        get => selectIndex;
        set
        {
            SetProperty(ref selectIndex, value);
            if (searchResultList.Count != 0 && value != -1)
                CurrentApp = searchResultList[value];
            else
                CurrentApp = null;
            OnPropertyChanged();
        }
    }

    public ListItemModel CurrentApp
    {
        get => currentApp;
        set
        {
            SetProperty(ref currentApp, value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ListItemModel> SearchResultList
    {
        get => searchResultList;
        set
        {
            SetProperty(ref searchResultList, value);
            OnPropertyChanged();
        }
    }

    public string Keyword
    {
        get => keyword;
        set
        {
            SetProperty(ref keyword, value);
            StartAllSearch(value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ListItemModel> RunningAppsList
    {
        get => runningAppsList;
        set
        {
            SetProperty(ref runningAppsList, value);
            CurrentApp = runningAppsList.First();
            OnPropertyChanged();
        }
    }

    public Dictionary<int, bool> SelectedItem
    {
        get => selectedItem;
        set
        {
            SetProperty(ref selectedItem, value);
            OnPropertyChanged();
        }
    }

    public ICommand ExitCommand { get; }
    public ICommand InputTabCommand { get; }
    public ICommand EnterCommand { get; }
    public ICommand UpCommand { get; }
    public ICommand DownCommand { get; }
    public ICommand PreviewCommand { get; }
    public ICommand ChangePageCommand { get; }

    private void StartAllSearch(string value)
    {
        if (SelectedItem[0] || SelectedItem[1])
            StartProcessSearch(value);
        if (SelectedItem[0] || SelectedItem[2])
            searchPendingTimer.Change(200, 0);
        if (searchResultList.Any())
            CurrentApp = SearchResultList.First();
    }

    private void SwitchPage(MainUiElement page)
    {
        switch (page)
        {
            case MainUiElement.SearchView:
                CurrentUIVisibility.SearchVisibility = Visibility.Visible;
                CurrentUIVisibility.WizardVisibility = Visibility.Collapsed;
                CurrentUIVisibility.AboutVisibility = Visibility.Collapsed;
                CurrentUIVisibility.SettingVisibility = Visibility.Collapsed;
                return;
            case MainUiElement.WizardView:
                CurrentUIVisibility.SearchVisibility = Visibility.Collapsed;
                CurrentUIVisibility.WizardVisibility = Visibility.Visible;
                CurrentUIVisibility.AboutVisibility = Visibility.Collapsed;
                CurrentUIVisibility.SettingVisibility = Visibility.Collapsed;
                return;
            case MainUiElement.AboutView:
                CurrentUIVisibility.SearchVisibility = Visibility.Collapsed;
                CurrentUIVisibility.WizardVisibility = Visibility.Collapsed;
                CurrentUIVisibility.AboutVisibility = Visibility.Visible;
                CurrentUIVisibility.SettingVisibility = Visibility.Collapsed;
                return;
            case MainUiElement.SettingView:
                CurrentUIVisibility.SearchVisibility = Visibility.Collapsed;
                CurrentUIVisibility.WizardVisibility = Visibility.Collapsed;
                CurrentUIVisibility.AboutVisibility = Visibility.Collapsed;
                CurrentUIVisibility.SettingVisibility = Visibility.Visible;
                return;
        }
    }

    private void CheckNeedWizard()
    {
        if (config.appSettings.FirstUse)
            SwitchPage(MainUiElement.WizardView);
        else
            SwitchPage(MainUiElement.SearchView);
    }

    private void InitAppListHandler(MainViewModel recipient, List<ListItemModel> message)
    {
        message.ForEach(x =>
        {
            RunningAppsList.Add(x);
            SearchResultList.Add(x);
            if (RunningAppsList.Any())
                CurrentApp = runningAppsList.First();
        });
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int ShowWindow(IntPtr hWnd, uint Msg);

    [DllImport("User32.dll")]
    public static extern int SetForegroundWindow(IntPtr hWnd);

    private void CheckSearchKeyword(object state)
    {
        if (string.IsNullOrWhiteSpace(Keyword))
            return;
        StartFileSearch(Keyword);
    }


    private void StartProcessSearch(string keyword)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SearchResultList.Clear();
                    foreach (var each in runningAppsList) SearchResultList.Add(each);
                    SelectIndex = 0;
                });
            keyword = keyword?.Replace(@"\", @"\\").Replace(".", @"\.").Replace("|", "");
            if (keyword == null) return;
            var raw = runningAppsList
                .Where(x => Regex.Matches(x.Title, keyword, RegexOptions.IgnoreCase).Count > 0)
                .ToList();
            var temp = new List<ListItemModel>();
            foreach (var each in raw)
                temp.Add(new ListItemModel(each.Icon,
                    Regex.Replace(each.Title, keyword, $"|~S~|{keyword.Replace(@"\.", ".")}|~E~|",
                        RegexOptions.IgnoreCase), each.Hwnd,
                    each.CreateTime, each.Size, each.Path, each.Extension, each.SvgIcon, each.ProcessId));
            Application.Current.Dispatcher.Invoke(() =>
            {
                SearchResultList.Clear();
                temp.ForEach(x => SearchResultList.Add(x));
                SelectIndex = 0;
            });
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString(), "Exception catch");
        }
    }

    private async Task StartFileSearch(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return;
        Application.Current.Dispatcher.Invoke(() => SearchResultList.Clear());
        var res = await everything.SearchFileAsync(keyword);
        if (res?.Result.Count == 0)
            return;
        Application.Current.Dispatcher.Invoke(() =>
        {
            var tempList = SearchResultList.Where(x => x.Path == null).ToList();
            res.Result.Take(20).ToList().ForEach(x => tempList.Add(x));
            SearchResultList.Clear();
            tempList.ForEach(x => { SearchResultList.Add(x); });
        });
        CurrentApp = searchResultList.First();
    }

    private void RefreshAppListHandler(MainViewModel recipient, RefreshProcessModel message)
    {
        if (message.IsAdd)
        {
            RunningAppsList.Add(message.ProcessInfo);
            StartProcessSearch(Keyword);
        }
        else
        {
            if (RunningAppsList.Any(x => x.ProcessId == message.ProcessInfo.ProcessId))
            {
                var temp = RunningAppsList.First(x => x.ProcessId == message.ProcessInfo.ProcessId);
                RunningAppsList.Remove(temp);
                StartProcessSearch(Keyword);
            }
        }
    }


    private void RegisterHotkey()
    {
    }
}