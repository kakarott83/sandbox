using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Kreditnehmereinheit
    /// </summary>
    public class KneDto
    {
        /// <summary>
        /// PKEY 
        /// </summary>
        public long sysitkne { get; set; }

        /// <summary>
        /// Übergeordnete Einheit
        /// </summary>
        public long sysober { get; set; }

        /// <summary>
        /// untergeordnete Einheit
        /// </summary>
        public long sysunter { get; set; }

        /// <summary>
        /// Area SYSID
        /// </summary>
        public long sysarea { get; set; }

        /// <summary>
        /// When set this KNE is marked for deletion
        /// </summary>
        public int flagdelete { get; set; }

        /// <summary>
        /// CODERELATEKIND
        /// </summary>
        public String coderelatekind { get; set; }
    }
}
