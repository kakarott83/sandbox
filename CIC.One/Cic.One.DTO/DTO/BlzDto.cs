using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class BlzDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysblz { get; set; }
        /*Bankname */
        public String name { get; set; }
        /*Bankleitzahl */
        public String blz { get; set; }
        /*BIC */
        public String bic { get; set; }
        override public long getEntityId()
        {
            return sysblz;
        }
    }
}