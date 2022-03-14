using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model.Wizard;

public class WizardModel : ObservableObject
{
    private Visibility endPageVisibility = Visibility.Collapsed;
    private Visibility enginePageVisibility = Visibility.Collapsed;
    private bool environmentError;

    private bool environmentReady;
    private Visibility exitButtonVisibility = Visibility.Collapsed;
    private string fontColor = "0";

    private bool loadingAnimation = true;
    private Visibility loadingAnimationVisibility = Visibility.Visible;
    private Visibility nextButtonVisibility = Visibility.Collapsed;
    private ObservableCollection<StaticShortcutModel> shortcutList = new();
    private Visibility shortcutPageVisibility = Visibility.Collapsed;
    private int stepIndex;
    private string tips;
    private Visibility welcomePageVisibility = Visibility.Visible;

    public Visibility EndPageVisibility
    {
        get => endPageVisibility;
        set
        {
            SetProperty(ref endPageVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility ShortcutPageVisibility
    {
        get => shortcutPageVisibility;
        set
        {
            SetProperty(ref shortcutPageVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility EnginePageVisibility
    {
        get => enginePageVisibility;
        set
        {
            SetProperty(ref enginePageVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility WelcomePageVisibility
    {
        get => welcomePageVisibility;
        set
        {
            SetProperty(ref welcomePageVisibility, value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<StaticShortcutModel> ShortcutList
    {
        get => shortcutList;
        set
        {
            SetProperty(ref shortcutList, value);
            OnPropertyChanged();
        }
    }

    public string FontColor
    {
        get => fontColor;
        set
        {
            SetProperty(ref fontColor, value);
            OnPropertyChanged();
        }
    }

    public string Tips
    {
        get => tips;
        set
        {
            SetProperty(ref tips, value);
            OnPropertyChanged();
        }
    }

    public Visibility LoadingAnimationVisibility
    {
        get => loadingAnimationVisibility;
        set
        {
            SetProperty(ref loadingAnimationVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility ExitButtonVisibility
    {
        get => exitButtonVisibility;
        set
        {
            SetProperty(ref exitButtonVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility NextButtonVisibility
    {
        get => nextButtonVisibility;
        set
        {
            SetProperty(ref nextButtonVisibility, value);
            OnPropertyChanged();
        }
    }

    public int StepIndex
    {
        get => stepIndex;
        set
        {
            SetProperty(ref stepIndex, value);
            OnPropertyChanged();
        }
    }

    public bool EnvironmentReady
    {
        get => environmentReady;
        set
        {
            SetProperty(ref environmentReady, value);
            OnPropertyChanged();
        }
    }

    public bool EnvironmentError
    {
        get => environmentError;
        set
        {
            SetProperty(ref environmentError, value);
            OnPropertyChanged();
        }
    }

    public bool LoadingAnimation
    {
        get => loadingAnimation;
        set
        {
            SetProperty(ref loadingAnimation, value);
            OnPropertyChanged();
        }
    }
}