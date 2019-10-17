using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAngebotService.copyAngebot"/> Methode
    /// </summary>
    public class icopyAngebotDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierendes Angebot
        /// </summary>
        public DTO.AngebotDto angebot
        {
            get;
            set;
        }
    }
}
