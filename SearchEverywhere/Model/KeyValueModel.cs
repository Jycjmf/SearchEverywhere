using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class KeyValueModel : ObservableObject
{
    private string eachvalue;
    private string key;


    public KeyValueModel(string key, string eachvalue)
    {
        this.key = key;
        this.eachvalue = eachvalue;
    }

    public string Key
    {
        get => key;
        set
        {
            SetProperty(ref key, value);
            OnPropertyChanged();
        }
    }

    public string Eachvalue
    {
        get => eachvalue;
        set
        {
            SetProperty(ref eachvalue, value);
            OnPropertyChanged();
        }
    }
}