using System;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.DAO
{

    /// <summary>
    /// Cached VG Data Access Object
    /// </summary>
    public class CachedVGDao : VGDao
    {
        private static CacheDictionary<object, object> VGCache = CacheFactory<object, object>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, double> saldoCache = CacheFactory<String, double>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, double> praemieCache = CacheFactory<String, double>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<KeyBaseVG, ValueVGCheckBoundaries> VGCheckBoundariesCache = CacheFactory<KeyBaseVG, ValueVGCheckBoundaries>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        /// <summary>
        /// KeyBase VG-Unterklasse
        /// </summary>
        public class KeyBaseVG : IEquatable<KeyBaseVG>
        {
            /// <summary>
            /// VG ID
            /// </summary>
            public long sysVg { get; set; }
            /// <summary>
            /// perDate
            /// </summary>
            public DateTime perDate { get; set; }
            /// <summary>
            /// Gleichheits Operation
            /// </summary>
            /// <param name="obj">Vergleichsobjekt</param>
            /// <returns>Gleichheits-Aussage</returns>
            public bool Equals(KeyBaseVG obj)
            {
                return (sysVg == obj.sysVg) && (perDate == obj.perDate);
            }
            /// <summary>
            /// Gleichheits Operation
            /// </summary>
            /// <param name="obj">Vergleichsobjekt</param>
            /// <returns>Gleichheits-Aussage</returns>
            public override bool Equals(Object obj)
            {
                if (obj == null) return base.Equals(obj);
                return Equals(obj as KeyBaseVG);
            }
            /// <summary>
            /// Hashcode Ermitteln
            /// </summary>
            /// <returns>Hash</returns>
            public override int GetHashCode()
            {
                return this.sysVg.GetHashCode() ^ this.perDate.GetHashCode();
            }
        }

        /// <summary>
        /// VG Key Wert-UnterKlasse
        /// </summary>
        public class KeyVGValue : KeyBaseVG
        {
            /// <summary>
            /// X-Wert
            /// </summary>
            public string xval { get; set; }
            /// <summary>
            /// Y-Wert
            /// </summary>
            public string yval { get; set; }
            /// <summary>
            /// Interpolations Modus
            /// </summary>
            public VGInterpolationMode interpolationMode { get; set; }
            /// <summary>
            /// Gleichheits Operation
            /// </summary>
            /// <param name="obj">Vergleichsobjekt</param>
            /// <returns>Gleichheits-Aussage</returns>
            public bool Equals(KeyVGValue obj)
            {
                return (base.Equals(obj) && (xval == obj.xval) && (yval == obj.yval) && (interpolationMode == obj.interpolationMode));
            }
            /// <summary>
            /// Hashcode Ermitteln
            /// </summary>
            /// <returns>Hash</returns>
            public override int GetHashCode()
            {
                return base.GetHashCode() ^ this.xval.GetHashCode() ^ this.yval.GetHashCode() ^ this.interpolationMode.GetHashCode();
            }
        }

        /// <summary>
        /// VG Schlüssel Skalierungswert-Unterklasse
        /// </summary>
        public class KeyVGScaleValues : KeyBaseVG
        {
            /// <summary>
            /// Typ
            /// </summary>
            public VGAxisType type { get; set; }
            /// <summary>
            /// Gleichheits Operation
            /// </summary>
            /// <param name="obj">Vergleichsobjekt</param>
            /// <returns>Gleichheits-Aussage</returns>
            public bool Equals(KeyVGScaleValues obj)
            {
                return (base.Equals(obj) && (type == obj.type));
            }
            /// <summary>
            /// Hashcode Ermitteln
            /// </summary>
            /// <returns>Hash</returns>
            public override int GetHashCode()
            {
                return base.GetHashCode() ^ this.type.GetHashCode();
            }
        }

        /// <summary>
        /// Prüfgrenzen Werte
        /// </summary>
        public class ValueVGCheckBoundaries
        {
            /// <summary>
            /// X-Grenze
            /// </summary>
            public double xval { get; set; }
            /// <summary>
            /// Y-Grenze
            /// </summary>
            public double yval { get; set; }
        }


        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for VG
        /// </summary>
        public CachedVGDao()
        {
        }

        private object getCachedData(object cacheid)
        {
            if (!VGCache.ContainsKey(cacheid))
            {
                object val = null;
                if (cacheid is KeyVGValue)
                {
                    KeyVGValue vgcacheid = (KeyVGValue)cacheid;
                    val = base.getVGValue(vgcacheid.sysVg, vgcacheid.perDate, vgcacheid.xval, vgcacheid.yval, vgcacheid.interpolationMode);
                }
                else if (cacheid is KeyVGScaleValues)
                {
                    KeyVGScaleValues vgcacheid = (KeyVGScaleValues)cacheid;
                    val = base.getVGScaleValues(vgcacheid.sysVg, vgcacheid.perDate, vgcacheid.type);
                }
                else if (cacheid is KeyBaseVG)
                {
                    KeyBaseVG vgcacheid = (KeyBaseVG)cacheid;
                    val = base.getVGBoundaries(vgcacheid.sysVg, vgcacheid.perDate);
                }
                VGCache[cacheid] = val;
            }
            return VGCache[cacheid];
        }
        private object getCachedData(KeyBaseVG cacheid, ValueVGCheckBoundaries cacheValue)
        {
            if (!VGCheckBoundariesCache.ContainsKey(cacheid))
            {
                double cacheXVal = 0;
                double cacheYVal = 0;
                base.checkBoundaries(cacheid.sysVg, cacheid.perDate, ref cacheXVal, ref cacheYVal);
                cacheValue.xval = cacheXVal;
                cacheValue.yval = cacheYVal;
                VGCheckBoundariesCache[cacheid] = cacheValue;
            }
            return VGCheckBoundariesCache[cacheid];
        }

        /// <summary>
        /// New Signature for cicvalue without format parameters
        /// Description see other getVGValue-method
        /// </summary>
        /// <param name="perDate"></param>
        /// <param name="sysVg"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <param name="interpolationMode"></param>
        /// <returns></returns>
        public override double getVGValue(long sysVg, DateTime perDate, string xval, string yval, VGInterpolationMode interpolationMode)
        {
            KeyVGValue cacheid = new KeyVGValue { sysVg = sysVg, perDate = perDate, xval = xval, yval = yval, interpolationMode = interpolationMode };
            return (double)getCachedData(cacheid);
        }

        /// <summary>
        /// Abfrage nach Stas 
        /// Liest die Zinssätze nach Produkt ID, Saldo und Laufzeit aus.
        /// Und berücksichtigt das Gültigkeitsdatum
        /// </summary>
        /// <param name="perDate">Aktuelles Datum</param>
        /// <param name="sysvg">Wertegruppen ID</param>
        /// <param name="saldo">Saldo</param>
        /// <param name="lz">Laufzeit</param>
        /// <returns>Zinssatz</returns>
        public override double getVGValueSaldo(long sysvg, double saldo, long lz, DateTime perDate)
        {
            String key = sysvg + "_" + saldo + "_" + lz + "_" + perDate.Year + "_" + perDate.Month + "_" + perDate.Day;
            if (!saldoCache.ContainsKey(key))
            {
                saldoCache[key] = base.getVGValueSaldo(sysvg, saldo, lz, perDate);
            }
            return saldoCache[key];
        }

        /// <summary>
        /// Abfrage nach Stas 
        /// Liest die Prämienzinssätze nach Produkt ID und Laufzeit aus.
        /// Und berücksichtigt das Gültigkeitsdatum
        /// </summary>
        /// <param name="perDate">Aktuelles Datum</param>
        /// <param name="sysvg">Wertegruppen ID</param>
        /// <param name="lz">Laufzeit</param>
        /// <returns>Zinssatz</returns>
        public override double getVGValuePraemie(long sysvg, long lz, DateTime perDate)
        {
            String key = sysvg + "_" + lz + "_" + perDate.Year + "_" + perDate.Month + "_" + perDate.Day;
            if (!praemieCache.ContainsKey(key))
            {
                praemieCache[key] = base.getVGValuePraemie(sysvg, lz, perDate);
            }
            return praemieCache[key];
        }

        /// <summary>
        /// Delivers the scale values for the given axis
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override String[] getVGScaleValues(long sysVg, DateTime perDate, VGAxisType type)
        {
            KeyVGScaleValues cacheid = new KeyVGScaleValues { sysVg = sysVg, perDate = perDate, type = type };
            return (String[])getCachedData(cacheid);
        }

        /// <summary>
        /// Delivers the the value group boundaries
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public override VGBoundaries getVGBoundaries(long sysVg, DateTime perDate)
        {
            KeyBaseVG cacheid = new KeyBaseVG { sysVg = sysVg, perDate = perDate };
            return (VGBoundaries)getCachedData(cacheid);
        }
    }
}