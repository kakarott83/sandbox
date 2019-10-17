using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cic.OpenLease.Service.Services.DdOl
{
    public class LsAddDao
    {
        private static CacheDictionary<long, LSADD> lsadd = CacheFactory<long, LSADD>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, LSADD> vartLsCache = CacheFactory<long, LSADD>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, VART> vartCache = CacheFactory<long, VART>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        private DdOlExtended context;

        public LsAddDao(DdOlExtended context)
        {
            this.context = context;
            if(lsadd.Count==0)
                init();
            
        }

        private void init()
        {
            try
            {
                var query = from lsadd in context.LSADD
                            select lsadd;
                List<LSADD> lsadds = query.ToList<LSADD>();
                foreach (LSADD ls in lsadds)
                {
                    if (ls.MWST == null)
                        context.Entry(ls).Reference(f => f.MWST).Load();
                    if (ls.INTSTRCT == null)
                        context.Entry(ls).Reference(f => f.INTSTRCT).Load();
 
                    
                    lsadd[ls.SYSLSADD] = ls;
                }

                var query2 = from vart in context.VART
                            select vart;

                List<VART> varts = query2.ToList<VART>();
                foreach (VART v in varts)
                {
                    vartCache[v.SYSVART] = v;
                    vartLsCache[v.SYSVART] = lsadds[0];
                  
                }

            }
            catch (Exception exception)
            {
                // Throw an exception
                throw new ApplicationException("Could not fetch LsAdd ", exception);
            }
        }

        public decimal GetTaxRate(long? sysvart)
        {
            long key = sysvart.HasValue ? sysvart.Value : -1;
           
             if (!vartLsCache.ContainsKey(key))
                init();

             LSADD ls = vartLsCache[key];
             return ls.MWST.PROZENT.GetValueOrDefault();
        }

        public long getMandantByVART(long? sysvart)
        {
            long key = sysvart.HasValue ? sysvart.Value : -1;

            if (!vartLsCache.ContainsKey(key))
                init();

            return vartLsCache[key].SYSLSADD;
        }
        public LSADD getLSADDByVART(long? sysvart)
        {
            long key = sysvart.HasValue ? sysvart.Value : -1;

            if (!vartLsCache.ContainsKey(key))
                init();

            return vartLsCache[key];
        }

        public LSADD DeliverLsAdd(long sysLsAdd)
        {
            if (!lsadd.ContainsKey(sysLsAdd))
                init();
            return lsadd[sysLsAdd];
        }
    }
}