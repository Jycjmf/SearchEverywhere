using System;
using System.Windows.Media.Imaging;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class ListItemModel : ObservableObject
{
    private DateTime createTime;
    private string extension;

    private IntPtr hwnd;

    private BitmapImage icon;
    private string path;

    private string size;
    private string svgIcon;

    private string title;

    public ListItemModel(BitmapImage icon, string title, IntPtr hwnd, DateTime createTime, string size, string path,
        string extension, string svgIcon)
    {
        Icon = icon;
        Title = title;
        Hwnd = hwnd;
        CreateTime = createTime;
        Size = size;
        Path = path;
        Extension = extension;
        SvgIcon = svgIcon;
    }

    public string SvgIcon
    {
        get => svgIcon;
        set
        {
            SetProperty(ref svgIcon, value);
            OnPropertyChanged();
        }
    }

    public string Extension
    {
        get => extension;
        set
        {
            SetProperty(ref extension, value);
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

    public DateTime CreateTime
    {
        get => createTime;
        set
        {
            SetProperty(ref createTime, value);
            OnPropertyChanged();
        }
    }

    public string Size
    {
        get => size;
        set
        {
            SetProperty(ref size, value);
            OnPropertyChanged();
        }
    }


    public IntPtr Hwnd
    {
        get => hwnd;
        set
        {
            SetProperty(ref hwnd, value);
            OnPropertyChanged();
        }
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