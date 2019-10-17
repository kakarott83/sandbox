using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    public static class ZinsHelper
    {
        private static CacheDictionary<long, decimal> refiCache = CacheFactory<long, decimal>.getInstance().createCache(1000 * 60 * 60 * 6);
        private static CacheDictionary<int, int> inttypeCache = CacheFactory<int, int>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /*  public static decimal DeliverZins(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long sysPrProduct, long term, decimal amound, long sysObTyp, long sysObArt, long sysPrkGroup, long prHGroup, long sysBrand, long sysPerole, long pSysINTTYPE)
          {
              return DeliverZins(context, sysPrProduct, term, amound, sysObTyp, sysObArt, sysPrkGroup, prHGroup, sysBrand, sysPerole, pSysINTTYPE, 0);
          }

          public static decimal DeliverRefiZinsVariabel()
          {
              if (!refiCache.ContainsKey(1))
              {
                  string query = "select finzins from zinstab, zinsdate where bezeichnung like '%efi%vari%' and zinstab.syszinstab=zinsdate.syszinstab order by zinsdate.datum desc";
                  using (Cic.OpenLease.Model.DdOl.OlExtendedEntities context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
                  {
                      refiCache[1] = context.ExecuteStoreQuery<decimal>(query, null).FirstOrDefault();
                  }
              }
              return refiCache[1];
          }
          public static decimal DeliverRefiZinsFix()
          {
              if (!refiCache.ContainsKey(2))
              {
                  string query = "select finzins from zinstab, zinsdate where bezeichnung like '%efi%fix' and zinstab.syszinstab=zinsdate.syszinstab order by zinsdate.datum desc";
                  using (Cic.OpenLease.Model.DdOl.OlExtendedEntities context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
                  {
                      refiCache[2] = context.ExecuteStoreQuery<decimal>(query, null).FirstOrDefault();
                  }
              }
              return refiCache[2];
          }
          */
        private static CacheDictionary<String, decimal> zCache = CacheFactory<String, decimal>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        public static decimal DeliverZins(DdOlExtended context, long sysPrProduct, long term, decimal amount, long sysObTyp, long sysObArt, 
            long sysPrkGroup, long prHGroup, long sysBrand, long sysPerole, long pSysINTTYPE, int excludeSubvRates)
        {

            System.DateTime Date = System.DateTime.Now;

            IZinsBo zinsBo = CommonBOFactory.getInstance().createZinsBo(ZinsBo.CONDITIONS_HCBE, "de-DE");
            prKontextDto ctx = new prKontextDto();
            ctx.perDate = Date;
            ctx.sysbrand = sysBrand;
            ctx.sysperole = sysPerole;
            ctx.sysprinttype = pSysINTTYPE;
            ctx.sysprproduct = sysPrProduct;
            ctx.sysobtyp = sysObTyp;
            ctx.sysobart = sysObArt;
            ctx.sysprkgroup = sysPrkGroup;
            ctx.sysprhgroup = prHGroup;
            ctx.subventionMode = excludeSubvRates;


            double zinsNew = zinsBo.getZins(ctx, term, (double)amount);
            return (decimal)zinsNew;/*

            String key = sysPrProduct +"_"+Date.Year+"_"+Date.Month+"_"+Date.Day+ "_" + term + "_" + amount + "_" + sysObTyp + "_" + sysObArt + "_" + sysPrkGroup + "_" + prHGroup + "_" + sysBrand + "_" + sysPerole + "_" + pSysINTTYPE + "_" + excludeSubvRates;
            decimal Result;
            if (!zCache.ContainsKey(key))
            {
                

                
                object[] Parameters =
                {
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pDate", Value = Date.ToString("yyyy-MM-dd")},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysPRPRODUCT", Value = sysPrProduct},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pTerm", Value = term},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysBRAND", Value = sysBrand},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysOBType", Value = sysObTyp},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysOBArt", Value = sysObArt},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysPRKGroup", Value = sysPrkGroup},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysPRHGroup", Value = prHGroup},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysPeRole", Value = sysPerole},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysINTTYPE", Value = pSysINTTYPE},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pAmound", Value = amount},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pExclude", Value = excludeSubvRates}

                };

                string Query = "SELECT CIC.CIC_PRISMA_UTILS.DeliverZins(:pSysPRPRODUCT, to_date(:pDate,'yyyy-mm-dd'), :pTerm, :pAmound, :pSysBRAND, :pSysOBType, :pSysOBArt, :pSysPRKGroup, :pSysPRHGroup, :pSysPeRole, :pSysINTTYPE, :pExclude) RESULT FROM DUAL";
                Result = context.ExecuteStoreQuery<decimal>(Query, Parameters).FirstOrDefault<decimal>();

                zCache[key]=Result;
            }
            Result= zCache[key];

            _Log.Error("Zins New: " + zinsNew + " Zins old: " + Result);
            if (Math.Abs(zinsNew - (double)Result) > 0)
                _Log.Error("Zins DIFFERS!!!!!!!!!!!!!!!!!!");

            return Result;*/
        }

        /// <summary>
        /// returns 0 for fix and 1 for variable interest
        /// </summary>
        /// <param name="sysinttype"></param>
        /// <returns></returns>
      /*  public static int DeliverZinsTyp(int sysinttype)
        {
            if (!inttypeCache.ContainsKey(sysinttype))
            {
                using (Cic.OpenLease.Model.DdOl.OlExtendedEntities context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
                {

                    int inttype = context.ExecuteStoreQuery<int>("select inttype from inttype where sysinttype=" + sysinttype, null).FirstOrDefault();
                    inttypeCache[sysinttype] = inttype;
                }
            }
            return inttypeCache[sysinttype];
        }*/


        public static decimal DeliverZinsBasis(DdOlExtended context, long sysPrProduct, long term, double amount)
        {

            IZinsBo zinsBo = CommonBOFactory.getInstance().createZinsBo(ZinsBo.CONDITIONS_BANKNOW, "de-DE");
            double zinsBasis = zinsBo.getZinsBasis(sysPrProduct, DateTime.Now, term, amount);
            return (decimal)zinsBasis;
            /*
            System.DateTime Date = System.DateTime.Now;
            String key = sysPrProduct + "_" + Date.Year + "_" + Date.Month + "_" + Date.Day + "_" + term + "_" + amount;
            if (!zCache.ContainsKey(key))
            { 
                object[] Parameters =
                {
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pDate", Value = Date.ToString("yyyy-MM-dd")},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysPRPRODUCT", Value = sysPrProduct},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pTerm", Value = term},
                    new Devart.Data.Oracle.OracleParameter{ ParameterName = "pAmound", Value = System.Convert.ToInt32(amount)}
                };

                string Query = "SELECT CIC.CIC_PRISMA_UTILS.DeliverZinsBasis(:pSysPRPRODUCT, to_date(:pDate,'yyyy-mm-dd'), :pTerm, :pAmound) RESULT FROM DUAL";
                decimal Result = context.ExecuteStoreQuery<decimal>(Query, Parameters).FirstOrDefault<decimal>();
                zCache[key] = Result;
            }
            decimal rval = zCache[key];
            _Log.Error("ZinsBasis New: " + zinsBasis + " ZinsBasis old: " + rval);
            if(Math.Abs(zinsBasis-(double)rval)>0)
                _Log.Error("ZinsBasis DIFFERS!!!!!!!!!!!!!!!!!!");
            return rval;*/
        }

        
    }
}