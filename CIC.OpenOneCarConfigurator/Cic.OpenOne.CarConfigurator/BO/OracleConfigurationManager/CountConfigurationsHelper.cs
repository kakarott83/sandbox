using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class CountConfigurationsHelper
	{
		#region Methods
		// TESTEDBY CountConfigurationsHelperTestFixture.WithoutUserCode
		// TESTEDBY CountConfigurationsHelperTestFixture.WithValidData
		// TESTEDBY CountConfigurationsHelperTestFixture.CountByUserCodeWhereUserIsOwner
		// TESTEDBY CountConfigurationsHelperTestFixture.CountByUserCodeWhereUserIsOwnerAndDataSourceIdentifier
		// TESTEDBY CountConfigurationsHelperTestFixture.CountByUserCodeWhereUserIsOwnerAndDataSourceVersion
		// TESTEDBY CountConfigurationsHelperTestFixture.CountByUserCodeWhereUserIsOwnerAndDataSourceIdentifierPlusVersion
		// TESTEDBY CountConfigurationsHelperTestFixture.CountByUserCodeWhereUserIsOwnerAndGroupName
		// TESTEDBY CountConfigurationsHelperTestFixture.CountByUserCodeWhereUserIsOwnerAndConfigurationDesignation
		internal static int Execute(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
		{
			System.Collections.Generic.List<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS> ConfigurationList;

			try
			{
				// Get configurationPackage list
                ConfigurationList =LoadConfigurationsHelper.GetConfigurationList(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner);
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check configurations
			if (ConfigurationList != null)
			{
				// Return
				// NOTE BK, Two returns
				return ConfigurationList.Count;
			}
			else
			{
				// Return
				// NOTE BK, Two returns
				return 0;
			}
		}
		#endregion
	}
}
