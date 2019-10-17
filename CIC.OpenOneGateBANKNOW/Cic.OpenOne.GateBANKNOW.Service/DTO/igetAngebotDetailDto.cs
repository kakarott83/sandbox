using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchAngebotService.getAngebotDetail"/> Methode
    /// </summary>
    public class igetAngebotDetailDto
    {
        /// <summary>
        /// Angebot Id
        /// </summary>
        public long sysangebot
        {
            get;
            set;
        }
    }
}
