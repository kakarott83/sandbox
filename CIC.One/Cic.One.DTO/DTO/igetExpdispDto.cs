using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.One.DTO
{
   /// <summary>
   /// Indicator values
   /// </summary>
    public class igetExpdispDto
    {
        /// <summary>
        /// The area, may be null
        /// </summary>
        public String area
        {
            get;
            set;
        }


        /// <summary>
        /// id of the area
        /// </summary>
        public long areaid
        {
            get;
            set;
        }

        /// <summary>
        /// id of the exptyp, optional
        /// </summary>
        public long sysexptyp
        {
            get;
            set;
        }

        /// <summary>
        /// value for the exptyp (used only when sysexptyp was given)
        /// written only if method of exptyp is user
        /// </summary>
        public double userValue { get; set; }

    }
}
