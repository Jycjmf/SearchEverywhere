using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using SearchEverywhere.Model;
using SearchEverywhere.Model.Wizard;
using SearchEverywhere.Utility;

namespace SearchEverywhere.ViewModel;

public class WizardViewModel : ObservableRecipient
{
    private readonly ConfigurationUtility config = Ioc.Default.GetService<ConfigurationUtility>();
    private readonly int maxPage = 0;
    private int currentPage;
    private WizardModel currentWizardModel = new();

    public WizardViewModel()
    {
        CloseCommand = new RelayCommand(() => Application.Current.Shutdown());
        GoBackCommand = new RelayCommand(() =>
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                SwitchPage(CurrentPage);
                return;
            }

            if (!config.appSettings.FirstUse)
            {
                WeakReferenceMessenger.Default.Send("", "GoToSearchToken");
                WeakReferenceMessenger.Default.Send("", "CheckFirstButtonToken");
            }
        });
        GoToEngineCommand = new RelayCommand(() =>
            {
                CurrentPage = 1;
                InitEnvironment();
                SwitchPage(1);
            }
        );
        GoToShortcutCommand = new RelayCommand(() =>
        {
            CurrentPage = 2;
            SwitchPage(2);
        });
        GoToEndCommand = new RelayCommand(() =>
        {
            CurrentPage = 3;
            SwitchPage(3);
        });
        CurrentWizardModel.ShortcutList.Add(new StaticShortcutModel("全屏预览", "Alt+Enter"));
        CurrentWizardModel.ShortcutList.Add(new StaticShortcutModel("选择搜索框", "Ctrl+s"));
        CurrentWizardModel.ShortcutList.Add(new StaticShortcutModel("切换搜索源", "Tab"));
        CurrentWizardModel.ShortcutList.Add(new StaticShortcutModel("显示主界面", "Shift+Shift"));
        CurrentWizardModel.ShortcutList.Add(new StaticShortcutModel("文件快速预览", "Ctrl+Enter"));
        CurrentWizardModel.ShortcutList.Add(new StaticShortcutModel("切换选中程序到前台", "Enter"));
        CurrentWizardModel.Tips = "请稍后";
        CurrentWizardModel.EnvironmentError = false;
        GoToMainPageCommand = new RelayCommand(() =>
        {
            WeakReferenceMessenger.Default.Send(new ChangeMainUiVisibilityModel(MainUiElement.SearchView),
                "SwitchPageToken");
            WeakReferenceMessenger.Default.Send("", "CheckFirstButtonToken");
        });
    }

    public ICommand GoToMainPageCommand { get; }

    public ICommand CloseCommand { get; }

    public ICommand GoBackCommand { get; }

    public ICommand GoToEngineCommand { get; }
    public ICommand GoToShortcutCommand { get; }
    public ICommand GoToEndCommand { get; }


    public int CurrentPage
    {
        get => currentPage;
        set
        {
            SetProperty(ref currentPage, value);
            OnPropertyChanged();
        }
    }

    public WizardModel CurrentWizardModel
    {
        get => currentWizardModel;
        set
        {
            SetProperty(ref currentWizardModel, value);
            OnPropertyChanged();
        }
    }


    private void SwitchPage(int index)
    {
        switch (index)
        {
            case 0:
                CurrentWizardModel.WelcomePageVisibility = Visibility.Visible;
                CurrentWizardModel.EnginePageVisibility = Visibility.Collapsed;
                CurrentWizardModel.ShortcutPageVisibility = Visibility.Collapsed;
                CurrentWizardModel.EndPageVisibility = Visibility.Collapsed;
                break;
            case 1:
                CurrentWizardModel.WelcomePageVisibility = Visibility.Collapsed;
                CurrentWizardModel.EnginePageVisibility = Visibility.Visible;
                CurrentWizardModel.ShortcutPageVisibility = Visibility.Collapsed;
                CurrentWizardModel.EndPageVisibility = Visibility.Collapsed;
                break;
            case 2:
                CurrentWizardModel.WelcomePageVisibility = Visibility.Collapsed;
                CurrentWizardModel.EnginePageVisibility = Visibility.Collapsed;
                CurrentWizardModel.ShortcutPageVisibility = Visibility.Visible;
                CurrentWizardModel.EndPageVisibility = Visibility.Collapsed;
                break;
            case 3:
                CurrentWizardModel.WelcomePageVisibility = Visibility.Collapsed;
                CurrentWizardModel.EnginePageVisibility = Visibility.Collapsed;
                CurrentWizardModel.ShortcutPageVisibility = Visibility.Collapsed;
                CurrentWizardModel.EndPageVisibility = Visibility.Visible;
                break;
        }
    }

    private async Task InitEnvironment()
    {
        var isEverythingRunning = false;
        await Task.Delay(4000);
        await Task.Run(() => { isEverythingRunning = CheckEverythingRunning(); });
        CurrentWizardModel.LoadingAnimationVisibility = Visibility.Collapsed;
        if (isEverythingRunning)
        {
            CurrentWizardModel.NextButtonVisibility = Visibility.Visible;
            CurrentWizardModel.ExitButtonVisibility = Visibility.Collapsed;
            CurrentWizardModel.EnvironmentReady = true;
            CurrentWizardModel.EnvironmentError = false;
            CurrentWizardModel.FontColor = "#2db84d";
            CurrentWizardModel.Tips = "依赖检测完成，未发现异常。";
        }
        else
        {
            CurrentWizardModel.NextButtonVisibility = Visibility.Collapsed;
            CurrentWizardModel.ExitButtonVisibility = Visibility.Visible;
            CurrentWizardModel.EnvironmentReady = false;
            CurrentWizardModel.EnvironmentError = true;
            CurrentWizardModel.FontColor = "#db3340";
            CurrentWizardModel.Tips = "未检测到Everything进程，请安装并启动Everything后重试。";
        }
    }

    private bool CheckEverythingRunning()
    {
        var processes = Process.GetProcesses();
        return processes.Any(each => Regex.Matches(each.ProcessName, "everything", RegexOptions.IgnoreCase).Count > 0);
    }
}