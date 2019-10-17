using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the complete translation map
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetTranslationsDto : oBaseDto
    {
        public List<Cic.OpenOne.Common.DTO.TranslationDto> result { get; set; }
    }
}