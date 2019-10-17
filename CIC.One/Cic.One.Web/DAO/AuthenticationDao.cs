using Cic.One.DTO;
using Cic.One.Web.DTO;
using Cic.OpenOne.Common.Model.DdCt;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using CIC.Database.OL.EF4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cic.One.Web.DAO
{
    /// <summary>
    /// DAO for user authentication and logon infos
    /// </summary>
    public class AuthenticationDao : IAuthenticationDao
    {
        private const String GETISOCODE = "select ctlang.isocode value, ctlang.sysctlang id from person,ctlang where ctlang.sysctlang=person.sysctlang and person.sysperson=:sysperson";
        private const String GETISOCODES = "select ctlang.isocode value, ctlang.sysctlang id from ctlang where flagtranslate=1";
        private const String QUERYPW = "select pw from cic.rpw,cic.radd,cic.vausradd where vausradd.sysvausradd=rpw.syswfuser and radd.sysradd=syswfuser and rpw.syswfuser=:syswfuser and (expiredate+(select nvl(gracedays,0) from wfsys)>sysdate or expiredate is null)  order by sysrpw desc";//and (vausradd.disabled is null or vausradd.disabled=0)

        private const String QUERYEXPDATE = "select expiredate from cic.rpw,cic.radd,cic.vausradd where vausradd.sysvausradd=rpw.syswfuser and radd.sysradd=syswfuser and rpw.syswfuser=:syswfuser  order by sysrpw desc";
        private const String QUERYDISABLED = "select disabled from cic.rpw,cic.radd,cic.vausradd where vausradd.sysvausradd=rpw.syswfuser and radd.sysradd=syswfuser and rpw.syswfuser=:syswfuser  order by sysrpw desc";

        private const String QUERYPUSER = "select syspuser from cic.puser where syswfuser=:syswfuser";
        private const String QUERYPUSERID = "select externeid from cic.puser where syswfuser=:syswfuser";

        private const String QUERYWFUSER = "select case when(ctlang.isocode is not null) then ctlang.isocode else ctlang2.isocode end language,wfuser.*, person.strasse pstrasse,person.hsnr phsnr,person.ort port,person.plz pplz,person.telefon ptelefon from wfuser,person,ctlang,ctlang ctlang2 where wfuser.sysperson=person.sysperson(+) and wfuser.sysctlang=ctlang.sysctlang(+) and person.sysctlang=ctlang2.sysctlang(+) and wfuser.syswfuser=:syswfuser";
        private const String QUERYWFUSERCODE = "select ctlang.isocode language,wfuser.*, person.strasse pstrasse,person.hsnr phsnr,person.ort port,person.plz pplz,person.telefon ptelefon from wfuser,person,ctlang where wfuser.sysperson=person.sysperson(+) and wfuser.sysctlang=ctlang.sysctlang(+) and upper(wfuser.code)=:code";
        private const String QUERYWFUSERPR = "select wfuser.*, person.strasse pstrasse,person.hsnr phsnr,person.ort port,person.plz pplz,person.telefon ptelefon, pr.sysperole,roletype.typ from wfuser, perole pr,roletype,person where roletype.sysroletype=pr.sysroletype and wfuser.sysperson=person.sysperson and pr.sysperson=person.sysperson and pr.sysperole in (select sysperole from perole connect by prior perole.sysperole = perole.sysparent start with perole.sysperole=:sysperole  ) order by pr.name";
        private const String QUERYDEFAULTPEROLE = "select sysdefaultperole from puser where  externeid=:extid";
        private const String QUERYPWBYEXTID = "select kennwort from puser where  externeid=:extid";
        private const String QUERYSYSPERSONPEROLE = "select sysperson from perole where sysperole=:p";
        private const String QUERYSYSPERSONPUSER = "select sysperson from person where syspuser=:p";
        private const String QUERYROLETYPETYP = "select roletype.typ from perole,roletype where perole.sysroletype=roletype.sysroletype and perole.sysperole=:sysperole";
        private const String QUERYSSOUSER = "select code from wfuser,wfsso where wfsso.syswfuser=wfuser.syswfuser and UPPER(wfsso.benutzer) like UPPER(:uname) ";
        private const String QUERYBILDWELTEN =
                                        "SELECT DEFAULTFLAG,  NAME,  prbildwelt.SYSPRBILDWELT,prbildwelt.RESSOURCE " +
                                        " FROM prbildweltM,  prbildwelt " +
                                        " WHERE prbildweltM.sysprbildwelt = prbildwelt.sysprbildwelt AND prbildweltM.sysPerole = :haendlerSysPerole" +
                                            " AND prbildweltM.sysperole  > 0 AND prbildweltM.activeflag   = 1 AND prbildwelt.activeflag    = 1 " +
                                            " AND ( prbildweltM.validfrom  IS NULL " +
                                                    " OR prbildweltM.validfrom    <= :perDate " +
                                                    " OR prbildweltM.validfrom     = to_date('01.01.0111' , 'dd.MM.yyyy') ) " +
                                            " AND ( prbildweltM.validuntil IS NULL " +
                                                    " OR prbildweltM.validuntil   >= :perDate " +
                                                    " OR prbildweltM.validuntil    = to_date('01.01.0111' , 'dd.MM.yyyy')) " +
                                        " ORDER BY prbildweltM.rank ";
        private const String QUERYDEFAULTBILDWELT = "select sysprbildwelt,ressource from prbildwelt where activeflag=1 and defaultflag=1";
        private const String QUERYCHANNELS = @"select bchannel.sysbchannel from prchannelm,bchannel,perole,roletype where bchannel.sysbchannel=prchannelm.sysbchannel and prchannelm.activeflag=1 
                and perole.SYSROLETYPE=roletype.sysroletype and roletype.typ=6 and perole.sysperole=prchannelm.sysperole and perole.sysperole=:sysvpperole
                and (prchannelm.validfrom is null or prchannelm.validfrom<=sysdate or prchannelm.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy'))
                and (prchannelm.validuntil is null or prchannelm.validuntil>=sysdate or prchannelm.validuntil<=to_date('01.01.0111' , 'dd.MM.yyyy'))";
        private const String PEROLEBYROLETYPE = "select perole.* from perole, roletype where (perole.validuntil is null or perole.validuntil>=:currentdate  or perole.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (perole.validfrom is null or perole.validfrom<=:currentdate  or perole.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and  perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=:sysperole) and roletype.typ=:roletype";
        private const String NATIVESQLQUERY = "SELECT PEROLE.SYSPEROLE FROM PEROLE, PERELATE,ROLETYPE WHERE PERELATE.SYSPEROLE1 = PEROLE.SYSPEROLE AND PEROLE.SYSROLETYPE = ROLETYPE.SYSROLETYPE and ROLETYPE.TYP=:roletype AND PERELATE.SYSPEROLE2 =:sysperole";
        private const String PERSONBYPEROLE = "select person.* from person, perole where person.sysperson=perole.sysperson and perole.sysperole=:sysperole";
        private const String QUERYEPOSADMIN = "select sysrgm from perole p, RGR, RGM,  wfuser where rgr.sysrgr = rgm.sysrgr and rgr.name = 'EPOS_ADMIN' and rgm.syswfuser = wfuser.syswfuser and wfuser.sysperson =p.sysperson and p.sysperole=:sysperole";
        private const String QUERYABWICKLUNGSORTWFUSER = @"SELECT *
                                        FROM (SELECT peabwo.sysperole sysabwicklung
                                        FROM perelate, perole pevm, perole peabwo, wfuser antragsowner
                                        WHERE pevm.sysperson = antragsowner.sysperson
                                        AND perelate.sysperole2 = pevm.sysperole
                                        AND perelate.sysperole1 = peabwo.sysperole
                                        and antragsowner.syswfuser=:syswfuser
                                        AND pevm.sysperson > 0 AND (perelate.relbeginndate IS NULL
                                        OR perelate.relbeginndate <= to_date('01.01.0111' , 'dd.MM.yyyy')
                                        OR perelate.relbeginndate <= sysdate)
                                        AND (perelate.relenddate IS NULL
                                        OR perelate.relenddate <= to_date('01.01.0111' , 'dd.MM.yyyy')
                                        OR perelate.relenddate >= sysdate)
                                        AND peabwo.sysroletype = 11 
                                        ORDER BY NVL(perelate.flagdefault, 0) DESC, perelate.sysperelate DESC)
                                        ";
        private const String QUERY_EPOSRAUM = @"select count(*)
                                        from wftzust, wftzvar, perole, wfzust
                                        where perole.sysperson = wftzust.syslease
                                        and wftzust.syswftable = 2
                                        and wfzust.syscode = 'EPOS_BEDINGUNGEN'
                                        and wfzust.syswfzust = wftzust.syswfzust
                                        and wftzvar.syswftzust = wftzust.syswftzust
                                        and wftzust.status = 0
                                        and perole.sysperole = :sysperole
                                        order by decode (wftzvar.code, 'BESTAETIGT',1, 'BESTAETIGT_AM',2,3)";
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime nullDate = new DateTime(1800, 1, 1);

        public AuthenticationDao()
        {

        }

        /// <summary>
        /// Returns all parent workflow users for the perole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public WfuserDto[] getWfusers(long sysperole)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                long parentRole = ctx.ExecuteStoreQuery<long>("select sysparent from perole where sysperole=" + sysperole, null).FirstOrDefault();

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = parentRole });

                return ctx.ExecuteStoreQuery<WfuserDto>(QUERYWFUSERPR, parameters.ToArray()).ToArray();

            }
        }

        /// <summary>
        /// finds a certain wfuser
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public WfuserDto getWfuserDto(igetWfuserDto input)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                if (input.syswfuser > 0)
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = input.syswfuser });
                    return ctx.ExecuteStoreQuery<WfuserDto>(QUERYWFUSER, parameters.ToArray()).FirstOrDefault();
                }
                String qprefix = @"
SELECT person.SYSCTLANG SYSCTLANGKORR,wfuser.*,
  person.strasse pstrasse,
  person.hsnr phsnr,
  person.ort port,
  person.plz pplz,
  person.telefon ptelefon,
  pr.sysperole,
  roletype.typ
FROM wfuser,
  perole pr,
  roletype,
  person
WHERE roletype.sysroletype=pr.sysroletype
AND wfuser.sysperson      =person.sysperson
AND pr.sysperson          =person.sysperson";


                if (input.roletypetyp > 0)
                {
                    qprefix += " and roletype.typ=" + input.roletypetyp;
                }
                if (input.sysroletype > 0)
                {
                    qprefix += " and roletype.sysroletype=" + input.sysroletype;
                }
                if (input.sysperson > 0)
                {
                    qprefix += @"
AND pr.sysperole         IN
  (SELECT sysperole
  FROM perole
    CONNECT BY prior perole.sysparent = perole.sysperole
    start with perole.sysperson=" + input.sysperson + ")";
                }
                else if (input.sysperole > 0)
                {
                    qprefix += @"
AND pr.sysperole         IN
  (SELECT sysperole
  FROM perole
    CONNECT BY prior perole.sysparent = perole.sysperole
    start with perole.sysperole=" + input.sysperole + ")";
                }
                return ctx.ExecuteStoreQuery<WfuserDto>(qprefix, null).FirstOrDefault();
            }
        }

        /// <summary>
        /// gets the wfuser password
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public String getWfuserPassword(long syswfuser)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });

                return ctx.ExecuteStoreQuery<String>(QUERYPW, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// gets the wfuser code from the wfsso table by looking for the kerberos userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public String getSSOWfuser(String userName)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String uPar = userName;
                uPar = ASCIIEncoding.ASCII.GetString(ASCIIEncoding.ASCII.GetBytes(userName));
                
                if (uPar.IndexOf("@") > -1)
                    uPar = uPar.Substring(0, uPar.IndexOf("@"));
                
                /*
                byte[] crs = ASCIIEncoding.ASCII.GetBytes(userName);
                _log.Debug("GOT (ASCII): " + userName);
                for (int i = 0; i < crs.Length; i++)
                    _log.Debug(""+crs[i]);

                crs = ASCIIEncoding.UTF8.GetBytes(userName);
                _log.Debug("GOT (UTF8): " + userName);
                for (int i = 0; i < crs.Length; i++)
                    _log.Debug("" + crs[i]);


                crs = ASCIIEncoding.UTF32.GetBytes(userName);
                _log.Debug("GOT (UTF32): " + userName);
                for (int i = 0; i < crs.Length; i++)
                    _log.Debug("" + crs[i]);

                crs = ASCIIEncoding.UTF32.GetBytes(userName);
                _log.Debug("GOT (UTF32): " + userName);
                for (int i = 0; i < crs.Length; i++)
                    _log.Debug("" + crs[i]);*/

                uPar = uPar.Replace("?", "%");
                _log.Debug("Searching SSO USER " + uPar);
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "uname", Value = uPar });

                return ctx.ExecuteStoreQuery<String>(QUERYSSOUSER, parameters.ToArray()).FirstOrDefault();

              


            }
        }

        /// <summary>
        /// returns the workflow user
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public WfuserDto getWfuser(long syswfuser)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                WfuserDto rval =  ctx.ExecuteStoreQuery<WfuserDto>(QUERYWFUSER, parameters.ToArray()).FirstOrDefault();

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                rval.expireDate = ctx.ExecuteStoreQuery<DateTime>(QUERYEXPDATE, parameters.ToArray()).FirstOrDefault();

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                rval.disabled = ctx.ExecuteStoreQuery<int>(QUERYDISABLED, parameters.ToArray()).FirstOrDefault();
                
                return rval;
            }
        }

        /// <summary>
        /// returns the workflow user by code
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public WfuserDto getWfuserByCode(String code)
        {
            if (code == null) return null;
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code.ToUpper() });
                return ctx.ExecuteStoreQuery<WfuserDto>(QUERYWFUSERCODE, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// returns the puser SYSPUSER for the wfuser
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public long getPUser(long syswfuser)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                return ctx.ExecuteStoreQuery<long>(QUERYPUSER, parameters.ToArray()).FirstOrDefault();
            }
        }
        /// <summary>
        /// returns the puser EXTERNEID for the wfuser
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public String getPUserId(long syswfuser)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                return ctx.ExecuteStoreQuery<String>(QUERYPUSERID, parameters.ToArray()).FirstOrDefault();
            }
        }
        /// <summary>
        /// returns the pusers defaultperole
        /// </summary>
        /// <param name="extid"></param>
        /// <returns></returns>
        public long getDefaultperole(String extid)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "extid", Value = extid });
                return ctx.ExecuteStoreQuery<long>(QUERYDEFAULTPEROLE, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// get the puser password by extid
        /// </summary>
        /// <param name="extid"></param>
        /// <returns></returns>
        public String getPasswordByExtId(String extid)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "extid", Value = extid });
                return ctx.ExecuteStoreQuery<String>(QUERYPWBYEXTID, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// gets the sysperson by the given value
        /// </summary>
        /// <param name="value">syspuser or sysperole</param>
        /// <param name="sysperole">when false value is syspuser else sysperole</param>
        /// <returns></returns>
        public long getSysPerson(long value, bool sysperole)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String query = QUERYSYSPERSONPUSER;
                if (sysperole)
                    query = QUERYSYSPERSONPEROLE;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p", Value = value });
                return ctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// gets the roletype typ for the perole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public long getRoletypeTypByPerole(long sysperole)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                return ctx.ExecuteStoreQuery<long>(QUERYROLETYPETYP, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// gets the isocode fro the sysperson
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public AttributeValue getIsoCode(long sysperson)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> attparameters = new List<Devart.Data.Oracle.OracleParameter>();
                attparameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysperson });
                return ctx.ExecuteStoreQuery<AttributeValue>(GETISOCODE, attparameters.ToArray()).FirstOrDefault();
            }

        }

        /// <summary>
        /// gets all available Isocodes
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public List<AttributeValue> getAllIsocodes()
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {

                return ctx.ExecuteStoreQuery<AttributeValue>(GETISOCODES, null).ToList();
            }

        }

        /// <summary>
        /// returns the bildwelt for the haendler
        /// </summary>
        /// <param name="haendlerSysPerole"></param>
        /// <returns></returns>
        public BildweltInfoDto getBildwelten(long haendlerSysPerole)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                // Bildwelten -----------
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) },
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "haendlerSysPerole", Value = haendlerSysPerole } };
                // Ein Händler kann maximal einer Bildwelt zugeordnet sein
                return ctx.ExecuteStoreQuery<BildweltInfoDto>(QUERYBILDWELTEN, pars).FirstOrDefault();
            }
        }

        /// <summary>
        /// return the default bildwelt
        /// </summary>
        /// <returns></returns>
        public BildweltInfoDto getDefaultBildwelt()
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                
                // Ein Händler kann maximal einer Bildwelt zugeordnet sein
                return ctx.ExecuteStoreQuery<BildweltInfoDto>(QUERYDEFAULTBILDWELT, null).FirstOrDefault();
            }
        }
        /// <summary>
        /// determine Brand attributes
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public AttributeMapDto getBrandAttributes(long sysperole)
        {
            AttributeMapDto rval = new AttributeMapDto();
            rval.attributeName = AttributeName.BRAND;
            rval.attributeValues = new List<AttributeValue>();
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                long sysVpPerole = PeRoleUtil.FindRootPEROLEByRoleType(olCtx, sysperole, (long)RoleTypeTyp.HAENDLER);
                DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);

                var query = from BRAND in olCtx.BRAND // Selektiere alle BRANDs
                            join PRBRANDM in olCtx.PRBRANDM on BRAND.SYSBRAND equals PRBRANDM.BRAND.SYSBRAND // aus der Liste der Brands al ler
                            join PRHGROUPM in olCtx.PRHGROUPM on PRBRANDM.PRHGROUP.SYSPRHGROUP equals PRHGROUPM.PRHGROUP.SYSPRHGROUP // Handelsgruppen des Verkäufers
                            join PEROLEVP in olCtx.PEROLE on PRHGROUPM.PEROLE.SYSPEROLE equals PEROLEVP.SYSPEROLE // Verkäuferrolle
                            where PEROLEVP.ROLETYPE.TYP == (int)RoleTypeTyp.HAENDLER  // Einschränkung für Verkäuferrolle
                            && PEROLEVP.SYSPEROLE == sysVpPerole // Konkreter Verkäufer
                            && PRHGROUPM.ACTIVEFLAG == 1
                            && (PRHGROUPM.VALIDFROM == null || PRHGROUPM.VALIDFROM <= aktuell || PRHGROUPM.VALIDFROM <= nullDate)
                            && (PRHGROUPM.VALIDUNTIL == null || PRHGROUPM.VALIDUNTIL >= aktuell || PRHGROUPM.VALIDUNTIL <= nullDate)
                            orderby PRHGROUPM.DEFAULTFLAG descending, BRAND.SYSBRAND
                            select BRAND;

                
                foreach (BRAND item in query)
                {
                    if (item.ACTIVEFLAG == 1)
                    {
                        AttributeValue av = new AttributeValue();
                        av.value = item.NAME;
                        av.id = ""+item.SYSBRAND;
                        rval.attributeValues.Add(av);
                        if (rval.defaultValue == null)
                            rval.defaultValue = ""+item.SYSBRAND;
                    }
                }
                
               
            }
            return rval;
        }
        
        /// <summary>
        /// determine EPOS_ADMIN rgr 
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public AttributeMapDto getEposAdminAttribute(long sysperole)
        {


            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
            {
                object[] pars = { 
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole } };

                long isadmin = ctx.ExecuteStoreQuery<long>(QUERYEPOSADMIN,pars).FirstOrDefault();
                if (isadmin == 0) return null;
                AttributeMapDto rval = new AttributeMapDto();
                rval.attributeName = AttributeName.EPOS_ADMIN;
                rval.defaultValue = ""+isadmin;
                return rval;
            }
        
            
        }


        /// <summary>
        /// determine ABWICKLUNGSORT Attribute
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public AttributeMapDto getAbwicklungsortAttribute(long syswfuser)
        {


            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
            {
                object[] pars = { 
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser } };

                List<long> sysabwicklungen = ctx.ExecuteStoreQuery<long>(QUERYABWICKLUNGSORTWFUSER, pars).ToList();
                if (sysabwicklungen == null || sysabwicklungen.Count == 0) return null;
                AttributeMapDto rval = new AttributeMapDto();
                rval.attributeName = AttributeName.ABWICKLUNGSORT;
                rval.defaultValue = "" + sysabwicklungen[0];
                rval.attributeValues = new List<AttributeValue>();
                foreach(long sysabw in sysabwicklungen)
                {
                    AttributeValue av = new AttributeValue();
                    av.id = ""+sysabw;
                    av.value = ""+sysabw;
                    rval.attributeValues.Add(av);
                }

                return rval;
            }


        }

        /// <summary>
        /// determine EPOS_BEDINGUNGEN Attribute
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public AttributeMapDto getEposBedingungen(long sysperole)
        {


            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
            {
                object[] pars = {  new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole } };

                long countbed = ctx.ExecuteStoreQuery<long>(QUERY_EPOSRAUM, pars).FirstOrDefault();
                if (countbed == 0) return null;
                //wenn keine zustände da, alles ok, falls gesetzt, anzahl liefern
                //sobald anzahl vorhanden eine loginbestätigung anfordern
                AttributeMapDto rval = new AttributeMapDto();
                rval.attributeName = AttributeName.EPOS_BEDINGUNGEN;
                rval.defaultValue = "" + countbed;
                return rval;
            }


        }


         /// <summary>
        /// determine BPEROLES
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public AttributeMapDto getBPERoles(long syswfuser)
        {


            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
            {
                

                List<String> bperoles = ctx.ExecuteStoreQuery<String>("select namebprole from bproleusr where syswfuser="+syswfuser,null).ToList();
                if (bperoles == null || bperoles.Count==0) return null;
                
                AttributeMapDto rval = new AttributeMapDto();
                rval.attributeName = AttributeName.BPEROLES;
                rval.defaultValue = "0";
                rval.attributeValues = new List<AttributeValue>();
                if (bperoles.Contains("SALES_FF") || bperoles.Contains("SALES_FF_Breite") || bperoles.Contains("SALES_FF_SA") || bperoles.Contains("SALES_FF_SA_Breite") || bperoles.Contains("SALES_FF_SD") )
                {
                    AttributeValue av = new AttributeValue();
                    av.value = "SBFG";
                    av.id = "1";
                    rval.attributeValues.Add(av);
                } 
                if (bperoles.Contains("DECISION_WFA") || bperoles.Contains("FRAUD_WFA")||bperoles.Contains("PAYMENTS_WFA"))
                {
                    AttributeValue av = new AttributeValue();
                    av.value = "SBFR";
                    av.id = "2";
                    rval.attributeValues.Add(av);
                }
                if (bperoles.Contains("SALES_KF")  )
                {
                    AttributeValue av = new AttributeValue();
                    av.value = "SBFK";
                    av.id = "3";
                    rval.attributeValues.Add(av);
                }
                return rval;
            }


        }

         /// <summary>
        /// determine NAMEBPLANES
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public AttributeMapDto getBPELanes(long syswfuser)
        {
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
            {
                List<AttributeValue> bpelanes = ctx.ExecuteStoreQuery<AttributeValue>("SELECT namebplane value,bprole.sysbprole id,teamleaderflag  flag1 FROM bproleusr a1, bplane a2, bprole WHERE bprole.namebprole=a1.namebprole and a1.namebprole = a2.namebprole and a1.syswfuser =" + syswfuser, null).ToList();
                if (bpelanes == null || bpelanes.Count == 0) return null;
                
                AttributeMapDto rval = new AttributeMapDto();
                rval.attributeName = AttributeName.BPELANES;
                rval.defaultValue =null;
                rval.attributeValues = bpelanes;
                return rval;
            }
        }


		/// <summary>
		/// determine the User's branches (Filialen)
		/// </summary>
		/// <param name="syswfuser"></param>
		/// <returns></returns>
		public AttributeMapDto getUsersFilialen (long sysperson)
		{
			using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended ())
			{
				// GET user's haendler
				// long haendler = getHaendlerDetails (syswfuser);
			
				AttributeMapDto rval = new AttributeMapDto ();
				rval.attributeName = AttributeName.USERFILIALEN;
				// rh 20171018: ADDED "Händlerfiliale" (sysroletype = 2) as possible DEFAULT: (pperole.sysroletype = 15 OR pperole.sysroletype = 2)
				rval.defaultValue = ""+ctx.ExecuteStoreQuery<long> (
					"select pperole.sysperson filiale from perole,perole pperole where perole.sysperson=" + sysperson + " and perole.sysparent=pperole.sysperole and (pperole.sysroletype = 15 OR pperole.sysroletype = 2)", null).FirstOrDefault ();

				return rval;
			}
		}

        

        /// <summary>
        /// determine KAM attributes
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public AttributeMapDto getKamAttributes(long sysperole)
        {
            AttributeMapDto rval = new AttributeMapDto();
            rval.attributeName = AttributeName.KAM;
            rval.attributeValues = new List<AttributeValue>();
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended context = new DdOlExtended())
            {
                Cic.OpenOne.Common.Model.DdOl.RoleTypeTyp pRoleType = RoleTypeTyp.HAENDLER;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "roletype", Value = pRoleType });

                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) });
                PEROLE retval = context.ExecuteStoreQuery<PEROLE>(PEROLEBYROLETYPE, parameters.ToArray()).FirstOrDefault<PEROLE>();
                if (retval != null)
                {
                    long hdperole = retval.SYSPEROLE;
                    Cic.OpenOne.Common.Model.DdOl.RoleTypeTyp roletype = RoleTypeTyp.GEBIETSLEITER;
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "roletype", Value = roletype });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = hdperole });
                    long sysperelate2 = context.ExecuteStoreQuery<long>(NATIVESQLQUERY, parameters.ToArray()).FirstOrDefault();
                    if (sysperelate2 != 0)
                    {
                        parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperelate2 });
                        KundeDto kd = context.ExecuteStoreQuery<KundeDto>(PERSONBYPEROLE, parameters.ToArray()).FirstOrDefault();
                        if (kd != null)
                        {
                            AttributeValue av = new AttributeValue();
                            av.value = kd.vorname;
                            av.id = "VORNAME";
                            rval.attributeValues.Add(av);

                            av = new AttributeValue();
                            av.value = kd.name;
                            av.id = "NAME";
                            rval.attributeValues.Add(av);

                            av = new AttributeValue();
                            av.value = kd.telefon;
                            av.id = "TELEFON";
                            rval.attributeValues.Add(av);
                        }
                    }
                }
                
            
            }
            return rval;
        }
        /// <summary>
        /// fetch all Vertriebskanäle
        /// </summary>
        /// <param name="sysvpperole"></param>
        /// <returns></returns>
        public List<long> getChannels(long sysperole)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {

                try
                {
                    long sysVpPerole = PeRoleUtil.FindRootPEROLEByRoleType(ctx, sysperole, (long)RoleTypeTyp.HAENDLER);
                    if(sysVpPerole==0) return new List<long>();
                    object[] pars = { 
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvpperole", Value = sysVpPerole } };

                    return ctx.ExecuteStoreQuery<long>(QUERYCHANNELS, pars).ToList();
                }catch(Exception)
                {
                    return new List<long>();
                }
            }
        }

        /// <summary>
        /// Changes the Users' password
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="newpassword"></param>
        public void changeUserPassword(long syswfuser, String newpassword)
        {

            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                String encPw = RpwComparator.Encode(newpassword);
                long zeit = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "zeit", Value = zeit });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pw", Value = encPw });
                ctx.ExecuteStoreCommand("insert into rpw(datum,uhrzeit,syswfuser,pw) values(sysdate,:zeit,:syswfuser,:pw)", parameters.ToArray());
                ctx.SaveChanges();
                //default next day
                ctx.ExecuteStoreCommand("update radd set expiredate=sysdate+1 where sysradd=" + syswfuser);
                int expdays = ctx.ExecuteStoreQuery<int>("select expirationdays from wfsys").FirstOrDefault();
                if(expdays>0)
                {
                    ctx.ExecuteStoreCommand("update radd set expiredate=expiredate+"+expdays+" where sysradd=" + syswfuser);
                }
                int expmonths = 30*ctx.ExecuteStoreQuery<int>("select expirationmonths from wfsys").FirstOrDefault();
                if (expmonths > 0)
                {
                    ctx.ExecuteStoreCommand("update radd set expiredate=expiredate+" + expdays + " where sysradd=" + syswfuser);
                }
            }
        }

    }
}
