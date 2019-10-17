using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.listAvailableObjekttypen"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.listAvailableObjekttypen"/> Methode
    /// </summary>
    public class olistAvailableObjekttypenDto : oBaseDto
    {
        /// <summary>
        /// Array von Objekttypen
        /// </summary>
        public DropListDto[] objekttypen
        {
            get;
            set;
        }
    }
}
