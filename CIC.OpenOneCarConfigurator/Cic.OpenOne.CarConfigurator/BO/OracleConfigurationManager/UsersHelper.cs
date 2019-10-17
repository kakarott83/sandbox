using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
	#endregion

	//[System.CLSCompliant(true)]
	internal static class UsersHelper
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

		// TESTEDBY CodeExistsTestFixture.WithoutCode
		// TESTEDBY CodeExistsTestFixture.WithoutDataContext
		internal static bool CodeExists(string code)
		{
			try
			{
				// Return
				return (MyCodeExists(code, null) != null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY CodeExistsTestFixture.WithNullDataContext
		// TESTEDBY CodeExistsTestFixture.WithDataContext
        internal static bool CodeExists(string code, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return (MyCodeExists(code, configurationManager) != null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.User GetItem(string code)
		//{
		//    try
		//    {
		//        // Return
		//        return MyCodeExists(code, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS GetItem(string code, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyCodeExists(code, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY GetNewCodeTestFixture.WithoutDataContext
		internal static string GetNewCode()
		{
			try
			{
				// Return
				return MyGetNewCode(null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY GetNewCodeTestFixture.WithNullDataContext
		// TESTEDBY GetNewCodeTestFixture.WithDataContext
        internal static string GetNewCode(Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetNewCode(configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY InsertTestFixture.WithoutCode
		// TESTEDBY InsertTestFixture.WithoutDataContext
		internal static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS Insert(string code, string description)
		{
			try
			{
				// Return
				return MyInsert(code, description, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY InsertTestFixture.WithNullDataContext
		// TESTEDBY InsertTestFixture.WithDataContext
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS Insert(string code, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyInsert(code, description, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY InsertNewTestFixture.WithoutDataContext
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS InsertNew()
		{
			try
			{
				// Return
				return MyInsertNew(null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TESTEDBY InsertNewTestFixture.WithNullDataContext
		// TESTEDBY InsertNewTestFixture.WithDataContext
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS InsertNew(Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyInsertNew(configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BK, Not tested
		//internal staticSqlCompact.User GetItemForceCreating(string code, string description)
		//{
		//    try
		//    {
		//        // Return
		//        return MyGetItemForceCreating(code, description, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BK, Not tested
        internal static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS GetItemForceCreating(string code, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyGetItemForceCreating(code, description, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		//// TODO BK 0 BO, Not tested
		//internal static bool Delete(string code)
		//{
		//    try
		//    {
		//        // Return
		//        return MyDelete(code, null);
		//    }
		//    catch
		//    {
		//        // Throw caught exception
		//        throw;
		//    }
		//}

		// TODO BK 0 BO, Not tested
        internal static bool Delete(string code, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
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
				return MyDelete(code, configurationManager);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
        private static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS MyCodeExists(string code, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User = null;
			bool Internal = false;

			// Check user code
			if (string.IsNullOrEmpty(code))
			{
				// Throw exception
				throw new System.ArgumentNullException("code");
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
				// Select user
                User = configurationManager.CMUSERS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMUSERS>(para => (para.CODE.ToUpper() == code.ToUpper()));
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
			return User;
		}

        private static string MyGetNewCode(Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
			string Code;
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
				// Create user code from new identifier
				Code = System.Guid.NewGuid().ToString();
				// Check existence
				Exists = configurationManager.CMUSERS.Any(para => (para.CODE.ToUpper() == Code.ToUpper()));
			}
			while (Exists);

			// Check state
			if (Internal)
			{
				// Close
               DataContextHelper.CloseDataContext(configurationManager);
			}

			// Return
			return Code;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS MyInsert(string code, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User;
			bool Internal = false;

			// Check user code
			if (string.IsNullOrEmpty(code))
			{
				// Throw exception
				throw new System.ArgumentNullException("code");
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

			// New user
			User =  new Cic.OpenOne.Common.Model.DdCcCm.CMUSERS();
			// Set properties
			User.CODE = code;
			User.DESCRIPTION = description;
			// Insert
			configurationManager.AddToCMUSERS(User);

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

			// Reset user
			User = null;
			// Get user
            User = configurationManager.CMUSERS.FirstOrDefault<Cic.OpenOne.Common.Model.DdCcCm.CMUSERS>(para => (para.CODE.ToUpper() == code.ToUpper()));
			// Check user
			if (User == null)
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
			return User;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS MyInsertNew(Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User;
			string Code;
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

			// Get new code
			Code = MyGetNewCode(configurationManager);

			// Check code
			if (Code == null)
			{
				// Check state
				if (Internal)
				{
					// Close
                   DataContextHelper.CloseDataContext(configurationManager);
				}
				// Throw exception
				// TODO BK 0 BK, Add exception class, Localize text
				throw new System.ApplicationException("An unknown error occured while searching for a new user code.");
			}

			try
			{
				// Insert
				User = MyInsert(Code, null, configurationManager);
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
			return User;
		}

        private static Cic.OpenOne.Common.Model.DdCcCm.CMUSERS MyGetItemForceCreating(string code, string description, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User;
			bool Internal = false;

			// Check user code
			if (string.IsNullOrEmpty(code))
			{
				// Throw exception
				throw new System.ArgumentNullException("code");
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

			// Get user
			User = MyCodeExists(code, configurationManager);

			// Check user
			if (User == null)
			{
				try
				{
					// Insert
					User = MyInsert(code, description, configurationManager);
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
			return User;
		}

        private static bool MyDelete(string code, Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended configurationManager)
		{
			bool Internal = false;

			// Check user code
			if (string.IsNullOrEmpty(code))
			{
				// Throw exception
				throw new System.ArgumentNullException("code");
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
            foreach (Cic.OpenOne.Common.Model.DdCcCm.CMUSERS LoopUser in configurationManager.CMUSERS.Where(para => para.CODE.ToUpper() == code.ToUpper()))
			{
				// Delete on submit
				configurationManager.DeleteObject(LoopUser);
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
