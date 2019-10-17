// OWNER MK, 23-11-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class MYCALCHelper
    {
        #region Methods
        public static MYCALC DeliverMyCalc(OlExtendedEntities context, long sysMyCalc)
        {
            return context.MYCALC.Where(par => par.SYSMYCALC == sysMyCalc).FirstOrDefault<MYCALC>();
        }
        #endregion
    }
}
