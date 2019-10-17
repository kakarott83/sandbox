using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchKundeService.getKundeDetail"/> Methode
    /// </summary>
    public class ogetKundeDetailDto : oBaseDto
    {
        /// <summary>
        /// Liste Persistenzobjekte Adressen zu Kunde
        /// </summary>
        public DTO.KundeDto kunde
        {
            get;
            set;
        }

    }
}
