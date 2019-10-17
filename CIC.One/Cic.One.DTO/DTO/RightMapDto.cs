using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class RightMapDto
    {
        /// <summary>
        /// Code für Rechte-Modul, z.B. „B2B“
        /// </summary>
        public string codeRMO { get; set; }
        /// <summary>
        /// Code für Rechte-Funktion, z.B. „Vertrag_Vertragslisten“
        /// </summary>
        public string codeRFU { get; set; }

        /// <summary>
        /// Eine Kodierung der RFU- und RRO-Rechte im Format "1010110000"
        /// rfu:scdix rro:scdix
        /// </summary>
        public string rechte { get; set; }
    }
}