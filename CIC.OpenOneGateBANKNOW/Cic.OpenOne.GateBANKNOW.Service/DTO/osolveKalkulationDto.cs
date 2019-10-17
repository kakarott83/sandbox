using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createSchnellkalkulationService.solveKalkulation"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.solveKalkulation"/> Methode
    /// </summary>
    public class osolveKalkulationDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Kalkulation
        /// </summary>
        public KalkulationDto kalkulation
        {
            get;
            set;
        }

        /// <summary>
        /// Provisions-Zusatzinformationen zur Darstellung am Client
        /// </summary>
        public List<ZusatzinformationDto> zusatzinformationen { get; set; }
    }
}
