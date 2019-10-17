// OWNER JJ, 30-11-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class ExtensionProductDto
    {
        [System.Runtime.Serialization.DataMember]
        public PRPRODUCTDto[] neuprodukte;
        [System.Runtime.Serialization.DataMember]
        public PRPRODUCTDto[] verlaengerungsprodukte;
    }

    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class PRPRODUCTDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string NAMEINTERN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime VALIDFROM
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public DateTime VALIDUNTIL
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
        public long SYSPRPRODUCT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long RANGSL
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long SYSINTTYPE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long SYSVART
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long SYSPRPRODTYPE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int CONDITIONTYPE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int ACTIVEFLAG
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int SYSKALKTYP
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int MODUS
        {
            get;
            set;
        }
        #endregion
    }
}
