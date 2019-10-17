using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.One.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.One.Web.Contract
{
    /// <summary>
    /// Methods for delivering a certain entity with all its data
    /// </summary>
    [ServiceContract(Name = "IgetEntityService", Namespace = "http://cic-software.de/One")]

    public interface IgetEntityService
    {

        /// <summary>
        /// delivers CREFO detail
        /// </summary>
        /// <param name="iDto"></param>
        /// <returns></returns>
        [OperationContract]
        ogetCREFODetailDto getCREFODetail(igetCREFODetailDto iDto);

        /// <summary>
        /// delivers Wfexec detail
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        [OperationContract]
        ogetWfexecDetailDto getWfexecDetail(long syswfexec);
            
        /// <summary>
        /// delivers System
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        [OperationContract]
        ogetSystemDetailDto getSystemDetail(long syssystem);

        /// <summary>
        /// delivers Guardean detail
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="gviewId"></param>
        /// <returns></returns>
        [OperationContract]
        ogetGuardeanDetailDto getGuardeanDetail(igetGuardeanDto iDto);

        /// <summary>
        /// delivers Gview detail
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="gviewId"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [OperationContract]
        ogetGviewDetailDto getGviewDetail(long sysid, String gviewId,WorkflowContext ctx);

        /// <summary>
        /// delivers Staffelpositionstyp detail
        /// </summary>
        /// <param name="sysslpostyp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetStaffelpositionstypDetailDto getStaffelpositionstypDetail(long sysslpostyp);

        /// <summary>
        /// delivers Staffeltyp detail
        /// </summary>
        /// <param name="syssltyp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetStaffeltypDetailDto getStaffeltypDetail(long syssltyp);

        /// <summary>
        /// delivers Rolle detail
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        [OperationContract]
        ogetRolleDetailDto getRolleDetail(long sysperole);

        /// <summary>
        /// delivers Rollentyp detail
        /// </summary>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        [OperationContract]
        ogetRollentypDetailDto getRollentypDetail(long sysroletype);

        /// <summary>
        /// delivers Handelsgruppe detail
        /// </summary>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        [OperationContract]
        ogetHandelsgruppeDetailDto getHandelsgruppeDetail(long sysprhgroup);

        /// <summary>
        /// delivers Vertriebskanal detail
        /// </summary>
        /// <param name="sysbchannel"></param>
        /// <returns></returns>
        [OperationContract]
        ogetVertriebskanalDetailDto getVertriebskanalDetail(long sysbchannel);

        /// <summary>
        /// delivers Brand detail
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        [OperationContract]
        ogetBrandDetailDto getBrandDetail(long sysbrand);

        /// <summary>
        /// delivers Rechnung detail
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        [OperationContract]
        ogetRechnungDetailDto getRechnungDetail(long sysid);

        /// <summary>
        /// delivers Angobbrief detail
        /// </summary>
        /// <param name="sysangobbrief"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAngobbriefDetailDto getAngobbriefDetail(long sysangobbrief);

        /// <summary>
        /// delivers Zahlungsplan detail
        /// </summary>
        /// <param name="sysslpos"></param>
        /// <returns></returns>
        [OperationContract]
        ogetZahlungsplanDetailDto getZahlungsplanDetail(long sysslpos);

        /// <summary>
        /// delivers Fahrzeugbrief detail
        /// </summary>
        /// <param name="sysobbrief"></param>
        /// <returns></returns>
        [OperationContract]
        ogetFahrzeugbriefDetailDto getFahrzeugbriefDetail(long sysobbrief);

        /// <summary>
        /// delivers Kalk detail
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        [OperationContract]
        ogetKalkDetailDto getKalkDetail(long syskalk);

        /// <summary>
        /// delivers Person detail
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPersonDetailDto getPersonDetail(long sysperson);

        /// <summary>
        /// Returns the Eaihot
        /// </summary>
        /// <param name="syseaihot"></param>
        /// <returns></returns>
        [OperationContract]
        ogetEaihotDto getEaihotDetail(long syseaihot);

        /// <summary>
        /// Returns the image link to the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        [OperationContract]
        ogetEntityLinkDto getEntityLink(String entity, long sysid, String vlmCode);

        /// <summary>
        /// Returns the icon data to the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        [OperationContract]
        ogetEntityIconDto getEntityIcon(String entity, long sysid, String vlmCode);

         /// <summary>
        /// Returns the icon Informations
        /// </summary>
        /// <param name="icons"></param>       
        /// <returns></returns>
        [OperationContract]
        ogetEntityIconsDto getEntityIcons(igetEntityIconsDto icons);

        /// <summary>
        /// Returns all rubrik-data for a certain area (and id, if given)
        /// </summary>
        /// <param name="rubInput"></param>
        /// <returns></returns>
        [OperationContract]
        ogetRubDto getRubInfo(igetRubDto rubInput);

         /// <summary>
        /// Returns all indicator details
        /// </summary>
        /// <param name="expTyp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetExptypDto getExptypDetails(igetExptypDto expTyp);

        /// <summary>
        /// Returns all indicator detail values
        /// </summary>
        /// <param name="expTyp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetExpdispDto getExpdispDetails(igetExpdispDto expTyp);

		/// <summary>
		/// Returns all Sla details
		/// </summary>
		/// <param name="slaTyp"></param>
		/// <returns></returns>
		[OperationContract]
		ogetSlaDto getSlaDetails (igetSlaDto slaTyp);
		
		/// <summary>
        /// Returns all finanzierung detail values
        /// </summary>
        /// <param name="sysnkk"></param>
        /// <returns></returns>
        [OperationContract]
        ogetFinanzierungDetailDto getFinanzierungDetail(long sysnkk);

        /// <summary>
        /// Returns all finanzierung detail values
        /// </summary>
        /// <param name="sysnkk"></param>
        /// <returns></returns>
        [OperationContract]
        oKreditlinieDto getKreditlinieDetail(long sysklinie);


          /// <summary>
        /// delivers Opportunity detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        [OperationContract]
        ogetOpportunityDetailDto getOpportunityDetail(long sysoppo);

        /// <summary>
        /// delivers Oppotask detail
        /// </summary>
        /// <param name="sysoppotask"></param>
        /// <returns></returns>
        [OperationContract]
        ogetOppotaskDetailDto getOppotaskDetail(long sysoppotask);

        /// <summary>
        /// delivers Wfsignature detail
        /// </summary>
        /// <param name="syswfsignature"></param>
        /// <returns></returns>
        [OperationContract]
        ogetWfsignatureDetailDto getWfsignatureDetail(long syswfsignature);

        /// <summary>
        /// delivers Wfsignature detail by Type
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ogetWfsignatureDetailDto getWfsignatureFromTypeDetail(igetWfsignatureDetailDto input);

        /// <summary>
        /// delivers objekt detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ogetObjektDetailDto getObjektDetail(long sysob);

         /// <summary>
        /// delivers objekt detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ogetObtypDetailDto getObtypDetail(long sysobtyp);

        /// <summary>
        /// delivers rahmen detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ogetRahmenDetailDto getRahmenDetail(long sysrvt);

        /// <summary>
        /// delivers Haendler detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ogetHaendlerDetailDto getHaendlerDetail(long sysperson);

        /// <summary>
        /// delivers Kunde detail
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        [OperationContract]
        ogetKundeDetailDto getKundeDetail(long sysperson);

        /// <summary>
        /// delivers logdump detail
        /// </summary>
        /// <param name="logdump"></param>
        /// <returns>logdump detail</returns>
        [OperationContract]
        ogetLogDumpDetailDto getLogDumpDetail(long logdump);
        
        /// <summary>
        /// delivers It detail
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        [OperationContract]
        ogetItDetailDto getItDetail(long sysit);

         /// <summary>
        /// delivers Contact detail
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        [OperationContract]
        ogetContactDetailDto getContactDetail(long syscontact);

        /// <summary>
        /// delivers Konto detail
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        [OperationContract]
        ogetKontoDetailDto getKontoDetail(long syskonto);

        /// <summary>
        /// delivers camp detail
        /// </summary>
        /// <param name="syscamp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetCampDetailDto getCampDetail(long syscamp);

        /// <summary>
        /// delivers Adresse detail
        /// </summary>
        /// <param name="sysadresse"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAdresseDetailDto getAdresseDetail(long sysadresse);

        /// <summary>
        /// delivers Itkonto detail
        /// </summary>
        /// <param name="sysitkonto"></param>
        /// <returns></returns>
        [OperationContract]
        ogetItkontoDetailDto getItkontoDetail(long sysitkonto);

        /// <summary>
        /// delivers Itadresse detail
        /// </summary>
        /// <param name="sysitadresse"></param>
        /// <returns></returns>
        [OperationContract]
        ogetItadresseDetailDto getItadresseDetail(long sysitadresse);

        /// <summary>
        /// delivers Ptask detail
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPtaskDetailDto getPtaskDetail(long sysptask);

        /// <summary>
        /// delivers Apptmt detail
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        [OperationContract]
        ogetApptmtDetailDto getApptmtDetail(long sysapptmt);

        /// <summary>
        /// delivers Reminder detail
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        [OperationContract]
        ogetReminderDetailDto getReminderDetail(long sysreminder);

        /// <summary>
        /// delivers Mailmsg detail
        /// </summary>
        /// <param name="sysmailmsg"></param>
        /// <returns></returns>
        [OperationContract]
        ogetMailmsgDetailDto getMailmsgDetail(long sysmailmsg);

        /// <summary>
        /// delivers Memo detail
        /// </summary>
        /// <param name="sysmailmsg"></param>
        /// <returns></returns>
        [OperationContract]
        ogetMemoDetailDto getMemoDetail(long syswfmmemo);

        /// <summary>
        /// delivers Prun detail
        /// </summary>
        /// <param name="sysprun"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPrunDetailDto getPrunDetail(long sysprun);

        /// <summary>
        /// delivers Fileatt detail
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        [OperationContract]
        ogetFileattDetailDto getFileattDetail(long sysfileatt);

        /// <summary>
        /// delivers Fileatt detail
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        [OperationContract]
        ogetFileattDetailDto getFileattEntity(string area, long sysid);

        
        /// <summary>
        /// delivers Dmsdoc detail
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDmsdocDetailDto getDmsdocDetail(long sysdmsdoc);

        /// <summary>
        /// delivers Dmsdoc detail
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDmsdocEntityDto getDmsdocEntity(string area, long sysid);

        /// <summary>
        /// delivers Prproduct detail
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPrproductDetailDto getPrproductDetail(long sysprproduct);

        /// <summary>
        /// delivers Itemcat detail
        /// </summary>
        /// <param name="sysitemcat"></param>
        /// <returns></returns>
        [OperationContract]
        ogetItemcatDetailDto getItemcatDetail(long sysitemcat);

        /// <summary>
        /// delivers Ctlang detail
        /// </summary>
        /// <param name="sysctlang"></param>
        /// <returns></returns>
        [OperationContract]
        ogetCtlangDetailDto getCtlangDetail(long sysctlang);

        /// <summary>
        /// delivers land detail
        /// </summary>
        /// <param name="sysland"></param>
        /// <returns></returns>
        [OperationContract]
        ogetLandDetailDto getLandDetail(long sysland);

        /// <summary>
        /// delivers Branche detail
        /// </summary>
        /// <param name="sysbranche"></param>
        /// <returns></returns>
        [OperationContract]
        ogetBrancheDetailDto getBrancheDetail(long sysbranche);

        /// <summary>
        /// delivers Account detail
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAccountDetailDto getAccountDetail(long sysaccount);

                /// <summary>
        /// delivers Account detail
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAccountDetailDto getAccountDetailArea(String area, long sysaccount);

        /// <summary>
        /// delivers Vorgang detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        [OperationContract]
        ogetVorgangDetailDto getVorgangDetailArea(String area, long sysId);

         /// <summary>
        /// delivers WktAccount detail
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        [OperationContract]
        ogetWktAccountDetailDto getWktaccountDetail(long syswkt);

        /// <summary>
        /// delivers Finanzdaten by area id
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        [OperationContract]
        ogetFinanzDatenDto getFinanzdatenByArea(long syskd, string area, long sysid);

          /// <summary>
        /// delivers Partner detail
        /// </summary>
        /// <param name="syspartner"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPartnerDetailDto getPartnerDetail(long syspartner);

         /// <summary>
        /// delivers Beteiligter detail
        /// </summary>
        /// <param name="syscrmnm"></param>
        /// <returns></returns>
        [OperationContract]
        ogetBeteiligterDetailDto getBeteiligterDetail(long syscrmnm);

        /// <summary>
        /// delivers Adrtp detail
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAdrtpDetailDto getAdrtpDetail(long sysadrtp);

        /// <summary>
        /// delivers Kontotp detail
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetKontotpDetailDto getKontotpDetail(long syskontotp);

        /// <summary>
        /// delivers Blz detail
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetBlzDetailDto getBlzDetail(long sysblz);

        /// <summary>
        /// delivers Ptrelate detail
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPtrelateDetailDto getPtrelateDetail(long sysptrelate);

        /// <summary>
        /// delivers Crmnm detail
        /// </summary>
        /// <param name="syscrmnm"></param>
        /// <returns></returns>
        [OperationContract]
        ogetCrmnmDetailDto getCrmnmDetail(long syscrmnm);

        /// <summary>
        /// delivers Ddlkprub detail
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDdlkprubDetailDto getDdlkprubDetail(long sysddlkprub);

        /// <summary>
        /// delivers Ddlkpcol detail
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDdlkpcolDetailDto getDdlkpcolDetail(long sysddlkpcol);

        /// <summary>
        /// delivers Ddlkppos detail
        /// </summary>
        /// <param name="sysddlkppos"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDdlkpposDetailDto getDdlkpposDetail(long sysddlkppos);

        /// <summary>
        /// delivers ddlkpspos detail
        /// </summary>
        /// <param name="sysddlkpsposp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDdlkpsposDetailDto getDdlkpsposDetail(long sysddlkpspos);

        /// <summary>
        /// delivers Camptp detail
        /// </summary>
        /// <param name="syscamptp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetCamptpDetailDto getCamptpDetail(long syscamptp);

        /// <summary>
        /// delivers Oppotp detail
        /// </summary>
        /// <param name="sysoppotp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetOppotpDetailDto getOppotpDetail(long sysoppotp);

        /// <summary>
        /// delivers Crmpr detail
        /// </summary>
        /// <param name="syscrmpr"></param>
        /// <returns></returns>
        [OperationContract]
        ogetCrmprDetailDto getCrmprDetail(long syscrmpr);

        /// <summary>
        /// delivers Contacttp detail
        /// </summary>
        /// <param name="syscontacttp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetContacttpDetailDto getContacttpDetail(long syscontacttp);

        /// <summary>
        /// delivers Recurr detail
        /// </summary>
        /// <param name="sysitemcatm"></param>
        /// <returns></returns>
        [OperationContract]
        ogetRecurrDetailDto getRecurrDetail(long sysitemcatm);

        /// <summary>
        /// delivers Ptype detail
        /// </summary>
        /// <param name="sysptype"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPtypeDetailDto getPtypeDetail(long sysptype);

        /// <summary>
        /// delivers Prunstep detail
        /// </summary>
        /// <param name="sysprunstep"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPrunstepDetailDto getPrunstepDetail(long sysprunstep);

        /// <summary>
        /// delivers Pstep detail
        /// </summary>
        /// <param name="syspstep"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPstepDetailDto getPstepDetail(long syspstep);

        /// <summary>
        /// delivers Stickynote detail
        /// </summary>
        /// <param name="sysStickynote"></param>
        /// <returns></returns>
        [OperationContract]
        ogetStickynoteDetailDto getStickynoteDetail(long sysstickynote);

        /// <summary>
        /// delivers Stickytype detail
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        [OperationContract]
        ogetStickytypeDetailDto getStickytypeDetail(long sysstickytype);

      
         /// <summary>
        /// delivers Angebot detail
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAngebotDetailDto getAngebotDetail(long sysangebot);

        /// <summary>
        /// delivers BN Angebot detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        [OperationContract]
        ogetBNAngebotDetailDto getBNAngebotDetail(long sysangebot);
        
        /// <summary>
        /// delivers BN Antrag detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        [OperationContract]
        ogetBNAntragDetailDto getBNAntragDetail(long sysantrag);

        /// <summary>
        /// delivers Angob detail
        /// </summary>
        /// <param name="sysangob"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAngobDetailDto getAngobDetail(long sysangob);

        /// <summary>
        /// delivers Ob detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        [OperationContract]
        ogetObDetailDto getObDetail(long sysob);

        /// <summary>
        /// delivers Angvar detail
        /// </summary>
        /// <param name="sysangvar"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAngvarDetailDto getAngvarDetail(long sysangvar);

        /// <summary>
        /// delivers Angkalk detail
        /// </summary>
        /// <param name="sysangkalk"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAngkalkDetailDto getAngkalkDetail(long sysangkalk);

        /// <summary>
        /// delivers Antkalk detail
        /// </summary>
        /// <param name="sysantkalk"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAntkalkDetailDto getAntkalkDetail(long sysantkalk);

        /// <summary>
        /// delivers Antragt detail
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAntragDetailDto getAntragDetail(long sysantrag);


        /// <summary>
        /// delivers Vertrag detail
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        [OperationContract]
        ogetVertragDetailDto getVertragDetail(long sysvertrag);


        /// <summary>
        /// delivers Vorgang detail
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        [OperationContract]
        ogetVorgangDetailDto getVorgangDetail(long sysId, string area);

        /// <summary>
        /// delivers MyCalc detail
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        [OperationContract]
        ogetMycalcDetailDto getMycalcDetail(long sysmycalc);

        /// <summary>
        /// delivers recalc detail
        /// </summary>
        /// <param name="sysrecalc"></param>
        /// <returns></returns>
        [OperationContract]
        ogetRecalcDetailDto getRecalcDetail(long sysrecalc);

        /// <summary>
        /// Returns all Products
        /// </summary>
        /// <param name="expVal"></param>
        /// <returns></returns>
        [OperationContract]
        ogetProductsDto getProducts(prKontextDto prodCtx);

        /// <summary>
        /// Returns all Product Parameters
        /// </summary>
        /// <param name="expVal"></param>
        /// <returns></returns>
        [OperationContract]
        ogetProductParameterDto getProductParameters(prKontextDto prodCtx);

                /// <summary>
        /// Returns RAP Zins for Product and SCORE
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        [OperationContract]
        ogetRAPZinsDto getRAPZins(igetRAPZinsDto inputData);

        /// <summary>
        /// delivers ZEK detail
        /// </summary>
        /// <param name="syszek"></param>
        /// <returns></returns>
        [OperationContract]
        ogetZekDto getZekDetail(long syszek);

        /// <summary>
        /// delivers process details
        /// </summary>
        /// <param name="sysprocess">process id</param>
        /// <returns>process data</returns>
        [OperationContract]
        ogetProcessDto getProcessDetail(long sysprocess);

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        [OperationContract]
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
        [OperationContract]
        ogetZusatzdaten getZusatzdatenDetailByArea(String area, long sysid, String subarea, long subsysid);

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        [OperationContract]
        ogetZusatzdaten getZusatzdatenDetailByAngebot(long sysit, long sysangebot);

        /// <summary>
        /// Get Strasse Details
        /// </summary>
        /// <param name="sysstrasse"></param>
        /// <returns></returns>
        [OperationContract]
        ogetStrasseDto getStrasseDetail(long sysstrasse);

                /// <summary>
        /// Get Puser Details
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        [OperationContract]
        ogetPuserDetailDto getPuserDetail(long syswfuser);

         /// <summary>
        /// Get Dokval Details
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDokvalDetailDto getDokvalDetail(String area, long sysid);
        
        /// <summary>
        /// Get Checklist Details
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        [OperationContract]
        ogetChklistDetailDto getChklistDetail(igetChecklistDetailDto sysid);

          /// <summary>
        /// Get Prunart Details
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetPrunartDetailDto getPrunartDetail(long sysprunart);

        /// <summary>
        /// Get Rule-Engine-Result Details
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ogetRuleSetDetailDto getRuleSetDetail(igetRuleSetDetailDto input);
    }

}
