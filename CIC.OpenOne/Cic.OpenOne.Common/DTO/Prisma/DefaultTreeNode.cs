using System.Collections.Generic;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// DefaultTreeNode-Klasse
    /// </summary>
    public class DefaultTreeNode : AbstractTreeNode, System.IEquatable<DefaultTreeNode>
    {
        /// <summary>
        ///the dataobject referenced by sysid
        /// </summary>
        private object data;

        /// <summary>
        /// DB ID 
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Parent ID
        /// </summary>
        public long? sysparent { get; set; }

        /// <summary>
        /// Gleichheitsauswertung
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Vergleichsergebnis</returns>
        public bool Equals(DefaultTreeNode other)
        {
            return sysid == other.sysid;
        }

        /// <summary>
        /// Hashcode ermitteln
        /// </summary>
        /// <returns>Hash</returns>
        public override int GetHashCode()
        {
            return (int)sysid;
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
                depth++;
            }
            return depth;
        }

        /// <summary>
        /// Tiefe des Baums ermitteln
        /// </summary>
        /// <returns>Tiefe des Baums</returns>
        public List<long> getPath()
        {
            ITreeNode p = this;
            List<long> rval = new List<long>();
            rval.Add(this.sysid);
            while ((p = p.getParent()) != null)
            {
                DefaultTreeNode node = (DefaultTreeNode)p;
                rval.Add(node.sysid);
            }
            return rval;
        }
    }
}
