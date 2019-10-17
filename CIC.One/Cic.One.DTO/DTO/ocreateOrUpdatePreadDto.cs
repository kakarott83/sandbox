using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created pread flag
    /// </summary>
    public class ocreateOrUpdatePreadDto : oBaseDto
    {
        public PreadDto pread { get; set; }
    }
}