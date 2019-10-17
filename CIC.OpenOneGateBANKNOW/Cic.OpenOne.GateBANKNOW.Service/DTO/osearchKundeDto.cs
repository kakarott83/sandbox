using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System.Xml.Serialization;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchKundeService.searchKunde"/> Methode
    /// </summary>
    public class osearchKundeDto : oBaseDto
    {
        /// <summary>
        /// Liste mit Persistenzobjekten Kunde
        /// </summary>
        public oSearchDto<KundeDto> result
        {
            get;
            set;
        }
    }
}
