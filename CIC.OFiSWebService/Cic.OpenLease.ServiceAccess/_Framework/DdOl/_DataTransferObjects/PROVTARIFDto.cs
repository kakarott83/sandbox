namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class PROVTARIFDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSPROVTARIF
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long STANDARDFLAG
        {
            get;
            set;
        }

        #endregion
    }
       
}
