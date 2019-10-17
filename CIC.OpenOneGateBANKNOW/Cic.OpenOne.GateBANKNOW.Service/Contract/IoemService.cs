using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.GateOEM.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.OpenOne.GateOEM.Service.Contract
{
    [ServiceContract(Name = "IoemService", Namespace = "http://cic-software.de/GateOEM")]
    public interface IoemService
    {
        /// <summary>
        /// Processes an invoice from external provider
        /// </summary>
        /// <param name="sXmlInputDataSet"></param>
        /// <returns></returns>
        [OperationContract]
        string sendInvoice(string sXmlInputDataSet);

        /// <summary>
        /// returns the data for a credit line report of all active credit lines per boundary contract
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetCreditLimitDto getCreditLimits();

        /// <summary>
        /// returns a list of saldo information for all dealers
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetSaldoListDto getSaldolist();

    }
}
