using Cic.OpenOne.Common.DTO;
using System;

namespace Cic.One.DTO
{
    public class igetXproItemsDto
    {
        public String isoCode { get; set; }
        public long sysperole { get; set; }
        public long sysvlm { get; set; }
        /// <summary>
        /// Strongly typed xpro area/code
        /// </summary>
        public XproEntityType Area { get; set; }
        public String favCode { get; set; }
        /// <summary>
        /// ddlkppos xpro code
        /// </summary>
        public String areaCode { get; set; }
        /// <summary>
        /// area domainid for ddlkppos
        /// </summary>
        public String domainId { get; set; }
        public string Filter { get; set; }
        public Filter[] filters { get; set; }
        public DDLKPPOSDomain? domain { get; set; }
    }
}