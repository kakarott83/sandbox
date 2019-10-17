using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.ServiceAccess.Merge.DTO
{
    /// <summary>
    /// Request Dto
    /// </summary>
    public class RequestDto
    {
        /// <summary>
        /// Timestamp vom Request
        /// </summary>
        public DateTime datumsstempel { get; set; }

        /// <summary>
        /// Liefert einen True wenn es ein Request ist sonst ein False wenn es ein Response ist
        /// </summary>
        public bool isRequest { get; set; }

        /// <summary>
        /// String Header
        /// todo: Korrekt ausarbeiten
        /// </summary>
        public string header { get; set; }

        /// <summary>
        /// String body
        /// todo: Korrekt ausarbeiten
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// Kompletter Request im string Format
        /// </summary>
        public string wholeXMLRequest { get; set; }
    }
}