using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Kampagnen 
    /// </summary>
    public class ocreateOrUpdateCampDto : oBaseDto
    {
        public CampDto camp { get; set; }
    }
}
