using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Provision Shares as of Prisma concept 5.2.2.2.4
    /// </summary>
    public class PROVSHAREDATA
    {
        /// <summary>
        /// unique id of share
        /// </summary>
        public long sysprprovshr { get; set; }

        /// <summary>
        /// sysvprole
        /// </summary>
        public long sysvprole { get; set; }

        /// <summary>
        /// sysvkrole
        /// </summary>
        public long sysvkrole { get; set; }

        /// <summary>
        /// method
        /// </summary>
        public int method { get; set; }

        /// <summary>
        /// shrrate
        /// </summary>
        public long shrrate { get; set; }

        /// <summary>
        /// shrval
        /// </summary>
        public long shrval { get; set; }

        /// <summary>
        /// sysprprovtype
        /// </summary>
        public long sysprprovtype { get; set; }

        /// <summary>
        /// sysprhgroup
        /// </summary>
        public long sysprhgroup { get; set; }

        /// <summary>
        /// validfrom
        /// </summary>
        public DateTime VALIDFROM { get; set; }

        /// <summary>
        /// validuntil
        /// </summary>
        public DateTime VALIDUNTIL { get; set; }

        /// <summary>
        /// peroleparent - parent role of vkperole
        /// </summary>
        public long peroleparent { get; set; }
        
    }
}
