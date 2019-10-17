using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Ausgabe DTO Rechtsformen
    /// </summary>
    public class olistRechtsformenDto : oBaseDto
    {
        /// <summary>
        /// Liste mit Rechtsformen
        /// </summary>
        public DropListDto[] rechtsformen
        {
            get;
            set;
        }
    }
}