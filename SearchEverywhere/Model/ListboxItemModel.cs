using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class ListboxItemModel : ObservableObject
{
    private int index;
    private string path;

    public ListboxItemModel(int index, string path)
    {
        this.index = index;
        this.path = path;
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

    public string Path
    {
        get => path;
        set
        {
            SetProperty(ref path, value);
            OnPropertyChanged();
        }
    }
}