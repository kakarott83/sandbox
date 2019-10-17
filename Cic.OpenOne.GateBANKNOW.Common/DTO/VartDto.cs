using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Vertragsart DTO
    /// </summary>
    public class VartDto
    {
        /// <summary>
        /// Sertragsart ID
        /// </summary>
        public long sysvart { get; set; }
        /// <summary>
        /// Bezeichnung
        /// </summary>
        public string bezeichnung { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string code { get; set; }
    }
}
