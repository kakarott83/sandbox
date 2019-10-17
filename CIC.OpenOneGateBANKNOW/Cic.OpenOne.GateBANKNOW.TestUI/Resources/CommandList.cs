using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;


namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    class CommandListe : ICommand
    {
        public void Execute(Object parameter)
        {
            test(parameter as CommandParam);
        }

        public bool CanExecute(Object parameter)
        {
            return true;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        private void test(CommandParam parameter)
        {      
            GeneratorInput gi = new GeneratorInput();
            PropertyInfo pItem = parameter.pancestor.PropertyType.GetProperty("Item");
            Object obj = Activator.CreateInstance(pItem.PropertyType);
            string labeltemp = gi.getLabel(parameter.pancestor);
            parameter.liste.Add(obj);               
            Grid tempgrid = new Grid();
            gi.GenerationInitial(obj, parameter.pancestor.PropertyType.ToString(), parameter.ancestor, parameter.pancestor, parameter.vorlabel, parameter.filter, tempgrid);        
            parameter.tree.Items.Add(tempgrid);
            parameter.tree.IsExpanded = true;
            parameter.pancestor.SetValue(parameter.ancestor, parameter.liste, null);
        }
    }
}



