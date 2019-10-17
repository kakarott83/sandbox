using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateKundeService.saveKunde"/> Methode
    /// </summary>
    public class isaveKundeDto
    {
        /// <summary>
        /// Persistenzobjekt Kunde
        /// </summary>
        public KundeDto kunde
        {
            get;
            set;
        }
    }
}
