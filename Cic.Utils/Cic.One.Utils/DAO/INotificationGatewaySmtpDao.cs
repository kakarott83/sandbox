using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
        /// <summary>
        /// Data Access Object for Notification Gateway
        /// </summary>
    public interface INotificationGatewaySmtpDao
    {
        /// <summary>
        /// Send the e-mail to the server to send
        /// </summary>
        /// <returns>Success = true, Failure = false</returns>
        bool sendNachricht(oNotifyGatewaySmtpSendDto Data);

        /// <summary>
        /// Store the E-Mail in a Path for archiving
        /// </summary>
        /// <param name="Data">Store Data</param>
        /// <returns>Success = true, Failure = false</returns>
        bool storeNachricht(oNotifyGatewaySmtpStoreDto Data);
    }
}
