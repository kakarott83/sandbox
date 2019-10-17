using System.Collections.Generic;
using Cic.One.DTO;

namespace Cic.One.Web.DAO.Mail
{
    public interface IMailDBDao
    {
        /// <summary>
        /// Gibt den Benutzer zurück, welcher verwendet werden soll
        /// </summary>
        /// <param name="id">Id von dem User</param>
        /// <returns></returns>
        EWSUserDto GetUser(long syswfuser);

        /// <summary>
        /// Findet den SyncState eines bestimmten Benutzers raus
        /// </summary>
        /// <param name="userDataDto">Benutzer, von welchem der Status geladen werden soll</param>
        /// <returns>Gefundene Status</returns>
        string GetSyncState(EWSUserDto userDataDto);

        /// <summary>
        /// Setzt den Syncstate eines Benutzers
        /// </summary>
        /// <param name="userDataDto">Benutzer</param>
        /// <param name="syncState">neuer Status</param>
        void SetSyncState(EWSUserDto userDataDto, string syncState);

        /// <summary>
        /// Speichert eine Liste von ItemChanges in der Datenbank ab
        /// </summary>
        /// <param name="list">Liste der veränderten Items</param>
        void SaveChangedItems(List<MItemChangeDto> list);

        /// <summary>
        /// Erzeugt ein neues item
        /// </summary>
        /// <param name="item">Item, welches erzeugt werden soll</param>
        void createOrUpdateItem(MItemDto item);

        /// <summary>
        /// Erzeugt ein neues Mail Item
        /// </summary>
        /// <param name="item">Item</param>
        MailmsgDto createOrUpdateItem(MEmailMessageDto item);

        /// <summary>
        /// Erzeugt ein neues Task Item
        /// </summary>
        /// <param name="item">Item</param>
        void createOrUpdateItem(MTaskDto item);

        /// <summary>
        /// Erzeugt ein neues AppointmentDto Item
        /// </summary>
        /// <param name="item">Item</param>
        void createOrUpdateItem(MAppointmentDto item);

        /// <summary>
        /// Erzeugt ein neues ContactDto Item
        /// </summary>
        /// <param name="item">Item</param>
        void createOrUpdateItem(MContactDto item);

        /// <summary>
        /// Änderungen von Exchange sollen nicht gelöscht werden
        /// </summary>
        /// <param name="item"></param>
        void DeleteItem(MItemDto item);

        /// <summary>
        /// Read Flag setzen
        /// </summary>
        /// <param name="id">Item-Id</param>
        /// <param name="value">Wert</param>
        void SetReadFlag(string id, bool value);

        /// <summary>
        /// Liefert das Appointment Objekt zurück, welches an Exchange geliefert wird.
        /// </summary>
        /// <param name="sysApptmt"></param>
        /// <returns></returns>
        MAppointmentDto getMAppointmentDetails(long sysApptmt);

        /// <summary>
        /// Liefert das Task Objekt zurück, welches an Exchange geliefert wird.
        /// </summary>
        /// <param name="sysPtask"></param>
        /// <returns></returns>
        MTaskDto getMTaskDetails(long sysPtask);

        /// <summary>
        /// Liefert das EmailMessage Objekt zurück, welches an Exchange geliefert wird.
        /// </summary>
        /// <param name="sysMailmsg"></param>
        /// <returns></returns>
        MEmailMessageDto getMEmailMessage(long sysMailmsg);

        /// <summary>
        /// Updatet die ExchangeId von einem Datenbank Objekt (Mailmsg)
        /// </summary>
        /// <param name="sysMailmsg">Id</param>
        /// <param name="exchangeId">Neue ExchangeId, welche gespeichert werden soll</param>
        void UpdateMailmsgExchangeId(long sysMailmsg, string exchangeId);

        /// <summary>
        /// Updatet die ExchangeId von einem Datenbank Objekt (Apptmt)
        /// </summary>
        /// <param name="sysApptmt">Id</param>
        /// <param name="exchangeId">Neue ExchangeId, welche gespeichert werden soll</param>
        void UpdateApptmtExchangeId(long sysApptmt, string exchangeId);

        /// <summary>
        /// Updatet die ExchangeId von einem Datenbank Objekt (PTask)
        /// </summary>
        /// <param name="sysPtask">Id</param>
        /// <param name="exchangeId">Neue ExchangeId, welche gespeichert werden soll</param>
        void UpdatePtaskExchangeId(long sysPtask, string exchangeId);
    }
}