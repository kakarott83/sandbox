using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Antkalk
    /// </summary>
    public class ocreateOrUpdateAntkalkDto : oBaseDto
    {
        public AntkalkDto antkalk { get; set; }
    }
}