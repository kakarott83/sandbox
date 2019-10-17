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
    public class ocalcBSIDto : oBaseDto
    {
        /// <summary>
        /// bsi package price
        /// </summary>
        public double price { get; set; }
    }
}