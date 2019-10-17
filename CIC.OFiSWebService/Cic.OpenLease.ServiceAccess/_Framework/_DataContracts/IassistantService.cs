using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Cic.OpenLease.ServiceAccess.Merge.DTO;

namespace Cic.OpenLease.ServiceAccess.Merge.Contract
{
    /// <summary>
    /// Dieses Interface assistantService stellt Methoden zum Configurieren und Editieren des Services dar und bietet ein übersichtlicheres Logfile
    /// </summary>
    [ServiceContract(Name = "IassistantService", Namespace = "http://cic-software.de/contract")]
    public interface IassistantService
    {
        /// <summary>
        /// liefert alle pids 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetLogFilePidDto getLogFilePid();

        /// <summary>
        /// Liefert das Logfile zurück, wenn gewünscht mit passender PID
        /// </summary>
        /// <param name="input">igetLogFile</param>
        /// <returns>ogetLogFile</returns>
        [OperationContract]
        ogetLogFileDto getLogFile(igetLogFileDto input);

        /// <summary>
        /// Liefert die default PID vom Logfile
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetLogFileDefaultPid getLogFileDefaultPid();
    }
}