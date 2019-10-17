using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.CarConfigurator.Util.Xml;
	
	#endregion

	//[System.CLSCompliant(true)]
	internal static class ConfigurationPackagesHelper
	{
		#region Enums
		// TODO BK 0 BK, Not tested
		//[System.CLSCompliant(true)]
		internal enum UpdateModeConstants : int
		{
			Standard = 0,
			Designation = 1,
			IsPublic = 2,
			IsLocked = 3,
			SYSCMCONFGRPS = 4,
		}
		#endregion

		#region Methods
		//// TODO BK 0 BK, Not tested
		//internal static bool IdExists(long id)
		//{
		//    try
		//    {
		//        // Return
		//        return (MyIdExists(id, null) != null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		//internal static bool IdExists(long id, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		//{
		//    // Check configurationPackage manager
		//    if (configurationManager == null)
		//    {
		//        // Throw exception
		//        throw new System.ArgumentNullException("configurationManager");
		//    }

		//    try
		//    {
		//        // Return
		//        return (MyIdExists(id, configurationManager) != null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		internal static bool IdentifierExists(System.Guid identifier)
		{
			try
			{
				// Return
				return (MyIdentifierExists(identifier, null) != null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
        internal static bool IdentifierExists(System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return (MyIdentifierExists(identifier, configurationManager) != null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS GetItem(System.Guid identifier)
		{
			try
			{
				// Return
				return MyIdentifierExists(identifier, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS GetItem(System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyIdentifierExists(identifier, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BK, Not tested
		//internal static System.Collections.Generic.List<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS> GetItems(long SYSCMUSERS, long? SYSCMDSRCVERS, long? SYSCMCONFGRPS, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetItems(SYSCMUSERS, SYSCMDSRCVERS, SYSCMCONFGRPS, configurationIdentifier, configurationDesignation, whereUserIsOwner, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static System.Collections.Generic.List<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS> GetItems(long SYSCMUSERS, long? SYSCMDSRCVERS, long? SYSCMCONFGRPS, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetItems(SYSCMUSERS, SYSCMDSRCVERS, SYSCMCONFGRPS, configurationIdentifier, configurationDesignation, whereUserIsOwner, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		//internal static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS Insert(long SYSCMDSRCVERS, long SYSCMCONFGRPS, long SYSCMUSERS, System.DateTime createdAt, Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS configurationPackage)
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsert(null, SYSCMDSRCVERS, SYSCMCONFGRPS, SYSCMUSERS, createdAt, configurationPackage, null,  null, null, null, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS Insert(long SYSCMDSRCVERS, long SYSCMCONFGRPS, long SYSCMUSERS, System.DateTime createdAt, Cic.P000001.Common.ConfigurationManager.ConfigurationPackage configurationPackage, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyInsert(null, SYSCMDSRCVERS, SYSCMCONFGRPS, SYSCMUSERS, createdAt, configurationPackage, null, null, configurationManager, null, null, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		//internal static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS Insert(System.Guid targetConfigurationIdentifier, long SYSCMDSRCVERS, long SYSCMCONFGRPS, long SYSCMUSERS, System.DateTime createdAt, Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS sourceConfiguration)
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsert(targetConfigurationIdentifier, SYSCMDSRCVERS, SYSCMCONFGRPS, SYSCMUSERS, createdAt, null, sourceConfiguration, null, null, null, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS Insert(System.Guid targetConfigurationIdentifier, long SYSCMDSRCVERS, long SYSCMCONFGRPS, long SYSCMUSERS, System.DateTime createdAt, Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS sourceConfiguration, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
			try
			{
				// Return
				return MyInsert(targetConfigurationIdentifier, SYSCMDSRCVERS, SYSCMCONFGRPS, SYSCMUSERS, createdAt, null, sourceConfiguration, null, configurationManager, null, null, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS Insert(System.Guid targetConfigurationIdentifier, long SYSCMDSRCVERS, long SYSCMCONFGRPS, long SYSCMUSERS, System.DateTime createdAt, Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS sourceConfiguration, string targetConfigurationDesignation, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
			try
			{
				// Return
				return MyInsert(targetConfigurationIdentifier, SYSCMDSRCVERS, SYSCMCONFGRPS, SYSCMUSERS, createdAt, null, sourceConfiguration, targetConfigurationDesignation, configurationManager, null, null, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		//internal static bool UpdateStandard(long sysConfiguration, string designation, string description, System.DateTime lastModifiedAt, Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode configurationTreeNode, Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] configurationComponents)
		//{
		//    try
		//    {
		//        // Return
		//        return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.Standard, sysConfiguration, null, designation, description, null, null, lastModifiedAt, configurationTreeNode, configurationComponents, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		internal static bool UpdateStandard(long sysConfiguration, string designation, string description, System.DateTime lastModifiedAt, Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode configurationTreeNode, Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] configurationComponents, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
                return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.Standard, sysConfiguration, null, designation, description, null, null, lastModifiedAt, configurationTreeNode, configurationComponents, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		//internal static bool UpdateDesignation(long sysConfiguration, string designation)
		//{
		//    try
		//    {
		//        // Return
		//        return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.Designation, sysConfiguration, null, designation, null, null, null, null, null, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		internal static bool UpdateDesignation(long sysConfiguration, string designation, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
                return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.Designation, sysConfiguration, null, designation, null, null, null, null, null, null, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		//internal static bool UpdateIsPublic(long sysConfiguration, bool isPublic)
		//{
		//    try
		//    {
		//        // Return
		//        return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.IsPublic, sysConfiguration, null, null, null, isPublic, null, null, null, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static bool UpdateIsPublic(long sysConfiguration, bool isPublic, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
                return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.IsPublic, sysConfiguration, null, null, null, isPublic, null, null, null, null, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		//internal static bool UpdateIsLocked(long sysConfiguration, bool isLocked)
		//{
		//    try
		//    {
		//        // Return
		//        return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.IsLocked, sysConfiguration, null, null, null, null, isLocked, null, null, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		internal static bool UpdateIsLocked(long sysConfiguration, bool isLocked, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
                return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.IsLocked, sysConfiguration, null, null, null, null, isLocked, null, null, null, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BK, Not tested
		//internal static bool UpdateSYSCMCONFGRPS(long sysConfiguration, long SYSCMCONFGRPS)
		//{
		//    try
		//    {
		//        // Return
		//        return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.SYSCMCONFGRPS, sysConfiguration, SYSCMCONFGRPS, null, null, null, null, null, null, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		internal static bool UpdateSYSCMCONFGRPS(long sysConfiguration, long SYSCMCONFGRPS, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
                return MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants.SYSCMCONFGRPS, sysConfiguration, SYSCMCONFGRPS, null, null, true, null, null, null, null, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(System.Guid identifier)
		//{
		//    try
		//    {
		//        // Return
		//        return MyDelete(identifier, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BO, Not tested
		internal static bool Delete(System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyDelete(identifier, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
        private static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS MyIdentifierExists(System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS ConfigurationPackage = null;
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

			try
			{
				// Select configurationPackage
                string IdentifierString = identifier.ToString();
                ConfigurationPackage = configurationManager.CMCPKGS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>(para => (para.CMCIDENTS.IDENTIFIER == IdentifierString));

                //Load references
                if (!ConfigurationPackage.CMCIDENTSReference.IsLoaded)
                {
                    ConfigurationPackage.CMCIDENTSReference.Load();
                }

                if (!ConfigurationPackage.CMCATCPKGSList.IsLoaded)
                {
                    ConfigurationPackage.CMCATCPKGSList.Load();
                }

                foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS CMCATCPKGSLoop in ConfigurationPackage.CMCATCPKGSList)
                {
                    if (!CMCATCPKGSLoop.CMCATITEMSReference.IsLoaded)
                    {
                        CMCATCPKGSLoop.CMCATITEMSReference.Load();
                    }

                    if (!CMCATCPKGSLoop.CMCPKGSReference.IsLoaded)
                    {
                        CMCATCPKGSLoop.CMCPKGSReference.Load();
                    }
                }

                if (!ConfigurationPackage.CMCONFGRPSReference.IsLoaded)
                {
                    ConfigurationPackage.CMCONFGRPSReference.Load();
                }

                if (!ConfigurationPackage.CMDSRCVERSReference.IsLoaded)
                {
                    ConfigurationPackage.CMDSRCVERSReference.Load();
                }

                if (!ConfigurationPackage.CMDSRCVERS.CMDATASRCSReference.IsLoaded)
                {
                    ConfigurationPackage.CMDSRCVERS.CMDATASRCSReference.Load();
                }

                if (!ConfigurationPackage.CMUSERSReference.IsLoaded)
                {
                    ConfigurationPackage.CMUSERSReference.Load();
                }
			}
			catch
			{
				// Ignore exception
			}

			// Check state
			if (Internal)
			{
				// Close
               DataContextHelper.CloseDataContext(configurationManager);
			}

			// Return
			return ConfigurationPackage;
		}

		// TODO BK 0 BK, Better code design required
		// NOTE BK, It is th truth. This code is not well designed. Maybe later
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static System.Collections.Generic.List<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS> MyGetItems(long SYSCMUSERS, long? SYSCMDSRCVERS, long? SYSCMCONFGRPS, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            System.Collections.Generic.List<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS> ConfigurationPackages = null;
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

			try
			{
				// Check state
				if (whereUserIsOwner)
				{
					// Select configurations: only valid user
                    ConfigurationPackages = configurationManager.CMCPKGS.Where(para => (para.CMUSERS.SYSCMUSERS == SYSCMUSERS)).ToList<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>();
                    
				}
				else
				{
					// Select configurations: valid user or public and not locked
                    ConfigurationPackages = configurationManager.CMCPKGS.Where(para => ((para.CMUSERS.SYSCMUSERS == SYSCMUSERS) || ((para.ISPUBLIC == 1) && (para.ISLOCKED == 0)))).ToList<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>();
				}

				// Check object
				if ((ConfigurationPackages != null) && (ConfigurationPackages.Count > 0))
				{
                    //Load references
                    foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS CMCATCPKGSLoop in ConfigurationPackages)
                    {
                        if (!CMCATCPKGSLoop.CMCIDENTSReference.IsLoaded)
                        {
                            CMCATCPKGSLoop.CMCIDENTSReference.Load();
                        }

                        if (!CMCATCPKGSLoop.CMCONFGRPSReference.IsLoaded)
                        {
                            CMCATCPKGSLoop.CMCONFGRPSReference.Load();
                        }

                        if (!CMCATCPKGSLoop.CMDSRCVERSReference.IsLoaded)
                        {
                            CMCATCPKGSLoop.CMDSRCVERSReference.Load();
                        }

                        if (!CMCATCPKGSLoop.CMDSRCVERS.CMDATASRCSReference.IsLoaded)
                        {
                            CMCATCPKGSLoop.CMDSRCVERS.CMDATASRCSReference.Load();
                        }

                        if (!CMCATCPKGSLoop.CMUSERSReference.IsLoaded)
                        {
                            CMCATCPKGSLoop.CMUSERSReference.Load();
                        }

                        if (!CMCATCPKGSLoop.CMCATCPKGSList.IsLoaded)
                        {
                            CMCATCPKGSLoop.CMCATCPKGSList.Load();
                        }

                        foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS CMCATCPKGSSeconeLoop in CMCATCPKGSLoop.CMCATCPKGSList)
                        {
                            if (!CMCATCPKGSSeconeLoop.CMCATITEMSReference.IsLoaded)
                            {
                                CMCATCPKGSSeconeLoop.CMCATITEMSReference.Load();
                            }

                            if (!CMCATCPKGSSeconeLoop.CMCPKGSReference.IsLoaded)
                            {
                                CMCATCPKGSSeconeLoop.CMCPKGSReference.Load();
                            }
                        }

                        
                    }

					// Check sys configurationPackage identifier
					if (configurationIdentifier != null)
					{
						// Select configurations: valid data source version
                        string IdentifierString = configurationIdentifier.ToString();
                        
                        //Load reference
                        foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS CMCATPKGSLoop in ConfigurationPackages)
                        {
                            if (!CMCATPKGSLoop.CMCIDENTSReference.IsLoaded)
                            {
                                CMCATPKGSLoop.CMCIDENTSReference.Load();
                            }
                        }

                        ConfigurationPackages = ConfigurationPackages.Where(para => (para.CMCIDENTS.IDENTIFIER == IdentifierString)).ToList<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>();
					}
				}

				// Check object
				if ((ConfigurationPackages != null) && (ConfigurationPackages.Count > 0))
				{
					// Check sys configurationPackage designation
					if (configurationDesignation != null)
					{
						// Select configurations: valid data source version
                        ConfigurationPackages = ConfigurationPackages.Where(para => (para.DESIGNATION.ToUpperInvariant() == configurationDesignation.ToUpperInvariant())).ToList<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>();
					}
				}

				// Check object
				if ((ConfigurationPackages != null) && (ConfigurationPackages.Count > 0))
				{
					// Check sys configurationPackage group
					if (SYSCMCONFGRPS != null)
					{
						// Select configurations: valid data source version
                       
                        ConfigurationPackages = ConfigurationPackages.Where(para => (para.CMCONFGRPS.SYSCMCONFGRPS == SYSCMCONFGRPS)).ToList<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>();
					}
				}

				// Check object
				if ((ConfigurationPackages != null) && (ConfigurationPackages.Count > 0))
				{
					// Check sys data source version
					if (SYSCMDSRCVERS != null)
					{
						// Select configurations: valid data source version
                        ConfigurationPackages = ConfigurationPackages.Where(para => (para.CMDSRCVERS.SYSCMDSRCVERS == SYSCMDSRCVERS)).ToList<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>();
					}
				}
			}
			catch
			{
				// Ignore exception
			}

			// Check state
			if (Internal)
			{
				// Close
               DataContextHelper.CloseDataContext(configurationManager);
			}

			// Return
			return ConfigurationPackages;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS MyInsert(System.Guid? targetConfigurationIdentifier, long SYSCMDSRCVERS, long SYSCMCONFGRPS, long SYSCMUSERS, System.DateTime createdAt, Cic.P000001.Common.ConfigurationManager.ConfigurationPackage commonConfiguration, Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS sourceConfiguration, string targetConfigurationDesignation, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager, Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS dataSourceVersion, Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS configurationGroup, Cic.OpenOne.Common.Model.DdCcCm.CMUSERS user)
		{
			Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS ConfigurationPackage;
            
			bool Internal = false;

			// Check target identifier
			if (targetConfigurationIdentifier == null)
			{
				// Check common configurationPackage
				if (commonConfiguration == null)
				{
					// Throw exception
					throw new System.ArgumentNullException("commonConfiguration");
				}
				// Reset source configurationPackage
				sourceConfiguration = null;
				targetConfigurationDesignation = null;
			}
			else
			{
				// Check source configurationPackage
				if (sourceConfiguration == null)
				{
					// Throw exception
					throw new System.ArgumentNullException("sourceConfiguration");
				}
				// Check target configuration designation
				if (targetConfigurationDesignation == null)
				{
					// Throw exception
					throw new System.ArgumentNullException("targetConfigurationDesignation");
				}
				// Reset common configurationPackage
				commonConfiguration = null;
			}

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

			// Check data source version
			if ((dataSourceVersion == null) || (dataSourceVersion.SYSCMDSRCVERS != SYSCMDSRCVERS))
			{
				// Reset data source version
				dataSourceVersion = null;

				try
				{
					// Select data source version
                    dataSourceVersion = configurationManager.CMDSRCVERS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS>(para => (para.SYSCMDSRCVERS == SYSCMDSRCVERS));
				}
				catch
				{
					// Ignore exception
				}

				// Check data source version
				if (dataSourceVersion == null)
				{
					// Check state
					if (Internal)
					{
						// Close
						DataContextHelper.CloseDataContext(configurationManager);
					}
					// Throw exception
					// OK
					throw new System.Exception("missing fkey: SYSCMDSRCVERS");
				}
			}

			// Check configurationPackage group
			if ((configurationGroup == null) || (configurationGroup.SYSCMCONFGRPS != SYSCMCONFGRPS))
			{
				// Reset configurationPackage group
				configurationGroup = null;

				try
				{
					// Select configurationPackage group
                    configurationGroup = configurationManager.CMCONFGRPS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS>(para => (para.SYSCMCONFGRPS == SYSCMCONFGRPS));
				}
				catch
				{
					// Ignore exception
				}

				// Check configurationPackage group
				if (configurationGroup == null)
				{
					// Check state
					if (Internal)
					{
						// Close
                       DataContextHelper.CloseDataContext(configurationManager);
					}
					// Throw exception
					// OK
					throw new System.Exception("missing fkey: SYSCMCONFGRPS");
				}
			}

			// Check user
			if ((user == null) || (user.SYSCMUSERS != SYSCMUSERS))
			{
				// Reset user
				user = null;

				try
				{
					// Select user
                    user = configurationManager.CMUSERS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMUSERS>(para => (para.SYSCMUSERS == SYSCMUSERS));
				}
				catch
				{
					// Ignore exception
				}

				// Check user
				if (user == null)
				{
					// Check state
					if (Internal)
					{
						// Close
                       DataContextHelper.CloseDataContext(configurationManager);
					}
					// Throw exception
					// OK
					throw new System.Exception("missing fkey: SYSCMUSERS");
				}
			}

			// Check SYSCMUSERS
			if (configurationGroup.CMUSERS.SYSCMUSERS != SYSCMUSERS)
			{
				// Check state
				if (Internal)
				{
					// Close
                   DataContextHelper.CloseDataContext(configurationManager);
				}
				// Throw exception
				// TODO BK 0 BK, Add exception class, Localize text
				throw new System.ApplicationException("SYSCMCONFGRPS: The referenced entry has an invalid value for SYSCMUSERS.");
			}

			// New configurationPackage
			ConfigurationPackage = new Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS();
			// Set properties
			ConfigurationPackage.CMDSRCVERS = dataSourceVersion;
			ConfigurationPackage.CMCONFGRPS = configurationGroup;
			ConfigurationPackage.CMUSERS = user;
			// Check common configurationPackage
			if (commonConfiguration != null)
			{
				// Set properties
                ConfigurationPackage.CMCIDENTS = ConfigurationIdentifiersHelper.GetItemForceCreating(user.CODE, commonConfiguration.Identifier, configurationManager);
                ConfigurationPackage.DESIGNATION = commonConfiguration.Designation;
                ConfigurationPackage.DESCRIPTION = commonConfiguration.Description;
                ConfigurationPackage.CRATEDAT = commonConfiguration.CreatedAt;
				ConfigurationPackage.LASTMODIFIEDAT = commonConfiguration.CreatedAt; // NOTE BK, Same date
                ConfigurationPackage.ISPUBLIC = commonConfiguration.IsPublic ? (byte) 1 : (byte) 0;
                ConfigurationPackage.ISLOCKED = commonConfiguration.IsLocked ? (byte)1 : (byte)0;
				ConfigurationPackage.SELECTEDLANGUAGE = commonConfiguration.Setting.SelectedLanguage;
                ConfigurationPackage.SELECTEDCURRENCY = commonConfiguration.Setting.SelectedCurrency;
				// Serialize tree node
				ConfigurationPackage.XMLTREENODE =SerializationHelper.SerializeUTF8(commonConfiguration.ConfigurationTreeNode);
				// Serialize components
				ConfigurationPackage.XMLCOMPONENTS = SerializationHelper.SerializeUTF8(commonConfiguration.ConfigurationComponents);
			}
			else
			{
				// Set properties
                ConfigurationPackage.CMCIDENTS = ConfigurationIdentifiersHelper.GetItemForceCreating(user.CODE, targetConfigurationIdentifier.GetValueOrDefault(), configurationManager);;
				ConfigurationPackage.DESIGNATION = targetConfigurationDesignation;
                ConfigurationPackage.DESCRIPTION = sourceConfiguration.DESCRIPTION;
				ConfigurationPackage.CRATEDAT = createdAt;
				ConfigurationPackage.LASTMODIFIEDAT = createdAt; // NOTE BK, Same date
                ConfigurationPackage.ISPUBLIC = sourceConfiguration.ISPUBLIC;
                ConfigurationPackage.ISLOCKED = sourceConfiguration.ISLOCKED;
				ConfigurationPackage.SELECTEDLANGUAGE = sourceConfiguration.SELECTEDLANGUAGE;
				ConfigurationPackage.SELECTEDCURRENCY = sourceConfiguration.SELECTEDCURRENCY;
				// Serialize tree node
				ConfigurationPackage.XMLTREENODE = sourceConfiguration.XMLTREENODE;
				// Serialize components
				ConfigurationPackage.XMLCOMPONENTS = sourceConfiguration.XMLCOMPONENTS;
			}

			// Insert
			configurationManager.AddToCMCPKGS(ConfigurationPackage);

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

			// Reset configurationPackage
			ConfigurationPackage = null;
			// Check common configurationPackage
			if (commonConfiguration != null)
			{
				// Get configurationPackage
                string IdentifierString = commonConfiguration.Identifier.ToString();
                ConfigurationPackage = configurationManager.CMCPKGS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>(para => (para.CMCIDENTS.IDENTIFIER == IdentifierString));
			}
			else
			{
				// Get configurationPackage
                string IdentifierString = targetConfigurationIdentifier.ToString();
                ConfigurationPackage = configurationManager.CMCPKGS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>(para => (para.CMCIDENTS.IDENTIFIER == IdentifierString));
			}
			// Check configurationPackage
			if (ConfigurationPackage == null)
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
			return ConfigurationPackage;
		}

		// TODO BK 0 BK, Better code design required
		// NOTE BK, It is th truth. This code is not well designed. Maybe later
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static bool MyUpdate(ConfigurationPackagesHelper.UpdateModeConstants updateModeConstants, long sysConfiguration, long? SYSCMCONFGRPS, string designation, string description, bool? isPublic, bool? isLocked, System.DateTime? lastModifiedAt, Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode configurationTreeNode, Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] configurationComponents, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS ConfigurationPackage = null;
			int Index;
			bool Internal = false;

			// Check component type constant
            if (!System.Enum.IsDefined(typeof(ConfigurationPackagesHelper.UpdateModeConstants), updateModeConstants))
			{
				// Throw exception
				throw new System.ArgumentOutOfRangeException("updateModeConstants");
			}

			// Check sys configurationPackage group
            if ((SYSCMCONFGRPS == null) && (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.SYSCMCONFGRPS))
			{
				// Throw exception
				throw new System.ArgumentNullException("SYSCMCONFGRPS");
			}


			// Check designation
            if ((string.IsNullOrEmpty(designation)) && ((updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.Standard) || (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.Designation)))
			{
				// Throw exception
				throw new System.ArgumentNullException("designation");
			}

			// Check ispublic
            if ((isPublic == null) && (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.IsPublic))
			{
				// Throw exception
				throw new System.ArgumentNullException("isPublic");
			}

			// Check islocked
            if ((isLocked == null) && (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.IsLocked))
			{
				// Throw exception
				throw new System.ArgumentNullException("isLocked");
			}

			// Check last modiified at
            if ((lastModifiedAt == null) && (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.Standard))
			{
				// Throw exception
				throw new System.ArgumentNullException("lastModifiedAt");
			}

			// Check configurationPackage tree node
            if ((configurationTreeNode == null) && (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.Standard))
			{
				// Throw exception
				throw new System.ArgumentNullException("configurationTreeNode");
			}

			// Check configurationPackage components
            if ((configurationComponents != null) && (configurationComponents.GetLength(0) > 0) && (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.Standard))
			{
				// Reset index
				Index = 0;
				// Loop through configurationPackage components
				foreach (Cic.P000001.Common.ConfigurationManager.ConfigurationComponent LoopConfigurationComponent in configurationComponents)
				{
					// Increase Index
					Index += 1;
					// Check configurationPackage component
					if (LoopConfigurationComponent == null)
					{
						// Throw exception
						throw new System.ArgumentNullException("configurationComponents[" + Index.ToString() + "]");
					}
				}
			}

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

			try
			{
				// Select configurationPackage
                ConfigurationPackage = configurationManager.CMCPKGS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS>(para => (para.SYSCMCPKGS == sysConfiguration));
			}
			catch
			{
				// Ignore exception
			}

			// Check configurationPackage
			if (ConfigurationPackage == null)
			{
				// Check state
				if (Internal)
				{
					// Close
                   DataContextHelper.CloseDataContext(configurationManager);
				}
				// Throw exception
				// OK
				throw new System.ArgumentException("sysConfiguration");
			}

			// Check state
            if (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.Standard)
			{
				// Set properties
				ConfigurationPackage.DESIGNATION = designation;
				ConfigurationPackage.DESCRIPTION = description;
				ConfigurationPackage.LASTMODIFIEDAT = (System.DateTime)lastModifiedAt;
				// Serialize tree node
				ConfigurationPackage.XMLTREENODE = SerializationHelper.SerializeUTF8(configurationTreeNode);
				// Serialize components
				ConfigurationPackage.XMLCOMPONENTS = SerializationHelper.SerializeUTF8(configurationComponents);
			}
			// Check state
            if (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.Designation)
			{
				// Set properties
				ConfigurationPackage.DESIGNATION = designation;
			}
			// Check state
            if (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.IsPublic)
			{
				// Set properties
				ConfigurationPackage.ISPUBLIC = isPublic.GetValueOrDefault() == false ? (byte) 0 : (byte) 1;
			}
			// Check state
            if (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.IsLocked)
			{
				// Set properties
                ConfigurationPackage.ISLOCKED = isLocked.GetValueOrDefault() == false ? (byte) 0 : (byte) 1;
			}
			// Check state
            if (updateModeConstants ==ConfigurationPackagesHelper.UpdateModeConstants.SYSCMCONFGRPS)
			{
                if (!ConfigurationPackage.CMCONFGRPSReference.IsLoaded)
                {
                    ConfigurationPackage.CMCONFGRPSReference.Load();
                }
				// Set properties
				ConfigurationPackage.CMCONFGRPS = ConfigurationGroupsHelper.GetItem((long)SYSCMCONFGRPS, configurationManager);
			}

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
				// Throw exception
				throw;
			}

			// Check state
			if (Internal)
			{
				// Close
               DataContextHelper.CloseDataContext(configurationManager);
			}

			// Return
			return true;
		}

        private static bool MyDelete(System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
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

			// Loop through configurations
            string IdentifierString = identifier.ToString();
            foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS LoopConfiguration in configurationManager.CMCPKGS.Where(para => (para.CMCIDENTS.IDENTIFIER == IdentifierString)))
			{
				// Delete on submit
				configurationManager.DeleteObject(LoopConfiguration);
			}

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
				// Throw exception
				throw;
			}

			// Check state
			if (Internal)
			{
				// Close
               DataContextHelper.CloseDataContext(configurationManager);
			}

			// Return
			return true;
		}
		#endregion
	}
}
