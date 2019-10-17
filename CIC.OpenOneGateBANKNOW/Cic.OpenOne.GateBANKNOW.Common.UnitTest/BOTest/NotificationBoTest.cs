using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    using One.Utils.DAO;

    using OpenOne.Common.DAO.Sms;

    /// <summary>
    /// Testklasse der NotificationGateayBO
    /// </summary>
    [TestFixture()]
    public class NotificationBoTest
    {
        /// <summary>
        /// Dao Mock, to replace the Gateway's standard DB DAO usage
        /// </summary>
        public DynamicMock NotificationGatewayDbDaoMock;
        /// <summary>
        /// Dao Mock, to replace the Gateway's standard SMTP DAO usage
        /// </summary>
        public DynamicMock NotificationGatewaySmtpDaoMock;

        public DynamicMock NotificationGatewaySmsDaoMock;

        /// <summary>
        /// Object to be tested
        /// </summary>
        public NotificationGatewayBo notificationGatewayBO;

        /// <summary>
        /// Setup of the test
        /// </summary>
        [SetUp]
        public void NotificationGatewayBoInit()
        {
            NotificationGatewayDbDaoMock = new DynamicMock(typeof(INotificationGatewayDbDao));
            NotificationGatewaySmtpDaoMock = new DynamicMock(typeof(INotificationGatewaySmtpDao));
            NotificationGatewaySmsDaoMock = new DynamicMock(typeof(ISmsDao));
            notificationGatewayBO = new NotificationGatewayBo((INotificationGatewayDbDao)NotificationGatewayDbDaoMock.MockInstance, (INotificationGatewaySmtpDao)NotificationGatewaySmtpDaoMock.MockInstance, (ISmsDao)NotificationGatewaySmsDaoMock.MockInstance);
        }

        /// <summary>
        /// Test call for E-Mail sending
        /// </summary>
        [Test]
        public void sendMailTest()
        {
            Byte[] Body = new Byte[10] { 64, 65, 66, 67, 68, 69, 70, 71, 72, 73 };
            iNotificationGatewayServerDto inotificationGatewayServerData = new iNotificationGatewayServerDto()
            {
                FaxAdresse = "fax.bank-suisse.ch",
                SmsAdresse = "sms.bank-suisse.ch",
                SMTPPort = 25,
                SMTPServer = "m2.cic-group.eu"
            };


             oNotifyGatewaySmtpSendDto SendData = new oNotifyGatewaySmtpSendDto()
             {
                 Message = new MailMessage(),
                 Port = 25,
                 Server = "m2.cic-group.eu"
             };

            SendData.Message.To.Add(new MailAddress("test@cic-group.eu"));
            SendData.Message.From = new MailAddress("Horst.Ruppert@cic-group.eu");
            ContentType CTyp = new ContentType("application/pdf");
            CTyp.Name = "Mailbody.pdf";
            Attachment DataBody = new Attachment(new MemoryStream(Body), CTyp);
            SendData.Message.Attachments.Add(DataBody);
            SendData.Message.Subject = "Testmail";
            SendData.Message.Body = "";


            NotificationGatewaySmtpDaoMock.ExpectAndReturn("sendNachricht", true, SendData);
            int Return = notificationGatewayBO.sendMail("test@cic-group.eu", "Horst.Ruppert@cic-group.eu", "Testmail", Body, inotificationGatewayServerData);
            
            Assert.AreEqual(Return, 2);
        }

        /// <summary>
        /// Test call for Fax sending
        /// </summary>
        [Test]
        public void sendFaxTest()
        {
            Byte[] Body = new Byte[10] { 64, 65, 66, 67, 68, 69, 70, 71, 72, 73 };
            iNotificationGatewayServerDto inotificationGatewayServerData = new iNotificationGatewayServerDto()
            {
                FaxAdresse = "fax.bank-suisse.ch",
                SmsAdresse = "sms.bank-suisse.ch",
                SMTPPort = 25,
                SMTPServer = "m2.cic-group.eu"
            };


            oNotifyGatewaySmtpSendDto SendData = new oNotifyGatewaySmtpSendDto()
            {
                Message = new MailMessage(),
                Port = 25,
                Server = "m2.cic-group.eu"
            };

            SendData.Message.To.Add(new MailAddress("+498912345@fax.bank-suisse.ch"));
            SendData.Message.From = new MailAddress("Horst.Ruppert@cic-group.eu");
            ContentType CTyp = new ContentType("application/pdf");
            CTyp.Name = "Fax.pdf";
            Attachment DataBody = new Attachment(new MemoryStream(Body), CTyp);
            SendData.Message.Attachments.Add(DataBody);
            SendData.Message.Subject = "Faxversand";
            SendData.Message.Body = "";


            NotificationGatewaySmtpDaoMock.ExpectAndReturn("sendNachricht", true, SendData);
            int Return = notificationGatewayBO.sendFax("+498912345", "Horst.Ruppert@cic-group.eu", Body, inotificationGatewayServerData);

            Assert.AreEqual(Return, 2);
        }

        /// <summary>
        /// Test call for SMS sending
        /// </summary>
        [Test]
        public void sendSmsTest()
        {
            Byte[] Body = new Byte[10] { 64, 65, 66, 67, 68, 69, 70, 71, 72, 73 };
            iNotificationGatewayServerDto inotificationGatewayServerData = new iNotificationGatewayServerDto()
            {
                FaxAdresse = "fax.bank-suisse.ch",
                SmsAdresse = "sms.bank-suisse.ch",
                SMTPPort = 25,
                SMTPServer = "m2.cic-group.eu"
            };


            oNotifyGatewaySmtpSendDto SendData = new oNotifyGatewaySmtpSendDto()
            {
                Message = new MailMessage(),
                Port = 25,
                Server = "m2.cic-group.eu"
            };

            SendData.Message.To.Add(new MailAddress("+498912345@sms.bank-suisse.ch"));
            SendData.Message.From = new MailAddress("Horst.Ruppert@cic-group.eu");
            SendData.Message.Subject = "SMS-Versand";
            SendData.Message.Body = "Test-SMS";


            NotificationGatewaySmtpDaoMock.ExpectAndReturn("sendNachricht", true, SendData);
            int Return = notificationGatewayBO.sendSms("+498912345", "Horst.Ruppert@cic-group.eu", "Test-SMS", inotificationGatewayServerData);

            Assert.AreEqual(Return, 2);
        }

        /// <summary>
        /// Test call for SMS sending
        /// </summary>
        [Test]
        public void sendEAINotificationTest()
        {
            Byte[] Body = new Byte[10] { 64, 65, 66, 67, 68, 69, 70, 71, 72, 73 };
            iNotificationGatewayServerDto inotificationGatewayServerData = new iNotificationGatewayServerDto()
            {
                FaxAdresse = "fax.bank-suisse.ch",
                SmsAdresse = "sms.bank-suisse.ch",
                SMTPPort = 25,
                SMTPServer = "m2.cic-group.eu"
            };


            oNotifyGatewaySmtpSendDto SendData = new oNotifyGatewaySmtpSendDto()
            {
                Message = new MailMessage(),
                Port = 25,
                Server = "m2.cic-group.eu"
            };

            SendData.Message.To.Add(new MailAddress("+498912345@sms.bank-suisse.ch"));
            SendData.Message.From = new MailAddress("Horst.Ruppert@cic-group.eu");
            SendData.Message.Subject = "SMS-Versand";
            SendData.Message.Body = "Test-SMS";


            NotificationGatewaySmtpDaoMock.ExpectAndReturn("sendNachricht", true, SendData);

            EaiNotificationDataDto Data = new EaiNotificationDataDto();
            Data.SYSEAIHOT = 123;
            Data.Recipients = new List<string>() {"Sydney.doeffert@cic-group.eu","Dennis.Quaid@Universal.us"};
            Data.CarbonCopyRecipients = new List<string>() {"Michael.Grassmann@cic-group.eu","Bruce.Willis@Universal.us"};
            Data.BlindCarbonCopyRecipients = new List<string> { "Harald.Cich@cic-group.eu", "Jean.Connery@Universal.us" };
            Data.Sender = "horst.ruppert@cic-group.eu";
            Data.Subject = "Testmail sendEAINotification";
            Data.Text = "This should not be visible!";
            Data.Type = NotificationType.EMail;
            Data.Attachments = new List<EaiNotificationAttachmentsDto>();
            Data.Attachments.Add(new EaiNotificationAttachmentsDto());
            string Text = "This is just the Textpart. Should not be directly visible!";
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Data.Attachments[0].Data = enc.GetBytes(Text);
            Data.Attachments[0].Filename = "BODY.txt";
            Data.Attachments.Add(new EaiNotificationAttachmentsDto());
            string Html = "<html><header></header><body><h1>This is HTML in H1 size</h1><br /><h2>This is HTML in H2 size</h2><br /><h3>This is HTML in H3 size</h3><br /></body></html>";
            Data.Attachments[1].Data = enc.GetBytes(Html);
            Data.Attachments[1].Filename = "BODY.htm";
            Data.Attachments.Add(new EaiNotificationAttachmentsDto());
            string Attachment = "The red brown fox jumps over the lazy dog.";
            Data.Attachments[2].Data = enc.GetBytes(Attachment);
            Data.Attachments[2].Filename = "Element.txt";
            Data.XTags = new Dictionary<string, string>();
            Data.XTags.Add("X-Test1", "XValue1");
            Data.XTags.Add("X-Test2", "XValue2");
            Data.XTags.Add("X-Test3", "XValue3");
            Data.XTags.Add("X-Test4", "XValue4");
            Data.XTags.Add("X-Test5", "XValue5");


            NotificationGatewayDbDaoMock.ExpectAndReturn("getNotificationDaten", Data, 123, inotificationGatewayServerData);

            NotificationGatewayDbDaoMock.Expect("setNotifcationReturnValues", 123, 2);

            int Return = notificationGatewayBO.sendEAINotification(123, inotificationGatewayServerData);

            Assert.AreEqual(Return, 2);
        }
    }
}
