using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class CancelConfigurationIdentifierReservationHelper
	{
		#region Methods
		// TESTEDBY CancelConfigurationIdentifierReservationHelperTestFixture.WithoutUserCode
		// TESTEDBY CancelConfigurationIdentifierReservationHelperTestFixture.WithUserCode
		internal static bool Execute(string userCode, System.Guid identifier)
		{
            Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended ConfigurationManager;
			bool Exists = false;
			bool Result = false;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
				// Throw exception
				throw new System.ArgumentNullException("userCode");
			}

			try
			{
				// Get data context
                ConfigurationManager =DataContextHelper.GetDataContext();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			try
			{
				// Search identifier
                Exists =ConfigurationPackagesHelper.IdentifierExists(identifier, ConfigurationManager);
			}
			catch
			{
				// Close
               DataContextHelper.CloseDataContext(ConfigurationManager);
				// Throw caught exception
				throw;
			}

			// Check existence
			if (!Exists)
			{
				try
				{
					// Delete
                    Result =ConfigurationIdentifiersHelper.Delete(userCode, identifier, ConfigurationManager);
				}
				catch
				{
					// Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
					// Throw caught exception
					throw;
				}
			}

			// Close
           DataContextHelper.CloseDataContext(ConfigurationManager);

			// Return
			return Result;
		}
		#endregion
	}
}
