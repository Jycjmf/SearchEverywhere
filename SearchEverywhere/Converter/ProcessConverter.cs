using System;
using System.Globalization;
using System.Windows.Data;
using SearchEverywhere.Model;

namespace SearchEverywhere.Converter;

public class ProcessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || value.GetType() != typeof(ListItemModel)) return null;
        var eachProcess = (ListItemModel) value;
        return eachProcess?.Title;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}