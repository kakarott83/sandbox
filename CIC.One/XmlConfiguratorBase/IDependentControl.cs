using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlConfiguratorBase
{
    public interface IDependentControl
    {
        ICoreControl CoreControl { set; }
        bool IsEnabled { set; }
    }
}
