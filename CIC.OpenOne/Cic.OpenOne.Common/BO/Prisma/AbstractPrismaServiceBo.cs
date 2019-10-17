using System.Collections.Generic;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Abstract Class: Prisma Product Business Object
    /// </summary>
    public abstract class AbstractPrismaServiceBo : IPrismaServiceBo
    {
        /// <summary>
        /// Prisma Data Access Object
        /// </summary>
        protected IPrismaServiceDao pDao;

        /// <summary>
        /// Object Type Data Access Object
        /// </summary>
        protected IObTypDao obDao;

        /// <summary>
        /// Object Type Data Access Object
        /// </summary>
        protected ITranslateDao translateDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao">Prisma Data Access Object</param>
        /// <param name="obDao">Object Type Data Access Object</param>
        /// <param name="translateDao">Übersetzungs DAO</param>
        public AbstractPrismaServiceBo(IPrismaServiceDao pDao, IObTypDao obDao, ITranslateDao translateDao)
        {
            this.pDao = pDao;
            this.obDao = obDao;
            this.translateDao = translateDao;
        }

        /// <summary>
        /// List available Products
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns>Product List</returns>
        public abstract List<AvailableServiceDto> listAvailableServices(srvKontextDto context, string isoCode);
    }
}