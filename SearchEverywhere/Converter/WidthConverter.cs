using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchEverywhere.Converter
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var res = int.Parse(value.ToString());
                return res - 12;
            }

            return 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
