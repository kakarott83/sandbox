using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
	#region Using directives
	using System.Linq;
	#endregion

	//[System.CLSCompliant(true)]
	internal static class DataContextHelper
	{
		#region Private constants
		private const string CnstConfigurationManagerConnectionStringPart1 = "Data Source=";
		private const string CnstConfigurationManagerConnectionStringPart2 = @"\_Data\ConfigurationManager.sdf";
		#endregion

		#region Methods
		// TESTEDBY GetDataContextTestFixture.Test
        internal static Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended GetDataContext()
		{
       
            //System.Data.SqlServerCe.SqlCeConnection SqlCeConnection;
            //SqlCompact.ConfigurationManager ConfigurationManager;
            Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended ConfigurationManager;
            try
            {

                ConfigurationManager = new Cic.OpenOne.Common.Model.DdCcCm.DdCcCmExtended();
                ConfigurationManager.Connection.Open();
            }
            catch
            {
                throw;
            }

            //Return
            return ConfigurationManager;
		}

		// TODO BK 0 BK, Not tested
        internal static void CloseDataContext(Cic.OpenOne.Common.Model.DdCcCm.CcCmEntities ConfigurationManager)
		{
			// Check object
            if (ConfigurationManager != null)
			{
				try
				{
					// Close connection
                    ConfigurationManager.Connection.Close();
                    ConfigurationManager.Dispose();
				}
				catch
				{
					// ignore exception
				}
			}
		}
		#endregion

		#region My methods
        //private static string MyGetConnectionString()
        //{
        //    string ConnectionString;

        //    try
        //    {
        //        // Create connection string
        //        ConnectionString = (CnstConfigurationManagerConnectionStringPart1 + Cic.Basic.Reflection.Assembly.AssemblyHelper.GetExePath(System.Reflection.Assembly.GetExecutingAssembly(), false) + CnstConfigurationManagerConnectionStringPart2);
        //    }
        //    catch
        //    {
        //        // Throw caught exception
        //        throw;
        //    }

        //    // return
        //    return ConnectionString;
        //}
		#endregion
	}
}
