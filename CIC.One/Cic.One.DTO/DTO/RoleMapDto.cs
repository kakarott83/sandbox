using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Parameterklasse für Rolemap
    /// </summary>
    public class RoleMapDto
    {
        /// <summary>
        /// Person Role ID
        /// </summary>
        public long sysPerole;
        /// <summary>
        /// Default Flag
        /// </summary>
        public int isDefault;
        /// <summary>
        /// Rolename
        /// </summary>
        public String roleName;
        /// <summary>
        /// Is User Role active
        /// </summary>
        public int inactive;
        /// <summary>
        /// Rollentypnummer
        /// </summary>
        public int roletyp;
    }
}