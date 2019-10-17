﻿using System;
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
    public class osolveSchnellkalkulationDto : oBaseDto
    {
        /// <summary>
        /// Ergebnisse mehrerer Kalkulationen
        /// </summary>
        public List<osolveKalkulationDto> kalkulationen { get; set; }
       
    }
}
