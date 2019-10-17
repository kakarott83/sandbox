using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.Util.Extension;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface searchVertragService stellt die Methoden zur Service-Statusabfrage bereit
    /// </summary>
    [ServiceContract(Name = "IStateService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IStateService
    {
        /// <summary>
        /// Service Information liefern
        /// </summary>
        /// <returns>Service Informationsdaten</returns>
        [OperationContract]
        ServiceInformation DeliverServiceInformation();

        /// <summary>
        /// Delivers the logfile
        /// </summary>
        /// <returns>LogInfo</returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        LogInfo getLogData();

        /// <summary>
        /// Delivers the web.config
        /// </summary>
        /// <returns>ConfigInfo</returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ConfigInfo getConfigData();

        /// <summary>
        /// Flushes the cache
        /// </summary>
        [OperationContract]
        void flushCache();
    }
}