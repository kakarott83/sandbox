using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class LandDao : ILandDao
    {
        private static readonly CacheDictionary<String, long> LandCache = CacheFactory<String, long>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        public long GetSysLandFromIsoCountry(string isoCountry)
        {
            if (string.IsNullOrEmpty(isoCountry))
                return 0;

            if (LandCache.ContainsKey(isoCountry))
                return LandCache[isoCountry];


            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = isoCountry });



            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new Cic.OpenOne.Common.Model.Prisma.PrismaExtended())
            {
                
                long sysland = ctx.ExecuteStoreQuery<long>("select sysland from land where iso=:p1", parameters.ToArray()).FirstOrDefault();
                //var result = ctx.ExecuteStoreQuery<long>("select sysland from land where iso = :p1", new OracleParameter() { ParameterName = "p1", Value = isoCountry }).FirstOrDefault();

                LandCache[isoCountry] = sysland;
                return sysland;
            }
        }
    }
}
