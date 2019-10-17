using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class CamptpDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysCampTp { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Beschreibung */
        public String description { get; set; }
        /*Active Flag*/
        public int activeFlag { get; set; }

        override public long getEntityId()
        {
            return sysCampTp;
        }
    }
}