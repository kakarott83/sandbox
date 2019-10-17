using Cic.OpenOne.Util.Reflection;
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal partial class ConfigurationManagerAdapterLoaderHelper : AdapterLoaderHelper<Cic.P000001.Common.ConfigurationManager.IAdapter>
	{
		#region Constructors
		// TODO BK 0 BK, Not tested
        internal ConfigurationManagerAdapterLoaderHelper()
			: base(System.Reflection.Assembly.GetExecutingAssembly(), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ExeConfigurationHelper.ConfigurationManagerAdapterAssemblyFileNameKey)
		{
		}
		#endregion
	}
}
