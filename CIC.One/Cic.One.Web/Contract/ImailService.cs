using System.ServiceModel;
using Cic.One.DTO;

namespace Cic.One.Web.Contract
{
    /// <summary>
    /// Das Interface ImailService stellt die Methoden für Maildienste bereit
    /// </summary>
    [ServiceContract(Name = "ImailService", Namespace = "http://cic-software.de/One")]
    public interface ImailService
    {
        /// <summary>
        /// Sendet eine Mail anhand des Inputs
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        [OperationContract]
        osendMailDto SendMail(isendMailDto input);

        /// <summary>
        /// Erzeugt ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        [OperationContract]
        ocreateItem CreateItem(icreateItem input);



        /*

        /// <summary>
        /// Synchronisiert Items ab einem speziellen Punkt.
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        [OperationContract]
        osyncItemsDto SyncItems(isyncItemsDto input);

        /// <summary>
        /// Verschiebt ein Item. Falls im Input keine FolderId und kein
        /// WellknownFolderName angegeben ist, wird es in den AKF-Ordner geschoben
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        [OperationContract]
        omoveItem MoveItem(imoveItem input);

        /// <summary>
        /// Sucht nach Items in einem bestimmten Ordner mit optionalen Suchparametern
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        [OperationContract]
        ofindItemsDto FindItems(ifindItemsDto input);

        /// <summary>
        /// Verändert ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        [OperationContract]
        ochangeItem ChangeItem(ichangeItem input);

        /// <summary>
        /// Löscht ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        [OperationContract]
        odeleteItem DeleteItem(ideleteItem input);

        /// <summary>
        /// Sucht nach einem string in der GAL und auf dem Mailserver
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        [OperationContract]
        ofindContact FindContacts(ifindContact input);

        */

        /// <summary>
        /// Überprüft ob eine Subscription vorhanden ist und erstellt eine wenn keine existiert.
        /// Ruft außerdem eine Synchronisierung auf.
        /// </summary>
        /// <param name="CacheId"></param>
        /// <returns></returns>
        [OperationContract]
        ocheckCreateSubscriptionDto CheckCreateSubscription(int CacheId);

        /// <summary>
        /// sends mail to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <returns></returns>
        [OperationContract]
        osendMailmsgToExchangeDto sendMailmsgToExchange(long sysid);

        /// <summary>
        /// sends appointment to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <returns></returns>
        [OperationContract]
        osendApptmtToExchangeDto sendApptmtToExchange(long sysid);

        /// <summary>
        /// sends a task to the ExchangeServer
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <returns></returns>
        [OperationContract]
        osendPtaskToExchangeDto sendPtaskToExchange(long sysid);
    }
}