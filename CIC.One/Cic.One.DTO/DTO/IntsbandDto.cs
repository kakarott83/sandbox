using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class IntsbandDto
    {
        /// <summary>
        /// Betragstufe
        /// </summary>
        public double lowerb { get; set; }

        /// <summary>
        /// Zinsabschlag
        /// </summary>
        public double intrate { get; set; }

        /// <summary>
        /// Reduzierter Zinssatz
        /// </summary>
        public double redrate { get; set; }
    }
}
