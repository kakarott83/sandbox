namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class TireDto
    {
        #region Properties
        
        
        [System.Runtime.Serialization.DataMember]
        public string Code
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Manufacturer
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? Price
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Bauart
        {
            get;
            set;
        }
        #endregion
    }
}
