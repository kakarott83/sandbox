using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AntobslDto : EntityDto
    {
      
        /// <summary>
        /// 
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// sysVT/ANGOBSL
        /// </summary>
        public long sysVT { get; set; }

        /// <summary>
        /// Rang /ANGOBSL
        /// </summary>
        public int rang { get; set; }

        /// <summary>
        /// Bezeichnung/ANGOBSL
        /// </summary>
        public string bezeichnung { get; set; }

        /// <summary>
        /// Faellig/ANGOBSL
        /// </summary>
        public DateTime faellig { get; set; }

        /// <summary>
        /// AntobslposDto
        /// </summary>
        public List<AntobslposDto> antobslposList { get; set; }

        /// <summary>
        /// activeFlag 
        /// </summary>
        public int activeFlag { get; set; }

        /// <summary>
        /// Antrag 
        /// </summary>
        public string antrag { get; set; }

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