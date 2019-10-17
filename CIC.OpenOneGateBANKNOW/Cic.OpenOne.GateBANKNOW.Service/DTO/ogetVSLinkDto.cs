using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.getVSLink(igetVSLinkDto)"/> Methode
    /// </summary>
    public class ogetVSLinkDto : oBaseDto
    {
        /// <summary>
        /// Link into external insurance application
        /// </summary>
        public String deepLink { get; set; }
    }
}
