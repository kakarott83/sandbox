using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    /// <summary>
    /// Report-Ergebnis
    /// select rownum sysreport, a.* from (select to_char(beginn, 'YYYY-MM') X,sum(bgextern) A1,sum(rw) A2, sum (sz) A3 from vt where beginn>='01.01.2014' and beginn<='01.12.2014' group by to_char(beginn, 'YYYY-MM') order by to_char(beginn, 'YYYY-MM')) a;
    /// </summary>
    public class ReportDto : EntityDto
    {
        /// <summary>
        /// Primärschlüssel
        /// </summary>
        public long sysReport { get; set; }

        public String x1 { get; set; }
        public String x2 { get; set; }
        public String x3 { get; set; }
        public String x4 { get; set; }
        public String x5 { get; set; }
        public String x6 { get; set; }
        public String x7 { get; set; }
        public String x8 { get; set; }
        public String x9 { get; set; }
        public String x10 { get; set; }
        public double? y1 { get; set; }
        public double? y2 { get; set; }
        public double? y3 { get; set; }
        public double? y4 { get; set; }
        public double? y5 { get; set; }
        public double? y6 { get; set; }
        public double? y7 { get; set; }
        public double? y8 { get; set; }
        public double? y9 { get; set; }
        public double? y10 { get; set; }
       
        override public long getEntityId()
        {
            return sysReport;
        }
        public override string getEntityBezeichnung()
        {
            return "R" + sysReport;
        }

    }
}