using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter.PreviewConverter;

public class ZoomConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var res = double.Parse(value.ToString()) / 7;
        if (res < 1)
            res = 1;
        return (int) res;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return 500;
    }
}