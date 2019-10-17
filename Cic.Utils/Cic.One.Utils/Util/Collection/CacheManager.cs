using System;
using System.Collections.Generic;

namespace Cic.OpenOne.Common.Util.Collection
{
    /// <summary>
    /// CacheManager-Klasse
    /// </summary>
    public class CacheManager
    {
        private static CacheManager instance = new CacheManager();
        private IList<ICacheFactory> cacheFactories = new List<ICacheFactory>();
        private static string LOCK = "LOCK";
       
        private Func<CacheCategory, long> configAction;

        /// <summary>
        /// getInstance
        /// </summary>
        /// <returns></returns>
        public static CacheManager getInstance()
        {
             lock (LOCK)
            {
                if (instance == null)
                    instance = new CacheManager();
            }
            return instance;
        }
       
        /// <summary>
        /// sets a config action fetching the correct cache duration for a given cache category
        /// </summary>
        /// <param name="configAction"></param>
        public void setConfigAction(Func<CacheCategory, long> configAction)
        {
            this.configAction = configAction;
        }

        /// <summary>
        /// registerCacheFactory
        /// </summary>
        /// <param name="factory"></param>
        public void registerCacheFactory(ICacheFactory factory)
        {
            cacheFactories.Add(factory);
        }
        /// <summary>
        /// Reconfigures all CacheFactories lifetime with the set configAction
        /// </summary>
        public void reconfigure()
        {
            if (configAction == null) return;

            foreach (ICacheFactory factory in cacheFactories)
                factory.reconfigure(configAction);
        }
        /// <summary>
        /// Clears caches of minimum age and category None
        /// </summary>
        /// <param name="age"></param>
        public void flush(long age)
        {
            foreach (ICacheFactory factory in cacheFactories)
                factory.flush(age);
        }

        /// <summary>
        /// Clears caches of minimum age and given category
        /// </summary>
        /// <param name="age"></param>
        /// <param name="cat"></param>
        public void flush(long age, CacheCategory cat)
        {
            foreach (ICacheFactory factory in cacheFactories)
                factory.flush(age, cat);
        }
    }
}