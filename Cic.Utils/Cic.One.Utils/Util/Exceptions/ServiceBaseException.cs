using System;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.Util.Exceptions
{
    /// <summary>
    /// Base Exception class for all Services
    /// </summary>
    [System.Serializable]
    public class ServiceBaseException : System.SystemException
    {
        private string _code;
        private MessageType _type;

        /// <summary>
        /// ServiceBaseException
        /// </summary>
        /// <param name="message"></param>
        public ServiceBaseException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// ServiceBaseException
        /// </summary>
        public ServiceBaseException()
            : base()
        {
        }

        /// <summary>
        /// ServiceBaseException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ServiceBaseException(String message, System.Exception ex)
            : base(message, ex)
        {
        }

        /// <summary>
        /// ServiceBaseException
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ServiceBaseException(String code, String message)
            : base(message)
        {
            this._code = code;
            this._type = MessageType.Error;
        }

        /// <summary>
        /// ServiceBaseException
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ServiceBaseException(String code, String message, System.Exception ex)
            : base(message, ex)
        {
            this._code = code;
            this._type = MessageType.Error;
        }

        /// <summary>
        /// ServiceBaseException
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public ServiceBaseException(String code, String message, MessageType type)
            : base(message)
        {
            this._code = code;
            this._type = type;
        }

        /// <summary>
        /// ServiceBaseException
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="type"></param>
        public ServiceBaseException(String code, String message, System.Exception ex, MessageType type)
            : base(message, ex)
        {
            this._code = code;
            this._type = type;
        }

        /// <summary>
        /// code
        /// </summary>
        public string code
        {
            get { return _code; }
        }

        /// <summary>
        /// type
        /// </summary>
        public MessageType type
        {
            get { return _type; }
        }
    }
}