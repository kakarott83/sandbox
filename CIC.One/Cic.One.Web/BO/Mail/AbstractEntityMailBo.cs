using System;
using Cic.One.Web.DAO;
using Cic.One.Web.DAO.Mail;
using Cic.One.DTO;

using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.Web.BO.Mail
{
    public abstract class AbstractEntityMailBo : IEntityMailBo
    {
        protected IEntityDao entityDao;
        protected IMailDao mailDao;
        protected IMailDBDao mailDBDao;
        protected IAppSettingsBo appBo;
        protected long sysWfUser = 0;

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public AbstractEntityMailBo(IEntityDao entityDao, IMailDao mailDao, IMailDBDao mailDBDao, IAppSettingsBo appBo)
        {
            this.entityDao = entityDao;
            this.mailDao = mailDao;
            this.mailDBDao = mailDBDao;
            this.appBo = appBo;
        }

        /// <summary>
        /// Exchange sync callback
        /// </summary>
        /// <param name="userDataDto"></param>
        /// <param name="SyncState"></param>
        /// <returns></returns>
        public abstract string SyncSaveItems(EWSUserDto userDataDto, string SyncState);
        

        public void setSysWfUser(long sysWfUser)
        {
            this.sysWfUser = sysWfUser;
        }

        public long getSysWfUser()
        {
            return this.sysWfUser;
        }
        private void insertRecent(EntityDto dto)
        {
            appBo.insertRecent(dto, sysWfUser);
        }
      

        /// <summary>
        /// sends mail to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        public abstract void sendMailmsg(long sysid);

        /// <summary>
        /// sends appointment to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="sysOwnerOld">´Der alte Owner von der Entity</param>
        public abstract void sendApptmt(long sysid, long? sysOwnerOld = null);

        /// <summary>
        /// sends a task to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="sysOwnerOld">´Der alte Owner von der Entity</param>
        public abstract void sendPtask(long sysid, long? sysOwnerOld = null);


        /// <summary>
        /// sends mail to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        public abstract void sendMailmsgAsync(long sysid);

        /// <summary>
        /// sends appointment to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="sysOwnerOld">´Der alte Owner von der Entity</param>
        public abstract void sendApptmtAsync(long sysid,long? sysOwnerOld = null);

        /// <summary>
        /// sends a task to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="sysOwnerOld">´Der alte Owner von der Entity</param>
        public abstract void sendPtaskAsync(long sysid, long? sysOwnerOld = null);



        /// <summary>
        /// updates/creates Kategorien
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public virtual ItemcatDto createOrUpdateItemcat(icreateOrUpdateItemcatDto input)
        {
            var result = entityDao.createOrUpdateItemcat(input.itemcat);
            if (input.Send && mailDao.getUser() != null)
            {
                try {
                    //TODO
                    throw new Exception("You can't send a Itemcat to Exchange.");
                }
                catch (Exception e)
                {
                    input.error = new Message();
                    input.error.type = MessageType.Warn;
                    input.error.detail = e.Message;
                    input.error.code = "SEND_ERROR";
                    input.error.stacktrace = e.StackTrace;
                }
                
            }
            return result;
        }

        /// <summary>
        /// updates/creates ItemKategorien
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public virtual ItemcatmDto createOrUpdateItemcatm(icreateOrUpdateItemcatmDto input)
        {
            var result = entityDao.createOrUpdateItemcatm(input.itemcatm);
            if (input.Send && mailDao.getUser() != null)
            {
                try { 
                    if (result.area == "mailmsg")
                        sendMailmsgAsync(result.sysid);
                    else if (result.area == "apptmt")
                        sendApptmtAsync(result.sysid);
                    else if (result.area == "ptask")
                        sendPtaskAsync(result.entityId);
                }
                catch (Exception e)
                {
                    input.error = new Message();
                    input.error.type = MessageType.Warn;
                    input.error.detail = e.Message;
                    input.error.code = "SEND_ERROR";
                    input.error.stacktrace = e.StackTrace;
                }
            }
            return result;
        }

        /// <summary>
        /// updates/creates Attachement
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public virtual FileattDto createOrUpdateFileatt(icreateOrUpdateFileattDto input)
        {
            var result = entityDao.createOrUpdateFileatt(input.fileatt);
            if (input.Send && mailDao.getUser() != null)
            {
                ////Mails dürfen nicht über die Fileattachements gesendet werden
                //if (result.sysMailMsg != 0)
                //    sendMailmsgAsync(result.sysMailMsg);
                //else 
                try { 
                    if (result.sysApptmt != 0)
                        sendApptmtAsync(result.sysApptmt);
                    else if (result.sysPtask != 0)
                        sendPtaskAsync(result.sysPtask);
                }
                catch (Exception e)
                {
                    input.error = new Message();
                    input.error.type = MessageType.Warn;
                    input.error.detail = e.Message;
                    input.error.code = "SEND_ERROR";
                    input.error.stacktrace = e.StackTrace;
                }
            }
            return result;
        }

        /// <summary>
        /// updates/creates Reminder
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public virtual ReminderDto createOrUpdateReminder(icreateOrUpdateReminderDto input)
        {
            var result = entityDao.createOrUpdateReminder(input.reminder);
            if (input.Send && mailDao.getUser() != null)
            {
                try { 
                if (result.sysMailMsg != 0)
                    sendMailmsgAsync(result.sysMailMsg);
                else if (result.sysApptmt != 0)
                    sendApptmtAsync(result.sysApptmt);
                else if (result.sysPtask != 0)
                    sendPtaskAsync(result.sysPtask);
                }
                catch (Exception e)
                {
                    input.error = new Message();
                    input.error.type = MessageType.Warn;
                    input.error.detail = e.Message;
                    input.error.code = "SEND_ERROR";
                    input.error.stacktrace = e.StackTrace;
                }
            }
            return result;
        }

        /// <summary>
        /// updates/creates Recurrence
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public virtual RecurrDto createOrUpdateRecurr(icreateOrUpdateRecurrDto input)
        {
            var result = entityDao.createOrUpdateRecurr(input.recurr);
            if (input.Send && mailDao.getUser() != null)
            {
                try { 
                if (result.sysApptmt != 0)
                    sendApptmtAsync(result.sysApptmt);
                else if (result.sysPtask != 0)
                    sendPtaskAsync(result.sysPtask);
                }
                catch (Exception e)
                {
                    input.error = new Message();
                    input.error.type = MessageType.Warn;
                    input.error.detail = e.Message;
                    input.error.code = "SEND_ERROR";
                    input.error.stacktrace = e.StackTrace;
                }
            }
            return result;
        }



        /// <summary>
        /// updates/creates Mailmsg
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public virtual MailmsgDto createOrUpdateMailmsg(icreateOrUpdateMailmsgDto input)
        {
            bool sendMail = input.Send && input.mailmsg.IsDraft == 1 && mailDao.getUser() != null;

            if (sendMail)
            {
                input.mailmsg.sentDate = DateTime.Now;
                input.mailmsg.sentTime = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(input.mailmsg.sentDate.Value);
            }
            long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
            
            MailmsgDto result = entityDao.createOrUpdateMailmsg(input.mailmsg);
            _log.Debug("createOrUpdateMailmsg1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            insertRecent(result);
            _log.Debug("createOrUpdateMailmsg2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            if (sendMail)
            {
                try
                {
                    result.IsDraft = 0;
                    sendMailmsgAsync(result.sysMailmsg);
                    
                }catch(Exception e)
                {
                    input.error = new Message();
                    input.error.type = MessageType.Warn;
                    input.error.detail = e.Message;
                    input.error.code = "SEND_ERROR";
                    input.error.stacktrace = e.StackTrace;
                }
            }
            _log.Debug("createOrUpdateMailmsg3: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            //hier noch keine Item-Id in result!
            return result;
        }

        /// <summary>
        /// updates/creates Apptmt
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public virtual ApptmtDto createOrUpdateApptmt(icreateOrUpdateApptmtDto input)
        {
            ApptmtDto result = entityDao.createOrUpdateApptmt(input.apptmt);
            insertRecent(result);
            if (input.Send && mailDao.getUser() != null)
            {
                try { 
                    sendApptmtAsync(result.sysApptmt,input.apptmt.sysOwnerOld);
                }
                catch (Exception e)
                {
                    input.error = new Message();
                    input.error.type = MessageType.Warn;
                    input.error.detail = e.Message;
                    input.error.code = "SEND_ERROR";
                    input.error.stacktrace = e.StackTrace;
                }
            }
            return result;
        }

        /// <summary>
        /// updates/creates Ptask
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual PtaskDto createOrUpdatePtask(icreateOrUpdatePtaskDto input)
        {
            PtaskDto result = entityDao.createOrUpdatePtask(input.ptask);
            insertRecent(result);
            if (input.Send && mailDao.getUser() != null)
            {
                try { 
                    sendPtaskAsync(result.sysPtask, input.ptask.sysOwnerOld);
                }
                catch (Exception e)
                {
                    input.error = new Message();
                    input.error.type = MessageType.Warn;
                    input.error.detail = e.Message;
                    input.error.code = "SEND_ERROR";
                    input.error.stacktrace = e.StackTrace;
                }
            }
            return result;
        }

        /// <summary>
        /// updates/creates Wfsignature
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public WfsignatureDto createOrUpdateWfsignature(icreateOrUpdateWfsignatureDto input)
        {
            return entityDao.createOrUpdateWfsignature(input.wfsignature);
        }


        public WfsignatureDto getWfsignatureDetail(igetWfsignatureDetailDto input)
        {
            return entityDao.getWfsignatureDetail(input.type, sysWfUser);
        }

        public MailmsgDto forwardMail(long input)
        {
            MailmsgDto msg = entityDao.getMailmsgDetails(input);

            if (msg != null)
            {
                AddResponseHeader(msg, WfsignatureType.Weiterleitung);
                msg.subject = "WG: " + msg.subject;

                if (isOutgoing(msg))
                {
                    //get my mail
                }
                else
                {

                }


                ClearMail(msg);
                var fileatts =  entityDao.getFileatts("MAILMSG", input);
                if (fileatts != null && fileatts.Count > 0)
                {
                    msg = entityDao.createOrUpdateMailmsg(msg);
                    foreach (var fileatt in fileatts)
                    {
                        fileatt.sysFileAtt = 0;
                        fileatt.sysId = 0;
                        fileatt.sysPtask = 0;
                        fileatt.sysApptmt = 0;
                        fileatt.sysMailMsg = msg.sysMailmsg;
                        entityDao.createOrUpdateFileatt(fileatt);
                    }
                    return msg;
                }

                return msg;
            }
            return null;
        }

        public MailmsgDto replyMail(long input, bool all)
        {
            MailmsgDto msg = entityDao.getMailmsgDetails(input);

            if (msg != null)
            {
                AddResponseHeader(msg,WfsignatureType.Antwort);
                msg.subject = "AW: " + msg.subject;

                string recvFromTemp = msg.recvFrom;
                string to = msg.sentTo;
                string cc = msg.sentToCC;
                string bcc = msg.sentToBCC;

                ClearMail(msg);
                if (!all)
                {
                    if (isOutgoing(msg))
                    {
                        msg.sentTo = mailDao.getUser().ImpersonatedUser;
                    }
                    else
                    {
                        msg.sentTo = recvFromTemp;
                    }
                }
                else
                {
                    if (isOutgoing(msg))
                    {
                        string toDistinct = string.Join(";", to.Split(';').Distinct());
                        msg.sentTo = mailDao.getUser().ImpersonatedUser + ";" + toDistinct;
                    }
                    else
                    {
                        string user = mailDao.getUser().ImpersonatedUser.ToLower();
                        string toDistinctWithoutUser = string.Join(";",to.Split(';').Distinct().Where(a=>a.ToLower()!=user));

                        msg.sentTo = recvFromTemp + ";" + toDistinctWithoutUser;
                    }
                    msg.sentToCC = cc;
                    msg.sentToBCC = bcc;
                }
                //Antworten werden Anhänge nicht weitergeleitet
                
                return msg;
            }
            return null;
        }

        public bool isIncoming(MailmsgDto msg)
        {
            return msg.recvTime > 0;
        }
        public bool isOutgoing(MailmsgDto msg)
        {
            return !isIncoming(msg);
        }

        private void AddResponseHeader(MailmsgDto msg,WfsignatureType signatureType)
        {
            String von = "";
            String an = msg.sentTo;
            String cc = msg.sentToCC;
            String gesendet = "";//Dienstag, 15. Januar 2013 11:32";
            String betreff = msg.subject;
            DateTime? gesendetDate;

            WfsignatureDto sig = entityDao.getWfsignatureDetail(signatureType,sysWfUser);
            string signature = (sig == null)?"":("<br><br>"+sig.mailsignature);

            if (isIncoming(msg))
            { 
                von = msg.recvFrom;
                gesendetDate = CreateDate(msg.recvDate, msg.recvTimeGUI);

            }
            else
            {
                var user = mailDao.getUser();
                von =  user.ImpersonatedUser;//todo get exchange addresse;
                gesendetDate = CreateDate(msg.sentDate, msg.sentTimeGUI);
            }

            if (gesendetDate.HasValue)
            {
                string temp = gesendetDate.Value.ToLongDateString();
            }
            gesendet = string.Format("{0:ddd, dd. MMMMM yyyy HH:mm}", gesendetDate);
            if (msg.content == null)
            {
                msg.content = "";
            }

            if (!string.IsNullOrEmpty(cc))
            {
                cc = "<b>CC:</b> " + cc + "<br>";
            }
            //span style=\"font-size:10.0pt;font-family:\"Tahoma\",\"sans-serif\";mso-fareast-language:DE\"
            //TODO linie einfügen zwischen signatur und von
            String header = signature+"<br><br>"
                    + "<div>"
                        + "<div>"
                            + "<span style=\"font-size:10.0pt;font-family:'Tahoma','sans-serif';mso-fareast-language:DE\">"
                            + "<b>Von:</b> " + von
                                + "<br>"
                            + "<b>Gesendet:</b> " + gesendet
                                + "<br>"
                            + "<b>An:</b> " + an
                                + "<br>"
                            + cc
                            + "<b>Betreff:</b> " + betreff
                                + "<br>"
                            + "</span>"
                        + "</div>"
                    + "</div>"
                  + "<br>";


            if (msg.content.Contains("<html>"))
            {
                Regex r = new Regex("(<body.*?>)");
                msg.content = r.Replace(msg.content, "$1" + header, 1);
                //current.setContent(current.getContent().replaceFirst("(<body.*?>)", "$1" + header));
            }
            else
            {

                msg.content = "<html><body>" + header + msg.content + "</body></html>";

                //current.setContent("<html><body>" + header + current.getContent() + "</body></html>");
            }
        }

        private DateTime? CreateDate(DateTime? date, DateTime? time)
        {
            if (date.HasValue && time.HasValue)
            {
                return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day,time.Value.Hour,time.Value.Minute,time.Value.Second);
            }
            return null;
        }



        private void ClearMail(MailmsgDto msg)
        {
            msg.sysMailmsg = 0;
            msg.sentTo = null;
            msg.sentToBCC = null;
            msg.sentToCC = null;
            msg.IsDraft = 1;
            msg.recvDate = null;
            msg.recvFrom = null;
            msg.recvTime = 0;
            msg.itemId = null;
            msg.sentDate = null;
            msg.sentTime = 0;

        }




    }
}