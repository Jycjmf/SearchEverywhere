using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SearchEverywhere.Converter.PreviewConverter;

public class MatrixToDataViewConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var array = value as ObservableCollection<ObservableCollection<string>>;
        if (array == null)
            return null;
        if (array.Count == 0)
            return null;
        var t = new DataTable();
        for (var i = 0; i < array.First().Count; i++) t.Columns.Add(i.ToString());
        foreach (var eachRow in array)
        {
            var newRow = t.NewRow();
            for (var i = 0; i < eachRow.Count; i++) newRow[i] = eachRow[i];

            t.Rows.Add(newRow);
        }

        return t.DefaultView;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}