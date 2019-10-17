// OWNER JJ, 30-11-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class ITHelper
    {
        #region Methods
        public static bool Contains(OlExtendedEntities olExtendedEntities, long sysIT)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.IT.Where(par => par.SYSIT == sysIT).Any<IT>();
        }

        public static IT GetIT(OlExtendedEntities context, long sysIT)
        {
            IT IT;
            var Query = from it in context.IT
                        where it.SYSIT == sysIT
                        select it;

            IT = Query.FirstOrDefault();
            
            if (IT == null)
            {
                throw new System.Exception("IT is null");
            }
            return Query.FirstOrDefault();
        }
        #endregion
    }
}