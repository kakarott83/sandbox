using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Cic.OpenOne.Common.Util.Mapper;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using AutoMapper.Mappers;
using CIC.Database.OL.EF6.Model;
using AutoMapper.Configuration;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Automapper Config for Kalkulation
    /// </summary>
    class Conversion
    {
        private static Conversion Instance = null;
        private IMapper Mapping = null;

        private Conversion()
        {
            Mapping = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config) {
            
            Config.CreateMap<short, int?>().ConvertUsing<GenericNullableConverter<short, int?>>();
            Config.CreateMap<double, decimal?>().ConvertUsing<GenericNullableConverter<double, decimal?>>();
            Config.CreateMap<bool, int?>().ConvertUsing<GenericNullableConverter<bool, int?>>();
            Config.CreateMap<AngAntVarDto, ANGVAR>().ForMember(d => d.RANG, m => m.MapFrom<short>(s => s.rang))
                                                  .ForMember(d => d.BESCHREIBUNG, opt => opt.Ignore())
                                                  .ForMember(d => d.SYSVTTYP, opt => opt.Ignore())
                                                  .ForMember(d => d.SYSWAEHRUNG, opt => opt.Ignore())
                                                  .ForMember(d => d.ANGVSList, opt => opt.Ignore())
                                                  .ForMember(d => d.ANGSUBVList, opt => opt.Ignore())
                                                  .ForMember(d => d.ANGKALKList, opt => opt.Ignore())
                                                  .ForMember(d => d.SANGKALK, opt => opt.Ignore())
                                                  .ForMember(d => d.ANGPROVList, opt => opt.Ignore())
                                                  .ForMember(d => d.ANGEBOT, opt => opt.Ignore());
            Config.CreateMap<AngAntKalkDto, ANGKALK>().ForMember(d => d.BGEXTERN, m => m.MapFrom<double>(s => s.bgextern))
                                                      .ForMember(d => d.LZ, m => m.MapFrom<short>(s => s.lz))
                                                      .ForMember(d => d.VERRECHNUNGFLAG, m => m.MapFrom<bool>(s => s.verrechnungFlag))
                                                      .ForMember(d => d.BEGINN, opt => opt.Ignore())
                                                      .ForMember(d => d.SYSKALKTYP, opt => opt.Ignore())
                                                      .ForMember(d => d.GRUND, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUBEHOER, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUABSCHLAG1, opt => opt.Ignore())
                                                      .ForMember(d => d.GESAMT, opt => opt.Ignore())
                                                      .ForMember(d => d.RABATTO, opt => opt.Ignore())
                                                      .ForMember(d => d.SUBVENTIONO, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUABSCHLAG2, opt => opt.Ignore())
                                                      .ForMember(d => d.RABATTV, opt => opt.Ignore())
                                                      .ForMember(d => d.SUBVENTIONV, opt => opt.Ignore())
                                                      .ForMember(d => d.AHK, opt => opt.Ignore())
                                                      .ForMember(d => d.SYSVT, opt => opt.Ignore())
                                                      .ForMember(d => d.ANZAHLUNG, opt => opt.Ignore())
                                                      .ForMember(d => d.RGGEBUEHR, opt => opt.Ignore())
                                                      .ForMember(d => d.RGGVERR, opt => opt.Ignore())
                                                      .ForMember(d => d.PROVISION, opt => opt.Ignore())
                                                      .ForMember(d => d.GEBUEHR, opt => opt.Ignore())
                                                      .ForMember(d => d.DISAGIO, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUABSCHLAG4, opt => opt.Ignore())
                                                      .ForMember(d => d.RWKALK, opt => opt.Ignore())
                                                      .ForMember(d => d.DB, opt => opt.Ignore())
                                                      .ForMember(d => d.PPY, opt => opt.Ignore())
                                                      .ForMember(d => d.MODUS, opt => opt.Ignore())
                                                      .ForMember(d => d.BASISZINS, opt => opt.Ignore())
                                                      .ForMember(d => d.REFIZINS1, opt => opt.Ignore())
                                                      .ForMember(d => d.ZINS1, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUBEHOERORP, opt => opt.Ignore())
                                                      .ForMember(d => d.FZUABSCHLAG2, opt => opt.Ignore())
                                                      .ForMember(d => d.RABATTVP, opt => opt.Ignore())
                                                      .ForMember(d => d.FZUABSCHLAG3, opt => opt.Ignore())
                                                      .ForMember(d => d.ANZAHLUNGP, opt => opt.Ignore())
                                                      .ForMember(d => d.SZP, opt => opt.Ignore())
                                                      .ForMember(d => d.PROVISIONP, opt => opt.Ignore())
                                                      .ForMember(d => d.MARGE, opt => opt.Ignore())
                                                      .ForMember(d => d.MARGEP, opt => opt.Ignore())
                                                      .ForMember(d => d.FZUABSCHLAG4, opt => opt.Ignore())
                                                      .ForMember(d => d.RWP, opt => opt.Ignore())
                                                      .ForMember(d => d.DBP, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUBEHOEROR, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUBEHOERP, opt => opt.Ignore())
                                                      .ForMember(d => d.RABATTOP, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUSTAND, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUSTANDAM, opt => opt.Ignore())
                                                      .ForMember(d => d.SCHWACKE, opt => opt.Ignore())
                                                      .ForMember(d => d.SERVICEFEE, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUBEHOERNETTO, opt => opt.Ignore())
                                                      .ForMember(d => d.ZUBEHOERBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.ZOLLGEBUEHR, opt => opt.Ignore())
                                                      .ForMember(d => d.ZOLLGEBUEHRP, opt => opt.Ignore())
                                                      .ForMember(d => d.ZINSTYP, opt => opt.Ignore())
                                                      .ForMember(d => d.PAKETE, opt => opt.Ignore())
                                                      .ForMember(d => d.PAKRABO, opt => opt.Ignore())
                                                      .ForMember(d => d.PAKRABOP, opt => opt.Ignore())
                                                      .ForMember(d => d.PAKRABV, opt => opt.Ignore())
                                                      .ForMember(d => d.PAKRABVP, opt => opt.Ignore())
                                                      .ForMember(d => d.SONZUB, opt => opt.Ignore())
                                                      .ForMember(d => d.SONZUBRABO, opt => opt.Ignore())
                                                      .ForMember(d => d.SONZUBRABOP, opt => opt.Ignore())
                                                      .ForMember(d => d.SONZUBRABV, opt => opt.Ignore())
                                                      .ForMember(d => d.SONZUBRABVP, opt => opt.Ignore())
                                                      .ForMember(d => d.RWBASE, opt => opt.Ignore())
                                                      .ForMember(d => d.RWCRV, opt => opt.Ignore())
                                                      .ForMember(d => d.CALCTARGET, opt => opt.Ignore())
                                                      .ForMember(d => d.HOLDFIELDS, opt => opt.Ignore())
                                                      .ForMember(d => d.MITFINB, opt => opt.Ignore())
                                                      .ForMember(d => d.DEPOTP, opt => opt.Ignore())
                                                      .ForMember(d => d.BGEXTERNBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.BGEXTERNUST, opt => opt.Ignore())
                                                      .ForMember(d => d.GEBUEHRBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.GEBUEHRINTERNBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.GEBUEHRUST, opt => opt.Ignore())
                                                      .ForMember(d => d.MITFINBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.MITFINUST, opt => opt.Ignore())
                                                      .ForMember(d => d.RGGFREI, opt => opt.Ignore())
                                                      .ForMember(d => d.RWBASEBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.RWBASEBRUTTOP, opt => opt.Ignore())
                                                      .ForMember(d => d.RWBASEUST, opt => opt.Ignore())
                                                      .ForMember(d => d.RWCRVBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.RWCRVBRUTTOP, opt => opt.Ignore())
                                                      .ForMember(d => d.RWCRVUST, opt => opt.Ignore())
                                                      .ForMember(d => d.RWKALKBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.RWKALKBRUTTOP, opt => opt.Ignore())
                                                      .ForMember(d => d.RWKALKUST, opt => opt.Ignore())
                                                      .ForMember(d => d.SZBRUTTOP, opt => opt.Ignore())
                                                      .ForMember(d => d.MITFIN, opt => opt.Ignore())
                                                      .ForMember(d => d.GESAMTBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.GESAMTUST, opt => opt.Ignore())
                                                      .ForMember(d => d.GESAMTNETTO, opt => opt.Ignore())
                                                      .ForMember(d => d.GESAMTKOSTENBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.GESAMTKOSTENUST, opt => opt.Ignore())
                                                      .ForMember(d => d.GESAMTKOSTEN, opt => opt.Ignore())
                                                      .ForMember(d => d.RATEGESAMT, opt => opt.Ignore())
                                                      .ForMember(d => d.RATEGESAMTBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.RATEGESAMTUST, opt => opt.Ignore())
                                                      .ForMember(d => d.GESAMTKREDIT, opt => opt.Ignore())
                                                      .ForMember(d => d.RESTKAUFPREIS, opt => opt.Ignore())
                                                      .ForMember(d => d.SYSINTTYPE, opt => opt.Ignore())
                                                      .ForMember(d => d.BGINTERNBRUTTO, opt => opt.Ignore())
                                                      .ForMember(d => d.RWKALKBRUTTOPDEF, opt => opt.Ignore())
                                                      .ForMember(d => d.RWKALKBRUTTODEF, opt => opt.Ignore())
                                                      .ForMember(d => d.RWKALKUSTDEF, opt => opt.Ignore())
                                                      .ForMember(d => d.RWKALKDEF, opt => opt.Ignore())
                                                      .ForMember(d => d.GEBUEHRNACHLASS, opt => opt.Ignore())
                                                      .ForMember(d => d.OBUSETYPE, opt => opt.Ignore())
                                                      
                                                      .ForMember(d => d.ANGVAR, opt => opt.Ignore())
                                                      
                                                      .ForMember(d => d.ANGKALKFS, opt => opt.Ignore())
                                                      .ForMember(d => d.OBTYP, opt => opt.Ignore())
                                                      
                                                      .ForMember(d => d.ANGPROVList, opt => opt.Ignore())
                                                      .ForMember(d => d.ANGOB, opt => opt.Ignore())
                                                      
                                                      .ForMember(d => d.SYSANGEBOT, opt => opt.Ignore())
                                                      .ForMember(d => d.BGINTERNUST, opt => opt.Ignore());
            Config.CreateMap<Cic.OpenOne.Common.DTO.AngAntProvDto, ANGPROV>().ForMember(d => d.SYSFI, opt => opt.Ignore())
                                                    .ForMember(d => d.BASIS, opt => opt.Ignore())
                                                    .ForMember(d => d.PROVISIONPRO, opt => opt.Ignore())
                                                    .ForMember(d => d.VALUTA, opt => opt.Ignore())
                                                    .ForMember(d => d.TEXT, opt => opt.Ignore())
                                                    .ForMember(d => d.USTPFLICHT, opt => opt.Ignore())
                                                    .ForMember(d => d.ART, opt => opt.Ignore())
                                                    .ForMember(d => d.ABRECHNUNG, opt => opt.Ignore())
                                                    .ForMember(d => d.INAKTIV, opt => opt.Ignore())
                                                    .ForMember(d => d.INAKTIVBIS, opt => opt.Ignore())
                                                    .ForMember(d => d.WUNSCHPROVISION, opt => opt.Ignore())
                                                    .ForMember(d => d.SYSPROVTARIF, opt => opt.Ignore())
                                                    .ForMember(d => d.PARTNER, opt => opt.Ignore())
                                                    
                                                    .ForMember(d => d.ANGOBSL, opt => opt.Ignore())
                                                    
                                                    .ForMember(d => d.ANGKALK, opt => opt.Ignore())
                                                    
                                                    .ForMember(d => d.PERSON, opt => opt.Ignore())
                                                    
                                                    .ForMember(d => d.ANGVAR, opt => opt.Ignore())
                                                    
                                                    .ForMember(d => d.RAPPROVISIONBRUTTOMIN, opt => opt.Ignore())
                                                    .ForMember(d => d.RAPPROVISIONBRUTTOMAX, opt => opt.Ignore());
            Config.CreateMap<Cic.OpenOne.Common.DTO.AngAntSubvDto, ANGSUBV>().ForMember(d => d.BETRAG, opt => opt.Ignore())
                                                    .ForMember(d => d.BEGINN, opt => opt.Ignore())
                                                    .ForMember(d => d.LZ, opt => opt.Ignore())
                                                    .ForMember(d => d.BETRAGUST, opt => opt.Ignore())
                                                    .ForMember(d => d.BETRAGDEF, opt => opt.Ignore())
                                                    .ForMember(d => d.CODE, opt => opt.Ignore())
                                                    .ForMember(d => d.SYSPRSUBV, opt => opt.Ignore())
                                                    .ForMember(d => d.ANGVAR, opt => opt.Ignore())                                                    
                                                    .ForMember(d => d.SUBVTYP, opt => opt.Ignore())                                                    
                                                    .ForMember(d => d.ANGEBOT, opt => opt.Ignore());
            Config.CreateMap<Cic.OpenOne.Common.DTO.AngAntVsDto, ANGVS>().ForMember(d => d.PRAEMIENSTUFE, opt => opt.Ignore())
                                                
                                                .ForMember(d => d.VORVERSICHERUNG, opt => opt.Ignore())
                                                .ForMember(d => d.NACHLASS, opt => opt.Ignore())
                                                .ForMember(d => d.PRAEMIEDEFAULT, opt => opt.Ignore())
                                                .ForMember(d => d.VERSICHERUNGSSTEUER, opt => opt.Ignore())
                                                .ForMember(d => d.PRAEMIENETTO, opt => opt.Ignore())
                                                .ForMember(d => d.ZUBEHOERFINANZIERT, opt => opt.Ignore())
                                                .ForMember(d => d.FREMDVERSICHERUNG, opt => opt.Ignore())
                                                .ForMember(d => d.POLKENNZEICHEN, opt => opt.Ignore())
                                                .ForMember(d => d.SB1, opt => opt.Ignore())
                                                .ForMember(d => d.SB2, opt => opt.Ignore())
                                                .ForMember(d => d.ANGVAR, opt => opt.Ignore())                                                
                                                .ForMember(d => d.VSTYP, opt => opt.Ignore())                                                
                                                .ForMember(d => d.SYSANGEBOT, opt => opt.Ignore())
                                                .ForMember(d => d.CODE, opt => opt.Ignore())
                                                .ForMember(d => d.PPY, opt => opt.Ignore())
                                                .ForMember(d => d.LZ, opt => opt.Ignore());

            Config.CreateMap<ANGVAR, AngAntVarDto>().ForMember(d => d.sysangebot, opt => opt.Ignore())
                                                    .ForMember(d => d.kalkulation, opt => opt.Ignore());
            Config.CreateMap<ANGKALK, AngAntKalkDto>().ForMember(d => d.sysangvar, opt => opt.Ignore())
                                                      .ForMember(d => d.sysantrag, opt => opt.Ignore())
                                                      .ForMember(d => d.sysprproduct, opt => opt.Ignore())
                                                      .ForMember(d => d.sysobusetype, opt => opt.Ignore())
                                                      .ForMember(d => d.calcZinskosten, opt => opt.Ignore())
                                                      .ForMember(d => d.calcRsvgesamt, opt => opt.Ignore())
                                                      .ForMember(d => d.calcRsvmonat, opt => opt.Ignore())
                                                      .ForMember(d => d.calcRsvzins, opt => opt.Ignore())
                                                      .ForMember(d => d.calcRsvmonatMin, opt => opt.Ignore())
                                                      .ForMember(d => d.calcRsvmonatMax, opt => opt.Ignore())
                                                      .ForMember(d => d.calcZinskostenMin, opt => opt.Ignore())
                                                      .ForMember(d => d.calcZinskostenMax, opt => opt.Ignore())
                                                      .ForMember(d => d.calcUstzins, opt => opt.Ignore());
            Config.CreateMap<ANGPROV, Cic.OpenOne.Common.DTO.AngAntProvDto>().ForMember(d => d.sysantrag, opt => opt.Ignore())
                                                      .ForMember(d => d.sysprprovtype, opt => opt.Ignore())
                                                      .ForMember(d => d.syspartner, opt => opt.Ignore());
            Config.CreateMap<ANGSUBV, Cic.OpenOne.Common.DTO.AngAntSubvDto>().ForMember(d => d.sysantsubv, opt => opt.Ignore())
                                                      .ForMember(d => d.sysangvar, opt => opt.Ignore())
                                                      .ForMember(d => d.sysantrag, opt => opt.Ignore())
                                                      .ForMember(d => d.syssubvtyp, opt => opt.Ignore());
            Config.CreateMap<ANGVS, Cic.OpenOne.Common.DTO.AngAntVsDto>().ForMember(d => d.sysantvs, opt => opt.Ignore())
                                                  .ForMember(d => d.sysangvar, opt => opt.Ignore())
                                                  .ForMember(d => d.sysantrag, opt => opt.Ignore())
                                                  .ForMember(d => d.sysvstyp, opt => opt.Ignore())
                                                  .ForMember(d => d.sysvs, opt => opt.Ignore());

            },true);
        }

        public static Conversion Create ()
        {
            if (Instance == null)
            {
                Instance = new Conversion();
            }
            return Instance;
        }

        public T2 Convert<T1, T2>(T1 Source)
        {
            try
            {
                return Mapping.Map<T1, T2>(Source);
            }
            catch (AutoMapperMappingException e)
            {
                throw new Exception("Mapping Exception", e);
            }
        }

        public void Convert<T1, T2>(T1 Source, T2 Dest)
        {
            try
            {
                Mapping.Map<T1, T2>(Source, Dest);
            }
            catch (AutoMapperMappingException e)
            {
                throw new Exception("Mapping Exception", e);
            }
        }
    }
}
