using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Ausgabe DTO Auslagenarten
    /// </summary>
    public class olistAuslagenartenDto : oBaseDto
    {
        /// <summary>
        /// Liste mit Ausgabenarten
        /// </summary>
        public DropListDto[] auslagenarten
        {
            get;
            set;
        }
    }
}