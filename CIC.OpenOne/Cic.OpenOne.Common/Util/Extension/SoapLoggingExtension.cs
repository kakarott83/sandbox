using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Behaviour;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Linq;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.OpenOne.Common.Util.Extension
{
    /// <summary>
    /// Chains the i/o streams. Sets itself in the middle and logs the streams to EAI.
    /// </summary>
    public class SoapLoggingExtension : System.Web.Services.Protocols.SoapExtension, IDispatchMessageInspector
    {
        #region Private variables

        //streams to chain
        private System.IO.Stream _OldStream;
        private System.IO.Stream _NewStream;
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ThreadSafeDictionary<String, double> durations = new ThreadSafeDictionary<String, double>();
        private const string CnstStrTraceHeader = "TraceHeader";
        private const string CnstStrNs = "http://cic-software.de/MessageHeader";
        private static List<ISOAPMessageHandler> handlers;
        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// add a message handler for externally receiving all messages passing through
        /// </summary>
        /// <param name="handler"></param>
        public static void addSoapMessageHandler(ISOAPMessageHandler handler)
        {
            if(handlers==null)
                handlers = new List<ISOAPMessageHandler>();
            handlers.Add(handler);
        }

        /// <summary>
        /// inform all message handlers about a request
        /// </summary>
        /// <param name="msg"></param>
        private void informHandlersRequest(Message msg)
        {
            if (handlers == null) return;
            foreach(ISOAPMessageHandler hndl in handlers)
            {
                hndl.messageRequested(msg);
            }

        }
        /// <summary>
        /// inform all message handlers about a reply
        /// </summary>
        /// <param name="msg"></param>
        private void informHandlersReply(Message msg)
        {
            if (handlers == null) return;
            foreach (ISOAPMessageHandler hndl in handlers)
            {
                hndl.messageReplied(msg);
            }

        }

        /// <summary>
        /// ChainStream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public override System.IO.Stream ChainStream(System.IO.Stream stream)
        {
            if (!Cic.OpenOne.Common.Util.Config.Configuration.getSoapLoggingEnabled()) return stream;

            _OldStream = stream;
            _NewStream = new System.IO.MemoryStream();
            return _NewStream;
        }

        /// <summary>
        /// GetInitializer
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public override object GetInitializer(System.Type serviceType)
        {
            return null;
        }

        /// <summary>
        /// GetInitializer
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public override object GetInitializer(System.Web.Services.Protocols.LogicalMethodInfo methodInfo,
            System.Web.Services.Protocols.SoapExtensionAttribute attribute)
        {
            return null;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="initializer"></param>
        public override void Initialize(object initializer)
        {
        }

        /// <summary>
        /// ProcessMessage
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessage(System.Web.Services.Protocols.SoapMessage message)
        {
            // Early return
            if (!Cic.OpenOne.Common.Util.Config.Configuration.getSoapLoggingEnabled()) return;

            switch (message.Stage)
            {
                // Request before desreialization
                case System.Web.Services.Protocols.SoapMessageStage.BeforeDeserialize:
                    MyLogRequest();
                    break;
                // Response after serialization
                case System.Web.Services.Protocols.SoapMessageStage.AfterSerialize:
                    MyLogResponse();
                    break;
                // Do nothing
                case System.Web.Services.Protocols.SoapMessageStage.AfterDeserialize:
                // Do nothing
                case System.Web.Services.Protocols.SoapMessageStage.BeforeSerialize:
                default:
                    break;
            }
            // Multiple returns
        }

        #endregion

        #region Internals
        private void MyLogRequest()
        {
            try
            {
                // Copy stream
                MyCopy(_OldStream, _NewStream);
                _NewStream.Position = 0;

                string SoapRequest = MyExtractFromStream(_NewStream);
                _NewStream.Position = 0;

                _Log.Debug(SoapRequest);

            }
            catch (Exception e)
            {
                _Log.Error("Logging SoapRequest failed", e);

            }
        }

        private void MyLogResponse()
        {
            _NewStream.Position = 0;
            string SoapResponse = MyExtractFromStream(_NewStream);
            _NewStream.Position = 0;

            MyCopy(_NewStream, _OldStream);

            _Log.Debug(SoapResponse);
        }

        private string MyExtractFromStream(System.IO.Stream target)
        {
            byte[] StreamBytes;

            // Early return
            if (target == null) return string.Empty;

            StreamBytes = MyReadStream(target);
            System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

            // With early returns
            return Encoding.GetString(StreamBytes);
        }

        /// <summary>
        /// MyReadStream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] MyReadStream(System.IO.Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return memoryStream.ToArray();
                    memoryStream.Write(buffer, 0, read);
                }
            }
        }

        private void MyCopy(System.IO.Stream from, System.IO.Stream to)
        {
            try
            {
                byte[] byteArray;

                byteArray = MyReadStream(from);
                to.Write(byteArray, 0, byteArray.Length);
            }
            catch
            {
            }
            finally
            {
                // Closing reader/writer casues the stream to by closed.
                // So we have to presume that GC will deal with that.
            }
        }

        private static string MyExtractUrl()
        {
            string url = string.Empty;
            try
            {
                url = System.Web.HttpContext.Current.Request.Url.ToString();
            }
            catch
            {
                // Ignore exception
            }
            return url;
        }

        private static string MyExtractResponseUrl()
        {
            string url = string.Empty;
            try
            {
                url = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            catch
            {
                // Ignore exception
            }
            return url;
        }

        #endregion

        private OperationDescription GetOperationDescription(OperationContext operationContext)
        {
            OperationDescription od = null;
            string bindingName = operationContext.EndpointDispatcher.ChannelDispatcher.BindingName;
            string methodName;
            if (bindingName.Contains("WebHttpBinding"))
            {
                //REST request
                methodName = (string)operationContext.IncomingMessageProperties["HttpOperationName"];
            }
            else
            {
                //SOAP request
                string action = operationContext.IncomingMessageHeaders.Action;
                methodName = operationContext.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o => o.Action == action).Name;
            }

            EndpointAddress epa = operationContext.EndpointDispatcher.EndpointAddress;
            ServiceDescription hostDesc = operationContext.Host.Description;
            ServiceEndpoint ep = hostDesc.Endpoints.Find(epa.Uri);

            if (ep != null)
            {
                od = ep.Contract.Operations.Find(methodName);
            }

            return od;
        }

        /// <summary>
        /// Returns true if the web method (the operationContract Interface!!!) has 
        ///  [SoapLoggingAttribute(disable=true)] defined
        /// </summary>
        /// <returns></returns>
        private bool isDisabled()
        {
            OperationDescription operationDesc = GetOperationDescription(OperationContext.Current);
            if (operationDesc != null)
            {
                Type contractType = operationDesc.DeclaringContract.ContractType;
                var attrs = contractType.GetMethod(operationDesc.Name).GetCustomAttributes(typeof(SoapLoggingAttribute), false) as SoapLoggingAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    if (attrs[0].disable) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// True when user is enabled
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool isUserEnabled(Message request)
        {
            Cic.OpenOne.Common.Util.SOAP.DefaultMessageHeader dmh = request.Headers.GetHeader<Cic.OpenOne.Common.Util.SOAP.DefaultMessageHeader>("DefaultMessageHeader", "http://cic-software.de/MessageHeader");
            if (dmh != null)
            {
                String soapuser = AppConfig.Instance.GetEntry("LOG", "SOAPUSER", "", "SETUP.NET");
                if(dmh.SysPEROLE.ToString().Equals(soapuser))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// AfterReceiveRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (request != null)
            {
                informHandlersRequest(request);
                try
                {



                    if (_Log.IsInfoEnabled)
                    {
                        if (!Cic.OpenOne.Common.Util.Config.Configuration.getSoapLoggingEnabled() && !isUserEnabled(request)) return null;
                        if (isDisabled()) return null;
                        
                        try
                        {
                            durations[request.Headers.MessageId.ToString()] = DateTime.Now.TimeOfDay.TotalMilliseconds;
                            _Log.Info("Start of Request (" + request.Headers.MessageId.ToString() + ") " + request.Headers.Action);
                        }
                        catch
                        {
                            //ignore exception
                        }

                        string url = MyExtractUrl(request.ToString());
                        _Log.Info("SOAP Request from " + url);
                        _Log.Info(hidePassword(request.ToString()));
                    }

                }
                catch
                {
                    //ignore exception
                }
            }

            return null;
        }

        /// <summary>
        /// BeforeSendReply
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply != null)
            {
                informHandlersReply(reply);
                try
                {
                    if (!Cic.OpenOne.Common.Util.Config.Configuration.getSoapLoggingEnabled() && !isUserEnabled(reply)) return ;
                    if (isDisabled()) return;

                    try
                    {
                        if (_Log.IsInfoEnabled)
                        {
                            if (reply != null && reply.Headers != null && reply.Headers.RelatesTo != null)
                            {
                                String key = reply.Headers.RelatesTo.ToString();
                                if (durations.ContainsKey(key))
                                {
                                    double duration = (DateTime.Now.TimeOfDay.TotalMilliseconds - durations[key]);
                                    AddTraceHeader(reply, duration);
                                    _Log.Info("Duration of (" + key + ") " + reply.Headers.Action + ": " + duration);
                                    durations.RemoveSafe(key);
                                    double orgStart;
                                    foreach (String testKey in durations.Keys)
                                    {

                                        if (durations.TryGetValue(testKey, out orgStart))
                                        {
                                            double testDuration = (DateTime.Now.TimeOfDay.TotalMilliseconds - orgStart);
                                            if (testDuration > 5000)
                                            {
                                                _Log.Info("Check (" + testKey + ") ");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        //ignore exception
                    }

                    //Get information from Operation contract, needed for client IP
                    OperationContext OperationContext = OperationContext.Current;
                    MessageProperties MessageProperties = OperationContext.IncomingMessageProperties;
                    RemoteEndpointMessageProperty EndpointProperty = MessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                    if (_Log.IsInfoEnabled)
                    {
                        _Log.Info("SOAP Response to " + EndpointProperty.Address);
                        _Log.Info(reply.ToString());
                    }
                }
                catch
                {
                    //ignore exception
                }
            }
        }

        /// <summary>
        /// AddTraceHeader
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="duration"></param>
        public static void AddTraceHeader(System.ServiceModel.Channels.Message reply, double duration)
        {
            TraceHeader theader = null;

            if (System.ServiceModel.OperationContext.Current == null)
            {
                return;
            }
            try
            {
                theader = new TraceHeader(duration);
                System.ServiceModel.MessageHeader<TraceHeader> header = new System.ServiceModel.MessageHeader<TraceHeader>(theader);
                var untyped = header.GetUntypedHeader(CnstStrTraceHeader, CnstStrNs);
                reply.Headers.Add(untyped);
            }
            catch
            {
                throw;
            }
        }
        private String hidePassword(String request)
        {
            try
            {
                int End = request.IndexOf("</Password>");
                if (End > -1)
                {
                    int Start = request.LastIndexOf(">", End)+1;
                    String pwd = request.Substring(Start, End - Start);
                    return request.Replace(pwd, "XXX");
                }
            }catch(Exception e)
            {

            }
            return request;
        }
        private string MyExtractUrl(String request)
        {
            string url = string.Empty;
            try
            {
                int First = request.IndexOf("<a:To s:mustUnderstand=\"1\">") + "<a:To s:mustUnderstand=\"1\">".Length;
                int Last = request.IndexOf("</a:To>");
                if(First>-1 && Last>-1)
                {
                    url = request.Substring(First, Last - First);
                    return url;
                }
                First = request.IndexOf("<To ");
                if(First<0) return url;
                First = request.IndexOf(">",First)+1;
                Last = request.IndexOf("</",First);
                url = request.Substring(First, Last - First);
            }
            catch
            {
                // Ignore exception
            }
            return url;
        }
    }
}