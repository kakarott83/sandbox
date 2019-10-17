using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class KreditlinieDto : EntityDto
    {
		// rh: neu 20171004
        /// <summary>
		/// Sys ID (rh?: == nkk.sysklinie)
        /// </summary>
		public long sysklinie { get; set; }

        override public long getEntityId()
        {
			return sysklinie;
        }

        override public String getEntityBezeichnung()
        {
			return art;
        }

		public long limitanzahl  { get; set; }
		public long limitsumme { get; set; }
			
    
		// ORIG:
        public string art {get;set;}
        public double gesamtlimit { get; set; }
        public double ausschoepfung {get;set;}
        public double ausschoepfungP {get;set;}
        public DateTime beginn {get;set;}
        public DateTime ende {get;set;}
        public double frei {get;set;}
        public double freiP  {get;set;}
        public double ausschoepfungFF  {get;set;}
        public double ausschoepfungAF { get; set; }
        public double ausschoepfungFFP { get; set; }
        public double ausschoepfungAFP { get; set; }
        public string waehrung { get; set; }

    }
}
