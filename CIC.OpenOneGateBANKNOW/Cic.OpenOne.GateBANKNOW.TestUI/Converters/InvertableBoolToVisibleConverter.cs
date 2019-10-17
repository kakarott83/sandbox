using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Globalization;
using System.Windows.Data;
namespace Cic.OpenOne.GateBANKNOW.TestUI.Converters
{
    /// <summary>
    /// Invertable Bool To Visible Converter
    /// </summary>
    public class InvertableBoolToVisibleConverter : IValueConverter
    {
        private bool _inverted = false;

        /// <summary>
        /// Inverted
        /// </summary>
        public bool Inverted
        {
            get { return _inverted; }
            set { _inverted = value; }
        }

        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return DependencyProperty.UnsetValue;

            return ((bool)value ^ Inverted) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// conver Back
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType,
                object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                return DependencyProperty.UnsetValue;

            return ((Visibility)value) == Visibility.Visible;
        }
    }
}