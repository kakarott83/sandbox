namespace Cic.OpenLease.ServiceAccess.DdOl
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 public   class ObArtZinsQuickDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public string BEZEICHNUNG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ZINS
        {
            get;
            set;
        }

       
    }
}
