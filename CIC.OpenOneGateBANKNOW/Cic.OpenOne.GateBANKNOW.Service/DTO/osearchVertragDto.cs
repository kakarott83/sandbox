using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchVertragService.searchVertrag"/> Methode
    /// </summary>
    public class osearchVertragDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Vertrag
        /// </summary>
        public oSearchDto<VertragDto> result
        {
            get;
            set;
        }

    }
}
