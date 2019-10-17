using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Activities.Presentation.Model;
using System.Activities;
using Microsoft.VisualBasic.Activities;
using System.Activities.Expressions;
using System.Windows.Controls;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace Cic.One.Workflow.Design.Converter
{
    public class ComboBoxItemConverter : IValueConverter
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ModelItem modelItem = value as ModelItem;

            if (modelItem != null)
            {
                InArgument<string> inArgument = modelItem.GetCurrentValue() as InArgument<string>;

                if (inArgument != null)
                {
                    Activity<string> expression = inArgument.Expression;
                    VisualBasicValue<string> vbexpression = expression as VisualBasicValue<string>;
                    Literal<string> literal = expression as Literal<string>;

                    if (literal != null)
                    {
                        return literal.Value.Replace("\"","");
                    }
                    else if (vbexpression != null)
                    {
                        return vbexpression.ExpressionText;
                    }
                }
            }
            return null;
        }
        public static String getString(ModelItem modelItem)
        {
           
            if (modelItem != null)
            {
                InArgument<string> inArgument = modelItem.GetCurrentValue() as InArgument<string>;

                if (inArgument != null)
                {
                    Activity<string> expression = inArgument.Expression;
                    VisualBasicValue<string> vbexpression = expression as VisualBasicValue<string>;
                    Literal<string> literal = expression as Literal<string>;

                    if (literal != null)
                    {
                        return literal.Value.Replace("\"", "");
                    }
                    else if (vbexpression != null)
                    {
                        return vbexpression.ExpressionText.Replace("\"", "");
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           // string itemContent = (string)value;
           // return new InArgument<string>(itemContent);
            
            // Convert combo box value to InArgument<string>
            string itemContent = "\""+(string)value+"\"";// (string)((ComboBoxItem)value).Content;
            VisualBasicValue<string> vbArgument = new VisualBasicValue<string>(itemContent);
            InArgument<string> inArgument = new InArgument<string>(vbArgument);
            return inArgument;

        }
    }
}
