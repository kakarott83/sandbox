using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Model.DdOl;

using System.Data.EntityClient;
using Devart.Data.Oracle;
using Cic.One.DTO;
using Microsoft.Win32;
using System.Globalization;
using System.Text;
using Cic.One.Web.BO.Search;
using CIC.Monitoring.Builders;

namespace Cic.One.Web.BO
{
   /// <summary>
    /// Bo for Service State information
    /// </summary>
    public class StateServiceBo : IStateServiceBo
    {
        //TODO -> Into DAO
        /// <summary>
        /// SQL
        /// </summary>
        const String QUERYVERSION = "select VERMAJOR||'.'||VERMINOR  from DBVERS order by VERMAJOR desc, VERMINOR desc";
        const String QUERYHOST = "select HOST_NAME as hostName, INSTANCE_NAME as instanceName, VERSION as version from v$instance";
        const String DUALQUERY = "select 1 from dual";

        private static List<SanityCheckInfoDto> sanityInfos = new List<SanityCheckInfoDto>();

        public static void addSanityCheckInfo(SanityCheckInfoDto info)
        {
            sanityInfos.Add(info);
        }
        public static void monitorChecks(CheckResultBuilder builder)
        {
            foreach(SanityCheckInfoDto si in sanityInfos)
            {
                builder.WithProperty(si.description, si.setting, si.ok ? CIC.Monitoring.Model.PropertyState.Success : CIC.Monitoring.Model.PropertyState.Warning);
            }
        }
        /// <summary>
        /// Returns a summary of sanity checks
        /// </summary>
        /// <returns></returns>
        public String performSanityChecks()
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (SanityCheckInfoDto si in sanityInfos)
            
            {
                sb.Append("* ");
                sb.Append(si.ToString());
                sb.Append("\n");
            }
            
            
            return sb.ToString();
        }
        /// <summary>
        /// Diensteinformation auslesen
        /// </summary>
        /// <param name="Info">Daten</param>
        public void getServiceInformation(ServiceInfoDto Info)
        {
            // Get the service version
            try
            {
                //Cic.OpenOne.Common.Model.DdOl.PeRoleUtil p;
                Type t = Activator.CreateInstance("Cic.One.Web.Service", "Cic.One.Web.Service.VersionHandle").Unwrap().GetType();
                String serviceVersion = System.Reflection.Assembly.GetAssembly(t).GetName().Version.ToString();
                String dbVersion = System.Reflection.Assembly.GetAssembly(new Cic.OpenOne.Common.Model.DdOl.PeRoleUtil().GetType()).GetName().Version.ToString();
                dbVersion = dbVersion.Split('.')[2];
                String[] versions = serviceVersion.Split('.');
                Info.ServiceVersion = versions[0] + "." + versions[1] + "." + dbVersion + "." + versions[3];
            }
            catch (Exception ex)
            {
                Info.ServiceVersion = "Not available: " + ex.Message;
            }
            try
            {
                Info.iisVersion = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp").GetValue("VersionString").ToString();
            }
            catch (Exception e)
            {
                Info.iisVersion = "Not available: " + e.Message;
            }
            try
            {
                RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
                string[] version_names = installed_versions.GetSubKeyNames();
                //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
                int SP = Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1]).GetValue("SP", 0));

                Info.netVersion = Framework + ((SP>0)?(" SP " + SP):"");
                
              

            }
            catch (Exception e)
            {
                Info.netVersion = "Not available: " + e.Message;
            }
            try
            {
               
                Info.osVersion = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion").GetValue("ProductName").ToString()
                    + " " +
                    Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion").GetValue("CurrentVersion").ToString()
                    + " " +
                    Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion").GetValue("BuildLabEx").ToString();

            }
            catch (Exception e)
            {
                Info.osVersion = "Not available: " + e.Message;
            }

            using (DdOlExtended ctx = new DdOlExtended())
            {
                // Get the connection
                EntityConnection EntityConnection = ctx.Connection as EntityConnection;

                // Check if the connection is valid
                if (EntityConnection == null)
                {
                    // Nullify
                    Info.DatabaseConnection = null;
                    Info.DatabaseInstance = null;


                }
                else
                {
                    // Get the store connection
                    OracleConnection OracleConnection = EntityConnection.StoreConnection as OracleConnection;

                    // Check if the store connection is valid
                    if (OracleConnection == null)
                    {
                        // Nullify
                        Info.DatabaseConnection = null;
                        Info.DatabaseInstance = null;


                    }
                    else
                    {
                        Info.DatabaseConnection = new DatabaseConnectionInfo();
                        // Set the connection data
                        Info.DatabaseConnection.clientVersion = OracleConnection.ClientVersion;
                        Info.DatabaseConnection.directConnection = OracleConnection.Direct;
                        Info.DatabaseConnection.hostName = OracleConnection.DataSource;
                        Info.DatabaseConnection.name = OracleConnection.Name;
                        Info.DatabaseConnection.sid = OracleConnection.Sid;
                        Info.DatabaseConnection.port = OracleConnection.Port;
                        Info.DatabaseConnection.home = OracleConnection.Home;
                        double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        ctx.ExecuteStoreQuery<long>(DUALQUERY, null).FirstOrDefault();
                        Info.DatabaseConnection.latency = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds - start);
                    }
                }
                try
                {

                    Info.OpenLeaseDatabaseVersion = ctx.ExecuteStoreQuery<String>(QUERYVERSION, null).FirstOrDefault();

                    // Check if the version was found
                    if (Info.OpenLeaseDatabaseVersion == null)
                    {
                        // Version is not available
                        Info.OpenLeaseDatabaseVersion = "Not available";
                    }

                }
                catch
                {
                    // Version is not available
                    Info.OpenLeaseDatabaseVersion = "Not available";
                }

                try
                {
                    // Get the instance information
                    Info.DatabaseInstance = ctx.ExecuteStoreQuery<DatabaseInstanceInfo>(QUERYHOST, null).FirstOrDefault();

                }
                catch
                {
                    // Instance information is not available
                    Info.DatabaseInstance = null;
                }
            }

        }
    }
}