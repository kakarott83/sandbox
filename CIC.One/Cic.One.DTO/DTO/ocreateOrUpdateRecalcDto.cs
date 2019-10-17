using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Objekt
    /// </summary>
    public class ocreateOrUpdateRecalcDto : oBaseDto
    {
        public RecalcDto recalc { get; set; }
    }
}