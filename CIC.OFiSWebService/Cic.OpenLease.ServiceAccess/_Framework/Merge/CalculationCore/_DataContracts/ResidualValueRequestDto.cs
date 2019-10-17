// OWNER MK, 22-05-2009
namespace Cic.OpenLease.ServiceAccess.Merge.CalculationCore
{
    /// <summary>
    /// Not in use yet.
    /// </summary>
    [System.CLSCompliant(true)]
    public class ResidualValueRequestDto
    {
        /// <summary>
        /// Laufzeit.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int Term
        {
            get;
            set;
        }

        /// <summary>
        /// Km pro Jahr.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int KmPerYear
        {
            get;
            set;
        }

        /// <summary>
        /// Restwertgruppe. Wird ignoriert wenn MappingId gesetzt.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public ValueGroupTypes ValueGroupType
        {
            get;
            set;
        }

        /// <summary>
        /// Mapping feld zu externe systeme. Durch dieses feld wird das Objekttyp bestimmt. Objekttyp ist wiederumg mit werte gruppen verbunden.
        /// Wenn MappingId gesetzt ist wird die ValueGrupType ignoriert.
        /// OBTYPMAP:CODE
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string MappingId
        {
            get;
            set;
        }
    }
}
