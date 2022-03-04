using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SearchEverywhere.Model;

namespace SearchEverywhere.Converter;

public class IconSvgVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null && value.GetType() == typeof(ListItemModel) && ((ListItemModel) value).SvgIcon != null)
            return Visibility.Visible;
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}