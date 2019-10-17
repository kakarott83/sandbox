using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Parameterklasse für ParamDto
    /// </summary>
    public class IntstrctDto
    {
        /// <summary>
        /// Interest Rate Strict
        /// </summary>
        public long sysintstrct
        {
            get;
            set;
        }

        /// <summary>
        /// PR Product
        /// </summary>
        public long sysprproduct
        {
            get;
            set;
        }

        /// <summary>
        /// Interest Rate Date
        /// </summary>
        public long sysintsdate
        {
            get;
            set;
        }

        /// <summary>
        /// Method
        /// </summary>
        public long method
        {
            get;
            set;
        }

        /// <summary>
        /// Valid From
        /// </summary>
        public DateTime validFrom
        {
            get;
            set;
        }

    }
}
