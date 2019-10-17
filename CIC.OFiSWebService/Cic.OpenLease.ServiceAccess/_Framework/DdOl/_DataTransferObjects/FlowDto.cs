// OWNER JJ, 27-01-2010
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class FlowDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public long SYSFLOW
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public short DONE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int STATUS
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long USERID
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string KOMMENTAR
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string CATEGORY
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public DateTime? DATEOFACTION
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public DateTime? BEFRISTETBIS
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public double WERT
        {
            get;
            set;
        }

    }
}