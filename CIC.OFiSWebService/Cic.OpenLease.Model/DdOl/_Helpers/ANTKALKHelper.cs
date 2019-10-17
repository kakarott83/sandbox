// OWNER WB, 18-03-2010
namespace Cic.OpenLease.Model.DdOl
{

    #region Using
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANTKALKHelper
    {
        #region Methods
        
        public static Cic.OpenLease.Model.DdOl.ANTKALKFS GetAntkalkfsFromAntkalk(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSKALK)
        {
            // Create ANTKALKFS query
            var ANTKALKFSQuery = from antkalkfs in context.ANTKALKFS
                                 where antkalkfs.SYSANTKALKFS == SYSKALK
                                 select antkalkfs;
            return ANTKALKFSQuery.FirstOrDefault<Cic.OpenLease.Model.DdOl.ANTKALKFS>();
        }

       #endregion

    }
}
