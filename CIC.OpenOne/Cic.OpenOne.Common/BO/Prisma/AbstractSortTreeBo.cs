using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Abstract Class: Sort Tree Business Object
    /// </summary>
    public abstract class AbstractSortTreeBo : ISortTreeBo
    {
        /// <summary>
        /// Data Access Object
        /// </summary>
        protected ISortTreeDao sDao;
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sDao">Sortierbaumdaten</param>
        public AbstractSortTreeBo(ISortTreeDao sDao)
        {
            this.sDao = sDao;
        }

        /// <summary>
        /// get SortTree Root for given WfTableCode
        /// </summary>
        /// <returns>Root of the sort tree</returns>
        abstract public SortTreeNode getSortTree();

        private void filter(List<SortTreeNode> nodes, List<FlatSortableDto> items)
        {
            List<SortTreeNode> delNodes = new List<SortTreeNode>();

            foreach (SortTreeNode node in nodes)
            {
                if (node.getChildren() == null || node.getChildren().Count == 0)//a leaf
                {
                    var q = from i in items
                            where i.getSortTargetId() == node.getSortTargetId()
                            select i;
                    if (q.FirstOrDefault() == null) //that is not contained in items
                    {
                        delNodes.Add(node);
                    }
                    node.setData(q.FirstOrDefault());
                }
            }
            foreach (SortTreeNode node in delNodes)
            {
                nodes.Remove(node);
                if (node.getParent() != null)
                    node.getParent().getChildren().Remove(node);
            }
            if (delNodes.Count > 0) filter(nodes, items);
        }

        /// <summary>
        /// Removes all Tree-Leaf-Items and ascending paths which are not contained in the provided item id list
        /// </summary>
        /// <param name="root"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public SortTreeNode filterTree(SortTreeNode root, List<FlatSortableDto> items)
        {
            List<SortTreeNode> nodes = flattenTree(root);
            filter(nodes, items);
            return buildTree(nodes, false);
        }

        /// <summary>
        /// Flattens the tree to a list
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public List<SortTreeNode> flattenTree(SortTreeNode root)
        {
            PreorderEnumeration pe = new PreorderEnumeration(root);

            List<SortTreeNode> rval = new List<SortTreeNode>();
            while (pe.MoveNext())
            {
                rval.Add((SortTreeNode)pe.Current);
            }
            return rval;
        }

        /// <summary>
        /// builds a tree from the list, resetting all parent/child links in the given items
        /// the returned root is a new item!
        /// </summary>
        /// <param name="nodeList">Liste neuer Knoten</param>
        /// <returns></returns>
        public static DefaultTreeNode buildTree(List<DefaultTreeNode> nodeList)
        {
            //create tree items list
            Dictionary<long, DefaultTreeNode> keyItemDict = new Dictionary<long, DefaultTreeNode>();

            keyItemDict[-1] = new DefaultTreeNode();
            foreach (DefaultTreeNode node in nodeList)
            {
                keyItemDict[node.sysid] = node;
                node.setParent(null);
                node.setChildren(null);
            }
            foreach (DefaultTreeNode node in nodeList)
            {
                long key = node.sysparent.HasValue ? node.sysparent.Value : -1;
                if (!keyItemDict.ContainsKey(key))
                {
                    _log.Warn("Invalid root-entry for SortPos " + node.sysid + ": " + node.sysparent + " - skipping SortPos");
                    continue;
                }
                keyItemDict[key].addChild(node);
                node.setParent(keyItemDict[key]);
            }
            return keyItemDict[-1];
        }

        /// <summary>
        /// builds a tree from the list, resetting all parent/child links in the given items
        /// </summary>
        /// <param name="nodeList">Liste neuer Knoten</param>
        /// <param name="noNewRoot">a new item containing the root of the list</param>
        /// <returns></returns>
        public SortTreeNode buildTree(List<SortTreeNode> nodeList, bool noNewRoot)
        {
            //create tree items list
            Dictionary<long, SortTreeNode> keyItemDict = new Dictionary<long, SortTreeNode>();

            keyItemDict[-1] = new SortTreeNode();//root
            foreach (SortTreeNode node in nodeList)
            {
                keyItemDict[node.syssortpos] = node;
                node.setParent(null);
                node.setChildren(null);
            }
            foreach (SortTreeNode node in nodeList)
            {
                if (!keyItemDict.ContainsKey(node.syssortposp))
                {
                    _log.Warn("Invalid root-entry for SortPos " + node.syssortpos + ": " + node.syssortposp + " - skipping SortPos");
                    continue;
                }
                keyItemDict[node.syssortposp].addChild(node);
                node.setParent(keyItemDict[node.syssortposp]);
            }
            if (noNewRoot)
            {
                List<ITreeNode> rootChildren = keyItemDict[-1].getChildren();
                if (rootChildren == null) throw new Exception("Tree has no elements");
                if (rootChildren.Count > 1) throw new Exception("Tree has more than one root element");
                return (SortTreeNode)rootChildren[0];
            }
            return keyItemDict[-1];
        }

        /// <summary>
        /// createPreorderEnumeration
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static PreorderEnumeration createPreorderEnumeration(ITreeNode root)
        {
            return new PreorderEnumeration(root);
        }
    }

    /// <summary>
    /// Vorsortierklasse
    /// </summary>
    public class PreorderEnumeration : IEnumerator
    {
        /// <summary>
        /// stack
        /// </summary>
        protected Stack<IEnumerator<ITreeNode>> stack;
        private ITreeNode current = null;

        /// <summary>
        /// PreorderEnumeration
        /// </summary>
        /// <param name="rootNode"></param>
        public PreorderEnumeration(ITreeNode rootNode)
        {
            List<ITreeNode> v = new List<ITreeNode>();
            v.Add(rootNode);
            stack = new Stack<IEnumerator<ITreeNode>>();
            stack.Push(v.GetEnumerator());
        }

        /// <summary>
        /// MoveNext
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            IEnumerator<ITreeNode> enumer = null;

            do
            {
                enumer = stack.Peek();
                if (!enumer.MoveNext())
                {
                    stack.Pop();
                    enumer = null;
                    if (stack.Count == 0) return false;
                }
            } while (enumer == null);

            this.current = (ITreeNode)enumer.Current;
            List<ITreeNode> children = current.getChildren();

            if (children == null || children.Count == 0)
            {
                ;
            }
            else
            {
                stack.Push(children.GetEnumerator());
            }
            return true;

        }

        /// <summary>
        /// Current
        /// </summary>
        public object Current
        {
            get
            {
                return current;
            }
        }

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
        }
    }

    /// <summary>
    /// Postorder/Breadth-First/Depth-First Enumeration
    /// </summary>
    class PostorderEnumeration : IEnumerator
    {
        protected IEnumerator children;
        protected IEnumerator subChildren;
        private ITreeNode current = null;
        private ITreeNode rootNode;

        public PostorderEnumeration(ITreeNode rootNode)
        {
            this.rootNode = rootNode;
            if (rootNode.getChildren() != null)
                children = rootNode.getChildren().GetEnumerator();
        }

        public bool MoveNext()
        {
            if (subChildren != null && subChildren.MoveNext())
            {
                current = (ITreeNode)subChildren.Current;
                return false;
            }

            if (children != null && children.MoveNext())
            {
                subChildren = new PostorderEnumeration((ITreeNode)children.Current);
                bool t = subChildren.MoveNext();
                current = (ITreeNode)subChildren.Current;
                return t;
            }
            else if (children != null || children == null)
            {
                children = null;
                current = (ITreeNode)rootNode;
                return false;
            }
            else
            {
                throw new Exception("Cant find a node.");
            }
        }

        public object Current
        {
            get
            {
                return current;
            }
        }


        public void Reset()
        {
        }
    }
}