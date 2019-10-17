﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAngebotService.copyAngebot"/> Methode
    /// </summary>
    public class ocopyAngebotDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Angebot
        /// </summary>
        public AngebotDto angebot
        {
            get;
            set;
        }
    }
}
