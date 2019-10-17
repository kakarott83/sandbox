// OWNER JJ, 16-09-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Prisma
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class InterestDto
    {
        #region Properties
        [System.Runtime.Serialization.DataMember]
        public double? Min
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public double? Value
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public double? Max
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.ServiceAccess.Merge.Prisma.InterestType InterestType
        {
            get;
            set;
        }
        #endregion
    }
}