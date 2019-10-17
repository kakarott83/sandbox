using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Extension;

namespace Cic.OpenOne.Common.DAO
{
    using System.Net;

    /// <summary>
    /// Data access object for SMTP output
    /// </summary>
    public class NotificationGatewaySmtpDao : INotificationGatewaySmtpDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Send Mail via SMTP to Server
        /// </summary>
        /// <param name="Data">Data output with Data to send</param>
        /// <returns>Success = true, Failure = false</returns>
        public bool sendNachricht(oNotifyGatewaySmtpSendDto Data)
        {
            try
            {
                _log.Info("Starting SMTP Transfer.");
                SmtpClient MailClient = new SmtpClient(Data.Server, Data.Port);
                // Debug Only
                //oNotifyGatewaySmtpStoreDto SData = new oNotifyGatewaySmtpStoreDto();
                //SData.Message = Data.Message;
                //SData.Pfad = @"C:\test\Temp.eml";
                //storeNachricht(SData);
                // Debug Only Ende

                if (Data.ServerData != null && Data.ServerData.SMTPSendAsEnabled)
                {
                    MailClient.EnableSsl = true;
                    MailClient.UseDefaultCredentials = false;
                    MailClient.Credentials = new NetworkCredential(Data.ServerData.SMTPServiceAccount, Data.ServerData.SMTPServiceAccountPassword);
                }
                
                DateTime StartSending = DateTime.Now;
                MailClient.Send(Data.Message);
                DateTime EndSending = DateTime.Now;
                TimeSpan Difference = new TimeSpan();
                Difference = (EndSending - StartSending);
                _log.Info("SMTP Transfer complete. Time needed: " + Difference.ToString() );
                return true;
            }
            catch (Exception Exp)
            {
                _log.Error("Send Message to SMTP Server failed! Exception: " + Exp);
                return false;
            }
        }

        /// <summary>
        /// Store Mail on Filesystem
        /// </summary>
        /// <param name="Data">Dataoutput to store, including file name</param>
        /// <returns>Success = true, Failure = false</returns>
        public bool storeNachricht(oNotifyGatewaySmtpStoreDto Data)
        {
            try
            {
                _log.Info("Starting storing of MailMessage to file " + Data.Pfad);
                Data.Message.Save(Data.Pfad);
                _log.Info("Storing complete.");
                return true;
            }
            catch (Exception Exp)
            {
                _log.Error("Store Message on Filesystem failed! Exception: " + Exp);
                return false;
            }
        }
    }
}
