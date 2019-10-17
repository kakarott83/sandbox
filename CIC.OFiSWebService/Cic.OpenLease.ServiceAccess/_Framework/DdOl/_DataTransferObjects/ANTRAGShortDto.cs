// OWNER JJ, 08-02-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class ANTRAGShortDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSID
        {
            get;
            set;
        }
        #endregion

        public long SYSKD
        {
            get;
            set;
        }

        #region Properties
        
        // TODO JJ 0 JJ, Dokumentart

       

        
        
        [System.Runtime.Serialization.DataMember]
        public string ZUSTAND
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
        public string ANTOBHERSTELLER
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ANTOBFABRIKAT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ANTRAG1
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
        public string VART
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? ANTKALKLZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? ANTOBJAHRESKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANTKALKRATE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANTKALKBGEXTERN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANTKALKSZ
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANTKALKDEPOT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANTKALKRW
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? DATANGEBOT
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

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSBERATADDB
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public long? SYSVK
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public long? SYSIT
        {
            get;
            set;
        }

        
        public long? SYSANGEBOT
        {
            get;
            set;
        }

        #endregion
    }
}
