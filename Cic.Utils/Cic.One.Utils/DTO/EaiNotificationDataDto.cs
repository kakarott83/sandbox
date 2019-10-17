using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.BO;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Notification Data DTO
    /// </summary>
    public class EaiNotificationDataDto
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int SYSEAIHOT;
        /// <summary>
        /// Mail, Fax oder SMS-Kennung
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Betreff
        /// </summary>
        public String Subject { get; set; }

        /// <summary>
        /// Empfängerliste (TO)
        /// </summary>
        public List<String> Recipients { get; set; }

        /// <summary>
        /// Empfängerliste (CC)
        /// </summary>
        public List<String> CarbonCopyRecipients { get; set; }

        /// <summary>
        /// Empfängerliste (BCC)
        /// </summary>
        public List<String> BlindCarbonCopyRecipients { get; set; }

        /// <summary>
        /// Absender (FROM)
        /// </summary>
        public String Sender { get; set; }

        /// <summary>
        /// Textpart 1
        /// Primär für SMS. Kann aber auch für kurze Mails genutzt werden.
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// List of attachments
        /// </summary>
        public List<EaiNotificationAttachmentsDto> Attachments { get; set; }

        /// <summary>
        /// X-Tag Liste
        /// </summary>
        public Dictionary<string, string> XTags { get; set; }

        /// <summary>
        /// Variables of the Notification (for the Report-Creation)
        /// </summary>
        public Dictionary<string, string> Variables { get; set; }
    }
}
