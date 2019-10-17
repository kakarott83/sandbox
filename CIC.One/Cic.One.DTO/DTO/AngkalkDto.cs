using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AngkalkDto : KalkbaseDto 
    {

        public long? sysangebot { get; set; }
     
        public long? sysangvar { get; set; }
    }
}
