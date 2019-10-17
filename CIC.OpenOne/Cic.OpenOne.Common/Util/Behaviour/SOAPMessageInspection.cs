using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Cic.OpenOne.Common.Util.Extension;

namespace Cic.OpenOne.Common.Util.Behaviour
{
    /// <summary>
    /// Behaviour to log SOAP requests/responses
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SOAPMessageInspection : Attribute, IServiceBehavior
    {

        #region IServiceBehavior Members
        /// <summary>
        /// AddBindingParameters
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// ApplyDispatchBehavior
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatch in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatch in channelDispatch.Endpoints)
                {
                    endpointDispatch.DispatchRuntime.MessageInspectors.Add(new SoapLoggingExtension());
                }
            }
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
        #endregion
    }
}