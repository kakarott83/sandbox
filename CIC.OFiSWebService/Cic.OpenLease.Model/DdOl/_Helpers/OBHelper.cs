// OWNER WB, 19-03-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class OBHelper
    {
        #region Methods
        
        public static Cic.OpenLease.Model.DdOl.OBBRIEF GetObbriefFromOb(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long SYSOB)
        {
            var query = from obbrief in context.OBBRIEF
                        where obbrief.OB.SYSOB == SYSOB
                        select obbrief;

            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.OBBRIEF>();
        }
        
        #endregion
    }
}