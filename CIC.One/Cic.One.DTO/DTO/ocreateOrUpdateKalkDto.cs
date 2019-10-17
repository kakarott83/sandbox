using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Kalk
    /// </summary>
    public class ocreateOrUpdateKalkDto : oBaseDto
    {
        public KalkDto kalk { get; set; }
    }
}
