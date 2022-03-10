using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class PreviewModel : ObservableObject
{
    private AnimationModel animationConfig = new();
    private ObservableCollection<KeyValueModel> configList = new();
    private string currentImage;
    private CurrentTimeModel currentVideoInfo = new(TimeSpan.Zero, TimeSpan.Zero);
    private VisibilityModel elementVisibility = new();
    private int fontSize = 14;

    private ObservableCollection<ListboxItemModel> imageItemList = new();
    private string imagePath;

    private MusicTagModel musicTag = new();

    private int selectIndex;
    private ObservableCollection<ObservableCollection<string>> sheetContentMatrix = new();
    private VideoSliderModel sliderInfo = new(0, 1);
    private string textFile;
    private int titleHeight;
    private string videoPath;
    private ObservableCollection<WordContentModel> wordContentList = new();

    public int TitleHeight
    {
        get => titleHeight;
        set
        {
            SetProperty(ref titleHeight, value);
            OnPropertyChanged();
        }
    }

    public string ImagePath
    {
        get => imagePath;
        set
        {
            SetProperty(ref imagePath, value);
            OnPropertyChanged();
        }
    }

    public VisibilityModel ElementVisibility
    {
        get => elementVisibility;
        set
        {
            SetProperty(ref elementVisibility, value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ObservableCollection<string>> SheetContentMatrix
    {
        get => sheetContentMatrix;
        set
        {
            SetProperty(ref sheetContentMatrix, value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<WordContentModel> WordContentList
    {
        get => wordContentList;
        set
        {
            SetProperty(ref wordContentList, value);
            OnPropertyChanged();
        }
    }

    public int SelectIndex
    {
        get => selectIndex;
        set
        {
            SetProperty(ref selectIndex, value);
            CurrentImage = ImageItemList[value].Path;
            OnPropertyChanged();
        }
    }

    public string CurrentImage
    {
        get => currentImage;
        set
        {
            SetProperty(ref currentImage, value);
            OnPropertyChanged();
        }
    }


    public ObservableCollection<ListboxItemModel> ImageItemList
    {
        get => imageItemList;
        set
        {
            SetProperty(ref imageItemList, value);
            OnPropertyChanged();
        }
    }

    public AnimationModel AnimationConfig
    {
        get => animationConfig;
        set
        {
            SetProperty(ref animationConfig, value);
            OnPropertyChanged();
        }
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

    public ObservableCollection<KeyValueModel> ConfigList
    {
        get => configList;
        set
        {
            SetProperty(ref configList, value);
            OnPropertyChanged();
        }
    }
}