using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input for calculating bsi package price
    /// </summary>
    public class icalcBSIDto
    {
        /// <summary>
        /// Vehicle code
        /// </summary>
        public long sysobtyp { get; set; }
        /// <summary>
        /// package code
        /// </summary>
        public long sysfstyp { get; set; }
    }
}