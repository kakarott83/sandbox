using Cic.One.Web.DAO;
using Cic.One.Web.DAO.Mail;

using System.Threading.Tasks;
using System;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Cic.One.DTO;
using Cic.OpenOne.Common.BO;

namespace Cic.One.Web.BO.Mail
{
    public class EntityMailBo : AbstractEntityMailBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EntityMailBo(IEntityDao entityDao, IMailDao mailDao, IMailDBDao mailDBDao, IAppSettingsBo appBo)
            : base(entityDao, mailDao, mailDBDao,appBo)
        {
        }

        
        public override string SyncSaveItems(EWSUserDto userDataDto, string SyncState)
        {
          
            osyncItemsDto res = mailDao.SyncItems(new isyncItemsDto() { SyncState = SyncState });
            var newSyncstate = res.SyncState;

            if (res != null && res.Changed != null && res.Changed.Count > 0)
            {
                foreach (MItemChangeDto change in res.Changed)
                {
                    switch (change.ChangeType)
                    {
                        case MChangeTypeEnum.Create:
                            createOrUpdateItem(change.Item);
                            break;

                        case MChangeTypeEnum.Delete:
                            mailDBDao.DeleteItem(change.Item);
                            break;

                        case MChangeTypeEnum.ReadFlagChange:
                            mailDBDao.SetReadFlag(change.ItemId, change.IsRead);
                            break;

                        case MChangeTypeEnum.Update:
                            createOrUpdateItem(change.Item);
                            break;
                    }
                }
            }
            if (newSyncstate != SyncState)
            {
                SyncState = newSyncstate;
                mailDBDao.SetSyncState(userDataDto, SyncState);
            }
            return SyncState;
        }

        /// <summary>
        /// Erzeugt ein neues Item
        /// Oder Updatet ein Item, welches schon vorhanden ist
        /// </summary>
        /// <param name="item">Jeglicher Typ von Item</param>
        private void createOrUpdateItem(MItemDto item)
        {
            if (item is MContactDto)
            {
                mailDBDao.createOrUpdateItem(item as MContactDto);
            }
            else if (item is MAppointmentDto)
            {
                mailDBDao.createOrUpdateItem(item as MAppointmentDto);
            }
            else if (item is MTaskDto)
            {
                mailDBDao.createOrUpdateItem(item as MTaskDto);
            }
            else if (item is MEmailMessageDto)
            {
                
                MailmsgDto mail = mailDBDao.createOrUpdateItem(item as MEmailMessageDto);
                if (mail.isNew)
                {
                    PtaskDto task = new PtaskDto();
                    task.content = "EMail bearbeiten";
                    task.dueDate = DateTime.Now;
                    task.dueTimeGUI = DateTime.Now;
                    task.subject = mail.subject;
                    task.sysContact = mail.sysContact;
                    task.sysOwner = mail.sysOwner;
                    task.sysPerson = mail.sysPerson;
                    icreateOrUpdatePtaskDto icreate = new icreateOrUpdatePtaskDto();
                    icreate.ptask = task;
                    icreate.Send = true;
                    createOrUpdatePtask(icreate);//create a new exchange-task when a new mail is created
                }
            }
        }

        /// <summary>
        /// sends mail to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        public override void sendMailmsg(long sysid)
        {
            //MailmsgDto result = entityDao.createOrUpdateMailmsg(mailmsg);

            if (mailDao.getUser() == null) //Kein Benutzer -> Exchange ausgeschaltet
                return;
            long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
            
            MEmailMessageDto emailMessage = mailDBDao.getMEmailMessage(sysid);
            
            _log.Debug("getMailMessage: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

            if (emailMessage.Id == EWSDao.SentButNoIdString)
                return;

            if (!string.IsNullOrEmpty(emailMessage.Id))
            {
                try
                {
                    mailDao.ChangeItem(new ichangeItem() { ConflictResolution = MConflictResolutionModeEnum.AutoResolve, Item = emailMessage });
                }
                catch (Microsoft.Exchange.WebServices.Data.ServiceResponseException e)
                {
                    if (e.ErrorCode == Microsoft.Exchange.WebServices.Data.ServiceError.ErrorItemNotFound)
                    {
                        emailMessage.Id = null;
                    }
                    else throw e;
                }
            }

            if (string.IsNullOrEmpty(emailMessage.Id))
            {
                ocreateItem createItemResult = mailDao.CreateItem(new icreateItem() { Item = emailMessage, UseStandardFolder = true });
                _log.Debug("createItem: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                mailDBDao.UpdateMailmsgExchangeId(sysid, createItemResult.Id);
                _log.Debug("updateExchange: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            }
           
        }

        /// <summary>
        /// sends appointment to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        public override void sendApptmt(long sysid, long? sysOwnerOld = null)
        {
            if (mailDao.getUser() == null) //Kein Benutzer -> Exchange ausgeschaltet
                return;

            //ApptmtDto result = entityDao.createOrUpdateApptmt(apptmt);
            MAppointmentDto appointment = mailDBDao.getMAppointmentDetails(sysid);

            if (appointment.Id == EWSDao.SentButNoIdString)
                return;

            //Existiert auf dem Exchange?
            if (!string.IsNullOrEmpty(appointment.Id))
            {
                if (sysOwnerOld != null && sysOwnerOld != 0 && sysOwnerOld != appointment.SysOwner)
                {
                    //Owner hat gewechselt -> Item wird bei dem alten Owner gelöscht
                    DeleteItem(appointment.Id, sysOwnerOld);
                    appointment.Id = null;
                }
                else
                {
                    //Kein Ownerwechsel -> Item wird verändert
                    ChangeItem(appointment);
                }
            }
            //Keine Id -> neues Element wird erstellt. (Kann auch aus einem Ownerwechsel aufgerufen werden)
            if (string.IsNullOrEmpty(appointment.Id))
            {
                ocreateItem createItemResult = mailDao.CreateItem(new icreateItem() { SendInvitationsMode = MSendInvitationsModeEnum.SendToAllAndSaveCopy, Item = appointment, UseStandardFolder = true });
                mailDBDao.UpdateApptmtExchangeId(sysid, createItemResult.Id);
            }
        }


        /// <summary>
        /// sends a task to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        public override void sendPtask(long sysid, long? sysOwnerOld = null)
        {
            if (mailDao.getUser() == null) //Kein Benutzer -> Exchange ausgeschaltet
                return;

            //PtaskDto result = entityDao.createOrUpdatePtask(ptask);
            MTaskDto task = mailDBDao.getMTaskDetails(sysid);

            if (task.Id == EWSDao.SentButNoIdString)
                return;

            if (!string.IsNullOrEmpty(task.Id))
            {
                if (sysOwnerOld != null && sysOwnerOld != 0 && sysOwnerOld != task.SysOwner)
                {
                    //Owner hat gewechselt -> Item wird bei dem alten Owner gelöscht
                    DeleteItem(task.Id, sysOwnerOld);
                    task.Id = null;
                }
                else
                {
                    //Kein Ownerwechsel -> Item wird verändert
                    ChangeItem(task);
                }
            }
            if (string.IsNullOrEmpty(task.Id))
            {
                ocreateItem createItemResult = mailDao.CreateItem(new icreateItem() { Item = task, UseStandardFolder = true });
                mailDBDao.UpdatePtaskExchangeId(sysid, createItemResult.Id);
            }
        }

        /// <summary>
        /// Verändert ein Item
        /// </summary>
        /// <param name="item">Item, welches geändert werden soll</param>
        private void ChangeItem(MItemDto item)
        {
            try
            {
                mailDao.ChangeItem(new ichangeItem() { ConflictResolution = MConflictResolutionModeEnum.AutoResolve, Item = item, SendInvitationsOrCancellationsMode = MSendInvitationsOrCancellationsModeEnum.SendToChangedAndSaveCopy });
            }
            catch (Microsoft.Exchange.WebServices.Data.ServiceResponseException e)
            {
                if (e.ErrorCode == Microsoft.Exchange.WebServices.Data.ServiceError.ErrorItemNotFound)
                {
                    item.Id = null;
                }
                else throw e;
            }
        }

        /// <summary>
        /// Löscht ein item von einem Owner
        /// </summary>
        /// <param name="sysOwnerOld">Owner des Items</param>
        /// <param name="itemid">Item, welches gelöscht werden soll</param>
        private void DeleteItem(string itemid, long? sysOwnerOld = null)
        {
            try
            {
                mailDao.DeleteItem(new ideleteItem() { SysOwner = sysOwnerOld, Id = itemid, DeleteMode = MDeleteModeEnum.MoveToDeletedItems, SendInvitationsOrCancellationsMode = MSendCancellationModeEnum.SendOnlyToAll });
            }
            catch (Exception e)
            {
                _log.Debug("Trying to delete " + itemid + " from Exchange", e);
            }
        }


        public static List<long> MailsSending = new List<long>();
        public static List<long> MailsToSend = new List<long>();

        /// <summary>
        /// Wird noch nicht verwendet.
        /// Später um das ganze asynchron zu machen.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <param name="list"></param>
        /// <param name="toUpdate"></param>
        public void CheckCache(string name, long id, Action<long> action, List<long> list, List<long> toUpdate)
        {
            lock (list)
            {
                if (list.Contains(id))
                {
                    if (MailsToSend != null && !MailsToSend.Contains(id))
                        MailsToSend.Add(id);
                    return;
                }

                list.Add(id);
                Task.Factory.StartNew(() =>
                    {
                        action(id);
                        list.Remove(id);
                        if (MailsToSend != null && MailsToSend.Contains(id))
                        {
                            CheckCache(name, id, action,list, toUpdate);
                        }
                    })
                    .ContinueWith((a) => _log.Error("Send "+name+" to Exchange failed for Mailmsg " + id + ": ", a.Exception), TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <summary>
        /// sends mail to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        public override void sendMailmsgAsync(long sysid)
        {
            //CheckCache("Mail", sysid, MailsSending, MailsToSend, sendMailmsg);

            sendMailmsg(sysid);
            //Task.Factory.StartNew(() => sendMailmsg(sysid))
            //    .ContinueWith((a) => _log.Error("Send Mail to Exchange failed for Mailmsg " + sysid + ": ", a.Exception), TaskContinuationOptions.OnlyOnFaulted);
      
        }

        /// <summary>
        /// sends appointment to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        public override void sendApptmtAsync(long sysid, long? sysOwnerOld = null)
        {
            sendApptmt(sysid,sysOwnerOld);
            //Task.Factory.StartNew(() => sendApptmt(sysid))
            //    .ContinueWith((a) => _log.Error("Send Appointment to Exchange failed for Apptmt " + sysid + ": ", a.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// sends a task to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        public override void sendPtaskAsync(long sysid, long? sysOwnerOld = null)
        {
            sendPtask(sysid,sysOwnerOld);
            //Task.Factory.StartNew(() => sendPtask(sysid))
            //    .ContinueWith((a) => _log.Error("Send Task to Exchange failed for Ptask " + sysid + ": ", a.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}