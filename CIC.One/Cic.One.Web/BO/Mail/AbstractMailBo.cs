using Cic.One.Web.DAO.Mail;
using Cic.One.DTO;

namespace Cic.One.Web.BO.Mail
{
    public abstract class AbstractMailBo : IMailBo
    {
        protected IMailDao mailDao;

        public AbstractMailBo(IMailDao mailDao = null)
        {
            this.mailDao = mailDao;
        }

        /// <summary>
        /// Sendet eine Mail anhand des Inputs
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public virtual osendMailDto SendMail(isendMailDto input)
        {
            return mailDao.SendMail(input);
        }

        /// <summary>
        /// Synchronisiert Mails ab einem speziellen Punkt.
        /// </summary>
        /// <param name="input">Eingabeparameter</param>r
        /// <returns>Output</returns>
        public virtual osyncItemsDto SyncItems(isyncItemsDto input)
        {
            return mailDao.SyncItems(input);
        }

        /// <summary>
        /// Verschiebt ein Item. Falls im Input keine FolderId und kein
        /// WellknownFolderName angegeben ist, wird es in den AKF-Ordner geschoben
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public virtual omoveItem MoveItem(imoveItem input)
        {
            return mailDao.MoveItem(input);
        }

        /// <summary>
        /// Sucht nach Items in einem bestimmten Ordner mit optionalen Suchparametern
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public virtual ofindItemsDto FindItems(ifindItemsDto input)
        {
            return mailDao.FindItems(input);
        }

        /// <summary>
        /// Erzeugt ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public virtual ocreateItem CreateItem(icreateItem input)
        {
            return mailDao.CreateItem(input);
        }

        /// <summary>
        /// Verändert ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public virtual ochangeItem ChangeItem(ichangeItem input)
        {
            return mailDao.ChangeItem(input);
        }

        /// <summary>
        /// Löscht ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public virtual odeleteItem DeleteItem(ideleteItem input)
        {
            return mailDao.DeleteItem(input);
        }

        /// <summary>
        /// Sucht nach einem string in der GAL und auf dem Mailserver
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public virtual ofindContact FindContacts(ifindContact input)
        {
            return mailDao.FindContacts(input);
        }
    }
}