using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter;

public class StatusIconWidthConventer : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return 20;
        return System.Convert.ToInt64(Math.Round(System.Convert.ToDouble(value))) *
               (double.Parse(parameter.ToString()) / 10);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}