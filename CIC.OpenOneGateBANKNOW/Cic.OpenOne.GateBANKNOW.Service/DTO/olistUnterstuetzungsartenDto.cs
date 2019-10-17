using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Ausgabe DTO mit Unterstüzungsarten
    /// </summary>
    public class olistUnterstuetzungsartenDto : oBaseDto
    {
        /// <summary>
        /// Liste mit Unterstützungsarten
        /// </summary>
        public DropListDto[] unterstuetzungsarten
        {
            get;
            set;
        }
    }
}