using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.P000001.Common
{
    [System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    [System.CLSCompliant(true)]
    public class ObViewDto
    {
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String id { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String bezeichnung { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String aufbau { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String getriebe { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String treibstoff { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String emission { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String baumonat { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String baujahr { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String typengenehmigung { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String fahrzeugart { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String marke { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String modell { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String schwacke { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public double neupreisnetto { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public double neupreisbrutto { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public int art { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public long leistung { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String baubismonat { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String baubisjahr { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String path { get; set; }
        public int level { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String bezeichnung2 { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String bezeichnung3 { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public long sysob { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public long sysobart { get; set; }
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public String vin { get; set; }
        
    }
}
