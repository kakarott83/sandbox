using System;
using System.Collections.Generic;
using System.Text;
using CIC.Bas.Infrastructure;
using Blowfish = Cic.OpenOne.Common.Util.Security.Blowfish;
using Devart.Data.Oracle;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.Util.Config
{
    /// <summary>
    /// Central Configuration File access
    /// </summary>
    public class Configuration
    {
        private const string CnstBlowfishKey = "C.I.C.-S0ftwareGmbH1987Muenchen0";
        private static bool soapLoggingEnabled = Cic.OpenOne.Common.Properties.Config.Default.SoapLoggingEnabled;
        private static bool soapLoggingEnabledAuskunft = Cic.OpenOne.Common.Properties.Config.Default.SoapLoggingEnabledAuskunft;

        #region Private variables
        private static Dictionary<string, string> valueMap = new Dictionary<string, string>();
        private static DBSettings settings;

        private static CacheDictionary<String, String> pwCache = CacheFactory<String, String>.getInstance().createCache(2000*60, CacheCategory.Prisma);
        #endregion

        /// <summary>
        /// getSoapLoggingEnabled
        /// </summary>
        /// <returns></returns>
        public static bool getSoapLoggingEnabled()
        {
            return soapLoggingEnabled;
        }

        /// <summary>
        /// setSoapLoggingEnabled
        /// </summary>
        /// <param name="value"></param>
        public static void setSoapLoggingEnabled(bool value)
        {
            soapLoggingEnabled = value;
        }

        /// <summary>
        /// getSoapLoggingEnabledAuskunft
        /// </summary>
        /// <returns></returns>
        public static bool getSoapLoggingEnabledAuskunft()
        {
            return soapLoggingEnabledAuskunft;
        }

        /// <summary>
        /// setSoapLoggingEnabledAuskunft
        /// </summary>
        /// <param name="value"></param>
        public static void setSoapLoggingEnabledAuskunft(bool value)
        {
            soapLoggingEnabledAuskunft = value;
        }

        /// <summary>
        /// allows to override the default source of dbsettings
        /// </summary>
        /// <param name="s"></param>
        public static void setDBSettings(DBSettings s)
        {
            settings = s;
        }
        /// <summary>
        /// Returns a SettingsObject from an Oracle Connection
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static DBSettings getSettingsFromConnection(OracleConnection conn)
        {
            DBSettings settings = new DBSettings();
            settings.plainPassword = false;
            settings.isDirect = true;
            settings.ServerName = conn.Server;
            settings.ServerPort = conn.Port.ToString();
            settings.ServerSID = conn.ServiceName;


            settings.timeout = conn.ConnectionTimeout.ToString();

            // Get values for tns connection - Oracle Net Service Name
            settings.DataSource = conn.DataSource;
            
            // Get values for db login
            settings.UserId = conn.UserId;
            settings.Password = conn.Password;

             settings.minPool = Cic.OpenOne.Common.Properties.Config.Default.DBMinPoolSize;
            settings.maxPool = Cic.OpenOne.Common.Properties.Config.Default.DBMaxPoolSize;
            settings.valConn = Cic.OpenOne.Common.Properties.Config.Default.DBValidateConnection; //only direct mode
            settings.stmtCache = Cic.OpenOne.Common.Properties.Config.Default.DBStatementCacheSize; //only direct mode

            settings.dynamicPassword = false;
            settings.passwordServiceAddress = Cic.OpenOne.Common.Properties.Config.Default.CicPasswordServiceAddress;
            settings.appId = Cic.OpenOne.Common.Properties.Config.Default.AppId;
            return settings;
        }
        private static void initDefaultSettings()
        {
            settings = new DBSettings();
            settings.plainPassword = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringPlainPassword;
            settings.isDirect = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringDirect;
            settings.ServerName = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringServerName;
            settings.ServerPort = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringServerPort;
            settings.ServerSID = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringSID;
            if ("OFF".Equals(settings.ServerSID))
                settings.ServerSID = null;

            settings.timeout = Cic.OpenOne.Common.Properties.Config.Default.DBConnectionTimeout;

            // Get values for tns connection - Oracle Net Service Name
            settings.DataSource = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringDataSource;
            if ("OFF".Equals(settings.DataSource))
                settings.DataSource = null;

            // Get values for db login
            settings.UserId = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringUserId;
            settings.Password = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringPassword;

            settings.oraClientHome = Cic.OpenOne.Common.Properties.Config.Default.OpenLeaseConnectionStringOraClientHome;

            settings.minPool = Cic.OpenOne.Common.Properties.Config.Default.DBMinPoolSize;
            settings.maxPool = Cic.OpenOne.Common.Properties.Config.Default.DBMaxPoolSize;
            settings.valConn = Cic.OpenOne.Common.Properties.Config.Default.DBValidateConnection; //only direct mode
            settings.stmtCache = Cic.OpenOne.Common.Properties.Config.Default.DBStatementCacheSize; //only direct mode

            settings.dynamicPassword = Cic.OpenOne.Common.Properties.Config.Default.DynamicPassword;
            if (settings.dynamicPassword)
            {
                settings.Password = null;
            }

            settings.passwordServiceAddress = Cic.OpenOne.Common.Properties.Config.Default.CicPasswordServiceAddress;
            settings.appId = Cic.OpenOne.Common.Properties.Config.Default.AppId;

            
        }

        
        /// <summary>
        /// Delivers the database connection String
        /// </summary>
        /// <returns></returns>
        public static string DeliverOpenLeaseConnectionString()
        {
            if (settings == null) initDefaultSettings();
            return DeliverOpenLeaseConnectionString(settings);
        }
        /// <summary>
        /// Delivers the database connection String for the given settings
        /// </summary>
        /// <param name="userSettings"></param>
        /// <returns></returns>
        public static string DeliverOpenLeaseConnectionString(DBSettings userSettings)
        {
            if (userSettings == null)
            {
                initDefaultSettings();
                userSettings = settings;
            }
            String OpenLeaseConnectionString;

            String Password = userSettings.Password;
            Blowfish Blowfish = null;
            if (!userSettings.plainPassword && !userSettings.dynamicPassword)
            {
                try
                {
                    // New blowfish
                    Blowfish = new Blowfish(CnstBlowfishKey);
                }
                catch
                {
                    // Ignore exception
                }
                // Check object
                if (Blowfish != null)
                {
                    // Set password
                    Password = Blowfish.Encode(Password);
                }
            }

            //BAS Password support
            if (userSettings.dynamicPassword && userSettings.Password==null)
            {
                String service = null;
                if (userSettings.DataSource != null && userSettings.DataSource.Length > 0)
                {
                    service = userSettings.DataSource;
                }
                else
                {
                    service = userSettings.ServerSID;
                }
                String cacheKey = service + "_" + settings.UserId + "_" + Cic.OpenOne.Common.Properties.Config.Default.AppId + "_"+userSettings.passwordServiceAddress;
                if (!pwCache.ContainsKey(cacheKey))
                { 
                    userSettings.Password = OpenLeaseDatabaseHelper.DeliverOpenLeasePassword(service, settings.UserId, Cic.OpenOne.Common.Properties.Config.Default.AppId, userSettings.passwordServiceAddress);
                    pwCache[cacheKey] = userSettings.Password;
                }
                else
                {
                    userSettings.Password = pwCache[cacheKey];
                }

                Password = userSettings.Password;
            }

            // Create connection string
            if (userSettings.isDirect)
            {
                OpenLeaseConnectionString = MyDeliverDirectConnectionString(Password, userSettings);
            }
            else
            {
                OpenLeaseConnectionString = MyDeliverBasicConnectionString(Password, userSettings);
            }

            // Check string value
            if (OpenLeaseConnectionString == null)
            {
                // Set result
                OpenLeaseConnectionString = string.Empty;
            }
            return OpenLeaseConnectionString;
        }

        /// <summary>
        /// Creates a connection string for connecting directly with devart, not the local oracle driver (DirectMode = True)
        /// </summary>
        /// <param name="password"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static string MyDeliverDirectConnectionString(string password, DBSettings settings)
        {
            System.Text.StringBuilder ConnectionStringBuilder = new System.Text.StringBuilder();

            //User Id=_id_;Password=_pass_;Server=_srv_;Direct=True;Sid=_sid_;Port=_port_;Persist Security Info=True
            ConnectionStringBuilder.Append("User Id=");
            ConnectionStringBuilder.Append(settings.UserId);
            ConnectionStringBuilder.Append(";Password=");
            ConnectionStringBuilder.Append(password);
            ConnectionStringBuilder.Append(";Server=");
            ConnectionStringBuilder.Append(settings.ServerName);
            ConnectionStringBuilder.Append(";Direct=True");
            //use service name when provided
            if (settings.DataSource != null && settings.DataSource.Length > 0)
            {
                ConnectionStringBuilder.Append(";Service Name=");
                ConnectionStringBuilder.Append(settings.DataSource);
            }
            else
            {
                ConnectionStringBuilder.Append(";Sid=");
                ConnectionStringBuilder.Append(settings.ServerSID);
            }
            

            ConnectionStringBuilder.Append(";Port=");
            ConnectionStringBuilder.Append(settings.ServerPort);
            ConnectionStringBuilder.Append(";Persist Security Info=True");
            ConnectionStringBuilder.Append(";Default Command Timeout=");
            ConnectionStringBuilder.Append(settings.timeout);
            ConnectionStringBuilder.Append(";");

            ConnectionStringBuilder.Append("Min Pool Size=");
            ConnectionStringBuilder.Append(settings.minPool);
            ConnectionStringBuilder.Append(";");

            ConnectionStringBuilder.Append("Max Pool Size=");
            ConnectionStringBuilder.Append(settings.maxPool);
            ConnectionStringBuilder.Append(";");

            ConnectionStringBuilder.Append("Validate Connection=");
            ConnectionStringBuilder.Append(settings.valConn);
            ConnectionStringBuilder.Append(";");

            ConnectionStringBuilder.Append("Statement Cache Size=");
            ConnectionStringBuilder.Append(settings.stmtCache);
            ConnectionStringBuilder.Append(";");

            return ConnectionStringBuilder.ToString();
        }

        /// <summary>
        /// Creates the non-devart connection string to connect to local oracle client (when DirectMode = False)
        /// </summary>
        /// <param name="password"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static string MyDeliverBasicConnectionString(string password, DBSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Data Source=");
            sb.Append(settings.DataSource);
            sb.Append(";Persist Security Info=True;User ID=");
            sb.Append(settings.UserId);
            sb.Append(";Password=");
            sb.Append(password);
            sb.Append(";Min Pool Size=");
            sb.Append(settings.minPool);
            sb.Append(";Max Pool Size=");
            sb.Append(settings.maxPool);
            if(settings.timeout!=null && settings.timeout.Length>0)
            {
                sb.Append(";Default Command Timeout=");
                sb.Append(settings.timeout);
            }
            // New Parameter to select the Oracle Client
            if (settings.oraClientHome != null && settings.oraClientHome.Length > 0)
            {
                sb.Append(";home=");
                sb.Append(settings.oraClientHome);
            }
            return sb.ToString();
        }
    }
}