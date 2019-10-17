using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Prisma Parameter Business Object Interface
    /// </summary>
    public interface IPrismaParameterBo
    {
        /// <summary>
        /// Get List of Available Parameters
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>Parameterlist</returns>
        List<ParamDto> listAvailableParameter(prKontextDto context);


         /// <summary>
        /// gets an available prisma parameter for the given field code
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objectmeta"></param>
        /// <returns></returns>
        ParamDto getParameter(prKontextDto context, String objectmeta);

        /// <summary>
        /// gets an available field ID for the given field code
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objectmeta"></param>
        /// <returns></returns>
        long getFieldID(prKontextDto context, String objectmeta);

    }
}
