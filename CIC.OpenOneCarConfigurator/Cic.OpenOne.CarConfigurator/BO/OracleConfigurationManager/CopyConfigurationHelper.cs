using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
using System.Data.Common;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class CopyConfigurationHelper
	{
		#region Private constants
		private const string CnstUserDescription = null;
		#endregion

		#region Methods
		// TODO BK 0 BK, Not tested
		internal static bool Execute(string userCode, System.DateTime createdAt, string groupName, string groupDescription, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems)
		{
            Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended ConfigurationManager;
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User;
            Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS SourceConfigurationPackage;
            Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS TargetConfigurationIdentifier = null;
            Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS ConfigurationGroup;
            Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS TargetConfigurationPackage;
            Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS CatalogItem;
            Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS CatalogConfigurationPackage = null;
			bool SourceIdentifierExists;
			bool TargetIdentifierExists;
			bool TargetConfigurationIdentifierExists;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
			    // Throw exception
			    throw new System.ArgumentNullException("userCode");
			}

			// Check group name
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(groupName))
			{
				// Throw exception
				throw new System.ArgumentNullException("groupName");
			}

			// Trim group name
			// TODO BK 0 BK, The trimming is also used in the configurationPackage class
			groupName = groupName.Trim();

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
                    SourceIdentifierExists =ConfigurationPackagesHelper.IdentifierExists(sourceConfigurationIdentifier, ConfigurationManager);
                    // Check existence
                    TargetIdentifierExists =ConfigurationPackagesHelper.IdentifierExists(targetConfigurationIdentifier, ConfigurationManager);
                }
                catch
                {

                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw caught exception
                    throw;
                }

                // Check state
                if (!SourceIdentifierExists)
                {

                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw exception
                    // OK
                    throw new System.ArgumentException("sourceConfigurationIdentifier");
                }

                // Check state
                if (TargetIdentifierExists)
                {

                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw exception
                    // TODO BK 0 BK, Add exception class, Localize text
                    throw new System.ApplicationException("targetConfigurationIdentifier: An entry with this identifier still exists.");
                }

                try
                {
                    // Check configurationPackage identifier
                    TargetConfigurationIdentifierExists =ConfigurationIdentifiersHelper.IdentifierExists(targetConfigurationIdentifier, ConfigurationManager);
                    // Check state
                    if (TargetConfigurationIdentifierExists)
                    {
                        // Get configurationPackage identifier
                        TargetConfigurationIdentifier =ConfigurationIdentifiersHelper.GetItemForceCreating(userCode, targetConfigurationIdentifier, ConfigurationManager);
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
                if ((TargetConfigurationIdentifier == null) || (TargetConfigurationIdentifier.USERCODE != userCode))
                {

                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw exception
                    // TODO BK 0 BK, Add exception class, Localize text
                    throw new System.ApplicationException("Cannot find a valid reservation.");
                }

                // Get configurationPackage
                SourceConfigurationPackage =ConfigurationPackagesHelper.GetItem(sourceConfigurationIdentifier, ConfigurationManager);

                try
                {
                    // Get configurationPackage group
                    ConfigurationGroup =ConfigurationGroupsHelper.GetItemForceCreating(User.SYSCMUSERS, groupName, groupDescription, ConfigurationManager);
                    // Insert
                    // NOTE BK, The new configurationPackage group is used not the one of the source configurationPackage
                    // NOTE BK, The User.SYSCMUSERS is used not the SYSCMUSERS from the source configurationPackage
                    if (!SourceConfigurationPackage.CMDSRCVERSReference.IsLoaded)
                    {
                        SourceConfigurationPackage.CMDSRCVERSReference.Load();
                    }

                    TargetConfigurationPackage =ConfigurationPackagesHelper.Insert(targetConfigurationIdentifier, SourceConfigurationPackage.CMDSRCVERS.SYSCMDSRCVERS, ConfigurationGroup.SYSCMCONFGRPS, User.SYSCMUSERS, createdAt, SourceConfigurationPackage, ConfigurationManager);
                }
                catch
                {

                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw caught exception
                    throw;
                }

                // Check configurationPackage and state
                if ((TargetConfigurationPackage != null) && (includeCatalogItems))
                {
                    // Check catalog configurations
                    if ((SourceConfigurationPackage.CMCATCPKGSList != null) && (SourceConfigurationPackage.CMCATCPKGSList.Count > 0))
                    {
                        // Loop through catlog configurations
                        foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS LoopCatalogConfiguration in SourceConfigurationPackage.CMCATCPKGSList)
                        {
                            // Check catalog item
                            if (LoopCatalogConfiguration != null)
                            {
                                // Reset catalog item
                                CatalogItem = null;

                                try
                                {
                                    // Get item
                                    CatalogItem =CatalogItemsHelper.GetItem(LoopCatalogConfiguration.CMCATITEMS.NAME, ConfigurationManager);
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
                                        CatalogConfigurationPackage =CatalogConfigurationPackagesHelper.Insert(CatalogItem.SYSCMCATITEMS, TargetConfigurationPackage.SYSCMCPKGS, ConfigurationManager);
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
                }

                TransactionScope.Complete();
            }
			// Close
           DataContextHelper.CloseDataContext(ConfigurationManager);

			// Return
			return (CatalogConfigurationPackage != null);
		}

		// TODO BK 0 BK, Not tested
		internal static bool Execute(string userCode, System.DateTime createdAt, string targetConfigurationDesignation, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems)
		{
			
            Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended ConfigurationManager;
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User;
            Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS SourceConfigurationPackage;
            Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS TargetConfigurationIdentifier = null;
            Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS TargetConfigurationPackage;
            Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS CatalogItem;
            Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS CatalogConfigurationPackage = null;
			bool SourceIdentifierExists;
			bool TargetIdentifierExists;
			bool TargetConfigurationIdentifierExists;

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

			// Begin transaction
			
            DbTransaction trans =    ConfigurationManager.Connection.BeginTransaction();

			try
			{
				// Get user
                User =UsersHelper.GetItemForceCreating(userCode, CnstUserDescription, ConfigurationManager);
				// Check existence
                SourceIdentifierExists =ConfigurationPackagesHelper.IdentifierExists(sourceConfigurationIdentifier, ConfigurationManager);
				// Check existence
                TargetIdentifierExists =ConfigurationPackagesHelper.IdentifierExists(targetConfigurationIdentifier, ConfigurationManager);
			}
			catch
			{
				// Rollback transaction
                trans.Rollback();
				// Close
               DataContextHelper.CloseDataContext(ConfigurationManager);
				// Throw caught exception
				throw;
			}

			// Check state
			if (!SourceIdentifierExists)
			{
				// Rollback transaction
                trans.Rollback();
				// Close
               DataContextHelper.CloseDataContext(ConfigurationManager);
				// Throw exception
				// OK
				throw new System.ArgumentException("sourceConfigurationIdentifier");
			}

			// Check state
			if (TargetIdentifierExists)
			{
				// Rollback transaction
                trans.Rollback();
				// Close
               DataContextHelper.CloseDataContext(ConfigurationManager);
				// Throw exception
				// TODO BK 0 BK, Add exception class, Localize text
				throw new System.ApplicationException("targetConfigurationIdentifier: An entry with this identifier still exists.");
			}

			try
			{
				// Check configurationPackage identifier
                TargetConfigurationIdentifierExists =ConfigurationIdentifiersHelper.IdentifierExists(targetConfigurationIdentifier, ConfigurationManager);
				// Check state
				if (TargetConfigurationIdentifierExists)
				{
					// Get configurationPackage identifier
                    TargetConfigurationIdentifier =ConfigurationIdentifiersHelper.GetItemForceCreating(userCode, targetConfigurationIdentifier, ConfigurationManager);
				}
			}
			catch
			{
				// Rollback transaction
                trans.Rollback();
				// Close
               DataContextHelper.CloseDataContext(ConfigurationManager);
				// Throw caught exception
				throw;
			}

			// Check configurationPackage identifier
			if ((TargetConfigurationIdentifier == null) || (TargetConfigurationIdentifier.USERCODE != userCode))
			{
				// Rollback transaction
                trans.Rollback();
				// Close
               DataContextHelper.CloseDataContext(ConfigurationManager);
				// Throw exception
				// TODO BK 0 BK, Add exception class, Localize text
				throw new System.ApplicationException("Cannot find a valid reservation.");
			}

			// Get configurationPackage
            SourceConfigurationPackage =ConfigurationPackagesHelper.GetItem(sourceConfigurationIdentifier, ConfigurationManager);

			try
			{
				// Insert
				// NOTE BK, The new configurationPackage group is used not the one of the source configurationPackage
				// NOTE BK, The User.SYSCMUSERS is used not the SYSCMUSERS from the source configurationPackage
                TargetConfigurationPackage =ConfigurationPackagesHelper.Insert(targetConfigurationIdentifier, SourceConfigurationPackage.CMDSRCVERS.SYSCMDSRCVERS, SourceConfigurationPackage.CMCONFGRPS.SYSCMCONFGRPS, User.SYSCMUSERS, createdAt, SourceConfigurationPackage, targetConfigurationDesignation, ConfigurationManager);
			}
			catch
			{
				// Rollback transaction
                trans.Rollback();
				// Close
               DataContextHelper.CloseDataContext(ConfigurationManager);
				// Throw caught exception
				throw;
			}

			// Check configurationPackage and state
			if ((TargetConfigurationPackage != null) && (includeCatalogItems))
			{
				// Check catalog configurations
                if ((SourceConfigurationPackage.CMCATCPKGSList != null) && (SourceConfigurationPackage.CMCATCPKGSList.Count > 0))
				{
					// Loop through catlog configurations
					foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS LoopCatalogConfiguration in SourceConfigurationPackage.CMCATCPKGSList)
					{
						// Check catalog item
						if (LoopCatalogConfiguration != null)
						{
							// Reset catalog item
							CatalogItem = null;

							try
							{
								// Get item
                                CatalogItem =CatalogItemsHelper.GetItem(LoopCatalogConfiguration.CMCATITEMS.NAME, ConfigurationManager);
							}
							catch
							{
								// Rollback transaction
                                trans.Rollback();
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
                                    CatalogConfigurationPackage =CatalogConfigurationPackagesHelper.Insert(CatalogItem.SYSCMCATITEMS, TargetConfigurationPackage.SYSCMCPKGS, ConfigurationManager);
								}
								catch
								{
									// Rollback transaction
                                    trans.Rollback();
									// Close
                                   DataContextHelper.CloseDataContext(ConfigurationManager);
									// Throw caught exception
									throw;
								}
							}
						}
					}
				}
			}


			// Commit transaction
            trans.Commit();
			// Close
           DataContextHelper.CloseDataContext(ConfigurationManager);

			// Return
			return (CatalogConfigurationPackage != null);
		}
		#endregion
	}
}
