using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public enum AnciliaryField
    {
        ALL=0,
        LOCKED=1
    }
    public class igetAnciliaryDetailDto
    {
        /// <summary>
        /// sysid
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Area 
        /// </summary>
        public String area { get; set; }

        public AnciliaryField field { get; set; } 
    }
}
