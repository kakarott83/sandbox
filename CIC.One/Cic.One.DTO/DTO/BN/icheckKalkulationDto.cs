using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// Input for validating checking a kalkulation
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
    /// <summary>
    /// AngAntObSmallDto
    /// </summary>
    public class AngAntObSmallDto
    {
        /// <summary>
        /// Erstzulassung
        /// </summary>
        public DateTime? erstzulassung { get; set; }

        /// <summary>
        /// Übernahmekilometer
        /// </summary>
        public long ubnahmeKm { get; set; }
    }
}
