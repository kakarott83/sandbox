using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.listAvailableNutzungsarten"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.listAvailableNutzungsarten"/> Methode
    /// </summary>
    public class olistAvailableNutzungsartenDto : oBaseDto
    {
        /// <summary>
        /// Array von Nutzungsarten
        /// </summary>
        public DropListDto[] nutzungsarten
        {
            get;
            set;
        }
    }
}
