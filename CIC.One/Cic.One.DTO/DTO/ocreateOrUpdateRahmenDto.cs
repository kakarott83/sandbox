using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Rahmen
    /// </summary>
    public class ocreateOrUpdateRahmenDto : oBaseDto
    {
        public RahmenDto rahmen { get; set; }
    }
}