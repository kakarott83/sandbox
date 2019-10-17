using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.MergeDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Cic.OpenOne.Util.Reflection;
    
    
    #endregion

    internal class DataProviderAdapterLoader : AdapterLoaderHelper<Cic.P000001.Common.DataProvider.IAdapter>
    {
        #region Private constants
        private const string CnstPrimaryAdapterConfigKey = "PrimaryDataProviderAdapterAssemblyFileName";
        private const string CnstSecondaryAdapterConfigKey = "SecondaryDataProviderAdapterAssemblyFileName";
        #endregion

        #region Constructors
        public DataProviderAdapterLoader(AdapterType adapterType)
            : base(System.Reflection.Assembly.GetExecutingAssembly(), adapterType == AdapterType.Primary ? CnstPrimaryAdapterConfigKey : CnstSecondaryAdapterConfigKey)
        {
        }
        #endregion
    }
}