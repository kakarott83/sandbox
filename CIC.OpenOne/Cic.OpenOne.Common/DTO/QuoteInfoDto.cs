using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Stores Information for every quote with date
    /// </summary>
    public class QuoteInfoDto
    {
        public double prozent { get; set; }
        public long sysquote { get; set; }
        public String bezeichnung { get; set; }
        public DateTime? gueltigab { get; set; }
    }
}
