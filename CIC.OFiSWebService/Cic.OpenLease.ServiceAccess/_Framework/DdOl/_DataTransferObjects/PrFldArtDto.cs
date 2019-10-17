// OWNER WB, 14-04-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class PrFldArtDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string DESCRIPTION
        {
            get;
            set;
        }
        #endregion
    }
}
