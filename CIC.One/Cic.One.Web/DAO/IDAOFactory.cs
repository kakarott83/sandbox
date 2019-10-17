using Cic.OpenOne.Common.DAO;
using System;

namespace Cic.One.Web.DAO
{
    /// <summary>
    /// Factory Interface for all DAO's of the system
    /// </summary>
    public interface IDAOFactory
    {
        /// <summary>
        /// returns the Entity DAO
        /// </summary>
        /// <returns></returns>
        IEntityDao getEntityDao();

        /// <summary>
        /// returns the Workflow DAO
        /// </summary>
        /// <returns></returns>
        Cic.One.DTO.IWFVDao getWorkflowDao();

        /// <summary>
        /// returns the Recurr DAO
        /// </summary>
        /// <returns></returns>
        IRecurrDao getRecurrDao();

        /// <summary>
        /// returns the Search DAO
        /// </summary>
        /// <returns></returns>
        ISearchDao<R> getSearchDao<R>();


        /// <summary>
        /// returns the DocumentSearchDao
        /// </summary>
        /// <returns></returns>
        IDocumentSearchDao getDocumentSearchDao();

        /// <summary>
        /// returns the XproLoaderDao
        /// </summary>
        /// <returns></returns>
        IXproLoaderDao getXproLoaderDao();

        /// <summary>
        /// returns the TADocumentSearchDao
        /// </summary>
        /// <returns></returns>
        ITADocumentSearchDao getTADocumentSearchDao();

        /// <summary>
        /// returns the Authentication Dao
        /// </summary>
        /// <returns></returns>
        IAuthenticationDao getAuthenticationDao();

        /// <summary>
        /// returns the AppSettings DAO
        /// </summary>
        /// <returns></returns>
        IAppSettingsDao getAppSettingsDao();

        /// <summary>
        /// returns the RightsMap DAO
        /// </summary>
        /// <returns></returns>
        IRightsMapDao getRightsMapDao();

        /// <summary>
        /// returns the dictionary list dao
        /// </summary>
        /// <returns></returns>
        IDictionaryListsDao getDictionaryListsDao();
    }
}
