using Cic.OpenLease.Service.Versicherung;
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
    public class  PersonName
        {
            public String NAME { get; set; }
            public String VORNAME { get; set; }
        }
    /// <summary>
    /// Data Access Object for Insurances
    /// </summary>
    [System.CLSCompliant(true)]
    public class VSTYPDao
    {
        #region Private variables
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DdOlExtended _context;

        #endregion
        public static DateTime nullDate = new DateTime(111, 1, 1);
        private static CacheDictionary<long, VSTYP> vstypCache = CacheFactory<long, VSTYP>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, VSTYPDto> vstypCache2 = CacheFactory<long, VSTYPDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        #region Constructors
        public VSTYPDao(DdOlExtended context)
        {
            _context = context;
        }
        #endregion

        public VSTYP getVsTyp(long sysvstyp)
        {
            if (!vstypCache.ContainsKey(sysvstyp))
            {
                List<VSTYP> vstypen = (from c in _context.VSTYP
                                      select c).ToList();//_context.ExecuteStoreQuery<VSTYP>("select * from vstyp", null).ToList();
                foreach (VSTYP vs in vstypen)
                {
                    

                    if (vs.VSART == null)
                        _context.Entry(vs).Reference(f => f.VSART).Load();

                    vstypCache.MergeSafe(vs.SYSVSTYP, vs);
                }
            }
            if (!vstypCache.ContainsKey(sysvstyp))
                return null;
            return vstypCache[sysvstyp];
        }

        public VSTYPDto getVsTypDto(long sysvstyp)
        {
            if (!vstypCache2.ContainsKey(sysvstyp))
            {
                List<VSTYPDto> vstypen = _context.ExecuteStoreQuery<VSTYPDto>("select * from vstyp", null).ToList();
                foreach (VSTYPDto vs in vstypen)
                    vstypCache2.MergeSafe((long)vs.SYSVSTYP, vs);
            }
            if (!vstypCache2.ContainsKey(sysvstyp))
                return null;
            return vstypCache2[sysvstyp];
        }
        /// <summary>
        /// get all vstypen for the vsart
        /// </summary>
        /// <param name="sysvsart"></param>
        /// <param name="vsperson"></param>
        /// <param name="insurances"></param>
        /// <returns></returns>
        public List<VSTYPDto> getVSTYPList(long sysvsart, long vsperson, List<PRVSDto> insurances)
        {
            List<VSTYPDto> rval = new List<VSTYPDto>();

            var q = from c in insurances
                    where c.SYSVSART == sysvsart && c.SYSPERSON == vsperson
                    group c by c.SYSVSTYP;

            foreach (var group in q)
            {
                PRVSDto info = group.First();
                
                VSTYPDto tmp = getVsTypDto(info.SYSVSTYP);
                if (tmp.CODEMETHOD == null || tmp.CODEMETHOD.Length == 0) continue;//dont allow vstyp with empty codemethod
                if (tmp.ACTIVEFLAG.HasValue && tmp.ACTIVEFLAG.Value == 0) continue;//no inactive insurances
                Boolean isValid = true;
                // Check Valid from
                if (tmp.VALIDFROM.HasValue)
                {
                    isValid = isValid && (tmp.VALIDFROM.Value.Date <= System.DateTime.Now.Date || tmp.VALIDFROM.Value.Date <= nullDate);
                }

                // Check Valid until
                if (tmp.VALIDUNTIL.HasValue)
                {
                    isValid = isValid && (tmp.VALIDUNTIL.Value.Date >= System.DateTime.Now.Date || tmp.VALIDUNTIL.Value.Date <= nullDate);
                }
                if (!isValid) continue;

                tmp.NEEDED = info.NEEDED;
                tmp.DISABLED = info.DISABLEDFLAG;
                tmp.FLAGDEFAULT = info.FLAGDEFAULT>0?true:false;
             
                rval.Add(tmp);
            }
            return rval;
        }

        public PersonName getVSPERSON(long sysvstyp)
        {

            PersonName tmp = _context.ExecuteStoreQuery<PersonName>("select vs.name, vs.vorname from vs,vstyp where vs.sysperson=vstyp.sysvs and vstyp.sysvstyp=" + sysvstyp, null).FirstOrDefault();
            return tmp;
        }

        private static CacheDictionary<long, PERSONDto> personCache = CacheFactory<long, PERSONDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        public List<PERSONDto> getVSPERSON(long sysvsart, List<PRVSDto> insurances)
        {
            List<PERSONDto> rval = new List<PERSONDto>();

            var q = from c in insurances
                    where c.SYSVSART == sysvsart
                    group c by c.SYSPERSON;

            foreach (var group in q)
            {
                PRVSDto info = group.First();
                PERSONDto tmp = null;
                if (!personCache.ContainsKey((long)info.SYSPERSON))
                {
                    tmp = _context.ExecuteStoreQuery<PERSONDto>("select sysperson,name,vorname,code,privatflag from vs where sysperson=" + info.SYSPERSON, null).First();
                    personCache[(long)info.SYSPERSON] = tmp;
                }
                tmp = new PERSONDto(personCache[(long)info.SYSPERSON]);

                tmp.NEEDED = info.NEEDED;
                tmp.DISABLED = info.DISABLEDFLAG;

                rval.Add(tmp);
            }
            return rval;
        }

        private static CacheDictionary<long, VSARTDto> vsartCache = CacheFactory<long, VSARTDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        public List<VSARTDto> getVSART(List<PRVSDto> insurances)
        {
            List<VSARTDto> rval = new List<VSARTDto>();

            var q = from c in insurances
                    group c by c.SYSVSART;

            foreach (var group in q)
            {
                PRVSDto info = group.First();
                VSARTDto tmp = null;
                if (!vsartCache.ContainsKey((long)info.SYSVSART))
                {
                    tmp = _context.ExecuteStoreQuery<VSARTDto>("select * from vsart where sysvsart=" + info.SYSVSART, null).First();
                    vsartCache[(long)info.SYSVSART] = tmp;
                }
                tmp = new VSARTDto(vsartCache[(long)info.SYSVSART]);
                tmp.NEEDED = info.NEEDED;
                tmp.DISABLED = info.DISABLEDFLAG;

                rval.Add(tmp);
            }
            return rval;
        }
        private static CacheDictionary<long, List<PRVSDto>> insCache = CacheFactory<long, List<PRVSDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        /// <summary>
        /// Returns all VSTYP-Infos configured for the product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public List<PRVSDto> getAllInsurances(long sysprproduct)
        {
            if (!insCache.ContainsKey(sysprproduct))
            {

                String query = "select defaultflag,disabledflag,needed,flagdefault,sysprproduct, vsart.sysvsart,vstyp.sysvstyp, vs.sysperson from (SELECT *  FROM  CIC.PRVSOB_V  union all select * from CIC.PRVSP_V) v1,vsart,vstyp,vs where vsart.sysvsart = v1.sysvsart AND v1.method=1 AND vsart.sysvsart = vstyp.sysvsart AND vs.sysperson = vstyp.sysvs and sysprproduct=" + sysprproduct +
    " union all select defaultflag,disabledflag,needed,flagdefault,sysprproduct, vsart.sysvsart,vstyp.sysvstyp, vs.sysperson from (SELECT *  FROM  CIC.PRVSOB_V  union all select * from CIC.PRVSP_V) v1,vsart,vstyp,vs where vs.sysperson = v1.sysperson AND v1.method=2 AND vsart.sysvsart = vstyp.sysvsart AND vs.sysperson = vstyp.sysvs  and sysprproduct=" + sysprproduct +
    " union all select defaultflag,disabledflag,needed,flagdefault,sysprproduct, vsart.sysvsart,vstyp.sysvstyp, vs.sysperson from (SELECT *  FROM  CIC.PRVSOB_V  union all select * from CIC.PRVSP_V) v1,vsart,vstyp,vs where vstyp.sysvstyp = v1.sysvstyp AND v1.method=3 AND vsart.sysvsart = vstyp.sysvsart AND vs.sysperson = vstyp.sysvs and sysprproduct=" + sysprproduct;

                List<PRVSDto> values = _context.ExecuteStoreQuery<PRVSDto>(query, null).ToList();
                List<PRVSDto> rval = new List<PRVSDto>();
                var q = from c in values
                        where !(c.NEEDED == 0 && c.DISABLEDFLAG == 1)
                        select c;
                int defaults = 0;
                foreach (PRVSDto row in values)
                {
                    defaults += row.DEFAULTFLAG;
                }
                bool useVSTYPDefault = defaults == 0;//HCERZWEI-1890 Gibt der folgende Selekt 0 zurück, dann den Default aus dem VSTYP holen, sonst eben aus der geflaggten Gruppenposition.

                //group by sysvstyp and take the ones that are needed
                Dictionary<long, PRVSDto> groupMap = new Dictionary<long, PRVSDto>();
                //Default-settings are set by the Field FLAGDEFAULT
                //however the datamodel contains a default in DEFAULTFLAG for the vstyp positions and FLAGDEFAULT for the vstyp setting
                foreach (PRVSDto row in q)
                {
                    if (!groupMap.ContainsKey(row.SYSVSTYP))
                    {
                        if (row.DEFAULTFLAG > 0)//use defaultflag from view for default-handling now. Write to old field flagdefault
                            row.FLAGDEFAULT = 1;
                        else//wenn die gruppenposition kein default gesetzt hat
                        {
                            //wenn die gesamte gruppe noch keinen default gesetzt hat
                            if (useVSTYPDefault)
                                row.FLAGDEFAULT = row.FLAGDEFAULT;//dann den default aus vstyp nehmen
                            else
                                row.FLAGDEFAULT = 0;//sonst kein default gesetzt , also 0
                        }
                        groupMap[row.SYSVSTYP] = row;                        
                    }
                    else
                    {
                        PRVSDto lrow = groupMap[row.SYSVSTYP];
                        if (row.NEEDED > 0)
                            lrow.NEEDED = 1;

                        if (row.DEFAULTFLAG > 0)//use defaultflag from view for default-handling now. Write to old field flagdefault
                            lrow.FLAGDEFAULT = 1;
                        else//wenn die gruppenposition kein default gesetzt hat
                        {
                            //wenn die gesamte gruppe noch keinen default gesetzt hat
                            if (useVSTYPDefault)
                                lrow.FLAGDEFAULT = lrow.FLAGDEFAULT;//dann den default aus vstyp nehmen
                            else
                                lrow.FLAGDEFAULT = 0;//sonst kein default gesetzt , also 0
                        }
                        if (row.DISABLEDFLAG > 0)
                            lrow.DISABLEDFLAG = 1;
                    }
                }

                foreach (long key in groupMap.Keys)
                {
                    rval.Add(groupMap[key]);
                }


                insCache[sysprproduct] = rval;
            }
            return insCache[sysprproduct];
        }

        /// <summary>
        /// Returns all allowed SYSVSTYP-Ids for the given context parameters
        /// </summary>
        /// <param name="sysPrkGroup"></param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="sysKdTyp"></param>
        /// <returns></returns>
        public List<long> getAvailabilites(long sysPrkGroup, long sysObTyp, long sysObArt, long sysKdTyp)
        {
            String query = "SELECT vstyp.sysvstyp  FROM vstyp WHERE " +
        " ( (select count(*) from prclvstypkdtyp where prclvstypkdtyp.syskdtyp=:sysKdTyp and " 
        // + " (prclvstypkdtyp.validfrom IS NULL OR prclvstypkdtyp.validfrom <= TRUNC(SYSDATE))  AND (prclvstypkdtyp.validuntil IS NULL OR prclvstypkdtyp.validuntil >= TRUNC(SYSDATE)) "
        + SQL.CheckCurrentSysDate ("prclvstypkdtyp")
        + " and  prclvstypkdtyp.sysvstyp=vstyp.sysvstyp and activeflag=1)>0 or  (select count(*) from prclvstypkdtyp where prclvstypkdtyp.sysvstyp=vstyp.sysvstyp and "
        // + " (prclvstypkdtyp.validfrom IS NULL OR prclvstypkdtyp.validfrom <= TRUNC(SYSDATE))  AND (prclvstypkdtyp.validuntil IS NULL OR prclvstypkdtyp.validuntil >= TRUNC(SYSDATE)) "
        + SQL.CheckCurrentSysDate ("prclvstypkdtyp")
        + " and activeflag=1)=0 ) " +
        " and  ( (select count(*) from prclvstypobart where prclvstypobart.sysobart=:sysObArt and "
        // + " (prclvstypobart.validfrom IS NULL OR prclvstypobart.validfrom <= TRUNC(SYSDATE))  AND (prclvstypobart.validuntil IS NULL OR prclvstypobart.validuntil >= TRUNC(SYSDATE)) "
        + SQL.CheckCurrentSysDate ("prclvstypobart")
        + " and  prclvstypobart.sysvstyp=vstyp.sysvstyp and activeflag=1)>0 or  (select count(*) from prclvstypobart where prclvstypobart.sysvstyp=vstyp.sysvstyp  and "
        // + " (prclvstypobart.validfrom IS NULL OR prclvstypobart.validfrom <= TRUNC(SYSDATE))  AND (prclvstypobart.validuntil IS NULL OR prclvstypobart.validuntil >= TRUNC(SYSDATE)) "
        + SQL.CheckCurrentSysDate ("prclvstypobart")
        + " and activeflag=1)=0 ) " +
        " and  ( (select count(*) from prclvstypobtyp where prclvstypobtyp.sysobtyp in (select sysobtyp from obtyp start with sysobtyp=:sysObTyp connect by prior sysobtypp=sysobtyp) and "
        // + " (prclvstypobtyp.validfrom IS NULL OR prclvstypobtyp.validfrom <= TRUNC(SYSDATE))  AND (prclvstypobtyp.validuntil IS NULL OR prclvstypobtyp.validuntil >= TRUNC(SYSDATE)) "
        + SQL.CheckCurrentSysDate ("prclvstypobtyp")
        + " and  prclvstypobtyp.sysvstyp=vstyp.sysvstyp and activeflag=1)>0 or  (select count(*) from prclvstypobtyp where prclvstypobtyp.sysvstyp=vstyp.sysvstyp  and "
        // + " (prclvstypobtyp.validfrom IS NULL OR prclvstypobtyp.validfrom <= TRUNC(SYSDATE))  AND (prclvstypobtyp.validuntil IS NULL OR prclvstypobtyp.validuntil >= TRUNC(SYSDATE)) "
        + SQL.CheckCurrentSysDate ("prclvstypobtyp")
        + " and activeflag=1)=0 ) " +
        " and  ( (select count(*) from prclvstypkgrp where prclvstypkgrp.sysprkgroup=:sysPrkGroup and "
        // + " (prclvstypkgrp.validfrom IS NULL OR prclvstypkgrp.validfrom <= TRUNC(SYSDATE))  AND (prclvstypkgrp.validuntil IS NULL OR prclvstypkgrp.validuntil >= TRUNC(SYSDATE)) "
        + SQL.CheckCurrentSysDate ("prclvstypkgrp")
        + " and  prclvstypkgrp.sysvstyp=vstyp.sysvstyp and activeflag=1)>0 or  (select count(*) from prclvstypkgrp where prclvstypkgrp.sysvstyp=vstyp.sysvstyp  and "
        // + " (prclvstypkgrp.validfrom IS NULL OR prclvstypkgrp.validfrom <= TRUNC(SYSDATE))  AND (prclvstypkgrp.validuntil IS NULL OR prclvstypkgrp.validuntil >= TRUNC(SYSDATE)) "
        + SQL.CheckCurrentSysDate ("prclvstypkgrp")
        + " and activeflag=1)=0 ) ";

            //Parameters for query
            System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrkGroup", Value = sysPrkGroup}, 
                            
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObTyp", Value = sysObTyp},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObArt", Value = sysObArt}
                            ,new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysKdTyp", Value = sysKdTyp}
                        };
            return _context.ExecuteStoreQuery<long>(query, Parameters).ToList();

        }
        /// <summary>
        /// Delivers all Insurances for the given parameters
        /// </summary>
        /// <param name="sysPrkGroup">Kundengruppe</param>
        /// <param name="sysVsArt">kind of insurance</param>
        /// <param name="sysVs">PERSON (company) attached to the insurance</param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="sysKdTyp"></param>
        /// <returns></returns>
        public List<VSTYPDto> DeliverVsTyp(long sysPrkGroup, long sysVsArt, long sysVs, long sysObTyp, long sysObArt, long sysKdTyp)
        {

            List<VSTYPDto> VsTypDtoList = null;

            try
            {

                //Parameters for query
                System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrkGroup", Value = sysPrkGroup}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysVsArt", Value = sysVsArt},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysVs", Value = sysVs},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObTyp", Value = sysObTyp},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObArt", Value = sysObArt}
                            ,new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysKdTyp", Value = sysKdTyp}
                        };

                string Query = "SELECT distinct p.*,NVL(ob.needed,0)+NVL(per.needed,0) NEEDED,NVL(ob.disabledflag,0) + NVL(per.disabledflag,0) DISABLED  FROM vstyp p  left outer join CIC.PRVSOB_V ob on ob.SYSVSTYP=p.sysvstyp  left outer join CIC.PRVSP_V per on per.SYSVSTYP=p.sysvstyp, TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailVsTyp(:sysPrkGroup, :sysVsArt, :sysVs, :sysObTyp, :sysObArt, :sysKdTyp)) t where p.sysvstyp = t.sysid and " 
                    // + " (p.validfrom IS NULL OR p.validfrom <= TRIM(SYSDATE))  AND (p.validuntil IS NULL OR p.validuntil >= TRIM(SYSDATE)) "
                    + SQL.CheckCurrentSysDate ("p")
                    ;
                //string Query = "SELECT p.* FROM vstyp p,TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailVsTypNew(:sysPrkGroup, :sysVsArt, :sysVs, :sysObTyp, :sysObArt, :sysKdTyp)) t where p.sysvstyp = t.sysid";
                VsTypDtoList = _context.ExecuteStoreQuery<VSTYPDto>(Query, Parameters).ToList();

            }
            catch
            {
                throw;
            }


            return VsTypDtoList;
        }

        public InsuranceResultDto DeliverVSData(long sysPerole, long sysBrand, InsuranceParameterDto param)
        {
           

           
            VSTYP vsType = this.getVsTyp(param.SysVSTYP);
            
            try
            {
                IVSCalculator calc = VSCalcFactory.createCalculator(vsType.CODEMETHOD);

                return calc.calculate(_context, sysPerole, sysBrand, vsType, param);
            }
            catch (Exception ex)
            {
                _log.Error("Insurance-Calculation failed: " + ex.Message, ex);
                throw;
            }
        }
    }
}