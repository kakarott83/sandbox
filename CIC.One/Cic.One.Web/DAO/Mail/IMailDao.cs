using Cic.One.DTO;

namespace Cic.One.Web.DAO.Mail
{
    public interface IMailDao
    {
        /// <summary>
        /// Sendet eine Mail anhand des Inputs
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        osendMailDto SendMail(isendMailDto input);

        /// <summary>
        /// Synchronisiert Mails ab einem speziellen Punkt.
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        osyncItemsDto SyncItems(isyncItemsDto input);

        /// <summary>
        /// Verschiebt ein Item. Falls im Input keine FolderId und kein
        /// WellknownFolderName angegeben ist, wird es in den AKF-Ordner geschoben
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        omoveItem MoveItem(imoveItem input);

        /// <summary>
        /// Sucht nach Items in einem bestimmten Ordner mit optionalen Suchparametern
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        ofindItemsDto FindItems(ifindItemsDto input);

        /// <summary>
        /// Erzeugt ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        ocreateItem CreateItem(icreateItem input);

        /// <summary>
        /// Verändert ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        ochangeItem ChangeItem(ichangeItem input);

        /// <summary>
        /// Löscht ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        odeleteItem DeleteItem(ideleteItem input);

        /// <summary>
        /// Sucht nach einem string in der GAL und auf dem Mailserver
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        ofindContact FindContacts(ifindContact input);

        /// <summary>
        /// Gibt den Benutzer zurück falls er null ist, soll der Mail-Teil nicht verwendet werden.
        /// </summary>
        /// <returns></returns>
        EWSUserDto getUser();
    }
}