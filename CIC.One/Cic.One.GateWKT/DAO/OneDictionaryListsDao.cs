using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.DAO;
using CIC.Database.OL.EF4.Model;

namespace Cic.One.GateWKT.DAO
{
    /// <summary>
    /// Data Access Object for dictionary lists
    /// </summary>
    public class OneDictionaryListsDao : DictionaryListsDao
    {
       
        private static string LANDQUERY = "select * from cic.land where activeflag=1 order by countryname";
        private static string LANGQUERY = "select * from cic.ctlang where activeflag=1 order by languagename";


        /// <summary>
        /// Delivers a list of countrys (Länder)
        /// </summary>
        /// <returns>list of countrys</returns>
        override public DropListDto[] deliverLAND()
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();

            
            using (DdOlExtended olCtx = new DdOlExtended())
            {
                

                List<LAND> values = olCtx.ExecuteStoreQuery<LAND>(LANDQUERY, null).ToList();
                LAND de = values.Where(a => a.ISO!=null && a.ISO.ToUpper().Equals("DE")).FirstOrDefault();
                if (de != null)
                {
                    values.Remove(de);
                    values.Insert(0, de);
                }

                foreach (LAND land in values)
                {
                    
                    if (land.ACTIVEFLAG.HasValue && land.ACTIVEFLAG.Value == 1)
                    {
                        dropListDtoList.Add(
                            new DropListDto()
                            {

                                sysID = (long)land.SYSLAND,
                                code = land.ISO,
                                beschreibung = land.COUNTRYNAME,
                                bezeichnung = land.COUNTRYNAME
                            });
                    }
                }
            }
            return dropListDtoList.ToArray();
        }

        
        /// <summary>
        /// Delivers a list of languages
        /// </summary>
        /// <returns>list of languages</returns>
        override public DropListDto[] deliverCTLANG()
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();

            
            using (DdOwExtended owCtx = new DdOwExtended())
            {
            

                List<CTLANG> langs = owCtx.ExecuteStoreQuery<CTLANG>(LANGQUERY, null).ToList();
                //var query = from CTLANG in owCtx.CTLANG select CTLANG;
                foreach (CTLANG ctlang in langs)
                {
                    if (ctlang.ACTIVEFLAG == 1)
                    {
                        dropListDtoList.Add(
                            new DropListDto()
                            {
                                sysID = (long)ctlang.SYSCTLANG,
                                code = ctlang.ISOCODE,
                                beschreibung = ctlang.ISOCODE,
                                bezeichnung = ctlang.LANGUAGENAME
                            });
                    }
                }
            }
            return dropListDtoList.ToArray();
        }

        

       


    }
}