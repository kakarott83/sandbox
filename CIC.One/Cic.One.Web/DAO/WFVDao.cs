using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using System.Text;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.One.Web.DAO
{
    /// <summary>
    /// DAO for WFV Configurations like Menu, Toolbar, WFV
    /// </summary>
    public class WFVDao : IWFVDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static String ENCODING = "UTF-8";

        private static String QUERYMENU = "select vlmmenu.beschreibung text,vlmmenu.sysvlmmenu,wftable.syscode code  from vlmmenu, vlmconf, wftable where wftable.syswftable = vlmmenu.syswftable and vlmconf.sysvlmconf=vlmmenu.sysvlmconf and vlmconf.code=:vlmcode";
        private static String QUERYITEM = "select coderfu,codermo,sysvlmmitem,sysvlmmparent, mtext text,mart art, maction action, checkausdruck checkExpression,micon icon,checkauswirkung checkeffect from vlmmitem  where sysvlmmenu=:sysvlmmenu  connect by prior vlmmitem.sysvlmmitem = vlmmitem.sysvlmmparent start with vlmmitem.sysvlmmparent is null order siblings by rang";
        private static String QUERYTOOLBAR = "select vlmtool.beschreibung text,vlmtool.sysvlmtool sysvlmmenu,wftable.syscode code  from vlmtool, vlmconf, wftable where wftable.syswftable = vlmtool.syswftable and vlmconf.sysvlmconf=vlmtool.sysvlmconf and vlmconf.code=:vlmcode   order by case when code='SYSTEM' then 1 end asc";
        private static String QUERYTOOLBARITEMS = "select coderfu,codermo,headerflag header,bicon icon,sysvlmitem sysvlmmitem, btext text,btip tip, baction action, checkausdruck checkExpression,sysvlmtool sysvlmmparent,checkauswirkung checkeffect, webart from vlmitem  where sysvlmtool in (select vlmtool.sysvlmtool  from vlmtool, vlmconf where vlmconf.sysvlmconf=vlmtool.sysvlmconf and vlmconf.code=:vlmcode)  order by rang";

        private static String QUERYCONTEXTMENU = "select sysvlmpf sysvlmmenu,wftable.syscode code, vlmpf.beschreibung text from vlmpf, wftable,vlmconf where wftable.syswftable = VLMPF.syswftable and vlmconf.sysvlmconf=VLMPF.sysvlmconf and vlmconf.code=:vlmcode";
        private static String QUERYCONTEXTMENUITEMS = "select coderfu,codermo,modicon icon,sysvlmpfitem sysvlmmitem, modtext text,pfaction action, checkausdruck checkExpression,VLMPFITEM.sysvlmpf sysvlmmparent,checkauswirkung checkeffect from VLMPFITEM where sysvlmpf in (select vlmpf.sysvlmpf  from vlmpf, vlmconf where vlmconf.sysvlmconf=vlmpf.sysvlmconf and vlmconf.code=:vlmcode) order by rang";

        private static String QUERYWFV = "select coderfu,codermo,wfv.syscode,wfv.befehlszeile,wfv.einrichtung,entrytype from wfv, wfvcommand where entrytype<3 and wfvcommand.webflag=1 and wfv.befehlszeile=wfvcommand.befehlszeile and wfvcommand.revisionfrom<=:vers and (wfvcommand.revisionuntil>=:vers or wfvcommand.revisionuntil is null) and wfv.kurzbez not like 'AUTO-TEMPLATE%'";


        //private static String QUERYWFVAVAIL = "select wfv.* from wfv where syscode=:syscode";//, wfvcommand where entrytype<3 and wfvcommand.webflag=1 and wfv.befehlszeile=wfvcommand.befehlszeile and wfvcommand.revisionfrom<=:vers and (wfvcommand.revisionuntil>=:vers or wfvcommand.revisionuntil is null) and wfvcommand.befehlszeile=:befehlszeile and syscode=:syscode";

        //all command valid for a certain version
        private static String QUERYCMDS = "select befehlszeile,revisionfrom,revisionuntil from wfvcommand where wfvcommand.webflag=1";// and wfvcommand.revisionfrom<=:vers and (wfvcommand.revisionuntil>=:vers or wfvcommand.revisionuntil is null)";
        //private static String QUERYCMD = "select syswfvcommand from wfvcommand where wfvcommand.webflag=1 and wfvcommand.revisionfrom<=:vers and (wfvcommand.revisionuntil>=:vers or wfvcommand.revisionuntil is null) and befehlszeile=:befehlszeile";

        private static String QUERYTABLES = "select vlmtable.*,wftable.syscode from vlmtable,wftable,vlmconf where vlmconf.sysvlmconf=vlmtable.sysvlm and wftable.syswftable=vlmtable.syswftable and vlmconf.code=:code";
        //private static String QUERYWFV = "select syscode,befehlszeile,einrichtung from wfv where typ=1 and befehlszeile!='WORKFLOW'";
        private static String QUERYVLM = "select vlmconf.*,(select sysmwst from lsadd where lsadd.syslsadd=vlmconf.sysls) sysmwst,(select syshauswaehrung from lsadd where lsadd.syslsadd=vlmconf.sysls) syshauswaehrung from vlmconf where (flagdeactivate=0 or flagdeactivate is null) and webvlm=1  order by sysvlmconf";
        private static String QUERYCURRENCY = "select cic.waehrung.syswaehrung, cic.waehrung.code,  cic.waehrung.bezeichnung  from cic.waehrung";
        private static String QUERYLANGUAGES = "select * from ctlang where flagtranslate=1 and activeflag=1 and cic.mdbs_getfavorite('FAVORITEN','SPRACHE', trim(upper(languagename))) is not null  order by cic.mdbs_getfavorite('FAVORITEN','SPRACHE', trim(upper(languagename))), languagename";
        private static String QUERYWFSYS = "select * from wfsys";
        private static String QUERYCICCONF = "select * from cicconf";

        private static String QUERYWFVCONFIG = "select syscode,befehlszeile,einrichtung from wfv where lower(syscode)=lower(:syscode)";
        private static String PATHWFV = "\\..\\wfvconfig.dll";
        private static String PATHWFVDEV = "\\..\\Cic.One.Web.Service\\wfvconfig.dll";
        private static String PATHWFVDEV2 = "\\..\\..\\Cic.One.Web.Service\\wfvconfig.dll";

        public static bool dbsynced = false;//is set to true when wfvconfig.dll was synced into db
        private static CacheDictionary<String, List<VlmConfDto>> vlmCache = CacheFactory<String, List<VlmConfDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, WfvEntry> wfvEntryCache = CacheFactory<String, WfvEntry>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static WfvConfig config;

        /// <summary>
        /// Flushes all cached Data
        /// </summary>
        public static void flushCache()
        {
            config = null;
            dbsynced = false;
            wfvEntryCache.Clear();
            vlmCache.Clear();
        }

        /// <summary>
        /// fetches the wfv configuration for the wfvEntry syscode from wfvconfig.dll
        /// also allows overriding the wfventry-settings from inside the dashboard, when wfvid is of structure dashboardsyscode:wfvsyscode
        /// </summary>
        /// <param name="wfvid">lowercase id</param>
        /// <returns></returns>
        public WfvEntry getWfvEntry(String wfvid)
        {
            if (String.IsNullOrEmpty(wfvid))
                return null;

            String orgId = wfvid;
            String dashId = null;

            if (wfvid.IndexOf(":") > -1)
            {
                String[] ids = wfvid.Split(':');
                dashId = ids[0];
                wfvid = ids[1];
                if ("null".Equals(dashId))
                    dashId = null;
                if ("null".Equals(wfvid))
                    return null;
            }

            if (!wfvEntryCache.ContainsKey(orgId))
            {
                ogetVlmConfigDto rval = new ogetVlmConfigDto();
                byte[] wfvconfigData = null;

                try
                {
                    wfvconfigData = FileUtils.loadData(FileUtils.getCurrentPath() + PATHWFV);
                }
                catch (Exception)
                {
                    try
                    {
                        wfvconfigData = FileUtils.loadData(FileUtils.getCurrentPath() + PATHWFVDEV);
                    }
                    catch (Exception)
                    {
                        wfvconfigData = FileUtils.loadData(FileUtils.getCurrentPath() + PATHWFVDEV2);
                    }

                }
                WfvConfig loadedCfg = XMLDeserializer.objectFromXml<WfvConfig>(wfvconfigData, ENCODING);
                addWFListQueries(loadedCfg.entries);
                WfvEntry entry = loadedCfg.entries.Where(a => a.syscode.Equals(wfvid)).FirstOrDefault();
                if (entry == null)
                    entry = loadedCfg.entries.Where(a => a.syscode.Equals(wfvid.ToLower())).FirstOrDefault();
                if (entry == null)
                    entry = getWfvConfig(wfvid);

                if (dashId != null)
                {
                    WfvEntry entryov = loadedCfg.entries.Where(a => a.syscode.Equals(dashId)).FirstOrDefault();

                    if (entryov != null)
                    {
                        WfvRef wfref = entryov.references.Where(a => a.syscode.Equals(wfvid)).FirstOrDefault();
                        if (wfref != null && wfref.customentry != null)
                            overlayWfvEntry(entry.customentry, wfref.customentry);
                    }
                }


                wfvEntryCache[orgId] = entry;
            }
            return wfvEntryCache[orgId];
        }
        /**
         * Overlays a overriden wfv CustomEntry over the original ViewConfig Entry
         *
         * @param vc
         * @param cust
         */
        private static void overlayWfvEntry(CustomEntry org, CustomEntry cust)
        {
            if (cust == null)
            {
                return;
            }

            if (cust.createsyscode != null)
            {
                org.createsyscode = cust.createsyscode;
            }
            if (cust.detailsyscode != null)
            {
                org.detailsyscode = cust.detailsyscode;
            }
            if (cust.filter != null)
            {
                org.filter = cust.filter;
            }
            if (cust.filterfields != null)
            {
                org.filterfields = cust.filterfields;
            }
            if (cust.forwardsyscode != null)
            {
                org.forwardsyscode = cust.forwardsyscode;
            }
            if(cust.searchmode!=SearchMode.Unset)
                org.searchmode = cust.searchmode;   

            if (cust.icon != null)
            {
                org.icon = cust.icon;
            }
            if (cust.internalfilter != null)
            {
                org.internalfilter = cust.internalfilter;
            }
            if (cust.sortfields != null)
            {
                org.sortfields = cust.sortfields;
            }
            if (cust.sortorder != null)
            {
                org.sortorder = cust.sortorder;
            }
            if (cust.title != null)
            {
                org.title = cust.title;
            }

            if (cust.filters != null && cust.filters.Count > 0)
            {
                org.filters = cust.filters;
            }

            if (cust.viewmeta != null)
            {
                org.viewmeta = cust.viewmeta;
            }

            //IMPORTANT - every wfvref with customentry has to define instantSearch filteropen and readOnly, else these values will be reset to 0
            org.instantSearch = cust.instantSearch;
            org.filteropen = cust.filteropen;
            org.readOnly = cust.readOnly;

        }
        private class CommandInfoDto
        {
            public String befehlszeile { get; set; }
            public long revisionfrom { get; set; }
            public long revisionuntil { get; set; }

        }
        /// <summary>
        /// Synchronizes the table of available webvlm commandlines(templates)
        /// 
        /// new entry: when not found in db
        /// revisionuntil to revision-1: when found in db, but not in sent commandLines
        ///                              when found in db, but marked deprecated in commandLines
        /// </summary>
        /// <param name="commandLines"></param>
        public void synchronizeViewConfig(List<ViewConfigDto> commandLines)
        {
            long version = long.Parse(getVersion());
            if (version == 0) return;
            //read all wfv-commandlines with revisionfrom< currentversion
            try
            {
                Dictionary<String, String> dblCheck = new Dictionary<string, string>();
                using (DdOwExtended ctx = new DdOwExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vers", Value = version });

                    //find all wfv for given version
                    List<CommandInfoDto> available = ctx.ExecuteStoreQuery<CommandInfoDto>(QUERYCMDS, pars.ToArray()).ToList();
                    foreach (CommandInfoDto cmd in available)
                    {

                        var f = (from s in commandLines
                                 where s.befehlszeile.Equals(cmd.befehlszeile)
                                 select s.befehlszeile).FirstOrDefault();

                        if (f == null)//in db but not in sent command lines, update db and set validuntil revision
                        {
                            //update the revisionuntil commandline without a revisionuntil for the command
                            List<Devart.Data.Oracle.OracleParameter> pars2 = new List<Devart.Data.Oracle.OracleParameter>();
                            pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "version", Value = version - 1 });
                            pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "cmd", Value = cmd.befehlszeile });
                            ctx.ExecuteStoreCommand("update wfvcommand set revisionuntil=:version where befehlszeile=:cmd and revisionuntil is null", pars2.ToArray());
                        }
                    }
                    List<String> availCmds = (from s in available                                              
                                              select s.befehlszeile).ToList();
                    foreach (ViewConfigDto cmd in commandLines)
                    {
                        if (dblCheck.ContainsKey(cmd.befehlszeile))
                        {
                            _log.Warn("Double BEFEHLSZEILE " + cmd.befehlszeile+" found in db");
                        }
                        if (availCmds.Contains(cmd.befehlszeile))//command already in db
                        {
                            if (cmd.deprecated == 1)//command is now deprecated, update db
                            {
                                //update the revisionuntil commandline without a revisionuntil for the command
                                List<Devart.Data.Oracle.OracleParameter> pars2 = new List<Devart.Data.Oracle.OracleParameter>();
                                pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "version", Value = version - 1 });
                                pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "cmd", Value = cmd.befehlszeile });
                                ctx.ExecuteStoreCommand("update wfvcommand set revisionuntil=:version where befehlszeile=:cmd and revisionuntil is null", pars2.ToArray());
                                continue;
                            }
                            CommandInfoDto dbcmd = (from s in available
                                     where s.befehlszeile.Equals(cmd.befehlszeile)
                                     select s).FirstOrDefault();
                            if (dbcmd.revisionuntil>0 && dbcmd.revisionuntil < version)//in db, but was disabled with untilversion, is available again now
                            {
                                List<Devart.Data.Oracle.OracleParameter> pars2 = new List<Devart.Data.Oracle.OracleParameter>();
                                pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "version", Value = version});                                
                                pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "cmd", Value = cmd.befehlszeile });
                                ctx.ExecuteStoreCommand("update wfvcommand set revisionuntil=null,revisionfrom=:version where befehlszeile=:cmd", pars2.ToArray());
                                continue;
                            }
                        }

                        if (!availCmds.Contains(cmd.befehlszeile) && !dblCheck.ContainsKey(cmd.befehlszeile))//nicht in db und noch nicht hinzugefügt
                        {
                            // new command entry, not available yet
                            WFVCOMMAND ncmd = new WFVCOMMAND();
                            _log.Debug("Add new WFVCOMMAND " + cmd.befehlszeile + " for version " + version);
                            ncmd.BEFEHLSZEILE = cmd.befehlszeile;
                            ncmd.ENTRYTYPE = cmd.entryType;
                            ncmd.CRTDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(DateTime.Now);
                            ncmd.CRTTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                            ncmd.REVISIONFROM = version;
                            ncmd.WEBFLAG = 1;
                            dblCheck[cmd.befehlszeile] = "1";
                            ctx.AddToWFVCOMMAND(ncmd);
                        }

                        
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _log.Error("Error updating wfvcommands", e);
            }

        }


        /// <summary>
        /// returns a list of all webvlms
        /// </summary>
        /// <returns></returns>
        public List<VlmConfDto> getVlmList()
        {
            if (!vlmCache.ContainsKey("VLM"))
            {
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<VlmConfDto> rval = ctx.ExecuteStoreQuery<VlmConfDto>(QUERYVLM, null).ToList();

                    foreach (VlmConfDto vlm in rval)
                    {
                        vlm.currencies = ctx.ExecuteStoreQuery<WaehrungDto>(QUERYCURRENCY, null).ToList();
                        vlm.languages = ctx.ExecuteStoreQuery<LanguageDto>(QUERYLANGUAGES, null).ToList();
                        vlm.wfsys = ctx.ExecuteStoreQuery<WfsysDto>(QUERYWFSYS, null).FirstOrDefault();
                        vlm.cicconf = ctx.ExecuteStoreQuery<CicconfDto>(QUERYCICCONF, null).FirstOrDefault();

                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = vlm.code });
                        try
                        {
                            vlm.tables = ctx.ExecuteStoreQuery<VlmTableDto>(QUERYTABLES, pars.ToArray()).ToList();
                            if (vlm.tables == null || vlm.tables.Count == 0)
                                throw new Exception("autofill");
                        }
                        catch (Exception)
                        {
                            WfvConfig loadedCfg = getWfvConfig();
                            //all configs for this vlm
                            var allforvlm = (from v in loadedCfg.vlmtables
                                             where v.vlmcode != null && v.vlmcode.Equals(vlm.code)
                                             select v).ToList();
                            //all configs at all
                            vlm.tables = (from v in loadedCfg.vlmtables
                                          where v.vlmcode == null
                                          select v).ToList();
                            foreach(VlmTableDto vt in allforvlm)
                            {
                                VlmTableDto vtr = (from z in vlm.tables
                                                   where z.syscode == vt.syscode
                                                   select z).FirstOrDefault();
                                //remove config available already for the same syscode
                                if(vtr!=null)
                                    vlm.tables.Remove(vtr);
                                //add vlm specific entry for the syscode
                                vlm.tables.Add(vt);
                            }
                           
                        }
                    }

                    vlmCache["VLM"] = rval;
                }
            }
            return vlmCache["VLM"];
        }

        /// <summary>
        /// Delivers a DB based wfvEntry from the syscode or NULL
        /// </summary>
        /// <param name="syscode"></param>
        /// <returns></returns>
        public static WfvEntry getWfvConfig(String syscode)
        {
            try
            {

                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscode", Value = syscode });


                    WfvTmpEntry entry = ctx.ExecuteStoreQuery<WfvTmpEntry>(QUERYWFVCONFIG, pars.ToArray()).FirstOrDefault();
                    entry.einrichtung = entry.einrichtung.Replace("&apos;", "\'");
                    //remove old clob content after new content
                    if (entry.einrichtung.IndexOf('\0') > 0)
                    {
                        entry.einrichtung = entry.einrichtung.Substring(0, entry.einrichtung.IndexOf('\0'));
                    }

                    pars.Clear();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "befehlszeile", Value = entry.befehlszeile });
                    entry.entrytype = ctx.ExecuteStoreQuery<int>("select entrytype from wfvcommand where befehlszeile=:befehlszeile", pars.ToArray()).FirstOrDefault();

                    if (syscode.IndexOf("detail") > -1)
                        entry.entrytype = 1;

                    if (entry == null) return null;
                    return entry.getWfvEntry();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// returns the bos revision number
        /// </summary>
        /// <returns></returns>
        public static String getVersion()
        {
            Type t = Activator.CreateInstance("Cic.One.Web.Service", "Cic.One.Web.Service.VersionHandle").Unwrap().GetType();
            return System.Reflection.Assembly.GetAssembly(t).GetName().Version.ToString().Split('.')[3];
        }

        /// <summary>
        /// delivers vlm config
        /// 
        /// loads static wfvconfig.dll
        /// inserts or updates the settings in wfv with prefix TPL_
        /// returns all entries from db
        /// </summary>
        /// <param name="vlmid"></param>
        /// <returns></returns>
        public List<WfvEntry> getVlmConfig(String vlmid)
        {
            String version = getVersion();
            if ("0".Equals(version))
            {
                version = "99999";//developer always up to date
                dbsynced = true;
            }

            List<WfvEntry> rvals = new List<WfvEntry>();
            try
            {

                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vers", Value = version });


                    List<WfvTmpEntry> entries = ctx.ExecuteStoreQuery<WfvTmpEntry>(QUERYWFV, pars.ToArray()).ToList();
                    //...build WfvEntry from Database...
                    foreach (WfvTmpEntry tmp in entries)
                    {
                        tmp.einrichtung = tmp.einrichtung.Replace("&apos;", "\'");
                        //remove old clob content after new content
                        if (tmp.einrichtung.IndexOf('\0') > 0)
                        {
                            tmp.einrichtung = tmp.einrichtung.Substring(0, tmp.einrichtung.IndexOf('\0'));
                        }
                        try
                        {
                            rvals.Add(tmp.getWfvEntry());
                        }
                        catch (Exception e)
                        {
                            _log.Error("Parsing WFV " + tmp.syscode + " failed: " + e.Message + " xml: " + tmp.einrichtung);
                        }
                    }

                }
            }
            catch (Exception de)
            {
                _log.Error("WFV-Loading from DB not possible", de);
            }
            //FileUtils.saveFile("C:\\temp\\wfvconfig.xml", data);

            ogetVlmConfigDto rval = new ogetVlmConfigDto();
            WfvConfig loadedCfg = getWfvConfig();

            if (!dbsynced)//synchronize the wfvconfig once with db upon startup
            {
                Dictionary<String, WFV> saved = new Dictionary<String, WFV>();
                using (DdOwExtended ctx = new DdOwExtended())
                {


                    foreach (WfvConfigEntry entry in loadedCfg.configentries)
                    {
                        WFV te = (from p in ctx.WFV
                                  where p.SYSCODE.Equals(entry.syscode)
                                  select p).FirstOrDefault();
                        //wfventry nicht in db -> 
                        //wfventry anlegen
                        if (te == null)
                        {
                            if (saved.ContainsKey(entry.syscode))
                            {
                                _log.Debug("SYSCODE MULTIPLE TIMES IN wfvconfig: " + entry.syscode);
                                continue;
                            }
                            WFV wfv = new WFV();
                            wfv.SYSCODE = entry.syscode;
                            wfv.BEFEHLSZEILE = entry.befehlszeile;
                            wfv.EINRICHTUNG = entry.einrichtung;
                            wfv.BESCHREIBUNG = entry.beschreibung;
                            wfv.KURZBEZ = "AUTO-TEMPLATE, DONT CHANGE";
                            wfv.TYP = 1;
                            ctx.AddToWFV(wfv);
                            saved[entry.syscode] = wfv;
                        }
                        else//update wfventry
                        {
                            te.SYSCODE = entry.syscode;
                            te.BEFEHLSZEILE = entry.befehlszeile;
                            te.BESCHREIBUNG = entry.beschreibung;
                            te.KURZBEZ = "AUTO-TEMPLATE, DONT CHANGE";
                            te.EINRICHTUNG = entry.einrichtung;
                        }
                    }


                    //static wfv 
                    foreach (WfvEntry entry in loadedCfg.entries)
                    {
                        WFV te = (from p in ctx.WFV
                                  where p.SYSCODE.Equals(entry.syscode)
                                  select p).FirstOrDefault();
                        //wfventry nicht in db -> 
                        //wfventry anlegen
                        if (te == null)
                        {
                            if (saved.ContainsKey(entry.syscode))
                            {
                                _log.Debug("SYSCODE MULTIPLE TIMES IN wfvconfig: " + entry.syscode);
                                continue;
                            }
                            WFV wfv = new WFV();
                            wfv.SYSCODE = entry.syscode;
                            wfv.BEFEHLSZEILE = entry.befehlszeile;
                            if (entry.customentry != null)
                            {
                                wfv.BESCHREIBUNG = entry.customentry.title;
                            }
                            wfv.KURZBEZ = "AUTO-TEMPLATE, DONT CHANGE";
                            wfv.ART = "P";
                            wfv.EINRICHTUNG = System.Text.Encoding.UTF8.GetString(XMLSerializer.objectToXml(entry, ENCODING));
                            wfv.TYP = 1;
                            ctx.AddToWFV(wfv);
                            saved[entry.syscode] = wfv;
                        }
                        else//update wfventry
                        {
                            te.SYSCODE = entry.syscode;
                            te.BEFEHLSZEILE = entry.befehlszeile;
                            if (entry.customentry != null)
                            {
                                te.BESCHREIBUNG = entry.customentry.title;
                            }
                            te.KURZBEZ = "AUTO-TEMPLATE, DONT CHANGE";
                            te.EINRICHTUNG = System.Text.Encoding.UTF8.GetString(XMLSerializer.objectToXml(entry, ENCODING));
                        }



                    }
                    ctx.SaveChanges();
                }
                dbsynced = true;
            }


            //add all entries from static wfv to result which where not found in database
            List<WfvEntry> staticLoaded = new List<WfvEntry>();
            foreach (WfvEntry entry in loadedCfg.entries)
            {
                WfvEntry test = rvals.Where(a => a.syscode.Equals(entry.syscode)).FirstOrDefault();
                if (test == null)
                    staticLoaded.Add(entry);
            }
            rvals.AddRange(staticLoaded);

            addWFListQueries(rvals);


            return rvals;
        }

        /// <summary>
        /// Convert all WFFILE Clarion Query configurations to a CIC One GVIEW Config
        /// </summary>
        /// <param name="results"></param>
        private void addWFListQueries(List<WfvEntry> results)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {

                List<WFListInfo> definitions = ctx.ExecuteStoreQuery<WFListInfo>("select description config, bezeichnung id from wflist where (art=1 or art=2)  and description is not null and length(trim(description))>50", null).ToList();
                foreach(WFListInfo wfInfo in definitions)
                {
                    try
                    {


                        WfvEntry listEntry = fillWfvEntryFromWFList(wfInfo, "gview_liste","",0);
                        results.Add(listEntry);
                        wfvEntryCache[listEntry.syscode] = listEntry;//register for searchdao


                        WfvEntry detailEntry = fillWfvEntryFromWFList(wfInfo, "gview_detail","_detail",1);
                        results.Add(detailEntry);
                        wfvEntryCache[detailEntry.syscode] = detailEntry;//register for searchdao







                    }
                    catch(Exception e)
                    {
                        _log.Info("Unsupported WFLIST-Einrichtung: " + wfInfo.id + ": " + e.Message);
                        //do not use the entry
                    }

                }
            }
        }

        private WfvEntry fillWfvEntryFromWFList(WFListInfo wfInfo, String befehlszeile, String syscodeSuffix, int entrytype)
        {
            einrichtung er = XMLDeserializer.objectFromXml<einrichtung>(Encoding.UTF8.GetBytes(wfInfo.config), "UTF-8");
            WfvEntry entry = new WfvEntry();
            entry.syscode = wfInfo.id.ToLower()+ syscodeSuffix;
            entry.syscode = entry.syscode.Replace(":", "_");
            entry.entrytype = entrytype;
            entry.befehlszeile = befehlszeile;
            entry.customentry = new CustomEntry();
            entry.customentry.title = er.grp.titel;
            if (entry.customentry.title == null)
                entry.customentry.title = "Unnamed";
            if (entry.customentry.title.IndexOf("'") == 0)
                entry.customentry.title = entry.customentry.title.Replace("'", "");

            entry.customentry.instantSearch = 1;
            entry.customentry.internalfilter = "1=1";
            entry.customentry.searchmode = SearchMode.NoCount;// RowNum;
            entry.customentry.viewmeta = new ViewMeta();

            if (er.grp.absteigend > 0)
                entry.customentry.sortorder = "DESC";
            else
                entry.customentry.sortorder = "ASC";
            entry.customentry.viewmeta.query = new Query();
            entry.customentry.viewmeta.query.query = er.grp.query;
            entry.customentry.viewmeta.query.expressions = new List<String>();
            entry.customentry.detailsyscode = er.grp.aktionedit;
            entry.customentry.createsyscode = er.grp.aktioninsert;

            if (er.grp.idausdruck != null&& er.grp.idausdruck.Length>7)
                entry.customentry.viewmeta.query.pkey = er.grp.idausdruck.Substring(7);

            if (er.grp.p1 != null)
                entry.customentry.viewmeta.query.expressions.Add(er.grp.p1);
            if (er.grp.p2 != null)
                entry.customentry.viewmeta.query.expressions.Add(er.grp.p2);
            if (er.grp.p3 != null)
                entry.customentry.viewmeta.query.expressions.Add(er.grp.p3);
            if (er.grp.p4 != null)
                entry.customentry.viewmeta.query.expressions.Add(er.grp.p4);
            if (er.grp.p5 != null)
                entry.customentry.viewmeta.query.expressions.Add(er.grp.p5);
            if (er.grp.p6 != null)
                entry.customentry.viewmeta.query.expressions.Add(er.grp.p6);

            /*if (entry.customentry.viewmeta.query.query.ToLower().IndexOf("where") > -1)
                entry.customentry.viewmeta.query.query += " {FILTERCONDITIONS} ";*/


            entry.customentry.viewmeta.fields = new List<Viewfield>();
            entry.customentry.filters = new List<Filter>();
            int idx = 0;
            List<String> filters = new List<string>();
            foreach (einrichtungQ1rec rec in er.q1)
            {

                Viewfield vf = new Viewfield();
                vf.id = "" + rec.rang;
                vf.attr = new ViewFieldAttributes();
                vf.attr.field = rec.ausdruck.Substring(7);
                filters.Add(vf.attr.field);//Every column is a searchfilter
                vf.attr.label = rec.bezeichnung;
                if (er.grp.deftab == idx)
                {
                    entry.customentry.sortfields = vf.attr.field;
                }

                idx++;

                Filter filt = new Filter();
                filt.fieldname = vf.attr.field;
                filt.description = vf.attr.label;

                if (rec.columntype.Equals("String"))
                {
                    vf.attr.type = "String";
                    vf.attr.viewtype = "text";
                    filt.filterType = FilterType.Like;
                    filt.valueType = FilterValueType.STRING;
                }
                else if (rec.columntype.Equals("Number"))
                {
                    vf.attr.type = "Double";
                    vf.attr.viewtype = "number";
                    vf.attr.pattern = "#,##0";
                    filt.filterType = FilterType.BETWEENNUMBER;
                    filt.valueType = FilterValueType.DOUBLE;
                    filt.fieldType = FilterFieldType.BETWEENNUMBER;
                    filt.groupName = vf.attr.field;
                }
                else if (rec.columntype.Equals("Decimal"))
                {
                    vf.attr.type = "Double";
                    vf.attr.viewtype = "currency";
                    vf.attr.pattern = "#,##0.00";
                    filt.filterType = FilterType.BETWEENNUMBER;
                    filt.valueType = FilterValueType.DOUBLE;
                    filt.fieldType = FilterFieldType.BETWEENNUMBER;
                    filt.groupName = vf.attr.field;
                }
                else if (rec.columntype.Equals("Date"))
                {
                    vf.attr.type = "DateTime";
                    vf.attr.viewtype = "date";
                    filt.filterType = FilterType.BETWEEN;
                    filt.fieldType = FilterFieldType.BETWEEN;
                    filt.valueType = FilterValueType.DATE;
                    filt.groupName = vf.attr.field;
                }
                else
                {
                    _log.Warn("Unsupported WFLIST-Type: " + rec.columntype);
                    break;
                }
                entry.customentry.viewmeta.fields.Add(vf);


                entry.customentry.filters.Add(filt);

            }
            entry.customentry.filterfields = String.Join(",", filters);
            return entry;
        }
        class WFListInfo
        {
            public String config { get; set; }
            public String id { get; set; }
        }

        /// <summary>
        /// Load the wfvconfig file from assembly location
        /// </summary>
        /// <returns></returns>
        public static WfvConfig getWfvConfig()
        {
            if (config == null)
            {
                byte[] data = null;
                try
                {
                    data = FileUtils.loadData(FileUtils.getCurrentPath() + PATHWFV);
                }
                catch (Exception)
                {
                    try
                    {
                        data = FileUtils.loadData(FileUtils.getCurrentPath() + PATHWFVDEV);
                    }
                    catch (Exception)
                    {
                        data = FileUtils.loadData(FileUtils.getCurrentPath() + PATHWFVDEV2);
                    }

                }

                config=XMLDeserializer.objectFromXml<WfvConfig>(data, ENCODING);
            }
            return config;
        }

        /// <summary>
        /// Returns the customer config for this webservice instance as configured in the cfg SETUP.NET/CONFIG/CUSTOMER
        /// </summary>
        /// <returns></returns>
        public static CustomerConfig getCustomerConfig()
        {
            String customer = AppConfig.Instance.GetCfgEntry("SETUP.NET", "CONFIG", "CUSTOMER", "BANKNOW");
            WfvConfig config = WFVDao.getWfvConfig();
            List<CustomerConfig> cconfigs = config.customerconfigs;
            if (cconfigs == null || cconfigs.Count == 0) return null;
            return (from f in cconfigs
                    where f.customer.Equals(customer)
                    select f).FirstOrDefault();
        }

        /// <summary>
        /// Load all Menus and Toolbars for the vlm
        /// </summary>
        /// <param name="vlmid"></param>
        /// <returns></returns>
        public List<VlmMenuDto> getMenus(String vlmid)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {

                //first all menus
                int allItemCount = 0;
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vlmcode", Value = vlmid });
                List<VlmMenuDto> rval = ctx.ExecuteStoreQuery<VlmMenuDto>(QUERYMENU, pars.ToArray()).ToList();
                if (rval == null) rval = new List<VlmMenuDto>();
                allItemCount += rval.Count;
                foreach (VlmMenuDto menu in rval)
                {
                    pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvlmmenu", Value = menu.sysvlmmenu });
                    menu.art = MenuArt.MENU;
                    menu.text = menu.text.Replace("&", "");
                    menu.items = new List<VlmMenuItemDto>();
                    Dictionary<long, VlmMenuItemDto> keyItemDict = new Dictionary<long, VlmMenuItemDto>();
                    List<VlmMenuItemDto> allitems = ctx.ExecuteStoreQuery<VlmMenuItemDto>(QUERYITEM, pars.ToArray()).ToList();
                    foreach (VlmMenuItemDto mitem in allitems)
                    {
                        keyItemDict[mitem.sysvlmmitem] = mitem;
                        mitem.text = mitem.text.Replace("&", "");
                    }
                    foreach (VlmMenuItemDto mitem in allitems)
                    {
                        if (mitem.sysvlmmparent == 0)
                        {
                            menu.items.Add(mitem);
                            allItemCount++;
                        }
                        else if (!keyItemDict.ContainsKey(mitem.sysvlmmitem))
                        {
                            continue;
                        }
                        else
                        {
                            if (keyItemDict[mitem.sysvlmmparent].items == null)
                                keyItemDict[mitem.sysvlmmparent].items = new List<VlmMenuItemDto>();
                            keyItemDict[mitem.sysvlmmparent].items.Add(mitem);
                            allItemCount++;
                        }
                    }
                }

                pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vlmcode", Value = vlmid });
                List<VlmMenuDto> toolbars = ctx.ExecuteStoreQuery<VlmMenuDto>(QUERYTOOLBAR, pars.ToArray()).ToList();

                //fetch all toolbaritems for all toolbars, MISUSE sysvlmmparent as sysvlmtool because its not needed in this case
                pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vlmcode", Value = vlmid });
                List<VlmMenuItemDto> toolbarItems = ctx.ExecuteStoreQuery<VlmMenuItemDto>(QUERYTOOLBARITEMS, pars.ToArray()).ToList();

                foreach (VlmMenuDto menu in toolbars)
                {

                    menu.art = MenuArt.TOOLBAR;
                    menu.text = menu.text.Replace("&", "");
                    menu.items = new List<VlmMenuItemDto>();
                    VlmMenuDto lastHeader = menu;
                    bool hasHeader = false;
                    IQueryable<VlmMenuItemDto> allitems = (from a in toolbarItems
                                                           where a.sysvlmmparent == menu.sysvlmmenu
                                                           select a).AsQueryable();


                    foreach (VlmMenuItemDto mitem in allitems)
                    {
                        mitem.text = mitem.text == null ? mitem.tip : mitem.text;

                        if (mitem.text == null) mitem.text = mitem.icon;
                        if (mitem.text != null)
                            mitem.text = mitem.text.Replace("&", "");

                        if (mitem.header > 0)
                        {
                            lastHeader = new VlmMenuDto();
                            lastHeader.art = MenuArt.TOOLBAR;
                            lastHeader.code = menu.code;
                            lastHeader.items = new List<VlmMenuItemDto>();
                            lastHeader.text = mitem.text;
                            rval.Add(lastHeader);
                            allItemCount++;
                            hasHeader = true;
                        }
                        else
                        {
                            lastHeader.items.Add(mitem);
                            allItemCount++;
                        }
                    }
                    if (!hasHeader)
                    {
                        rval.Add(menu);
                        allItemCount++;
                    }
                }
                _log.Debug("Loaded " + allItemCount + " Menu/Toolbaritems for " + vlmid);

                allItemCount = 0;
                pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vlmcode", Value = vlmid });
                List<VlmMenuDto> contextmenus = ctx.ExecuteStoreQuery<VlmMenuDto>(QUERYCONTEXTMENU, pars.ToArray()).ToList();

                //fetch all contextmenuItems for all contextmenus, MISUSE sysvlmmparent as sysvlmpf because its not needed in this case
                pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vlmcode", Value = vlmid });
                List<VlmMenuItemDto> contextmenuItems = ctx.ExecuteStoreQuery<VlmMenuItemDto>(QUERYCONTEXTMENUITEMS, pars.ToArray()).ToList();

                if (contextmenuItems != null)
                    foreach (VlmMenuDto menu in contextmenus)
                    {

                        menu.art = MenuArt.CONTEXTMENU;
                        if (menu.text == null) menu.text = "";
                        menu.text = menu.text.Replace("&", "");
                        menu.items = new List<VlmMenuItemDto>();
                        VlmMenuDto lastHeader = menu;
                        bool hasHeader = false;
                        IQueryable<VlmMenuItemDto> allitems = (from a in contextmenuItems
                                                               where a.sysvlmmparent == menu.sysvlmmenu
                                                               select a).AsQueryable();

                        if (allitems != null)
                            foreach (VlmMenuItemDto mitem in allitems)
                            {
                                mitem.text = mitem.text == null ? mitem.tip : mitem.text;

                                if (mitem.text == null) mitem.text = mitem.icon;
                                if (mitem.text != null)
                                    mitem.text = mitem.text.Replace("&", "");

                                if (mitem.header > 0)
                                {
                                    lastHeader = new VlmMenuDto();
                                    lastHeader.art = MenuArt.CONTEXTMENU;
                                    lastHeader.code = menu.code;
                                    lastHeader.items = new List<VlmMenuItemDto>();
                                    lastHeader.text = mitem.text;
                                    rval.Add(lastHeader);
                                    allItemCount++;
                                    hasHeader = true;
                                }
                                else
                                {
                                    lastHeader.items.Add(mitem);
                                    allItemCount++;
                                }
                            }
                        if (!hasHeader)
                        {
                            rval.Add(menu);
                            allItemCount++;
                        }
                    }
                _log.Debug("Loaded " + allItemCount + " Contextmenus/Contextmenuitems for " + vlmid);
                return rval;
            }
        }
    }
}