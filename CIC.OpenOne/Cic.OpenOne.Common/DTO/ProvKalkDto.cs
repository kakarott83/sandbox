using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// provision calc step tracing table
    /// </summary>
    public class ProvKalkDto
    {
        public long syspartner { get;set;}
        public long sysprprovtype { get; set; }
        public AngAntProvDto prov { get; set; }
        public long rang { get; set; }
        public String area { get; set; }
        public long syslease { get; set; }
        public double basis { get; set; }
        public double kalkbasis { get; set; }
        public int method { get; set; }
        public double provrate { get; set; }
        public double provval { get; set; }
        public double provision { get; set; }
        public String remark { get; set; }
    }
}
