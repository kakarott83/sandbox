using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Deeplink into an offer from Service-Method getLink
    /// </summary>
    public class ogetLinkDto : oBaseDto
    {
        public String deeplink { get; set; }

    }
}