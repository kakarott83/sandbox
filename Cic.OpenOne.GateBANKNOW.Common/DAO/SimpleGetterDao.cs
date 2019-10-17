using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Data access object for simple getters
    /// </summary>
    public class SimpleGetterDao : ISimpleGetterDao
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SQL
        /// </summary>
        const String NATIVESQLQUERY = "SELECT PEROLE.SYSPEROLE FROM PEROLE, PERELATE,ROLETYPE WHERE PERELATE.SYSPEROLE1 = PEROLE.SYSPEROLE AND PEROLE.SYSROLETYPE = ROLETYPE.SYSROLETYPE and ROLETYPE.TYP=:roletype AND PERELATE.SYSPEROLE2 =:sysperole";
        const String PERSONBYPEROLE = "select person.* from person, perole where person.sysperson=perole.sysperson and perole.sysperole=:sysperole";
        const String PEROLEBYROLETYPE = "select perole.* from perole, roletype where (perole.validuntil is null or perole.validuntil>=:currentdate  or perole.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (perole.validfrom is null or perole.validfrom<=:currentdate  or perole.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and  perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=:sysperole) and roletype.typ=:roletype";
        const String INFOQUERY = "select ddlkppos.VALUE from     ddlkppos, peoption         where peoption.STR06 = ddlkppos.DOMAINID and ddlkppos.CODE = 'KOMMUNIKATIONSKANAL' and peoption.SYSID = :sysid";
        const String CONTRACTPROPOSAL = "select sysantrag, rw, rueckgabe from vt where sysid=:sysvt";

        /// <summary>
        /// get the associated person of the perole (Händlerrolle)
        /// </summary>
        /// <param name="sysperole">Händlerrolle</param>
        /// <returns>Person</returns>
        public virtual PERSON findPersonBySysperole(long sysperole)
        {
            PERSON person;

            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                _log.Debug("Created OlEntities context");
                var q1 = from role in olCtx.PEROLE where role.SYSPEROLE == sysperole select role.SYSPERSON;
                long? sysPerson = q1.FirstOrDefault();
                if (!sysPerson.HasValue) throw new ArgumentException("invalid sysperole " + sysperole);
                var query = from PERSON in olCtx.PERSON where PERSON.SYSPERSON == sysPerson select PERSON;
                person = query.Single();
                if (person != null)
                    olCtx.Detach(person);
            }
            return person;
        }

        /// <summary>
        /// get the associated puser of the person
        /// </summary>
        /// <param name="person">person</param>
        /// <returns>puser</returns>
        public virtual PUSER getPuser(PERSON person)
        {
            PUSER puser;
            if (person == null)
            {
                puser = null;
            }
            else
            {
                _log.Debug("Create OlEntities context");
                using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
                {
                    _log.Debug("Created OlEntities context");
                    var query = from PUSER in olCtx.PUSER where PUSER.SYSPUSER == person.SYSPUSER select PUSER;
                    puser = query.FirstOrDefault();
                    if (puser != null)
                        olCtx.Detach(puser);
                }
            }
            return puser;
        }

        /// <summary>
        /// get the kam person of the given perole (User-Role)
        /// </summary>
        /// <param name="sysPerole">User-Role</param>
        /// <returns>person</returns>
        public PERSON findKamPersonBySysperole(long sysPerole)
        {
            //Get from HAENDLER-Role
            long sysVpPerole = findRootPEROLEByRoleType(sysPerole, RoleTypeTyp.HAENDLER);
            if (sysVpPerole == 0)
            {
                _log.Error("No Händler found for User: " + sysPerole);
                throw new ApplicationException("No Händler found for User: " + sysPerole);
            }

            return findPersonByRoletypeAndSysperole((long)RoleTypeTyp.GEBIETSLEITER, sysVpPerole);
        }

        /// <summary>
        /// get the abwicklungsort person of the given perole (User-Role)
        /// </summary>
        /// <param name="sysPerole">User-Role</param>
        /// <returns>abwicklungsort person</returns>
        public PERSON findAbwicklungsortPersonBySysperole(long sysPerole)
        {
            //Get from HAENDLER-Role
            long sysVpPerole = findRootPEROLEByRoleType(sysPerole, RoleTypeTyp.HAENDLER);
            if (sysVpPerole == 0)
            {
                _log.Error("No Händler found for User: " + sysPerole);
                throw new ApplicationException("No Händler found for User: " + sysPerole);
            }

            return findPersonByRoletypeAndSysperole((long)RoleTypeTyp.GESCHAEFTSSTELLE, sysVpPerole);
        }

        /// <summary>
        /// findPersonByRoletypeAndSysperole
        /// </summary>
        /// <param name="roletype"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public virtual PERSON findPersonByRoletypeAndSysperole(long roletype, long sysperole)
        {
            PERSON person;

            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                _log.Debug("Created OlEntities context");

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "roletype", Value = roletype });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                long sysperelate2 = olCtx.ExecuteStoreQuery<long>(NATIVESQLQUERY, parameters.ToArray()).FirstOrDefault();
                if (sysperelate2 == 0)
                    throw new ApplicationException("SYSPEROLE " + sysperole + " NOT CONNECTED TO ROLETYPE " + roletype);

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperelate2 });
                person = olCtx.ExecuteStoreQuery<PERSON>(PERSONBYPEROLE, parameters.ToArray()).FirstOrDefault();

            }
            return person;
        }

        /// <summary>
        /// Get The information channel to communicate with this person
        /// </summary>
        /// <param name="sysid">PersonID</param>
        /// <returns>Channelstring</returns>
        public virtual string getinformationUeberBySysPerson(long sysid)
        {
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                _log.Debug("Created OlEntities context");

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });

                return olCtx.ExecuteStoreQuery<String>(INFOQUERY, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// finds the parent perole pkey for the given perole and roletype
        /// </summary>
        /// <param name="sysPEROLE"></param>
        /// <param name="pRoleType"></param>
        /// <returns></returns>
        public long findRootPEROLEByRoleType(long sysPEROLE, RoleTypeTyp pRoleType)
        {
            PEROLE retval = findRootPEROLEObjByRoleType(sysPEROLE, pRoleType);
            return retval.SYSPEROLE;
        }

        /// <summary>
        /// finds the parent perole for the given perole and roletype
        /// </summary>
        /// <param name="sysPEROLE"></param>
        /// <param name="pRoleType"></param>
        /// <returns></returns>
        public virtual PEROLE findRootPEROLEObjByRoleType(long sysPEROLE, RoleTypeTyp pRoleType)
        {
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysPEROLE });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "roletype", Value = pRoleType });

                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) });
                PEROLE retval = context.ExecuteStoreQuery<PEROLE>(PEROLEBYROLETYPE, parameters.ToArray()).FirstOrDefault<PEROLE>();

                if (retval == null)
                {
                    _log.Error("No " + pRoleType.ToString() + " found for User: " + sysPEROLE);
                    throw new ApplicationException("No " + pRoleType.ToString() + " found for User: " + sysPEROLE);
                }
                return retval;
            }
        }

    }
}