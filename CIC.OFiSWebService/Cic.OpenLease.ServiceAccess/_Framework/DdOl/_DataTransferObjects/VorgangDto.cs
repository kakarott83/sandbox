// OWNER JJ, 08-02-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class VorgangDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSID
        {
            get;
            set;
        }

        
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string OBHERSTELLER
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string OBFABRIKAT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string VART
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string OBJEKTVT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string PRODUCTNAME
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int LAUFZEIT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long JAHRESKM
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string ZUSTAND
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal RW
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal RATE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string ANGEBOT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string ANTRAG
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string VERTRAG
        {
            get;
            set;
        }
        #endregion
    }
}
