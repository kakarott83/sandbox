using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Notification Type Enum 
    /// Declares the type of Notification done
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Notification via E-Mail
        /// </summary>
        EMail,
        /// <summary>
        /// Notification via Fax
        /// </summary>
        Fax,
        /// <summary>
        /// Notification via SMS
        /// </summary>
        SMS
    };

    /// <summary>
    /// Enum for Notificationsettings
    /// </summary>
    public enum NotifySettings
    {
        /// <summary>
        /// Regular expression for Telephone validation
        /// </summary>
        TelefonRegEx,
        /// <summary>
        /// Regular expression for E-Mail validation
        /// </summary>
        EMailRegEx,
        /// <summary>
        /// SMS filter for filtering out unwanted characters
        /// </summary>
        SmsFilter,
        /// <summary>
        /// Fax Filter for filtering out unwanted characters
        /// </summary>
        FaxFilter,
        /// <summary>
        /// E-Mail Filter for filtering out unwanted characters
        /// </summary>
        EMailFilter,
        /// <summary>
        /// SMTP Server to talk to
        /// </summary>
        SMTPServer,
        /// <summary>
        /// SMTP Port of Server to use (default 25)
        /// </summary>
        SMTPPort,
        /// <summary>
        /// Fax-Adress postfix for Fax-Sending to SMTP-Server
        /// </summary>
        FaxAdresse,
        /// <summary>
        /// SMS-Adress postfix for SMS-Sending to SMTP Server
        /// </summary>
        SmsAdresse
    }

    /// <summary>
    /// Defines the interfce to send Mails, Fax- and SMS-Messages
    /// </summary>
    public interface INotificationGatewayBo
    {
        /// <summary>
        /// Set Configuration Settings for the Notification Gateway
        /// In case a key is not found it is logged.
        /// </summary>
        /// <param name="SettingList"></param>
        void SetConfig(Dictionary<NotifySettings, String> SettingList);

        /// <summary>
        /// Sending E-Mail to Recipient via MessagingInterface
        /// </summary>
        /// <param name="To">Recipient of the mail.</param>
        /// <param name="From">Sender of the mail.</param>
        /// <param name="Subject">Subject of the mail.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        int sendMail(String To, String From, String Subject, Byte[] Body, iNotificationGatewayServerDto ServerData);
        
        /// <summary>
        /// Sending E-Mail to Recipient via MessagingInterfac
        /// </summary>
        /// <param name="inotDto"></param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns></returns>
        int sendMail(inotificationMailDto inotDto, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending a Fax to Recipient via MessagingInterface
        /// </summary>
        /// <param name="To">Recipient of the fax.</param>
        /// <param name="From">Sender of the fax.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        int sendFax(String To, String From, Byte[] Body, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending a Fax to Recipient via MessagingInterface
        /// </summary>
        /// <param name="inotDto"></param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns></returns>
        int sendFax(inotificationFaxDto inotDto, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending a SMS to Recipient via MessagingInterface
        /// </summary>
        /// <param name="To">Recipient of the fax.</param>
        /// <param name="From">Sender of the fax.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        int sendSms(String To, String From, String Body, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending a SMS to Recipient via MessagingInterface
        /// </summary>
        /// <param name="inotDto"></param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns></returns>
        int sendSms(inotificationSmsDto inotDto, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Senden einer Notification via EAI DB-Datenschema
        /// Dies kann eine E-Mail, SMS oder ein Fax sein abhänging von den DB Einstellungen
        ///</summary>
        /// <param name="NotificationID">Primary Key auf der EAIHOT-Tabelle</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        int sendEAINotification(int NotificationID, iNotificationGatewayServerDto ServerData);
    }
}
