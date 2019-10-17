using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Wfsignature
    /// </summary>
    public class ocreateOrUpdateWfsignatureDto : oBaseDto
    {
        public WfsignatureDto wfsignature { get; set; }
    }
}
