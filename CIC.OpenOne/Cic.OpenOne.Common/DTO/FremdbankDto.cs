using System;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Parameterklasse für FremdbankDto
    /// </summary>
    public class FremdbankDto
    {
        /// <summary>
        /// id der Fremdbank
        /// </summary>
        public long sysperson { get; set; }

        /// <summary>
        /// Fremdbank-Name
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// Fremdbank Empfängerangaben Ort 
        /// </summary>
        public String ort { get; set; }

        /// <summary>
        /// Fremdbank Kontonummer des Kunden 
        /// </summary>
        public String kontoNummer { get; set; }

        /// <summary>
        /// Kontoart
        /// </summary>
        public long kontoArt { get; set; }

        /// <summary>
        /// Fremdbank Bankleitzahl (Clearingnummer) 
        /// </summary>
        public String clearingNr { get; set; }

        /// <summary>
        /// Verweis zum Ablösetyp (Eigen, Fremd) 
        /// </summary>
        public long sysabltyp { get; set; }

    }
}
