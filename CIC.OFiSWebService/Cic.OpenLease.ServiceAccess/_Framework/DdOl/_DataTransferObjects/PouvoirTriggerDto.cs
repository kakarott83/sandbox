
using System.Collections.Generic;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class PouvoirTriggerDto
    {

        public const int TRGTYPE_LS = 1;
        public const int TRGTYPE_BRAND = 2;
        public const int TRGTYPE_VART = 10;
        public const int TRGTYPE_VARTTAB = 11;
        public const int TRGTYPE_VTTYP = 12;
        public const int TRGTYPE_KALKTYP = 13;
        public const int TRGTYPE_PRPRODUCT = 14;
        public const int TRGTYPE_HGROUP = 20;
        public const int TRGTYPE_PEROLE = 21;
        public const int TRGTYPE_OBART = 30;
        public const int TRGTYPE_OBTYP = 31;
        public const int TRGTYPE_KDART = 40;
        public const int TRGTYPE_KDTYP = 41;

        #region Constructor
        public PouvoirTriggerDto(int trgType, long trgId)
        {
            this.TRG_ID = trgId;
            this.TRG_TYPE = trgType;
        }
        public PouvoirTriggerDto(int trgType, List<long> trgIds)
        {
            this.TRG_IDS = trgIds;
            this.TRG_TYPE = trgType;
        }
        
        #endregion
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public int TRG_TYPE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long TRG_ID
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public List<long> TRG_IDS
        {
            get;
            set;
        }

       
        #endregion
    }
}
