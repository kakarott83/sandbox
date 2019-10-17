using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;
using Cic.One.DTO.BN;
using Cic.OpenOne.Common.Model.DdOl;


namespace Cic.One.Web.DAO
{
    public interface IEntityDao
    {
           /// <summary>
        /// Returns all Vart Details
        /// </summary>
        /// <param name="sysvart"></param>
        /// <returns></returns>
        List<VartDto> getVarten();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysPerole"></param>
        void setSysPerole(long sysPerole);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysWfuser"></param>
        void setSysWfuser(long sysWfuser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        void setISOLanguage(String language);

        /// <summary>
        /// returns the current users language code
        /// </summary>
        /// <returns></returns>
        String getISOLanguage();

         /// <summary>
        /// Returns the queryable result list for the given sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IEnumerable<long> getSQLResults(String sql);

        #region CREATEORUPDATE

        /// <summary>
        /// updates/creates Checklist data
        /// </summary>
        /// <param name="chklist"></param>
        /// <returns></returns>
        ChklistDto createOrUpdateChklist(ChklistDto chklist);

        /// <summary>
        /// updates/creates prunart data
        /// </summary>
        /// <param name="prunart"></param>
        /// <returns></returns>
        PrunartDto createOrUpdatePrunart(PrunartDto prunart);

        /// <summary>
        /// updates/creates clarification
        /// </summary>
        /// <param name="clarification"></param>
        /// <returns></returns>
        ClarificationDto createOrUpdateClarification(ClarificationDto clarification);

        /// <summary>
        /// updates/creates eaihot
        /// </summary>
        /// <param name="eaihot"></param>
        /// <returns></returns>
        EaihotDto createOrUpdateEaihot(EaihotDto eaihot);

        /// <summary>
		/// updates/creates Pread datarow (gelesen/ungelesen) (rh 20170515)
        /// </summary>
        /// <param name="eaihot"></param>
        /// <returns></returns>
		PreadDto createOrUpdatePread (PreadDto pread);
		

        /// <summary>
        /// updates/creates Gview
        /// </summary>
        /// <param name="gview"></param>
        /// <returns></returns>
        GviewDto createOrUpdateGview(GviewDto gview);

        /// <summary>
        /// updates/creates Staffelpositionstyp
        /// </summary>
        /// <param name="Staffelpositionstyp"></param>
        /// <returns></returns>
        StaffelpositionstypDto createOrUpdateStaffelpositionstyp(StaffelpositionstypDto Staffelpositionstyp);

        /// <summary>
        /// updates/creates Staffeltyp
        /// </summary>
        /// <param name="Staffeltyp"></param>
        /// <returns></returns>
        StaffeltypDto createOrUpdateStaffeltyp(StaffeltypDto Staffeltyp);

        /// <summary>
        /// updates/creates Rolle
        /// </summary>
        /// <param name="Rolle"></param>
        /// <returns></returns>
        RolleDto createOrUpdateRolle(RolleDto Rolle);

        /// <summary>
        /// updates/creates Rollentyp
        /// </summary>
        /// <param name="Rollentyp"></param>
        /// <returns></returns>
        RollentypDto createOrUpdateRollentyp(RollentypDto Rollentyp);

        /// <summary>
        /// updates/creates Handelsgruppe
        /// </summary>
        /// <param name="Handelsgruppe"></param>
        /// <returns></returns>
        HandelsgruppeDto createOrUpdateHandelsgruppe(HandelsgruppeDto Handelsgruppe);

        /// <summary>
        /// updates/creates Vertriebskanal
        /// </summary>
        /// <param name="Vertriebskanal"></param>
        /// <returns></returns>
        VertriebskanalDto createOrUpdateVertriebskanal(VertriebskanalDto Vertriebskanal);

        /// <summary>
        /// updates/creates Brand
        /// </summary>
        /// <param name="Brand"></param>
        /// <returns></returns>
        BrandDto createOrUpdateBrand(BrandDto Brand);

        /// <summary>
        /// updates/creates Rechnung
        /// </summary>
        /// <param name="Rechnung"></param>
        /// <returns></returns>
        RechnungDto createOrUpdateRechnung(RechnungDto Rechnung);

        /// <summary>
        /// updates/creates Angobbrief
        /// </summary>
        /// <param name="Angobbrief"></param>
        /// <returns></returns>
        AngobbriefDto createOrUpdateAngobbrief(AngobbriefDto Angobbrief);

        /// <summary>
        /// updates/creates Zahlungsplan
        /// </summary>
        /// <param name="Zahlungsplan"></param>
        /// <returns></returns>
        ZahlungsplanDto createOrUpdateZahlungsplan(ZahlungsplanDto Zahlungsplan);

        /// <summary>
		/// updates/creates Kreditlinie
        /// </summary>
		/// <param name="Kreditlinie"></param>
        /// <returns></returns>
		KreditlinieDto createOrUpdateKreditlinie (KreditlinieDto Kreditlinie);

        /// <summary>
        /// updates/creates Fahrzeugbrief
        /// </summary>
        /// <param name="Fahrzeugbrief"></param>
        /// <returns></returns>
        FahrzeugbriefDto createOrUpdateFahrzeugbrief(FahrzeugbriefDto Fahrzeugbrief);

        /// <summary>
        /// updates/creates Kalk
        /// </summary>
        /// <param name="Kalk"></param>
        /// <returns></returns>
        KalkDto createOrUpdateKalk(KalkDto Kalk);

        /// <summary>
        /// updates/creates Person
        /// </summary>
        /// <param name="Person"></param>
        /// <returns></returns>
        PersonDto createOrUpdatePerson(PersonDto Person);

        /// <summary>
        /// updates/creates expval
        /// </summary>
        /// <param name="expval"></param>
        /// <returns></returns>
        ExpvalDto createOrUpdateExpval(ExpvalDto expval);

        /// <summary>
        /// updates/creates expvalar
        /// </summary>
        /// <param name="expval"></param>
        /// <returns></returns>
        ExpvalDto createOrUpdateExpvalar(ExpvalDto expval);

        /// <summary>
        /// updates/creates Finanzierung
        /// </summary>
        /// <param name="finanzierung"></param>
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
        ObDto createOrUpdateOb(ObDto objekt);

        /// <summary>
        /// updates/creates Objekt
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        ObDto createOrUpdateHEKOb(ObDto objekt);

        /// <summary>
        /// updates/creates Recalc
        /// </summary>
        /// <param name="recalc"></param>
        /// <returns></returns>
        RecalcDto createOrUpdateRecalc(RecalcDto recalc);

        /// <summary>
        /// updates/creates Mycalc
        /// </summary>
        /// <param name="mycalc"></param>
        /// <returns></returns>
        MycalcDto createOrUpdateMycalc(MycalcDto mycalc);

        /// <summary>
        /// updates/creates Mycalcfs
        /// </summary>
        /// <param name="mycalcfs"></param>
        /// <returns></returns>
        MycalcfsDto createOrUpdateMycalcfs(MycalcfsDto mycalcfs);

        /// <summary>
        /// updates/creates Rahmen
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        RahmenDto createOrUpdateRahmen(RahmenDto rahmen);

        /// <summary>
        /// updates/creates Haendler
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        HaendlerDto createOrUpdateHaendler(HaendlerDto haendler);

        /// <summary>
        /// Erzeugt eine Peuni
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysperole"></param>
        /// <param name="sysid"></param>
        /// <param name="forceChangePerole">Gibt an ob die Perole überschrieben werden soll, falls schon ein Eintrag existiert</param>
        /// <returns></returns>
        bool createOrUpdatePeuni(String area, long sysperole, long sysid, bool forceChangePerole = false);


        /// <summary>
        /// updates/creates Kunde
        /// </summary>
        /// <param name="sysob"></param>
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
        /// updates/creates Angvar
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        AngvarDto createOrUpdateAngvar(AngvarDto angvar);

        /// <summary>
        /// updates/creates Angebot
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        AngebotDto createOrUpdateAngebot(AngebotDto angebot);

        /// <summary>
        /// updates/creates Antrag
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        AntragDto createOrUpdateAntrag(AntragDto antrag);

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
        /// <param name="sysob"></param>
        /// <returns></returns>
        ContactDto createOrUpdateContact(ContactDto Contact);

        /// <summary>
        /// updates/creates Adresse
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        AdresseDto createOrUpdateAdresse(AdresseDto Adresse);

        /// <summary>
        /// updates/creates Camp
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        CampDto createOrUpdateCamp(CampDto Camp);

        /// <summary>
        /// updates/creates Opportunity
        /// </summary>
        /// <param name="oppo"></param>
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
        /// <param name="Account"></param>
        /// <returns></returns>
        AccountDto createOrUpdateAccount(AccountDto Account);

        /// <summary>
        /// updates/creates Wkt Account
        /// </summary>
        /// <param name="wktAccount"></param>
        /// <returns></returns>
        WktaccountDto createOrUpdateWktAccount(WktaccountDto wktAccount);

        /// <summary>
        /// updates/creates Partner
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        PartnerDto createOrUpdatePartner(PartnerDto partner);

        /// <summary>
        /// updates/creates BeteiligterDto
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        BeteiligterDto createOrUpdateBeteiligter(BeteiligterDto account);

         /// <summary>
        /// updates/creates PartnerRelation
        /// </summary>
        /// <param name="crmnm"></param>
        /// <returns></returns>
        CrmnmDto[] createOrUpdateCrmnm(CrmnmDto[] crmnm);

        /// <summary>
        /// updates/creates Konto
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        KontoDto createOrUpdateKonto(KontoDto Konto);

        /// <summary>
        /// updates/creates PartnerRelation
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        PtrelateDto[] createOrUpdatePtrelate(PtrelateDto[] ptrelate);

        /// <summary>
        /// updates/creates CrmProdukte
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        CrmprDto createOrUpdateCrmProdukte(CrmprDto crmpr);

        /// <summary>
        /// updates/creates Kategorien
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ItemcatDto createOrUpdateItemcat(ItemcatDto itemcat);

        /// <summary>
        /// updates/creates ItemKategorien
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ItemcatmDto createOrUpdateItemcatm(ItemcatmDto itemcatm);

        /// <summary>
        /// updates/creates Attachement
        /// </summary>
        /// <param name="fileatt"></param>
        /// <returns></returns>
        FileattDto createOrUpdateFileatt(FileattDto fileatt);

        /// <summary>
        /// updates/creates Document for dms
        /// </summary>
        /// <param name="dmsdoc"></param>
        /// <returns></returns>
        DmsdocDto createOrUpdateDmsdoc(DmsdocDto dmsdoc);

        /// <summary>
        /// updates/creates Reminder
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ReminderDto createOrUpdateReminder(ReminderDto reminder);

        /// <summary>
        /// updates/creates Recurrence
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        RecurrDto createOrUpdateRecurr(RecurrDto recurr);

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
        /// updates/creates Mailmsg
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        MailmsgDto createOrUpdateMailmsg(MailmsgDto mailmsg);

        /// <summary>
        /// updates/creates Appointment
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        ApptmtDto createOrUpdateApptmt(ApptmtDto apptmt);

        /// <summary>
        /// updates/creates Task
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        PtaskDto createOrUpdatePtask(PtaskDto ptask);

        /// <summary>
        /// updates/creates prkgroup
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        PrkgroupDto createOrUpdatePrkgroup(PrkgroupDto prkgroup);

        /// <summary>
        /// updates/creates prkgroupm
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        PrkgroupmDto createOrUpdatePrkgroupm(PrkgroupmDto[] prkgroup);

        /// <summary>
        /// updates/creates stickynote
        /// </summary>
        /// <param name="stickynotes"></param>
        /// <returns></returns>
        StickynoteDto[] createOrUpdateStickynotes(StickynoteDto[] stickynotes);

        /// <summary>
        /// updates/creates stickytype
        /// </summary>
        /// <param name="stickytype"></param>
        /// <returns></returns>
        StickytypeDto createOrUpdateStickytype(StickytypeDto stickytype);


        /// <summary>
        /// updates/creates ddlkppos
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos);


        /// <summary>
        /// updates/creates besuchsbericht
        /// </summary>
        /// <param name="besuchsbericht"></param>
        /// <returns></returns>
        BesuchsberichtDto createOrUpdateBesuchsbericht(BesuchsberichtDto besuchsbericht);


        /// <summary>
        /// updates/creates Wfsignature
        /// </summary>
        /// <param name="wfsignature"></param>
        /// <returns></returns>
        WfsignatureDto createOrUpdateWfsignature(WfsignatureDto wfsignature);

        /// <summary>
        /// updates/creates ZEK request
        /// </summary>
        /// <param name="zek"></param>
        /// <returns></returns>
        ZekDto createOrUpdateZek(ZekDto zek);


        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende
        /// </summary>
        /// <param name="memo">sysid, notiz, wftableSyscode, kurzbez, kategorie, ...</param>
        /// <param name="refTable">optional: name of table to be referenced by syswfmtable</param>
        /// <returns>saved memo</returns>
        MemoDto createOrUpdateMemo(MemoDto memo, String refTable = null);

         /// <summary>
        /// Creates or updates a puser
        /// </summary>
        /// <param name="puser"></param>
        /// <returns></returns>
        PuserDto createOrUpdatePuser(PuserDto puser);

        #endregion

        #region GET

        /// <summary>
        ///  Returns all Gview Details
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="gviewId"></param>
        /// <returns></returns>
        GviewDto getGviewDetails(long sysid, String gviewId, WorkflowContext ctx);

        /// <summary>
        ///  Returns all Staffelpositionstyp Details
        /// </summary>
        /// <param name="sysslpostyp"></param>
        /// <returns></returns>
        StaffelpositionstypDto getStaffelpositionstypDetails(long sysslpostyp);

        /// <summary>
        ///  Returns all Staffeltyp Details
        /// </summary>
        /// <param name="syssltyp"></param>
        /// <returns></returns>
        StaffeltypDto getStaffeltypDetails(long syssltyp);

        /// <summary>
        ///  Returns all Rolle Details
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        RolleDto getRolleDetails(long sysperole);

        /// <summary>
        ///  Returns all Rollentyp Details
        /// </summary>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        RollentypDto getRollentypDetails(long sysroletype);

        /// <summary>
        ///  Returns all Handelsgruppe Details
        /// </summary>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        HandelsgruppeDto getHandelsgruppeDetails(long sysprhgroup);

        /// <summary>
        ///  Returns all Vertriebskanal Details
        /// </summary>
        /// <param name="sysbchannel"></param>
        /// <returns></returns>
        VertriebskanalDto getVertriebskanalDetails(long sysbchannel);

        /// <summary>
        ///  Returns all Brand Details
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        BrandDto getBrandDetails(long sysbrand);

        /// <summary>
        ///  Returns all Rechnung Details
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        RechnungDto getRechnungDetails(long sysid);

        /// <summary>
        ///  Returns all Angobbrief Details
        /// </summary>
        /// <param name="sysangobbrief"></param>
        /// <returns></returns>
        AngobbriefDto getAngobbriefDetails(long sysangobbrief);

        /// <summary>
        ///  Returns all Zahlungsplan Details
        /// </summary>
        /// <param name="sysslpos"></param>
        /// <returns></returns>
        ZahlungsplanDto getZahlungsplanDetails(long sysslpos);

        /// <summary>
        ///  Returns all Fahrzeugbrief Details
        /// </summary>
        /// <param name="sysobbrief"></param>
        /// <returns></returns>
        FahrzeugbriefDto getFahrzeugbriefDetails(long sysobbrief);

        /// <summary>
        ///  Returns all Kalk Details
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        KalkDto getKalkDetails(long syskalk);

        /// <summary>
        ///  Returns all Person Details
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        PersonDto getPersonDetails(long sysperson);


        /// <summary>
        /// Liefert die SysPerole für einen Wfuser. 
        /// Achtung: Falls die lokale Variable schon die Perole beinhaltet, wird diese zurückgeliefert.
        /// </summary>
        /// <param name="sysWfuser">Wfuser welcher gesucht wird</param>
        /// <returns>Perole von dem User</returns>
        long getSysPerole(long sysWfuser);
        
        /// <summary>
        /// Returns all Updateinfos for the area and id
        /// (all indicators for one area)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        List<ExpUpdDto> getExpUpdates(String area, long areaid);

         /// <summary>
        /// Returns one Updateinfos for the area and id and sysexptyp
        /// (all indicators for one area)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        List<ExpUpdDto> getExpUpdate(String area, long areaid, long sysexptyp);

        /// <summary>
        /// Returns all Updateinfos for the area and ids
        /// (one indicator for all items of this area)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaids"></param>
        /// <returns></returns>
        List<ExpUpdDto> getExpDefUpdates(String area, long[] areaids);

        /// <summary>
        /// returns all indicators gui values for the area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        List<ExpdispDto> getExpdisps(String area, long areaid);

        /// <summary>
        /// returns one indicators gui value for the area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <param name="sysexptyp"></param>
        /// <returns></returns>
        List<ExpdispDto> getExpdisp(String area, long areaid, long sysexptyp);

		/// <summary>
		///  Returns all SLA Details
		/// </summary>
		/// <param name="sysid"></param>
		/// <param name="isocode"></param>
		/// <returns></returns>
		List<SlaDto> getSlaDetails (long sysid, string isocode);
		
		/// <summary>
        /// returns all default indicators
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        List<ExpdefDto> getExpdef(String area, long[] areaid);

        /// <summary>
        /// returns all indicators for the area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        List<ExptypDto> getExptypes(String area);

        /// <summary>
        /// returns the default indicator for the area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="defaultflag"></param>
        /// <returns></returns>
        ExptypDto getExptype(String area, int defaultflag);

        /// <summary>
        /// returns all indicator ranges for the area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        List<ExprangeDto> getExpranges(String area);

        /// <summary>
        /// returns select items for a rub entry
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        List<DdlkpposDto> getDdlkppos(long sysddlkpcol);

        /// <summary>
        /// returns the rub entries
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        List<DdlkpcolDto> getDdlkpcols(long sysddlkprub);

        /// <summary>
        /// returns a list of rubs for the area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        List<DdlkprubDto> getDdlkprubs(String code, String area);

        /// <summary>
        /// returns a list of Ddlkpspos (rub-values for a certain entity)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        List<DdlkpsposDto> getDdlkpspos(String area, long areaid);

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
        /// Returns all Recalc Details
        /// </summary>
        /// <param name="sysob"></param>
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
        /// <param name="sysob"></param>
        /// <returns></returns>
        RahmenDto getRahmenDetails(long sysrvt);

        /// <summary>
        /// Returns all Haendler Details
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        HaendlerDto getHaendlerDetails(long sysperson);

        /// <summary>
        /// Returns all KundeDetails
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        KundeDto getKundeDetails(long sysperson);

        /// <summary>
        /// Returns all LogDumpDetails
        /// </summary>
        /// <param name="logdump"></param>
        /// <returns></returns>
        LogDumpDto getLogDumpDetails(long logdump);
        
        /// <summary>
        /// Returns all ItDetails
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        ItDto getItDetails(long sysit);
        
        /// <summary>
        /// Returns all AngvarDetails
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        AngvarDto getAngvarDetails(long sysangvar);

        /// <summary>
        /// Returns all AngobDetails
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        AngobDto getAngobDetails(long sysangob);

        /// <summary>
        /// Returns OBDetail
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        ObDto getObDetails(long sysob);

        /// <summary>
        /// Returns all AngkalkDetails
        /// </summary>
        /// <param name="angkalk"></param>
        /// <returns></returns>
        AngkalkDto getAngkalkDetails(long sysangkalk);

        /// <summary>
        /// loads antkalk details from db
        /// </summary>
        /// <param name="sysantkalk"></param>
        /// <returns></returns>
        AntkalkDto getAntkalkDetails(long sysantkalk);

        /// <summary>
        /// Returns all OpportunityDetails
        /// </summary>
        /// <param name="sysopportunity"></param>
        /// <returns></returns>
        OpportunityDto getOpportunityDetails(long sysopportunity);

        /// <summary>
        /// Returns all Oppotask Details
        /// </summary>
        /// <param name="sysOppotask"></param>
        /// <returns></returns>
        OppotaskDto getOppotaskDetails(long sysOppotask);

        /// <summary>
        ///  Returns all FinanzierungDetails
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
        ///  Returns all ContactDetails
        /// </summary>
        /// <param name="syscontact"></param>
        /// <returns></returns>
        ContactDto getContactDetails(long syscontact);


        /// <summary>
        /// Returns the checklist data for the antrag
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        ChklistDto getChklistDetails(igetChecklistDetailDto input);

        /// <summary>
        ///  Returns all KontoDetails
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        KontoDto getKontoDetails(long syskonto);

        /// <summary>
        ///  Returns all CampDetails
        /// </summary>
        /// <param name="syscamp"></param>
        /// <returns></returns>
        CampDto getCampDetails(long syscamp);

        /// <summary>
        ///  Returns all WfuserDetails
        /// </summary>
        /// <param name="syscamp"></param>
        /// <returns></returns>
        WfuserDto getWfuserDetails(long syswfuser);

        /// <summary>
        ///  Returns all AdresseDetails
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
        ///  Returns all PtaskDetails
        /// </summary>
        /// <param name="sysadresse"></param>
        /// <returns></returns>
        PtaskDto getPtaskDetails(long sysptask);

        /// <summary>
        ///  Returns all ApptmtDetails
        /// </summary>
        /// <param name="sysapptmt"></param>
        /// <returns></returns>
        ApptmtDto getApptmtDetails(long sysapptmt);

        /// <summary>
        ///  Returns all Reminder Details
        /// </summary>
        /// <param name="sysreminder"></param>
        /// <returns></returns>
        ReminderDto getReminderDetails(long sysreminder);

        /// <summary>
        ///  Returns all Mailmsg Details
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
        ///  Returns all Prun Details
        /// </summary>
        /// <param name="sysprun"></param>
        /// <returns></returns>
        PrunDto getPrunDetails(long sysprun);

        /// <summary>
        ///  Returns all Prunart Details
        /// </summary>
        /// <param name="sysprun"></param>
        /// <returns></returns>
        PrunartDto getPrunartDetails(long sysprunart);

        /// <summary>
        ///  Returns all Fileatt Details
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        FileattDto getFileattDetails(long sysfileatt);

        /// <summary>
        ///  Returns all Fileatt Details by entity
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        FileattDto getFileattDetails(string area, long sysid);

         /// <summary>
        /// Returns all Dmsdoc Details
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        DmsdocDto getDmsdocDetails(long sysdmsdoc);

        /// <summary>
        /// Returns all Dmsdoc Details
        /// </summary>
        /// <param name="sysDmsdoc"></param>
        /// <returns></returns>
        List<DmsdocDto> getDmsdocDetails(string area, long sysid);

        /// <summary>
        ///  Returns all Prproduct Details
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        PrproductDto getPrproductDetails(long sysprproduct);

        ///  Returns all Itemcat Details
        /// </summary>
        /// <param name="sysitemcat"></param>
        /// <returns></returns>
        ItemcatDto getItemcatDetails(long sysitemcat);

        /// <summary>
        ///  Returns all Ctlang Details
        /// </summary>
        /// <param name="sysctlang"></param>
        /// <returns></returns>
        CtlangDto getCtlangDetails(long sysctlang);

        /// <summary>
        ///  Returns all land Details
        /// </summary>
        /// <param name="sysland"></param>
        /// <returns></returns>
        LandDto getLandDetails(long sysland);

		/// <summary>
		///  Returns all Branche Details
		/// </summary>
		/// <param name="sysbranche"></param>
		/// <returns></returns>
		BrancheDto getBrancheDetails (long sysbranche);

        /// <summary>
        ///  Returns all Account Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        AccountDto getAccountDetails(long sysaccount);

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
        ///  Returns all WktAccount Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        WktaccountDto getWktAccountDetails(long syswktaccount);

        /// <summary>
        ///  Returns all PartnerDto Details
        /// </summary>
        /// <param name="syspartner"></param>
        /// <returns></returns>
        PartnerDto getPartnerDetails(long syspartner);

        /// <summary>
        /// Returns all Beteiligter Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        BeteiligterDto getBeteiligterDetails(long syspartner);

        /// <summary>
        ///  Returns all Adrtp Details
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
        ///  Returns all Kontotp Details
        /// </summary>
        /// <param name="sysKontotp"></param>
        /// <returns></returns>
        KontotpDto getKontotpDetails(long sysKontotp);

        /// <summary>
        ///  Returns all Blz Details
        /// </summary>
        /// <param name="sysBlz"></param>
        /// <returns></returns>
        BlzDto getBlzDetails(long sysBlz);

        /// <summary>
        ///  Returns all Ptrelate Details
        /// </summary>
        /// <param name="sysPtrelate"></param>
        /// <returns></returns>
        PtrelateDto getPtrelateDetails(long sysPtrelate);

        /// <summary>
        /// Returns all Wfexec Details
        /// </summary>
        /// <param name="syswfexec"></param>
        /// <returns></returns>
        WfexecDto getWfexecDetails(long syswfexec);

         /// <summary>
        /// Returns all Crmnm Details
        /// </summary>
        /// <param name="sysCrmnm"></param>
        /// <returns></returns>
        CrmnmDto getCrmnmDetails(long syscrmnm);

        /// <summary>
        ///  Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysDdlkprub"></param>
        /// <returns></returns>
        DdlkprubDto getDdlkprubDetails(long sysDdlkprub);

        /// <summary>
        ///  Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol);

        /// <summary>
        ///  Returns all Ddlkpcol Details
        /// </summary>
        /// <param name="sysDdlkppos"></param>
        /// <returns></returns>
        DdlkpposDto getDdlkpposDetails(long sysDdlkppos);

        /// <summary>
        ///  Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysddlkpspos"></param>
        /// <returns></returns>
        DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos);

        /// <summary>
        ///  Returns all Camptp Details
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
        ///  Returns all Oppotp Details
        /// </summary>
        /// <param name="sysoppotp"></param>
        /// <returns></returns>
        OppotpDto getOppotpDetails(long sysoppotp);

        /// <summary>
        ///  Returns all Crmpr Details
        /// </summary>
        /// <param name="syscrmpr"></param>
        /// <returns></returns>
        CrmprDto getCrmprDetails(long syscrmpr);

        /// <summary>
        ///  Returns all contacttp Details
        /// </summary>
        /// <param name="syscontacttp"></param>
        /// <returns></returns>
        ContacttpDto getContacttpDetails(long syscontacttp);

        /// <summary>
        ///  Returns all Itemcatm Details
        /// </summary>
        /// <param name="sysitemcat"></param>
        /// <returns></returns>
        ItemcatmDto getItemcatmDetails(long sysitemcatm);

        /// <summary>
        ///  Returns all Recurr Details
        /// </summary>
        /// <param name="sysrecurr"></param>
        /// <returns></returns>
        RecurrDto getRecurrDetails(long sysrecurr);

        /// <summary>
        ///  Returns all Ptype Details
        /// </summary>
        /// <param name="sysptype"></param>
        /// <returns></returns>
        PtypeDto getPtypeDetails(long sysptype);

        /// <summary>
        ///  Returns all Prunstep Details
        /// </summary>
        /// <param name="sysprunstep"></param>
        /// <returns></returns>
        PrunstepDto getPrunstepDetails(long sysprunstep);

        /// <summary>
        ///  Returns all Pstep Details
        /// </summary>
        /// <param name="syspstep"></param>
        /// <returns></returns>
        PstepDto getPstepDetails(long syspstep);

        /// <summary>
        ///  Returns all Prkgroup Details
        /// </summary>
        /// <param name="sysprkgroup"></param>
        /// <returns></returns>
        PrkgroupDto getPrkgroupDetails(long sysprkgroup);

        ///  Returns all Prkgroupm Details
        /// </summary>
        /// <param name="sysprkgroupm"></param>
        /// <returns></returns>
        PrkgroupmDto getPrkgroupmDetails(long sysprkgroupm);

        /// <summary>
        ///  Returns all Prkgroupz Details
        /// </summary>
        /// <param name="sysprkgroupz"></param>
        /// <returns></returns>
        PrkgroupzDto getPrkgroupzDetails(long sysprkgroupz);

        /// <summary>
        ///  Returns all Prkgroups Details
        /// </summary>
        /// <param name="sysprkgroups"></param>
        /// <returns></returns>
        PrkgroupsDto getPrkgroupsDetails(long sysprkgroups);

        /// <summary>
        ///  Returns all Seg Details
        /// </summary>
        /// <param name="ssysSeg"></param>
        /// <returns></returns>
        SegDto getSegDetails(long sysSeg);

        /// <summary>
        ///  Returns all Segc Details
        /// </summary>
        /// <param name="syssegc"></param>
        /// <returns></returns>
        SegcDto getSegcDetails(long syssegc);

        /// <summary>
        ///  Returns all Stickynote Details
        /// </summary>
        /// <param name="stikynote"></param>
        /// <returns></returns>
        StickynoteDto getStickynoteDetails(long sysstickynote);

        /// <summary>
        ///  Returns all Stickytype Details
        /// </summary>
        /// <param name="stikytype"></param>
        /// <returns></returns>
        StickytypeDto getStickytypeDetails(long sysstickytype);


        /// <summary>
        ///  Returns all Wfsignature Details
        /// </summary>
        /// <param name="stikytype"></param>
        /// <returns></returns>
        WfsignatureDto getWfsignatureDetail(long input);
        ///  Returns Angebot Details
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        AngebotDto getAngebotDetails(long sysangebot);

        /// <summary>
        /// returns product info for angebot
        /// </summary>
        /// <param name="sysang"></param>
        /// <returns></returns>
        ProduktInfoDto getProduktInfoAngebotDetails(long sysang);

        /// <summary>
        /// returns product info for antrag
        /// </summary>
        /// <param name="sysant"></param>
        /// <returns></returns>
        ProduktInfoDto getProduktInfoAntragDetails(long sysant);

        ///  Returns Angebot Details
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        AntragDto getAntragDetails(long sysantrag);

        ///  Returns all Vertrag
        ///  okDetails
        /// </summary>
        /// <param name="sysvertrag"></param>
        /// <returns></returns>
        VertragDto getVertragDetails(long sysvertrag);

        ///  Returns all Vorgang
        /// </summary>
        /// <param name="sysvertrag"></param>
        /// <returns></returns>
        VorgangDto getVorgangDetails(long sysId, string area);

        /// <summary>
        /// return all adm details
        /// </summary>
        /// <param name="sysAdmadd"></param>
        /// <returns></returns>
        AdmaddDto getAdmaddDetail(long sysAdmadd);

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
        /// Returns all ZEK requests
        /// </summary>
        /// <param name="syszek"></param>
        /// <returns></returns>
        ZekDto getZek(long syszek);

        /// <summary>
        /// check if entry of given type with given id exists in database
        /// </summary>
        /// <param name="area">entry type</param>
        /// <param name="sysid">primary key</param>
        /// <returns>exists</returns>
        bool getExists(string area, long sysid);

        /// <summary>
        /// get sysid by "nice" number and area
        /// </summary>
        /// <param name="area">entry type</param>
        /// <param name="number">"nice" unique number</param>
        /// <returns>sysid</returns>
        long getSysidFromNumber(string area, string number);

        /// <summary>
        /// returns process data
        /// </summary>
        /// <param name="sysprocess">process id</param>
        /// <returns>process data</returns>
        ProcessDto getProcess(long sysprocess);

        /// <summary>
        /// return Waehrung 
        /// </summary>
        /// <param name="sysWaehrung"></param>
        /// <returns></returns>
        WaehrungDto getWaehrungDetail(long? sysWaehrung);

        #endregion GET


        /// <summary>
        ///  Returns all Fileatts
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        List<FileattDto> getFileatts(string area,long sysid);

        

        /// <summary>
        /// Returns all Wfsignature Details over type
        /// </summary>
        /// <param name="wfsignatureType"></param>
        /// <param name="sysWfUser"></param>
        /// <returns></returns>
        WfsignatureDto getWfsignatureDetail(WfsignatureType wfsignatureType, long sysWfUser);

        /// <summary>
        /// Löscht eine Entity
        /// </summary>
        /// <param name="area">Area</param>
        /// <param name="sysid">Sysid</param>
        void deleteEntity(string area, long sysid);

        /// <summary>
        /// Fetches all Auflagen for Antrag
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        List<RatingAuflageDto> getAuflagen(long sysantrag, String isoCode);

        /// <summary>
        /// Fetches all Rules for Antrag
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        List<AuskunftRegelDto> getAuskunftRegeln(long sysantrag, String isoCode);

        /// <summary>
        /// update LegitimationMethodeCode in ITPKZ or ITUKZ
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
        /// Returns the it id for a person
        /// uses only it in peuni of current perole
        /// when multiple it's found uses the most recent
        /// </summary>
        /// <param name="sysperson"></param>
        /// <param name="peuni">true to use sightfield</param>
        /// <returns></returns>
        long getItIdFromPerson(long sysperson, bool peuni);

         /// <summary>
        /// Returns all active insurances for the offer
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        List<Cic.OpenOne.Common.DTO.AngAntVsDto> getAngebotVersicherung(long sysangebot);

        /// <summary>
        /// accept EPOS Conditions of current user
        /// </summary>
        void acceptEPOSConditions();

        /// <summary>
        /// Set Absender/Empänger in Riskmail
        /// </summary>
        /// <param name="input"></param>
        void setRiskMailContact(ref isendRiskmailDto input);

        /// <summary>
        /// Performs the 4-eyes principle for the area/id setting the given result of the current user
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        void fourEyesPrinciple(ifourEyesDto input, ofourEyesDto output);
    }
}
