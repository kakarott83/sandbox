using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.createOrUpdateKalkulation"/> Methode
    /// </summary>
    public class ocreateKalkulationDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Kalkulation
        /// </summary>
        public AngAntVarDto kalkulation
        {
            get;
            set;
        }
    }
}
