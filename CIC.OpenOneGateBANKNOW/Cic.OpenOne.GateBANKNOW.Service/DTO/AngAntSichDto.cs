using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Angebot Antrag Sicherheit Dto
    /// </summary>
    public class AngAntSichDto
    {
        /// <summary>
        /// Verweis zum Angebot 
        /// </summary>
        public long sysangebot { get; set; }
        /// <summary>
        /// Verweis zum Antrag 
        /// </summary>
        public long sysantrag { get; set; }
        /// <summary>
        /// Verweis zum Interessenten 
        /// </summary>
        public long sysit { get; set; }
        /// <summary>
        /// Verweis zum Kunden 
        /// </summary>
        public long syskd { get; set; }
        /// <summary>
        /// Verweis zum Sicherheitentyp (Kaution bzw 2ter Antragsteller) 
        /// </summary>
        public long syssichtyp { get; set; }
        /// <summary>
        /// Wert der Sicherheit (hier nur bei Kaution) 
        /// </summary>
        public double wert { get; set; }

    }
}
