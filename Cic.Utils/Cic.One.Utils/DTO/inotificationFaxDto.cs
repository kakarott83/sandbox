using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Notification Fax Eingangs DTO
    /// </summary>
    public class inotificationFaxDto
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
        /// Fax-Daten
        /// </summary>
        public Byte[] Body { get; set; }
    }
}
