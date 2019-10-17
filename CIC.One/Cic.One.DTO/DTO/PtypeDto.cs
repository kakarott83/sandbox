using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class PtypeDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysPtype { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }
        /*Code */
        public String syscode { get; set; }
        /*Gebiet (zb Aufgaben) */
        public String area { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Beschreibung */
        public String description { get; set; }
        /*Anzeigebedingung (Script) */
        public String visibilityCondition { get; set; }
        /*Ladebedingung (Script) */
        public String loadCondition { get; set; }
        public int systemflag { get; set; }
        public DateTime? validfrom { get; set; }
        public DateTime? validuntil { get; set; }
        /// <summary>
        /// Kontrollnr
        /// </summary>
        public String refnumber { get; set; }
        public int art { get; set; }
        public String status { get; set; }
        public int completeflag { get; set; }
        /// <summary>
        /// Create User syswfuser reference
        /// </summary>
        public long createuser { get; set; }

        override public long getEntityId()
        {
            return sysPtype;
        }
        /// <summary>
        /// Prüferliste
        /// </summary>
        public String plist { get; set; }

        //PCONTENT
        public long syspcontent { get; set; }
        public long sysctlang { get; set; }
        
        public String titel { get; set; }
        public String inhalt { get; set; }
        public String docname { get; set; }
        public byte[] docfile { get; set; }
        public String linktext { get; set; }
        public String linkurl { get; set; }

        public List<PsubjectDto> subjects { get; set; }

        public String checkresult { get; set; }
        public long sysptask { get; set; }

    }
}