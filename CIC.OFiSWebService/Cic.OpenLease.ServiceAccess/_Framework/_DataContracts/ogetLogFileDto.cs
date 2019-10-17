using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Cic.OpenLease.ServiceAccess.Merge.DTO
{
    /// <summary>
    /// Output Dto von getLogFile
    /// </summary>
    public class ogetLogFileDto 
    {
        /// <summary>
        /// Integer mit der PID
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// Request und Response DTO
        /// </summary>
        public RequestAndResponseDto[] requests { get; set; }
    }
}