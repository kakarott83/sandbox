// OWNER MK, 19-09-2008

namespace Cic.OpenLease.ServiceAccess.Merge.MembershipProvider
{
    /// <summary>
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "MembershipProviderService", Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.MembershipProvider")]
    public interface IMembershipProviderService
    {
        #region Methods
        /// <summary>
        /// </summary>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus" /></returns>
        [System.ServiceModel.OperationContract]
		Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus ValidateUser(string userName, string password);
        /// <summary>
        /// </summary>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus" /></returns>
		[System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ExtendedValidateUser(string userName, string password);
        /// <summary>
        /// </summary>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus" /></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ExtendedValidateDealer(string userName, string password, string dealerCode);
        /// <summary>
        /// Use this method to set default PEROLE.
        /// </summary>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        void SetDefaultPerole(long defaultPerole);
        /// <summary>
        /// Use this method to change the password.
        /// </summary>
        /// <returns>bool</returns>
        [System.ServiceModel.OperationContract]
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        /// <summary>
        /// Use this method to get the test message header.
        /// </summary>
        /// <returns>MessageHeader</returns>
        [System.ServiceModel.OperationContract]
        MessageHeader ReturnExampleMessageHeader();

        [System.ServiceModel.OperationContract]
        RfgDto[] DeliverPermissions();
        #endregion

        #region Methods (not in contract)
        //[System.ServiceModel.OperationContract]
        bool DeleteUser(string userName);
        
        //[System.ServiceModel.OperationContract]
        //RFG DeliverRfg(string serviceName, string methodName);
        //[System.ServiceModel.OperationContract]
        //Cic.OpenLease.Model.DdOl.PEROLE[] DeliverPeRole(string userName, string password);
        #endregion
    }
}
