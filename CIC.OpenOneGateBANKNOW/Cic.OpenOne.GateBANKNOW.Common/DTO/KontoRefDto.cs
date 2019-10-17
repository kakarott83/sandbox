using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Kontoreferenzen DTO
    /// </summary>
    public class KontoRefDto
    {
        /// <summary>
        /// PKEY 
        /// </summary>
        public long syskontoref { get; set; }
        /// <summary>
        /// Verweis zum Konto 
        /// </summary>
        public long syskonto { get; set; }
        /// <summary>
        /// Verweis zum Kontotyp (Typisierung) 
        /// </summary>
        public long syskontotp { get; set; }
        /// <summary>
        /// Verweis zum Interessent 
        /// </summary>
        public long sysit { get; set; }
        /// <summary>
        /// Verweis zur Person 
        /// </summary>
        public long sysperson { get; set; }
        /// <summary>
        /// Verweis zum Angebot 
        /// </summary>
        public long sysangebot { get; set; }
        /// <summary>
        /// Verweis zum Antrag 
        /// </summary>
        public long sysantrag { get; set; }

    }
}
