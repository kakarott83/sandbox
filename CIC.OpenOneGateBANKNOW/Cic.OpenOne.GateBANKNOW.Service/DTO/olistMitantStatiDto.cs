using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateKundeService.listWohnSituationen"/> Methode
    /// </summary>
    public class olistMitantStatiDto : oBaseDto
    {
        /// <summary>
        /// Array von Wohn-Situationen
        /// </summary>
        public DropListDto[] mitantragstellerStati
        {
            get;
            set;
        }
    }
}
