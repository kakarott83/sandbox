//OWNER WB, 19.05.2010

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class WRRateDto
    {
        #region Properties

        
        [System.Runtime.Serialization.DataMember]
        public decimal RateNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RateDefault
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RateBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RateUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KMCharge
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal excessKMCharge
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal recessKMCharge
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Provision
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Subvention
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

        #endregion
    }
}
