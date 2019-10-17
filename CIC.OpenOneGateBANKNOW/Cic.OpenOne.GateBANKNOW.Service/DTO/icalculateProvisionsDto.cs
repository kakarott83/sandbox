using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.calculateProvisions"/> Methode
    /// used for calculating Provisions for Expected Loss Calculations
    /// </summary>
    public class icalculateProvisionsDto
    {
       
        /// <summary>
        /// Produktkontext zur Zinsermittlung
        /// </summary>
        public prKontextDto prodKontext { get; set; }

        /// <summary>
        /// Customer Score for RAP determination
        /// </summary>
        public String kundenScore { get; set; }

        /// <summary>
        /// Financing amount
        /// </summary>
        public double finanzierungsbetrag { get; set; }

        /// <summary>
        /// Income by interest
        /// </summary>
        public double zinsertrag { get; set; }
 

    }
}
