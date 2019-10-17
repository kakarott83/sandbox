using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Object Type Data Access Object
    /// </summary>
    public class ObTypDao : IObTypDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string QUERYOBJTYPDATA = "SELECT * FROM obtyp o where o.sysobtyp = :sysid";

        private const string QUERYDESCENDANTS = "SELECT SYSOBTYP FROM obtyp o START WITH o.sysobtyp = :obtyp  CONNECT BY PRIOR o.sysobtyp = o.sysobtypp";
        private const string QUERYASCENDANTS = "SELECT SYSOBTYP FROM obtyp o START WITH o.sysobtyp = :obtyp  CONNECT BY PRIOR o.sysobtypp = o.sysobtyp";

        private string QUERYGROUPUNASSIGNED = "select prhgroupm.sysprhgroup from prhgroupm,prhgroup "
            + " where prhgroup.sysprhgroup=prhgroupm.sysprhgroup and prhgroupm.activeflag=1 and prhgroup.activeflag=1 and sysperole=:sysperole and "
            //+ " (prhgroupm.validuntil is null or prhgroupm.validuntil>=  :perDate  or prhgroupm.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) "
            //+ " and "
            //+ " (prhgroupm.validfrom is null or prhgroupm.validfrom<=  :perDate  or prhgroupm.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) ";
            + SQL.CheckDate (" :perDate ", "prhgroupm");

        private string QUERYGROUPASSIGNED = "SELECT m.sysprhgroup, mob.sysobtyp FROM prhgroupmob mob, prhgroupm m, prhgroup g "
            + " WHERE m.sysprhgroupm = mob.sysprhgroupm AND m.sysprhgroup = g.sysprhgroup AND m.sysperole = :sysperole AND m.activeflag = 1 AND g.activeflag = 1 AND excludeflag = :exclude  and "
            //+ " (m.validuntil is null or m.validuntil>=  :perDate  or m.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) "
            //+ " and "
            //+ " (m.validfrom is null or m.validfrom<=  :perDate  or m.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy'))";
            + SQL.CheckDate (" :perDate ", "m");


        string QUERYSCHWACKEOBJTYPDATA = string.Format("SELECT * FROM obtyp o where o.schwacke = :nationalVC");
        private static CacheDictionary<long, List<long>> obTypAscCache = CacheFactory<long, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, List<long>> obTypDescCache = CacheFactory<long, List<long>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        /// <summary>
        /// Constructor
        /// </summary>
        public ObTypDao()
        {
        }
        /// <summary>
        /// Prhgroups for the role and obtyp
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public virtual List<long> getPrhGroups(long sysperole, long sysobtyp, DateTime perDate)
        {
            using (OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                //List<long> retval;   
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                List<long> unassigned = ctx.ExecuteStoreQuery<long>(QUERYGROUPUNASSIGNED, parameters.ToArray()).ToList();
                List<long> free = new List<long>();
                foreach (long unass in unassigned)
                {
                    free.Add(unass);
                }
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "exclude", Value = 0 });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                List<GroupResultsDto> inRes = ctx.ExecuteStoreQuery<GroupResultsDto>(QUERYGROUPASSIGNED, parameters.ToArray()).ToList();
                bool bDedicated = inRes.Count > 0 ? true : false;
                List<long> dedicated = new List<long>();
                List<long> exclusive = new List<long>();
                //if(_log.IsDebugEnabled)
                //{
                //    string DedicatedList = "Liste der Dedicated Items: ";
                //    foreach (long Item in dedicated)
                //    {
                //        DedicatedList += " " + Item.ToString();
                //    }
                //    _log.Debug(DedicatedList);
                //}
                foreach (GroupResultsDto result in inRes)
                {
                    List<long> ObtypList = getObTypDescendants(result.sysobtyp);
                    if (ObtypList.Contains(sysobtyp) == true)
                    {
                        dedicated.Add(result.sysprhgroup);
                        _log.Debug("Dedizierung akzeptiert: " + result.sysprhgroup.ToString());
                    }
                    else
                    {
                        exclusive.Add(result.sysprhgroup);
                    }
                    free.Remove(result.sysprhgroup);
                }

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "exclude", Value = 1 });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                List<GroupResultsDto> exRes = ctx.ExecuteStoreQuery<GroupResultsDto>(QUERYGROUPASSIGNED, parameters.ToArray()).ToList();
                bool bExclusive = exRes.Count > 0 ? true : false;
                //if (_log.IsDebugEnabled)
                //{
                //    string ExcludedList = "Liste der Excluded Items: ";
                //    foreach (long Item in exclusive)
                //    {
                //        ExcludedList += " " + Item.ToString();
                //    }
                //    _log.Debug(ExcludedList);
                //}
                foreach (GroupResultsDto result in exRes)
                {
                    List<long> ObtypList = getObTypDescendants(result.sysobtyp);
                    if (ObtypList.Contains(sysobtyp) == true)
                    {
                        exclusive.Add(result.sysprhgroup);
                        _log.Debug("Exclusion gefunden: " + result.sysprhgroup.ToString());
                    }
                    else
                    {
                        dedicated.Add(result.sysprhgroup);
                    }
                    free.Remove(result.sysprhgroup);
                }

                //return (bDedicated || bExclusive) ? Compile(unassigned, dedicated, exclusive, free) : unassigned;
                return unassigned.Except(exclusive).Union(dedicated).Union(free).Distinct().ToList();
            }
        }
        /// <summary>
        /// returns all items beneath the given obtyp (all paths to the leaves)
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public List<long> getObTypDescendants(long sysobtyp)
        {
            if (!obTypDescCache.ContainsKey(sysobtyp))
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obtyp", Value = sysobtyp });
                    obTypDescCache[sysobtyp] =  ctx.ExecuteStoreQuery<long>(QUERYDESCENDANTS, parameters.ToArray()).ToList();
                }
            }
            return obTypDescCache[sysobtyp];
        }

        /// <summary>
        /// returns the path to the root for the given obtyp
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public List<long> getObTypAscendants(long sysobtyp)
        {
            if (!obTypAscCache.ContainsKey(sysobtyp))
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obtyp", Value = sysobtyp });
                    obTypAscCache[sysobtyp] =  ctx.ExecuteStoreQuery<long>(QUERYASCENDANTS, parameters.ToArray()).ToList();
                }
            }
            return obTypAscCache[sysobtyp];
        }

      
    }

    /// <summary>
    /// Gruppenergebnisse DTO
    /// </summary>
    public class GroupResultsDto
    {
        /// <summary>
        /// Handelsgruppend ID
        /// </summary>
        public long sysprhgroup { get; set; }
        /// <summary>
        /// Objektyp ID
        /// </summary>
        public long sysobtyp { get; set; }
    }
}
