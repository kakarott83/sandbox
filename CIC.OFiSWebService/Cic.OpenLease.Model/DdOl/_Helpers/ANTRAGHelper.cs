// OWNER WB, 10-03-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANTRAGHelper
    {
        #region Methods

        public static bool Contains(OlExtendedEntities olExtendedEntities, long sysID)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.ANTRAG.Where(par => par.SYSID == sysID).Any<ANTRAG>();
        }

        public static Cic.OpenLease.Model.DdOl.IT GetITFromAntrag(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSIT)
        {
            var query = from it in context.IT
                        where it.SYSIT == SYSIT
                        select it;

            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.IT>();

        }


        #endregion
    }
}