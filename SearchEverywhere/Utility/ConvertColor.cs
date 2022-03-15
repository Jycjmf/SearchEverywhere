using System.Windows.Media;

namespace SearchEverywhere.Utility;

internal class ConvertColor
{
    public static SolidColorBrush ConvertFromString(string color)
    {
        return (SolidColorBrush) new BrushConverter().ConvertFrom(color);
    }
}