using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created It
    /// </summary>
    public class ocreateOrUpdateItDto : oBaseDto
    {
        public ItDto It { get; set; }
    }
}

