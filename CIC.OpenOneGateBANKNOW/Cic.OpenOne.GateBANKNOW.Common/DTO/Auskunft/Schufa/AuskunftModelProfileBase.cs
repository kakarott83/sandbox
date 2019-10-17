namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    using System;
    using AutoMapper;
    using Cic.OpenOne.Common.Util.Mapper;

    public class AuskunftModelProfileBase : Profile
    {
        /// <summary>
        /// Konfigurieren
        /// </summary>
        public AuskunftModelProfileBase()
        {
            CreateMap<double, decimal?>().ConvertUsing<DoubleDecimalConverter>();
            CreateMap<double, double?>().ConvertUsing<GenericNullableConverter<double, double?>>();
            CreateMap<double?, decimal?>().ConvertUsing<GenericNullableConverter<double?, decimal?>>();
            CreateMap<double?, decimal>().ConvertUsing<GenericNullableConverter<double?, decimal>>();
            CreateMap<double?, long?>().ConvertUsing<GenericNullableConverter<double?, long?>>();
            CreateMap<double?, long>().ConvertUsing<GenericNullableConverter<double?, long>>();

            CreateMap<decimal, double?>().ConvertUsing<GenericNullableConverter<decimal, double?>>();
            CreateMap<decimal?, double?>().ConvertUsing<GenericNullableConverter<decimal?, double?>>();
            CreateMap<decimal?, float>().ConvertUsing<GenericNullableConverter<decimal?, float>>();
            CreateMap<decimal, int?>().ConvertUsing<GenericNullableConverter<decimal, int?>>();
            CreateMap<decimal, long?>().ConvertUsing<GenericNullableConverter<decimal, long?>>();

            CreateMap<long?, double?>().ConvertUsing<GenericNullableConverter<long?, double?>>();
            CreateMap<long?, int>().ConvertUsing<GenericNullableConverter<long?, int>>();
            CreateMap<long, long?>().ConvertUsing<GenericNullableConverter<long, long?>>();
            CreateMap<long, int?>().ConvertUsing<GenericNullableConverter<long, int?>>();
            CreateMap<long?, decimal?>().ConvertUsing<GenericNullableConverter<long?, decimal?>>();
            CreateMap<long, decimal?>().ConvertUsing<GenericNullableConverter<long, decimal?>>();

            CreateMap<sbyte?, decimal?>().ConvertUsing<GenericNullableConverter<sbyte?, decimal?>>();
            CreateMap<sbyte, decimal?>().ConvertUsing<GenericNullableConverter<sbyte, decimal?>>();

            CreateMap<int, int?>().ConvertUsing<GenericNullableConverter<int, int?>>();
            CreateMap<int, long?>().ConvertUsing<GenericNullableConverter<int, long?>>();
            CreateMap<int?, bool>().ConvertUsing<GenericNullableConverter<int?, bool>>();
            CreateMap<int?, decimal>().ConvertUsing<GenericNullableConverter<int?, decimal>>();
            CreateMap<int, decimal?>().ConvertUsing<GenericNullableConverter<int, decimal?>>();
            CreateMap<int, long>().ConvertUsing<GenericNullableConverter<int, long>>();

            CreateMap<short, short?>().ConvertUsing<GenericNullableConverter<short, short?>>();
            CreateMap<short, int?>().ConvertUsing<GenericNullableConverter<short, int?>>();
            CreateMap<short, long?>().ConvertUsing<GenericNullableConverter<short, long?>>();

            CreateMap<float, float?>().ConvertUsing<GenericNullableConverter<float, float?>>();
            CreateMap<float, double?>().ConvertUsing<GenericNullableConverter<float, double?>>();
            CreateMap<float, decimal?>().ConvertUsing<GenericNullableConverter<float, decimal?>>();

            CreateMap<bool, bool?>().ConvertUsing<GenericNullableConverter<bool, bool?>>();
            CreateMap<bool, int?>().ConvertUsing<GenericNullableConverter<bool, int?>>();
            CreateMap<bool, short?>().ConvertUsing<GenericNullableConverter<bool, short?>>();
            CreateMap<bool, long?>().ConvertUsing<GenericNullableConverter<bool, long?>>();
            CreateMap<string[], string>().ConvertUsing<GenericNullableConverter<string[], string>>();

            CreateMap<DateTime, DateTime?>().ConvertUsing<GenericNullableConverter<DateTime, DateTime?>>();
        }
    }
}