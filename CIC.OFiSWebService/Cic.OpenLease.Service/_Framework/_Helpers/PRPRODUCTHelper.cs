
namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Cic.OpenOne.Common.Util.Collection;
    using CIC.Database.OL.EF6.Model;
    using Cic.OpenOne.Common.Model.DdOl;
    #endregion

    [System.CLSCompliant(true)]
    public static class PRPRODUCTHelper
    {

        private static CacheDictionary<String, List<PRPRODUCT>> pCache = CacheFactory<String, List<PRPRODUCT>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        #region Public constants
        public const int ConditionTypeAktions = 2;
        public const int ConditionTypeStandard = 1;
        #endregion

        #region Methods
        public static long DeliverSYSVART(DdOlExtended context, long sysPrProduct)
        {


            var Query = from prProduct in context.PRPRODUCT
                        where prProduct.SYSPRPRODUCT == sysPrProduct
                        select prProduct.VART.SYSVART;

            return Query.FirstOrDefault();

        }
        public static System.Collections.Generic.List<PRPRODUCT> DeliverPRPRODUCTs(DdOlExtended context, long sysPerson)
        {
            System.Collections.Generic.List<PRPRODUCT> PrProductList;

            PrProductList = new System.Collections.Generic.List<PRPRODUCT>();

            var hgrpQuery = from hgrpm in context.PRHGROUPM
                            where hgrpm.PEROLE.SYSPERSON == sysPerson
                            select hgrpm.PRHGROUP.SYSPRHGROUP;

            System.Collections.Generic.List<long> SysHGrpList = null;

            try
            {
                SysHGrpList = hgrpQuery.ToList<long>();
            }
            catch
            {
                throw;
            }

            if (SysHGrpList != null && SysHGrpList.Count > 0)
            {
                foreach (long SysHGrpLoop in SysHGrpList)
                {
                    var productQuery = from hgrpg in context.PRCLPRHG
                                     where hgrpg.PRHGROUP.SYSPRHGROUP == SysHGrpLoop
                                     select hgrpg.PRPRODUCT;

                    PrProductList.AddRange(productQuery.ToList());
                }
            }

            if (PrProductList != null && PrProductList.Count > 0)
            {
                // return list
                return PrProductList.Distinct<PRPRODUCT>().ToList<PRPRODUCT>();
            }
            else
            {
                // Return eempty
                return new System.Collections.Generic.List<PRPRODUCT>();
            }
        }
        /*
        public static long DeliverSYSVART(OlExtendedEntities context, long sysPrProduct)
        {
            List<PRPRODUCT> products = rebuild(context);

            var Query = from prProduct in products
                               where prProduct.SYSPRPRODUCT == sysPrProduct
                               select prProduct.VART.SYSVART;

            return Query.FirstOrDefault();

        }
        */
       

        public static int? DeliverConditionType(DdOlExtended context, long sysPrProduct)
        {
            List<PRPRODUCT> products = rebuild(context);

            PRPRODTYPE p = (from prProduct in products
                        where prProduct.SYSPRPRODUCT == sysPrProduct
                        select prProduct.PRPRODTYPE).FirstOrDefault();
            if (p == null) return null;

            return p.CONDITIONTYPE;

        }

        public static KALKTYP DeliverKalkTyp(DdOlExtended context, long sysPrProduct)
        {

            List<PRPRODUCT> products = rebuild(context);

            var KalkTypQuery = from prProduct in products
                                  where prProduct.SYSPRPRODUCT == sysPrProduct
                                  select prProduct.KALKTYP;

            KALKTYP KalkTyp;

            try
            {
                KalkTyp = KalkTypQuery.FirstOrDefault<KALKTYP>();
            }
            catch
            {
                throw;
            }
            
            return KalkTyp;
            
        }

        public static long DeliverSysInttype(DdOlExtended context, long sysPrProduct)
        {

            List<PRPRODUCT> products =  rebuild(context);
            var query = from prProduct in products
                               where prProduct.SYSPRPRODUCT == sysPrProduct
                        select prProduct.INTTYPE.SYSINTTYPE;

            try
            {
                return query.FirstOrDefault<long>();
            }
            catch
            {
                throw;
            }

           

        }


        private static List<PRPRODUCT> rebuild(DdOlExtended context)
        {
            if (!pCache.ContainsKey("PRODUCTS"))
            {

                var Query = from p in context.PRPRODUCT.Include("INTTYPE").Include("KALKTYP").Include("VART").Include("PRPRODTYPE")
                            select p;
                pCache["PRODUCTS"] = Query.ToList<PRPRODUCT>();
            }
            return pCache["PRODUCTS"];
        }
        


        #endregion
    }
}
