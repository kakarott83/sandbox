using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für copyAngebot Methode
    /// </summary>
    public class icopyAngebotDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierendes Angebot
        /// </summary>
        public DTO.AngebotDto angebot
        {
            get;
            set;
        }
    }
}
