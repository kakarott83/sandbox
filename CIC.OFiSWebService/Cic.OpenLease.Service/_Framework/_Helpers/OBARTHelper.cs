namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using CIC.Database.OW.EF6.Model;


    using System;
    using Cic.OpenOne.Common.Model.DdOw;
    using CIC.Database.OL.EF6.Model;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Model.DdOl;
    using System.Collections.Generic;
    #endregion
    [System.CLSCompliant(true)]
    public class OBARTHelper
    {
        private static CacheDictionary<String, List<OBART>> obartcache = CacheFactory<String, List<OBART>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        public const int CnstObArtNew = 0;
        public const int CnstObArtUsed = 1;
        public const int CnstObArtVorfuehr = 2;

        #region Methods
        public static OBART[] SearchDescription(DdOlExtended context, string description)
        {
            List<OBART> obarten = rebuild(context);

            OBART[] OBART;

            // if description is null search all
            if (description == null || description == string.Empty)
            {
                var Query = from obArt in obarten
                            orderby obArt.RANK
                            select obArt;
                OBART = Query.ToArray<OBART>();
            }
            else
            {
                var Query = from obArt in obarten
                            where obArt.DESCRIPTION == description
                            orderby obArt.RANK
                            select obArt;
                OBART = Query.ToArray<OBART>();
            }

            return OBART;
        }
        public static OBART SearchName(DdOlExtended context, string name)
        {
            List<OBART> obarten = rebuild(context);

            OBART OBART;

            var Query = from obArt in obarten
                            where obArt.NAME == name
                            select obArt;
                OBART = Query.FirstOrDefault<OBART>();

            return OBART;
        }

        public static bool isOfType(DdOlExtended context, long sysobart, long type)
        {
            List<OBART>  obarten = rebuild(context);

            OBART obart = (from o in obarten
                           where o.SYSOBART == sysobart
                           select o).FirstOrDefault();
                           
            if (obart == null) return false;
            if (obart.TYP == type) return true; 

            return false;
        }

        public static long getObartOfType(DdOlExtended context, long type)
        {
            List<OBART> obarten = rebuild(context);

            OBART obart = (from o in obarten
                           where o.TYP.Value == type
                           select o).FirstOrDefault();

            if (obart == null) return -1;

            return obart.SYSOBART;
        }

        private static List<OBART> rebuild(DdOlExtended context)
        {
            if (!obartcache.ContainsKey("OBARTEN"))
            {
                var Query = from obArt in context.OBART

                            select obArt;
                obartcache["OBARTEN"] = Query.ToList<OBART>();
            }
            return obartcache["OBARTEN"];
        }
        
        #endregion
    }
}

