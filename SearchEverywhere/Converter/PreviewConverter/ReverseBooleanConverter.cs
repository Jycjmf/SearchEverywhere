using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter.PreviewConverter;

public class ReverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return false;
        return !(bool) value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}