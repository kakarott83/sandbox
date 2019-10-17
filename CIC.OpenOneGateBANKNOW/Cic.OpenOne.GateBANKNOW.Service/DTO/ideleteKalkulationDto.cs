using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.deleteKalkulation"/> Methode
    /// </summary>
    public class ideleteKalkulationDto
    {
        /// <summary>
        /// ID der Kalkulation
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
