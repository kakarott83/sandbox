// OWNER MK, 24-02-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class OBARTDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public long SYSOBART
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
        public string DESCRIPTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public OBKATDto[] ObjectCategories
        {
            get;
            set;
        }
        #endregion
    }
}
