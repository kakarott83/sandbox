// OWNER JJ, 09-12-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class PLZDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSPLZ
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string PLZ1
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ORT
        {
            get;
            set;
        }   
        #endregion
    }
}