using Cic.One.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.One.Web.Contract
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract(Name = "IchatService",
        Namespace = "http://cic-software.de/One",
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IchatCallback))]
    public interface IchatService
    {
        [OperationContract(IsOneWay = false, IsInitiating = false, IsTerminating = false)]
        osendMessageDto SendMessage(isendMessageDto message);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        ojoinChatDto JoinChat(ijoinChatDto message);

        [OperationContract(IsOneWay = false, IsInitiating = false, IsTerminating = true)]
        oleaveChatDto LeaveChat(ileaveChatDto message);
    }
}
