using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Prkgroup Identifizierungsregel für Segmentierung
    /// </summary>
    public class PrkGroupIdentDto
    {
        public long sysprkgroupident { get;set;}
        public long sysprkgroup { get; set; }
        public String description { get; set; }
        public String exprident { get; set; }
        /// <summary>
        /// PRKGROUP CODE
        /// </summary>
        public String code { get; set; }
    }
}
