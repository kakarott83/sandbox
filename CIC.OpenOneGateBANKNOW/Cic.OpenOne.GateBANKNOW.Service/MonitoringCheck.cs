using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST;
using Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DecisionEngineRef;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DeltavistaRef;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DeltavistaRef2;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.KREMORef;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKBatchRef;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.FoodasLoginServiceReference;
using Cic.OpenOne.GateBANKNOW.Common.FoodasServiceReference;
using Cic.OpenOne.GateBANKNOW.Common.GuardeanServiceReference;
using Cic.OpenOne.GateBANKNOW.Common.IBANService;
using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;
using CIC.Monitoring;
using CIC.Monitoring.Builders;
using CIC.Monitoring.Model;
using CIC.Monitoring.Services;
using Devart.Data.Oracle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web;
using ISOcountryType = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ISOcountryType;
using ISOcurrencyType = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcurrencyType;
using ISOlanguageType = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ISOlanguageType;


namespace Cic.OpenOne.GateBANKNOW.Service
{
    using Cic.OpenOne.Common.MediatorService;
    using CIC.Bas.Modules.OpenLeaseCommon.Evaluate;
    using CIC.Database.DE.EF6.Model;
    using Common.CrifSoapService;
    using Common.GuardeanStatusUpdateServiceReference;
    using System.Data.Common;

    /// <summary>
    /// Initializes a Montioring of external services
    /// </summary>
    public class MonitoringCheck
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static MonitoringConfiguration MonitoringConfiguration { get; private set; }
        public static IMonitoringService MonitoringService { get; private set; }

        public static MonitoringConfigurationBuilder Initialize()
        {
            try
            {

                var configurationBuilder = MonitoringConfiguration.New("BOS")
                    .CanRunChecksParallel(true)
                    //.RegisterLoggers(message => _log.Debug(message), exception => _log.Debug("Monitoring Error", exception))
                    .MonitorSelf(MonitorSelf);

                return configurationBuilder;
            }

            catch (Exception exception)
            {
                _log.Warn("An exception occured while starting the monitoring service.", exception);
                //throw new Exception("An exception occured while starting the monitoring service.", exception);
                return null;
            }
        }
        public static MonitoringConfigurationBuilder InitializeGateBANKNOWServices(MonitoringConfigurationBuilder configurationBuilder)
        {
            try
            {
                //.MonitorDependency("Eurotax Forecast RPC", builder =>
                //{
                //    builder.CheckService(new ForecastPortRPCClient());
                //})
                configurationBuilder.MonitorDependency("Eurotax Forecast Document", builder =>
                    {
                        builder.CheckService(new ForecastPortDocumentClient());
                        var eurotaxBo = AuskunftBoFactory.CreateDefaultEurotaxBo();

                        try
                        {
                            EurotaxInDto etin = new EurotaxInDto();
                            etin.CurrentMileageValue = (uint)0;
                            etin.EstimatedAnnualMileageValue = (uint)15000;

                            etin.ForecastPeriodFrom = "" + (36 + 12);
                            etin.ForecastPeriodUntil = etin.ForecastPeriodFrom;
                            etin.NationalVehicleCode = 10133253;
                            etin.RegistrationDate = new DateTime(2012, 05, 01);
                            etin.ISOCountryCode = Common.DAO.Auskunft.EurotaxRef.ISOcountryType.DE;
                            etin.ISOCurrencyCode = Common.DAO.Auskunft.EurotaxRef.ISOcurrencyType.EUR;
                            etin.ISOLanguageCode = Common.DAO.Auskunft.EurotaxRef.ISOlanguageType.DE;
                            etin.TotalListPriceOfEquipment = (double)0;
                            EurotaxOutDto etout = eurotaxBo.getEurotaxForecast(etin);

                            builder.WithProperty("Actual-New-Price", etout.ActualNewPrice.ToString());
                            builder.WithProperty("Error-Description", etout.ErrorDescription);
                        }
                        catch (Exception ex)
                        {
                            var inner = ex.InnerException.InnerException as FaultException;
                            if (inner != null)
                            {
                                var reason = inner.Reason.ToString();
                                builder.WithProperty("Fault-Code", inner.Code.Name);
                                builder.WithProperty("Fault-Reason", reason);

                                if (reason != "Processing Error")
                                    builder.WithProperty("Message", "Expected Reason: Processing Error", PropertyState.Error);
                            }
                            else
                                throw;
                        }

                    })
                    .MonitorDependency("Eurotax Valuation", builder =>
                    {
                        builder.CheckService(new ValuationSoapPortClient());

                        var vinClient = new ValuationSoapPortClient();
                        SoapXMLDto soapXmlDto = new SoapXMLDto() { };
                        vinClient.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXmlDto));
                        var fakeInput = new EurotaxInDto();

                        fakeInput.ISOCountryCodeValuation = Common.DAO.Auskunft.EurotaxValuationRef.ISOcountryType.DE;
                        fakeInput.ISOLanguageCodeValuation = Common.DAO.Auskunft.EurotaxValuationRef.ISOlanguageType.DE;
                        fakeInput.ISOCurrencyCodeValuation = ISOcurrencyType.EUR;

                        var eurotaxWsDao = new EurotaxWSDao();
                        var eurotaxBo = new EurotaxBo(eurotaxWsDao, new EurotaxDBDao(), null, null, null);
                        var header = eurotaxBo.MyGetHeaderTypeForValuation(fakeInput);
                        var settings = eurotaxBo.MyGetSettingTypeForValuation(fakeInput);
                        var versionInfo = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ETGversionInformationType();

                        vinClient.GetVersion(ref header, ref settings, ref versionInfo);

                        builder.WithProperty("ServiceVersion", versionInfo.ServiceVersion);
                        builder.WithProperty("IntelligenceVersion", versionInfo.IntelligenceVersion);
                        var releaseDate = versionInfo.DataVersionDate.Year + versionInfo.DataVersionDate.Month + versionInfo.DataVersionDate.Day;
                        builder.WithProperty("DataVersionDate", releaseDate);
                    })
                    .MonitorDependency("Eurotax VIN Search INTL", builder =>
                    {
                        builder.CheckService(new vinsearchSoapPortClient());
                        var eurotaxBo = new EurotaxBo(new EurotaxWSDao(), new EurotaxDBDao(), new AuskunftDao(), new CachedVGDao(), new CachedObTypDao());

                        var code = "ALIVESTATUS"; //"WVWZZZAUZFW167306"

                        var response = eurotaxBo.GetVinDecode(new EurotaxVinInDto()
                        {
                            vinCode = code
                        });

                        builder.WithProperty("Sent-VinCode", code);
                        builder.WithProperty("Status-Code", response.statusCode.ToString());
                        builder.WithProperty("Status-Message", string.IsNullOrEmpty(response.statusMsg) ? "<empty>" : response.statusMsg);
                        builder.WithProperty("Error-Code", response.ErrorCode.ToString());
                        builder.WithProperty("Error-Message", string.IsNullOrEmpty(response.ErrorDescription) ? "<empty>" : response.ErrorDescription);

                        //var vinClient = new vinsearchSoapPortClient();
                        //SoapXMLDto soapXmlDto = new SoapXMLDto() { };
                        //vinClient.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXmlDto));
                        //var fakeInput = new EurotaxVinInDto();

                        //fakeInput.ISOCountryCode = ISOcountryType.DE;
                        //fakeInput.ISOLanguageCode = ISOlanguageType.DE;

                        //var header = MyGetHeaderTypeHCEINTL(fakeInput);

                        //var request = new VersionRequestInputType
                        //{
                        //    ServiceId = MyGetVinServiceIdHCE(),
                        //    Settings = new Common.DAO.Auskunft.EurotaxVinRef.ETGsettingType
                        //    {
                        //        ISOcountryCode = ISOcountryType.DE,
                        //        ISOlanguageCode = ISOlanguageType.DE
                        //    }
                        //};

                        //var version = vinClient.GetVersion(ref header, request);

                        //builder.WithProperty("ServiceVersion", version.ServiceVersion);
                        //builder.WithProperty("IntelligenceVersion", version.IntelligenceVersion);
                        //var releaseDate = version.DataReleaseDate.Year + "-" + version.DataReleaseDate.Month + "-" + version.DataReleaseDate.Day;
                        //builder.WithProperty("ReleaseDate", releaseDate);
                    })
                    .MonitorDependency("Eurotax VIN Search", builder =>
                    {
                        builder.CheckService(new Common.DAO.Auskunft.VinSearch.vinsearchSoapPortClient());

                        var vinClient = new Common.DAO.Auskunft.VinSearch.vinsearchSoapPortClient();
                        SoapXMLDto soapXmlDto = new SoapXMLDto() { };
                        vinClient.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXmlDto));
                        var fakeInput = new EurotaxVinInDto
                        {
                            ISOCountryCode = ISOcountryType.DE,
                            ISOLanguageCode = ISOlanguageType.DE
                        };

                        var header = MyGetHeaderTypeHCE(fakeInput);
                        var request = new Common.DAO.Auskunft.VinSearch.VersionRequestInputType
                        {
                            ServiceId = MyGetVinServiceIdHCE(),
                            Settings = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ETGsettingType
                            {
                                ISOcountryCode = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ISOcountryType.DE,
                                ISOlanguageCode = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ISOlanguageType.DE
                            }
                        };


                        var version = vinClient.GetVersion(ref header, request);

                        builder.WithProperty("ServiceVersion", version.ServiceVersion);
                        builder.WithProperty("IntelligenceVersion", version.IntelligenceVersion);
                        var releaseDate = version.DataReleaseDate.Year + "-" + version.DataReleaseDate.Month + "-" + version.DataReleaseDate.Day;
                        builder.WithProperty("ReleaseDate", releaseDate);

                    })
                    .MonitorDependency("Decision Engine", builder => builder.CheckService(new S1PublicClient()))
                    .MonitorDependency("KREMO", builder => builder.CheckService(new ServiceSoapClient()))
                    .MonitorDependency("ZEK", builder => builder.CheckService(new ZEKServiceClient()))
                    .MonitorDependency("ZEK Batch", builder => builder.CheckService(new ZEKBatchServiceClient()))
                    .MonitorDependency("Deltavista", builder => builder.CheckService(new DVSoapServiceClient()))
                    .MonitorDependency("Deltavista V4", builder => builder.CheckService(new DVSoapServiceV4Client()))
                    .MonitorDependency("IBAN", builder => builder.CheckService(new BANKernelClient()))
                    .MonitorDependency("SimpleService", builder => builder.CheckService(new CicServiceClient()))
                    .MonitorDependency("B2BOL", builder => builder.CheckService(new B2BOLClient()))
                    .MonitorDependency("Guardean Decision Engine", builder => builder.CheckService(new Wkf_Control_InterfacePortTypeClient()))
                    .MonitorDependency("Guardean Decision Engine Status Update", builder => builder.CheckService(new Wkf_Status_UpdatePortTypeClient()))
                    .MonitorDependency("Guardean SCHUFA Engine", builder => builder.CheckService(new SchufaSiml2AuskunfteiWorkflowPortTypeClient()))
                    .MonitorDependency("CRIF", builder => builder.CheckService(new CrifSoapServicePortTypeV1_0Client()))
                    .MonitorDependency("BAS", builder =>
                    {
                        
                        MediatorServiceClient client = new MediatorServiceClient();
                        /*var modifyHeadersBehavior = new ModifyHeadersBehavior();
                        modifyHeadersBehavior.AddHeader("X-CIC-SYSWFUSER", "0");
                        ((MediatorServiceClient)client).Endpoint.Behaviors.Add(modifyHeadersBehavior);*/

                        builder.CheckService(client);
                        Stopwatch sw = Stopwatch.StartNew();
                        var response = client.Execute(new EvaluateFunctionsRequest() { Expression = "_delay(1)" }) as EvaluateFunctionsResponse;
                        var time = sw.ElapsedMilliseconds;
                        builder.WithProperty("Evaluate-Latency", time.ToString());
                        if (response == null || response.Result == null || response.Result != "1")
                        {
                            builder.WithProperty("Evaluate-Error", "Expected a response of 1.", PropertyState.Error);
                        }
                        if (response != null && response.Result != null)
                            builder.WithProperty("Evaluate-Response-_delay(1)", response.Result);
                    })
                    .MonitorDependency("PSTEAM Foodas", builder =>
                    {
                        builder.CheckService(new PSFoodasWSSoapClient());

                        var foodasBo = new FoodasBo();

                        var document = foodasBo.getDokument("abc", false);
                        builder.WithProperty("Document-Has-Errors", document.hasError.ToString());
                        builder.WithProperty("Document-Response-Id", (string.IsNullOrEmpty(document.Id) ? "<empty>" : document.Id));
                    })
                    .MonitorDependency("PSTEAM Foodas Login", builder => builder.CheckService(new WSLoginSoapClient()))
                    ;
                return configurationBuilder;
            }

            catch (Exception exception)
            {
                _log.Warn("An exception occured while starting the monitoring service.", exception);
                //throw new Exception("An exception occured while starting the monitoring service.", exception);
                return null;
            }
        }

        public static void Register(MonitoringConfigurationBuilder configurationBuilder)
        {
            try
            {
                MonitoringService = configurationBuilder.GetService();
                MonitoringConfiguration = configurationBuilder.Start(string.Format("http://localhost:1399{0}", HttpRuntime.AppDomainAppVirtualPath));
            }
            catch (Exception exception)
            {
                _log.Warn("An exception occured while starting the monitoring service.", exception);
                //throw new Exception("An exception occured while starting the monitoring service.", exception);
            }
        }

        private static string MyGetVinServiceIdHCE()
        {
            EurotaxLoginDataDto accessData = new EurotaxDBDao().GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE);
            return accessData.serviceId;
        }

        private static Common.DAO.Auskunft.VinSearch.ETGHeaderType MyGetHeaderTypeHCE(EurotaxVinInDto fakeInput)
        {
            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ETGHeaderType header = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ETGHeaderType();

            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.LoginDataType LoginData = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.LoginDataType();

            EurotaxLoginDataDto accessData = new EurotaxDBDao().GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE);

            LoginData.Name = accessData.name;
            LoginData.Password = accessData.password;
            header.Originator = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.OriginatorType();
            header.Originator.LoginData = LoginData;
            header.Originator.Signature = accessData.signature;
            header.VersionRequest = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.VersionType.Item110;
            return header;
        }
 

        public static string GetInformationalVersion(Assembly assembly)
        {
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }

        private static void MonitorSelf(CheckResultBuilder builder)
        {
            builder.WithVersion(Assembly.GetExecutingAssembly().GetName().Version.ToString());
            builder.WithProperty("Git-Version", GetInformationalVersion(Assembly.GetExecutingAssembly()));

            //IEnumerable<string> environmentVariables = (Environment.GetEnvironmentVariables().Cast<DictionaryEntry>())
            //    .Select(entry => string.Format("{0}={1}", entry.Key, entry.Value));

            //builder.WithProperty("Environment-Variables", string.Join("; <br>", environmentVariables));

            //string str1 = (string)null;
            //string environmentVariable = Environment.GetEnvironmentVariable("TNS_ADMIN");
            //string path1 = environmentVariable == null ? string.Empty : (environmentVariable == string.Empty || (int)environmentVariable[environmentVariable.Length - 1] == 92 ? environmentVariable + "tnsnames.ora" : environmentVariable + (object)'\\' + "tnsnames.ora");
            //builder.WithProperty("TNS-Search-Path", path1);
            //builder.WithProperty("TNS-Search-Path-Exists", File.Exists(path1).ToString());

            using (DdOwExtended ctx = new DdOwExtended())
            {
                var dllVersion = typeof(AUSKUNFT).Assembly.GetName().Version.Build;
                if (dllVersion > 10000)
                {
                    dllVersion = (dllVersion / 10);
                }
                var dllVersionString = dllVersion.ToString();
                var edmxVersion = dllVersionString.Insert(Math.Max(0, dllVersionString.Length - 3), ".");
                builder.WithProperty("DB-Version-EDMX", edmxVersion);

                DbConnection connection = ctx.getObjectContext().Connection;
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

                    try
                    {
                        foreach (OracleHome home in OracleConnection.Homes)
                        {
                            builder.WithProperty(string.Format("DB-OraHome-{0}", home.Name), string.Format("ClientVersion={0}, Path={1}", home.ClientVersion, home.Path));
                            try
                            {
                                var servers = home.GetServerList();
                                builder.WithProperty(string.Format("DB-OraHome-{0}-Servers", home.Name), string.Join("; ", servers));
                            }
                            catch (Exception ex)
                            {
                                builder.WithProperty(string.Format("DB-OraHome-{0}-Servers", home.Name), ex.ToString(), PropertyState.Warning);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        builder.WithProperty("DB-OraHome", "Could not load OracleConnection.Homes", PropertyState.Warning);
                    }
                }

                var stats = ctx.ExecuteStoreQuery<MachineInfo>("select count(*) sessions, count(distinct cicbenutzer) users, sum(sysdate-LOGINDATE)*24 sessionsLength from ciclog where sysdate BETWEEN logindate AND nvl(logoutdate,sysdate) and logindate >= sysdate - 1").FirstOrDefault();
                var machineStats = ctx.ExecuteStoreQuery<MachineInfo>("select maschine machine, count(*) sessions, count(distinct cicbenutzer) users, sum(sysdate-LOGINDATE)*24 sessionsLength from ciclog where sysdate BETWEEN logindate AND nvl(logoutdate,sysdate) and logindate >= sysdate - 1 group by maschine").ToList();
                
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

                var metrics = new List<MonitoringMetric>();

                if (stats != null)
                {
                    metrics.Add(new MonitoringMetric("Sessions", stats.Sessions));
                    metrics.Add(new MonitoringMetric("SessionsLength", stats.SessionsLength, "h"));
                    metrics.Add(new MonitoringMetric("Users", stats.Users));
                }

                foreach (var machineStat in machineStats)
                {
                    metrics.Add(new MonitoringMetric("SessionsByMachine-" + (machineStat.Machine ?? "null"), machineStat.Sessions));
                    metrics.Add(new MonitoringMetric("UsersByMachine-" + (machineStat.Machine ?? "null"), machineStat.Users));
                    metrics.Add(new MonitoringMetric("SessionsLengthByMachine-" + (machineStat.Machine ?? "null"), machineStat.SessionsLength, "h"));
                }

                metrics.Add(new MonitoringMetric("MemoryUsage", currentProcess.WorkingSet64));
                metrics.Add(new MonitoringMetric("Threads", currentProcess.Threads.Count));

                metrics = metrics.OrderBy(a => a.Key).ToList();

                metrics.ForEach(metric => builder.WithMetric(metric));

                builder.WithDatabaseInfo(query =>
                {
                    var info = ctx.ExecuteStoreQuery<DatabaseInfo>(query).FirstOrDefault();
                    int edmxVersionInt;
                    int.TryParse(edmxVersion.Replace(".", ""), out edmxVersionInt);

                    int openLeaseVersionInt;
                    int.TryParse(info.OpenleaseVersion.Replace(".", ""), out openLeaseVersionInt);

                    if (edmxVersionInt > openLeaseVersionInt)
                    {
                        builder.WithProperty("DB-Version-Mismatch", "EDMX Version darf nicht größer sein als DB Version", PropertyState.Error);
                    }
                    else if (edmxVersionInt < openLeaseVersionInt)
                    {
                        builder.WithProperty("DB-Version-Mismatch", true.ToString(), PropertyState.Warning);
                    }
                    return info;
                });
                builder.WithDatabasePackagesInfo(query => ctx.ExecuteStoreQuery<OraclePackageState>(query).ToList());
            }
        }


    }

    public class MachineInfo
    {
        public string Machine { get; set; }

        public long Users { get; set; }

        public long Sessions { get; set; }

        public float SessionsLength { get; set; }
    }
}