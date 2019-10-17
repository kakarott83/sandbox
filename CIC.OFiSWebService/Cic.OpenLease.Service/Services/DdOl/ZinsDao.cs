using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{


    /// <summary>
    /// Caches the access to the prisma zins configuration
    /// </summary>
    public class ZinsDao
    {

        private List<IntsDto> band;
        private List<IntsDto> matu;
        private List<IntsDto> raten;
        private List<IntstrctDto> intstrct;
        private List<IntstrctDto> intstrctvz;
        private List<InterestConditionLink> intstep;
        private List<PRCLPRINTSETDto> productLinks;
        private List<PRINTSETDto> intgroups;
        private DateTime nullDate = new DateTime(111, 1, 1);

        private ThreadSafeDictionary<ConditionLinkType, string> mapConditionLinkToParameter = new ThreadSafeDictionary<ConditionLinkType, string>();

        private ObTypDao obDao;
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ZinsDao(DdOlExtended context)
        {
            mapConditionLinkToParameter[ConditionLinkType.BRAND] = "sysbrand";
            mapConditionLinkToParameter[ConditionLinkType.OBTYP] = "sysobtyp";
            mapConditionLinkToParameter[ConditionLinkType.OBART] = "sysobart";
            mapConditionLinkToParameter[ConditionLinkType.PRKGROUP] = "sysprkgroup";
            mapConditionLinkToParameter[ConditionLinkType.PRHGROUP] = "sysprhgroup";            
            mapConditionLinkToParameter[ConditionLinkType.PEROLE] = "sysperole";            
            mapConditionLinkToParameter[ConditionLinkType.INTTYPE] = "sysinttype";

        
            _Log.Info("Caching Zins...");
            intstrct = context.ExecuteStoreQuery<IntstrctDto>("select INTSTRCT.SYSINTSTRCT, SYSPRPRODUCT,INTSDATE.SYSINTSDATE,METHOD,INTSDATE.VALIDFROM from intstrct,intsdate,prproduct where prproduct.sourcebasis=1 and prproduct.sysintstrct=intstrct.sysintstrct   and intstrct.sysintstrct = intsdate.sysintstrct", null).ToList();
            intstrctvz = context.ExecuteStoreQuery<IntstrctDto>("select INTSTRCT.SYSINTSTRCT, INTSDATE.SYSINTSDATE,METHOD,INTSDATE.VALIDFROM from intstrct,intsdate where  intstrct.sysintstrct = intsdate.sysintstrct", null).ToList();
            _Log.Info("STRUCT: " + intstrct[0].SYSINTSTRCT);
            raten = context.ExecuteStoreQuery<IntsDto>("select * from intsrate", null).ToList();
            band = context.ExecuteStoreQuery<IntsDto>("select * from intsband", null).ToList();
            matu = context.ExecuteStoreQuery<IntsDto>("select * from intsmatu", null).ToList();
            intstep = context.ExecuteStoreQuery<InterestConditionLink>("select * from PRINTSTEP", null).ToList();
            productLinks = context.ExecuteStoreQuery<PRCLPRINTSETDto>("select * from PRCLPRINTSET", null).ToList();
            intgroups = context.ExecuteStoreQuery<PRINTSETDto>("select * from PRINTSET where activeflag=1", null).ToList();
            _Log.Info("Cached Zins.");
           

            obDao = new ObTypDao();

        }

        public decimal DeliverZinsBasis(long sysPRPRODUCT, DateTime date, long lz, decimal amount)
        {
            //return 0;


            var q = from s in intstrct
                    where s.SYSPRPRODUCT == sysPRPRODUCT
                    && s.VALIDFROM <= date
                    orderby s.VALIDFROM descending
                    select s;

            IntstrctDto strct = q.FirstOrDefault();
            if (strct == null)
                throw new ApplicationException("No intstrct for product " + sysPRPRODUCT + " date " + date);
            return getIntstrctValue(strct, raten, band, matu, lz, amount);

         
        }
        public decimal DeliverZins(long sysPRPRODUCT, DateTime date, long term, decimal amount, long sysBrand, long sysObType, long sysObArt, long sysPRKGroup, long sysPRHGroup, long sysPeRole, long sysINTTYPE, bool excludeSubvRates)
        {
            decimal bzins = DeliverZinsBasis(sysPRPRODUCT, date, term, amount);
            _Log.Debug("BasisZins: " + bzins + " for " + sysPRPRODUCT+"_"+date+"_"+term+"_"+amount+"_"+sysBrand+"_"+sysObType+"_"+sysObArt+"_"+sysPRKGroup+"_"+sysPRHGroup+"_"+sysPeRole+"_"+sysINTTYPE);
            return DeliverZinsSchritte(sysPRPRODUCT, term,amount,date, bzins, sysBrand, sysObType, sysObArt, sysPRKGroup, sysPRHGroup, sysPeRole, sysINTTYPE, excludeSubvRates);
        }

        public decimal DeliverVerZinsung(long term, decimal amount, long SysINTSTRCT)
        {
            //return 0;
            var q = from s in intstrctvz
                    where s.SYSINTSTRCT==SysINTSTRCT && s.VALIDFROM <= System.DateTime.Now
                    orderby s.VALIDFROM descending
                    select s;

            IntstrctDto strct = q.FirstOrDefault();
            if (strct == null)
                return 0;
            return getIntstrctValue(strct, raten, band, matu, term, amount);

        }

        public decimal DeliverZinsSchritte(long sysPRPRODUCT, long lz, decimal amount,DateTime date, decimal zinsBase, long sysBrand, long sysObType, long sysObArt, 
            long sysPRKGroup, long sysPRHGroup, long sysPeRole, long sysINTTYPE, bool pExcludeSubvRates)
        {

            decimal rval = zinsBase;

            
            List<PRCLPRINTSETDto> prlinks = productLinks.Where(a => a.SYSPRPRODUCT == sysPRPRODUCT).OrderBy(a => a.RANK).ToList();
            List<InterestConditionLink> allsteps = intstep;

            ZinsKontextDto ctx = new ZinsKontextDto();
            ctx.perDate = date;
            ctx.sysbrand = sysBrand;
            ctx.sysobtyp = sysObType;
            ctx.sysobart = sysObArt;
            ctx.sysprkgroup = sysPRKGroup;
            ctx.sysprhgroup = sysPRHGroup;
            ctx.sysperole = sysPeRole;
            ctx.sysinttype = sysINTTYPE;

            foreach (PRCLPRINTSETDto prlink in prlinks)//all product links to interest groups
            {
                List<PRINTSETDto> zinsgroups = intgroups.Where(z => z.SYSPRINTSET == prlink.SYSPRINTEST &&
                    (z.VALIDFROM == null || z.VALIDFROM <= date || z.VALIDFROM <= nullDate)
                                           && (z.VALIDUNTIL == null || z.VALIDUNTIL >= date || z.VALIDUNTIL <= nullDate)).ToList();

                foreach (PRINTSETDto group in zinsgroups)//all interest groups
                {
                    List<InterestConditionLink> groupsteps = allsteps.Where(a => a.SYSPRINTSET == group.SYSPRINTSET).OrderBy(a => a.RANK).ToList();

                    List<InterestConditionLink> availsteps = new List<InterestConditionLink>();

                    foreach (InterestConditionLink step in groupsteps)
                    {
                        List<long> condvalues = ConditionLink.getParameter(ctx, step.CONDITIONTYPE, mapConditionLinkToParameter, obDao, DateTime.Now);
                        if (condvalues.Contains(step.CONDITIONID))
                            rval = applyStep(step, rval, lz, amount, raten, band, matu, intstrct);
                    }
                }
            }
            return rval;


       
        }

        /// <summary>
        /// applies the interest modification step to the given interest
        /// </summary>
        /// <param name="step"></param>
        /// <param name="zins"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <param name="raten"></param>
        /// <param name="band"></param>
        /// <param name="matu"></param>
        /// <param name="intstrcts"></param>
        /// <returns></returns>
        private decimal applyStep(InterestConditionLink step, decimal zins, long lz, decimal amount, List<IntsDto> raten, List<IntsDto> band, List<IntsDto> matu, List<IntstrctDto> intstrcts)
        {


            decimal szins = zins;
            switch (step.SOURCEBASIS)
            {
                case (0): //IBOR
                    break;
                case (1)://INTSTRCT
                    IntstrctDto strct = intstrcts.Where(a => a.SYSINTSTRCT == step.TARGETID).FirstOrDefault();
                    szins = getIntstrctValue(strct, raten, band, matu, lz, amount);
                    break;
                case (2)://VG
                    break;
                case (3):
                    szins = step.INTRATE;
                    break;
            }

            return applyMethod(step.METHOD, zins, szins);

        }

        /// <summary>
        /// applies val by the calculation method to the base interest rate
        /// </summary>
        /// <param name="method"></param>
        /// <param name="zins"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private static decimal applyMethod(long method, decimal zins, decimal val)
        {
            switch (method)
            {
                case 0://percent
                    return zins * val / 100;
                case 1://percentpoints
                    return zins + val;
                default://overwrite
                    return val;
            }
        }


        /// <summary>
        /// returns the interest structure value
        /// </summary>
        /// <param name="strct"></param>
        /// <param name="rate"></param>
        /// <param name="lz"></param>
        /// <param name="band"></param>
        /// <param name="matu"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private decimal getIntstrctValue(IntstrctDto strct, List<IntsDto> rate, List<IntsDto> band, List<IntsDto> matu, long lz, decimal amount)
        {
            IntsDto cstep = null;

            switch (strct.METHOD)
            {
                case 1://Einfache Zinsstaffel Datumsbezogen
                    cstep = (from i in rate
                             where i.SYSINTSDATE == strct.SYSINTSDATE
                             select i).FirstOrDefault();
                    break;
                case 2://Laufzeitbezogen
                    cstep = (from i in matu
                             where i.SYSINTSDATE == strct.SYSINTSDATE
                             && i.MATURITY == lz
                             select i).FirstOrDefault();
                    break;
                case 3://Betragsbezogen
                    cstep = (from i in band
                             where i.SYSINTSDATE == strct.SYSINTSDATE
                             && i.LOWERB <= amount && (i.UPPERB > amount || i.UPPERB == 0)
                             select i).FirstOrDefault();
                    break;
            }
            if (cstep == null)
                throw new ApplicationException("No INTS-Data for INTSDATE " + strct.SYSINTSDATE);

            return cstep.INTRATE + cstep.ADDRATE - cstep.REDRATE;
        }
    }



    /// <summary>
    /// Dto for Zins
    /// </summary>
    public class IntsDto
    {
        /// <summary>
        /// Interest Rate Date
        /// </summary>
        public long SYSINTSDATE
        {
            get;
            set;
        }

        /// <summary>
        /// Maturity
        /// </summary>
        public long MATURITY
        {
            get;
            set;
        }

        /// <summary>
        /// Upper Base
        /// </summary>
        public decimal UPPERB
        {
            get;
            set;
        }

        /// <summary>
        /// Lower Base
        /// </summary>
        public decimal LOWERB
        {
            get;
            set;
        }

        /// <summary>
        /// Interest Rate
        /// </summary>
        public decimal INTRATE
        {
            get;
            set;
        }

        /// <summary>
        /// Add Rate
        /// </summary>
        public decimal ADDRATE
        {
            get;
            set;
        }

        /// <summary>
        /// Reduced Rate
        /// </summary>
        public decimal REDRATE
        {
            get;
            set;
        }

    }

    public class IntstrctDto
    {
        /// <summary>
        /// Interest Rate Strict
        /// </summary>
        public long SYSINTSTRCT
        {
            get;
            set;
        }

        /// <summary>
        /// PR Product
        /// </summary>
        public long SYSPRPRODUCT
        {
            get;
            set;
        }

        /// <summary>
        /// Interest Rate Date
        /// </summary>
        public long SYSINTSDATE
        {
            get;
            set;
        }

        /// <summary>
        /// Method
        /// </summary>
        public long METHOD
        {
            get;
            set;
        }

        /// <summary>
        /// Valid From
        /// </summary>
        public DateTime VALIDFROM
        {
            get;
            set;
        }

    }


   

    /// <summary>
    /// Wraps Interest Conditions to a common ConditionLink Class
    /// 
    /// This is a template for the pattern
    ///     availability for one ConditionType, where ConditionType and all possible ConditionValues are in one row
    ///     e.g.  PRCLPARSET (sysA, sysB, sysC, AREA)
    ///     the conditionid is determined from the correct condition value column while reading the conditionid
    /// </summary>
    public class InterestConditionLink : ConditionLink
    {
        //condition-ids
        private long _sysbrand;
        private long _sysobtyp;
        private long _sysobart;
        private long _sysprkgroup;
        private long _sysprhgroup;
        private long _sysperole;
        private long _sysinttype;

        //condition-switch
        private long _adjustmenttrigger;


        //target-references
        /// <summary>
        /// source base
        /// </summary>
        public long SOURCEBASIS { get; set; }

        /// <summary>
        /// sysibor
        /// </summary>
        public long SYSIBOR { get; set; }

        /// <summary>
        /// sysintstrct
        /// </summary>
        public long SYSINTSTRCT { get; set; }

        /// <summary>
        /// sysvg
        /// </summary>
        public long SYSVG { get; set; }

        /// <summary>
        /// intrate
        /// </summary>
        public decimal INTRATE { get; set; }

        /// <summary>
        /// rank
        /// </summary>
        public long RANK { get; set; }

        /// <summary>
        /// method
        /// </summary>
        public long METHOD { get; set; }

        /// <summary>
        /// sysprintset
        /// </summary>
        public long SYSPRINTSET { get; set; }


        /// <summary>
        /// Condition ID
        /// </summary>
        new
        public long CONDITIONID
        {
            get
            {
                switch (_adjustmenttrigger)
                {
                    case (0):
                        return _sysbrand;
                    case (1):
                        return _sysobtyp;
                    case (2):
                        return _sysprhgroup;
                    case (3):
                        return _sysprkgroup;
                    case (4):
                        return _sysperole;
                    case (5):
                        return _sysobart;
                    case (6):
                        return _sysinttype;
                }
                return 0;
            }
        }

        /// <summary>
        /// Condition Type
        /// </summary>
        public ConditionLinkType CONDITIONTYPE
        {
            get
            {
                switch (_adjustmenttrigger)
                {
                    case (0):
                        return ConditionLinkType.BRAND;
                    case (1):
                        return ConditionLinkType.OBTYP;
                    case (2):
                        return ConditionLinkType.PRHGROUP;
                    case (3):
                        return ConditionLinkType.PRKGROUP;
                    case (4):
                        return ConditionLinkType.PEROLE;
                    case (5):
                        return ConditionLinkType.OBART;
                    case (6):
                        return ConditionLinkType.INTTYPE;
                    case (99):
                        return ConditionLinkType.COMMON;
                }
                return ConditionLinkType.NONE;
            }

            set
            {
                switch (value)
                {
                    case (ConditionLinkType.BRAND):
                        _adjustmenttrigger = 0;
                        break;
                    case (ConditionLinkType.OBTYP):
                        _adjustmenttrigger = 1;
                        break;
                    case (ConditionLinkType.PRHGROUP):
                        _adjustmenttrigger = 2;
                        break;
                    case (ConditionLinkType.PRKGROUP):
                        _adjustmenttrigger = 3;
                        break;
                    case (ConditionLinkType.PEROLE):
                        _adjustmenttrigger = 4;
                        break;
                    case (ConditionLinkType.OBART):
                        _adjustmenttrigger = 5;
                        break;
                    case (ConditionLinkType.INTTYPE):
                        _adjustmenttrigger = 6;
                        break;
                    case (ConditionLinkType.COMMON):
                        _adjustmenttrigger = 99;
                        break;
                }
            }
        }

        /// <summary>
        /// Target ID
        /// </summary>
        new
        public long TARGETID
        {
            get
            {
                switch (SOURCEBASIS)
                {
                    case (0):
                        return SYSIBOR;
                    case (1):
                        return SYSINTSTRCT;
                    case (2):
                        return SYSVG;
                    case (3):
                        return -1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Adjustment Trigger
        /// </summary>
        public long ADJUSTMENTTRIGGER
        {
            get { return _adjustmenttrigger; }
            set { _adjustmenttrigger = value; }
        }

        /// <summary>
        /// SysIntType
        /// </summary>
        public long SYSINTTYPE
        {
            get { return _sysinttype; }
            set { _sysinttype = value; }
        }

        /// <summary>
        /// SysPerole
        /// </summary>
        public long SYSPEROLE
        {
            get { return _sysperole; }
            set { _sysperole = value; }
        }

        /// <summary>
        /// SysBrand
        /// </summary>
        public long SYSBRAND
        {
            get { return _sysbrand; }
            set { _sysbrand = value; }
        }

        /// <summary>
        /// SysPrhGroup
        /// </summary>
        public long SYSPRHGROUP
        {
            get { return _sysprhgroup; }
            set { _sysprhgroup = value; }
        }

        /// <summary>
        /// SysPrkGroup
        /// </summary>
        public long SYSPRKGROUP
        {
            get { return _sysprkgroup; }
            set { _sysprkgroup = value; }
        }

        /// <summary>
        /// SysObTyp
        /// </summary>
        public long SYSOBTYP
        {
            get { return _sysobtyp; }
            set { _sysobtyp = value; }
        }

        /// <summary>
        /// SysObArt
        /// </summary>
        public long SYSOBART
        {
            get { return _sysobart; }
            set { _sysobart = value; }
        }


    }


   

    /// <summary>
    /// Dto for PRCLPRINTSET
    /// </summary>
    public class PRCLPRINTSETDto
    {
        /// <summary>
        /// Product clearance Print Set
        /// </summary>
        public long SYSPRCLPRINTSET
        {
            get;
            set;
        }

        /// <summary>
        /// Product In Test
        /// </summary>
        public long SYSPRINTEST
        {
            get;
            set;
        }

        /// <summary>
        /// Product
        /// </summary>
        public long SYSPRPRODUCT
        {
            get;
            set;
        }

        /// <summary>
        /// Rank
        /// </summary>
        public long RANK
        {
            get;
            set;
        }


    }

    /// <summary>
    /// Dto for PRINTSET
    /// </summary>
    public class PRINTSETDto
    {
        /// <summary>
        /// Printset
        /// </summary>
        public long SYSPRINTSET
        {
            get;
            set;
        }

        /// <summary>
        /// Valid From
        /// </summary>
        public DateTime VALIDFROM
        {
            get;
            set;
        }

        /// <summary>
        /// Valid Until
        /// </summary>
        public DateTime VALIDUNTIL
        {
            get;
            set;
        }


    }

    public class ZinsKontextDto 
    {
        public DateTime perDate { get; set; }
        public long sysperole { get; set; }
        public long sysbrand { get; set; }
        public long syskdtyp { get; set; }
        public long sysobart { get; set; }
        public long sysobtyp { get; set; }
        public long sysprchannel { get; set; }
        public long sysprhgroup { get; set; }
        public long sysinttype { get; set; }
        public long sysprkgroup { get; set; }
        public long sysprproduct { get; set; }
        public long sysprusetype { get; set; }
        public long sysvart { get; set; }
    }
}