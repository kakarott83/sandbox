using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class ReserveConfigurationIdentifierHelper
	{
		#region Methods
		// TESTEDBY ReserveConfigurationIdentifierHelperTestFixture.WithoutUserCode
		// TESTEDBY ReserveConfigurationIdentifierHelperTestFixture.WithUserCode
		internal static System.Guid? Execute(string userCode)
		{
			Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS ConfigurationIdentifier;

			try
			{
				// Insert new configurationPackage identifier
                ConfigurationIdentifier =ConfigurationIdentifiersHelper.InsertNew(userCode);
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return new System.Guid(ConfigurationIdentifier.IDENTIFIER);
		}
		#endregion
	}
}
