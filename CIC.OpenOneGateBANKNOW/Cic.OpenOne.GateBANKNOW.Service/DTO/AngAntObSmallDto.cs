using System;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
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
    }
}