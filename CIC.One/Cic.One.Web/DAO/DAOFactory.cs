using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.Web.BO.Mail;
using Cic.One.Web.DAO.Mail;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.One.Web.Service.DAO;


namespace Cic.One.Web.DAO
{
    /// <summary>
    /// Factory of all DA Objects
    /// </summary>
    public class DAOFactory : IDAOFactory
    {
       

        public DAOFactory() { }

        /// <summary>
        /// returns the xpro search bo
        /// </summary>
        /// <returns></returns>
        public virtual IXproSearchBo getXproSearchBo()
        {
            return new XproSearchBo(new XproLoaderDao(), XproInfoFactory.getInstance());
        }

        /// <summary>
        /// returns the Entity DAO
        /// </summary>
        /// <returns></returns>
        public IEntityDao getEntityDao()
        {
            return new EntityDao();
        }

        /// <summary>
        /// returns the Workflow DAO
        /// </summary>
        /// <returns></returns>
        public Cic.One.DTO.IWFVDao getWorkflowDao()
        {
            return new WFVDao();
        }
        /// <summary>
        /// returns the Recurr DAO
        /// </summary>
        /// <returns></returns>
        public IRecurrDao getRecurrDao()
        {
            return new RecurrDao();
        }

        /// <summary>
        /// returns the Search DAO
        /// </summary>
        /// <returns></returns>
        public ISearchDao<R> getSearchDao<R>()
        {
            return new SearchDao<R>();
        }


        /// <summary>
        /// returns the DocumentSearchDao
        /// </summary>
        /// <returns></returns>
        public IDocumentSearchDao getDocumentSearchDao()
        {
            return new ITADocumentSearchDao();
        }

        /// <summary>
        /// returns the XproLoaderDao
        /// </summary>
        /// <returns></returns>
        public IXproLoaderDao getXproLoaderDao()
        {
            return new XproLoaderDao();
        }

        /// <summary>
        /// returns the TADocumentSearchDao
        /// </summary>
        /// <returns></returns>
        public ITADocumentSearchDao getTADocumentSearchDao()
        {
            return new ITADocumentSearchDao();
        }

        /// <summary>
        /// returns the Authentication Dao
        /// </summary>
        /// <returns></returns>
        public IAuthenticationDao getAuthenticationDao()
        {
            return new AuthenticationDao();
        }

        /// <summary>
        /// returns the AppSettings DAO
        /// </summary>
        /// <returns></returns>
        public IAppSettingsDao getAppSettingsDao()
        {
            return new AppSettingsDao();
        }

        /// <summary>
        /// returns the RightsMap DAO
        /// </summary>
        /// <returns></returns>
        public IRightsMapDao getRightsMapDao()
        {
            return new RightsMapDao();
        }

        /// <summary>
        /// returns the dictionary list dao
        /// </summary>
        /// <returns></returns>
        public IDictionaryListsDao getDictionaryListsDao()
        {
            return new CachedDictionaryListsDao(new DictionaryListsDao());
        }
    }
}