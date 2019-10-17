using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstrakte Klasse des Schnellkalkulation BO
    /// </summary>
    public abstract class AbstractSchnellKalkulation : ISchnellKalkulationBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected ISchnellkalkulationDao pDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SchnellKalkulationDao">EingangsDto</param>
        public AbstractSchnellKalkulation(ISchnellkalkulationDao SchnellKalkulationDao)
        {
            this.pDao = SchnellKalkulationDao;
        }

        /// <summary>
        /// Neue Schnellkalkulation erzeugen 
        /// Updates sind hier eigentlich nicht möglich Ich behalte den Paramter sysID als reinen Dummy, 
        /// falls das irgendwann doch nötig sein sollte. 
        /// </summary>
        /// <param name="sysID">Primärschlüssel der DB (0 wenn keiner gegeben)</param>
        /// <returns>Neues oder geöffnetes KalkulationDto</returns>
        public abstract KalkulationDto createOrUpdateSchnellkalkulation(long sysID);

    }
}
