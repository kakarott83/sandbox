using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Indicator value
    /// </summary>
    public class ExpvalDto:EntityDto
    {
        public long sysexpval { get; set; }
        public long sysexptyp { get; set; }
        public String area { get; set; }
        public long sysid { get; set; }
        public double val { get; set; }
        public String output { get; set; }
        public DateTime crtdate { get; set; }
        public long crttime { get; set; }


        public override long getEntityId()
        {
            return sysexpval;
        }
    }
}
