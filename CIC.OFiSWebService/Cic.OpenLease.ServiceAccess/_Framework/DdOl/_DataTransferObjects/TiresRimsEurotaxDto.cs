
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class TiresRimsEurotaxDto
    {
        
        
        [System.Runtime.Serialization.DataMember]
        public TireDto[] TiresFront
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public TireDto[] TiresRear
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public RimDto[] RimsRear
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public RimDto[] RimsFront
        {
            get;
            set;
        }
    }
}