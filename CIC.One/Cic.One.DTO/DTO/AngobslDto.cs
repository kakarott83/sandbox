using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AngobslDto : EntityDto
    {
    

        /// <summary>
        /// sysVT
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// sysVT
        /// </summary>
        public long sysvt { get; set; }

        /// <summary>
        /// Rang 
        /// </summary>
        public int rang { get; set; }

        /// <summary>
        /// Bezeichnung
        /// </summary>
        public string bezeichnung { get; set; }

        /// <summary>
        /// Faellig
        /// </summary>
        public DateTime faellig { get; set; }

        /// <summary>
        /// AngobslposDto
        /// </summary>
        public List<AngobslposDto> angobslposList { get; set; }

        /// <summary>
        /// activeFlag 
        /// </summary>
        public int activeFlag { get; set; }

        /// <summary>
        /// Variante 
        /// </summary>
        public string variante { get; set; }

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