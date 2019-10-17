using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für saveKunde Methode
    /// </summary>
    public class isaveKundeDto
    {
        /// <summary>
        /// Persistenzobjekt Kunde
        /// </summary>
        public DTO.KundeDto kunde
        {
            get;
            set;
        }
    }
}
