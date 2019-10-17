using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Holds data needed for incentive calculation of a contract
    /// </summary>
    public class VTIncentivierungDataDto
    {
        /// <summary>
        /// BGEXTERN
        /// </summary>
        public double umsatz { get; set; }

        /// <summary>
        /// Anzahl der Versicherungen (ANTVS-Sätze)
        /// </summary>
        public int anzInsurance { get; set; }
    }
}
