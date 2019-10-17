using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Notification Eingangs DTO
    /// </summary>
    public class inotificationDto
    {
        /// <summary>
        /// Empfänger
        /// </summary>
        public string to { get; set; }
        /// <summary>
        /// Absender
        /// </summary>
        public string from { get; set; }
        /// <summary>
        /// Betreff
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// Mail/Fax/SMS-Daten
        /// </summary>
        public Byte[] body { get; set; }
    }
}
