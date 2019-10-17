using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Business object for getting lists that are role context dependent
    /// </summary>
    public class RoleContextListsBo : AbstractRoleContextListsBo
    {
        TranslateBo translatorBo = null;

        /// <summary>
        /// contructs a roleContextLists business object
        /// </summary>
        /// <param name="roleContextListsDao">the data access object to use</param>
        /// <param name="TranslateDao">Übersetzungs DAO</param>
        public RoleContextListsBo(IRoleContextListsDao roleContextListsDao, ITranslateDao TranslateDao)
            : base(roleContextListsDao, TranslateDao)
        {
            translatorBo = new TranslateBo(TranslateDao);
        }

        /// <summary>
        /// get a list of available alters for this user
        /// </summary>
        /// <returns>olistAvailableAlerts</returns>
        public override AvailableAlertsDto[] listAvailableAlerts(string isoCode, long sysperole)
        {
            return roleContextListsDao.listAvailableAlerts(isoCode, sysperole);
        }


        /// <summary>
        /// get a list of available brands for this user
        /// </summary>
        /// <returns>olistAvailableBrandsDto</returns>
        public override DropListDto[] listAvailableBrands(long sysPEROLE)
        {
            return roleContextListsDao.getBrands(sysPEROLE);
        }

        /// <summary>
        /// get a list of available channels for this user
        /// </summary>
        /// <returns>olistAvailableChannelsDto</returns>
        public override DropListDto[] listAvailableChannels(long sysPEROLE, string isoCode)
        {
            DropListDto[] List =  roleContextListsDao.getChannels(sysPEROLE);

            return translatorBo.TranslateList(List, TranslateArea.BCHANNEL, isoCode);
        }

        /// <summary>
        /// get a list of available kundentypen for this user
        /// </summary>
        /// <returns>olistAvailableKundentypenDto</returns>
        /// <remarks>Noch ohne Rollenkontext laut Konzept b2b_steuerung_1_6.docx</remarks>
        public override DropListDto[] listAvailableKundentypen(string isoCode)
        {
            DropListDto[] List = roleContextListsDao.getKundentypen();

            return translatorBo.TranslateList(List, TranslateArea.KDTYP, isoCode);
        }

        /// <summary>
        /// get a list of available nutzungsarten for the actual user
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        public override DropListDto[] listAvailableNutzungsarten(string isoCode)
        {
            DropListDto[] List = roleContextListsDao.getNutzungsarten();

            return translatorBo.TranslateList(List, TranslateArea.OBUSETYPE, isoCode);
        }

        /// <summary>
        /// get a list of available objektarten for the actual user
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        public override DropListDto[] listAvailableObjektarten(string isoCode)
        {
            DropListDto[] List = roleContextListsDao.getObjektarten();

            return translatorBo.TranslateList(List, TranslateArea.OBART, isoCode);
        }

        /// <summary>
        /// get a list of available objekttypen for the actual user
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        public override DropListDto[] listAvailableObjekttypen(string isoCode)
        {
             DropListDto[] List = roleContextListsDao.getObjekttypen(false);
             return translatorBo.TranslateList(List, TranslateArea.OBTYP, isoCode);
        }

        /// <summary>
        /// get a list of available objekttypen for the actual user
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        public DropListDto[] listAllAvailableObjekttypen(string isoCode)
        {
            DropListDto[] List = roleContextListsDao.getObjekttypen(true);
            return translatorBo.TranslateList(List, TranslateArea.OBTYP, isoCode);
        }

        /// <summary>
        /// get a list of available objekttypen for the actual user
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        public override DropListDto[] listAvailableObjekttypen(string isoCode, long sysPEROLE) {
            DropListDto[] List = roleContextListsDao.getObjekttypen(false,sysPEROLE);
            return translatorBo.TranslateList(List, TranslateArea.OBTYP, isoCode);
        }

        /// <summary>
        /// get a list of available objekttypen for the actual user
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        public override DropListDto[] listAllAvailableObjekttypen(string isoCode, long sysPEROLE) {
            DropListDto[] List = roleContextListsDao.getObjekttypen(true, sysPEROLE);
            return translatorBo.TranslateList(List, TranslateArea.OBTYP, isoCode);
        }



        /// <summary>
        /// Alarmmeldungen als gelesen markieren
        /// </summary>
        /// <param name="antrag">Antrag ID</param>
        /// <param name="userid">Benutzer ID</param>
        public override void setAlertsAsReaded(long antrag, long userid)
        {
            roleContextListsDao.setAlertsAsReaded(antrag, userid);
        }
    }
}