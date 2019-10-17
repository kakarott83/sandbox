using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.CarConfigurator.DAO
{
    /// <summary>
    /// EAIPAR DAO
    /// </summary>
    public class EaiparDao  

    {
        #region Private vars
        public static CacheDictionary<String, String> parCache = CacheFactory<String, String>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Data);
        #endregion

        /// <summary>
        /// EaiparDao Konstruktor
        /// </summary>
        public EaiparDao()
        {
        }

       

        /// <summary>
        /// ParamFile des EAIPAR auslesen
        /// </summary>
        /// <param name="entryCode"></param>
        /// <param name="defaultValue"></param>
        /// <returns>ParamFile als String</returns>
        public String getEaiParFileByCode(string entryCode, string defaultValue)
        {
            if (!parCache.ContainsKey(entryCode))
            {
                String retval = defaultValue;

                using (DdOwExtended eaiParContext = new DdOwExtended())
                {
                    EAIPAR selectedEaiPar = (from par in eaiParContext.EAIPAR
                                             where (par.CODE.ToUpper() == entryCode.ToUpper())
                                             select par).FirstOrDefault();
                    if (selectedEaiPar != null && selectedEaiPar.PARAMFILE != null && selectedEaiPar.PARAMFILE.Trim().Length > 0)
                    {
                        retval = selectedEaiPar.PARAMFILE.Trim();
                    }
                }
                parCache[entryCode] = retval;
            }
            return parCache[entryCode];
        }
       

    }
}
