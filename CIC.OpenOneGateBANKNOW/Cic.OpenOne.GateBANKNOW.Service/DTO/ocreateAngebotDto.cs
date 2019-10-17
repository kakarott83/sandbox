using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.createOrUpdateAngebot"/> Methode
    /// </summary>
    public class ocreateAngebotDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Angebot
        /// </summary>
        public DTO.AngebotDto angebot
        {
            get;
            set;
        }
    }
}
