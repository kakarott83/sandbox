using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Cic.OpenOne.Common.Util.Logging;

// Define a SOAP Extension that traces the SOAP request and SOAP
// response for the XML Web service method the SOAP extension is
// applied to.
namespace Cic.OpenOne.Common.Util.Behaviour
{
    /// <summary>
    /// SOAPLoggingBehaviour-Klasse
    /// </summary>
    public class SOAPLoggingBehaviour : IEndpointBehavior
    {
        /// <summary>
        /// AddBindingParameters
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// ApplyClientBehavior
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new SOAPLogging());
            clientRuntime.ChannelInitializers.Add(new SOAPChannelExtension());
        }

        /// <summary>
        /// ApplyDispatchBehavior
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    /// <summary>
    /// SOAPChannelExtension-Klasse
    /// </summary>
    public class SOAPChannelExtension : IChannelInitializer, IExtension<IContextChannel>
    {
        #region IChannelInitializer Members
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="channel"></param>
        public void Initialize(IClientChannel channel)
        {
            channel.Extensions.Add(this);
        }
        #endregion

        #region IExtension<IContextChannel> Members
        /// <summary>
        /// Attach
        /// </summary>
        /// <param name="owner"></param>
        public void Attach(IContextChannel owner) { }
        /// <summary>
        /// Detach
        /// </summary>
        /// <param name="owner"></param>
        public void Detach(IContextChannel owner) { }
        #endregion

        // This is where you will save the returned data
        /// <summary>
        /// Response
        /// </summary>
        public string Response { get; set; }
    }

    /// <summary>
    /// Die SOAPLogging Klasse erbt die Methoden von den Interfaces IClientMessageInspector, IDispatchMessageInspector und IEndpointBehavior. Die Klasse wird direkt am Endpoint Behavoir angehängt und schneidet Request und Reply´s von den Clients mit.
    /// </summary>
    public class SOAPLogging : IClientMessageInspector, IDispatchMessageInspector, IEndpointBehavior
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SOAPLogging-Konstruktor
        /// </summary>
        public SOAPLogging() { }

        /// <summary>
        /// Die Methode AfterReceiveReply erhält die Antwort die der SOAPClient erhält und loggt diese in Log4Net mit.
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            String replyStr = reply.ToString();
            if (correlationState != null && correlationState is SOAPChannelExtension)
            {
                SOAPChannelExtension sce = (SOAPChannelExtension)correlationState;
                sce.Response = replyStr;
            }
            if (Cic.OpenOne.Common.Util.Config.Configuration.getSoapLoggingEnabled())
            {
                _log.Debug("-----" + " Reply " + DateTime.Now + " -----");
                _log.Debug(replyStr);
                _log.Debug("-----" + " Reply end " + " -----");
            }
        }

        /// <summary>
        /// Die Methode BeforSendRequest erhält die Anfrage die der SOAPClient sendet und loggt diese in Log4Net mit.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            if (Cic.OpenOne.Common.Util.Config.Configuration.getSoapLoggingEnabled())
            {
                _log.Debug("-----" + " Request " + DateTime.Now + " -----");
                _log.Debug(request);
                _log.Debug("-----" + " Request end " + " -----");
            }
            SOAPChannelExtension sce = new SOAPChannelExtension();
            channel.Extensions.Add(sce);
            return channel.Extensions.Find<SOAPChannelExtension>(); ;
        }

        /// <summary>
        /// AfterReceiveRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// BeforeSendReply
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// AddBindingParameters
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Keine Dokumentation
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            SOAPLogging logger = new SOAPLogging();
            clientRuntime.MessageInspectors.Add(logger);
        }

        /// <summary>
        /// ApplyDispatchBehavior
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            SOAPLogging logger = new SOAPLogging();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(logger);
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}