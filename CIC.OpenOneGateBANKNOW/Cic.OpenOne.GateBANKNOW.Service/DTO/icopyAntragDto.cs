using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAntragService.copyAntrag"/> Methode
    /// </summary>
    public class icopyAntragDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierender Antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }
    }
}
