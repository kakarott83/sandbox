using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Ausgabe DTO Branchen
    /// </summary>
    public class olistBranchenDto : oBaseDto
    {
        /// <summary>
        /// Liste mit Branchen
        /// </summary>
        public DropListDto[] branchen
        {
            get;
            set;
        }
    }
}