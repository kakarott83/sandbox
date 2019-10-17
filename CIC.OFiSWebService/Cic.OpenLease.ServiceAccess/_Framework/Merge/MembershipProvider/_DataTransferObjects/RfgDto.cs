
namespace Cic.OpenLease.ServiceAccess.Merge.MembershipProvider
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    #endregion
   
    [System.CLSCompliant(true)]
    public class RfgDto
    {
        /// <summary>
        /// Code für Rechte-Modul, z.B. „B2B“
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string codeRMO { get; set; }
        /// <summary>
        /// Code für Rechte-Funktion, z.B. „Vertrag_Vertragslisten“
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string codeRFU { get; set; }

        /// <summary>
        /// Eine Kodierung der RFU- und RRO-Rechte im Format "1010110000"
        /// rfu:scdix rro:scdix
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string rechte { get; set; }

    }
}
