using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface createOrUpdateAntragService stellt die die Methoden zur Lieferung verschiedener Listen sowie Editier und Speicher Methoden und die Einreichung bereit.
    /// </summary>
    [ServiceContract(Name = "IcreateOrUpdateAntragService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IcreateOrUpdateAntragService
    {

        /// <summary>
        /// Delivers the object information from a car configurator id
        /// </summary>
        /// <param name="key">Schluessel</param>
        /// <returns>Fahrzeugspezifische Daten aus Eurotax</returns>
        [OperationContract]
        DTO.oGetObjektDatenDto getObjektDaten(String key);

        /// <summary>
        /// Delivers the object information from a car VIN-Number
        /// </summary>
        /// <param name="key">VIN Number</param>
        /// <returns>Fahrzeugspezifische Daten aus Eurotax map2ETG</returns>
        [OperationContract]
        DTO.oGetObjektDatenDto getObjektDatenByVIN(String key);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Objekttypen im Kontext
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        [OperationContract]
        DTO.olistAvailableObjekttypenDto listAvailableObjekttypen();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Objektarten im Kontext
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        [OperationContract]
        DTO.olistAvailableObjektartenDto listAvailableObjektarten();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Produkte im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableProdukteDto</param>
        /// <returns>olistAvailableProdukteDto</returns>
        [OperationContract]
        DTO.olistAvailableProdukteDto listAvailableProdukte(DTO.ilistAvailableProdukteDto input);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Kundentypen im Kontext
        /// </summary>
        /// <returns>olistAvailableKundentypenDto</returns>
        [OperationContract]
        DTO.olistAvailableKundentypenDto listAvailableKundentypen();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Nutzungsarten im Kontext
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        [OperationContract]
        DTO.olistAvailableNutzungsartenDto listAvailableNutzungsarten();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Nutzungsarten Privat im Kontext
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        [OperationContract]
        DTO.olistAvailableNutzungsartenDto listAvailableNutzungsartenPrivat();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Nutzungsarten Firma im Kontext
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
        DTO.olistAvailableServicesDto listAvailableServices(DTO.ilistAvailableServicesDto input);

        /// <summary>
        /// Liefert eine Liste aller Parameter und Eckwerte des Produkts im Kontext
        /// </summary>
        /// <param name="input">igetParameterDto</param>
        /// <returns>ogetParameterDto</returns>
        [OperationContract]
        DTO.ogetParameterDto getParameter(DTO.igetParameterDto input);

        /// <summary>
        /// Löst die aktuelle Kalkulation auf und liefert die Berechnungsergebnisse
        /// </summary>
        /// <param name="input">isolveKalkulationDto</param>
        /// <returns>osolveKalkulationDto</returns>
        [OperationContract]
        DTO.osolveKalkulationDto solveKalkulation(DTO.isolveKalkulationDto input);

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt des Antrags
        /// </summary>
        /// <param name="input">icreateAntragDto</param>
        /// <returns>ocreateAntragDto</returns>
        [OperationContract]
        DTO.ocreateAntragDto createOrUpdateAntrag(DTO.icreateAntragDto input);

        /// <summary>
        /// Speichert Persistenzobjekt des Antrags
        /// </summary>
        /// <param name="input">isaveAntragDto</param>
        /// <returns>osaveAntragDto</returns>
        [OperationContract]
        DTO.osaveAntragDto saveAntrag(DTO.isaveAntragDto input);

        /// <summary>
        /// Prüft den Antrag
        /// </summary>
        /// <param name="input">icheckAntragDto</param>
        /// <returns>ocheckAntragDto</returns>
        [OperationContract]
        DTO.ocheckAntAngDto checkAntrag(DTO.icheckAntragDto input);

        /// <summary>
        /// Prüft den Antrag Flag
        /// </summary>
        /// <param name="sysid">id des Antrags</param>
        /// <param name="sysvart">id der Vertragsart</param>
        /// <param name="nurallgemeine">nur allgemeine Prüfung</param>
        /// <returns>Status der Antragsprüfung (rot, grün, gelb) und/oder Fehler</returns>
        [OperationContract]
        DTO.ocheckAntAngDto checkAntragByIdFlag(long sysid, long sysvart, bool nurallgemeine);

        /// <summary>
        /// Prüft den Antrag
        /// </summary>
        /// <param name="sysid">id des Antrags</param>
        /// <param name="sysvart">id der Vertragsart</param>
        /// <returns>Status der Antragsprüfung (rot, grün, gelb) und/oder Fehler</returns>
        [OperationContract]
        DTO.ocheckAntAngDto checkAntragById(long sysid, long sysvart);

        /// <summary>
        /// Prüft den Antrag Flag
        /// </summary>
        /// <param name="sysid">id des Antrags</param>
        /// <param name="sysvart">id der Vertragsart</param>
        /// <param name="nurallgemeine">nur allgemeine Prüfung</param>
        /// /// <param name="sysprproduct">id der prproduct</param>
        /// <returns>Status der Antragsprüfung (rot, grün, gelb) und/oder Fehler</returns>
        [OperationContract]
        DTO.ocheckAntAngDto checkAntragByIdFlag2(long sysid, long sysvart, bool nurallgemeine, long sysprproduct);

        /// <summary>
        /// Prüft den Antrag
        /// </summary>
        /// <param name="sysid">id des Antrags</param>
        /// <param name="sysvart">id der Vertragsart</param>
        /// <param name="sysprproduct">id der Prproduct</param>
        /// <returns>Status der Antragsprüfung (rot, grün, gelb) und/oder Fehler</returns>
        [OperationContract]
        DTO.ocheckAntAngDto checkAntragById2(long sysid, long sysvart, long sysprproduct);

        /// <summary>
        /// Reicht den Antrag ein, um einen Vertrag zu erzeugen
        /// </summary>
        /// <param name="input">iprocessAntragToVertragDto</param>
        /// <returns>Vertrags-Informationen</returns>
        [OperationContract]
        DTO.oprocessAntragToVertragDto processAntragEinreichung(DTO.iprocessAntragToVertragDto input);

        /// <summary>
        /// Prüft die Kalkulation
        /// </summary>
        /// <param name="input">icheckAngebotDto</param>
        /// <returns>Ergebnis der Kalkulationsprüfung (rot, grün, gelb)</returns>
        [OperationContract]
        Service.DTO.ocheckAntAngDto checkKalkulation(icheckKalkulationDto input);
        
        /// <summary>
        /// Liefert eine Liste aller Lenker
        /// </summary>
        /// <returns>olistAvailableLenkerDto</returns>
        [OperationContract]
        Service.DTO.olistAvailableLenkerDto listAvailableLenker();

        /// <summary>
        /// Liefert eine Liste aller Versichreungen
        /// </summary>
        /// <returns>olistAvailableInsuranceDto</returns>
        [OperationContract]
        Service.DTO.olistAvailableInsuranceDto listAvailableInsurance();

        /// <summary>
        /// Löst eine Änderung des Restwertrechnungsempfängers aus
        /// </summary>
        /// <param name="input"></param>
        [OperationContract]
        ochangeRRReceiverDto changeRRReceiver(ichangeRRReceiverDto input);

        /// <summary>
        /// searches for uploaded files
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        osearchUploadDto searchUpload(isearchUploadDto input);


        
        /// <summary>
        /// createOrUpdateUpload
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateUploadDto createOrUpdateUpload(icreateUploadDto input);
        
        /// <summary>
        /// Liefert ein Upload-Objekt
        /// </summary>
        /// <param name="input">igetUploadDetailDto</param>
        /// <returns>ogetUploadDetailDto</returns>
        [OperationContract]
        ogetUploadDetailDto getUploadDetail(igetUploadDetailDto input);

        /// <summary>
        /// Finanzierungsvorschlag Einreichen
        /// </summary>
        /// <param name="input">ifinVorEinreichungDto</param>
        /// <returns>ofinVorEinreichungDto</returns>
        [OperationContract]
        ofinVorEinreichungDto processFinVorEinreichung(ifinVorEinreichungDto input);

        /// <summary>
        /// CheckAntrag mit TR
        /// </summary>
        /// <param name="input">iautomatischePruefungDto</param>
        /// <returns>oautomatischePruefungDto</returns>
        [OperationContract]
        oautomatischePruefungDto automatischePruefung(iautomatischePruefungDto input);

        /// <summary>
        /// Varianten rechnen
        /// </summary>
        /// <param name="input">isolveKalkVariantenDto</param>
        /// <returns>osolveKalkVariantenDto</returns>
        [OperationContract]
        osolveKalkVariantenDto solveKalkVarianten(isolveKalkVariantenDto input);

        /// <summary>
        /// TransactionRisikoprüfung durchführen
        /// </summary>
        /// <param name="input">icheckTrRiskDto </param>
        /// <returns>ocheckTrRiskDto</returns>
        [OperationContract]
        ocheckTrRiskDto checkTrRisk(Cic.OpenOne.GateBANKNOW.Service.DTO.icheckTrRiskDto input);


        /// <summary>
        /// TransactionRisikoprüfung durchführen
        /// </summary>
        /// <param name="input">icheckTrRiskDto </param>
        /// <returns>ocheckTrRiskDto</returns>
        [OperationContract]
        ocheckTrRiskByIdDto checkTrRiskById(Cic.OpenOne.GateBANKNOW.Service.DTO.icheckTrRiskByIdDto input);

        /// <summary>
        /// Risikoprüfung simulieren
        /// </summary>
        /// <param name="input">irisikoSimDto</param>
        /// <returns>orisikoSimDto</returns>
        [OperationContract]
        orisikoSimDto risikoSim(Cic.OpenOne.GateBANKNOW.Service.DTO.irisikoSimDto input);

        /// <summary>
        /// finanzierungsvariantenDrucken
        /// </summary>
        /// <param name="input">iFinVariantenDruckenDto</param>
        /// <returns>oFinVariantenDruckenDto</returns>
        [OperationContract]
        oFinVariantenDruckenDto finanzierungsvariantenDrucken(iFinVariantenDruckenDto input);

        /// <summary>
        /// Erstellt einen neuen Vertrag, der eine Restwertverlängerung des gegebenen Vertrags darstellt
        /// </summary>
        /// <param name="input">icreateExtendedContract</param>
        /// <returns>ocreateExtendedContract</returns>
        [OperationContract]
        ocreateExtendedContract createExtendedContract(icreateExtendedContract input);

        /// <summary>
        /// Erstellt einen neuen Antrag von einem Nkk,
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateAntragFromNkkDto createAntragFromNkk(icreateAntragFromNkkDto input);


        /// <summary>
        /// Liefert eine Matrix der Produkte(X)/Laufzeiten(Y) für die Kreditlimits im angegebenen Kontext
        /// </summary>
        /// <param name="input">igetCreditLimitsDto</param>
        /// <returns>ogetCreditLimitsDto</returns>
        [OperationContract]
        ogetCreditLimitsDto getCreditLimits(DTO.igetCreditLimitsDto input);

        /// <summary>
        /// Fetches the CRIF Control Ownership 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        DTO.ogetControlPersonBusinessDto getControlPersonBusiness(igetControlPersonBusinessDto input);


        /// <summary>
        /// KREMO Budgetcalculation
        /// </summary>
        /// <param name="input"></param>
        /// <returns>budget</returns>
        [OperationContract]
        ogetKremoBudget getKremoBudget(igetKremoBudgetDto input);

        /// <summary>
        /// Returns a link to an external insurance company appliaction for the given offer id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract] ogetVSLinkDto getVSLink(igetVSLinkDto input);

        /// <summary>
        /// TransactionRisikoprüfung durchführen 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        orisikoSimulationDto risikoSimulation(Cic.OpenOne.GateBANKNOW.Service.DTO.irisikoSimIODto input);

        /// <summary>
        /// Calculates Provisions for Expected Loss Calculation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocalculateProvisionsDto calculateProvisions(icalculateProvisionsDto input);
    }
}
