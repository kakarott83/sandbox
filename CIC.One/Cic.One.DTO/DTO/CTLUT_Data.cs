using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Uebersetzungsdaten aus CTLUT
    /// </summary>
    public class CTLUT_Data
    {
        /// <summary>
        /// Bereich der Übersetzung
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// SIO Sprachen CODE
        /// </summary>
        public string IsoCode { get; set; }
        /// <summary>
        /// Schlüssel des Begriffs.
        /// </summary>
        public long sysID { get; set; }
        /// <summary>
        /// Original-Name
        /// </summary>
        public string OrigTerm { get; set; }
        /// <summary>
        /// Übersetzter Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Bezeichnung
        /// </summary>
        public string Bezeichnung { get; set; }
    }
}