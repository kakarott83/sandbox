using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Notification Mail Eingangs DTO
    /// </summary>
    public class inotificationMailDto
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
        /// Betreff
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Mail-Body
        /// </summary>
        public Byte[] Body { get; set; }
    }
}
