using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.One.DTO
{
    /// <summary>
    /// Output for bsi calculation
    /// </summary>
    public class ocalcVSDto : oBaseDto
    {
        /// <summary>
        /// insurance praemie
        /// </summary>
        public double praemie { get; set; }
        /// <summary>
        /// insurance tax
        /// </summary>
        public double steuer { get; set; }
    }
}