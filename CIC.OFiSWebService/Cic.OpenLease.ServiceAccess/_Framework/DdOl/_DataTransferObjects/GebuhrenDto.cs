namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class GebuhrenDto
    {
        #region Properties
        
        
        [System.Runtime.Serialization.DataMember]
        public decimal Gebuhren
        {
            get;
            set;
        }

       

        

        
        [System.Runtime.Serialization.DataMember]
        public decimal GebuhrenBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal GebuhrenUst
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal Gebuhren_Default
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

        #endregion
    }
}
