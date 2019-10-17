// OWNER WB, 17-03-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANTOBHelper
    {
        #region Methods

        public static Cic.OpenLease.Model.DdOl.ANTOBINI GetAntobiniFromAntob(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long SYSOB)
        {
            var query = from antobini in context.ANTOBINI
                        where antobini.SYSOBINI == SYSOB
                        select antobini;

            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.ANTOBINI>();
        }

        #endregion
    }
}