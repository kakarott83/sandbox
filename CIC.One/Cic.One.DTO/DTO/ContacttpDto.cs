using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class ContacttpDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysContactTp { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Beschreibung */
        public String description { get; set; }
        /*Code für Icon, Farbe */
        public String designCode { get; set; }
        /*Active Flag*/
        public int activeFlag { get; set; }

        override public long getEntityId()
        {
            return sysContactTp;
        }
    }
}