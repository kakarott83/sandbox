using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Rückgabedaten aus CarConfigurator
    /// </summary>
    public class oGetObjektDatenDto : oBaseDto
    {
        /// <summary>
        /// Objektdaten
        /// </summary>
        public AngAntObDto objekt { get; set; }
    }
}