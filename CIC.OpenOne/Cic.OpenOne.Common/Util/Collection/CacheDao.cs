using System;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.OpenOne.Common.Util.Collection
{
    /// <summary>
    /// Manages the cache lifetime
    /// 
    /// Kategorie	Bereich
    /// -------------------
    /// Data		Obtyp-Hierarchie, AngAntOb (Fahrzeugdaten)
    /// Prisma		Mwst, Versicherungen, Quote, Wertegruppen, Vertragsarten, Produkte, Produktlinks, Parameter, 
    ///             Parameterlinks, News, Provisionen, Ablösetypen, Subventionen
    /// Role		Peroles, Prhgroup, Haendler, Person
    /// Zins		Zinsdaten
    /// Translation	Übersetzungsdaten
    /// </summary>
    public class CacheDao
    {
        private static volatile CacheDao _self;
        private static readonly object InstanceLocker = new Object();

        /// <summary>
        /// getInstance
        /// </summary>
        /// <returns></returns>
        public static CacheDao getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new CacheDao();
                }
            }
            return _self;
        }

        /// <summary>
        /// getCacheDuration
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public long getCacheDuration(CacheCategory cat)
        {
            long rval = 1000 * 60 * 60;//default 1 hour
            try
            {
                rval = Cic.OpenOne.Common.Properties.Config.Default.CacheLifetime;//overridable in config
            }
            catch (Exception)
            {
            }
            try
            {
                String entry = cat.ToString().ToUpper();
                String lifetime = AppConfig.Instance.GetEntry("CACHE", entry, rval.ToString(), "SETUP.NET");//if not found use config or default
                if (lifetime != null && lifetime.Length > 0)
                    rval = long.Parse(lifetime);
            }
            catch (Exception)
            {
            }
            return rval;
        }
    }
}