using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Schnittstelle des Mehrwertsteuer-Ermittlungs DAO
    /// </summary>
    public interface IMwStDao
    {
        /// <summary>
        /// Ermitteln der Mehrwertsteuer aus der Vertragsart
        /// </summary>
        /// <param name="sysvart">Vertragsart ID</param>
        /// <param name="perDate">Datum der Gültigkeit</param>
        /// <returns>Mehrwertsteuer</returns>
        double getMehrwertSteuer(long sysvart, DateTime perDate);

        /// <summary>
        /// Returns the global Ust Value of MWST-Table, defined by the code in the Configsection AIDA/GENERAL/USTCODE
        /// </summary>
        /// <param name="sysls">Mandanten ID</param>
        /// <param name="perDate">Datum der Gültigkeit</param>
        /// <returns>Umsatzsteuer</returns>
        double getGlobalUst(long sysls, DateTime perDate);

    }
}
