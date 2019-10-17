namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenOne.Common.Model.DdEurotax;
    using Cic.OpenOne.Common.Util.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    #endregion

    public class EurotaxNumberHelper
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Methods

        /// <summary>
        /// returns all possible option codes for a mapping
        /// </summary>
        /// <returns></returns>
        public static List<string> GetOptionCodes()
        {

            // Create the entities
            using (DdEurotaxExtended Entities = new DdEurotaxExtended())
            {
                try
                {

                    // Set the query
                    string Query = "select distinct code from obtypmap where art='20'";

                    // Execure the query
                    return Entities.ExecuteStoreQuery<string>(Query, null).ToList<string>();


                }
                catch (Exception e)
                {
                    // Throw an exception with an inner exception
                    throw new ApplicationException("Could not deliver the Eurotax option codes", e);
                }
            }

        }

        /// <summary>
        /// returns all possible option codes for a mapping
        /// </summary>
        /// <returns></returns>
        public static List<string> GetOptionCodes(String okaCode)
        {

            // Create the entities
            using( DdEurotaxExtended Entities = new DdEurotaxExtended())
            {
                try
                {
                    string aklasseQuery = "select aklasse from obtyp, obtypmap where  obtypmap.code='"+okaCode+"' and obtypmap.art=10 and obtyp.sysobtyp=obtypmap.sysobtyp";
                    String aklasse = Entities.ExecuteStoreQuery<string>(aklasseQuery, null).FirstOrDefault();
                    string obtypidquery = "select sysobtyp from obtypmap where  obtypmap.code='" + okaCode + "' and obtypmap.art=10";
                    long sysobtyp = Entities.ExecuteStoreQuery<long>(obtypidquery, null).FirstOrDefault();
                    string brandQuery = "select sysobtyp from obtyp  where importtable='ETGMAKE' connect by prior sysobtypp=sysobtyp start with sysobtyp=" + sysobtyp;
                    long sysobtypbrand = Entities.ExecuteStoreQuery<long>(brandQuery, null).FirstOrDefault();

                    string obtypquery = "select distinct obtypmap.code from obtyp, obtypmap where obtypmap.sysobtyp in (select sysobtyp from obtyp where level=4  connect by prior sysobtyp=sysobtypp start with sysobtyp="+sysobtypbrand+") and obtypmap.art=20 and obtyp.sysobtyp=obtypmap.sysobtyp and obtyp.aklasse='" + aklasse+"'";
                    //string optQuery = "select distinct obtypmap.code from obtypmap where art=20 and sysobtyp in (select sysobtyp from obtypmap where obtypmap.code='"+okaCode+"'";

                    // Execure the query
                    return Entities.ExecuteStoreQuery<string>(obtypquery, null).ToList<string>();


                }
                catch (Exception e)
                {
                    // Throw an exception with an inner exception
                    throw new ApplicationException("Could not deliver the Eurotax option codes", e);
                }
            }

        }

        public static ObtypMapInfo[] GetEurotaxNumbersFromOka(string oka, List<String> options, decimal? nova)
        {
            int optionCount = options.Count;//number of different options that are mapped
            
            List<String> codes = new List<String>();
            //different sysobtyp for the given options and oka
            //String query1 = "select sysobtyp from obtypmap where sysobtyp in (select distinct sysobtyp from obtypmap where code='"+oka+"') and art='20' group by sysobtyp having count(*)=" + optionCount;
            String query1 = "select schwacke from obtyp where sysobtyp in (select sysobtyp from obtypmap where sysobtyp in (select distinct sysobtyp from obtypmap where code='" + oka + "' and art=10) and art=20 group by sysobtyp having count(*)=" + optionCount + ")";
            String queryZ = "select schwacke from obtyp where sysobtyp in (select sysobtyp from obtypmap where sysobtyp in (select distinct sysobtyp from obtypmap where code='" + oka + "' and art=10) and art!=30 and ( ( art=10 and  code='" + oka + "') or art!=10) group by sysobtyp having count(*)=1)";
            if (optionCount == 0)
                query1 = queryZ;

            try
            {
                using (DdEurotaxExtended ctx = new DdEurotaxExtended())
                {
                    _Log.Debug("Eurotax from OKA: " + query1);
                    codes = ctx.ExecuteStoreQuery<String>(query1, null).ToList<String>();
                    //Für RFO Reifenemmission dies deaktivieren:
                    /*if (false && codes.Count > 1 && nova.HasValue)
                    {

                        String schwackes = "'" + string.Join("','", codes.ToArray()) + "'";
                        int novaWert = (int) ((nova - 1)*100);
                        try
                        {
                            String query2 = "select distinct schwacke from obtyp,obtypmap where obtyp.sysobtyp=obtypmap.sysobtyp and schwacke in (" + schwackes + ") and obtypmap.art=30 and obtypmap.code='" + novaWert + "'";
                            List<String> codes2 = ctx.ExecuteStoreQuery<String>(query2, null).ToList<String>();
                            if (codes2.Count == 0)
                            {
                                _Log.Info("Nova-Matching for SA3-OKA failed - no nova of value " + nova + " found for OKA " + oka + " . not matching Nova!");
                            }
                            else codes = codes2;
                        }
                        catch (Exception e)
                        {
                            _Log.Info("Nova-Matching for SA3-OKA failed - no nova of value " + nova + " found for OKA " + oka + " . not matching Nova!", e);
                        }
                    }*/
                }
            }
            catch (Exception ex)
            {
                _Log.Error("SA3-OKA Matching failed", ex);
                throw new ApplicationException("Could not deliver the Eurotax codes", ex);
            }
            List<ObtypMapInfo> rval = new List<ObtypMapInfo>();
            foreach (String code in codes)
            {
                ObtypMapInfo info = new ObtypMapInfo();
                info.GUELTIGVON = new DateTime();
                info.Eurotaxnummer = code;
                rval.Add(info);
            }
            return rval.ToArray();
        }

        public static string GetEurotaxNumberFromOka(string oka, string optionCode1, string optionCode2)
        {
            List<string> etcodes = EurotaxNumberHelper.GetOptionCodes(oka);
                // Execute the method from helper and return its result
                List<String> optionCodes = new List<String>();
                // Loop through all options
                if (optionCode1!=null && optionCode1.Length>0 && etcodes.Contains(optionCode1))//use every possible option just once
                    {
                        etcodes.Remove(optionCode1);
                        optionCodes.Add(optionCode1);
                    }

            if (optionCode2!=null && optionCode2.Length>0 && etcodes.Contains(optionCode2))//use every possible option just once
                    {
                        etcodes.Remove(optionCode2);
                        optionCodes.Add(optionCode2);
                    }
            ObtypMapInfo[] infos = GetEurotaxNumbersFromOka(oka, optionCodes, null);
            if (infos == null || infos.Length == 0) throw new ApplicationException("The specified OKA code could not be found.");
            return infos[0].Eurotaxnummer;

            
        }
        #endregion
    }
}