using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Dto for Zins
    /// </summary>
    public class IntsDto
    {
        /// <summary>
        /// Interest Rate Date
        /// </summary>
        public long sysintsdate
        {
            get;
            set;
        }

        /// <summary>
        /// Maturity
        /// </summary>
        public long maturity
        {
            get;
            set;
        }

        /// <summary>
        /// Upper Base
        /// </summary>
        public double upperb
        {
            get;
            set;
        }

        /// <summary>
        /// Lower Base
        /// </summary>
        public double lowerb
        {
            get;
            set;
        }

        /// <summary>
        /// Interest Rate
        /// </summary>
        public double intrate
        {
            get;
            set;
        }

        /// <summary>
        /// Add Rate
        /// </summary>
        public double addrate
        {
            get;
            set;
        }

        /// <summary>
        /// Reduced Rate
        /// </summary>
        public double redrate
        {
            get;
            set;
        }

        /// <summary>
        /// minimum rate
        /// </summary>
        public double minrate
        {
            get;
            set;
        }

        /// <summary>
        /// maximum rate
        /// </summary>
        public double maxrate
        {
            get;
            set;
        }

    }
}
