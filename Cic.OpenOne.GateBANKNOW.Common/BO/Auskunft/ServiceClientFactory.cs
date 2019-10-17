using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Singleton factory class for WCF Services.
    /// Creates service clients based on interface type and automatically
    /// resets faulted channels.
    /// </summary>
    public class ServiceClientFactory
    {
        private readonly Dictionary<Type, object> factories;
        private static ServiceClientFactory instance;
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ServiceClientFactory()
        {
            factories = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="beh"></param>
        /// <returns></returns>
        public T GetClientWithBeh<T>(Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft beh)
        {
            var genericType = typeof(T);
            Type serviceClientType;
            if (genericType.IsInterface)
            {
                serviceClientType = GetClientType(genericType);

                if (serviceClientType == null)
                {
                    return default(T);
                }

                var client = Activator.CreateInstance(serviceClientType);

                GetEndpoint<T>((T)client).EndpointBehaviors.Remove(typeof(Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft));
                GetEndpoint<T>((T)client).EndpointBehaviors.Add(beh);

                if (!(client is ICommunicationObject))
                {
                    client = null;
                    return (T)client;

                }
                (client as ICommunicationObject).Faulted += Channel_Faulted<T>;

                if (!factories.ContainsKey(typeof(T)))
                {
                    var prop = serviceClientType.GetProperty("ChannelFactory");
                    var factory = prop.GetValue(client, null);
                    factories.Add(typeof(T), factory);
                }
                return (T)client;
            }
            return default(T);
        }

        /// <summary>
        /// Retrieves a service client for the interface specified in generic parameter.
        /// </summary>
        /// <typeparam name="T">Interface type to use for Service Client creation.</typeparam>
        /// <returns>Service client instance for specified interface.</returns>
        public T GetClient<T>()
        {
            var genericType = typeof(T);
            Type serviceClientType;
            if (genericType.IsInterface)
            {
                serviceClientType = GetClientType(genericType);

                if (serviceClientType == null)
                {
                    return default(T);
                }

                var client = Activator.CreateInstance(serviceClientType);
                if (!(client is ICommunicationObject))
                {
                    client = null;
                    return (T)client;

                }
                (client as ICommunicationObject).Faulted += Channel_Faulted<T>;

                if (!factories.ContainsKey(typeof(T)))
                {
                    var prop = serviceClientType.GetProperty("ChannelFactory");
                    var factory = prop.GetValue(client, null);
                    factories.Add(typeof(T), factory);
                }
                return (T)client;
            }
            return default(T);
        }

        public ServiceEndpoint GetEndpoint<T>(T inst)
        {
            var genericType = typeof(T);
            Type serviceClientType;
            if (genericType.IsInterface)
            {
                serviceClientType = GetClientType(genericType);

                if (serviceClientType == null)
                {
                    return null;
                }

                var prop = serviceClientType.GetProperty("Endpoint");
                var endpoint = prop.GetValue(inst, null);
                return (ServiceEndpoint)endpoint;
                
            }
            return null;
        }

        #region Reflection Utilities
        private static Type GetClientType(Type type)
        {
            var assy = type.Assembly;
            var serviceModelAssy = typeof(ChannelFactory).Assembly;
            var clientBaseType = serviceModelAssy.GetType("System.ServiceModel.ClientBase`1").MakeGenericType(type);

            foreach (var classType in assy.GetTypes())
            {
                if (classType.IsClass && type.IsAssignableFrom(classType))
                {
                    if (classType.IsSubclassOf(clientBaseType))
                    {
                        return classType;
                    }
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Event handler for ClientBase.Faulted event.
        /// </summary>
        /// <typeparam name="T">Interface type of service</typeparam>
        /// <param name="sender">ClientBase instance</param>
        /// <param name="e">Event Args</param>
        private void Channel_Faulted<T>(object sender, EventArgs e)
        {
            _log.Warn("Channel for " + sender + " faulted, reconnect: " + e);
            ((ICommunicationObject)sender).Abort();
            var factory = (ChannelFactory<T>)factories[typeof(T)];
            factory.CreateChannel();
        }

        /// <summary>
        /// Returns the singleton instance of ServiceClientFactory.
        /// </summary>
        /// <returns>Singleton instance of ServiceClientFactory</returns>
        public static ServiceClientFactory GetFactory()
        {
            if (instance == null)
            {
                instance = new ServiceClientFactory();
            }
            return instance;
        }
    }
}
