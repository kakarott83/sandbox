using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class HandelsgruppeDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long sysprhgroup { get; set; }

        override public long getEntityId()
        {
            return sysprhgroup;
        }

        override public String getEntityBezeichnung()
        {
            return name;
        }

        public bool activeFlag { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validUntil { get; set; }

    }
}

