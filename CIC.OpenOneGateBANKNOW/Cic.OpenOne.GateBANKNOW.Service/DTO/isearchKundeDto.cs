using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchKundeService.searchKunde"/> Methode
    /// </summary>
    public class isearchKundeDto
    {
        /// <summary>
        /// Allgemeines Suchobjekt
        /// </summary>
        public iSearchDto searchInput
        {
            get;
            set;
        }
    }
}
