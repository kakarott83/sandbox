using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util;
using Cic.One.Utils.DAO;

namespace Cic.OpenOne.Common.BO
{

    /// <summary>
    /// Concrete Implementation of the Notification Gateway Interface
    /// </summary>
    public class NotificationGatewayBo : AbstractNotificationGatewayBo
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary for Notificationsettings
        /// </summary>
        Dictionary<NotifySettings, String> NotificationSettings = new Dictionary<NotifySettings, string>{   {NotifySettings.EMailFilter, @""},
                                                                                                            {NotifySettings.EMailRegEx, @""},
                                                                                                            {NotifySettings.FaxFilter, @""},
                                                                                                            {NotifySettings.SmsFilter, @""},
                                                                                                            {NotifySettings.TelefonRegEx,  @"^[A-Za-z0-9+ ]+$"},
                                                                                                            {NotifySettings.FaxAdresse,  @""},
                                                                                                            {NotifySettings.SmsAdresse,  @""},
                                                                                                            {NotifySettings.SMTPPort,  @""},
                                                                                                            {NotifySettings.SMTPServer,  @""},
                                                                                                        };

        public iNotificationGatewayServerDto ServerData { get; set; }

        /// <summary>
        /// contructs a NotificationGateway business object
        /// </summary>
        /// <param name="notificationGatewayDbDao">DB Data access object</param>
        /// <param name="notificationGatewaySmtpDao">SMTP Data access object</param>
        /// <param name="smsDao">SMS Data access object</param>
        public NotificationGatewayBo(INotificationGatewayDbDao notificationGatewayDbDao, 
            INotificationGatewaySmtpDao notificationGatewaySmtpDao,
            ISmsDao smsDao)
            : base(notificationGatewayDbDao, notificationGatewaySmtpDao, smsDao)
        {
        }

        /// <summary>
        /// Set Configuration Settings for the Notification Gateway
        /// In case a key is not found it is logged.
        /// </summary>
        /// <todo>No final decision if the Settings should come from the web.config file</todo>
        /// <param name="SettingList"></param>
        public override void SetConfig(Dictionary<NotifySettings, String> SettingList)
        {
            foreach (int value in System.Enum.GetValues(typeof(NotifySettings)))
            {
                String Entry = (from setting in SettingList where setting.Key == (NotifySettings)value select setting.Value).FirstOrDefault();
                if (Entry != null && Entry.Length > 0)
                {
                    NotificationSettings[(NotifySettings)value] = Entry;
                }
                SettingList.Remove((NotifySettings)value);
            }
            if (SettingList.Count > 0)
            {
                String LeftoverKeys = "";
                foreach (NotifySettings key in SettingList.Keys)
                {
                    if (LeftoverKeys.Length > 0)
                        LeftoverKeys += ", ";
                    LeftoverKeys += key.ToString();
                }
                _log.Error("The keys '" + LeftoverKeys + "' where invalid!");
            }
        }

        /// <summary>
        /// Send E-Mail Proxy for sendNotification
        /// </summary>
        /// <param name="To">Recipient of the mail.</param>
        /// <param name="From">Sender of the mail.</param>
        /// <param name="Subject">Subject of the mail.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public override int sendMail(String To, String From, String Subject, Byte[] Body, iNotificationGatewayServerDto ServerData)
        {
            _log.Info("NotificationGatewayBo.sendMail");
            FillOldServerData(ServerData);
            EaiNotificationDataDto Data = new EaiNotificationDataDto();
            Data.Type = NotificationType.EMail;
            Data.Recipients = new List<string>();
            Data.Recipients.Add(To);
            Data.Sender = From;
            Data.Subject = Subject;
            Data.Attachments = new List<EaiNotificationAttachmentsDto>();
            EaiNotificationAttachmentsDto Attachment = new EaiNotificationAttachmentsDto();
            Attachment.Data = Body;
            Attachment.Filename = "BODY.PDF";
            Data.Attachments.Add(Attachment);
            Data.SYSEAIHOT = 0;
            return sendNotification(Data);
        }

        /// <summary>
        /// Send E-Mail Proxy for sendNotification
        /// </summary>
        /// <param name="inotDto">Input Dto.</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public override int sendMail(inotificationMailDto inotDto, iNotificationGatewayServerDto ServerData)
        {
            _log.Info("NotificationGatewayBo.sendMail");
            FillOldServerData(ServerData);
            EaiNotificationDataDto Data = new EaiNotificationDataDto();
            Data.Type = NotificationType.EMail;
            Data.Recipients = new List<string>();
            Data.Recipients.Add(inotDto.To);
            Data.Sender = inotDto.From;
            Data.Subject = inotDto.Subject;
            Data.Attachments = new List<EaiNotificationAttachmentsDto>();
            EaiNotificationAttachmentsDto Attachment = new EaiNotificationAttachmentsDto();
            Attachment.Data = inotDto.Body;
            Attachment.Filename = "BODY.PDF";
            Data.Attachments.Add(Attachment);
            Data.SYSEAIHOT = 0;
            return sendNotification(Data);
        }

        /// <summary>
        /// Send Fax Proxy for sendNotification
        /// </summary>
        /// <param name="To">Recipient of the mail.</param>
        /// <param name="From">Sender of the mail.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public override int sendFax(String To, String From, Byte[] Body, iNotificationGatewayServerDto ServerData)
        {
            _log.Info("NotificationGatewayBo.sendFax");
            FillOldServerData(ServerData);
            EaiNotificationDataDto Data = new EaiNotificationDataDto();
            Data.Type = NotificationType.Fax;
            Data.Recipients = new List<string>();
            Data.Recipients.Add(To);
            Data.Sender = From;
            Data.Attachments = new List<EaiNotificationAttachmentsDto>();
            EaiNotificationAttachmentsDto Attachment = new EaiNotificationAttachmentsDto();
            Attachment.Data = Body;
            Attachment.Filename = "BODY.PDF";
            Data.Attachments.Add(Attachment);
            Data.SYSEAIHOT = 0;
            return sendNotification(Data);
        }

        /// <summary>
        /// Send Fax Proxy for sendNotification
        /// </summary>
        /// <param name="inotDto">Input Dto.</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public override int sendFax(inotificationFaxDto inotDto, iNotificationGatewayServerDto ServerData)
        {
            _log.Info("NotificationGatewayBo.sendFax");
            FillOldServerData(ServerData);
            EaiNotificationDataDto Data = new EaiNotificationDataDto();
            Data.Type = NotificationType.Fax;
            Data.Recipients = new List<string>();
            Data.Recipients.Add(inotDto.To);
            Data.Sender = inotDto.From;
            Data.Attachments = new List<EaiNotificationAttachmentsDto>();
            EaiNotificationAttachmentsDto Attachment = new EaiNotificationAttachmentsDto();
            Attachment.Data = inotDto.Body;
            Attachment.Filename = "BODY.PDF";
            Data.Attachments.Add(Attachment);
            Data.SYSEAIHOT = 0;
            return sendNotification(Data);
        }

        /// <summary>
        /// Send SMS Proxy for sendNotification
        /// </summary>
        /// <param name="To">Recipient of the mail.</param>
        /// <param name="From">Sender of the mail.</param>
        /// <param name="Body">Mailbody</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public override int sendSms(String To, String From, String Body, iNotificationGatewayServerDto ServerData)
        {
            _log.Info("NotificationGatewayBo.sendSms");
            FillOldServerData(ServerData);
            EaiNotificationDataDto Data = new EaiNotificationDataDto();
            Data.Type = NotificationType.SMS;
            Data.Recipients = new List<string>();
            Data.Recipients.Add(To);
            Data.Sender = From;
            Data.Text = Body;
            Data.SYSEAIHOT = 0;
            return sendNotification(Data);
        }

        /// <summary>
        /// Send SMS Proxy for sendNotification
        /// </summary>
        /// <param name="inotDto">Input Dto.</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public override int sendSms(inotificationSmsDto inotDto, iNotificationGatewayServerDto ServerData)
        {
            _log.Info("NotificationGatewayBo.sendSms");
            FillOldServerData(ServerData);
            EaiNotificationDataDto Data = new EaiNotificationDataDto();
            Data.Type = NotificationType.SMS;
            Data.Recipients = new List<string>();
            Data.Recipients.Add(inotDto.To);
            Data.Sender = inotDto.From;
            Data.Text = inotDto.Body;
            Data.SYSEAIHOT = 0;
            return sendNotification(Data);
        }

        /// <summary>
        /// Senden einer Notification via EAI DB-Datenschema
        /// Dies kann eine E-Mail, SMS oder ein Fax sein abhänging von den DB Einstellungen
        ///</summary>
        /// <param name="NotificationID">Primary Key auf der EAIHOT-Tabelle</param>
        /// <param name="ServerData">Serverparameter Datenset</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        public override int sendEAINotification(int NotificationID, iNotificationGatewayServerDto ServerData)
        {
            FillOldServerData(ServerData);
            DateTime Start = DateTime.Now;
            int rval = 0;

            EaiNotificationDataDto Data = notificationGatewayDbDao.getNotificationDaten(NotificationID, NotificationSettings);
            DateTime DataGot = DateTime.Now;
            TimeSpan DataDifference = new TimeSpan();
            DataDifference = (DataGot - Start);
            _log.Debug("Time to get Data: " + DataDifference.ToString());
            int ErrorCode = sendNotification(Data);
            DateTime MailSent = DateTime.Now;
            TimeSpan SentDifference = new TimeSpan();
            SentDifference = (MailSent - DataGot);
            _log.Debug("Time to send Mail: " + SentDifference.ToString());
            notificationGatewayDbDao.setNotifcationReturnValues(NotificationID, ErrorCode);
            rval = ErrorCode;
            DateTime Stop = DateTime.Now;
            TimeSpan Difference = new TimeSpan();
            Difference = (Stop - Start);
            _log.Debug("Overall time to send DB Notification: " + Difference.ToString());
            return rval;
        }

        /// <summary>
        /// Sending E-Mail, Fax or SMS  to Recipient via MessagingInterface
        /// This is a central Routine that will be served by the Interfaces front.
        /// </summary>
        /// <param name="Input">Übergebene Parameterstruktur</param>
        /// <returns>Success flag: true = Success; false = Failure )</returns>
        private int sendNotification(EaiNotificationDataDto Input)
        {
            _log.Info("NotificationGatewayBo.sendNotification");
            int ReturnCode = 2;

            List<string> Recipients = new List<string>();
            bool bCheck = false;

            bool enabled = AppConfig.Instance.getBooleanEntry("NOTIFICATION", "ENABLED", false, "SETUP.NET");
            if (!enabled)
            {
                _log.Info("NotificationGatewayBo: Disabled - SETUP.NET/NOTIFICATION/ENABLED is not TRUE");
                return ReturnCode;
            }

            bool redirect = AppConfig.Instance.getBooleanEntry("NOTIFICATION", "REDIRECT", false, "SETUP.NET");
            if (redirect)
            {
                String newRecipient = "";
                switch (Input.Type)
                {
                    case NotificationType.EMail:
                        _log.Info("NotificationGatewayBo: Redirect to - SETUP.NET/NOTIFICATION/REDIRECT_EMAIL");
                        newRecipient = AppConfig.Instance.GetEntry("NOTIFICATION", "REDIRECT_EMAIL", "", "SETUP.NET");
                        break;
                    case NotificationType.Fax:
                        _log.Info("NotificationGatewayBo: Redirect to - SETUP.NET/NOTIFICATION/REDIRECT_FAX");
                        newRecipient = AppConfig.Instance.GetEntry("NOTIFICATION", "REDIRECT_FAX", "", "SETUP.NET");
                        break;
                    case NotificationType.SMS:
                        _log.Info("NotificationGatewayBo: Redirect to - SETUP.NET/NOTIFICATION/REDIRECT_SMS");
                        newRecipient = AppConfig.Instance.GetEntry("NOTIFICATION", "REDIRECT_SMS", "", "SETUP.NET");
                        break;
                }
                String orgRecipient = String.Join(",", Input.Recipients);
                Input.Recipients.Clear();
                Input.Recipients.Add(newRecipient);

                if (Input.CarbonCopyRecipients != null)
                    Input.CarbonCopyRecipients.Clear();

                Input.Text = "Redirected from: " + orgRecipient + "\n" + Input.Text;
            }

            switch (Input.Type)
            {
                case NotificationType.EMail:
                {
                    string DefectRecipient = "";
                    ;
                    foreach (string Recipient in Input.Recipients)
                    {
                        Recipients.Add(CleanEMail(Recipient));
                        _log.Info("Check E-Mail Recipient: " + Recipient);
                        bCheck = CheckEMail(Recipient);
                        if (bCheck == false)
                        {
                            DefectRecipient = Recipient;
                            break;
                        }
                    }
                    if (bCheck == false)
                    {
                        throw (new Exception("Recipient " + DefectRecipient + " is no valid address!"));
                    }
                    ReturnCode = SendNotifyMail(Input.Type, Input.Sender, Recipients, Input.CarbonCopyRecipients, Input.BlindCarbonCopyRecipients, Input.Variables, Input.Subject, Input.Text, Input.Attachments, Input.XTags, NotificationSettings[NotifySettings.SMTPServer], Convert.ToInt32(NotificationSettings[NotifySettings.SMTPPort]));
                }
                    break;
                case NotificationType.Fax:
                {
                    List<string> EMails = new List<string>();
                    string DefectRecipient = "";

                    foreach (string Recipient in Input.Recipients)
                    {
                        string EMail = CleanFax(Recipient);
                        bCheck = CheckFaxSms(Recipient);
                        if (bCheck == false)
                        {
                            DefectRecipient = Recipient;
                            break;
                        }
                        if (NotificationSettings[NotifySettings.FaxAdresse][0] == '@')
                        {
                            EMail += NotificationSettings[NotifySettings.FaxAdresse];
                        }
                        else
                        {
                            EMail += "@" + NotificationSettings[NotifySettings.FaxAdresse];
                        }
                        EMails.Add(EMail);
                        _log.Info("Check E-Mail Recipient: " + EMail);
                    }
                    if (bCheck == false)
                    {
                        throw (new Exception("Recipient " + DefectRecipient + " is no valid address!"));
                    }
                    foreach (string EMail in EMails)
                    {
                        List<string> Recipient = new List<string>();
                        Recipient.Add(EMail);
                        ReturnCode = SendNotifyMail(Input.Type, Input.Sender, Recipient, null, null, null, Input.Subject, Input.Text, Input.Attachments, Input.XTags, NotificationSettings[NotifySettings.SMTPServer], Convert.ToInt32(NotificationSettings[NotifySettings.SMTPPort]));
                    }
                }
                    break;
                case NotificationType.SMS:
                {
                    var sendSmsThroughWebService = ServerData != null && ServerData.SmsExternalGatewayEnabled;

                    List<string> EMails = new List<string>();
                    string DefectRecipient = "";
                    foreach (string Recipient in Input.Recipients)
                    {
                        string EMail = CleanSMS(Recipient);
                        bCheck = CheckFaxSms(Recipient);
                        if (bCheck == false)
                        {
                            DefectRecipient = Recipient;
                            break;
                        }
                        if (!sendSmsThroughWebService)
                        {
                            if (NotificationSettings[NotifySettings.SmsAdresse][0] == '@')
                            {
                                EMail += NotificationSettings[NotifySettings.SmsAdresse];
                            }
                            else
                            {
                                EMail += "@" + NotificationSettings[NotifySettings.SmsAdresse];
                            }
                        }
                        EMails.Add(EMail);
                        _log.Info("Check E-Mail Recipient: " + EMail);
                    }
                    if (bCheck == false)
                    {
                        throw (new Exception("Recipient " + DefectRecipient + " is no valid address!"));
                    }
                    foreach (string EMail in EMails)
                    {
                        ReturnCode = SendNotifySms(sendSmsThroughWebService, Input.Type, Input.Sender, EMail, null, null, null, Input.Subject, Input.Text, Input.Attachments, Input.XTags, NotificationSettings[NotifySettings.SMTPServer], Convert.ToInt32(NotificationSettings[NotifySettings.SMTPPort]));
                    }
                }
                    break;
            }
            return ReturnCode;
        }

        private int SendNotifySms(bool sendSmsThroughWebService, NotificationType Type, string Sender, string Recipient, List<string> CCs, List<string> BCCs, Dictionary<string, string> Variables, string Subject, string Text, List<EaiNotificationAttachmentsDto> Attachments, Dictionary<string, string> XTags, string Server, int Port)
        {
            if (sendSmsThroughWebService)
            {
                int retval = 2;

                if (this.smsDao.SendSms(ServerData, Sender, Recipient, Text))
                {
                    _log.Info("Successful SMS Transmission!");
                }
                else
                {
                    _log.Info("SMS Transmission failed!");
                    retval = 4;
                }
                return retval;
            }
            return SendNotifyMail(Type, Sender, new List<string>() { Recipient }, CCs, BCCs, Variables, Subject, Text, Attachments, XTags, Server, Port);
        }

        /// <summary>
        /// Send an E-Mail to the Server
        /// </summary>
        /// <param name="Type">Type of the Notification</param>
        /// <param name="Sender">Sender</param>
        /// <param name="Recipients">Recipients</param>
        /// <param name="CCs">Kopie Empfängerliste</param>
        /// <param name="BCCs">Blindkopie Empfängerliste</param>
        /// <param name="Variables">Variablen</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Text">Text-Body</param>
        /// <param name="Attachments">Attachments</param>
        /// <param name="Server">Servername</param>
        /// <param name="Port">Portnumber (can be null)</param>
        /// <param name="XTags">X-Tags Liste (schlüsselbasiert)</param>
        /// <returns>ErrorCode 2 = Success >2 = Failure</returns>
        private int SendNotifyMail(NotificationType Type, string Sender, List<string> Recipients, List<string> CCs, List<string> BCCs, Dictionary<string, string> Variables, string Subject, string Text, List<EaiNotificationAttachmentsDto> Attachments, Dictionary<string, string> XTags, String Server, int Port)
        {
            int retval = 2;

            MailMessage Mail = null;

            Mail = BuildMessage(Type, Sender, Recipients, CCs, BCCs, Variables, Subject, Text, Attachments, XTags);

            // In case we need the binary Mail-representation to store in DB
            //Byte[] Maildata = Message.getMessage();

            _log.Info("Prepare oNotifyGatewaySmtpSendDto for sending Data to SMTP-Server");
            oNotifyGatewaySmtpSendDto SendData = new oNotifyGatewaySmtpSendDto();
            SendData.Message = Mail;
            SendData.Server = Server;
            SendData.ServerData = ServerData;
            if (Port != 0)
            {
                SendData.Port = (int)Port;
            }
            else
            {
                SendData.Port = 25;
            }
            _log.Info("Call notificationGatewaySmtpDao.sendNachricht.");
            if (notificationGatewaySmtpDao.sendNachricht(SendData) == true)
            {
                _log.Info("Successful SMTP Transmission!");
            }
            else
            {
                _log.Info("SMTP Transmission failed!");
                retval = 4;
            }
            return retval;
        }

        /// <summary>
        /// Check an e-mail address for vailidity
        /// 
        /// Currently no internal testing of E-Mail-Adresses is done. So this is currently just a Dummy.
        /// </summary>
        /// <param name="Address">Address to be checked for validity</param>
        /// <returns>Valid = true, Invalid = false</returns>
        private bool CheckEMail(string Address)
        {
            bool bReturn = false;
            try
            {
                if (NotificationSettings[NotifySettings.EMailRegEx].Length > 0)
                {
                    // @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]+$"
                    Regex reVerify = new Regex(NotificationSettings[NotifySettings.EMailRegEx]);
                    Match RecipientMatch = reVerify.Match(Address);
                    if (RecipientMatch.Success == true)
                    {
                        _log.Info("Address passed testing");
                        bReturn = true;
                    }
                    else
                    {
                        _log.Info("Adddress failed testing");
                    }
                }
                bReturn = true;
            }
            catch (Exception Exp)
            {
                _log.Error("Critical Error in sendNotification during Regex evaluation of '" + Address + "'! Exception:" + Exp);
            }
            return bReturn;
        }

        /// <summary>
        /// Build Message
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Sender"></param>
        /// <param name="Recipients"></param>
        /// <param name="CCs"></param>
        /// <param name="BCCs"></param>
        /// <param name="Variables"></param>
        /// <param name="Subject"></param>
        /// <param name="Text"></param>
        /// <param name="Attachments"></param>
        /// <param name="XTags"></param>
        /// <returns></returns>
        private MailMessage BuildMessage(NotificationType Type, string Sender, List<string> Recipients, List<string> CCs, List<string> BCCs, Dictionary<string, string> Variables, string Subject, string Text, List<EaiNotificationAttachmentsDto> Attachments, Dictionary<string, string> XTags)
        {
            _log.Info("Create MailMessage");
            MailMessage Message = new MailMessage();
            foreach (string Recipient in Recipients)
            {
                if ((Recipient != null) && (Recipient.Length != 0))
                {
                    MailAddress adrTo = new MailAddress(Recipient);
                    _log.Info("Add Recipient to MailMessage");
                    Message.To.Add(adrTo);
                }
            }
            if (Message.To.Count == 0)
            {
                throw new Exception("Kein Empänger angegeben!");
            }

            _log.Info("Check E-Mail Sender: " + Sender);
            if ((Sender == null) || Sender == "")
            {
                throw new Exception("No Sender set!");
            }
            if (CheckEMail(Sender) == false)
            {
                throw (new Exception("Sender is no valid address!"));
            }

            MailAddress adrFrom = new MailAddress(Sender);

            // Create Client to send the Mail to a file

            _log.Info("Create Attachment");
            Attachment DataBody = null;

            _log.Info("Add Sender to MailMessage");
            Message.From = adrFrom;

            switch (Type)
            {
                case NotificationType.EMail:
                    if ((Subject == null) || (Subject == ""))
                    {
                        throw new Exception("Kein Betreff angegeben!");
                    }
                    _log.Info("Add Subject >>" + Subject + "<< to MailMessage");
                    Message.Subject = Subject;
                    _log.Info("Set empty Mailbody");
                    Message.Body = Text;
                    _log.Info("Add Attachment to MailMessage");
                    SetComplexMailBody(Message, Type, Text,Variables, Attachments, XTags);
                    if (CCs != null)
                    {
                        foreach (string Recipient in CCs)
                        {
                            MailAddress adrCc = new MailAddress(Recipient);
                            _log.Info("Add Recipient to MailMessage");
                            Message.CC.Add(adrCc);
                        }
                    }
                    if (BCCs != null)
                    {
                        foreach (string Recipient in BCCs)
                        {
                            MailAddress adrBcc = new MailAddress(Recipient);
                            _log.Info("Add Recipient to MailMessage");
                            Message.Bcc.Add(adrBcc);
                        }
                    }
                    break;
                case NotificationType.Fax:
                    _log.Info("Set Subject >>Faxversand<< to MailMessage");
                    Message.Subject = "Faxversand";
                    _log.Info("Set empty Mailbody");
                    Message.Body = Text;
                    _log.Info("Add Attachment to MailMessage");
                    if (XTags != null)
                    {
                        foreach (string Key in XTags.Keys.ToArray())
                        {
                            Message.Headers.Add(Key, XTags[Key]);
                        }
                    }
                    foreach (EaiNotificationAttachmentsDto Attachment in Attachments)
                    {
                        if (Attachment.MIME != null)
                        {
                            ContentType CTyp = new ContentType(Attachment.MIME);
                            CTyp.Name = Attachment.Filename;
                            DataBody = new Attachment(new MemoryStream(Attachment.Data), CTyp);
                            Message.Attachments.Add(DataBody);
                        }
                    }
                    break;
                case NotificationType.SMS:
                    Message.Subject = "";
                    Message.Body = Text;
                    Message.BodyEncoding = Encoding.Unicode;
                    _log.Info("SMS message built.");
                    break;
            }
            return Message;
        }

        /// <summary>
        /// Check an phone number prefixed e-mail address for validity
        /// </summary>
        /// <param name="Address">Address to be checked for validity</param>
        /// <returns>Valid = true, Invalid = false</returns>
        private bool CheckFaxSms(string Address)
        {
            bool bReturn = false;
            try
            {
                if (NotificationSettings[NotifySettings.TelefonRegEx].Length > 0)
                {
                    // Nur Prüfung auf den Telefonnummer-Teil
                    Regex reVerify = new Regex(NotificationSettings[NotifySettings.TelefonRegEx]);
                    Match RecipientMatch = reVerify.Match(Address);
                    if (RecipientMatch.Success == true)
                    {
                        _log.Info("Address passed testing");
                        bReturn = true;
                    }
                    else
                    {
                        _log.Info("Adddress failed testing");
                    }
                }
                else
                    bReturn = true;
            }
            catch (Exception Exp)
            {
                _log.Error("Critical Error in sendNotification during Regex evaluation of '" + Address + "'! Exception:" + Exp);
            }
            return bReturn;
        }

        private void SetComplexMailBody(MailMessage Mail, NotificationType Type, string Text, Dictionary<string, string> Variables, List<EaiNotificationAttachmentsDto> Attachments, Dictionary<string, string> XTags)
        {
            if (XTags != null)
            {
                foreach (string Key in XTags.Keys.ToArray())
                {
                    Mail.Headers.Add(Key, XTags[Key]);
                }
            }

            EaiNotificationAttachmentsDto body = null;
            EaiNotificationAttachmentsDto template = null;
            Dictionary<string, string> snippets = new Dictionary<string, string>();
            List<LinkedResource> images = new List<LinkedResource>();

            foreach (EaiNotificationAttachmentsDto Attachment in Attachments)
            {
                string Mime = Attachment.MIME;
                string Extension = Path.GetExtension(Attachment.Filename);
                string FileName = Path.GetFileName(Attachment.Filename);
                string Name = FileName.Substring(0, FileName.Length - Extension.Length);
                Name = Name.ToUpper();

                if (Name == "TEMPLATE")
                {
                    if (template == null)
                        template = Attachment;
                }
                else if (Name.StartsWith("_IMG"))
                {
                    if (images.FirstOrDefault(a => a.ContentId == Attachment.Filename) == null)
                    {
                        LinkedResource img = new LinkedResource(new MemoryStream(Attachment.Data));

                        ContentType CTyp = new ContentType(Attachment.MIME);
                        img.ContentId = Attachment.Filename;
                        img.ContentType = CTyp;
                        img.TransferEncoding = TransferEncoding.Base64;
                        img.ContentType.Name = img.ContentId;
                        img.ContentLink = new Uri("cid:" + img.ContentId);

                        images.Add(img);
                    }
                    else
                    {
                        _log.Warn("Already added a Picture with ContentId: " + Attachment.Filename);
                    }
                }
                else if (Name.StartsWith("_SNIPPET"))
                {
                    if (!snippets.ContainsKey(Attachment.Filename))
                    {
                        snippets.Add(Attachment.Filename, StringConversionHelper.ClarionByteToString(Attachment.Data));
                    }
                    else
                    {
                        _log.Warn("Already added a Snippet with ContentId: " + Attachment.Filename);
                    }
                }
                else if (Name == "BODY")
                {
                    if (body == null)
                        body = Attachment;
                }
                else
                {
                    if (Attachment.MIME != null)
                    {
                        ContentType CTyp = new ContentType(Attachment.MIME);
                        CTyp.Name = Attachment.Filename;
                        Attachment DataBody = new Attachment(new MemoryStream(Attachment.Data), CTyp);
                        Mail.Attachments.Add(DataBody);
                    }
                }
            }
            
            string message = Text;
            if (body != null)
            {
                switch (body.MIME)
                {
                    case "text/html":
                    case "text/plain":
                        {
                            ASCIIEncoding enc = new ASCIIEncoding();
                            message = enc.GetString(body.Data);
                        }
                        break;
                    case "application/pdf":
                        {
                            ContentType CTyp = new ContentType("application/pdf");
                            CTyp.Name = body.Filename;
                            Attachment DataBody = new Attachment(new MemoryStream(body.Data), CTyp);
                            Mail.Attachments.Add(DataBody);
                        }
                        break;
                }
            }
            if (template != null)
            {
                //ASCIIEncoding enc = new ASCIIEncoding();
                //string templateString = enc.GetString(template.Data);
                string templateString = StringConversionHelper.ClarionByteToString(template.Data);
                HtmlReportBo bo = new HtmlReportBo(new StringHtmlTemplateDao(templateString));
                message = bo.CreateHtmlMailReport(Variables, snippets);
                
            }

            string plainMessage = Regex.Replace(message, "<[^>]+?>", "");
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(plainMessage,
                                                                                Encoding.GetEncoding("ISO-8859-1"),
                                                                                MediaTypeNames.Text.Plain);

            message = message.Replace("\r\n", "<br/>");
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(message,
                                                                               Encoding.GetEncoding("ISO-8859-1"),
                                                                               MediaTypeNames.Text.Html);

            foreach (LinkedResource img in images)
            {
                htmlView.LinkedResources.Add(img);
            }

            Mail.AlternateViews.Add(plainView);
            Mail.AlternateViews.Add(htmlView);
            Mail.IsBodyHtml = true;

        }

        /// <summary>
        /// Filter Function for E-Mails
        /// </summary>
        /// <param name="Recipient">Recipient to clean</param>
        /// <returns>Filtered Recipient</returns>
        private String CleanEMail(String Recipient)
        {
            return CleanString(Recipient, NotificationSettings[NotifySettings.EMailFilter]);
        }

        /// <summary>
        /// Filter Function for SMS
        /// </summary>
        /// <param name="Recipient">Recipient to clean</param>
        /// <returns>Filtered Recipient</returns>
        private String CleanSMS(String Recipient)
        {
            return CleanString(Recipient, NotificationSettings[NotifySettings.SmsFilter]);
        }

        /// <summary>
        /// Filter Function for SMS
        /// </summary>
        /// <param name="Recipient">Recipient to clean</param>
        /// <returns>Filtered Recipient</returns>
        private String CleanFax(String Recipient)
        {
            return CleanString(Recipient, NotificationSettings[NotifySettings.FaxFilter]);
        }

        /// <summary>
        /// General string Filter function for the Cleaning of Adresses
        /// </summary>
        /// <param name="Input">String to clean</param>
        /// <param name="Filter">Filter to use</param>
        /// <returns></returns>
        private String CleanString(String Input, String Filter)
        {
            String Data = Input;
            foreach (char chRem in Filter)
            {
                bool bRepeat = true;
                while (bRepeat)
                {
                    int iPos = Data.IndexOf(chRem);
                    if (iPos != -1)
                        Data = Data.Remove(iPos, 1);
                    else
                        bRepeat = false;
                }
            }
            return Data;
        }

        /// <summary>
        /// Übergabe der Serverdaten nach altem Standard handhaben
        /// Hier werden die Serverdaten übergeben falls sie nicht bereits vorher mit der neuen Methode vorbelegt wurden.
        /// </summary>
        /// <param name="serverData">Übergabe der Serverparameter nach dem alten Standard</param>
        private void FillOldServerData(iNotificationGatewayServerDto serverData)
        {
            if (serverData != null)
            {
                this.ServerData = serverData;
                if (NotificationSettings[NotifySettings.SMTPServer].Trim().Length == 0)
                {
                    NotificationSettings[NotifySettings.SMTPServer] = serverData.SMTPServer;
                }
                if (NotificationSettings[NotifySettings.SMTPPort].Trim().Length == 0)
                {
                    if (serverData.SMTPPort != 25)
                        NotificationSettings[NotifySettings.SMTPPort] = serverData.SMTPPort.ToString();
                    else
                        NotificationSettings[NotifySettings.SMTPPort] = "25";
                }
                if (NotificationSettings[NotifySettings.FaxAdresse].Trim().Length == 0)
                {
                    NotificationSettings[NotifySettings.FaxAdresse] = serverData.FaxAdresse;
                }
                if (NotificationSettings[NotifySettings.SmsAdresse].Trim().Length == 0)
                {
                    NotificationSettings[NotifySettings.SmsAdresse] = serverData.SmsAdresse;
                }
            }
        }

    }
}