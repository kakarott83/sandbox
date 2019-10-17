using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// abstract business object getting lists that are role context dependent
    /// </summary>
    public abstract class AbstractRoleContextListsBo : IRoleContextListsBo
    {
        /// <summary>
        /// the data access object to use
        /// </summary>
        protected IRoleContextListsDao roleContextListsDao;

        /// <summary>
        /// the data access object to use
        /// </summary>
        protected ITranslateDao TranslateDao;

        /// <summary>
        /// constructs a abstractRoleContextLists business object
        /// </summary>
        /// <param name="roleContextListsDao">the data access object to use</param>
        /// <param name="TranslateDao">Übersetzungs DAO</param>
        public AbstractRoleContextListsBo(IRoleContextListsDao roleContextListsDao, ITranslateDao TranslateDao)
        {
            this.roleContextListsDao = roleContextListsDao;
            this.TranslateDao = TranslateDao;
        }

        /// <summary>
        /// get a list of available alerts for the current user
        /// </summary>
        /// <returns>olistAvailableAlertsDto</returns>
        public abstract AvailableAlertsDto[] listAvailableAlerts(string isoCode, long sysperole);

        /// <summary>
        /// get a list of available brand for the current user 
        /// </summary>
        /// <returns>olistAvailableBrandsDto</returns>
        public abstract DropListDto[] listAvailableBrands(long sysVpPerole);

        /// <summary>
        /// get a list of available dokumente for the current user
        /// </summary>
        /// <returns>olistAvailableChannelsDto</returns>
        public abstract DropListDto[] listAvailableChannels(long sysVpPerole, string isoCode);

        /// <summary>
        /// get a list of available kundentypen for the current user
        /// </summary>
        /// <returns>olistAvailableKundentypenDto</returns>
        public abstract DropListDto[] listAvailableKundentypen(string isoCode);


        /// <summary>
        /// get a list of available nutzungsarten for the current user
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        public abstract DropListDto[] listAvailableNutzungsarten(string isoCode);

        /// <summary>
        /// get a list of available objektarten for the current user
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        public abstract DropListDto[] listAvailableObjektarten(string isoCode);

        /// <summary>
        /// get a list of available objektarten for the current user with Sort 
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        public abstract DropListDto[] listAvailableObjekttypen(string isoCode, long sysPEROLE);

        /// <summary>
        /// get a list of available objektarten for the current user with Sort 
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        public abstract DropListDto[] listAllAvailableObjekttypen(string isoCode, long sysPEROLE);

        /// <summary>
        /// get a list of available objekttypen for the current user
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        public abstract DropListDto[] listAvailableObjekttypen(string isoCode);

        /// <summary>
        /// Alarmmeldungen als gelesen markieren
        /// </summary>
        /// <param name="antrag">Antrag ID</param>
        /// <param name="userid">User ID</param>
        public abstract void setAlertsAsReaded(long antrag, long userid);
    }
}