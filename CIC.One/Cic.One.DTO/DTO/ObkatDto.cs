using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ObkatDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysobkat;
        }

        public long sysobkat { get; set; } 
        public String name { get; set; }
        public String description { get; set; }
    }
}
