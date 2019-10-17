using System;
using Cic.OpenOne.Common.Model.DdIc;
using AutoMapper;
using Cic.OpenOne.Common.Util;
using System.Collections.Generic;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using CIC.Database.IC.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// BankNowModelProfile: Mappings für Auskunft
    /// </summary>
    public class BankNowModelProfile : BankNowModelProfileGeneral
    {

        protected class CustomIntDateResolver : IMemberValueResolver<object,object,int?, DateTime?>
        {
            public DateTime? Resolve(object a, object b, int? source,  DateTime? destMember, ResolutionContext context)
            {
                
                if (source == null || source == 0)
                    return null;
                else
                {
                    DateTime result;
                    result = DateTimeHelper.ClarionDateToDateTime((int)source);
                    return result;
                }
            }
        }


        protected class CustomStringDateResolver : IMemberValueResolver<object ,object,String, DateTime?>
        {
            public DateTime? Resolve(object a, object b,String source,DateTime? destMember, ResolutionContext context)
            {
                if (source == null || source == "")
                    return null;
                else
                {
                    DateTime result;
                    result = DateTime.ParseExact(source,"dd'.MM'.yy", null);
                    return result;
                }
            }
        }

        /// <summary>
        /// Konfigurieren
        /// </summary>
        public BankNowModelProfile()
        {
            

            //AUSKUNFT GLOBAL
            #region  Auskunft
            CreateMap<AUSKUNFT, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AuskunftDto>()
                .ForMember(dest => dest.sysAuskunfttyp, opt => opt.MapFrom(src => src.AUSKUNFTTYP.SYSAUSKUNFTTYP));

            // Deltavista
            CreateMap<DAO.Auskunft.DeltavistaRef.CompanyDetailsResponse, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DeltavistaOutDto>()
                .ForMember(dest => dest.NogaCode02Description, opt => opt.MapFrom(src => src.noga02Description))
                .ForMember(dest => dest.NogaCode08Description, opt => opt.MapFrom(src => src.noga08Description))
                .ForMember(dest => dest.ContactList, opt => opt.Ignore())
                .ForMember(dest => dest.DebtList, opt => opt.Ignore())
                .ForMember(dest => dest.CandidateList, opt => opt.Ignore())
                .ForMember(dest => dest.KeyValueList, opt => opt.Ignore())
                .ForMember(dest => dest.ManagementList, opt => opt.Ignore())
                .ForMember(dest => dest.SamePhoneList, opt => opt.Ignore())
                ;
            CreateMap<DAO.Auskunft.DeltavistaRef.AddressDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVAddressDescriptionDto>();
            CreateMap<DAO.Auskunft.DeltavistaRef.AddressCorrection, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVAddressCorrectionDto>();
            CreateMap<DAO.Auskunft.DeltavistaRef.AddressMatch, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVAddressMatchDto>();
            CreateMap<DAO.Auskunft.DeltavistaRef.KeyValueItem, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVKeyValuePairDto>();
            CreateMap<DAO.Auskunft.DeltavistaRef.TransactionError, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVTransactionErrorDto>();
            CreateMap<DAO.Auskunft.DeltavistaRef.ManagementMember, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVManagementMemberDto>();
            CreateMap<DAO.Auskunft.DeltavistaRef.DebtEntry, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVDebtEntryDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVAddressDescriptionDto, DAO.Auskunft.DeltavistaRef.AddressDescription>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVAddressDescriptionDto, DAO.Auskunft.DeltavistaRef2.AddressDescription>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVOrderDescriptionDto, DAO.Auskunft.DeltavistaRef2.OrderDescriptor>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVAddressCorrectionDto, DVADRCORR>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVKeyValuePairDto, DVKVP>()
                .ForMember(dest => dest.DVKEY, opt => opt.MapFrom(src => src.Key));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVManagementMemberDto, DVMGMNTM>()
                .ForMember(dest => dest.HEIMATORT, opt => opt.MapFrom(src => src.Hometown))
                .ForMember(dest => dest.SIGNATURERIGHT, opt => opt.Ignore());
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVContactDescriptionDto, DVCONTACT>();
            CreateMap<DVADRDESC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVAddressDescriptionDto>()
                .ForMember(dest => dest.LegalForm, opt => opt.MapFrom(src => (int)src.LEGALFORM));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DeltavistaOutDto, DVOUTCD>()
                .ForMember(dest => dest.NOGA02DESCRIPTION, opt => opt.Ignore())
                .ForMember(dest => dest.NOGA08DESCRIPTION, opt => opt.Ignore());
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DeltavistaInDto, DVINPBA>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVDebtEntryDto, DVDEBTENTRY>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DVAddressDescriptionDto, DVADRDESC>();


            // ZEK
            // DTO nach DB-Tabelle und DB-Tabelle nach DTO
            CreateMap<ZEKREQEN, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekRequestEntityDto>()
                .ForMember(dest => dest.DebtorRole, opt => opt.MapFrom(src => src.DEBTROLE));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekRequestEntityDto, ZEKREQEN>()
                .ForMember(dest => dest.DEBTROLE, opt => opt.MapFrom(src => src.DebtorRole))
                .ForMember(dest => dest.REFNO, opt => opt.MapFrom(src => (long?)src.RefNo));
            CreateMap<ZEKADRDESC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto>()
                .ForMember(dest => dest.LegalForm, opt => opt.MapFrom(src => src.LEGALFORM))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.SEX));
            CreateMap<ZEKINPEC1, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto>()
                .ForMember(dest => dest.Anfragegrund, opt => opt.MapFrom(src => src.ANFRAGEGRUND))
                .ForMember(dest => dest.Zielverein, opt => opt.MapFrom(src => src.ZIELVEREIN));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto, ZEKADRDESC>()
                .ForMember(dest => dest.LEGALFORM, opt => opt.MapFrom(src => src.LegalForm))
                .ForMember(dest => dest.SEX, opt => opt.MapFrom(src => src.Sex));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto, ZEKINPEC1>()
                .ForMember(dest => dest.ANFRAGEGRUND, opt => opt.MapFrom(src => src.Anfragegrund))
                .ForMember(dest => dest.ZIELVEREIN, opt => opt.MapFrom(src => src.Zielverein));
            CreateMap<ZEKOUTEC1, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekOutDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekResponseDescriptionDto, ZEKRESDESC>()
                .ForMember(dest => dest.RETCODE, opt => opt.MapFrom(src => (long?)src.ReturnCode.Code))
                .ForMember(dest => dest.RETTEXT, opt => opt.MapFrom(src => src.ReturnCode.Text));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAmtsinformationDescriptionDto, ZEKAIC>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto, ZEKBDC>()
                .ForMember(dest => dest.BONITAETSCODE, opt => opt.MapFrom(src => src.bonitaetscodeZEK))
                .ForMember(dest => dest.DATUMBONITAET, opt => opt.MapFrom(src => src.datumBonitaetZEK))
                .ForMember(dest => dest.ANZAHLMONATLICHERRATEN, opt => opt.MapFrom(src => (long)src.anzahlMonatlicheRaten))
                .ForMember(dest => dest.KREDITBETRAG, opt => opt.MapFrom(src => (decimal?)src.kreditbetrag))
                .ForMember(dest => dest.MONATSRATE, opt => opt.MapFrom(src => (decimal?)src.monatsrate));
           

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFestkreditDescriptionDto, ZEKFKC>()
                .ForMember(dest => dest.BONITAETSCODE, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.DATUMBONITAET, opt => opt.MapFrom(src => src.DatumBonitaetZEK))
                .ForMember(dest => dest.KREDITBETRAG, opt => opt.MapFrom(src => (decimal?)src.Kreditbetrag));
          

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekLeasingMietvertragDescriptionDto, ZEKLMC>()
                .ForMember(dest => dest.BONITAETSCODE, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.DATUMBONITAET, opt => opt.MapFrom(src => src.DatumBonitaetZEK))
                .ForMember(dest => dest.ANZAHLMONATLICHERRATEN, opt => opt.MapFrom(src => (long)src.AnzahlMonatlicheRaten))
                .ForMember(dest => dest.KREDITBETRAG, opt => opt.MapFrom(src => (decimal?)src.Kreditbetrag))
                .ForMember(dest => dest.ERSTEGROSSERATE, opt => opt.MapFrom(src => (decimal?)src.ErsteGrosseRate))
                .ForMember(dest => dest.MONATSRATE, opt => opt.MapFrom(src => (decimal?)src.Monatsrate));
           

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungskreditDescriptionDto, ZEKTKC>()
                .ForMember(dest => dest.BONITAETSCODE, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.DATUMBONITAET, opt => opt.MapFrom(src => src.DatumBonitaetZEK))
                .ForMember(dest => dest.ANZAHLMONATLICHERRATEN, opt => opt.MapFrom(src => (long)src.AnzahlMonatlicheRaten))
                .ForMember(dest => dest.KREDITBETRAG, opt => opt.MapFrom(src => (decimal?)src.Kreditbetrag))
                .ForMember(dest => dest.MONATSRATE, opt => opt.MapFrom(src => (decimal?)src.Monatsrate));
          

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungsvertragDescriptionDto, ZEKTKC>()
                .ForMember(dest => dest.BONITAETSCODE, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.DATUMBONITAET, opt => opt.MapFrom(src => src.DatumBonitaetZEK))
                .ForMember(dest => dest.ANZAHLMONATLICHERRATEN, opt => opt.MapFrom(src => (long)src.AnzahlMonatlicheRaten))
                .ForMember(dest => dest.KREDITBETRAG, opt => opt.MapFrom(src => (decimal?)src.Kreditbetrag))
                .ForMember(dest => dest.MONATSRATE, opt => opt.MapFrom(src => (decimal?)src.Monatsrate));
            CreateMap<ZEKTKC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungsvertragDescriptionDto>()
                .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.BONITAETSCODE))
                .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.DATUMBONITAET))
                .ForMember(dest => dest.AnzahlMonatlicheRaten, opt => opt.MapFrom(src => (int)src.ANZAHLMONATLICHERRATEN))
                .ForMember(dest => dest.Kreditbetrag, opt => opt.MapFrom(src => (float)src.KREDITBETRAG))
                .ForMember(dest => dest.Monatsrate, opt => opt.MapFrom(src => (float)src.MONATSRATE));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKontokorrentkreditDescriptionDto, ZEKKKC>()
                .ForMember(dest => dest.BONITAETSCODE, opt => opt.MapFrom(src => src.bonitaetscodeZEK))
                .ForMember(dest => dest.DATUMBONITAET, opt => opt.MapFrom(src => src.datumBonitaetZEK))
                .ForMember(dest => dest.DATUMERSTERATE, opt => opt.MapFrom(src => src.datumVertragsBeginn))
                .ForMember(dest => dest.DATUMLETZTERATE, opt => opt.MapFrom(src => src.datumVertragsEnde));
           

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekSolidarschuldnerDescriptionDto, ZEKSSC>()
                .ForMember(dest => dest.BONITAETSCODE, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.DATUMBONITAET, opt => opt.MapFrom(src => src.DatumBonitaetZEK))
                .ForMember(dest => dest.ANZAHLMONATLICHERRATEN, opt => opt.MapFrom(src => (long)src.AnzahlMonatlicheRaten))
                .ForMember(dest => dest.MONATSRATE, opt => opt.MapFrom(src => (decimal?)src.Monatsrate));
          
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKartenengagementDescriptionDto, ZEKKEC>()
                .ForMember(dest => dest.SALDOABRECHNUNGSTAG, opt => opt.MapFrom(src => (decimal?)src.SaldoAbrechnungsTag));
          
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKarteninformationDescriptionDto, ZEKKIC>();
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKreditgesuchDescriptionDto, ZEKKGC>();
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekUeberziehungskreditDescriptionDto, ZEKUKC>()
                .ForMember(dest => dest.SALDOKONTOAUSZUG, opt => opt.MapFrom(src => (decimal?)src.SaldoKontoAuszug));
            
            CreateMap<ZEKINPEC2, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto>();
            CreateMap<ZEKINPEC3, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto>();
            CreateMap<ZEKINPEC4, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto>();
            CreateMap<ZEKINPEC5, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto>();
            CreateMap<ZEKINPEC6, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto>();
            CreateMap<ZEKINPEC7, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.AddressDescription>()
                .ForMember(dest => dest.UID, opt => opt.MapFrom(src => src.UIDNummer));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekeCode178Dto, ZEKCODE178>();
            CreateMap<ZEKCODE178,Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekeCode178Dto>();
            
           
           
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ArmItem, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekArmItemDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekArmItemDto, ZEKARMRESPONSE>();

            // DTO nach ZEKRef
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.BardarlehenDescription>()
                .ForMember(dest => dest.bonitaetsCodeZEK, opt => opt.MapFrom(src => src.bonitaetscodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFestkreditDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.Festkredit>()
                .ForMember(dest => dest.bonitaetsCodeZEK, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.DatumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekLeasingMietvertragDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.LeasingMietvertragDescription>()
                .ForMember(dest => dest.bonitaetsCodeZEK, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.DatumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungskreditDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription>()
                .ForMember(dest => dest.bonitaetsCodeZEK, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.DatumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungskreditDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription>()
                .ForMember(dest => dest.bonitaetsCodeZEK, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.DatumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKontokorrentkreditDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.KontokorrentkreditDescription>()
                .ForMember(dest => dest.bonitaetsCodeZEK, opt => opt.MapFrom(src => src.bonitaetscodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekSolidarschuldnerDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.SolidarschuldnerDescription>()
                .ForMember(dest => dest.bonitaetsCodeZEK, opt => opt.MapFrom(src => src.BonitaetscodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.DatumBonitaetZEK));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKartenengagementDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.KartenengagementDescription>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekUeberziehungskreditDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.UeberziehungskreditDescription>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.TransactionError, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTransactionErrorDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ReturnCode, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekReturnCodeDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.AddressDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.AmtsinformationDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAmtsinformationDescriptionDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekeCode178Dto, DAO.Auskunft.ZEKRef.eCode178>();


            // ZEKRef nach DTO
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.BardarlehenDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto>()
                .ForMember(dest => dest.bonitaetscodeZEK, opt => opt.MapFrom(src => src.bonitaetsCodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.Festkredit, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFestkreditDescriptionDto>()
                .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.bonitaetsCodeZEK))
                .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.LeasingMietvertragDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekLeasingMietvertragDescriptionDto>()
                .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.bonitaetsCodeZEK))
                .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungskreditDescriptionDto>()
                .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.bonitaetsCodeZEK))
                .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungsvertragDescriptionDto>()
                .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.bonitaetsCodeZEK))
                .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.KontokorrentkreditDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKontokorrentkreditDescriptionDto>()
                .ForMember(dest => dest.bonitaetscodeZEK, opt => opt.MapFrom(src => src.bonitaetsCodeZEK))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.SolidarschuldnerDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekSolidarschuldnerDescriptionDto>()
                .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.bonitaetsCodeZEK))
                .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.datumBonitaetZEK));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.KartenengagementDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKartenengagementDescriptionDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.KarteninformationDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKarteninformationDescriptionDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.KreditGesuchDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKreditgesuchDescriptionDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.UeberziehungskreditDescription, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekUeberziehungskreditDescriptionDto>();

            // Zek-Batch
            // CloseContracts
            CreateMap<ZEKINPEC5, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchContractClosureInstructionDto>()
                .ForMember(dest => dest.bonitaetsCodeZEK, opt => opt.MapFrom(src => src.BONITAETSCODE))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.DATUMBONITAET));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchContractClosureInstructionDto, ZEKINPEC5>()
                .ForMember(dest => dest.BONITAETSCODE, opt => opt.MapFrom(src => src.bonitaetsCodeZEK))
                .ForMember(dest => dest.DATUMBONITAET, opt => opt.MapFrom(src => src.datumBonitaetZEK));
            
            // UpdateContracts
            CreateMap<ZEKINPEC7, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchContractUpdateInstructionDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchContractUpdateInstructionDto, ZEKINPEC7>();

            

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKBatchRef.BardarlehenDescription>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFestkreditDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKBatchRef.Festkredit>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKontokorrentkreditDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKBatchRef.KontokorrentkreditDescription>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekLeasingMietvertragDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKBatchRef.LeasingMietvertragDescription>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungskreditDescriptionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKBatchRef.TeilzahlungskreditDescription>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchContractClosureInstructionDto, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKBatchRef.ContractClosureInstruction>();
            
            CreateMap<DAO.Auskunft.ZEKBatchRef.ReturnCode, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekReturnCodeDto>();
            CreateMap<DAO.Auskunft.ZEKBatchRef.TransactionError, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTransactionErrorDto>();

            CreateMap<DAO.Auskunft.ZEKBatchRef.BatchStatusResponse, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchStatusResponseDto>();
            CreateMap<DAO.Auskunft.ZEKBatchRef.BatchInstructionError, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchInstructionErrorDto>();
            CreateMap<DAO.Auskunft.ZEKBatchRef.BatchInstructionResponse, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchInstructionResponseDto>();

            CreateMap<DAO.Auskunft.ZEKRef.eCode178,Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekeCode178Dto>();

            // DecisionEngine
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DecisionEngineInDto, CIC.Database.DE.EF6.Model.DEENVINP>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DecisionEngineInDto, CIC.Database.DE.EF6.Model.DECONTRACT>()
                .ForMember(dest => dest.KKGPFLICHT, opt => opt.MapFrom(src => (int?)src.KKG_Pflicht))
                .ForMember(dest => dest.PPIFLAGPAKET1, opt => opt.MapFrom(src => (int?)src.PPI_Flag_Paket1))
                .ForMember(dest => dest.PPIFLAGPAKET2, opt => opt.MapFrom(src => (int?)src.PPI_Flag_Paket2))
                .ForMember(dest => dest.PPIBETRAG, opt => opt.MapFrom(src => src.PPI_Betrag))
                .ForMember(dest => dest.ANZAHLUNERSTERATE, opt => opt.MapFrom(src => src.Anzahlung_ErsteRate))
                .ForMember(dest => dest.BUDGETUEBERSCHUSS1, opt => opt.MapFrom(src => src.Budgetueberschuss_1))
                .ForMember(dest => dest.BUDGETUEBERSCHUSS2, opt => opt.MapFrom(src => src.Budgetueberschuss_2))
                .ForMember(dest => dest.BUDGETUEBERSCHUSSGESAMT, opt => opt.MapFrom(src => src.Budgetueberschuss_gesamt))
                .ForMember(dest => dest.LAUFZEIT, opt => opt.MapFrom(src => (int?)src.Laufzeit))
                .ForMember(dest => dest.RISKFLAG, opt => opt.MapFrom(src => (int?)src.Riskflag))
                .ForMember(dest => dest.VERTRAGSART, opt => opt.MapFrom(src => (long?)src.Vertragsart))
                .ForMember(dest => dest.RANDOMNUMBER, opt => opt.MapFrom(src => (int?)src.RandomNumber))
                .ForMember(dest => dest.RWVERLFLAG, opt => opt.MapFrom(src => (int?)src.RW_Verl))
                .ForMember(dest => dest.FLAGNEULV, opt => opt.MapFrom(src => (int?)src.Vertragsversion_NEU))
                .ForMember(dest => dest.ERNEUTEPRUEFUNG, opt => opt.MapFrom(src => (int?)src.Erneute_Pruefung))
                .ForMember(dest => dest.PPIBEW, opt => opt.MapFrom(src => src.PPI_Betrag_Bewilligt))
                .ForMember(dest => dest.FINANZIERUNGSBETRAGBEW, opt => opt.MapFrom(src => src.Finanzierungsbetragbew))
                .ForMember(dest => dest.TOLERANZRISKDEC, opt => opt.MapFrom(src => src.Toleranzriskdec))
                .ForMember(dest => dest.SYSRWGA, opt => opt.MapFrom(src => src.Restwertgarant))
                ;

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DecisionEngineInDto, CIC.Database.DE.EF6.Model.DEOBJECT>()
                .ForMember(dest => dest.INVERKEHRSSETZUNG, opt => opt.MapFrom(src => (DateTime?)src.Inverkehrssetzung))
                .ForMember(dest => dest.KMPROJAHR, opt => opt.MapFrom(src => (long?)src.KM_prJahr))
                .ForMember(dest => dest.KMSTAND, opt => opt.MapFrom(src => (long?)src.KM_Stand))
                .ForMember(dest => dest.ZUSTAND, opt => opt.MapFrom(src => (int?)src.Zustand))
                .ForMember(dest => dest.FAHRZEUGPREISEUROTAX, opt => opt.MapFrom(src => src.Fahrzeugpreis_Eurotax))
                .ForMember(dest => dest.KATALOGPREISEUROTAX, opt => opt.MapFrom(src => src.Katalogpreis_Eurotax))
                .ForMember(dest => dest.RESTWERTBANK, opt => opt.MapFrom(src => src.Restwert_Banknow))
                .ForMember(dest => dest.RESTWERTEUROTAX, opt => opt.MapFrom(src => src.Restwert_Eurotax))
                .ForMember(dest => dest.EWBBETRAG, opt => opt.MapFrom(src => src.Expected_Loss))
                .ForMember(dest => dest.EWBLGD, opt => opt.MapFrom(src => src.Expected_Loss_LGD))
                .ForMember(dest => dest.EWBPROC, opt => opt.MapFrom(src => src.Expected_Loss_Prozent))
                .ForMember(dest => dest.EWBPROF, opt => opt.MapFrom(src => src.Profitabilitaet_Prozent))
                .ForMember(dest => dest.FIDENT, opt => opt.MapFrom(src => src.VIN_Nummer))
                .ForMember(dest => dest.CLUSTEROBJEKT, opt => opt.MapFrom(src => src.Marktwert_Cluster))
                .ForMember(dest => dest.RESTWERTABSICHERUNG, opt => opt.MapFrom(src => src.Restwertabsicherung))
                .ForMember(dest => dest.AUSSTATTUNG, opt => opt.MapFrom(src => src.Ausstattung))
                ;

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.RecordRRDto, CIC.Database.DE.EF6.Model.DEOPENLEASE>()
                .ForMember(dest => dest.ANZANNULIERUNGEN12M, opt => opt.MapFrom(src => (int?)src.OL_Anz_Annulierungen_l12M))
                .ForMember(dest => dest.ANZANTRAEGE, opt => opt.MapFrom(src => (int?)src.OL_Anz_Antraege))
                .ForMember(dest => dest.ANZKUNDENIDS, opt => opt.MapFrom(src => (int?)src.OL_Anz_KundenIDs))
                .ForMember(dest => dest.ANZLFDVERTRAEGE, opt => opt.MapFrom(src => (int?)src.OL_Anz_lfd_Vertraege))
                .ForMember(dest => dest.ANZMAHNUNGEN1, opt => opt.MapFrom(src => (int?)src.OL_Anz_Mahnungen_1))
                .ForMember(dest => dest.ANZMAHNUNGEN2, opt => opt.MapFrom(src => (int?)src.OL_Anz_Mahnungen_2))
                .ForMember(dest => dest.ANZMAHNUNGEN3, opt => opt.MapFrom(src => (int?)src.OL_Anz_Mahnungen_3))
                .ForMember(dest => dest.ANZMEHRFACHANTRAEGE, opt => opt.MapFrom(src => (int?)src.OL_Anz_Mehrfachantraege))
                .ForMember(dest => dest.ANZOP, opt => opt.MapFrom(src => (int?)src.OL_Anz_OP))
                .ForMember(dest => dest.ANZSTUNDUNGEN, opt => opt.MapFrom(src => (int?)src.OL_Anz_Stundungen))
                .ForMember(dest => dest.ANZVERTRAEGE, opt => opt.MapFrom(src => (int?)src.OL_Anz_Vertraege))
                .ForMember(dest => dest.ANZVERTRAEGEIMRECOVERY, opt => opt.MapFrom(src => (int?)src.OL_Anz_Vertraege_im_Recovery))
                .ForMember(dest => dest.ANZVERZICHTE12M, opt => opt.MapFrom(src => (int?)src.OL_Anz_Verzichte_l12M))
                .ForMember(dest => dest.ANZZAHLUNGSVEREINBARUNGEN, opt => opt.MapFrom(src => (int?)src.OL_Anz_Zahlungsvereinbarungen))
                .ForMember(dest => dest.DAUERKUNDENBEZIEHUNG, opt => opt.MapFrom(src => (int?)src.OL_Dauer_Kundenbeziehung))
                .ForMember(dest => dest.EFFEKTIVEKUNDENBEZIEHUNG, opt => opt.MapFrom(src => (int?)src.OL_Effektive_Kundenbeziehung))
                .ForMember(dest => dest.MAXAKTRKLASSEKUNDE, opt => opt.MapFrom(src => (int?)src.OL_Maximale_akt_RKlasse_des_Kunden))
                .ForMember(dest => dest.MAXBADLISTEINTRAG, opt => opt.MapFrom(src => (int?)src.OL_Maximaler_Bandlisteintrag))
                .ForMember(dest => dest.MAXIMALEMAHNSTUFE, opt => opt.MapFrom(src => (int?)src.OL_Maximale_Mahnstufe))
                .ForMember(dest => dest.MAXRISIKOKLASSEKUNDE, opt => opt.MapFrom(src => (int?)src.OL_Maximale_Risikoklasse_des_Kunden))
                .ForMember(dest => dest.STATUS, opt => opt.MapFrom(src => (int?)src.OL_Status))
                .ForMember(dest => dest.LETZTESARBEITSVERHAELTNIS, opt => opt.MapFrom(src => src.OL_Letztes_Arbeitsverhaeltnis))
                .ForMember(dest => dest.LETZTESWOHNVERHAELTNIS, opt => opt.MapFrom(src => src.OL_Letztes_Wohnverhaeltnis))
                .ForMember(dest => dest.LETZTERZIVILSTAND, opt => opt.MapFrom(src => src.OL_Letzter_Zivilstand))
                .ForMember(dest => dest.ENGAGEMENT, opt => opt.MapFrom(src => src.OL_Engagement))
                .ForMember(dest => dest.EVENTUALENGAGEMENT, opt => opt.MapFrom(src => src.OL_Eventualengagement))
                .ForMember(dest => dest.GESAMTENGAGEMENT, opt => opt.MapFrom(src => src.OL_Gesamtengagement))
                .ForMember(dest => dest.HAUSHALTENGAGEMENT, opt => opt.MapFrom(src => src.OL_Haushaltsengagement))
                .ForMember(dest => dest.LETZTEMIETE, opt => opt.MapFrom(src => src.OL_Letzte_Miete))
                .ForMember(dest => dest.LETZTENATIONALITAET, opt => opt.MapFrom(src => src.OL_Letzte_Nationalitaet))
                .ForMember(dest => dest.LETZTERBONUS, opt => opt.MapFrom(src => src.OL_Letzter_Bonus))
                .ForMember(dest => dest.LETZTERZIVILSTAND, opt => opt.MapFrom(src => src.OL_Letzter_Zivilstand))
                .ForMember(dest => dest.LETZTESHAUPTEINKOMMEN, opt => opt.MapFrom(src => src.OL_Letztes_Haupteinkommen))
                .ForMember(dest => dest.LETZTESNEBENEINKOMMEN, opt => opt.MapFrom(src => src.OL_Letztes_Nebeneinkommen))
                .ForMember(dest => dest.LETZTESZUSATZEINKOMMEN, opt => opt.MapFrom(src => src.OL_Letztes_Zusatzeinkommen))
                .ForMember(dest => dest.MINDATUMKUNDESEIT, opt => opt.MapFrom(src => src.OL_Minimales_Datum_Kunde))
                .ForMember(dest => dest.OPENLEASEDATUMAUSKUNFT, opt => opt.MapFrom(src => src.OL_OpenLease_Datum_der_Anmeldung))
                .ForMember(dest => dest.SUMMEOP, opt => opt.MapFrom(src => src.OL_Summe_OP))
                .ForMember(dest => dest.ANZMANABL6M, opt => opt.MapFrom(src => (int?)src.OL_Anz_manAblehnungen_l6M))
                .ForMember(dest => dest.ANZMANABL12M, opt => opt.MapFrom(src => (int?)src.OL_Anz_manAblehnungen_l12M))
                .ForMember(dest => dest.ANZVTSPEZ, opt => opt.MapFrom(src => (int?)src.OL_Anz_Vertraege_mit_Spezialfall))
                .ForMember(dest => dest.ANZVTSPEZLFD, opt => opt.MapFrom(src => (int?)src.OL_Anz_lfd_Vertraege_mit_Spezialfall))
                .ForMember(dest => dest.DATEMAHN1, opt => opt.MapFrom(src => (DateTime?)src.OL_Datum_Mahnung_1))
                .ForMember(dest => dest.DATEMAHN2, opt => opt.MapFrom(src => (DateTime?)src.OL_Datum_Mahnung_2))
                .ForMember(dest => dest.DATEMAHN3, opt => opt.MapFrom(src => (DateTime?)src.OL_Datum_Mahnung_3))
                .ForMember(dest => dest.DATESTUNDUNGEN, opt => opt.MapFrom(src => (DateTime?)src.OL_Datum_letzte_Stundung))
                .ForMember(dest => dest.DATEZVB, opt => opt.MapFrom(src => (DateTime?)src.OL_Datum_letzte_ZVB))
                .ForMember(dest => dest.DATEAUFSTOCKSTOP, opt => opt.MapFrom(src => (DateTime?)src.OL_Datum_Aufstockungssperre))
                .ForMember(dest => dest.ANZAUFSTOCKSTOP, opt => opt.MapFrom(src => (int?)src.OL_Anzahl_Aufstockungssperren))
                .ForMember(dest => dest.LETZTEEINKOMMENART, opt => opt.MapFrom(src => src.OL_Letzte_Einkommensart ))
                .ForMember(dest => dest.RATENPAUSEN, opt => opt.MapFrom(src => (int?)src.OL_Ratenpausen))
                .ForMember(dest => dest.GBEZIEHUNG, opt => opt.MapFrom(src => (int?)src.OL_Gbezeichnung))
                //*BNR13 TODO IN EDMX und DB
                //.ForMember(dest => dest.DATUMERSTERANTRAG, opt => opt.MapFrom(src => (int?)src.OL_Datum_erster_Antrag))
                //.ForMember(dest => dest.DATUMLETZTERANTRAG, opt => opt.MapFrom(src => (int?)src.OL_Datum_letzter_Antrag))
                //.ForMember(dest => dest.ANZMAHN1AVG6M, opt => opt.MapFrom(src => (int?)src.OL_Anzahl_Mahnstufe1_L6M))
                //.ForMember(dest => dest.ANZMAHN2AVG6M, opt => opt.MapFrom(src => (int?)src.OL_Anzahl_Mahnstufe2_L6M))
                //.ForMember(dest => dest.ANZMAHN3AVG6M, opt => opt.MapFrom(src => (int?)src.OL_Anzahl_Mahnstufe3_L6M))
                //.ForMember(dest => dest.ANZZAHLAVG12M, opt => opt.MapFrom(src => (int?)src.OL_Anzahl_Einzahlungen_L12M))
                //.ForMember(dest => dest.RUECKSTANDAVG, opt => opt.MapFrom(src => (int?)src.OL_Zahlungsrueckstand))
                //.ForMember(dest => dest.BUCHSALDOAVG, opt => opt.MapFrom(src => (int?)src.OL_Saldoreduktion_L12M))
                ;

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.RecordRRDto,  DEBUDGET>()

                .ForMember(dest => dest.ANFRAGEDATUM, opt => opt.MapFrom(src => (DateTime?)src.BU_AnfrageDatum))
                .ForMember(dest => dest.GRUNDBETRAG, opt => opt.MapFrom(src => (decimal?)src.BU_Grundbetrag))
                .ForMember(dest => dest.KRANKENKASSE, opt => opt.MapFrom(src => (decimal?)src.BU_Krankenkasse))
                .ForMember(dest => dest.QUELLSTEUER, opt => opt.MapFrom(src => (decimal?)src.BU_Quellsteuer))
                .ForMember(dest => dest.KREMOCODE, opt => opt.MapFrom(src => (int?)src.BU_Kremocode))
                .ForMember(dest => dest.BUDGETUEBERSCHUSS, opt => opt.MapFrom(src => (decimal?)src.BU_Budgetueberschuss))
                .ForMember(dest => dest.BUDGETUEBERSCHUSSGESAMT, opt => opt.MapFrom(src => (decimal?)src.BU_Budgetueberschuss_gesamt))
                .ForMember(dest => dest.EINKNETTOBERECH, opt => opt.MapFrom(src => (decimal?)src.BU_Einknettoberech))
                .ForMember(dest => dest.EINKNETTOBERECH2, opt => opt.MapFrom(src => (decimal?)src.BU_Einknettoberech2))
                .ForMember(dest => dest.EXTBETRKOSTENTAT, opt => opt.MapFrom(src => (decimal?)src.BU_Betreuungskosten_Extern))
                .ForMember(dest => dest.ARBEITSWEGAUSLAGE, opt => opt.MapFrom(src => (decimal?)src.BU_Arbeitswegpauschale))
                .ForMember(dest => dest.KRANKENKASSETAT, opt => opt.MapFrom(src => (decimal?)src.BU_Krankenkassenpraemie))
                 ;

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.RecordRRDto, CIC.Database.DE.EF6.Model.DEZEK>()
                .ForMember(dest => dest.ANZZEKGESUCHE, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Gesuche))
                .ForMember(dest => dest.ANZZEKVERTRAEGE, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Vertraege))
                .ForMember(dest => dest.ANZLFDZEKENG, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng))
                .ForMember(dest => dest.ANZLFDZEKENGBARDARLEHEN, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_Bardarlehen))
                .ForMember(dest => dest.ANZLFDZEKENGFESTKREDIT, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_Festkredit))
                .ForMember(dest => dest.ANZLFDZEKENGKARTEN, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_Kartenengagement))
                .ForMember(dest => dest.ANZLFDZEKENGKONTOKORRENT, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_Kontokorrent))
                .ForMember(dest => dest.ANZLFDZEKENGLEASING, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_Leasing))
                .ForMember(dest => dest.ANZLFDZEKENGTEILZVERTRAG, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_Teilz))

                .ForMember(dest => dest.ANZLFDZEKFREMDGES, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_FremdGes))
                .ForMember(dest => dest.ANZLFDZEKEIGENGES, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_EigenGes))

                .ForMember(dest => dest.ANZZEKAMTSMELDEN0105, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_AmtsMelden_01_05))
                .ForMember(dest => dest.ANZZEKAMTSMELDEN010512M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_AmtsMelden_01_05_l12M))

                .ForMember(dest => dest.ANZZEKENGMBCODE03, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_03))
                .ForMember(dest => dest.ANZZEKENGMBCODE0312M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_03l12M))
                .ForMember(dest => dest.ANZZEKENGMBCODE0324M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_03l24M))
                .ForMember(dest => dest.ANZZEKENGMBCODE0336M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_03l36M))
                .ForMember(dest => dest.ANZZEKENGMBCODE04, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_04))
                .ForMember(dest => dest.ANZZEKENGMBCODE0412M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_04l12M))
                .ForMember(dest => dest.ANZZEKENGMBCODE0424M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_04l24M))
                .ForMember(dest => dest.ANZZEKENGMBCODE0436M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_04l36M))
                .ForMember(dest => dest.ANZZEKENGMBCODE0506, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_lfd_ZEK_Eng_BCode_0506))

                .ForMember(dest => dest.ANZZEKGESMABLCODE, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode))
                .ForMember(dest => dest.ANZZEKGESMABLCODE0412M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE0424M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE0712M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE0724M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE0912M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE0924M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE1012M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE1024M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE1312M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE1324M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE1412M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE1424M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE9912M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE9924M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M))
                .ForMember(dest => dest.ANZZEKGESMABLCODE05060812, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_05060812))
                .ForMember(dest => dest.ANZZEKGESMABLCODE040709, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Ges_m_AblCode_040709))

                .ForMember(dest => dest.ANZZEKKMELDMERCODE2112M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2124M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2136M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2148M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2160M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2212M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2224M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2236M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2248M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE2260M, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M))
                .ForMember(dest => dest.ANZZEKKMELDMERCODE23242526, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26))

                .ForMember(dest => dest.ANZZEKSYNONYME, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_ZEK_Synonyme))
                .ForMember(dest => dest.SCHLECHTESTERZEKABLCODE, opt => opt.MapFrom(src => (int?)src.ZEK_schlechtester_ZEK_AblCode))
                .ForMember(dest => dest.SCHLECHTESTERZEKBCODE, opt => opt.MapFrom(src => (int?)src.ZEK_schlechtester_ZEK_Code))
                .ForMember(dest => dest.STATUS, opt => opt.MapFrom(src => (int?)src.ZEK_Status))
                .ForMember(dest => dest.DATUMAUSKUNFT, opt => opt.MapFrom(src => src.ZEK_Datum_der_Auskunft))
                .ForMember(dest => dest.KUNDEGESAMTENGAGEMENT, opt => opt.MapFrom(src => src.ZEK_Kunde_Gesamtengagement))

                .ForMember(dest => dest.ANZBC04LFD, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Eng_m_BCode_04_laufend))
                .ForMember(dest => dest.ANZBC04SAL, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Eng_m_BCode_04_saldiert))
                .ForMember(dest => dest.ANZBC03LFD, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Eng_m_BCode_03_laufend))
                .ForMember(dest => dest.ANZBC03SAL, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Eng_m_BCode_03_saldiert))
                .ForMember(dest => dest.ANZZEKGESMABLCODE04070999E, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Ges_m_AblCode_04_07_09_99_BN))
                .ForMember(dest => dest.ANZZEKGESMABLCODE04070999F, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Ges_m_AblCode_04_07_09_99_noBN))
                .ForMember(dest => dest.ANZZEKGESMABLCODE1314E, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Ges_m_AblCode_13_14_BN))
                .ForMember(dest => dest.ANZZEKGESMABLCODE1314F, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Ges_m_AblCode_13_14_noBN))
                .ForMember(dest => dest.ANZZEKGESMABLCODE10E, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Ges_m_AblCode_10_BN))
                .ForMember(dest => dest.ANZZEKGESMABLCODE10F, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_Ges_m_AblCode_10_noBN))
                .ForMember(dest => dest.ANZZEKKMELDMCODE21POS, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_KMeld_m_ErCode_21_mit_Positiv))
                .ForMember(dest => dest.ANZZEKKMELDMCODE21NEG, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_KMeld_m_ErCode_21_ohne_Positiv))
                .ForMember(dest => dest.ANZZEKKMELDMCODE22POS, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_KMeld_m_ErCode_22_mit_Positiv))
                .ForMember(dest => dest.ANZZEKKMELDMCODE22NEG, opt => opt.MapFrom(src => (int?)src.ZEK_Anz_KMeld_m_ErCode_22_ohne_Positiv));
                


            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.RecordRRDto, CIC.Database.DE.EF6.Model.DEARBEIT>()
                .ForMember(dest => dest.DECISIONMAKER, opt => opt.MapFrom(src => (int?)src.DV_AG_Decision_Maker))
                .ForMember(dest => dest.FIRMENSTATUS, opt => opt.MapFrom(src => (int?)src.DV_AG_Firmenstatus))
                .ForMember(dest => dest.RECHTSFORM, opt => opt.MapFrom(src => (int?)src.DV_AG_Rechtsform))
                .ForMember(dest => dest.STATUS, opt => opt.MapFrom(src => (int?)src.DV_AG_Status))
                .ForMember(dest => dest.DATUM, opt => opt.MapFrom(src => src.DV_AG_Datum))
                .ForMember(dest => dest.GRUENDUNGSDATUM, opt => opt.MapFrom(src => src.DV_AG_Gruendungsdatum))
                .ForMember(dest => dest.NOGACODE, opt => opt.MapFrom(src => src.DV_AG_NOGA_Code))
                .ForMember(dest => dest.ZEIT, opt => opt.MapFrom(src => src.DV_AG_Zeit))
                .ForMember(dest => dest.UIDNUMMER, opt => opt.MapFrom(src => src.DV_AG_UID))
                .ForMember(dest => dest.KAPITAL, opt => opt.MapFrom(src => src.DV_AG_Kapital))
                .ForMember(dest => dest.PLZAG1, opt => opt.MapFrom(src => src.A_AG_PLZ1))
                .ForMember(dest => dest.PLZAG2, opt => opt.MapFrom(src => src.A_AG_PLZ2))
                ;

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.RecordRRDto, CIC.Database.DE.EF6.Model.DEADRBONI>()
                .ForMember(dest => dest.KUNDENID, opt => opt.MapFrom(src => Convert.ToInt64(src.DV_ADDRESS_ID)))
                .ForMember(dest => dest.ANZBM, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM))
                .ForMember(dest => dest.ANZBPM12M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_l12m))
                .ForMember(dest => dest.ANZBPM24M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_l24m))
                .ForMember(dest => dest.ANZBMP36M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_l36m))
                .ForMember(dest => dest.ANZBMP48M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_l48m))
                .ForMember(dest => dest.ANZBMP60M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_l60m))

                .ForMember(dest => dest.ANZBPMMFSTAT01, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_01))
                .ForMember(dest => dest.ANZBPMMFSTAT02, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_02))
                .ForMember(dest => dest.ANZBPMMFSTAT03, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_03))
                .ForMember(dest => dest.ANZBPMMFSTAT04, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_04))

                .ForMember(dest => dest.ANZBPMMFSTAT0112M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_01_l12m))
                .ForMember(dest => dest.ANZBPMMFSTAT0124M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_01_l24m))
                .ForMember(dest => dest.ANZBPMMFSTAT0136M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_01_l36m))
                .ForMember(dest => dest.ANZBPMMFSTAT0148M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_01_l48m))
                .ForMember(dest => dest.ANZBPMMFSTAT0160M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_01_l60m))

                .ForMember(dest => dest.ANZBPMMFSTAT0212M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_02_l12m))
                .ForMember(dest => dest.ANZBPMMFSTAT0224M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_02_l24m))
                .ForMember(dest => dest.ANZBPMMFSTAT0236M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_02_l36m))
                .ForMember(dest => dest.ANZBPMMFSTAT0248M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_02_l48m))
                .ForMember(dest => dest.ANZBPMMFSTAT0260M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_02_l60m))

                .ForMember(dest => dest.ANZBPMMFSTAT0312M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_03_l12m))
                .ForMember(dest => dest.ANZBPMMFSTAT0324M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_03_l24m))
                .ForMember(dest => dest.ANZBPMMFSTAT0336M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_03_l36m))
                .ForMember(dest => dest.ANZBPMMFSTAT0348M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_03_l48m))
                .ForMember(dest => dest.ANZBPMMFSTAT0360M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_03_l60m))

                .ForMember(dest => dest.ANZBPMMFSTAT0412M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_04_l12m))
                .ForMember(dest => dest.ANZBPMMFSTAT0424M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_04_l24m))
                .ForMember(dest => dest.ANZBPMMFSTAT0436M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_04_l36m))
                .ForMember(dest => dest.ANZBPMMFSTAT0448M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_04_l48m))
                .ForMember(dest => dest.ANZBPMMFSTAT0460M, opt => opt.MapFrom(src => (int?)src.DV_Anz_BPM_m_FStat_04_l60m))

                .ForMember(dest => dest.FIRMENSTATUS, opt => opt.MapFrom(src => (int?)src.DV_Firmenstatus))
                .ForMember(dest => dest.RECHTSFORM, opt => opt.MapFrom(src => (int?)src.DV_Rechtform))

                .ForMember(dest => dest.SCHLECHTESTERFSTAT, opt => opt.MapFrom(src => (int?)src.DV_Schlechtester_FStat))
                .ForMember(dest => dest.SCHLECHTESTERFSTAT12M, opt => opt.MapFrom(src => (int?)src.DV_Schlechtester_FStat_l12m))
                .ForMember(dest => dest.SCHLECHTESTERFSTAT24M, opt => opt.MapFrom(src => (int?)src.DV_Schlechtester_FStat_l24m))
                .ForMember(dest => dest.SCHLECHTESTERFSTAT36M, opt => opt.MapFrom(src => (int?)src.DV_Schlechtester_FStat_l36m))
                .ForMember(dest => dest.SCHLECHTESTERFSTAT48M, opt => opt.MapFrom(src => (int?)src.DV_Schlechtester_FStat_l48m))
                .ForMember(dest => dest.SCHLECHTESTERFSTAT60M, opt => opt.MapFrom(src => (int?)src.DV_Schlechtester_FStat_l60m))

                .ForMember(dest => dest.STATUSAUSKUNFTADRESSVALID, opt => opt.MapFrom(src => (int?)src.DV_Status_Auskunft_Adressvalidierung))
                .ForMember(dest => dest.ANZDVTREFFERADRESSVALIDIERUNG, opt => opt.MapFrom(src => (int?)src.DV_Anz_DV_Treffer_Adressvalidierung))
                .ForMember(dest => dest.DATUMAKTUELLEADRESSESEIT, opt => opt.MapFrom(src => src.DV_Datum_an_der_aktuellen_Adresse_seit))
                .ForMember(dest => dest.DATUMDERAUSKUNFT, opt => opt.MapFrom(src => src.DV_Datum_der_Auskunft))
                .ForMember(dest => dest.DATUMERSTEMELDUNG, opt => opt.MapFrom(src => src.DV_Datum_der_ersten_Meld))
                .ForMember(dest => dest.FRAUDFELD, opt => opt.MapFrom(src => src.DV_Fraud_Feld))
                .ForMember(dest => dest.GEBURTSDATUM, opt => opt.MapFrom(src => src.DV_Geburtsdatum))
                .ForMember(dest => dest.GRUENDUNGSDATUM, opt => opt.MapFrom(src => src.DV_Gruendungsdatum))
                .ForMember(dest => dest.KAPITAL, opt => opt.MapFrom(src => src.DV_Kapital))
                .ForMember(dest => dest.LAND, opt => opt.MapFrom(src => src.DV_Land))
                .ForMember(dest => dest.NOGACODEBRANCHE, opt => opt.MapFrom(src => src.DV_NOGA_Code_Branche))
                .ForMember(dest => dest.PLZ, opt => opt.MapFrom(src => src.DV_PLZ))
                .ForMember(dest => dest.ZEITDERAUSKUNFT, opt => opt.MapFrom(src => src.DV_Zeit_der_Auskunft))

                .ForMember(dest => dest.UIDNUMMER, opt => opt.MapFrom(src => src.DV_UID))
                .ForMember(dest => dest.ANZDECISIONMAKER, opt => opt.MapFrom(src => src.DV_Anz_DecisionMaker))
                .ForMember(dest => dest.DATUMJUENGSTEREINTRAG, opt => opt.MapFrom(src => src.DV_Datum_juengster_Eintrag))
                .ForMember(dest => dest.KRITISCHERGLAEUBIGER, opt => opt.MapFrom(src => src.DV_Kritischer_Glaeubiger))
                .ForMember(dest => dest.SUMOFFENERBETREIBUNGEN, opt => opt.MapFrom(src => src.DV_Summe_offener_Betreibungen))
                .ForMember(dest => dest.ANZOFFENERBETREIBUNGEN, opt => opt.MapFrom(src => src.DV_Anzahl_offene_Betreibungen))
                .ForMember(dest => dest.DATUMJUENGSTEBETREIBUNG, opt => opt.MapFrom(src => src.DV_Datum_juengste_Betreibung))
                .ForMember(dest => dest.DECVALUEORG, opt => opt.MapFrom(src => src.DV_Organisation_belastet))
                .ForMember(dest => dest.DECVALUECCB, opt => opt.MapFrom(src => src.DV_Score))
                .ForMember(dest => dest.PAYMENTDELAYRATIO, opt => opt.MapFrom(src => src.DV_Payment_Delay))
                .ForMember(dest => dest.FIRSTSHABDATE, opt => opt.MapFrom(src => src.DV_First_SHAB_Date))
                .ForMember(dest => dest.DEVVALUERISK, opt => opt.MapFrom(src => src.DV_Risk_Alert))
                .ForMember(dest => dest.UIDSAMEASNAME, opt => opt.MapFrom(src => src.DV_Uid_SameAsName))
                .ForMember(dest => dest.ANZBPMMFSTAT00, opt => opt.MapFrom(src => (int?)src.DV_Anz_BM_m_FStat_00))
                
                 .ForMember(dest => dest.NETZWERKBEZIEHUNG, opt => opt.MapFrom(src => src.Netzwerkbeziehung))
                 .ForMember(dest => dest.FLUKTUATIONSRATE, opt => opt.MapFrom(src => src.Fluktuationsrate))
                 .ForMember(dest => dest.FRAUDMNGT, opt => opt.MapFrom(src => src.Fraudmngt))
                ;

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.RecordRRResponseDto, CIC.Database.DE.EF6.Model.DEDETAIL>()
                 .ForMember(dest => dest.PD, opt => opt.MapFrom(src => (int?)src.DEC_Score_PD));

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DecisionEngineOutDto, CIC.Database.DE.EF6.Model.DEDETAIL>()
                 .ForMember(dest => dest.CLUSTERVALUE, opt => opt.MapFrom(src => src.DEC_TR_Segment));
         


            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.RecordRRDto, CIC.Database.DE.EF6.Model.DEAPPLICANT>()
                .ForMember(dest => dest.ANZAHLBETREIBUNGEN, opt => opt.MapFrom(src => (int?)src.A_Anz_der_Betreibungen))
                .ForMember(dest => dest.ANZKIND11BIS12, opt => opt.MapFrom(src => (int?)src.A_Anz_Kinder_ueber_10_bis_12))
                .ForMember(dest => dest.ANZKINDBIS6, opt => opt.MapFrom(src => (int?)src.A_Anz_Kinder_bis_6))
                .ForMember(dest => dest.ANZKINDER7BIS10, opt => opt.MapFrom(src => (int?)src.A_Anz_Kinder_ueber_6_bis_10))
                .ForMember(dest => dest.ANZKINDERAB13, opt => opt.MapFrom(src => (int?)src.A_Anz_unterstuetzungsp_Kinder_ab_12))
                .ForMember(dest => dest.ANZMITARBEITER, opt => opt.MapFrom(src => (int?)src.A_Anz_Mitarbeiter))
                .ForMember(dest => dest.FLAG13MONATSLOHN, opt => opt.MapFrom(src => (int?)src.A_13_Montaslohn))
                .ForMember(dest => dest.FLAGMITARBEITERCS, opt => opt.MapFrom(src => (int?)src.A_marbeiter_Credit_Suisse_Group))
                .ForMember(dest => dest.INHANDELSREGISTEREINGETRAGEN, opt => opt.MapFrom(src => (int?)src.A_In_Handelsregister_eingetragen))
                .ForMember(dest => dest.KUNDENID, opt => opt.MapFrom(src => (long?)src.A_KundenID))
                .ForMember(dest => dest.QUELLENSTEUERPFLICHTIG, opt => opt.MapFrom(src => (int?)src.A_Quellensteuerpflichtig))
                .ForMember(dest => dest.REVISIONSSTELLEVORHANDEN, opt => opt.MapFrom(src => (int?)src.A_Revisionsstelle_vorhanden))
                .ForMember(dest => dest.VERLUSTSCHEINEPFAENDUNGEN, opt => opt.MapFrom(src => (int?)src.A_Verlustscheine_Pfaendungen))
                .ForMember(dest => dest.SPRACHE, opt => opt.MapFrom(src => src.A_Sprache))
                .ForMember(dest => dest.STATUS, opt => opt.MapFrom(src => src.A_Status))
                .ForMember(dest => dest.UNTERSTUETZUNGSBEITRAEGE, opt => opt.MapFrom(src => src.A_Unterstuetzungsbeitraege))
                .ForMember(dest => dest.WOHNVERHAELTNIS, opt => opt.MapFrom(src => src.A_Wohnverhaeltnis))
                .ForMember(dest => dest.ZIVILSTAND, opt => opt.MapFrom(src => src.A_Zivilstand))
                .ForMember(dest => dest.ZUSATZEINKOMMEN, opt => opt.MapFrom(src => src.A_Zusatzeinkommen))
                .ForMember(dest => dest.RUECKZAHLUNGSART, opt => opt.MapFrom(src => src.A_Rueckzahlungsart))
                .ForMember(dest => dest.REGELMAESSIGEAUSLAGEN, opt => opt.MapFrom(src => src.A_Regelmaessige_Auslagen))
                .ForMember(dest => dest.KUNDENART, opt => opt.MapFrom(src => src.A_Kundenart))
                .ForMember(dest => dest.CSEINHEIT, opt => opt.MapFrom(src => src.A_CS_Einheit))
                .ForMember(dest => dest.BERUFLICHESITUATION, opt => opt.MapFrom(src => src.A_Berufliche_Situation))
                .ForMember(dest => dest.AUSZAHLUNGSART, opt => opt.MapFrom(src => src.A_Auszahlungsart))
                .ForMember(dest => dest.AUSLAENDERAUSWEIS, opt => opt.MapFrom(src => src.A_Auslaenderausweis))
                .ForMember(dest => dest.EINKOMMENART, opt => opt.MapFrom(src => src.A_Einkommen_Art))
                .ForMember(dest => dest.BERUFSAUSLAGEN, opt => opt.MapFrom(src => src.A_Berufsauslagen))
                .ForMember(dest => dest.APID, opt => opt.MapFrom(src => src.A_A_PID))
                .ForMember(dest => dest.ARBEITGEBERBESCHAEFTIGTBIS, opt => opt.MapFrom(src => src.A_Arbeitgeber_beschaeftigt_bis))
                .ForMember(dest => dest.ARBEITGEBERSEITWANN, opt => opt.MapFrom(src => src.A_Arbeitgeber_seit_wann))
                .ForMember(dest => dest.AUSLAENDERAUSWEISEINREISEDATUM, opt => opt.MapFrom(src => src.A_Auslaenderausweis_Einreisedatum))
                .ForMember(dest => dest.AUSLAENDERAUSWEISGUELTIGKEIT, opt => opt.MapFrom(src => src.A_Auslaenderausweis_Gueltigkeitsdatum))
                .ForMember(dest => dest.BERUFSAUSLAGENBETRAG, opt => opt.MapFrom(src => src.A_Berufsauslagen_Betrag))
                .ForMember(dest => dest.BESTEHENDEKREDITRATE, opt => opt.MapFrom(src => src.A_Bestehende_Kreditrate))
                .ForMember(dest => dest.BESTEHENDELEASINGRATE, opt => opt.MapFrom(src => src.A_Bestehende_Leasingrate))
                .ForMember(dest => dest.BILANZSUMME, opt => opt.MapFrom(src => src.A_Bilanzsumme))
                .ForMember(dest => dest.DATUMLETZTERJAHRESABSCHLUSS, opt => opt.MapFrom(src => src.A_Datum_letzter_Jahresabschluss))
                .ForMember(dest => dest.EIGENKAPITAL, opt => opt.MapFrom(src => src.A_Eigenkapital))
                .ForMember(dest => dest.EMAIL, opt => opt.MapFrom(src => src.A_E_Mail))
                .ForMember(dest => dest.FLUESSIGEMITTEL, opt => opt.MapFrom(src => src.A_fluessige_mtel))
                .ForMember(dest => dest.GEBURTSDATUM, opt => opt.MapFrom(src => src.A_Geburtsdatum))
                .ForMember(dest => dest.HAUPTEINKOMMENBETRAG, opt => opt.MapFrom(src => src.A_Haupteinkommen_Betrag))
                .ForMember(dest => dest.HIWERWOHNHAFTSEIT, opt => opt.MapFrom(src => src.A_hier_Wohnhaft_seit))
                .ForMember(dest => dest.HOEHEBETREIBUNGEN, opt => opt.MapFrom(src => src.A_Hoehe_der_Betreibungen))
                .ForMember(dest => dest.INSTRADIERUNG, opt => opt.MapFrom(src => src.A_Instradierung))
                .ForMember(dest => dest.JAEHRLGRATIFIKATIONBONUS, opt => opt.MapFrom(src => src.A_Jaehl_Gratifikation_Bonus))
                .ForMember(dest => dest.JAHRESGEWINN, opt => opt.MapFrom(src => src.A_Jahregewinn))
                .ForMember(dest => dest.JAHRESUMSATZ, opt => opt.MapFrom(src => src.A_Jahresumsatz))
                .ForMember(dest => dest.KANTON, opt => opt.MapFrom(src => src.A_Kanton))
                .ForMember(dest => dest.KURZFRISTIGEVERBINDLICHKEITEN, opt => opt.MapFrom(src => src.A_Kurzfristige_Verbindlichkeiten))
                .ForMember(dest => dest.LAND, opt => opt.MapFrom(src => src.A_Land))
                .ForMember(dest => dest.MOBILTELEFON, opt => opt.MapFrom(src => src.A_Mobiltelefon))
                .ForMember(dest => dest.NATIONALITAET, opt => opt.MapFrom(src => src.A_Nationalitaet))
                .ForMember(dest => dest.NEBENEINKOMMENBETRAG, opt => opt.MapFrom(src => src.A_Nebeneinkommen_Betrag))
                .ForMember(dest => dest.PLZ, opt => opt.MapFrom(src => src.A_PLZ))
                .ForMember(dest => dest.REGELMAESSIGEAUSLAGENBETRAG, opt => opt.MapFrom(src => src.A_Regelmaessige_Auslagen_Betrag))
                .ForMember(dest => dest.TELEFON1, opt => opt.MapFrom(src => src.A_Telefon_1))
                .ForMember(dest => dest.TELEFON2, opt => opt.MapFrom(src => src.A_Telefon_2))
                .ForMember(dest => dest.TELEFONGESCHAEFTLICH, opt => opt.MapFrom(src => src.A_Telefon_geschaeftlich))
                .ForMember(dest => dest.TELEFONPRIVAT, opt => opt.MapFrom(src => src.A_Telefon_privat))
                .ForMember(dest => dest.UNTERSTUETZUNGSBEITRAEGEBETRAG, opt => opt.MapFrom(src => src.A_Unterstuetzungsbeitraege_Betrag))
                .ForMember(dest => dest.WOHNKOSTENMIETE, opt => opt.MapFrom(src => src.A_Wohnkosten_Miete))
                .ForMember(dest => dest.ZUSATZEINKOMMENBETRAG, opt => opt.MapFrom(src => src.A_Zusatzeinkommen_Betrag))
                .ForMember(dest => dest.MOCOUNTER, opt => opt.MapFrom(src => (int?)src.A_MO_Counter))
                .ForMember(dest => dest.NAMEAG2, opt => opt.MapFrom(src => src.A_AG_NAME2))

                // CRMGT00028820 Erweiterung Schnittstelle Decision Engine
                .ForMember(dest => dest.EHEPARTNERFLAG, opt => opt.MapFrom(src => (int?)src.A_EhePartnerFlag))
                .ForMember(dest => dest.WEITEREVERPFLICHTUNGENBETRAG, opt => opt.MapFrom(src => src.A_Weitere_Verpflichtungen_Betrag))
                .ForMember(dest => dest.WEITEREVERPFLICHTUNGEN, opt => opt.MapFrom(src => src.A_Weitere_Verpflichtungen))
                .ForMember(dest => dest.UIDNUMMER, opt => opt.MapFrom(src => src.A_UID))
                .ForMember(dest => dest.NAMEAG, opt => opt.MapFrom(src => src.A_AG_NAME))
                .ForMember(dest => dest.NEBENEINKOMMENSEIT, opt => opt.MapFrom(src => src.A_Nebeneinkommen_seit_wann))
                .ForMember(dest => dest.GESCHLECHT, opt => opt.MapFrom(src => src.A_Geschlecht))

                //BNR11 BNRELF-393
                .ForMember(dest => dest.ZULAGEKIND, opt => opt.MapFrom(src => src.A_ZULAGEKIND))
                .ForMember(dest => dest.ZULAGEAUSBILDUNG, opt => opt.MapFrom(src => src.A_ZULAGEAUSBILDUNG))
                .ForMember(dest => dest.ZULAGESONST, opt => opt.MapFrom(src => src.A_ZULAGESONST))
                ;

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DecisionEngineInDto, CIC.Database.DE.EF6.Model.DEPARTNER>()
                .ForMember(dest => dest.ANZANTRAEGE, opt => opt.MapFrom(src => (int?)src.Anz_Antraege))
                .ForMember(dest => dest.ANZLFDVERTRAEGE, opt => opt.MapFrom(src => (int?)src.Anz_lfd_Vertraege))
                .ForMember(dest => dest.ANZPENDENTEANTRAEGE, opt => opt.MapFrom(src => (int?)src.Anz_pendente_Antraege))
                .ForMember(dest => dest.ANZVERTRAEGE, opt => opt.MapFrom(src => (int?)src.Anz_Vertraege))
                .ForMember(dest => dest.FLAGAKTIV, opt => opt.MapFrom(src => (int?)src.flagAktiv))
                .ForMember(dest => dest.FLAGEPOS, opt => opt.MapFrom(src => (int?)src.flagEPOS))
                .ForMember(dest => dest.FLAGVSB, opt => opt.MapFrom(src => (int?)src.flagVSB))
                .ForMember(dest => dest.VERTRIEBSPARTNERID, opt => opt.MapFrom(src => (long?)src.VertriebspartnerID))
                .ForMember(dest => dest.UIDNUMMER, opt => opt.MapFrom(src => src.UIDNummer))
                .ForMember(dest => dest.STRATEGICACCOUNT, opt => opt.MapFrom(src => (int?)src.Strategic_Account))
                
                .ForMember(dest => dest.DEMOLIMIT, opt => opt.MapFrom(src => src.Demolimit))
                .ForMember(dest => dest.DEMOENGAGEMENT, opt => opt.MapFrom(src => src.Demoengagement))
                .ForMember(dest => dest.EVENTUALDEMOENGAGEMENT, opt => opt.MapFrom(src => src.Eventualdemoengagement))
                ;

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DecisionEngineInDto, CIC.Database.DE.EF6.Model.DESALES>()
                .ForMember(dest => dest.ANZABLOESEN, opt => opt.MapFrom(src => (int?)src.Anz_Abloesen))
                .ForMember(dest => dest.ANZEIGENABLOESEN, opt => opt.MapFrom(src => (int?)src.Anz_Eigenabloesen))
                .ForMember(dest => dest.ANZFREMDABLOESEN, opt => opt.MapFrom(src => (int?)src.Anz_Fremdabloesen))
                .ForMember(dest => dest.NAMEABLOESEBANK1, opt => opt.MapFrom(src => src.Name_Abloesebank_1))
                .ForMember(dest => dest.NAMEABLOESEBANK2, opt => opt.MapFrom(src => src.Name_Abloesebank_2))
                .ForMember(dest => dest.NAMEABLOESEBANK3, opt => opt.MapFrom(src => src.Name_Abloesebank_3))
                .ForMember(dest => dest.NAMEABLOESEBANK4, opt => opt.MapFrom(src => src.Name_Abloesebank_4))
                .ForMember(dest => dest.NAMEABLOESEBANK5, opt => opt.MapFrom(src => src.Name_Abloesebank_5))
                .ForMember(dest => dest.SUMMEABLOESEN, opt => opt.MapFrom(src => src.Summe_Abloesen))
                .ForMember(dest => dest.SUMMEEIGENABLOESEN, opt => opt.MapFrom(src => src.Summe_Eigenabloesen))
                .ForMember(dest => dest.SUMMEFREMDABLOESEN, opt => opt.MapFrom(src => src.Summe_Fremdabloesen))
                .ForMember(dest => dest.VALIDABL, opt => opt.MapFrom(src => src.Validabl))
                //BNR13  TODO
                //.ForMember(dest => dest.SYSVARTABL, opt => opt.MapFrom(src => (long?)src.Abl_Produkt))
                //.ForMember(dest => dest.VTDAUERABLTOTALL, opt => opt.MapFrom(src => src.Abl_Dauer_Total))
                //.ForMember(dest => dest.ANZABLVTPERVART, opt => opt.MapFrom(src => src.Abl_Anz_Vertragsart))
                //.ForMember(dest => dest.VTDAUERABLPERVART, opt => opt.MapFrom(src => src.Abl_Dauer_Vertragsart))
                //.ForMember(dest => dest.VTDAUERABL, opt => opt.MapFrom(src => src.Abl_LZ_Vorvertrag_Vertragsart))
                ;

            //Aggregation
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationOLOutDto, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationOLOutDto>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal!=null));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationZekOutDto, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationZekOutDto>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal!=null));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationInDto, AGGINPDATA>();
            CreateMap<AGGINPDATA, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationInDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationDVOutDto, AGGOUTDV>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationOLOutDto, AGGOUTOL>()
                .ForMember(dest => dest.DATEZVB, src=> src.ResolveUsing<CustomIntDateResolver,int?>(s => s.DATEZVB));
           
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationVPOutDto, AGGOUTVP>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AggregationZekOutDto, AGGOUTZEK>();



            

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBatchContractUpdateInstructionDto, DAO.Auskunft.ZEKBatchRef.ContractUpdateInstruction>()
                .ForMember(dest => dest.teilzahlungsvertrag, opt => opt.MapFrom(src => src.teilzahlungskredit));

            CreateMap<DAO.Auskunft.EurotaxVinRef.VinDecodeOutputType, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxVinOutDto>();

            #endregion Auskunft
            //BNR11 CR
            #region EC2FOROL
            CreateMap<AuskunftDto, AuskunftOLDto>();
            CreateMap<ZekOutDto, ZekOLOutDto>()
                .ForMember(dest => dest.FoundContracts, opt => opt.Ignore());
            CreateMap<ZekOLInDto, ZekInDto>();

            CreateMap<ZekAmtsinformationDescriptionDto, ZekOLAmtsinformationDescriptionDto>();
            CreateMap<ZekBardarlehenDescriptionDto, ZekOLBardarlehenDescriptionDto>();
            CreateMap<ZekFestkreditDescriptionDto, ZekOLFestkreditDescriptionDto>();
            CreateMap<ZekKartenengagementDescriptionDto, ZekOLKartenengagementDescriptionDto>();
            CreateMap<ZekKarteninformationDescriptionDto, ZekOLKarteninformationDescriptionDto>();
            CreateMap<ZekKontokorrentkreditDescriptionDto, ZekOLKontokorrentkreditDescriptionDto>();
            CreateMap<ZekKreditgesuchDescriptionDto, ZekOLKreditgesuchDescriptionDto>();
            CreateMap<ZekLeasingMietvertragDescriptionDto, ZekOLLeasingMietvertragDescriptionDto>();
            CreateMap<ZekSolidarschuldnerDescriptionDto, ZekOLSolidarschuldnerDescriptionDto>();
            CreateMap<ZekTeilzahlungskreditDescriptionDto, ZekOLTeilzahlungskreditDescriptionDto>();
            CreateMap<ZekUeberziehungskreditDescriptionDto, ZekOLUeberziehungskreditDescriptionDto>();
            CreateMap<ZekTeilzahlungsvertragDescriptionDto, ZekOLTeilzahlungsvertragDescriptionDto>();
            CreateMap<ZekeCode178Dto, ZekOLeCode178DtoDescriptionDto>();

            CreateMap<ZEKAIC,ZekAmtsinformationDescriptionDto>();

            CreateMap<ZEKBDC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto>()
               .ForMember(dest => dest.bonitaetscodeZEK, opt => opt.MapFrom(src => src.BONITAETSCODE))
               .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.DATUMBONITAET))
               .ForMember(dest => dest.anzahlMonatlicheRaten, opt => opt.MapFrom(src => (int)src.ANZAHLMONATLICHERRATEN))
               .ForMember(dest => dest.kreditbetrag, opt => opt.MapFrom(src => (float)src.KREDITBETRAG))
               .ForMember(dest => dest.monatsrate, opt => opt.MapFrom(src => (float)src.MONATSRATE));

            
            CreateMap<ZEKFKC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFestkreditDescriptionDto>()
              .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.BONITAETSCODE))
              .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.DATUMBONITAET))
              .ForMember(dest => dest.Kreditbetrag, opt => opt.MapFrom(src => (float)src.KREDITBETRAG));
            
            CreateMap<ZEKKEC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKartenengagementDescriptionDto>()
                .ForMember(dest => dest.SaldoAbrechnungsTag, opt => opt.MapFrom(src => (decimal?)src.SALDOABRECHNUNGSTAG));
            
            
            CreateMap<ZEKKIC,ZekKarteninformationDescriptionDto>();

            CreateMap<ZEKKKC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKontokorrentkreditDescriptionDto>()
                .ForMember(dest => dest.bonitaetscodeZEK, opt => opt.MapFrom(src => src.BONITAETSCODE))
                .ForMember(dest => dest.datumBonitaetZEK, opt => opt.MapFrom(src => src.DATUMBONITAET))
                .ForMember(dest => dest.datumVertragsBeginn, opt => opt.MapFrom(src => (String)src.DATUMERSTERATE))
                .ForMember(dest => dest.datumVertragsEnde, opt => opt.MapFrom(src => (String)src.DATUMLETZTERATE));

            
            
            CreateMap<ZEKKGC,ZekKreditgesuchDescriptionDto>();

            CreateMap<ZEKLMC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekLeasingMietvertragDescriptionDto>()
               .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.BONITAETSCODE))
               .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.DATUMBONITAET))
               .ForMember(dest => dest.AnzahlMonatlicheRaten, opt => opt.MapFrom(src => (int)src.ANZAHLMONATLICHERRATEN))
               .ForMember(dest => dest.Kreditbetrag, opt => opt.MapFrom(src => (float)src.KREDITBETRAG))
               .ForMember(dest => dest.ErsteGrosseRate, opt => opt.MapFrom(src => (int?)src.ERSTEGROSSERATE))
               .ForMember(dest => dest.Monatsrate, opt => opt.MapFrom(src => (float)src.MONATSRATE));

            CreateMap<ZEKSSC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekSolidarschuldnerDescriptionDto>()
                .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.BONITAETSCODE))
                .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.DATUMBONITAET))
                .ForMember(dest => dest.AnzahlMonatlicheRaten, opt => opt.MapFrom(src => (int)src.ANZAHLMONATLICHERRATEN))
                .ForMember(dest => dest.Monatsrate, opt => opt.MapFrom(src => (float)src.MONATSRATE));

            CreateMap<ZEKTKC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekTeilzahlungskreditDescriptionDto>()
              .ForMember(dest => dest.BonitaetscodeZEK, opt => opt.MapFrom(src => src.BONITAETSCODE))
              .ForMember(dest => dest.DatumBonitaetZEK, opt => opt.MapFrom(src => src.DATUMBONITAET))
              .ForMember(dest => dest.AnzahlMonatlicheRaten, opt => opt.MapFrom(src => (int)src.ANZAHLMONATLICHERRATEN))
              .ForMember(dest => dest.Kreditbetrag, opt => opt.MapFrom(src => (float)src.KREDITBETRAG))
              .ForMember(dest => dest.Monatsrate, opt => opt.MapFrom(src => (float)src.MONATSRATE));

            CreateMap<ZEKUKC, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekUeberziehungskreditDescriptionDto>()
                .ForMember(dest => dest.SaldoKontoAuszug, opt => opt.MapFrom(src => (float)src.SALDOKONTOAUSZUG));
                
            #endregion EC2FOROL




            CreateMap<Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BContract, Cic.OpenOne.GateBANKNOW.Common.DTO.mTanUserDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.mTanUserDto, Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BContract>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BStatusContract, Cic.OpenOne.GateBANKNOW.Common.DTO.mTanStatusDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.mTanStatusDto, Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BStatusContract>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract, Cic.OpenOne.GateBANKNOW.Common.DTO.mTanStatusDto>();
            

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract, Cic.OpenOne.GateBANKNOW.Common.DTO.ocreatemTanUserDto>()
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.users, opt => opt.MapFrom(src => src.Users));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract, Cic.OpenOne.GateBANKNOW.Common.DTO.osetmTanUserDataDto>()
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.users, opt => opt.MapFrom(src => src.Users));
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract, Cic.OpenOne.GateBANKNOW.Common.DTO.ogetmTanUserDataDto>()
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.users, opt => opt.MapFrom(src => src.Users));


           

            CreateMap<Cic.OpenOne.Common.DTO.DdlkpsposDto, CIC.Database.OD.EF6.Model.DDLKPSPOS>();
            CreateMap<CIC.Database.OD.EF6.Model.DDLKPSPOS, Cic.OpenOne.Common.DTO.DdlkpsposDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObAustDto, CIC.Database.OL.EF6.Model.ANTOBAUST>();
            CreateMap<CIC.Database.OL.EF6.Model.ANTOBAUST, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObAustDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ELInDto, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ELInDto>();

            //CRIF KNE CREATION
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>();
            
            
			//AUTOMAPPER6
            CreateMap<CIC.Database.OL.EF6.Model.ITKONTO, KontoDto>();
            CreateMap<CIC.Database.OL.EF6.Model.BLZ, BlzDto>().ForMember(dest => dest.blz, opt => opt.MapFrom(src => src.BLZ1));



        }
    }
}