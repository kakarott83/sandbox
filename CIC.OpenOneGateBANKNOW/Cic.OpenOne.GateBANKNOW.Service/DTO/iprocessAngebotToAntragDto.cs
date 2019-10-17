using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.processAngebotToAntrag"/> Methode
    /// </summary>
    public class iprocessAngebotToAntragDto
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
