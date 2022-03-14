using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model.Wizard;

public class StaticShortcutModel : ObservableObject
{
    private string shortcut;
    private string tips;

    public StaticShortcutModel(string tips, string shortcut)
    {
        this.tips = tips;
        this.shortcut = shortcut;
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

    public string Shortcut
    {
        get => shortcut;
        set
        {
            SetProperty(ref shortcut, value);
            OnPropertyChanged();
        }
    }
}