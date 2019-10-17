using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.Web.Contract;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Security;
using System.ServiceModel;

namespace Cic.One.Web.DAO
{

    public class ChatDaoFactory
    {
        private static volatile ChatDaoFactory _self = null;
        private static readonly object InstanceLocker = new Object();

        /// <summary>
        /// Instanz der DAO Factory erzeugen
        /// </summary>
        /// <returns></returns>
        public static ChatDaoFactory getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new ChatDaoFactory();
                }
            }
            return _self;
          
        }


        private static Dictionary<WfuserDto, IchatCallback> _callbackDict = new Dictionary<WfuserDto, IchatCallback>();

        public void JoinChat(ijoinChatDto message, ref ojoinChatDto rval, CredentialContext cc)
        {
            // Subscribe the guest to the beer inventory

            long syswfuser = cc.getMembershipInfo().sysWFUSER;
            rval.Users = _callbackDict.Keys.ToList();

            IchatCallback user = OperationContext.Current.GetCallbackChannel<IchatCallback>();
            WfuserDto wfuser = LoadUser(syswfuser);

            if (_callbackDict.Where(a => a.Key.syswfuser == syswfuser).Count() == 0)
            {
                _callbackDict.Values.ToList()
                    .ForEach((callback) => callback.UserEntered(wfuser));
            }
            if (!_callbackDict.ContainsValue(user))
            {
                _callbackDict.Add(wfuser, user);
            }

        }

        private WfuserDto LoadUser(long syswfuser)
        {
            return new WfuserDto()
            {
                name = syswfuser + ". Person",
                syswfuser = syswfuser
            };
        }

        public void LeaveChat(ileaveChatDto message, ref oleaveChatDto rval, CredentialContext cc)
        {

            WfuserDto wfuser = GetUser();
            if (wfuser != null)
            {
                _callbackDict.Remove(wfuser);

                _callbackDict.Values.ToList()
                    .ForEach((callback) => callback.UserLeft(wfuser));
            }



        }

        public  void SendMessage(isendMessageDto message, ref osendMessageDto rval, CredentialContext cc)
        {
            WfuserDto sender = GetUser();
            if (sender != null)
            {
                _callbackDict.Where((a) => a.Key.syswfuser == message.Syswfuser)
                    .Select((a)=>a.Value).ToList()
                    .ForEach((callback)=>callback.ReceiveMessage(sender, message));
            }
        }

        private WfuserDto GetUser(long? syswfuser = null)
        {
            IchatCallback user = OperationContext.Current.GetCallbackChannel<IchatCallback>();
            if (_callbackDict.ContainsValue(user))
            {
                return _callbackDict.Where(a => a.Value == user).Select(a => a.Key).FirstOrDefault();
            }
            else if (syswfuser.HasValue)
                return LoadUser(syswfuser.Value);
            else
                return null;
        }
    }
}