using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using SearchEverywhere.Utility;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SearchEverywhere.ViewModel;

public class SettingViewModel : ObservableRecipient
{
    private readonly ConfigurationUtility config = Ioc.Default.GetService<ConfigurationUtility>();
    private bool autoRun;
    private string autoStartIcon;
    private Visibility colorPickerVisibility = Visibility.Hidden;
    private int fontsize = 14;
    private WhichColor preChangeColor;
    private int searchDelay = 200;
    private SolidColorBrush selectedColorBrush = new(Colors.Red);
    private bool slienceRun;

    public SettingViewModel()
    {
        InitSetting();
        ChangeAccentCommand = new RelayCommand(() =>
        {
            preChangeColor = WhichColor.Accent;
            ColorPickerVisibility = Visibility.Visible;
        });
        ChangeThemeCommand = new RelayCommand(() =>
        {
            preChangeColor = WhichColor.Theme;
            ColorPickerVisibility = Visibility.Visible;
        });
        ConfirmColorCommand = new RelayCommand(() =>
        {
            if (preChangeColor == WhichColor.Accent)
            {
                Application.Current.Resources["AccentBrush"] = SelectedColorBrush;
                Application.Current.Resources["SecondaryBorderBrush"] = SelectedColorBrush;
                Application.Current.Resources["PrimaryBrush"] = SelectedColorBrush;
            }
            else
            {
                Application.Current.Resources["ThemeColor"] = SelectedColorBrush;
            }

            ColorPickerVisibility = Visibility.Hidden;
        });

        CancelColorCommand = new RelayCommand(() => { ColorPickerVisibility = Visibility.Hidden; });
        SaveCommand = new RelayCommand(() =>
        {
            try
            {
                var auto = new AutoRunUtility();
                config.appSettings.AccentColor = Application.Current.Resources["AccentBrush"].ToString();
                config.appSettings.ThemeColor = Application.Current.Resources["ThemeColor"].ToString();
                config.appSettings.SearchDelay = SearchDelay;
                config.appSettings.SilentWindows = SlienceRun;
                config.appSettings.StartWithWindows = AutoRun;
                config.appSettings.FontSize = Fontsize;
                if (AutoRun)
                    auto.SetAutoStart();
                else
                    auto.DeleteAutoStart();
                AutoStartIcon = AutoRun ? "/img/success.svg" : "/img/error.svg";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception catch");
            }
        });
        RestCommand = new RelayCommand(() =>
        {
            Application.Current.Resources["AccentBrush"] = ConvertColor.ConvertFromString("#FC9D10");
            Application.Current.Resources["SecondaryBorderBrush"] = ConvertColor.ConvertFromString("#FC9D10");
            Application.Current.Resources["PrimaryBrush"] = ConvertColor.ConvertFromString("#FC9D10");
            Application.Current.Resources["ThemeColor"] = ConvertColor.ConvertFromString("#51459E");
            Fontsize = 14;
            SearchDelay = 200;
        });
    }

    public string AutoStartIcon
    {
        get => autoStartIcon;
        set
        {
            SetProperty(ref autoStartIcon, value);
            OnPropertyChanged();
        }
    }

    public ICommand RestCommand { get; }
    public ICommand SaveCommand { get; }

    public int SearchDelay
    {
        get => searchDelay;
        set
        {
            SetProperty(ref searchDelay, value);
            OnPropertyChanged();
        }
    }

    public int Fontsize
    {
        get => fontsize;
        set
        {
            SetProperty(ref fontsize, value);
            OnPropertyChanged();
        }
    }

    public bool AutoRun
    {
        get => autoRun;
        set
        {
            SetProperty(ref autoRun, value);
            OnPropertyChanged();
        }
    }

    public bool SlienceRun
    {
        get => slienceRun;
        set
        {
            SetProperty(ref slienceRun, value);
            OnPropertyChanged();
        }
    }

    public ICommand ConfirmColorCommand { get; }
    public ICommand CancelColorCommand { get; }

    public Visibility ColorPickerVisibility
    {
        get => colorPickerVisibility;
        set
        {
            SetProperty(ref colorPickerVisibility, value);
            OnPropertyChanged();
        }
    }


    public SolidColorBrush SelectedColorBrush
    {
        get => selectedColorBrush;
        set
        {
            SetProperty(ref selectedColorBrush, value);
            OnPropertyChanged();
        }
    }

    public ICommand ChangeThemeCommand { get; }
    public ICommand ChangeAccentCommand { get; }

    private void InitSetting()
    {
        Application.Current.Resources["AccentBrush"] = ConvertColor.ConvertFromString(config.appSettings.AccentColor);
        Application.Current.Resources["SecondaryBorderBrush"] =
            ConvertColor.ConvertFromString(config.appSettings.AccentColor);
        Application.Current.Resources["PrimaryBrush"] = ConvertColor.ConvertFromString(config.appSettings.AccentColor);
        Application.Current.Resources["ThemeColor"] = ConvertColor.ConvertFromString(config.appSettings.ThemeColor);
        Fontsize = config.appSettings.FontSize;
        SearchDelay = config.appSettings.SearchDelay;
        AutoRun = config.appSettings.StartWithWindows;
        AutoStartIcon = autoRun ? "/img/success.svg" : "/img/error.svg";
        SlienceRun = config.appSettings.SilentWindows;
    }

    private enum WhichColor
    {
        Accent,
        Theme
    }
}