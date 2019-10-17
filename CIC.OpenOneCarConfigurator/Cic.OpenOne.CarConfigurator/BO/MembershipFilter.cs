using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;


namespace Cic.OpenOne.CarConfigurator.BO
{
    public class MembershipFilter
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region Private constants
        private const string CnstStrMessageHeader = "MessageHeader";
        private const string CnstStrNs = "http://cic-software.de/CarConfigurator/CarConfiguratorService";
        #endregion




        public static MessageHeader ReadHeader()
        {
            MessageHeader MessageHeader = null;

            if (System.ServiceModel.OperationContext.Current == null)
            {
                throw new ArgumentNullException("System.ServiceModel.OperationContext.Current");
            }

            try
            {
                System.ServiceModel.Channels.MessageHeaders headers = System.ServiceModel.OperationContext.Current.IncomingMessageHeaders;

                if (headers.FindHeader(CnstStrMessageHeader, CnstStrNs) != -1)
                {
                    MessageHeader = headers.GetHeader<MessageHeader>(CnstStrMessageHeader, CnstStrNs);
                }
            }
            catch
            {
                throw;
            }

            return MessageHeader;
        }

        public static Cic.P000001.Common.Setting setFilters(Cic.P000001.Common.MessageHeader header, Cic.P000001.Common.Setting setting)
        {
            if (header == null) return setting;
            _Log.Info("setting Filters for " + header + " / " + setting);
            return setFilters(header.UserName, header.Password, header.SysBRAND, setting);
        }

        public static Cic.P000001.Common.Setting setFilters(MessageHeader header, Cic.P000001.Common.Setting setting)
        {
            if (header == null) return setting;
            _Log.Info("setting Filters for " + header + " / " + setting);
            return setFilters(header.UserName, header.Password, header.SysBRAND, setting);
        }

        private static Cic.P000001.Common.Setting setFilters(string username, string password, long sysBrand, Cic.P000001.Common.Setting setting)
        {

            return setting;

        }
    }
}
