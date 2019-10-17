using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Mapper;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Model.DdOw;
using System.Data;
using Cic.One.Web.BO;
using Cic.One.DTO;
using Cic.One.DTO.BN;

namespace Cic.One.GateBANKNOW
{

    /// <summary>
    /// CIC.ONE GateBANKNOW Automapper configuration initalizer
    /// </summary>
    public class MappingProfile :  Profile
    {
      
        /// <summary>
        /// Konfigurieren
        /// </summary>
        protected override void Configure()
        {
            CreateMap<int, bool>().ConvertUsing<GenericNullableConverter<int, bool>>();
            CreateMap<bool, int>().ConvertUsing<GenericNullableConverter<bool, int>>();
            CreateMap<short, bool>().ConvertUsing<GenericNullableConverter<short, bool>>();
            CreateMap<bool, short>().ConvertUsing<GenericNullableConverter<bool, short>>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto, AccountDto>()
               .ForMember(dest => dest.rechtsform, opt => opt.MapFrom(src => src.LegalForm.ToString()))
               .ForMember(dest => dest.vorname, opt => opt.MapFrom(src => src.FirstName))
               .ForMember(dest => dest.strasse, opt => opt.MapFrom(src => src.Street))
               .ForMember(dest => dest.hsnr, opt => opt.MapFrom(src => src.Housenumber))
               .ForMember(dest => dest.plz, opt => opt.MapFrom(src => src.Zip))
               .ForMember(dest => dest.ort, opt => opt.MapFrom(src => src.City))
               .ForMember(dest => dest.landBezeichnung, opt => opt.MapFrom(src => src.Country))
               .ForMember(dest => dest.gebdatum, opt => opt.MapFrom(src =>
               {
                   if (src.Birthdate == null || src.Birthdate == "")
                       return new DateTime();
                   return DateTime.Parse(src.Birthdate);
               }))
               .ForMember(dest => dest.gruendung, opt => opt.MapFrom(src =>
               {
                   if (src.FoundingDate == null || src.FoundingDate == "")
                       return new DateTime();
                   return DateTime.Parse(src.FoundingDate);
               }))
               .ForMember(dest => dest.landNatBezeichnung, opt => opt.MapFrom(src => src.Nationality))
               .ForMember(dest => dest.anredeCode, opt => opt.MapFrom(src => src.Sex == 0/*unknown*/ ? "" : src.Sex == 1/*male*/ ? "2"/*Herr*/ : src.Sex == 2/*female*/ ? "3"/*Frau*/ : "")) //"müsste irgendwie mit DDLKPPOS und ZAKADRDESC:sex zusammenhängen"
               .ForMember(dest => dest.code, opt => opt.MapFrom(src=>src.KundenId))
               ;
            CreateMap<AccountDto, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto>()
                //.ForMember(dest => dest.LegalForm, opt => opt.MapFrom(src => src.rechtsform == null ? 0 : src.rechtsform.Length == 0 ? 0 : int.Parse(src.rechtsform)))
                .ForMember(dest => dest.LegalForm, opt => opt.MapFrom(src =>
                {
                    // ret 1=privat 2=GmbH
                    if (src.rechtsformCode == null)
                    {
                        if (src.rechtsform == "1") return 1;
                    }
                    int ret = 3;
                    try
                    {
                        ret = int.Parse(src.rechtsformCode);
                    }
                    catch (Exception )
                    {
                        try
                        {
                            ret = int.Parse(src.rechtsform);
                        }
                        catch (Exception )
                        {
                            return 1;
                        }
                    }
                    if (ret == 0) return 1;
                    return ret;
                }))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.vorname))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.strasse))
                .ForMember(dest => dest.Housenumber, opt => opt.MapFrom(src => src.hsnr))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.plz))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ort))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.landBezeichnung))
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src =>
                {
                    if (src.gebdatum == null||!src.gebdatum.HasValue)
                        return "";
                    return src.gebdatum.Value.ToString("yyyy-MM-dd");
                }))
                .ForMember(dest => dest.FoundingDate, opt => opt.MapFrom(src =>
                {
                    if (src.gruendung == null ||  !src.gruendung.HasValue)
                        return "";
                    return src.gruendung.Value.ToString("yyyy-MM-dd");
                }))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.landNatBezeichnung))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.anredeCode == null ? 0/*unknown*/ : src.anredeCode.Equals("2")/*Herr*/ ? 1/*male*/ : src.anredeCode.Equals("3")/*Frau*/ ? 2/*female*/ : 0/*unknown*/)) //"müsste irgendwie mit DDLKPPOS und ZAKADRDESC:sex zusammenhängen"
                ;
            
            CreateMap<BNAngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, BNAngebotDto>()
                .ForMember(dest => dest.kundeName, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.name))
                .ForMember(dest => dest.kundeVorname, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.vorname))
                .ForMember(dest => dest.kundePlz, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.plz))
                .ForMember(dest => dest.kundeOrt, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.ort))
                .ForMember(dest => dest.kundeStrasse, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.strasse))
                .ForMember(dest => dest.kundeHausnr, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.hsnr));

            CreateMap<BNAntragDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto,BNAntragDto>()
                .ForMember(dest => dest.kundeName, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.name))
                .ForMember(dest => dest.kundeVorname, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.vorname))
                .ForMember(dest => dest.kundePlz, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.plz))
                .ForMember(dest => dest.kundeOrt, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.ort))
                .ForMember(dest => dest.kundeStrasse, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.strasse))
                .ForMember(dest => dest.kundeHausnr, opt => opt.MapFrom(src => src.kunde == null ? "" : src.kunde.hsnr));
            CreateMap<BNKalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, BNKalkulationDto>();
            CreateMap<BNAngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, BNAngAntKalkDto>();
            CreateMap<AngAntOptionDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntOptionDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntOptionDto, AngAntOptionDto>();

            CreateMap<Cic.One.DTO.BNKundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.One.DTO.BNKundeDto>();

            CreateMap<Cic.One.DTO.BNKundeDto, Cic.One.DTO.ItDto>();
            CreateMap<Cic.One.DTO.ItDto, Cic.One.DTO.BNKundeDto>();

            CreateMap<Cic.One.DTO.ZusatzdatenDto, Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto, Cic.One.DTO.ZusatzdatenDto>();

            CreateMap<Cic.One.DTO.UkzDto, Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto, Cic.One.DTO.UkzDto>();

            CreateMap<Cic.One.DTO.KontoDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto, Cic.One.DTO.KontoDto>();

            CreateMap<Cic.One.DTO.AdresseDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto, Cic.One.DTO.AdresseDto>();

            CreateMap<Cic.One.DTO.PkzDto, Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto, Cic.One.DTO.PkzDto>();

            CreateMap<Cic.One.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.One.DTO.KalkulationDto>();

            CreateMap<Cic.One.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto, Cic.One.DTO.AngAntVarDto>();

            CreateMap<Cic.One.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto, Cic.One.DTO.AngAntObDto>();

            CreateMap<Cic.One.DTO.AngAntObBriefDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto, Cic.One.DTO.AngAntObBriefDto>();

            CreateMap<Cic.One.DTO.AngAntObAustDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObAustDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObAustDto, Cic.One.DTO.AngAntObAustDto>();

            CreateMap<Cic.One.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, Cic.One.DTO.AngAntKalkDto>();

            CreateMap<Cic.One.DTO.AngAntDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntDto, Cic.One.DTO.AngAntDto>();

            CreateMap<Cic.One.DTO.AngAntAblDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntAblDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntAblDto, Cic.One.DTO.AngAntAblDto>();


            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto, ZekContractDto>()
               .ForMember(dest => dest.datumBonitaetIKO, opt => opt.MapFrom(src =>
               {
                   if (src.datumBonitaetIKO == null || src.datumBonitaetIKO == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumBonitaetIKO);
               }))
               .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src =>
               {
                   if (src.datumBonitaetZEK == null || src.datumBonitaetZEK == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumBonitaetZEK);
               }))
               .ForMember(dest => dest.datumErsteRate, opt => opt.MapFrom(src =>
               {
                   if (src.datumErsteRate == null || src.datumErsteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumErsteRate);
               }))
               .ForMember(dest => dest.datumLetzteRate, opt => opt.MapFrom(src =>
               {
                   if (src.datumLetzteRate == null || src.datumLetzteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumLetzteRate);
               }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFestkreditDescriptionDto, ZekContractDto>()
               .ForMember(dest => dest.datumBonitaetIKO, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetIKO == null || src.DatumBonitaetIKO == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetIKO);
               }))
               .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetZEK == null || src.DatumBonitaetZEK == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetZEK);
               }))
               .ForMember(dest => dest.datumVertragsBeginn, opt => opt.MapFrom(src =>
               {
                   if (src.datumVertragsBeginn == null || src.datumVertragsBeginn == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumVertragsBeginn);
               }))
               .ForMember(dest => dest.datumVertragsEnde, opt => opt.MapFrom(src =>
               {
                   if (src.datumVertragsEnde == null || src.datumVertragsEnde == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumVertragsEnde);
               }));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAmtsinformationDescriptionDto, ZekContractDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKartenengagementDescriptionDto, ZekContractDto>()
                // Unsauberer Fix weil Frontendanzeige falsch
               .ForMember(dest => dest.datumErsteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumKontoEroeffnung == null || src.DatumKontoEroeffnung == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumKontoEroeffnung);
               }))
                // Unsauberer Fix weil Frontendanzeige falsch
               .ForMember(dest => dest.datumLetzteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumSaldoStichTag == null || src.DatumSaldoStichTag == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumSaldoStichTag);
               }))
                .ForMember(dest => dest.kreditbetrag, opt => opt.MapFrom(src =>
                {
                    return src.SaldoAbrechnungsTag;
                }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKarteninformationDescriptionDto, ZekKartenmeldungDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKontokorrentkreditDescriptionDto, ZekContractDto>()
                .ForMember(dest => dest.kreditbetrag, opt => opt.MapFrom(src =>
                {
                    return src.kreditLimite;
                }))
               .ForMember(dest => dest.datumBonitaetIKO, opt => opt.MapFrom(src =>
               {
                   if (src.datumBonitaetIKO == null || src.datumBonitaetIKO == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumBonitaetIKO);
               }))
               .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src =>
               {
                   if (src.datumBonitaetZEK == null || src.datumBonitaetZEK == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumBonitaetZEK);
               }))
                // Unsauberer Fix weil Frontendanzeige falsch
               .ForMember(dest => dest.datumErsteRate, opt => opt.MapFrom(src =>
               {
                   if (src.datumVertragsBeginn == null || src.datumVertragsBeginn == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumVertragsBeginn);
               }))
                // Unsauberer Fix weil Frontendanzeige falsch
               .ForMember(dest => dest.datumLetzteRate, opt => opt.MapFrom(src =>
               {
                   if (src.datumVertragsEnde == null || src.datumVertragsEnde == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumVertragsEnde);
               }))
               .ForMember(dest => dest.datumVertragsBeginn, opt => opt.MapFrom(src =>
               {
                   if (src.datumVertragsBeginn == null || src.datumVertragsBeginn == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumVertragsBeginn);
               }))
               .ForMember(dest => dest.datumVertragsEnde, opt => opt.MapFrom(src =>
               {
                   if (src.datumVertragsEnde == null || src.datumVertragsEnde == "")
                       return new DateTime();
                   return DateTime.Parse(src.datumVertragsEnde);
               }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekLeasingMietvertragDescriptionDto, ZekContractDto>()
               .ForMember(dest => dest.datumBonitaetIKO, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetIKO == null || src.DatumBonitaetIKO == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetIKO);
               }))
               .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetZEK == null || src.DatumBonitaetZEK == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetZEK);
               }))
               .ForMember(dest => dest.datumErsteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumErsteRate == null || src.DatumErsteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumErsteRate);
               }))
               .ForMember(dest => dest.datumLetzteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumLetzteRate == null || src.DatumLetzteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumLetzteRate);
               }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekSolidarschuldnerDescriptionDto, ZekContractDto>()
               .ForMember(dest => dest.datumBonitaetIKO, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetIKO == null || src.DatumBonitaetIKO == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetIKO);
               }))
               .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetZEK == null || src.DatumBonitaetZEK == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetZEK);
               }))
               .ForMember(dest => dest.datumErsteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumErsteRate == null || src.DatumErsteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumErsteRate);
               }))
               .ForMember(dest => dest.datumLetzteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumLetzteRate == null || src.DatumLetzteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumLetzteRate);
               }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungskreditDescriptionDto, ZekContractDto>()
               .ForMember(dest => dest.datumBonitaetIKO, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetIKO == null || src.DatumBonitaetIKO == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetIKO);
               }))
               .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetZEK == null || src.DatumBonitaetZEK == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetZEK);
               }))
               .ForMember(dest => dest.datumErsteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumErsteRate == null || src.DatumErsteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumErsteRate);
               }))
               .ForMember(dest => dest.datumLetzteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumLetzteRate == null || src.DatumLetzteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumLetzteRate);
               }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungsvertragDescriptionDto, ZekContractDto>()
               .ForMember(dest => dest.datumBonitaetIKO, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetIKO == null || src.DatumBonitaetIKO == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetIKO);
               }))
               .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src =>
               {
                   if (src.DatumBonitaetZEK == null || src.DatumBonitaetZEK == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumBonitaetZEK);
               }))
               .ForMember(dest => dest.datumErsteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumErsteRate == null || src.DatumErsteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumErsteRate);
               }))
               .ForMember(dest => dest.datumLetzteRate, opt => opt.MapFrom(src =>
               {
                   if (src.DatumLetzteRate == null || src.DatumLetzteRate == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumLetzteRate);
               }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekUeberziehungskreditDescriptionDto, ZekContractDto>()
                .ForMember(dest => dest.kreditbetrag, opt => opt.MapFrom(src =>
                {
                    return src.SaldoKontoAuszug;
                }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKreditgesuchDescriptionDto, ZekPleaDto>()
               .ForMember(dest => dest.DatumAblehnung, opt => opt.MapFrom(src =>
               {
                   if (src.DatumAblehnung == null || src.DatumAblehnung == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumAblehnung);
               }))
               .ForMember(dest => dest.DatumGueltigBis, opt => opt.MapFrom(src =>
               {
                   if (src.DatumGueltigBis == null || src.DatumGueltigBis == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumGueltigBis);
               }))
               .ForMember(dest => dest.DatumKreditgesuch, opt => opt.MapFrom(src =>
               {
                   if (src.DatumKreditgesuch == null || src.DatumKreditgesuch == "")
                       return new DateTime();
                   return DateTime.Parse(src.DatumKreditgesuch);
               }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekeCode178Dto, ZekECodeDto>()
               .ForMember(dest => dest.Datumgueltigab, opt => opt.MapFrom(src =>
               {
                   if (src.Datumgueltigab == null || src.Datumgueltigab == "")
                       return new DateTime();
                   return DateTime.Parse(src.Datumgueltigab);
               }))
               .ForMember(dest => dest.Datumgueltigbis, opt => opt.MapFrom(src =>
               {
                   if (src.Datumgueltigbis == null || src.Datumgueltigbis == "")
                       return new DateTime();
                   return DateTime.Parse(src.Datumgueltigbis);
               }));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto, ZusatzdatenDto>();
            CreateMap<ZusatzdatenDto, Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto>();

            
            CreateMap<Cic.One.DTO.BN.kalkKontext, Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext>();
            CreateMap<Cic.One.DTO.BN.AngAntObSmallDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObSmallDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto, ocheckAntAngDto>();

            CreateMap<Cic.OpenOne.Common.DTO.AttachmentDto, Cic.One.DTO.AttachmentDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AvailableNewsDto, Cic.One.DTO.AvailableNewsDto>();
            
        }

    }
}
