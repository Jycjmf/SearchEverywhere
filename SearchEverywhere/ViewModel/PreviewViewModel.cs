using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FlyleafLib.Controls.WPF;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using SearchEverywhere.Model;
using SearchEverywhere.Utility.office;
using TagLib;
using File = System.IO.File;

namespace SearchEverywhere.ViewModel;

public class PreviewViewModel : ObservableRecipient
{
    private PreviewModel currentPreviewModel = new();
    private bool isManualChangeSlider;

    public PreviewViewModel()
    {
        PauseCommand = new RelayCommand(str => WeakReferenceMessenger.Default.Send("play", "PausePlayToken"));
        WeakReferenceMessenger.Default.Register<PreviewViewModel, CurrentTimeModel, string>(this,
            "ChangeVideoTimeToken", (r, msg) =>
            {
                if (isManualChangeSlider == false)
                {
                    CurrentPreviewModel.CurrentVideoInfo.CurrentTime = msg.CurrentTime;
                    CurrentPreviewModel.CurrentVideoInfo.TotalTime = msg.TotalTime;
                    CurrentPreviewModel.SliderInfo.CurrentValue = msg.CurrentTime.TotalSeconds;
                    CurrentPreviewModel.SliderInfo.MaxValue = msg.TotalTime.TotalSeconds;
                }
            });
        JumpTimeCommand = new RelayCommand(r =>
        {
            Console.WriteLine(r);
            isManualChangeSlider = true;
            WeakReferenceMessenger.Default.Send(
                new VideoSliderModel(Convert.ToDouble(r),
                    CurrentPreviewModel.CurrentVideoInfo.TotalTime.TotalSeconds), "JumpToTimeCommand");
            isManualChangeSlider = false;
        });
        MuteCommand = new RelayCommand(e => WeakReferenceMessenger.Default.Send("mute", "MuteToken"));
        WeakReferenceMessenger.Default.Register<PreviewViewModel, PreviewUiElementModel, string>(this, "StartPreview",
            (r, msg) =>
            {
                switch (msg.ElementName)
                {
                    case PreviewUiElementModel.PreviewUiElement.Image:
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Image);
                        CurrentPreviewModel.ImagePath = msg.Path;
                        break;
                }
            });
        WeakReferenceMessenger.Default.Register<PreviewViewModel, string, string>(this, "ConvertFullDisplayMode",
            (r, msg) =>
            {
                if (msg == "true")
                    CurrentPreviewModel.TitleHeight = 40;
                else
                    CurrentPreviewModel.TitleHeight = 0;
            });
        // PreviewVideo(@"C:\Users\Jycjmf\Desktop\Flyleaf-master\Sample.mp4");
        // PreviewAudio(@"G:\小米5备份\音乐\佐橋俊彦 - 正义と自由.mp3");
        //PreviewAudio(@"D:\音乐\后来的我们-五月天.flac");
        //PreviewPowerPoint(@"C:\Users\Jycjmf\Desktop\1.pptx");
        //   PreviewPowerPoint(@"C:\Users\Jycjmf\Desktop\个人简历\金雨晨-院长奖学金答辩_1.pptx");
        //PreviewWord(@"C:\Users\Jycjmf\Desktop\作业一 (1).docx");
        //PreviewExcel(@"C:\Users\Jycjmf\Desktop\大创.xls");
        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Image);
    }

    public PreviewModel CurrentPreviewModel
    {
        get => currentPreviewModel;
        set
        {
            SetProperty(ref currentPreviewModel, value);
            OnPropertyChanged();
        }
    }


    public ICommand JumpTimeCommand { get; }
    public ICommand MuteCommand { get; }


    public ICommand PauseCommand { get; }

    private void PreviewTxt(string path)
    {
        CurrentPreviewModel.TextFile = File.ReadAllText(path);
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

        CurrentPreviewModel.ConfigList.Clear();
        formatList.ForEach(x => CurrentPreviewModel.ConfigList.Add(x));
    }

    private void PreviewVideo(string path)
    {
        CurrentPreviewModel.VideoPath = path;
        var res = WeakReferenceMessenger.Default.Send("play", "PausePlayToken");
    }

    private void PreviewAudio(string path)
    {
        CurrentPreviewModel.VideoPath = path;
        var tagFile = TagLib.File.Create(path);
        var res = tagFile.Tag.Pictures;
        if (res.Length > 0)
            CurrentPreviewModel.MusicTag.AlbumCover = ConvertIPictureToBitmapImage(res.First());
        else
            CurrentPreviewModel.MusicTag.AlbumCover = new BitmapImage(new Uri(@"..\img\cover.png", UriKind.Relative));
        CurrentPreviewModel.MusicTag.Title = tagFile.Tag.Title;
        if (tagFile.Tag.Artists.Length > 0)
            foreach (var each in tagFile.Tag.Artists)
                CurrentPreviewModel.MusicTag.Artist = $"{each} ";
        CurrentPreviewModel.MusicTag.Album = tagFile.Tag.Album;
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

    private async Task PreviewPowerPoint(string path)
    {
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = false;
        var pptReader = new PowerPointReader();
        var res = await pptReader.ReadPowerPointAsync(path);
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = true;
        if (res.IsSuccess == false)
            return;
        Console.WriteLine(Path.GetFileNameWithoutExtension(path));
        var files = Directory.EnumerateFiles(res.Result)
            .OrderBy(x => Convert.ToInt32(Path.GetFileNameWithoutExtension(x)));
        CurrentPreviewModel.ImageItemList.Clear();
        if (files.Count() == 0)
            return;
        for (var i = 0; i < files.Count(); i++)
            CurrentPreviewModel.ImageItemList.Add(new ListboxItemModel(i, files.ElementAt(i)));
        CurrentPreviewModel.SelectIndex = 0;
    }

    private async Task PreviewWord(string path)
    {
        var wordReader = new WordReader();
        var res = await wordReader.ReadWordAsync(path);
        if (!res.IsSuccess)
            return;
        CurrentPreviewModel.WordContentList.Clear();
        foreach (var each in res.Result)
            CurrentPreviewModel.WordContentList.Add(each);
    }

    private async Task PreviewExcel(string path)
    {
        var excelReader = new ExcelReader();
        var res = await excelReader.ReadExcel(path);
        CurrentPreviewModel.SheetContentMatrix.Clear();
        foreach (var each in res)
            CurrentPreviewModel.SheetContentMatrix.Add(each);
        OnPropertyChanged("CurrentPreviewModel");
    }

    private void ChangeVisibility(PreviewUiElementModel.PreviewUiElement element)
    {
        switch (element)
        {
            case PreviewUiElementModel.PreviewUiElement.Image:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Video:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Config:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Text:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Excel:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Word:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Ppt:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Visible;
                break;
        }
    }
}