using Cic.One.DTO;

namespace Cic.One.Web.BO.Mail
{
    public interface IEntityMailBo
    {
        /// <summary>
        /// Synchronization Callback from Exchange
        /// </summary>
        /// <param name="userDataDto"></param>
        /// <param name="SyncState"></param>
        /// <returns></returns>
        string SyncSaveItems(EWSUserDto userDataDto, string SyncState);
        
        /// <summary>
        /// sends mail to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        void sendMailmsg(long sysid);

        /// <summary>
        /// sends appointment to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="sysOwnerOld">´Der alte Owner von der Entity</param>
        void sendApptmt(long sysid, long? sysOwnerOld = null);

        /// <summary>
        /// sends a task to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="sysOwnerOld">´Der alte Owner von der Entity</param>
        void sendPtask(long sysid, long? sysOwnerOld = null);


        /// <summary>
        /// sends mail to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        void sendMailmsgAsync(long sysid);

        /// <summary>
        /// sends appointment to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="sysOwnerOld">´Der alte Owner von der Entity</param>
        void sendApptmtAsync(long sysid, long? sysOwnerOld = null);

        /// <summary>
        /// sends a task to the ExchangeServer (asynchron)
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="sysOwnerOld">´Der alte Owner von der Entity</param>
        void sendPtaskAsync(long sysid, long? sysOwnerOld = null);

        /// <summary>
        /// updates/creates Kategorien
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ItemcatDto createOrUpdateItemcat(icreateOrUpdateItemcatDto itemcat);

        /// <summary>
        /// updates/creates ItemKategorien
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ItemcatmDto createOrUpdateItemcatm(icreateOrUpdateItemcatmDto itemcatm);

        /// <summary>
        /// updates/creates Attachement
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        FileattDto createOrUpdateFileatt(icreateOrUpdateFileattDto fileatt);

        /// <summary>
        /// updates/creates Reminder
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ReminderDto createOrUpdateReminder(icreateOrUpdateReminderDto reminder);

        /// <summary>
        /// updates/creates Recurrence
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        RecurrDto createOrUpdateRecurr(icreateOrUpdateRecurrDto recurr);

        /// <summary>
        /// updates/creates Mailmsg
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        MailmsgDto createOrUpdateMailmsg(icreateOrUpdateMailmsgDto input);

        /// <summary>
        /// updates/creates Apptmt
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ApptmtDto createOrUpdateApptmt(icreateOrUpdateApptmtDto input);

        /// <summary>
        /// updates/creates Ptask
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        PtaskDto createOrUpdatePtask(icreateOrUpdatePtaskDto input);

        /// <summary>
        /// updates/creates Wfsignature
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        WfsignatureDto createOrUpdateWfsignature(icreateOrUpdateWfsignatureDto input);


        /// <summary>
        /// Returns all Wfsignature Details
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        WfsignatureDto getWfsignatureDetail(igetWfsignatureDetailDto input);

        /// <summary>
        /// Leitet eine Mail weiter und gibt die neue zurück
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        MailmsgDto forwardMail(long sysid);

        /// <summary>
        /// Antwortet auf eine Mail und gibt die neue zurück
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        MailmsgDto replyMail(long sysid, bool all);


    }
}