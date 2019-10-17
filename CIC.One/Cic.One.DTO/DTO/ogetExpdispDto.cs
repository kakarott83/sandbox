using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Details values for one area of indicators
    /// </summary>
    public class ogetExpdispDto : oBaseDto
    {
        public List<ExpdispDto> expvalues { get; set; }
    }
}