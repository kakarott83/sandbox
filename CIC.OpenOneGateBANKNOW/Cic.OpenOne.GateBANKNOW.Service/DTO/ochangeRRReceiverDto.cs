﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.changeRRReceiver"/> Methode
    /// </summary>
    public class ochangeRRReceiverDto : oBaseDto
    {
        /// <summary>
        /// Frontid des eai results
        /// </summary>
        public string frontid { get; set; }

    }
}
