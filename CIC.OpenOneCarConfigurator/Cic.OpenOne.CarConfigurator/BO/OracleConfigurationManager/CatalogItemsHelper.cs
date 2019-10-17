using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
	#endregion

	//[System.CLSCompliant(true)]
	internal static class CatalogItemsHelper
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

		//// TODO BK 0 BK, Not tested
		//internal static bool NameExists(string name)
		//{
		//    try
		//    {
		//        // Return
		//        return (MyNameExists(name, null) != null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal static bool NameExists(string name,SqlCompact.ConfigurationManager configurationManager)
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
		//        return (MyNameExists(name, configurationManager) != null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.CatalogItem GetItem(string name)
		//{
		//    try
		//    {
		//        // Return
		//        return MyNameExists(name, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS GetItem(string name, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyNameExists(name, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BK, Not tested
		//internal static string GetNewName()
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetNewName(null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal static string GetNewName(SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyGetNewName(configurationManager);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.CatalogItem Insert(string name, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsert(name, description, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.CatalogItem Insert(string name, string description,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyInsert(name, description, configurationManager);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.CatalogItem InsertNew()
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
		//internal staticSqlCompact.CatalogItem InsertNew(SqlCompact.ConfigurationManager configurationManager)
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
		//internal staticSqlCompact.CatalogItem GetItemForceCreating(string name, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetItemForceCreating(name, description, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS GetItemForceCreating(string name, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetItemForceCreating(name, description, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(string name)
		//{
		//    try
		//    {
		//        // Return
		//        return MyDelete(name, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(string name,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyDelete(name, configurationManager);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}
		#endregion

		#region My methods
        private static Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS MyNameExists(string name, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS CatalogItem = null;
			bool Internal = false;

			// Check name
			if (string.IsNullOrEmpty(name))
			{
				// Throw exception
				throw new System.ArgumentNullException("name");
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
				// Select catalog item
                CatalogItem = configurationManager.CMCATITEMS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS>(para => (para.NAME.ToUpper() == name.ToUpper()));
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
			return CatalogItem;
		}

		//// NOTE BK, This method will not throw any exception
		//private static string MyGetNewName(SqlCompact.ConfigurationManager configurationManager)
		//{
		//    string Name;
		//    bool Exists;
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

		//    do
		//    {
		//        // Create new name
		//        Name = System.Guid.NewGuid().ToString();
		//        // Check existence
		//        Exists = configurationManager.CatalogItems.Any(para => (para.Name.ToUpper() == Name.ToUpper()));
		//    }
		//    while (Exists);

		//    // Check state
		//    if (Internal)
		//    {
		//        // Close
		//       DataContextHelper.CloseDataContext(configurationManager);
		//    }

		//    // Return
		//    return Name;
		//}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS MyInsert(string name, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS CatalogItem;
			bool Internal = false;

			// Check name
			if (string.IsNullOrEmpty(name))
			{
				// Throw exception
				throw new System.ArgumentNullException("name");
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

			// New catalog item
			CatalogItem = new  Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS();
			// Set properties
			CatalogItem.NAME = name;
			CatalogItem.DESCRIPTION = description;
			// Insert
			configurationManager.AddToCMCATITEMS(CatalogItem);

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

			// Reset catalog item
			CatalogItem = null;
			// Get catalog item
            CatalogItem = configurationManager.CMCATITEMS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS>(para => (para.NAME.ToUpper() == name.ToUpper()));
			// Check catalog item
			if (CatalogItem == null)
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
			return CatalogItem;
		}

		//private staticSqlCompact.CatalogItem MyInsertNew(SqlCompact.ConfigurationManager configurationManager)
		//{
		//   SqlCompact.CatalogItem CatalogItem;
		//    string Name;
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

		//    // Get new name
		//    Name = MyGetNewName(configurationManager);

		//    // Check name
		//    if (Name == null)
		//    {
		//        // Check state
		//        if (Internal)
		//        {
		//            // Close
		//           DataContextHelper.CloseDataContext(configurationManager);
		//        }
		//        // Throw exception
		//        // TODO BK 0 BK, Add exception class, Localize text
		//        throw new System.ApplicationException("An unknown error occured while searching for a new catalog item.");
		//    }

		//    try
		//    {
		//        // Insert
		//        // TODO BK 0 BK, Localize text
		//        CatalogItem = MyInsert(Name, null, configurationManager);
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
		//    return CatalogItem;
		//}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS MyGetItemForceCreating(string name, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCATITEMS CatalogItem;
			bool Internal = false;

			// Check name
			if (string.IsNullOrEmpty(name))
			{
				// Throw exception
				throw new System.ArgumentNullException("name");
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

			// Get catalog item
			CatalogItem = MyNameExists(name, configurationManager);

			// Check catalog item
			if (CatalogItem == null)
			{
				try
				{
					// Insert
					CatalogItem = MyInsert(name, description, configurationManager);
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
			return CatalogItem;
		}

		//private static bool MyDelete(string name,SqlCompact.ConfigurationManager configurationManager)
		//{
		//    bool Internal = false;

		//    // Check name
		//    if (string.IsNullOrEmpty(name))
		//    {
		//        // Throw exception
		//        throw new System.ArgumentNullException("name");
		//    }

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

		//    // Loop through catalog items
		//    foreach (SqlCompact.CatalogItem LoopCatalogItem in configurationManager.CatalogItems.Where(para => (para.Name.ToUpper() == name.ToUpper())))
		//    {
		//        // Delete on submit
		//        configurationManager.CatalogItems.DeleteOnSubmit(LoopCatalogItem);
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
		//    return true;
		//}
		#endregion
	}
}
