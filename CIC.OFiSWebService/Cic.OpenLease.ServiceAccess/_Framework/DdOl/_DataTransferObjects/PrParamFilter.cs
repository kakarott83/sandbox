// OWNER MK, 16-03-2010
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class PrParamFilter
    {
        

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public decimal? VALUEN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? VALUEP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime? VALUED
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int TYP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PRFLDMETA
        {
            get;
            set;
        }
        #endregion
    }
}
