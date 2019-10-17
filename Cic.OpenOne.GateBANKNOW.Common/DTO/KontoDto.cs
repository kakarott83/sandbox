using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Paramterklasse für Konto
    /// </summary>
    public class KontoDto
    {
        /// <summary>
        /// PKEY 
        /// </summary>
        public long syskonto { get; set; }
        /// <summary>
        /// Konto-Bezeichnung
        /// </summary>
        public String kontoBezeichnung { get; set; }

        /// <summary>
        /// FKEY tp PERSON/IT
        /// </summary>
        public long sysperson { get; set; }
        /// <summary>
        /// Rang (Komptabilität zu rangbasiertem Backoffice System) 
        /// </summary>
        public short rang { get; set; }

        /// <summary>
        /// Kontoführende Bank 
        /// </summary>
        public long sysblz { get; set; }
        /// <summary>
        /// Clearingnummer / BLZ 
        /// </summary>
        public String blz { get; set; }

        /// <summary>
        /// Kontonummer 
        /// </summary>
        public String kontonr { get; set; }
        /// <summary>
        /// Iban 
        /// </summary>
        public String iban { get; set; }
        /// <summary>
        /// FKEY to Antrag
        /// </summary>
        public long sysantrag { get; set; }
    }
}
