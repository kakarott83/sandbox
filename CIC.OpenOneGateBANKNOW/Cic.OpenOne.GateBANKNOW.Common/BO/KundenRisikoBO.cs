using AutoMapper;
using AutoMapper.Mappers;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.PRISMA.EF6.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{


    /// <summary>
    /// Flags for EL Calculation, fetched from the valuegroup of the product
    /// </summary>
    public class Flags4KR
    {
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }
        public double d { get; set; }
        public double K { get; set; }
        public double LGD { get; set; }
        public decimal? ZFAKTOR { get; set; }
        public double ELBETRAG { get; set; }
        public double ELPROC { get; set; }
        public double ELPROF { get; set; }
        public bool FORCIERBAR { get; set; }
        public double ITERATIONMAX { get; set; }
        public double ITERATIONMIN { get; set; }
        public double ITERATIONSCHRITT { get; set; }
        public String FORMEL_EL { get; set; }
        public String FORMEL_PROF { get; set; }
        public double ELLZLIMIT { get; set; }
            
    }

    /// <summary>
    /// Contains values from Decision Engine needed for EL Calculation
    /// </summary>
    public class DEValues4KR
    {
        public double PD { get; set; }
        public decimal ZFAKTOR { get; set; }
        public String CLUSTERVALUE { get; set; }
        public String SCOREBEZEICHNUNG { get; set; }
        public double FREIBETRAG { get; set; }
        public double SCORETOTAL { get; set; }
        public decimal? BUDGETPUFFER { get; set; }
        
    }
    /// <summary>
    /// Contains values from the proposal used for el calculation and product validation
    /// </summary>
    public class ELCalcVars
    {
        public double zins { get; set; }
        public double lz { get; set; }
        public double bgexternbrutto { get; set; }
        public double bginternbrutto { get; set; }
        public double ratebrutto { get; set; }
        public double rsvgesamt { get; set; }
        public DateTime? gebdatum { get;set;}
        public String channel { get;set;}
        public double maxAlter { get;set;}
        public double endAlterKunde { get; set; }
        public double szbrutto { get; set; }
        

    }
    public class ELResults
    {
        /// <summary>
        /// Expected Loss CHF
        /// </summary>
        public double? EL { get; set; }
        /// <summary>
        /// Profitability
        /// </summary>
        public double? ELPROF { get; set; }
        /// <summary>
        /// Expected Loss %
        /// </summary>
        public double? ELP { get; set; }
    }


    public class CreditLimitsDEBUGDto
    {
        /// <summary>
        /// 
        /// </summary>
        public AvailableProduktDto  produkt { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public ELCalcVars KalkulationsWerten { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DEValues4KR DecisionEngineWerten { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Flags4KR ValuegroupWerten { get; set; }


        /// <summary>
        /// Expected Loss für Laufzeit 
        /// </summary>
        public List< ExpLossByLZDEBUGDto> ExpLossListeByLZ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Budget4DESimDto budget { get; set; }

    }

    public class ExpLossByLZDEBUGDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int lz { get; set; }

       

        /// <summary>
        /// 
        /// </summary>
        public double maxkreditlimit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? zfaktor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ExpLossByIteration> IterationsWerte { get; set; }

       

    }

    public class ExpLossByIteration
    {
        /// <summary>
        /// 
        /// </summary>
        public double zinsByProd { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public double zinsertrag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ratebrutto { get; set; }
        
        public double usekredit { get; set; }

        public double ERZLZ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ELResults expLossWerte { get; set; }

        public bool abgelehnt { get; set; }

        public string ablehnungsgrund { get; set; }

    }

    /// <summary>
    /// Kunden RisikoBO
    /// </summary>
    public class KundenRisikoBO : AbstractKundenRisikoBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// KundenRisikoBO
        /// </summary>
        /// <param name="angAntBo"></param>
        /// <param name="angAntDao"></param>
        /// <param name="vgDao"></param>
        /// <param name="eurotaxDBDao"></param>
        /// <param name="eurotaxBo"></param>
        /// <param name="mwStDao"></param>
        /// <param name="translateBo"></param>
        /// <param name="trDao"></param>
        public KundenRisikoBO(IAngAntDao angAntDao, IVGDao vgDao, IPrismaProductBo pBo,IPrismaParameterBo parBo, IMwStBo mwStBo, IMwStDao mwStDao, ITranslateBo translateBo, KundenRisikoDao trDao, IEaihotDao eaihotDao)
            : base(angAntDao, vgDao, pBo,parBo,mwStBo, mwStDao, translateBo, trDao, eaihotDao)
        {
        }

        /// <summary>
        /// Calculates the credit limits for the context
        /// </summary>
        /// <param name="kontext"></param>
        /// <param name="sysantrag"></param>
        /// <param name="sysvart"></param>
        /// <returns></returns>
        override public List<ProductCreditInfoDto> getCreditLimits(prKontextDto kontext, long sysantrag, long sysvart, String isoCode, long syswfuser)
        {
            if (sysvart >= 0)
                kontext.sysvart = sysvart;
            long sysvartOrg = kontext.sysvart;


            Boolean useRuleEngine = AppConfig.Instance.getBooleanEntry("RULEENGINE_EXPECTEDLOSS", "USE_RULEENGINE", false, "ANTRAGSASSISTENT");
            String ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "RULEENGINE_EXPECTEDLOSS", "USE_RULESET_VAR", "");

            List<ProductCreditInfoDto> rval = new List<ProductCreditInfoDto>();



            if (useRuleEngine)
            {
                return getCreditLimitsRE(kontext, sysantrag, sysvart, isoCode, syswfuser);

                /*
                 List<ProductCreditLimitFetchDto> savedItems = trDao.fetchCreditLimits(sysantrag);
                 if (savedItems != null && savedItems.Count > 0)
                 {
                     List<long> savedProds = (from pi in savedItems
                                                 select pi.sysprproduct).Distinct().ToList();
                     //Wunschkredit als erstes Produkt anfügen
                     long wunschKreditProduct = savedItems[0].sysprproduct;
                     savedProds.Remove(wunschKreditProduct);
                     savedProds.Insert(0, wunschKreditProduct);


                     List<PRPRODUCT> availproducts = prodBo.listAvailableProducts(kontext);
                     List<AvailableProduktDto> sortedAvailProds = prodBo.listSortedAvailableProducts(availproducts, kontext.sysprbildwelt);



                     foreach (long mprod in savedProds)
                     {
                         List<ProductCreditLimitFetchDto> prodLimits = (from pi in savedItems
                                                                         where pi.sysprproduct == mprod
                                                                         select pi).ToList();
                         ProductCreditInfoDto pii = new ProductCreditInfoDto();
                         pii.creditLimits = Mapper.Map<List<ProductCreditLimitFetchDto>, List<ProductCreditLimitDto>>(prodLimits);
                         pii.product = (from s in sortedAvailProds
                                         where s.sysID == mprod
                                         select s).FirstOrDefault();
                         if (pii.product == null) continue; //saved product no more in sorttree
                         rval.Add(pii);

                     }
                     return rval;
                 }
                 */

            }









            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;


            //INITIALIZATION 
            ELCalcVars antvars = trDao.getAntragDaten(sysantrag);
            long zinsVg = trDao.getZinsertragWertegruppe();
            antvars.maxAlter = 180;
            antvars.rsvgesamt = 0;//not used in interation!
            antvars.ratebrutto = 0;//not used in interation producttest

            //STEP 1 DECISION-ENGINE, get Values PD,Cluster,Budgetpuffer
            //DEDETAIL.PD,-DEDETAIL.CLUSTER,-DEDETAIL.FREIBETRAG  where DEDETAIL.ANTRAGSTELLER  = 1
            DEValues4KR devals = trDao.getDecisionValues(sysantrag);
            if (devals==null || devals.CLUSTERVALUE == null || devals.CLUSTERVALUE.Length == 0)
            {
                _log.Warn("No CLUSTERVALUE for Antrag " + sysantrag + " found in DE-Data");
                return rval;
            }
            // Ausfallwahrscheinlichkeit = 1/(1+EXP(-(Scorekartenabhängiger_Kalibrierungswert+ Scorekartenabhängiger_Kalibrierungsfaktor*Scorewert)))//
            //double ausfallwahrscheinlichkeitP = (1 / (1 + (double)Math.Pow(2.7182818285, (-(faktoren.kalibrierungswert + faktoren.kalibrierungsfaktor * devals.SCORETOTAL))))) * 100;
            if (devals.PD == 0)
            {
                devals.PD = 1.0;
            }

            //STEP 2 - get KREMO.BUDGET1(Saldo) 
            //Max(SYSKREMO) zum Antrag (Saldo bei 2. An-tragsteller)
            Budget4DESimDto budget = this.trDao.getBudget4DESim(sysantrag);
            bool hasMA = this.trDao.hasMA(sysantrag);
            
            //STEP3 - values from VG FlagsKR
            //DONE INSIDE LOOP            

            //STEP4 - LZ from VG - (inner iteration min/max  for-loop below)
            List<int> laufzeiten = trDao.getLaufzeiten();
            

            //STEP5 - availableProducts - (outer iteration  for-loop below)
            
            List< PRPRODUCT> products = prodBo.listAvailableProducts(kontext);
            List<AvailableProduktDto> sortProds = prodBo.listSortedAvailableProducts(products, kontext.sysprbildwelt);
            Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo pruefungbo = new Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo(PrismaDaoFactory.getInstance().getPrismaDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getPruefungDao());
            Common.DTO.JokerPruefungDto resultJokerPruefung = pruefungbo.analyzeJokerProducts(sortProds.ToArray(), kontext);
            sortProds = resultJokerPruefung.products;

            //reorganize sortProds by vart
            //sortProds = sortProds.OrderBy(a => a.vttypcode).ToList();
            _log.Debug("getCreditLimits for " + sortProds.Count + " products..");
            _log.Debug("Duration getCreditLimits init: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            //AFTER ALL VARIABLES NOW AVAILABLE, perform the loop below and calculate
            long sysvttypOrg = kontext.sysvttyp;
            List<CreditLimitsDEBUGDto> debugListe = new List<CreditLimitsDEBUGDto>();
            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> vars = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto>();
            CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto qv = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto();
            qv.Name = "qPRODUCTIN";
            vars.Add(qv);
            vars.Add(new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto() { Name = "qPRODUCTOUT" });
            List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto> qrecs = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto>();

            IZinsBo zinsBo = BOFactory.getInstance().createZinsBo(ZinsBo.CONDITIONS_BANKNOW, isoCode);
            IQuoteDao quoteDao = OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao();
            double szQuote = quoteDao.getQuote(QuoteDao.SZ_SCHWELLE_QUOTE);


            foreach (AvailableProduktDto prod in sortProds)//for every available Product
            { 
                CreditLimitsDEBUGDto debugDto = new CreditLimitsDEBUGDto();
                debugDto.produkt = prod;
                debugDto.DecisionEngineWerten = devals;
                debugDto.KalkulationsWerten = antvars;
                debugDto.budget=  budget;
                start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                if (prod.sysID < 0)
                {
                    continue;//Disabled product
                }
                VART vart = prismaDao.getVertragsart(prod.sysID);
                if (sysvartOrg != 0 && vart.SYSVART != sysvartOrg)
                {
                    continue;//check vart von Produkt
                }
                if (sysvartOrg == 0)
                {
                    kontext.sysvart = vart.SYSVART;
                }
                kontext.sysvttyp = sysvttypOrg;
                ProductCreditInfoDto prval = new ProductCreditInfoDto();
                prval.creditLimits = new List<ProductCreditLimitDto>();
                //fill product info from availproducts result
                prval.product = prod;
                
                kontext.sysprproduct = prod.sysID;
                //STEP6 - product-parameters (eg lz to avoid some values in the inner loop below)
                ConditionLinkType[] CONDITIONS_BANKNOW_KR = { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.PRODUCT, ConditionLinkType.BCHNL, ConditionLinkType.OBART, ConditionLinkType.KDTYP, ConditionLinkType.USETYPE, ConditionLinkType.VART, ConditionLinkType.VTTYP, ConditionLinkType.PEROLE };
                IPrismaParameterBo paramBo = PrismaBoFactory.getInstance().createPrismaParameterBo(CONDITIONS_BANKNOW_KR);

                List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> prodParams = paramBo.listAvailableParameter(kontext);
                Cic.OpenOne.Common.DTO.Prisma.ParamDto lzparam = (from lzp in prodParams
                                                                where lzp.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.kalkBorderLz))
                   
                                                             select lzp).FirstOrDefault();
                
                Cic.OpenOne.Common.DTO.Prisma.ParamDto zfac = (from zf in prodParams
                                                               where zf.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.ZFAKTOR))
                                                                  select zf).FirstOrDefault();

                Cic.OpenOne.Common.DTO.Prisma.ParamDto elv = (from zf in prodParams
                                                              where zf.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.EL_WERTEGRUPPE))
                                                               select zf).FirstOrDefault();

                Cic.OpenOne.Common.DTO.Prisma.ParamDto endAlter = (from zf in prodParams
                                                                   where zf.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.KalkBorderEndalterKunde))
                                                              select zf).FirstOrDefault();

                Cic.OpenOne.Common.DTO.Prisma.ParamDto maxKreditbetrag = (from zf in prodParams
                                                                   where zf.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.KalkBorderBgIntern))
                                                                   select zf).FirstOrDefault();

                
                if (endAlter != null)
                {
                    antvars.maxAlter = endAlter.maxvaln;
                }

                /*if (elv == null)//DEBUG
                {
                    elv = new Cic.OpenOne.Common.DTO.Prisma.ParamDto();
                    elv.sysvg = 256;
                    _log.Warn("No Wertegruppe for "+ prod.bezeichnung + "/" + prod.sysID + " - using sysvg 256");
                }*/

                  if (elv == null && !useRuleEngine)
                  {
                     _log.Info("Product " + prod.bezeichnung + "/" + prod.sysID + " - no Parameter EL_WERTEGRUPPE defined!");
                    continue;
                  }
                //STEP3 - values from VG FlagsKR
                //DONE INSIDE LOOP
                Flags4KR flags = null;
                if(elv!=null)
                    flags = getFaktoren(elv.sysvg, devals.CLUSTERVALUE);
                debugDto.ValuegroupWerten = flags;

                decimal zfaktor = devals.ZFAKTOR;
                if (flags!=null && flags.ZFAKTOR.HasValue)
                {
                    zfaktor = flags.ZFAKTOR.Value;
                }
                if (zfac != null)
                {
                    zfaktor = (decimal)zfac.defvaln;
                }

                List <ExpLossByLZDEBUGDto>   listExplossbyLZ = new List <ExpLossByLZDEBUGDto>();
                _log.Debug("Duration getCreditLimits Prod-Init: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

                VartDto vartDto = angAntDao.getVart(kontext.sysvart);
                foreach(int lz in laufzeiten)//for every configured runtime from step 4 
                {
                    start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    ProductCreditLimitDto pi = new ProductCreditLimitDto();
                    pi.laufzeit = lz;
                    pi.creditLimit = 0;
                    prval.creditLimits.Add(pi);

                    //check if lz is available for product, else set result value to zero and skip
                    if (lzparam != null)
                    {
                        if (lzparam.minvaln > lz)
                        {
                            continue;
                        }
                        if (lzparam.maxvaln < lz)
                        {
                            continue;
                        }
                        if (lz % lzparam.stepsize != 0.0)
                        {
                            continue;//stepsize mismatch
                        }
                    }
                    


                    //budget1*zfaktor für 1 as
                    //saldo * zfaktor für 2 as
                    //STEP8 - calculate max creditlimit
                    double maxkreditlimit = (double)(hasMA ? budget.saldo.Value * zfaktor : budget.budget1.Value * zfaktor);
                    if (lz < 36)
                    {
                        if (devals!=null)
                        {
                            if (hasMA)
                            {
                                maxkreditlimit = (double)(budget.saldo.Value - devals.BUDGETPUFFER) * lz;
                            }
                            else
                            {
                                maxkreditlimit = (double)(budget.budget1.Value - devals.BUDGETPUFFER) * lz;
                            }
                        }
                        else
                        {
                            _log.Info("don't contains values from Decision Engine needed for EL Calculation");
                            if (hasMA)
                            {
                                maxkreditlimit = (double)(budget.saldo.Value) * lz;
                            }
                            else
                            {
                                maxkreditlimit = (double)(budget.budget1.Value) * lz;
                            }
                        }
                       
                    }

                    if (maxkreditlimit < 0)
                    {
                        maxkreditlimit = 0;
                    }
                    else
                    {
                        maxkreditlimit = Math.Floor(maxkreditlimit / 1000) * 1000;
                    }

                    if (maxKreditbetrag != null)
                    {
                        if (maxkreditlimit > maxKreditbetrag.maxvaln)
                        {
                            maxkreditlimit = maxKreditbetrag.maxvaln;
                        }
                    }
                    

                        //STEP7 - calculate EL and profitability
                        //what if antvars.channel is empty?
                        string lztext = (lz + 1).ToString();
                    



                    //maxkreditlimit für iteration, beim speichern bginternbrutto
                    

                    double usekredit = maxkreditlimit;
                    if (usekredit == 0)
                    {
                        _log.Warn("No getCreditLimits for  ANTRAG=" + sysantrag + " possible, usekredit = 0 ");
                        return rval;
                    }

                    double ust = mwStBo.getMehrwertSteuer(1, vart.SYSVART, kontext.perDate);
                    double[] zinsen = getZinsByProdukt(lz, ust, devals.SCORETOTAL.ToString(), kontext, prod.sysID, isoCode, usekredit, antvars.szbrutto, kontext.sysprkgroup, vartDto.code,zinsBo,szQuote);



                    if (useRuleEngine)
                    {
                        /* F01 = sysprproduct
                        * F02 = zins nominal = zinsen[0]
                        * F03 = Laufzeit
                        * F04 = usekredit
                        * qPRODUCTIN
                        */
                        CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto rec = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                        rec.Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[4];
                        rec.Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                        rec.Values[0].VariableName = "F01";
                        rec.Values[0].Value = prod.sysID.ToString();
                        rec.Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                        rec.Values[1].VariableName = "F02";
                        rec.Values[1].Value = zinsen[0].ToString();
                        rec.Values[2] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                        rec.Values[2].VariableName = "F03";
                        rec.Values[2].Value = lz.ToString();
                        rec.Values[3] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                        rec.Values[3].VariableName = "F04";
                        rec.Values[3].Value = usekredit.ToString();
                        qrecs.Add(rec);

                        
                        continue;
                    }



                    antvars.endAlterKunde = MyGetEndAlterKunde(DateTime.Now.AddMonths(lz), antvars.gebdatum);
                    double lzzins = (double)vgDao.getVGValue(zinsVg, DateTime.Now, devals.CLUSTERVALUE, lztext, VGInterpolationMode.MIN, plSQLVersion.V1); //Faktor zur Berechnung
                    _log.Debug("Duration getCreditLimits LZ-Iter: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    int numiter = 0;
                    pi.creditLimit = 0;//N/A

                    ExpLossByLZDEBUGDto  expLossByLZ = new ExpLossByLZDEBUGDto();
                    expLossByLZ.lz = lz;
                    expLossByLZ.maxkreditlimit = maxkreditlimit;
                    expLossByLZ.zfaktor = (double)zfaktor;
                    _log.Debug(" Zins: "+ zinsen[0] + " für Produkt: " + prod.sysID + " laufzeit: " +lz);

                   
                    List<ExpLossByIteration> expLossListeByIteration = new List<ExpLossByIteration>();
                    while (numiter++ < flags.ITERATIONMAX)
                    {

                        double zinsertrag = -1.0 * System.Numeric.Financial.CumIPmt(zinsen[0] / 1200, lz, usekredit, 1, Math.Min(lz, lzzins), System.Numeric.PaymentDue.EndOfPeriod);
                        antvars.ratebrutto = System.Numeric.Financial.Pmt(zinsen[0] / 1200, lz, -1.0 * usekredit, 0, System.Numeric.PaymentDue.EndOfPeriod);//not used in interation producttest

                        ELResults elResult = new ELResults();
                        ExpLossByIteration iterationwerte = new ExpLossByIteration();
                        iterationwerte.zinsByProd = zinsen[0];
                        iterationwerte.zinsertrag = zinsertrag;
                        iterationwerte.ratebrutto = antvars.ratebrutto;
                        iterationwerte.usekredit = usekredit;
                        iterationwerte.ERZLZ = (double)Math.Min(lz, lzzins);
                        string grund = "";

                        if (!elCalcIteration(usekredit, zinsertrag, flags, kontext, sysantrag, lz, antvars, devals, budget, (double)zfaktor, elResult, ref grund, hasMA))
                        {
                            //kredit too high
                            iterationwerte.abgelehnt = true;
                            iterationwerte.ablehnungsgrund = grund;
                            iterationwerte.expLossWerte = elResult;
                            expLossListeByIteration.Add(iterationwerte);
                            usekredit -= flags.ITERATIONSCHRITT;
                        }
                        else
                        {
                            iterationwerte.abgelehnt = false;
                            iterationwerte.expLossWerte = elResult;
                            expLossListeByIteration.Add(iterationwerte);
                            pi.creditLimit = usekredit;
                            break;
                        }
                        if (usekredit < flags.ITERATIONMIN)
                        {
                            break;
                        }
                        
                        _log.Debug("Duration getCreditLimits ITERATION: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

                        expLossByLZ.IterationsWerte = expLossListeByIteration;
                        listExplossbyLZ.Add(expLossByLZ);
                    }
                }
                rval.Add(prval);

                if (useRuleEngine) continue;
                debugDto.ExpLossListeByLZ =  listExplossbyLZ;
                debugListe.Add(debugDto);
               
                kontext.sysvart = sysvartOrg;
                
            }


            if (useRuleEngine)
            {

                qv.Records = qrecs.ToArray();

                CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] variables = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[1];
                variables[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                variables[0].VariableName = "PP";
                variables[0].LookupVariableName = "SPRACHE";
                variables[0].Value = isoCode;
                List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queueResult = BPEBo.getQueueData(sysantrag, "ANTRAG", new String[] { "qRULES" }, ruleCode, variables, syswfuser, vars);
                
                try
                {
                    //first try by loading from db:
                    List<ProductCreditLimitFetchDto> savedItems = trDao.fetchCreditLimits(sysantrag);
                    if (savedItems != null && savedItems.Count > 0)
                    {
                        List<long> savedProds = (from pi in savedItems
                                                 select pi.sysprproduct).Distinct().ToList();
                        //Wunschkredit als erstes Produkt anfügen
                        long wunschKreditProduct = savedItems[0].sysprproduct;
                        savedProds.Remove(wunschKreditProduct);
                        savedProds.Insert(0, wunschKreditProduct);


                       

                        rval.Clear();
                        foreach (long mprod in savedProds)
                        {
                            List<ProductCreditLimitFetchDto> prodLimits = (from pi in savedItems
                                                                           where pi.sysprproduct == mprod
                                                                           select pi).ToList();
                            ProductCreditInfoDto pii = new ProductCreditInfoDto();
                            pii.creditLimits = Mapper.Map<List<ProductCreditLimitFetchDto>, List<ProductCreditLimitDto>>(prodLimits);
                            pii.product = (from s in sortProds
                                           where s.sysID == mprod
                                           select s).FirstOrDefault();
                            rval.Add(pii);

                        }
                        return rval;
                    }

                    //directly output:
                    if (queueResult != null && queueResult.Count > 0)
                    {
                        /*qPRODUCTOUT
                        * Queuefeld Beschreibung Wertebereich
                           F01 SYSPRPRODUCT    Numerisch / 4875
                           F02 ZINS    Numerisch / 8.8
                           F03 LZ  Numerisch / 48
                           F04 MAX BETRAG PRISMA   Numerisch / 25000
                           F05 MAX BETRAG LIMIT    Numerisch / 25000
                           F10 RATE    Numerisch / 345.85
                           F09 STATUS  String / OK oder NOK
                           F11 PROVISION   Numerisch / 345.85
                           F12 PROVISIONP  Numerisch / 8.8
                           F13 RATENABSICHERUNG    Numerisch / 1 oder 0
                         */
                        CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto outQueue = (from f in queueResult
                                                                                       where f.Name.Equals("qPRODUCTOUT")
                                                                                       select f).FirstOrDefault();
                        foreach(CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto qr in outQueue.Records)
                        {
                            try
                            {
                                long sysprproduct = long.Parse(getQueueRecordValue(qr, "F01"));
                                long lz = long.Parse(getQueueRecordValue(qr, "F03"));
                                ProductCreditInfoDto pi = (from f in rval
                                                           where f.product.sysID == sysprproduct
                                                           select f).FirstOrDefault();
                                ProductCreditLimitDto li = (from f in pi.creditLimits
                                                            where f.laufzeit == lz
                                                            select f).FirstOrDefault();
                                if (li == null) continue;
                                li.creditLimit = double.Parse(getQueueRecordValue(qr, "F05"), CultureInfo.InvariantCulture);
                                li.status = getQueueRecordValue(qr, "F09");
                                li.rate = double.Parse(getQueueRecordValue(qr, "F10"), CultureInfo.InvariantCulture);
                                li.provision = double.Parse(getQueueRecordValue(qr, "F11"), CultureInfo.InvariantCulture);
                                li.provisionp = double.Parse(getQueueRecordValue(qr, "F12"), CultureInfo.InvariantCulture);
                                li.ratenAbsicherung = int.Parse(getQueueRecordValue(qr, "F13"), CultureInfo.InvariantCulture);
                            }catch(Exception re)
                            {
                                _log.Debug("Problem processing qPRODUCTOUT-QueueRecord: " + re.Message);
                            }
                        }
                    }
                }
                catch (Exception te)
                {
                    _log.Error("Error processing EL QUEUE Result", te);
                }
                return rval;
            }


            EaiparDao eaiParDao = new EaiparDao();
            string kalkulationWertenInDB = eaiParDao.getEaiParFileByCode("KRWINDB_EL", null);
            if (kalkulationWertenInDB != null && kalkulationWertenInDB != "" && kalkulationWertenInDB.ToUpper().Equals("TRUE"))
            {
                _log.Debug("KALKULATIONWERTEN: " + XMLSerializer.SerializeUTF8WithoutNamespace(debugListe));
                trDao.CreateUpdateLogDump(XMLSerializer.SerializeUTF8WithoutNamespace(debugListe), "KRWINDB_EL", "Antrag", sysantrag);
            }
            return rval;
        }



        public List<ProductCreditInfoDto> getCreditLimitsRE(prKontextDto kontext, long sysantrag, long sysvart, String isoCode, long syswfuser)
        {
            if (sysvart >= 0)
                kontext.sysvart = sysvart;
            long sysvartOrg = kontext.sysvart;


            Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo pruefungbo = new Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo(PrismaDaoFactory.getInstance().getPrismaDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getPruefungDao());
            String ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "RULEENGINE_EXPECTEDLOSS", "USE_RULESET_VAR", "");

            List<ProductCreditInfoDto> rval = new List<ProductCreditInfoDto>();

            List<ProductCreditLimitFetchDto> savedItems = trDao.fetchCreditLimits(sysantrag);
            //Try loading from db, when already there use this data, no RuleEngine
            if (savedItems != null && savedItems.Count > 0)
            {
                List<long> savedProds = (from pi in savedItems
                                         select pi.sysprproduct).Distinct().ToList();
                //Wunschkredit als erstes Produkt anfügen
                long wunschKreditProduct = savedItems[0].sysprproduct;
                savedProds.Remove(wunschKreditProduct);
                savedProds.Insert(0, wunschKreditProduct);


                List<PRPRODUCT> availproducts = prodBo.listAvailableProducts(kontext);
                List<AvailableProduktDto> sortedAvailProds = prodBo.listSortedAvailableProducts(availproducts, kontext.sysprbildwelt);

                
                Common.DTO.JokerPruefungDto jokerResult = pruefungbo.analyzeJokerProducts(sortedAvailProds.ToArray(), kontext);
                sortedAvailProds = jokerResult.products;

                foreach (long mprod in savedProds)
                {
                    List<ProductCreditLimitFetchDto> prodLimits = (from pi in savedItems
                                                                   where pi.sysprproduct == mprod
                                                                   select pi).ToList();
                    ProductCreditInfoDto pii = new ProductCreditInfoDto();
                    pii.creditLimits = Mapper.Map<List<ProductCreditLimitFetchDto>, List<ProductCreditLimitDto>>(prodLimits);
                    pii.product = (from s in sortedAvailProds
                                   where s.sysID == mprod
                                   select s).FirstOrDefault();
                    if (pii.product == null) continue; //saved product no more in sorttree
                    rval.Add(pii);

                }
                return rval;
            }




            //INITIALIZATION 
            ELCalcVars antvars = trDao.getAntragDaten(sysantrag);

            //STEP 1 DECISION-ENGINE, get Values PD,Cluster,Budgetpuffer
            //DEDETAIL.PD,-DEDETAIL.CLUSTER,-DEDETAIL.FREIBETRAG  where DEDETAIL.ANTRAGSTELLER  = 1
            DEValues4KR devals = trDao.getDecisionValues(sysantrag);
            if (devals == null || devals.CLUSTERVALUE == null || devals.CLUSTERVALUE.Length == 0)
            {
                _log.Warn("No CLUSTERVALUE for Antrag " + sysantrag + " found in DE-Data");
                return rval;
            }

            if (devals.PD == 0)
            {
                devals.PD = 1.0;
            }

            //STEP 2 - get KREMO.BUDGET1(Saldo) 
            //Max(SYSKREMO) zum Antrag (Saldo bei 2. An-tragsteller)
            Budget4DESimDto budget = this.trDao.getBudget4DESim(sysantrag);
            bool hasMA = this.trDao.hasMA(sysantrag);

            //STEP3 - values from VG FlagsKR
            //DONE INSIDE LOOP            

            //STEP4 - LZ from VG - (inner iteration min/max  for-loop below)
            List<int> laufzeiten = trDao.getLaufzeiten();


            //STEP5 - availableProducts - (outer iteration  for-loop below)

            List<PRPRODUCT> products = prodBo.listAvailableProducts(kontext);
            List<AvailableProduktDto> sortProds = prodBo.listSortedAvailableProducts(products, kontext.sysprbildwelt);
            Common.DTO.JokerPruefungDto resultJokerPruefung = pruefungbo.analyzeJokerProducts(sortProds.ToArray(), kontext);
            sortProds = resultJokerPruefung.products;


            //reorganize sortProds by vart
            //sortProds = sortProds.OrderBy(a => a.vttypcode).ToList();
            _log.Debug("getCreditLimits for " + sortProds.Count + " products..");

            //AFTER ALL VARIABLES NOW AVAILABLE, perform the loop below and calculate
            long sysvttypOrg = kontext.sysvttyp;

            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            IZinsBo zinsBo = BOFactory.getInstance().createZinsBo(ZinsBo.CONDITIONS_BANKNOW, isoCode);
            IQuoteDao quoteDao = OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao();
            ConditionLinkType[] CONDITIONS_BANKNOW_KR = { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.PRODUCT, ConditionLinkType.BCHNL, ConditionLinkType.OBART, ConditionLinkType.KDTYP, ConditionLinkType.USETYPE, ConditionLinkType.VART, ConditionLinkType.VTTYP, ConditionLinkType.PEROLE };
            IPrismaParameterBo paramBo = PrismaBoFactory.getInstance().createPrismaParameterBo(CONDITIONS_BANKNOW_KR);


            List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> vars = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto>();
            CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto qv = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto();
            qv.Name = "qPRODUCTIN";
            vars.Add(qv);
            vars.Add(new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto() { Name = "qPRODUCTOUT" });
            List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto> qrecs = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto>();

            double szQuote = quoteDao.getQuote(QuoteDao.SZ_SCHWELLE_QUOTE);


            foreach (AvailableProduktDto prod in sortProds)//for every available Product
            {
                if (prod.sysID < 0)
                {
                    continue;//Disabled product
                }
                VART vart = prismaDao.getVertragsart(prod.sysID);
                if (sysvartOrg != 0 && vart.SYSVART != sysvartOrg)
                {
                    continue;//check vart von Produkt
                }
                if (sysvartOrg == 0)
                {
                    kontext.sysvart = vart.SYSVART;
                }
                kontext.sysvttyp = sysvttypOrg;
                kontext.sysprproduct = prod.sysID;
                //STEP6 - product-parameters (eg lz to avoid some values in the inner loop below)

                List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> prodParams = paramBo.listAvailableParameter(kontext);
                Cic.OpenOne.Common.DTO.Prisma.ParamDto lzparam = (from lzp in prodParams
                                                                  where lzp.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.kalkBorderLz))

                                                                  select lzp).FirstOrDefault();

                Cic.OpenOne.Common.DTO.Prisma.ParamDto zfac = (from zf in prodParams
                                                               where zf.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.ZFAKTOR))
                                                               select zf).FirstOrDefault();

                Cic.OpenOne.Common.DTO.Prisma.ParamDto elv = (from zf in prodParams
                                                              where zf.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.EL_WERTEGRUPPE))
                                                              select zf).FirstOrDefault();



                Cic.OpenOne.Common.DTO.Prisma.ParamDto maxKreditbetrag = (from zf in prodParams
                                                                          where zf.meta.Equals(EnumUtil.GetStringValue(PrismaParameters.KalkBorderBgIntern))
                                                                          select zf).FirstOrDefault();
                //STEP3 - values from VG FlagsKR
                //DONE INSIDE LOOP
                Flags4KR flags = null;
                if (elv != null)
                    flags = getFaktoren(elv.sysvg, devals.CLUSTERVALUE);

                decimal zfaktor = devals.ZFAKTOR;
                if (flags != null && flags.ZFAKTOR.HasValue)
                {
                    zfaktor = flags.ZFAKTOR.Value;
                }
                if (zfac != null)
                {
                    zfaktor = (decimal)zfac.defvaln;
                }
                VartDto vartDto = angAntDao.getVart(kontext.sysvart);
                foreach (int lz in laufzeiten)//for every configured runtime from step 4 
                {
                    //check if lz is available for product, else set result value to zero and skip
                    if (lzparam != null)
                    {
                        if (lzparam.minvaln > lz)
                        {
                            continue;
                        }
                        if (lzparam.maxvaln < lz)
                        {
                            continue;
                        }
                        if (lz % lzparam.stepsize != 0.0)
                        {
                            continue;//stepsize mismatch
                        }
                    }

                    //budget1*zfaktor für 1 as
                    //saldo * zfaktor für 2 as
                    //STEP8 - calculate max creditlimit
                    double maxkreditlimit = (double)(hasMA ? budget.saldo.Value * zfaktor : budget.budget1.Value * zfaktor);
                    if (lz < 36)
                    {
                        if (devals != null)
                        {
                            if (hasMA)
                            {
                                maxkreditlimit = (double)(budget.saldo.Value - devals.BUDGETPUFFER) * lz;
                            }
                            else
                            {
                                maxkreditlimit = (double)(budget.budget1.Value - devals.BUDGETPUFFER) * lz;
                            }
                        }
                        else
                        {
                            _log.Info("don't contains values from Decision Engine needed for EL Calculation");
                            if (hasMA)
                            {
                                maxkreditlimit = (double)(budget.saldo.Value) * lz;
                            }
                            else
                            {
                                maxkreditlimit = (double)(budget.budget1.Value) * lz;
                            }
                        }

                    }

                    if (maxkreditlimit < 0)
                    {
                        maxkreditlimit = 0;
                    }
                    else
                    {
                        maxkreditlimit = Math.Floor(maxkreditlimit / 1000) * 1000;
                    }

                    if (maxKreditbetrag != null)
                    {
                        if (maxkreditlimit > maxKreditbetrag.maxvaln)
                        {
                            maxkreditlimit = maxKreditbetrag.maxvaln;
                        }
                    }

                    //maxkreditlimit für iteration, when Zero bail out
                    double usekredit = maxkreditlimit;
                    if (usekredit == 0)
                    {
                        _log.Warn("No getCreditLimits for  ANTRAG=" + sysantrag + " possible, usekredit = 0 ");
                        return rval;
                    }

                    double ust = mwStBo.getMehrwertSteuer(1, vart.SYSVART, kontext.perDate);
                    double[] zinsen = getZinsByProdukt(lz, ust, devals.SCORETOTAL.ToString(), kontext, prod.sysID, isoCode, usekredit, antvars.szbrutto, kontext.sysprkgroup, vartDto.code, zinsBo, szQuote);

                    /* F01 = sysprproduct
                    * F02 = zins nominal = zinsen[0]
                    * F03 = Laufzeit
                    * F04 = usekredit
                    * qPRODUCTIN
                    */
                    double effzins = CalculateEffectiveInterest(zinsen[0], 12);
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto rec = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                    rec.Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[5];
                    rec.Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[0].VariableName = "F01";
                    rec.Values[0].Value = prod.sysID.ToString();
                    rec.Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[1].VariableName = "F02";
                    rec.Values[1].Value = zinsen[0].ToString();
                    rec.Values[2] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[2].VariableName = "F03";
                    rec.Values[2].Value = lz.ToString();
                    rec.Values[3] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[3].VariableName = "F04";
                    rec.Values[3].Value = usekredit.ToString();
                    rec.Values[4] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[4].VariableName = "F05";
                    rec.Values[4].Value = effzins.ToString();
                    qrecs.Add(rec);
                }


            }


            qv.Records = qrecs.ToArray();

            CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] variables = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[1];
            variables[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
            variables[0].VariableName = "PP";
            variables[0].LookupVariableName = "SPRACHE";
            variables[0].Value = isoCode;
            List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queueResult = BPEBo.getQueueData(sysantrag, "ANTRAG", new String[] { "qRULES" }, ruleCode, variables, syswfuser, vars);

            try
            {
                // loading from db:
                savedItems = trDao.fetchCreditLimits(sysantrag);
                if (savedItems != null && savedItems.Count > 0)
                {
                    List<long> savedProds = (from pi in savedItems
                                             select pi.sysprproduct).Distinct().ToList();
                    //Wunschkredit als erstes Produkt anfügen
                    long wunschKreditProduct = savedItems[0].sysprproduct;
                    savedProds.Remove(wunschKreditProduct);
                    savedProds.Insert(0, wunschKreditProduct);




                    rval.Clear();
                    foreach (long mprod in savedProds)
                    {
                        List<ProductCreditLimitFetchDto> prodLimits = (from pi in savedItems
                                                                       where pi.sysprproduct == mprod
                                                                       select pi).ToList();
                        ProductCreditInfoDto pii = new ProductCreditInfoDto();
                        pii.creditLimits = Mapper.Map<List<ProductCreditLimitFetchDto>, List<ProductCreditLimitDto>>(prodLimits);
                        pii.product = (from s in sortProds
                                       where s.sysID == mprod
                                       select s).FirstOrDefault();
                        rval.Add(pii);

                    }
                    return rval;
                }

                //directly from RE output:
                if (queueResult != null && queueResult.Count > 0)
                {
                    /*qPRODUCTOUT
                    * Queuefeld Beschreibung Wertebereich
                        F01 SYSPRPRODUCT    Numerisch / 4875
                        F02 ZINS    Numerisch / 8.8
                        F03 LZ  Numerisch / 48
                        F04 MAX BETRAG PRISMA   Numerisch / 25000
                        F05 MAX BETRAG LIMIT    Numerisch / 25000
                        F10 RATE    Numerisch / 345.85
                        F09 STATUS  String / OK oder NOK
                        F11 PROVISION   Numerisch / 345.85
                        F12 PROVISIONP  Numerisch / 8.8
                        F13 RATENABSICHERUNG    Numerisch / 1 oder 0
                        */
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto outQueue = (from f in queueResult
                                                                                   where f.Name.Equals("qPRODUCTOUT")
                                                                                   select f).FirstOrDefault();
                    foreach (CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto qr in outQueue.Records)
                    {
                        try
                        {
                            long sysprproduct = long.Parse(getQueueRecordValue(qr, "F01"));
                            long lz = long.Parse(getQueueRecordValue(qr, "F03"));
                            ProductCreditInfoDto pi = (from f in rval
                                                       where f.product.sysID == sysprproduct
                                                       select f).FirstOrDefault();
                            ProductCreditLimitDto li = (from f in pi.creditLimits
                                                        where f.laufzeit == lz
                                                        select f).FirstOrDefault();
                            if (li == null) continue;
                            li.creditLimit = double.Parse(getQueueRecordValue(qr, "F05"), CultureInfo.InvariantCulture);
                            li.status = getQueueRecordValue(qr, "F09");
                            li.rate = double.Parse(getQueueRecordValue(qr, "F10"), CultureInfo.InvariantCulture);
                            li.provision = double.Parse(getQueueRecordValue(qr, "F11"), CultureInfo.InvariantCulture);
                            li.provisionp = double.Parse(getQueueRecordValue(qr, "F12"), CultureInfo.InvariantCulture);
                            li.ratenAbsicherung = int.Parse(getQueueRecordValue(qr, "F13"), CultureInfo.InvariantCulture);
                        }
                        catch (Exception re)
                        {
                            _log.Debug("Problem processing qPRODUCTOUT-QueueRecord: " + re.Message);
                        }
                    }
                }
            }
            catch (Exception te)
            {
                _log.Error("Error processing EL QUEUE Result", te);
            }
            return rval;


        }


        public static double CalculateEffectiveInterest(double nominalInterest, int ppy)
        {
            int Ppy = ppy;

            double d = System.Math.Pow((double)(1 + (nominalInterest / (100 * Ppy))), (double)Ppy);

            d = (d - 1) * 100;

            return d;
        }

        /// <summary>
        /// returns the value from record with given variablename
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static String getQueueRecordValue(CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto rec, String id)
        {
            if (rec == null || rec.Values == null) return null;
            return  (from f in rec.Values
                                        where f.VariableName.Equals(id)
                                        select f.Value).FirstOrDefault();
        }

        private static void addVar(String lvname, String varName, String value, List<CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto> vars)
        {
            CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto v = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
            v.LookupVariableName = lvname;
            v.VariableName = varName;
            v.Value = value;
            vars.Add(v);
        }
        /// <summary>
        /// Calculates the EL and performs a product check and tests the cut-off values
        /// returns true if successful for the given kredit-value
        /// </summary>
        /// <param name="kreditvalue"></param>
        /// <param name="zinsertrag"></param>
        /// <param name="flags"></param>
        /// <param name="kontext"></param>
        /// <param name="sysantrag"></param>
        /// <param name="lz"></param>
        /// <param name="cvars"></param>
        /// <param name="devals"></param>
        /// <param name="budget"></param>
        /// <param name="zfaktor"></param>
        /// <returns></returns>
        private bool elCalcIteration(double kreditvalue, double zinsertrag, Flags4KR flags,  prKontextDto kontext, long sysantrag, int lz, ELCalcVars cvars, DEValues4KR devals, Budget4DESimDto budget, double zfaktor, ELResults elResult, ref string grund, bool hasMA)
        {
            ELResults elvals = calcEL(flags, lz, devals.PD, kreditvalue, zinsertrag, sysantrag);

            elResult.EL = elvals.EL;
            elResult.ELP = elvals.ELP;
            elResult.ELPROF = elvals.ELPROF;
           
            //STEP9 compare STEP7 values with cut-off, if not met, mark as unavailable
            if (elvals.EL <= flags.ELBETRAG && elvals.ELP <= flags.ELPROC && elvals.ELPROF >= flags.ELPROF)
            {
                //Produktprüfung ,wenn fail, continue
                if (!performProductValidation(elvals, flags, kontext, sysantrag, lz, cvars, devals, budget, (double)zfaktor,kreditvalue, ref grund, hasMA))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (elvals.EL > flags.ELBETRAG)
                {
                    grund+= " Expected Loss groesser Betrag ist groesser als "+   flags.ELBETRAG;
                }
                if (elvals.ELP > flags.ELPROC)
                {
                    grund += " - Expected Loss Prozent ist groesser als "+ flags.ELPROC;
                }
                if (elvals.ELPROF < flags.ELPROF)
                {
                    grund += " - Profitabilität ist kleiner als "+flags. ELPROF;
                }

                return false;
            
            }
        }

        /// <summary>
        ///  getCreditLimits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        override public ogetCreditLimitsGUIDto getCreditLimits(igetCreditLimitsGUIDto input)
        {
            ogetCreditLimitsGUIDto result = new ogetCreditLimitsGUIDto();
            prKontextDto kontextNew = new prKontextDto();
            kontextNew.sysprbildwelt = input.sysprbildwelt;
            kontextNew.prprodtype= input.prprodtype;
            kontextNew.sysprchannel = input.sysprchannel;
            kontextNew.sysprusetype = input.sysprusetype;
            kontextNew.sysperole = input.sysperole;
            kontextNew.syskdtyp = input.syskdtyp;
            kontextNew.perDate = input.perDate;
            kontextNew.sysvart = input.sysvart;
            kontextNew.sysprhgroup = input.sysprhgroup;
            kontextNew.sysprkgroup = input.sysprkgroup;
            kontextNew.sysvttyp = input.sysvttyp;



            result.productCreditInfoDto = getCreditLimits(kontextNew, input.sysantrag, input.sysvart, input.isoCode, input.syswfuser);
            result.XMLDto = XMLSerializer.SerializeUTF8WithoutNamespace(result);
            return result;
        }

        /// <summary>
        /// Calculates Credit Expected Loss for the offer and product context providing gui debug data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        override public ocheckKrRiskByIdDto checkKrRiskById(icheckKrRiskByIdDto input)
        {
            ocheckKrRiskByIdDto rval = new ocheckKrRiskByIdDto();

            if ((input.kontext == null || input.kontext.sysperole == 0) && input.sysPEROLE>0 && input.sysid>0)
            {
                input.kontext = MyCreateProductKontext(input.sysPEROLE, input.sysid);
            }

            ConditionLinkType[] CONDITIONS_BANKNOW_KR = { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.PRODUCT, ConditionLinkType.BCHNL, ConditionLinkType.OBART, ConditionLinkType.KDTYP, ConditionLinkType.USETYPE, ConditionLinkType.VART, ConditionLinkType.VTTYP, ConditionLinkType.PEROLE };

            
            ELCalcVars antvars = trDao.getAntragDaten(input.sysid);
            DEValues4KR devals = trDao.getDecisionValues(input.sysid);

            IPrismaParameterBo paramBo = PrismaBoFactory.getInstance().createPrismaParameterBo(CONDITIONS_BANKNOW_KR);
            ParamDto zfac = paramBo.getParameter(input.kontext, EnumUtil.GetStringValue(PrismaParameters.ZFAKTOR));
            if (zfac != null)
            {
                rval.zfaktor = zfac.defvaln;
            }

            ParamDto elv = paramBo.getParameter(input.kontext, EnumUtil.GetStringValue(PrismaParameters.EL_WERTEGRUPPE));
            if (devals == null || devals.CLUSTERVALUE == null || devals.CLUSTERVALUE.Length == 0)
            {
                _log.Warn("No CLUSTERVALUE for Antrag " + input.sysid + " found in DE-Data");
                return null;
            }
            if (elv == null)
            {
                _log.Warn("No EL Calculation for ANTRAG=" + input.sysid + " possible - no EL_WERTEGRUPPE-Parameter found for product=" + input.kontext.sysprproduct);
                return null;
            }
            long zinsVg = trDao.getZinsertragWertegruppe();
            string lztext = (antvars.lz + 1).ToString();
            double lzzins = (double)vgDao.getVGValue(zinsVg, DateTime.Now, devals.CLUSTERVALUE, lztext, VGInterpolationMode.MIN, plSQLVersion.V1); //Faktor zur Berechnung
            Flags4KR flags = getFaktoren(elv.sysvg, devals.CLUSTERVALUE);
            if (antvars.bgexternbrutto == 0)
            {
                _log.Warn("No checkKrRiskById for ANTRAG=" + input.sysid + " possible - bgexternbrutto = 0 ");
                return null;
            } 
            double zinsertrag = -1.0 * System.Numeric.Financial.CumIPmt(antvars.zins / 1200.0, antvars.lz, antvars.bgexternbrutto, 1, Math.Min(antvars.lz, lzzins), System.Numeric.PaymentDue.EndOfPeriod);
            ELResults elvals = calcEL(flags, (long)antvars.lz, devals.PD, antvars.bgexternbrutto, zinsertrag, input.sysid);

            rval.devalues = devals;
            rval.antvars = antvars;
            rval.elresults = elvals;
            rval.flags = flags;
            rval.zinsertrag = zinsertrag;

            String outputGUIString = XMLSerializer.SerializeUTF8WithoutNamespace(rval);
            _log.Info("OUTPUT KR: " + outputGUIString);
            _log.Info("CALCEL Laufzeit " + antvars.lz +" : " + XMLSerializer.SerializeUTF8WithoutNamespace(elvals));

            return rval;
        }

        /// <summary>
        /// Calculates Credit Expected Loss for the offer and product context
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="kontext"></param>
        /// <returns></returns>
        override public VClusterDto getELValues(long sysid, prKontextDto kontext)
        {
            VClusterDto rval = new VClusterDto();
            icheckKrRiskByIdDto input = new icheckKrRiskByIdDto();
            input.sysid = sysid;
            input.kontext = kontext;
            ocheckKrRiskByIdDto tmprval = checkKrRiskById(input);
            if (tmprval == null || tmprval.elresults == null)
            {
                return rval;
            }

            rval.v_el_betrag = tmprval.elresults.EL.Value;
            rval.v_el_prozent = tmprval.elresults.ELP.Value;
            rval.v_prof = tmprval.elresults.ELPROF.Value;

            return rval;
        }


        /// <summary>
        /// Productvalidation
        /// </summary>
        /// <param name="elvals"></param>
        /// <param name="flags"></param>
        /// <param name="kontext"></param>
        /// <param name="sysantrag"></param>
        /// <param name="lz"></param>
        /// <param name="cvars"></param>
        /// <param name="devals"></param>
        /// <param name="budget"></param>
        /// <param name="zfaktor"></param>
        /// <param name="kreditvalue"></param>
        /// <returns></returns>
        private bool performProductValidation(ELResults elvals, Flags4KR flags, prKontextDto kontext, long sysantrag, long lz, ELCalcVars cvars, DEValues4KR devals, Budget4DESimDto budget, double zfaktor, double kreditvalue, ref string text, bool hasMA)
        {
            text = "";
            double budgetval = (double)(hasMA ? budget.saldo.Value : budget.budget1.Value);

            if (lz >= 36)//KEL1
            {
              
                if (elvals.EL > flags.ELBETRAG)
                {
                    text += " Expected Loss groesser Betrag ist groesser als " + flags.ELBETRAG;
                }
                if (elvals.ELP > flags.ELPROC)
                {
                    text += " - Expected Loss Prozent ist groesser als " + flags.ELPROC;
                }
                if (elvals.ELPROF < flags.ELPROF)
                {
                    text += " - Profitabilität ist kleiner als " + flags.ELPROF;
                }
                if ((kreditvalue + cvars.rsvgesamt) > (budgetval - devals.FREIBETRAG) * zfaktor)
                {
                    text += " (kreditvalue + cvars.rsvgesamt) gröesser als (budgetval - devals.FREIBETRAG) * zfaktor ";
                }
                if (cvars.ratebrutto > (budgetval - devals.FREIBETRAG))
                {
                    text += "ratebrutto " + cvars.ratebrutto + " ist grösser als budget1 " + budgetval + "-Freibetrag " + devals.FREIBETRAG;
                }

                //TR zu hoch
                    
               if (text != "")
               {
                   return false;
               }
            }
            else//lz <36 - KEL1
            {
                if (elvals.EL > flags.ELBETRAG)
                {
                    text += " Expected Loss groesser Betrag = elvals.EL ist groesser als " + flags.ELBETRAG;
                }
                if (elvals.ELP > flags.ELPROC)
                {
                    text += " - Expected Loss Prozent ist groesser als " + flags.ELPROC;
                }
                if (elvals.ELPROF < flags.ELPROF)
                {
                    text += " - Profitabilität ist kleiner als " + flags.ELPROF;
                }

                if ((kreditvalue + cvars.rsvgesamt) > (budgetval - devals.FREIBETRAG) * lz)
                {
                    text += " kreditvalue + rsvgesamt) gröesser als  (budgetval - devals.FREIBETRAG) * lz ";
                }
                if (cvars.ratebrutto > (budgetval - devals.FREIBETRAG))
                {
                    text += "ratebrutto " + cvars.ratebrutto + " ist grösser als budget1 " + budgetval + "-Freibetrag " + devals.FREIBETRAG;
                }
                //TR zu hoch
                if (text != "")
                {
                    return false;
                }
            }


            if (MyIsGreaterThanMaxVal(cvars.endAlterKunde, cvars.maxAlter))
            {
                text = "endAlterKunde > maxAlter";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Quick access to a certain queue record value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        private static CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto getQueueRecordValue(String name, CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto record)
        {
            return (from r in record.Values
                    where r.VariableName.Equals(name)
                    select r).FirstOrDefault();
        }

        /// <summary>
        /// Performs the EL Product Validation
        /// </summary>
        /// <param name="rval"></param>
        /// <param name="sysid"></param>
        /// <param name="kontext"></param>
        override public void performProductValidation(ocheckAntAngDto rval, long sysid, prKontextDto kontext, String isoCode, bool hasMA)
        {
           

            icheckKrRiskByIdDto input = new icheckKrRiskByIdDto();
            input.sysid = sysid;
            input.kontext = kontext;
            ocheckKrRiskByIdDto checkVals = checkKrRiskById(input);
            if (checkVals == null)
            {
                MyAddErrorMessage(rval, "CHECKANTRAG_IMP", isoCode, "Transaktionsrisiko Prüfung nicht möglich", ocheckAntAngDto.STATUS_RED, "KEL1");
            }
            else
            {
                Budget4DESimDto budget = this.trDao.getBudget4DESim(sysid);
                double budgetval = (double)(hasMA ? budget.saldo.Value : budget.budget1.Value);

                if (checkVals.antvars.lz >= 36)//KEL1
                {

                    decimal zfaktor = checkVals.devalues.ZFAKTOR;
                    if (checkVals.flags.ZFAKTOR.HasValue)
                    {
                        zfaktor = checkVals.flags.ZFAKTOR.Value;
                    }
                    if (checkVals.zfaktor.HasValue)
                    {
                        zfaktor = (decimal)checkVals.zfaktor;
                    }

                    if (checkVals.elresults.EL > checkVals.flags.ELBETRAG
                        || checkVals.elresults.ELP > checkVals.flags.ELPROC
                        || checkVals.elresults.ELPROF < checkVals.flags.ELPROF
                        || (checkVals.antvars.bginternbrutto + checkVals.antvars.rsvgesamt) > (budgetval - checkVals.devalues.FREIBETRAG) * (double)zfaktor
                        || checkVals.antvars.ratebrutto > (budgetval - checkVals.devalues.FREIBETRAG))
                        //TR zu hoch
                    {
                        if (checkVals.flags.FORCIERBAR)
                        {
                            MyAddErrorMessage(rval, "CHECKANTRAG_KEL1", isoCode, "Transaktionsrisiko für diese Transaktion zu hoch.", ocheckAntAngDto.STATUS_YELLOW, "KEL1");
                        }
                        else
                        {
                            MyAddErrorMessage(rval, "CHECKANTRAG_KEL1", isoCode, "Transaktionsrisiko für diese Transaktion zu hoch.", ocheckAntAngDto.STATUS_RED, "KEL1");
                        }
                    }
                }
                else//lz <36 - KEL1
                {
                    if (checkVals.elresults.EL > checkVals.flags.ELBETRAG
                                      || checkVals.elresults.ELP > checkVals.flags.ELPROC
                                      || checkVals.elresults.ELPROF < checkVals.flags.ELPROF
                                      || (checkVals.antvars.bginternbrutto + checkVals.antvars.rsvgesamt) > (budgetval - checkVals.devalues.FREIBETRAG) * checkVals.antvars.lz
                                      || checkVals.antvars.ratebrutto > (budgetval - checkVals.devalues.FREIBETRAG))


                        //TR zu hoch
                    {
                        if (checkVals.flags.FORCIERBAR)
                        {
                            MyAddErrorMessage(rval, "CHECKANTRAG_KEL1", isoCode, "Transaktionsrisiko für diese Transaktion zu hoch.", ocheckAntAngDto.STATUS_YELLOW, "KEL1");
                        }
                        else
                        {
                            MyAddErrorMessage(rval, "CHECKANTRAG_KEL1", isoCode, "Transaktionsrisiko für diese Transaktion zu hoch.", ocheckAntAngDto.STATUS_RED, "KEL1");
                        }
                    }
                }

            }
        }

        /// <summary>
        /// EL_DEFlag  Es sollte kein TR berechnet werden, wenn das CLUSTERVALUE nicht von der DE übergeben wird
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public override bool KRBerechnen(long sysid)
        {
            try
            { 
                DEValues4KR devals = trDao.getDecisionValues(sysid);
                if (devals.CLUSTERVALUE != null && devals.CLUSTERVALUE.Length > 0)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }


        private double MyGetEndAlterKunde(DateTime? endeAm, DateTime? gebDatum)
        {
            if (gebDatum != null && endeAm != null && gebDatum < endeAm)
            {
                return MyGetJahreDiff((DateTime)endeAm, (DateTime)gebDatum);
            }

            return 0;
        }
        private bool MyIsGreaterThanMaxVal(double valueToCompare, double maxValue)
        {
            // Wenn PRPARAM.MAXVALN gleich 0 ist, dann soll sie nicht berücksichtigt werden.
            if (maxValue > 0)
            {
                // Wenn PRPARAM.MAXVALN > 0 und valueToCompare > maxValue, dann wird ein Fehlertext ausgegeben.
                return (valueToCompare > maxValue);
            }
            // Kein Fehler
            return false;
        }
        /// <summary>
        /// getJahreDiff
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        private double MyGetJahreDiff(DateTime date1, DateTime date2)
        {
            return MyGetMonatDiff(date1, date2) / 12;
        }

        /// <summary>
        /// getMonatDiff
        /// </summary>
        /// <param name="ndate1"></param>
        /// <param name="ndate2"></param>
        /// <returns></returns>
        private double MyGetMonatDiff(DateTime? ndate1, DateTime? ndate2)
        {
            if (ndate1 == null && ndate2 == null)
            {
                return 0;
            }

            DateTime date1 = ndate1 == null ? Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) : ndate1.Value;
            DateTime? dt2temp = DateTimeHelper.ClarionDateToDtoDate(ndate2);
            DateTime date2 = dt2temp == null ? Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) : ndate2.Value;

            // Beide Daten in einer Liste speichern und sortieren 
            List<DateTime> period = new List<DateTime>() { date1, date2 };
            period.Sort(DateTime.Compare);

            // Ganze Monate zählen
            int monthsFull;
            for (monthsFull = 0; period[0].AddMonths(monthsFull + 1).CompareTo(period[1]) <= 0; monthsFull++)
            {
                ;
            }

            int dayDiff = 0;
            if (date2.Day != date1.Day)
            {
                DateTime startTime = period[0];
                DateTime endTime = period[1];
                dayDiff = endTime.Day - startTime.Day;
                if (dayDiff < 0)
                {
                    dayDiff += 31;
                }
            }

            // nur die Resttage durch 31 teilen
            return monthsFull + dayDiff / 31.0;
        }
        
        /// <summary>
        /// Calculate Expected Loss
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="lz"></param>
        /// <param name="pd"></param>
        /// <param name="kreditbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        private ELResults calcEL(Flags4KR flags, long lz, double pd, double kreditbetrag, double zinsertrag, long sysantrag)
        {
            ELResults rval = new ELResults();

            rval.EL = trDao.calculateEL(flags, lz, pd, kreditbetrag, sysantrag);
           

            if (rval.EL.HasValue)
            {
                rval.ELPROF = trDao.calculateELPROF(flags, lz, pd, kreditbetrag, zinsertrag, rval.EL.Value, sysantrag);
                rval.ELP = rval.EL.Value / kreditbetrag * 100;
            }

            _log.Debug("CALCEL Laufzeit "+lz+" :" +XMLSerializer.SerializeUTF8WithoutNamespace(rval));
            return rval;
            
        }

        
        public Flags4KR getFaktoren(long sysvg, string scorebezeichnung)
        {
            Flags4KR faktorenDto = new Flags4KR();
            faktorenDto.a = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "a", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor zur Berechnung
            faktorenDto.b = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "b", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor zur Berechnung
            faktorenDto.c = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "c", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor zur Berechnung
            faktorenDto.d = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "d", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor zur Berechnung
            faktorenDto.K = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "K", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor zur Berechnung
            faktorenDto.LGD = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "LGD", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor zur Berechnung
            faktorenDto.ELBETRAG = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ELBETRAG", VGInterpolationMode.NONE, plSQLVersion.V1); //Relevant für Produktprüfung
            faktorenDto.ELPROC = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ELPROC", VGInterpolationMode.NONE, plSQLVersion.V1); //Relevant für Produktprüfung
            faktorenDto.ELPROF = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ELPROF", VGInterpolationMode.NONE, plSQLVersion.V1); //Relevant für Produktprüfung
            faktorenDto.FORCIERBAR = vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "FORCIERBAR", VGInterpolationMode.NONE, plSQLVersion.V1)>0?true:false; //Produktprüfregel forcierbar
            faktorenDto.ITERATIONMAX = vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ITMAX", VGInterpolationMode.NONE, plSQLVersion.V1); //Produktprüfregel forcierbar
            faktorenDto.ITERATIONSCHRITT = vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ITSCHRITT", VGInterpolationMode.NONE, plSQLVersion.V1); //Produktprüfregel forcierbar
            faktorenDto.ITERATIONMIN = vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ITMIN", VGInterpolationMode.NONE, plSQLVersion.V1); //Produktprüfregel forcierbar
            
            EaiparDao eaiParDao = new EaiparDao();
            faktorenDto.FORMEL_EL =  eaiParDao.getEaiParFileByCode(scorebezeichnung + "_EL", null);
            faktorenDto.FORMEL_PROF =  eaiParDao.getEaiParFileByCode(scorebezeichnung + "_PROF", null);

            try
            {
                faktorenDto.ELLZLIMIT = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ELLZLIMIT", VGInterpolationMode.NONE, plSQLVersion.V1); //Produktprüfregel forcierbar
            }
            catch
            {
                faktorenDto.ELLZLIMIT = 0; // 0 Lz limit bedeutet keine Limit
            }

            return faktorenDto;

        }

        /// <summary>
        /// MyCreateProductKontext
        /// </summary>
        /// <param name="sysPEROLE"></param>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private Cic.OpenOne.Common.DTO.Prisma.prKontextDto MyCreateProductKontext(long sysPEROLE, long sysid)
        {
            AntragDto antrag = this.angAntDao.getAntrag(sysid, sysPEROLE);
            Cic.OpenOne.Common.DTO.Prisma.prKontextDto pKontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();

            if (antrag != null)
            {
                if (antrag.kalkulation == null)
                {
                    throw new ApplicationException("No active calculation.");
                }

                KalkulationDto activeKalk = antrag.kalkulation;
                // Kontext erstellen
                pKontext = new OpenOne.Common.DTO.Prisma.prKontextDto();

                pKontext.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                pKontext.sysperole = sysPEROLE;
                pKontext.sysprproduct = activeKalk.angAntKalkDto.sysprproduct;
                pKontext.sysbrand = 0;// angebot.sysbrand;
                if (antrag.kunde != null)
                {
                    pKontext.syskdtyp = antrag.kunde.syskdtyp;
                }
                if (antrag.angAntObDto != null)
                {
                    pKontext.sysobart = antrag.angAntObDto.sysobart;
                    pKontext.sysobtyp = antrag.angAntObDto.sysobtyp;
                }
                pKontext.sysprchannel = (long)antrag.sysprchannel;
                pKontext.sysprhgroup = (long)antrag.sysprhgroup;
                pKontext.sysprusetype = activeKalk.angAntKalkDto.sysobusetype;
                pKontext.sysbrand = antrag.sysbrand;
                pKontext.sysobart = antrag.angAntObDto.sysobart;
                pKontext.sysprhgroup = (long)antrag.sysprhgroup;
                pKontext.sysprjoker = antrag.sysprjoker;
                pKontext.sysprproduct = antrag.kalkulation.angAntKalkDto.sysprproduct;
                pKontext.sysvart = antrag.sysvart;
                if (antrag.sysvttyp != null)
                {
                    pKontext.sysvttyp = (long)antrag.sysvttyp;
                }

                pKontext.sysprkgroup = angAntDao.getPrkgroupByAntragID(antrag.sysid);




                return pKontext;
            }
            throw new ApplicationException("No Antrag.");
        }

        // Die Methode vermeidet, dass der Status von rot auf gelb zurückgesetzt wird.
        private String MySetStatus(String oldstatus, String status)
        {
            if (oldstatus == ocheckAntAngDto.STATUS_RED)
            {
                return oldstatus;
            }
            else
            {
                return status;
            }
        }

        /// <summary>
        /// MyAddErrorMessage
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="messageCode"></param>
        /// <param name="isoCode"></param>
        /// <param name="defaultMessage"></param>
        /// <param name="newStatus"></param>
        private void MyAddErrorMessage(ocheckAntAngDto outDto, String messageCode, String isoCode, String defaultMessage, String newStatus, String code)
        {
            outDto.errortext.Add(translateBo.translateMessage(messageCode, isoCode, defaultMessage));
            outDto.status = MySetStatus(outDto.status, newStatus);
            outDto.code.Add(code);
        }

        /// <summary>
        /// MyAddErrorMessage2
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="message"></param>
        /// <param name="newStatus"></param>
        private void MyAddErrorMessage2(ocheckAntAngDto outDto, String message, String newStatus, String code)
        {
            outDto.errortext.Add(message);
            outDto.status = MySetStatus(outDto.status, newStatus);
            outDto.code.Add(code);
        }


        /// <summary>
        /// MyTranslate
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="messageCode"></param>
        /// <param name="defaultMessage"></param>
        /// <returns></returns>
        private string MyTranslate(String messageCode, String isoCode, String defaultMessage)
        {
            return (translateBo.translateMessage(messageCode, isoCode, defaultMessage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lz"></param>
        /// <param name="ust"></param>
        /// <param name="kalkCtx"></param>
        /// <param name="prodCtx"></param>
        /// <param name="sysprproduct"></param>
        /// <param name="isoCode"></param>
        /// <param name="zinsbasis"></param>
        /// <param name="calcPrkgroup"></param>
        /// <param name="szBrutto"></param>
        /// <param name="isCredit"></param>
        /// <returns></returns>
        private double[] getZinsByProdukt(int lz, double ust, string kundenScore, prKontextDto prodCtx, long sysprproduct, string isoCode, double zinsbasis, double szBrutto, long calcPrkgroup, String code, IZinsBo zinsBo, double szquote)
        {
            
            IRounding round = RoundingFactory.createRounding();
            IKalkulationDao kalkulationDao = new KalkulationDao();
            IPrismaDao prismaDao = new PrismaDao();
            


            double[] zinsen = new double[1];
          
            code = code.ToUpper();
            bool istzk = (code.IndexOf("TZK") > -1);//Teilzahlungskauf ist analog Kredit zu rechnen
            bool isCredit = (code.IndexOf("KREDIT") > -1) || istzk;
            bool isCreditClassic = (code.IndexOf("KREDIT_CLASSIC") > -1);
            bool isLeasing = (code.IndexOf("LEAS") > -1);
            bool isDispo = (code.IndexOf("DISPO") > -1) || (code.IndexOf ("FLEX") > -1);
            bool isDiffLeasing = prismaDao.isDiffLeasing(sysprproduct);
            bool isExpress = (code.IndexOf("EXPRESS") > -1);
          

          
         
            double mwst = ust; //always mwst, independent of credit
            if (isCredit)
            {
                ust = 0;
            }


            double quotepercent = szquote;
            if (quotepercent > 0)
            {
                if (isCredit)
                {
                    zinsbasis -= szBrutto;
                }
                else if (szBrutto > (quotepercent / 100 * zinsbasis))
                {
                    zinsbasis -= szBrutto;
                }
            }
          

            //Interests:-----------------------------------------------------------------------------------------------------
            //contains nominalinterets for calculation, order: base, min, max



            PRRAPVAL rapVal = null;
            DateTime perDate = DateTime.Now;
            PRRAP prrap = zinsBo.getPrRap(sysprproduct);
            // BNRNEUN-1382 / Im B2B dürfen RAP Methoden nicht verwendet werden weil es keinen Score gibt 
            if ((kundenScore != null && kundenScore.Length > 0 && Convert.ToDouble(kundenScore) != 0))
            {

                //immer rap-zinsen mitrechnen ausser differenzleasung und wenn kundenscore von aussen
                if (isDiffLeasing || (kundenScore != null && kundenScore.Length > 0 && Convert.ToDouble(kundenScore) != 0))
                {
                    if (prodCtx.sysprkgroup > 0 && prrap != null)
                    {
                        List< PRRAPVAL> rapvaluesliste =
                            zinsBo.getRapValues(prrap.SYSPRRAP, prodCtx.sysprkgroup);
                        if (rapvaluesliste.Count > 0)
                        {
                            rapVal = zinsBo.getRapValByScore(rapvaluesliste, kundenScore);
                        }
                        else
                        {
                            rapVal = zinsBo.getRapValByScore(sysprproduct, kundenScore);
                        }
                    }
                    else
                    {
                        rapVal = zinsBo.getRapValByScore(sysprproduct, kundenScore);
                    }
                    //BNRSIEBEN-406: gibt den Zinssatz anhand des Score-abhängigen Rap-Wertes (PRRAPVAL.VALUE) mit dem abgezogenen Zinsabschlag aus dem Zinsband der zugeordneten Zinstruktur sysintstrct (PRRAP.SYSINSTRCT) zurück
                    if (prrap != null && prrap.SYSINTSTRCT != null && prrap.SYSINTSTRCT.Value != 0 &&
                        rapVal.VALUE != null && rapVal.VALUE.Value != 0)
                    {
                        rapVal.VALUE = zinsBo.getZinsRap(sysprproduct, perDate,
                            lz, zinsbasis,
                            prrap.SYSINTSTRCT.Value, rapVal.VALUE.Value);
                    }
                    zinsen[0] = (double)rapVal.VALUE.Value;
                    double BasisEffectiv = zinsBo.getZins(prodCtx, lz, zinsbasis);

                    
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

                        if (calcPrkgroup > 0)
                        {
                            List< PRRAPVAL> rapvaluesliste =
                                zinsBo.getRapValues(prrap.SYSPRRAP,
                                    (long)calcPrkgroup);


                            if (rapvaluesliste != null)
                            {
                                zinsen[0] =(double) rapvaluesliste.FirstOrDefault().VALUE.Value;
                            }
                            else
                            {
                                zinsen[0] = zinsBo.getZins(prodCtx, lz, zinsbasis);
                            }
                        }
                    }
                    else
                    {
                        zinsen[0] = zinsBo.getZins(prodCtx,lz, zinsbasis);
                    }

                    if (prrap != null)
                    {
                        zinsen[1] = prrap.MINVALUE.HasValue ? (double)prrap.MINVALUE.Value : 0;
                        zinsen[2] = prrap.MAXVALUE.HasValue ? (double)prrap.MAXVALUE.Value : 0;

                    }
                    
                }
            }
            else
            {
                if (prrap != null)
                {
                    zinsen = new double[3];
                    zinsen[0] = (double)zinsBo.getRapZinsByScore(sysprproduct, "0");
                }
                else
                {
                    zinsen[0] = zinsBo.getZins(prodCtx, lz, zinsbasis);
                }

                if (prrap != null)//needed for min max slider 
                {
                    zinsen[1] = prrap.MINVALUE.HasValue ? (double)prrap.MINVALUE.Value : 0;
                    zinsen[2] = prrap.MAXVALUE.HasValue ? (double)prrap.MAXVALUE.Value : 0;
                }
              
            }
           
           //for credit, the interest has to be converted to nominal from effective
                for (int i = 0; i < zinsen.Length; i++)
                {
                    zinsen[i] = CalculateNominalInterest(zinsen[i], 12);
                }

            //Interests:----------------------------------------------------END-------------------------------------------------

            return zinsen;
        }


        /// <summary>
        /// Nominalzins berechnen
        /// </summary>
        /// <param name="effectiveInterest">Effektivzins</param>
        /// <param name="ppy">Zahlungen Pro Jahr</param>
        /// <returns>Nominalzins</returns>
        private static double CalculateNominalInterest(double effectiveInterest, int ppy)
        {
            double Ppy = ppy;

            double d = System.Math.Pow((double)((effectiveInterest / 100) + 1), (double)(1 / Ppy));
            d = (d - 1) * 100 * Ppy;

            return d;
        }


        /// <summary>
        /// Returns true if Antrag has a MA
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public override bool hasMA(long sysid)
        {
            return this.trDao.hasMA(sysid);
        }
    }



}

