using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Collection;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Cached Korrektur Data Access Object
    /// </summary>
    public class CachedKorrekturDao : KorrekturDao
    {

        private static CacheDictionary<long, List<KORRTYP>> korrtypCache = CacheFactory<long, List<KORRTYP>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, List<KORREKTUR>> korrekturCache = CacheFactory<long, List<KORREKTUR>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        
         /// <summary>
        /// Get all KORRTYP
        /// </summary>
        /// <returns></returns>
        override public List<KORRTYP> getKorrekturTypen()
        {
            if (!korrtypCache.ContainsKey(1))
            {
                korrtypCache[1] = base.getKorrekturTypen();
            }
            return korrtypCache[1];
        }

         /// <summary>
        /// Get KORREKTUR for korrtyp
        /// </summary>
        /// <returns></returns>
        override public List<KORREKTUR> getKorrekturen(long syskorrtyp)
        {
            if (!korrekturCache.ContainsKey(syskorrtyp))
            {
                korrekturCache[syskorrtyp] = base.getKorrekturen(syskorrtyp);
            }
            return korrekturCache[syskorrtyp];
        }
       
    }
}
