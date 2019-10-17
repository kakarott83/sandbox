using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Collections;


namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    /// <summary>
    /// Kommando Listen-Klasse
    /// </summary>
    public class CommandArray : ICommand
    {
        /// <summary>
        /// Kommando Ausführen
        /// </summary>
        /// <param name="parameter">Parameter</param>
        public void Execute(Object parameter)
        {
            test(parameter as CommandParam);
        }

        /// <summary>
        /// Prüfung ob ausführung möglich ist
        /// </summary>
        /// <param name="parameter">Parameter</param>
        /// <returns>Rückgabewert</returns>
        public bool CanExecute(Object parameter)
        {
            return true;
        }

#pragma warning disable 0067
        /// <summary>
        /// Eventhandler deklaration
        /// </summary>
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        /// <summary>
        /// Test-Prozedur
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void test(CommandParam parameter)
        {   
            int i = -1;
            string vlabel = "";
            GeneratorInput gi = new GeneratorInput();
            Object arrayElement = null;
            if (parameter.pancestor.GetValue(parameter.ancestor, null) != null)
            {
                i = 1 + (int)parameter.pancestor.GetValue(parameter.ancestor, null).GetType().GetProperty("Length").GetValue(parameter.pancestor.GetValue(parameter.ancestor, null), null);
                string labeltemp = gi.getLabel(parameter.pancestor);
                
                Array array = Array.CreateInstance(parameter.pancestor.PropertyType.GetElementType(), i);
                arrayElement = Activator.CreateInstance(parameter.pancestor.PropertyType.GetElementType());
                parameter.larray.Add(arrayElement);
                int j = -1;
                foreach (var arrElement in parameter.larray)
                {
                    if (arrElement != null)
                    {
                        j = j + 1;
            
                        array.SetValue(arrElement, j);
                    }
                }
             
                parameter.pancestor.SetValue(parameter.ancestor, array, null);
               
            }
            else
            {
                i = 1;
                string labeltemp = gi.getLabel(parameter.pancestor);

                Type listType = typeof(List<>).MakeGenericType(new[] { parameter.pancestor.PropertyType.GetElementType() });
                IList larray = (IList)Activator.CreateInstance(listType);
                
                Array array = Array.CreateInstance(parameter.pancestor.PropertyType.GetElementType(), i);
                arrayElement = Activator.CreateInstance(parameter.pancestor.PropertyType.GetElementType());
                array.SetValue(arrayElement, 0);
                larray.Add(arrayElement);
                parameter.pancestor.SetValue(parameter.ancestor, array, null);
                parameter.larray = larray;
            }
                  
            Grid tempgrid = new Grid();
            gi.GenerationInitial(arrayElement, parameter.pancestor.PropertyType.ToString(), parameter.ancestor, parameter.pancestor, vlabel, parameter.filter,tempgrid);
            parameter.tree.Items.Add(tempgrid);
            parameter.tree.IsExpanded = true;
           
     }
    }
}
