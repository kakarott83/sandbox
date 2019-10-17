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
    public class igetExpdefDto
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
        /// ids of the area
        /// </summary>
        public long[] areaids
        {
            get;
            set;
        }
    }
}
