using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.P000001.Common.DataProvider;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;

namespace Cic.OpenOne.CarConfigurator.BO.MergeDataProvider
{
    /// <summary>
    /// Provides embedded access to data providers of car configurator, no configuration possible
    /// </summary>
    public class EmbeddedDualAdapter : IDualAdapter
    {
       
        public EmbeddedDualAdapter()
        {
          
            this.SecondaryAdapter = new EmbeddedDataProviderAdapterLoader<Cic.OpenOne.CarConfigurator.BO.ObViewDataProvider.ObViewDataProvider>().LoadAdapter();
            this.PrimaryAdapter = new EmbeddedDataProviderAdapterLoader<Cic.OpenOne.CarConfigurator.BO.EurotaxDataProvider.EurotaxDataProvider>().LoadAdapter();

          
        }
    }
}