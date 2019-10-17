using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// common tree interface
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// Parent holen
        /// </summary>
        /// <returns>Parent</returns>
        ITreeNode getParent();

        /// <summary>
        /// Parent setzen
        /// </summary>
        /// <param name="parent">Parent</param>
        void setParent(ITreeNode parent);

        /// <summary>
        /// Kinder holen
        /// </summary>
        /// <returns>Kinder</returns>
        List<ITreeNode> getChildren();

        /// <summary>
        /// Kinder setzen
        /// </summary>
        /// <param name="children">Kinder</param>
        void setChildren(List<ITreeNode> children);

        /// <summary>
        /// Kind setzen
        /// </summary>
        /// <param name="treeNode">Kind</param>
        void addChild(ITreeNode treeNode);

        /// <summary>
        /// Tiefe ermitteln
        /// </summary>
        /// <returns>Tiefe</returns>
        int getDepth();
    }
}
