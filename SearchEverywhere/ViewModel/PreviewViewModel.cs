using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SearchEverywhere.Model;

namespace SearchEverywhere.ViewModel;

public class PreviewViewModel : ObservableRecipient
{
    private ObservableCollection<KeyValueModel> configList = new();
    private int fontSize = 14;
    private string textFile;

    public PreviewViewModel()
    {
        PreviewTxt(@"C:\Users\Jycjmf\Desktop\小工具\Bandicam\novel.txt");
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
}