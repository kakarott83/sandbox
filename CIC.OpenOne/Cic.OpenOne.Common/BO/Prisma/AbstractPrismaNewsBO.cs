using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Abstract Class: Prisma Product Business Object
    /// </summary>
    public abstract class AbstractPrismaNewsBO : IPrismaNewsBo
    {
        /// <summary>
        /// Prisma Data Access Object
        /// </summary>
        protected IPrismaDao pDao;

        /// <summary>
        /// Object Type Data Access Object
        /// </summary>
        protected IObTypDao obDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao">Prisma Data Access Object</param>
        /// <param name="obDao">Object Type Data Access Object</param>
        public AbstractPrismaNewsBO(IPrismaDao pDao, IObTypDao obDao)
        {
            this.pDao = pDao;
            this.obDao = obDao;
        }

        /// <summary>
        /// Get News entry
        /// </summary>
        /// <param name="sysprnews">ID</param>
        /// <returns>News</returns>
        public abstract PRNEWS getNews(long sysprnews);


        /// <summary>
        /// List available News
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="deliverBinaries">Binaries mitliefern-Flag</param>
        /// <returns>News List</returns>
        public abstract List<AvailableNewsDto> listAvailableNews(prKontextDto context, string isoCode, bool deliverBinaries);


    }
}
