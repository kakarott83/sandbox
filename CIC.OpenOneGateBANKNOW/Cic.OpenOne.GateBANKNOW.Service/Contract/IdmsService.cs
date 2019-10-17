using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Service Interface for providing trigger access from DMS into OL
    /// </summary>
    [ServiceContract(Name = "IdmsService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IdmsService
    {

        /// <summary>
        /// interface from DMS to OL for new incoming Documents
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oDMSUploadDto execDMSUploadTrigger(iDMSUploadDto input);

    }
}
