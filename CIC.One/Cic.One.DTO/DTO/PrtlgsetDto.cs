using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class PrtlgsetDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysprtlgset;
        }

        public long sysprtlgset { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public long activeflag { get; set; }
        public DateTime? validfrom { get; set; }
        public DateTime? validuntil { get; set; }
    }
}
