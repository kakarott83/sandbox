using AutoMapper;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cic.One.Workflow.DAO
{
    public class WorkflowDao
    {
        private const String QUERYWFV = "select einrichtung from wfv where lower(syscode)=lower(:syscode)";
        private const String QUERYBEFEHLSZEILE = "SELECT befehlszeile from wfv where lower(syscode)=lower(:syscode)";
        private const String QUERYDEEPLNK = "select * from deeplnk where code=:code";
        private const String QUERYALERTS = //"select count(*) alerts from cic.ctlang,cic.ptype ptype, cic.pcontent pcontent, cic.ptask ptask, cic.pchecker pchecker where ctlang.sysctlang=pcontent.sysctlang and ptype.art in (10,20) and ptask.completeflag=0 and ptype.status='Offen' and pchecker.sysptask=ptask.sysptask and ptask.sysptype=ptype.sysptype and ptype.sysptype=pcontent.sysptype(+) and pchecker.syswfuser=:syswfuser and ctlang.isocode=:isocode and (ptype.validuntil is null or ptype.validuntil>=sysdate  or ptype.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (ptype.validfrom is null or sysdate>=ptype.validfrom  or ptype.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and (ptype.art=20 or (ptype.art=10 and pchecker.result is null))";
                                           "select count(distinct ptype.sysptype) alerts from cic.ptype ptype, cic.pcontent pcontent, cic.ptask ptask, cic.pchecker pchecker where ptype.art in (10,20) and ptask.completeflag=0 and ptype.status='Offen' and pchecker.sysptask=ptask.sysptask and ptask.sysptype=ptype.sysptype and ptype.sysptype=pcontent.sysptype(+) and pchecker.syswfuser=:syswfuser and (ptype.validuntil is null or ptype.validuntil>=sysdate  or ptype.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (ptype.validfrom is null or sysdate>=ptype.validfrom  or ptype.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and (ptype.art=20 or (ptype.art=10 and pchecker.result is null))";
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// returns a einrichtungs content string for the wfv syscode
        /// </summary>
        /// <param name="syscode"></param>
        /// <returns></returns>
        public String getWfvEinrichtung(String syscode)
        {



            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscode", Value = syscode });


                String rval = ctx.ExecuteStoreQuery<String>(QUERYWFV, pars.ToArray()).FirstOrDefault();
                //remove old clob content after new content
                if (rval != null && rval.IndexOf('\0') > 0)
                {
                    rval = rval.Substring(0, rval.IndexOf('\0'));
                }
                return rval;
            }
        }

        /// <summary>
        /// returns the syscodes befehlszeile
        /// </summary>
        /// <param name="syscode"></param>
        /// <returns></returns>
        public String getBefehlszeile(String syscode)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscode", Value = syscode });
                return ctx.ExecuteStoreQuery<String>(QUERYBEFEHLSZEILE, pars.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// creates a eaihot for olstart from browser
        /// saves all necessary entrys to allow openlease to jump directly into the application at a certain point, bu just giving it a guid
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="code"> deeplink code</param>
        /// <param name="expr">expression to evaluate in target</param>
        /// <param name="wctx">context for current workflow</param>
        /// <param name="continueWorkflow">1 when wf shall continue</param>
        /// <param name="inbox">1 when deeplink is only for inbox</param>       
        /// <param name="sysdeeplink">sysdeeplink</param>       
        /// <returns>guid for link</returns>
        public String execOlStart(String area, long sysid, long syswfuser, String code, String expr, WorkflowContext wctx, int continueWorkflow, int inbox,long sysdeeplink)
        {
            EAIHOT eaihotInput = new EAIHOT();
            eaihotInput.CODE = code;
            if (code.Length > 25)
                eaihotInput.CODE = eaihotInput.CODE.Substring(0, 25);
            eaihotInput.OLTABLE = area;
            eaihotInput.COMPUTERNAME = Guid.NewGuid().ToString();
            eaihotInput.SYSOLTABLE = sysid;
            eaihotInput.PROZESSSTATUS = 0;
            eaihotInput.SYSWFUSER = syswfuser;
            DateTime d = DateTime.Now.AddHours(1);
            eaihotInput.STARTDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(d);
            eaihotInput.STARTTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(d);
            eaihotInput.INPUTPARAMETER1 = code;
            eaihotInput.INPUTPARAMETER2 = "" + continueWorkflow;
            eaihotInput.INPUTPARAMETER3 = "" + wctx.sysbpprocinst;
            d = DateTime.Now;
            eaihotInput.SUBMITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(d);
            eaihotInput.SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(d);

            using (DdOwExtended owCtx = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code });
                eaihotInput.EVALEXPRESSION = expr;
                owCtx.AddToEAIHOT(eaihotInput);
                eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
                                      where EaiArt.CODE.Equals("#GenericDeepLink")
                                      select EaiArt).FirstOrDefault();
                owCtx.SaveChanges();
                if(inbox==1)
                {
                    owCtx.ExecuteStoreCommand("update deeplnkreq set status=1 where syswfuser="+syswfuser+" and status=0", null);
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syseaihot", Value = eaihotInput.SYSEAIHOT});
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdeeplnk", Value = sysdeeplink });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "status", Value = 0 });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "createdate", Value = d });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "createtime", Value = eaihotInput.SUBMITTIME });
                    owCtx.ExecuteStoreCommand("insert into deeplnkreq(syseaihot,sysdeeplnk,syswfuser,status,createdate,createtime) values(:syseaihot,:sysdeeplnk,:syswfuser,:status,:createdate,:createtime)", parameters.ToArray());
                }

                owCtx.SaveChanges();
            }


            return eaihotInput.COMPUTERNAME;
        }

        /// <summary>
        /// Returns a deeplink
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DeepLnkDto getDeepLink(String code)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code });
                return ctx.ExecuteStoreQuery<DeepLnkDto>(QUERYDEEPLNK, pars.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// Creates a deeplink for the area/id, user and deeplink-Id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="deeplinkId"></param>
        /// <returns></returns>
        public String createDeeplink(String area, long sysid, long syswfuser, String deeplinkId)
        {
            //create deeplink url            
            DeepLnkDto link = getDeepLink(deeplinkId);
            if (link == null) return null;
            WorkflowContext wctx = new WorkflowContext();
            wctx.areaid = "" + sysid;
            wctx.area = area;
            wctx.entities.eaihot = new EaihotDto();
            wctx.entities.eaihot.computername = Guid.NewGuid().ToString();
            wctx.entities.eaihot.oltable = "DEEPLNK";
            wctx.entities.eaihot.sysoltable = link.SYSDEEPLNK;
            wctx.entities.eaihot.code = "DeepLink";
            wctx.entities.eaihot.syswfuser = syswfuser;
            wctx.entities.eaihot.startdate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
            wctx.entities.eaihot.starttime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);

            //when evalparam02 of deeplink is set, assign its sysvlmconf to the inputparameter2
            if (link.EVALPARAM02 != null)
            {
                using (DdOwExtended ctx = new DdOwExtended())
                {
                    try
                    {

                        String p2 = link.EVALPARAM02.Trim().Trim('\'');
                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = p2 });
                        wctx.entities.eaihot.inputparameter2 = "" + ctx.ExecuteStoreQuery<long>("select sysvlmconf from vlmconf where code =:code", pars.ToArray()).FirstOrDefault();
                    }catch(Exception exc2)
                    {
                        _log.Warn("Creating Deeplink INPUTPARAMETER2 from EVALPARAM02 failed: " + exc2.Message);   
                    }
                }
            }
            String expr = link.getParsedExpression(wctx);
            String linkexpr = link.getParsedExpression(wctx, link.PARAMEXPRESSION);
            String prefix = "";
            if (link.ALTERNATEBASISURL == null)
                link.ALTERNATEBASISURL = "SETUP/DEEPLINK/DEFAULTURL";
            if (link.ALTERNATEBASISURL != null)
            {
                link.ALTERNATEBASISURL = link.ALTERNATEBASISURL.Replace('\\', '/');
                String[] cfgs = link.ALTERNATEBASISURL.Split('/');
                prefix = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(cfgs[1], cfgs[2], "", cfgs[0]);
            }
            wctx.entities.eaihot.evalexpression = expr;
            EaihotDto eaihot = wctx.entities.eaihot;
            using (DdOwExtended ctx = new DdOwExtended())
            {
                
                    EAIHOT rval = new EAIHOT();
                    rval = Mapper.Map<EaihotDto, EAIHOT>(eaihot, rval);
                    rval.SYSEAIHOT = 0;

                    ctx.AddToEAIHOT(rval);
                    ctx.SaveChanges();
            }
            
            return prefix + linkexpr;//links via paramexpression into cic one, forwarding to the evalexpression of the eaihot
        }

        /// <summary>
        /// Returns true if the data is available in the database
        /// </summary>
        /// <param name="area"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool isDeepLinkDataAvailable(String area, String id)
        {
            if (area == null) return false;
            String tarea = area.ToUpper();
            if ("SYSTEM".Equals(tarea))
                return true;
            //areas not in this list wont be checked
            String[] supportedAreas = { "ANGEBOT", "ANTRAG", "VT", "IT", "PERSON" };
            if (!supportedAreas.Contains(area))
                return true;

            Dictionary<String, String> areaQuery = new Dictionary<string, string>();
            areaQuery["ANGEBOT"] = "select count(*) from ANGEBOT where sysid=:id";
            areaQuery["ANTRAG"] = "select count(*) from ANTRAG where sysid=:id";
            areaQuery["VT"] = "select count(*) from VT where sysid=:id";
            areaQuery["IT"] = "select count(*) from IT where sysit=:id";
            areaQuery["PERSON"] = "select count(*) from PERSON where sysperson=:id";

            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                int isAvail = ctx.ExecuteStoreQuery<int>(areaQuery[tarea], pars.ToArray()).FirstOrDefault();
                return isAvail > 0;
            }
        }

        /// <summary>
        /// Returns the amount of prüfaufgaben
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="isocode"></param>
        /// <returns></returns>
        public String getAlerts(long syswfuser, String isocode)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                 List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                //pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isocode });
            
                int rval = ctx.ExecuteStoreQuery<int>(QUERYALERTS,pars.ToArray()).FirstOrDefault();
                return rval.ToString();
            }
        }
    }
}
