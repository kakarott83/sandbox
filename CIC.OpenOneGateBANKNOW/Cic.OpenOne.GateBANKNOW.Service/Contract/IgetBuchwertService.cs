using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface getBuchwertService stellt die Methoden zur Lieferung des aktuellen Buchwertes eines Vertrages bereit.
    /// </summary>
    [ServiceContract(Name = "IgetBuchwertService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IgetBuchwertService
    {
        /// <summary>
        /// Liefert den aktuellen Buchwert des Vertrags
        /// </summary>
        /// <param name="input">igetBuchwertDto</param>
        /// <returns>ogetBuchwertDto</returns>
        [OperationContract]
        ogetBuchwertDto getBuchwert(igetBuchwertDto input);

        /// <summary>
        /// Eurotax GetForecast Auskunft einholen
        /// </summary>
        /// <param name="input">iEurotaxForecastDto</param>
        /// <returns>oEurotaxForecastDto</returns>     
        [OperationContract]
        oEurotaxGetForecastDto EurotaxGetForecast(iEurotaxGetForecastDto input);


        /// <summary>
        /// 2.3.2.1	Anzeige Button für indikativen Buchwertberechnung
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        [OperationContract]
        bool isBuchwertCalculationAllowed(long sysid);

        /// <summary>
        /// Löst eine Kaufofferte aus
        /// </summary>
        /// <param name="input"></param>
        [OperationContract]
        operformKaufofferte performKaufofferte(iperformKaufofferteDto input);

        /// <summary>
        /// Ist Kaufofferte bestellen erlaubt
        /// </summary>
        /// <param name="input"></param>
        [OperationContract]
        oisAllowed isPerformKaufofferteAllowed(iisAllowed input);

        /// <summary>
        ///  2.3.2.1	Anzeige Button für indikativen Buchwertberechnung
        /// </summary>
        /// <param name="input"></param>
        [OperationContract]
        oisAllowed isBuchwertCalculationAllowedNew(iisAllowed input);

        /// <summary>
        /// Liefert eine Liste für den angegebenen Listencode
        /// </summary>
        /// <returns>ogetListItemsDto</returns>
        [OperationContract]
        ogetListItemsDto getListItems(igetListItems input);
    }
}
