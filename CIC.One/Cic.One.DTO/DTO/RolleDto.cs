using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class RolleDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long sysperole { get; set; }

        override public long getEntityId()
        {
            return sysperole;
        }

        override public String getEntityBezeichnung()
        {
            return name;
        }

        public long sysroletype { get; set; }
        public long sysperson { get; set; }
        public String name { get; set; }
        public String description { get; set; }

    }
}

