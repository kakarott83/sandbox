using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Abstrakte Klasse: Baum-Knoten
    /// </summary>
    public abstract class AbstractTreeNode : ITreeNode
    {
        private ITreeNode parent;
        private List<ITreeNode> children;

        /// <summary>
        /// Übergeordneten Knoten zurückliefern
        /// </summary>
        /// <returns>Übergeordneter Knoten (null = die ist der oberste Knoten)</returns>
        public ITreeNode getParent()
        {
            return parent;
        }

        /// <summary>
        /// Übergeordneten Knoten setzen
        /// </summary>
        /// <param name="parent">Neuer übergeordneter Knoten</param>
        public void setParent(ITreeNode parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Liste von Kind-Knoten zurückliefern
        /// </summary>
        /// <returns>Kindknotenliste</returns>
        public List<ITreeNode> getChildren()
        {
            return children;
        }

        /// <summary>
        /// Mehrere Knoten als Kinder hinzufügen
        /// </summary>
        /// <param name="children">Liste mit neuen Kind-Knoten</param>
        public void setChildren(List<ITreeNode> children)
        {
            this.children = children;
        }

        /// <summary>
        /// Einzelnen Kindknoten hinzufügen
        /// </summary>
        /// <param name="treeNode">Neur Kind-Knoten</param>
        public void addChild(ITreeNode treeNode)
        {
            if (children == null)
                children = new List<ITreeNode>();

            children.Add(treeNode);
        }

        /// <summary>
        /// Tiefe des Baums ermitteln
        /// </summary>
        /// <returns>Tiefe des Baums</returns>
        public int getDepth()
        {
            int depth = 0;
            ITreeNode p = this;
            while ((p = p.getParent()) != null)
            {
              
                depth++;
            }
            return depth;
        }
    }
}
