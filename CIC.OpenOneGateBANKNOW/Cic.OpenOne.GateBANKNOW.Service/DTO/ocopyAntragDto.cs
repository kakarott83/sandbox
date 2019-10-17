﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAntragService.copyAntrag"/> Methode
    /// </summary>
    public class ocopyAntragDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }
    }
}
