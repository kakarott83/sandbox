using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlConfiguratorBase
{
    public interface IStatusBar
    {
        string TextLeft { get; set; }
        string TextRight { get; set; }
        string InvalidXmlWarning { get; set; }
        bool ProgressVisible { set; }
    }
}
