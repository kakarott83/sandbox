using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateKundeService.listKantone"/> Methode
    /// </summary>
    public class olistKantoneDto : oBaseDto
    {
        /// <summary>
        /// Array von Kantonen
        /// </summary>
        public DropListDto[] kantone
        {
            get;
            set;
        }
    }
}
