using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.MergeDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Cic.P000001.Common.DataProvider;
    #endregion

    internal class DualAdapter : IDualAdapter
    {
        #region Properties
        public new IAdapter PrimaryAdapter
        {
            get;
            private set;
        }

        public new IAdapter SecondaryAdapter
        {
            get;
            private set;
        }
        #endregion

        #region Constructors
        public DualAdapter()
        {
            DataProviderAdapterLoader AdapterLoader;

            // Load the primary adapter
            AdapterLoader = new DataProviderAdapterLoader(AdapterType.Primary);
            this.PrimaryAdapter = AdapterLoader.LoadAdapter();

            // Load the secondary adapter
            AdapterLoader = new DataProviderAdapterLoader(AdapterType.Secondary);
            this.SecondaryAdapter = AdapterLoader.LoadAdapter();
        }
        #endregion
    }
}
