using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;


namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    class CommandArrayParam
    {
        public PropertyInfo pancestor { get; set; }
        public Object ancestor { get; set; }
        public string filter { get; set; }
        public TreeViewItem tree { get; set; }
        public Array array { get; set; }
        public int iarray { get; set; }
        public string vorlabel { get; set; }
        public IList larray { get; set; }


    }
}
