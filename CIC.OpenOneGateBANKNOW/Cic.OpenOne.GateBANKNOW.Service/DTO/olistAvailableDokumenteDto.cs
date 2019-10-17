using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.printAngebotService.listAvailableDokumente"/> Methode
    /// </summary>
    public class olistAvailableDokumenteDto : oBaseDto
    {
        /// <summary>
        /// Allgemeines Dokumentenobjekt
        /// </summary>
        public DokumenteDto[] dokumente
        {
            get;
            set;
        }
    }
}
