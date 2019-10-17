using System;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using CIC.Database.OL.EF6.Model;
namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// CachedSimpleGetterDao-Klasse
    /// </summary>
    public class CachedSimpleGetterDao : SimpleGetterDao
    {
        private static CacheDictionary<long, PERSON> findPersonBySysperoleCache = CacheFactory<long, PERSON>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<PERSON, PUSER> getPuserCache = CacheFactory<PERSON, PUSER>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<String, PERSON> findPersonByRoletypeAndSysperoleCache = CacheFactory<String, PERSON>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<long, string> getinformationUeberBySysPersonCache = CacheFactory<long, string>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<String, PEROLE> findRootPEROLEObjByRoleTypeCache = CacheFactory<String, PEROLE>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);

        /// <summary>
        /// get the associated person of the perole (Händlerrolle)
        /// </summary>
        /// <param name="sysperole">Händlerrolle</param>
        /// <returns>Person</returns>
        public override PERSON findPersonBySysperole(long sysperole)
        {
            if (!findPersonBySysperoleCache.ContainsKey(sysperole))
            {
                findPersonBySysperoleCache[sysperole] = base.findPersonBySysperole(sysperole);
            }
            return findPersonBySysperoleCache[sysperole];
        }

        /// <summary>
        /// get the associated puser of the person
        /// </summary>
        /// <param name="person">person</param>
        /// <returns>puser</returns>
        public override PUSER getPuser(PERSON person)
        {
            if (!getPuserCache.ContainsKey(person))
            {
                getPuserCache[person] = base.getPuser(person);
            }
            return getPuserCache[person];
        }

        /// <summary>
        /// findPersonByRoletypeAndSysperole
        /// </summary>
        /// <param name="roletype"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public override PERSON findPersonByRoletypeAndSysperole(long roletype, long sysperole)
        {
            String key = roletype + "_" + sysperole;
            if (!findPersonByRoletypeAndSysperoleCache.ContainsKey(key))
            {
                findPersonByRoletypeAndSysperoleCache[key] = base.findPersonByRoletypeAndSysperole(roletype, sysperole);
            }
            return findPersonByRoletypeAndSysperoleCache[key];
        }

        /// <summary>
        /// Get The information channel to communicate with this person
        /// </summary>
        /// <param name="sysid">PersonID</param>
        /// <returns>Channelstring</returns>
        public override string getinformationUeberBySysPerson(long sysid)
        {
            if (!getinformationUeberBySysPersonCache.ContainsKey(sysid))
            {
                getinformationUeberBySysPersonCache[sysid] = base.getinformationUeberBySysPerson(sysid);
            }
            return getinformationUeberBySysPersonCache[sysid];
        }

        /// <summary>
        /// finds the parent perole for the given perole and roletype
        /// </summary>
        /// <param name="sysPEROLE"></param>
        /// <param name="pRoleType"></param>
        /// <returns></returns>
        public override PEROLE findRootPEROLEObjByRoleType(long sysPEROLE, RoleTypeTyp pRoleType)
        {
            String key = sysPEROLE + "_" + pRoleType;
            if (!findRootPEROLEObjByRoleTypeCache.ContainsKey(key))
            {
                findRootPEROLEObjByRoleTypeCache[key] = base.findRootPEROLEObjByRoleType(sysPEROLE, pRoleType);
            }
            return findRootPEROLEObjByRoleTypeCache[key];
        }
    }
}