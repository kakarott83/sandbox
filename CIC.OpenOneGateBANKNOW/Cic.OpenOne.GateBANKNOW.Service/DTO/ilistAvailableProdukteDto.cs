using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.listAvailableProdukte"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createSchnellkalkulationService.listAvailableProdukte"/> Methode
    /// </summary>
    public class ilistAvailableProdukteDto
    {
        /// <summary>
        /// Produktkontext
        /// </summary>
        public prKontextDto kontext
        {
            get;
            set;
        }
    }
}
