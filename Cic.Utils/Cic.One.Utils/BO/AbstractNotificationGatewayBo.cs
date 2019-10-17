using System;
using System.Collections.Generic;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    using One.Utils.DAO;

    /// <summary>
    /// Abstract class for the Implementation of the Messaging Interface
    /// </summary>
    public abstract class AbstractNotificationGatewayBo : INotificationGatewayBo
    {
        /// <summary>
        /// The data access object for DB access
        /// </summary>
        protected INotificationGatewayDbDao notificationGatewayDbDao;
        /// <summary>
        /// The data access object for SMTP output
        /// </summary>
        protected INotificationGatewaySmtpDao notificationGatewaySmtpDao;

        /// <summary>
        /// The data access object for SMS output
        /// </summary>
        protected ISmsDao smsDao;

        /// <summary>
        /// Constructs a abstract Notification Gateway business object
        /// </summary>
        /// <param name="notificationGatewayDbDao">data access object for DB access</param>
        /// <param name="notificationGatewaySmtpDao">Data access object for SMTP output</param>
        /// <param name="smsDao">Data access object for SMS output</param>
        public AbstractNotificationGatewayBo(INotificationGatewayDbDao notificationGatewayDbDao, INotificationGatewaySmtpDao notificationGatewaySmtpDao, ISmsDao smsDao)
        {
            this.notificationGatewayDbDao = notificationGatewayDbDao;
            this.notificationGatewaySmtpDao = notificationGatewaySmtpDao;
            this.smsDao = smsDao;
        }

        /// <summary>
        /// Set Configuration Settings for the Notification Gateway
        /// In case a key is not found it is logged.
        /// </summary>
        /// <param name="SettingList"></param>
        public abstract void SetConfig(Dictionary<NotifySettings, String> SettingList);

        /// <summary>
        /// Sending E-Mail to Recipient via MessagingInterface
        /// </summary>
        /// <param name="To">Recipient of the mail.</param>
        /// <param name="From">Sender of the mail.</param>
        /// <param name="Subject">Subject of the mail.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public abstract int sendMail(String To, String From, String Subject, Byte[] Body, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending E-Mail to Recipient via MessagingInterface/Dto Variant
        /// </summary>
        /// <param name="inotDto">Input Dto</param>
        /// <param name="ServerData">ServerData</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public abstract int sendMail(inotificationMailDto inotDto, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending a Fax to Recipient via MessagingInterface
        /// </summary>
        /// <param name="To">Recipient of the fax.</param>
        /// <param name="From">Sender of the fax.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public abstract int sendFax(String To, String From, Byte[] Body, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending Fax to Recipient via MessagingInterface/Dto Variant
        /// </summary>
        /// <param name="inotDto">Input Dto</param>
        /// <param name="ServerData">ServerData</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public abstract int sendFax(inotificationFaxDto inotDto, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending a SMS to Recipient via MessagingInterface
        /// </summary>
        /// <param name="To">Recipient of the fax.</param>
        /// <param name="From">Sender of the fax.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public abstract int sendSms(String To, String From, String Body, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Sending SMS to Recipient via MessagingInterface/Dto Variant
        /// </summary>
        /// <param name="inotDto">Input Dto</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public abstract int sendSms(inotificationSmsDto inotDto, iNotificationGatewayServerDto ServerData);

        /// <summary>
        /// Senden einer Notification via EAI DB-Datenschema
        /// Dies kann eine E-Mail, SMS oder ein Fax sein abhänging von den DB Einstellungen
        ///</summary>
        /// <param name="NotificationID">Primary Key auf der EAIHOT-Tabelle</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public abstract int sendEAINotification(int NotificationID, iNotificationGatewayServerDto ServerData);
    }
}