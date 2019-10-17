using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class Angobsl
    {
        /// <summary>
        /// sysVT/ANGOBSL
        /// </summary>
        public long sysVT { get; set; }

        /// <summary>
        /// Rang /ANGOBSL
        /// </summary>
        public int rang { get; set; }

        /// <summary>
        /// Bezeichnung/ANGOBSL
        /// </summary>
        public string bezeichnung { get; set; }

        /// <summary>
        /// Faellig/ANGOBSL
        /// </summary>
        public DateTime faellig { get; set; }

    }
}