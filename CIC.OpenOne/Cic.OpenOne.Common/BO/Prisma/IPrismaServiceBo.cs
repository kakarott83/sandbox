using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Prisma Product Business Object Interface
    /// </summary>
    public interface IPrismaServiceBo
    {
        /// <summary>
        /// Verfügbare Dienste auflisten
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="isoCode">ISO Sprachen Code</param>
        /// <returns>Diensteliste</returns>
        List<AvailableServiceDto> listAvailableServices(srvKontextDto context, string isoCode);
    }
}
