using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAngebotService.deleteAngebot"/>deleteAngebot Methode
    /// </summary>
    public class ideleteAngebotDto
    {
        /// <summary>
        /// ID des Angebots
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
