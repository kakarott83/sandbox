using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class BmwVtInfosDto : EntityDto
    {
        override public long getEntityId()
        {
            return 0;
        }

        public long km { get; set; }
        public DateTime? datum { get; set; }
        public String source { get; set; }
    }
}
