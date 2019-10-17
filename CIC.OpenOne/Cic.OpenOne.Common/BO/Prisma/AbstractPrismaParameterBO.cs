using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Abstract Class: Prisma Parameter Business Object
    /// </summary>
    public abstract class AbstractPrismaParameterBO : IPrismaParameterBo
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
        public AbstractPrismaParameterBO(IPrismaDao pDao, IObTypDao obDao)
        {
            this.pDao = pDao;
            this.obDao = obDao;
        }

        /// <summary>
        /// List Available Parameters
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>List of Parameters</returns>
        public abstract List<ParamDto> listAvailableParameter(prKontextDto context);

         /// <summary>
        /// gets an available prisma parameter for the given field code
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objectmeta"></param>
        /// <returns></returns>
        public abstract ParamDto getParameter(prKontextDto context, String objectmeta);

        /// <summary>
        /// gets an available field ID for the given field code
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objectmeta"></param>
        /// <returns></returns>
        public abstract long getFieldID(prKontextDto context, String objectmeta);

    }
}
