using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class ZahlungsplanDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long sysslpos { get; set; }

        override public long getEntityId()
        {
            return sysslpos;
        }

        override public String getEntityBezeichnung()
        {
            return gebiet;
        }

        //tabelle sllink
        public long syssllink { get; set; }
        public long syssl { get; set; }
        public long sysid { get; set; }
        public string gebiet { get; set; }

        //tabelle sl
        //public long syssl { get; set; }
        public long syssltyp { get; set; }
        public bool inaktiv { get; set; }
        public DateTime? inaktivab { get; set; }
        public String tilgungstyp { get; set; }
        //tabelle slpos
        //public long sysslpos { get; set; }
        public long sysslpostyp { get; set; }
        //public long syssl { get; set; }
        public long sysrn { get; set; }
        public DateTime? valuta { get; set; }
        public int gezogen { get; set; }
        public double betrag { get; set; }
        public double zinssaldo { get; set; }
        public int zinstage { get; set; }
        public double zinsen { get; set; }
        public double zinsenp { get; set; }
        public double zinsenab { get; set; }
        public double tilgsaldo { get; set; }
        public int tilgtage { get; set; }
        public double tilgung { get; set; }
        public double tilgungp { get; set; }
        public double restforderung { get; set; }
    }
}
