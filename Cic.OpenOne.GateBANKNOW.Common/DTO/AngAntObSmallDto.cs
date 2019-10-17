using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// AngAntObSmallDto
    /// </summary>
    public class AngAntObSmallDto
    {
        /// <summary>
        /// Erstzulassung
        /// </summary>
        public DateTime? erstzulassung { get; set; }

        /// <summary>
        /// Übernahmekilometer
        /// </summary>
        public long ubnahmeKm { get; set; }


        /// <summary>
        /// OriginalRate
        /// </summary>
        public double rateBruttoOrg { get; set; }

        /// <summary>
        /// Eingangskanal
        /// </summary>
        public String configsource { get; set; }


        /// <summary>
        /// RWA-Indikation Netto
        /// </summary>
        public double rwBase { get; set; }

    }
}
