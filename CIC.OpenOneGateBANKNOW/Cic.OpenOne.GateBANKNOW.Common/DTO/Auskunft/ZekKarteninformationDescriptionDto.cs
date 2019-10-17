
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Zeitkarteninformation Description Data Transfer Object
    /// </summary>
    public class ZekKarteninformationDescriptionDto
    {
        /// <summary>
        /// Getter/Setter signature
        /// </summary>
        public int Kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter Subsidiary
        /// </summary>
        public int Filiale { get; set; }

        /// <summary>
        /// Getter/Setter Event code
        /// </summary>
        public int EreignisCode { get; set; }

        /// <summary>
        /// Getter/Setter Card code
        /// </summary>
        public int KartenTypCode { get; set; }

        /// <summary>
        /// Getter/Setter Date of negative event
        /// </summary>
        public string DatumNegativereignis { get; set; }

        /// <summary>
        /// Getter/Setter Date of positive event
        /// </summary>
        public string DatumPositivmeldung { get; set; }
    }
}