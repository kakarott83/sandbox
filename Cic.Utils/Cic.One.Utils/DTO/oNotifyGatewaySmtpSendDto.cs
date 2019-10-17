using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Notification Gateway Send Data over SMTP Data Transmission Object
    /// </summary>
    public class oNotifyGatewaySmtpSendDto
    {
        /// <summary>
        /// Message Attribute
        /// </summary>
        public MailMessage Message
        {
            get;
            set;
        }

        /// <summary>
        /// Server Attribute
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        /// Port Attribute
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        /// <summary>
        /// Contains server Data
        /// </summary>
        public iNotificationGatewayServerDto ServerData { get; set; }

        /// <summary>
        /// Compare Operator (needed for Unit test to work)
        /// </summary>
        /// <param name="left">left compare object</param>
        /// <param name="right">right compare object</param>
        /// <returns>Objects have the same values = true, else false</returns>
        public static bool operator ==(oNotifyGatewaySmtpSendDto left, oNotifyGatewaySmtpSendDto right)
        {
            if ((object)left == null && null == (object)right)
            {
                return true;
            }

            if ((object)left != null && (object)right != null)
            {
                // Can't be different at the moment
                return true;
            }

            return false;
        }

        /// <summary>
        /// Negated Compare Operator (needed for Unit test to work)
        /// </summary>
        /// <param name="left">left compare object</param>
        /// <param name="right">right compare object</param>
        /// <returns>Objects have the same values = false, else true</returns>
        public static bool operator !=(oNotifyGatewaySmtpSendDto left, oNotifyGatewaySmtpSendDto right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Equals operation (needed for Unit test to work)
        /// </summary>
        /// <param name="obj">Object to compare the current object with</param>
        /// <returns>Objects have the same values = true, else false</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && (this.GetType() == obj.GetType()))
            {
                oNotifyGatewaySmtpSendDto Dto = (oNotifyGatewaySmtpSendDto)obj;
                return (this == Dto);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get Hashcode Operation
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
