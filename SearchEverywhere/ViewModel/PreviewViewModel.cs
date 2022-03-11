using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        PauseCommand = new RelayCommand(str =>
            WeakReferenceMessenger.Default.Send(new PlayStatusModel(PlayStatusModel.Status.Play, false),
                "PausePlayToken"));
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
                    case PreviewUiElementModel.PreviewUiElement.Video:
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Video);
                        PreviewVideo(msg.Path);
                        break;
                    case PreviewUiElementModel.PreviewUiElement.Audio:
                        SwitchLayout(msg.IsSmallWindow);
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Video);
                        PreviewAudio(msg.Path);
                        break;
                    case PreviewUiElementModel.PreviewUiElement.Excel:
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Excel);
                        PreviewExcel(msg.Path);
                        break;
                    case PreviewUiElementModel.PreviewUiElement.Ppt:
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Ppt);
                        PreviewPowerPoint(msg.Path);
                        break;
                    case PreviewUiElementModel.PreviewUiElement.Word:
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Word);
                        PreviewWord(msg.Path);
                        break;
                    case PreviewUiElementModel.PreviewUiElement.Config:
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Config);
                        PreviewConfig(msg.Path);
                        break;
                    case PreviewUiElementModel.PreviewUiElement.Text:
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Text);
                        PreviewTxt(msg.Path);
                        break;
                    case PreviewUiElementModel.PreviewUiElement.Unknown:
                        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Unknown);
                        CurrentPreviewModel.FirstTipsVisibility = Visibility.Collapsed;
                        CurrentPreviewModel.SecTipsVisibility = Visibility.Visible;
                        CurrentPreviewModel.PromptText = "暂不支持的格式";
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
        ChangeVisibility(PreviewUiElementModel.PreviewUiElement.Unknown);
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

    private void SwitchLayout(bool isSmall)
    {
        if (isSmall)
        {
            CurrentPreviewModel.TitleFontSize = 18;
            CurrentPreviewModel.SubTitleFontSize = 15;
            CurrentPreviewModel.AlbumMargin = 35;
            CurrentPreviewModel.SliderMargin = 10;
        }
        else
        {
            CurrentPreviewModel.TitleFontSize = 25;
            CurrentPreviewModel.SubTitleFontSize = 20;
            CurrentPreviewModel.AlbumMargin = 100;
            CurrentPreviewModel.SliderMargin = 100;
        }
    }

    private void PreviewTxt(string path)
    {
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = false;
        using var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var bs = new BufferedStream(fs);
        using var sr = new StreamReader(bs, Encoding.Default, true);
        var line = string.Empty;
        var resList = new List<string>();
        while ((line = sr.ReadLine()) != null && resList.Count < 200) resList.Add(line);
        var res = string.Empty;
        resList.ForEach(x => res += $"{x}\n");
        CurrentPreviewModel.TextFile = res;
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = true;
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
        var res = WeakReferenceMessenger.Default.Send(new PlayStatusModel(PlayStatusModel.Status.Play, true),
            "PausePlayToken");
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
        CurrentPreviewModel.ImageItemList.Clear();
        CurrentPreviewModel.CurrentImage = "";
        var pptReader = new PowerPointReader();
        var res = await pptReader.ReadPowerPointAsync(path);
        if (res.IsSuccess == false)
            return;
        var files = Directory.EnumerateFiles(res.Result)
            .OrderBy(x => Convert.ToInt32(Path.GetFileNameWithoutExtension(x)));
        if (files.Count() == 0)
            return;
        for (var i = 0; i < files.Count(); i++)
            CurrentPreviewModel.ImageItemList.Add(new ListboxItemModel(i, files.ElementAt(i)));
        CurrentPreviewModel.SelectIndex = 0;
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = true;
    }

    private async Task PreviewWord(string path)
    {
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = false;
        var wordReader = new WordReader();
        var res = await wordReader.ReadWordAsync(path);
        if (!res.IsSuccess)
            return;
        CurrentPreviewModel.WordContentList.Clear();
        foreach (var each in res.Result)
            CurrentPreviewModel.WordContentList.Add(each);
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = true;
    }

    private async Task PreviewExcel(string path)
    {
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = false;
        var excelReader = new ExcelReader();
        var res = await excelReader.ReadExcel(path);
        CurrentPreviewModel.SheetContentMatrix.Clear();
        foreach (var each in res)
            CurrentPreviewModel.SheetContentMatrix.Add(each);
        OnPropertyChanged("CurrentPreviewModel");
        CurrentPreviewModel.AnimationConfig.LoadingAnimation = true;
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
                CurrentPreviewModel.ElementVisibility.UnknownVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Video:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.UnknownVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Config:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.UnknownVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Text:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.UnknownVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Excel:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.UnknownVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Word:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.UnknownVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Ppt:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Visible;
                CurrentPreviewModel.ElementVisibility.UnknownVisibility = Visibility.Collapsed;
                break;
            case PreviewUiElementModel.PreviewUiElement.Unknown:
                CurrentPreviewModel.ElementVisibility.ImageVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.VideoVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ConfigVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.TextVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.ExcelVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.WordVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.PptVisibility = Visibility.Collapsed;
                CurrentPreviewModel.ElementVisibility.UnknownVisibility = Visibility.Visible;
                break;
        }
    }
}