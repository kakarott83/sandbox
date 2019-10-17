using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Rollentyp
    /// </summary>
    public class ocreateOrUpdateRollentypDto : oBaseDto
    {
        public RollentypDto rollentyp { get; set; }
    }
}

