using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class StaffeltypDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long syssltyp { get; set; }

        override public long getEntityId()
        {
            return syssltyp;
        }

        override public String getEntityBezeichnung()
        {
            return bezeichnung;
        }

        public String bezeichnung { get; set; }
    }
}

