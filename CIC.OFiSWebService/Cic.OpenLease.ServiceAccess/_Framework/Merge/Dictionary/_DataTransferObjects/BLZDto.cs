// OWNER JJ, 09-12-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class BLZDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSBLZ
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string BLZ1
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string BIC
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
        #endregion
    }
}