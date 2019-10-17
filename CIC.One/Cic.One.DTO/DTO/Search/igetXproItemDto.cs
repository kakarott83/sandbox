using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class igetXproItemDto
    {
        public XproEntityType Area { get; set; }
        public long sysvlm { get; set; }
        public long EntityId { get; set; }
        public String isoCode { get; set; }
        public long sysperole { get; set; }

        /// <summary>
        /// ddlkppos xpro code
        /// </summary>
        public String areaCode { get; set; }
        /// <summary>
        /// area domainid for ddlkppos
        /// </summary>
        public String domainId { get; set; }
    }
}