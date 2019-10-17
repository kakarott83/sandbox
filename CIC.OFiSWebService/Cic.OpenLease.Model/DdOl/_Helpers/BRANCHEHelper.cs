// OWNER JJ, 30-11-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class BRANCHEHelper
    {
        #region Methods
        public static bool Contains(OlExtendedEntities olExtendedEntities, long sysBRANCHE)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.BRANCHE.Where(par => par.SYSBRANCHE == sysBRANCHE).Any<BRANCHE>();
        }
        #endregion
    }
}