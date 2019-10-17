using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchAngebotService.searchAngebot"/>searchAngebot Methode
    /// </summary>
    public class osearchAngebotDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Angebot
        /// </summary>
        public oSearchDto<AngebotDto> result
        {
            get;
            set;
        }
    }
}
