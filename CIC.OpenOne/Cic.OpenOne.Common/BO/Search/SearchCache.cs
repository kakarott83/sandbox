using Cic.OpenOne.Common.Util.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO.Search
{
    /// <summary>
    /// Caches for Search
    /// Must be inside own class, because Generic typed class wont work with cache, there will be lots of different Cache-Instances!
    /// </summary>
    public class SearchCache
    {
        public static CacheDictionary<String, int> countCache = CacheFactory<String, int>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.SearchResult), CacheCategory.Data);
        public static CacheDictionary<String, List<long>> resultIdCache = CacheFactory<String, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.SearchResult), CacheCategory.Data);
        
        /// <summary>
        /// clean all caches regarding the entity
        /// the cache keys must all begin with infoData.entityTable + "_" !!!
        /// for this to work
        /// </summary>
        /// <param name="changedDto"></param>
        public static void entityChanged(Object changedDto)
        {

            //TODO Optimization:
            //add to a queue in cachedictionary
            //upon next Get in dictionary lookup this queue, if in queue, remove and clear cache

            if (changedDto is String)
            {
                String entity = (String)changedDto + "_";
                foreach (String key in countCache.Keys)
                {
                    if (key.StartsWith(entity))
                        countCache.Remove(key);
                }
                foreach (String key in resultIdCache.Keys)
                {
                    if (key.StartsWith(entity))
                        resultIdCache.Remove(key);
                }
            }
        }
    }
}
