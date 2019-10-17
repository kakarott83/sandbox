using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Mapper;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Model.DdOw;

using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;
using Cic.One.Web.BO;
using Cic.One.DTO;
using Cic.One.DTO.Mediator;
using CIC.Database.OL.EF4.Model;

namespace Cic.One.DTO
{


    public class MappingProfileOne : MappingProfileBase
    {
        /// <summary>
        /// Konfigurieren
        /// </summary>
        protected override void Configure()
        {
            base.Configure();
            CreateMap<PuserDto, PERSON>()
                   .ForMember(dest => dest.BLZ, opt => opt.Ignore());
            CreateMap<PuserDto, CIC.Database.OL.EF4.Model.WFUSER>();
            CreateMap<PuserDto, CIC.Database.OL.EF4.Model.PUSER>();

            CreateMap<LogDumpDto, LOGDUMP>();         // rh: 20161108                     
            CreateMap<LOGDUMP, LogDumpDto>();
            
            CreateMap<StaffelpositionstypDto, SLPOSTYP>();
            CreateMap<SLPOSTYP, StaffelpositionstypDto>();
            CreateMap<StaffeltypDto, SLTYP>();
            CreateMap<SLTYP, StaffeltypDto>();
            CreateMap<RolleDto, PEROLE>();
            CreateMap<PEROLE, RolleDto>();
            CreateMap<RollentypDto, ROLETYPE>();
            CreateMap<ROLETYPE, RollentypDto>();
            CreateMap<HandelsgruppeDto, PRHGROUP>();
            CreateMap<PRHGROUP, HandelsgruppeDto>();
            CreateMap<VertriebskanalDto, BCHANNEL>();
            CreateMap<BCHANNEL, VertriebskanalDto>();
            CreateMap<BrandDto, BRAND>();
            CreateMap<BRAND, BrandDto>();
            CreateMap<RechnungDto, RN>();
            CreateMap<RN, RechnungDto>();
            CreateMap<AngobbriefDto, ANGOBBRIEF>();
            CreateMap<ANGOBBRIEF, AngobbriefDto>();
			CreateMap<ZahlungsplanDto, SLPOS> ();
			CreateMap<SLPOS, ZahlungsplanDto> ();
			CreateMap<KreditlinieDto, HDKLINIE> ();
			CreateMap<HDKLINIE, KreditlinieDto> ();
			CreateMap<FahrzeugbriefDto, OBBRIEF> ();
            CreateMap<OBBRIEF, FahrzeugbriefDto>();
            CreateMap<KalkDto, KALK>();
            CreateMap<KALK, KalkDto>();
            CreateMap<PersonDto, PERSON>();
            CreateMap<PERSON, PersonDto>();
            CreateMap<ItDto, AccountDto>();
            CreateMap<Cic.OpenOne.Common.Model.DdOd.DDLKPSPOS, DdlkpsposDto>()
              .ForMember(dest => dest.sysddlkpcol, opt => opt.MapFrom(src => SysidFromReference(src.DDLKPCOLReference)))
              .ForMember(dest => dest.sysddlkppos, opt => opt.MapFrom(src => SysidFromReference(src.DDLKPPOSReference)))
              .ForMember(dest => dest.entityId, opt => opt.Ignore());
            CreateMap<DdlkpsposDto, Cic.OpenOne.Common.Model.DdOd.DDLKPSPOS>()
                .ForMember(dest => dest.DDLKPCOL, opt => opt.Ignore())
                  .ForMember(dest => dest.DDLKPPOS, opt => opt.Ignore())
                   .ForMember(dest => dest.DDLKPCOLReference, opt => opt.Ignore())
                  .ForMember(dest => dest.DDLKPPOSReference, opt => opt.Ignore())
                   .ForMember(dest => dest.EntityKey, opt => opt.Ignore())
                   .ForMember(dest => dest.EntityState, opt => opt.Ignore());

            CreateMap<PartnerDto, PERSON>()
                .ForMember(dest => dest.WAEHRUNG, opt => opt.Ignore())
                .ForMember(dest => dest.WAEHRUNGReference, opt => opt.Ignore())
                .ForMember(dest => dest.ERREICHBBIS, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.erreichbBisGUI))
                .ForMember(dest => dest.ERREICHBVON, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.erreichbVonGUI))
                ;

            CreateMap<HaendlerDto, PERSON>()
                .ForMember(dest => dest.WAEHRUNG, opt => opt.Ignore())
                .ForMember(dest => dest.WAEHRUNGReference, opt => opt.Ignore())
                ;

            CreateMap<PERSON, HaendlerDto>();

            CreateMap<WktaccountDto,AccountDto>();
            CreateMap<WktaccountDto, ItDto>()
                 .ForMember(dest => dest.rechtsform, opt => opt.Ignore());

            CreateMap<ObjektDto, OB>();
            CreateMap<OB, ObjektDto>();

            //CreateMap<RecalcDto, RECALC>();
            //CreateMap<RECALC, RecalcDto>();
            CreateMap<SlDto, SLPOS>();
            CreateMap<SLPOS, SlDto>();

            CreateMap<MycalcDto, MYCALC>();
            CreateMap<MYCALC, MycalcDto>()
                 .ForMember(dest => dest.syskalktyp, opt => opt.MapFrom(src => SysidFromReference(src.KALKTYPReference)))
                 .ForMember(dest => dest.sysobtyp, opt => opt.MapFrom(src => SysidFromReference(src.OBTYPReference)));

            CreateMap<EaihotDto, EAIHOT>();
            CreateMap<EAIHOT, EaihotDto>();

            CreateMap<ExpvalDto, EXPVAL>();
            CreateMap<EXPVAL, ExpvalDto>();
            

            CreateMap<EaihfileDto, EAIHFILE>();
            CreateMap<EAIHFILE, EaihfileDto>();

            //TODO Mapping aktivkz == byte? ????? das muss ein int sein
            CreateMap<RahmenDto, RVT>().ForMember(
                dest => dest.AKTIVKZ,
                opt => opt.MapFrom(y => (int?)y.aktivkz)                
                );


            

            Mapper.CreateMap<CRMNM, BeteiligterDto>();


            var map = CreateMap<WktaccountDto, PARTNER>();
            map.ForAllMembers(opt=>opt.Ignore());
             map.ForMember(dest => dest.LASTNAME, opt => opt.MapFrom(src => src.plastname))
                .ForMember(dest => dest.FIRSTNAME, opt => opt.MapFrom(src => src.pfirstname))
                .ForMember(dest => dest.ANREDE, opt => opt.MapFrom(src => src.panrede))
                .ForMember(dest => dest.TITEL, opt => opt.MapFrom(src => src.ptitel))
                .ForMember(dest => dest.TELEFON, opt => opt.MapFrom(src => src.ptelefon))
                .ForMember(dest => dest.MOBIL, opt => opt.MapFrom(src => src.pmobil))
                .ForMember(dest => dest.EMAIL, opt => opt.MapFrom(src => src.pemail))
                .ForMember(dest => dest.FAX, opt => opt.MapFrom(src => src.pfax))
                ;

           var map2= CreateMap<PARTNER, WktaccountDto>();
           map2.ForAllMembers(opt=>opt.Ignore());
           map2.ForMember(dest => dest.plastname, opt => opt.MapFrom(src => src.LASTNAME))
               .ForMember(dest => dest.pfirstname, opt => opt.MapFrom(src => src.FIRSTNAME))
               .ForMember(dest => dest.panrede, opt => opt.MapFrom(src => src.ANREDE))
               .ForMember(dest => dest.ptitel, opt => opt.MapFrom(src => src.TITEL))
               .ForMember(dest => dest.ptelefon, opt => opt.MapFrom(src => src.TELEFON))
               .ForMember(dest => dest.pmobil, opt => opt.MapFrom(src => src.MOBIL))
               .ForMember(dest => dest.pemail, opt => opt.MapFrom(src => src.EMAIL))
               .ForMember(dest => dest.pfax, opt => opt.MapFrom(src => src.FAX));
            
            CreateMap<RvtPosDto, RVTPOS>();
            CreateMap<RVTPOS, RvtPosDto>()
                .ForMember(dest => dest.sysrvt, opt => opt.MapFrom(src => SysidFromReference(src.RVTReference)))
                .ForMember(dest => dest.sysfstyp, opt => opt.MapFrom(src => SysidFromReference(src.FSTYPReference)))
                ;
            CreateMap<RVT, RahmenDto>().ForMember(dest => dest.aktivkz, opt => opt.MapFrom(src => (src.AKTIVKZ != null) ? (int)src.AKTIVKZ : 0))
                     .ForMember(dest => dest.sysperson, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)));
            
            CreateMap<BeteiligterDto, PERSON>()
                .ForMember(dest => dest.WAEHRUNG, opt => opt.Ignore())
                .ForMember(dest => dest.WAEHRUNGReference, opt => opt.Ignore())
                .ForMember(dest => dest.ERREICHBBIS, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.erreichbBisGUI))
                .ForMember(dest => dest.ERREICHBVON, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.erreichbVonGUI))
                ;


            CreateMap<PERSON, PartnerDto>()
                  .ForMember(dest => dest.waehrung, opt => opt.MapFrom(src => SysidFromReference(src.WAEHRUNGReference)))
                  .ForMember(dest => dest.sysland, opt => opt.MapFrom(src => SysidFromReference(src.LANDReference)));

            CreateMap<PERSON, BeteiligterDto>()
                .ForMember(dest => dest.waehrung, opt => opt.MapFrom(src => SysidFromReference(src.WAEHRUNGReference)))
                .ForMember(dest => dest.sysland, opt => opt.MapFrom(src => SysidFromReference(src.LANDReference)));
 

            CreateMap<AccountDto, PERSON>()
                .ForMember(dest => dest.WAEHRUNG, opt => opt.Ignore())
                .ForMember(dest => dest.WAEHRUNGReference, opt => opt.Ignore())
                .ForMember(dest => dest.ERREICHBBIS, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.erreichbBisGUI))
                .ForMember(dest => dest.ERREICHBVON, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.erreichbVonGUI))

                ;
            CreateMap<PERSON, AccountDto>()
                  .ForMember(dest => dest.waehrung, opt => opt.MapFrom(src => SysidFromReference(src.WAEHRUNGReference)))
                  .ForMember(dest => dest.sysadmadd, opt => opt.MapFrom(src => SysidFromReference(src.ADMADDReference)))
                  .ForMember(dest => dest.sysland, opt => opt.MapFrom(src => SysidFromReference(src.LANDReference)));


            CreateMap<KundeDto, PERSON>();
            CreateMap<PERSON, KundeDto>();
            CreateMap<PTRELATE, PartnerDto>();

            CreateMap<CONTACT, ContactDto>()
                .ForMember(dest => dest.comTimeb2b, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.COMTIME))
                .ForMember(dest => dest.sysPerson, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)))
                .ForMember(dest => dest.sysOppo, opt => opt.MapFrom(src => SysidFromReference(src.OPPOReference)))
                .ForMember(dest => dest.sysContactTp, opt => opt.MapFrom(src => SysidFromReference(src.CONTACTTPReference)))
                .ForMember(dest => dest.sysAntrag, opt => opt.MapFrom(src => SysidFromReference(src.ANTRAGReference)))
                .ForMember(dest => dest.sysCamp, opt => opt.MapFrom(src => SysidFromReference(src.CAMPReference)))
                .ForMember(dest => dest.sysAngebot, opt => opt.MapFrom(src => SysidFromReference(src.ANGEBOTReference)));

            CreateMap<ContactDto, CONTACT>()
                 .ForMember(dest => dest.COMTIME, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.comTimeb2b))
                 .ForMember(dest => dest.COMTIME, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.comTimeb2b));

            CreateMap<ADRESSE, AdresseDto>()
                .ForMember(dest => dest.sysPerson, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)))
                .ForMember(dest => dest.landBez, opt => opt.MapFrom(src => src.LANDBEZ));
            CreateMap<AdresseDto, ADRESSE>()
                .ForMember(dest => dest.TYPE, opt => opt.UseValue(0))
                .ForMember(dest => dest.LANDBEZ, opt => opt.MapFrom(src => src.landBez));

            CreateMap<ITADRESSE, ItadresseDto>();
            CreateMap<ItadresseDto, ITADRESSE>();

            CreateMap<CAMP, CampDto>()
                .ForMember(dest => dest.sysCampTp, opt => opt.MapFrom(src => SysidFromReference(src.CAMPTPReference)));

            CreateMap<CampDto, CAMP>();

            CreateMap<OPPO, OpportunityDto>()
                 .ForMember(dest => dest.sysOppoTp, opt => opt.MapFrom(src => SysidFromReference(src.OPPOTPReference)))
                 .ForMember(dest => dest.sysPerson, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)))
                 .ForMember(dest => dest.sysCamp, opt => opt.MapFrom(src => SysidFromReference(src.CAMPReference)))
                .ForMember(dest => dest.sysiam, opt => opt.MapFrom(src => SysidFromReference(src.IAMReference)));

            CreateMap<OpportunityDto, OPPO>();


            CreateMap<OPPOTASK, OppotaskDto>()
                 .ForMember(dest => dest.sysIamOppotask, opt => opt.MapFrom(src => SysidFromReference(src.IAMOPPOTASKReference)))
                 .ForMember(dest => dest.sysOppotaskType, opt => opt.MapFrom(src => SysidFromReference(src.OPPOTASKTYPEReference)))
                 .ForMember(dest => dest.sysoppo, opt => opt.MapFrom(src => SysidFromReference(src.OPPOReference)));
            CreateMap<OppotaskDto, OPPOTASK>();

            

            CreateMap<ContactDto, ContactDto>()
             .ForMember(dest => dest.comTimeb2b, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.comTime));

            CreateMap<KontoDto, KONTO>().ForMember(dest => dest.BLZ, opt => opt.Ignore());

            CreateMap<KONTO, KontoDto>()
                .ForMember(dest => dest.blz, opt => opt.Ignore())
                .ForMember(dest => dest.syskontoTp, opt => opt.MapFrom(src => SysidFromReference(src.KONTOTPReference)))
                .ForMember(dest => dest.sysperson, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)));

            CreateMap<ItkontoDto, ITKONTO>();
            CreateMap<ITKONTO, ItkontoDto>();


            CreateMap<PTRELATE, PtrelateDto>();
            CreateMap<PtrelateDto, PTRELATE>();

            CreateMap<PrtlgsetDto, PRTLGSET>();
            CreateMap<PRTLGSET, PrtlgsetDto>();
            //CreateMap<PrproductDto, PRPRODUCT>();
            //CreateMap<PRPRODUCT, PrproductDto>();
            CreateMap<Cic.OpenOne.Common.Model.Prisma.PRPRODUCT, PrproductDto>();
            
            CreateMap<ObkatDto, OBKAT>();
            CreateMap<OBKAT, ObkatDto>();
            CreateMap<ObbriefDto, OBBRIEF>();
            CreateMap<OBBRIEF, ObbriefDto>();

            CreateMap<CRMPR, CrmprDto>();
            CreateMap<CrmprDto, CRMPR>();

            CreateMap<ITEMCAT, ItemcatDto>();
            CreateMap<ItemcatDto, ITEMCAT>();

            CreateMap<ITEMCATM, ItemcatmDto>()
                  .ForMember(dest => dest.sysItemCat, opt => opt.MapFrom(src => SysidFromReference(src.ITEMCATReference)));
            CreateMap<ItemcatmDto, ITEMCATM>();

            CreateMap<FILEATT, FileattDto>()
                .ForMember(dest => dest.sysMailMsg, opt => opt.MapFrom(src => SysidFromReference(src.MAILMSGReference)))
                .ForMember(dest => dest.sysPtask, opt => opt.MapFrom(src => SysidFromReference(src.PTASKReference)))
                .ForMember(dest => dest.sysApptmt, opt => opt.MapFrom(src => SysidFromReference(src.APPTMTReference)));

            CreateMap<FileattDto, FILEATT>();

            CreateMap<DMSDOC, DmsdocDto>();

            CreateMap<DmsdocDto, DMSDOC>();


            CreateMap<WFSIGNATURE, WfsignatureDto>()
                  .ForMember(dest => dest.sysWfuser, opt => opt.MapFrom(src => SysidFromReference(src.WFUSERReference)));
            CreateMap<WfsignatureDto, WFSIGNATURE>();


            CreateMap<FileattDto, BesuchsberichtDto>().ConvertUsing((a) =>
            {
                BesuchsberichtDto bericht =  Cic.OpenOne.Common.Util.Serialization.XMLDeserializer.objectFromXml<BesuchsberichtDto>(a.content, "UTF-8");
                bericht.sysFileatt = a.sysFileAtt;
                bericht.sysId = a.sysId;
                bericht.area = a.area;
                return bericht;
            });

            CreateMap<BesuchsberichtDto, FileattDto>().ConvertUsing((a) =>
                {
                    return new FileattDto()
                    {
                        content =  Cic.OpenOne.Common.Util.Serialization.XMLSerializer.objectToXml(a, "UTF-8"),
                        sysFileAtt = a.sysFileatt,
                        area = a.area,
                        sysId = a.sysId
                    };
                });



            CreateMap<REMINDER, ReminderDto>()
                .ForMember(dest => dest.sysMailMsg, opt => opt.MapFrom(src => SysidFromReference(src.MAILMSGReference)))
                .ForMember(dest => dest.sysPtask, opt => opt.MapFrom(src => SysidFromReference(src.PTASKReference)))
                .ForMember(dest => dest.sysApptmt, opt => opt.MapFrom(src => SysidFromReference(src.APPTMTReference)));
            CreateMap<ReminderDto, REMINDER>();

            CreateMap<RECURR, RecurrDto>()
                .ForMember(dest => dest.sysPtask, opt => opt.MapFrom(src => SysidFromReference(src.PTASKReference)))
                .ForMember(dest => dest.sysApptmt, opt => opt.MapFrom(src => SysidFromReference(src.APPTMTReference)));
            CreateMap<RecurrDto, RECURR>();

            CreateMap<PRUN, PrunDto>()
                 .ForMember(dest => dest.sysPtype, opt => opt.MapFrom(src => SysidFromReference(src.PTYPEReference)));
            CreateMap<PrunDto, PRUN>();


            CreateMap<PTYPE, PtypeDto>();
            CreateMap<PtypeDto, PTYPE>();

            CreateMap<PRUNSTEP, PrunstepDto>()
                .ForMember(dest => dest.sysPstep, opt => opt.MapFrom(src => SysidFromReference(src.PSTEPReference)))
                .ForMember(dest => dest.sysPrun, opt => opt.MapFrom(src => SysidFromReference(src.PRUNReference)));
            CreateMap<PrunstepDto, PRUNSTEP>();

            CreateMap<PSTEP, PstepDto>()
                 .ForMember(dest => dest.sysPtype, opt => opt.MapFrom(src => SysidFromReference(src.PTYPEReference)));
            CreateMap<PstepDto, PSTEP>();

            CreateMap<PRKGROUP, PrkgroupDto>();
            CreateMap<PrkgroupDto, PRKGROUP>();

            CreateMap<PRKGROUPM, PrkgroupmDto>()
                  .ForMember(dest => dest.sysPrkgroup, opt => opt.MapFrom(src => SysidFromReference(src.PRKGROUPReference)));
            CreateMap<PrkgroupmDto, PRKGROUPM>();


            CreateMap<SEG, SegDto>();
            CreateMap<SegDto, SEG>();

            CreateMap<SEGC, SegcDto>()
                .ForMember(dest => dest.sysCamp, opt => opt.MapFrom(src => SysidFromReference(src.CAMPReference)))
                .ForMember(dest => dest.sysSeg, opt => opt.MapFrom(src => SysidFromReference(src.SEGReference)));

            CreateMap<SegcDto, SEGC>();


            CreateMap<MAILMSG, MailmsgDto>()
                .ForMember(dest => dest.IsDraft, opt => opt.MapFrom(src =>
                {
                    if (src.ITEMID == null)
                    {
                        return 1;
                    }
                    return 0;
                }
            ));

            CreateMap<MailmsgDto, MAILMSG>()
                .ForMember(dest => dest.ITEMID, opt => opt.Ignore());

            CreateMap<APPTMT, ApptmtDto>()
                .ForMember(dest => dest.sysOwnerOld, opt => opt.MapFrom(src=>src.SYSOWNER));
            //.ForMember(dest => dest.startTimeGUI, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.STARTTIME))
            //.ForMember(dest => dest.endTimeGUI, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.ENDTIME));


            CreateMap<ApptmtDto, ApptmtDto>();
            //.ForMember(dest => dest.startTimeGUI, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.startTime))
            //.ForMember(dest => dest.endTimeGUI, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.endTime));

            CreateMap<ApptmtDto, APPTMT>()
                .ForMember(dest => dest.STARTTIME, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.startTimeGUI))
                .ForMember(dest => dest.ENDTIME, opt => opt.ResolveUsing(typeof(CustomB2BClarionTimeResolver)).FromMember(src => src.endTimeGUI))
                .ForMember(dest => dest.ITEMID, opt => opt.Ignore());

            CreateMap<PtaskDto, PtaskDto>();
            //.ForMember(dest => dest.startTimeGUI, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.startTime))
            //.ForMember(dest => dest.dueTimeGUI, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.dueTime));

            CreateMap<PTASK, PtaskDto>()
                //.ForMember(dest => dest.startTimeGUI, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.STARTTIME))
                //.ForMember(dest => dest.dueTimeGUI, opt => opt.ResolveUsing(typeof(CustomClarionB2bTimeResolver)).FromMember(src => src.DUETIME))
                //.ForMember(dest => dest.subject, opt => opt.MapFrom(src => src.SUBJECT))
                //.ForMember(dest => dest.content, opt => opt.MapFrom(src => src.CONTENT))
                .ForMember(dest => dest.sysOwnerOld, opt => opt.MapFrom(src=>src.SYSOWNER))
                .ForMember(dest => dest.sysPtype, opt => opt.MapFrom(src => SysidFromReference(src.PTYPEReference)));

            CreateMap<PtaskDto, PTASK>()
                .ForMember(dest => dest.ITEMID, opt => opt.Ignore())
                //.ForMember(dest => dest.SUBJECT, opt => opt.MapFrom(src => src.subject))
                //.ForMember(dest => dest.CONTENT, opt => opt.MapFrom(src => src.content))
                ;

            CreateMap<PERSON, ContactDto>();

            CreateMap<STICKYNOTE, StickynoteDto>();
            CreateMap<StickynoteDto, STICKYNOTE>();
            CreateMap<STICKYTYPE, StickytypeDto>();
            CreateMap<StickytypeDto, STICKYTYPE>();

            CreateMap<NOTIZ, StickytypeDto>();
            CreateMap<StickytypeDto, NOTIZ>();
            
            CreateMap<ItDto, IT>();
            CreateMap<IT, ItDto>()
                .ForMember(dest => dest.sysland, opt => opt.MapFrom(src => SysidFromReference(src.LANDReference)));
            CreateMap<ANGOPTION, AngAntOptionDto>();
            CreateMap<AngAntOptionDto, ANGOPTION>()
                .ForMember(dest => dest.SYSID, opt => opt.Ignore());
            CreateMap<ANTOPTION, AngAntOptionDto>();
            CreateMap<AngAntOptionDto, ANTOPTION>()
                .ForMember(dest => dest.SYSID, opt => opt.Ignore());

            CreateMap<MYCALCFS, MycalcfsDto>()
                .ForMember(dest => dest.sysmycalcfs, opt => opt.MapFrom(src => SysidFromReference(src.MYCALCReference)));
            CreateMap<MycalcfsDto, MYCALCFS>();

            CreateMap<ANGEBOT, AngebotDto>()
                .ForMember(dest => dest.angebot, opt => opt.MapFrom(src => src.ANGEBOT1))
                .ForMember(dest => dest.SysPerson, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)))
                .ForMember(dest => dest.sysKd, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)))
                .ForMember(dest => dest.sysbrand, opt => opt.MapFrom(src => SysidFromReference(src.BRANDReference)))
                ;
            CreateMap<AngebotDto, ANGEBOT>();
            CreateMap<ANTRAG, AntragDto>()
                .ForMember(dest => dest.antrag, opt => opt.MapFrom(src => src.ANTRAG1))
                .ForMember(dest => dest.sysKd, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)))
                ;
            CreateMap<AntragDto, ANTRAG>();

            CreateMap<ANGVAR, AngvarDto>()
                .ForMember(dest => dest.sysAngebot, opt => opt.MapFrom(src => SysidFromReference(src.ANGEBOTReference)))
                ;
            CreateMap<ANGOB, AngobDto>()
             .ForMember(dest => dest.sysObTyp, opt => opt.MapFrom(src => SysidFromReference(src.OBTYPReference)))
             .ForMember(dest => dest.sysObart, opt => opt.MapFrom(src => SysidFromReference(src.OBARTReference)))
             .ForMember(dest => dest.sysObkat, opt => opt.MapFrom(src => SysidFromReference(src.OBKATReference)))
              ;

            CreateMap<ANTOB, AntobDto>()
            .ForMember(dest => dest.sysObTyp, opt => opt.MapFrom(src => SysidFromReference(src.OBTYPReference)))
            .ForMember(dest => dest.sysObart, opt => opt.MapFrom(src => SysidFromReference(src.OBARTReference)))
            .ForMember(dest => dest.sysObkat, opt => opt.MapFrom(src => SysidFromReference(src.OBKATReference)))
            ;
             
            CreateMap<ANGOBINI, AngobIniDto>();
            CreateMap<ANGKALK, AngkalkDto>();
            CreateMap<ANTKALK, AntkalkDto>();

            CreateMap<Cic.OpenOne.Common.DTO.Prisma.ParamDto, Cic.One.DTO.ParamDto>();
            CreateMap<Cic.OpenOne.Common.DTO.SteplistDto, Cic.One.DTO.SteplistDto>();

            CreateMap<AngvarDto, ANGVAR>();
            CreateMap<AngobDto, ANGOB>();
            CreateMap<AntobDto, ANTOB>();

            CreateMap<AngobIniDto, ANGOBINI>();
            CreateMap<AngkalkDto, ANGKALK>();
            CreateMap<AntkalkDto, ANTKALK>();

            CreateMap<ANGOBAUST, ObjektAustDto>();
            CreateMap<ObjektAustDto, ANGOBAUST>();
            CreateMap<MycalcaustDto, MYCALCAUST>();
            CreateMap<MYCALCAUST, MycalcaustDto>();

            CreateMap<ANTOBAUST, ObjektAustDto>();
            CreateMap<ObjektAustDto, ANTOBAUST>();
            CreateMap<OBAUST, ObjektAustDto>()
                 .ForMember(dest => dest.sysob, opt => opt.MapFrom(src => SysidFromReference(src.OBReference)));
            
            CreateMap<ObjektAustDto, OBAUST>();

            CreateMap<ANGOBSL, AngobslDto>();
            CreateMap<ANGOBSLPOS, AngobslposDto>();

            CreateMap<ANTRAG, AntragDto>()
                .ForMember(dest => dest.sysKd, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)))
                .ForMember(dest => dest.antrag, opt => opt.MapFrom(src => src.ANTRAG1));

            CreateMap<ANTOB, AntobDto>()
                 .ForMember(dest => dest.sysObTyp, opt => opt.MapFrom(src => SysidFromReference(src.OBTYPReference)))
             .ForMember(dest => dest.sysObart, opt => opt.MapFrom(src => SysidFromReference(src.OBARTReference)))
             .ForMember(dest => dest.sysObkat, opt => opt.MapFrom(src => SysidFromReference(src.OBKATReference)))
             .ForMember(dest => dest.erstzul, opt => opt.MapFrom(src => src.ERSTZULASSUNG));
            CreateMap<AntobDto, ANTOB>()
             .ForMember(dest => dest.ERSTZULASSUNG, opt => opt.MapFrom(src => src.erstzul));
            CreateMap<ANTOBSL, AntobslDto>();
            CreateMap<ANTOBSLPOS, AntobslposDto>();

            CreateMap<VT, VertragDto>()
                .ForMember(dest => dest.sysKd, opt => opt.MapFrom(src => SysidFromReference(src.PERSONReference)));
            CreateMap<OB, ObDto>()
             .ForMember(dest => dest.sysObart, opt => opt.MapFrom(src => SysidFromReference(src.OBARTReference)));
             
            CreateMap<ObDto, OB>();
                 
            //   CreateMap<VTOBSL, VtobslposDto>();
            //   CreateMap<VTOBSLPOS, VtobslDto>();

            CreateMap<LSADD, LsaddDto>();

            // TODO nkk in edmx
            CreateMap<NKK, FinanzierungDto>();
            CreateMap<FinanzierungDto, NKK>();

            CreateMap<RN, RechnungFaelligDto>()
                .ForMember(dest => dest.SYSID, opt => opt.MapFrom(src => src.SYSRN));
            CreateMap<RechnungFaelligDto, RN>()
                .ForMember(dest => dest.SYSRN, opt => opt.MapFrom(src => src.SYSID));

            CreateMap<SLPOS, TilgungDto>();
            CreateMap<TilgungDto, SLPOS>();


            CreateMap<ExobjectDto, AngobDto>()
                .ForMember(dest => dest.typ, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.bezeichnung, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ahkBrutto, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.lpBrutto, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.grundBrutto, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.ubnahmekm, opt => opt.MapFrom(src => src.Wear))
                ;

            CreateMap<ExcustomerDto, ItDto>()
                 .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.LastName))
                 .ForMember(dest => dest.vorname, opt => opt.MapFrom(src => src.FirstName))
                 .ForMember(dest => dest.ort, opt => opt.MapFrom(src => src.City))
                 .ForMember(dest => dest.plz, opt => opt.MapFrom(src => src.ZipCode))
                 .ForMember(dest => dest.kontonr, opt => opt.MapFrom(src => src.AccountNumber))
                 .ForMember(dest => dest.blz, opt => opt.MapFrom(src => src.BankNumber))
                 .ForMember(dest => dest.bankName, opt => opt.MapFrom(src => src.BankName))
                 .ForMember(dest => dest.strasse, opt => opt.MapFrom(src => src.Street));
                 //.ForMember(dest => dest.sysland, opt => opt.MapFrom(src => src.Country))
            
            CreateMap<ExcalculationDto, AngkalkDto>()
                 .ForMember(dest => dest.valutaa, opt => opt.MapFrom(src => src.Date))
                 .ForMember(dest => dest.bginternbrutto, opt => opt.MapFrom(src => src.Price))
                 .ForMember(dest => dest.rwkalkbrutto, opt => opt.MapFrom(src => src.LastPayment))
                 .ForMember(dest => dest.bezeichnung, opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest => dest.bgexternbrutto, opt => opt.MapFrom(src => src.Price))
                 .ForMember(dest => dest.rwbrutto, opt => opt.MapFrom(src => src.LastPayment))
                 .ForMember(dest => dest.ratebrutto, opt => opt.MapFrom(src => src.MonthlyPayment))
                 .ForMember(dest => dest.zins, opt => opt.MapFrom(src => src.InterestRate))
                 .ForMember(dest => dest.zins, opt => opt.MapFrom(src => src.BaseContractType))
                 .ForMember(dest => dest.lz, opt => opt.MapFrom(src => src.Period))
                 .ForMember(dest => dest.sz, opt => opt.MapFrom(src => src.OneOffPayment));

            CreateMap<AccountExtDataDto, AccountExtDataDto>();

            CreateMap<MemoDto, WFMMEMO>()
                .ForMember(dest => dest.CREATETIME, opt => opt.MapFrom(src => src.CREATETIME.HasValue ? Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(src.CREATETIME.Value) : (long?)null))
                .ForMember(dest => dest.EDITTIME, opt => opt.MapFrom(src => src.EDITTIME.HasValue ? Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(src.EDITTIME.Value) : (long?)null))
                ;
            CreateMap<ClarificationDto, WFMMEMO>();

            CreateMap<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto, QueueDto>();
            CreateMap<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto, QueueRecordDto>();
            CreateMap<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto, QueueRecordValueDto>();

            CreateMap<QueueDto,CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto>();
            CreateMap<QueueRecordDto,CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto>();
            CreateMap<QueueRecordValueDto,CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto>();

            CreateMap<WFMMEMO, MemoDto>()
                .ForMember(dest => dest.CREATETIME, opt => opt.MapFrom(src => ClarionTimeToDateTime(src.CREATETIME)))
                .ForMember(dest => dest.EDITTIME, opt => opt.MapFrom(src => ClarionTimeToDateTime(src.EDITTIME)))
                .ForMember(dest => dest.SYSWFMMKAT, opt => opt.MapFrom(src =>
                    {
                        if (src.WFMMKAT != null)
                            return src.WFMMKAT.SYSWFMMKAT;
                        if (!src.WFMMKATReference.IsLoaded)
                            src.WFMMKATReference.Load();
                        if (src.WFMMKATReference.Value == null)
                            return 0;
                        return src.WFMMKATReference.Value.SYSWFMMKAT;
                    }))
                ;


            CreateMap<ItadresseDto, ITADRESSE>().ForMember(dest => dest.TYPE, opt => opt.Ignore()).ForMember(dest => dest.IT, src => src.Ignore());
            CreateMap<ITADRESSE, ItadresseDto>();
            CreateMap<AdresseDto, ADRESSE>().ForMember(dest => dest.TYPE, opt => opt.Ignore()).ForMember(dest => dest.PERSON, src => src.Ignore());
            CreateMap<ADRESSE, AdresseDto>();
           
        }

        /// <summary>
        /// Convert clarion time to DateTime without exception. If it fails, just return null.
        /// </summary>
        /// <param name="clarionTime">time in clarion format</param>
        /// <returns>time as DateTime or null</returns>
        private static DateTime? ClarionTimeToDateTime(long? clarionTime)
        {
            if (!clarionTime.HasValue)
                return (DateTime?)null;
            try
            {
                return Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDateTime((int)clarionTime.Value);
            }
            catch (Exception)
            {
                //ignore exception. Not being able to parse means we just don't use the value.
                return (DateTime?)null;
            }
        }

    }
}
