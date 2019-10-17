using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AntobslposDto : EntityDto
    {

        /// <summary>
        /// sysid
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// sysVTOBSL/ANGOBSLPOS
        /// </summary>
        public long sysvtobsl { get; set; }

        /// <summary>
        /// Rang/ANGOBSLPOS
        /// </summary>
        public int rang{ get; set; }

        /// <summary>
        /// Anzahl/ANGOBSLPOS
        /// </summary>
        public int anzahl { get; set; }

        /// <summary>
        /// Betrag/ANGOBSLPOS
        /// </summary>
        public long betrag { get; set; }

        /// <summary>
        /// activeFlag 
        /// </summary>
        public int activeFlag { get; set; }

        /// <summary>
        /// Staffel 
        /// </summary>
        public string staffel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public long getEntityId()
        {
            return sysid;
        }
    }
}