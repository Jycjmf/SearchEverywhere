using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public enum MainUiElement
{
    SearchView,
    AboutView,
    WizardView,
    SettingView
}

public class MainUiElementModel : ObservableObject
{
    private Visibility aboutVisibility = Visibility.Collapsed;
    private Visibility searchVisibility = Visibility.Collapsed;
    private Visibility settingVisibility = Visibility.Collapsed;
    private Visibility wizardVisibility = Visibility.Visible;

    public Visibility AboutVisibility
    {
        get => aboutVisibility;
        set
        {
            SetProperty(ref aboutVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility SettingVisibility
    {
        get => settingVisibility;
        set
        {
            SetProperty(ref settingVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility WizardVisibility
    {
        get => wizardVisibility;
        set
        {
            SetProperty(ref wizardVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility SearchVisibility
    {
        get => searchVisibility;
        set
        {
            SetProperty(ref searchVisibility, value);
            OnPropertyChanged();
        }
    }
}