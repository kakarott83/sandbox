//Owner SD 22.02.2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using directives
    using System.Linq;
    using System.Collections.Generic;
    using System;
    using Cic.OpenOne.Common.Util.Collection;
    #endregion

    [System.CLSCompliant(true)]
    public class OBARTHelper
    {
        private static CacheDictionary<String, List<OBART>> obartcache = CacheFactory<String, List<OBART>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        public const int CnstObArtNew = 0;
        public const int CnstObArtUsed = 1;
        public const int CnstObArtVorfuehr = 2;

        #region Methods
        public static OBART[] SearchDescription(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, string description)
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
        public static OBART SearchName(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, string name)
        {
            List<OBART> obarten = rebuild(context);

            OBART OBART;

            var Query = from obArt in obarten
                            where obArt.NAME == name
                            select obArt;
                OBART = Query.FirstOrDefault<OBART>();

            return OBART;
        }

        public static bool isOfType(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long sysobart, long type)
        {
            List<OBART>  obarten = rebuild(context);

            OBART obart = (from o in obarten
                           where o.SYSOBART == sysobart
                           select o).FirstOrDefault();
                           
            if (obart == null) return false;
            if (obart.TYP == type) return true; 

            return false;
        }

        public static long getObartOfType(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long type)
        {
            List<OBART> obarten = rebuild(context);

            OBART obart = (from o in obarten
                           where o.TYP.Value == type
                           select o).FirstOrDefault();

            if (obart == null) return -1;

            return obart.SYSOBART;
        }

        private static List<OBART> rebuild(Cic.OpenLease.Model.DdOl.OlExtendedEntities context)
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

