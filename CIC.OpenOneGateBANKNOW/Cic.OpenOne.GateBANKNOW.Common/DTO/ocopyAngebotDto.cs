using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für copyAngebot Methode
    /// </summary>
    public class ocopyAngebotDto
    {
        /// <summary>
        /// Allgemeines Messageobjekt
        /// </summary>
        public DTO.Message message
        {
            get;
            set;
        }

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
