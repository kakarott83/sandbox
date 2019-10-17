using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using CIC.Database.OL.EF6.Model;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// BankNowModelProfileGeneral-Klasse
    /// </summary>
    public class BankNowModelProfileGeneral : BankNowModelProfileBase
    {
        /// <summary>
        /// Konfigurieren
        /// </summary>
        public BankNowModelProfileGeneral()
        {
            
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, ANGKALK>()
                   .ForMember(dest => dest.SYSKALK, opt => opt.Ignore())
                   .ForMember(dest => dest.RSVGESAMT, opt => opt.MapFrom(src => (decimal?)src.calcRsvgesamt))
                   .ForMember(dest => dest.ZINSKOSTEN, opt => opt.MapFrom(src => (decimal?)src.calcZinskosten))
                   .ForMember(dest => dest.RSVMONAT, opt => opt.MapFrom(src => (decimal?)src.calcRsvmonat))
                   .ForMember(dest => dest.RSVZINS, opt => opt.MapFrom(src => (decimal?)src.calcRsvzins))
                   .ForMember(dest => dest.USTZINS, opt => opt.MapFrom(src => (decimal?)src.calcUstzins));

            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, ANTKALK>()
                 .ForMember(dest => dest.SYSKALK, opt => opt.Ignore())
                 .ForMember(dest => dest.RSVGESAMT, opt => opt.MapFrom(src => (decimal?)src.calcRsvgesamt))
                 .ForMember(dest => dest.ZINSKOSTEN, opt => opt.MapFrom(src => (decimal?)src.calcZinskosten))
                 .ForMember(dest => dest.RSVMONAT, opt => opt.MapFrom(src => (decimal?)src.calcRsvmonat))
                 .ForMember(dest => dest.RSVZINS, opt => opt.MapFrom(src => (decimal?)src.calcRsvzins))
                 .ForMember(dest => dest.USTZINS, opt => opt.MapFrom(src => (decimal?)src.calcUstzins))
                 .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal != null));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto, ITPKZ>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto, PKZ>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto, UKZ>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto, ITUKZ>();

            CreateMap<EAIQIN, Cic.OpenOne.GateBANKNOW.Common.DTO.EaiqinDto>();
            CreateMap<EAIQOU, Cic.OpenOne.GateBANKNOW.Common.DTO.EaiqoutDto>();
            CreateMap<EAIHOT, Cic.OpenOne.Common.DTO.EaihotDto>();
            CreateMap<Cic.OpenOne.Common.DTO.EaihotDto, EAIHOT>();
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.EaiqinDto, EAIQIN>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.EaiqoutDto, EAIQOU>();

            //Ignore type, it is not really in source (only by the inherited field Type)
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto, ITADRESSE>().ForMember(dest => dest.TYPE, opt => opt.Ignore()).ForMember(dest => dest.IT, src => src.Ignore());
            CreateMap<ITADRESSE, Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto>();
            
            
           CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, ANTRAG>()
                   .ForMember(dest => dest.KKGPFLICHT, opt => opt.Ignore())
                   .ForMember(dest => dest.TESTFLAG, opt => opt.Ignore())
                   .ForMember(dest => dest.NOTSTOPFLAG, opt => opt.Ignore())
                   .ForMember(dest => dest.COUNTRENEWVAL, opt => opt.MapFrom(a=>a.contractextcount))
                   .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal!=null));

          
            CreateMap<Cic.OpenOne.Common.DTO.AngAntProvDto, ANTPROV>();

            CreateMap<Cic.OpenOne.Common.DTO.AngAntProvDto, PROV>()
                .ForMember(dest => dest.SYSPROV, opt => opt.Ignore());

            CreateMap<Cic.OpenOne.Common.DTO.ProvKalkDto, PROVKALK>()
                .ForMember(dest => dest.SYSPROVKALK, opt => opt.Ignore())
                .ForMember(dest => dest.PROV, opt => opt.Ignore())
                .ForMember(dest => dest.GEBIET, opt => opt.MapFrom(src => src.area))
                .ForMember(dest => dest.KOMMENTAR, opt => opt.MapFrom(src => src.remark))
                .ForMember(dest => dest.SYSGEBIET, opt => opt.MapFrom(src => src.syslease));
            
            CreateMap<Cic.OpenOne.Common.DTO.AngAntSubvDto, ANTSUBV>();
            CreateMap<Cic.OpenOne.Common.DTO.AngAntVsDto, ANTVS>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto, ANGOBBRIEF>();
            CreateMap<AngAntObBriefDto, ANTOBBRIEF>().ForMember(dest => dest.ECODEID, opt => opt.Ignore())
                               .ForMember(dest => dest.ECODESTATUS, opt => opt.Ignore());

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntAblDto, ANTABL>();
            CreateMap<ANTABL, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntAblDto>();

            
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, ANGEBOT>()
               //.ForMember(dest => dest.MANDATREFERENZ, opt => opt.MapFrom(src => src.emboss))
               .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal!=null));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KundePlusDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>();
            
            CreateMap<IT, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>()
                    .ForMember(dest => dest.telefon, opt => opt.MapFrom(s => s.PTELEFON))
                    .ForMember(dest => dest.telefon2, opt => opt.MapFrom(s => s.TELEFON))
                    .ForMember(dest => dest.sysland, opt => opt.MapFrom(src => src.SYSLAND))
                    .ForMember(dest => dest.syskd, opt => opt.MapFrom(s => s.SYSPERSON) );

            CreateMap<VART, Cic.OpenOne.GateBANKNOW.Common.DTO.VartDto>();
            CreateMap<CIC.Database.PRISMA.EF6.Model.VART, Cic.OpenOne.GateBANKNOW.Common.DTO.VartDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AngAntVsDto, ANGVS>();

            CreateMap<Cic.OpenOne.Common.DTO.Prisma.PRVSDto, Cic.OpenOne.Common.DTO.AvailableServiceDto>().ForMember(a => a.sysID, m => m.MapFrom(s => s.SYSVSTYP))
                                                            .ForMember(a => a.beschreibung, m => m.MapFrom(s => s.BESCHREIBUNG))
                                                            .ForMember(a => a.bezeichnung, m => m.MapFrom(s => s.BEZEICHNUNG))
                                                            .ForMember(a => a.selected, m => m.MapFrom(srcVal => srcVal.SELECTED))
                                                            .ForMember(a => a.editable, m => m.MapFrom(s => s.EDITABLE))
                                                            .ForMember(a => a.code, m => m.MapFrom(s => s.CODE))
                                                            .ForMember(a => a.mitfin, m => m.MapFrom(s => s.MITFIN))
                                                            .ForMember(a => a.serviceType, m => m.MapFrom(s => s.SERVICETYPE));
                                                            

            
            CreateMap<VT, Cic.OpenOne.GateBANKNOW.Common.DTO.VertragDto>();

            CreateMap<PKZ, PKZ>().ForMember(dest => dest.SYSPKZ, opt => opt.Ignore()) ;

            CreateMap<ITPKZ, ITPKZ>().ForMember(dest => dest.SYSITPKZ, opt => opt.Ignore()) ;

            
            CreateMap<UKZ, UKZ>().ForMember(dest => dest.SYSUKZ, opt => opt.Ignore()) ;

            CreateMap<ITUKZ, ITUKZ>().ForMember(dest => dest.SYSITUKZ, opt => opt.Ignore()) ;

           



            CreateMap<FileattDto, FILEATT>()
                   .ForMember(a => a.TYPCODE, m => m.MapFrom(s => s.format));

            CreateMap<FILEATT, FileattDto>()
                 .ForMember(a => a.format, m => m.MapFrom(s => s.TYPCODE));

            CreateMap<DMSDOC, FileDto>()
                .ForMember(a => a.content, m => m.MapFrom(s => s.INHALT))
                .ForMember(a => a.fileName, m => m.MapFrom(s => s.DATEINAME))
                .ForMember(a => a.description, m => m.MapFrom(s => s.BEMERKUNG))
                .ForMember(a => a.sysCrtDate, m => m.MapFrom(s => s.GEDRUCKTAM))
                .ForMember(a => a.syscrtuser, m => m.MapFrom(s => s.GEDRUCKTVON))
                .ForMember(dest => dest.syscrtuser, opt => opt.Ignore())
                .ForMember(a => a.activFlag, m => m.MapFrom(s => s.UNGUELTIGFLAG))
                .ForMember(a => a.sysCrtTime, m => m.MapFrom(s => s.GEDRUCKTUM))

                ;
            CreateMap<FileDto,DMSDOC>()
               .ForMember(a => a.INHALT, m => m.MapFrom(s => s.content))
               .ForMember(a => a.DATEINAME, m => m.MapFrom(s => s.fileName))
               .ForMember(a => a.BEMERKUNG, m => m.MapFrom(s => s.description))
               .ForMember(a => a.GEDRUCKTAM, m => m.MapFrom(s => s.sysCrtDate))
               .ForMember(a => a.GEDRUCKTVON, m => m.MapFrom(s => s.syscrtuser))
               .ForMember(dest => dest.GEDRUCKTVON, opt => opt.Ignore())
               .ForMember(a => a.UNGUELTIGFLAG, m => m.MapFrom(s => s.activFlag))
               .ForMember(a => a.GEDRUCKTUM, m => m.MapFrom(s => s.sysCrtTime))
               
               ;

            CreateMap<DmsDocDto, DMSDOC>();

            CreateMap<FileDto, DMSDOCAREA>()
                 .ForMember(a => a.SYSID, m => m.MapFrom(s => s.sysId))
                 .ForMember(a => a.AREA, m => m.MapFrom(s => s.area))
                 ;

            CreateMap<DMSDOCAREA, FileDto>()
                 .ForMember(a => a.sysId, m => m.MapFrom(s => s.SYSID))
                 .ForMember(a => a.area, m => m.MapFrom(s => s.AREA))
                 ;

           //BNR11
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, ANTKALKVAR>()
                  .ForMember(dest => dest.LAUFZEIT, opt => opt.MapFrom(src => src.lz));

            CreateMap<ANTKALKVAR,Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>()
                .ForMember(dest => dest.lz, opt => opt.MapFrom(src => src.LAUFZEIT));



            CreateMap<AngAntObDto, ANGOB>().ForMember(dest => dest.SYSOB, opt => opt.Ignore())
                .ForMember(a => a.BRIEF, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal!=null));
            CreateMap<AngAntObDto, ANTOB>().ForMember(dest => dest.SYSOB, opt => opt.Ignore())
                .ForMember(a => a.BRIEF, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal!=null));

            CreateMap<AngAntOptionDto, ANTOPTION>();
            CreateMap<AngAntOptionDto, ANGOPTION>();
            CreateMap<ANGOPTION, AngAntOptionDto>();
            CreateMap<ANTOPTION, AngAntOptionDto>();

            CreateMap<ITPKZ, PkzDto>();
            CreateMap<UKZ, UkzDto>();
            CreateMap<ITUKZ, UkzDto>();
            CreateMap<ITUKZ, KundeDto>();
            CreateMap<ITPKZ, KundeDto>();
            
            CreateMap<KundeDto, KundePlusDto>();
            CreateMap<LAND, LandDto>();
            CreateMap<KONTO, KontoDto>();
            CreateMap<RISIKOKL, RisikoklDto>();
            CreateMap<PKZ, PkzDto>();
        }
    }
}