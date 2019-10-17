using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Mehrwertsteuerermittlungs DAO
    /// </summary>
    public class CachedMwStDao : MwStDao
    {

        private static CacheDictionary<String, double> mwstCache = CacheFactory<String, double>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, double> globalUstCache = CacheFactory<String, double>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        
        /// <summary>
        /// Ermitteln der Mehrwertsteuer aus der Vertragsart
        /// </summary>
        /// <param name="sysvart">Vertragsart ID</param>
        /// <param name="perDate">Datum der Gültigkeit</param>
        /// <returns>Mehrwertsteuer</returns>
        override public double getMehrwertSteuer(long sysvart, DateTime perDate)
        {
            String key = sysvart + "_" + perDate.Year + "_" + perDate.Month + "_" + perDate.Day;
            if (!mwstCache.ContainsKey(key))
            {
                mwstCache[key] = base.getMehrwertSteuer(sysvart, perDate);
            }
            return mwstCache[key];

        }
        /// <summary>
        /// Returns the global Ust Value of MWST-Table, defined by the code in the Configsection AIDA/GENERAL/USTCODE
        /// </summary>
        /// <param name="sysls">Mandanten ID</param>
        /// <param name="perDate">Datum der Gültigkeit</param>
        /// <returns>Umsatzsteuer</returns>
        override public double getGlobalUst(long sysls, DateTime perDate)
        {
            String key = sysls + "_" + perDate.Year + "_" + perDate.Month + "_" + perDate.Day;
            if (!globalUstCache.ContainsKey(key))
            {
                globalUstCache[key] = base.getGlobalUst(sysls, perDate);
            }
            return globalUstCache[key];
        }
    }
}
