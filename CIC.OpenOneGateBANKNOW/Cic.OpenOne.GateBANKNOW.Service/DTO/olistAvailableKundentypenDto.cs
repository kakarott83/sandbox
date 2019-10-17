using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.listAvailableKundentypen"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.listAvailableKundentypen"/> Methode
    /// </summary>
    public class olistAvailableKundentypenDto : oBaseDto
    {
        /// <summary>
        /// Array von Kundentypen
        /// </summary>
        public DropListDto[] kundentypen
        {
            get;
            set;
        }
    }
}
