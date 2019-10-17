using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Quote Data Access Object Interface
    /// </summary>
    public interface IQuoteDao
    {

        /// <summary>
        /// gets the Quote for the given date by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        double getQuote(String name, DateTime perDate);

        /// <summary>
        /// gets the Quote for the given date by id
        /// </summary>
        /// <param name="sysquote"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        double getQuote(long sysquote, DateTime perDate);

        /// <summary>
        /// Get Quote  for current date by Key
        /// </summary>
        /// <returns></returns>
        double getQuote(long sysquote);

        /// <summary>
        /// Get Quote  for current date by Name
        /// </summary>
        /// <returns></returns>
        double getQuote(String name);

        /// <summary>
        /// returns all available Quotes
        /// </summary>
        /// <returns></returns>
        List<QuoteInfoDto> getQuotes();

        /// <summary>
        /// returns true if the quote exists for the given date
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        bool exists(String name, DateTime perDate);
    }
}
