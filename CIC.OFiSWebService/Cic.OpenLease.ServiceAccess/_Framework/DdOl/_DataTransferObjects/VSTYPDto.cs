// OWNER JJ, 27-01-2010
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class VSTYPDto
    {
        #region Ids properties
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSVSTYP
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
        public string CODEMETHOD
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public long MAXLAUFZEIT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public bool? FLAGDEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public bool? FLAGINKASSOART
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public bool? FLAGZUBINKL
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
        public long SYSKORRTYP1
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long SYSKORRTYP2
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long SYSQUOTE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SAPRAEMIENFREI
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

        
        [System.Runtime.Serialization.DataMember]
        public decimal? SBHP
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int? ACTIVEFLAG
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public DateTime? VALIDFROM
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public DateTime? VALIDUNTIL
        {
            get;
            set;
        }
        #endregion
    }
}