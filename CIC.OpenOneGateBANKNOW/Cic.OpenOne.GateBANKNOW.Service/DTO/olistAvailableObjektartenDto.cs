using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.listAvailableObjektarten"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.listAvailableObjektarten"/> Methode
    /// </summary>
    public class olistAvailableObjektartenDto : oBaseDto
    {
        /// <summary>
        /// Array von Objektarten
        /// </summary>
        public DropListDto[] objektarten
        {
            get;
            set;
        }
    }
}
