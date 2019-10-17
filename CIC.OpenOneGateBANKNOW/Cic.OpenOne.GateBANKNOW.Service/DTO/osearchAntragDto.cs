using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchAntragService.searchAntrag"/> Methode
    /// </summary>
    public class osearchAntragDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Antrag
        /// </summary>
        public oSearchDto<AntragDto> result
        {
            get;
            set;
        }
    }
}
