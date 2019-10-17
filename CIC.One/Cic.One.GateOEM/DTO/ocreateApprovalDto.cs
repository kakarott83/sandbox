using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    /// <summary>
    /// OutputParameter für createAdresse Methode
    /// </summary>
    public class ocreateApprovalDto : oBaseDto
    {
        /// <summary>
        /// Customer
        /// </summary>
        public CustomerDto customer
        {
            get;
            set;
        }
        /// <summary>
        /// Guarantor
        /// </summary>
        public CustomerDto guarantor
        {
            get;
            set;
        }

        /// <summary>
        /// Object
        /// </summary>
        public ObjectDto obj
        {
            get;
            set;
        }
        /// <summary>
        /// technical Key of Approval
        /// </summary>
        public long sysid { get;set;}
        /// <summary>
        /// Front-Office Deeplink
        /// </summary>
        public String deepLink { get; set; }
        /// <summary>
        /// Status of the Approval
        /// </summary>
        public String status { get; set; }
        /// <summary>
        /// Status number of the Approval
        /// </summary>
        public long statusCode { get; set; }
    }
}
