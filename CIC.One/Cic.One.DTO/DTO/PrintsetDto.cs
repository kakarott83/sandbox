using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class PrintsetDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysprintset;
        }

        public long sysprintset { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public long activeflag { get; set; }
        public DateTime? validfrom { get; set; }
        public DateTime? validuntil { get; set; }
        public long rounding { get; set; }
    }
}
