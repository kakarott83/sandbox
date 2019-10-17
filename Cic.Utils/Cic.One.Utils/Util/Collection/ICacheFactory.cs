
using System;
namespace Cic.OpenOne.Common.Util.Collection
{
    /// <summary>
    /// ICacheFactory
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// Flushes the cache older than age
        /// </summary>
        /// <param name="age"></param>
        void flush(long age);

        /// <summary>
        /// Flushes the cache of the given category, older than age
        /// </summary>
        /// <param name="minAge"></param>
        /// <param name="cat"></param>
        void flush(long minAge, CacheCategory cat);

        /// <summary>
        /// Reconfigures all caches with a new lifetime from the given function
        /// </summary>
        /// <param name="configAction"></param>
        void reconfigure(Func<CacheCategory, long> configAction);
    }
}