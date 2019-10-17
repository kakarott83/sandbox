using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateKundeService.listAnreden"/> Methode
    /// </summary>
    public class olistAusweisartenDto : oBaseDto
    {
        /// <summary>
        /// Array von Anreden
        /// </summary>
        public DropListDto[] ausweisarten
        {
            get;
            set;
        }
    }
}
