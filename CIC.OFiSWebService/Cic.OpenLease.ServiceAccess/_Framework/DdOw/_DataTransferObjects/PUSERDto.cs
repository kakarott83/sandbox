// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOw
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class PUSERDto
    {
        #region Ids properties
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPUSER
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSWFUSER
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSDEFAULTPEROLE
        {
            get;
            set;
        }
        #endregion       

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string EXTERNEID
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VORNAME
        {
            get;
            set;
        }
        #endregion
    }
}