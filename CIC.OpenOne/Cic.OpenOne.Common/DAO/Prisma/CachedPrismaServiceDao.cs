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
    /// <summary>
    /// Prisma Data Access Object
    /// </summary>
    public class CachedPrismaServiceDao : PrismaServiceDao
    {
        private static CacheDictionary<String, List<ServiceConditionLink>> condLinkCache = CacheFactory<String, List<ServiceConditionLink>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, List<VSTYP>> vstypCache = CacheFactory<String, List<VSTYP>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, List<PRVSDto>> vstypLinkCache = CacheFactory<String, List<PRVSDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, PRRSVCODE> rsvCodeCache = CacheFactory<String, PRRSVCODE>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, VSART> vsartCache = CacheFactory<long, VSART>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        
        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Prisma Products and Parameters
        /// </summary>
        public CachedPrismaServiceDao()
        {
        }

        /// <summary>
        /// returns all condition links for a service
        /// </summary>
        /// <param name="tableName">link typ table name</param>
        /// <returns></returns>
        override public List<ServiceConditionLink> getServiceConditionLinks(String tableName)
        {
            if (!condLinkCache.ContainsKey(tableName))
            {
                condLinkCache[tableName] = base.getServiceConditionLinks(tableName);
            }
            return condLinkCache[tableName];
        }

        /// <summary>
        /// Versicherungstypen holen
        /// </summary>
        /// <returns>Liste mit Versicherungstypen</returns>
        override public List<VSTYP> getVSTYP()
        {
            String key = "VSTYP";
            if (!vstypCache.ContainsKey(key))
            {
                vstypCache[key] = base.getVSTYP();
            }
            return vstypCache[key];

        }


        /// <summary>
        ///  returns all Link info between VSTYP and PRPRODUCT
        /// </summary>
        /// <returns>Parameter list</returns>
        override public List<PRVSDto> getVSTYPForProduct(DateTime perDate, long productID)
        {
            String key = productID + "_" + perDate.Year + "_" + perDate.Month + "_" + perDate.Day;
            if (!vstypLinkCache.ContainsKey(key))
            {
                vstypLinkCache[key] = base.getVSTYPForProduct(perDate, productID);
            }
            return vstypLinkCache[key];

        }

        /// <summary>
        /// Returns PRRSVCODE for an PRPRODUKT
        /// </summary>
        /// <param name="perDate">perDate</param>
        /// <param name="sysprprodukt">sysprprodukt</param>
        /// <returns>prrsvcode</returns>
        override public PRRSVCODE getPrrsvCodeByPrProdukt(DateTime perDate, long sysprprodukt)
        {

            if (!rsvCodeCache.ContainsKey(sysprprodukt + "_" + perDate))
            {
                rsvCodeCache[sysprprodukt + "_" + perDate] = base.getPrrsvCodeByPrProdukt(perDate, sysprprodukt);
            }
            return rsvCodeCache[sysprprodukt + "_" + perDate];

        }


        
        /// <summary>
        /// returns the VSART
        /// </summary>
        /// <param name="sysvsart"></param>
        /// <returns></returns>
        override public VSART getVSART(long sysvsart)
        {
            if (!vsartCache.ContainsKey(sysvsart))
            {
                vsartCache[sysvsart] = base.getVSART(sysvsart);
            }
            return vsartCache[sysvsart];

        }
    }
}
