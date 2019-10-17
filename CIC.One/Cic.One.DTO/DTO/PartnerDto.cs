using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class PartnerDto : AccountDto
    {
       

        /// <summary>
        /// typCode
        /// </summary>
        public string typCode { get; set; }


        /// <summary>
        /// funcCode
        /// </summary>
        public string funcCode { get; set; }
        /*Zusatzinformation */
        public String additionalInfo { get; set; }

        public long sysptrelate { get; set; }
        public long sysperson1 { get; set; }
        public long sysperson2 { get; set; }

        public int activeFlag { get; set; }
        public DateTime? relbeginndate { get; set; }
        public DateTime? relenddate { get; set; }

        public virtual String beziehung { get; set; }
        public virtual String beziehungsfunktion { get; set; }
        public virtual String beziehungstyp { get; set; }
        public virtual String beziehungzu { get; set; }
        
        override public long getEntityId()
        {
            return sysptrelate;
        }

        public long sysPartner { get { return sysptrelate; } }
    }
}