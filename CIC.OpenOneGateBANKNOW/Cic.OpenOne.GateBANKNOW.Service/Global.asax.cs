using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Linq;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Monitoring.Builders;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    using AutoMapper.Configuration;
    using CIC.Database.IC.EF6.Model;
    using Common.DTO.Auskunft.Crif;
    using Common.DTO.Auskunft.Schufa;
    using Devart.Data.Oracle;
    using OpenOne.Common.Model.DdOw;
    using System.Transactions;

    /// <summary>
    /// Global
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static String APPNAME = "Cic.OpenOne.GateBANKNOW.Service";
        private ProcessUpdater processUpdater;

        public static string GetFromWebconfig(string Name)
        {
            var config = ConfigurationManager.GetSection("applicationSettings/Cic.OpenOne.Common.Properties.Config");

            if (config != null)
            {
                if (((ClientSettingsSection)config).Settings.Get(Name) != null)
                    return ((ClientSettingsSection)config).Settings.Get(Name).Value.ValueXml.InnerText;
            }

            return string.Empty;
        }

        public static String GetAssemblyNameContainingType(String typeName)
        {
            foreach (var currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
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
                return null;
            }
            //Type t = Type.GetType(searchfactory+", "+asm);
            //Object test1 = t.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var oh = Activator.CreateInstance(asm, classname);
            return oh.Unwrap();
        }

        /// <summary>
        /// Application Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evt"></param>
        protected void Application_Start(object sender, EventArgs evt)
        {

            // initialize needed global context Variable for Log4Net
            log4net.GlobalContext.Properties["USERNAME"] = "";
            log4net.GlobalContext.Properties["AREA"] = "BANKNOW";
            log4net.ThreadContext.Properties["AREA"] = "BANKNOW";
            //log4net.LogicalThreadContext.Properties["AREA"] = "BANKNOW";

            // Load log4net configuration
            //System.IO.FileInfo logfile = new System.IO.FileInfo("log4net.config");
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(logfile);


            _log.Info("Deploying " + APPNAME + "...");
            try
            {
                Cic.OpenOne.Common.Util.Config.Configuration.DeliverOpenLeaseConnectionString();
            }
            catch(Exception exc)
            {
                _log.Fatal("Start Failed because of invalid Database-Connection!", exc);
                throw new Exception("Start Failed because of invalid Database-Connection!", exc);
            }

           

            try
            {

                Cic.One.Utils.Util.Mapper.MapperConfig.registerBaseMapper(delegate (MapperConfigurationExpression baseMappings)
                {
                    
                    baseMappings.AddProfile<Cic.OpenOne.GateBANKNOW.Service.DTO.BankNowModelProfileServices>();
                    baseMappings.AddProfile<AuskunftModelSchufaProfile>();
                    baseMappings.AddProfile<AuskunftModelCrifProfile>();
                    try
                    {
                        object prof = getInstance("Cic.OpenOne.AuskunftManagement.Common.DTO.AuskunfModelCrefoProfile");
                        if (prof != null)
                        {
                            baseMappings.AddProfile((AutoMapper.Profile)prof);
                            _log.Info("AuskunftManagement-Library installed and used!");
                        }
                        else
                            _log.Warn("AuskunftManagement-Library not installed and used!");
                    }catch(Exception )
                    {

                    }
                });

                new ServiceSetup();
                


                //Check external services
                MonitoringConfigurationBuilder monitor = MonitoringCheck.Initialize();
                MonitoringCheck.InitializeGateBANKNOWServices(monitor);
                MonitoringCheck.Register(monitor);

               

                

                /*
                var tls = GetFromWebconfig("SecurityProtocolTls");
                if (tls != null && "TRUE".Equals(tls.ToUpper()))
                {
                    System.Net.ServicePointManager.Expect100Continue = true;
                    System.Net.ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    _log.Debug("Using SecurityProtocol TLS only!");
                }*/
                //as of .net 4.7 the defaults are the best settings with the strongest sec protocol  choosen by the os


                

                _log.Debug("EF warmup...");
                warmup();
               
                _log.Debug("EF warmup finished");

                performTests();

                //set a function for fetching the cache-lifetime into the cachemanager
                CacheManager.getInstance().setConfigAction( new Func<CacheCategory,long>(delegate(CacheCategory cat){
                    return CacheDao.getInstance().getCacheDuration(cat);
                }));

                processUpdater = ProcessUpdater.getInstance();
                processUpdater.updaterFunction = new ProcessUpdater.ProcessUpdaterFunction(delegate()
                {
                    //do some other flush things here
                    var mon = GetFromWebconfig("DbMonitor");
                    if (mon != null && "TRUE".Equals(mon.ToUpper()))
                    {
                        //SQL Oracle Monitoring: for dbMonitor.exe
                        OracleMonitor om = new OracleMonitor();
                        om.IsActive = true;
                    }
                });
                processUpdater.cyclicFunction = new ProcessUpdater.ProcessUpdaterFunction(delegate () {
                    warmup();
                });

                Cic.One.Utils.Util.Mapper.MapperConfig.waitConfigurationFinished();
                _log.Info("Application ready to serve");
            }
            catch (Exception e)
            {
                _log.Fatal("Startup of application has an error", e);
            }
        }
        private void warmup()
        {
            try
            {

                using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        CIC.Database.OL.EF6.Model.CICCONF cicconf = ctx.ExecuteStoreQuery<CIC.Database.OL.EF6.Model.CICCONF>("select * from cicconf").FirstOrDefault();

                        CIC.Database.OL.EF6.Model.IT testIt = new CIC.Database.OL.EF6.Model.IT();
                        ctx.IT.Add(testIt);
                        testIt.NAME = "WARMUP";
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception excep)
            {
                _log.Warn("Warmup Issue 1: " + excep.Message);
            }
            try { 
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new OpenOne.Common.Model.DdOw.DdOwExtended())
                {
                    var query = from p in ctx.PUSER
                                where p.EXTERNEID.ToLower().Equals("XXX") && p.KENNWORT.Equals("XXX")
                                select p;
                    query.FirstOrDefault();
                    ctx.ExecuteStoreQuery<CIC.Database.OW.EF6.Model.WFUSER>("select * from wfuser where syswfuser=1000").FirstOrDefault();

                }
            }
            catch (Exception excep)
            {
                _log.Warn("Warmup Issue 2: " + excep.Message);
            }
            try { 
            using (Cic.OpenOne.Common.Model.DdIc.DdIcExtended ctx = new OpenOne.Common.Model.DdIc.DdIcExtended())
                {
                    var query = from p in ctx.AUSKUNFTTYP
                                where p.BEZEICHNUNG.ToLower().Equals("XXX")
                                select p;
                    query.FirstOrDefault();
                    ctx.ExecuteStoreQuery<AUSKUNFT>("select * from auskunft where sysauskunft=1").FirstOrDefault();

                }
            }
            catch (Exception excep)
            {
                _log.Warn("Warmup Issue 3: " + excep.Message);
            }
            try { 
            //Warmup Prisma Daos
            OpenOne.Common.BO.Prisma.IPrismaParameterBo paramBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createPrismaParameterBo();
            paramBo.listAvailableParameter(new OpenOne.Common.DTO.Prisma.prKontextDto());
            OpenOne.Common.DAO.Prisma.IPrismaDao prismaDao = OpenOne.Common.DAO.Prisma.PrismaDaoFactory.getInstance().getPrismaDao();
                CIC.Database.PRISMA.EF6.Model.VART vart = prismaDao.getVertragsart(1);
            }
            catch (Exception excep)
            {
                _log.Warn("Warmup Issue 3: " + excep.Message);
            }
        }

       private void performTests()
        {
            try
            {

                //byte[] filedata = FileUtils.loadData("C:\\temp\\eventdata.xml");
                //Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasEaiBOSAdapterFactory.processEventData(filedata);
            }catch(Exception e)
            {
                _log.Error("TESTS failed", e);
            }
        }

        /// <summary>
        /// Session Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Application Begin Request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Application Authentication Request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Application Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Session End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Application End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}