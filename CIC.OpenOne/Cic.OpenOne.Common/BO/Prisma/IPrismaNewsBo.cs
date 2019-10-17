using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Prisma News Business Object Interface
    /// </summary>
    public interface IPrismaNewsBo
    {
        /// <summary>
        /// Get News entry
        /// </summary>
        /// <param name="sysprnews">ID</param>
        /// <returns>News</returns>
        PRNEWS getNews(long sysprnews);


        /// <summary>
        /// List available News
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="deliverBinaries">Binaries mitliefern-Flag</param>
        /// <returns>News List</returns>
        List<AvailableNewsDto> listAvailableNews(prKontextDto context, string isoCode, bool deliverBinaries);

    }
}
