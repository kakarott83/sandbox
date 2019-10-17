using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Ausgangs DTO für Zusatzeinkommen
    /// </summary>
    public class olistZusatzeinkommenDto : oBaseDto
    {
        /// <summary>
        /// Liste mit Zusatzeinkommen
        /// </summary>
        public DropListDto[] zusatzeinkommen
        {
            get;
            set;
        }
    }
}