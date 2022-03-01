using System;
using System.Windows.Media.Imaging;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class ListItemModel : ObservableObject
{
    private BitmapImage icon;

    private string title;

    private IntPtr hwnd;

    public IntPtr Hwnd
    {
        get => hwnd;
        set
        {
            SetProperty(ref hwnd, value);
            OnPropertyChanged();
        }
    }

    public ListItemModel(BitmapImage icon, string title, IntPtr hwnd)
    {
        Icon = icon;
        Title = title;
        Hwnd = hwnd;
    }

    public BitmapImage Icon
    {
        get => icon;
        set
        {
            SetProperty(ref icon, value);
            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => title;
        set
        {
            SetProperty(ref title, value);
            OnPropertyChanged();
        }
    }
}