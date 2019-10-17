using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Security Exception
    /// </summary>
    public class SecurityException : ServiceBaseException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error Code</param>
        /// <param name="type">Type of Error</param>
        public SecurityException(String code, MessageType type)
            : base(code, code, type)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error Code</param>
        /// <param name="Message">Message</param>
        /// <param name="type">Type of Error</param>
        public SecurityException(String code, String Message, MessageType type)
            : base(code, Message, type)
        {
        }
    }
}
