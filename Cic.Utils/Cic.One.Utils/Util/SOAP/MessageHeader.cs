using System;
using System.Collections.Generic;

namespace Cic.OpenOne.Common.Util.SOAP
{
    /// <summary>
    /// Class for reading a generalized Message Header from the current Soap Operation Context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageHeader<T> where T : IMessageHeader
    {
        private static Dictionary<Type, T> typeDefaultCache = new Dictionary<Type, T>();

        /// <summary>
        /// Returns the header of the defined Type
        /// </summary>
        /// <returns></returns>
        public T ReadHeader()
        {
            if (System.ServiceModel.OperationContext.Current == null)
            {
                throw new ArgumentNullException("System.ServiceModel.OperationContext.Current");
            }

            //Create shallow instance to detect namespace and id
            T messageHeader = default(T);

            Type tType = typeof(T);
            if (!typeDefaultCache.ContainsKey(tType))
            {
                messageHeader = (T)Activator.CreateInstance(tType);
                typeDefaultCache[tType] = messageHeader;
            }
            else
                messageHeader = typeDefaultCache[tType];

            System.ServiceModel.Channels.MessageHeaders headers = System.ServiceModel.OperationContext.Current.IncomingMessageHeaders;

            if (headers.FindHeader(messageHeader.getID(), messageHeader.getNamespace()) != -1)
            {
                messageHeader = headers.GetHeader<T>(messageHeader.getID(), messageHeader.getNamespace());
            }
            return messageHeader;
        }
    }
}