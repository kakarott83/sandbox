using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Quote Data Access Object
    /// </summary>
    public class CachedQuoteDao : QuoteDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<String, double> quoteNameCache = CacheFactory<String, double>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, double> quoteCache = CacheFactory<long, double>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        private static CacheDictionary<String, List<QuoteInfoDto>> quoteInfoCache = CacheFactory<String, List<QuoteInfoDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        /// <summary>
        /// Standard Constructor
        /// Database access Object for Quotes
        /// </summary>
        public CachedQuoteDao()
        {

        }

        /// <summary>
        /// returns true if the quote exists for the given date
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        override public bool exists(String name, DateTime perDate)
        {
            List<QuoteInfoDto> quotes = getQuotes();
            String test = (from a in quotes
                           where a.bezeichnung.Equals(name)
                            && (!a.gueltigab.HasValue || a.gueltigab.Value <= perDate || a.gueltigab.Value <= nullDate)
                           select a.bezeichnung).FirstOrDefault();
            return test != null;

        }

        /// <summary>
        /// returns all available Quotes
        /// </summary>
        /// <returns></returns>
        override public List<QuoteInfoDto> getQuotes()
        {
            if (!quoteInfoCache.ContainsKey("X"))
            {
                quoteInfoCache["X"] = base.getQuotes();
            }
            return quoteInfoCache["X"];
        }

        /// <summary>
        /// gets the Quote for the given date by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        override public double getQuote(String name, DateTime perDate)
        {
            List<QuoteInfoDto> quotes = getQuotes();
            return (from a in quotes
                    where a.bezeichnung.Equals(name)
                     && (!a.gueltigab.HasValue || a.gueltigab.Value <= perDate || a.gueltigab.Value <= nullDate)
                    select a.prozent).FirstOrDefault();
        }

        /// <summary>
        /// gets the Quote for the given date by id
        /// </summary>
        /// <param name="sysquote"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        override public double getQuote(long sysquote, DateTime perDate)
        {
            List<QuoteInfoDto> quotes = getQuotes();
            return (from a in quotes
                    where a.sysquote == sysquote
                     && (!a.gueltigab.HasValue || a.gueltigab.Value <= perDate || a.gueltigab.Value <= nullDate)
                    select a.prozent).FirstOrDefault();
        }

        /// <summary>
        /// Get Quote  for current date by Name
        /// </summary>
        /// <returns></returns>
        override public double getQuote(String name)
        {
            if (!quoteNameCache.ContainsKey(name))
            {
                quoteNameCache[name] = base.getQuote(name);
            }
            return quoteNameCache[name];
        }

        /// <summary>
        /// Get Quote  for current date by Key
        /// </summary>
        /// <returns></returns>
        override public double getQuote(long sysquote)
        {
            if (!quoteCache.ContainsKey(sysquote))
            {
                quoteCache[sysquote] = base.getQuote(sysquote);
            }
            return quoteCache[sysquote];
        }


    }
}
