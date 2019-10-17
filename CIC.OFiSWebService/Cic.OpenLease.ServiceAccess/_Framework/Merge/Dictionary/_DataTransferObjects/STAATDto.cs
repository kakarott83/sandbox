// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class STAATDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSSTAAT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSLAND
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string STAAT1
        {
            get;
            set;
        }
        #endregion
    }
}