using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.processAngebotToAntrag"/> Methode
    /// </summary>
    public class oprocessAngebotToAntragByIdDto : oBaseDto
    {
        /// <summary>
        /// Antrags id
        /// </summary>
        public long sysid
        {
            get;
            set;
        }
    }
}
