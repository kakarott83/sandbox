

using Cic.OpenOne.Util.Reflection;
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
	//[System.CLSCompliant(true)]
	internal class DataProviderAdapterLoaderHelper : AdapterLoaderHelper<Cic.P000001.Common.DataProvider.IAdapter>
	{
		#region Constructors
		// TODO BK 0 BK, Not tested
        public DataProviderAdapterLoaderHelper()
			: base(System.Reflection.Assembly.GetExecutingAssembly(), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ExeConfigurationHelper.DataProviderAdapterAssemblyFileNameKey)
		{
		}
		#endregion
	}
}