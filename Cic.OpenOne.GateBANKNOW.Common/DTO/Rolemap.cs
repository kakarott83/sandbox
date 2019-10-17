using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Parameterklasse für Rolemap
    /// </summary>
    public class RoleMap
    {
        /// <summary>
        /// Person Role ID
        /// </summary>
        public long sysPerole { get; set; }
        /// <summary>
        /// Default Flag
        /// </summary>
        public bool isDefault { get; set; }
        /// <summary>
        /// Rolename
        /// </summary>
        public String roleName { get; set; }
        /// <summary>
        /// Is User Role active
        /// </summary>
        public bool inactive { get; set; }
    }
}
