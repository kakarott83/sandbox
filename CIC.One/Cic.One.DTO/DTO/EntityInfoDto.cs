using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class EntityInfoDto : EntityDto
    {
        public String area { get; set; }
        public long sysid { get; set; }

        public override long getEntityId()
        {
            return sysid;
        }
    }
}