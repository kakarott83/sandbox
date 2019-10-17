using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Holds the infos of view PRPOUVR_V
    /// </summary>
    public class PouvoirRule
    {   //"SYSPRPOUVRULE", "TYP","AREA","SYSID"
        public long SYSPRPOUVRULE
        {
            get;
            set;
        }
        public long TYP
        {
            get;
            set;
        }
        public string AREA
        {
            get;
            set;
        }
        public long SYSID
        {
            get;
            set;
        }
    }

    class PouvoirAssignment
    {
        public long sysperole { get; set; }
        public long sysprfld { get; set; }
        public long sysprpouvoir { get; set; }

    }
    /// <summary>
    /// Data Access Object for Insurances
    /// </summary>
    [System.CLSCompliant(true)]
    public class PouvoirDao
    {
        #region Private variables
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DdOlExtended _context;

        private static CacheDictionary<String, List<PRPOUVOIR>> prPouvoirCache = CacheFactory<String, List<PRPOUVOIR>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<PouvoirAssignment>> pouvoirAssignmentCache = CacheFactory<String, List<PouvoirAssignment>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<PouvoirRule>> ruleCache = CacheFactory<String, List<PouvoirRule>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        #endregion



        #region Constructors
        public PouvoirDao(DdOlExtended context)
        {
            _context = context;
        }
        #endregion


        /// <summary>
        /// Delivers Pouvoir for the given perole and prparam
        /// </summary>
        /// <param name="sysprparam"></param>
        /// <param name="sysprfld"></param>
        /// <param name="sysPerole"></param>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public PRPOUVOIR DeliverPouvoir(long sysprparam, long sysprfld, long sysPerole,PouvoirTriggerDto[] trigger)
        {
            if (!prPouvoirCache.ContainsKey("POUVOIRS"))
            {
                string pouvoirquery = "select * from prpouvoir";
                prPouvoirCache["POUVOIRS"] =  _context.ExecuteStoreQuery<PRPOUVOIR>(pouvoirquery, null).ToList<PRPOUVOIR>();
                string allPouvoirs = "select prkomgrpm.sysperole,PRPOUVSET.SYSPRFLD, prpouvoir.sysprpouvoir FROM prpouvoir,prkomgrpm,prkomgrp    ,PRPOUVRULE,PRPOUVSET " 
                    + " where PRPOUVSET.SYSPRPOUVSET=PRPOUVRULE.SYSPRPOUVSET and PRPOUVRULE.SYSPRPOUVRULE=PRPOUVOIR.SYSPRPOUVRULE and prkomgrpm.sysprkomgrp=prkomgrp.sysprkomgrp "
                    + " and prpouvoir.sysprkomgrp=prkomgrpm.sysprkomgrp and prkomgrpm.activeflag=1 and prkomgrp.activeflag=1 and " 
                    // + " (prkomgrp.validfrom is null or prkomgrp.validfrom <= TRUNC(SYSDATE)) and (prkomgrp.validuntil is null or prkomgrp.validuntil >= TRUNC(SYSDATE)) " 
                    + SQL.CheckCurrentSysDate ("prkomgrp")
                    + " and "
                    // + " (prkomgrpm.validfrom is null or prkomgrpm.validfrom<=TRUNC(SYSDATE)) and (prkomgrpm.validuntil is null or prkomgrpm.validuntil>=TRUNC(SYSDATE))";
                    + SQL.CheckCurrentSysDate ("prkomgrpm");

                pouvoirAssignmentCache["POUVOIRS"] =  _context.ExecuteStoreQuery<PouvoirAssignment>(allPouvoirs, null).ToList<PouvoirAssignment>();
                ruleCache["POUVOIRS"] =  _context.ExecuteStoreQuery<PouvoirRule>("select * from CIC.PRPOUVR_V", null).ToList<PouvoirRule>();

            }

            List<PouvoirAssignment> assignments = pouvoirAssignmentCache["POUVOIRS"];
            List<PRPOUVOIR> pouvoirs = prPouvoirCache["POUVOIRS"];
            assignments = (from p in assignments
                                    where p.sysperole == sysPerole && p.sysprfld == sysprfld
                                    select p).ToList();
            if (assignments == null || assignments.Count==0) return null;

            List<PouvoirRule> rules = ruleCache["POUVOIRS"];
            if (rules == null || rules.Count == 0) //no rule, nothing to do
                return null;

            List<PRPOUVOIR> checkpouvoirs = new List<PRPOUVOIR>();
            foreach(PouvoirAssignment pa in assignments)
            {
                checkpouvoirs.AddRange( (from p in pouvoirs
                            where p.SYSPRPOUVOIR == pa.sysprpouvoir
                            select p).ToList());
            }
            pouvoirs = checkpouvoirs;
           // List<PouvoirRule> rulesPouvoir = new List<PouvoirRule>();//contains all possible rules for this prisma param
            List<PRPOUVOIR> pouvoirsFiltered = new List<PRPOUVOIR>();
            foreach(PRPOUVOIR pouvoir in pouvoirs)
            {

                var q = from p in _context.PRPOUVOIR
                        where p.SYSPRPOUVOIR == pouvoir.SYSPRPOUVOIR
                        select p;
                PRPOUVOIR gp = q.FirstOrDefault();

                if (gp.PRPOUVRULE == null)
                    _context.Entry(gp).Reference(f => f.PRPOUVRULE).Load();

              

                var q1 = from r in rules
                         where r.SYSPRPOUVRULE == gp.PRPOUVRULE.SYSPRPOUVRULE
                         select r;
                
                List<PouvoirRule> rulesPouvoir = q1.ToList();

                //no rules, so use the pouvoir
                if (rulesPouvoir == null || rulesPouvoir.Count == 0) pouvoirsFiltered.Add(pouvoir);
                else //rules apply, check against triggers
                {
                    bool usepouvoir = true;
                    foreach (PouvoirTriggerDto trg in trigger)// long sysPrProdukt, long sysObArt, long sysBrand
                    {
                        var qa = from r in rulesPouvoir
                                 where r.TYP == trg.TRG_TYPE
                                 select r;
                        if (qa.FirstOrDefault() == null)//no rule for this type at all - so dont use the filter
                            continue;

                        var q2 = from r in rulesPouvoir
                                 where r.TYP == trg.TRG_TYPE && r.SYSID == trg.TRG_ID
                                 select r;

                        if (trg.TRG_IDS != null)
                        {
                            q2 = from r in rulesPouvoir
                                 where r.TYP == trg.TRG_TYPE && trg.TRG_IDS.Contains(r.SYSID)
                                 select r;
                        }

                        if (q2.FirstOrDefault() == null)//rule available but not for filter - > dont accept this prpouvoir
                        {   usepouvoir = false; 
                            break;
                        }
                    }
                    if(usepouvoir)
                        pouvoirsFiltered.Add(pouvoir);
                }
            }
            //no prisma rules - return first result
            return getMaxPouvoir(pouvoirsFiltered);

          
        }
        public PRPOUVOIR getMaxPouvoir(List<PRPOUVOIR> list)
        {
            if (list == null) return null;

            PRPOUVOIR rval = list.FirstOrDefault();

            if (rval == null) return null;

            if (list.Count == 1)
                return rval;

            foreach (PRPOUVOIR p in list)
            {
                if (p.TYP == 0)
                {
                    if (p.ADJMAXN > rval.ADJMAXN)
                        rval.ADJMAXN = p.ADJMAXN;
                    if (p.ADJMINN < rval.ADJMINN)
                        rval.ADJMINN = p.ADJMINN;
                }
                if (p.TYP == 1)
                {
                    if (p.ADJMAXP > rval.ADJMAXP)
                        rval.ADJMAXP = p.ADJMAXP;
                    if (p.ADJMINP < rval.ADJMINP)
                        rval.ADJMINP = p.ADJMINP;
                }
                if (p.TYP == 2)
                {
                    if (p.ADJMAXD > rval.ADJMAXD)
                        rval.ADJMAXD = p.ADJMAXD;
                    if (p.ADJMIND < rval.ADJMIND)
                        rval.ADJMIND = p.ADJMIND;
                }
            }

            return rval;
        }

        public void applyPouvoir(PRPARAMDto p, long sysPerole, PouvoirTriggerDto[] trigger)
        {

            try
            {
                if (p == null) return;
              /*  if (!p.PRFLDReference.IsLoaded)
                {
                    p.PRFLDReference.Load();
                }*/
                PRPOUVOIR pouvoir = DeliverPouvoir(p.SYSPRPARAM,p.SYSPRFLD, sysPerole, trigger);

                //...apply pouvoir
                if (pouvoir != null)
                {
                    if (pouvoir.TYP == 0 && pouvoir.ADJMINN.HasValue)//numeric
                    {
                        p.MINVALN += pouvoir.ADJMINN.Value;
                        p.MAXVALN += pouvoir.ADJMAXN.Value;
                    }
                    else if (pouvoir.TYP == 1 && pouvoir.ADJMINP.HasValue)//percent
                    {
                        p.MINVALP += pouvoir.ADJMINP.Value;
                        p.MAXVALP += pouvoir.ADJMAXP.Value;
                        if (p.MINVALP < 0) p.MINVALP = 0;
                        if (p.MAXVALP > 100) p.MAXVALP = 100;
                    }
                  /*  else if (pouvoir.TYP == 2)//date
                    {
                        p.minvald = pouvoir.ADJMIND;
                        p.maxvald = pouvoir.ADJMAXD;
                    }*/
                }
            }
            catch (Exception e)
            {
                _log.Error("PouvoirDao - applying Pouvoir failed: "+e.Message, e);
            }
        }


    }
}