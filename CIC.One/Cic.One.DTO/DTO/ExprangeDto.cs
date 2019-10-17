using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Indicator Range
    /// </summary>
    public class ExprangeDto:EntityDto
    {
        public long sysexprange { get; set; }
        public long sysexptyp { get; set; }
        public long rang { get; set; }
        public double minval { get; set; }
        public double maxval { get; set; }
        public String output { get; set; }
        public String description { get; set; }
        public String expression { get; set; }


        public override long getEntityId()
        {
            return sysexprange;
        }
    }
}
