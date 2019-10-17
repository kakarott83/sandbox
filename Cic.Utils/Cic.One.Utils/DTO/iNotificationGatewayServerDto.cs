using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Data Transfer Object for Serverdata of the Notification Gateway
    /// </summary>
    public class iNotificationGatewayServerDto
    {
        /// <summary>
        /// SMTP Server used
        /// </summary>
        public string SMTPServer;
        /// <summary>
        /// SMTP Port used
        /// </summary>
        public int SMTPPort;
        /// <summary>
        /// Fax adresse postfix
        /// </summary>
        public string FaxAdresse;
        /// <summary>
        /// SMS adresse postfix used when sending through smtp
        /// </summary>
        public string SmsAdresse;

        /// <summary>
        /// Should the external sms gateway be used
        /// </summary>
        public bool SmsExternalGatewayEnabled;

        /// <summary>
        /// SMS URL for external sms gateway
        /// </summary>
        public string SmsUrl;

        /// <summary>
        /// AccountId for the external sms gateway
        /// </summary>
        public string SmsAccountId;

        /// <summary>
        /// AccountName for the external sms gateway
        /// </summary>
        public string SmsAccountName;

        /// <summary>
        /// Password for the external sms gateway
        /// </summary>
        public string SmsAccountPassword;

        /// <summary>
        /// Proxy Url, if it is empty, no proxy is used.
        /// </summary>
        public string SmsProxyUrl { get; set; }

        /// <summary>
        /// Smtp service account if sendas is enabled
        /// </summary>
        public string SMTPServiceAccount;

        /// <summary>
        /// Smtp service account password if sendas is enabled
        /// </summary>
        public string SMTPServiceAccountPassword;

        /// <summary>
        /// Ture if the Service account is stored plain.
        /// </summary>
        public bool SMTPServiceAccountPlainPassword;

        /// <summary>
        /// Smtp SendAS enabled
        /// </summary>
        public bool SMTPSendAsEnabled { get; set; }

    }
}
