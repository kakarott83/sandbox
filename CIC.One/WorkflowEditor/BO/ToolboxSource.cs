using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Workflows.BO
{
    public class ToolboxSourceCollection : ObservableCollection<ToolboxSource>
    { }
    public class ToolboxSource
    {
        public string TargetCategory { get; set; }
        public Type AllSiblingsOf { get; set; }
    }
}
