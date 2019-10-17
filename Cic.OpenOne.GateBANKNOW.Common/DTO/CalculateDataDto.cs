using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Calculate Data DTO
    /// </summary>
    public class CalculateDataDto
    {
        /// <summary>
        /// Restwert in Prozent
        /// </summary>
        public double RestwertPercent { get; set; }
        /// <summary>
        /// Restwert als Summe
        /// </summary>
        public double RestwertValue { get; set; }
        /// <summary>
        /// Neupreis als Summe
        /// </summary>
        public double Neupreis { get; set; }
    }
}
