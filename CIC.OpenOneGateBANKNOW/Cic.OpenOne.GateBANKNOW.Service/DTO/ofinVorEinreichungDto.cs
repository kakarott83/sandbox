using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für Finanzierungsvorschlag Einreichung Methode
    /// </summary>
    public class ofinVorEinreichungDto : oBaseDto
    {


        /// <summary>
        /// Frontid des eai results
        /// </summary>
        public string frontid { get; set; }
    }
}