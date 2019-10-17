using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.Common.DTO
{
    public class iCASEvaluateDto
    {
        public String area { get; set; }
        public String[] expression { get; set; }
        public long[] sysID { get; set; }
        public String[] param { get; set; }

        public String url { get; set; }
        public long execID { get; set; }
    }
    
}