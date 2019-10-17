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
        /// Creates an approval from the external VAP System, returning a unique new approval id and a deeplink into the frontoffice
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateApprovalDto createApproval(icreateApprovalDto input);

        /// <summary>
        /// returns the approvalinformation previously created with createApproval for the given approval id approval id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ogetApprovalInformationDto getApprovalInformation(igetApprovalInformationDto input);

        /// <summary>
        /// returns the deeplink only
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[OperationContract]
		ogetInformationDto getInformation (igetInformationDto inp);

        /// <summary>
        /// Processes an invoice from external provider
        /// </summary>
        /// <param name="sXmlInputDataSet"></param>
        /// <returns></returns>
        [OperationContract]
        string sendInvoice(string sXmlInputDataSet);

        /// <summary>
        /// Validates User with Password
        /// </summary>
        /// <param name="sXmlInputDataSet"></param>
        /// <returns>true when Login accepted, false otherwise</returns>
        [OperationContract]
        bool validateUser(String user, String password);

         /// <summary>
        /// Validates Guid and returns User Login
        /// </summary>
        /// <param name="sXmlInputDataSet"></param>
        /// <returns>The User Login or null if no Entry found</returns>
        [OperationContract]
        String validateGuid(String guid);

		/// <summary>
		/// Creates a Deeplink to a password change GUI for the current user 
		/// and sends an email to this user
		/// </summary>
		/// <param name="username">the username</param>
		/// <returns></returns>
		[OperationContract]
		void resetPassword (String username);

    }
}
