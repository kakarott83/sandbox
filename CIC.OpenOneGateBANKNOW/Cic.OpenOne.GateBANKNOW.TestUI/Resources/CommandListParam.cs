using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;


namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    class Command2Param
    {
        public PropertyInfo pancestor { get; set; }
        public Object ancestor { get; set; }
        public string filter { get; set; }
        public TreeViewItem tree { get; set; }
        public IList liste { get; set; }
        public int iliste { get; set; }
        public string vorlabel { get; set; }


    }
    
}
