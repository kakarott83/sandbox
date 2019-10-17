using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    /// <summary>
    /// Subvention Data Access Object Interface
    /// </summary>
    public interface ISubventionDao
    {
        /// <summary>
        /// Delivers all subvention trigger infos as of Prisma Concept 5.6.2.1
        /// </summary>
        /// <param name="perDate"></param>
        /// <returns></returns>
        List<PrSubvTriggerDto> getSubventionTriggers(DateTime perDate);

        /// <summary>
        /// returns all Subventions
        /// </summary>
        /// <returns></returns>
        List<PRSUBV> getSubventions();

        /// <summary>
        /// returns all Subvention Positions
        /// </summary>
        /// <returns></returns>
        List<PRSUBVPOS> getSubventionPositions(long sysprsubv);
    
        /// <summary>
        /// returns all Subvention Positions
        /// </summary>
        /// <returns></returns>
        List<PRSUBVPOS> getSubventionPositions(long sysprsubv, double betrag);

        /// <summary>
        /// returns all excplicit subventions ids for the given area and id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        List<long> getExplicitSubventionIds(ExplicitSubventionArea area, long areaId);
    }
}
