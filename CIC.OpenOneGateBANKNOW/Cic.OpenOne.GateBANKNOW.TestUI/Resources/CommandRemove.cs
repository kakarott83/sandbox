using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;




namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    class CommandRemove : ICommand
    {
        public void Execute(Object parameter)
        {

            remove(parameter as CommandParam);
        }

        public bool CanExecute(Object parameter)
        {
            return true;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        private void remove(CommandParam parameter)
        {
            //Type underlyingType = null;
            if (!(parameter.pancestor.PropertyType.IsArray))
            {

                if (parameter.pancestor.PropertyType.IsGenericType && parameter.pancestor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) || parameter.pancestor.PropertyType.ToString().Equals("System.String"))
                {
                    parameter.pancestor.SetValue(parameter.ancestor, null, null);
                    if (parameter.control != null && parameter.control.GetType().GetProperty("Text") != null)
                    {
                        parameter.control.GetType().GetProperty("Text").SetValue(parameter.control, null, null);
                    }


                    if (parameter.control != null && parameter.control.IsEnabled) parameter.control.IsEnabled = false;

                    if (parameter.label != null)
                    {
                        Control s = parameter.label as Control;
                        if (s.IsEnabled) s.IsEnabled = false;
                    }
                }
                if (parameter.tree != null)
                {
                    parameter.pancestor.SetValue(parameter.ancestor, null, null);
                    parameter.tree.Items.Clear();
                    if (parameter.liste!=null) parameter.liste.Clear();
                    
                }
            }
            else
            {

                /*      int a = 0; 
                      object[] mParam = new object[] { a };


               
                      MethodInfo mInfo = typeof(System.Linq.Enumerable).GetMethod("ToList").MakeGenericMethod(typeof(object));
                      var lista = mInfo.Invoke(null, new object[] {parameter.pancestor.GetValue(parameter.ancestor, null)});
                      MethodInfo mInfoRemove = typeof(System.Collections.IList).GetMethod("RemoveAt");

                      mInfoRemove.Invoke(lista, new object [] {0});
               //     MethodInfo mInfo = typeof(System.Linq.Enumerable).GetMethod("ElementAt").MakeGenericMethod(typeof(object));

              //      object tempObj = mInfo.Invoke(null, new object[] {parameter.pancestor.GetValue(parameter.ancestor, null), 0});

               //     tempObj = null;
                      MethodInfo mInfoToArray = typeof(System.Linq.Enumerable).GetMethod("ToArray").MakeGenericMethod(typeof(object));
                      var array = mInfoToArray.Invoke(null, new object[] {lista});
                      */

                parameter.pancestor.SetValue(parameter.ancestor, null, null);
                parameter.larray = null;
                
                parameter.tree.Items.Clear();


            }




        }
    }

}
