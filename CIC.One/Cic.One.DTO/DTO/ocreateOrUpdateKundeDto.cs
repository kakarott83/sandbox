using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Kunde
    /// </summary>
    public class ocreateOrUpdateKundeDto: oBaseDto
    {
        public KundeDto kunde { get; set; }
    }
}
