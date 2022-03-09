using System.Windows;
using System.Windows.Media.Imaging;

namespace SearchEverywhere.Model;

public class WordContentModel
{
    public WordContentModel()
    {
    }

    public WordContentModel(string text, BitmapImage img, HorizontalAlignment align)
    {
        Text = text;
        Img = img;
        Align = align;
    }

    public string Text { get; set; }
    public BitmapImage Img { get; set; }

    public HorizontalAlignment Align { get; set; }
}