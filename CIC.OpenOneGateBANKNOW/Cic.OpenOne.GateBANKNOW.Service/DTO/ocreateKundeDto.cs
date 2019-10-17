using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für createKunde Methode
    /// </summary>
    public class ocreateKundeDto : oBaseDto
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
