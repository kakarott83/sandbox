using AutoMapper.Configuration;
using Cic.OpenLease.Service.MediatorService;
using Cic.OpenOne.CarConfigurator.VinSearch;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using CIC.Monitoring;
using CIC.Monitoring.Builders;
using CIC.Monitoring.Services;
using Devart.Data.Oracle;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Cic.OpenLease
{
    /// <summary>
    /// Used for Caching in .NET lower 4
    /// </summary>
    public class Global : System.Web.HttpApplication
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static IMonitoringService MonitoringService { get; private set; }
        public static MonitoringConfiguration MonitoringConfiguration { get; private set; }
        private ProcessUpdater processUpdater;

        protected void Application_Start(object sender, EventArgs e)
        {
            _log.Info("Deploying Cic.OpenLease.Service...");
             //Setup Automapper
            Cic.One.Utils.Util.Mapper.MapperConfig.registerBaseMapper(delegate (MapperConfigurationExpression baseMappings)
            {
                baseMappings.AddProfile<Cic.OpenLease.Service.MappingProfile>();
                baseMappings.AddProfile<Cic.OpenOne.GateBANKNOW.Common.DTO.BankNowModelProfile>();
            });

            new ServiceSetup();


            ConfigureMonitoringService();

           

            

            processUpdater = ProcessUpdater.getInstance();
            processUpdater.updaterFunction = new ProcessUpdater.ProcessUpdaterFunction(delegate()
            {
                //do some other flush things here
            });
            // AngebotSubmitDao sd = new AngebotSubmitDao();
            //sd.test(7058);


            Cic.One.Utils.Util.Mapper.MapperConfig.waitConfigurationFinished();
            _log.Info("Application ready to serve");
        }

        private void ConfigureMonitoringService()
        {
            try
            {
                MediatorServiceClient msc = new MediatorServiceClient();
                var modifyHeadersBehavior = new ModifyHeadersBehavior();
                modifyHeadersBehavior.AddHeader("X-CIC-SYSWFUSER", "0");
                ((MediatorServiceClient)msc).Endpoint.Behaviors.Add(modifyHeadersBehavior);

                var configurationBuilder = MonitoringConfiguration.New("BOS-B2B")
                    .CanRunChecksParallel(true)
                    .MonitorSelf(MonitorSelf)
                    .MonitorDependency("BAS", builder => builder.CheckService(msc))
                    .MonitorDependency("Eurotax VIN", builder => builder.CheckService(new vinsearchSoapPortClient()))
                    .MonitorDependency("Eurotax Forecast Document", builder => builder.CheckService(new ForecastPortDocumentClient()))
                    ;

                MonitoringService = configurationBuilder.GetService();
                MonitoringConfiguration = configurationBuilder.Start(string.Format("http://localhost:1399{0}", HttpRuntime.AppDomainAppVirtualPath));
			}
            catch (Exception exception)
            {
                _log.Warn("An exception occured while starting the monitoring service.", exception);
                //throw new Exception("An exception occured while starting the monitoring service.", exception);
            }
        }

        private void MonitorSelf(CheckResultBuilder builder)
        {
            builder.WithVersion(Assembly.GetExecutingAssembly().GetName().Version.ToString());

            using (DdOlExtended ctx = new DdOlExtended())
            {
                DbConnection connection = ctx.getObjectContext().Connection  ;
                if (connection != null)
                {
                    OracleConnection oracleConnection = ((System.Data.Entity.Core.EntityClient.EntityConnection)connection).StoreConnection as OracleConnection;
                    if (oracleConnection != null)
                    {
                        builder.WithDatabaseInfo(oracleConnection.Server,
                            oracleConnection.ServiceName,
                            oracleConnection.Sid,
                            oracleConnection.Port.ToString(),
                            null,
                            oracleConnection.ServerVersion,
                            null,
                            oracleConnection.ClientVersion,
                            oracleConnection.Direct.ToString(),
                            oracleConnection.Home
                        );
                    }
                }

                builder.WithDatabaseInfo(query => ctx.ExecuteStoreQuery<CIC.Monitoring.Model.DatabaseInfo>(query).FirstOrDefault());
            }
        }


        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            _log.Debug("WS App Warning: ", Server.GetLastError());
            _log.Debug("Reason: ", Server.GetLastError().InnerException);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            _log.Info("Undeploying WS...");
        }
    }
}