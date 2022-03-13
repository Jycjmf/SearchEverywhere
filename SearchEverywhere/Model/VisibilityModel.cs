using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class VisibilityModel : ObservableObject
{
    private Visibility albumCoverVisibility = Visibility.Collapsed;
    private Visibility configVisibility = Visibility.Collapsed;

    private Visibility excelVisibility = Visibility.Collapsed;
    private Visibility imageVisibility = Visibility.Collapsed;

    private Visibility pptVisibility = Visibility.Collapsed;
    private Visibility textVisibility = Visibility.Collapsed;
    private Visibility unknownVisibility = Visibility.Visible;

    private Visibility videoVisibility = Visibility.Collapsed;

    private Visibility wordVisibility = Visibility.Collapsed;

    public Visibility AlbumCoverVisibility
    {
        get => albumCoverVisibility;
        set
        {
            SetProperty(ref albumCoverVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility UnknownVisibility
    {
        get => unknownVisibility;
        set
        {
            SetProperty(ref unknownVisibility, value);
            OnPropertyChanged();
        }
    }


    public Visibility ImageVisibility
    {
        get => imageVisibility;
        set
        {
            SetProperty(ref imageVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility ConfigVisibility
    {
        get => configVisibility;
        set
        {
            SetProperty(ref configVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility TextVisibility
    {
        get => textVisibility;
        set
        {
            SetProperty(ref textVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility VideoVisibility
    {
        get => videoVisibility;
        set
        {
            SetProperty(ref videoVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility PptVisibility
    {
        get => pptVisibility;
        set
        {
            SetProperty(ref pptVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility WordVisibility
    {
        get => wordVisibility;
        set
        {
            SetProperty(ref wordVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility ExcelVisibility
    {
        get => excelVisibility;
        set
        {
            SetProperty(ref excelVisibility, value);
            OnPropertyChanged();
        }
    }
}