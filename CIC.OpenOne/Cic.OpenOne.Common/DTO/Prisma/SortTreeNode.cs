using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Node of a SortTree
    /// </summary>
    public class SortTreeNode : AbstractTreeNode, IFlatSortableDto, System.IEquatable<SortTreeNode>
    {
        /// <summary>
        ///the dataobject referenced by sysid
        /// </summary>
        private object data;

        /// <summary>
        /// DB ID 
        /// </summary>
        public long syssortpos { get; set; }

        /// <summary>
        /// Parent ID
        /// </summary>
        public long syssortposp { get; set; }

        /// <summary>
        /// WFTABLE Code reference describing the type of sysid
        /// </summary>
        public String wftablecode { get; set; }

        /// <summary>
        /// Linked Entity Id
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Sorttyp Description 
        /// </summary>
        public String sortTypBezeichnung { get; set; }

        /// <summary>
        /// Visibility 
        /// </summary>
        public bool visible { get; set; }
       
        /// <summary>
        /// Description
        /// </summary>
        public String bezeichnung { get; set; }

        /// <summary>
        /// Order 
        /// </summary>
        public long rang { get; set; }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public SortTreeNode() { }

        //Copy Constructor;
        public SortTreeNode(SortTreeNode n)
        {
            this.bezeichnung = n.bezeichnung;
            this.data = n.data;
            this.rang = n.rang;
            this.sortTypBezeichnung = n.sortTypBezeichnung;
            this.sysid = n.sysid;
            this.syssortpos = n.syssortpos;
            this.syssortposp = n.syssortposp;
            this.visible = n. visible;
            this.wftablecode = n.wftablecode;
        }

        /// <summary>
        /// Primary Key auslesen
        /// </summary>
        /// <returns>Primary Key</returns>
        public long getSortTargetId()
        {
            return sysid;
        }

        /// <summary>
        /// Gleichheitsauswertung
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Vergleichsergebnis</returns>
        public bool Equals(SortTreeNode other)
        {
            return getSortSourceId() == other.getSortSourceId();
        }

        /// <summary>
        /// Sortierposition auslesen
        /// </summary>
        /// <returns>Sortierposition</returns>
        public long getSortSourceId()
        {
            return syssortpos;
        }

        /// <summary>
        /// Hashcode ermitteln
        /// </summary>
        /// <returns>Hash</returns>
        public override int GetHashCode()
        {
            return (int)getSortSourceId();

        }

        /// <summary>
        /// Daten auslesen
        /// </summary>
        /// <returns>Daten</returns>
        public object getData()
        {
            return data;
        }

        /// <summary>
        /// Daten setzen
        /// </summary>
        /// <param name="obj">Daten</param>
        public void setData(object obj)
        {
            this.data = obj;
        }

        /// <summary>
        /// Tiefe des Baums ermitteln
        /// </summary>
        /// <returns>Tiefe des Baums</returns>
        new
        public int getDepth()
        {
            int depth = 0;
            ITreeNode p = this;
            while ((p = p.getParent()) != null)
            {
                if(  ((SortTreeNode)p).visible)
                    depth++;
            }
            return depth;
        }
    }
}
