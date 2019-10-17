using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
	#endregion

	//[System.CLSCompliant(true)]
	internal static class DataSourcesHelper
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

		//// TODO BK 0 BK, Not tested
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

		//// TODO BK 0 BK, Not tested
		//internal static bool IdentifierExists(System.Guid identifier,SqlCompact.ConfigurationManager configurationManager)
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
		//        return (MyIdentifierExists(identifier, configurationManager) != null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS GetItem(System.Guid identifier)
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

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSource GetItem(System.Guid identifier,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyIdentifierExists(identifier, configurationManager);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal static System.Guid? GetNewIdentifier()
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetNewIdentifier(null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal static System.Guid? GetNewIdentifier(SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyGetNewIdentifier(configurationManager);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSource Insert(System.Guid identifier, string designation, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsert(identifier, designation, description, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSource Insert(System.Guid identifier, string designation, string description,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyInsert(identifier, designation, description, configurationManager);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSource InsertNew()
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsertNew(null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSource InsertNew(SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyInsertNew(configurationManager);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.DataSource GetItemForceCreating(System.Guid identifier, string designation, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetItemForceCreating(identifier, designation, description, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS GetItemForceCreating(System.Guid identifier, string designation, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetItemForceCreating(identifier, designation, description, configurationManager);
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

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(System.Guid identifier,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyDelete(identifier, configurationManager);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}
		#endregion

		#region My methods
        private static Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS MyIdentifierExists(System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS DataSource = null;
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
				// Select data source
                string IdentifierString = identifier.ToString();
                DataSource = configurationManager.CMDATASRCS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS>(para => (para.IDENTIFIER == IdentifierString));

                //Get reference
                if (!DataSource.CMDSRCVERSList.IsLoaded)
                {
                    DataSource.CMDSRCVERSList.Load();
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
			return DataSource;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS MyInsert(System.Guid identifier, string designation, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS DataSource;
			bool Internal = false;

			// Check designation
			if (string.IsNullOrEmpty(designation))
			{
				// Throw exception
				throw new System.ArgumentNullException("designation");
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

			// New data source
			DataSource = new  Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS();
			// Set properties
			DataSource.IDENTIFIER = identifier.ToString();
			DataSource.DESIGNATION = designation;
			DataSource.DESCRIPTION = description;
			// Insert
			configurationManager.AddToCMDATASRCS(DataSource);

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

			// Reset data source
			DataSource = null;
			// Get data source
            string IdentifierString = identifier.ToString();
            DataSource = configurationManager.CMDATASRCS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS>(para => (para.IDENTIFIER == IdentifierString));

            // Get reference
            if (!DataSource.CMDSRCVERSList.IsLoaded)
            {
                DataSource.CMDSRCVERSList.Load();
            }

			// Check data source
			if (DataSource == null)
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

			
            //return
			return DataSource;
		}

		//private staticSqlCompact.DataSource MyInsertNew(SqlCompact.ConfigurationManager configurationManager)
		//{
		//   SqlCompact.DataSource DataSource;
		//    System.Guid? Identifier;
		//    bool Internal = false;

		//    // Check configurationPackage manager
		//    if (configurationManager == null)
		//    {
		//        try
		//        {
		//            // New configurationPackage manager
		//            configurationManager =DataContextHelper.GetDataContext();
		//            // Set state
		//            Internal = true;
		//        }
		//        catch
		//        {
		//            // Throw caught exception
		//            throw;
		//        }
		//    }

		//    // Get new identifier
		//    Identifier = MyGetNewIdentifier(configurationManager);

		//    // Check identifier
		//    if (Identifier == null)
		//    {
		//        // Check state
		//        if (Internal)
		//        {
		//            // Close
		//           DataContextHelper.CloseDataContext(configurationManager);
		//        }
		//        // Throw exception
		//        // TODO BK 0 BK, Add exception class, Localize text
		//        throw new System.ApplicationException("An unknown error occured while searching for a new data source.");
		//    }

		//    try
		//    {
		//        // Insert
		//        // TODO BK 0 BK, Localize text
		//        DataSource = MyInsert((System.Guid)Identifier, "Unnamed", null, configurationManager);
		//    }
		//    catch
		//    {
		//        // Check state
		//        if (Internal)
		//        {
		//            // Close
		//           DataContextHelper.CloseDataContext(configurationManager);
		//        }
		//        // Throw caught exception
		//        throw;
		//    }

		//    // Check state
		//    if (Internal)
		//    {
		//        // Close
		//       DataContextHelper.CloseDataContext(configurationManager);
		//    }

		//    // Return
		//    return DataSource;
		//}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS MyGetItemForceCreating(System.Guid identifier, string designation, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS DataSource;
			bool Internal = false;

			// Check designation
			if (string.IsNullOrEmpty(designation))
			{
				// Throw exception
				throw new System.ArgumentNullException("designation");
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

			// Get data source
			DataSource = MyIdentifierExists(identifier, configurationManager);

			// Check data source
			if (DataSource == null)
			{
				try
				{
					// Insert
					DataSource = MyInsert(identifier, designation, description, configurationManager);
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
			return DataSource;
		}

		//private static bool MyDelete(System.Guid identifier,SqlCompact.ConfigurationManager configurationManager)
		//{
		//    bool Internal = false;

		//    // Check configurationPackage manager
		//    if (configurationManager == null)
		//    {
		//        try
		//        {
		//            // New configurationPackage manager
		//            configurationManager =DataContextHelper.GetDataContext();
		//            // Set state
		//            Internal = true;
		//        }
		//        catch
		//        {
		//            // Throw caught exception
		//            throw;
		//        }
		//    }

		//    // Loop through data sources
		//    foreach (SqlCompact.DataSource LoopDataSource in configurationManager.DataSources.Where(para => (para.Identifier == identifier)))
		//    {
		//        // Delete on submit
		//        configurationManager.DataSources.DeleteOnSubmit(LoopDataSource);
		//    }

		//    try
		//    {
		//        // Submit changes
		//        configurationManager.SubmitChanges();
		//    }
		//    catch
		//    {
		//        // Check state
		//        if (Internal)
		//        {
		//            // Close
		//           DataContextHelper.CloseDataContext(configurationManager);
		//        }
		//        // Throw exception
		//        throw;
		//    }

		//    // Check state
		//    if (Internal)
		//    {
		//        // Close
		//       DataContextHelper.CloseDataContext(configurationManager);
		//    }

		//    // Return
		//    return true;
		//}
		#endregion
	}
}
