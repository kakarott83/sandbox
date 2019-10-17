using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// defines the interface for data access objects getting lists that are role context dependent
    /// </summary>
    public interface IRoleContextListsDao
    {
        /// <summary>
        /// get the available Brands
        /// </summary>
        /// <param name="sysPEROLE">User Perole Id</param>
        /// <returns>Array of Brands</returns>
        DropListDto[] getBrands(long sysPEROLE);

        /// <summary>
        /// get the available Channels
        /// </summary>
        /// <param name="sysPEROLE">Role of the user</param>
        /// <returns>Array of Channels</returns>
        DropListDto[] getChannels(long sysPEROLE);

        /// <summary>
        /// get the available Kundentypen
        /// </summary>
        /// <returns>Array of Kundentypen</returns>
        DropListDto[] getKundentypen();

        /// <summary>
        /// get the available nutzungsarten
        /// </summary>
        /// <returns>Array of Nutzungsarten</returns>
        DropListDto[] getNutzungsarten();

        /// <summary>
        ///  get the available objektarten
        /// </summary>
        /// <returns>Array of Objektarten</returns>
        DropListDto[] getObjektarten();

        /// <summary>
        /// get the available objekttypen
        /// </summary>
        /// <returns>Array of Objekttypen</returns>
        DropListDto[] getObjekttypen(bool withOthers);

        /// <summary>
        /// get the available objekttypen
        /// </summary>
        /// <returns>Array of Objekttypen</returns>
        DropListDto[] getObjekttypen(bool withOthers, long sysPEROLE);

        /// <summary>
        /// Verfügbare Alarmmeldungen auflisten
        /// </summary>
        /// <returns>Alarmmeldungsliste</returns>
        AvailableAlertsDto[] listAvailableAlerts(string isoCode, long sysperole);

        /// <summary>
        /// Alarmmeldungen als gelesen markieren
        /// </summary>
        /// <param name="antrag">Antrag ID</param>
        /// <param name="userid">Benutzer ID</param>
        void setAlertsAsReaded(long antrag, long userid);

    }
}