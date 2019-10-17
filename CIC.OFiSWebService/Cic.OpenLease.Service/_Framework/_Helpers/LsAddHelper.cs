namespace Cic.OpenLease.Service
{

    #region Using
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Collection;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public static class LsAddHelper
    {
        #region Methods
        public const int KREDIT_VART = 200;
        public const int LEASING_VART = 100;
        //public const String MANDANT_CODE = "HCSD";

        private static CacheDictionary<long, decimal> taxesVart = CacheFactory<long, decimal>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, decimal> taxesPerole = CacheFactory<long, decimal>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, String> peroleMandant = CacheFactory<long, String>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static String QUERY_MANDANT_CODE = "select lsadd.mandant from perole, person,lsadd where  person.sysperson=perole.sysperson and lsadd.sysperson=person.sysperson and sysparent is null and "
            // + " (perole.validuntil is null or perole.validuntil>=TRUNC(SYSDATE)) and (perole.validfrom is null or perole.validfrom<=TRUNC(SYSDATE)) "
            + SQL.CheckCurrentSysDate ("perole")
            + " and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=:sysperole)";

        /// <summary>
        /// Gets the tax rate. Creates a new object context.
        /// </summary>
        /// <returns>Tax rate.</returns>
        public static decimal GetTaxRate(long? sysvart)
        {
            // Create the context
            using (DdOlExtended Context = new DdOlExtended())
            {
                // Return the tax rate from another version of this method
                return GetTaxRate(Context, sysvart);
            }
        }

        /// <summary>
        /// Gets the tax rate.
        /// </summary>
        /// <param name="context">Object context to use.</param>
        /// <param name="sysvart">Vart for the tax</param>
        /// <returns>Tax rate.</returns>
        public static decimal GetTaxRate(DdOlExtended context, long? sysvart)
        {

            long key = 0;
            if (sysvart == null)
                key = -1;
            else key = (long)sysvart;
            if (taxesVart.ContainsKey(key))
                return taxesVart[key];

            try
            {
                //Get Angebot unique identifier



                decimal TaxRate = context.ExecuteStoreQuery<decimal>(
                    
                       "select mwst.prozent from vart,mwst where mwst.sysmwst=vart.sysmwst and sysvart=" + sysvart, null).FirstOrDefault();

                
                // Return the tax rate
                taxesVart[key] = TaxRate;

                return TaxRate;
            }
            catch (Exception exception)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the tax rate.", exception);
            }


        }

        public static long getMandantByPEROLE(DdOlExtended  context, long sysperole)
        {
            try
            {
                String mcode = getPeroleMandant(sysperole);
                return context.ExecuteStoreQuery<long>(
                      "select syslsadd from lsadd where lsadd.mandant='" + mcode + "'", null).FirstOrDefault();
               
            }
            catch (Exception exception)
            {
                // Throw an exception
                throw new ApplicationException("Could not get mandant for perole " + sysperole, exception);
            }
        }

        /// <summary>
        /// Returns the hauswaehrung sysid
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public static long getHauswaehrungByPEROLE(DdOlExtended context, long sysperole)
        {
            try
            {
                String mcode = getPeroleMandant(sysperole);
                return context.ExecuteStoreQuery<long>(
                      "select syshauswaehrung from lsadd where lsadd.mandant='" + mcode + "'", null).FirstOrDefault();

            }
            catch (Exception exception)
            {
                // Throw an exception
                throw new ApplicationException("Could not get mandant for perole " + sysperole, exception);
            }
        }
        


        public static long getSYSMWSTByVART(DdOlExtended context, long? sysvart)
        {
            try
            {
                return context.ExecuteStoreQuery<long>(
                       
                       "select sysmwst from vart where sysvart="+sysvart, null).FirstOrDefault();
               
            }
            catch (Exception exception)
            {
                // Throw an exception
                throw new ApplicationException("Could not get mandant for vart " + sysvart, exception);
            }
        }

        

        /// <summary>
        /// determine the mandant-code for the perole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>         
        private static String getPeroleMandant(long sysperole)
        {
            if(!peroleMandant.ContainsKey(sysperole))
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter>  parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                    peroleMandant[sysperole] = context.ExecuteStoreQuery<String>(QUERY_MANDANT_CODE, parameters.ToArray()).FirstOrDefault();

                }
            }
            return peroleMandant[sysperole];
        }

        /// <summary>
        /// Returns the global Ust Value of MWST-Table, defined by the code in the Configsection AIDA/GENERAL/USTCODE
        /// </summary>
        /// <returns></returns>
       public static decimal getGlobalUst(long sysperole)
        {


            if (!taxesPerole.ContainsKey(sysperole))
            {
                
                
                using (DdOlExtended context = new DdOlExtended())
                {
                    String mcode = getPeroleMandant(sysperole);
                    taxesPerole[sysperole] = context.ExecuteStoreQuery<decimal>(
                        "select prozent from mwst,lsadd where mwst.sysmwst=lsadd.sysmwst and lsadd.mandant='" + mcode + "'", null).FirstOrDefault();
                  
                }
            }
            return taxesPerole[sysperole];
        }

        public static decimal DeliverNova()
        {
            return 0M;
        }

        private static LSADD DeliverLsAdd(DdOlExtended context, long syslsadd)
        {
            LSADD LSADD = null;

            try
            {
                
                var Query = from lsadd in context.LSADD
                            where lsadd.SYSLSADD == syslsadd
                            select lsadd;
                LSADD = Query.FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return LSADD;
        }


        #endregion
    }
}
