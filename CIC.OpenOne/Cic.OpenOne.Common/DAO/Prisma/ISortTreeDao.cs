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
    /// Sort Tree Access Object Interface
    /// </summary>
    public interface ISortTreeDao
    {
        /// <summary>
        /// get SortTree Root for given WfTableCode
        /// </summary>
        /// <returns>Root of the sort tree</returns>
        SortTreeNode getSortTree();
    }
}
