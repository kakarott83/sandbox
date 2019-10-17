//OWNER WB, 19.05.2010

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class TransferVendorDto
    {
        #region Properties

        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long syswfuser
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long sysperole
        {
            get;
            set;
        }

        #endregion
    }
}
