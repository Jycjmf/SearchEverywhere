using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter;

public class WidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            var res = System.Convert.ToInt32(Math.Round(System.Convert.ToDouble(value)));
            if (parameter == null)
                parameter = 0;
            if (res - long.Parse(parameter.ToString()) <= 10) return 90;
            return res - long.Parse(parameter.ToString());
        }

        return 100;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}