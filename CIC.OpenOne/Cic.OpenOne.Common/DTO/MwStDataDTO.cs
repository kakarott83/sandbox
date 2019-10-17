using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Mehrwertsteuer-Daten DTO
    /// </summary>
    public class MwStDataDTO
    {
        /// <summary>
        /// mehrwertsteueranteil in Prozent
        /// </summary>
        public double Prozent { get; set; }
        /// <summary>
        /// Mehrwertsteuer Gueltigkeits ID
        /// </summary>
        public long SYSMWSTDATE { get; set; }
        /// <summary>
        /// Mehrwertsteuer ID
        /// </summary>
        public long SYSMWST { get; set; }
        /// <summary>
        /// Gueltigkeitsdatum
        /// </summary>
        public DateTime GueltigAb { get; set; }
        /// <summary>
        /// Prozent 1
        /// </summary>
        public double ProzentAkt { get; set; }
        /// <summary>
        /// Konto ID
        /// </summary>
        public long sysKonto { get; set; }
        /// <summary>
        /// mehrwertsteuer FIBU
        /// </summary>
        public double MwStFibu { get; set; }
        /// <summary>
        /// Skonto
        /// </summary>
        public string EvalSkonto { get; set; }
        /// <summary>
        /// Vertragsart ID
        /// </summary>
        public long sysvart { get; set; }
    }
}
