// OWNER MK, 21-07-2009
namespace Cic.OpenLease.ServiceAccess.Merge.OlClient
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class HotInputDto
    {
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOw.EAIHOT EAIHOT
        {
            get;
            set;
        }
    }
}
