using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class SlDto : EntityDto 
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public long getEntityId()
        {
            return syssl;
        }

        
        public long syssllink { get; set; }
        public long syssltyp { get; set; }

        public long sysslpos { get; set; }
        public long sysslpostyp { get; set; }
        public DateTime? valuta { get; set; }
        public int gezogen { get; set; }
        public double betrag { get; set; }
        public double zinssaldo { get; set; }
        public int zinstage { get; set; }
        public double zinsen { get; set; }
        public double zinsenp { get; set; }
        public double zinsenab { get; set; }
        public double tilgsaldo { get; set; }
        public int tilgtage { get; set; }
        public double tilgung { get; set; }
        public double tilgungp { get; set; }
        public double restforderung { get; set; }
        public long syssl { get; set; }
        public long sysrn { get; set; }
        public int inaktiv { get; set; }
        public DateTime? inaktivab { get; set; }

    }
}
