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
    class CommandParam
    {
        public PropertyInfo pancestor { get; set; }
        public Object ancestor { get; set; }
        public string filter { get; set; }
        public TreeViewItem tree { get; set; }
        public string vorlabel { get; set; }
        public IList larray { get; set; }
        public Grid grid { get; set; }
        public Control control { get; set; }
        public Object label { get; set; }
        public IList liste { get; set; }
        public int tempcrow { get; set; }
    

     


    }
}
