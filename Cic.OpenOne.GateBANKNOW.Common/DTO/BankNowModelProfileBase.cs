using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.CrifSoapService;
using CIC.Database.OL.EF6.Model;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// BankNowModelProfileBase-Klasse
    /// </summary>
    public class BankNowModelProfileBase : OpenOne.Common.DTO.MappingProfileBase
    {
        /// <summary>
        /// Gibt die Sysid einer EntityReferenz zurück
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityReference"></param>
        /// <returns></returns>
        public long SysidFromReference<T>(EntityReference<T> entityReference) where T : class
        {
            return entityReference.EntityKey == null ? 0 : long.Parse(entityReference.EntityKey.EntityKeyValues.ElementAt(0).Value.ToString());
        }

        /// <summary>
        /// Konfigurieren
        /// </summary>
        public BankNowModelProfileBase()
        {

            #region Config for Cic.OpenOne.GateBANKNOW.Common.BO.SearchKundeBo----------------------
            CreateMap<Candidate, KundeExternResultDto>()
              .ConstructUsing(src => SearchKundeBo.GetKundeExtern(src.address))
              .ForMember(dest => dest.rang, opt => opt.MapFrom(src => src.candidateRank))
              .ForMember(dest => dest.groupId, opt => opt.MapFrom(src => src.groupId))
              .ForMember(dest => dest.adressId, opt => opt.MapFrom(src => SearchKundeBo.GetAddressId(src.identifiers)))
              ;


            CreateMap<Location, KundeExternResultDto>()
                .ForMember(dest => dest.ort, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.land, opt => opt.MapFrom(src => src.country))
                .ForMember(dest => dest.hsnr, opt => opt.MapFrom(src => src.houseNumber))
                .ForMember(dest => dest.regionCode, opt => opt.MapFrom(src => src.regionCode))
                .ForMember(dest => dest.strasse, opt => opt.MapFrom(src => src.street))
                .ForMember(dest => dest.subRegionCode, opt => opt.MapFrom(src => src.subRegionCode))
                .ForMember(dest => dest.plz, opt => opt.MapFrom(src => src.zip))
                ;

            CreateMap<PersonAddressDescription, KundeExternResultDto>()
                .ForMember(dest => dest.gebdatum, opt => opt.MapFrom(src => SearchKundeBo.GetDateTime(src.birthDate)))
                .ForMember(dest => dest.coname, opt => opt.MapFrom(src => src.coName))
                .ForMember(dest => dest.vorname, opt => opt.MapFrom(src => src.firstName))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.lastName))
                .ForMember(dest => dest.geburtsName, opt => opt.MapFrom(src => src.maidenName))
                .ForMember(dest => dest.zweiterVorname, opt => opt.MapFrom(src => src.middleName))
                .ForMember(dest => dest.anredeCode, opt => opt.MapFrom(src => SearchKundeBo.GetAnredeCode(src.sex, src.sexSpecified)))
                .ForMember(dest => dest.firma, opt => opt.MapFrom(src => 0))
                ;

            CreateMap<CompanyAddressDescription, KundeExternResultDto>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.companyName))
                .ForMember(dest => dest.coname, opt => opt.MapFrom(src => src.coName))
                .ForMember(dest => dest.firma, opt => opt.MapFrom(src => 1))
                ;

            CreateMap<AddressDescription, KundeExternResultDto>()
                .Include<PersonAddressDescription, KundeExternResultDto>()
                .Include<CompanyAddressDescription, KundeExternResultDto>()
                ;

            CreateMap<MatchedAddress, KundeExternResultDto>()
                .ConstructUsing(src => SearchKundeBo.GetKundeExtern(src.address))
                .ForMember(dest => dest.rang, opt => opt.MapFrom(src => -1))
                .ForMember(dest => dest.identifikationsTyp, opt => opt.MapFrom(src => src.identificationType.ToString()))
                .ForMember(dest => dest.adressId, opt => opt.MapFrom(src => SearchKundeBo.GetAddressId(src.identifiers)))
                ;

            CreateMap<LocationIdentification, osearchKundeExternNonGenericDto>()
                .ForMember(dest => dest.haustyp, opt => opt.MapFrom(src => src.houseType))
                .ForMember(dest => dest.adresstyp, opt => opt.MapFrom(src => src.locationIdentificationType.ToString()))
                ;

            CreateMap<AddressMatchResult, osearchKundeExternNonGenericDto>()
                .ForMember(dest => dest.character, opt => opt.MapFrom(src => SearchKundeBo.GetStringValue(src.character, src.characterSpecified)))
                .ForMember(dest => dest.result, opt => opt.MapFrom(src => SearchKundeBo.GetResults(src.foundAddress, src.candidates)))
                .ForMember(dest => dest.ergebnis, opt => opt.MapFrom(src => src.addressMatchResultType.ToString()))
                .ForMember(dest => dest.namehint, opt => opt.MapFrom(src => SearchKundeBo.GetStringValue(src.nameHint, src.nameHintSpecified)))
                ;

            CreateMap<TypeIdentifyAddressResponse, osearchKundeExternNonGenericDto>();
            #endregion

            #region createAngebot
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto, ANGVAR>();
            CreateMap<OpenOne.Common.DTO.AngAntProvDto, ANGPROV>();
            CreateMap<OpenOne.Common.DTO.AngAntSubvDto, ANGSUBV>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntAblDto, ANGABL>();
            #endregion
            #region updateAngebot
            CreateMap<ANGOBBRIEF, AngAntObBriefDto>();
            #endregion
            #region getAntragSwitch
            CreateMap<PERSON, KundeDto>();
            CreateMap<ANTOB, AngAntObDto>()
                    .ForMember(dest => dest.brief, opt => opt.Ignore());
            CreateMap<ANTOBBRIEF, AngAntObBriefDto>();
            CreateMap<ANTKALK, AngAntKalkDto>()
                   .ForMember(dest => dest.calcRsvgesamt, opt => opt.MapFrom(src => src.RSVGESAMT))
                   .ForMember(dest => dest.calcZinskosten, opt => opt.MapFrom(src => src.ZINSKOSTEN))
                   .ForMember(dest => dest.calcRsvmonat, opt => opt.MapFrom(src => src.RSVMONAT))
                   .ForMember(dest => dest.calcRsvzins, opt => opt.MapFrom(src => src.RSVZINS))
                   .ForMember(dest => dest.calcUstzins, opt => opt.MapFrom(src => src.USTZINS));
            #endregion
            #region getAngebot
            CreateMap<ANGKALK, AngAntKalkDto>()
                   .ForMember(dest => dest.calcRsvgesamt, opt => opt.MapFrom(src => src.RSVGESAMT))
                   .ForMember(dest => dest.calcZinskosten, opt => opt.MapFrom(src => src.ZINSKOSTEN))
                   .ForMember(dest => dest.calcRsvmonat, opt => opt.MapFrom(src => src.RSVMONAT))
                   .ForMember(dest => dest.calcRsvzins, opt => opt.MapFrom(src => src.RSVZINS))
                   .ForMember(dest => dest.calcUstzins, opt => opt.MapFrom(src => src.USTZINS));
            #endregion
            #region calculateMitNeuAnzahlung
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AngAntProvDto, Cic.OpenOne.Common.DTO.AngAntProvDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AngAntSubvDto, Cic.OpenOne.Common.DTO.AngAntSubvDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>();
            #endregion
            #region processAngebotToAntrag
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto>();
            #endregion

            #region copyAntrag
            CreateMap<OBUSETYPE, OBUSETYPE>();
            CreateMap<ANTKALKFS, ANTKALKFS>();
            CreateMap<ANTOB, ANTOB>()
                                            .ForMember(dest => dest.SYSOB, opt => opt.Ignore())
                                            .ForMember(dest => dest.OBJEKT, opt => opt.Ignore())
                                            .ForMember(dest => dest.ANTKALK, opt => opt.Ignore())

                                            .ForMember(dest => dest.ANTOBBRIEF, opt => opt.Ignore())
                                            .ForMember(dest => dest.PERSON, opt => opt.Ignore())

                                            .ForMember(dest => dest.ANTOBHDList, opt => opt.Ignore())
                                            .ForMember(dest => dest.ANTOBSICHList, opt => opt.Ignore())
                                            
                                            .ForMember(dest => dest.ANTOBSLList, opt => opt.Ignore())
                                            .ForMember(dest => dest.ANTPROVList, opt => opt.Ignore())
                                            .ForMember(dest => dest.ANTVSList, opt => opt.Ignore())
                                            .ForMember(dest => dest.OBAUSTList, opt => opt.Ignore())
                                            .ForMember(dest => dest.OBPOOLList, opt => opt.Ignore())
                                            ;

            CreateMap<OBTYP, OBTYP>();
            CreateMap<ANTPROV, ANTPROV>();
            CreateMap<ANTKALK, ANTKALK>().ForMember(dest => dest.SYSKALK, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTKALKFS, opt => opt.Ignore())

                                                .ForMember(dest => dest.ANTOB, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTRAG, opt => opt.Ignore())
                                                .ForMember(dest => dest.OBTYP, opt => opt.Ignore())
                                                .ForMember(dest => dest.OBUSETYPE, opt => opt.Ignore())
                                                .ForMember(dest => dest.PRPRODUCT, opt => opt.Ignore())

                                                .ForMember(dest => dest.ANTRAG, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTPROVList, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTOBSLList, opt => opt.Ignore())
                                                
                                                .ForMember(dest => dest.ANTKALKVARList, opt => opt.Ignore())
                                                ;

            CreateMap<ANTRAG, ANTRAG>()
                                              
                                              .ForMember(dest => dest.SYSID, opt => opt.Ignore())
                                              
                                              .ForMember(dest => dest.ANTKALKList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANTOBHDList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANTOBSLList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANGOBList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANTOPTION, opt => opt.Ignore())
                                              
                                              .ForMember(dest => dest.ANTPROVList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANTSUBVList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANTVSList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANTZEKABLList, opt => opt.Ignore())
                                              .ForMember(dest => dest.APPROVALList, opt => opt.Ignore())
                                              .ForMember(dest => dest.BONITAETList, opt => opt.Ignore())
                                              
                                              .ForMember(dest => dest.GENEHM, opt => opt.Ignore())
                                              
                                              .ForMember(dest => dest.KREMOList, opt => opt.Ignore())

                                              .ForMember(dest => dest.ITKONTOREFList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ITRELATEList, opt => opt.Ignore())
                                              .ForMember(dest => dest.KONTOREFList, opt => opt.Ignore())
                                              
                                              .ForMember(dest => dest.ZEKList, opt => opt.Ignore())
                                              
                                              .ForMember(dest => dest.SLAList, opt => opt.Ignore())

                                              // Ticket#2013012410000188 — Fehler beim Antrag kopieren
                                              // CONTACTList ist wahrscheinlich nur für AKFCRM
                                              .ForMember(dest => dest.CONTACTList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANTOBList, opt => opt.Ignore())
                                              .ForMember(dest => dest.ANTOBSICHList, opt => opt.Ignore())
                                              .ForMember(dest => dest.CARDList, opt => opt.Ignore())
                                              .ForMember(dest => dest.PRJOKERList, opt => opt.Ignore())
                                              ;



            CreateMap<ANTOBSICH, ANTOBSICH>();
                                                    
            #endregion

            
        }


    }
}