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

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Quote Data Access Object
    /// </summary>
    public class QuoteDao : IQuoteDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string QUERYQUOTEDATBYBEZEICHNUNG = "select PROZENT from quote, quotedat where quote.sysquote=quotedat.sysquote and quote.bezeichnung=:bezeichnung and (quotedat.gueltigab<=sysdate or gueltigab is null) order by gueltigab desc";
        private const string QUERYQUOTEDATBYSYSQUOTE = "select PROZENT from quote, quotedat where quote.sysquote=quotedat.sysquote and quote.sysquote=:sysquote and (quotedat.gueltigab<=sysdate  or gueltigab is null) order by gueltigab desc";
        private const string QUERYQUOTES = "select PROZENT, gueltigab,bezeichnung,quote.sysquote from quote, quotedat where quote.sysquote=quotedat.sysquote order by bezeichnung, gueltigab desc";
        protected DateTime nullDate = new DateTime(1800, 1, 1);

        /// <summary>
        /// Standard Constructor
        /// Database access Object for Zins
        /// </summary>
        public QuoteDao()
        {

        }

        /// <summary>
        /// returns true if the quote exists for the given date
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        virtual public bool exists(String name, DateTime perDate)
        {
             List<QuoteInfoDto> quotes = getQuotes();
             String test =   (from a in quotes
                    where a.bezeichnung.Equals(name)
                    && (!a.gueltigab.HasValue || a.gueltigab.Value <= perDate || a.gueltigab.Value <= nullDate)
                                  select a.bezeichnung).FirstOrDefault();
             return test != null;

        }

        /// <summary>
        /// gets the Quote for the given date by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        virtual public double getQuote(String name, DateTime perDate)
        {
            List<QuoteInfoDto> quotes = getQuotes();
            return  (from a in quotes
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
        virtual public double getQuote(long sysquote, DateTime perDate)
        {
            List<QuoteInfoDto> quotes = getQuotes();
            return (from a in quotes
                    where a.sysquote == sysquote
                    && (!a.gueltigab.HasValue || a.gueltigab.Value <= perDate || a.gueltigab.Value <= nullDate)
                    select a.prozent).FirstOrDefault();
        }

        /// <summary>
        /// returns all available Quotes
        /// </summary>
        /// <returns></returns>
        virtual public List<QuoteInfoDto> getQuotes()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<QuoteInfoDto>(QUERYQUOTES, null).ToList();
            }
        }

        /// <summary>
        /// Get Quote for current date by Name
        /// </summary>
        /// <returns></returns>
        virtual public double getQuote(String name)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "bezeichnung", Value = name } };
                return ctx.ExecuteStoreQuery<double>(QUERYQUOTEDATBYBEZEICHNUNG, pars).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get Quote for current date by Key
        /// </summary>
        /// <returns></returns>
        virtual public double getQuote(long sysquote)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysquote", Value = sysquote } };
                return ctx.ExecuteStoreQuery<double>(QUERYQUOTEDATBYSYSQUOTE, pars).FirstOrDefault();
            }
        }


    }
}
