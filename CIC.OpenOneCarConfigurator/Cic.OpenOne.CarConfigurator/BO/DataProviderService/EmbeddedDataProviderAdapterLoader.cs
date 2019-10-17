using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// Embedded CarConfigurator DataProvider Access
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EmbeddedDataProviderAdapterLoader<T> where T : Cic.P000001.Common.DataProvider.IAdapter, new()
    {

        public Cic.P000001.Common.DataProvider.IAdapter LoadAdapter()
        {
            return new T();
        }
        public Cic.P000001.Common.DataProvider.IAdapter LoadAdapter(string appSettingsAdapterAssemblyFileNameSuffix)
        {
            return new T();
        }
    }
}