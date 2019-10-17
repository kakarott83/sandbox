using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class WfuserDto : EntityDto
    {
     /// <summary>
        /// Sys ID
        /// </summary>
        public long syswfuser { get; set; }

        override public long getEntityId()
        {
            return syswfuser;
        }

        public String code { get; set; }

      
        /// <summary>
        /// Name
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// Vorname
        /// </summary>
        public String vorname { get; set; }
        
       
        /// <summary>
        /// Strasse
        /// </summary>
        public String strasse { get; set; }
     
        /// <summary>
        /// Postleitzahl
        /// </summary>
        public String plz { get; set; }
        /// <summary>
        /// Ort
        /// </summary>
        public String ort { get; set; }

        /// <summary>
        /// Strasse
        /// </summary>
        public String pstrasse { get; set; }
        /// <summary>
        /// Strasse
        /// </summary>
        public String phsnr { get; set; }

        /// <summary>
        /// Postleitzahl
        /// </summary>
        public String pplz { get; set; }
        /// <summary>
        /// Ort
        /// </summary>
        public String port { get; set; }

        /// <summary>
        /// Land
        /// </summary>
        public long sysland { get; set; }
		/// <summary>
		/// Korrespondenzsprache (rh 20170117: not in DB::wfuser)
		/// </summary>
		public long sysctlangkorr { get; set; }
		/// <summary>
		/// Sprache (rh: neu 20170117)
		/// </summary>
		public long sysctlang { get; set; }
		/// <summary>
        /// Land (sysland) Bezeichnung
        /// </summary>
        public String landBezeichnung { get; set; }

        /// <summary>
        /// ISO Language like de-DE
        /// </summary>
        public String language { get; set; }
        /// <summary>
        /// Telefon
        /// </summary>
        public String telefon { get; set; }
        /// <summary>
        /// Telefon
        /// </summary>
        public String ptelefon { get; set; }

        /// <summary>
        /// Durchwahl
        /// </summary>
        public String durchwahl { get; set; }
        /// <summary>
        /// Mobiltelefon
        /// </summary>
        public String mobile { get; set; }
      
        /// <summary>
        /// E-Mail
        /// </summary>
        public String email { get; set; }
        /// <summary>
        /// Titel
        /// </summary>
        public String titel { get; set; }

        /// <summary>
        /// Korrespondenz
        /// </summary>
        public String korrespondenz { get; set; }

        /// <summary>
        /// Person
        /// </summary>
        public long sysperson { get; set; }

        /// <summary>
        /// Perole
        /// </summary>
        public long sysperole { get; set; }

        /// <summary>
        /// Roletype typ
        /// </summary>
        public long typ { get; set; }

        /// <summary>
        /// Masterflag
        /// </summary>
        public int master { get; set; }

        /// <summary>
        /// Sende-Mail-Adresse
        /// </summary>
        public String extmailaddress { get; set; }
        public String extmailaccount { get; set; }
        public String extmailpassword { get; set; }

        public DateTime? expireDate { get;set;}
        public int disabled { get; set; }
    }
}
