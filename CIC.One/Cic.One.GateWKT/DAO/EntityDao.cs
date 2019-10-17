using System;
using System.Collections.Generic;
using System.Linq;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using AutoMapper;

using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Util.Security;
using System.Text.RegularExpressions;
using Cic.One.Web.BO.Search;
using System.Data.Common;
using System.Data.EntityClient;
using Devart.Data.Oracle;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenLease.Model.DdOw;
using Cic.OpenOne.Common.Util;
using Cic.OpenLease.Service;

namespace Cic.One.GateWKT.DAO
{
    /// <summary>
    /// Allows modifications to the standard get/createorupdate of Entities by overriding base methods
    /// </summary>
    public class EntityDao : Cic.One.Web.DAO.EntityDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static String QUERYOPP = "select  iamtype.code iamcode, vt.syskd syspersonkd, hd.code hdCode, vtobligo.RISIKOGR2 eurotaxblau,vtobligo.RISIKOGR3 eurotaxgelb,OPPO.*, trim(wf.name)||' '||trim(wf.vorname) wfuserName, vt.vertrag gebietName,trim(vk.name)||' '||trim(vk.vorname) vkName, trim(hd.name)||' '||trim(hd.vorname) hdName,trim(hd.ort) hdOrt,vt.ENDE,vt.vart, ob.erstzul, trim(ob.hersteller) marke, trim(ob.objektvt) modell,trim(ob.kennzeichen) kennzeichen,trim(ob.serie) serie,camp.name campName,(select max(oppotask.phase) from oppotask where oppotask.sysoppo=oppo.sysoppo) worstActivityStatus,(select min(oppotask.status) from oppotask where oppotask.sysoppo=oppo.sysoppo) bestActivityStatus,(select count(*) from oppotask where oppotask.sysoppo=oppo.sysoppo) activityCount, (select sklasse from schwacke where schwacke.schwacke=ob.schwacke) baureihe   from CIC.OPPO OPPO left outer join camp on camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser, vt left outer join vtobligo on  vtobligo.sysvtobligo=vt.sysid , person kd, person vk, person hd ,ob,iam,iamtype where kd.sysperson=vt.syskd and vk.sysperson=vt.sysberatadda and hd.sysperson=vt.sysvpfil and vt.sysid=oppo.sysid and oppo.area='VT' and ob.sysvt=vt.sysid and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype and oppo.sysoppo=:sysoppo";

        //private static String QUERYOPP2 = "Select sysobsich from obsich where aktivflag=1 and sysvt=:sysvt and rang=200";
        private static double CalculateGrossValue(double netValue, double taxRate)
        {
            return netValue + ((netValue * taxRate) / 100.0);
        }
        /// <summary>
        /// Returns all OpportunityDetails
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        override
        public OpportunityDto getOpportunityDetails(long sysopportunity)
        {
            //ob.fzart,ob.ubnahmekm,ob.schwacke,ob.jahreskm,kalk.lz laufzeit,bgextern ahk,trim(vt.konstellation) konstellation,vt.ENDE,vt.vart,vt.beginn,vt.vertrag vertrag,kalk.sz+kalk.anzahlung sz, kalk.rate,kalk.depot, kalk.rw, vtobligo.RISIKOGR2 eurotaxblau,vtobligo.RISIKOGR3 eurotaxgelb,
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysoppo", Value = sysopportunity });
                OpportunityDto rval = ctx.ExecuteStoreQuery<OpportunityDto>(QUERYOPP, parameters.ToArray()).FirstOrDefault();
                if (rval == null) return base.getOpportunityDetails(sysopportunity);

                rval.kunde = getAccountDetails(rval.sysPersonKd);
                rval.vertrag = getVertragDetails(rval.sysid);
                if (rval.baureihe == null || rval.baureihe.Length == 0)
                {
                    long sysobtyp = ctx.ExecuteStoreQuery<long>("select obtyp.sysobtyp from obtyp,ob,vt where vt.sysid=ob.sysvt and ob.schwacke=obtyp.schwacke and vt.sysid=" + rval.vertrag.sysID, null).FirstOrDefault();
                    rval.baureihe = ctx.ExecuteStoreQuery<String>("select bezeichnung from obtyp where importtable='ETGMODEL' start with sysobtyp=" + sysobtyp + "  connect by prior sysobtypp=sysobtyp", null).FirstOrDefault();
                    if (rval.baureihe == null)
                    {
                        List<String> bezeichnungen = ctx.ExecuteStoreQuery<String>("select bezeichnung from obtyp  start with sysobtyp=" + sysobtyp + "  connect by prior sysobtypp=sysobtyp", null).ToList<String>();
                        if (bezeichnungen.Count > 1)
                            rval.baureihe = bezeichnungen[1];
                    };
                }
                //fetch for linking person into aida by IT:
                OpportunityDto itinfo = ctx.ExecuteStoreQuery<OpportunityDto>("select sysit, syskdtyp syskdtypit from it where sysperson=" + rval.sysPersonKd, null).FirstOrDefault();
                if (itinfo != null && itinfo.sysit > 0)
                {
                    rval.syskdtypit = itinfo.syskdtypit;
                    rval.sysit = itinfo.sysit;
                }
             /*   else if (rval.kunde != null && rval.kunde.name != null)
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "name", Value = rval.kunde.name.Trim() });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vorname", Value = rval.kunde.vorname != null ? rval.kunde.vorname.Trim() : "" });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "plz", Value = rval.kunde.plz != null ? rval.kunde.plz.Trim() : "" });
                    itinfo = ctx.ExecuteStoreQuery<OpportunityDto>("select sysit, syskdtyp syskdtypit from it where trim(name)=:name and trim(vorname)=:vorname and trim(plz)=:plz", parameters.ToArray()).FirstOrDefault();
                    if (itinfo != null && itinfo.sysit > 0)
                    {
                        rval.syskdtypit = itinfo.syskdtypit;
                        rval.sysit = itinfo.sysit;
                    }
                }*/

                if (rval.vertrag != null)
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.vertrag.sysID });
                    rval.vertrag.mitantragsteller = ctx.ExecuteStoreQuery<String>("select trim(it.vorname)||' '||trim(it.name) ma from vtobsich,it where it.sysit=vtobsich.sysmh and rang>=100 and rang<105 and aktivflag=1 and sysvt=:sysvt", parameters.ToArray()).ToArray();
                    //TODO RNV-Flag
                }
                if (rval.vertrag != null && rval.vertrag.obList != null && rval.vertrag.obList.Count > 0)
                {

                    long sysob = rval.vertrag.obList[0].sysOb;
                    long syskalk = ctx.ExecuteStoreQuery<long>("select syskalk from kalk where sysob=" + sysob, null).FirstOrDefault();
                    rval.kalk = getKalkDetails(syskalk);

                    double tax = (double)LsAddHelper.GetTaxRate(rval.vertrag.sysVart);
                    rval.kalk.rate = CalculateGrossValue(rval.kalk.rate, tax);
                    rval.kalk.rw = CalculateGrossValue(rval.kalk.rw, tax);
                    rval.kalk.sz = CalculateGrossValue(rval.kalk.sz, tax);
                    if (rval.kalk.anzahlung > 0)
                        rval.kalk.sz = rval.kalk.anzahlung;

                    rval.kalk.bgExtern = CalculateGrossValue(rval.kalk.bgExtern, tax);
                        
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.vertrag.sysID });
                    rval.kalk.zinstyp = ctx.ExecuteStoreQuery<int>("select vtobsl.variabel from vtobsl where vtobsl.sysvt = :sysvt and vtobsl.syssltyp in (select syssltyp from sltyp where renditeflag = 1) and vtobsl.inaktiv = 0", parameters.ToArray()).FirstOrDefault();

                }


                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = rval.area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = rval.sysid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysiam", Value = rval.sysiam });
                rval.sysOppoArea = ctx.ExecuteStoreQuery<long>("select sysoppo from oppo where area=:area and sysid=:sysid and sysiam!=:sysiam order by sysoppo desc", parameters.ToArray()).FirstOrDefault();
                if (rval.syschguser > 0)
                {
                    rval.ownershipsf = ctx.ExecuteStoreQuery<int>(@"select 1 from dual where exists (SELECT roletype.typ FROM perole, roletype    WHERE roletype.typ in (3, 9, 14, 4) and roletype.sysroletype = perole.sysroletype and (validfrom is null or validfrom <= trunc(sysdate)) AND 
(validuntil is null or validuntil >= trunc(sysdate))   AND sysperson IN (SELECT person.sysperson FROM person,puser WHERE puser.syspuser =person.syspuser and puser.syswfuser=" + rval.syschguser + "))", null).FirstOrDefault();
                }
                return rval;
            }
        }

        /// <summary>
        /// updates/creates Oppo
        /// 
        /// </summary>
        /// <param name="oppo"></param>
        /// <returns></returns>
        override
        public OpportunityDto createOrUpdateOppo(OpportunityDto oppo)
        {

            if ("EOT".Equals(oppo.iamcode) && oppo.sysid > 0)
            {
                try
                {
                    using (OwExtendedEntities ctx = new OwExtendedEntities())
                    {
                        long oldresultat = ctx.ExecuteStoreQuery<long>("select resultat from oppo where sysoppo=" + oppo.sysOppo, null).FirstOrDefault();

                        
                        if (oldresultat != oppo.resultat)
                        {
                            long[] trues = { 0, 1, 4, 6 };
                            long[] falses = { 2, 3, 5, 7, 8, 9, 10 };
                            int durchfuehrenFlag = 1;
                            String mainMsg = "Unterdrückung automatische Verlängerung und Reminder Schreiben";
                            if (trues.Contains(oppo.resultat))
                            {
                                durchfuehrenFlag = 0;
                                mainMsg = "Aktivierung automatische Verlängerung und Reminder Schreiben";
                            }
                            int cflag = ctx.ExecuteStoreQuery<int>("select flag01 from VTOPTION  where sysid=" + oppo.sysid, null).FirstOrDefault();
                            if (cflag != durchfuehrenFlag)
                            {
                                ctx.ExecuteStoreCommand("update VTOPTION set flag01=" + durchfuehrenFlag + " where sysid=" + oppo.sysid, null);
                                String username = ctx.ExecuteStoreQuery<String>("select name from wfuser where syswfuser=" + getSysWfuser(), null).FirstOrDefault();
                                if (username == null)
                                    username = "";
                                if (username.Length > 40)
                                    username = username.Substring(0, 40);
                                long syswftable = ctx.ExecuteStoreQuery<long>("select syswftable from wftable where syscode='VT'", null).FirstOrDefault();
                                WFMMEMO memo = new WFMMEMO();
                                /*(from p in ctx.WFMMEMO
                                                where p.SYSLEASE == oppo.sysid && p.SYSWFMTABLE == syswftable
                                                select p).FirstOrDefault();*/
                                memo.CREATEUSER = getSysWfuser();
                                memo.CREATEDATE = DateTime.Now;
                                memo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                                memo.SYSWFMTABLE = syswftable;
                                memo.STR02 = username;
                                memo.SYSLEASE = oppo.sysid;
                                memo.STR07 = mainMsg.Substring(0, 40);

                                memo.STR10 = "End of Term";
                                memo.NOTIZMEMO = StringConversionHelper.StringToClarionByte(mainMsg);
                                ctx.AddToWFMMEMO(memo);
                                ctx.SaveChanges();
                            }

                        }

                    }
                }
                catch (Exception e)
                {
                    _log.Error("Automatischer Änderungsdienst fehlgeschlagen: " + e.Message, e);
                }
            }
            /*
            
            using (DdOlExtended ctx = new DdOlExtended())
            {
                long oldstatus = ctx.ExecuteStoreQuery<long>("select status from oppo where sysoppo="+oppo.sysOppo,null).FirstOrDefault();
                long oldchguser = ctx.ExecuteStoreQuery<long>("select syschguser from oppo where sysoppo="+oppo.sysOppo,null).FirstOrDefault();
                if (oldstatus != oppo.status||oldchguser!=oppo.syschguser)
                {
                         DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                        con.Open();
                        try
                        {
                            DbCommand cmdkey = con.CreateCommand();
                            cmdkey.CommandText = "insert into cictlog (syscictlog) select max(syscictlog)+1 from cictlog returning syscictlog into :myOutputParameter";
                            cmdkey.Parameters.Add(new OracleParameter("myOutputParameter", OracleDbType.Long, System.Data.ParameterDirection.ReturnValue));
                            cmdkey.ExecuteNonQuery();
                            int syscictlog = Convert.ToInt32(cmdkey.Parameters["myOutputParameter"].Value);

                            DbCommand cmd = con.CreateCommand();
                            cmd.CommandText = "insert into cictlog (syscictlog,olarea, syslease, changedate, orauser) values(:syscictlog,'OPPO',:sysoppo, current_date,'CIC')";
                            cmd.Parameters.Add(new OracleParameter("sysoppo", OracleDbType.Long));
                            cmd.Parameters.Add(new OracleParameter("syscictlog", OracleDbType.Long));
                            cmd.Parameters["sysoppo"].Value = oppo.sysOppo;
                            cmd.Parameters["syscictlog"].Value = syscictlog;
                            cmd.ExecuteNonQuery();
                            

                            cmd = con.CreateCommand();
                            cmd.Parameters.Add(new OracleParameter("syscictlog", OracleDbType.Long));
                            cmd.Parameters.Add(new OracleParameter("field", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("old", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("new", OracleDbType.VarChar));


                            cmdkey = con.CreateCommand();
                            cmdkey.CommandText = "insert into cicflog (syscicflog) select max(syscicflog)+1 from cicflog returning syscicflog into :myOutputParameter";
                            cmdkey.Parameters.Add(new OracleParameter("myOutputParameter", OracleDbType.Long, System.Data.ParameterDirection.ReturnValue));
                            cmd.CommandText = "insert into cicflog (syscicflog, syscictlog, olfield, oldvalue,newvalue) values(:syscicflog, :syscictlog, :field,:old, :new)";
                            cmd.Parameters["syscictlog"].Value = syscictlog;


                            if (oldstatus != oppo.status)
                            {
                                cmdkey.ExecuteNonQuery();
                                cmd.Parameters["syscicflog"].Value = Convert.ToInt32(cmdkey.Parameters["myOutputParameter"].Value);

                                cmd.Parameters["field"].Value = "STATUS";
                                cmd.Parameters["old"].Value = ""+oldstatus;
                                cmd.Parameters["new"].Value = "" + oppo.status;
                                cmd.ExecuteNonQuery();
                            }
                            if (oldstatus != oppo.status)
                            {
                                cmdkey.ExecuteNonQuery();
                                cmd.Parameters["syscicflog"].Value = Convert.ToInt32(cmdkey.Parameters["myOutputParameter"].Value);

                                cmd.Parameters["field"].Value = "SYSCHGUSER";
                                cmd.Parameters["old"].Value = "" + oldchguser;
                                cmd.Parameters["new"].Value = "" + oppo.syschguser;
                                cmd.ExecuteNonQuery();
                            }
                            cmd.Dispose();
                        }
                        catch (Exception e)
                        {
                            _log.Error("Error updating changelog", e);
                        }
                
                        finally
                        {
                            con.Close();
                        }

                    
                }
            }*/
            OpportunityDto rval = base.createOrUpdateOppo(oppo);
            return getOpportunityDetails(rval.sysOppo);
        }
    }
}