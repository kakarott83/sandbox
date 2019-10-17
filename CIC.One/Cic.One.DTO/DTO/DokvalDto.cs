using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO.Mediator;
namespace Cic.One.DTO
{
    public class DokvalDto : EntityDto
    {

        public long sysid { get;set;}
        public String area { get; set; }

        public QueueDto data { get; set; }
        override public long getEntityId()
        {
            return sysid;
        }

    }
}
