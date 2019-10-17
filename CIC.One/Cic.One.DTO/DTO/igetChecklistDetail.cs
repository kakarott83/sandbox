using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
  
    public class igetChecklistDetailDto
    {
        /// <summary>
        /// sysid ANTRAG
        /// </summary>
        public long sysid { get; set; }

        public long sysbplistener { get; set; }
        public long sysbprole { get; set; }
        public int salesFlag { get; set; }
        public String namebplane { get; set; }

    }
}
