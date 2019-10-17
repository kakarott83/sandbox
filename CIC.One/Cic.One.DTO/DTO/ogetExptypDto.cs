using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Details for one area of indicators
    /// </summary>
    public class ogetExptypDto : oBaseDto
    {
        public List<ExptypDto> exptypes { get; set; }
    }
}