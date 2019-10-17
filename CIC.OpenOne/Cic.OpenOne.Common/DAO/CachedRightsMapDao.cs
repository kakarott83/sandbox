using System.Collections.Generic;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Cached RightsMap Data Access Object
    /// </summary>
    public class CachedRightsMapDao : RightsMapDao
    {
        private static CacheDictionary<long, List<RightsMap>> rightsMapCache = 
            CacheFactory<long, List<RightsMap>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        /// <summary>
        /// RightsMap holen
        /// </summary>
        /// <returns></returns>
        override public List<RightsMap> getRightsForWFUser(long sysWFUser)
        {
            if (!rightsMapCache.ContainsKey(sysWFUser))
            {
                rightsMapCache[sysWFUser] = base.getRightsForWFUser(sysWFUser);
            }
            return rightsMapCache[sysWFUser];
        }
    }
}