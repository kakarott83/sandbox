using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Web;
using AutoMapper;
using Cic.One.DTO;
using Cic.One.GateWKT.BO;
using Cic.One.Web.BO;
using Cic.One.Web.BO.Search;
using Cic.OpenLeaseAuskunftManagement.DTO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Extension;
using System.Linq;
using ILog = Cic.OpenOne.Common.Util.Logging.ILog;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.Util.Behaviour;
using Cic.OpenOne.Common.Util;
using System.IO;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;

using CIC.Monitoring;
using CIC.Monitoring.Services;
using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service;
using CIC.Monitoring.Builders;
using Cic.OpenOne.Common.Util.Security;
using System.Timers;
using CIC.ASS.Common.BO;
using Cic.OpenOne.Common.Model.DdOl;
using System.Data.Metadata.Edm;
using System.Collections.Generic;
using CIC.Database.OL.EF4.Model;

namespace Cic.One.Web.Service
{
    /// <summary>
    ///     Dummyclass for reflection access to this assemblys'Type
    /// </summary>
    public class VersionHandle
    {
    }

   
    /// <summary>
    ///     Global
    /// </summary>
    public class Global : HttpApplication
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly String APPNAME = "Cic.One.Web.Service";
        private ProcessUpdater processUpdater;
        
        public static MonitoringConfiguration MonitoringConfiguration { get; private set; }
        public static IMonitoringService MonitoringService { get; private set; }
        private Timer cutimer = new Timer();

        public static string GetFromWebconfig(string Name)
        {
            var config = ConfigurationManager.GetSection("applicationSettings/Cic.OpenOne.Common.Properties.Config");

            if (config != null)
            {
                if (((ClientSettingsSection) config).Settings.Get(Name) != null)
                    return ((ClientSettingsSection) config).Settings.Get(Name).Value.ValueXml.InnerText;
            }

            return string.Empty;
        }

        public static String GetAssemblyNameContainingType(String typeName)
        {
            foreach (var currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                _log.Debug("Search in " + currentassembly.FullName);
                if (currentassembly.FullName.ToUpper().StartsWith("CIC"))
                {
                    var t = currentassembly.GetType(typeName, false, true);
                    if (t != null)
                    {
                        return currentassembly.FullName;
                    }
                }
            }

            return "not found";
        }

        public static Object getInstance(String classname)
        {
            var asm = GetAssemblyNameContainingType(classname);
            if ("not found".Equals(asm))
            {
                _log.Error("Could not find class " + classname);
                return null;
            }
            //Type t = Type.GetType(searchfactory+", "+asm);
            //Object test1 = t.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var oh = Activator.CreateInstance(asm, classname);
            return oh.Unwrap();
        }

        /// <summary>
        ///     Application Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs evt)
        {
            _log.Info("Deploying " + APPNAME + "...");

           
            try
            {
                Cic.OpenOne.Common.Util.Config.Configuration.DeliverOpenLeaseConnectionString();
            }
            catch (Exception exc)
            {
                _log.Fatal("Start Failed because of invalid Database-Connection!", exc);
                throw new Exception("Start Failed because of invalid Database-Connection!", exc);
            }
            
            try
            {

                
                //Handler for recording SOAPs for the TestRunner
                Cic.OpenOne.Common.Util.Extension.SoapLoggingExtension.addSoapMessageHandler(new SoapTestMessageHandler());

                //Allow rewriting the default search query structure and test it
                var queryPattern = GetFromWebconfig("QueryInfoDataType1");
                _log.Debug("QueryInfoDataType1: " + queryPattern + " Default: " + QueryInfoDataType1.partialQueryDefault);
                if (!String.IsNullOrEmpty(queryPattern))
                {
                    QueryInfoDataType1.overridePartialQuery = queryPattern;
                }
                bool qerr = true;
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    object[] pars = new object[] { "sysvlmconf", "CIC.VLMCONF", "1=1", "1", "0","","" };
                    if (QueryInfoDataType1.overridePartialQuery != null) //check if this partial query is usable
                    {
                      String testQuery1= String.Format(QueryInfoDataType1.overridePartialQuery, pars);
                        try
                        {
                            long t = ctx.ExecuteStoreQuery<long>(testQuery1, null).FirstOrDefault();
                            qerr = false;
                        }catch(Exception qe)
                        {
                            QueryInfoDataType1.overridePartialQuery = null;//Fallback to default
                            _log.Error("Configured PartialQuery from QueryInfoDataType1 not usable, using default. Reason: " + qe.Message);
                        }
                    }
                    if(qerr)
                    {
                        String testQuery1 = String.Format(QueryInfoDataType1.partialQueryDefault, pars);
                        try
                        {
                            long t = ctx.ExecuteStoreQuery<long>(testQuery1, null).FirstOrDefault();
                            qerr = false;
                        }
                        catch (Exception qe)
                        {
                            QueryInfoDataType1.overridePartialQuery = QueryInfoDataType1.partialQueryDefault2;
                            _log.Error("PartialQuery for Oracle12 not usable, using default. Reason: " + qe.Message);
                        }
                    }
                    
                }


                

                //Allow oracle session commands from web config, comma separated
                var sessionSettings = GetFromWebconfig("SessionSettings");
                _log.Debug("SessionSettings: " + sessionSettings);
                Cic.One.Web.BO.StateServiceBo.addSanityCheckInfo(new SanityCheckInfoDto("Database Session Commands", sessionSettings, "web.config <applicationSettings><Cic.OpenOne.Common.Properties.Config><setting name='SessionSettings'>", "change web.config", true));
                
                if (!String.IsNullOrEmpty(sessionSettings))
                {
                    var dbfixes = sessionSettings.Split(',');
                        // new String[] { "alter session set \"_complex_view_merging\"=false", "alter session set \"_optimizer_connect_by_cost_based\"=false" };
                    
                    //Legacy Fixes for WKT:
                    //Fix ORA-00600: internal error code, arguments: [qctcte1], [0], [], [], [], [], [], [] on Oracle 10
                    //Cic.OpenOne.Common.Model.DdOw.DdOwExtended.addSessionCommand("alter session set \"_optimizer_cost_based_transformation\"=off");
                    //Cic.OpenOne.Common.Model.DdOw.DdOwExtended.addSessionCommand("alter session set \"_optimizer_push_pred_cost_based\"=false");
                    foreach (var s in dbfixes)
                    {
                        _log.Error("Setting DB Session Fix " + s);
                        Cic.OpenOne.Common.Util.Extension.EFContextExtension.AddSessionCommand(s);
                    }
                }
                
                //Initialize devart and ssl
                new ServiceSetup();
              

                //internal tests
                try
                {
                    performTests();
                }
                catch (Exception ex)
                {
                    _log.Warn("Internal Tests failed with " + ex.Message, ex);
                }

                //Check external services
                MonitoringConfigurationBuilder monitor = MonitoringCheck.Initialize();
                monitor.MonitorDependency("Crefo", builder => builder.CheckService(new Cic.OpenLeaseAuskunftManagement.CrefoService.CtoMessagesClient()));
                monitor.MonitorDependency("Guardean", builder => builder.CheckService(new Cic.One.Web.GateGuardean.WorkflowEnginePortTypeClient()));

                MonitoringCheck.InitializeGateBANKNOWServices(monitor);
                MonitoringCheck.Register(monitor);

                //Force wkt-load of assembly
                var wkt = new WktBO();

                //force banknow load
                Cic.One.GateBANKNOW.BO.Search.XproInfoFactory.getInstance();

                //initialize the Factory used for this deployment - important setting! 
                var bofactory = GetFromWebconfig("BOFactoryFactory");

                //Setup the correct BO factory
                Cic.One.Web.BO.StateServiceBo.addSanityCheckInfo(new SanityCheckInfoDto("BO Factory",bofactory,"web.config <applicationSettings><Cic.OpenOne.Common.Properties.Config><setting name='BOFactoryFactory'>","change web.config",bofactory!=null));
                if (!String.IsNullOrEmpty(bofactory))
                {
                    try
                    {
                        _log.Info("Setting BOFactoryFactory to " + bofactory);
                        BOFactoryFactory.setFactory((IBOFactory) getInstance(bofactory));
                    }
                    catch (Exception e)
                    {
                        _log.Error("Failed setting configured BOFactoryFactory", e);
                    }
                }

                //Setup the correct Search Query factory
                var searchfactory = GetFromWebconfig("SearchQueryFactoryFactory");
                Cic.One.Web.BO.StateServiceBo.addSanityCheckInfo(new SanityCheckInfoDto("SearchQuery Factory", sessionSettings, "web.config <applicationSettings><Cic.OpenOne.Common.Properties.Config><setting name='SearchQueryFactoryFactory'>", "change web.config", true));
                if (!String.IsNullOrEmpty(searchfactory))
                {
                    try
                    {
                        _log.Info("Setting SearchQueryFactoryFactory to " + searchfactory);
                        SearchQueryFactoryFactory.setFactory((ISearchQueryInfoFactory) getInstance(searchfactory));
                    }
                    catch (Exception e)
                    {
                        _log.Error("Failed setting configured SearchQueryFactoryFactory", e);
                    }
                }


                //Setup Automapper
                // Actung! Das ist Sicherheitsrelevant. Und darf NUR im Debugfall kurzfristig aktiviert werden. Für Produktive Releases muss dieser Teil IMMER auskommentiert bleiben.
                //_log.Info("Database-Connection: " + Cic.OpenOne.Common.Util.Config.Configuration.DeliverOpenLeaseConnectionString());
                Mapper.AddProfile<MappingProfileOne>();
                Mapper.AddProfile<MappingProfileMail>();
                Mapper.AddProfile<AuskunfCrefoModelProfile>();
                Mapper.AddProfile<AuskunftModelSchufaProfile>();
                Mapper.AddProfile<Cic.OpenOne.GateBANKNOW.Service.DTO.BankNowModelProfileServices>();

                //Flag for using the tls continue option for SSL
                var tls = GetFromWebconfig("SecurityProtocolTls");
                if (tls != null && "TRUE".Equals(tls.ToUpper()))
                {
                    System.Net.ServicePointManager.Expect100Continue = true;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    _log.Debug("Using SecurityProtocol TLS only!");
                }

               
                //Initialize XPRO and Lucene Settings
                try
                {
                    String version = WFVDao.getVersion();
                    if (!"0".Equals(version))
                    {   //non-develop version will preload configured xproinfofactory
                        BOFactoryFactory.getInstance().getXproSearchBo();
                    }
                    
                    var ad = new AppSettingsDao();
                    var input = new icreateOrUpdateAppSettingsItemDto();
                    input.regVar = new RegVarDto();
                    input.regVar.area = "PRELOAD";
                    input.regVar.completePath = RegVarPaths.getInstance().LUCENE + "PRELOAD";
                    input.regVar.code = "PRELOAD";
                    input.regVar.wert = "0";
                    input.regVar.syswfuser = -1;
                    input.regVar.sysid = -1; //needed!
                    input.sysWfuser = -1;

                    ad.createOrUpdateAppSettingsItem(input);
                }
                catch (Exception e)
                {
                    _log.Error("Failed preload EF", e);
                    Cic.One.Web.BO.StateServiceBo.addSanityCheckInfo(new SanityCheckInfoDto("Preloading EntityFramework",  e.Message,"check Database configuration and version"));
                }
                if (LuceneFactory.getInstance().getIndexUpdateInterval() > 0)
                {
                    Timer timer = new Timer();
                    timer.Elapsed += new ElapsedEventHandler(updateIndex);
                    timer.Start();//start immediately
                 
                }
                else
                {
                    _log.Debug("Lucene disabled - UpdateInterval <= 0");
                }

                //Entity Framework warmup by doing some operations with it
                _log.Debug("EF warmup...");
                using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
                {
                    CIC.Database.OL.EF4.Model.CICCONF cicconf = ctx.ExecuteStoreQuery<CIC.Database.OL.EF4.Model.CICCONF>("select * from cicconf").FirstOrDefault();
                    //String v1 = ctx.ExecuteStoreQuery<String>("SELECT NLS_UPPER ('große')  FROM DUAL", null).FirstOrDefault();//für "alter session set \"NLS_SORT\"=GERMAN"; GROßE, für XGERMAN GROSSE
                    
                }
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new OpenOne.Common.Model.DdOw.DdOwExtended())
                {
                    var query = from p in ctx.PUSER
                                where p.EXTERNEID.ToLower().Equals("XXX") && p.KENNWORT.Equals("XXX")
                                select p;
                    query.FirstOrDefault();
                    ctx.ExecuteStoreQuery<Cic.OpenOne.Common.Model.DdOw.WFUSER>("select * from wfuser where syswfuser=1000").FirstOrDefault();
                }
                _log.Debug("EF warmup finished");


                //set a function for fetching the cache-lifetime into the cachemanager
                CacheManager.getInstance().setConfigAction(new Func<CacheCategory, long>(delegate(CacheCategory cat)
                {
                    return CacheDao.getInstance().getCacheDuration(cat);
                }));

                //Autoupdate webservice settings by db value change of chgtime
                processUpdater = ProcessUpdater.getInstance();
                processUpdater.updaterFunction = new ProcessUpdater.ProcessUpdaterFunction(delegate(){
                    WFVDao.flushCache();
                });


                
                cutimer.Elapsed += cleanupTasks;
                cutimer.Interval = 60000 * 15;
                cutimer.Start();
                
                
            }
            catch (Exception e)
            {
                _log.Fatal("Startup of application has an error", e);
                
                Cic.One.Web.BO.StateServiceBo.addSanityCheckInfo(new SanityCheckInfoDto("Startup of Webservice",e.Message, "check webservice logfile"));
                throw new Exception("Startup of application has an error", e);
            }
        }

        private void updateIndex(object source, ElapsedEventArgs eArgs)
        {
            try
            {
                ((System.Timers.Timer)source).Stop();
                LuceneBO.getInstance().startIndexer();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Called every 15 min to cleanup things
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void cleanupTasks(object source, ElapsedEventArgs e)
        {
            Cic.One.Workflow.BO.XmlWorkflowInstanceStore.cleanStorage();
        }
        
        private String fieldLengthOut(String prefix,List<KeyValuePair<String, int>> vals)
        {
            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<String, int> kv in vals)
            {
                sb.Append(prefix + "." + kv.Key + "=" + kv.Value + "\n");
            }
            return sb.ToString();
        }
        /// <summary>
        ///     perform some tests
        /// </summary>
        private void performTests()
        {

           /* byte[] data = FileUtils.loadData(@"C:\temp\invoice.xml");
            new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Santander.InvoiceBo().processInvoice(System.Text.Encoding.UTF8.GetString(data));
            data = FileUtils.loadData(@"C:\temp\invoice2.xml");
            new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Santander.InvoiceBo().processInvoice(System.Text.Encoding.UTF8.GetString(data));
            */
            //Write a file with db table fieldlengths

            /* using (DdOlExtended ctx = new DdOlExtended())
             {
                 //int mclen = ctx.Length<CIC.Database.OL.EF4.Model.IT>("MATCHCODE");
                 StringBuilder sb = new StringBuilder();
                 sb.Append(fieldLengthOut("de.cic.one.service.bnpartner.KundeDto",ctx.getLengths<CIC.Database.OL.EF4.Model.PERSON>()));
                 sb.Append(fieldLengthOut("de.cic.one.service.bnpartner.KundeDto", ctx.getLengths<CIC.Database.OL.EF4.Model.PKZ>()));
                 sb.Append(fieldLengthOut("de.cic.one.service.bnpartner.ObjektDto", ctx.getLengths<CIC.Database.OL.EF4.Model.ANTOB>()));
                 sb.Append(fieldLengthOut("de.cic.one.service.bnpartner.ObjektDto", ctx.getLengths<CIC.Database.OL.EF4.Model.ANTOBBRIEF>()));
                 FileUtils.saveFile("C:\temp\fieldlengths.properties", Encoding.ASCII.GetBytes(sb.ToString()));

             }*/

            /*     String[] passwords = new String[]{@"ä!@+,.ÄÖÜ!""",@"""!ÄnderPW123""",@"""TestPW1!Ä@""",
                 "TESTPassW96!",
                 "ÄnderPW@1!",
                 "Pass_Word1!",
                 "Pass_word1",
                 "Asd_qwer1!",
                 "Lhf_trtw1!"};

                 foreach (String s in passwords)
             {
                 try
                 {
                     String encoded = RpwComparator.Encode(s);
                     String decoded = RpwComparator.Decode(encoded);
                     _log.Debug("ORG: " + s + " enc: " + encoded + " dec: " + decoded);
                 }catch(Exception exc)
                 {
                     _log.Error(exc.Message);
                 }
             }*/

        }

        private ClientBase<T> tryCreate<T>(Type t) where T : class
        {
            try
            {
                return (ClientBase<T>)Activator.CreateInstance(t);
            }catch(Exception)
            {
                return null;
            }
            
        }
       

        /// <summary>
        ///     Session Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Application Begin Request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Application Authentication Request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Application Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Session End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Application End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
        }
    }

    /// <summary>
    /// Handles the management of saving new soap-ui request for unit-tests
    /// </summary>
    class SoapTestMessageHandler :ISOAPMessageHandler
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool enabled = true;
        public void messageReplied(System.ServiceModel.Channels.Message msg)
        {
            
        }

        public void messageRequested(System.ServiceModel.Channels.Message msg)
        {
            if (msg == null) return;
            if (!enabled) return;
            String tname=null;
            try
            {
                String to = msg.Headers.To.ToString();
                String action = msg.Headers.Action;


                String path = FileUtils.getCurrentPath() + "\\..\\..\\UnitTest\\resources";
                if(!Directory.Exists(path))
                { 
                    path = FileUtils.getCurrentPath() + "\\..\\UnitTest\\resources";
                    if (!Directory.Exists(path))
                    {
                        path = FileUtils.getCurrentPath() + "\\UnitTest\\resources";
                        if (!Directory.Exists(path))
                        {
                            _log.Info("SOAP-Logging to " + path + " disabled: Folder not found");
                            enabled = false;
                            return;
                        }
                    }
                    
                    //Directory.CreateDirectory(path);
                }
                String[] allFiles = Directory.GetFiles(path, "*.xml");
                //the index of the new file
                int idx = allFiles.Length;
                String nameSuffix = getSuffix(to) + "_" + getSuffix(action) + ".xml";
                String avail = (from tmp in allFiles
                                where tmp.IndexOf(nameSuffix) > -1
                                select tmp).FirstOrDefault();
                //xml already there, skip
                if (avail != null)
                    return;
                String newName = idx + "_" + nameSuffix;
                UTF8Encoding enc = new UTF8Encoding();
                String msgToSave = msg.ToString().Replace(getPrefix(to), "DESTINATION_SERVER");
                tname = path + "\\" + newName;
                FileUtils.saveFile(tname, enc.GetBytes(msgToSave));
            }catch(Exception e)
            {
                _log.Error("Error saving SOAP Test xml to "+tname+": " + e.Message);
            }


        }
        private String getSuffix(String s)
        {
            if (s == null) return "";
            return s.Substring(s.LastIndexOf("/")+1).Replace("?wsdl","").Replace("?singleWsdl","").Replace(".","_").Replace("?","_");
        }
        private String getPrefix(String s)
        {
            return s.Substring(0,s.LastIndexOf("/")+1);
        }



       
    }

    
}