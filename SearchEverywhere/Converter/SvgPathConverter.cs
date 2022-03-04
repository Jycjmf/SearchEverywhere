﻿using System;
using System.Globalization;
using System.Windows.Data;
using SearchEverywhere.Model;

namespace SearchEverywhere.Converter;

public class SvgPathConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || value.GetType() != typeof(ListItemModel)) return null;
        var raw = (ListItemModel) value;
        return raw.SvgIcon;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}