using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Collection;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// EAIPAR DAO
    /// </summary>
    public class EaiparDao : IEaiparDao
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

        #region Methods
        /// <summary>
        /// LoadEAIPAR
        /// </summary>
        /// <returns></returns>
        private List<EAIPAR> LoadEAIPARForAGG()
        {
            List<EAIPAR> cfgList = new List<EAIPAR>();
            //using (DdOiQueueExtended context = new DdOiQueueExtended())
            //{
            //    CFG selectedCfg = (from c in context.CFG
            //                       where c.CODE == cfg.ToUpper()
            //                       select c).FirstOrDefault();

            //    if (selectedCfg != null)
            //    {
            //        selectedCfg.CFGSECList.Load();
            //        CFGSEC selectedCfgSec = (from s in selectedCfg.CFGSECList
            //                                 where (s.CODE == cfgsection.ToUpper() && s.ACTIVEFLAG == 1)
            //                                 select s).FirstOrDefault();
            //        if (selectedCfgSec != null)
            //        {
            //        }
            //    }
            //}
            //            DdOwExtended eaiParContext = new DdOwExtended();

            //            EAIPAR selectedEaiPar = (from v in eaiParContext.EAIPAR
            //                                     where (v.CODE == entrycode.ToUpper())
            //                                     select v).FirstOrDefault();
            //            if (selectedEaiPar != null && selectedEaiPar.PARAMFILE != null)
            //            {
            //                retval = selectedEaiPar.PARAMFILE.Trim();
            //            }
            return cfgList;
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
                    if (selectedEaiPar != null && selectedEaiPar.PARAMFILE != null && selectedEaiPar.PARAMFILE.Trim().Length>0)
                    {
                        retval = selectedEaiPar.PARAMFILE.Trim();
                    }
                }
                parCache[entryCode] = retval;
            }
            return parCache[entryCode];
        }
        #endregion Methods

    }
}