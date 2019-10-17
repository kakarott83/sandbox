using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Mehrwertsteuer-Ermittlungs BO
    /// </summary>
    public class MwStBo : AbstractMwStBo
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="mwstDao">Mehrwertsteuerermittlungs DAO</param>
        public MwStBo(IMwStDao mwstDao)
            :base (mwstDao)
        {
        }
        /// <summary>
        /// Ermitteln der Mehrwertsteuer aus der Vertragsart
        /// </summary>
        /// <param name="sysls">Mandanten ID</param>
        /// <param name="sysvart">Vertragsart ID</param>
        /// <param name="perDate">Prüfungsdatum</param>
        /// <returns>Mehrwertsteuer</returns>
        public override double getMehrwertSteuer(long sysls, long sysvart, DateTime perDate)
        {
            double specific = MwStDao.getMehrwertSteuer(sysvart, perDate);
            if (specific > 0)
                return specific;
            else
                return MwStDao.getGlobalUst(sysls, perDate);
                
        }
    }
}
