// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Class holding data from view PRFST_V
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class PRFSTDto
    {
        
        
        [System.Runtime.Serialization.DataMember]
        public long SYSPRPRODUCT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSFSTYP
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSFSART
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSFS
        {
            get;
            set;
        }
        /// <summary>
        /// 1=fsart, 2=fs, 3=fstyp
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        
        public long METHOD
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long NEEDED
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long DISABLEDFLAG
        {
            get;
            set;
        }
         
        [System.Runtime.Serialization.DataMember]
        public long NEEDEDPOS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long DISABLEDFLAGPOS
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int MITFINFLAG
        {
            get;
            set;
        }
        
    }
}