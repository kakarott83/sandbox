// OWNER: BK, 09-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
	

    /// <summary>
    /// Embedded ConfigurationManager DataProvider Access
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EmbeddedConfigurationManagerAdapterLoader<T> where T : Cic.P000001.Common.ConfigurationManager.IAdapter, new()
    {

        public Cic.P000001.Common.ConfigurationManager.IAdapter LoadAdapter()
        {
            return new T();
        }
        public Cic.P000001.Common.ConfigurationManager.IAdapter LoadAdapter(string appSettingsAdapterAssemblyFileNameSuffix)
        {
            return new T();
        }
    }
}
