using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class RimDto
    {
        #region Properties

        
        [System.Runtime.Serialization.DataMember]
        public string Code
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Manufacturer
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? Price
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Width
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Diameter
        {
            get;
            set;
        }
        #endregion
    }
}
