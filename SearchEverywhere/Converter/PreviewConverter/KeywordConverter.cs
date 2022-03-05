using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter.PreviewConverter;

public class KeywordConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        var input = value.ToString();
        if (string.IsNullOrWhiteSpace(input))
            return null;
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}