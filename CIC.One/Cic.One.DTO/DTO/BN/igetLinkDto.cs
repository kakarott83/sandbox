using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Deeplink request information for Service-Method getLink
    /// </summary>
    public class igetLinkDto 
    {
        /// <summary>
        /// OpportunityId als Fremdschlüssel
        /// </summary>
        public String extreferenz { get; set; }
        /// <summary>
        /// External User Id
        /// </summary>
        public String extuserid { get; set; }
        /// <summary>
        /// External Dealer Id
        /// </summary>
        public String extdealerid { get; set; }
    }
}