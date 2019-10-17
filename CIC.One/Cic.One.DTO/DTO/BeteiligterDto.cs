using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class BeteiligterDto : AccountDto
    {
       
        /// <summary>
        /// typCode
        /// </summary>
        public string typCode { get; set; }

        /*Zusatzinformation */
        public String additionalInfo { get; set; }

        public long syscrmnm { get; set; }
        public long sysidparent { get; set; }
        public long sysidchild { get; set; }
        public String parentarea {get;set;}
        public String childarea { get; set; }
        public int activeFlag { get; set; }
        

        override public long getEntityId()
        {
            return syscrmnm;
        }

        public long sysPartner { get { return syscrmnm; } }
    }
}