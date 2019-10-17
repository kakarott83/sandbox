using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Object Type Data Access Object
    /// </summary>
    public class ObTypDao : IObTypDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string QUERYOBJTYPDATA = "SELECT schwacke FROM obtyp o where o.sysobtyp = :sysobtyp";

        private const string QUERYDESCENDANTS = "SELECT SYSOBTYP FROM obtyp o START WITH o.sysobtyp = :obtyp  CONNECT BY PRIOR o.sysobtyp = o.sysobtypp";
        private const string QUERYASCENDANTS = "SELECT SYSOBTYP FROM obtyp o START WITH o.sysobtyp = :obtyp  CONNECT BY PRIOR o.sysobtypp = o.sysobtyp";

        private const string QUERYGROUPUNASSIGNED = "select prhgroupm.sysprhgroup from prhgroupm,prhgroup where prhgroup.sysprhgroup=prhgroupm.sysprhgroup and prhgroupm.activeflag=1 and prhgroup.activeflag=1 and sysperole=:sysperole and (prhgroupm.validuntil is null or prhgroupm.validuntil>=  :perDate  or prhgroupm.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (prhgroupm.validfrom is null or prhgroupm.validfrom<=  :perDate  or prhgroupm.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) ";
        private const string QUERYGROUPASSIGNED = "SELECT m.sysprhgroup, mob.sysobtyp FROM prhgroupmob mob, prhgroupm m, prhgroup g WHERE m.sysprhgroupm = mob.sysprhgroupm AND m.sysprhgroup = g.sysprhgroup AND m.sysperole = :sysperole AND m.activeflag = 1 AND g.activeflag = 1 AND excludeflag = :exclude  and (m.validuntil is null or m.validuntil>=  :perDate  or m.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (m.validfrom is null or m.validfrom<=  :perDate  or m.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy'))";

        private const string QUERYSCHWACKEOBJTYPDATA = "SELECT sysobtyp FROM obtyp o where o.schwacke = :nationalVC";
        private const string QUERYPERSONID = "select person.sysperson from perole, person where perole.sysperson=person.sysperson and perole.sysperole=:sysperole";

        /// <summary>
        /// Constructor
        /// </summary>
        public ObTypDao()
        {
        }

        /// <summary>
        /// returns all items beneath the given obtyp (all paths to the leaves)
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public virtual List<long> getObTypDescendants(long sysobtyp)
        {
            using (OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obtyp", Value = sysobtyp });
                return ctx.ExecuteStoreQuery<long>(QUERYDESCENDANTS, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// returns the path to the root for the given obtyp
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public virtual List<long> getObTypAscendants(long sysobtyp)
        {
            using (OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obtyp", Value = sysobtyp });
                return ctx.ExecuteStoreQuery<long>(QUERYASCENDANTS, parameters.ToArray()).ToList();
            }
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
                List<long> unassigned =  ctx.ExecuteStoreQuery<long>(QUERYGROUPUNASSIGNED, parameters.ToArray()).ToList();
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
        /// Prhgroups for the role
        /// </summary>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Handelsgruppenliste</returns>
        public virtual List<long> getPrhGroupsbyPerole(long sysperole)
        {
            using (OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                DateTime perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                return ctx.ExecuteStoreQuery<long>(QUERYGROUPUNASSIGNED, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// Schlüssel des Händlers aus dem des Angestellten ermittlen.
        /// </summary>
        /// <param name="sysperole">Angestellten Schlüssel</param>
        /// <returns>Händlerschlüssel</returns>
        public virtual long getHaendlerByEmployee(long sysperole)
        {
            using (OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                return PeRoleUtil.FindRootPEROLEByRoleType(ctx, sysperole,(int) RoleTypeTyp.HAENDLER);
            }
        }

        /// <summary>
        /// Schlüssel der PersonID aus PEROLE ermittlen.
        /// </summary>
        /// <param name="sysperole">Perole</param>
        /// <returns>PersonID</returns>
        public virtual long getPersonIDByPEROLE(long sysperole)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                return context.ExecuteStoreQuery<long>(QUERYPERSONID, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        ///  Person aus PEROLE ermittlen.
        /// </summary>
        /// <param name="sysperole">Perole</param>
        /// <returns>PersonID</returns>
        public virtual PERSON getPersonByPEROLE(long sysperole)
        {
            using (OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
            {
         
                PERSON pers = PERSONHelper.getPERSONByPeRole(ctx, sysperole);
                if (pers != null)
                {
                    return pers;
                }
                return null;
            }
        }

        /// <summary>
        /// Ermitteln der Objekttypdaten eines Fahrzeugs
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <returns>Daten</returns>
        public ObtypDataRestwertDto getObTypData(long sysobtyp)
        {
            ObtypDataRestwertDto retval = new ObtypDataRestwertDto();
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });

                String schwacke = ctx.ExecuteStoreQuery<String>(QUERYOBJTYPDATA, parameters.ToArray()).FirstOrDefault();
                if (schwacke != null)
                {
                    retval.sysobtyp = sysobtyp;
                    retval.Schwacke = Convert.ToInt64(schwacke);
                }
              
                return retval;
            }
        }

        /// <summary>
        /// Ermitteln der Objekttypdaten eines Fahrzeugs nach Nationalem Fahrzeugcode
        /// </summary>
        /// <param name="nationalVC">Nationaler Fahrzeugcode</param>
        /// <returns>Daten</returns>
        public ObtypDataRestwertDto getObTypDataByNVC(long nationalVC)
        {
            ObtypDataRestwertDto retval = new ObtypDataRestwertDto();
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "nationalVC", Value = nationalVC });
                retval.sysobtyp = ctx.ExecuteStoreQuery<long>(QUERYSCHWACKEOBJTYPDATA, parameters.ToArray()).FirstOrDefault();
                if (retval.sysobtyp>0)
                {
                    retval.Schwacke = nationalVC;
                }

                return retval;
            }
        }

        /// <summary>
        /// Ermitteln der Objekttypdaten eines Fahrzeugs nach Nationalem Fahrzeugcode
        /// </summary>
        /// <param name="nationalVC">Nationaler Fahrzeugcode</param>
        /// <returns>Daten</returns>
        public ObtypDataRestwertDto getObTypDataByNVCByString(string nationalVC)
        {
            ObtypDataRestwertDto retval = new ObtypDataRestwertDto();
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "nationalVC", Value = nationalVC });
                retval.sysobtyp = ctx.ExecuteStoreQuery<long>(QUERYSCHWACKEOBJTYPDATA, parameters.ToArray()).FirstOrDefault();
                return retval;
            }
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
