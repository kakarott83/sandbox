using System;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using AutoMapper.Configuration;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Mapper for generic search results to map the containing result array types
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class SearchResultMapper<S, T>
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchResultMapper()
        {
        }

        /// <summary>
        /// Map Search Result 
        /// </summary>
        /// <param name="source">source</param>
        /// <returns>Search Dto</returns>
        public oSearchDto<T> mapSearchResult(oSearchDto<S> source)
        {
            oSearchDto<T> rval = new oSearchDto<T>();

            rval.searchCountFiltered = source.searchCountFiltered;
            rval.searchCountMax = source.searchCountMax;
            rval.searchNumPages = source.searchNumPages;

            if (source.results != null)
            {
                rval.results = new T[source.results.Length];
                //changed from profile -> just add the typemapping to the basic profile!
                IMapper mapper = Mapper.Instance;/* Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper(typeof(S)+"_"+typeof(T), delegate (MapperConfigurationExpression cfg) {
                    cfg.CreateMap<S, T>();
                });*/
                
                int i = 0;
                foreach (S k in source.results)
                {
                    rval.results[i++] = mapper.Map<S, T>(k);

                }
            }
            return rval;
        }

        /// <summary>
        /// Mappe ein Suchergebnis eines Interessenten
        /// </summary>
        /// <param name="source">Quelle</param>
        /// <param name="fkey">Schlüssel</param>
        /// <param name="dto">DTO</param>
        /// <param name="kundeBo">Kundendaten</param>
        /// <returns>Suchdaten</returns>
        public oSearchDto<T> mapSearchResultIT(oSearchDto<S> source, string fkey, string dto, IKundeBo kundeBo)
        {
            oSearchDto<T> rval = new oSearchDto<T>();

            rval.searchCountFiltered = source.searchCountFiltered;
            rval.searchCountMax = source.searchCountMax;
            rval.searchNumPages = source.searchNumPages;

            if (source.results != null)
            {
                rval.results = new T[source.results.Length];

                IMapper mapper = Mapper.Instance;/* Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("mapSearchResultIT_"+typeof(S) + "_" + typeof(T), delegate (MapperConfigurationExpression cfg) {
                    cfg.CreateMap<S, T>();
                    cfg.CreateMap(typeof(KundeDto), rval.results.GetType().GetElementType().GetProperty(dto).PropertyType);
                });*/

                

                int i = 0;
                object id = 0;

                foreach (S k in source.results)
                {
                    rval.results[i] = mapper.Map<S, T>(k);
                    if (k.GetType().GetProperty(fkey) != null && k.GetType().GetProperty(fkey).GetValue(k, null) != null && (long?)k.GetType().GetProperty(fkey).GetValue(k, null) != 0)
                    {
                        double start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                        object obj = mapper.Map(kundeBo.getKunde((long)k.GetType().GetProperty(fkey).GetValue(k, null)), typeof(KundeDto), rval.results[i].GetType().GetProperty(dto).PropertyType);

                        rval.results[i].GetType().GetProperty(dto).SetValue(rval.results[i], obj, null);
                        _log.Debug("Duration Map IT: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                    }
                    i++;
                }
            }
            else
                rval.results = new T[0];

            return rval;
        }

        /// <summary>
        /// setStatusEPOS
        /// </summary>
        /// <param name="source"></param>
        public void setStatusEPOS(oSearchDto<S> source)
        {
            foreach (S obj in source.results)
            {
                String zustand = (String)obj.GetType().GetProperty("zustand").GetValue(obj, null);
                String attribut = (String)obj.GetType().GetProperty("attribut").GetValue(obj, null);

                if (zustand != null && zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Neu)) && attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.NeuGueltig)))
                    obj.GetType().GetProperty("zustand").SetValue(obj, Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Gueltig), null);
                else if (zustand != null && zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Neu)) && attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Abgelaufen)))
                    obj.GetType().GetProperty("zustand").SetValue(obj, Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Abgelaufen), null);
                else if (zustand != null && zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Gedruckt)) && attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.GedrucktGueltig)))
                    obj.GetType().GetProperty("zustand").SetValue(obj, Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Gedruckt), null);
                else if (zustand != null && zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Gedruckt)) && attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Abgelaufen)))
                    obj.GetType().GetProperty("zustand").SetValue(obj, Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Abgelaufen), null);
                else if (zustand != null && zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Abgeschlossen)) && attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Antrageingereicht)))
                    obj.GetType().GetProperty("zustand").SetValue(obj, Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Antrageingereicht), null);
            }
        }
    }
}