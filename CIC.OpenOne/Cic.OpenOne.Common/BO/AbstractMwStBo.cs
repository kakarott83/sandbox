using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Mehrwertsteuer-Ermittlung Abstraktes BO
    /// </summary>
    public abstract class AbstractMwStBo : IMwStBo
    {
        /// <summary>
        /// Mehrwertsteuer DAO
        /// </summary>
        protected IMwStDao MwStDao;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="mwStDao"></param>
        public AbstractMwStBo(IMwStDao mwStDao)
        {
            this.MwStDao = mwStDao;
        }

        /// <summary>
        /// Ermitteln der Mehrwertsteuer aus der Vertragsart
        /// </summary>
        /// <param name="sysls">Mandanten ID</param>
        /// <param name="sysvart">Vertragsart ID</param>
        /// <param name="perDate">Datum der Gültigkeit</param>
        /// <returns>Mehrwertsteuer</returns>
        public abstract double getMehrwertSteuer(long sysls, long sysvart, DateTime perDate);
    }
}
