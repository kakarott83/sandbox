using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// SortTree Business Object Interface
    /// </summary>
    public interface ISortTreeBo
    {
        /// <summary>
        /// get SortTree Root for given WfTableCode
        /// </summary>
        /// <returns>Root of the sort tree</returns>
        SortTreeNode getSortTree();

        /// <summary>
        /// Removes all Tree-Leaf-Items which are not contained in the provided item id list
        /// </summary>
        /// <param name="root"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        SortTreeNode filterTree(SortTreeNode root, List<FlatSortableDto> items);

        /// <summary>
        /// Flattens the tree to a list
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        List<SortTreeNode> flattenTree(SortTreeNode root);

        /// <summary>
        /// builds a tree from the list
        /// </summary>
        /// <param name="items">Baumeinträge</param>
        /// <param name="noNewRoot">Neue Root-Flag</param>
        /// <returns></returns>
        SortTreeNode buildTree(List<SortTreeNode> items, bool noNewRoot);
    }
}
