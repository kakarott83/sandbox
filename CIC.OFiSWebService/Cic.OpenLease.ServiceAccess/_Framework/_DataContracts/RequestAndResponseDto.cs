using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.ServiceAccess.Merge.DTO
{
    /// <summary>
    /// Request und Response Dto
    /// </summary>
    public class RequestAndResponseDto
    {
        /// <summary>
        /// MessageID
        /// </summary>
        public string messageID { get; set; }

        /// <summary>
        /// Methode
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// Request
        /// </summary>
        public RequestDto request { get; set; }

        /// <summary>
        /// Response
        /// </summary>
        public RequestDto response { get; set; }
    }
}