// OWNER MK, 02-07-2009
namespace Cic.OpenLease.Service.Merge.ServicesState
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.ServicesState;
    using Cic.OpenOne.Common.Model.DdOiqueue;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Config;
    using Devart.Data.Oracle;
    using System.Data.Common;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.ServicesState")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class ServicesStateService : Cic.OpenLease.ServiceAccess.Merge.ServicesState.IServicesStateService
    {        
        #region IServicesStateService Members
        public string DeliverVersion()
        {
            try
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); //Cic.OpenLease.Service.AssemblyInfo.DeliverVersion();
            }
            catch
            {
                throw;
            }
        }

        public void FlushCache()
        {
            // Reload the configuration
            //CfgSingleton.Instance.Init();
            AppConfig.Instance.Init();
            CacheManager.getInstance().flush(0);
        }

        public ServiceInformation DeliverServiceInformation()
        {
            // Create a service information
            ServiceInformation Info = new ServiceInformation();

            // Get the service version
            Info.ServiceVersion = DeliverVersion();

            // Create a context
            using (DdOiQueueExtended Context = new DdOiQueueExtended())
            {
                // Get the connection
                DbConnection EntityConnection = Context.getObjectContext().Connection;

                // Check if the connection is valid
                if (EntityConnection == null)
                {
                    // Nullify
                    Info.DatabaseConnection = null;
                    Info.DatabaseInstance = null;

                    // Exit
                    return Info;
                }

                // Get the store connection
                OracleConnection OracleConnection = ((System.Data.Entity.Core.EntityClient.EntityConnection)EntityConnection).StoreConnection as OracleConnection;

                // Check if the store connection is valid
                if (OracleConnection == null)
                {
                    // Nullify
                    Info.DatabaseConnection = null;
                    Info.DatabaseInstance = null;

                    // Exit
                    return Info;
                }

                // Set the connection data
                Info.DatabaseConnection.ClientVersion = OracleConnection.ClientVersion;
                Info.DatabaseConnection.DirectConnection = OracleConnection.Direct;
                Info.DatabaseConnection.HostName = OracleConnection.DataSource;
                Info.DatabaseConnection.Name = OracleConnection.Name;
                Info.DatabaseConnection.SId = OracleConnection.Sid;
                Info.DatabaseConnection.Port = OracleConnection.Port;

                try
                {
                    // Get the OpenLease version
                    var OpenLeaseVersion = (from Version in Context.DBVERS
                                            orderby Version.VERMAJOR, Version.VERMINOR descending
                                            select Version).FirstOrDefault();

                    // Check if the version was found
                    if (OpenLeaseVersion == null)
                    {
                        // Version is not available
                        Info.OpenLeaseDatabaseVersion = "Not available";
                    }
                    else
                    {
                        // Set the version
                        Info.OpenLeaseDatabaseVersion = OpenLeaseVersion.VERMAJOR + "." + OpenLeaseVersion.VERMINOR;
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
                    Info.DatabaseInstance = Context.ExecuteStoreQuery<DatabaseInstanceInfo>("select HOST_NAME as HostName, INSTANCE_NAME as InstanceName, VERSION as Version from v$instance", null).FirstOrDefault();

                }
                catch
                {
                    // Instance information is not available
                    Info.DatabaseInstance = null;
                }
            }

            // Return the information
            return Info;
        }

        public Cic.OpenLease.ServiceAccess.Merge.ServicesState.ServiceState DeliverServiceState()
        {
            bool IsServiceable = true;
            string Message = null;
            long ProcessingTime = 0;

            System.Diagnostics.Stopwatch Stopwatch;
            Stopwatch = new System.Diagnostics.Stopwatch();
            
            // Start watch
            Stopwatch.Start();

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    // Create test query
                    var TestQuery = from person in context.PERSON
                                    select person;
                    
                    // Get count
                    TestQuery.Count();
                }
            }
            catch (System.Exception e)
            {
                IsServiceable = false;
                Message = MyDeliverMessage(e);
            }

            // Stop watch
            Stopwatch.Stop();

            // Set processing time
            ProcessingTime = Stopwatch.ElapsedMilliseconds;

            return new Cic.OpenLease.ServiceAccess.Merge.ServicesState.ServiceState(IsServiceable, Message, ProcessingTime);
        }
        #endregion

        #region My methods
        private string MyDeliverMessage(System.Exception execption)
        {
            if (execption == null)
            {
                return string.Empty;
            }

            string Message = execption.Message;
            Message += System.Environment.NewLine;

            if (execption.InnerException != null)
            {
                Message += MyDeliverMessage(execption.InnerException);
            }

            return Message;
        }
        #endregion
    }
}
