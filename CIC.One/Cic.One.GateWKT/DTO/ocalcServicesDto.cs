using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.One.DTO
{
    /// <summary>
    /// Output for calculation of all service prices
    /// </summary>
    public class ocalcServicesDto : oBaseDto
    {
        public double fuelPrice { get; set; }
        public double maintenancePrice { get; set; }
        public double maintenancePriceDef { get; set; }
    }
}