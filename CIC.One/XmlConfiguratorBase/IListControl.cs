using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace XmlConfiguratorBase
{
    public interface IListControl : IDependentControl
    {
        ListBox ListOfWfvEntries { get; }
        ListBox ListOfWfvConfigEntries { get; }
    }
}
