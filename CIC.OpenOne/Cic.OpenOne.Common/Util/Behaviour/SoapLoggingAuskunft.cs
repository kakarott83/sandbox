using System;
using System.Reflection;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.Common.Util.Behaviour
{
    /// <summary>
    /// 
    /// </summary>
    public class SoapLoggingAuskunft : IClientMessageInspector, IDispatchMessageInspector, IEndpointBehavior
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto;
        Boolean removeAction = false;
        /// <summary>
        /// SoapLoggingAuskunf
        /// </summary>
        /// <param name="soapXMLDto"></param>
        public SoapLoggingAuskunft(ref Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }

        public SoapLoggingAuskunft(ref Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto, Boolean removeAction)
        {
            this.removeAction = removeAction;
            this.soapXMLDto = soapXMLDto;
        }
        /// <summary>
        /// Die Methode AfterReceiveReply erhält die Antwort die der SOAPClient erhält und 
        /// loggt diese in Log4Net mit und schreibt in Tabelle LogDump das Reply von Service .
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
           

            if (Cic.OpenOne.Common.Properties.Config.Default.SoapLoggingEnabled)
            {
                _log.Debug("-----" + " Reply " + DateTime.Now + " -----");
                _log.Debug(reply);
                _log.Debug("-----" + " Reply end " + " -----");
            }
            if (Cic.OpenOne.Common.Properties.Config.Default.SoapLoggingEnabledAuskunft || soapXMLDto.logDumpFlag)
            {
                Cic.OpenOne.Common.DAO.Auskunft.SoapLogDumpDao dao = new Cic.OpenOne.Common.DAO.Auskunft.SoapLogDumpDao();
                dao.CreateUpdateLogDump(reply.ToString(), soapXMLDto.code + "REPLY", soapXMLDto.entity, soapXMLDto.sysid);
                soapXMLDto.responseXML = reply.ToString();
            }
        }


        /// <summary>
        /// Die Methode BeforSendRequest erhält die Anfrage die der SOAPClient sendet und 
        /// loggt diese in Log4Net mit und schreibt in Tabelle LogDump das Request von Service .
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            if (this.removeAction)
                request.Headers.RemoveAll("Action", "http://schemas.microsoft.com/ws/2005/05/addressing/none");
            
            if (Cic.OpenOne.Common.Properties.Config.Default.SoapLoggingEnabled)
            {
                _log.Debug("-----" + " Request " + DateTime.Now + " -----");
                _log.Debug(request);
                _log.Debug("-----" + " Request end " + " -----");
            }
            if (Cic.OpenOne.Common.Properties.Config.Default.SoapLoggingEnabledAuskunft || soapXMLDto.logDumpFlag)
            {
                Cic.OpenOne.Common.DAO.Auskunft.SoapLogDumpDao dao = new Cic.OpenOne.Common.DAO.Auskunft.SoapLogDumpDao();
                dao.CreateUpdateLogDump(request.ToString(), soapXMLDto.code + "REQUEST", soapXMLDto.entity, soapXMLDto.sysid);
                soapXMLDto.requestXML = request.ToString();
            }
            return null;
        }

        /// <summary>
        /// AfterReceiveRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel,
                                            System.ServiceModel.InstanceContext instanceContext)
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
        /// ApplyClientBehavior
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            SoapLoggingAuskunft logger = new SoapLoggingAuskunft(ref soapXMLDto, this.removeAction);
            clientRuntime.MessageInspectors.Add(logger);
        }

        /// <summary>
        /// ApplyDispatchBehavior
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            SoapLoggingAuskunft logger = new SoapLoggingAuskunft(ref soapXMLDto,this.removeAction);
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