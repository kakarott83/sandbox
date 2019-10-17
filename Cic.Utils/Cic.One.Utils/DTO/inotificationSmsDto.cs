using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Notification SMS Eingangs DTO
    /// </summary>
    public class inotificationSmsDto
    {
        /// <summary>
        /// Empfänger
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Absender
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Mailbody
        /// </summary>
        public string  Body { get; set; }
    }
}
