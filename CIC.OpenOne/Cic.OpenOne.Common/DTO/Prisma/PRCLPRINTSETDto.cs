using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Dto for PRCLPRINTSET
    /// </summary>
    public class PRCLPRINTSETDto
    {
        /// <summary>
        /// Product clearance Print Set
        /// </summary>
        public long sysprclprintset
        {
            get;
            set;
        }

        /// <summary>
        /// Product In Test
        /// </summary>
        public long sysprintest
        {
            get;
            set;
        }

        /// <summary>
        /// Product
        /// </summary>
        public long sysprproduct
        {
            get;
            set;
        }

        /// <summary>
        /// Rank
        /// </summary>
        public long rank
        {
            get;
            set;
        }


    }
}
