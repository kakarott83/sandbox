// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class PRVSDto
    {

        
        [System.Runtime.Serialization.DataMember]
        public long SYSPRPRODUCT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSVSART
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPERSON
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long SYSVSTYP
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long METHOD
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int NEEDED
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int DISABLEDFLAG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int FLAGDEFAULT
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int DEFAULTFLAG
        {
            get;
            set;
        }

    }
}