// OWNER JJ, 08-02-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class ITShortDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long SYSIT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSKDTYP
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VORNAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string STRASSE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string HSNR
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PLZ
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

        
        [System.Runtime.Serialization.DataMember]
        public string PTELEFON
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string TELEFON
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string HANDY
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string FAX
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string EMAIL
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? BESCHARTAG1
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? GEBDATUM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? GRUENDUNG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int ANGEBOTCount
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int ANTRAGCount
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int VTCount
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public long? SYSPERSON
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int CAMPCount{ get; set; }
        [System.Runtime.Serialization.DataMember]
        public int EOTCount { get; set; }
        [System.Runtime.Serialization.DataMember]
        public bool ISPERSON { get { if (SYSPERSON.HasValue && SYSPERSON.Value > 0) return true; return false; } set { } }

        #endregion
    }
}
