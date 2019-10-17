using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// defines interface for business objects getting lists that are role context dependent
    /// </summary>
    public interface IRoleContextListsBo
    {
        /// <summary>
        /// get a list of available alerts for the actual user
        /// </summary>
        /// <returns>olistAvailableAlertsDto</returns>
        AvailableAlertsDto[] listAvailableAlerts(string isocode, long sysperole);

        /// <summary>
        /// get a list of available brands for the actual user
        /// </summary>
        /// <returns>olistAvailableBrandsDto</returns>
        DropListDto[] listAvailableBrands(long sysVpPerole);

        /// <summary>
        /// get a list of available channels for the actual user
        /// </summary>
        /// <returns>olistAvailableChannelsDto</returns>
        DropListDto[] listAvailableChannels(long sysVpPerole, string isoCode);


        /// <summary>
        /// get a list of available kundentypen for the actual user
        /// </summary>
        /// <returns></returns>
        DropListDto[] listAvailableKundentypen(string isoCode);


        /// <summary>
        /// get a list of available nutzungsarten for the actual user
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        DropListDto[] listAvailableNutzungsarten(string isoCode);

        /// <summary>
        /// get a list of available objektarten for the actual user
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        DropListDto[] listAvailableObjektarten(string isoCode);

        /// <summary>
        /// get a list of available objekttypen for the actual user
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        DropListDto[] listAvailableObjekttypen(string isoCode);

        /// <summary>
        /// Alarmmeldungen als gelesen markieren
        /// </summary>
        /// <param name="antrag">Antrag ID</param>
        /// <param name="userid">benutzer ID</param>
        void setAlertsAsReaded(long antrag, long userid);



        
    }
}