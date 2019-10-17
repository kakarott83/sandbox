// OWNER: BK, 13-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    // [System.CLSCompliant(true)]
    internal class ExeConfigurationHelper
	{
		#region Constants
		private const string CnstPassThroughExceptionsKey = "PassThroughExceptions";
		private const string CnstConfigurationManagerAdapterAssemblyFileName = "ConfigurationManagerAdapterAssemblyFileName";
        private const string CnstDataProviderAdapterAssemblyFileName = "DataProviderAdapterAssemblyFileName";
		#endregion

		#region Private variables
		
		//private static string _PassThroughExceptionsValue;
		#endregion

		#region Constructors
		// NOTE BK, Private Constructur, to avoid the creation of a standard constructor through the compiler
		// TODO BK 0 BK, Not tested
		private ExeConfigurationHelper()
		{
		}
		#endregion

		#region Method
		public static bool GetPassThroughExceptionsValue()
        {
			//return false ;)
            return true;
        }
		#endregion

		#region Properties
		public static string ConfigurationManagerAdapterAssemblyFileNameKey
		{
			get
			{
				// Return
				return CnstConfigurationManagerAdapterAssemblyFileName;
			}
		}

        public static string DataProviderAdapterAssemblyFileNameKey
        {
            get
            {
                // Return
                return CnstDataProviderAdapterAssemblyFileName;
            }
        }
		#endregion
	}
}
