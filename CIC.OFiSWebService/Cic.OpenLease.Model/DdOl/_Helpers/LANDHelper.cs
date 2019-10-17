// OWNER JJ, 30-11-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

	[System.CLSCompliant(true)]
	public static class LANDHelper
    {
        #region Methods
        public static bool Contains(OlExtendedEntities olExtendedEntities, long sysLAND)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.LAND.Where(par => par.SYSLAND == sysLAND).Any<LAND>();
        }
        #endregion
    }
}