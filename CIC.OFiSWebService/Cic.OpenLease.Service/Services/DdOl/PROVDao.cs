using Cic.OpenLease.Service.Provision;
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
    /// Container Class to hold return values for the provstep adjust-Method
    /// 
    /// </summary>
    [System.CLSCompliant(true)]
    public class PROVSTEPInfo
    {
        private decimal _provstep;
        private long _method;
        private decimal _provval;

        /// <summary>
        /// value (for method 3-4)
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal provval
        {
            get { return _provval; }
            set { _provval = value; }
        }

        /// <summary>
        /// percent (for method 0-2)
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal provstep
        {
            get { return _provstep; }
            set { _provstep = value; }
        }

        /// <summary>
        /// 0=Prozent
        /// 1=Prozentpunkte
        /// 2=Überschreiben Prozent
        /// 3=Betrag
        /// 4=Überschreiben Betrag
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long method
        {
            get { return _method; }
            set { _method = value; }
        }
    }

    /// <summary>
    /// Data Access Object for Provisions
    /// </summary>
    [System.CLSCompliant(true)]
    public class PROVDao
    {
        #region Private variables
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DdOlExtended _context;
        #endregion


        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<long, string> obtypen = CacheFactory<long, string>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        #region Constructors
        public PROVDao(DdOlExtended context)
        {
            _context = context;
        }
        #endregion

        private string getObTypen(long sysobtyp)
        {
            if (!obtypen.ContainsKey(sysobtyp))
            {
                double ctime = DateTime.Now.TimeOfDay.Milliseconds;
                String query = "select sysobtyp from obtyp start with sysobtyp=:sysobtyp connect by prior sysobtypp=sysobtyp";
                //Parameters for query
                System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysobtyp", Value = sysobtyp},
                        };

                List<long> obTypen = _context.ExecuteStoreQuery<long>(query, Parameters).ToList<long>();
                List<String> strings = new List<String>();
                foreach (long l in obTypen)
                    strings.Add(l.ToString());

                _Log.Debug("PROVDao: getObTypen Duration: " + (DateTime.Now.TimeOfDay.Milliseconds - ctime));

                obtypen[sysobtyp] =  System.String.Join(",", strings.ToArray());
            }
            return obtypen[sysobtyp];
        }

        private static CacheDictionary<String, PROVSTEPInfo> stepInfo = CacheFactory<String, PROVSTEPInfo>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        /// <summary>
        /// Deliver the Adjustementstep to the provrate with rank
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="sysPerole"></param>
        /// <param name="sysBrand"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        public PROVSTEPInfo DeliverProvStepInfo(long sysprproduct, long sysobtyp, long sysPerole, long sysBrand, int rank)
        {
            String key = sysprproduct + "_" + sysobtyp + "_" + sysPerole + "_" + sysBrand + "_" + rank;

            if (!stepInfo.ContainsKey(key))
            {
                double ctime = DateTime.Now.TimeOfDay.Milliseconds;

                /*
                        ADJUSTMENTTRIGGER = 99 = allgemein
                        =0 = BRAND = SYSBRAND
                        =1 = Objekttyp = SYSOBTYP
                        =2 = handelsgruppe = SYSPRHGROUP
                        =3 = Kundengruppe = SYSPRKGROUP
                        =4 = Vertriebspartnerrolle = SYSPEROLE 
                 */
                string obtypen = getObTypen(sysobtyp);
                if (obtypen.Length == 0)
                    obtypen = "-1";

                // Ticket#2012112910000198 
                string link = " and (ADJUSTMENTTRIGGER = 99 or (ADJUSTMENTTRIGGER = 0 and sysbrand = :sysbrand) or " +
                              " (ADJUSTMENTTRIGGER = 1 and sysobtyp in (" + obtypen + ") ) or (ADJUSTMENTTRIGGER = 2 and sysprhgroup = :sysprhgroup) or " +
                              " (ADJUSTMENTTRIGGER = 3 and sysprkgroup = :sysprkgroup) or (ADJUSTMENTTRIGGER = 4 and sysperole = :sysperole)) ";

                String Query = "select prprovadjstep.ADJRATE provstep, prprovadjstep.method method, prprovadjstep.ADJVAL provval " + 
                               " from prclprprovadj, prprovadj, prprovadjstep " + 
                               " where prprovadjstep.rank = :rank and prclprprovadj.sysprproduct = :sysprproduct and " + 
                               "       prclprprovadj.sysprprovadj = prprovadj.sysprprovadj and prprovadjstep.sysprprovadj = prprovadj.sysprprovadj " + link;

                try
                {
                    // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                    long sysprhgroup = PeroleHelper.DeliverPrHGroupList(_context, sysPerole, sysBrand).FirstOrDefault();

                    if (sysprhgroup > 0)
                    {
                        //Parameters for query
                        System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysbrand", Value = sysBrand},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprhgroup", Value = sysprhgroup},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysperole", Value = sysPerole},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprkgroup", Value = 0},
                            
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprproduct", Value = sysprproduct},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "rank", Value = rank}
                        };

                        PROVSTEPInfo rval = _context.ExecuteStoreQuery<PROVSTEPInfo>(Query, Parameters).FirstOrDefault();
                        _Log.Debug("DeliverProvStepInfo Duration: " + (DateTime.Now.TimeOfDay.Milliseconds - ctime));
                        stepInfo[key] = rval;
                    }
                    else throw new ArgumentException("Prhgroup konnte nicht ermittelt werden");
                }
                catch
                {
                    throw;
                }
            }
            return stepInfo[key];
        }

        /// <summary>
        /// Applies Adjustementsteps to the provrate with rank
        /// </summary>
        /// <param name="provrate"></param>
        /// <param name="sysprproduct"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="sysPerole"></param>
        /// <param name="sysBrand"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        public decimal DeliverProvRateAdjusted(decimal provrate, long sysprproduct, long sysobtyp, long sysPerole, long sysBrand, int rank)
        {
            PROVSTEPInfo rval = DeliverProvStepInfo(sysprproduct, sysobtyp, sysPerole, sysBrand, rank);
            return DeliverProvRateAdjusted(provrate, rval);
        }

        /// <summary>
        /// Applies Adjustementsteps to the provrate with rank
        /// </summary>
        /// <param name="provrate"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public decimal DeliverProvRateAdjusted(decimal provrate, PROVSTEPInfo info)
        {
            if (info != null)
            {
                //method 0=prozent, 1=prozentpunkte, 2=überschreiben
                if (info.method == 0)
                {
                    provrate = provrate + provrate * info.provstep / 100;
                }
                else if (info.method == 1)
                {
                    provrate = provrate + info.provstep;
                }
                else if (info.method == 2)
                {
                    provrate = info.provstep;
                }
                else if (info.method == 3)
                {
                    provrate = provrate + info.provval;
                }
                else if (info.method == 4)
                {
                    provrate = info.provval;
                }
            }
            return provrate;
        }

        private static CacheDictionary<String, PROVRATE> rateAdjCache = CacheFactory<String, PROVRATE>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        /// <summary>
        /// Applies Adjustementsteps to the provrate with rank
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="sysPerole"></param>
        /// <param name="sysBrand"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        public PROVRATE DeliverProvRateAdjusted(long sysprproduct, long sysobtyp, long sysPerole, long sysBrand, int rank)
        {
            String key = sysprproduct+"_"+sysobtyp+"_"+sysPerole+"_"+sysBrand+"_"+rank;

            if (!rateAdjCache.ContainsKey(key))
            {
                double ctime = DateTime.Now.TimeOfDay.Milliseconds;

                /*
                        ADJUSTMENTTRIGGER = 99 = allgemein
                        =0 = BRAND = SYSBRAND
                        =1 = Objekttyp = SYSOBTYP
                        =2 = handelsgruppe = SYSPRHGROUP
                        =3 = Kundengruppe = SYSPRKGROUP
                        =4 = Vertriebspartnerrolle = SYSPEROLE 
                 */
                string obtypen = getObTypen(sysobtyp);
                if (obtypen.Length == 0)
                    obtypen = "-1";

                // Ticket#2012112910000198 
                string link = " and (ADJUSTMENTTRIGGER = 99 or (ADJUSTMENTTRIGGER = 0 and sysbrand = :sysbrand) or " +
                              " (ADJUSTMENTTRIGGER = 1 and sysobtyp in (" + obtypen + ") ) or (ADJUSTMENTTRIGGER = 2 and sysprhgroup = :sysprhgroup) or " +
                              " (ADJUSTMENTTRIGGER = 3 and sysprkgroup = :sysprkgroup) or (ADJUSTMENTTRIGGER = 4 and sysperole = :sysperole)) ";

                String Query = "select prprovadjstep.ADJRATE provstep, prprovadjstep.method method " +
                               " from prclprprovadj, prprovadj, prprovadjstep " +
                               " where prprovadjstep.rank = :rank and prclprprovadj.sysprproduct = :sysprproduct and " +
                               "       prclprprovadj.sysprprovadj = prprovadj.sysprprovadj and prprovadjstep.sysprprovadj = prprovadj.sysprprovadj " + link;

                try
                {
                    // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                    long sysprhgroup = PeroleHelper.DeliverPrHGroupList(_context, sysPerole, sysBrand).FirstOrDefault();

                    if (sysprhgroup > 0)
                    {
                        PROVRATE tarif = DeliverProvRate(sysprhgroup, sysPerole, sysBrand, rank);
                        if (tarif == null) return null;

                        //Parameters for query
                        System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysbrand", Value = sysBrand},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprhgroup", Value = sysprhgroup},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysperole", Value = sysPerole},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprkgroup", Value = 0},
                            
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprproduct", Value = sysprproduct},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "rank", Value = rank}
                           
                        };


                        PROVSTEPInfo rval = _context.ExecuteStoreQuery<PROVSTEPInfo>(Query, Parameters).FirstOrDefault();
                        if (rval != null)
                        {
                            //method 0=prozent, 1=prozentpunkte, 2=überschreiben
                            if (rval.method == 0)
                            {
                                tarif.PROVRATE1 = tarif.PROVRATE1 + tarif.PROVRATE1 * rval.provstep / 100;
                            }
                            else if (rval.method == 1)
                            {
                                tarif.PROVRATE1 = tarif.PROVRATE1 + rval.provstep;
                            }
                            else if (rval.method == 2)
                            {
                                tarif.PROVRATE1 = rval.provstep;
                            }
                        }
                        _Log.Debug("DeliverProvRateAdjusted Duration: " + (DateTime.Now.TimeOfDay.Milliseconds - ctime));
                        rateAdjCache[key]= tarif;
                    }
                    else throw new ArgumentException("Prhgroup konnte nicht ermittelt werden");
                }
                catch
                {
                    throw;
                }

            }
            return rateAdjCache[key];
        }

        private static CacheDictionary<String, PROVRATE> rateCache = CacheFactory<String, PROVRATE>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        /// <summary>
        /// returns the provision rate
        /// </summary>
        /// <param name="sysPerole"></param>
        /// <param name="sysBrand"></param>
        /// <param name="rank">Provision type</param>
        /// <param name="sysprhgroup">sysprhgroup</param>
        /// <returns>PROVRATE or null if not configured</returns>
        private PROVRATE DeliverProvRate(long sysprhgroup, long sysPerole, long sysBrand, int rank)
        {
            String key = sysprhgroup + "_" + sysPerole + "_" + sysBrand + "_" + rank;
            if (!rateCache.ContainsKey(key))
            {
                double ctime = DateTime.Now.TimeOfDay.Milliseconds;
                PROVRATE rval = null;

                //Parameters for query
                System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysBRAND", Value = sysBrand},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysPrHGroup", Value = sysprhgroup},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysPEROLE", Value = sysPerole},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "provsteprank", Value = rank}
                           
                        };

                string Query = "SELECT p.*,p.provrate provrate1 FROM provrate p,TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailPROVRATE(:SysBRAND, :SysPrHGroup,  :SysPEROLE , :provsteprank)) t where p.sysprovrate = t.sysid";
                rval = _context.ExecuteStoreQuery<PROVRATE>(Query, Parameters).FirstOrDefault();



                _Log.Debug("DeliverProvRate Duration: " + (DateTime.Now.TimeOfDay.Milliseconds - ctime));

                rateCache[key]= rval;
            }
            return rateCache[key];
        }

        private static CacheDictionary<String, List<PROVTARIF>> tarifeCache = CacheFactory<String, List<PROVTARIF>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        /// <summary>
        /// Delivers all Provtarife (Vorzug, Standard, 1, 2, 3) for e.g. Abschlussprovision
        /// no PRPRODUCT-Dependancies
        /// </summary>
        /// <param name="sysPerole"></param>
        /// <param name="sysBrand"></param>
        /// <param name="sysprhgroup"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        public List<PROVTARIF> DeliverProvTarife(long sysPerole, long sysBrand, long sysprhgroup, int rank)
        {
            String key = sysPerole+"_"+sysBrand+"_"+sysprhgroup+"_"+rank;

            if (!tarifeCache.ContainsKey(key))
            {
                double ctime = DateTime.Now.TimeOfDay.Milliseconds;
                List<PROVTARIF> rval = null;
                if (sysprhgroup == 0)
                {
                    // Get VP PeRole
                    long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(_context, sysPerole,  PeroleHelper.CnstVPRoleTypeNumber);

                    // Get PrHGroup - in BMW there can be only one - thats why FirstOrDefault
                    PRHGROUP PrHGroup = PeroleHelper.DeliverVPPrHGroupList(_context, sysBrand, sysVpPeRole).FirstOrDefault<PRHGROUP>();
                    if (PrHGroup != null)
                        sysprhgroup = PrHGroup.SYSPRHGROUP;
                }
                try
                {



                    //Parameters for query
                    System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysBRAND", Value = sysBrand},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysPrHGroup", Value = sysprhgroup},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysPEROLE", Value = sysPerole},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "provsteprank", Value = rank}
                           
                        };

                    string Query = "SELECT p.* FROM provtarif p,TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailPROVTARIF(:SysBRAND, :SysPrHGroup,  :SysPEROLE , :provsteprank)) t where p.sysprovtarif = t.sysid";
                    rval = _context.ExecuteStoreQuery<PROVTARIF>(Query, Parameters).ToList();


                }
                catch
                {
                    throw;
                }

                _Log.Debug("ProvTarife Duration: " + (DateTime.Now.TimeOfDay.Milliseconds - ctime));
                tarifeCache[key]= rval;
            }
            return tarifeCache[key];
        }

        /// <summary>
        /// Calculates the Provisions by the given rank
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ProvisionDto DeliverProvision(ProvisionDto param)
        {
            IProvisionCalculator calc = ProvisionCalcFactory.createCalculator((ProvisionTypeConstants)param.rank);
            ProvisionDto rval = calc.calculate(_context, this, param);
            if (param.noProvision )//#3601
            {
                rval.provision = 0;
                rval.zugangsProvision = 0;
            }
            return rval;

        }


        public List<PROVTARIF> deliverAdjustedTarife(ProvisionDto param)
        {
            double ctime = DateTime.Now.TimeOfDay.Milliseconds;
            List<PROVTARIF> rval = new List<PROVTARIF>();
            PROVTARIF def = new PROVTARIF();
            def.SYSPROVTARIF = 0;
            def.NAME = "Individuell";

            List<PROVTARIF> tarife = DeliverProvTarife(param.sysPerole, param.sysBrand, param.sysPrhgroup, param.rank);
            if (tarife==null || tarife.Count == 0)
            {
                def.STANDARDFLAG = 1;
                rval.Add(def);
                return rval;
            }

           

            List<PROVTARIF> tarife2 = new List<PROVTARIF>();
            foreach (PROVTARIF pt in tarife)
            {
                PROVTARIF pt2 = new PROVTARIF();
                pt2.NAME = pt.NAME;
                pt2.PROVRATE = pt.PROVRATE;
                pt2.SYSPROVTARIF = pt.SYSPROVTARIF;
                pt2.STANDARDFLAG = pt.STANDARDFLAG;
                tarife2.Add(pt2);

            }
            var tQuery = from t in tarife2
                         orderby t.PROVRATE
                         select t;

            bool noInd = false;
            bool hasZero = false;
            bool hasJustOne = false;
            PROVSTEPInfo info = DeliverProvStepInfo(param.sysprproduct, param.sysobtyp, param.sysPerole, param.sysBrand, param.rank);
            foreach (PROVTARIF tarif in tQuery)
            {
                
                if (info == null)//Standardtarif immer direkt übernehmen?// || tarif.STANDARDFLAG==1
                {
                    tarif.NAME += " (" + tarif.PROVRATE + ")";
                    rval.Add(tarif);//no adjustment
                    continue;
                }
                if (hasJustOne) continue;
                decimal tarifadj = DeliverProvRateAdjusted((decimal)tarif.PROVRATE, info);
                tarif.PROVRATE = tarifadj;
                tarif.NAME += " (" + tarif.PROVRATE + ")";
                if (info.method == 2)//überschreiben von einem wert -> nur mehr den ersten zurückgeben
                {
                    //noInd = true;

                    if (tarif.PROVRATE == 0) tarif.NAME = "Keine Provision";
                    else tarif.NAME = "Produktprovision";
                   
                    rval.Add(tarif);
                    hasJustOne = true;
                    break;
                }
                if (tarifadj <= 0 && !hasZero)//only one zero entry
                {
                    hasZero = true;
                    rval.Add(tarif);
                    continue;
                }
                if (tarifadj <= 0) continue;
                
                rval.Add(tarif);
            }
            if (!noInd)
            {
                
                rval.Insert(0, def);
            }
            _Log.Debug("AdjustedTarife Duration: " + (DateTime.Now.TimeOfDay.Milliseconds - ctime));
            return rval;
        }


    }
}