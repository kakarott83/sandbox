using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO;
using System.Reflection;
using System.Dynamic;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// BO for Sort Trees
    /// </summary>
    public class SortTreeBo : AbstractSortTreeBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sDao">Sorttree DAO</param>
        public SortTreeBo(ISortTreeDao sDao)
            : base(sDao)
        {

          
           
        }


        /// <summary>
        /// get SortTree Root for given WfTableCode
        /// </summary>
        /// <returns>Root of the sort tree</returns>
        public override DTO.Prisma.SortTreeNode getSortTree()
        {
            return this.sDao.getSortTree();
        }
    }

   
    

   
}
