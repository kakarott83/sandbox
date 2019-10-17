using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// DDLKPPOSData
    /// </summary>
    public class DDLKPPOSData
    {
        /// <summary>
        /// Tooltip
        /// </summary>
        public String TOOLTIP { get; set; }

        /// <summary>
        /// Translated description
        /// </summary>
        public String ACTUALTERM { get; set; }

        /// <summary>
        /// Original description
        /// </summary>
        public String ORIGTERM { get; set; }

        /// <summary>
        /// CODE of this group
        /// </summary>
        public String CODE { get; set; }

        /// <summary>
        /// DB Key
        /// </summary>
        public long SYSDDLKPPOS { get; set; }

        /// <summary>
        /// Value Code
        /// </summary>
        public String ID { get; set; }
    }
}
