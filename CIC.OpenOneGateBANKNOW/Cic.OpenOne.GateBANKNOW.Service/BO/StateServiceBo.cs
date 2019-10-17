using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Devart.Data.Oracle;
using System;
using System.Data.Common;
using System.Linq;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
    /// <summary>
    /// Bo for Service State information
    /// </summary>
    public class StateServiceBo
    {
        /// <summary>
        /// SQL
        /// </summary>
        const String QUERYVERSION = "select VERMAJOR||'.'||VERMINOR  from DBVERS order by VERMAJOR desc, VERMINOR desc";
        const String QUERYHOST = "select HOST_NAME as hostName, INSTANCE_NAME as instanceName, VERSION as version from v$instance";
        const String DUALQUERY = "select 1 from dual";
        /// <summary>
        /// Diensteinformation auslesen
        /// </summary>
        /// <param name="Info">Daten</param>
        public void getServiceInformation(ServiceInformation Info)
        {
            // Get the service version
            try
            {
                Info.ServiceVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception ex)
            {
                Info.ServiceVersion = "Not available: " + ex.Message;
            }
            using (DdOlExtended ctx = new DdOlExtended())
            {
                // Get the connection
                
                DbConnection EntityConnection = ctx.getObjectContext().Connection;

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
                    OracleConnection OracleConnection = ((System.Data.Entity.Core.EntityClient.EntityConnection)EntityConnection).StoreConnection as OracleConnection;

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
                        double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        ctx.ExecuteStoreQuery<long>(DUALQUERY, null).FirstOrDefault();
                        Info.DatabaseConnection.latency = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds - start);
                        Info.DatabaseConnection.orahome = OracleConnection.Home;
                    }
                }
                try
                {
                    Info.DatabaseInstance = ctx.ExecuteStoreQuery<DatabaseInstanceInfo>(QUERYVERSION, null).FirstOrDefault();

                    // Check if the version was found
                    if (Info.DatabaseInstance == null)
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