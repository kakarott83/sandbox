using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Describes the csv export format
    /// </summary>
    public class ExportCSVDto
    {
        public String dateFormat { get; set; }
        public String header { get; set; }
        public String fields { get; set; }
        public String separator { get; set; }
        public String encoding { get; set; }
    }
}
