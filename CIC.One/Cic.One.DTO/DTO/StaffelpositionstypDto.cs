using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class StaffelpositionstypDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long sysslpostyp { get; set; }

        override public long getEntityId()
        {
            return sysslpostyp;
        }

        override public String getEntityBezeichnung()
        {
            return name;
        }

        public long sysft { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public bool iscashflow { get; set; }

    }
}

