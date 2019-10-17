using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class RollentypDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long sysroletype { get; set; }

        override public long getEntityId()
        {
            return sysroletype;
        }

        override public String getEntityBezeichnung()
        {
            return name;
        }

        public String name { get; set; }
        public String description { get; set; }

    }
}

