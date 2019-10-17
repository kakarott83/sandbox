using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.P000001.Common.DataProvider;

namespace Cic.OpenOne.CarConfigurator.BO.MergeDataProvider
{
    public abstract class IDualAdapter
    {
        public IAdapter PrimaryAdapter;
        public IAdapter SecondaryAdapter;
    }
}
