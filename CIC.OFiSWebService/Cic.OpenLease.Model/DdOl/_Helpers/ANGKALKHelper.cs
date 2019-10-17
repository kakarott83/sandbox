// OWNER MK, 21-09-2009
namespace Cic.OpenLease.Model.DdOl
{
    
    #region Using
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANGKALKHelper
    {
        #region Methods
        public static void Delete(OlExtendedEntities context, long SysAngKalk)
        {
            throw new System.NotImplementedException();
        }

     

        public static Cic.OpenLease.Model.DdOl.ANGKALKFS GetAngkalkfsFromAngkalk(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSKALK)
        {
            var query = from angkalkfs in context.ANGKALKFS
                        where angkalkfs.SYSANGKALKFS == SYSKALK
                        select angkalkfs;

            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.ANGKALKFS>();
            
        }

        public static long GetLastSYSKALK(Cic.OpenLease.Model.DdOl.OlExtendedEntities context)
        {

            var query = from angkalk in context.ANGKALK
                        orderby angkalk.SYSKALK descending
                        select angkalk;

            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.ANGKALK>().SYSKALK;
            
        }
        #endregion

    }
}
