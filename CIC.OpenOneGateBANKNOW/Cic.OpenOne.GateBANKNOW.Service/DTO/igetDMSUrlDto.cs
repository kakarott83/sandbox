using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter for getDMSUrl Methode
    /// </summary>
    public class igetDMSUrlDto
    {
        /// <summary>
        /// (ANGEBOT,ANTRAG,VT)
        /// </summary>
        public String area { get; set; }
        /// <summary>
        /// techn. Schlüssel für area
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// DMR-Vorgangsnummer
        /// Optional
        /// Pflichtfeld wenn Vorgang im OL über DMR/DMS-Trigger angestossen wurde
        /// </summary>
        public String caseid { get; set; }
        
    }
}
