using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Paramterklasse für Konto
    /// </summary>
    public class BlzDto
    {
        /// <summary>
        /// Primärschlüssel Bank 
        /// </summary>
        public long sysblz { get; set; }
        /// <summary>
        /// Bankleitzahl/Clearingnummer/BC-Nummer 
        /// </summary>
        public String blz { get; set; }
        /// <summary>
        /// BIC/SWIFT-Adresse 
        /// </summary>
        public String bic { get; set; }
        /// <summary>
        /// Name 
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// Kurzname 
        /// </summary>
        public String kurzname { get; set; }
        /// <summary>
        /// Zusatz 
        /// </summary>
        public String zusatz { get; set; }
        /// <summary>
        /// Strasse 
        /// </summary>
        public String strasse { get; set; }
        /// <summary>
        /// Strasse Zusatz 
        /// </summary>
        public String strasseZusatz { get; set; }
        /// <summary>
        /// Postleitzahl 
        /// </summary>
        public String plz { get; set; }
        /// <summary>
        /// Ort 
        /// </summary>
        public String ort { get; set; }
        /// <summary>
        /// Telefon 
        /// </summary>
        public String telefon { get; set; }
        /// <summary>
        /// Fax 
        /// </summary>
        public String fax { get; set; }

    }
}
