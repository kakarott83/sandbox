using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;
using Cic.One.Web.DAO;
using Cic.One.DTO.BN;


namespace Cic.One.Web.BO
{
    public abstract class AbstractEntityBo : IEntityBo
    {
        protected IEntityDao dao;
        public long SysPerole { get; set; }

        public AbstractEntityBo(IEntityDao dao)
        {
            this.dao = dao;
        }

        #region CREATEORUPDATE

        /// <summary>
        /// updates/creates prunart data
        /// </summary>
        /// <param name="prunart"></param>
        /// <returns></returns>
        public abstract PrunartDto createOrUpdatePrunart(PrunartDto prunart);

        /// <summary>
        /// updates/creates Checklist data
        /// </summary>
        /// <param name="chklist"></param>
        /// <returns></returns>
        public abstract ChklistDto createOrUpdateChklist(ChklistDto chklist);

        /// <summary>
        /// updates/creates Dokvalidation
        /// </summary>
        /// <param name="dokval"></param>
        /// <returns></returns>
        public abstract DokvalDto createOrUpdateDokval(DokvalDto dokval);

        /// <summary>
        /// updates/creates Gview
        /// </summary>
        /// <param name="gview"></param>
        /// <returns></returns>
        public abstract GviewDto createOrUpdateGview(GviewDto gview);

		/// <summary>
		/// updates/creates Pread
		/// </summary>
		/// <param name="pread"></param>
		/// <returns></returns>
		public abstract PreadDto createOrUpdatePread (PreadDto pread);

		/// <summary>
        /// updates/creates Staffelpositionstyp
        /// </summary>
        /// <param name="staffelpositionstyp"></param>
        /// <returns></returns>
        public abstract StaffelpositionstypDto createOrUpdateStaffelpositionstyp(StaffelpositionstypDto staffelpositionstyp);

        /// <summary>
        /// updates/creates Staffeltyp
        /// </summary>
        /// <param name="staffeltyp"></param>
        /// <returns></returns>
        public abstract StaffeltypDto createOrUpdateStaffeltyp(StaffeltypDto staffeltyp);

        /// <summary>
        /// updates/creates Rolle
        /// </summary>
        /// <param name="rolle"></param>
        /// <returns></returns>
        public abstract RolleDto createOrUpdateRolle(RolleDto rolle);

        /// <summary>
        /// updates/creates Rollentyp
        /// </summary>
        /// <param name="rollentyp"></param>
        /// <returns></returns>
        public abstract RollentypDto createOrUpdateRollentyp(RollentypDto rollentyp);

        /// <summary>
        /// updates/creates Handelsgruppe
        /// </summary>
        /// <param name="handelsgruppe"></param>
        /// <returns></returns>
        public abstract HandelsgruppeDto createOrUpdateHandelsgruppe(HandelsgruppeDto handelsgruppe);

        /// <summary>
        /// updates/creates Vertriebskanal
        /// </summary>
        /// <param name="vertriebskanal"></param>
        /// <returns></returns>
        public abstract VertriebskanalDto createOrUpdateVertriebskanal(VertriebskanalDto vertriebskanal);

        /// <summary>
        /// updates/creates Brand
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public abstract BrandDto createOrUpdateBrand(BrandDto brand);

        /// <summary>
        /// updates/creates Rechnung
        /// </summary>
        /// <param name="rechnung"></param>
        /// <returns></returns>
        public abstract RechnungDto createOrUpdateRechnung(RechnungDto rechnung);

        /// <summary>
        /// updates/creates Angobbrief
        /// </summary>
        /// <param name="angobbrief"></param>
        /// <returns></returns>
        public abstract AngobbriefDto createOrUpdateAngobbrief(AngobbriefDto angobbrief);

        /// <summary>
        /// updates/creates Zahlungsplan
        /// </summary>
        /// <param name="zahlungsplan"></param>
        /// <returns></returns>
        public abstract ZahlungsplanDto createOrUpdateZahlungsplan(ZahlungsplanDto zahlungsplan);

		/// <summary>
		/// updates/creates Kreditlinie
		/// </summary>
		/// <param name="zahlungsplan"></param>
		/// <returns></returns>
		public abstract KreditlinieDto createOrUpdateKreditlinie (KreditlinieDto kreditlinie);

		/// <summary>
        /// updates/creates Fahrzeugbrief
        /// </summary>
        /// <param name="fahrzeugbrief"></param>
        /// <returns></returns>
        public abstract FahrzeugbriefDto createOrUpdateFahrzeugbrief(FahrzeugbriefDto fahrzeugbrief);

        /// <summary>
        /// updates/creates Kalk
        /// </summary>
        /// <param name="kalk"></param>
        /// <returns></returns>
        public abstract KalkDto createOrUpdateKalk(KalkDto kalk);

        /// <summary>
        /// updates/creates Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public abstract PersonDto createOrUpdatePerson(PersonDto person);

        /// <summary>
        /// updates/creates expval
        /// </summary>
        /// <param name="expval"></param>
        /// <returns></returns>
        public abstract ExpvalDto createOrUpdateExpval(ExpvalDto expval);

        /// <summary>
        /// updates/creates Finanzierung
        /// </summary>
        /// <param name="finanzierung"></param>
        /// <returns></returns>
        public abstract FinanzierungDto createOrUpdateFinanzierung(FinanzierungDto finanzierung, int saveMode);

        /// <summary>
        /// updates/creates RechnungFaellig
        /// </summary>
        /// <param name="rechnungFaellig"></param>
        /// <returns></returns>
        public abstract RechnungFaelligDto createOrUpdateRechnungFaellig(RechnungFaelligDto rechnungFaellig);

        /// <summary>
        /// updates/creates Tilgung
        /// </summary>
        /// <param name="tilgung"></param>
        /// <returns></returns>
        public abstract TilgungDto createOrUpdateTilgung(TilgungDto tilgung);


        /// <summary>
        /// updates/creates Objekt
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        public abstract ObDto createOrUpdateObjekt(ObDto objekt);

        /// <summary>
        /// updates/creates Objekt
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        public abstract ObDto createOrUpdateHEKObjekt(ObDto objekt);

        // <summary>
        /// updates/creates Recalc
        /// </summary>
        /// <param name="recalc"></param>
        /// <returns></returns>
        public abstract RecalcDto createOrUpdateRecalc(RecalcDto recalc);

        // <summary>
        /// updates/creates Mycalc
        /// </summary>
        /// <param name="mycalc"></param>
        /// <returns></returns>
        public abstract MycalcDto createOrUpdateMycalc(MycalcDto mycalc);

        // <summary>
        /// updates/creates Mycalcfs
        /// </summary>
        /// <param name="mycalcfs"></param>
        /// <returns></returns>
        public abstract MycalcfsDto createOrUpdateMycalcfs(MycalcfsDto mycalcfs);

        /// <summary>
        /// updates/creates Rahmen
        /// </summary>
        /// <param name="rahmen"></param>
        /// <returns></returns>
        public abstract RahmenDto createOrUpdateRahmen(RahmenDto rahmen);

        /// <summary>
        /// updates/creates Haendler
        /// </summary>
        /// <param name="haendler"></param>
        /// <returns></returns>
        public abstract HaendlerDto createOrUpdateHaendler(HaendlerDto haendler);

        /// <summary>
        /// Returns the image-url parameters for the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        public abstract String getEntityLink(String entity, long sysid, long syswfuser, String vlmcode);

        /// <summary>
        /// Returns the image-url parameters for the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        public abstract EntityIconDto getEntityIcon(String entity, long sysid, long syswfuser, String vlmcode);

        /// <summary>
        /// Returns the icon for the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        public abstract List<EntityIconDto> getEntityIcons(String entity, List<long> ids, long syswfuser, String vlmcode);

        /// <summary>
        /// updates/creates Zusatzdaten
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        public abstract DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos);

        /// <summary>
        /// updates/creates Kunde
        /// </summary>
        /// <param name="kunde"></param>
        /// <returns></returns>
        public abstract KundeDto createOrUpdateKunde(KundeDto kunde);

        /// <summary>
        /// updates/creates It
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public abstract ItDto createOrUpdateIt(ItDto it);

        /// <summary>
        /// updates/creates Itkonto
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract ItkontoDto createOrUpdateItkonto(ItkontoDto itkonto);

        /// <summary>
        /// updates/creates Itadresse
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract ItadresseDto createOrUpdateItadresse(ItadresseDto itadresse);

        /// <summary>
        /// updates/creates Angvar
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        public abstract AngvarDto createOrUpdateAngvar(AngvarDto angvar);

        /// <summary>
        /// updates/creates Angebot
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        public abstract AngebotDto createOrUpdateAngebot(AngebotDto angebot);


        /// <summary>
        /// updates/creates Antrag
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        public abstract AntragDto createOrUpdateAntrag(AntragDto antrag);

        /// <summary>
        /// updates/creates Angob
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        public abstract AngobDto createOrUpdateAngob(AngobDto angob);

        /// <summary>
        /// updates/creates Antob
        /// </summary>
        /// <param name="antob"></param>
        /// <returns></returns>
        abstract public AntobDto createOrUpdateAntob(AntobDto antob);

        /// <summary>
        /// updates/creates Angkalk
        /// </summary>
        /// <param name="angkalk"></param>
        /// <returns></returns>
        public abstract AngkalkDto createOrUpdateAngkalk(AngkalkDto angkalk);

        /// <summary>
        /// updates/creates Antkalk
        /// </summary>
        /// <param name="antkalk"></param>
        /// <returns></returns>
        abstract public AntkalkDto createOrUpdateAntkalk(AntkalkDto antkalk);

        /// 
        /// <summary>
        /// updates/creates Contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public abstract ContactDto createOrUpdateContact(ContactDto contact);

        /// <summary>
        /// updates/creates Contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public abstract void createOrUpdateContacts(ContactDto contact, String personSQL);

        /// <summary>
        /// updates/creates Adresse
        /// </summary>
        /// <param name="adresse"></param>
        /// <returns></returns>
        public abstract AdresseDto createOrUpdateAdresse(AdresseDto adresse);

        /// <summary>
        /// updates/creates Camp
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public abstract CampDto createOrUpdateCamp(CampDto camp);

        /// <summary>
        /// updates/creates Oppo
        /// </summary>
        /// <param name="Oppo"></param>
        /// <returns></returns>
        public abstract OpportunityDto createOrUpdateOppo(OpportunityDto Oppo);

        /// <summary>
        /// updates/creates Oppotask
        /// </summary>
        /// <param name="oppotask"></param>
        /// <returns></returns>
        public abstract OppotaskDto createOrUpdateOppotask(OppotaskDto oppotask);

        /// <summary>
        /// updates/creates Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public abstract AccountDto createOrUpdateAccount(AccountDto account);

        /// <summary>
        /// updates/creates WktAccount
        /// </summary>
        /// <param name="wktaccount"></param>
        /// <returns></returns>
        public abstract WktaccountDto createOrUpdateWktAccount(WktaccountDto wktaccount);

        /// <summary>
        /// Returns all  Partner Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public abstract PartnerDto getPartnerDetails(long sysaccount);

        /// <summary>
        /// Returns all  Beteiligter Details
        /// </summary>
        /// <param name="sysbeteiligter"></param>
        /// <returns></returns>
        public abstract BeteiligterDto getBeteiligterDetails(long sysbeteiligter);

        /// <summary>
        ///  Returns all ItkontoDetails
        /// </summary>
        /// <param name="sysitkonto"></param>
        /// <returns></returns>
        public abstract ItkontoDto getItkontoDetails(long sysitkonto);

        /// <summary>
        ///  Returns all ItadresseDetails
        /// </summary>
        /// <param name="sysitadresse"></param>
        /// <returns></returns>
        public abstract ItadresseDto getItadresseDetails(long sysitadresse);

        /// <summary>
        /// updates/creates Beteiligter
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public abstract BeteiligterDto createOrUpdateBeteiligter(BeteiligterDto partner);

        /// <summary>
        /// updates/creates Partner
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public abstract PartnerDto createOrUpdatePartner(PartnerDto partner);

        /// <summary>
        /// updates/creates Konto
        /// </summary>
        /// <param name="konto"></param>
        /// <returns></returns>
        public abstract KontoDto createOrUpdateKonto(KontoDto konto);

        /// <summary>
        /// updates/creates Kundengruppen
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        public abstract PrkgroupDto createOrUpdatePrkgroup(PrkgroupDto prkgroup);

        /// <summary>
        /// updates/creates Kundengruppenzuordnungen
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        public abstract PrkgroupmDto createOrUpdatePrkgroupm(PrkgroupmDto[] prkgroup);

        /// <summary>
        /// updates/creates PartnerRelation
        /// </summary>
        /// <param name="ptrelate"></param>
        /// <returns></returns>
        public abstract PtrelateDto[] createOrUpdatePtrelate(PtrelateDto[] ptrelate);

        /// <summary>
        /// updates/creates BeteiligterRelation
        /// </summary>
        /// <param name="ptrelate"></param>
        /// <returns></returns>
        public abstract CrmnmDto[] createOrUpdateCrmnm(CrmnmDto[] crmnm);

        /// <summary>
        /// updates/creates CrmProdukte
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract CrmprDto createOrUpdateCrmProdukte(CrmprDto crmpr);

        /// <summary>
        /// updates/creates Checklist
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract PrunDto createOrUpdatePrun(PrunDto prun);

        /// <summary>
        /// updates/creates Checklisttype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract PtypeDto createOrUpdatePtype(PtypeDto ptype);

        /// <summary>
        /// updates/creates Check
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract PrunstepDto createOrUpdatePrunstep(PrunstepDto prunstep);

        /// <summary>
        /// updates/creates Checktype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract PstepDto createOrUpdatePstep(PstepDto pstep);

        /// <summary>
        /// updates/creates Segment
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract SegDto createOrUpdateSeg(SegDto seg);

        /// <summary>
        /// updates/creates SegmentKampagne
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract SegcDto createOrUpdateSegc(SegcDto segc);

        /// <summary>
        /// updates/creates Stickynotes
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public abstract StickynoteDto[] createOrUpdateStickynotes(StickynoteDto[] notes);


        /// <summary>
        /// updates/creates Stickytype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract StickytypeDto createOrUpdateStickytype(StickytypeDto sysStickytypeDto);


        /// <summary>
        /// updates/creates Besuchsbericht
        /// </summary>
        /// <param name="besuchsbericht"></param>
        /// <returns></returns>
        public abstract BesuchsberichtDto createOrUpdateBesuchsbericht(BesuchsberichtDto besuchsbericht);

        ///// <summary>
        ///// updates/creates Dmsdoc
        ///// </summary>
        ///// <param name="dmsdoc"></param>
        ///// <returns></returns>
        public abstract DmsdocDto createOrUpdateDmsdoc(DmsdocDto dmsdoc);

        #region Mail

        ///// <summary>
        ///// updates/creates Kategorien
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public abstract ItemcatDto createOrUpdateItemcat(ItemcatDto itemcat);

        ///// <summary>
        ///// updates/creates ItemKategorien
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public abstract ItemcatmDto createOrUpdateItemcatm(ItemcatmDto itemcatm);

        ///// <summary>
        ///// updates/creates Attachement
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public abstract FileattDto createOrUpdateFileatt(FileattDto fileatt);

        ///// <summary>
        ///// updates/creates Reminder
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public abstract ReminderDto createOrUpdateReminder(ReminderDto reminder);

        ///// <summary>
        ///// updates/creates Recurrence
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public abstract RecurrDto createOrUpdateRecurr(RecurrDto recurr);

        ///// <summary>
        ///// updates/creates MailMsg
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public abstract MailmsgDto createOrUpdateMailmsg(MailmsgDto mailmsg);

        ///// <summary>
        ///// updates/creates Apptmt
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public abstract ApptmtDto createOrUpdateApptmt(ApptmtDto apptmt);

        ///// <summary>
        ///// updates/creates Ptask
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public abstract PtaskDto createOrUpdatePtask(PtaskDto ptask);

        #endregion

        /// <summary>
        /// updates/creates from generic Object
        /// ACHTUNG: Kann fehleranfällig sein, wenn die Klassen umbenannt werden/Methoden nicht richtig benannt wurden.
        /// </summary>
        /// <typeparam name="T">Typ, von welchem die Details geladen werden sollen</typeparam>
        /// <param name="item">Item, welches gespeichert werden soll</param>
        /// <returns></returns>
        public T createOrUpdate<T>(T item)
        {
            Type mytype = typeof(AbstractEntityBo);
            Type searched = typeof(T);
            string name = searched.Name.Substring(0, searched.Name.Length - 3);
            System.Reflection.MethodInfo mi = mytype.GetMethod("createOrUpdate" + name);
            if (mi != null)
                return (T)mi.Invoke(this, new object[] { item });
            else
                throw new Exception("Type not supported for createOrUpdate: " + typeof(T));
        }

        /// <summary>
        /// updates/creates ZEK
        /// </summary>
        /// <param name="zek"></param>
        /// <returns></returns>
        public abstract ZekDto createOrUpdateZek(ZekDto zek);
        
        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende
        /// </summary>
        /// <param name="memo">sysid, notiz, wftableSyscode, kurzbez, kategorie, ...</param>
        /// <param name="refTable">optional: name of table to be referenced by memo.syswfmtable</param>
        public abstract MemoDto createOrUpdateMemo(MemoDto memo, String refTable = null);

        /// <summary>
        /// Creates or updates a puser
        /// </summary>
        /// <param name="puser"></param>
        /// <returns></returns>
        public abstract PuserDto createOrUpdatePuser(PuserDto puser);

        /// <summary>
        /// Creates or updates a clarification
        /// </summary>
        /// <param name="clarification"></param>
        /// <returns></returns>
        public abstract ClarificationDto createOrUpdateClarification(ClarificationDto clarification);

        #endregion

        #region GET


        /// <summary>
        /// Returns the document validation Details
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public abstract DokvalDto getDokvalDetails(String area, long sysId);

        /// <summary>
        /// Returns the checklist data for the antrag
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public abstract ChklistDto getChklistDetails(igetChecklistDetailDto sysId);

        /// <summary>
        /// Returns the checklist art
        /// </summary>
        /// <param name="sysprunart"></param>
        /// <returns></returns>
        public abstract PrunartDto getPrunartDetails(long sysprunart);

        /// <summary>
        /// Returns all Gviewtyp Details
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="gviewId"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public abstract GviewDto getGviewDetails(long sysId, String gviewId, WorkflowContext ctx);

        /// <summary>
        /// Returns all Staffelpositionstyp Details
        /// </summary>
        /// <param name="sysslpostyp"></param>
        /// <returns></returns>
        public abstract StaffelpositionstypDto getStaffelpositionstypDetails(long sysslpostyp);

        /// <summary>
        /// Returns all Staffeltyp Details
        /// </summary>
        /// <param name="syssltyp"></param>
        /// <returns></returns>
        public abstract StaffeltypDto getStaffeltypDetails(long syssltyp);

        /// <summary>
        /// Returns all Rolle Details
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public abstract RolleDto getRolleDetails(long sysperole);

        /// <summary>
        /// Returns all Rollentyp Details
        /// </summary>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        public abstract RollentypDto getRollentypDetails(long sysroletype);

        /// <summary>
        /// Returns all Handelsgruppe Details
        /// </summary>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        public abstract HandelsgruppeDto getHandelsgruppeDetails(long sysprhgroup);

        /// <summary>
        /// Returns all Vertriebskanal Details
        /// </summary>
        /// <param name="sysbchannel"></param>
        /// <returns></returns>
        public abstract VertriebskanalDto getVertriebskanalDetails(long sysbchannel);

        /// <summary>
        /// Returns all Brand Details
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        public abstract BrandDto getBrandDetails(long sysbrand);

        /// <summary>
        /// Returns all Rechnung Details
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public abstract RechnungDto getRechnungDetails(long sysid);

        /// <summary>
        /// Returns all Angobbrief Details
        /// </summary>
        /// <param name="sysangobbrief"></param>
        /// <returns></returns>
        public abstract AngobbriefDto getAngobbriefDetails(long sysangobbrief);

        /// <summary>
        /// Returns all Zahlungsplan Details
        /// </summary>
        /// <param name="sysslpos"></param>
        /// <returns></returns>
        public abstract ZahlungsplanDto getZahlungsplanDetails(long sysslpos);

        /// <summary>
        /// Returns all Fahrzeugbrief Details
        /// </summary>
        /// <param name="sysobbrief"></param>
        /// <returns></returns>
        public abstract FahrzeugbriefDto getFahrzeugbriefDetails(long sysobbrief);

        /// <summary>
        /// Returns all Kalk Details
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        public abstract KalkDto getKalkDetails(long syskalk);

        /// <summary>
        /// Returns all Person Details
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public abstract PersonDto getPersonDetails(long sysperson);

        /// <summary>
        /// Returns all Printset Details
        /// </summary>
        /// <param name="sysprintset"></param>
        /// <returns></returns>
        public abstract PrintsetDto getPrintsetDetails(long sysprintset);

        /// <summary>
        /// Returns all Prtlgset Details
        /// </summary>
        /// <param name="sysprtlgset"></param>
        /// <returns></returns>
        public abstract PrtlgsetDto getPrtlgsetDetails(long sysprtlgset);


        /// <summary>
        /// Returns all Nkonto Details
        /// </summary>
        /// <param name="sysnkonto"></param>
        /// <returns></returns>
        public abstract NkontoDto getNkontoDetails(long sysnkonto);

        /// <summary>
        /// Returns AdmaddDto Details
        /// </summary>
        /// <param name="sysAdmadd"></param>
        /// <returns></returns>
        abstract public AdmaddDto getAdmaddDetail(long sysAdmadd);

        /// <summary>
        /// Returns all Indicator Details for the area
        /// </summary>
        /// <param name="exptyp"></param>
        /// <returns></returns>
        abstract public List<ExptypDto> getExptypDetails(igetExptypDto exptyp);

		/// <summary>
		/// Returns all Indicator Value Details for the area
		/// </summary>
		/// <param name="expdisp"></param>
		/// <returns></returns>
		abstract public List<ExpdispDto> getExpdispDetails (igetExpdispDto expdisp);

		/// <summary>
		/// Returns all SLA Details for the sysid (Ang/Ant)
		/// </summary>
		/// <param name="slaid"></param>
		/// <returns></returns>
		abstract public List<SlaDto> getSlaDetails (igetSlaDto slaid);

		/// <summary>
        /// Returns all Indicator Value Details for the area
        /// </summary>
        /// <param name="expdef"></param>
        /// <returns></returns>
        abstract public List<ExpdefDto> getExpdefDetails(igetExpdefDto expdef);

        /// <summary>
        /// Returns all Rub Infos
        /// </summary>
        /// <param name="iRub"></param>
        /// <returns></returns>
        public abstract List<DdlkprubDto> getRubInfo(igetRubDto iRub);

        /// <summary>
        /// Returns all ObjektDetails
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract ObjektDto getObjektDetails(long sysob);

        /// <summary>
        /// Returns all ObjektDetails
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public abstract ObtypDto getObtypDetails(long sysobtyp);

        /// <summary>
        /// Returns all Obkat Details
        /// </summary>
        /// <param name="sysobkat"></param>
        /// <returns></returns>
        public abstract ObkatDto getObkatDetails(long sysobkat);

        /// <summary>
        /// Returns all RecalcDetails
        /// </summary>
        /// <param name="sysrecalc"></param>
        /// <returns></returns>
        public abstract RecalcDto getRecalcDetails(long sysrecalc);

        /// <summary>
        /// Returns all Mycalc Details
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        public abstract MycalcDto getMycalcDetails(long sysmycalc);

        /// <summary>
        /// Returns all RahmenDetails
        /// </summary>
        /// <param name="sysrvt"></param>
        /// <returns></returns>
        public abstract RahmenDto getRahmenDetails(long sysrvt);

        /// <summary>
        /// Returns all HaendlerDetails
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public abstract HaendlerDto getHaendlerDetails(long sysperson);

        /// <summary>
        /// Returns all Kunden Details
        /// </summary>
        /// <param name=" sysperson"></param>
        /// <returns></returns>
        public abstract KundeDto getKundeDetails(long sysperson);

        /// <summary>
        /// updates/creates It
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public abstract ItDto getItDetails(long sysit);

        /// <summary>
        /// Returns all AngvarDetails
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        public abstract AngvarDto getAngvarDetails(long sysangvar);

        /// <summary>
        /// Returns all AngobDetails
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        public abstract AngobDto getAngobDetails(long sysangob);

        /// <summary>
        /// Returns  ObDto Details
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public abstract ObDto getObDetails(long sysob);

        /// <summary>
        /// Returns all AngkalkDetails
        /// </summary>
        /// <param name="angkalk"></param>
        /// <returns></returns>
        public abstract AngkalkDto getAngkalkDetails(long sysangkalk);

        /// <summary>
        /// Returns all AntkalkDetails
        /// </summary>
        /// <param name="antkalk"></param>
        /// <returns></returns>
        public abstract AntkalkDto getAntkalkDetails(long sysantkalk);

        /// <summary>
        /// Returns all OpportunityDetails
        /// </summary>
        /// <param name="sysopportunity"></param>
        /// <returns></returns>
        public abstract OpportunityDto getOpportunityDetails(long sysopportunity);

        /// <summary>
        /// Returns all OppotaskDetails
        /// </summary>
        /// <param name="sysOppotask"></param>
        /// <returns></returns>
        public abstract OppotaskDto getOppotaskDetails(long sysOppotask);



        /// <summary>
        /// Returns all ContactDetails
        /// </summary>
        /// <param name="syscontact"></param>
        /// <returns></returns>
        public abstract ContactDto getContactDetails(long syscontact);


        /// <summary>
        /// Returns all KontoDetails
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        public abstract KontoDto getKontoDetails(long syskonto);

        /// <summary>
        /// Returns all LogDumpDetails
        /// </summary>
        /// <param name="logdump"></param>
        /// <returns></returns>
        public abstract LogDumpDto getLogDumpDetails(long logdump);

        /// <summary>
        /// Returns all CampDetails
        /// </summary>
        /// <param name="syscamp"></param>
        /// <returns></returns>
        public abstract CampDto getCampDetails(long syscamp);

        /// <summary>
        /// Returns all WfuserDetails
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public abstract WfuserDto getWfuserDetails(long syswfuser);

        /// <summary>
        /// Returns all AdresseDetails
        /// </summary>
        /// <param name="sysadresse"></param>
        /// <returns></returns>
        public abstract AdresseDto getAdresseDetails(long sysadresse);

        /// <summary>
        /// Returns the Eaihot
        /// </summary>
        /// <param name="syseaihot"></param>
        /// <returns></returns>
        public abstract EaihotDto getEaihotDetails(long syseaihot);

        /// <summary>
        /// Returns all FinanzierungDetails
        /// </summary>
        /// <param name="sysnkk"></param>
        /// <returns></returns>
        public abstract FinanzierungDto getFinanzierungDetails(long sysnkk);

        /// <summary>
        /// Returns Kreditlinie
        /// </summary>
        /// <param name="sysklinie"></param>
        /// <returns></returns>
        public abstract KreditlinieDto getKreditlinieDetail(long sysklinie);
      
        /// <summary>
        /// Returns all PtaskDetails
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        public abstract PtaskDto getPtaskDetails(long sysptask);

        /// <summary>
        /// Returns all ApptmtDetails
        /// </summary>
        /// <param name="sysapptmt"></param>
        /// <returns></returns>
        public abstract ApptmtDto getApptmtDetails(long sysapptmt);

        /// <summary>
        /// Returns all ReminderDetails
        /// </summary>
        /// <param name="sysreminder"></param>
        /// <returns></returns>
        public abstract ReminderDto getReminderDetails(long sysreminder);

        /// <summary>
        /// Returns all Mailmsg Details
        /// </summary>
        /// <param name="sysmailmsg"></param>
        /// <returns></returns>
        public abstract MailmsgDto getMailmsgDetails(long sysmailmsg);

        /// <summary>
        /// Returns all Memo Details
        /// </summary>
        /// <param name="syswfmmemo"></param>
        /// <returns></returns>
        public abstract MemoDto getMemoDetails(long syswfmmemo);

        /// <summary>
        /// Returns all Prun Details
        /// </summary>
        /// <param name="sysprun"></param>
        /// <returns></returns>
        public abstract PrunDto getPrunDetails(long sysprun);

        /// <summary>
        /// Returns all Fileatt Details
        /// </summary>
        /// <param name="sysFileatt"></param>
        /// <returns></returns>
        public abstract FileattDto getFileattDetails(long sysFileatt);

        /// <summary>
        /// Returns all Fileatt Details
        /// </summary>
        /// <param name="sysFileatt"></param>
        /// <returns></returns>
        public abstract FileattDto getFileattDetails(string area, long sysid);

        /// <summary>
        /// Returns all Dmsdoc Details
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        abstract public DmsdocDto getDmsdocDetails(long sysdmsdoc);       

        /// <summary>
        /// Returns all Dmsdoc Details by entity
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        abstract public List<DmsdocDto> getDmsdocDetails(string area, long sysid);

        /// <summary>
        /// Returns all Prproduct Details
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public abstract PrproductDto getPrproductDetails(long sysprproduct);

        /// <summary>
        /// Returns all Itemcat Details
        /// </summary>
        /// <param name="systemcat"></param>
        /// <returns></returns>
        public abstract ItemcatDto getItemcatDetails(long sysitemcat);

        /// <summary>
        /// Returns all Ctlang Details
        /// </summary>
        /// <param name="sysctlang"></param>
        /// <returns></returns>
        public abstract CtlangDto getCtlangDetails(long sysctlang);

        /// <summary>
        /// Returns all Land Details
        /// </summary>
        /// <param name="sysland"></param>
        /// <returns></returns>
        public abstract LandDto getLandDetails(long sysland);

		/// <summary>
		/// Returns all Branche Details
		/// </summary>
		/// <param name="sysbranche"></param>
		/// <returns></returns>
		public abstract BrancheDto getBrancheDetails (long sysbranche);
		
        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public abstract AccountDto getAccountDetails(long sysaccount);

        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public abstract AccountDto getAccountDetails(String area,long sysaccount);

        /// <summary>
        /// Returns all WktAccount Details
        /// </summary>
        /// <param name="syswktaccount"></param>
        /// <returns></returns>
        public abstract WktaccountDto getWktAccountDetails(long syswktaccount);


        /// <summary>
        /// delivers Finanzdaten by area
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public abstract ogetFinanzDatenDto getFinanzdatenByArea(long syskd, string area, long sysid);

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        public abstract ogetZusatzdaten getZusatzdatenDetail(long syskd, long sysantrag);

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        public abstract ogetZusatzdaten getZusatzdatenDetailByAngebot(long sysit, long sysAngebot);

        /// <summary>
        /// Returns all Adrtp Details
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        public abstract AdrtpDto getAdrtpDetails(long sysadrtp);

        /// <summary>
        /// Returns Strasse Details
        /// </summary>
        /// <param name="sysstrasse"></param>
        /// <returns></returns>
        public abstract StrasseDto getStrasseDetails(long systrasse);

        /// <summary>
        /// Returns PUser Details
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public abstract PuserDto getPuserDetails(long syswfuser);
           
        /// <summary>
        /// Returns all Kontotp Details
        /// </summary>
        /// <param name="syskontotp"></param>
        /// <returns></returns>
        public abstract KontotpDto getKontotpDetails(long syskontotp);

        /// <summary>
        /// Returns all Blz Details
        /// </summary>
        /// <param name="sysblz"></param>
        /// <returns></returns>
        public abstract BlzDto getBlzDetails(long sysblz);

        /// <summary>
        /// Returns all Ptrelatep Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        public abstract PtrelateDto getPtrelateDetails(long sysptrelate);

        /// <summary>
        /// Returns all Wfexec Details
        /// </summary>
        /// <param name="syswfexec"></param>
        /// <returns></returns>
        abstract public WfexecDto getWfexecDetails(long syswfexec);

        /// <summary>
        /// Returns all Crmnm Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        public abstract CrmnmDto getCrmnmDetails(long syscrmnm);

        /// <summary>
        /// Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        public abstract DdlkprubDto getDdlkprubDetails(long sysddlkprub);

        /// <summary>
        /// Returns all Ptrelatep Details
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        public abstract DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol);

        /// <summary>
        /// Returns all Ddlkppos Details
        /// </summary>
        /// <param name="sysddlkppos"></param>
        /// <returns></returns>
        public abstract DdlkpposDto getDdlkpposDetails(long sysddlkppos);

        /// <summary>
        /// Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysOppotp"></param>
        /// <returns></returns>
        public abstract DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos);

        /// <summary>
        /// Returns all Camptp Details
        /// </summary>
        /// <param name="syscamptp"></param>
        /// <returns></returns>
        public abstract CamptpDto getCamptpDetails(long syscamptp);

        /// <summary>
        /// Returns all zinstab Details
        /// </summary>
        /// <param name="syszinstab"></param>
        /// <returns></returns>
        public abstract ZinstabDto getZinstabDetails(long syszinstab);

        /// <summary>
        /// Returns all Oppotp Details
        /// </summary>
        /// <param name="sysoppotp"></param>
        /// <returns></returns>
        public abstract OppotpDto getOppotpDetails(long sysoppotp);

        /// <summary>
        /// Returns all Crmpr Details
        /// </summary>
        /// <param name="syscrmpr"></param>
        /// <returns></returns>
        public abstract CrmprDto getCrmprDetails(long syscrmpr);

        /// <summary>
        /// Returns all Contacttp Details
        /// </summary>
        /// <param name="syscontacttp"></param>
        /// <returns></returns>
        public abstract ContacttpDto getContacttpDetails(long syscontacttp);

        /// <summary>
        /// Returns all Itemcatm Details
        /// </summary>
        /// <param name="sysitemcatm"></param>
        /// <returns></returns>
        public abstract ItemcatmDto getItemcatmDetails(long sysitemcatm);


        /// <summary>
        /// Returns all Recurr Details
        /// </summary>
        /// <param name="sysrecurr"></param>
        /// <returns></returns>
        public abstract RecurrDto getRecurrDetails(long sysrecurr);

        /// <summary>
        /// Returns all Ptype Details
        /// </summary>
        /// <param name="sysptype"></param>
        /// <returns></returns>
        public abstract PtypeDto getPtypeDetails(long sysptype);

        /// <summary>
        /// Returns all Prunstep Details
        /// </summary>
        /// <param name="sysprunstep"></param>
        /// <returns></returns>
        public abstract PrunstepDto getPrunstepDetails(long sysprunstep);

        /// <summary>
        /// Returns all Pstep Details
        /// </summary>
        /// <param name="syspstep"></param>
        /// <returns></returns>
        public abstract PstepDto getPstepDetails(long syspstep);

        /// <summary>
        /// Returns all Prkgroup Details
        /// </summary>
        /// <param name="sysprkgroup"></param>
        /// <returns></returns>
        public abstract PrkgroupDto getPrkgroupDetails(long sysprkgroup);

        /// <summary>
        /// Returns all Prkgroupm Details
        /// </summary>
        /// <param name="sysprkgroupm"></param>
        /// <returns></returns>
        public abstract PrkgroupmDto getPrkgroupmDetails(long sysprkgroupm);

        /// <summary>
        /// Returns all Prkgroupz Details
        /// </summary>
        /// <param name="sysprkgroupz"></param>
        /// <returns></returns>
        public abstract PrkgroupzDto getPrkgroupzDetails(long sysprkgroupz);

        /// <summary>
        /// Returns all Prkgroups Details
        /// </summary>
        /// <param name="sysprkgroups"></param>
        /// <returns></returns>
        public abstract PrkgroupsDto getPrkgroupsDetails(long sysprkgroups);

        /// <summary>
        /// Returns all Seg Details
        /// </summary>
        /// <param name="sysseg"></param>
        /// <returns></returns>
        public abstract SegDto getSegDetails(long sysseg);

        /// <summary>
        /// Returns all Segc Details
        /// </summary>
        /// <param name="syssegc"></param>
        /// <returns></returns>
        public abstract SegcDto getSegcDetails(long syssegc);

        /// <summary>
        /// Returns all Stickynote Details
        /// </summary>
        /// <param name="sysstickynote"></param>
        /// <returns></returns>
        public abstract StickynoteDto getStickynoteDetails(long sysstickynote);

        /// <summary>
        /// Returns all Stickytype Details
        /// </summary>
        /// <param name="sysstickynote"></param>
        /// <returns></returns>
        public abstract StickytypeDto getStickytypeDetails(long sysstickytype);


        /// <summary>
        /// Returns all WfsignatureDetail
        /// </summary>
        /// <param name="sysopportunity"></param>
        /// <returns></returns>
        public abstract WfsignatureDto getWfsignatureDetail(long input);

        /// <summary>
        /// Returns all Angebot Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        public abstract AngebotDto getAngebotDetails(long sysAngebot);

        /// <summary>
        /// returns product details of angebot
        /// </summary>
        /// <param name="sysang"></param>
        /// <returns></returns>
        public abstract ProduktInfoDto getProduktInfoAngebotDetails(long sysang);

        /// <summary>
        /// returns product details of antrag
        /// </summary>
        /// <param name="sysant"></param>
        /// <returns></returns>
        public abstract ProduktInfoDto getProduktInfoAntragDetails(long sysant);

        /// <summary>
        /// Returns Angebot Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        public abstract BNAngebotDto getBNAngebotDetails(long sysAngebot);

        /// <summary>
        /// Returns Antrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        public abstract BNAntragDto getBNAntragDetails(long sysAntrag);

        /// <summary>
        /// Returns all Antrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        public abstract AntragDto getAntragDetails(long sysAntrag);

        /// <summary>
        /// Returns all Vertrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        public abstract VertragDto getVertragDetails(long sysVertrag);

        /// <summary>
        /// Returns all Vorgang Details
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public abstract VorgangDto getVorgangDetails(long sysId, string area);

        /// <summary>
        /// Waehrung
        /// </summary>
        /// <param name="sysWaehrung"></param>
        /// <returns></returns>
        public abstract WaehrungDto getWaehrungDetails(long? sysWaehrung);

        /// <summary>
        /// Returns all Details of generic Object
        /// ACHTUNG: Kann fehleranfällig sein, wenn die Klassen umbenannt werden/Methoden nicht richtig benannt wurden.
        /// </summary>
        /// <typeparam name="T">Typ, von welchem die Details geladen werden sollen</typeparam>
        /// <param name="sysid">Id vom zu ladenden Objekt</param>
        /// <returns></returns>
        public T getDetails<T>(long sysid)
        {
            Type mytype = typeof(AbstractEntityBo);
            Type searched = typeof(T);
            string name = searched.Name.Substring(0, searched.Name.Length - 3);
            System.Reflection.MethodInfo mi = mytype.GetMethod("get" + name + "Details");
            if (mi != null)
                return (T)mi.Invoke(this, new object[] { sysid });
            else
                throw new Exception("Type not supported for getDetails: " + typeof(T));
        }

        /// <summary>
        /// Returns all ZEK requests
        /// </summary>
        /// <param name="syszek"></param>
        /// <returns></returns>
        public abstract ZekDto getZek(long syszek);

        /// <summary>
        /// returns process data
        /// </summary>
        /// <param name="sysprocess">process id</param>
        /// <returns>process data</returns>
        public abstract ProcessDto getProcess(long sysprocess);

        #endregion





        /// <summary>
        /// Sucht Appointments inklusive den Recurrences
        /// </summary>
        /// <param name="search">Parameter</param>
        /// <returns></returns>
        public abstract List<ApptmtDto> searchApptmts(iSearchApptmtsWithRecurrDto search);


        /// <summary>
        /// Löscht eine Entity
        /// </summary>
        /// <param name="area">Area</param>
        /// <param name="sysid">Sysid</param>
        public abstract void deleteEntity(string area, long sysid);

        /// <summary>
        /// update LegitimationMethode
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="syswfuser"></param>
        /// <param name="sysit"></param>
        /// <param name="legitimationMethode"></param>
        public abstract void updateLegitimationMethode(long sysangebot, long syswfuser, long sysit, string legitimationMethode);

        /// <summary>
        /// delivers the pkz or ukz for the it or person 
        /// optionally for the subarea like angebot/antrag and its id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="subarea"></param>
        /// <param name="subsysid"></param>
        /// <returns></returns>
        public abstract ogetZusatzdaten getZusatzdatenDetail(String area, long sysid, String subarea, long subsysid);

        /// <summary>
        /// update abwicklungsort for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public abstract bool updateAbwicklungsort(iupdateAbwicklungsortDto input);

        /// <summary>
        /// update smstext for ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public abstract bool updateSMSText(iupdateSMSTextDto input);

        /// <summary>
        /// get anciliary details for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public abstract ogetAnciliaryDetailDto getAnciliaryDetail(igetAnciliaryDetailDto input);

        /// <summary>
        /// accept EPOS Conditions of current user
        /// </summary>
        public abstract void acceptEPOSConditions();

        /// <summary>
        /// Create or updates a Riskmail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract osendRiskmailDto sendRiskmail(isendRiskmailDto input);

        /// <summary>
        /// Performs the 4-eyes principle for the area/id setting the given result of the current user
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public abstract void fourEyesPrinciple(ifourEyesDto input, ofourEyesDto output);
    }
}
