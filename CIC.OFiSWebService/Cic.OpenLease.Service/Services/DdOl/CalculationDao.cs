using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cic.OpenLease.Service.Services.DdOl
{
    public class VartDTO {
        public String CODE { get; set; }
        public long SYSVART { get; set; }
        public long sysprproduct { get; set; }
    }
    public class ObartDTO
    {
        public long SYSOBART { get; set; }
        public int TYP { get; set; }
        public String NAME { get; set; }
    }
    public class CalculationDao
    {
        
        //private static CacheDictionary<long, KALKTYP> kalktypcache = CacheFactory<long, KALKTYP>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, VartDTO> vartcache = CacheFactory<long, VartDTO>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, ObartDTO> obartcache = CacheFactory<long, ObartDTO>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);


        public CalculationDao(DdOlExtended context)
        {
            if (vartcache.Count == 0)
            {
                try
                {
                    List<ObartDTO> obarts = context.ExecuteStoreQuery<ObartDTO>("select sysobart, typ, name from obart").ToList();
                    
                    foreach (ObartDTO o in obarts)
                    {
                        obartcache[o.SYSOBART] = o;
                    }

                    List<VartDTO> varts = context.ExecuteStoreQuery<VartDTO>("select vart.sysvart, prproduct.sysprproduct, vart.code from vart, prproduct where prproduct.sysvart=vart.sysvart").ToList();

                    foreach (VartDTO o in varts)
                    {
                        vartcache[o.sysprproduct] = o;
                    }



                }
                catch (Exception exception)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not fetch Vertragsarten.", exception);
                }
            }
        }
      
        public VartDTO getVART(long sysprproduct)
        {
            return vartcache[sysprproduct];
        }
        public ObartDTO getObArt(long sysobart)
        {
            return obartcache[sysobart];
        }
    }
}