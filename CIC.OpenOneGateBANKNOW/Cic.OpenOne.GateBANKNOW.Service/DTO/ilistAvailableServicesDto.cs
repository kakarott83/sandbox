using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.listAvailableServices"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createSchnellkalkulationService.listAvailableServices"/> Methode
    /// </summary>
    public class ilistAvailableServicesDto
    {
        /// <summary>
        /// Servicekontext
        /// </summary>
        public srvKontextDto kontext
        {
            get;
            set;
        }
    }
}
