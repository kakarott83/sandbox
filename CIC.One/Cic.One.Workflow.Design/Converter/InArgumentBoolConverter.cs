using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Activities;
using System.Activities.Expressions;

namespace Cic.One.Workflow.Design.Converter
{
    public class InArgumentBoolConverter : IValueConverter
    {

        public object Convert(

            object value,

            Type targetType,

            object parameter,

            System.Globalization.CultureInfo culture)
        {

            if (value is InArgument<bool>)
            {

                Activity<bool> expression = ((InArgument<bool>)value).Expression;

                if (expression is Literal<bool>)
                {

                    return ((Literal<bool>)expression).Value;

                }

            }



            return null;

        }



        public object ConvertBack(

            object value,

            Type targetType,

            object parameter,

            System.Globalization.CultureInfo culture)
        {

            if (value is bool)
            {

                return new InArgument<bool>(new Literal<bool>((bool)value));

            }

            else
            {

                return null;

            }

        }

    }
}
