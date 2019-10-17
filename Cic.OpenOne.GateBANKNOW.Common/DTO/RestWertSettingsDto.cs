using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Restwert Einstellungen
    /// </summary>
    public class RestWertSettingsDto
    {
        /// <summary>
        /// Wertegruppe
        /// </summary>
        public long sysvgrw { get; set; }
        
        /// <summary>
        /// Externe Daten-Flag
        /// </summary>
        public bool External { get; set; }

        /// <summary>
        /// Ermittelte sysbrand
        /// </summary>
        public long sysbrand { get; set; }
    }

    /// <summary>
    /// Restwert Einstellungen DB
    /// </summary>
    public class RestWertSettingsDBDto
    {
        /// <summary>
        /// Wertegruppe
        /// </summary>
        public long sysvgrw { get; set; }

        /// <summary>
        /// Externe Daten-Flag
        /// </summary>
        public long External { get; set; }
    }
}
