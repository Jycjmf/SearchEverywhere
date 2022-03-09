using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SearchEverywhere.Model;

namespace SearchEverywhere.Converter.PreviewConverter;

public class AlignmentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return TextAlignment.Left;
        var res = (WordContentModel) value;
        return res.Align;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}