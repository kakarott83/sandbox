using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Combined DTO to hold all Data from PROVRATE,PROVTARIF,PROVPLAN
    /// The source is defined by typ
    /// </summary>
    public class PROVSTRCTDATA
    {
        /// <summary>
        /// type of data
        /// 0 == Rate
        /// 1 == Plan
        /// 2 == Tarif
        /// </summary>
        public int typ { get; set; }

        /// <summary>
        /// pkey of the db entity
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// percent of the provision (typ 2,1,0)
        /// </summary>
        public double provrate { get; set; }

        /// <summary>
        /// value of the provision (typ 0)
        /// </summary>
        public double provval { get; set; }

        /// <summary>
        /// name (typ 2)
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// is it a standard tarif (typ 2)
        /// </summary>
        public int standardflag { get; set; }

        /// <summary>
        /// lower boundary for amounts (typ 1)
        /// </summary>
        public double lowerb { get; set; }

        /// <summary>
        /// lower boundary for percent (typ 1)
        /// </summary>
        public double lowerbp { get; set; }
    }
}
