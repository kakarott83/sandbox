using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	[System.CLSCompliant(true)]
    public class OracleConfigurationManager : Cic.P000001.Common.ConfigurationManager.IAdapter
	{
		#region IAdapter methods
		// TODO BK 0 BK, Not tested
		public Cic.P000001.Common.AdapterState DeliverAdapterState()
		{
			try
			{
				// Invoke helper method
                return DeliverAdapterStateHelper.Execute();
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public System.Guid? ReserveConfigurationIdentifier(string userCode)
		{
			try
			{
				// Invoke helper method
                return ReserveConfigurationIdentifierHelper.Execute(userCode);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool CancelConfigurationIdentifierReservation(string userCode, System.Guid configurationIdentifier)
		{
			try
			{
				// Invoke helper method
				return CancelConfigurationIdentifierReservationHelper.Execute(userCode, configurationIdentifier);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool SaveConfiguration(string userCode, Cic.P000001.Common.ConfigurationManager.ConfigurationPackage configurationPackage)
		{
			try
			{
				// Invoke helper method
				return SaveConfigurationHelper.Execute(userCode, configurationPackage);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool RenameConfiguration(string userCode, System.Guid configurationIdentifier, string designation)
		{
			try
			{
				// Invoke helper method
				return RenameConfigurationHelper.Execute(userCode, configurationIdentifier, designation);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool PublishConfiguration(string userCode, System.Guid configurationIdentifier, bool isPublic)
		{
			try
			{
				// Invoke helper method
				return PublishConfigurationHelper.Execute(userCode, configurationIdentifier, isPublic);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool LockConfiguration(string userCode, System.Guid configurationIdentifier, bool isLocked)
		{
			try
			{
				// Invoke helper method
				return LockConfigurationHelper.Execute(userCode, configurationIdentifier, isLocked);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool MoveConfiguration(string userCode, System.Guid configurationIdentifier, string targetGroupName, string targetGroupDescription)
		{
			try
			{
				// Invoke helper method
				return MoveConfigurationHelper.Execute(userCode, configurationIdentifier, targetGroupName, targetGroupDescription);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool CopyConfiguration(string userCode, System.DateTime createdAt, string groupName, string groupDescription, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems)
		{
			try
			{
				// Invoke helper method
				return CopyConfigurationHelper.Execute(userCode, createdAt, groupName, groupDescription, sourceConfigurationIdentifier, targetConfigurationIdentifier, includeCatalogItems);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool CopyConfiguration(string userCode, System.DateTime createdAt, string targetConfigurationDesignation, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems)
		{
			try
			{
				// Invoke helper method
				return CopyConfigurationHelper.Execute(userCode, createdAt, targetConfigurationDesignation, sourceConfigurationIdentifier, targetConfigurationIdentifier, includeCatalogItems);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public bool DeleteConfiguration(string userCode, System.Guid configurationIdentifier)
		{
			try
			{
				// Invoke helper method
				return DeleteConfigurationHelper.Execute(userCode, configurationIdentifier);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public Cic.P000001.Common.ConfigurationManager.ConfigurationPackage LoadConfiguration(string userCode, System.Guid configurationIdentifier)
		{
			try
			{
				// Invoke helper method
				return LoadConfigurationHelper.Execute(userCode, configurationIdentifier);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public Cic.P000001.Common.ConfigurationManager.ConfigurationPackage[] LoadConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
		{
			try
			{
				// Invoke helper method
				return LoadConfigurationsHelper.Execute(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		public int CountConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
		{
			try
			{
				// Invoke helper method
				return CountConfigurationsHelper.Execute(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion
	}
}
