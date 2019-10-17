using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.createOrUpdateKalkulation"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createSchnellkalkulationService.createOrUpdateKalkulation"/>
    /// </summary>
    public class icreateKalkulationDto
    {
        /// <summary>
        /// Eingangs Dto (null für neue Kalkulation)
        /// </summary>
        public AngAntVarDto angVar
        {
            get;
            set;
        }

}
}
