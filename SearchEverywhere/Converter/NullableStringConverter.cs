using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter;

public class NullableStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return "";

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return "";

        return value.ToString();
    }
}