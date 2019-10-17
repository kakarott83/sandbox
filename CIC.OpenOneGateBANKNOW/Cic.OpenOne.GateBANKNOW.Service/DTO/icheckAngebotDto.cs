using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter
    /// </summary>
    public class icheckAngebotDto
    {
        /// <summary>
        /// Persistenzobjekt Kalkulation
        /// </summary>
        public AngAntKalkDto kalkulation
        {
            get;
            set;
        }

        /// <summary>
        /// Produktkontext zur Zinsermittlung
        /// </summary>
        public prKontextDto prodKontext { get; set; }
    }
}
