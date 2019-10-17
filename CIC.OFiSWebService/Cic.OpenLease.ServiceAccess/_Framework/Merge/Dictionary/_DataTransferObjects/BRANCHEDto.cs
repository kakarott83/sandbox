// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class BRANCHEDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSBRANCHE
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string BEZEICHNUNG
        {
            get;
            set;
        }
        #endregion
    }
}