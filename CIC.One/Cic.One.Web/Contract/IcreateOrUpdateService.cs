using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.One.DTO;


namespace Cic.One.Web.Contract
{
    /// <summary>
    /// provides methods for create and update of an entity
    /// </summary>
    [ServiceContract(Name = "IcreateOrUpdateService", Namespace = "http://cic-software.de/One")]
    public interface IcreateOrUpdateService
    {

        /// <summary>
        /// creates or updates an offer from the external application data structure
        /// </summary>
        /// <param name="angebot">the external offer data</param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateExAngebotDto createOrUpdateExAngebot(ExcalcDto angebot);

        /// <summary>
        /// saves/updates Staffelpositionstyp detail
        /// </summary>
        /// <param name="Staffelpositionstyp"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateStaffelpositionstypDto createOrUpdateStaffelpositionstyp(icreateOrUpdateStaffelpositionstypDto Staffelpositionstyp);

        /// <summary>
        /// saves/updates Staffeltyp detail
        /// </summary>
        /// <param name="Staffeltyp"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateStaffeltypDto createOrUpdateStaffeltyp(icreateOrUpdateStaffeltypDto Staffeltyp);

        /// <summary>
        /// saves/updates Rolle detail
        /// </summary>
        /// <param name="Rolle"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateRolleDto createOrUpdateRolle(icreateOrUpdateRolleDto Rolle);

        /// <summary>
        /// saves/updates Rollentyp detail
        /// </summary>
        /// <param name="Rollentyp"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateRollentypDto createOrUpdateRollentyp(icreateOrUpdateRollentypDto Rollentyp);

        /// <summary>
        /// saves/updates Handelsgruppe detail
        /// </summary>
        /// <param name="Handelsgruppe"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateHandelsgruppeDto createOrUpdateHandelsgruppe(icreateOrUpdateHandelsgruppeDto Handelsgruppe);

        /// <summary>
        /// saves/updates Vertriebskanal detail
        /// </summary>
        /// <param name="Vertriebskanal"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateVertriebskanalDto createOrUpdateVertriebskanal(icreateOrUpdateVertriebskanalDto Vertriebskanal);

        /// <summary>
        /// saves/updates Brand detail
        /// </summary>
        /// <param name="Brand"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateBrandDto createOrUpdateBrand(icreateOrUpdateBrandDto Brand);

        /// <summary>
        /// saves/updates Rechnung detail
        /// </summary>
        /// <param name="Rechnung"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateRechnungDto createOrUpdateRechnung(icreateOrUpdateRechnungDto Rechnung);

        /// <summary>
        /// saves/updates Angobbrief detail
        /// </summary>
        /// <param name="Angobbrief"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAngobbriefDto createOrUpdateAngobbrief(icreateOrUpdateAngobbriefDto Angobbrief);

        /// <summary>
        /// saves/updates Zahlungsplan detail
        /// </summary>
        /// <param name="Zahlungsplan"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateZahlungsplanDto createOrUpdateZahlungsplan(icreateOrUpdateZahlungsplanDto Zahlungsplan);

        /// <summary>
		/// saves/updates Kreditlinie detail
        /// </summary>
		/// <param name="Kreditlinie"></param>
        /// <returns></returns>
        [OperationContract]
		ocreateOrUpdateKreditlinieDto createOrUpdateKreditlinie (icreateOrUpdateKreditlinieDto Kreditlinie);

		/// <summary>
        /// saves/updates Fahrzeugbrief detail
        /// </summary>
        /// <param name="Fahrzeugbrief"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateFahrzeugbriefDto createOrUpdateFahrzeugbrief(icreateOrUpdateFahrzeugbriefDto Fahrzeugbrief);

        /// <summary>
        /// saves/updates Kalk detail
        /// </summary>
        /// <param name="Kalk"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateKalkDto createOrUpdateKalk(icreateOrUpdateKalkDto Kalk);

        /// <summary>
        /// saves/updates Person detail
        /// </summary>
        /// <param name="Person"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePersonDto createOrUpdatePerson(icreateOrUpdatePersonDto Person);
        /// <summary>
        /// saves/updates Wfsignature
        /// </summary>
        /// <param name="wfsignature"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateWfsignatureDto createOrUpdateWfsignature(icreateOrUpdateWfsignatureDto wfsignature);

        /// <summary>
        /// saves/updates Dddlkppos
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateDdlkpsposDto createOrUpdateDddlkppos(icreateOrUpdateDdlkpsposDto ddlkpspos);

        /// <summary>
        /// delivers Finanzierung detail
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateFinanzierungDto createOrUpdateFinanzierung(icreateOrUpdateFinanzierungDto finanzierung);

		/// <summary>
		/// delivers Pread-flags 
		/// </summary>
		/// <param name="objekt"></param>
		/// <returns></returns>
		[OperationContract]
		ocreateOrUpdatePreadDto createOrUpdatePread (icreateOrUpdatePreadDto preaddata);

		/// <summary>
        /// delivers RechnungFaellig detail
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateRechnungFaelligDto createOrUpdateRechnungFaellig(icreateOrUpdateRechnungFaelligDto rechnungFaellig);

        /// <summary>
        /// delivers Tilgung detail
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateTilgungDto createOrUpdateTilgung(icreateOrUpdateTilgungDto tilgung);


        /// <summary>
        /// delivers Objekt detail
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateObjektDto createOrUpdateObjekt(icreateOrUpdateObjektDto objekt);

        /// <summary>
        ///updates HEK Objekt detail in eaihot
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateHEKObjektDto createOrUpdateHEKObjekt(icreateOrUpdateHEKObjektDto objekt);

        /// <summary>
        /// delivers Rahmen detail
        /// </summary>
        /// <param name="rahmen"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateRahmenDto createOrUpdateRahmen(icreateOrUpdateRahmenDto rahmen);

        /// <summary>
        /// delivers Haendler detail
        /// </summary>
        /// <param name="haendler"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateHaendlerDto createOrUpdateHaendler(icreateOrUpdateHaendlerDto haendler);

        /// <summary>
        /// delivers Kunde detail
        /// </summary>
        /// <param name="kunde"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateKundeDto createOrUpdateKunde(icreateOrUpdateKundeDto kunde);

        /// <summary>
        /// delivers It detail
        /// </summary>
        /// <param name="it"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateItDto createOrUpdateIt(icreateOrUpdateItDto it);

        /// <summary>
        /// delivers Itadresse detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateItadresseDto createOrUpdateItadresse(icreateOrUpdateItadresseDto itadresse);

        /// <summary>
        /// delivers Itkonto detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateItkontoDto createOrUpdateItkonto(icreateOrUpdateItkontoDto itkonto);

        /// <summary>
        /// delivers Angebot detail
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAngebotDto createOrUpdateAngebot(icreateOrUpdateAngebotDto angebot);

        /// <summary>
        /// delivers Antrag detail
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAntragDto createOrUpdateAntrag(icreateOrUpdateAntragDto antrag);

        /// <summary>
        /// delivers Angebot detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAngkalkDto createOrUpdateAngkalk(icreateOrUpdateAngkalkDto angkalk);

        /// <summary>
        /// delivers Antkalk detail
        /// </summary>
        /// <param name="antkalk"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAntkalkDto createOrUpdateAntkalk(icreateOrUpdateAntkalkDto antkalk);

        /// <summary>
        /// delivers angvar detail
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAngvarDto createOrUpdateAngvar(icreateOrUpdateAngvarDto angvar);

        /// <summary>
        /// delivers angob detail
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAngobDto createOrUpdateAngob(icreateOrUpdateAngobDto angob);

        /// <summary>
        /// delivers antob detail
        /// </summary>
        /// <param name="antob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAntobDto createOrUpdateAntob(icreateOrUpdateAntobDto antob);

        /// <summary>
        /// delivers Contact detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateContactDto createOrUpdateContact(icreateOrUpdateContactDto contact);

        /// <summary>
        /// delivers Adresse detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAdresseDto createOrUpdateAdresse(icreateOrUpdateAdresseDto Adresse);

        /// <summary>
        /// delivers Camp detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateCampDto createOrUpdateCamp(icreateOrUpdateCampDto camp);

        /// <summary>
        /// delivers Opportunity detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateOppoDto createOrUpdateOppo(icreateOrUpdateOppoDto oppo);

        /// <summary>
        /// updates Oppotask detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateOppotaskDto createOrUpdateOppotask(icreateOrUpdateOppotaskDto activity);

        /// <summary>
        /// saves/updates Account detail
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateAccountDto createOrUpdateAccount(icreateOrUpdateAccountDto Account);

        /// <summary>
        /// creates/updates WktAccount detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateWktAccountDto createOrUpdateWktAccount(icreateOrUpdateWktAccountDto account);

        /// <summary>
        /// saves/updates Partner
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePartnerDto createOrUpdatePartner(icreateOrUpdatePartnerDto partner);

        /// <summary>
        /// delivers Beteiligter detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateBeteiligterDto createOrUpdateBeteiligter(icreateOrUpdateBeteiligterDto beteiligter);

        /// <summary>
        /// delivers Prkgroup detail
        /// </summary>
        /// <param name="Prkgroup"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePrkgroupDto createOrUpdatePrkgroup(icreateOrUpdatePrkgroupDto Prkgroup);

        /// <summary>
        /// delivers Prkgroupm detail
        /// </summary>
        /// <param name="Prkgroup"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePrkgroupmDto createOrUpdatePrkgroupm(icreateOrUpdatePrkgroupmDto Prkgroup);

        /// <summary>
        /// delivers Konto detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateKontoDto createOrUpdateKonto(icreateOrUpdateKontoDto konto);


        /// <summary>
        /// delivers Ptrelate detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePtrelateDto createOrUpdatePtrelate(icreateOrUpdatePtrelateDto ptrelate);

        /// <summary>
        /// delivers Crmnm detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateCrmnmDto createOrUpdateCrmnm(icreateOrUpdateCrmnmDto Crmnm);

        /// <summary>
        /// delivers CrmProdukte detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateCrmprDto createOrUpdateCrmProdukte(icreateOrUpdateCrmprDto crmpr);

        /// <summary>
        /// delivers Kategorien detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateItemcatDto createOrUpdateItemcat(icreateOrUpdateItemcatDto itemcat);


        /// <summary>
        /// delivers ItemKategorien detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateItemcatmDto createOrUpdateItemcatm(icreateOrUpdateItemcatmDto itemcatm);


        /// <summary>
        /// delivers Attachement detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateFileattDto createOrUpdateFileatt(icreateOrUpdateFileattDto fileatt);


        /// <summary>
        /// creates or updates Dmsdoc detail
        /// </summary>
        /// <param name="dmsdoc"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateDmsdocDto createOrUpdateDmsdoc(icreateOrUpdateDmsdocDto dmsdoc);

        /// <summary>
        /// delivers Reminder detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateReminderDto createOrUpdateReminder(icreateOrUpdateReminderDto reminder);


        /// <summary>
        /// delivers Recurrence detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateRecurrDto createOrUpdateRecurr(icreateOrUpdateRecurrDto recurr);

        /// <summary>
        /// delivers Checklist detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePrunDto createOrUpdatePrun(icreateOrUpdatePrunDto prun);

        /// <summary>
        /// delivers Checklisttype detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePtypeDto createOrUpdatePtype(icreateOrUpdatePtypeDto ptype);

        /// <summary>
        /// delivers Check detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePrunstepDto createOrUpdatePrunstep(icreateOrUpdatePrunstepDto prunstep);

        /// <summary>
        /// delivers Checktype detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePstepDto createOrUpdatePstep(icreateOrUpdatePstepDto pstep);

        /// <summary>
        /// delivers Segment detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateSegDto createOrUpdateSeg(icreateOrUpdateSegDto seg);

        /// <summary>
        /// delivers SegmentKampagne detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateSegcDto createOrUpdateSegc(icreateOrUpdateSegcDto segc);

        /// <summary>
        /// delivers Stickynote detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateStickynoteDto createOrUpdateStickynote(icreateOrUpdateStickynoteDto stickynote);

        /// <summary>
        /// delivers Stickytype detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateStickytypeDto createOrUpdateStickytype(icreateOrUpdateStickytypeDto stickytype);


        /// <summary>
        /// saves/updates besuchsbericht
        /// </summary>
        /// <param name="besuchsbericht"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateBesuchsberichtDto createOrUpdateBesuchsbericht(icreateOrUpdateBesuchsberichtDto besuchsbericht);


        /// <summary>
        /// delivers Mailmsg detail
        /// </summary>
        /// <param name="mailmsg"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateMailmsgDto createOrUpdateMailmsg(icreateOrUpdateMailmsgDto mailmsg);


        /// <summary>
        /// delivers Appointment detail
        /// </summary>
        /// <param name="apptm"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateApptmtDto createOrUpdateApptmt(icreateOrUpdateApptmtDto apptm);


        /// <summary>
        /// delivers Task detail
        /// </summary>
        /// <param name="ptask"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdatePtaskDto createOrUpdatePtask(icreateOrUpdatePtaskDto ptask);


        /// <summary>
        /// Überprüft ob eine Subscription vorhanden ist und erstellt eine wenn keine existiert.
        /// Ruft außerdem eine Synchronisierung auf.
        /// </summary>
        /// <param name="CacheId"></param>
        /// <returns></returns>
        [OperationContract]
        ocheckCreateSubscriptionDto CheckCreateSubscription();


        /// <summary>
        /// forwards a Mail
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <returns></returns>
        [OperationContract]
        oforwardMail forwardMail(iforwardMail sysid);

        /// <summary>
        /// forwards a Mail
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <returns></returns>
        [OperationContract]
        oreplyMail replyMail(ireplyMail input);

        /// <summary>
        /// deletes an Entity
        /// </summary>
        /// <param name="input">Entity welche gelöscht werden soll</param>
        /// <returns></returns>
        [OperationContract]
        odeleteEntity deleteEntity(ideleteEntity input);


        /// <summary>
        /// Solve a calculation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        osolveKalkulationDto solveKalkulation(isolveKalkulationDto input);

        /// <summary>
        /// creates/updates Recalc
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateRecalcDto createOrUpdateRecalc(icreateOrUpdateRecalcDto recalc);

        /// <summary>
        /// creates/updates MyCalc
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateMycalcDto createOrUpdateMycalc(icreateOrUpdateMycalcDto mycalc);

        /// <summary>
        /// creates/updates Mycalcfs
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateMycalcfsDto createOrUpdateMycalcfs(icreateOrUpdateMycalcfsDto mycalcfs);

        /// <summary>
        /// creates or updates a generic view entity data
        /// </summary>
        /// <param name="gviewdata"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateGviewDto createOrUpdateGview(icreateOrUpdateGviewDto gviewdata);

        /// <summary>
        /// creates or updates a zek data
        /// </summary>
        /// <param name="zek"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateZekDto createOrUpdateZek(icreateOrUpdateZekDto zek);


        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende
        /// </summary>
        /// <param name="memo">sysid, notiz, wftableSyscode, kurzbez, kategorie, ...</param>
        /// <returns>saved memo</returns>
        [OperationContract]
        ocreateOrUpdateMemoDto createOrUpdateMemo(icreateOrUpdateMemoDto memo);

         /// <summary>
        /// Aktualisiert oder erstellt einen PUser
        /// </summary>
        /// <param name="puser"></param>
        [OperationContract]
        ocreateOrUpdatePuserDto createOrUpdatePuser(icreateOrUpdatePuserDto puser);

        /// <summary>
        /// Aktualisiert oder erstellt eine Abklärung
        /// </summary>
        /// <param name="clarification"></param>
        [OperationContract]
        ocreateOrUpdateClarificationDto createOrUpdateClarification(icreateOrUpdateClarificationDto clarification);

        /// <summary>
        /// Creates or updates the Dok validation
        /// </summary>
        /// <param name="dok"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateDokvalDto createOrUpdateDokval(icreateOrUpdateDokvalDto dok);

        /// <summary>
        /// Creates or updates the checklist data
        /// </summary>
        /// <param name="chklist"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateChklistDto createOrUpdateChklist(icreateOrUpdateChklistDto chklist);

        /// <summary>
        /// Creates or updates the prunart data
        /// </summary>
        /// <param name="prunart"></param>
        /// <returns></returns>
        [OperationContract]        
        ocreateOrUpdatePrunartDto createOrUpdatePrunart(icreateOrUpdatePrunartDto prunart);

         /// <summary>
        /// Performs the 4-eyes principle for the area/id setting the given result of the current user
        /// </summary>
        /// <param name="ipar"></param>
        /// <returns></returns>
        [OperationContract]
        ofourEyesDto fourEyesPrinciple(ifourEyesDto ipar);
    }

}
