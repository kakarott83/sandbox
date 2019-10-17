using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class RenameConfigurationHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static bool Execute(string userCode, System.Guid configurationIdentifier, string designation)
		{
            
            Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended ConfigurationManager;
            Cic.OpenOne.Common.Model.DdCcCm.CMUSERS User;
            Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS ConfigurationPackage;
			bool CodeExists;
			bool IdentifierExists;
			bool Result = false;

			// Check user code
			if (string.IsNullOrEmpty(userCode))
			{
				// Throw exception
				throw new System.ArgumentNullException("userCode");
			}

			// Check designation
			// NOTE BK 0, This checking must be the same like the one in the configurationPackage class
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(designation))
			{
				// Throw exception
				throw new System.ArgumentNullException("designation");
			}

			// Trim designation
			designation = designation.Trim();

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
                    // Check existence
                    CodeExists =UsersHelper.CodeExists(userCode);
                    // Check existence
                    IdentifierExists =ConfigurationPackagesHelper.IdentifierExists(configurationIdentifier, ConfigurationManager);
                }
                catch
                {
                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw caught exception
                    throw;
                }

                // Check state
                if (!CodeExists)
                {
                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw exception
                    // OK
                    throw new System.ArgumentException("userCode");
                }

                // Check state
                if (!IdentifierExists)
                {
                    // Close
                   DataContextHelper.CloseDataContext(ConfigurationManager);
                    // Throw exception
                    // OK
                    throw new System.ArgumentException("configurationIdentifier");
                }

                // Get user
                User =UsersHelper.GetItem(userCode, ConfigurationManager);
                // Get configurationPackage
                ConfigurationPackage =ConfigurationPackagesHelper.GetItem(configurationIdentifier, ConfigurationManager);

                // Check configurationPackage
                if ((ConfigurationPackage.CMUSERS.SYSCMUSERS == User.SYSCMUSERS) || ((ConfigurationPackage.ISPUBLIC == 1) && (ConfigurationPackage.ISLOCKED == 0)))
                {
                    try
                    {
                        // Update
                        Result =ConfigurationPackagesHelper.UpdateDesignation(ConfigurationPackage.SYSCMCPKGS, designation, ConfigurationManager);
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
