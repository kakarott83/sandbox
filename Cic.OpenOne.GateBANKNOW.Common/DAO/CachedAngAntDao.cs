using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
   
    /// <summary>
    /// Offer/Application Data Access Object
    /// </summary>
    public class CachedAngAntDao : AngAntDao
    {
     
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<long, AngAntObDto> obDataCache = CacheFactory<long, AngAntObDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, String> vsArtCodeCache = CacheFactory<long, String>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, VartDto> vArtCache = CacheFactory<long, VartDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);


        public CachedAngAntDao(IEaihotDao eaiHotDao):base(eaiHotDao)
        {
        }

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="sysobtyp">ObjectType</param>
        /// <returns>Daten</returns>
        override public AngAntObDto getObjektdaten(long sysobtyp)
        {
            if (!obDataCache.ContainsKey(sysobtyp))
            {
                obDataCache[sysobtyp] = base.getObjektdaten(sysobtyp);
            }
            return obDataCache[sysobtyp];
        }


        /// <summary>
        /// getVsArtCode
        /// </summary>
        /// <param name="sysvstyp"></param>
        /// <returns></returns>
        override public string getVsArtCode(long sysvstyp)
        {
            if (!vsArtCodeCache.ContainsKey(sysvstyp))
            {
                vsArtCodeCache[sysvstyp] = base.getVsArtCode(sysvstyp);
            }
            return vsArtCodeCache[sysvstyp];
        }

        /// <summary>
        /// getVart
        /// </summary>
        /// <param name="sysvart"></param>
        /// <returns></returns>
        override public VartDto getVart(long sysvart)
        {
            if (!vArtCache.ContainsKey(sysvart))
            {
                vArtCache[sysvart] = base.getVart(sysvart);
            }
            return vArtCache[sysvart];
        }

       


      
    }

   
}
