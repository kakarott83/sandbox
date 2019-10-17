// OWNER: BK, 09-04-2008
namespace Cic.P000001.Common.ConfigurationManager
{
	[System.CLSCompliant(true)]
	public interface IAdapter : Cic.OpenOne.Util.Reflection.IAdapter
	{
		#region Methods
		// TESTEDBY AdapterInterfaceTestFixture.CheckDeliverAdapterState
		//Cic.P000001.Common.AdapterState DeliverAdapterState();

		// TESTEDBY AdapterInterfaceTestFixture.CheckReserveConfigurationIdentifier
		System.Guid? ReserveConfigurationIdentifier(string userCode);

		// TESTEDBY AdapterInterfaceTestFixture.CheckCancelConfigurationIdentifierReservation
		bool CancelConfigurationIdentifierReservation(string userCode, System.Guid configurationIdentifier);

		// TESTEDBY AdapterInterfaceTestFixture.CheckSaveConfiguration
		bool SaveConfiguration(string userCode, Cic.P000001.Common.ConfigurationManager.ConfigurationPackage configurationPackage);

		// TESTEDBY AdapterInterfaceTestFixture.CheckRenameConfiguration
		bool RenameConfiguration(string userCode, System.Guid configurationIdentifier, string designation);

		// TESTEDBY AdapterInterfaceTestFixture.CheckPublishConfiguration
		bool PublishConfiguration(string userCode, System.Guid configurationIdentifier, bool isPublic);

		// TESTEDBY AdapterInterfaceTestFixture.CheckLockConfiguration
		bool LockConfiguration(string userCode, System.Guid configurationIdentifier, bool isLocked);

		// TESTEDBY AdapterInterfaceTestFixture.CheckMoveConfiguration
		bool MoveConfiguration(string userCode, System.Guid configurationIdentifier, string targetGroupName, string targetGroupDescription);

		// TESTEDBY AdapterInterfaceTestFixture.CheckCopyConfiguration
		bool CopyConfiguration(string userCode, System.DateTime createdAt, string groupName, string groupDescription, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems);

		// TODO BK 0 BK, Not tested
		bool CopyConfiguration(string userCode, System.DateTime createdAt, string targetConfigurationDesignation, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems);

		// TESTEDBY AdapterInterfaceTestFixture.CheckDeleteConfiguration
		bool DeleteConfiguration(string userCode, System.Guid configurationIdentifier);

		// TESTEDBY AdapterInterfaceTestFixture.CheckLoadConfiguration
		Cic.P000001.Common.ConfigurationManager.ConfigurationPackage LoadConfiguration(string userCode, System.Guid configurationIdentifier);

		// TESTEDBY AdapterInterfaceTestFixture.CheckLoadConfigurations
		Cic.P000001.Common.ConfigurationManager.ConfigurationPackage[] LoadConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner);

		// TESTEDBY AdapterInterfaceTestFixture.CheckCountConfigurations
		int CountConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner);
		#endregion
	}
}
