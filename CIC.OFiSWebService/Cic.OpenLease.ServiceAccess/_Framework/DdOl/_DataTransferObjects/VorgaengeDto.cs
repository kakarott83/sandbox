namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class VorgaengeDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public VorgangDto[] Vorgaenge
        {
            get;
            set;
        }


    }
}