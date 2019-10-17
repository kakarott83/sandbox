using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Schnittstelle für Mehrwertsteuer-Ermittlung
    /// </summary>
    public interface IMwStBo
    {
        /// <summary>
        /// Ermitteln der Mehrwertsteuer aus der Vertragsart
        /// </summary>
        /// <param name="sysls">Mandanten ID</param>
        /// <param name="sysvart">Vertragsart ID</param>
        /// <param name="perDate">Datum der Gültigkeit</param>
        /// <returns>Mehrwertsteuer</returns>
        double getMehrwertSteuer(long sysls, long sysvart, DateTime perDate);
    }
}
