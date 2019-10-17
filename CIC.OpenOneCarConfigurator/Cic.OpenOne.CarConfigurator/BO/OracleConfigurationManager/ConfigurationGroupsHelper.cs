using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
	#endregion

	//[System.CLSCompliant(true)]
	internal static class ConfigurationGroupsHelper
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
		//internal static bool NameExists(long SYSCMUSERS, string name)
		//{
		//    try
		//    {
		//        // Return
		//        return (MyNameExists(SYSCMUSERS, name, null, null) != null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
		internal static bool NameExists(long SYSCMUSERS, string name, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return (MyNameExists(SYSCMUSERS, name, configurationManager, null) != null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.ConfigurationGroup GetItem(long SYSCMUSERS, string name)
		//{
		//    try
		//    {
		//        // Return
		//        return MyNameExists(SYSCMUSERS, name, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS GetItem(long SYSCMUSERS, string name, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyNameExists(SYSCMUSERS, name, configurationManager, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS GetItem(long SYSCMCONFGRPS, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
                return MyNameExists(SYSCMCONFGRPS, configurationManager);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

		//// TODO BK 0 BK, Not tested
		//internal static string GetNewName(long SYSCMUSERS)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetNewName(SYSCMUSERS, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal static string GetNewName(long SYSCMUSERS,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyGetNewName(SYSCMUSERS, configurationManager, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.ConfigurationGroup Insert(long SYSCMUSERS, string name, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsert(SYSCMUSERS, name, description, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.ConfigurationGroup Insert(long SYSCMUSERS, string name, string description,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyInsert(SYSCMUSERS, name, description, configurationManager, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.ConfigurationGroup InsertNew(long SYSCMUSERS)
		//{
		//    try
		//    {
		//        // Return
		//        return MyInsertNew(SYSCMUSERS, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.ConfigurationGroup InsertNew(long SYSCMUSERS,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyInsertNew(SYSCMUSERS, configurationManager, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.ConfigurationGroup GetItemForceCreating(long SYSCMUSERS, string name, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetItemForceCreating(SYSCMUSERS, name, description, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS GetItemForceCreating(long SYSCMUSERS, string name, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetItemForceCreating(SYSCMUSERS, name, description, configurationManager, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(long SYSCMUSERS, string name)
		//{
		//    try
		//    {
		//        // Return
		//        return MyDelete(SYSCMUSERS, name, null, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(long SYSCMUSERS, string name,SqlCompact.ConfigurationManager configurationManager)
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
		//        return MyDelete(SYSCMUSERS, name, configurationManager, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}
		#endregion

		#region My methods
        private static Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS MyNameExists(long SYSCMUSERS, string name, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager, Cic.OpenOne.Common.Model.DdCcCm.CMUSERS user)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS ConfigurationGroup = null;
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
				if ((user == null) || (user.SYSCMUSERS != SYSCMUSERS))
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

			try
			{
				// Select configurationPackage group
                ConfigurationGroup = configurationManager.CMCONFGRPS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS>(para => ((para.CMUSERS.SYSCMUSERS == SYSCMUSERS) && (para.NAME.ToUpper() == name.ToUpper())));
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
			return ConfigurationGroup;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS MyNameExists(long SYSCMCONFGRPS, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
        {
            Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS ConfigurationGroup = null;
            bool Internal = false;

            // Check name
            if (SYSCMCONFGRPS == 0)
            {
                // Throw exception
                throw new System.ArgumentNullException("SYSCMCONFGRPS");
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
                // Select configurationPackage group
                ConfigurationGroup = configurationManager.CMCONFGRPS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS>(para => ((para.SYSCMCONFGRPS == SYSCMCONFGRPS)));
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
            return ConfigurationGroup;
        }

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS MyInsert(long SYSCMUSERS, string name, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager, Cic.OpenOne.Common.Model.DdCcCm.CMUSERS user)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS ConfigurationGroup;
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
				if ((user == null) || (user.SYSCMUSERS != SYSCMUSERS))
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

			// New configurationPackage group
            ConfigurationGroup = new Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS();
            
			// Set properties
			ConfigurationGroup.CMUSERS = user;
			ConfigurationGroup.NAME = name;
			ConfigurationGroup.DESCRIPTION = description;
			// Insert
			configurationManager.AddToCMCONFGRPS(ConfigurationGroup);

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

			// Reset configurationPackage group
			ConfigurationGroup = null;
			// Get configurationPackage group
            ConfigurationGroup = configurationManager.CMCONFGRPS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS>(para => ((para.CMUSERS.SYSCMUSERS == SYSCMUSERS) && (para.NAME.ToUpper() == name.ToUpper())));
			// Check configurationPackage group
			if (ConfigurationGroup == null)
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
			return ConfigurationGroup;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS MyGetItemForceCreating(long SYSCMUSERS, string name, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager, Cic.OpenOne.Common.Model.DdCcCm.CMUSERS user)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCONFGRPS ConfigurationGroup;
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
				if ((user == null) || (user.SYSCMUSERS != SYSCMUSERS))
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

			// Get configurationPackage group
			ConfigurationGroup = MyNameExists(SYSCMUSERS, name, configurationManager, user);

			// Check configurationPackage group
			if (ConfigurationGroup == null)
			{
				try
				{
					// Insert
					ConfigurationGroup = MyInsert(SYSCMUSERS, name, description, configurationManager, user);
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
			return ConfigurationGroup;
		}
		#endregion
	}
}
