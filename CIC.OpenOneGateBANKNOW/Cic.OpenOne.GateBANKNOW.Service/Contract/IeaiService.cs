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
    [ServiceContract(Name = "IeaiService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IeaiService
    {
       /// <summary>
        /// internally used method to trigger a webservice-call
        /// internal version for job-service to fire an event
        /// </summary>
        /// <param name="sysEaiHOT"></param>
        /// <returns></returns>
        [OperationContract]
        oEAIExecDto execEAIHOT(int sysEaiHOT);

    }
}
