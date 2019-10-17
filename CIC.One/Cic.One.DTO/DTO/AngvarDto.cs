using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AngvarDto : EntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long sysAngvar { get; set; } 

        /// <summary>
        /// sysAngebot
        /// </summary>
        public long sysAngebot{ get; set; }

        /// <summary>
        /// Rang
        /// </summary>
        public int rang {get; set; }

        /// <summary>
        /// Bezeichnung
        /// </summary>
        public string bezeichnung { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string beschreibung { get; set; }

        /// <summary>
        /// AngobDto
        /// </summary>
        public List<AngobDto> angobList { get; set; }


        /// <summary>
        ///  AngobslDto
        /// </summary>
        public List<AngobslDto> angobslList { get; set; }

        /// <summary>
        /// activeFlag 
        /// </summary>
        public int activeFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public long getEntityId()
        {
            return sysAngvar;
        }

      
      
    }
}