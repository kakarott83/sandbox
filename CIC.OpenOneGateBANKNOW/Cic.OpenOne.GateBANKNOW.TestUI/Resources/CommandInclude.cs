using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    /// <summary>
    /// Command Include Class
    /// </summary>
    public class CommandInclude : ICommand
    {
        /// <summary>
        /// Execute Command Include
        /// </summary>
        /// <param name="parameter">Parameter</param>
         public void Execute(Object parameter)
        {
            test(parameter as CommandParam);
        }

        /// <summary>
        /// Can Execute Abfrage
        /// </summary>
        /// <param name="parameter">Parameter</param>
        /// <returns>Rückgabe true</returns>
        public bool CanExecute(Object parameter)
        {
            return true;
        }

#pragma warning disable 0067
        /// <summary>
        /// Event Handler
        /// </summary>
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        private void test(CommandParam parameter)
        {

            if (parameter.control != null)
            {
                parameter.control.IsEnabled = true;
                //   parameter.control.GetType().GetProperty("Text").SetValue(parameter.control, null, null);
            }
            if (parameter.pancestor.GetValue(parameter.ancestor, null) == null)
            {
                if (!(parameter.pancestor.PropertyType.GetProperty("Count") != null || parameter.pancestor.PropertyType.IsArray || parameter.pancestor.PropertyType.IsPrimitive || parameter.pancestor.PropertyType.IsEnum || parameter.pancestor.PropertyType.IsValueType || parameter.pancestor.PropertyType.ToString().Equals("System.String")))
                {
                    GeneratorInput gi = new GeneratorInput();

                    Object obj = Activator.CreateInstance(parameter.pancestor.PropertyType);
                    parameter.pancestor.SetValue(parameter.ancestor, obj, null);
                    Type t = obj.GetType();
                    PropertyInfo[] pi = t.GetProperties();
                    foreach (PropertyInfo p in pi)
                    {
                        gi.GenerationInitial(p.GetValue(obj, null), p.PropertyType.ToString(), obj, p, parameter.vorlabel + parameter.pancestor.Name, parameter.filter, parameter.grid);
                    }
                }


                if (parameter.tree != null)
                {
                    parameter.tree.Items.Add(parameter.grid);
                    parameter.tree.IsEnabled = true;
                    parameter.tree.IsExpanded = true;
                }
            }
        }
            
        }

    
}
