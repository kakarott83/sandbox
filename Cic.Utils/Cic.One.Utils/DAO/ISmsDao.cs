using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.Utils.DAO
{
    public interface ISmsDao
    {
        /// <summary>
        /// Sends an SMS through a web interface
        /// </summary>
        /// <param name="serverDto">Contains Server config</param>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool SendSms(Cic.OpenOne.Common.DTO.iNotificationGatewayServerDto serverDto, string sender, string receiver, string message);
    }

}
