using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für Cic.OpenOne.GateBANKNOW.ServiceloginPartnerService.getTranslations Methode
    /// </summary>
    public class oTranslationDto : oBaseDto
    {
        /// <summary>
        /// List of all fields to translate
        /// </summary>
        public List<TranslationDto> translations { get; set; }  
    }
}
