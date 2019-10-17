using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
using Cic.OpenOne.Common.Model.DdOiqueue;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cic.OpenLease.Service.Services.DdOl
{
    public class DictionaryDao
    {
        private const string CnstDictionaryCfg = "AUSWAHLLISTEN";
        public const string CnstDictionaryRechtsform= "RECHTSFORM";
        public const string CnstDictionaryRechtsformEinzel = "RECHTSFORM_EINZELUNT";
        public const string CnstDictionaryAusweisart = "AUSWEISART";
        public const string CnstDictionaryFamilienstand = "FAMILIENSTAND";
        
        private static CacheDictionary<String, DictionaryDto[]> dictcache = CacheFactory<String, DictionaryDto[]>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        public static DictionaryDto[] getDictionaryValues(String dictionaryName)
        {
            if (!dictcache.ContainsKey(dictionaryName))
            {
                String domain = null;
                String cacheName = dictionaryName;
                if(dictionaryName.IndexOf(".")>-1)
                {
                    domain = dictionaryName.Substring( dictionaryName.IndexOf(".")+1);
                    dictionaryName = dictionaryName.Substring(0, dictionaryName.IndexOf("."));

                }
                String Query = "select value result1, id result2 from ddlkppos where code='" + dictionaryName + "' order by rank";
                if(domain!=null)
                {
                    if (domain.IndexOf("'") != 0)
                        domain = "'" + domain + "'";
                    Query = "select value result1, id result2 from ddlkppos where code='" + dictionaryName + "' and domainid in (" + domain + ") order by rank";
                }
                try
                {
                    // Get the config entries
                    List<CIC.Database.OIQUEUE.EF6.Model.CFGVAR> ConfigEntries = ConfigHelper.GetConfigEntries(CnstDictionaryCfg, dictionaryName);

                    // Get the table name
                    string TableName = MyGetCfgEntryValue("TABLE", ConfigEntries, false);

                    // Get key1 name
                    string Key1Name = MyGetCfgEntryValue("KEY1NAME", ConfigEntries, false);

                    // Get key1 value
                    string Key1Value = MyGetCfgEntryValue("KEY1VALUE", ConfigEntries, false);

                    // Get key2 name
                    string Key2Name = MyGetCfgEntryValue("KEY2NAME", ConfigEntries, TableName != "BMWKURZ");

                    // Get key2 value
                    string Key2Value = MyGetCfgEntryValue("KEY2VALUE", ConfigEntries, TableName != "BMWKURZ");

                    // Get result1 name
                    string Result1Name = MyGetCfgEntryValue("RESULT1NAME", ConfigEntries, false);

                    // Get result2 name
                    string Result2Name = MyGetCfgEntryValue("RESULT2NAME", ConfigEntries, false);

                    // Get sort1 name
                    string Sort1Name = MyGetCfgEntryValue("SORT1NAME", ConfigEntries, true);

                    // Get sort2 name
                    string Sort2Name = MyGetCfgEntryValue("SORT2NAME", ConfigEntries, true);

                    // Build the query
                    Query = "select " + Result1Name + " as Result1, " + Result2Name + " as Result2 from " + TableName;
                    Query += " where " + Key1Name + "=" + Key1Value;

                    // Check if table is BMWKURZ
                    if (TableName == "BMWKURZ")
                    {
                        // Add the second condition
                        Query += " and " + Key2Name + "=" + Key2Value;
                    }

                    // Check if sorting is specified
                    if (Sort1Name != null)
                    {
                        // Add sorting to the query
                        Query += " order by " + Sort1Name;

                        // Check if second sorting field is specified
                        if (Sort2Name != null)
                        {
                            Query += ", " + Sort2Name;
                        }
                    }
                }catch(Exception e)//no configuration found, use default
                {

                }

                // Create a context
                using (DdOiQueueExtended Context = new DdOiQueueExtended())
                {
                    // Execute the query
                    IEnumerable<DictionaryDto> Result = Context.ExecuteStoreQuery<DictionaryDto>(Query);

                    // Return the result
                   dictcache[cacheName] = Result.ToArray();
                }
                return dictcache[cacheName];
            }
            return dictcache[dictionaryName];
        }
        private static string MyGetCfgEntryValue(string entryCode, List<CIC.Database.OIQUEUE.EF6.Model.CFGVAR> entries, bool optional)
        {
            // Query entries
            var CurrentEntry = (from Entry in entries
                                where Entry.CODE == entryCode
                                select Entry).FirstOrDefault();

            // Check if the entry exists
            if (CurrentEntry == null || string.IsNullOrEmpty(CurrentEntry.WERT))
            {
                // Check if the value is optional
                if (optional)
                {
                    // Return null value
                    return null;
                }

                // Throw an exception
                throw new Exception("Value for \"" + entryCode + "\" not found.");
            }

            // Return the value
            return CurrentEntry.WERT;
        }

        
        public static List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> getLaender()
        {
            
            
            List<LAND> LANDList = null;
            List< LAND> LANDFinalList = null;
            List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> LANDDtoList = null;

            using (DdOlExtended context = new DdOlExtended())
            {
                LANDFinalList = new List<LAND>();
                var LANDListQueryDefault = from land in context.LAND
                                           where (land.COUNTRYNAME != null) && (land.DEFAULTFLAG != 0) && (land.DEFAULTFLAG != null)
                                           orderby land.COUNTRYNAME
                                           select land;

                //Get list
                LANDList = LANDListQueryDefault.ToList<LAND>();

                //Sort
                var LANDListSortedQuery = from land in LANDList
                                          orderby land.COUNTRYNAME
                                          select land;
                //Add to return list
                LANDFinalList.AddRange(LANDListSortedQuery);

                //Clear list
                LANDList.Clear();

                var LANDListQueryBestfive = from land in context.LAND
                                            where (land.COUNTRYNAME != null) && (land.BESTFIVEFLAG != 0) && (land.BESTFIVEFLAG != null)
                                            orderby land.COUNTRYNAME
                                            select land;

                //Get list
                LANDList = LANDListQueryBestfive.ToList<LAND>();

                //Sort
                LANDListSortedQuery = from land in LANDList
                                      orderby land.COUNTRYNAME
                                      select land;
                //Add to return list
                LANDFinalList.AddRange(LANDListSortedQuery);

                //Clear list
                LANDList.Clear();

                var LANDListQueryRest = from land in context.LAND
                                        where (land.COUNTRYNAME != null) && (land.DEFAULTFLAG != 1 || land.DEFAULTFLAG == null) && (land.BESTFIVEFLAG != 1 || land.BESTFIVEFLAG == null)
                                        orderby land.COUNTRYNAME
                                        select land;

                //Get list
                LANDList = LANDListQueryRest.ToList<LAND>();

                //Sort
                LANDListSortedQuery = from land in LANDList
                                      orderby land.COUNTRYNAME
                                      select land;
                //Add to return list
                LANDFinalList.AddRange(LANDListSortedQuery);

                //Clear list
                LANDList.Clear();
                //LANDList = MyFilterLandList(context.LANDExtension.Select(null, null, Cic.OpenLease.Model.DdOl.LAND.FieldNames.COUNTRYNAME, 0, 0));
            }


            if (LANDFinalList != null)
            {
                // New list
                LANDDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto>();
                // New assembler
                LANDAssembler LANDAssembler = new LANDAssembler();

                foreach ( LAND LANDLoop in LANDFinalList)
                {
                    // Convert and add to the list
                    LANDDtoList.Add(LANDAssembler.ConvertToDto(LANDLoop));
                }
            }
            
            return LANDDtoList;
        }
    }
}