using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter;

public class NormalCurrentTitleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        return value.ToString().Replace(@"|~S~|", "").Replace(@"|~E~|", "");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}