// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class CTLANGDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSCTLANG
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string LANGUAGENAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ISOCODE
        {
            get;
            set;
        }
        #endregion
    }
}