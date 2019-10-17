
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class FSTYPDto
    {
        #region Ids properties

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSFSTYP
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string BEZEICHNUNG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string BESCHREIBUNG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ARTCODE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int ZINSFLAG
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public decimal ZINS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSFSART
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSFS
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public int? FIXVAROPTION
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public int? FIXVARDEFAULT
        {
            get;
            set;
        }




        
        [System.Runtime.Serialization.DataMember]
        public int METHOD
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public long SYSVG
        {
            get;
            set;
        }

        

        
        [System.Runtime.Serialization.DataMember]
        public int? NEEDED
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public int? DISABLED
        {
            get;
            set;
        }
        
        #endregion
    }
}
