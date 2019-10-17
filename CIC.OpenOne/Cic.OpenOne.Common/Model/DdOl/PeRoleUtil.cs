using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.Common.Util.Collection;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF4.Model;
using System.Data.Objects;

namespace Cic.OpenOne.Common.Model.DdOl
{
    /// <summary>
    /// Perole Hilfsmethoden
    /// </summary>
    public class PeRoleUtil 
    {
        private static CacheDictionary<String, PEROLE> peroleCache = CacheFactory<String, PEROLE>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static CacheDictionary<String, bool> peroleCache2 = CacheFactory<String, bool>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Role), CacheCategory.Role);
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Methods
        /// <summary>
        /// FindRootPEROLEByRoleType
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="pRoleType"></param>
        /// <returns></returns>
        public static long FindRootPEROLEByRoleType(ObjectContext context, long sysPEROLE, long pRoleType)
        {
            //context.PEROLE.EnablePlanCaching = true;
            //context.PEROLE.MergeOption = MergeOption.NoTracking;

            PEROLE rval1 = FindRootPEROLEObjByRoleType(context, sysPEROLE, pRoleType);
			if(rval1==null) 
				throw new Exception("User with Role "+sysPEROLE+" is not in RoleType "+pRoleType);
            return rval1.SYSPEROLE;
        }

        /// <summary>
        /// FindRootPEROLEObjByRoleType
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="pRoleType"></param>
        /// <returns></returns>
        public static PEROLE FindRootPEROLEObjByRoleType(ObjectContext context, long sysPEROLE, long pRoleType)
        {
            String key = sysPEROLE + "_" + pRoleType;

            if (!peroleCache.ContainsKey(key))
            {
                //context.PEROLE.EnablePlanCaching = true;
                //context.PEROLE.MergeOption = MergeOption.NoTracking;

                string query = "select perole.* from perole, roletype where " +
                               "  (perole.validuntil is null or perole.validuntil >= :currentdate or  perole.validuntil<=to_date('01.01.0111' , 'dd.MM.yyyy')) and " +
                               " (perole.validfrom is null or perole.validfrom <= :currentdate or  perole.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy')) and  perole.sysroletype = roletype.sysroletype and " +
                               " perole.sysperole in " +
                               " (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole = :sysperole) and roletype.typ = :roletype ";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysPEROLE });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "roletype", Value = pRoleType });

                // TODO: perDate useKONTEXT ?
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDateNoTime(null) });

                peroleCache[key] = context.ExecuteStoreQuery<PEROLE>(query, parameters.ToArray()).FirstOrDefault<PEROLE>();
            }
            return peroleCache[key];
        }
		
		/// <summary>
        /// FindRootPEROLEObjByRoleType 
        /// uses the roletype.code to determine the parent role
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public static PEROLE FindRootPEROLEObjByRoleCode(DdOlExtended context, long sysPEROLE, String roleCode)
        {
            String key = sysPEROLE + "_" + roleCode;

            if (!peroleCache.ContainsKey(key))
            {
                
                string query = @"select perole.* from perole, roletype where 
                                 (perole.validuntil is null or perole.validuntil >= :currentdate or  perole.validuntil<=to_date('01.01.0111' , 'dd.MM.yyyy') ) and 
                                (perole.validfrom is null or perole.validfrom <= :currentdate or  perole.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy')) and  perole.sysroletype = roletype.sysroletype and 
                                perole.sysperole in 
                                (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole = :sysperole) and roletype.code = :rolecode ";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysPEROLE });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "rolecode", Value = roleCode });

                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDateNoTime(null) });

                peroleCache[key] = context.ExecuteStoreQuery<PEROLE>(query, parameters.ToArray()).FirstOrDefault<PEROLE>();
            }
            return peroleCache[key];
        }

        /// <summary>
        /// Returns false if one of the peroles of the given perole branch to the root has inactiveflag>0
        /// </summary>
        /// <param name="sysPEROLE"></param>
        /// <returns></returns>
        public static bool isActive(long sysPEROLE)
        {
            //String key = sysPEROLE.ToString();

           // if (!peroleCache2.ContainsKey(key))
            {
                using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended context = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
                {
                    string query = "select sum(inactiveflag) inaktiv from perole where  (perole.validuntil is null or perole.validuntil >= :currentdate or  perole.validuntil<=to_date('01.01.0111' , 'dd.MM.yyyy') ) and  (perole.validfrom is null or perole.validfrom <=:currentdate or  perole.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy') )  and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole = :sysperole)";

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysPEROLE });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDateNoTime(null) });

                    long inaktiv = context.ExecuteStoreQuery<long>(query, parameters.ToArray()).FirstOrDefault<long>();
                    bool rval = (inaktiv == 0);
                    if(!rval)
                        _log.Info("Perole " + sysPEROLE + " has " + inaktiv + " inactive connected roles");
                    return rval;
                    //peroleCache2[key] = (inaktiv == 0);
                }
            }
           // return peroleCache2[key];
        }
        /// <summary>
        /// Removes the isActive-Cache
        /// </summary>
        /// <param name="sysPEROLE"></param>
        public static void clearisActiveCache(long sysPEROLE)
        {
            String key = sysPEROLE.ToString();
            peroleCache2.Remove(key);
        }
        #endregion
    }
}