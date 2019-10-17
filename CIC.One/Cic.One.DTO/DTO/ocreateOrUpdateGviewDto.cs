using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Gview
    /// </summary>
    public class ocreateOrUpdateGviewDto : oBaseDto
    {
        public GviewDto gview { get; set; }
    }
}