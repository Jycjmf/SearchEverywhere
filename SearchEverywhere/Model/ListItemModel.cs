using System;
using System.Windows.Media.Imaging;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class ListItemModel : ObservableObject
{
    private DateTime createTime;

    public DateTime CreateTime
    {
        get => createTime;
        set
        {
            SetProperty(ref createTime, value);
            OnPropertyChanged();
        }
    }

    private string size;

    public string Size
    {
        get => size;
        set
        {
            SetProperty(ref size, value);
            OnPropertyChanged();
        }
    }

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

    public ListItemModel(BitmapImage icon, string title, IntPtr hwnd, DateTime createTime, string size)
    {
        Icon = icon;
        Title = title;
        Hwnd = hwnd;
        CreateTime = createTime;
        Size = size;
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