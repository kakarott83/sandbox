using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class LoadConfigurationsHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ConfigurationManager.ConfigurationPackage[] Execute(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
		{
			System.Collections.Generic.List<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS> ConfigurationList;
			Cic.P000001.Common.ConfigurationManager.ConfigurationPackage[] CommonConfigurations = null;
			long Index;

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
			if ((ConfigurationList != null) && (ConfigurationList.Count > 0))
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.ConfigurationManager.ConfigurationPackage>(ref CommonConfigurations, ConfigurationList.Count);
				// Reset index
				Index = -1;
				// Loop through dictionary
				foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS LoopConfiguration in ConfigurationList)
				{
					// Increase indes
					Index += 1;
					// Set value
                    CommonConfigurations[Index] =SqlCompactToCommonConvertHelper.ConvertConfiguration(LoopConfiguration);
				}
			}

			// Return
			return CommonConfigurations;
		}

		// TODO BK 0 BK, Not tested
        internal static System.Collections.Generic.List<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS> GetConfigurationList(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
		{
            Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended ConfigurationManager;
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User;
            Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS DataSource;
            Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS DataSourceVersion = null;
            Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS ConfigurationGroup = null;
            System.Collections.Generic.List<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS> ConfigurationList = null;
			bool UserCodeExists;
			bool DataSourceIdentifierExists;
			bool GroupNameExists;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
				// Throw exception
				throw new System.ArgumentNullException("userCode");
			}

			try
			{
				// Open connection
                ConfigurationManager =DataContextHelper.GetDataContext();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			try
			{
				// Check existence
                UserCodeExists =UsersHelper.CodeExists(userCode);
			}
			catch
			{
				// Close
				DataContextHelper.CloseDataContext(ConfigurationManager);
				// Throw caught exception
				throw;
			}

			// Check state
			if (UserCodeExists)
			{
				// Check data source identifier and data source version
				if ((dataSourceIdentifier != null) && (dataSourceVersion != null))
				{
					try
					{
						// Check existence
                        DataSourceIdentifierExists =DataSourcesHelper.IdentifierExists((System.Guid)dataSourceIdentifier);
					}
					catch
					{
						// Close
                       DataContextHelper.CloseDataContext(ConfigurationManager);
						// Throw caught exception
						throw;
					}

					// Check state
					if (DataSourceIdentifierExists)
					{
						// Get data source
                        DataSource =DataSourcesHelper.GetItem((System.Guid)dataSourceIdentifier);

						// Check data source versions
                        if ((DataSource.CMDSRCVERSList != null) && (DataSource.CMDSRCVERSList.Count > 0))
						{
							// Loop through data source versions
                            foreach (Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS LoopDataSourceVersion in DataSource.CMDSRCVERSList)
							{
								// Check versions
								if (LoopDataSourceVersion.VERSION == dataSourceVersion)
								{
									// Set data source version
									DataSourceVersion = LoopDataSourceVersion;
									// Exit
									break;
								}
							}
						}
					}
				}

				// Get user
                User =UsersHelper.GetItem(userCode, ConfigurationManager);

				// Check group name
				if (groupName != null)
				{
					try
					{
						// Check existence
                        GroupNameExists =ConfigurationGroupsHelper.NameExists(User.SYSCMUSERS, groupName, ConfigurationManager);
					}
					catch
					{
						// Close
                       DataContextHelper.CloseDataContext(ConfigurationManager);
						// Throw caught exception
						throw;
					}

					// Check state
					if (GroupNameExists)
					{
						// Get configurationPackage group
                        ConfigurationGroup =ConfigurationGroupsHelper.GetItem(User.SYSCMUSERS, groupName, ConfigurationManager);
					}
				}

				// Check data source version
				if (DataSourceVersion == null)
				{
					// Check configurationPackage group
					if (ConfigurationGroup == null)
					{
						// Get configurations
                        ConfigurationList =ConfigurationPackagesHelper.GetItems(User.SYSCMUSERS, null, null, configurationIdentifier, configurationDesignation, whereUserIsOwner, ConfigurationManager);
					}
					else
					{
						// Get configurations
                        ConfigurationList =ConfigurationPackagesHelper.GetItems(User.SYSCMUSERS, null, ConfigurationGroup.SYSCMCONFGRPS, configurationIdentifier, configurationDesignation, whereUserIsOwner, ConfigurationManager);
					}
				}
				else
				{
					// Check configurationPackage group
					if (ConfigurationGroup == null)
					{
						// Get configurations
                        ConfigurationList =ConfigurationPackagesHelper.GetItems(User.SYSCMUSERS, DataSourceVersion.SYSCMDSRCVERS, null, configurationIdentifier, configurationDesignation, whereUserIsOwner, ConfigurationManager);
					}
					else
					{
						// Get configurations
                        ConfigurationList =ConfigurationPackagesHelper.GetItems(User.SYSCMUSERS, DataSourceVersion.SYSCMDSRCVERS, ConfigurationGroup.SYSCMCONFGRPS, configurationIdentifier, configurationDesignation, whereUserIsOwner, ConfigurationManager);
					}
				}
			}

			// Close
           DataContextHelper.CloseDataContext(ConfigurationManager);

			// Return
			return ConfigurationList;
		}
		#endregion
	}
}
