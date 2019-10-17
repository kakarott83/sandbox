
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class TiresDto
    {
        
        
        [System.Runtime.Serialization.DataMember]
        public TireDto[] WinterTires
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public TireDto[] SummerTires
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public RimDto[] Rims
        {
            get;
            set;
        }

    }
}
