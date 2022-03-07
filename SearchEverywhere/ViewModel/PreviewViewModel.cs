using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FlyleafLib.Controls.WPF;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using SearchEverywhere.Model;
using TagLib;
using File = System.IO.File;

namespace SearchEverywhere.ViewModel;

public class PreviewViewModel : ObservableRecipient
{
    private ObservableCollection<KeyValueModel> configList = new();
    private CurrentTimeModel currentVideoInfo = new(TimeSpan.Zero, TimeSpan.Zero);
    private int fontSize = 14;
    private bool isManualChangeSlider;
    private MusicTagModel musicTag = new();
    private VideoSliderModel sliderInfo = new(0, 1);
    private string textFile;
    private string videoPath;

    public PreviewViewModel()
    {
        PauseCommand = new RelayCommand(str => WeakReferenceMessenger.Default.Send("play", "PausePlayToken"));
        WeakReferenceMessenger.Default.Register<PreviewViewModel, CurrentTimeModel, string>(this,
            "ChangeVideoTimeToken", (r, msg) =>
            {
                if (isManualChangeSlider == false)
                {
                    CurrentVideoInfo.CurrentTime = msg.CurrentTime;
                    CurrentVideoInfo.TotalTime = msg.TotalTime;
                    SliderInfo.CurrentValue = msg.CurrentTime.TotalSeconds;
                    SliderInfo.MaxValue = msg.TotalTime.TotalSeconds;
                }
            });
        JumpTimeCommand = new RelayCommand(r =>
        {
            Console.WriteLine(r);
            isManualChangeSlider = true;
            WeakReferenceMessenger.Default.Send(
                new VideoSliderModel(Convert.ToDouble(r),
                    CurrentVideoInfo.TotalTime.TotalSeconds), "JumpToTimeCommand");
            isManualChangeSlider = false;
        });
        MuteCommand = new RelayCommand(e => WeakReferenceMessenger.Default.Send("mute", "MuteToken"));
        // PreviewVideo(@"C:\Users\Jycjmf\Desktop\Flyleaf-master\Sample.mp4");
        // PreviewAudio(@"G:\小米5备份\音乐\佐橋俊彦 - 正义と自由.mp3");
        PreviewAudio(@"D:\音乐\后来的我们-五月天.flac");
    }

    public MusicTagModel MusicTag
    {
        get => musicTag;
        set
        {
            SetProperty(ref musicTag, value);
            OnPropertyChanged();
        }
    }


    public string VideoPath
    {
        get => videoPath;
        set
        {
            SetProperty(ref videoPath, value);
            OnPropertyChanged();
        }
    }

    public ICommand JumpTimeCommand { get; }
    public ICommand MuteCommand { get; }

    public VideoSliderModel SliderInfo
    {
        get => sliderInfo;
        set
        {
            SetProperty(ref sliderInfo, value);
            OnPropertyChanged();
        }
    }

    public CurrentTimeModel CurrentVideoInfo
    {
        get => currentVideoInfo;
        set
        {
            SetProperty(ref currentVideoInfo, value);
            OnPropertyChanged();
        }
    }

    public int FontSize
    {
        get => fontSize;
        set
        {
            SetProperty(ref fontSize, value);
            OnPropertyChanged();
        }
    }

    public string TextFile
    {
        get => textFile;
        set
        {
            SetProperty(ref textFile, value);
            OnPropertyChanged();
        }
    }

    public string EqualStr { get; set; } = "=";

    public ObservableCollection<KeyValueModel> ConfigList
    {
        get => configList;
        set
        {
            SetProperty(ref configList, value);
            OnPropertyChanged();
        }
    }

    public ICommand PauseCommand { get; }

    private void PreviewTxt(string path)
    {
        TextFile = File.ReadAllText(path);
    }

    private void PreviewConfig(string path)
    {
        var res = File.ReadAllLines(path).ToList();

        var formatList = new List<KeyValueModel>();
        foreach (var x in res)
        {
            var key = Regex.Match(x, @"(\[.*\])|(.*(?=\=))").Value;
            if (string.IsNullOrWhiteSpace(key))
                continue;
            var value = Regex.Match(x, @"(\=).*").Value;
            formatList.Add(new KeyValueModel(key, value));
        }

        ConfigList.Clear();
        formatList.ForEach(x => ConfigList.Add(x));
    }

    private void PreviewVideo(string path)
    {
        VideoPath = path;
        var res = WeakReferenceMessenger.Default.Send("play", "PausePlayToken");
    }

    private void PreviewAudio(string path)
    {
        VideoPath = path;
        var tagFile = TagLib.File.Create(path);
        var res = tagFile.Tag.Pictures;
        if (res.Length > 0)
            MusicTag.AlbumCover = ConvertIPictureToBitmapImage(res.First());
        else
            MusicTag.AlbumCover = new BitmapImage(new Uri(@"..\img\cover.png", UriKind.Relative));
        MusicTag.Title = tagFile.Tag.Title;
        if (tagFile.Tag.Artists.Length > 0)
            foreach (var each in tagFile.Tag.Artists)
                musicTag.Artist = $"{each} ";
        MusicTag.Album = tagFile.Tag.Album;
    }

    private BitmapImage ConvertIPictureToBitmapImage(IPicture pic)
    {
        var ms = new MemoryStream(pic.Data.Data);
        ms.Seek(0, SeekOrigin.Begin);

        // ImageSource for System.Windows.Controls.Image
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.StreamSource = ms;
        bitmap.EndInit();
        return bitmap;
    }
}