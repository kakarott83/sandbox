// OWNER MK, 02-07-2009
namespace Cic.OpenLease.ServiceAccess.Merge.ServicesState
{
    /// <summary>
    /// This service is used generally to obtain the information about readiness of services.
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "ServicesStateService", Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.ServicesState")]
    public interface IServicesStateService
    {
        /// <summary>
        /// Returns the number of a current version.
        /// </summary>
        /// <returns>string</returns>
        [System.ServiceModel.OperationContract]
        string DeliverVersion();

        /// <summary>
        /// Clears the configuration cache.
        /// </summary>
        /// <returns>string</returns>
        [System.ServiceModel.OperationContract]
        void FlushCache();

        /// <summary>
        /// Returns the information about the services.
        /// </summary>
        /// <returns>string</returns>
        [System.ServiceModel.OperationContract]
        ServiceInformation DeliverServiceInformation();

        /// <summary>
        /// Returns the ServiceState object which contains information about the current service state.
        /// </summary>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.Merge.ServicesState.ServiceState" /></returns>
        [System.ServiceModel.OperationContract]
        ServiceState DeliverServiceState();
    }
}
