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
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Collection;
using System.Collections;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    enum PrismaDaoCacheIds
    {
        Params,
        ParamSets,
        ParamConditionLinks,
        News,
        ProductTypes,
        Fields,
        BildweltVart
    }
    class DiffInfo
    {
        public long cnt {get;set;}
        public long sysprproduct { get; set; }
    }

    class VARTCacheInfo
    {
       
        public int? AKTIVKZ { get; set; }
      
        public string BEZEICHNUNG { get; set; }
       
        public string CODE { get; set; }
       
        public double? LGD { get; set; }
       
        public long SYSVART { get; set; }

        public long SYSPRPRODUCT { get; set; }

        public VART getVART()
        {
            VART rval = new VART();
            rval.AKTIVKZ = AKTIVKZ;
            rval.BEZEICHNUNG = BEZEICHNUNG;
            rval.CODE = CODE;
            rval.LGD = LGD;
            rval.SYSVART = SYSVART;
            return rval;
        }
    }

   
    /// <summary>
    /// Cached Prisma Data Access Object
    /// </summary>
    public class CachedPrismaDao : PrismaDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static CacheDictionary<long, List<ParameterSetConditionLink>> paramSetChildrenCache = CacheFactory<long, List<ParameterSetConditionLink>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, VART> vartCache = CacheFactory<long, VART>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, VTTYP> vttypCache = CacheFactory<long, VTTYP>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, VTTYP> vttypIdCache = CacheFactory<long, VTTYP>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        private static CacheDictionary<String, List<PRPRODUCT>> prodCache = CacheFactory<String, List<PRPRODUCT>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<PrismaDaoCacheIds, object> listCaches = CacheFactory<PrismaDaoCacheIds, object>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, List<ProductConditionLink>> prodCondLinkCache = CacheFactory<String, List<ProductConditionLink>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, bool> diffLeasCache = CacheFactory<long, bool>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, ParamDto> zinsParamCache = CacheFactory<long, ParamDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, KundenzinsDto> zinsCache = CacheFactory<String, KundenzinsDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Zins), CacheCategory.Zins);
        private static CacheDictionary<String, List<NewsConditionLink>> newsLinkCache = CacheFactory<String, List<NewsConditionLink>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        private const string QUERYDIFF = "select prproduct.sysprproduct, NVL(sysprclprsubvset,0) cnt from prproduct left outer join prclprsubvset on prclprsubvset.sysprproduct=prproduct.sysprproduct";
        private const string QUERYVART = "select vart.*, sysprproduct from vart,prproduct where prproduct.sysvart=vart.sysvart";

        
        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Prisma Products and Parameters
        /// </summary>
        public CachedPrismaDao()
            : base()
        {

        }

        private object getCachedData(PrismaDaoCacheIds cacheid)
        {
            if (!listCaches.ContainsKey(cacheid))
            {
                object val = null;
                switch (cacheid)
                {
                    case (PrismaDaoCacheIds.Params):
                        val = base.getParams();
                        break;
                    case (PrismaDaoCacheIds.ParamSets):
                        val = base.getParamSets();
                        break;
                    case (PrismaDaoCacheIds.ParamConditionLinks):
                        val = base.getParamConditionLinks();
                        break;
                    case (PrismaDaoCacheIds.News):
                        val = base.getNews();
                        break;
                    case (PrismaDaoCacheIds.ProductTypes):
                        val = base.getProductTypes();
                        break;
                    case (PrismaDaoCacheIds.Fields):
                        val = base.getFields();
                        break;
                    case (PrismaDaoCacheIds.BildweltVart):
                        val = base.getBildweltVertragsarten();
                        break;

                }
                listCaches[cacheid] = val;
            }
            return listCaches[cacheid];
        }

        /// <summary>
        ///  returns all Prisma Parameters 
        /// </summary>
        /// <returns>Parameter list</returns>
        public override List<ParamDto> getParams()
        {
            return new List<ParamDto>((List<ParamDto>)getCachedData(PrismaDaoCacheIds.Params));
        }


        /// <summary>
        /// returns all Product Parameter Sets (Hierarchical) for Sets not linked to Products
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public override List<ParameterSetConditionLink> getParamSets()
        {
            return new List<ParameterSetConditionLink>((List<ParameterSetConditionLink>)getCachedData(PrismaDaoCacheIds.ParamSets));

        }

        /// <summary>
        /// returns all Product Parameter Sets linked to Products
        /// </summary>
        /// <returns>Parameter Condition Link List</returns>
        public override List<ParameterConditionLink> getParamConditionLinks()
        {
            return new List<ParameterConditionLink>((List<ParameterConditionLink>)getCachedData(PrismaDaoCacheIds.ParamConditionLinks));

        }


        /// <summary>
        /// Get Vertragsart
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Vertragsart</returns>
        public override VART getVertragsart(long sysprproduct)
        {
            if (!vartCache.ContainsKey(sysprproduct))
            {
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<VARTCacheInfo> vartInfos = ctx.ExecuteStoreQuery<VARTCacheInfo>(QUERYVART, null).ToList();
                    foreach (VARTCacheInfo vai in vartInfos)
                    {
                        vartCache[vai.SYSPRPRODUCT] = vai.getVART();
                    }
                }
               
            }
            return vartCache[sysprproduct];
        }

        /// <summary>
        /// delivers the vttyp
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public override VTTYP getVttyp(long sysprproduct)
        {
            if (!vttypCache.ContainsKey(sysprproduct))
            {
                vttypCache[sysprproduct] = base.getVttyp(sysprproduct);

            }
            return vttypCache[sysprproduct];
        }

        /// <summary>
        /// Delivers the vttyp by sysvttyp
        /// </summary>
        /// <param name="sysvttyp"></param>
        /// <returns></returns>
        public override VTTYP getVttypById(long sysvttyp)
        {
            if (!vttypIdCache.ContainsKey(sysvttyp))
            {
                vttypIdCache[sysvttyp] = base.getVttypById(sysvttyp);

            }
            return vttypIdCache[sysvttyp];
        }

        /// <summary>
        /// returns all Product Parameter Sets (Hierarchical) Children
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public override List<ParameterSetConditionLink> getParamSetChildren(long sysprparset)
        {

            if (!paramSetChildrenCache.ContainsKey(sysprparset))
            {
                paramSetChildrenCache[sysprparset] = base.getParamSetChildren(sysprparset);
            }
            return new List<ParameterSetConditionLink>(paramSetChildrenCache[sysprparset]);
        }


        /// <summary>
        /// returns all Prisma Products
        /// </summary>
        /// <returns>Product list</returns>
        public override List<PRPRODUCT> getProducts(String isoCode)
        {
            if (!prodCache.ContainsKey(isoCode))
            {
                prodCache[isoCode] = base.getProducts(isoCode);
            }
            return new List<PRPRODUCT>(prodCache[isoCode]);
        }

        /// <summary>
        /// Get all News
        /// </summary>
        /// <returns>News</returns>
        public override List<PRNEWS> getNews()
        {
            return new List<PRNEWS>( (List<PRNEWS>)getCachedData(PrismaDaoCacheIds.News));
        }

        /// <summary>
        /// returns all Product types
        /// </summary>
        /// <returns>Product List</returns>
        public override List<PRPRODTYPE> getProductTypes()
        {
            return new List<PRPRODTYPE>((List<PRPRODTYPE>)getCachedData(PrismaDaoCacheIds.ProductTypes));
        }

        /// <summary>
        /// returns all Product Condition Links
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Product Condition List</returns>
        public override List<ProductConditionLink> getProductConditionLinks(String tableName)
        {
            if (!prodCondLinkCache.ContainsKey(tableName))
            {
                prodCondLinkCache[tableName] = base.getProductConditionLinks(tableName);
            }
            return new List<ProductConditionLink>(prodCondLinkCache[tableName]);

        }

        /// <summary>
        /// Get Prisma Fields
        /// </summary>
        /// <returns></returns>
        public override List<PRFLD> getFields()
        {
            return new List<PRFLD>( (List<PRFLD>)getCachedData(PrismaDaoCacheIds.Fields));
        }

        /// <summary>
        /// Returns true if product is difference Leasing
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public override bool isDiffLeasing(long sysPrProduct)
        {
            if (diffLeasCache.Count==0)//!diffLeasCache.ContainsKey(sysPrProduct))
            {

                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<DiffInfo> diffInfos = ctx.ExecuteStoreQuery<DiffInfo>(QUERYDIFF, null).ToList();
                    foreach(DiffInfo di in diffInfos)
                    {
                        diffLeasCache[di.sysprproduct] = di.cnt>0;
                    }
                }
               
            }
            return diffLeasCache[sysPrProduct];
        }

        /// <summary>
        /// Returns the 'virtual' Prisma Parameter for Kundenzins, generated from the zinsstructure
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public override ParamDto getKundenzinsParam(long sysPrProduct)
        {
            if (!zinsParamCache.ContainsKey(sysPrProduct))
            {
                zinsParamCache[sysPrProduct] = base.getKundenzinsParam(sysPrProduct);
            }
            return zinsParamCache[sysPrProduct];


        }

        /// <summary>
        /// Returns the 'virtual' extended Prisma Parameter for Kundenzins, generated from the zinsstructure
        /// </summary>
        /// <param name="sysPrProduct">ProduktID</param>
        /// <param name="laufzeit">laufzeit</param>
        /// <param name="saldo">Saldo</param>
        /// <returns></returns>
        public override KundenzinsDto getKundenzins(long sysPrProduct, long laufzeit, double saldo)
        {
            String key = sysPrProduct + "_" + laufzeit + "_" + saldo;
            if (!zinsCache.ContainsKey(key))
            {
                zinsCache[key] = base.getKundenzins(sysPrProduct, laufzeit, saldo);
            }
            return zinsCache[key];

        }


        /// <summary>
        /// returns all News Condition Links
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Product Condition List</returns>
        public override List<NewsConditionLink> getNewsConditionLinks(String tableName)
        {
            if (!newsLinkCache.ContainsKey(tableName))
            {
                newsLinkCache[tableName] = base.getNewsConditionLinks(tableName);
            }
            return new List<NewsConditionLink>( newsLinkCache[tableName]);


        }

        /// <summary>
        /// Delivers Vertragsarten for different bildwelten
        /// </summary>
        /// <returns></returns>
        public override List<PrBildweltVDto> getBildweltVertragsarten()
        {
            return new List<PrBildweltVDto>( (List<PrBildweltVDto>)getCachedData(PrismaDaoCacheIds.BildweltVart));
        }
    }
}
