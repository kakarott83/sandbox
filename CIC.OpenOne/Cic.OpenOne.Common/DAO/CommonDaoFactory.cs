using Cic.OpenOne.Common.DAO.Versicherung;
using System;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// CommonDaoFactory-Klasse
    /// </summary>
    public class CommonDaoFactory
    {
        private static volatile CommonDaoFactory _self;
        private static readonly object InstanceLocker = new Object();

        /// <summary>
        /// Instanz der Prisma DAO Factory erzeugen
        /// </summary>
        /// <returns></returns>
        public static CommonDaoFactory getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new CommonDaoFactory();
                }
            }
            return _self;
        }

        /// <summary>
        /// Prisma DAO Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IObTypDao getObTypDao()
        {
            return new CachedObTypDao();
        }

       

        /// <summary>
        /// VG DAO Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IVGDao getVGDao()
        {
            return new ValueGroupDao();//new CachedVGDao();
        }

        /// <summary>
        /// DictionaryListsDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IDictionaryListsDao getDictionaryListsDao()
        {
            return new DictionaryListsDao();
        }

        /// <summary>
        /// ZinsDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IZinsDao getZinsDao()
        {
            return new CachedZinsDao();
        }

        /// <summary>
        /// QuoteDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IQuoteDao getQuoteDao()
        {
            return new CachedQuoteDao();
        }

        /// <summary>
        /// TranslateDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public ITranslateDao getTranslateDao()
        {
            return new CachedTranslateDao();
        }

        /// <summary>
        /// MwStDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IMwStDao getMwStDao()
        {
            return new CachedMwStDao();
        }

        /// <summary>
        /// InsuranceDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IInsuranceDao getInsuranceDao()
        {
            return new CachedInsuranceDao();
        }

       

        /// <summary>
        /// KorrekturDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IKorrekturDao getKorrekturDao()
        {
            return new CachedKorrekturDao();
        }

        /// <summary>
        /// RightsMapDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IRightsMapDao getRightsMapDao()
        {
            return new CachedRightsMapDao();
        }
    }
}