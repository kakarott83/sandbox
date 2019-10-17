using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    /// <summary>
	/// OutputParameter für deepLink
    /// </summary>
    public class ogetInformationDto : oBaseDto
    {
        /// <summary>
        /// Front-Office Deeplink
        /// </summary>
        public String deepLink { get; set; }
	}
}
