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
        var t = new DataTable();
        if (value is not ObservableCollection<ObservableCollection<string>> array)
            return t;
        if (array.Count == 0)
            return t;
        for (var i = 0; i < array.Max(x => x.Count) + 1; i++) t.Columns.Add(i == 0 ? "Index" : i.ToString());
        for (var eachRowIndex = 0; eachRowIndex < array.Count; eachRowIndex++)
        {
            var newRow = t.NewRow();
            newRow[0] = eachRowIndex;
            for (var colIndex = 0; colIndex < array[eachRowIndex].Count; colIndex++)
                newRow[colIndex + 1] = array[eachRowIndex][colIndex];

            t.Rows.Add(newRow);
        }

        return t.DefaultView;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}