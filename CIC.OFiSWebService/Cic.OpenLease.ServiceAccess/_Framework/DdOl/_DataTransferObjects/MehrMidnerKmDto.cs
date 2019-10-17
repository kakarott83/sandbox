namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class MehrMinderKmDto
    {
         #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public decimal SatzMinderKm
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SatzMehrKm
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public decimal SatzMinderKmBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SatzMehrKmBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SatzMinderKmUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SatzMehrKmUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MehrKMToleranzgrenze
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MinderKMToleranzgrenze
        {
            get;
            set;
        }

        #endregion
    }
}


