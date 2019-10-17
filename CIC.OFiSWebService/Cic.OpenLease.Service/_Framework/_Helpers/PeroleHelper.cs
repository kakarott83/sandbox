using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    public class PersonDto
    {
        public long sysperson { get; set; }
        public String NAME { get; set; }
        public String VORNAME { get; set; }
    }
    public class PeroleDto
    {
        public long sysperole { get; set; }
        public long sysparent { get; set; }
        public long sysperson { get; set; }
        public long sysroletype { get; set; }
        public String NAME { get; set; }
    }

   
    public class PeroleHelper
    {
        #region Private constants
        public const long CnstVPRoleTypeNumber = 6;
        public const long CnstIMRoleTypeNumber = 3;


        public const long CnstBusinessLineRoleTypeNumber = 14;
        public const long CnstHaendlerRoleTypeNumber = 6;
        public const long CnstInternerMitarbeiterRoleTypeNumber = 3;
        public const long CnstVerkaueferTypeNumber = 7;
        public const long CnstGebietsleiterTypeNumber = 9;
        public const long CnstAussendienstTypeNumber = 4;
        #endregion

        private static CacheDictionary<String, List<PRHGROUP>> vpprhCache = CacheFactory<String, List<PRHGROUP>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<long>> prhgroupListCache = CacheFactory<String, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, PersonDto> personCache = CacheFactory<long, PersonDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, PeroleDto> peroleDtoCache = CacheFactory<long, PeroleDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, long> peroleCache = CacheFactory<String, long>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, PEROLE> peroleCache2 = CacheFactory<String, PEROLE>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Methods

        /// <summary>
        /// Valid PEROLE has Handelsguppe
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPerson"></param>
        /// <returns></returns>
        public static List<PEROLE> DeliverValidPeRoles(DdOlExtended context, long sysPerson)
        {
            List<PEROLE> PeroleList;

            // PeRole list for a given person
            var Query = from perole in context.PEROLE
                        where perole.SYSPERSON == sysPerson
                        select perole;

            PeroleList = Query.ToList();

            return PeroleList;
        }

        /// <summary>
        /// get the parent-role for the given roletype.typ
        /// @Deprecated, use CODE version, because multiple roletypes of same type are now available!
        /// Use this only if the found role is only needed to determine if the user has one roletype.typ, but when its not important which kind (CODE/RANK) it is
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="pRoleType"></param>
        /// <returns></returns>
        public static long FindRootPEROLEByRoleType(DdOlExtended context, long sysPEROLE, long pRoleType)
        {
            String key = sysPEROLE + "_" + pRoleType;
            if (peroleCache.ContainsKey(key))
                return peroleCache[key];

            string query = "select perole.* from perole, roletype where "
                // + " (perole.validuntil is null or perole.validuntil>=TRUNC(SYSDATE)) and (perole.validfrom is null or perole.validfrom<=TRUNC(SYSDATE)) "
                + SQL.CheckCurrentSysDate ("perole")
                + " and  perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=" + sysPEROLE + ") and roletype.typ=" + pRoleType+"  order by decode(roletype.code,'HD',1,'MAGRP',2,'REGION',3,'GRP',4,'DIST',5)";
            PEROLE rval1 = context.ExecuteStoreQuery<PEROLE>(query).FirstOrDefault<PEROLE>();
            if (rval1 == null) return 0;
            peroleCache[key] = rval1.SYSPEROLE;

            return rval1.SYSPEROLE;
        }
        /// <summary>
        /// get the parent-role for the given roletype.code
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static long FindRootPEROLEByRoleTypeCODE(DdOlExtended context, long sysPEROLE, String code)
        {
            String key = sysPEROLE + "_" + code;
            if (peroleCache.ContainsKey(key))
                return peroleCache[key];

            string query = "select perole.* from perole, roletype where "
                // + " (perole.validuntil is null or perole.validuntil>=TRUNC(SYSDATE)) and (perole.validfrom is null or perole.validfrom<=TRUNC(SYSDATE)) "
                + SQL.CheckCurrentSysDate("perole")
                + " and  perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=" + sysPEROLE + ") and roletype.code='" + code+"'";
            PEROLE rval1 = context.ExecuteStoreQuery<PEROLE>(query).FirstOrDefault<PEROLE>();
            if (rval1 == null) return 0;
            peroleCache[key] = rval1.SYSPEROLE;

            return rval1.SYSPEROLE;
        }
        /// <summary>
        /// @DEPRECATED, use ..CODE-Version!
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="pRoleType"></param>
        /// <returns></returns>
        public static PEROLE FindRootPEROLEObjByRoleType(DdOlExtended context, long sysPEROLE, long pRoleType)
        {
            String key = sysPEROLE + "_" + pRoleType;
            if (peroleCache2.ContainsKey(key))
                return peroleCache2[key];

            //context.PEROLE.EnablePlanCaching = true;
            //context.PEROLE.MergeOption = MergeOption.NoTracking;

            string query = "select perole.* from perole, roletype where "
                // + " (perole.validuntil is null or perole.validuntil>=TRUNC(SYSDATE)) and (perole.validfrom is null or perole.validfrom<=TRUNC(SYSDATE)) "
                + SQL.CheckCurrentSysDate ("perole")
                + " and  perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=" + sysPEROLE + ") and roletype.typ=" + pRoleType;
            PEROLE rval1 = context.ExecuteStoreQuery<PEROLE>(query).FirstOrDefault<PEROLE>();
            peroleCache2[key] = rval1;

            return rval1;
        }
        public static PEROLE FindRootPEROLEObjByRoleTypeCODE(DdOlExtended context, long sysPEROLE, String code)
        {
            String key = sysPEROLE + "_" + code;
            if (peroleCache2.ContainsKey(key))
                return peroleCache2[key];

            //context.PEROLE.EnablePlanCaching = true;
            //context.PEROLE.MergeOption = MergeOption.NoTracking;

            string query = "select perole.* from perole, roletype where "
                // + " (perole.validuntil is null or perole.validuntil>=TRUNC(SYSDATE)) and (perole.validfrom is null or perole.validfrom<=TRUNC(SYSDATE)) "
                + SQL.CheckCurrentSysDate("perole")
                + " and  perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=" + sysPEROLE + ") and roletype.code='" + code+"'";
            PEROLE rval1 = context.ExecuteStoreQuery<PEROLE>(query).FirstOrDefault<PEROLE>();
            peroleCache2[key] = rval1;

            return rval1;
        }

        public static List<PRHGROUP> DeliverVPPrHGroupList(DdOlExtended context, long sysBrand, long sysVpPeRole)
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
        }

        public static List<long> DeliverPrHGroupList(DdOlExtended context, long sysPeRole, long sysBrand)
        {
            String key = sysPeRole+"_"+sysBrand;
            if (!prhgroupListCache.ContainsKey(key))
            {
                DateTime CurrentTime = DateTime.Now;

                // get VP perole
                long sysVpPeRole = FindRootPEROLEByRoleType(context, sysPeRole, CnstVPRoleTypeNumber);

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
                            where QueryPrHGroupM.Contains<long>(prhgroup.SYSPRHGROUP)
                            && QueryPrBrandM.Contains<long>(prhgroup.SYSPRHGROUP)
                            && (prhgroup.ACTIVEFLAG != null && prhgroup.ACTIVEFLAG > 0 && (prhgroup.VALIDFROM == null || prhgroup.VALIDFROM <= CurrentTime) && (prhgroup.VALIDUNTIL == null || prhgroup.VALIDUNTIL >= CurrentTime))
                            orderby prhgroup.SYSPRHGROUP descending
                            select prhgroup.SYSPRHGROUP;

                prhgroupListCache[key] = Query.ToList<long>();
            }
            return prhgroupListCache[key];
        }

        public static PersonDto DeliverPerson(DdOlExtended context, long sysPeRole)
        {
            if (!personCache.ContainsKey(sysPeRole))
            {
                try
                {
                    personCache[sysPeRole] = context.ExecuteStoreQuery<PersonDto>("select person.name, person.vorname, person.sysperson from perole, person where person.sysperson=perole.sysperson and perole.sysperole=" + sysPeRole, null).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    if (ex is System.Reflection.ReflectionTypeLoadException)
                    {
                        ReflectionTypeLoadException rtl = ex as ReflectionTypeLoadException;
                        Exception[] excepts = rtl.LoaderExceptions;
                        foreach (Exception e in excepts)
                        {
                            _Log.Error("TypeLoad", e);
                        }
                    }
                }
            }
            return personCache[sysPeRole];
        }

        public static PeroleDto DeliverPeRole(DdOlExtended context, long sysPeRole)
        {
            if (!peroleDtoCache.ContainsKey(sysPeRole))
            {
               peroleDtoCache[sysPeRole] = context.ExecuteStoreQuery<PeroleDto>("select sysperole,sysroletype,sysperson,sysparent,name from perole where sysperole=" + sysPeRole, null).FirstOrDefault();
            }
            return peroleDtoCache[sysPeRole];
        }
        #endregion
    }
}