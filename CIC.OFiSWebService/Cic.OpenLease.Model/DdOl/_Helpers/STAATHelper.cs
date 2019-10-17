// OWNER JJ, 30-11-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class STAATHelper
    {
        #region Methods
        public static bool Contains(OlExtendedEntities olExtendedEntities, long sysSTAAT)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.STAAT.Where(par => par.SYSSTAAT == sysSTAAT).Any<STAAT>();
        }
        #endregion
    }
}