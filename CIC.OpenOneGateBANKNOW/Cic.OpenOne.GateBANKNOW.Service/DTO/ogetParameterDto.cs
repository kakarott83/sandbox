using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.getParameter"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createSchnellkalkulationService.getParameter"/> Methode
    /// </summary>
    public class ogetParameterDto : oBaseDto
    {
        /// <summary>
        /// Parametersetobjekt
        /// </summary>
        public ParamDto[] parameters
        {
            get;
            set;
        }
    }
}
