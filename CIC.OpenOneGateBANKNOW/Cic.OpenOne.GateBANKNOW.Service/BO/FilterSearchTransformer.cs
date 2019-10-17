using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Search;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
    /// <summary>
    /// FilterSearchTransformer-Klasse
    /// </summary>
    public class FilterSearchTransformer
    {
        /// <summary>
        /// filterZustandTransformer
        /// </summary>
        /// <param name="isearchDto"></param>
        /// <returns></returns>
        public iSearchDto filterZustandTransformer(iSearchDto isearchDto)
        {
            List<Filter> list = new List<Filter>();
            if (isearchDto.filters.Count() > 0)
            {
                Filter attribut = new Filter();
                attribut.fieldname = "";
                foreach (Filter fp in isearchDto.filters)
                {
                    if (fp.value == null) continue;
                    if (fp.fieldname.ToUpper().Equals("ANGEBOT.ZUSTAND"))
                    {
                        if (fp.value.ToString().Equals("Gültig"))
                        {
                            fp.value = "Neu";
                            attribut.fieldname = "ANGEBOT.ATTRIBUT";
                            attribut.value = "Neu/gültig";
                            attribut.filterType = fp.filterType;
                        }
                        else
                            if (fp.value.ToString().Equals("Abgelaufen"))
                            {
                                fp.fieldname = "ANGEBOT.ATTRIBUT";

                            }
                            else
                                if (fp.value.ToString().Equals("Antrag eingereicht"))
                                {
                                    fp.value = "Abgeschlossen";
                                    attribut.fieldname = "ANGEBOT.ATTRIBUT";
                                    attribut.value = "Antrag eingereicht";
                                    attribut.filterType = fp.filterType;
                                }
                                else
                                    if (fp.value.ToString().Equals("Gedruckt"))
                                    {
                                        attribut.fieldname = "ANGEBOT.ATTRIBUT";
                                        attribut.value = "Gedruckt/gültig";
                                        attribut.filterType = fp.filterType;
                                    }
                    }
                }

                if (!attribut.fieldname.Equals(""))
                {
                    list = isearchDto.filters.ToList();
                    list.Add(attribut);
                    isearchDto.filters = list.ToArray();
                }
            }
            return isearchDto;
        }
    }
}