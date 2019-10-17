using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für setGeschaeftsart Methode
    /// </summary>
    public class isetGeschaeftsartDto
    {
        /// <summary>
        /// Persistenzobjekt Angebot
        /// </summary>
        public DTO.AngebotDto angebot
        {
            set;
            get;
        }
    }
}
