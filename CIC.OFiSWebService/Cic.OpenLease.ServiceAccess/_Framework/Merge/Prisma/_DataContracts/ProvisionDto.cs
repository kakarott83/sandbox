// OWNER JJ, 16-09-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Prisma
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class ProvisionDto
    {
        #region Properties
        [System.Runtime.Serialization.DataMember]
        public double? Value
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.ServiceAccess.Merge.Prisma.ProvisionBase ProvisionBase
        {
            get;
            set;
        }
        #endregion
    }
}