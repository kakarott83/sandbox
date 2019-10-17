using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    [ServiceContract(Name = "Ib2xService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface Ib2xService
    {
        /// <summary>
        /// Data upload/assignment a certain AREA
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateExtraValueDto createOrUpdateExtraValue(icreateExtraValueDto input);

        /// <summary>
        /// Document upload Service for a certain AREA
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateUploadDto createOrUpdateUpload(icreateUploadDto input);

        /// <summary>
        /// KREMO Budgetcalculation
        /// </summary>
        /// <param name="input"></param>
        /// <returns>budget</returns>
        [OperationContract]
        okremoGetBudget kremoGetBudget(ikremoGetBudgetDto input);

        /// <summary>
        /// Delivers the perole for the given username
        /// username is puser.externeid
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns>SYSPEROLE</returns>
        [OperationContract]
        ogetDealerDto getDealerID(String userid, String password);

        /// <summary>
        /// Delivers the object information from a eurotax id
        /// </summary>
        /// <param name="id">Schluessel</param>
        /// <returns>Fahrzeugspezifische Daten aus Eurotax</returns>
        [OperationContract]
        oGetObjektDatenDto getObjektDaten(String id);

        /// <summary>
        /// Delivers a list of all products and its parameters for the given context
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        olistAvailableProduktInfoDto listAvailableProduktInfo(ilistAvailableProduktInfoDto input);


        /// <summary>
        /// validates the calculation and returns result messages
        /// </summary>
        /// <param name="input">icheckAngebotDto</param>
        /// <returns>ocheckAngebotDto</returns>
        [OperationContract]
        Service.DTO.ocheckAntAngDto checkKalkulation(icheckKalkulationDto input);

        /// <summary>
        /// solves the calculation and delivers the results
        /// </summary>
        /// <param name="input">isolveKalkulationDto</param>
        /// <returns>osolveKalkulationDto</returns>
        [OperationContract]
        osolveKalkulationDto solveKalkulation(isolveKalkulationDto input);

        /// <summary>
        /// delivers a list of available services for the given context
        /// </summary>
        /// <param name="input">ilistAvailableServicesDto</param>
        /// <returns>olistAvailableServicesDto</returns>
        [OperationContract]
        olistAvailableServicesDto listAvailableServices(ilistAvailableServicesDto input);

        /// <summary>
        /// creates or updates the offer in the database
        /// </summary>
        /// <param name="input">icreateAngebotDto</param>
        /// <returns>ocreateAngebotDto</returns>
        [OperationContract]
        ocreateAngebotDto createOrUpdateAngebot(icreateAngebotDto input);


        /// <summary>
        /// delivers a list of available documents for printing
        /// </summary>
        /// <param name="input">ilistAvailableDokumenteDto</param>
        /// <returns>olistAvailableDokumenteDto</returns>
        [OperationContract]
        olistAvailableDokumenteDto listAvailableDokumente(ilistAvailableDokumenteDto input);

        /// <summary>
        /// creates a print-job for the given documents
        /// </summary>
        /// <param name="input">iprintCheckedDokumenteDto</param>
        /// <returns>oprintCheckedDokumenteDto</returns>
        [OperationContract]
        oprintCheckedDokumenteDto printCheckedDokumente(iprintCheckedDokumenteDto input);

        /// <summary>
        /// creates/updates the proposal
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateAntragDto createOrUpdateAntrag(icreateAntragDto input);

                /// <summary>
        /// returns all GUI-Field Translations
        /// by this query
        /// select ctfoid.frontid,typ,verbaldescription MASTER, replaceterm TRANSLATION,replaceblob,isocode 
        /// FROM ctfoid,cttfoid,ctlang where ctfoid.frontid=cttfoid.frontid and ctlang.sysctlang=cttfoid.sysctlang and flagtranslate=1 order by ctfoid.frontid;
        /// </summary>
        /// <returns>oTranslationDto</returns>
        [OperationContract]
        oTranslationDto getTranslations();

        /// <summary>
        /// Service Information liefern
        /// </summary>
        /// <returns>Service Informationsdaten</returns>
        [OperationContract]
        ServiceInformation DeliverServiceInformation();

        /// <summary>
        /// get all quotes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        olistQuoteInfoDto getQuotes();

    }
}
