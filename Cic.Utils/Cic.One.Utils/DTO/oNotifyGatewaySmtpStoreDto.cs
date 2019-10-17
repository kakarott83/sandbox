using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Notification Gateway Smtp Store Data Transfer Object
    /// </summary>
    public class oNotifyGatewaySmtpStoreDto
    {
        /// <summary>
        /// Message to be stored
        /// </summary>
        public MailMessage Message;
        /// <summary>
        /// Path to store Message in
        /// </summary>
        public String Pfad;
    }
}
