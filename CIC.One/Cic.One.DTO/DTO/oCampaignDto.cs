using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returnvalue for Campaign Management Functions
    /// used for returning a Status Message or the correct Error
    /// </summary>
    public class oCampaignDto : oBaseDto
    {
        public String result { get; set; }
    }
}