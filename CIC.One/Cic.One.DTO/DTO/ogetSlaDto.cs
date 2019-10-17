using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Details for one Ang/Ant
    /// </summary>
    public class ogetSlaDto : oBaseDto
    {
        public List<SlaDto> sladetails { get; set; }
    }
}