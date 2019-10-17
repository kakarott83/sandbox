// OWNER MK, 07-07-2009
using System;
namespace Cic.OpenLease.ServiceAccess
{
    [System.CLSCompliant(true)]
    public static class MessageHeaderHelper
    {
        #region Private constants
        private const string CnstStrMessageHeader = "MessageHeader";
        private const string CnstStrNs = "http://cic-software.de/MessageHeader";
        #endregion

        public static Cic.OpenLease.ServiceAccess.MessageHeader ReadHeader()
        {
			Cic.OpenLease.ServiceAccess.MessageHeader MessageHeader = null;

            if (System.ServiceModel.OperationContext.Current == null)
            {
                throw new Exception("System.ServiceModel.OperationContext.Current");
            }

            try
            {
                System.ServiceModel.Channels.MessageHeaders headers = System.ServiceModel.OperationContext.Current.IncomingMessageHeaders;

                if (headers.FindHeader(CnstStrMessageHeader, CnstStrNs) != -1)
                {
                    MessageHeader = headers.GetHeader<Cic.OpenLease.ServiceAccess.MessageHeader>(CnstStrMessageHeader, CnstStrNs);
                }
            }
            catch
            {
                throw;
            }

            return MessageHeader;
        }

        public static void CreateMessageHeader(string userName, string password, long sysBRAND, long sysPEROLE, string isoLanguageCode)
        {
            Cic.OpenLease.ServiceAccess.MessageHeader MessageHeader = null;

            if (System.ServiceModel.OperationContext.Current == null)
            {
                throw new Exception("System.ServiceModel.OperationContext.Current");
            }

            try
            {
                MessageHeader = new Cic.OpenLease.ServiceAccess.MessageHeader(userName, password, sysBRAND, sysPEROLE, isoLanguageCode);
                System.ServiceModel.MessageHeader<Cic.OpenLease.ServiceAccess.MessageHeader> header = new System.ServiceModel.MessageHeader<Cic.OpenLease.ServiceAccess.MessageHeader>(MessageHeader);
                var untyped = header.GetUntypedHeader(CnstStrMessageHeader, CnstStrNs);
                System.ServiceModel.OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
            }
            catch
            {
                throw;
            }
        }

        
    }
}
