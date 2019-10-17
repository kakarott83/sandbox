using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Schnittstelle für das Schnellkalkulation DAO
    /// </summary>
    public interface ISchnellkalkulationDao
    {
        /// <summary>
        /// Erzeuge eine neue Schnellkalkulation
        /// </summary>
        /// <returns>Kalkulation Daten Transfer Objekt</returns>
        KalkulationDto CreateSchnellkalkulation();
    }
}
