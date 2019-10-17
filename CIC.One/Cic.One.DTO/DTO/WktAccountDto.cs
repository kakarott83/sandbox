using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    /// <summary>
    /// Combines it and person for WKT
    /// if flagkd = 1 its a person, else an it
    /// </summary>
    public class WktaccountDto : AccountDto
    {

        public long sysit { get; set; }
        public long wktid { get; set; }
        override public long getEntityId()
        {
            return wktid;
        }
        public int monatsrechnung { get; set; }
        public int dauerrechnung { get; set; }
        public int monatsrechnungart { get; set; }
        public int fparkgroesse { get; set; }




    }
}
