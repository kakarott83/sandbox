using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
	#endregion

	//[System.CLSCompliant(true)]
	internal static class DataSourceVersionsHelper
	{
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
		//internal static bool IdExists(long id,SqlCompact.ConfigurationManager configurationManager)
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

		//// TODO BK 0 BK, Not tested
		//internal static bool VersionExists(long SYSCMDATASRCS, string version)
		//{
		//    try
		//    {
		//        // Return
		//        return (MyVersionExists(SYSCMDATASRCS, version, null, null) != null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal static bool VersionExists(long SYSCMDATASRCS, string version,SqlCompact.ConfigurationManager configurationManager)
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
		//        return (MyVersionExists(SYSCMDATASRCS, version, configurationManager, null) != null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal static string GetNewVersion(long SYSCMDATASRCS)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetNewVersion(SYSCMDATASRCS, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal static string GetNewVersion(long SYSCMDATASRCS,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyGetNewVersion(SYSCMDATASRCS, configurationManager, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSourceVersion Insert(long SYSCMDATASRCS, string version, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsert(SYSCMDATASRCS, version, description, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSourceVersion Insert(long SYSCMDATASRCS, string version, string description,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyInsert(SYSCMDATASRCS, version, description, configurationManager, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSourceVersion InsertNew(long SYSCMDATASRCS)
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsertNew(SYSCMDATASRCS, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSourceVersion InsertNew(long SYSCMDATASRCS,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyInsertNew(SYSCMDATASRCS, configurationManager, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSourceVersion GetItemForceCreating(long SYSCMDATASRCS, string version, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetItemForceCreating(SYSCMDATASRCS, version, description, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS GetItemForceCreating(long SYSCMDATASRCS, string version, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetItemForceCreating(SYSCMDATASRCS, version, description, configurationManager, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(long SYSCMDATASRCS, string version)
		//{
		//    try
		//    {
		//        // Return
		//        return MyDelete(SYSCMDATASRCS, version, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(long SYSCMDATASRCS, string version,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyDelete(SYSCMDATASRCS, version, configurationManager, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}
		#endregion

		#region My methods
        private static Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS MyVersionExists(long SYSCMDATASRCS, string version, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager, Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS dataSource)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS DataSourceVersion = null;
			bool Internal = false;

			// Check version
			if (string.IsNullOrEmpty(version))
			{
				// Throw exception
				throw new System.ArgumentNullException("version");
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

			// Check data source
			if ((dataSource == null) || (dataSource.SYSCMDATASRCS != SYSCMDATASRCS))
			{
				// Reset data source
				dataSource = null;

				try
				{
					// Select data source
                    dataSource = configurationManager.CMDATASRCS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS>(para => (para.SYSCMDATASRCS == SYSCMDATASRCS));
				}
				catch
				{
					// Ignore exception
				}

				// Check data source
				if (dataSource == null)
				{
					// Check state
					if (Internal)
					{
						// Close
						DataContextHelper.CloseDataContext(configurationManager);
					}
					// Throw exception
					// OK
					throw new System.Exception("missing fkey: SYSCMDATASRCS");
				}
			}

			try
			{
				// Select data source version
                DataSourceVersion = configurationManager.CMDSRCVERS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS>(para => ((para.CMDATASRCS.SYSCMDATASRCS == SYSCMDATASRCS) && (para.VERSION.ToUpper() == version.ToUpper())));

                //Get reference
                if (!DataSourceVersion.CMDATASRCSReference.IsLoaded)
                {
                    DataSourceVersion.CMDATASRCSReference.Load();
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
			return DataSourceVersion;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS MyInsert(long SYSCMDATASRCS, string version, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager, Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS dataSource)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS DataSourceVersion;
			bool Internal = false;

			// Check version
			if (string.IsNullOrEmpty(version))
			{
				// Throw exception
				throw new System.ArgumentNullException("version");
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

			// Check data source
			if ((dataSource == null) || (dataSource.SYSCMDATASRCS != SYSCMDATASRCS))
			{
				// Reset data source
				dataSource = null;

				try
				{
					// Select data source
                    dataSource = configurationManager.CMDATASRCS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS>(para => (para.SYSCMDATASRCS == SYSCMDATASRCS));
				}
				catch
				{
					// Ignore exception
				}

				// Check data source
				if (dataSource == null)
				{
					// Check state
					if (Internal)
					{
						// Close
                       DataContextHelper.CloseDataContext(configurationManager);
					}
					// Throw exception
					// OK
					throw new System.Exception("missing fkey: SYSCMDATASRCS");
				}
			}

			// New data source version
            DataSourceVersion = new Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS();
           
			// Set properties
			DataSourceVersion.CMDATASRCS = dataSource;
			DataSourceVersion.VERSION = version;
			DataSourceVersion.DESCRIPTION = description;
			// Insert
			configurationManager.AddToCMDSRCVERS(DataSourceVersion);

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

			// Reset data source version
			DataSourceVersion = null;
			// Get data source version
            DataSourceVersion = configurationManager.CMDSRCVERS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS>(para => ((para.CMDATASRCS.SYSCMDATASRCS == SYSCMDATASRCS) && (para.VERSION.ToUpper() == version.ToUpper())));

            //Get reference
            if (!DataSourceVersion.CMDATASRCSReference.IsLoaded)
            {
                DataSourceVersion.CMDATASRCSReference.Load();
            }

			// Check data source version
			if (DataSourceVersion == null)
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
			return DataSourceVersion;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS MyGetItemForceCreating(long SYSCMDATASRCS, string version, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager, Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS dataSource)
		{
			Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS DataSourceVersion;
			bool Internal = false;

			// Check version
			if (string.IsNullOrEmpty(version))
			{
				// Throw exception
				throw new System.ArgumentNullException("version");
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

			// Check data source
			if ((dataSource == null) || (dataSource.SYSCMDATASRCS != SYSCMDATASRCS))
			{
				// Reset data source
				dataSource = null;

				try
				{
					// Select data source
                    dataSource = configurationManager.CMDATASRCS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS>(para => (para.SYSCMDATASRCS == SYSCMDATASRCS));
				}
				catch
				{
					// Ignore exception
				}

				// Check data source
				if (dataSource == null)
				{
					// Check state
					if (Internal)
					{
						// Close
                       DataContextHelper.CloseDataContext(configurationManager);
					}
					// Throw exception
					// OK
					throw new System.Exception("missing fkey: SYSCMDATASRCS");
				}
			}

			// Get data source version
			DataSourceVersion = MyVersionExists(SYSCMDATASRCS, version, configurationManager, dataSource);

			// Check data source version
			if (DataSourceVersion == null)
			{
				try
				{
					// Insert
					DataSourceVersion = MyInsert(SYSCMDATASRCS, version, description, configurationManager, dataSource);
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
			}

			// Check state
			if (Internal)
			{
				// Close
               DataContextHelper.CloseDataContext(configurationManager);
			}



			// Return
			return DataSourceVersion;
		}
		#endregion
	}
}
