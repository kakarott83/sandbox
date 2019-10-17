using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class SaveConfigurationHelper
	{
		#region Private constants
		private const string CnstUserDescription = null;
		#endregion

		#region Methods
		// TODO BK 0 BK, Not tested
		internal static bool Execute(string userCode, Cic.P000001.Common.ConfigurationManager.ConfigurationPackage commonConfiguration)
		{
            
            Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended ConfigurationManager;
            Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS ConfigurationIdentifier = null;
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User;
            Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS ConfigurationGroup;
            Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS DataSource;
            Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS DataSourceVersion;
            Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS ConfigurationPackage;
            Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS CatalogItem;
            Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS CatalogConfigurationPackage = null;
			bool IdentifierExists;
			bool ConfigurationIdentifierExists;
			bool Result = false;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
				// Throw exception
				throw new System.ArgumentNullException("userCode");
			}

			// Check common configurationPackage
			if (commonConfiguration == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("commonConfiguration");
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

			// Begin transaction
            using (System.Transactions.TransactionScope TransactionScope = new System.Transactions.TransactionScope())
            {
                try
                {
                    // Get user
                    User =UsersHelper.GetItemForceCreating(userCode, CnstUserDescription, ConfigurationManager);
                    // Check existence
                    IdentifierExists =ConfigurationPackagesHelper.IdentifierExists(commonConfiguration.Identifier, ConfigurationManager);
                }
                catch
                {
                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw caught exception
                    throw;
                }


                // Check state
                if (!IdentifierExists)
                {
                    // ConfigurationPackage does not exist
                    try
                    {
                        // Check configurationPackage identifier
                        ConfigurationIdentifierExists =ConfigurationIdentifiersHelper.IdentifierExists(commonConfiguration.Identifier, ConfigurationManager);
                        // Check state
                        if (ConfigurationIdentifierExists)
                        {
                            // Get configurationPackage identifier
                            ConfigurationIdentifier =ConfigurationIdentifiersHelper.GetItemForceCreating(userCode, commonConfiguration.Identifier, ConfigurationManager);
                        }
                    }
                    catch
                    {
                        // Close
                       DataContextHelper.CloseDataContext(ConfigurationManager);
                        // Throw caught exception
                        throw;
                    }

                    // Check configurationPackage identifier
                    if ((ConfigurationIdentifier == null) || (ConfigurationIdentifier.USERCODE != userCode))
                    {
                        // Close
                       DataContextHelper.CloseDataContext(ConfigurationManager);
                        // Throw exception
                        // TODO BK 0 BK, Add exception class, Localize text
                        throw new System.ApplicationException("Cannot find a valid reservation.");
                    }

                    try
                    {
                        // Get configurationPackage group
                        ConfigurationGroup =ConfigurationGroupsHelper.GetItemForceCreating(User.SYSCMUSERS, commonConfiguration.GroupName, commonConfiguration.GroupDescription, ConfigurationManager);
                        // Get data source
                        DataSource =DataSourcesHelper.GetItemForceCreating(commonConfiguration.DataSourceInformation.Identifier, commonConfiguration.DataSourceInformation.Designation, commonConfiguration.DataSourceInformation.Description, ConfigurationManager);
                        // Get data source version
                        DataSourceVersion =DataSourceVersionsHelper.GetItemForceCreating(DataSource.SYSCMDATASRCS, commonConfiguration.DataSourceInformation.Version, commonConfiguration.DataSourceInformation.VersionDescription, ConfigurationManager);
                        // Insert
                        ConfigurationPackage =ConfigurationPackagesHelper.Insert(DataSourceVersion.SYSCMDSRCVERS, ConfigurationGroup.SYSCMCONFGRPS, User.SYSCMUSERS, commonConfiguration.CreatedAt, commonConfiguration, ConfigurationManager);
                    }
                    catch
                    {
                        // Close
                       DataContextHelper.CloseDataContext(ConfigurationManager);
                        // Throw caught exception
                        throw;
                    }

                    // Check catalog items
                    if ((commonConfiguration.CatalogItems != null) && (commonConfiguration.CatalogItems.GetLength(0) > 0))
                    {
                        // Loop through catlog items
                        foreach (Cic.P000001.Common.ConfigurationManager.CatalogItem LoopCatalogItem in commonConfiguration.CatalogItems)
                        {
                            // Check catalog item
                            if (LoopCatalogItem != null)
                            {
                                // Reset catalog item
                                CatalogItem = null;

                                try
                                {
                                    // Insert
                                    CatalogItem =CatalogItemsHelper.GetItemForceCreating(LoopCatalogItem.Name, LoopCatalogItem.Description, ConfigurationManager);
                                }
                                catch
                                {
                                    // Close
                                   DataContextHelper.CloseDataContext(ConfigurationManager);
                                    // Throw caught exception
                                    throw;
                                }

                                // Check catalog item
                                if (CatalogItem != null)
                                {

                                    try
                                    {
                                        // Insert
                                        CatalogConfigurationPackage =CatalogConfigurationPackagesHelper.Insert(CatalogItem.SYSCMCATITEMS, ConfigurationPackage.SYSCMCPKGS, ConfigurationManager);
                                    }
                                    catch
                                    {

                                        // Close
                                       DataContextHelper.CloseDataContext(ConfigurationManager);
                                        // Throw caught exception
                                        throw;
                                    }
                                }
                            }
                        }
                    }

                    // Set result
                    Result = (CatalogConfigurationPackage != null);
                }
                else
                {
                    // Get configurationPackage
                    ConfigurationPackage =ConfigurationPackagesHelper.GetItem(commonConfiguration.Identifier, ConfigurationManager);

                    // Check configurationPackage
                    if ((ConfigurationPackage.CMUSERS.SYSCMUSERS == User.SYSCMUSERS) || ((ConfigurationPackage.ISPUBLIC == 1) && (ConfigurationPackage.ISLOCKED == 0)))
                    {
                        try
                        {
                            // Update
                            Result =ConfigurationPackagesHelper.UpdateStandard(ConfigurationPackage.SYSCMCPKGS, commonConfiguration.Designation, commonConfiguration.Description, commonConfiguration.LastModifiedAt, commonConfiguration.ConfigurationTreeNode, commonConfiguration.ConfigurationComponents, ConfigurationManager);
                        }
                        catch
                        {
                            // Close
                           DataContextHelper.CloseDataContext(ConfigurationManager);
                            // Throw caught exception
                            throw;
                        }
                    }
                    else
                    {
                        // Close
                       DataContextHelper.CloseDataContext(ConfigurationManager);
                        // Throw exception
                        throw new System.Exception("no permission");
                    }
                }

                // Commit transaction
                TransactionScope.Complete();
            }
			// Close
           DataContextHelper.CloseDataContext(ConfigurationManager);

			// Return
			return Result;
		}
		#endregion
	}
}
