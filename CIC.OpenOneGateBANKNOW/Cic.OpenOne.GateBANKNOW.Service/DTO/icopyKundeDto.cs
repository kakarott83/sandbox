using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für copyKunde Methode
    /// </summary>
    public class icopyKundeDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierender Kunde
        /// </summary>
        public DTO.KundeDto kunde
        {
            get;
            set;
        }
    }
}
