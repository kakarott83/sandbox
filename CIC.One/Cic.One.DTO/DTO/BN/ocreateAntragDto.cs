using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// returnvalue from offer creation, contains Deeplink into an offer from Service-Method createAntrag
    /// </summary>
    public class ocreateAntragDto : oBaseDto
    {
        public String deeplink { get; set; }
    }
}