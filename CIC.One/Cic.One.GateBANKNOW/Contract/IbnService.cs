using Cic.One.DTO;
using Cic.One.DTO.BN;
using Cic.One.Utils.Util.Behaviour;
using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Cic.One.GateBANKNOW.Contract
{
    /// <summary>
    /// Methods for BankNow Gateway
    /// </summary>
    [ServiceContract(Name = "IbnService", Namespace = "http://cic-software.de/One")]
    [WsdlDocumentationAttribute("BNOW Gateway Service")]
    public interface IbnService
    {

        /// <summary>
        /// Creates a Antrag from NKK
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateAntragFromNkkDto createAntragFromNkk(icreateAntragFromNkkDto input);

        /// <summary>
        /// createOrUpdateeAngebot
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateBNAngebotDto createOrUpdateBNAngebot(icreateOrUpdateBNAngebotDto input);

        /// <summary>
        /// delivers a list of Zek data
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchZekDto searchZek(iSearchDto iSearch, ZekDto request);

        /// <summary>
        /// createOrUpdateBNKundeIdentifikation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateBNKundenIdentifikationDto createOrUpdateBNKundenIdentifikation(icreateOrUpdateBNKundenIdentifikationDto input);


        /// <summary>
        /// updateLegitimationsmethode
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oupdateLegMethodeDto updateLegitimationsmethode(iupdateLegMethodeDto legMethodeInput);
        
        /// <summary>
        /// Liefert eine Liste der verfügbaren Services im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableServicesDto</param>
        /// <returns>olistAvailableServicesDto</returns>
        [OperationContract]
        olistAvailableServicesDto listAvailableServices(ilistAvailableServicesDto input);

        /// <summary>
        /// Delivers the Customer Information
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        [WsdlDocumentationAttribute("Delivers Customer Detail")]
        ogetBNKundeDetailDto getBNKundeDetail(igetBNKundeDetailDto input);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Produkte im Kontext
        /// </summary>
        /// <param name="input">Kontext</param>
        /// <returns>verfügbare Produkte</returns>
        [OperationContract]
        olistAvailableProductsDto listAvailableProducts(ilistAvailableProductsDto input);

        /// <returns></returns>
        /// <summary>
        /// Liefert eine Liste der verfügbaren News im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableNewsDto</param>
        /// <returns>olistAvailableNewsDto</returns>
        [OperationContract]
        olistAvailableNewsDto listAvailableNews(ilistAvailableNewsDto input);

        /// <summary>
        /// Bank-Now Ratenkalkulator
        /// löscht die aktuelle Kalkulation auf und liefert die Berechnungsergebnisse
        /// </summary>
        /// <param name="input">isolveKalkulationDto</param>
        /// <returns>osolveKalkulationDto</returns>
        [OperationContract]
        Cic.One.DTO.BN.osolveBNKalkulationDto solveBNKalkulation(Cic.One.DTO.BN.isolveBNKalkulationDto input);

        /// <summary>
        /// Prüft die Kalkulation
        /// </summary>
        /// <param name="input">icheckAngebotDto</param>
        /// <returns>ocheckAngebotDto</returns>
        [OperationContract]
        ocheckAntAngDto checkKalkulation(icheckKalkulationDto inp);

        /// <summary>
        /// update abwicklungsort for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        [OperationContract]
        oupdateAbwicklungsortDto updateAbwicklungsort(iupdateAbwicklungsortDto input);

        /// <summary>
        /// get abwicklungsort details for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        [OperationContract]
        ogetAnciliaryDetailDto getAnciliaryDetail(igetAnciliaryDetailDto input);

        /// <summary>
        /// update smstext for ANTRAG
        /// </summary>
        /// <param name="input"></param>
        [OperationContract]
        oupdateSMSTextDto updateSMSText(iupdateSMSTextDto input);

        /// <summary>
        /// accept EPOS Conditions of current user
        /// </summary>
        [OperationContract]
        oacceptEPOSConditionsDto acceptEPOSConditions();

        /// <summary>
        /// returns ewk/bwk/ga details
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAuskunftDetailDto getAuskunftDetail(igetAuskunftDetailDto inp);

        /// <summary>
        /// send Risk Email-Eaihot
        /// </summary>
        /// <param name="riskmail"></param>
        /// <returns></returns>
        [OperationContract]
        osendRiskmailDto sendRiskmail(isendRiskmailDto riskmail);
    }


   
}
