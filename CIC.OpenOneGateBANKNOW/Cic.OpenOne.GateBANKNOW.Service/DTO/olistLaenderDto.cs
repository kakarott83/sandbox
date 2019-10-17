﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateKundeService.listLaender"/> Methode
    /// </summary>
    public class olistLaenderDto : oBaseDto
    {
        /// <summary>
        /// Array von Ländern
        /// </summary>
        public DropListDto[] laender
        {
            get;
            set;
        }
    }
}
