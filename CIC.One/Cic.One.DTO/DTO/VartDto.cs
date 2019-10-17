using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class VartDto  : EntityDto
    {
        override public long getEntityId()
        {
            return sysVart;
        }

        public long sysVart {get;set;}
        public string Bezeichnung { get; set; }
        public string Code { get; set; }

    }
}