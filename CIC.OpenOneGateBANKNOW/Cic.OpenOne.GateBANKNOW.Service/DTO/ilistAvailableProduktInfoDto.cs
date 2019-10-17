﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    
    public class ilistAvailableProduktInfoDto
    {
        /// <summary>
        /// Produktkontext
        /// </summary>
        public prKontextDto kontext
        {
            get;
            set;
        }
    }
}
