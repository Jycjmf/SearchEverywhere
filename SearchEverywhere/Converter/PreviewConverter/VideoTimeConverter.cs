using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter.PreviewConverter;

public class VideoTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return TimeSpan.Zero;
        var time = (TimeSpan) value;
        return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}