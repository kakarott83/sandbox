// OWNER MK, 29-09-2008
namespace Cic.OpenLease.ServiceAccess.Merge.Recorder
{
	[System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "RecorderService", Namespace = "http://cic-software.de/contract")]
    public interface IRecorderService
    {
        #region Methods
		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
        long LogOn(string machine, string cicUser);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
        void LogOff(long sysCICLOG);


		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogDebug(long sysCICLOG, string message);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationDebug(long sysCICLOG, string message, string applicationName);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogDebugException(long sysCICLOG, System.Exception exception);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationDebugException(long sysCICLOG, System.Exception exception, string applicationName);


		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogInformation(long sysCICLOG, string message);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationInformation(long sysCICLOG, string message, string applicationName);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogInformationException(long sysCICLOG, System.Exception exception);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationInformationException(long sysCICLOG, System.Exception exception, string applicationName);


		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogWarning(long sysCICLOG, string message);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationWarning(long sysCICLOG, string message, string applicationName);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogWarningException(long sysCICLOG, System.Exception exception);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationWarningException(long sysCICLOG, System.Exception exception, string applicationName);

		
		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogError(long sysCICLOG, string message);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationError(long sysCICLOG, string message, string applicationName);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogErrorException(long sysCICLOG, System.Exception exception);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationErrorException(long sysCICLOG, System.Exception exception, string applicationName);


		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogFatalError(long sysCICLOG, string message);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationFatalError(long sysCICLOG, string message, string applicationName);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogFatalException(long sysCICLOG, System.Exception exception);

		// TEST BK 0 BK, Not tested
		[System.ServiceModel.OperationContract]
		void LogApplicationFatalException(long sysCICLOG, System.Exception exception, string applicationName);
		#endregion
	}
}
