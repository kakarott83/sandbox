using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// BankNowModelProfileServices
    /// </summary>
    public class BankNowModelProfileServices : BankNowModelProfile
    {
        /// <summary>
        /// Konfigurieren
        /// </summary>
        public BankNowModelProfileServices()
        {

            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto, Cic.OpenOne.GateBANKNOW.Service.DTO.UkzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto, Cic.OpenOne.GateBANKNOW.Service.DTO.PkzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ZusatzdatenDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AdresseDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KontoDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.BuchwertDto, Cic.OpenOne.GateBANKNOW.Service.DTO.BuchwertDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.BuchwertInfoDto, Cic.OpenOne.GateBANKNOW.Service.DTO.BuchwertInfoDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.BlzDto, Cic.OpenOne.GateBANKNOW.Service.DTO.BlzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.ParamDto, Cic.OpenOne.Common.DTO.Prisma.ParamDto>();
            CreateMap<Cic.OpenOne.Common.DTO.Prisma.ParamDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ParamDto>();
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntAblDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntAblDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntKalkDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AngAntProvDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntProvDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AngAntSubvDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntSubvDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AngAntVsDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObBriefDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AvailableNewsDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AvailableNewsDto>();
            CreateMap<Cic.OpenOne.Common.DTO.AttachmentDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AttachmentDto>();
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.PkzDto, Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.UkzDto, Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.ZusatzdatenDto, Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AdresseDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.KontoDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto>();
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>().
                ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal!=null));             // this allows nullables that are null not to overwrite db values
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>().
                ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal != null));             // this allows nullables that are null not to overwrite db values
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntAblDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntAblDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntProvDto, Cic.OpenOne.Common.DTO.AngAntProvDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntSubvDto, Cic.OpenOne.Common.DTO.AngAntSubvDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObBriefDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObSmallDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObSmallDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.DokumenteDto, Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.FileattDto, Cic.OpenOne.GateBANKNOW.Common.DTO.FileattDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.FileDto, Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.icreateListenExportDto, Cic.OpenOne.GateBANKNOW.Common.DTO.icreateListenExportDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto, Cic.OpenOne.GateBANKNOW.Service.DTO.DokumenteDto>();
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AvailableAlertsDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AvailableAlertsDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.FileattDto, Cic.OpenOne.GateBANKNOW.Service.DTO.FileattDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto, Cic.OpenOne.GateBANKNOW.Service.DTO.FileDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.ocreatemTanUserDto, Cic.OpenOne.GateBANKNOW.Common.DTO.ocreatemTanUserDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.ogetmTanUserDataDto, Cic.OpenOne.GateBANKNOW.Common.DTO.ogetmTanUserDataDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.osetmTanUserDataDto, Cic.OpenOne.GateBANKNOW.Common.DTO.osetmTanUserDataDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.imTanUserDto, Cic.OpenOne.GateBANKNOW.Common.DTO.imTanUserDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.mTanStatusDto, Cic.OpenOne.GateBANKNOW.Common.DTO.mTanStatusDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.mTanUserDto, Cic.OpenOne.GateBANKNOW.Common.DTO.mTanUserDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ocreatemTanUserDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ocreatemTanUserDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ogetmTanUserDataDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ogetmTanUserDataDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.osetmTanUserDataDto, Cic.OpenOne.GateBANKNOW.Service.DTO.osetmTanUserDataDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.imTanUserDto, Cic.OpenOne.GateBANKNOW.Service.DTO.imTanUserDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.mTanStatusDto, Cic.OpenOne.GateBANKNOW.Service.DTO.mTanStatusDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.mTanUserDto, Cic.OpenOne.GateBANKNOW.Service.DTO.mTanUserDto>();


            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.isetmTanUserPasswordDto, Cic.OpenOne.GateBANKNOW.Common.DTO.isetmTanUserPasswordDto>();
            CreateMap<GateBANKNOW.Common.DTO.operformKaufofferte, Cic.OpenOne.GateBANKNOW.Service.DTO.operformKaufofferte>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObAustDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObAustDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObAustDto,Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObAustDto>();

            CreateMap<BudgetDto, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.KREMOInDto>().
                ForMember(a => a.GebDatum, opt => opt.Ignore());
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.KREMOInDto>().
                ForMember(a => a.GebDatum, opt => opt.Ignore());
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.KREMOInDto>().
                ForMember(a => a.Famstandcode, opt => opt.MapFrom(src => (float)src.familienstand)).
                ForMember(a => a.Anzkind1,  opt => opt.MapFrom(src => (float)src.anzkinder)).
                ForMember(a => a.Anzkind2, opt => opt.MapFrom(src => (float)src.anzkinder1)).
                ForMember(a => a.Anzkind3, opt => opt.MapFrom(src => (float)src.anzkinder2)).
                ForMember(a => a.Anzkind4, opt => opt.MapFrom(src => (float)src.anzkinder3));

            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.KremoBudgetDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KremoBudgetDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ogetKremoBudget, Cic.OpenOne.GateBANKNOW.Service.DTO.ogetKremoBudget>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KremoLaufzeitFaktorDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KremoLaufzeitFaktorDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.DocumentTypeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.DocumentTypeDto>();
            

            CreateMap<GateBANKNOW.Common.DTO.oPreisschildDruckDto, Cic.OpenOne.GateBANKNOW.Service.DTO.oPreisschildDruckDto>();

            CreateMap<GateBANKNOW.Common.DTO.oFinVariantenDruckenDto, Cic.OpenOne.GateBANKNOW.Service.DTO.oFinVariantenDruckenDto>();

            //R11 TrasaktionRisikoPrüfung
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.irisikoSimDto, GateBANKNOW.Common.DTO.irisikoSimDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.icheckTrRiskDto, GateBANKNOW.Common.DTO.icheckTrRiskDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.ifinVorEinreichungDto, GateBANKNOW.Common.DTO.ifinVorEinreichungDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.isolveKalkVariantenDto, GateBANKNOW.Common.DTO.icheckTrRiskDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.orisikoSimDto, GateBANKNOW.Common.DTO.orisikoSimDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto, GateBANKNOW.Common.DTO.ocheckTrRiskDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.ofinVorEinreichungDto, GateBANKNOW.Common.DTO.ofinVorEinreichungDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.osolveKalkVariantenDto, GateBANKNOW.Common.DTO.ocheckTrRiskDto>();
          
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.irisikoSimDto, GateBANKNOW.Service.DTO.irisikoSimDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.icheckTrRiskDto, GateBANKNOW.Service.DTO.icheckTrRiskDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ifinVorEinreichungDto, GateBANKNOW.Service.DTO.ifinVorEinreichungDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.isolveKalkVariantenDto, GateBANKNOW.Service.DTO.icheckTrRiskDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.orisikoSimDto, GateBANKNOW.Service.DTO.orisikoSimDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckTrRiskDto, GateBANKNOW.Service.DTO.ocheckTrRiskDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ofinVorEinreichungDto, GateBANKNOW.Service.DTO.ofinVorEinreichungDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.osolveKalkVariantenDto, GateBANKNOW.Service.DTO.ocheckTrRiskDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckTrRiskDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskByIdDto>();


            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.icreateOrUpdateDMSAkteDto, Cic.OpenOne.GateBANKNOW.Common.DTO.icreateOrUpdateDMSAkteDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.igetDMSUrlDto, Cic.OpenOne.GateBANKNOW.Common.DTO.igetDMSUrlDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.iDMSUploadDto, Cic.OpenOne.GateBANKNOW.Common.DTO.iDMSUploadDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.DMSField, Cic.OpenOne.GateBANKNOW.Common.DTO.DMSField>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.DMSValue, Cic.OpenOne.GateBANKNOW.Common.DTO.DMSValue>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.DMSFieldType, Cic.OpenOne.GateBANKNOW.Common.DTO.DMSFieldType>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.icreateOrUpdateDMSDokumentDto, Cic.OpenOne.GateBANKNOW.Common.DTO.icreateOrUpdateDMSDokmentDto>();

            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto, Cic.OpenOne.GateBANKNOW.Service.DTO.DmsDocDto>();
            
            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.DmsDocDto,Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto>();


            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.VertragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.VertragDto>();
            
            CreateMap<Common.DTO.MyPocketDto, Service.DTO.MyPocketDto>();



            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ProductCreditInfoDto, GateBANKNOW.Service.DTO.ProductCreditInfoDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ProductCreditLimitDto, GateBANKNOW.Service.DTO.ProductCreditLimitDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ProductCreditLimitFetchDto, GateBANKNOW.Service.DTO.ProductCreditLimitDto>();



            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KneDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KneDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ogetControlPersonBusinessDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ogetControlPersonBusinessDto>();

//AUTOMAPPER6 adding inline mappings here
            CreateMap<Common.BO.Auskunft.PST.OutputDataData, Common.DTO.Auskunft.FoodasEventOutDataDto>();
            CreateMap<Common.BO.Auskunft.PST.OutputDataData, Common.DTO.Auskunft.FoodasGetDokumentOutDto>();


            CreateMap<KundeExternDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.osearchKundeExternNonGenericDto, osearchKundeExternNonGenericDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternResultDto, KundeExternResultDto>();


            CreateMap<iFibuAccountMasterDTO, Cic.OpenOne.Common.DTO.FibuAccountMasterDTO>();
            CreateMap<iFibuAccountDTO, Cic.OpenOne.Common.DTO.FibuAccountDTO>();


            CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>();

            
            
            CreateMap<iEurotaxGetForecastDto, Common.DTO.Auskunft.EurotaxInDto>();
            CreateMap<Common.DTO.Auskunft.EurotaxOutDto, EurotaxGetForecastDto>();
            CreateMap<Common.DTO.Auskunft.EurotaxOutDto, EurotaxGetValuationDto>();


            
            
            CreateMap<GateBANKNOW.Common.DTO.ProfilDto, ogetProfilDto>();
            CreateMap<GateBANKNOW.Common.DTO.KamDto, ogetKamDto>();
            CreateMap<GateBANKNOW.Common.DTO.AbwicklungsortDto, ogetAbwicklungsortDto>();

            
            CreateMap<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>();
            
            CreateMap<Cic.OpenOne.Common.DTO.InsuranceDto, Cic.OpenOne.GateBANKNOW.Service.DTO.InsuranceDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ochangeRRReceiver, Cic.OpenOne.GateBANKNOW.Service.DTO.ochangeRRReceiverDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.PlzDto, Cic.OpenOne.GateBANKNOW.Service.DTO.PlzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ofindBankByBlzDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ofindBankByBlzDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.ofindIBANByBlzDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ofindIBANByBlzDto>();

            CreateMap<GateBANKNOW.Common.DTO.ogetBuchwertDto, ogetBuchwertDto>();
            CreateMap<igetBuchwertDto, GateBANKNOW.Common.DTO.igetBuchwertDto>();
            CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternGUIDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KundeExternGUIDto>();
            


        }
    }
}