using System;

namespace Cic.OpenOne.Common.Util.SOAP
{
    /// <summary>
    /// MessageHeader-Interface
    /// Base of all used Message Headers
    /// Classes HAVE TO implement the empty constructor!
    /// </summary>
    public interface IMessageHeader
    {
        /// <summary>
        /// getNamespace
        /// </summary>
        /// <returns></returns>
        String getNamespace();

        /// <summary>
        /// getID
        /// </summary>
        /// <returns></returns>
        String getID();
    }
}