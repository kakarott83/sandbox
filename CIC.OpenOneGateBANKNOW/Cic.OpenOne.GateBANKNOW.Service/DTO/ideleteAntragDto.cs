using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAntragService.deleteAntrag"/> Methode
    /// </summary>
    public class ideleteAntragDto
    {
        /// <summary>
        /// ID des Antrags
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
