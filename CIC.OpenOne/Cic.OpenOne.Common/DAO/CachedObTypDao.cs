using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF4.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Object Type Data Access Object
    /// </summary>
    public class CachedObTypDao : ObTypDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static CacheDictionary<long, List<long>> obTypAscCache = CacheFactory<long, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, List<long>> obTypDescCache = CacheFactory<long, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<long>> prhGroupCache = CacheFactory<String, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<long, List<long>> prhGroupPeroleCache = CacheFactory<long, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<long, long> haendlerCache = CacheFactory<long, long>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<long, long> personPeroleCache = CacheFactory<long, long>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<long, PERSON> personDataPeroleCache = CacheFactory<long, PERSON>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        

        /// <summary>
        /// Constructor
        /// </summary>
        public CachedObTypDao()
        {
        }

        /// <summary>
        /// returns all items beneath the given obtyp (all paths to the leaves)
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public override List<long> getObTypDescendants(long sysobtyp)
        {
            if (!obTypDescCache.ContainsKey(sysobtyp))
            {
                obTypDescCache[sysobtyp] = base.getObTypDescendants(sysobtyp);
            }
            return new List<long>(obTypDescCache[sysobtyp]);
        }

        /// <summary>
        /// returns the path to the root for the given obtyp
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public override List<long> getObTypAscendants(long sysobtyp)
        {
            if (!obTypAscCache.ContainsKey(sysobtyp))
            {
                obTypAscCache[sysobtyp] = base.getObTypAscendants(sysobtyp);
            }
            return new List<long>(obTypAscCache[sysobtyp]);

        }


        /// <summary>
        /// Prhgroups for the role and obtyp
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public override List<long> getPrhGroups(long sysperole, long sysobtyp, DateTime perDate)
        {
            String key = sysperole + "_" + sysobtyp + "_" + perDate.Year + "_" + perDate.Month + "_" + perDate.Day;
            if (!prhGroupCache.ContainsKey(key))
            {
                prhGroupCache[key] = base.getPrhGroups(sysperole,sysobtyp,perDate);
            }
            return new List<long>(prhGroupCache[key]);
        }

        /// <summary>
        /// Prhgroups for the role
        /// </summary>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Handelsgruppenliste</returns>
        public override List<long> getPrhGroupsbyPerole(long sysperole)
        {
            if (!prhGroupPeroleCache.ContainsKey(sysperole))
            {
                prhGroupPeroleCache[sysperole] = base.getPrhGroupsbyPerole(sysperole);
            }
            return new List<long>(prhGroupPeroleCache[sysperole]);
        }

        /// <summary>
        /// Schlüssel des Händlers aus dem des Angestellten ermittlen.
        /// </summary>
        /// <param name="sysperole">Angestellten Schlüssel</param>
        /// <returns>Händlerschlüssel</returns>
        public override long getHaendlerByEmployee(long sysperole)
        {
            if (!haendlerCache.ContainsKey(sysperole))
            {
                haendlerCache[sysperole] = base.getHaendlerByEmployee(sysperole);
            }
            return haendlerCache[sysperole];
          
        }

        /// <summary>
        /// Schlüssel des PersonID aus PEROLE ermittlen.
        /// </summary>
        /// <param name="sysperole">Perole</param>
        /// <returns>PersonID</returns>
        public override long getPersonIDByPEROLE(long sysperole)
        {
            if (!personPeroleCache.ContainsKey(sysperole))
            {
                personPeroleCache[sysperole] = base.getPersonIDByPEROLE(sysperole);
            }
            return personPeroleCache[sysperole];

        }

        /// <summary>
        ///  Person aus PEROLE ermittlen.
        /// </summary>
        /// <param name="sysperole">Perole</param>
        /// <returns>PersonID</returns>
        public override PERSON getPersonByPEROLE(long sysperole)
        {
            if (!personDataPeroleCache.ContainsKey(sysperole))
            {
                personDataPeroleCache[sysperole] = base.getPersonByPEROLE(sysperole);
            }
            return personDataPeroleCache[sysperole];

        }

    }

    
}
