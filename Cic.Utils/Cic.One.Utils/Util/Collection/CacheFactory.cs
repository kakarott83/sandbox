using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.Common.Util.Collection
{
    /// <summary>
    /// CacheCategory-Enum
    /// </summary>
    public enum CacheCategory
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Prisma
        /// </summary>
        Prisma,

        /// <summary>
        /// Zins
        /// </summary>
        Zins,

        /// <summary>
        /// Data
        /// </summary>
        Data,

        /// <summary>
        /// Role
        /// </summary>
        Role,

        /// <summary>
        /// Translation
        /// </summary>
        Translation,

        /// <summary>
        /// SearchResultCache
        /// </summary>
        SearchResult
    }

    /// <summary>
    /// CacheFactory-Klasse
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class CacheFactory<T, V> : ICacheFactory
    {
        private static ThreadSafeDictionary<string, CacheFactory<T, V>> instances = new ThreadSafeDictionary<string, CacheFactory<T, V>>();

        private ThreadSafeDictionary<CacheCategory, IList<DictionaryInfo<T, V>>> caches = new ThreadSafeDictionary<CacheCategory, IList<DictionaryInfo<T, V>>>();
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        /// <summary>
        /// Clears all caches of minimum age
        /// </summary>
        /// <param name="minAge"></param>
        public void flush(long minAge)
        {
            flush(minAge, CacheCategory.None, true);
        }

        /// <summary>
        /// Clears caches of minimum age and given category
        /// </summary>
        /// <param name="minAge"></param>
        /// <param name="cat"></param>
        public void flush(long minAge, CacheCategory cat)
        {
            flush(minAge, cat, false);
        }

        

        /// <summary>
        /// Reconfigures all caches with a new lifetime from the given function
        /// </summary>
        /// <param name="configAction"></param>
        public void reconfigure(Func<CacheCategory, long> configAction)
        {
            int ccount = 0;
            foreach (CacheCategory ccat in caches.Keys)
            {               
                IList<DictionaryInfo<T, V>> ccaches = caches[ccat];
                int mcount = ccaches.Count;
                foreach (DictionaryInfo<T, V> cache in ccaches)
                {
                    
                        cache.reconfigure(configAction);
                        _Log.Debug("Reconfigure Duration of Cache of category " + ccat + " for " + typeof(T) + "," + typeof(V) );
                        ccount++;
                    
                }
               
            }
        }
        /// <summary>
        /// Clears the caches of minimum age
        /// </summary>
        /// <param name="minAge"></param>
        /// <param name="cat"></param>
        /// <param name="all"></param>
        private void flush(long minAge, CacheCategory cat, bool all)
        {
            int ccount = 0;
            foreach (CacheCategory ccat in caches.Keys)
            {
                if (!all && ccat != cat) continue;

                IList<DictionaryInfo<T, V>> ccaches = caches[ccat];
                int mcount = ccaches.Count;
                foreach (DictionaryInfo<T, V> cache in ccaches)
                {
                    if (cache.getAge() > minAge || minAge < 1)
                    {
                        cache.dict.Clear();
                       
                        _Log.Debug("Flushing Cache of category " + ccat + " for " + typeof(T) + "," + typeof(V) + " age: " + cache.getAge() / 1000 + "secs");
                        ccount++;
                    }
                }
                _Log.Debug((mcount - ccount) + " Caches for " + typeof(T) + "," + typeof(V) + " not cleared");
            }
        }

        /// <summary>
        /// getInstance
        /// </summary>
        /// <returns></returns>
        public static CacheFactory<T, V> getInstance()
        {
            string key = typeof(T).FullName + "_" + typeof(V).FullName;

            if (!instances.ContainsKey(key))
            {
                CacheFactory<T, V> instance = new CacheFactory<T, V>();
                CacheManager.getInstance().registerCacheFactory(instance);
                instances.MergeSafe(key, instance);
            }
            return instances[key];
        }

        /// <summary>
        /// createCache
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="cat"></param>
        /// <returns></returns>
        public CacheDictionary<T, V> createCache(long duration, CacheCategory cat)
        {
            CacheDictionary<T, V> rval = new CacheDictionary<T, V>(duration);

            if (!caches.ContainsKey(cat))
                caches[cat] = new SynchronizedCollection<DictionaryInfo<T, V>>();
            caches[cat].Add(new DictionaryInfo<T, V>(rval,cat));

            return rval;
        }

        /// <summary>
        /// createCache from Category None
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public CacheDictionary<T, V> createCache(long duration)
        {
            return createCache(duration, CacheCategory.None);
        }
    }

    class DictionaryInfo<T, V>
    {
        public CacheDictionary<T, V> dict;
        private double registerTime;
        private CacheCategory cat;

        public DictionaryInfo(CacheDictionary<T, V> dict, CacheCategory cat)
        {
            this.dict = dict;
            this.cat = cat;
            registerTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
        }
        public void reconfigure(Func<CacheCategory, long> configAction)
        {
            dict.setDuration(configAction(cat));
        }
        public double getAge()
        {
            return DateTime.Now.TimeOfDay.TotalMilliseconds - registerTime;
        }
    }
}