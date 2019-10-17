using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class SystemDto : EntityDto
    {
        public long syssystem { get; set; }

        override public long getEntityId()
        {
            return syssystem;
        }
    }
}
