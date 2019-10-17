using System;
using System.Globalization;
using System.Windows.Data;

namespace Cic.OpenOne.GateBANKNOW.TestUI.Converters
{
    class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? d = null;
            string s = System.Convert.ToString(value);
            if (value != null && s != "0")
                d = System.DateTime.ParseExact(s, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            return d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = 0;
            DateTime date = (DateTime)value;
            string temp = date.ToString("yyyyMMdd");
            if (temp != null)
                d = System.Convert.ToDouble(temp);
            return d;
        }
    }
}