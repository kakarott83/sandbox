// OWNER WB, 19-03-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class VTShortDto
    {
        #region Ids

        
        [System.Runtime.Serialization.DataMember]
        public long VTSYSID
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? VTSYSKD
        {
            get;
            set;
        }

        #endregion

        #region Properties
        
        
        [System.Runtime.Serialization.DataMember]
        public string VTVERTRAG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? VTBEGINN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? VTENDE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VTZUSTAND
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string AUFLAGEN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PERSONNAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PERSONVORNAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PERSONZUSATZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PERSONPLZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PERSONORT
        {
            get;
            set;
        }

        
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
        public string VTVART
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string OBHALTERKFZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KALKRATE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? KALKLZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? OBJAHRESKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KALKBGEXTERN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KALKSZ
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? KALKSZBRUTTO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KALKANZAHLUNG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KALKDEPOT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KALKRW
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VERKAEUFERVORNAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VERKAEUFERNAME
        {
            get;
            set;
        }
        
        

        
        [System.Runtime.Serialization.DataMember]
        public string KDTYPNAME
        {
            get;
            set;
        }
        
        public long? SYSBERATADDB
        {
            get;
            set;
        }
        public long? SYSVART
        {
            get;
            set;
        }
        #endregion

    }
}
