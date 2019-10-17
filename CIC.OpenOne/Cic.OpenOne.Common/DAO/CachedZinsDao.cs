using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.DAO
{
    enum ZinsDaoCacheIds
    {
        Ibor=0,
        Intsrate,
        Intsmatu,
        Intsband,
        Intstrct,
        IntstrctById,
        ProductLinks,
        IntGroups,
        IntSteps,
        PrRap,
        RapValues
    }

    class ZinsDaoCacheIdParam
    {
        private ZinsDaoCacheIds cid;
        private long id;
        public ZinsDaoCacheIdParam(ZinsDaoCacheIds cid, long id)
        {
            this.cid = cid;
            this.id = id;
        }
        public bool Equals(ZinsDaoCacheIdParam other)
        {
            return GetHashCode() == other.GetHashCode();
        }
        public override int GetHashCode()
        {
            return (int)id + (int)cid*1000000;
        }
    }
    /// <summary>
    /// Interest Rate Data Access Object
    /// </summary>
    public class CachedZinsDao : ZinsDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<ZinsDaoCacheIds, object> listCaches = CacheFactory<ZinsDaoCacheIds, object>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Zins), CacheCategory.Zins);
        private static CacheDictionary<ZinsDaoCacheIdParam, object> listCachesParam = CacheFactory<ZinsDaoCacheIdParam, object>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Zins), CacheCategory.Zins);
   
        /// <summary>
        /// Standard Constructor
        /// Database access Object for Zins
        /// </summary>
        public CachedZinsDao()
        {
          
        }

        private object getCachedData(ZinsDaoCacheIds cacheid)
        {
            if (!listCaches.ContainsKey(cacheid))
            {
                object val = null;
                switch (cacheid)
                {
                    case (ZinsDaoCacheIds.Ibor):
                        val = base.getIbor();
                        break;
                    case (ZinsDaoCacheIds.IntGroups):
                        val = base.getIntGroups();
                        break;
                    case (ZinsDaoCacheIds.Intsband):
                        val = base.getIntsband();
                        break;
                    case (ZinsDaoCacheIds.Intsmatu):
                        val = base.getIntsmatu();
                        break;
                    case (ZinsDaoCacheIds.Intsrate):
                        val = base.getIntsrate();
                        break;
                    case (ZinsDaoCacheIds.IntSteps):
                        val = base.getIntSteps();
                        break;
                    case (ZinsDaoCacheIds.Intstrct):
                        val = base.getIntstrct();
                        break;
                    case (ZinsDaoCacheIds.ProductLinks):
                        val = base.getProductLinks();
                        break;

                }
                listCaches[cacheid] = val;
            }
            return listCaches[cacheid];
        }

        private object getCachedData(ZinsDaoCacheIds cacheid, long id)
        {
            ZinsDaoCacheIdParam key = new ZinsDaoCacheIdParam(cacheid, id);
            if (!listCachesParam.ContainsKey(key))
            {
                object val = null;
                switch (cacheid)
                {
                    case (ZinsDaoCacheIds.PrRap):
                        val = base.getPrRap(id);
                        break;
                    case (ZinsDaoCacheIds.RapValues):
                        val = base.getRapValues(id);
                        break;
                    case (ZinsDaoCacheIds.IntstrctById):
                        val = base.getIntstrctById(id);
                        break;
                }
                listCachesParam[key] = val;
            }
            return listCachesParam[key];
        }
        /// <summary>
        /// Get Interest Rate
        /// </summary>
        /// <returns>Interest Rate List</returns>
        override public List<IborDto> getIbor()
        {
            return (List<IborDto>)getCachedData(ZinsDaoCacheIds.Ibor);
        }

        /// <summary>
        /// Get Interest Rate
        /// </summary>
        /// <returns>Interest Rate List</returns>
        override public List<IntsDto> getIntsrate()
        {
            return (List<IntsDto>)getCachedData(ZinsDaoCacheIds.Intsrate);
        }

        /// <summary>
        /// Get Interest Rates Matured
        /// </summary>
        /// <returns>Interest Rate List</returns>
        override public List<IntsDto> getIntsmatu()
        {
            return (List<IntsDto>)getCachedData(ZinsDaoCacheIds.Intsmatu);
        }

        /// <summary>
        /// Get Interest Rates Band
        /// </summary>
        /// <returns>Interest Rate List</returns>
        override public List<IntsDto> getIntsband()
        {
            return (List<IntsDto>)getCachedData(ZinsDaoCacheIds.Intsband);
        }

        /// <summary>
        /// Get Interest Rate Strict
        /// </summary>
        /// <returns>Interest Rate List</returns>
        override public List<IntstrctDto> getIntstrct()
        {
            return (List<IntstrctDto>)getCachedData(ZinsDaoCacheIds.Intstrct);
        }

        /// <summary>
        /// Get Interest Rate Struct by Id
        /// </summary>
        /// <returns>Interest Rate List</returns>
        override public List<IntstrctDto> getIntstrctById(long sysIntstrct)
        {
            return (List<IntstrctDto>)getCachedData(ZinsDaoCacheIds.IntstrctById, sysIntstrct);
        }


        /// <summary>
        /// Get Product Links
        /// </summary>
        /// <returns>Product List</returns>
        override public List<PRCLPRINTSETDto> getProductLinks()
        {
            return (List<PRCLPRINTSETDto>)getCachedData(ZinsDaoCacheIds.ProductLinks);
        }

        /// <summary>
        /// Get Interest Rate Groups
        /// </summary>
        /// <returns>Interest Rate Groups List</returns>
        override public List<PRINTSETDto> getIntGroups()
        {
            return (List<PRINTSETDto>)getCachedData(ZinsDaoCacheIds.IntGroups);
        }

        /// <summary>
        /// Get Ínterest Rate Steps
        /// </summary>
        /// <returns>Interest Rate Condition Link List</returns>
        override
        public List<InterestConditionLink> getIntSteps()
        {
            return (List<InterestConditionLink>)getCachedData(ZinsDaoCacheIds.IntSteps);
        }

        /// <summary>
        /// returns the prrap of the product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        override
        public PRRAP getPrRap(long sysprproduct)
        {
            return (PRRAP)getCachedData(ZinsDaoCacheIds.PrRap, sysprproduct);
        }

        /// <summary>
        /// returns the rapvalues for the rap id
        /// </summary>
        /// <param name="sysprrap"></param>
        /// <returns></returns>
        override
        public List<PRRAPVAL> getRapValues(long sysprrap)
        {
            return (List<PRRAPVAL>)getCachedData(ZinsDaoCacheIds.RapValues, sysprrap);
        }
    }
}
