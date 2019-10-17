using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AntkalkDto : KalkbaseDto 
    {
        public long? sysantrag { get; set; }
        public long? sysob { get; set; }
    }
}
