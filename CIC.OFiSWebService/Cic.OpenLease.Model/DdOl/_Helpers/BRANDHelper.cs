// OWNER MK, 01-27-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System.Linq;
    using System.Collections.Generic;
    using Cic.OpenOne.Common.Util.Collection;
    using System;
    
    #endregion

    [System.CLSCompliant(true)]
    public class BRANDHelper
    {
        private static CacheDictionary<long, BRAND> brandCache = CacheFactory<long, BRAND>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        #region Methods
        public static List<BRAND> DeliverBRANDList(OlExtendedEntities context)
        {
            if (context == null)
            {
                throw new Exception("context");
            }

            var Query = from brand in context.BRAND
                        select brand;

            return Query.ToList<BRAND>();
        }

        public static BRAND DeliverBRAND(OlExtendedEntities context, long sysBRAND)
        {
            if (!brandCache.ContainsKey(sysBRAND))
            {
                if (context == null)
                {
                    throw new Exception("context");
                }

                var Query = from brand in context.BRAND
                            where brand.SYSBRAND == sysBRAND
                            select brand;

                brandCache[sysBRAND] = Query.FirstOrDefault<BRAND>();
            }
            return brandCache[sysBRAND];
        }

        public static bool CheckPeroleInBrand(OlExtendedEntities context, long sysPEROLE, long sysBRAND)
        {
            List<long> IntersectList = null;
            bool IsValid = false;

            if (context == null)
            {
                throw new Exception("context");
            }

            // Set current date
            System.DateTime CurrentTime = System.DateTime.Now.Date;

            // Get SYSPRHGROUP list by sysPEROLE
            var Query1 = from prhgroupm in context.PRHGROUPM
                        where prhgroupm.PEROLE.SYSPEROLE == sysPEROLE
                        select prhgroupm;

            var Query2 = from prhgroupm in Query1
                         where (prhgroupm.ACTIVEFLAG != null && prhgroupm.ACTIVEFLAG > 0 && (prhgroupm.VALIDFROM == null || prhgroupm.VALIDFROM <= CurrentTime) && (prhgroupm.VALIDUNTIL == null || prhgroupm.VALIDUNTIL >= CurrentTime))
                         select prhgroupm.PRHGROUP.SYSPRHGROUP;

            // Create list
            List<long> SYSPRHGROUPListFromPerole = Query2.ToList<long>();

            if (SYSPRHGROUPListFromPerole != null && SYSPRHGROUPListFromPerole.Count > 0)
            {
                // Get SYSPRHGROUP list by sysBRAND
                var Query3 = from prbrandm in context.PRBRANDM
                             where prbrandm.BRAND.SYSBRAND == sysBRAND
                             select prbrandm;

                var Query4 = from brand in Query3
                             where (brand.ACTIVEFLAG != null && brand.ACTIVEFLAG > 0 && (brand.VALIDFROM == null || brand.VALIDFROM <= CurrentTime) && (brand.VALIDUNTIL == null || brand.VALIDUNTIL >= CurrentTime))
                             select brand.PRHGROUP.SYSPRHGROUP;

                // Create list
                List<long> SYSPRHGROUPlistFromBrand = Query4.ToList<long>();

                if (SYSPRHGROUPlistFromBrand != null && SYSPRHGROUPlistFromBrand.Count > 0)
                {
                    // Make intersect
                    IntersectList = SYSPRHGROUPListFromPerole.Intersect(SYSPRHGROUPlistFromBrand).ToList();
                }
            }

            // Check intersect list
            if (IntersectList != null && IntersectList.Count > 0)
            {
                var tq = from p in context.PRHGROUP
                         where 
                           p.ACTIVEFLAG!=null 
                          && p.ACTIVEFLAG > 0 && (p.VALIDFROM == null || p.VALIDFROM <= CurrentTime) && (p.VALIDUNTIL == null || p.VALIDUNTIL >= CurrentTime)
                          && IntersectList.Contains(p.SYSPRHGROUP)
                          select p.SYSPRHGROUP;
                IsValid = tq.Any();
                         
               /* var Query = context.PRHGROUP.Where(Cic.Basic.Data.Objects.EntityFrameworkHelper.BuildContainsExpression<Cic.OpenLease.Model.DdOl.PRHGROUP, long>(prhgroup => prhgroup.SYSPRHGROUP, IntersectList));
                Query = Query.Where(prhgroup => prhgroup.ACTIVEFLAG != null && prhgroup.ACTIVEFLAG > 0 && (prhgroup.VALIDFROM == null || prhgroup.VALIDFROM <= CurrentTime) && (prhgroup.VALIDUNTIL == null || prhgroup.VALIDUNTIL >= CurrentTime));

                // Get any
                IsValid = Query.Any<PRHGROUP>();*/
            }

            return IsValid;            
        }

        #endregion
    }
}
