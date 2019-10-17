using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface createOrUpdateKundeService stellt die Methoden zum Editieren des Kunden sowie Speichern und Auswahlliste zur Verfügung
    /// </summary>
    [ServiceContract(Name = "IcreateOrUpdateKundeService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IcreateOrUpdateKundeService
    {
        /// <summary>
        /// Liefert eine Liste der verfügbaren Anreden
        /// </summary>
        /// <returns>olistAnredenDto</returns>
        [OperationContract]
        olistAvailableKundentypenDto listAvailableKundentypen();
        
        /// <summary>
        /// Liefert eine Liste der verfügbaren Anreden
        /// </summary>
        /// <returns>olistAnredenDto</returns>
        [OperationContract]
        olistAnredenDto listAnreden();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Länder
        /// </summary>
        /// <returns>olistLaenderDto</returns>
        [OperationContract]
        olistLaenderDto listLaender();

         /// <summary>
        /// Liefert eine Liste der verfügbaren Ausweisarten
        /// </summary>
        /// <returns>olistAusweisartenDto</returns>
        [OperationContract]
        olistAusweisartenDto listAusweisarten();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Kantone
        /// </summary>
        /// <returns>olistKantoneDto</returns>
        [OperationContract]
        olistKantoneDto listKantone();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Sprachen
        /// </summary>
        /// <returns>olistSprachenDto</returns>
        [OperationContract]
        olistSprachenDto listSprachen();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Zivilstände
        /// </summary>
        /// <returns>olistZivilstaendeDto</returns>
        [OperationContract]
        olistZivilstaendeDto listZivilstaende();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Nationalitäten
        /// </summary>
        /// <returns>olistNationalitaetenDto</returns>
        [OperationContract]
        olistNationalitaetenDto listNationalitaeten();

        /// <summary>
        /// Liefert eine Liste der verfügbaren beruflichen Situationen
        /// </summary>
        /// <returns>olistBeruflicheSituationDto</returns>
        [OperationContract]
        olistBeruflicheSituationenDto listBeruflicheSituationen();

        /// <summary>
        /// Liefert eine Liste der verfügbaren Wohnsituationen
        /// </summary>
        /// <returns>olistWohnSituationenDto</returns>
        [OperationContract]
        olistWohnSituationenDto listWohnSituationen();

       
       /// <returns></returns>
        /// <summary>
        /// Liefert Kanton und Ort zur Postleitzahl
        /// </summary>
        /// <param name="input">ifindOrtByPlzDto</param>
        /// <returns>ofindOrtByPlzDto</returns>
        [OperationContract]
        ofindOrtByPlzDto findOrtByPlz(ifindOrtByPlzDto input);

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt des Händlerkunden
        /// </summary>
        /// <param name="input">icreateKundeDto</param>
        /// <returns>ocreateKundeDto</returns>
        [OperationContract]
        ocreateKundeDto createOrUpdateKunde(DTO.icreateKundeDto input);
        
        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt der Adresse des Händlerkunden
        /// </summary>
        /// <param name="input">icreateAdresseDto</param>
        /// <returns>ocreateAdresseDto</returns>
        [OperationContract]
        ocreateAdresseDto createOrUpdateAdresse(DTO.icreateAdresseDto input);

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt der Kontverbindung des Händlerkunden
        /// </summary>
        /// <param name="input">icreateKontoDto</param>
        /// <returns>ocreateKontoDto</returns>
        [OperationContract]
        ocreateKontoDto createOrUpdateKonto(DTO.icreateKontoDto input);

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt der Zusatzdaten des Händlerkunden
        /// </summary>
        /// <param name="input">icreateZusatzdatenDto</param>
        /// <returns>ocreateZusatzdatenDto</returns>
        [OperationContract]
        ocreateZusatzdatenDto createOrUpdateZusatzdaten(DTO.icreateZusatzdatenDto input);

        /// <summary>
        /// Speichert alle Persistenzobjekte des Händlerkunden
        /// </summary>
        /// <param name="input">isaveKundeDto</param>
        /// <returns>osaveKundeDto</returns>
        [OperationContract]
        osaveKundeDto saveKunde(DTO.isaveKundeDto input);

        /// <summary>
        /// Listet alle Branchen auf
        /// </summary>
        /// <returns>olistBranchenDto</returns>
        [OperationContract]
        olistBranchenDto listBranchen();

        /// <summary>
        /// Listet alle Rechtsformen auf
        /// </summary>
        /// <returns>olistRechtsformenDto</returns>
        [OperationContract]
        olistRechtsformenDto listRechtsformen();

        /// <summary>
        /// Listet alle Unterstuetzungsarten auf
        /// </summary>
        /// <returns>olistUnterstuetzungsartenDto</returns>
        [OperationContract]
        olistUnterstuetzungsartenDto listUnterstuetzungsarten();

        /// <summary>
        /// Listet alle Auslagenarten auf
        /// </summary>
        /// <returns>olistAuslagenartenDto</returns>
        [OperationContract]
        olistAuslagenartenDto listAuslagenarten();

        /// <summary>
        /// Listet alle weiteren Auslagenarten auf
        /// </summary>
        /// <returns>olistAuslagenartenDto</returns>
        [OperationContract]
        olistAuslagenartenDto listWeitereAuslagenarten();

        /// <summary>
        /// Listet alle Zusatzeinkommen auf
        /// </summary>
        /// <returns>olistZusatzeinkommenDto</returns>
        [OperationContract]
        olistZusatzeinkommenDto listZusatzeinkommen();

         /// <summary>
        /// Sucht alle Blz für den übergebenen Filter (BLZ|IBAN|BIC)
        /// </summary>
        /// <param name="input">ifindBlzDto</param>
        /// <returns>ofindBlzDto</returns>
        [OperationContract]
        ofindBlzDto findBlz(ifindBlzDto input);

        /// <summary>
        /// Sucht die konntonummer für den übergebenen für IBAN
        /// </summary>
        /// <param name="input">ifindBlzDto</param>
        /// <returns>ofindBlzDto</returns>
        [OperationContract]
        ofindKontoNrByIBANDto findKontoNrByIBAN(ifindKontoNrByIBANDto input);

        /// <summary>
        /// Berufsauslagenarten
        /// </summary>
        /// <returns>olistBerufsauslagenartenDto</returns>
        [OperationContract]
        olistBerufsauslagenartenDto listBerufsauslagenarten();

        /// <summary>
        /// Ermittelt Bankname und BankId für eine Kontonr und BLZ
        /// </summary>
        /// <param name="input">ifindBankByBlzDto</param>
        /// <returns>Bankinformationen</returns>
        [OperationContract]
        ofindBankByBlzDto findBankByBlz(ifindBankByBlzDto input);

        /// <summary>
        /// Ermittelt IBAN für eine Kontonr und BLZ
        /// </summary>
        /// <param name="input">ifindBankByBlzDto</param>
        /// <returns>IBAN</returns>
        [OperationContract]
        ofindIBANByBlzDto findIBANByBlz(ifindBankByBlzDto input);

         /// <summary>
        /// Liefert eine Liste der verfügbaren Mitantragsteller Zustände
        /// </summary>
        /// <returns>olistMitantStatiDto</returns>
        [OperationContract]
        olistMitantStatiDto listMitantragstellerStati();

        /// <summary>
        /// Verwendungszweck Liste liefern
        /// </summary>
        /// <returns>olistVerwendungszweckDto</returns>
        [OperationContract]
        olistVerwendungszweckDto listVerwendungszweck();

        /// <summary>
        /// Liefert eine Liste der Fremdbanken
        /// </summary>
        /// <returns>olistFremdBankenDto</returns>
        [OperationContract]
        olistFremdBankenDto listFremdBanken();

        /// <summary>
        /// Attaches the given disclaimer to the given area/id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateDisclaimerDto createDisclaimer(icreateDisclaimerDto input);

    }
}
