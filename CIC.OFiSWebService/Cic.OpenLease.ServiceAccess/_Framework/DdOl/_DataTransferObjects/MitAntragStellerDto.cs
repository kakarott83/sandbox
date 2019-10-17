using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class MitantragstellerDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public long SysId
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSVT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSIT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string BEZEICHNUNG
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long SYSSICHTYP
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long SYSHALTER
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long SYSMH
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int? AKTIVZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BEGIN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? END
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int RANG
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int SICHTYPRANG
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string OPTION1
        {
            get;
            set;
        }

    }
}
