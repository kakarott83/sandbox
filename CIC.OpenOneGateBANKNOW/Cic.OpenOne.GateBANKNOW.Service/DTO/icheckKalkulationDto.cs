using System.Collections.Generic;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter
    /// </summary>
    public class icheckKalkulationDto
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
        /// Versicherungsinformationen aus der Kalkulation
        /// </summary>
        public AngAntVsDto[] angAntVs { get; set; }

        /// <summary>
        /// Produktkontext zur Zinsermittlung
        /// </summary>
        public prKontextDto prodKontext { get; set; }

        /// <summary>
        /// angAntOb
        /// </summary>
        public AngAntObSmallDto angAntOb { get; set; }

    }
}