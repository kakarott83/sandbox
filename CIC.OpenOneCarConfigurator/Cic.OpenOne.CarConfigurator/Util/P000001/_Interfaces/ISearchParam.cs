using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.P000001.Common
{
    public interface ISearchParam
    {
        #region Properties
        string Pattern
        {
            get;
            set;
        }
        #endregion
    }
}
