using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Cic.One.DTO;

namespace Cic.One.Web.Contract
{
    public interface IchatCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(WfuserDto sender, isendMessageDto message);

        [OperationContract(IsOneWay = true)]
        void UserEntered(WfuserDto person);

        [OperationContract(IsOneWay = true)]
        void UserLeft(WfuserDto person);
    }
}