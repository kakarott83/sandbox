﻿using System;
using System.Windows.Data;

namespace XmlConfiguratorBase.BO.GUI
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

            if ((bool) value)
                return parameter;
            else
                return Binding.DoNothing;
        }
    }
}
