using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input for fetching all data about tires
    /// </summary>
    public class ogetTiresDto : oBaseDto
    {
        public TireInfoDto result { get; set; }
        
    }
}