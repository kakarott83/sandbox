using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.Basic.OpenLease
{
    [System.CLSCompliant(true)]
    public static class BooleanHelper
    {
        #region Methods
        public static bool Convert(int? clarionBoolean)
        {
            return clarionBoolean.HasValue && clarionBoolean.Value != 0;
        }
        #endregion
    }
}
