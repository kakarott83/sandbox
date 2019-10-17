using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// BO for Prisma Product (including Parameter and Availability) access
    /// </summary>
    public class ZinsBo : AbstractZinsBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<ConditionLinkType, string> mapConditionLinkToParameter = new Dictionary<ConditionLinkType, string>();

        private DateTime nullDate = new DateTime(1800, 1, 1);
        private String isoCode;
        private ConditionLinkType[] supportedLinks;


        /// <summary>
        /// Different supported Configurations for Prisma Product Condition Links
        /// </summary>
        public static ConditionLinkType[] CONDITIONS_BANKNOW = { ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.OBART, ConditionLinkType.PEROLE };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="zDao"></param>
        /// <param name="prismaDao"></param>
        /// <param name="obDao"></param>
        /// <param name="supportedLinks"></param>
        /// <param name="isoCode">ISO Code für Übersetzung</param>
        /// <param name="vgdao">Wertegruppen DAO</param>
        public ZinsBo(IZinsDao zDao, IPrismaDao prismaDao, IObTypDao obDao, ConditionLinkType[] supportedLinks, String isoCode, IVGDao vgdao)
            : base(zDao, prismaDao, obDao, vgdao)
        {
            this.supportedLinks = supportedLinks;
            this.isoCode = isoCode;

            mapConditionLinkToParameter[ConditionLinkType.BRAND] = "sysbrand";
            mapConditionLinkToParameter[ConditionLinkType.OBTYP] = "sysobtyp";
            mapConditionLinkToParameter[ConditionLinkType.PRHGROUP] = "sysprhgroup";
            mapConditionLinkToParameter[ConditionLinkType.PRKGROUP] = "sysprkgroup";
            mapConditionLinkToParameter[ConditionLinkType.PEROLE] = "sysperole";
            mapConditionLinkToParameter[ConditionLinkType.OBART] = "sysobart";
            mapConditionLinkToParameter[ConditionLinkType.INTTYPE] = "sysinttype";

        }

        /// <summary>
        /// returns the base interest value
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="perDate"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        override
        public double getZinsBasis(long sysprproduct, DateTime perDate, long lz, double amount)
        {
            List<IntsDto> band = zinsDao.getIntsband();
            List<IntsDto> matu = zinsDao.getIntsmatu();
            List<IntsDto> rate = zinsDao.getIntsrate();
            List<IborDto> ibor = zinsDao.getIbor();

            List<IntstrctDto> intstrct = zinsDao.getIntstrct();
            PRPRODUCT product = prismaDao.getProduct(sysprproduct, isoCode);

            long sourcebasis = product.SOURCEBASIS.HasValue ? product.SOURCEBASIS.Value : 0;
            switch (sourcebasis)
            {
                case (0)://IBOR-----------------------------------------------------------------------------------------------
                    var qi = from s in ibor
                             where s.sysprproduct == sysprproduct
                             && s.validFrom <= perDate
                             orderby s.validFrom descending
                             select s;
                    IborDto iborDat = qi.FirstOrDefault();
                    if (iborDat == null)
                        throw new ApplicationException("No ibor for product " + sysprproduct + " date " + perDate);
                    return iborDat.m1;

                case (2)://VG-----------------------------------------------------------------------------------------------
                    long? sysvg = product.SYSVG;
                    double rval = 0;
                    if (sysvg != null && lz != 0 && amount != 0)
                        // HR Einbau 1.9.2011 als Übergangslösung bis C# getVGValue fertig ist
                        rval = vgDao.getVGValueSaldo((long)sysvg, amount, lz, Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    //rval = vgDao.getVGValue((long)sysvg,  Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null), lz.ToString(), amount.ToString(), VGInterpolationMode.MIN);
                    return rval;
                default://INTSTRCT-------------------------------------------------------------------------------------------
                    var q = from s in intstrct
                            where s.sysprproduct == sysprproduct
                            && s.validFrom <= perDate
                            orderby s.validFrom descending
                            select s;

                    IntstrctDto strct = q.FirstOrDefault();
                    if (strct == null)
                        throw new ApplicationException("No intstrct for product " + sysprproduct + " date " + perDate);
                    return getIntstrctValue(strct, rate, band, matu, lz, amount);
            }
        }

        /// <summary>
        /// returns ZinsRap saldoabhängigen mit Zinsstruktur 
        /// 
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="perDate"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <param name="sysintstrct"></param>
        /// <param name="rapValByScor"></param>
        /// <returns></returns>
        override
        public double getZinsRap(long sysprproduct, DateTime perDate, long lz, double amount, long sysintstrct, double rapValByScor)
        {
            List<IntsDto> band = zinsDao.getIntsband();
            List<IntsDto> matu = zinsDao.getIntsmatu();
            List<IntsDto> rate = zinsDao.getIntsrate();
            List<IborDto> ibor = zinsDao.getIbor();


            PRRAP prrap = getPrRap(sysprproduct);

            List<IntstrctDto> intstrct = zinsDao.getIntstrctById(sysintstrct);
            PRPRODUCT product = prismaDao.getProduct(sysprproduct, isoCode);

            //INTSTRCT-------------------------------------------------------------------------------------------
            var q = from s in intstrct
                    where s.validFrom <= perDate
                    orderby s.validFrom descending
                    select s;
            IntstrctDto strct = q.FirstOrDefault();
            if (strct == null)
                return rapValByScor;

            return getIntstrctValueRap(strct, rate, band, matu, lz, amount, rapValByScor);
        }


        /// <summary>
        /// return  PRRAPVAL.VALUE mit saldoabhängigen Zinsstruktur 
        /// </summary>
        /// <param name="strct"></param>
        /// <param name="rate"></param>
        /// <param name="band"></param>
        /// <param name="matu"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <param name="rapValByScor"></param>
        /// <returns></returns>
        private double getIntstrctValueRap(IntstrctDto strct, List<IntsDto> rate, List<IntsDto> band, List<IntsDto> matu, long lz, double amount, double rapValByScor)
        {
            IntsDto cstep = null;

            switch (strct.method)
            {
                case 1://Einfache Zinsstaffel Datumsbezogen
                    cstep = (from i in rate
                             where i.sysintsdate == strct.sysintsdate
                             select i).FirstOrDefault();
                    break;
                case 2://Laufzeitbezogen
                    cstep = (from i in matu
                             where i.sysintsdate == strct.sysintsdate
                             && i.maturity <= lz
                             orderby i.maturity descending
                             select i).FirstOrDefault();
                    break;
                case 3://Betragsbezogen
                    cstep = (from i in band
                             where i.sysintsdate == strct.sysintsdate
                             && i.lowerb <= amount && (i.upperb > amount || i.upperb == 0)
                             select i).FirstOrDefault();
                    break;
            }
            if (cstep == null)
                return rapValByScor;


            return rapValByScor + cstep.addrate - cstep.redrate;
        }



        /// <summary>
        /// Deliver Interest Rate Steps
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <param name="zinsBase"></param>
        /// <returns></returns>
        override
        public double getZinsSchritte(prKontextDto ctx, long lz, double amount, double zinsBase)
        {
            double rval = zinsBase;

            PRPRODUCT product = prismaDao.getProduct(ctx.sysprproduct, isoCode);
            List<PRCLPRINTSETDto> prlinks = zinsDao.getProductLinks().Where(a => a.sysprproduct == ctx.sysprproduct).OrderBy(a => a.rank).ToList();
            List<InterestConditionLink> allsteps = zinsDao.getIntSteps();

            List<IntsDto> raten = zinsDao.getIntsrate();
            List<IntsDto> band = zinsDao.getIntsband();
            List<IntsDto> matu = zinsDao.getIntsmatu();

            List<IntstrctDto> intstrcts = zinsDao.getIntstrct();

            foreach (PRCLPRINTSETDto prlink in prlinks)//all product links to interest groups
            {
                List<PRINTSETDto> zinsgroups = zinsDao.getIntGroups().Where(z => z.sysprintset == prlink.sysprintest &&
                    (z.validfrom == null || z.validfrom <= ctx.perDate || z.validfrom <= nullDate)
                                           && (z.validuntil == null || z.validuntil >= ctx.perDate || z.validuntil <= nullDate)).ToList();

                foreach (PRINTSETDto group in zinsgroups)//all interest groups
                {
                    List<InterestConditionLink> groupsteps = allsteps.Where(a => a.sysprintset == group.sysprintset).OrderBy(a => a.rank).ToList();

                    List<InterestConditionLink> availsteps = new List<InterestConditionLink>();

                    foreach (InterestConditionLink step in groupsteps)
                    {
                        List<long> condvalues = ConditionLink.getParameter(ctx, step.CONDITIONTYPE, mapConditionLinkToParameter, obDao, Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                        if (condvalues.Contains(step.CONDITIONID))
                            rval = applyStep(step, rval, lz, amount, raten, band, matu, intstrcts);
                    }
                }
            }
            return rval;
        }

        /// <summary>
        /// returns the base interest with all interest modification steps applied
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        override
        public double getZins(prKontextDto ctx, long lz, double amount)
        {
            double zinsBase = getZinsBasis(ctx.sysprproduct, ctx.perDate, lz, amount);
            return getZinsSchritte(ctx, lz, amount, zinsBase);
        }

        /// <summary>
        /// returns the Rap for the product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        override
        public PRRAP getPrRap(long sysprproduct)
        {
            return this.zinsDao.getPrRap(sysprproduct);
        }

        /// <summary>
        /// returns the basel-ii (simple) score adjusted rap zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        override
        public double getRapZinsByScore(long sysprproduct, String score)
        {
            return getRapValByScore(sysprproduct, score).VALUE.Value;
        }

        /// <summary>
        /// returns the basel-ii (simple) score adjusted rap zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        override
        public PRRAPVAL getRapValByScore(long sysprproduct, String score)
        {
            PRRAP prrap = this.zinsDao.getPrRap(sysprproduct);
            PRRAPVAL rval = new PRRAPVAL();
            PRRAPVAL cval = null;
            rval.VALUE = 0;
            if (prrap != null)
            {
                List<PRRAPVAL> values = zinsDao.getRapValues(prrap.SYSPRRAP);

                if (isNumeric(score))
                {
                    double dScore = Convert.ToDouble(score);
                    double minBand = -1;
                    foreach (PRRAPVAL item in values)
                    {
                        cval = item;
                        double maxBand = Convert.ToDouble(item.SCORE);

                        if (dScore > minBand && dScore <= maxBand)
                        {
                            break;
                        }
                        minBand = maxBand;
                    }
                }
                else
                {
                    String minBand = "-1";
                    foreach (PRRAPVAL item in values)
                    {
                        cval = item;
                        String maxBand = item.SCORE;

                        if (score.CompareTo(minBand) < 0 && score.CompareTo(maxBand) >= 0)
                        {
                            break;
                        }
                        minBand = maxBand;
                    }
                }
                if (cval == null || !cval.VALUE.HasValue)
                    return rval;
                rval.VALUE = cval.VALUE;
                rval.SCORE = cval.SCORE;
                rval.FAKTOR1 = cval.FAKTOR1;
                rval.FAKTOR2 = cval.FAKTOR2;
            }
            return rval;
        }

        private static bool isNumeric(string theValue)
        {
            try
            {
                Convert.ToDouble(theValue);
                return true;
            }
            catch
            {
                return false;
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
        private double getIntstrctValue(IntstrctDto strct, List<IntsDto> rate, List<IntsDto> band, List<IntsDto> matu, long lz, double amount)
        {
            IntsDto cstep = null;

            switch (strct.method)
            {
                case 1://Einfache Zinsstaffel Datumsbezogen
                    cstep = (from i in rate
                             where i.sysintsdate == strct.sysintsdate
                             select i).FirstOrDefault();
                    break;
                case 2://Laufzeitbezogen
                    cstep = (from i in matu
                             where i.sysintsdate == strct.sysintsdate
                             && i.maturity <= lz
                             orderby i.maturity descending
                             select i).FirstOrDefault();
                    break;
                case 3://Betragsbezogen
                    cstep = (from i in band
                             where i.sysintsdate == strct.sysintsdate
                             && i.lowerb <= amount && (i.upperb > amount || i.upperb == 0)
                             select i).FirstOrDefault();
                    break;
            }
            if (cstep == null)
                throw new ApplicationException("No INTS-Data for INTSDATE " + strct.sysintsdate);

            // HR 14.8.2011: Auskommentiert in Abstrache mit AS 
            // Grund: Keine Abschläge im Front Office.
            return cstep.intrate + cstep.addrate; // -cstep.redrate;
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
        private double applyStep(InterestConditionLink step, double zins, long lz, double amount, List<IntsDto> raten, List<IntsDto> band, List<IntsDto> matu, List<IntstrctDto> intstrcts)
        {
            double szins = zins;
            switch (step.sourcebasis)
            {
                case (0): //IBOR
                    break;
                case (1)://INTSTRCT
                    IntstrctDto strct = intstrcts.Where(a => a.sysintstrct == step.TARGETID).FirstOrDefault();
                    szins = getIntstrctValue(strct, raten, band, matu, lz, amount);
                    break;
                case (2)://VG
                    break;
                case (3):
                    szins = step.intrate;
                    break;
            }
            return applyMethod(step.method, zins, szins);
        }

        /// <summary>
        /// applies val by the calculation method to the base interest rate
        /// </summary>
        /// <param name="method"></param>
        /// <param name="zins"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private static double applyMethod(long method, double zins, double val)
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
        /// Returns the RAP Zins
        /// index zero=RAP
        /// index 1 = MIN
        /// index 2 = MAX
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="kundenScore"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <param name="prodCtx"></param>
        /// <returns></returns>
        override
            public double[] getRAPZins(long sysprproduct, String kundenScore, long lz, double amount, prKontextDto prodCtx)
        {
            double[] zinsen = new double[1];
            PRRAP prrap = getPrRap(sysprproduct);
            PRRAPVAL rapVal = null;

            if ((kundenScore != null && kundenScore.Length > 0 && Convert.ToDouble(kundenScore) != 0))
            {

                //immer rap-zinsen mitrechnen ausser differenzleasung und wenn kundenscore von aussen
                if ((kundenScore != null && kundenScore.Length > 0 && Convert.ToDouble(kundenScore) != 0))
                {
                    if (prodCtx.sysprkgroup > 0 && prrap != null)
                    {
                        List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> rapvaluesliste =
                            zinsDao.getRapValues(prrap.SYSPRRAP, prodCtx.sysprkgroup);
                        if (rapvaluesliste.Count > 0)
                        {
                            rapVal = zinsDao.getRapValByScore(rapvaluesliste, kundenScore);
                        }
                        else
                        {
                            rapVal = getRapValByScore(sysprproduct, kundenScore);
                        }
                    }
                    else
                    {
                        rapVal = getRapValByScore(sysprproduct, kundenScore);
                    }
                    //BNRSIEBEN-406: gibt den Zinssatz anhand des Score-abhängigen Rap-Wertes (PRRAPVAL.VALUE) mit dem abgezogenen Zinsabschlag aus dem Zinsband der zugeordneten Zinstruktur sysintstrct (PRRAP.SYSINSTRCT) zurück
                    if (prrap != null && prrap.SYSINTSTRCT != null && prrap.SYSINTSTRCT.Value != 0 &&
                        rapVal.VALUE != null && rapVal.VALUE.Value != 0)
                    {
                        rapVal.VALUE = getZinsRap(sysprproduct,prodCtx.perDate,
                            lz, amount,
                            prrap.SYSINTSTRCT.Value, rapVal.VALUE.Value);
                    }
                    zinsen[0] = rapVal.VALUE.Value;
                    double BasisEffectiv = getZins(prodCtx, lz, amount);

                    if (zinsen[0] < BasisEffectiv)
                    {
                        zinsen[0] = BasisEffectiv;
                    }
                }
                else
                {

                    if (prrap != null)
                    {
                        zinsen = new double[3];

                        if (prodCtx.sysprkgroup>0)
                        {
                            List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> rapvaluesliste =
                                zinsDao.getRapValues(prrap.SYSPRRAP,prodCtx.sysprkgroup);

                            if (rapvaluesliste != null)
                            {
                                zinsen[0] = rapvaluesliste.FirstOrDefault().VALUE.Value;
                            }
                            else
                            {
                                zinsen[0] = getZins(prodCtx, lz, amount);
                            }
                        }
                    }
                    else
                    {
                        zinsen[0] = getZins(prodCtx,lz, amount);
                    }

                    if (prrap != null)
                    {
                        zinsen[1] = prrap.MINVALUE.HasValue ? prrap.MINVALUE.Value : 0;
                        zinsen[2] = prrap.MAXVALUE.HasValue ? prrap.MAXVALUE.Value : 0;

                    }
                }
            }
            else
            {
                if (prrap != null)
                {
                    zinsen = new double[3];
                    zinsen[0] = getRapZinsByScore(sysprproduct, "0");
                }
                else
                {
                    zinsen[0] = getZins(prodCtx, lz, amount);
                }

                if (prrap != null)//needed for min max slider 
                {
                    zinsen[1] = prrap.MINVALUE.HasValue ? prrap.MINVALUE.Value : 0;
                    zinsen[2] = prrap.MAXVALUE.HasValue ? prrap.MAXVALUE.Value : 0;
                }
                
            }
            return zinsen;
        }

        /// <summary>
        /// getRapValByScore
        /// </summary>
        /// <param name="values"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        override public Cic.OpenOne.Common.Model.Prisma.PRRAPVAL getRapValByScore(List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> values, String score)
        {
            return this.zinsDao.getRapValByScore(values, score);
        }

        /// <summary>
        /// getRapValues
        /// </summary>
        /// <param name="sysprrap"></param>
        /// <param name="sysprkgroup"></param>
        /// <returns></returns>
        override public List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> getRapValues(long sysprrap, long sysprkgroup)
        {
            return this.zinsDao.getRapValues(sysprrap,sysprkgroup);
        }
        
    }
}