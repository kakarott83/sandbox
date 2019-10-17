using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// IFibuService
    /// </summary>
    [ServiceContract(Name = "IFibuService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IFibuService
    {
        /// <summary>
        /// FibuTes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        oMessagingDto FibuTest(string input);

        /// <summary>
        /// UploadAccountMasterdata
        /// </summary>
        [OperationContract]
        oMessagingDto UploadAccountMasterdata(iFibuAccountMasterDTO input);
    }
}