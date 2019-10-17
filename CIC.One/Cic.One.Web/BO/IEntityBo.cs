using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;
using Cic.One.DTO.BN;

namespace Cic.One.Web.BO
{
    public interface IEntityBo
    {



        #region CREATEORUPDATE

        /// <summary>
        /// updates/creates prunart data
        /// </summary>
        /// <param name="prunart"></param>
        /// <returns></returns>
        PrunartDto createOrUpdatePrunart(PrunartDto prunart);

        /// <summary>
        /// updates/creates Gview
        /// </summary>
        /// <param name="gview"></param>
        /// <returns></returns>
        GviewDto createOrUpdateGview(GviewDto gview);


        /// <summary>
        /// updates/creates Pread
        /// </summary>
        /// <param name="pread"></param>
        /// <returns></returns>
		PreadDto createOrUpdatePread (PreadDto pread);
		

        /// <summary>
        /// updates/creates Staffelpositionstyp
        /// </summary>
        /// <param name="staffelpositionstyp"></param>
        /// <returns></returns>
        StaffelpositionstypDto createOrUpdateStaffelpositionstyp(StaffelpositionstypDto staffelpositionstyp);

        /// <summary>
        /// updates/creates Staffeltyp
        /// </summary>
        /// <param name="staffeltyp"></param>
        /// <returns></returns>
        StaffeltypDto createOrUpdateStaffeltyp(StaffeltypDto staffeltyp);

        /// <summary>
        /// updates/creates Rolle
        /// </summary>
        /// <param name="rolle"></param>
        /// <returns></returns>
        RolleDto createOrUpdateRolle(RolleDto rolle);

        /// <summary>
        /// updates/creates Rollentyp
        /// </summary>
        /// <param name="rollentyp"></param>
        /// <returns></returns>
        RollentypDto createOrUpdateRollentyp(RollentypDto rollentyp);

        /// <summary>
        /// updates/creates Handelsgruppe
        /// </summary>
        /// <param name="handelsgruppe"></param>
        /// <returns></returns>
        HandelsgruppeDto createOrUpdateHandelsgruppe(HandelsgruppeDto handelsgruppe);

        /// <summary>
        /// updates/creates Vertriebskanal
        /// </summary>
        /// <param name="vertriebskanal"></param>
        /// <returns></returns>
        VertriebskanalDto createOrUpdateVertriebskanal(VertriebskanalDto vertriebskanal);

        /// <summary>
        /// updates/creates Brand
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        BrandDto createOrUpdateBrand(BrandDto brand);

        /// <summary>
        /// updates/creates Rechnung
        /// </summary>
        /// <param name="rechnung"></param>
        /// <returns></returns>
        RechnungDto createOrUpdateRechnung(RechnungDto rechnung);

        /// <summary>
        /// updates/creates Angobbrief
        /// </summary>
        /// <param name="angobbrief"></param>
        /// <returns></returns>
        AngobbriefDto createOrUpdateAngobbrief(AngobbriefDto angobbrief);

        /// <summary>
        /// updates/creates Zahlungsplan
        /// </summary>
        /// <param name="zahlungsplan"></param>
        /// <returns></returns>
        ZahlungsplanDto createOrUpdateZahlungsplan(ZahlungsplanDto zahlungsplan);

		/// <summary>
		/// updates/creates Kreditlinie
		/// </summary>
		/// <param name="kreditlinie"></param>
		/// <returns></returns>
		KreditlinieDto createOrUpdateKreditlinie (KreditlinieDto kreditlinie);

		/// <summary>
        /// updates/creates Fahrzeugbrief
        /// </summary>
        /// <param name="fahrzeugbrief"></param>
        /// <returns></returns>
        FahrzeugbriefDto createOrUpdateFahrzeugbrief(FahrzeugbriefDto fahrzeugbrief);

        /// <summary>
        /// updates/creates Kalk
        /// </summary>
        /// <param name="kalk"></param>
        /// <returns></returns>
        KalkDto createOrUpdateKalk(KalkDto kalk);

        /// <summary>
        /// updates/creates Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        PersonDto createOrUpdatePerson(PersonDto person);

        /// <summary>
        /// updates/creates expval
        /// </summary>
        /// <param name="expval"></param>
        /// <returns></returns>
        ExpvalDto createOrUpdateExpval(ExpvalDto expval);

        /// <summary>
        /// updates/creates Finanzierung
        /// </summary>
        /// <param name="sysfinanzierung"></param>
        /// <returns></returns>
        FinanzierungDto createOrUpdateFinanzierung(FinanzierungDto finanzierung, int saveMode);

        /// <summary>
        /// updates/creates RechnungFaellig
        /// </summary>
        /// <param name="rechnungFaellig"></param>
        /// <returns></returns>
        RechnungFaelligDto createOrUpdateRechnungFaellig(RechnungFaelligDto rechnungFaellig);

        /// <summary>
        /// updates/creates Tilgung
        /// </summary>
        /// <param name="tilgung"></param>
        /// <returns></returns>
        TilgungDto createOrUpdateTilgung(TilgungDto tilgung);

        /// <summary>
        /// updates/creates Objekt
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        ObDto createOrUpdateObjekt(ObDto objekt);

        /// <summary>
        /// updates/creates Objekt
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        ObDto createOrUpdateHEKObjekt(ObDto objekt);

        /// <summary>
        /// updates/creates Recalc
        /// </summary>
        /// <param name="recalc"></param>
        /// <returns></returns>
        RecalcDto createOrUpdateRecalc(RecalcDto recalc);

        // <summary>
        /// updates/creates Mycalc
        /// </summary>
        /// <param name="mycalc"></param>
        /// <returns></returns>
        MycalcDto createOrUpdateMycalc(MycalcDto mycalc);

        // <summary>
        /// updates/creates Mycalcfs
        /// </summary>
        /// <param name="mycalcfs"></param>
        /// <returns></returns>
        MycalcfsDto createOrUpdateMycalcfs(MycalcfsDto mycalcfs);

        /// <summary>
        /// updates/creates Rahmen
        /// </summary>
        /// <param name="sysrahmen"></param>
        /// <returns></returns>
        RahmenDto createOrUpdateRahmen(RahmenDto rahmen);

        /// <summary>
        /// updates/creates Haendler
        /// </summary>
        /// <param name="syshaendler"></param>
        /// <returns></returns>
        HaendlerDto createOrUpdateHaendler(HaendlerDto haendler);

        /// <summary>
        /// Returns the image-url parameters for the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        String getEntityLink(String entity, long sysid, long syswfuser, String vlmcode);

        /// <summary>
        /// Returns the icon for the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        EntityIconDto getEntityIcon(String entity, long sysid, long syswfuser, String vlmcode);

        /// <summary>
        /// Returns the icon for the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        List<EntityIconDto> getEntityIcons(String entity, List<long> sysid, long syswfuser, String vlmcode);

        /// <summary>
        /// updates/creates Zusatzdaten
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos);

        /// <summary>
        /// updates/creates Kunde
        /// </summary>
        /// <param name="syskunde"></param>
        /// <returns></returns>
        KundeDto createOrUpdateKunde(KundeDto kunde);
        
        /// <summary>
        /// updates/creates It
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        ItDto createOrUpdateIt(ItDto it);

        /// <summary>
        /// updates/creates Itkonto
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ItkontoDto createOrUpdateItkonto(ItkontoDto itkonto);

        /// <summary>
        /// updates/creates Itadresse
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ItadresseDto createOrUpdateItadresse(ItadresseDto itadresse);

        /// <summary>
        /// updates/creates Angebot
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        AngebotDto createOrUpdateAngebot(AngebotDto angebot);

        /// <summary>
        /// updates/creates Antrag
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        AntragDto createOrUpdateAntrag(AntragDto antrag);

        /// <summary>
        /// updates/creates Angvar
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        AngvarDto createOrUpdateAngvar(AngvarDto angvar);

        /// <summary>
        /// updates/creates Angob
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        AngobDto createOrUpdateAngob(AngobDto angob);

        /// <summary>
        /// updates/creates Antob
        /// </summary>
        /// <param name="antob"></param>
        /// <returns></returns>
        AntobDto createOrUpdateAntob(AntobDto antob);

        /// <summary>
        /// updates/creates Angkalk
        /// </summary>
        /// <param name="angkalk"></param>
        /// <returns></returns>
        AngkalkDto createOrUpdateAngkalk(AngkalkDto angkalk);

        /// <summary>
        /// updates/creates Antkalk
        /// </summary>
        /// <param name="antkalk"></param>
        /// <returns></returns>
        AntkalkDto createOrUpdateAntkalk(AntkalkDto antkalk);

        /// <summary>
        /// updates/creates Contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        ContactDto createOrUpdateContact(ContactDto contact);
        
        /// <summary>
        /// updates/creates Contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        void createOrUpdateContacts(ContactDto contact, String personSQL);

        /// <summary>
        /// updates/creates Adresse
        /// </summary>
        /// <param name="adresse"></param>
        /// <returns></returns>
        AdresseDto createOrUpdateAdresse(AdresseDto adresse);

        /// <summary>
        /// updates/creates camp
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        CampDto createOrUpdateCamp(CampDto camp);

        /// <summary>
        /// updates/creates Oppo
        /// </summary>
        /// <param name="opp"></param>
        /// <returns></returns>
        OpportunityDto createOrUpdateOppo(OpportunityDto oppo);

        /// <summary>
        /// updates/creates Oppotask
        /// </summary>
        /// <param name="oppotask"></param>
        /// <returns></returns>
        OppotaskDto createOrUpdateOppotask(OppotaskDto oppotask);

         /// <summary>
        /// updates/creates Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        AccountDto createOrUpdateAccount(AccountDto account);

        /// <summary>
        /// updates/creates WktAccount
        /// </summary>
        /// <param name="wktaccount"></param>
        /// <returns></returns>
        WktaccountDto createOrUpdateWktAccount(WktaccountDto wktaccount);

        /// <summary>
        /// Returns all  Partner Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        PartnerDto getPartnerDetails(long sysaccount);

        /// <summary>
        /// Returns all  Beteiligter Details
        /// </summary>
        /// <param name="sysbeteiligter"></param>
        /// <returns></returns>
        BeteiligterDto getBeteiligterDetails(long sysbeteiligter);

        /// <summary>
        /// updates/creates Beteiligter
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        BeteiligterDto createOrUpdateBeteiligter(BeteiligterDto partner);

        /// <summary>
        /// updates/creates Partner
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        PartnerDto createOrUpdatePartner(PartnerDto partner);


        /// <summary>
        /// updates/creates Konto
        /// </summary>
        /// <param name="konto"></param>
        /// <returns></returns>
        KontoDto createOrUpdateKonto(KontoDto konto);

        /// <summary>
        /// updates/creates PrkgroupDto 
        /// </summary>
        /// <param name="konto"></param>
        /// <returns></returns>
        PrkgroupDto createOrUpdatePrkgroup(PrkgroupDto prkgroupDto);

        /// <summary>
        /// updates/creates PrkgroupmDto 
        /// </summary>
        /// <param name="konto"></param>
        /// <returns></returns>
        PrkgroupmDto createOrUpdatePrkgroupm(PrkgroupmDto[] prkgroupDto);

        /// <summary>
        /// updates/creates PartnerRelation
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        PtrelateDto[] createOrUpdatePtrelate(PtrelateDto[] ptrelate);

        /// <summary>
        /// updates/creates BeteiligterRelation
        /// </summary>
        /// <param name="ptrelate"></param>
        /// <returns></returns>
        CrmnmDto[] createOrUpdateCrmnm(CrmnmDto[] crmnm);

        /// <summary>
        /// updates/creates CrmProdukte
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        CrmprDto createOrUpdateCrmProdukte(CrmprDto crmpr);

        /// <summary>
        /// updates/creates Checklist
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        PrunDto createOrUpdatePrun(PrunDto prun);

        /// <summary>
        /// updates/creates Checklisttype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        PtypeDto createOrUpdatePtype(PtypeDto ptype);

        /// <summary>
        /// updates/creates Check
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        PrunstepDto createOrUpdatePrunstep(PrunstepDto prunstep);

        /// <summary>
        /// updates/creates Checktype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        PstepDto createOrUpdatePstep(PstepDto pstep);

        /// <summary>
        /// updates/creates Segment
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        SegDto createOrUpdateSeg(SegDto seg);

        /// <summary>
        /// updates/creates SegmentKampagne
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        SegcDto createOrUpdateSegc(SegcDto segc);

        /// <summary>
        /// updates/creates Stickynote
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        StickynoteDto[] createOrUpdateStickynotes(StickynoteDto[] notes);

        /// <summary>
        /// updates/creates Stickytype
        /// </summary>
        /// <param name="stickytype"></param>
        /// <returns></returns>
        StickytypeDto createOrUpdateStickytype(StickytypeDto stickytype);

        /// <summary>
        /// updates/creates Besuchsbericht
        /// </summary>
        /// <param name="besuchsbericht"></param>
        /// <returns></returns>
        BesuchsberichtDto createOrUpdateBesuchsbericht(BesuchsberichtDto besuchsbericht);

        ///// <summary>
        ///// updates/creates Dmsdoc
        ///// </summary>
        ///// <param name="dmsdoc"></param>
        ///// <returns></returns>
        DmsdocDto createOrUpdateDmsdoc(DmsdocDto dmsdoc);

        #region Mail

        ///// <summary>
        ///// updates/creates Dmsdoc
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //FileattDto createOrUpdateFileatt(FileattDto fileatt);

        ///// <summary>
        ///// updates/creates Kategorien
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //ItemcatDto createOrUpdateItemcat(ItemcatDto itemcat);

        ///// <summary>
        ///// updates/creates ItemKategorien
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //ItemcatmDto createOrUpdateItemcatm(ItemcatmDto itemcatm);

        ///// <summary>
        ///// updates/creates Attachement
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //FileattDto createOrUpdateFileatt(FileattDto fileatt);

        ///// <summary>
        ///// updates/creates Reminder
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //ReminderDto createOrUpdateReminder(ReminderDto reminder);

        ///// <summary>
        ///// updates/creates Recurrence
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //RecurrDto createOrUpdateRecurr(RecurrDto recurr);

        ///// <summary>
        ///// updates/creates MailMsg
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //MailmsgDto createOrUpdateMailmsg(MailmsgDto mailmsg);

        ///// <summary>
        ///// updates/creates Apptmt
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //ApptmtDto createOrUpdateApptmt(ApptmtDto apptmt);

        ///// <summary>
        ///// updates/creates Ptask
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //PtaskDto createOrUpdatePtask(PtaskDto ptask);

        #endregion

        /// <summary>
        /// updates/creates from generic Object
        /// ACHTUNG: Kann fehleranfällig sein, wenn die Klassen umbenannt werden/Methoden nicht richtig benannt wurden.
        /// </summary>
        /// <typeparam name="T">Typ, von welchem die Details geladen werden sollen</typeparam>
        /// <param name="item">Item, welches gespeichert werden soll</param>
        /// <returns></returns>
        T createOrUpdate<T>(T item);

        /// <summary>
        /// updates/creates ZEK
        /// </summary>
        /// <param name="zek"></param>
        /// <returns></returns>
        ZekDto createOrUpdateZek(ZekDto zek);

        /// <summary>
        /// updates/creates Dokvalidation
        /// </summary>
        /// <param name="dokval"></param>
        /// <returns></returns>
        DokvalDto createOrUpdateDokval(DokvalDto dokval);

        /// <summary>
        /// updates/creates Checklist data
        /// </summary>
        /// <param name="chklist"></param>
        /// <returns></returns>
        ChklistDto createOrUpdateChklist(ChklistDto chklist);

        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende
        /// </summary>
        /// <param name="memo">sysid, notiz, wftableSyscode, kurzbez, kategorie, ...</param>
        /// <param name="refTable">optional: name of table to be referenced by memo.syswfmtable</param>
        /// <returns>saved memo</returns>
        MemoDto createOrUpdateMemo(MemoDto memo, String refTable = null);


        /// <summary>
        /// Creates or updates a puser
        /// </summary>
        /// <param name="puser"></param>
        /// <returns></returns>
        PuserDto createOrUpdatePuser(PuserDto puser);

        /// <summary>
        /// Creates or updates a clarification
        /// </summary>
        /// <param name="clarification"></param>
        /// <returns></returns>
        ClarificationDto createOrUpdateClarification(ClarificationDto clarification);

        #endregion

        #region GET

        /// <summary>
        /// Returns the document validation Details
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        
        /// <returns></returns>
        DokvalDto getDokvalDetails(String area, long sysId);

        /// <summary>
        /// Returns the checklist data for the antrag
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        ChklistDto getChklistDetails(igetChecklistDetailDto sysId);

        /// <summary>
        /// Returns the checklist art
        /// </summary>
        /// <param name="sysprunart"></param>
        /// <returns></returns>
        PrunartDto getPrunartDetails(long sysprunart);

        /// <summary>
        /// Returns all Gviewtyp Details
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="gviewId"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        GviewDto getGviewDetails(long sysId, String gviewId, WorkflowContext ctx);

        /// <summary>
        /// Returns all Staffelpositionstyp Details
        /// </summary>
        /// <param name="sysslpostyp"></param>
        /// <returns></returns>
        StaffelpositionstypDto getStaffelpositionstypDetails(long sysslpostyp);

        /// <summary>
        /// Returns all Staffeltyp Details
        /// </summary>
        /// <param name="syssltyp"></param>
        /// <returns></returns>
        StaffeltypDto getStaffeltypDetails(long syssltyp);

        /// <summary>
        /// Returns all Rolle Details
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        RolleDto getRolleDetails(long sysperole);

        /// <summary>
        /// Returns all Rollentyp Details
        /// </summary>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        RollentypDto getRollentypDetails(long sysroletype);

        /// <summary>
        /// Returns all Handelsgruppe Details
        /// </summary>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        HandelsgruppeDto getHandelsgruppeDetails(long sysprhgroup);

        /// <summary>
        /// Returns all Vertriebskanal Details
        /// </summary>
        /// <param name="sysbchannel"></param>
        /// <returns></returns>
        VertriebskanalDto getVertriebskanalDetails(long sysbchannel);

        /// <summary>
        /// Returns all Brand Details
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        BrandDto getBrandDetails(long sysbrand);

        /// <summary>
        /// Returns all Rechnung Details
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        RechnungDto getRechnungDetails(long sysid);

        /// <summary>
        /// Returns all Angobbrief Details
        /// </summary>
        /// <param name="sysangobbrief"></param>
        /// <returns></returns>
        AngobbriefDto getAngobbriefDetails(long sysangobbrief);

        /// <summary>
        /// Returns all Zahlungsplan Details
        /// </summary>
        /// <param name="sysslpos"></param>
        /// <returns></returns>
        ZahlungsplanDto getZahlungsplanDetails(long sysslpos);

        /// <summary>
        /// Returns all Fahrzeugbrief Details
        /// </summary>
        /// <param name="sysobbrief"></param>
        /// <returns></returns>
        FahrzeugbriefDto getFahrzeugbriefDetails(long sysobbrief);

        /// <summary>
        /// Returns all Kalk Details
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        KalkDto getKalkDetails(long syskalk);

        /// <summary>
        /// Returns all Person Details
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        PersonDto getPersonDetails(long sysperson);

        /// <summary>
        /// Returns FinanzierungDto Details
        /// </summary>
        /// <param name="sysnkk"></param>
        /// <returns></returns>
        FinanzierungDto getFinanzierungDetails(long sysnkk);

        /// <summary>
        /// Returns Kreditlinie
        /// </summary>
        /// <param name="sysklinie"></param>
        /// <returns></returns>
        KreditlinieDto getKreditlinieDetail(long sysklinie);
      
        /// <summary>
        /// Returns AdmaddDto Details
        /// </summary>
        /// <param name="sysAdmadd"></param>
        /// <returns></returns>
        AdmaddDto getAdmaddDetail(long sysAdmadd);

        /// <summary>
        /// Returns all Indicator Details for the area
        /// </summary>
        /// <param name="exptyp"></param>
        /// <returns></returns>
        List<ExptypDto> getExptypDetails(igetExptypDto exptyp);

        /// <summary>
        /// Returns all Indicator Value Details for the area
        /// </summary>
        /// <param name="expdisp"></param>
        /// <returns></returns>
        List<ExpdispDto> getExpdispDetails(igetExpdispDto expdisp);

		/// <summary>
		/// Returns all SLA Details for the sysid (Ang/Ant)
		/// </summary>
		/// <param name="expdisp"></param>
		/// <returns></returns>
		List<SlaDto> getSlaDetails (igetSlaDto slaid);

		/// <summary>
        /// Returns all Indicator Value Details for the area
        /// </summary>
        /// <param name="expdef"></param>
        /// <returns></returns>
        List<ExpdefDto> getExpdefDetails(igetExpdefDto expdef);

        /// <summary>
        /// Returns all Rub Infos
        /// </summary>
        /// <param name="iRub"></param>
        /// <returns></returns>
        List<DdlkprubDto> getRubInfo(igetRubDto iRub);

        /// <summary>
        /// Returns all ObjektDetails
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ObjektDto getObjektDetails(long sysob);

        /// <summary>
        /// Returns all ObjektDetails
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        ObtypDto getObtypDetails(long sysobtyp);

        /// <summary>
        /// Returns all Obkat Details
        /// </summary>
        /// <param name="sysobkat"></param>
        /// <returns></returns>
        ObkatDto getObkatDetails(long sysobkat);

        /// <summary>
        /// Returns all RecalcDetails
        /// </summary>
        /// <param name="sysrecalc"></param>
        /// <returns></returns>
        RecalcDto getRecalcDetails(long sysrecalc);

        /// <summary>
        /// Returns all Mycalc Details
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        MycalcDto getMycalcDetails(long sysmycalc);
        

        /// <summary>
        /// Returns all RahmenDetails
        /// </summary>
        /// <param name="sysrvt"></param>
        /// <returns></returns>
        RahmenDto getRahmenDetails(long sysrvt);

        /// <summary>
        /// Returns all HaendlerDetails
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        HaendlerDto getHaendlerDetails(long sysperson);

        /// <summary>
        /// Returns all KundeDetails
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        KundeDto getKundeDetails(long sysperson);

        /// <summary>
        /// Returns all KundeDetails
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        LogDumpDto getLogDumpDetails(long logdump);

        /// <summary>
        /// Returns all OpportunityDetails
        /// </summary>
        /// <param name="sysopportunity"></param>
        /// <returns></returns>
        OpportunityDto getOpportunityDetails(long sysopportunity);

        /// <summary>
        /// Returns all OppotaskDetails
        /// </summary>
        /// <param name="sysOppotask"></param>
        /// <returns></returns>
        OppotaskDto getOppotaskDetails(long sysOppotask);

        /// <summary>
        /// Returns all ContactDetails
        /// </summary>
        /// <param name="syscontat"></param>
        /// <returns></returns>
        ContactDto getContactDetails(long syscontact);

        /// <summary>
        /// Returns all AngvarDetails
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        AngvarDto getAngvarDetails(long sysangvar);

        /// <summary>
        /// Returns AngobDetails
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        AngobDto getAngobDetails(long sysangob);

        /// <summary>
        /// Returns  ObDto Details
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ObDto getObDetails(long sysob);

        /// <summary>
        /// Returns all AngkalkDetails
        /// </summary>
        /// <param name="angkalk"></param>
        /// <returns></returns>
        AngkalkDto getAngkalkDetails(long sysangkalk);

        /// <summary>
        /// Returns all AntkalkDetails
        /// </summary>
        /// <param name="antkalk"></param>
        /// <returns></returns>
        AntkalkDto getAntkalkDetails(long sysantkalk);

        /// <summary>
        /// Returns all KontoDetails
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        KontoDto getKontoDetails(long syskonto);

        /// <summary>
        /// Returns all CampDetails
        /// </summary>
        /// <param name="syscamp"></param>
        /// <returns></returns>
        CampDto getCampDetails(long syscamp);

        /// <summary>
        /// Returns all WfuserDetails
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        WfuserDto getWfuserDetails(long syswfuser);

        /// <summary>
        /// Returns all AdresseDetails
        /// </summary>
        /// <param name="sysadresse"></param>
        /// <returns></returns>
        AdresseDto getAdresseDetails(long sysadresse);

        /// <summary>
        /// Returns the Eaihot
        /// </summary>
        /// <param name="syseaihot"></param>
        /// <returns></returns>
        EaihotDto getEaihotDetails(long syseaihot);

        /// <summary>
        /// Returns all PtaskDetails
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        PtaskDto getPtaskDetails(long sysptask);

        /// <summary>
        /// Returns all ApptmtDetails
        /// </summary>
        /// <param name="sysapptmt"></param>
        /// <returns></returns>
        ApptmtDto getApptmtDetails(long sysapptmt);

        /// <summary>
        /// Returns all Reminder Details
        /// </summary>
        /// <param name="sysreminder"></param>
        /// <returns></returns>
        ReminderDto getReminderDetails(long sysreminder);

        /// <summary>
        /// Returns all Mailmsg Details
        /// </summary>
        /// <param name="sysmailmsg"></param>
        /// <returns></returns>
        MailmsgDto getMailmsgDetails(long sysmailmsg);

        /// <summary>
        /// Returns all Memo Details
        /// </summary>
        /// <param name="syswfmmemo"></param>
        /// <returns></returns>
        MemoDto getMemoDetails(long syswfmmemo);

        /// <summary>
        /// Returns all Prun Details
        /// </summary>
        /// <param name="sysprun"></param>
        /// <returns></returns>
        PrunDto getPrunDetails(long sysprun);

        /// <summary>
        /// Returns all Fileatt Details
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        FileattDto getFileattDetails(long sysfileatt);        
        
        /// <summary>
        /// Returns all Fileatt Details by entity
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        FileattDto getFileattDetails(String area,long sysíd);

        /// <summary>
        /// Returns all Dmsdoc Details
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        DmsdocDto getDmsdocDetails(long sysdmsdoc);

        /// <summary>
        /// Returns all Dmsdoc Details by entity
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        List<DmsdocDto> getDmsdocDetails(string area, long sysid);

        /// <summary>
        /// Returns all Prproduct Details
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        PrproductDto getPrproductDetails(long sysprproduct);

        /// <summary>
        /// Returns all Itemcat Details
        /// </summary>
        /// <param name="sysitemcat"></param>
        /// <returns></returns>
        ItemcatDto getItemcatDetails(long sysitemcat);

        /// <summary>
        /// Returns all Ctlang Details
        /// </summary>
        /// <param name="sysctlang"></param>
        /// <returns></returns>
        CtlangDto getCtlangDetails(long sysctlang);

        /// <summary>
        /// Returns all Land Details
        /// </summary>
        /// <param name="sysland"></param>
        /// <returns></returns>
        LandDto getLandDetails(long sysland);

        /// <summary>
        /// Returns all Branche Details
        /// </summary>
        /// <param name="sysbranche"></param>
        /// <returns></returns>
        BrancheDto getBrancheDetails(long sysbranche);

        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        AccountDto getAccountDetails(long sysaccount);

        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        AccountDto getAccountDetails(String area, long sysaccount);

        /// <summary>
        /// Returns all WktAccount Details
        /// </summary>
        /// <param name="syswktaccount"></param>
        /// <returns></returns>
        WktaccountDto getWktAccountDetails(long syswktaccount);

        
        /// <summary>
        /// delivers Finanzdaten by Area
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        ogetFinanzDatenDto getFinanzdatenByArea(long syskd, string area, long sysid);

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        ogetZusatzdaten getZusatzdatenDetail(long syskd, long sysantrag);

        /// <summary>
        /// delivers the pkz or ukz for the it or person 
        /// optionally for the subarea like angebot/antrag and its id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="subarea"></param>
        /// <param name="subsysid"></param>
        /// <returns></returns>
        ogetZusatzdaten getZusatzdatenDetail(String area, long sysid, String subarea, long subsysid);
        

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        ogetZusatzdaten getZusatzdatenDetailByAngebot(long sysit, long sysangebot);

        /// <summary>
        /// Returns all Adrtp Details
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        AdrtpDto getAdrtpDetails(long sysadrtp);

        /// <summary>
        /// Returns Strasse Details
        /// </summary>
        /// <param name="sysstrasse"></param>
        /// <returns></returns>
        StrasseDto getStrasseDetails(long sysstrasse);


        /// <summary>
        /// Returns PUser Details
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        PuserDto getPuserDetails(long syswfuser);

        /// <summary>
        /// updates/creates It
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        ItDto getItDetails(long sysit);

        /// <summary>
        ///  Returns all ItkontoDetails
        /// </summary>
        /// <param name="sysitkonto"></param>
        /// <returns></returns>
        ItkontoDto getItkontoDetails(long sysitkonto);

        /// <summary>
        ///  Returns all ItadresseDetails
        /// </summary>
        /// <param name="sysitadresse"></param>
        /// <returns></returns>
        ItadresseDto getItadresseDetails(long sysitadresse);

        /// <summary>
        /// Returns all Kontotp Details
        /// </summary>
        /// <param name="syskontotp"></param>
        /// <returns></returns>
        KontotpDto getKontotpDetails(long syskontotp);

        /// <summary>
        /// Returns all Blz Details
        /// </summary>
        /// <param name="sysblz"></param>
        /// <returns></returns>
        BlzDto getBlzDetails(long sysblz);

        /// <summary>
        /// Returns all Ptrelate Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        PtrelateDto getPtrelateDetails(long sysptrelate);

        /// <summary>
        /// Returns all Wfexec Details
        /// </summary>
        /// <param name="syswfexec"></param>
        /// <returns></returns>
        WfexecDto getWfexecDetails(long syswfexec);

        /// <summary>
        /// Returns all Crmnm Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        CrmnmDto getCrmnmDetails(long syscrmnm);

        /// <summary>
        /// Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        DdlkprubDto getDdlkprubDetails(long sysddlkprub);

        /// <summary>
        /// Returns all Ddlkpcol Details
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol);

        /// <summary>
        /// Returns all DdlkpposDto Details
        /// </summary>
        /// <param name="sysddlkpposDto"></param>
        /// <returns></returns>
        DdlkpposDto getDdlkpposDetails(long sysddlkppos);

        /// <summary>
        /// Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysddlkpspos"></param>
        /// <returns></returns>
        DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos);

        /// <summary>
        /// Returns all Camptp Details
        /// </summary>
        /// <param name="syscamptp"></param>
        /// <returns></returns>
        CamptpDto getCamptpDetails(long syscamptp);

         /// <summary>
        /// Returns all zinstab Details
        /// </summary>
        /// <param name="syszinstab"></param>
        /// <returns></returns>
        ZinstabDto getZinstabDetails(long syszinstab);

        /// <summary>
        /// Returns all Oppotp Details
        /// </summary>
        /// <param name="syscrmpr"></param>
        /// <returns></returns>
        OppotpDto getOppotpDetails(long sysoppotp);

        /// <summary>
        /// Returns all Crmpr Details
        /// </summary>
        /// <param name="Crmpr"></param>
        /// <returns></returns>
        CrmprDto getCrmprDetails(long syscrmpr);

        /// <summary>
        /// Returns all Contacttp Details
        /// </summary>
        /// <param name="syscontacttp"></param>
        /// <returns></returns>
        ContacttpDto getContacttpDetails(long syscontacttp);

        /// <summary>
        /// Returns all Itemcatm Details
        /// </summary>
        /// <param name="sysitemcatm"></param>
        /// <returns></returns>
        ItemcatmDto getItemcatmDetails(long sysitemcatm);

        /// <summary>
        /// Returns all Recurr Details
        /// </summary>
        /// <param name="sysrecurr"></param>
        /// <returns></returns>
        RecurrDto getRecurrDetails(long sysrecurr);

        /// <summary>
        /// Returns all PtypeDto Details
        /// </summary>
        /// <param name="sysptype"></param>
        /// <returns></returns>
        PtypeDto getPtypeDetails(long sysptype);

        /// <summary>
        /// Returns all Prunstep Details
        /// </summary>
        /// <param name="sysprunstep"></param>
        /// <returns></returns>
        PrunstepDto getPrunstepDetails(long sysprunstep);

        /// <summary>
        /// Returns all Pstep Details
        /// </summary>
        /// <param name="syspstepDto"></param>
        /// <returns></returns>
        PstepDto getPstepDetails(long syspstep);


        /// <summary>
        /// Returns all Prkgroup Details
        /// </summary>
        /// <param name="sysPrkgroupDto"></param>
        /// <returns></returns>
        PrkgroupDto getPrkgroupDetails(long sysPrkgroup);


        /// <summary>
        /// Returns all Prkgroupm Details
        /// </summary>
        /// <param name="sysPrkgroupmDto"></param>
        /// <returns></returns>
        PrkgroupmDto getPrkgroupmDetails(long sysPrkgroupm);


        /// <summary>
        /// Returns all Prkgroupz Details
        /// </summary>
        /// <param name="sysPrkgroupzDto"></param>
        /// <returns></returns>
        PrkgroupzDto getPrkgroupzDetails(long sysPrkgroupz);


        /// <summary>
        /// Returns all Prkgroups Details
        /// </summary>
        /// <param name="sysPrkgroupsDto"></param>
        /// <returns></returns>
        PrkgroupsDto getPrkgroupsDetails(long sysPrkgroups);


        /// <summary>
        /// Returns all Seg Details
        /// </summary>
        /// <param name="sysSegDto"></param>
        /// <returns></returns>
        SegDto getSegDetails(long sysSeg);


        /// <summary>
        /// Returns all Segc Details
        /// </summary>
        /// <param name="sysSegcDto"></param>
        /// <returns></returns>
        SegcDto getSegcDetails(long sysSegc);

        /// <summary>
        /// Returns all Stickynote Details
        /// </summary>
        /// <param name="sysStickynote"></param>
        /// <returns></returns>
        StickynoteDto getStickynoteDetails(long sysStickynote);

        /// <summary>
        /// Returns all Stickytype Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        StickytypeDto getStickytypeDetails(long sysStickytype);

        /// <summary>
        /// Returns Angebot Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        AngebotDto getAngebotDetails(long sysAngebot);

        /// <summary>
        /// Returns Angebot Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        BNAngebotDto getBNAngebotDetails(long sysAngebot);

        /// <summary>
        /// Returns Antrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        BNAntragDto getBNAntragDetails(long sysAntrag);

        /// <summary>
        /// Returns Antrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        AntragDto getAntragDetails(long sysAntrag);

        /// <summary>
        /// Returns Vertrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        VertragDto getVertragDetails(long sysVertrag);

        /// <summary>
        /// Returns Vorgang Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        VorgangDto getVorgangDetails(long sysId, string area);

        /// <summary>
        /// Returns all Wfsignature Details
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        WfsignatureDto getWfsignatureDetail(long input);

        /// <summary>
        /// Returns all Nkonto Details
        /// </summary>
        /// <param name="sysnkonto"></param>
        /// <returns></returns>
        NkontoDto getNkontoDetails(long sysnkonto);

        /// <summary>
        /// Returns all Printset Details
        /// </summary>
        /// <param name="sysprintset"></param>
        /// <returns></returns>
        PrintsetDto getPrintsetDetails(long sysprintset);

        /// <summary>
        /// Returns all Prtlgset Details
        /// </summary>
        /// <param name="sysprtlgset"></param>
        /// <returns></returns>
        PrtlgsetDto getPrtlgsetDetails(long sysprtlgset);

        /// <summary>
        /// Returns all Details of generic Object
        /// ACHTUNG: Kann fehleranfällig sein, wenn die Klassen umbenannt werden/Methoden nicht richtig benannt wurden.
        /// </summary>
        /// <typeparam name="T">Typ, von welchem die Details geladen werden sollen</typeparam>
        /// <param name="sysid">Id vom zu ladenden Objekt</param>
        /// <returns></returns>
        T getDetails<T>(long sysid);

        /// <summary>
        /// Returns all ZEK requests
        /// </summary>
        /// <param name="syszek">primary key</param>
        /// <returns></returns>
        ZekDto getZek(long syszek);

        /// <summary>
        /// returns process data
        /// </summary>
        /// <param name="sysprocess">process id</param>
        /// <returns>process data</returns>
        ProcessDto getProcess(long sysprocess);

        #endregion

        /// <summary>
        /// Sucht Appointments inklusive den Recurrences
        /// </summary>
        /// <param name="search">Parameter</param>
        /// <returns></returns>
        List<ApptmtDto> searchApptmts(iSearchApptmtsWithRecurrDto search);

        /// <summary>
        /// Löscht eine Entity
        /// </summary>
        /// <param name="area">Area</param>
        /// <param name="sysid">Sysid</param>
        void deleteEntity(string area, long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="syswfuser"></param>
        /// <param name="sysit"></param>
        /// <param name="legitimationMethode"></param>
        void updateLegitimationMethode(long sysangebot, long syswfuser, long sysit, string legitimationMethode);

        /// <summary>
        /// update abwicklungsort for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        bool updateAbwicklungsort(iupdateAbwicklungsortDto input);

        /// <summary>
        /// update smstext for ANTRAG
        /// </summary>
        /// <param name="input"></param>
        bool updateSMSText(iupdateSMSTextDto input);

        /// <summary>
        /// get anciliary details for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        ogetAnciliaryDetailDto getAnciliaryDetail(igetAnciliaryDetailDto input);

        /// <summary>
        /// accept EPOS Conditions of current user
        /// </summary>
        void acceptEPOSConditions();

        /// <summary>
        /// send Risk email / EAIHOT
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        osendRiskmailDto sendRiskmail(isendRiskmailDto input);

        /// <summary>
        /// Performs the 4-eyes principle for the area/id setting the given result of the current user
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        void fourEyesPrinciple(ifourEyesDto input, ofourEyesDto output);
    }
}
