using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WfvXmlConfigurator.BO.GUI
{
    public class CustomCommands
    {
        public static readonly RoutedUICommand OpenFile = ApplicationCommands.Open;
        public static readonly RoutedUICommand OpenDatabase = new RoutedUICommand("Daten aus Datenbank laden", "LoadFromDatabase", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.O, ModifierKeys.Alt) });
        public static readonly RoutedUICommand ShowDependencies = new RoutedUICommand("Liste der Konfigurationselemente anzeigen, von denen das aktuelle Konfigurationselement abhängt", "ShowDependencies", typeof(CustomCommands));
    }
}
