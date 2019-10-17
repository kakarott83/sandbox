using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using log4net;
using System.Linq;
using log4net.Repository.Hierarchy;
using Configuration = Cic.OpenOne.Common.Util.Config.Configuration;
using ILog = Cic.OpenOne.Common.Util.Logging.ILog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace Cic.OpenOne.Common.Util.Config
{
    /// <summary>
    /// Class Managing an db timestamp-change to force webservice settings to update
    /// </summary>
    public class ProcessUpdater
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Timer timer;
        private static DateTime? lastCacheChgTime = null;
        private static String oldLogLevel;
        private static String defaultSoapLogging = "false";
        private static String defaultSoapLoggingAuskunft = "false";
        private static int checkIntervalMs = 60000;
        public static String logLevel = "DEBUG";
        private static ProcessUpdater _instance;
        private static string LOCK = "LOCK";

        public delegate void ProcessUpdaterFunction();
        
        public ProcessUpdaterFunction updaterFunction {get;set;}

        /// <summary>
        /// Starts a timer thread looking for a new update timestamp in database
        /// </summary>
        private ProcessUpdater()
        {
            defaultSoapLogging = Configuration.getSoapLoggingEnabled().ToString();
            defaultSoapLoggingAuskunft = Configuration.getSoapLoggingEnabledAuskunft().ToString();

            timer = new Timer();
            timer.Elapsed += pollDbValues;
            timer.Interval = checkIntervalMs;
            timer.Start();
        }
        public static ProcessUpdater getInstance()
        {
            lock (LOCK)
            {
                if (_instance == null)
                    _instance = new ProcessUpdater();
            }
            return _instance;
        }
        /// <summary>
        ///     cyclic update of db-settings for the application
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void pollDbValues(object source, ElapsedEventArgs e)
        {
            try
            {
                var cacheChgTime = AppConfig.Instance.getValueFromDb("SETUP.NET", "CACHE", "CHGTIME");
                //yyyy-MM-dd HH:mm:ss

                if (cacheChgTime == null || cacheChgTime.Length < 1) return; //ignore invalid settings

                try
                {
                    var updTime = DateTime.Parse(cacheChgTime, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None);

                    if (DateTime.Now.ToFileTime() > updTime.ToFileTime() &&
                        (lastCacheChgTime==null||
                        updTime.ToFileTime() > lastCacheChgTime.Value.ToFileTime()))
                    {
                        updateSettings();
                        lastCacheChgTime = DateTime.Now;
                    }
                    else
                    {
                        setCurrentLogLevelExcludedHosts();
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Flush not possible: Flush Timestamp invalid: ", ex);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Problem in pollDbValues-Timer", ex);
            }
        }

        /// <summary>
        /// UPdates the db loglevel with the current db value
        /// </summary>
        public static void updateLoglevelFromDB()
        {
            var loglevel = AppConfig.Instance.getValueFromDb("SETUP.NET","LOG","LEVEL","INFO");
            //OFF, FATAL, ERROR, WARN, INFO, DEBUG, ALL
            if (oldLogLevel != loglevel)
            {
                setLoggingLevel(loglevel,true);
            }
        }

        /// <summary>
        /// Callback for updating settings for the application from new database values
        /// </summary>
        private void updateSettings()
        {
            if (updaterFunction != null)
                updaterFunction();
            CacheManager.getInstance().reconfigure();
            CacheManager.getInstance().flush(0);
            AppConfig.Instance.reloadCFG();

            var loglevel = AppConfig.Instance.GetEntry("LOG", "LEVEL", "INFO", "SETUP.NET");
            //OFF, FATAL, ERROR, WARN, INFO, DEBUG, ALL
            if (oldLogLevel != loglevel)
            {
                setLoggingLevel(loglevel,true);
                oldLogLevel = loglevel;
            }
            var soapLogging = AppConfig.Instance.GetEntry("LOG", "SOAP", defaultSoapLogging, "SETUP.NET");
            if (soapLogging != null)
            {
                var cSoapLogging = Configuration.getSoapLoggingEnabled();
                soapLogging = soapLogging.ToUpper();
                var nSoapLogging = "TRUE".Equals(soapLogging);
                if (nSoapLogging != cSoapLogging)
                    Configuration.setSoapLoggingEnabled(nSoapLogging);
            }

            var soapLoggingAuskunft = AppConfig.Instance.GetEntry("LOG", "SOAPAUSKUNFT", defaultSoapLoggingAuskunft,
                "SETUP.NET");
            if (soapLoggingAuskunft != null)
            {
                var cSoapLoggingAuskunft = Configuration.getSoapLoggingEnabledAuskunft();
                soapLoggingAuskunft = soapLoggingAuskunft.ToUpper();
                var nSoapLoggingAuskunft = "TRUE".Equals(soapLoggingAuskunft);
                if (nSoapLoggingAuskunft != cSoapLoggingAuskunft)
                    Configuration.setSoapLoggingEnabledAuskunft(nSoapLoggingAuskunft);
            }
        }

        /// <summary>
        /// Sets the current log level again for all loggers
        /// </summary>
        private static void setCurrentLogLevelExcludedHosts()
        {
            try
            {

                String hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
                String excludehosts = AppConfig.Instance.getValueFromDb("SETUP.NET", "LOG", "EXCLUDEDHOSTS", "");
                String[] hosts = excludehosts.Split(',');
                if (hosts.Contains(hostname))//set from config-file here!
                {
                    //Configure the root logger.
                    var h = (Hierarchy)LogManager.GetRepository();
                    var rootLogger = h.Root;
                    //rootLogger.Level = h.LevelMap[strLogLevel];
                    setLoggingLevel(rootLogger.Level.ToString(), false);
                    oldLogLevel = rootLogger.Level.ToString();
                }
            }
            catch(Exception e)
            {

            }
        }
        /// <summary>
        ///     Activates debug level
        /// </summary>
        /// <sourceurl>http://geekswithblogs.net/rakker/archive/2007/08/22/114900.aspx</sourceurl>
        private static void setLoggingLevel(string strLogLevel, bool checkExclude)
        {
            if (checkExclude)
            {
                String hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
                String excludehosts = AppConfig.Instance.getValueFromDb("SETUP.NET", "LOG", "EXCLUDEDHOSTS", "");
                String[] hosts = excludehosts.Split(',');
                if (hosts.Contains(hostname))
                    return;
            }
            try
            {
                var strChecker = "OFF_WARN_INFO_DEBUG_ERROR_FATAL_ALL";

                if (String.IsNullOrEmpty(strLogLevel) || strChecker.Contains(strLogLevel) == false)
                    return;

                logLevel = strLogLevel;
                var repositories = LogManager.GetAllRepositories();

                //Configure all loggers to be at the debug level.
                foreach (var repository in repositories)
                {
                    repository.Threshold = repository.LevelMap[strLogLevel];
                    var hier = (Hierarchy)repository;
                    var loggers = hier.GetCurrentLoggers();
                    foreach (var logger in loggers)
                    {
                        ((Logger)logger).Level = hier.LevelMap[strLogLevel];
                    }
                }

                //Configure the root logger.
                var h = (Hierarchy)LogManager.GetRepository();
                var rootLogger = h.Root;
                rootLogger.Level = h.LevelMap[strLogLevel];
            }
            catch (Exception e)
            {
                _log.Error("Error setting LogLevel", e);
            }
        }
    }
}
