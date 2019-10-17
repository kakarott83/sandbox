using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class DeliverAdapterStateHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.AdapterState Execute()
		{
			Cic.P000001.Common.AdapterState AdapterState;
			bool IsServiceable = true;
			string Message = null;

			try
			{
				// New configurationPackage manager
               DataContextHelper.GetDataContext();
			}
			catch
			{
				// Set state
				IsServiceable = false;
				// Set message
				// TODO BK 0 BK, Localize text
				Message = "Unable to open connection to database.";
			}

			// TODO BK 0 BK, Add other tests

			// Create adapter state
			AdapterState = new Cic.P000001.Common.AdapterState(AssemblyInfo.GetFullName(), IsServiceable, Message);
			// Return
			return AdapterState;
		}
		#endregion
	}
}
