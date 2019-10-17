using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Schnellkalkulation BO 
    /// </summary>
    public class SchnellKalkulationBo : AbstractSchnellKalkulation
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao">Kalkulation Data Access Object</param>
        public SchnellKalkulationBo(ISchnellkalkulationDao pDao): base(pDao)
        {
        }   

        /// <summary>
        /// Neues Schnellkalkulation Dto erzeugen 
        /// Updates sind hier eigentlich nicht möglich Ich behalte den Paramter sysID als reinen Dummy, 
        /// falls das irgendwann doch nötig sein sollte. Warum auch immer...
        /// </summary>
        /// <param name="sysID">Primärschlüssel der DB (0 wenn keiner gegeben)</param>
        /// <returns>Neues oder geöffnetes KalkulationDto</returns>
        override
        public KalkulationDto createOrUpdateSchnellkalkulation(long sysID)
        {
            return pDao.CreateSchnellkalkulation();
        }
    }
}
