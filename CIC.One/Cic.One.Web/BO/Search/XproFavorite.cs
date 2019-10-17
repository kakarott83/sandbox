using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.One.Web.BO.Search
{
    class FavInfoDto
    {
        public long id { get; set; }
        public String code { get; set; }
    }
    public class XproFavorite
    {
        private static long CACHE_TIMEOUT = 2592000000;//month
        private static CacheDictionary<String, List<FavInfoDto>> favCache = CacheFactory<String, List<FavInfoDto>>.getInstance().createCache(CACHE_TIMEOUT);


        private static CacheDictionary<String, List<String>> favCodeCache = CacheFactory<String, List<String>>.getInstance().createCache(CACHE_TIMEOUT);

        public static List<T> sortFavorites<T>(String xprocode, List<T> toSort, Func<T, String> codeSelector)
        {
            if(favCodeCache.Count==0)
            {
                using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                {
                    List<String> codes = ctx.ExecuteStoreQuery<String>("SELECT UPPER(cfgsec.code) FROM cfgsec, cfg WHERE UPPER(cfg.code) = 'FAVORITEN' AND cfgsec.syscfg = cfg.syscfg").ToList();
                    favCodeCache["CODES"] = codes;
                }
            }
            if (!favCodeCache["CODES"].Contains(xprocode.ToUpper()))
                return toSort;

            if (!favCache.ContainsKey(xprocode))
            {
                using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                {
                    String favcode = "FAVORITEN";
                    List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                    par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "favcode", Value = favcode });
                    par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "xprocode", Value = xprocode });

                    List<FavInfoDto> values = ctx.ExecuteStoreQuery<FavInfoDto>("SELECT cfgvar.wert id, cfgvar.code FROM cfgvar, cfgsec, cfg WHERE UPPER(cfg.code) = UPPER(:favcode) AND cfgsec.syscfg = cfg.syscfg AND UPPER(cfgsec.code) = UPPER(:xprocode) AND cfgvar.syscfgsec = cfgsec.syscfgsec", par.ToArray()).ToList();
                    favCache.Add(xprocode, values);
                }
            }
            List<FavInfoDto> favSort = favCache[xprocode];
            if (favSort == null || favSort.Count == 0) return toSort;

         
            return (from v in toSort
                    orderby getRang(favSort,codeSelector(v))
                   select v).ToList();
            
         
        }
        private static long getRang(List<FavInfoDto> favSort, String code)
        {
            long rval = (from v in favSort
                    where v.code.Equals(code)
                    select v.id).FirstOrDefault();
            if (rval == 0) return long.MaxValue;
            return rval;
        }
    }
 
}
