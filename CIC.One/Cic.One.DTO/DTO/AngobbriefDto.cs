using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class AngobbriefDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long sysangobbrief { get; set; }

        override public long getEntityId()
        {
            return sysangobbrief;
        }

        override public String getEntityBezeichnung()
        {
            return aart;
        }

        public long sysob { get; set; }
        public string aart { get; set; }
        public string treibstoff { get; set; }
        public string getriebe { get; set; }
        public string motor { get; set; }
        public int hubraum { get; set; }
        public string antrieb { get; set; }
        public string energieeff { get; set; }
        public int leergew { get; set; }
        public int zulgew { get; set; }
        public int achsen { get; set; }
        public string reifv { get; set; }
        public string reifmuh { get; set; }
        public string felgv { get; set; }
        public string felgmuh { get; set; }
        public int kmh { get; set; }
        public int tank { get; set; }
        public double verbrauchgesamt { get; set; }
        public string fident { get; set; }
        public string motornummer { get; set; }
        public string laufnummer { get; set; }
        public string impcode { get; set; }
        public double co2emi { get; set; }
        public double nox { get; set; }
        public double dpf { get; set; }
    }
}
