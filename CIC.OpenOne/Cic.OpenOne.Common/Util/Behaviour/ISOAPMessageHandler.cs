using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace Cic.OpenOne.Common.Util.Behaviour
{
    public interface ISOAPMessageHandler
    {
        /// <summary>
        /// called upon receiving a soap message from a client
        /// </summary>
        /// <param name="msg"></param>
        void messageRequested(Message msg);
        /// <summary>
        /// called upon sending a soap message back to client
        /// </summary>
        /// <param name="msg"></param>
        void messageReplied(Message msg);
    }
}
