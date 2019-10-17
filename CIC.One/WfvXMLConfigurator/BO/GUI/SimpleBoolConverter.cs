using System;
using System.Windows.Data;

namespace WfvXmlConfigurator.BO.GUI
{
    public class SimpleBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return parameter == null;
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
