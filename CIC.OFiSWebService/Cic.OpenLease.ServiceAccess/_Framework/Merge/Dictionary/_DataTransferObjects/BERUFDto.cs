// OWNER JJ, 25-02-2010
namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class BERUFDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string ID
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public string CODE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string DOMAINID
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VALUE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string TOOLTIP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? RANK
        {
            get;
            set;
        }
        #endregion
    }
}
