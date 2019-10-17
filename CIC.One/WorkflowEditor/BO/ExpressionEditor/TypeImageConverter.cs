using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace Workflows.BO.ExpressionEditor
{

   
    public class TypeImageConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType()!=typeof(TreeNodes.NodeTypes))
                return null;

            DrawingBrush result = null;
            object resImage = null;
            Workflows.BO.ExpressionEditor.TreeNodes.NodeTypes inputValue = (Workflows.BO.ExpressionEditor.TreeNodes.NodeTypes)Enum.Parse(typeof(TreeNodes.NodeTypes), value.ToString());
            switch (inputValue)
            {
                case TreeNodes.NodeTypes.Namespace:
                    resImage = System.Windows.Application.Current.Resources["ISNamespace"];
                    break;
                case TreeNodes.NodeTypes.Interface:
                    resImage = System.Windows.Application.Current.Resources["ISInterface"];
                    break;
                case TreeNodes.NodeTypes.Class:
                    resImage = System.Windows.Application.Current.Resources["ISClass"];
                    break;
                case TreeNodes.NodeTypes.Method:
                    resImage = System.Windows.Application.Current.Resources["ISMethod"];
                    break;
                case TreeNodes.NodeTypes.Property:
                    resImage = System.Windows.Application.Current.Resources["ISProperty"];
                    break;
                case TreeNodes.NodeTypes.Field:
                    resImage = System.Windows.Application.Current.Resources["ISField"];
                    break;
                case TreeNodes.NodeTypes.Enum:
                    resImage = System.Windows.Application.Current.Resources["ISEnum"];
                    break;
                case TreeNodes.NodeTypes.ValueType:
                    resImage = System.Windows.Application.Current.Resources["ISStructure"];
                    break;
                case TreeNodes.NodeTypes.Event:
                    resImage = System.Windows.Application.Current.Resources["ISEvent"];
                    break;
                case TreeNodes.NodeTypes.Primitive:
                    break;
                default:
                    break;
            }

            if (resImage != null)
            {
                result = new DrawingBrush { Drawing = new ImageDrawing((ImageSource)resImage, new Rect(0, 0, 16, 16)) };
            }
            return result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}