using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    using Newtonsoft.Json;

    using OpenOne.Common.Util.Config;

    public class ConfigurationBO
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Retrieve configuration data of the Notification Gateway Interface Server
        /// from web.config applicationSettings Cic.OpenOne.GateBANKNOW.Common.Settings
        /// </summary>
        /// <returns>Dataset with the defined Gateway Server Data</returns>
        public static iNotificationGatewayServerDto getServerDaten()
        {
            _log.Info("Starting Serverdata readout.");
            iNotificationGatewayServerDto RetVal = new iNotificationGatewayServerDto();
            try
            {
                RetVal.SMTPServer = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySmtpServer", Settings.Default.NotificationGatewaySmtpServer);
                _log.Info("SMTP Server: " + RetVal.SMTPServer);
                string strSmtpPort = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySmtpPort", Settings.Default.NotificationGatewaySmtpPort);
                RetVal.SMTPPort = 25;
                if (strSmtpPort != null)
                {
                    if (strSmtpPort.Length != 0)
                    {
                        int iSmtpPort = Convert.ToInt32(strSmtpPort);
                        if (iSmtpPort != 25)
                        {
                            RetVal.SMTPPort = iSmtpPort;
                        }
                    }
                }
                _log.Info("SMTP Port: " + RetVal.SMTPPort.ToString());
                RetVal.FaxAdresse = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySmtpFaxConnector", Settings.Default.NotificationGatewaySmtpFaxConnector);
                _log.Info("Fax Connector: " + RetVal.FaxAdresse);
                RetVal.SmsAdresse = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySmtpSmsConnector", Settings.Default.NotificationGatewaySmtpSmsConnector);
                _log.Info("SMS Connector: " + RetVal.SmsAdresse);

                RetVal.SMTPSendAsEnabled = AppConfig.Instance.getBooleanEntry("NOTIFICATION", "NotificationGatewaySmtpSendAsEnabled", Settings.Default.NotificationGatewaySmtpSendAsEnabled, "SETUP.NET");
                RetVal.SMTPServiceAccount = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySmtpServiceAccount", Settings.Default.NotificationGatewaySmtpServiceAccount);
                RetVal.SMTPServiceAccountPassword = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySmtpServiceAccountPassword", Settings.Default.NotificationGatewaySmtpServiceAccountPassword);
                RetVal.SMTPServiceAccountPlainPassword = AppConfig.Instance.getBooleanEntry("NOTIFICATION", "NotificationGatewaySmtpServiceAccountPlainPassword", Settings.Default.NotificationGatewaySmtpServiceAccountPlainPassword, "SETUP.NET");
                
                RetVal.SmsExternalGatewayEnabled = AppConfig.Instance.getBooleanEntry("NOTIFICATION", "NotificationGatewaySMSEnabled", Settings.Default.NotificationGatewaySMSEnabled, "SETUP.NET");
                RetVal.SmsUrl = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySMSURL", Settings.Default.NotificationGatewaySMSURL);
                RetVal.SmsAccountId = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySMSAccountId", Settings.Default.NotificationGatewaySMSAccountId);
                RetVal.SmsAccountName = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySMSAccountName", Settings.Default.NotificationGatewaySMSAccountName);
                RetVal.SmsAccountPassword = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySMSAccountPassword", Settings.Default.NotificationGatewaySMSAccountPassword);
                RetVal.SmsProxyUrl = AppConfig.Instance.GetCfgEntry("SETUP.NET", "NOTIFICATION", "NotificationGatewaySMSProxyUrl", Settings.Default.NotificationGatewaySMSProxyUrl);

                _log.Info("NotifcationGateway Settings: " + JsonConvert.SerializeObject(RetVal));
            }
            catch (Exception Exp)
            {
                _log.Error("Error at reading Server Data! Exception: " + Exp);
                throw (new Exception("Error reading SMTP-Data: " + Exp.Message, Exp));
            }
            return RetVal;
        }
    }
}
