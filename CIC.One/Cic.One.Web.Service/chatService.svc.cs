using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.Web.Contract;
using Cic.One.DTO;
using Cic.One.Web.BO;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.Web.DAO;

namespace Cic.One.Web.Service
{
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class chatService : IchatService
    {
        public osendMessageDto SendMessage(isendMessageDto message)
        {
            ServiceHandler<isendMessageDto, osendMessageDto> ew = new ServiceHandler<isendMessageDto, osendMessageDto>(message);
            return ew.process(delegate(isendMessageDto input, osendMessageDto rval, CredentialContext cc)
            {
                if (input == null)
                    throw new ArgumentException("No message input");

                ChatDaoFactory.getInstance().SendMessage(message,ref rval, cc);
            });
        }

        public ojoinChatDto JoinChat(ijoinChatDto message)
        {
            ServiceHandler<ijoinChatDto, ojoinChatDto> ew = new ServiceHandler<ijoinChatDto, ojoinChatDto>(message);
            return ew.process(delegate(ijoinChatDto input, ojoinChatDto rval, CredentialContext cc)
            {
                if (input == null)
                    throw new ArgumentException("No message input");

                ChatDaoFactory.getInstance().JoinChat(message, ref rval, cc);
            });
        }

        public oleaveChatDto LeaveChat(ileaveChatDto message)
        {
            ServiceHandler<ileaveChatDto, oleaveChatDto> ew = new ServiceHandler<ileaveChatDto, oleaveChatDto>(message);
            return ew.process(delegate(ileaveChatDto input, oleaveChatDto rval, CredentialContext cc)
            {
                if (input == null)
                    throw new ArgumentException("No message input");

                ChatDaoFactory.getInstance().LeaveChat(message, ref rval, cc);
            });
        }
    }
}
