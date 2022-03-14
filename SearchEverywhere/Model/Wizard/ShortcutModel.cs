using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model.Wizard;

public class ShortcutModel : ObservableObject
{
    private bool alt;

    private bool ctrl;
    private Key eachKey;
    private int index;
    private bool shift;
    private string tips;

    public ShortcutModel(int index, string tips, Key eachKey, bool ctrl, bool shift, bool alt)
    {
        this.index = index;
        this.tips = tips;
        this.eachKey = eachKey;
        this.ctrl = ctrl;
        this.shift = shift;
        this.alt = alt;
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

    public int Index
    {
        get => index;
        set
        {
            SetProperty(ref index, value);
            OnPropertyChanged();
        }
    }

    public Key EachKey
    {
        get => eachKey;
        set
        {
            SetProperty(ref eachKey, value);
            OnPropertyChanged();
        }
    }

    public bool Ctrl
    {
        get => ctrl;
        set
        {
            SetProperty(ref ctrl, value);
            OnPropertyChanged();
        }
    }

    public bool Alt
    {
        get => alt;
        set
        {
            SetProperty(ref alt, value);
            OnPropertyChanged();
        }
    }

    public bool Shift
    {
        get => shift;
        set
        {
            SetProperty(ref shift, value);
            OnPropertyChanged();
        }
    }
}