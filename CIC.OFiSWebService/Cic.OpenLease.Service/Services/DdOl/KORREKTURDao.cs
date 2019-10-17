namespace Cic.OpenLease.Service.Services.DdOl
{
    #region Using
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Clarion;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    #endregion


    [System.CLSCompliant(true)]
    public class KORREKTURDao
    {
        #region Private Variables

        private static CacheDictionary<string, List<KORREKTUR>> cache = CacheFactory<string, List<KORREKTUR>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, string> idNameCache = CacheFactory<long, string>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<string, decimal> korrcache = CacheFactory<string, decimal>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        private static DateTime nullDate = new DateTime(1800, 1, 1);
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const int TYPE_AUTO = 0;
        public const int TYPE_STRING = 2;
        public const int TYPE_DECIMAL = 1;


        #endregion

        public KORREKTURDao(DdOlExtended context)
        {

            if (cache.Count == 0)
            {
                _Log.Info("Caching Korrektur...");
                var Querykorrtyp = from korrtyp in context.KORRTYP
                                   select korrtyp;
                List<KORRTYP> korrtyplist = Querykorrtyp.ToList();
                foreach (KORRTYP korrtyp in korrtyplist)
                {
                    if (!context.Entry(korrtyp).Collection(f => f.KORREKTURList).IsLoaded)
                        context.Entry(korrtyp).Collection(f => f.KORREKTURList).Load();
                    
                    cache[korrtyp.NAME] = korrtyp.KORREKTURList.ToList();
                    idNameCache[korrtyp.SYSKORRTYP] = korrtyp.NAME;
                }
                _Log.Info("Cached " + idNameCache.Count + " Korrtypen");
            }

        }
        #region Methods

        //select factorvalue, art from korrektur,korrtyp where korrektur.syskorrtyp=korrtyp.syskorrtyp and korrtyp.name='RW_KORR_FIN' and (disabledflag=0 or disabledflag is null) 
        // and (validfrom is null or validfrom<='01.01.2000') and (validuntil is null or validuntil>='01.01.2010')
        public decimal Correct(string korrtypName, decimal value, string op, DateTime perDate, String p1, String p2)
        {
            return CorrectType(korrtypName, value, op, perDate, p1, p2, TYPE_AUTO, TYPE_AUTO);
        }

        //select factorvalue, art from korrektur,korrtyp where korrektur.syskorrtyp=korrtyp.syskorrtyp and korrtyp.name='RW_KORR_FIN' and (disabledflag=0 or disabledflag is null) 
        // and (validfrom is null or validfrom<='01.01.2000') and (validuntil is null or validuntil>='01.01.2010')
        public decimal CorrectType(string korrtypName, decimal value, string op, DateTime perDate, String p1, String p2, int type1, int type2)
        {
            string datekey = "now";
            if (perDate != null)
            {
                datekey = perDate.Year + "_" + perDate.Month + "_" + perDate.Day;
            }
            string key = korrtypName + "_" + value + "_" + op + "_" + p1 + "_" + p2 + "_" + datekey;
            if (!korrcache.ContainsKey(key))
            {
                //Execute Query
                Evaluator ev = new Evaluator();
                if (type1 == TYPE_DECIMAL || type1 == TYPE_AUTO)
                {
                    try
                    {
                        ev.Bind("loc_p1", Decimal.Parse(p1));
                    }
                    catch (Exception)
                    {
                        if (type1 != TYPE_DECIMAL)
                            ev.Bind("loc_p1", p1);
                        //if no number, dont use it
                    }
                }
                else if (type1 == TYPE_STRING)
                {

                    ev.Bind("loc_p1", p1);

                }

                if (type2 == TYPE_DECIMAL || type2 == TYPE_AUTO)
                {
                    try
                    {
                        ev.Bind("loc_p2", Decimal.Parse(p2));
                    }
                    catch (Exception)
                    {
                        if (type2 != TYPE_DECIMAL)
                            //if no number, dont use it
                            ev.Bind("loc_p2", p2);
                    }
                }
                else if (type2 == TYPE_STRING)
                {

                    ev.Bind("loc_p2", p2);

                }
                String cVal = value.ToString();

                //foreach (KORRTYP ktloop in korrtyplist)
                {

                    var Querykorrektur = from korrektur in cache[korrtypName]
                                         where (korrektur.DISABLEDFLAG == 0 || korrektur.DISABLEDFLAG == null)
                                         && (korrektur.VALIDFROM == null || korrektur.VALIDFROM.Value.Date <= perDate.Date || korrektur.VALIDFROM <= nullDate)
                                         && (korrektur.VALIDUNTIL == null ||korrektur.VALIDUNTIL.Value.Date >= perDate.Date || korrektur.VALIDUNTIL <= nullDate)
                                         orderby korrektur.POSITION ascending
                                         select korrektur;

                    List<KORREKTUR> korrekturList = Querykorrektur.ToList<KORREKTUR>();

                    foreach (KORREKTUR kloop in korrekturList)
                    {

                        string fac = (kloop.FACTORVALUENET);
                        string expr = (kloop.EXPRNET);

                        if (ev.validate(expr))
                        {
                            if (kloop.ART == 0)
                            {
                                cVal = ev.evaluate(cVal + " " + op + " (" + fac + ")");
                            }
                            else
                                cVal = ev.evaluate(fac);
                            if (kloop.BREAKCONDITION == 1) break;
                        }
                        else
                        {
                            if (kloop.BREAKCONDITION == 2) break;
                            else continue;
                        }
                    }
                }

                decimal rval = decimal.Parse(cVal, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                korrcache[key] = rval;
            }
            return korrcache[key];
        }


        //select factorvalue, art from korrektur,korrtyp where korrektur.syskorrtyp=korrtyp.syskorrtyp and korrtyp.name='RW_KORR_FIN' and (disabledflag=0 or disabledflag is null) 
        // and (validfrom is null or validfrom<='01.01.2000') and (validuntil is null or validuntil>='01.01.2010')
        public decimal Correct(long sysKorrTyp, decimal value, string op, DateTime perDate, String p1, String p2)
        {

            return Correct(idNameCache[sysKorrTyp], value, op, perDate, p1, p2);
        }
        #endregion
    }
}

