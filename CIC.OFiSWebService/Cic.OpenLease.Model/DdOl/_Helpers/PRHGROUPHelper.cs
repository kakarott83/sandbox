// OWNER JJ, 04-02-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    using System.Collections.Generic;
        using Cic.OpenOne.Common.Util.Collection;
    #endregion

    /// <summary>
    /// @Deprecated old crap
    /// </summary>
    [System.CLSCompliant(true)]
    public static class PRHGROUPHelper
    {
        private static CacheDictionary<String, List<PRHGROUP>> vpprhCache = CacheFactory<String, List<PRHGROUP>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        #region Methods
       /* public static List<PRHGROUP> DeliverPrHGroupList(OlExtendedEntities context, long sysPeRole, long sysBrand)
        {
            DateTime CurrentTime = DateTime.Now;

            // get VP perole
            PEROLE VpPeRole = PEROLEHelper.FindVpOrRootPEROLE(context, sysPeRole);

            // Early return
            if (VpPeRole == null) return null;

            var QueryPrHGroupM = from prhgroupm in context.PRHGROUPM
                                 where prhgroupm.PEROLE.SYSPEROLE == VpPeRole.SYSPEROLE
                        && (prhgroupm.ACTIVEFLAG != null && prhgroupm.ACTIVEFLAG > 0 && (prhgroupm.VALIDFROM == null || prhgroupm.VALIDFROM <= CurrentTime) && (prhgroupm.VALIDUNTIL == null || prhgroupm.VALIDUNTIL >= CurrentTime))
                                 select prhgroupm.PRHGROUP.SYSPRHGROUP;

            var QueryPrBrandM = from prbrandm in context.PRBRANDM
                                where prbrandm.BRAND.SYSBRAND == sysBrand
                                && (prbrandm.ACTIVEFLAG != null && prbrandm.ACTIVEFLAG > 0 && (prbrandm.VALIDFROM == null || prbrandm.VALIDFROM <= CurrentTime) && (prbrandm.VALIDUNTIL == null || prbrandm.VALIDUNTIL >= CurrentTime))
                                select prbrandm.PRHGROUP.SYSPRHGROUP;

            var Query = from prhgroup in context.PRHGROUP
                        where QueryPrHGroupM.Contains<long>(prhgroup.SYSPRHGROUP)
                        && QueryPrBrandM.Contains<long>(prhgroup.SYSPRHGROUP)
                        && (prhgroup.ACTIVEFLAG != null && prhgroup.ACTIVEFLAG > 0 && (prhgroup.VALIDFROM == null || prhgroup.VALIDFROM <= CurrentTime) && (prhgroup.VALIDUNTIL == null || prhgroup.VALIDUNTIL >= CurrentTime))
                        orderby prhgroup.SYSPRHGROUP descending
                        select prhgroup;

            return Query.ToList<PRHGROUP>();
        }

        */
/*
        public static List<PRHGROUP> DeliverVPPrHGroupList(OlExtendedEntities context, long sysBrand, long sysVpPeRole)
        {
            String key = sysBrand + "_" + sysVpPeRole;
            if (vpprhCache.ContainsKey(key))
                return vpprhCache[key];

            DateTime CurrentTime = DateTime.Now;
            // Early return
            if (sysVpPeRole == 0) return null;

            var QueryPrHGroupM = from prhgroupm in context.PRHGROUPM
                                 where prhgroupm.PEROLE.SYSPEROLE == sysVpPeRole
                        && (prhgroupm.ACTIVEFLAG != null && prhgroupm.ACTIVEFLAG > 0 && (prhgroupm.VALIDFROM == null || prhgroupm.VALIDFROM <= CurrentTime) && (prhgroupm.VALIDUNTIL == null || prhgroupm.VALIDUNTIL >= CurrentTime))
                                 select prhgroupm.PRHGROUP.SYSPRHGROUP;

            var QueryPrBrandM = from prbrandm in context.PRBRANDM
                                where prbrandm.BRAND.SYSBRAND == sysBrand
                                && (prbrandm.ACTIVEFLAG != null && prbrandm.ACTIVEFLAG > 0 && (prbrandm.VALIDFROM == null || prbrandm.VALIDFROM <= CurrentTime) && (prbrandm.VALIDUNTIL == null || prbrandm.VALIDUNTIL >= CurrentTime))
                                select prbrandm.PRHGROUP.SYSPRHGROUP;

            var Query = from prhgroup in context.PRHGROUP
                        where QueryPrHGroupM.Contains(prhgroup.SYSPRHGROUP)
                        && QueryPrBrandM.Contains(prhgroup.SYSPRHGROUP)
                        && (prhgroup.ACTIVEFLAG != null && prhgroup.ACTIVEFLAG > 0 && (prhgroup.VALIDFROM == null || prhgroup.VALIDFROM <= CurrentTime) && (prhgroup.VALIDUNTIL == null || prhgroup.VALIDUNTIL >= CurrentTime))
                        orderby prhgroup.SYSPRHGROUP descending
                        select prhgroup;

            List<PRHGROUP> rval = Query.ToList<PRHGROUP>();
            vpprhCache[key] = rval;
            return rval;
        }*/
        #endregion
    }
}
