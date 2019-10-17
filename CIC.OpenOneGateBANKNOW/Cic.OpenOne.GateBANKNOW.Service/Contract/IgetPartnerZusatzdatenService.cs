using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Die Schnittstelle getPartnerZusatzdatenService definiert, dass eine Klasse Zusatzdaten eines Partners bereitstellen können muss.
    /// </summary>
    [ServiceContract(Name = "IgetPartnerZusatzdatenService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IgetPartnerZusatzdatenService
    {
        /// <summary>
        /// Liefert Daten des Händlerprofils
        /// </summary>
        /// <returns>ogetProfilDto</returns>
        [OperationContract]
        ogetProfilDto getProfil();

        /// <summary>
        /// Liefert Daten über den Key Account Manager des Händlers
        /// </summary>
        /// <returns>ogetKamDto</returns>
        [OperationContract]
        ogetKamDto getKam();

        /// <summary>
        /// Liefert Daten über den Abwicklungsort des Händlers
        /// </summary>
        /// <returns>ogetAbwicklungsortDto</returns>
        [OperationContract]
        ogetAbwicklungsortDto getAbwicklungsort();

        /// <returns></returns>
        /// <summary>
        /// Liefert eine Liste der verfügbaren News im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableNewsDto</param>
        /// <returns>olistAvailableNewsDto</returns>
        [OperationContract]
        olistAvailableNewsDto listAvailableNews(ilistAvailableNewsDto input);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Alerts im Kontext
        /// </summary>
        /// <param name="isoCode">isoCode</param>
        /// <returns>olistAvailableAlertsDto</returns>
        [OperationContract]
        olistAvailableAlertsDto listAvailableAlerts(string isoCode);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Kanäle im Kontext
        /// </summary>
        /// <returns>olistAvailableChannelsDto</returns>
        [OperationContract]
        olistAvailableChannelsDto listAvailableChannels();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Brands (Bildwelten) im Kontext
        /// </summary>
        /// <returns>olistAvailableBrandsDto</returns>
        [OperationContract]
        olistAvailableBrandsDto listAvailableBrands();

        /// <summary>
        /// Liefert alle User zurück die zum Händler gehören 
        /// </summary>
        /// <returns>Liste aller User</returns>
        [OperationContract]
        olistAvailableUserDto listAvailableUser();


        /// <summary>
        /// Liefert alle User zurück die zum Händler gehören
        /// </summary>
        /// <returns>Liste aller User</returns>
        [OperationContract]
        olistAvailableUserDto listAvailableUserWithStatus(PeroleActiveStatus activeStatus);

        /// <summary>
        /// Markiert die Alerts der übergebenen Antraege als gelesen
        /// </summary>
        /// <param name="input">ideleteAvailableAlertsDto</param>
        /// <returns>Liste der verfügbaren Alerts im Kontext</returns>
        [OperationContract]
        olistAvailableAlertsDto deleteAvailableAlerts(ideleteAvailableAlertsDto input);

        /// <summary>
        /// get all insurance statistical info
        /// </summary>
        /// <returns>all quotes</returns>
        [OperationContract]
        olistQuoteInfoDto listAvailableQuotes();
    }
}