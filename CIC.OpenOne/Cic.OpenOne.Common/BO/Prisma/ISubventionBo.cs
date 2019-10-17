using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Subvention Business Object Interface
    /// </summary>
    public interface ISubventionBo
    {
        /// <summary>
        /// calculates the subvention
        /// </summary>
        /// <param name="subvention"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<AngAntSubvDto> calcSubvention(iSubventionDto subvention, prKontextDto context);
    }
}
