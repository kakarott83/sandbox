using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Neuwert Einstellungen
    /// </summary>
    public class NeuwertSettingsDto
    {
        /// <summary>
        /// Wertegruppe
        /// </summary>
        public long sysvgrw { get; set; }
        
        /// <summary>
        /// Externe Daten-Flag
        /// </summary>
        public bool External { get; set; }
    }

    /// <summary>
    /// Neuwert Einstellungen DB
    /// </summary>
    public class NeuwertSettingsDBDto
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
