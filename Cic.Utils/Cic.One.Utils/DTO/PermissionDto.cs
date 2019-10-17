using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Hold all possible permission flags
    /// </summary>
    public class PermissionDto
    {
        /// <summary>
        /// Show Permission
        /// </summary>
        public long RSHOW { get; set; }

        /// <summary>
        /// Change
        /// </summary>
        public long RCHANGE { get; set; }

        /// <summary>
        /// Insert
        /// </summary>
        public long RINSERT { get; set; }

        /// <summary>
        /// Delete
        /// </summary>
        public long RDELETE { get; set; }

        /// <summary>
        /// Execution
        /// </summary>
        public long REXECUTE { get; set; }

        /// <summary>
        /// On Start 
        /// </summary>
        public long RONSTART { get; set; }

        /// <summary>
        /// Reserved
        /// </summary>
        public long RRES7 { get; set; }

        /// <summary>
        /// Reserved
        /// </summary>
        public long RRES8 { get; set; }
    }
}
