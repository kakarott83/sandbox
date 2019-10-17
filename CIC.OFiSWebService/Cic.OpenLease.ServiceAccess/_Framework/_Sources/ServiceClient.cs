// OWNER MK, 20-02-2009
using System;
namespace Cic.OpenLease.ServiceAccess
{
    public delegate void UseServiceDelegate<T>(T proxy);

    [System.CLSCompliant(true)]
    public class ServiceClient<T> : System.IDisposable
    {
        #region Private variables
        System.ServiceModel.IClientChannel _Proxy;
        System.ServiceModel.OperationContextScope _Scope;
        #endregion

        #region Constructors
		// TODO MK 5 BK, Add summary
		public ServiceClient(System.ServiceModel.IClientChannel proxy)
        {
            _Proxy = proxy;

            _Scope = new System.ServiceModel.OperationContextScope((System.ServiceModel.IContextChannel) proxy);
        }
        #endregion

		// TODO MK 5 BK, Add summary
		public void Call(UseServiceDelegate<T> codeBlock)
        {
            try
            {
                codeBlock((T)_Proxy);
            }
            catch
            {
                throw;
            }
        }

		// TODO MK 5 BK, Add summary
		// TODO MK 0 MK, Move this to constructor!
        public void AddHeader(MessageHeader messageHeader)
        {
            if (messageHeader == null)
            {
                throw new Exception("messageHeader");
            }

            System.ServiceModel.MessageHeader<MessageHeader> header = new System.ServiceModel.MessageHeader<MessageHeader>(messageHeader);
            
            var untyped = header.GetUntypedHeader("Identity", "http://www.my-website.com");
            System.ServiceModel.OperationContext.Current.OutgoingMessageHeaders.Add(untyped);

            // Client side!
            //System.ServiceModel.Channels.MessageHeaders headers = System.ServiceModel.OperationContext.Current.IncomingMessageHeaders;
            //string identity = headers.GetHeader<string>("Identity", "http://www.my-website.com");

        }

        private System.Exception Exception(string p)
        {
            throw new System.NotImplementedException();
        }

        #region IDisposable Members

		// TODO MK 5 BK, Add summary
		public void Dispose()
        {
            try
            {
                // Try to gently close (server notification and other)
                _Proxy.Close();
            }
            catch (Exception)
            {
                // If it fails abort
                _Proxy.Abort();
            }
           

            if (_Scope != null)
            {
                try
                {
                    _Scope.Dispose();
                }
                catch
                {
                    // TODO MK 0 MK, Get rid of throw
                    throw;
                }
            }
        }

        #endregion
    }
}
