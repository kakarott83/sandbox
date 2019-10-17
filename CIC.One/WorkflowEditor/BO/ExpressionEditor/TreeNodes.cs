using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Workflows.BO.ExpressionEditor
{



    public class TreeNodes
    {


        public enum NodeTypes : int
        {
            Namespace = 0,
            Interface,
            Class,
            Method,
            Property,
            Field,
            Enum,
            ValueType,
            Event,
            Primitive
        }

        private List<TreeNodes> _nodes = new List<TreeNodes>();


        public string Name { get; set; }
        public string AddStrings { get; set; }
        public NodeTypes ItemType { get; set; }
        public Type SystemType { get; set; }
        public string Description { get; set; }
        public TreeNodes Parent { get; set; }


        private object _syncLock = new object();
        public List<TreeNodes> Nodes
        {
            get { return _nodes; }
        }



        public void AddNode(TreeNodes target)
        {
            lock ((_syncLock))
            {
                _nodes.Add(target);
            }
        }

        public string GetFullPath()
        {
            string result = this.Name;
            if (Parent != null)
            {
                String parentString = Parent.GetFullPath();
                if (parentString!=null && parentString.Trim().Length>0)
                    result = parentString + "." + result;
            }
            return result;
        }

        public TreeNodes SearchNodes(string namePath)
        {
            return this.SearchNodesInPrivate(this, namePath);
        }
        private TreeNodes SearchNodesInPrivate(TreeNodes targetNodes, string namePath)
        {
            string[] targetPath = namePath.Split('.');
            bool validPath = false;
            TreeNodes existsNodes = null;

            dynamic validNode = (from x in targetNodes.Nodes
                                 where x.Name.ToLower() == targetPath[0].ToLower()
                                 select x);

            if ((validNode != null) && (validNode.Count > 0))
            {
                existsNodes = validNode.FirstOrDefault;
                validPath = true;
            }

            if (!validPath)
                return targetNodes;

            dynamic nextPath = namePath.Substring(targetPath[0].Length, namePath.Length - targetPath[0].Length);
            if (nextPath.StartsWith("."))
                nextPath = nextPath.Substring(1, nextPath.Length - 1);
            if (string.IsNullOrEmpty(nextPath.Trim()))
                return existsNodes;
            return this.SearchNodesInPrivate(existsNodes, nextPath);
        }


    }

}