using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für getAngebotDetail Methode
    /// </summary>
    public class igetAngebotDetailDto
    {
        /// <summary>
        /// Persistenzobjekt Angebot
        /// </summary>
        public DTO.AngebotDto angebot
        {
            get;
            set;
        }
    }
}
