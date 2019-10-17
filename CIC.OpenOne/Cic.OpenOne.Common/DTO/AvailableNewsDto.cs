using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Parameterklasse für AvailableNewsDto
    /// </summary>
    public class AvailableNewsDto
    {
        /// <summary>
        /// ID des Eintrags
        /// </summary>
        public long sysID
        {
            get;
            set;
        }

        /// <summary>
        /// Zeitpunkt der Nachrichtenerstellung
        /// </summary>
        public DateTime datum
        {
            get;
            set;
        }

        /// <summary>
        /// Überschrift der Nachricht
        /// </summary>
        public string header
        {
            get;
            set;
        }

        /// <summary>
        /// Text der Nachricht
        /// </summary>
        public string text
        {
            get;
            set;
        }

        /// <summary>
        /// Anhänge
        /// </summary>
        public List<AttachmentDto> attachments { get; set; }

        /// <summary>
        /// ValidFrom
        /// </summary>
        public DateTime validFrom
        {
            get;
            set;
        }
    }
}
