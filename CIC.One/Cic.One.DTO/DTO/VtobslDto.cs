using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class VtobslDto : EntityDto
    {


        /// <summary>
        /// vtobsl
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// VT
        /// </summary>
        public long sysVT {get;set;}
	
        /// <summary>
        /// Rang
        /// </summary>
        public int rang	{get;set;}

        /// <summary>
        /// Bezeichnung
        /// </summary>
        public string bezeichnung {get;set;}

        /// <summary>
        /// Fällig ab
        /// </summary>
        public DateTime faellig { get; set; }

        /// <summary>
        /// vtobslpostList
        /// </summary>
        public List<VtobslposDto> vtobslpostList { get; set; }

        /// <summary>
        /// activeFlag 
        /// </summary>
        public int activeFlag { get; set; }

        /// <summary>
        /// vertrag 
        /// </summary>
        public string vertrag { get; set; }

        /// <summary>
        /// Abo Auszahlungsbetrag
        /// </summary>
        public double folgerate { get; set; }

        /// <summary>
        /// Abo Periodizität
        /// </summary>
        public int ppy { get; set; }



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