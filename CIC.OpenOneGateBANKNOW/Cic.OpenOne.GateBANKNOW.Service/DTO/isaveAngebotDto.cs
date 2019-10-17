using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.saveAngebot"/> Methode
    /// </summary>
    public class isaveAngebotDto
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
