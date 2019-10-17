using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// oVertragsartenDto
    /// </summary>
    public class oVertragsartenDto : oBaseDto
    {
        /// <summary>
        /// vertragsarten
        /// </summary>
        public List<PrBildweltVDto> vertragsarten { get; set; }
    }
}