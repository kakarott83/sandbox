
using Cic.One.DTO;
using Cic.One.GateSL.Service;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateSL.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace Cic.OpenOne.GateSL.Service.Contract
{
    /// <summary>
    /// Contract Definition for Endpoint of SL Frontend Service Methods
    /// </summary>
    [ServiceContract(Name = "IslService", Namespace = "http://cic-software.de/GateSL")]
    public interface IslService
    {
        /// <summary>
        /// loads, recalculates and safes the offer
        /// </summary>
        /// <param name="sysId">offer id</param>
        /// <returns></returns>
        [OperationContract]
        orecalculateOfferDto recalculateOffer(long sysId);

        /// <summary>
        /// loads, recalculates and safes the offer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oBaseDto recalcOffer(recalcInput input);

    }
}
