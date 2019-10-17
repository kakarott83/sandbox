using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.copyKalkulation"/>copyKalkulation Methode
    /// </summary>
    public class icopyKalkulationDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierende Kalkulation
        /// </summary>
        public AngAntVarDto angVar
        {
            get;
            set;
        }
    }
}
