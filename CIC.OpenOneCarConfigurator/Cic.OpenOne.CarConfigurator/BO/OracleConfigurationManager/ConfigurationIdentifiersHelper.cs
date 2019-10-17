using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
	#endregion

	//[System.CLSCompliant(true)]
	internal static class ConfigurationIdentifiersHelper
	{
		#region Methods
		// TESTEDBY IdentifierExistsTestFixture.WithoutDataContext
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

		// TESTEDBY IdentifierExistsTestFixture.WithNullDataContext
		// TESTEDBY IdentifierExistsTestFixture.WithDataContext
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

		// TESTEDBY GetNewIdentifierTestFixture.WithoutDataContext
		internal static System.Guid? GetNewIdentifier()
		{
			try
			{
				// Return
				return MyGetNewIdentifier(null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY GetNewIdentifierTestFixture.WithNull
		// TESTEDBY GetNewIdentifierTestFixture.WithDataContext
        internal static System.Guid? GetNewIdentifier(Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetNewIdentifier(configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY InsertTestFixture.WithoutCode
		// TESTEDBY InsertTestFixture.WithoutDataContext
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS Insert(string userCode, System.Guid identifier)
		{
			try
			{
				// Return
				return MyInsert(userCode, identifier, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY InsertTestFixture.WithNullDataContext
		// TESTEDBY InsertTestFixture.WithDataContext
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS Insert(string userCode, System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyInsert(userCode, identifier, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY InsertNewTestFixture.WithoutUserCode
		// TESTEDBY InsertNewTestFixture.WithoutDataContext
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS InsertNew(string userCode)
		{
			try
			{
				// Return
				return MyInsertNew(userCode, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY InsertNewTestFixture.WithNullDataContext
		// TESTEDBY InsertNewTestFixture.WithDataContext
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS InsertNew(string userCode, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyInsertNew(userCode, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.ConfigurationIdentifier GetItemForceCreating(string userCode, System.Guid identifier)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetItemForceCreating(userCode, identifier, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS GetItemForceCreating(string userCode, System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetItemForceCreating(userCode, identifier, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BO, Not tested
		internal static bool Delete(string userCode, System.Guid identifier)
		{
			try
			{
				// Return
				return MyDelete(userCode, identifier, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TODO BK 0 BO, Not tested
        internal static bool Delete(string userCode, System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyDelete(userCode, identifier, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
        private static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS MyIdentifierExists(System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS ConfigurationIdentifier = null;
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
				// Select configurationPackage identifier
                string IdentifierString = identifier.ToString();
                ConfigurationIdentifier = configurationManager.CMCIDENTS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS>(para => (para.IDENTIFIER == IdentifierString));
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
			return ConfigurationIdentifier;
		}

		// NOTE BK, This method will not throw any exception
		private static System.Guid? MyGetNewIdentifier(Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
			System.Guid? Identifier;
			bool Exists;
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

			do
			{
				// Create new identifier
				Identifier = System.Guid.NewGuid();
                
				// Check existence
                string IdentifierString = Identifier.ToString();
				Exists = configurationManager.CMCIDENTS.Any(para => (para.IDENTIFIER == IdentifierString));
			}
			while (Exists);

			// Check state
			if (Internal)
			{
				// Close
               DataContextHelper.CloseDataContext(configurationManager);
			}

			// Return
			return Identifier;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS MyInsert(string userCode, System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS ConfigurationIdentifier;
			bool Internal = false;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
				// Throw exception
				throw new System.ArgumentNullException("userCode");
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

			// New configurationPackage identifier
			ConfigurationIdentifier = new  Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS();
			// Set properties
			ConfigurationIdentifier.IDENTIFIER = identifier.ToString();
            ConfigurationIdentifier.USERCODE = userCode;
			// Insert
			configurationManager.AddToCMCIDENTS(ConfigurationIdentifier);

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

			// Reset configurationPackage identifier  
            ConfigurationIdentifier = null;

			// Get configurationPackage identifier
            string IdentifierString = identifier.ToString();


            ConfigurationIdentifier = configurationManager.CMCIDENTS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS>(para => (para.IDENTIFIER == IdentifierString));
			// Check configurationPackage identifier
			if (ConfigurationIdentifier == null)
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
			return ConfigurationIdentifier;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS MyInsertNew(string userCode, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS ConfigurationIdentifier;
			System.Guid? Identifier;
			bool Internal = false;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
				// Throw exception
				throw new System.ArgumentNullException("userCode");
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

			// Get new identifier
			Identifier = MyGetNewIdentifier(configurationManager);

			// Check identifier
			if (Identifier == null)
			{
				// Check state
				if (Internal)
				{
					// Close
                   DataContextHelper.CloseDataContext(configurationManager);
				}
				// Throw exception
				// TODO BK 0 BK, Add exception class, Localize text
				throw new System.ApplicationException("An unknown error occured while searching for a new configurationPackage identifier.");
			}

			try
			{
				// Insert
				ConfigurationIdentifier = MyInsert(userCode, (System.Guid)Identifier, configurationManager);
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

			// Check state
			if (Internal)
			{
				// Close
               DataContextHelper.CloseDataContext(configurationManager);
			}

			// Return
			return ConfigurationIdentifier;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS MyGetItemForceCreating(string userCode, System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS ConfigurationIdentifier;
			bool Internal = false;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
				// Throw exception
				throw new System.ArgumentNullException("userCode");
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

			// Get configurationPackage identifier
			ConfigurationIdentifier = MyIdentifierExists(identifier, configurationManager);

			// Check configurationPackage identifier
			if (ConfigurationIdentifier == null)
			{
				try
				{
					// Insert
					ConfigurationIdentifier = MyInsert(userCode, identifier, configurationManager);
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
			return ConfigurationIdentifier;
		}

        private static bool MyDelete(string userCode, System.Guid identifier, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
			bool Internal = false;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
				// Throw exception
				throw new System.ArgumentNullException("userCode");
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

			// Loop through configurationPackage identifiers
            string IdentifierString = identifier.ToString();
            foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCIDENTS LoopConfigurationIdentifier in configurationManager.CMCIDENTS.Where(para => ((para.IDENTIFIER == IdentifierString) && (para.USERCODE.ToUpper() == userCode.ToUpper()))))
			{
				// Delete on submit
				configurationManager.DeleteObject(LoopConfigurationIdentifier);
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
				// Throw caught exception
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
