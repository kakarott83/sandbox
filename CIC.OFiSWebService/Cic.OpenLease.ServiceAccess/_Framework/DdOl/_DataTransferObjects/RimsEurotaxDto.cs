using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class RimsEurotaxDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public RimDto[] FrontRims
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public RimDto[] RearRims
        {
            get;
            set;
        }
    }
}
