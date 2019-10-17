// OWNER JJ, 09-12-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class LANDDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSLAND
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string COUNTRYNAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? EG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? DEFAULTFLAG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? BESTFIVEFLAG
        {
            get;
            set;
        }
        #endregion
    }
}