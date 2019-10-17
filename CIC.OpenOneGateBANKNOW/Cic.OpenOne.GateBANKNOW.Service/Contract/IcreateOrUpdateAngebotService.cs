
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface createOrUpdateAngebotService stellt die Methoden zur Lieferung verschiedener Listen sowie Update und Speichermöglichkeiten und die Einreichung des Angebots zum Antrag bereit.
    /// </summary>
    /// <remarks>Die Methode callKonfigurator ist im b2b_steuerung_1_2.pdf nicht aufgelistet.</remarks>
    [ServiceContract(Name = "IcreateOrUpdateAngebotService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IcreateOrUpdateAngebotService
    {

        /// <summary>
        /// Delivers the object information from a car configurator id
        /// </summary>
        /// <param name="key">Schluessel</param>
        /// <returns>Fahrzeugspezifische Daten aus Eurotax</returns>
        [OperationContract]
        oGetObjektDatenDto getObjektDaten(String key);

        /// <summary>
        /// Delivers the object information from a car VIN-Number
        /// </summary>
        /// <param name="key">VIN Number</param>
        /// <returns>Fahrzeugspezifische Daten aus Eurotax map2ETG</returns>
        [OperationContract]
        oGetObjektDatenDto getObjektDatenByVIN(String key);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Objekttypen im Kontext
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        [OperationContract]
        olistAvailableObjekttypenDto listAvailableObjekttypen();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Objektarten im Kontext
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        [OperationContract]
        olistAvailableObjektartenDto listAvailableObjektarten();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Produkte im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableProdukteDto</param>
        /// <returns>olistAvailableProdukteDto</returns>
        [OperationContract]
        olistAvailableProdukteDto listAvailableProdukte(ilistAvailableProdukteDto input);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Kundentypen im Kontext
        /// </summary>
        /// <returns>olistAvailableKundentypenDto</returns>
        [OperationContract]
        olistAvailableKundentypenDto listAvailableKundentypen();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Nutzungsarten im Kontext
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        [OperationContract]
        olistAvailableNutzungsartenDto listAvailableNutzungsarten();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Nutzungsarten Privat im Kontext
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        [OperationContract]
        DTO.olistAvailableNutzungsartenDto listAvailableNutzungsartenPrivat();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Nutzungsarten für Firma im Kontext
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        [OperationContract]
        DTO.olistAvailableNutzungsartenDto listAvailableNutzungsartenFirma();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Services im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableServicesDto</param>
        /// <returns>olistAvailableServicesDto</returns>
        [OperationContract]
        olistAvailableServicesDto listAvailableServices(ilistAvailableServicesDto input);

        /// <summary>
        /// Liefert eine Liste aller Parameter und Eckwerte des Produkts im Kontext
        /// </summary>
        /// <param name="input">igetParameterDto</param>
        /// <returns>ogetParameterDto</returns>
        [OperationContract]
        ogetParameterDto getParameter(igetParameterDto input);

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt der Angebotsvariante
        /// </summary>
        /// <param name="input">icreateKalkulationDto</param>
        /// <returns>ocreateKalkulationDto</returns>
        [OperationContract]
        ocreateKalkulationDto createOrUpdateKalkulation(icreateKalkulationDto input);

        /// <summary>
        /// Kopiert Persistenzobjekt der Angebotsvariante
        /// </summary>
        /// <param name="input">icopyKalkulationDto</param>
        /// <returns>ocopyKalkulationDto</returns>
        [OperationContract]
        ocopyKalkulationDto copyKalkulation(icopyKalkulationDto input);

        /// <summary>
        /// Löscht Persistenzobjekt der Angebotsvariante
        /// </summary>
        /// <param name="input">ideleteKalkulationDto</param>
        /// <returns>odeleteKalkulationDto</returns>
        [OperationContract]
        odeleteKalkulationDto deleteKalkulation(ideleteKalkulationDto input);

        /// <summary>
        /// Löst die aktuelle Kalkulation auf und liefert die Berechnungsergebnisse
        /// </summary>
        /// <param name="input">isolveKalkulationDto</param>
        /// <returns>osolveKalkulationDto</returns>
        [OperationContract]
        osolveKalkulationDto solveKalkulation(isolveKalkulationDto input);

        /// <summary>
        /// Ermittelt die Incentivierungsprovision
        /// </summary>
        /// <param name="input">igetVTProvDto</param>
        /// <returns>ogetVTProvDto</returns>
        [OperationContract]
        ogetVTProvDto getVTProv(igetVTProvDto input);

        /// <summary>
        /// Zeigt den Status im aktuellen Bonusvergabe-Programm (Stufe Bronze/Silber/...)
        /// </summary>
        /// <param name="input">igetMyPocketDataDto</param>
        /// <returns>ogetMyPocketDataDto</returns>
        [OperationContract]
        ogetMyPocketDataDto getMyPocketData(igetMyPocketDataDto input);

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt des Angebots
        /// </summary>
        /// <param name="input">icreateAngebotDto</param>
        /// <returns>ocreateAngebotDto</returns>
        [OperationContract]
        ocreateAngebotDto createOrUpdateAngebot(icreateAngebotDto input);

        /// <summary>
        /// Speichert Persistenzobjekte des Angebots und aller Angebotsvarianten
        /// </summary>
        /// <param name="input">isaveAngebotDto</param>
        /// <returns>osaveAngebotDto</returns>
        [OperationContract]
        osaveAngebotDto saveAngebot(isaveAngebotDto input);

        /// <summary>
        /// Prüft die Kalkulation
        /// </summary>
        /// <param name="input">icheckKalkulationDto</param>
        /// <returns>ocheckAntAngDto</returns>
        [OperationContract]
        Service.DTO.ocheckAntAngDto checkKalkulation(icheckKalkulationDto input);

            
        /// <summary>
        /// Übernimmt Angebot in Antrag
        /// </summary>
        /// <param name="input">iprocessAngebotToAntragDto</param>
        /// <returns>oprocessAngebotToAntragDto</returns>
        [OperationContract]
        oprocessAngebotToAntragDto processAngebotToAntrag(iprocessAngebotToAntragDto input);

        /// <summary>
        /// Übernimmt Angebot in Antrag
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>oprocessAngebotToAntragDto</returns>
        [OperationContract]
        oprocessAngebotToAntragByIdDto processAngebotToAntragById(long sysid);

        /// <summary>
        /// preisschildDruck
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        [OperationContract]
        oPreisschildDruckDto preisschildDruck(iPreisschildDruckDto input);


        /// <summary>
        /// Attaches the given disclaimer to the given area/id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateDisclaimerDto createDisclaimer(icreateDisclaimerDto input);
    }
}
