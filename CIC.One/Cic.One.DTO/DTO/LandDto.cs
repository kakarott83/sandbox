using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class LandDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysLand { get; set; }
        /*Land */
        public String countryName { get; set; }

        override public long getEntityId()
        {
            return sysLand;
        }

    }
}