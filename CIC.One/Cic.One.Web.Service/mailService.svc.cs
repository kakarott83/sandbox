using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


using Cic.OpenOne.Common.Util.Security;
using Cic.One.Web.BO.Mail;
using Cic.One.Web.DAO.Mail;
using System.Security;

using Cic.OpenOne.Common.DTO;
using Cic.One.Web.BO;
using Cic.One.Web.Contract;
using Cic.One.DTO;
using Cic.One.Web.DAO;
using Cic.OpenLeaseAuskunftManagement.BO.SF;
using Cic.OpenLeaseAuskunftManagement.DTO;

namespace Cic.One.Web.Service
{
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class mailService : ImailService
    {

        public osendMailDto SendMail(isendMailDto input)
        {
            return ServiceHandlerSimple.process(CreateMailBo(), (mailBo) =>
            {
                if (input.Mail == null)
                    throw new ArgumentException("No Mail");
                return mailBo.SendMail(input);
            });
        }



     /*   public osyncItemsDto SyncItems(isyncItemsDto input)
        {
            return ServiceHandlerSimple.process(CreateMailBo(), (mailBo) =>
            {
                return mailBo.SyncItems(input);
            });
        }

        public omoveItem MoveItem(imoveItem input)
        {
            return ServiceHandlerSimple.process(CreateMailBo(), (mailBo) =>
            {
                if (input.Id == null)
                    throw new ArgumentException("No Id");
                return mailBo.MoveItem(input);
            });
        }*/

     /*   public ofindItemsDto FindItems(ifindItemsDto input)
        {
            return ServiceHandlerSimple.process(CreateMailBo(), (mailBo) =>
            {
                if (input.PageSize <= 0)
                    throw new ArgumentException("Pagesize must be > 0");
                return mailBo.FindItems(input);
            });
        }*/

        public ocreateItem CreateItem(icreateItem input)
        {
            return ServiceHandlerSimple.process(CreateMailBo(), (mailBo) =>
            {
                if (input.Item == null)
                    throw new ArgumentException("No Item");
                return mailBo.CreateItem(input);
            });
        }
        /*
        public ochangeItem ChangeItem(ichangeItem input)
        {
            return ServiceHandlerSimple.process(CreateMailBo(), (mailBo) => 
                {
                    if(input.Item == null)
                        throw new ArgumentException("No Item");
                    return mailBo.ChangeItem(input);
                });
        }

        public odeleteItem DeleteItem(ideleteItem input)
        {
            return ServiceHandlerSimple.process(CreateMailBo(), (mailBo) =>
            {
                if (string.IsNullOrEmpty(input.Id))
                    throw new ArgumentException("No Id");
                return mailBo.DeleteItem(input);
            });
        }


        public ofindContact FindContacts(ifindContact input)
        {
            return ServiceHandlerSimple.process(CreateMailBo(), (mailBo) =>
            {
                return mailBo.FindContacts(input);
            });
        }*/




        /// <summary>
        /// sends mail to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <returns></returns>
        public osendMailmsgToExchangeDto sendMailmsgToExchange(long sysid)
        {
            ServiceHandler<long, osendMailmsgToExchangeDto> ew = new ServiceHandler<long, osendMailmsgToExchangeDto>(sysid);
            return ew.process(delegate(long input, osendMailmsgToExchangeDto rval, CredentialContext ctx)
            {

                if (input == 0)
                    throw new ArgumentException("No valid input");

                 BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).sendMailmsg(input);
            });
        }
        /// <summary>
        /// sends appointment to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <returns></returns>
        public osendApptmtToExchangeDto sendApptmtToExchange(long sysid)
        {
            ServiceHandler<long, osendApptmtToExchangeDto> ew = new ServiceHandler<long, osendApptmtToExchangeDto>(sysid);
            return ew.process(delegate(long input, osendApptmtToExchangeDto rval, CredentialContext ctx)
            {

                if (input == 0)
                    throw new ArgumentException("No valid input");

                BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).sendApptmt(input);
            });
        }

        /// <summary>
        /// sends a task to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <returns></returns>
        public osendPtaskToExchangeDto sendPtaskToExchange(long sysid)
        {
            ServiceHandler<long, osendPtaskToExchangeDto> ew = new ServiceHandler<long, osendPtaskToExchangeDto>(sysid);
            return ew.process(delegate(long input, osendPtaskToExchangeDto rval, CredentialContext ctx)
            {

                if (input == 0)
                    throw new ArgumentException("No valid input");

                BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).sendPtask(input);
            });
        }


        public ocheckCreateSubscriptionDto CheckCreateSubscription(int CacheId)
        {
            ServiceHandler<long, ocheckCreateSubscriptionDto> ew = new ServiceHandler<long, ocheckCreateSubscriptionDto>(CacheId);
            return ew.process(delegate(long input, ocheckCreateSubscriptionDto rval)
            {
                //if (input == 0)
                //    throw new ArgumentException("No valid input");
                MailDaoFactory.getInstance().CheckCreateSubscriptionAsync(input);
            });

            
        }

        private EWSMailBo CreateMailBo()
        {
            var dao = MailDaoFactory.getInstance().getMailDao(1);
            return new EWSMailBo(dao);
        }

    }
}
