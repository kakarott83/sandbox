using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.One.Web.DAO
{
    /// <summary>
    /// Manages one static Dictionary per Type T holding key/object pairs for caching
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheDelegator<T>
    {
        private static ThreadSafeDictionary<string, CacheDelegator<T>> instances = new ThreadSafeDictionary<string, CacheDelegator<T>>();
        private CacheDictionary<String, object> caches = CacheFactory<String, object>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        public delegate object CacheDelegatorMethod();

        public static CacheDelegator<T> getInstance()
        {
            string key = typeof(T).FullName;

            if (!instances.ContainsKey(key))
            {
                CacheDelegator<T> instance = new CacheDelegator<T>();
                instances.MergeSafe(key, instance);
            }
            return instances[key];
        }

        /// <summary>
        /// Fetches the data with the delegator if not in cache and caches it
        /// Clones the result from the cache so it may be modified during usage without altering the cached values
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public V getCachedCloned<V>(String key, CacheDelegatorMethod m) where V : ICloneable
        {
            
            if (!caches.ContainsKey(key))
            {
                caches[key] = m();
            }
            return (V)((V)caches[key]).Clone();
        }

        /// <summary>
        /// Fetches the data with the delegator, if not in cache and caches it
        /// Returns the cache result which may be altered by the code!
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public V getCached<V>(String key, CacheDelegatorMethod m) 
        {

            if (!caches.ContainsKey(key))
            {
                caches[key] = m();
            }
            return (V)((V)caches[key]);
        }
    }
}