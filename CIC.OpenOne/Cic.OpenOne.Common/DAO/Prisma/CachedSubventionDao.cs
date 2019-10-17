using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.DAO.Prisma
{

    enum SubventionDaoCacheIds
    {
        Triggers,
        Subventions
    }
    /// <summary>
    /// Subvention Data Access Object
    /// </summary>
    public class CachedSubventionDao : SubventionDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<SubventionDaoCacheIds, object> listCaches = CacheFactory<SubventionDaoCacheIds, object>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, List<PRSUBVPOS>> subvPositionsCache = CacheFactory<long, List<PRSUBVPOS>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, List<PRSUBVPOS>> subvPositionsCache2 = CacheFactory<String, List<PRSUBVPOS>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, List<long>> explPositionsCache = CacheFactory<String, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, List<PrSubvTriggerDto>> triggerCache = CacheFactory<String, List<PrSubvTriggerDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);


        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Subventions
        /// </summary>
        public CachedSubventionDao()
        {

        }
        private object getCachedData(SubventionDaoCacheIds cacheid)
        {
            if (!listCaches.ContainsKey(cacheid))
            {
                object val = null;
                switch (cacheid)
                {
                    case (SubventionDaoCacheIds.Subventions):
                        val = base.getSubventions();
                        break;
                }
                listCaches[cacheid] = val;
            }
            return listCaches[cacheid];
        }


        /// <summary>
        /// Delivers all subvention trigger infos as of Prisma Concept 5.6.2.1
        /// </summary>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public override List<PrSubvTriggerDto> getSubventionTriggers(DateTime perDate)
        {
            String key = perDate.Year + "_" + perDate.Month + "_" + perDate.Day;

            if (!triggerCache.ContainsKey(key))
            {
                triggerCache[key] = (List<PrSubvTriggerDto>)base.getSubventionTriggers(perDate);
            }
            return triggerCache[key];
        }

        /// <summary>
        /// returns all Subventions
        /// </summary>
        /// <returns></returns>
        public override List<PRSUBV> getSubventions()
        {
            return (List<PRSUBV>)getCachedData(SubventionDaoCacheIds.Subventions);
        }

        /// <summary>
        /// returns all Subvention Positions
        /// </summary>
        /// <returns></returns>
        public override List<PRSUBVPOS> getSubventionPositions(long sysprsubv)
        {
            if (!subvPositionsCache.ContainsKey(sysprsubv))
            {
                subvPositionsCache[sysprsubv] = base.getSubventionPositions(sysprsubv);
            }
            return subvPositionsCache[sysprsubv];
        }

        /// <summary>
        /// returns all Subvention Positions
        /// </summary>
        /// <returns></returns>
        public override List<PRSUBVPOS> getSubventionPositions(long sysprsubv, double betrag)
        {
            String key = sysprsubv + "_" + betrag;
            if (!subvPositionsCache2.ContainsKey(key))
            {
                subvPositionsCache2[key] = base.getSubventionPositions(sysprsubv, betrag);
            }
            return subvPositionsCache2[key];
        }

        /// <summary>
        /// returns all excplicit subventions ids for the given area and id  as of Prisma Concept 5.6.2.2
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public override List<long> getExplicitSubventionIds(ExplicitSubventionArea area, long areaId)
        {
            String key = area + "_" + areaId;
            if (!explPositionsCache.ContainsKey(key))
            {
                explPositionsCache[key] = base.getExplicitSubventionIds(area, areaId);
            }
            return explPositionsCache[key];
        }

    }
}
