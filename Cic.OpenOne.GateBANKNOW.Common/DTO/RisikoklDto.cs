using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Risikoklasse DTO
    /// </summary>
    public class RisikoklDto
    {
        /// <summary>
        /// Risikoklasse ID
        /// </summary>
        public long sysrisikokl { get; set; }
        /// <summary>
        /// Bezeichnung
        /// </summary>
        public string bezeichnung { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// Score
        /// </summary>
        public string score { get; set; }
    }
}
