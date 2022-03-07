using System.Windows.Media.Imaging;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class MusicTagModel : ObservableObject
{
    private string album;

    private BitmapImage albumCover;

    private string artist;
    private string title;

    public string Title
    {
        get => title;
        set
        {
            SetProperty(ref title, value);
            OnPropertyChanged();
        }
    }

    public string Artist
    {
        get => artist;
        set
        {
            SetProperty(ref artist, value);
            OnPropertyChanged();
        }
    }

    public string Album
    {
        get => album;
        set
        {
            SetProperty(ref album, value);
            OnPropertyChanged();
        }
    }

    public BitmapImage AlbumCover
    {
        get => albumCover;
        set
        {
            SetProperty(ref albumCover, value);
            OnPropertyChanged();
        }
    }
}