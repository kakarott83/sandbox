using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
	#endregion

	//[System.CLSCompliant(true)]
	internal static class CatalogConfigurationPackagesHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS Insert(long SYSCMCATITEMS, long sysConfiguration, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
			// Check configurationPackage manager
			if (configurationManager == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("configurationManager");
			}

			try
			{
				// Return
				return MyInsert(SYSCMCATITEMS, sysConfiguration, configurationManager, null, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
        private static Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS MyInsert(long SYSCMCATITEMS, long sysConfiguration, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager, Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS catalogItem, Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS configurationPackage)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS CatalogConfigurationPackage;
			bool Internal = false;

			// Check configurationPackage manager
			if (configurationManager == null)
			{
				try
				{
					// New configurationPackage manager
                    configurationManager =DataContextHelper.GetDataContext();
					// Set state
					Internal = true;
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Check catalog item
			if ((catalogItem == null) || (catalogItem.SYSCMCATITEMS != SYSCMCATITEMS))
			{
				// Reset catalog item
				catalogItem = null;

				try
				{
					// Select catalog item
                    catalogItem = configurationManager.CMCATITEMS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS>(para => (para.SYSCMCATITEMS == SYSCMCATITEMS));
				}
				catch
				{
					// Ignore exception
				}

				// Check catalog item
				if (catalogItem == null)
				{
					// Check state
					if (Internal)
					{
						// Close
                       DataContextHelper.CloseDataContext(configurationManager);
					}
					// Throw exception
					// OK
					throw new System.Exception("missing fkey: SYSCMCATITEMS");
				}
			}

			// Check configurationPackage
			if ((configurationPackage == null) || (configurationPackage.CMDSRCVERS.SYSCMDSRCVERS != sysConfiguration))
			{
				// Reset configurationPackage
				configurationPackage = null;

				try
				{
					// Select configurationPackage
                    configurationPackage = configurationManager.CMCPKGS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>(para => (para.SYSCMCPKGS == sysConfiguration));
				}
				catch
				{
					// Ignore exception
				}

				// Check configurationPackage
				if (configurationPackage == null)
				{
					// Check state
					if (Internal)
					{
						// Close
						DataContextHelper.CloseDataContext(configurationManager);
					}
					// Throw exception
					// OK
					throw new System.Exception("missing fkey: sysConfiguration");
				}
			}

			// New catalog configurationPackage
			CatalogConfigurationPackage = new Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS();
			// Set properties
			CatalogConfigurationPackage.CMCATITEMS = catalogItem;
			CatalogConfigurationPackage.CMCPKGS = configurationPackage;

			// Insert
			configurationManager.AddToCMCATCPKGS(CatalogConfigurationPackage);

			try
			{
				// Submit changes
				configurationManager.SaveChanges();
			}
			catch
			{
				// Check state
				if (Internal)
				{
					// Close
					DataContextHelper.CloseDataContext(configurationManager);
				}
				// Throw caught exception
				throw;
			}

			// Reset catalog configurationPackage
			CatalogConfigurationPackage = null;
			// Get configurationPackage
            CatalogConfigurationPackage = configurationManager.CMCATCPKGS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS>(para => ((para.CMCATITEMS.SYSCMCATITEMS == SYSCMCATITEMS) && (para.CMCPKGS.SYSCMCPKGS == sysConfiguration)));
			// Check catalog configurationPackage
			if (CatalogConfigurationPackage == null)
			{
				// Check state
				if (Internal)
				{
					// Close
					DataContextHelper.CloseDataContext(configurationManager);
				}
				// Throw exception
				// TODO BK 0 BK, Add exception class, Localize text
				throw new System.ApplicationException("Unknown error.");
			}

			// Check state
			if (Internal)
			{
				// Close
				DataContextHelper.CloseDataContext(configurationManager);
			}

			// Return
			return CatalogConfigurationPackage;
		}
		#endregion
	}
}
