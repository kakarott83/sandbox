using CIC.Database.OL.EF4.Model;
using System.Collections.Generic;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Korrektur Data Access Object Interface
    /// </summary>
    public interface IKorrekturDao
    {
        /// <summary>
        /// Get all KORRTYP
        /// </summary>
        /// <returns></returns>
        List<KORRTYP> getKorrekturTypen();

        /// <summary>
        /// Get KORREKTUR for korrtyp
        /// </summary>
        /// <returns></returns>
        List<KORREKTUR> getKorrekturen(long syskorrtyp);


    }
}
