using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class ANGOBAUSDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string SNR  
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string BESCHREIBUNG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? BETRAG
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public decimal? BETRAG2
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int? FLAGRWREL
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? FLAGPACKET
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string FREITEXT
        {
            get;
            set;
        }
        #endregion
    }
}
