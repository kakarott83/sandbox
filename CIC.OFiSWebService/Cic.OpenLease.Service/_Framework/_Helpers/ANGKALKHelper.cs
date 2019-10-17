// OWNER MK, 21-09-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using CIC.Database.OW.EF6.Model;


    using System;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANGKALKHelper
    {
        #region Methods
        public static void Delete(DdOlExtended context, long SysAngKalk)
        {
            throw new System.NotImplementedException();
        }

     

        public static  ANGKALKFS GetAngkalkfsFromAngkalk(DdOlExtended context, long? SYSKALK)
        {
            var query = from angkalkfs in context.ANGKALKFS
                        where angkalkfs.SYSANGKALKFS == SYSKALK
                        select angkalkfs;

            return query.FirstOrDefault< ANGKALKFS>();
            
        }

        public static long GetLastSYSKALK(DdOlExtended context)
        {

            var query = from angkalk in context.ANGKALK
                        orderby angkalk.SYSKALK descending
                        select angkalk;

            return query.FirstOrDefault< ANGKALK>().SYSKALK;
            
        }
        #endregion

    }
}
