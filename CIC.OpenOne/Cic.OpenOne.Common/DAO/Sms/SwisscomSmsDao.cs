using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.DAO.Sms
{
    using System.Reflection;

    using DTO;

    using Newtonsoft.Json;

    using One.Utils.DAO;

    using Properties;

    using Util.Config;
    using Util.Logging;

    public class SwisscomSmsDao : ISmsDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool SendSms(iNotificationGatewayServerDto serverDto, string sender, string receiver, string message)
        {
            try
            {
                var client = new SwisscomSmsClient(serverDto.SmsUrl, serverDto.SmsAccountName, serverDto.SmsAccountPassword, serverDto.SmsProxyUrl);
                _log.Info(string.Format("SwisscomSmsDao: Sending SMS from {0} to {1}: {2}", sender, receiver, message));
                
                var response = client.SubmitSmByAccountId(serverDto.SmsAccountId,
                    new SMRequest()
                    {
                        Source_addr = sender.Trim(),
                        Destination_addr = receiver.Trim(),
                        Short_message = message.Trim(),
                    });
                
                _log.Info(string.Format("SwisscomSmsDao: Received SMS response {0}", JsonConvert.SerializeObject(response)));

                return true;
            }
            catch (Exception e)
            {
                _log.Error("Could not send SMS via SwisscomSmsDao", e);
                return false;
            }
        }
    }
}
