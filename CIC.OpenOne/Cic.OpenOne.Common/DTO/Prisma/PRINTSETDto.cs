using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Dto for PRINTSET
    /// </summary>
    public class PRINTSETDto
    {
        /// <summary>
        /// Printset
        /// </summary>
        public long sysprintset
        {
            get;
            set;
        }

        /// <summary>
        /// Valid From
        /// </summary>
        public DateTime validfrom
        {
            get;
            set;
        }

        /// <summary>
        /// Valid Until
        /// </summary>
        public DateTime validuntil
        {
            get;
            set;
        }


    }
}
