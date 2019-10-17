using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.P000001.Common
{
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    [System.Serializable]
    public class SimpleSearchParam
    {
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public string SearchPattern
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public SearchBy SearchBy
        {
            get;
            set;
        }
    }
}
