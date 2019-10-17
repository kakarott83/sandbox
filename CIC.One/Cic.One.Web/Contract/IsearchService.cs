using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.One.DTO;


namespace Cic.One.Web.Contract
{
    /// <summary>
    /// Das Interface IwholesaleInfoService stellt die Methoden bereit zur Darstellung aller HEK Informationen
    /// </summary>
    [ServiceContract(Name = "IsearchService", Namespace = "http://cic-software.de/One")]

    public interface IsearchService
    {
        /// <summary>
        /// delivers a list of Clarifications
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchClarificationDto searchClarification(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of LogDump item results
        /// rh: 20161108
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchLogDumpDto searchLogDump(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of generic view item results
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchGviewDto searchGview(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Staffelpositionstyp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchStaffelpositionstypDto searchStaffelpositionstyp(iSearchDto input);

        /// <summary>
        /// delivers a list of Staffeltyp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchStaffeltypDto searchStaffeltyp(iSearchDto input);

        /// <summary>
        /// delivers a list of Rolle
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchRolleDto searchRolle(iSearchDto input);

        /// <summary>
        /// delivers a list of Rollentyp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchRollentypDto searchRollentyp(iSearchDto input);

        /// <summary>
        /// delivers a list of Handelsgruppe
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchHandelsgruppeDto searchHandelsgruppe(iSearchDto input);

        /// <summary>
        /// delivers a list of Vertriebskanal
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchVertriebskanalDto searchVertriebskanal(iSearchDto input);

        /// <summary>
        /// delivers a list of Brand
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchBrandDto searchBrand(iSearchDto input);

        /// <summary>
        /// delivers a list of Rechnung
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchRechnungDto searchRechnung(iSearchDto input);

        /// <summary>
        /// delivers a list of Angobbrief
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAngobbriefDto searchAngobbrief(iSearchDto input);

        /// <summary>
        /// delivers a list of Zahlungsplan
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchZahlungsplanDto searchZahlungsplan(iSearchDto input);
        
        /// <summary>
        /// delivers a list of Zahlungsplan NKK
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchNkkabschlDto searchNkkabschl(iSearchDto input);

        /// <summary>
        /// delivers a list of Fahrzeugbrief
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchFahrzeugbriefDto searchFahrzeugbrief(iSearchDto input);

        /// <summary>
        /// delivers a list of Kalk
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchKalkDto searchKalk(iSearchDto input);

        /// <summary>
        /// delivers a list of Person
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPersonDto searchPerson(iSearchDto input);

        /// <summary>
        /// searches in eaihot
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchEaihotDto searchEaihot(iSearchDto iSearch);

        /// <summary>
        /// Sucht nach Dokumenten
        /// </summary>
        /// <param name="dynamicSearch">Parameter</param>
        /// <returns>Infos zu den gefundenen Dokumenten</returns>
        [OperationContract]
        oDynamicDocumentSearchDto DynamicDocumentSearch(iDynamicDocumentSearchDto input);

        /// <summary>
        /// Lädt ein Dokument anhand dem Input
        /// </summary>
        /// <param name="docLoad"></param>
        /// <returns>Enthält das Dokument</returns>
        [OperationContract]
        oDocumentLoadDto DocumentLoad(iDocumentLoadDto docLoad);

        /// <summary>
        /// delivers a list of Objekt
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchObjektDto searchObjekt(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Recalc
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchRecalcDto searchRecalc(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Mycalc
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchMycalcDto searchMycalc(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Mycalc
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchMycalcfsDto searchMycalcfs(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Exptyp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchExptypDto searchExptyp(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Expval
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchExpvalDto searchExpval(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Rahmen
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchRahmenDto searchRahmen(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Haendler
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchHaendlerDto searchHaendler(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of vorgänge
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchVorgangDto searchVorgang(iSearchDto input);

        /// <summary>
        /// delivers a list for the vc_activities View
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchActivitiesDto searchActivities(iSearchDto iSearch);

        /// <summary>
        /// delivers a list for the Oppotask-Table
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchOppotaskDto searchOppotask(iSearchDto iSearch);

        /// <summary>
        /// delivers a list for the In Equity Forecast-Table
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchIefDto searchIef(iSearchDto iSearch);

        /// <summary>
        /// searches the entity
        /// duplicate this service for every new entity with a list in the gui
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchFinanzierungDto searchFinanzierungen(iSearchDto input);

		/// <summary>
		/// searches the entity
		/// duplicate this service for every new entity with a list in the gui
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[OperationContract]
		oSearchPreadDto searchPread (iSearchDto input);


        /// <summary>
        /// delivers a list of customers
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchKundeDto searchKunden(iSearchDto input);

        /// <summary>
        /// delivers a list of it
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchItDto searchIt(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of wfsignatures
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchWfsignatureDto searchWfsignature(iSearchDto input);

        /// <summary>
        /// delivers a list of memos
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchMemoDto searchMemos(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of historical entries
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchHistoryDto searchHistory(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of open invoices
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchRechnungFaelligDto searchRechnungFaellig(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of tilgungen
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchTilgungDto searchTilgungen(iSearchDto iSearch);


        /// <summary>
        /// delivers a list of Zinsabschlüsse
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchZinsabschlDto searchZinsabschl(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of contacts
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchContactDto searchContacts(iSearchDto input);

        /// <summary>
        /// delivers a list of konto
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchKontoDto searchKonten(iSearchDto input);

        /// <summary>
        /// delivers a list of camp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchCampDto searchCamp(iSearchDto input);

        /// <summary>
        /// delivers a list of Itkonto
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchItkontoDto searchItkonten(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Angebot
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAngebotDto searchAngebot(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of BN Angebot
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchBNAngebotDto searchBNAngebot(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of BN Antrag
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchBNAntragDto searchBNAntrag(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Antrag
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAntragDto searchAntrag(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Vertrg
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchVertragDto searchVertrag(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Itadresse
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchItadresseDto searchItadressen(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Adresse
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAdresseDto searchAdressen(iSearchDto input);


        /// <summary>
        /// delivers a list of Strasse
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchStrasseDto searchStrasse(iSearchDto input);


        /// <summary>
        /// delivers a list of Plz
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPlzDto searchPLZ(iSearchDto input);

        /// <summary>
        /// delivers a list of Ptask
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPtaskDto searchPtasks(iSearchDto input);

        /// <summary>
        /// delivers a list of Apptmt
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchApptmtDto searchApptmts(iSearchDto input);


        /// <summary>
        /// delivers a list of Apptmt with Recurr
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchApptmtsWithRecurrDto searchApptmtsWithRecurr(iSearchApptmtsWithRecurrDto input);

        /// <summary>
        /// delivers a list of Reminder
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchReminderDto searchReminders(iSearchDto input);

        /// <summary>
        /// delivers a list of Mailmsg
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchMailmsgDto searchMailmsgs(iSearchDto input);

        /// <summary>
        /// delivers a list of Prun
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPrunDto searchPruns(iSearchDto input);

        /// <summary>
        /// delivers a list of Prun
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchFileattDto searchFileatts(iSearchDto input);


        /// <summary>
        /// delivers a list of Besuchsberichte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchBesuchsberichteDto searchBesuchsberichte(iSearchDto input);

        /// <summary>
        /// delivers a list of Prproduct
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPrproductDto searchPrproducts(iSearchDto input);

        /// <summary>
        /// delivers a list of Itemcat
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchItemcatDto searchItemcats(iSearchDto input);

        /// <summary>
        /// delivers a list of Ctlang
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchCtlangDto searchCtlangs(iSearchDto input);

        /// <summary>
        /// delivers a list of Land
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchLandDto searchLand(iSearchDto input);

        /// <summary>
        /// delivers a list of Branche
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchBrancheDto searchBranchen(iSearchDto input);

        /// <summary>
        /// delivers a list of Antragsubersicht
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAntragsubersichtDto searchAntragsubersicht(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of EKF-Antrage
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchEkfantrageDto searchEkfantrage(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Abgerechnete Vertrage
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAbgerechnetevertrageDto searchAbgerechnetevertrage(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Abrufscheine
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAbrufscheineDto searchAbrufscheine(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Account
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAccountDto searchAccounts(iSearchDto input);

        /// <summary>
        /// delivers a list of WktAccount
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchWktAccountDto searchWktAccounts(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Account by the given query
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAccountDto searchAccountsWithQuery(iSearchDto iSearch, String sql);

        /// <summary>
        /// delivers a list of Wfuser
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchWfuserDto searchWfuser(iSearchDto input);

        /// <summary>
        /// delivers a list of Oppo
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchOppoDto searchOpportunity(iSearchDto input);

        /// <summary>
        /// delivers a list of Adrtp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAdrtpDto searchAdrtp(iSearchDto input);

        /// <summary>
        /// delivers a list of Kontotp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchKontotpDto searchKontotp(iSearchDto input);

        /// <summary>
        /// delivers a list of Blz
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchBlzDto searchBlz(iSearchDto input);

        /// <summary>
        /// delivers a list of Ptrelate
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPtrelateDto searchPtrelates(iSearchDto input);

        /// <summary>
        /// delivers a list of Crmnm
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchCrmnmDto searchCrmnms(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Partner
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPartnerDto searchPartner(iSearchDto input);

        /// <summary>
        /// delivers a list of Beteiligter
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchBeteiligterDto searchBeteiligter(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Ddlkprub / Rubriken
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchDdlkprubDto searchDdlkprub(iSearchDto input);

        /// <summary>
        /// delivers a list of Ddlkpcol / Wertebereiche
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchDdlkpcolDto searchDdlkpcol(iSearchDto input);

        /// <summary>
        /// delivers a list of Ddlkppos / Werte
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchDdlkpposDto searchDdlkppos(iSearchDto input);

        /// <summary>
        /// delivers a list of DdlkpsposDto / WerteSelektiert
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchDdlkpsposDto searchDdlkpspos(iSearchDto input);


        /// <summary>
        /// delivers a list of ddlkprub/Rubriken, only the rubs for user-selected items
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchDdlkprubDto searchZusatzdaten(iSearchDto iSearch, igetRubDto rubInfo);

        /// <summary>
        /// delivers a list of Camptp 
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchCamptpDto searchCamptp(iSearchDto input);

        /// <summary>
        /// delivers a list of Oppotp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchOppotpDto searchOppotp(iSearchDto input);

        /// <summary>
        /// delivers a list of Crmpr
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchCrmprDto searchCrmpr(iSearchDto input);

        /// <summary>
        /// delivers a list of Contacttp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchContacttpDto searchContacttp(iSearchDto input);

        /// <summary>
        /// delivers a list of Itemcatm 
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchItemcatmDto searchItemcatm(iSearchDto input);

        /// <summary>
        /// delivers a list of Recurr 
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchRecurrDto searchRecurr(iSearchDto input);

        /// <summary>
        /// delivers a list of Ptype 
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPtypeDto searchPtype(iSearchDto input);

        /// <summary>
        /// delivers a list of Itemcatm
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPrunstepDto searchPrunstep(iSearchDto input);

        /// <summary>
        /// delivers a list of Pstep
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPstepDto searchPstep(iSearchDto input);

        /// <summary>
        /// delivers a list of Prkgroup
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPrkgroupDto searchPrkgroup(iSearchDto input);

        /// <summary>
        /// delivers a list of Prkgroupm
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPrkgroupmDto searchPrkgroupm(iSearchDto input);

        /// <summary>
        /// delivers a list of Prkgroupz
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPrkgroupzDto searchPrkgroupz(iSearchDto input);

        /// <summary>
        /// delivers a list of Prkgroups
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPrkgroupsDto searchPrkgroups(iSearchDto input);

        /// <summary>
        /// delivers a list of Seg
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchSegDto searchSeg(iSearchDto input);

        /// <summary>
        /// delivers a list of Segc
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchSegcDto searchSegc(iSearchDto input);

        /// <summary>
        /// delivers a list of stickynote
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchStickynoteDto searchStickynote(iSearchDto input);

        /// <summary>
        /// delivers a list of Notizen
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchNotizDto searchNotiz(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of stickytype
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchStickytypeDto searchStickytype(iSearchDto input);

        /// <summary>
        /// delivers a list of RegVars
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchRegVarDto searchRegVars(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Angvar
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAngvarDto searchAngvar(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Angob
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAngobDto searchAngob(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Angkalk
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAngkalkDto searchAngkalk(iSearchDto iSearch);

        /// <summary>
        /// delivers a list ofAngobsl
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAngobslDto searchAngobsl(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Angobslpos
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAngobslposDto searchAngobslpos(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Antob
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAntobDto searchAntob(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Antobsl
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAntobslDto searchAntobsl(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Antobslpos
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAntobslposDto searchAntobslpos(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Ob
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchObDto searchOb(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Vtobsl
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchVtobslDto searchVtobsl(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Vtobslpos
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchVtobslposDto searchVtobslpos(iSearchDto iSearch);

        /// <summary>
        /// delivers Report Data
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchReportDto searchReport(iSearchDto iSearch);
        // [OperationContract]
        //  object executeScript(String input);

        /// <summary>
        /// delivers a list of Inbox-Items
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchInboxDto searchInbox(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Process Items from BPE
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchProcessDto searchProcess(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Dmsdocs
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchDmsdocDto searchDmsdoc(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of PUsers
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchPuserDto searchPuser(iSearchDto iSearch);

        /// <summary>
        /// delivers a list of Aufträge
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchAuftragDto searchAuftrag(iSearchDto iSearch);
    }
}
