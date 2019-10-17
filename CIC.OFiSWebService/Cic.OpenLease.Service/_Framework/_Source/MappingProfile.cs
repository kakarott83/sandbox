using AutoMapper;
using Cic.One.DTO.Mediator;
using Cic.OpenOne.Common.Util.Mapper;
using CIC.Database.OL.EF6.Model;
using System;
using System.Linq;

namespace Cic.OpenLease.Service
{

    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }

        /*public static IMappingExpression<TSource, TDestination>IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps = Mapper.GetAllTypeMaps().First(x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }*/
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var typeMap = Mapper.Configuration.FindTypeMapFor<TSource, TDestination>();
            if (typeMap != null)
            {
                foreach (var unmappedPropertyName in typeMap.GetUnmappedPropertyNames())
                {
                    try
                    {
                        expression.ForMember(unmappedPropertyName, opt => opt.Ignore());
                    }catch(Exception ){}
                }
            }

            return expression;
        }
    }

    
    public class MappingProfile:Profile
    {

        
 /*
            /// <summary>
            /// Gibt die Sysid einer EntityReferenz zurück
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="entityReference"></param>
            /// <returns></returns>
           public long SysidFromReference<T>(EntityReference<T> entityReference) where T : class
            {
                return entityReference.EntityKey == null ? 0 : long.Parse(entityReference.EntityKey.EntityKeyValues.ElementAt(0).Value.ToString());
            }*/

            /// <summary>
            /// Konfigurieren
            /// </summary>
            public MappingProfile()
            {

            CreateMap<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto, QueueDto>();
            CreateMap<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto, QueueRecordDto>();
            CreateMap<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto, QueueRecordValueDto>();

            // Attention:  A piece of BankNowModelProfile is in Cic.OpenOne.GateBANKNOW.Common.DTO.BankNowModelProfile4UI, if you change something here, you will also have to change it there too. 
            CreateMap<double, decimal?>().ConvertUsing<DoubleDecimalConverter>();
                CreateMap<double, double?>().ConvertUsing<GenericNullableConverter<double, double?>>();
                CreateMap<decimal, double?>().ConvertUsing<GenericNullableConverter<decimal, double?>>();
                CreateMap<decimal?, double?>().ConvertUsing<GenericNullableConverter<decimal?, double?>>();
                CreateMap<long?, double?>().ConvertUsing<GenericNullableConverter<long?, double?>>();
                CreateMap<double?, decimal?>().ConvertUsing<GenericNullableConverter<double?, decimal?>>();
                CreateMap<double?, decimal>().ConvertUsing<GenericNullableConverter<double?, decimal>>();
                CreateMap<int, int?>().ConvertUsing<GenericNullableConverter<int, int?>>();
                CreateMap<int, long?>().ConvertUsing<GenericNullableConverter<int, long?>>();
                CreateMap<long?, int>().ConvertUsing<GenericNullableConverter<long?, int>>();

                CreateMap<short, short?>().ConvertUsing<GenericNullableConverter<short, short?>>();
                CreateMap<short, int?>().ConvertUsing<GenericNullableConverter<short, int?>>();
                CreateMap<short, long?>().ConvertUsing<GenericNullableConverter<short, long?>>();

                CreateMap<long, long?>().ConvertUsing<GenericNullableConverter<long, long?>>();
                CreateMap<long, int?>().ConvertUsing<GenericNullableConverter<long, int?>>();

                CreateMap<float, float?>().ConvertUsing<GenericNullableConverter<float, float?>>();
                CreateMap<float, double?>().ConvertUsing<GenericNullableConverter<float, double?>>();

                CreateMap<bool, bool?>().ConvertUsing<GenericNullableConverter<bool, bool?>>();
                CreateMap<bool, int?>().ConvertUsing<GenericNullableConverter<bool, int?>>();
                CreateMap<int?, bool>().ConvertUsing<GenericNullableConverter<int?, bool>>();
                CreateMap<int, bool?>().ConvertUsing<GenericNullableConverter<int, bool?>>();
                CreateMap<bool, short?>().ConvertUsing<GenericNullableConverter<bool, short?>>();
                CreateMap<bool, long?>().ConvertUsing<GenericNullableConverter<bool, long?>>();
                CreateMap<DateTime, DateTime?>().ConvertUsing<GenericNullableConverter<DateTime, DateTime?>>();

                CreateMap<decimal?, float>().ConvertUsing<GenericNullableConverter<decimal?, float>>();
                CreateMap<float, decimal?>().ConvertUsing<GenericNullableConverter<float, decimal?>>();
                CreateMap<decimal, int?>().ConvertUsing<GenericNullableConverter<decimal, int?>>();
                CreateMap<int?, decimal>().ConvertUsing<GenericNullableConverter<int?, decimal>>();

                // Null nicht durch Default-Wert ersetzen, sondern weitergeben (Zek)
                CreateMap<long?, int?>().ForAllMembers(opt => opt.NullSubstitute(null));
                CreateMap<long?, int?>().ConvertUsing<GenericNullableConverter<long?, int?>>();

                CreateMap<CIC.Database.PRISMA.EF6.Model.PRPRODUCT, Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto>();
                CreateMap<Cic.OpenOne.Common.DTO.Prisma.ParamDto, Cic.OpenLease.ServiceAccess.DdOl.PRPARAMDto>()
                    .ForMember(a => a.PRFLDOBJECTMETA, opt => opt.MapFrom(s => s.meta))
                    .ForMember(a => a.SYSPRPARAM, opt => opt.MapFrom(s => s.sysID))
                    .ForMember(a => a.TYP, opt => opt.MapFrom(s => s.type));


                
               CreateMap<ANGEBOT, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>()
                       .ForMember(dest => dest.antrag, o => o.MapFrom(src => src.ANGEBOT1))
                       .ForMember(dest => dest.einzug, o => o.MapFrom(src => src.EINZUG))
                       .ForMember(dest => dest.sysit, o => o.MapFrom(src => src.SYSIT))
                       .ForMember(dest => dest.sysprprod, o => o.MapFrom(src => src.SYSPRPRODUCT))
                       .ForMember(dest => dest.sysvart, o => o.MapFrom(src => src.SYSVART))
                       .ForMember(dest => dest.sysVM, o => o.MapFrom(src => src.SYSVP))
                       .ForMember(dest => dest.sysVK, o => o.MapFrom(src => src.SYSBERATADDB))
                       .ForMember(dest => dest.sysLS, o => o.MapFrom(src => src.SYSLS))
                       .ForMember(dest => dest.ppy, o => o.MapFrom(src => src.PPY))
                       .ForMember(dest => dest.vertriebsweg, o => o.MapFrom(src => src.VERTRIEBSWEG))
                       .ForMember(dest => dest.syswfuser, o => o.MapFrom(src => src.SYSWFUSER))
                       .ForMember(dest => dest.sysprchannel, o => o.MapFrom(src => src.SYSPRCHANNEL))
                    //.ForMember(dest => dest.mandatreferenz, o => o.MapFrom(src => src.MANDATREFERENZ))
                       .ForMember(dest => dest.syskd, o => o.MapFrom(src => src.SYSKD))
                       .ForMember(dest => dest.sysid, o => o.Ignore())
                       .ForMember(dest => dest.sysKI, o => o.MapFrom(src => src.SYSKI))

                       ;


                 CreateMap< ANGOB, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto>()
                     .ForMember(dest => dest.sysob, o => o.Ignore())
                     .ForMember(dest => dest.sysantrag, o => o.Ignore())
                     .ForMember(dest => dest.sysangebot, o => o.Ignore())
                     
                        .ForMember(dest => dest.ahk, o => o.MapFrom(src => src.AHK))
                        .ForMember(dest => dest.ahkBrutto, o => o.MapFrom(src => src.AHKBRUTTO))
                        .ForMember(dest => dest.ahkExternBrutto, o => o.MapFrom(src => src.AHKEXTERNBRUTTO))
                        .ForMember(dest => dest.automatik, o => o.MapFrom(src => src.AUTOMATIK))
                        .ForMember(dest => dest.fabrikat, o => o.MapFrom(src => src.FABRIKAT))
                        .ForMember(dest => dest.farbeA, o => o.MapFrom(src => src.FARBEA))
                        .ForMember(dest => dest.grund, o => o.MapFrom(src => src.GRUND))
                        .ForMember(dest => dest.grundBrutto, o => o.MapFrom(src => src.GRUNDBRUTTO))
                        .ForMember(dest => dest.hersteller, o => o.MapFrom(src => src.HERSTELLER))
                        .ForMember(dest => dest.herzub, o => o.MapFrom(src => src.HERZUB))
                        .ForMember(dest => dest.herzubBrutto, o => o.MapFrom(src => src.HERZUBBRUTTO))
                        .ForMember(dest => dest.herzubExternBrutto, o => o.MapFrom(src => src.HERZUBEXTERNBRUTTO))

                         .ForMember(dest => dest.sonzub, o => o.MapFrom(src => src.SONZUB))
                        .ForMember(dest => dest.sonzubBrutto, o => o.MapFrom(src => src.SONZUBBRUTTO))
                        .ForMember(dest => dest.sonzubExternBrutto, o => o.MapFrom(src => src.SONZUBEXTERNBRUTTO))

                        //HCE new fields
                        /*.ForMember(dest => dest.ueberfuehrung, o => o.MapFrom(src => src.UEBERFUEHRUNG))
                        .ForMember(dest => dest.ueberfuehrungUst, o => o.MapFrom(src => src.UEBERFUEHRUNGUST))
                        .ForMember(dest => dest.ueberfuehrungBrutto, o => o.MapFrom(src => src.UEBERFUEHRUNGBRUTTO))

                        .ForMember(dest => dest.zulassung, o => o.MapFrom(src => src.ZULASSUNG))
                        .ForMember(dest => dest.zulassungUst, o => o.MapFrom(src => src.ZULASSUNGUST))
                        .ForMember(dest => dest.zulassungBrutto, o => o.MapFrom(src => src.ZULASSUNGBRUTTO))*/

                        .ForMember(dest => dest.jahresKm, o => o.MapFrom(src => src.JAHRESKM))
                        .ForMember(dest => dest.kmtoleranz, o => o.MapFrom(src => src.KMTOLERANZ))
                        .ForMember(dest => dest.paketeBrutto, o => o.MapFrom(src => src.PAKETEBRUTTO))
                        .ForMember(dest => dest.pakete, o => o.MapFrom(src => src.PAKETE))
                        .ForMember(dest => dest.paketeExternBrutto, o => o.MapFrom(src => src.PAKETEEXTERNBRUTTO))
                        .ForMember(dest => dest.schwacke, o => o.MapFrom(src => src.SCHWACKE))
                        .ForMember(dest => dest.serie, o => o.MapFrom(src => src.SERIE))
                        .ForMember(dest => dest.baujahr, o => o.MapFrom(src => src.BAUJAHR))
                        .ForMember(dest => dest.baumonat, o => o.MapFrom(src => src.BAUMONAT))
                        .ForMember(dest => dest.satzmehrKm, o => o.MapFrom(src => src.SATZMEHRKM))
                        .ForMember(dest => dest.satzmehrKmBrutto, o => o.MapFrom(src => src.SATZMEHRKMBRUTTO))
                        .ForMember(dest => dest.satzmehrKm, o => o.MapFrom(src => src.SATZMINDERKM))
                        .ForMember(dest => dest.satzminderKmBrutto, o => o.MapFrom(src => src.SATZMINDERKMBRUTTO))
                        .ForMember(dest => dest.sysobart, o => o.MapFrom(src => src.SYSOBART))
                        .ForMember(dest => dest.sysobtyp, o => o.MapFrom(src => src.SYSOBTYP))
                        .ForMember(dest => dest.zubehoer, o => o.MapFrom(src => src.ZUBEHOER))
                        .ForMember(dest => dest.zubehoerBrutto, o => o.MapFrom(src => src.ZUBEHOERBRUTTO))
                        .ForMember(dest => dest.fzart, o => o.MapFrom(src => src.FZART))
                        .ForMember(dest => dest.zubehoerUst, o => o.MapFrom(src => src.ZUBEHOERUST))
                        .ForMember(dest => dest.rwUst, o => o.MapFrom(src => src.RWUST))
                        .ForMember(dest => dest.rwBrutto, o => o.MapFrom(src => src.RWBRUTTO))
                        ;

               
               CreateMap< ANGKALK, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>()
                   .ForMember(dest => dest.sysangvar, o => o.Ignore())
                   .ForMember(dest => dest.sysantrag, o => o.Ignore())
                   .ForMember(dest => dest.syskalk, o => o.Ignore())
                        .ForMember(dest => dest.bgextern, o => o.MapFrom(src => src.BGEXTERN))
                        .ForMember(dest => dest.bgexternbrutto, o => o.MapFrom(src => src.BGEXTERNBRUTTO))
                        .ForMember(dest => dest.bgexternust, o => o.MapFrom(src => src.BGEXTERNUST))
                        .ForMember(dest => dest.bgintern, o => o.MapFrom(src => src.BGINTERN))
                        .ForMember(dest => dest.depot, o => o.MapFrom(src => src.DEPOT))
                        .ForMember(dest => dest.gesamtBrutto, o => o.MapFrom(src => src.GESAMTBRUTTO))
                        .ForMember(dest => dest.gesamtkostenBrutto, o => o.MapFrom(src => src.GESAMTKOSTENBRUTTO))
                        .ForMember(dest => dest.lz, o => o.MapFrom(src => src.LZ))
                        .ForMember(dest => dest.ppy, o => o.MapFrom(src => src.PPY))
                        .ForMember(dest => dest.rate, o => o.MapFrom(src => src.RATE))
                        .ForMember(dest => dest.rateBrutto, o => o.MapFrom(src => src.RATEBRUTTO))
                        .ForMember(dest => dest.rateUst, o => o.MapFrom(src => src.RATEUST))
                        .ForMember(dest => dest.rw, o => o.MapFrom(src => src.RW))
                        .ForMember(dest => dest.rwBrutto, o => o.MapFrom(src => src.RWBRUTTO))
                        .ForMember(dest => dest.rwUst, o => o.MapFrom(src => src.RWUST))
                        .ForMember(dest => dest.sz, o => o.MapFrom(src => src.SZ))
                        .ForMember(dest => dest.szBrutto, o => o.MapFrom(src => src.SZBRUTTO))
                        .ForMember(dest => dest.szUst, o => o.MapFrom(src => src.SZUST))
                        .ForMember(dest => dest.verrechnung, o => o.MapFrom(src => src.VERRECHNUNG))
                        .ForMember(dest => dest.zins, o => o.MapFrom(src => src.ZINS))
                        .ForMember(dest => dest.zinseff, o => o.MapFrom(src => src.ZINSEFF))
                        .ForMember(dest => dest.modus, o => o.MapFrom(src => src.MODUS))
                        .ForMember(dest => dest.verrechnungFlag, o => o.MapFrom(src => src.VERRECHNUNGFLAG))
                        .ForMember(dest => dest.auszahlung, o => o.UseValue(0))
                        .ForMember(dest => dest.sysobusetype, o => o.UseValue(0))
                        .ForMember(dest => dest.bginternbrutto, o => o.MapFrom(src => src.BGINTERNBRUTTO))
                        

                        ;

                
                CreateMap< ANGOBINI, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto>()
                    .ForMember(dest => dest.sysid, o => o.Ignore())
                        //.ForMember(dest => dest.kw, o => o.Ignore())
                        .ForMember(dest => dest.ps, o => o.Ignore())
                        .ForMember(dest => dest.hubraum, o => o.MapFrom(src => src.CCM))
                        .ForMember(dest => dest.treibstoff, o => o.MapFrom(src => src.MOTORFUEL))
                        .ForMember(dest => dest.kw, o => o.MapFrom(src => src.KW))
                        
                        ;

                //MAP IT to PKZ
                CreateMap<Cic.OpenLease.ServiceAccess.DdOl.ITDto, Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto>()
                    .ForMember(dest => dest.anzkinder, o => o.MapFrom(src => src.KINDERIMHAUS))
                    .ForMember(dest => dest.familienstand, o => o.MapFrom(src => src.FAMILIENSTAND))
                    .ForMember(dest => dest.beruflichCode, o => o.MapFrom(src => src.BERUF))
                    .ForMember(dest => dest.nameAg1, o => o.MapFrom(src => src.NAMEAG))
                    .ForMember(dest => dest.beschseitAg1, o => o.MapFrom(src => src.BESCHSEITAG))
                    .ForMember(dest => dest.beschbisAg1, o => o.MapFrom(src => src.BESCHBISAG))
                    .ForMember(dest => dest.strasseAg1, o => o.MapFrom(src => src.STRASSEAG))
                    .ForMember(dest => dest.hsnrAg1, o => o.MapFrom(src => src.HSNRAG))
                    .ForMember(dest => dest.plzAg1, o => o.MapFrom(src => src.PLZAG))
                    .ForMember(dest => dest.ortAg1, o => o.MapFrom(src => src.ORTAG))
                    .ForMember(dest => dest.syslandAg1, o => o.MapFrom(src => src.SYSLANDAG))
                    .ForMember(dest => dest.einknetto, o => o.MapFrom(src => src.EINKNETTO))
                    .ForMember(dest => dest.zeinknetto, o => o.MapFrom(src => src.ZEINKNETTO))
                    .ForMember(dest => dest.kredtrate, o => o.MapFrom(src => src.KREDRATE1))
                    .ForMember(dest => dest.miete, o => o.MapFrom(src => src.MIETE))
                    .ForMember(dest => dest.nebeinknetto, o => o.MapFrom(src => src.NEBENEINKNETTO))
                    .ForMember(dest => dest.wohnungart, o => o.MapFrom(src => src.WOHNUNGART))
                    .ForMember(dest => dest.wohnverhCode, o => o.MapFrom(src => src.WOHNVERH))
                    .ForMember(dest => dest.einknettoFlag, o => o.UseValue(true))
                    ;
                CreateMap<Cic.OpenLease.ServiceAccess.DdOl.ITDto, Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto>();
                
            }
        
    }
}