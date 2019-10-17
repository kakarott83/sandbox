using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Checklist art
    /// </summary>
    public class ocreateOrUpdatePrunartDto : oBaseDto
    {
        public PrunartDto prunart { get; set; }
    }
}