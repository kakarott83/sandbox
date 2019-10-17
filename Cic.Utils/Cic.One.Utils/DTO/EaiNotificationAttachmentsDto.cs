using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Attachment structure DTO
    /// </summary>
    public class EaiNotificationAttachmentsDto
    {
        /// <summary>
        /// Name to be set on Mail
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Name to be set on Mail
        /// </summary>
        public string MIME { get; set; }
        
        /// <summary>
        /// Binary Data (optional)
        /// </summary>
        public Byte[] Data { get; set; }

    }
}
