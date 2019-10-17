using Cic.One.DTO;
using Cic.One.DTO.BN;
using Cic.One.GateBANKNOW.DTO;
using Cic.One.Utils.Util.Behaviour;
using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Cic.One.GateBANKNOW.Contract
{
    /// <summary>
    /// Methods for BankNow Partner Gateway
    /// </summary>
    [ServiceContract(Name = "IBNPartnerService", Namespace = "http://cic-software.de/One")]
    [WsdlDocumentationAttribute("BNOW Partner Gateway Service")]
    public interface IBNPartnerService
    {
        /// <summary>
        /// Delivers Service state information
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetStatusDto getStatus();

        /// <summary>
        /// Delivers a deeplink for the given offer
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetLinkDto getLink(igetLinkDto input);

        /// <summary>
        /// creates a new offer including calculation, customer, objectdata
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ocreateAntragDto createAntrag(icreateAntragDto input);


    }



}
