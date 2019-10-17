using System;
using System.Globalization;
using System.Reflection;
using System.Timers;
using AutoMapper;
using System.Linq;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Mapper;
using System.Data.Objects.DataClasses;
using Cic.OpenOne.Common.Util;


namespace Cic.One.DTO
{
    public class MappingProfileBase : Profile
    {
        /// <summary>
        /// Konfigurieren
        /// </summary>
        protected override void Configure()
        {
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
            CreateMap<bool, short?>().ConvertUsing<GenericNullableConverter<bool, short?>>();
            CreateMap<bool, long?>().ConvertUsing<GenericNullableConverter<bool, long?>>();
            CreateMap<DateTime, DateTime?>().ConvertUsing<GenericNullableConverter<DateTime, DateTime?>>();

            CreateMap<decimal?, float>().ConvertUsing<GenericNullableConverter<decimal?, float>>();
            CreateMap<float, decimal?>().ConvertUsing<GenericNullableConverter<float, decimal?>>();
            CreateMap<decimal, int?>().ConvertUsing<GenericNullableConverter<decimal, int?>>();
            CreateMap<int?, decimal>().ConvertUsing<GenericNullableConverter<int?, decimal>>();

            CreateMap<double, decimal?>().ConvertUsing<GenericNullableConverter<double, decimal?>>();
            CreateMap<decimal?, double>().ConvertUsing<GenericNullableConverter<decimal?, double>>();

            CreateMap<double, int?>().ConvertUsing<GenericNullableConverter<double, int?>>();
            CreateMap<int?, double>().ConvertUsing<GenericNullableConverter<int?, double>>();


            CreateMap<DateTime, DateTime?>().ConvertUsing<GenericNullableConverter<DateTime, DateTime?>>();

        }

        
        private bool IsSpecified(object ob)
        {
            if (ob == null)
                return false;
            else
                return true;
        }


        protected class CustomBoolResolver : ValueResolver<bool, String>
        {
            protected override string ResolveCore(bool source)
            {
                return (source == true ? "1" : "0");
            }
        }

        protected class CustomToBoolResolver : ValueResolver<String, bool>
        {
            protected override bool ResolveCore(String source)
            {
                if (source == null)
                    return false;
                return (source == "1" ? true : false);
            }
        }


        protected class CustomStringArrayResolver : ValueResolver<string[], String>
        {
            protected override string ResolveCore(string[] source)
            {
                return source.ToString();
            }
        }

        protected class CustomStringDateResolver : ValueResolver<string, DateTime?>
        {
            protected override DateTime? ResolveCore(string source)
            {
                if (source == null)
                    return null;
                else
                {
                    DateTime result;
                    return DateTime.TryParse(source, out result) ? (DateTime?)result : null;
                }
            }
        }


        protected class CustomStringIntResolver : ValueResolver<string, int?>
        {
            protected override int? ResolveCore(string source)
            {
                if (source == null)
                    return null;
                else
                {
                    int number;

                    if (Int32.TryParse(source, out number))
                        return number;
                    else
                        return null;

                }
            }
        }

        protected class CustomClarionB2bTimeResolver : ValueResolver<long?, DateTime>
        {
            protected override DateTime ResolveCore(long? source)
            {
                if (source == null || source == 0)
                    return DateTimeHelper.DeliverMinDateForJava();
                int clariontime = (int)source;
                return DateTimeHelper.ClarionTimeToDateTime(clariontime);


            }
        }

        protected class CustomB2BClarionTimeResolver : ValueResolver<DateTime?, long?>
        {

            protected override long? ResolveCore(DateTime? source)
            {

                int? clariontime = DateTimeHelper.DateTimeToClarionTime(source);
                if (clariontime == null)
                    return null;
                return (long)clariontime.Value;

            }
        }

        protected class CustomDoubleLongResolver : ValueResolver<Double, long>
        {
            protected override long ResolveCore(Double source)
            {
                long x = (long)source;
                return x;
            }
        }


        public static bool IsNull<T>(T value)
        {
            if (value is ValueType)
            {
                return false;
            }
            return null == (object)value;
        }


        public long resolveEntityKey(String s)
        {
            if (s != null)
                return long.Parse(s);
            return 0;
        }


        /// <summary>
        /// Gibt die Sysid einer EntityReferenz zurück
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityReference"></param>
        /// <returns></returns>
        protected long SysidFromReference<T>(EntityReference<T> entityReference) where T : class
        {
            return entityReference.EntityKey == null ? 0 : long.Parse(entityReference.EntityKey.EntityKeyValues.ElementAt(0).Value.ToString());
        }

     

    }
}