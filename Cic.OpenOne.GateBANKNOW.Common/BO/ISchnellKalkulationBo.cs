using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Schnittstelle des Schnellkalkulation BO
    /// </summary>
    public interface ISchnellKalkulationBo
    {
        /// <summary>
        /// Neue Schnellkalkulation erzeugen 
        /// Updates sind hier eigentlich nicht möglich Ich behalte den Paramter sysID als reinen Dummy, 
        /// falls das irgendwann doch nötig sein sollte. 
        /// </summary>
        /// <param name="sysID">Primärschlüssel der DB (0 wenn keiner gegeben)</param>
        /// <returns>Neues oder geöffnetes KalkulationDto</returns>
        KalkulationDto createOrUpdateSchnellkalkulation(long sysID);
    }
}
