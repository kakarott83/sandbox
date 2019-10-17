using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlConfiguratorBase.BO.ContentLogics
{
    public class Quest
    {
        public string TaskDescription { get; set; }
        public Action ActionBeforeTask { get; set; }
        public Action<Configurator> ActionForTask { get; set; }
        public Action ActionAfterTask { get; set; }
    }
}
