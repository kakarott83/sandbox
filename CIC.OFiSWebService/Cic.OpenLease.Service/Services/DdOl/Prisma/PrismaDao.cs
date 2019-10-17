using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Util.Collection;

using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenLease.Model.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Prisma Data Access Object
    /// </summary>
    public class PrismaDao : IPrismaDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string QUERYPRODUCTS = "select SYSPRPRODUCT ,SYSVART ,SYSVARTTAB  ,SYSVTTYP  , NAME   ,DESCRIPTION   ,prod.ACTIVEFLAG  ,VALIDFROM  ,VALIDUNTIL  ,SOURCEBASIS  ,SYSIBOR ,SYSINTSTRCT,SYSVG ,SYSPRRAP  ,SYSPRPRODTYPE ,SYSKALKTYP   ,SYSINTTYPE   ,NAMEINTERN ,TARIFCODE  ,SYSAKTION, SYSPRINTSETVF, SYSPRINTSETAF, SYSPRTLGSET, CODE from prproduct prod";

        private const string QUERYPRODUCTTYPES = "select * from prprodtype";

        private const string QUERYPRODUCTLINKS = "select * from {0} where ACTIVEFLAG=1";
        private const string QUERYPARAMS = "select prparam.*, prfld.objectmeta PRFLDOBJECTMETA,prfld.name prfldname  from prparam, prfld where prparam.sysprfld=prfld.sysprfld";

        private const string QUERYNEWSLINKS = "select * from {0} where ACTIVEFLAG=1";

        //all productparameter-sets from the parameterset-hierarchy without the parametersets with productlinks
        private const string QUERYPRODUCTPARSET = "select level,prparset.* from prparset left outer join prclparset on prclparset.sysprparset=prparset.sysprparset where sysprclparset is null and activeflag=1 connect by prior prparset.sysprparset=sysparent start with sysparent is null order by level";
        //private const string QUERYPRODUCTPARSETCHILDREN = "select prparset.sysprparset from prparset where activeflag=1 connect by prior prparset.sysprparset=sysparent start with sysprparset=:sysprparset";

        //all productparameter links
        // Ticket#2013010810000119 : Migration Kontext Von Konditionslink Zu Parametergruppe
        //private const string QUERYPARAMLINKS = "SELECT prclparset.*,prparset.validfrom, prparset.validuntil FROM prclparset, prparset " +
        //                                       " WHERE prclparset.sysprparset = prparset.sysprparset AND prparset.ActiveFlag = 1  " +
        //                                       " order by rank desc, case prclparset.area when 99 then 1 when 0 then 1 when 1 then 2 when 3 then 3 when 2 then 4 else 5 end";
        private const string QUERYPARAMLINKS = "SELECT prclparset.*,prparset.validfrom, prparset.validuntil FROM prclparset, prparset WHERE prclparset.sysprparset = prparset.sysprparset AND prparset.ActiveFlag = 1  order by rank desc, case prclparset.area when 99 then 1 when 0 then 1 when 1 then 2 when 3 then 3 when 2 then 4 else 5 end";

        private const string QUERYPRFLDS = "select * from prfld";
        private const string QUERYVART = "select vart.* from vart,prproduct where prproduct.sysvart=vart.sysvart and prproduct.sysprproduct=:sysprproduct";

        private static CacheDictionary<String, List<PRPARAMDto>> paramCache = CacheFactory<String, List<PRPARAMDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<PRPRODUCTDto>> prodCache = CacheFactory<String, List<PRPRODUCTDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<PRPRODTYPE>> ptypeCache = CacheFactory<String, List<PRPRODTYPE>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<ParameterSetConditionLink>> psetCLinkCache = CacheFactory<String, List<ParameterSetConditionLink>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<ParameterConditionLink>> parCLinkCache = CacheFactory<String, List<ParameterConditionLink>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<ProductConditionLink>> prodCLinkCache = CacheFactory<String, List<ProductConditionLink>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<PRFLD>> fieldCache = CacheFactory<String, List<PRFLD>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Prisma Products and Parameters
        /// </summary>
        public PrismaDao()
        {
        }

        /// <summary>
        ///  returns all Prisma Parameters 
        /// </summary>
        /// <returns>Parameter list</returns>
        public virtual List<PRPARAMDto> getParams()
        {
            if (!paramCache.ContainsKey("PARAMS"))
            {
                using (OlExtendedEntities ctx = new OlExtendedEntities())
                {
                    paramCache["PARAMS"] = ctx.ExecuteStoreQuery<PRPARAMDto>(QUERYPARAMS, null).ToList();
                }
            }
            return paramCache["PARAMS"];
        }


        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="sysprproduct">Product ID</param>
        /// <returns>Product Data</returns>
        public virtual PRPRODUCTDto getProduct(long sysprproduct)
        {
            return getProducts().Where(p => p.SYSPRPRODUCT == sysprproduct).FirstOrDefault();
        }



        /// <summary>
        /// returns all Prisma Products
        /// </summary>
        /// <returns>Product list</returns>
        public virtual List<PRPRODUCTDto> getProducts()
        {
            if (!prodCache.ContainsKey("PRODUCTS"))
            {
                using (OlExtendedEntities ctx = new OlExtendedEntities())
                {
                    prodCache["PRODUCTS"] = ctx.ExecuteStoreQuery<PRPRODUCTDto>(QUERYPRODUCTS, null).ToList();
                }

            }
            return prodCache["PRODUCTS"];
        }




        /// <summary>
        /// returns all Product types
        /// </summary>
        /// <returns>Product List</returns>
        public virtual List<PRPRODTYPE> getProductTypes()
        {
            if (!ptypeCache.ContainsKey("PTYPE"))
            {
                using (OlExtendedEntities ctx = new OlExtendedEntities())
                {
                    ptypeCache["PTYPE"] = ctx.ExecuteStoreQuery<PRPRODTYPE>(QUERYPRODUCTTYPES, null).ToList();
                }
            }
            return ptypeCache["PTYPE"];
        }

        /// <summary>
        /// returns all Product Condition Links
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Product Condition List</returns>
        public virtual List<ProductConditionLink> getProductConditionLinks(String tableName)
        {
            if (!prodCLinkCache.ContainsKey(tableName))
            {
                using (OlExtendedEntities ctx = new OlExtendedEntities())
                {
                    prodCLinkCache[tableName] = ctx.ExecuteStoreQuery<ProductConditionLink>(String.Format(QUERYPRODUCTLINKS, tableName), null).ToList();

                }
            }
            return prodCLinkCache[tableName];
        }

        /// <summary>
        /// returns all Product Parameter Sets (Hierarchical) for Sets not linked to Products
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public virtual List<ParameterSetConditionLink> getParamSets()
        {

            if (!psetCLinkCache.ContainsKey("PSETS"))
            {
                using (OlExtendedEntities ctx = new OlExtendedEntities())
                {
                    psetCLinkCache["PSETS"] = ctx.ExecuteStoreQuery<ParameterSetConditionLink>(QUERYPRODUCTPARSET, null).ToList();

                }
            }
            return psetCLinkCache["PSETS"];
        }

      /*  /// <summary>
        /// returns all Product Parameter Sets (Hierarchical) Children
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public virtual List<ParameterSetConditionLink> getParamSetChildren(long sysprparset)
        {
            if (!psetCLinkCache.ContainsKey(""+sysprparset))
            {
                using (OlExtendedEntities ctx = new OlExtendedEntities())
                {
                    object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprparset", Value = sysprparset } };

                    psetCLinkCache["" + sysprparset] = ctx.ExecuteStoreQuery<ParameterSetConditionLink>(QUERYPRODUCTPARSETCHILDREN, pars).ToList();

                }
            }
            return psetCLinkCache["" + sysprparset];
        }*/


        /// <summary>
        /// returns all Product Parameter Sets linked to Products
        /// </summary>
        /// <returns>Parameter Condition Link List</returns>
        public virtual List<ParameterConditionLink> getParamConditionLinks()
        {
            if (!parCLinkCache.ContainsKey("PAR"))
            {
                using (OlExtendedEntities ctx = new OlExtendedEntities())
                {
                    parCLinkCache["PAR"] = ctx.ExecuteStoreQuery<ParameterConditionLink>(QUERYPARAMLINKS, null).ToList();
                }
            }
            return parCLinkCache["PAR"];
        }

        /// <summary>
        /// Get Prisma Fields
        /// </summary>
        /// <returns></returns>
        public virtual List<PRFLD> getFields()
        {

            if (!fieldCache.ContainsKey("FIELD"))
            {
                using (OlExtendedEntities ctx = new OlExtendedEntities())
                {
                    fieldCache["FIELD"] = ctx.ExecuteStoreQuery<PRFLD>(QUERYPRFLDS, null).ToList();

                }
            }
            return fieldCache["FIELD"];
        }


    }
}
