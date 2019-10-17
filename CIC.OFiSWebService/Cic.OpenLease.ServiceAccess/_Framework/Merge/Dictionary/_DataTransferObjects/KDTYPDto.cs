namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class KDTYPDto
    {
        
        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? ACTIVEFLAG
        {
            get;
            set;
        }
        

       
        
        [System.Runtime.Serialization.DataMember]
        public string DESCRIPTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSKDTYP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int TYP
        {
            get;
            set;
        }
        
    }
}
