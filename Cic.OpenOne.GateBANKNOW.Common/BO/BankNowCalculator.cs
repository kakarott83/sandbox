using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.DTO.Prisma;
using CIC.Database.PRISMA.EF6.Model;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Versicherung;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.DTO.Versicherung;
using Cic.One.Util.IO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// calculator for BankNow
    /// </summary>
    public class BankNowCalculator : AbstractBankNowCalculator
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const String SZ_SCHWELLE_QUOTE = "SZ_SCHWELLE_QUOTE";
        
        List<long> UsedProv = new List<long>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obTypDao"></param>
        /// <param name="zinsDao"></param>
        /// <param name="prismaDao"></param>
        /// <param name="vgdao"></param>
        /// <param name="ProvisionDao"></param>
        /// <param name="subventionDao"></param>
        /// <param name="insuranceDao"></param>
        /// <param name="mwstDao">Mehrwertsteuerermittlungs DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="kalkulationDao">kalkulationDao</param>
        /// <param name="prismaServiceDao">prismaServiceDao</param>
        public BankNowCalculator(IObTypDao obTypDao, IZinsDao zinsDao, IPrismaDao prismaDao, IVGDao vgdao, IProvisionDao ProvisionDao, ISubventionDao subventionDao, IInsuranceDao insuranceDao, IMwStDao mwstDao, IQuoteDao quoteDao, String isoCode, IKalkulationDao kalkulationDao, IPrismaServiceDao prismaServiceDao)
            : base(obTypDao, zinsDao, prismaDao, vgdao, ProvisionDao, subventionDao, insuranceDao, mwstDao, quoteDao, isoCode, kalkulationDao, prismaServiceDao)
        {
        }


        /// <summary>
        /// Assigns sysprhgroup from product and perole information when 0
        /// </summary>
        /// <param name="prodCtx"></param>
        public static void autoAssignPrhgroup(prKontextDto prodCtx, IObTypDao obTypDao, IPrismaDao prismaDao)
        {
            // Determine used Trading group and store in Output ---------------------------START--------------------------------
            if (prodCtx!=null && prodCtx.sysprhgroup == 0)
            {
                try
                {
                    Dictionary<ConditionLinkType, String> map = new Dictionary<ConditionLinkType, string>{ {ConditionLinkType.PRHGROUPEXT, "sysperole"},
                                                                                                  {ConditionLinkType.OBTYP, "sysobtyp"} };
                    long orgperole = prodCtx.sysperole;
                    prodCtx.sysperole = obTypDao.getHaendlerByEmployee(prodCtx.sysperole);
                    List<long> prhgroups = ConditionLink.getParameter(prodCtx, ConditionLinkType.PRHGROUPEXT, map, obTypDao, prodCtx.perDate);
                    List<long> resultPrhgroups = new List<long>();
                    if (prodCtx.sysprproduct > 0)
                    {
                        List<ProductConditionLink> allConditions = prismaDao.getProductConditionLinks("prclprhg");

                        foreach (long sysprhgroup in prhgroups)
                        {
                            resultPrhgroups.AddRange(allConditions.Where(a => a.sysprhgroup == sysprhgroup && a.SYSPRPRODUCT == prodCtx.sysprproduct).Select(a => a.sysprhgroup).ToList());
                        }
                        resultPrhgroups = resultPrhgroups.Distinct().ToList();
                    }
                    else
                    {
                        resultPrhgroups = prhgroups;
                    }

                    prodCtx.sysprhgroup = resultPrhgroups.FirstOrDefault();
                    prodCtx.sysperole = orgperole; //reset original perole!
                                                   //_log.Info("Setting sysprhgroup to " + prodCtx.sysprhgroup + " for sysobtyp " + prodCtx.sysobtyp + " perole: " + prodCtx.sysperole + " product: " + prodCtx.sysprproduct);
                                                   // Determine used Trading group and store in Output ----------------------------END---------------------------------
                }catch(Exception e)
                {
                    _log.Info("Auto-Assignment of sysprhgroup failed: " + e.Message);
                }
            }
        }

        /// <summary>
        /// calculates the calculation
        /// </summary>
        /// <param name="kalkulation">Kalkulations DTO</param>
        /// <param name="prodCtx">Produktkontext</param>
        /// <param name="kalkCtx">Berechnungs-Kontext</param>
        /// <param name="calcError">Fehler bei Ratenberechnung</param>
        ///        Grundsätzliche Unterscheidung (man kann sich das als Eselsbrücke „überkreuz“ merken)
        ///
        ///-	bgIntern* ist exklusive mitfinanzierter (kapitalisierter) Bestandteile
        ///-	bgExtern* ist inklusive mitfinanzierter (kapitalisierter) Bestandteile
        ///
        ///Mitfinanzierte Bestandteile sind
        ///
        ///-	die RSV Prämie in Summe beim Kredit und TZK
        ///-	die Umsatzsteuer auf den Zinsanteil im TZK
        ///
        ///Unterscheidung bzgl Umsatzsteuer
        ///
        ///-	bg* ist exklusive Umsatzsteuerbestandteile
        ///-	bg*Brutto ist inklusive Umsatzsteuerbestandteile (eben auch die Umsatzsteuer auf den Zinsanteil im TZK)
        ///
        ///
        ///Wir haben also in der Fahrzeugfinanzierung die folgenden Ausprägungen / Feldbelegungen:
        ///
        ///-	bginternBrutto wird aus ahkBrutto übernommen (das ist der Barkaufpreis, den man auf den Tisch legen muss)
        ///-	bgintern wird aus ahk übernommen (das ist der Barkaufpreis ohne die enthaltene Umsatzsteuer)
        ///
        ///-	bgexternBrutto enthält zusätzlich zu bginternBrutto noch die RSV Prämie in Summe sowie die Umsatzsteuer auf den Zinsanteil im TZK
        ///-	bgextern enthält zusätzlich zu bgintern noch die RSV Prämie in Summe
        ///
        ///Achtung: Im Leasing haben wir keine mitfinanzierten Bestandteile, daher sind extern und intern jeweils identisch.
        ///
        ///
        ///In der Kreditfinanzierung haben wir die folgenden Ausprägungen (vom Schema her natürlich analog):
        ///
        ///-	bginternBrutto wird aus der Eingabe des Kreditbetrags übernommen (das ist das Geld, das bar ausbezahlt oder überwiesen wird)
        ///-	bgintern = bginternBrutto
        ///
        ///-	bgexternBrutto enthält zusätzlich zu bginternBrutto noch die RSV Prämie in Summe
        ///-	bgextern = bgexternBrutto
        ///
        /// <returns></returns>
        public override KalkulationDto calculate(KalkulationDto kalkulation, prKontextDto prodCtx, kalkKontext kalkCtx, ref byte calcError)
        {
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            /* Overview:
             * v-solve to rate only
             * v-calculation dependent on product type: CreditNowClassic|CreditNowTeilzahlungskauf|LeaseNow|LeaseNowDifferenzleasing
             * 
             * v- leasing vorschüssig, Kredit nachschüssig
             * v- general rounding handling
             * - general tax handling -> general is on vacation ;)
             * - for subvention: update existing dtos, add new, delete missing subventions from given KalkulationDto
             *              - nur Differenzleasing hat eine Subvention: Zinsprovision
             * - for provision: Grundprovision  als Umsatz/Zins/Stückprovision
             * - insurance calculations
             * v- kalkulation for n zinsen (rap min /max current)
             * v- kalkulation for n products? -> iterate outside
             *  die 1. Leasingrate und die Kaution nicht vom Barkaufpreis abgezogen werden dürfen, wenn VERECHNUNGFLAG mit 0 übergeben wird.

             */

            //Initialization:------------------------------------------------------------
            if (kalkulation.angAntKalkDto == null)
            {
                kalkulation.angAntKalkDto = new AngAntKalkDto();
            }
            kalkulation.angAntKalkDto.calcRsvzins = 0;
            CalculationOutputParameters result = null;
            
            IPrismaProductBo productBo = PrismaBoFactory.getInstance().createPrismaProductBo(PrismaProductBo.CONDITIONS_BANKNOW, isoCode);
            IPrismaParameterBo paramBo = PrismaBoFactory.getInstance().createPrismaParameterBo(PrismaParameterBo.CONDITIONS_BANKNOW);
            IRounding round = RoundingFactory.createRounding();
            IProvisionBo provBo = PrismaBoFactory.getInstance().createProvisionBo();
            IZinsBo zinsBo = BOFactory.getInstance().createZinsBo(ZinsBo.CONDITIONS_BANKNOW, isoCode);

            ISubventionBo subventionBo = new SubventionBo(subventionDao, paramBo);
            IInsuranceBo insuranceBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createInsuranceBO();
            IMwStBo mwstBo = BOFactory.getInstance().createMwstBo();
            IAngAntDao angAntDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDao();
            
            // Damit wird der setter und verifyPerDate aufgerufen 
            prodCtx.perDate = prodCtx.perDate;

            ParamDto aufschubParam = paramBo.getParameter(prodCtx, EnumUtil.GetStringValue(PrismaParameters.Aufschub));
            kalkulation.angAntKalkDto.aufschub = 0;
            if (aufschubParam != null)
            {
                kalkulation.angAntKalkDto.aufschub = (int)aufschubParam.defvaln;
            }
            ParamDto satzMehrMinderKm = paramBo.getParameter(prodCtx,EnumUtil.GetStringValue(PrismaParameters.SatzMehrKm));
            kalkulation.angAntKalkDto.satzmehrkm = 0;
            if (satzMehrMinderKm != null && prodCtx.sysobtyp>0)
            {
                //AngAntObDto obDto = angAntDao.getObjektdaten(prodCtx.sysobtyp);
                kalkulation.angAntKalkDto.satzmehrkm = Math.Round(   (kalkCtx.zubehoerBrutto+kalkCtx.grundBrutto) * satzMehrMinderKm.defvalp / 100 , 2);
                kalkulation.angAntKalkDto.ob_mark_satzmehrkm = satzMehrMinderKm.defvalp;
            }

            PRPRODUCT product = productBo.getProduct(kalkulation.angAntKalkDto.sysprproduct);
            if (product == null)
            {
                throw new Exception("Product not found:" + kalkulation.angAntKalkDto.sysprproduct);
            }
            VART vart = productBo.getVertragsart(product.SYSPRPRODUCT);
            if (vart == null)
            {
                throw new Exception("Product has no VART:" + kalkulation.angAntKalkDto.sysprproduct);
            }

            //Produkte werden anhand vart unterschieden: es gibt KREDIT, TZK, LEASING
            String Code = vart.CODE.ToUpper();
            bool istzk = (Code.IndexOf("TZK") > -1);//Teilzahlungskauf ist analog Kredit zu rechnen
            bool isCredit = (Code.IndexOf("KREDIT") > -1) || istzk;
            bool isCreditClassic = (Code.IndexOf("KREDIT_CLASSIC") > -1);
            bool isLeasing = (Code.IndexOf("LEAS") > -1);
            bool isDispo = (Code.IndexOf("DISPO") > -1) || (Code.IndexOf ("FLEX") > -1);
            bool isDiffLeasing = prismaDao.isDiffLeasing(product.SYSPRPRODUCT);
            bool isExpress = (Code.IndexOf("EXPRESS") > -1);
            double kundenZins = kalkulation.angAntKalkDto.zinscust;

            if (isCreditClassic)
            {
                kalkulation.angAntKalkDto.rwBrutto = 0;//no residual value for this product allowed (Quick-Calc has this field opened because it is for several products)
            }

            // HR 24.08.2011: Anpassung wegen SZ >= Barwert. Die alte Anpassung entsprach nicht den Vorgaben von BankNow
            if (kalkulation.angAntKalkDto.szBrutto >= kalkulation.angAntKalkDto.bginternbrutto)
            {
                kalkulation.angAntKalkDto.szBrutto = 0;
                kalkulation.angAntKalkDto.sz = 0;
            }
            // HR 24.08.2011: Ich habe sysLS hier auf 1 festgelegt für Mandantenfähigkeit müßte das in die Methode irgendwo reinlaufen was momentan nicht gewährleistet ist.
            double ust = mwstBo.getMehrwertSteuer(1, vart.SYSVART, prodCtx.perDate);

            double mwst = ust; //always mwst, independent of credit
            if (isCredit)
            {
                ust = 0;
            }

            if (!istzk || kalkulation.angAntKalkDto.bgintern==0)
            {
                kalkulation.angAntKalkDto.bgintern = round.getNetValue(kalkulation.angAntKalkDto.bginternbrutto, ust);
            }

            double[] zinsen = new double[1];
            double zinsbasis = kalkulation.angAntKalkDto.bginternbrutto;
            double quotepercent = quoteDao.getQuote(SZ_SCHWELLE_QUOTE);
            if (quotepercent > 0)
            {
                if (isCredit)
                {
                    zinsbasis -= kalkulation.angAntKalkDto.szBrutto;
                }
                else if (kalkulation.angAntKalkDto.szBrutto > (quotepercent / 100 * zinsbasis))
                {
                    zinsbasis -= kalkulation.angAntKalkDto.szBrutto;
                }
            }

            KundenzinsDto zinsDto = null;
            if (isDiffLeasing && kalkCtx.setDefaultCustZins )
            {
                zinsDto = prismaDao.getKundenzins(kalkulation.angAntKalkDto.sysprproduct, kalkulation.angAntKalkDto.lz, zinsbasis);
                kundenZins = zinsDto.maxrate;
                kalkulation.angAntKalkDto.zinscust = kundenZins;
            }
            //Interests:-----------------------------------------------------------------------------------------------------
            //contains nominalinterets for calculation, order: base, min, max
            kalkulation.angAntKalkDto.sz = round.getNetValue(kalkulation.angAntKalkDto.szBrutto, ust);


            PRRAPVAL rapVal = null;
            DateTime perDate = DateTime.Now;
            PRRAP prrap = zinsBo.getPrRap(kalkulation.angAntKalkDto.sysprproduct);
            // BNRNEUN-1382 / Im B2B dürfen RAP Methoden nicht verwendet werden weil es keinen Score gibt 
            if ((kalkCtx.kundenScore != null && kalkCtx.kundenScore.Length > 0 && Convert.ToDouble(kalkCtx.kundenScore) != 0))
            {
                
                //immer rap-zinsen mitrechnen ausser differenzleasung und wenn kundenscore von aussen
                if (isDiffLeasing || (kalkCtx.kundenScore != null && kalkCtx.kundenScore.Length > 0 && Convert.ToDouble(kalkCtx.kundenScore) != 0))
                {
                    if (prodCtx.sysprkgroup > 0 && prrap != null)
                    {
                        List<PRRAPVAL> rapvaluesliste =
                            zinsBo.getRapValues(prrap.SYSPRRAP, prodCtx.sysprkgroup);
                        if (rapvaluesliste.Count > 0)
                        {
                            rapVal = zinsBo.getRapValByScore(rapvaluesliste, kalkCtx.kundenScore);
                        }
                        else
                        {
                            rapVal = zinsBo.getRapValByScore(kalkulation.angAntKalkDto.sysprproduct, kalkCtx.kundenScore);
                        }
                    }
                    else
                    {
                        rapVal = zinsBo.getRapValByScore(kalkulation.angAntKalkDto.sysprproduct, kalkCtx.kundenScore);
                    }
                    //BNRSIEBEN-406: gibt den Zinssatz anhand des Score-abhängigen Rap-Wertes (PRRAPVAL.VALUE) mit dem abgezogenen Zinsabschlag aus dem Zinsband der zugeordneten Zinstruktur sysintstrct (PRRAP.SYSINSTRCT) zurück
                    if (prrap != null && prrap.SYSINTSTRCT != null && prrap.SYSINTSTRCT.Value != 0 &&
                        rapVal.VALUE != null && rapVal.VALUE.Value != 0)
                    {
                        rapVal.VALUE = zinsBo.getZinsRap(kalkulation.angAntKalkDto.sysprproduct, perDate,
                            kalkulation.angAntKalkDto.lz, kalkulation.angAntKalkDto.bginternbrutto,
                            prrap.SYSINTSTRCT.Value, rapVal.VALUE.Value);
                    }
                    zinsen[0] = (double)rapVal.VALUE.Value;
                    double BasisEffectiv = zinsBo.getZins(prodCtx, kalkulation.angAntKalkDto.lz, zinsbasis);

                    kalkulation.angAntKalkDto.zinsrap = zinsen[0];
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

                        if (kalkulation.angAntKalkDto.calcPrkgroup != null || kalkulation.angAntKalkDto.calcPrkgroup > 0)
                        {
                            List< PRRAPVAL> rapvaluesliste =
                                zinsBo.getRapValues(prrap.SYSPRRAP,
                                    (long)kalkulation.angAntKalkDto.calcPrkgroup);


                            if (rapvaluesliste != null)
                            {
                                zinsen[0] = (double)rapvaluesliste.FirstOrDefault().VALUE.Value;
                            }
                            else
                            {
                                zinsen[0] = zinsBo.getZins(prodCtx, kalkulation.angAntKalkDto.lz, zinsbasis);
                            }
                        }
                    }
                    else
                    {
                        zinsen[0] = zinsBo.getZins(prodCtx, kalkulation.angAntKalkDto.lz, zinsbasis);
                    }

                    if (prrap != null)
                    {
                        zinsen[1] = prrap.MINVALUE.HasValue ? (double)prrap.MINVALUE.Value : 0;
                        zinsen[2] = prrap.MAXVALUE.HasValue ? (double)prrap.MAXVALUE.Value : 0;

                    }
                    kalkulation.angAntKalkDto.zinsrap = zinsen[0];
                }
            }
            else
            {
                if (prrap != null)
                {
                    zinsen = new double[3];
                    zinsen[0] = (double)zinsBo.getRapZinsByScore(kalkulation.angAntKalkDto.sysprproduct, "0");
                }
                else
                {
                    zinsen[0] = zinsBo.getZins(prodCtx, kalkulation.angAntKalkDto.lz, zinsbasis);
                }

                if (prrap != null)//needed for min max slider 
                {
                    zinsen[1] = prrap.MINVALUE.HasValue ? (double)prrap.MINVALUE.Value : 0;
                    zinsen[2] = prrap.MAXVALUE.HasValue ? (double)prrap.MAXVALUE.Value : 0;
                }
                kalkulation.angAntKalkDto.zinsrap = zinsen[0];
            }
            //this overwrites zins
            if (kalkCtx.useZinsNominal)
            {
                zinsen[0] = kalkCtx.zinsNominal;
            }

            if (isCredit)
            {
                //for credit, the interest has to be converted to nominal from effective
                for (int i = 0; i < zinsen.Length; i++)
                {
                    zinsen[i] = CalculateNominalInterest(zinsen[i], 12);
                }
            }
            //Interests:----------------------------------------------------END-------------------------------------------------

            // Determine used Trading group and store in Output ---------------------------START--------------------------------
            autoAssignPrhgroup(prodCtx, obTypDao, prismaDao);

            //Provision initialization------------------------------------------------------------------------------------------
            provKontextDto provKontext = new provKontextDto();
            provKontext.sysprproduct = prodCtx.sysprproduct;
            provKontext.perDate = prodCtx.perDate;
            provKontext.sysbrand = prodCtx.sysbrand;
            provKontext.sysobtyp = prodCtx.sysobtyp;
            provKontext.sysperole = prodCtx.sysperole;
            provKontext.sysprhgroup = prodCtx.sysprhgroup;
            provKontext.sysvttyp = product.SYSVTTYP.HasValue ? product.SYSVTTYP.Value : 0;
            provKontext.sysvart = product.SYSVART.HasValue ? product.SYSVART.Value : 0;
            provKontext.sysvarttab = product.SYSVARTTAB.HasValue ? product.SYSVARTTAB.Value : 0;
            provKontext.sysperole = prodCtx.sysperole;
            provKontext.sysprhgroup = prodCtx.sysprhgroup;
            provKontext.sysvstyp = 0;//??
            provKontext.sysabltyp = 0;//??
            //Provision initialization---------------------------------END------------------------------------------------------

            _log.Debug("Duration A: "+(DateTime.Now.TimeOfDay.TotalMilliseconds-start));
            start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            //Input value assignments-------------------------------------------------------------------------------------------
            //Entered values are brutto, convert to netto
            kalkulation.angAntKalkDto.rw = round.getNetValue(kalkulation.angAntKalkDto.rwBrutto, ust);

            CalculationInputParameters input = new CalculationInputParameters();
            input.barwert = kalkulation.angAntKalkDto.bgintern;
            input.ersteRate = kalkulation.angAntKalkDto.sz;
            input.zins = zinsen[0];
            if (isDiffLeasing)
            {
                input.zins = kundenZins;
            }

            input.ust = ust;
            input.laufzeit = kalkulation.angAntKalkDto.lz;
            input.restwert = kalkulation.angAntKalkDto.rw;
            input.isCredit = isCredit;
            input.istzk = istzk;
            input.mwst = mwst;
            //Input value assignments---------------------------------END-------------------------------------------------------


            //Calculation (with default/score-interest)------------------------------------------------------------------------

            long externType = ProvisionDao.getExternalABlID();
            double extAbloese = 0;//Fremdablöse
            double gesAbloese = 0;//Gesamtablöse
            double intAbloese = 0;//Eigenablöse
            if (kalkulation.angAntAblDto != null && kalkulation.angAntAblDto.Count > 0)
            {
                foreach (AngAntAblDto abl in kalkulation.angAntAblDto)
                {
                    // Ticket#2012013110000149:
                    // jeder Eintrag (egal ob intern oder extern) bei dem flagEpos gesetzt ist, muss als indikativ betrachtet und nicht summiert werden
                    if (abl.flagEpos == false)
                    {
                        gesAbloese += abl.betrag;
                        if (abl.sysabltyp == externType)
                        {
                            extAbloese += abl.betrag;
                        }
                        else
                        {
                            //PPI Eigenablöse:
                            if (abl.sysvorvt > 0)
                            {
                                EigenAblInfo eigenAbl = provBo.getEigenabloeseInfo(abl.sysvorvt);
                                if(eigenAbl!=null)
                                {
                                    intAbloese += eigenAbl.getPPI() * abl.betrag;
                                }
                                else
                                {
                                    _log.Warn("Eigenablöse Vertragsnummer nicht gefunden: " + _log.dumpObject(abl));
                                }
                            }
                            else
                            {
                                _log.Warn("Eigenablöse ohne Vertragsnummer: " + _log.dumpObject(abl));
                            }
                        }
                    }
                }
            }

            /*------------------------------------------------------------------------------------------------------------------
             * VERSICHERUNGEN
             * benötigt als Input Werte aus Ratenkalkulation
             * ändert ggf. Input-Werte (barwert) für finale Ratenkalkulation
             */
            kalkulation.angAntKalkDto.calcRsvmonat = 0;
            kalkulation.angAntKalkDto.calcRsvgesamt = 0;
            List<AngAntVsDto> insurances = cloneInsurances(kalkulation.angAntVsDto);
            result = calcInsurance(insurances, kalkulation, prodCtx.perDate, insuranceBo, input, CalcType.DEFAULT, isLeasing);
            double rsvMitFinBrutto = result.rsvMitFinBrutto;
            double zkOhneSicherung = result.zinskostenOhneAbsicherung;
            //Fix Rundungsdifferenzen Zinskosten RSV:
            if (input.zins == 0)
            {
                zkOhneSicherung = 0;
            }
            // double rateOhneRatenabsicherungGerundet = BankNowCalculator.RoundCHF(result.rateBrutto);
            double bgexternBruttoOhneRatenabsicherung = RoundCHF(round.getGrossValue(result.barwertOhneAbsicherung, ust));

            kalkulation.angAntVsDto = insurances;
            //---------------------------------------------------Insurance END---------------------------------------------------
            //input.barwert ist hier nun mit kapitalisierten bestandteilen aus versicherungen und bei tzk mit mwstteilzahlungszuschlag

            //---------------------------------------------------ANFANG BG* SOWIE TZK SPEZIELLE UST BEHANDLUNG---------------------------------
            if (istzk)
            {
                kalkulation.angAntKalkDto.calcUstzins = result != null ? RoundCHF(result.mwstTeilzahlungszuschlag) : 0;
                kalkulation.angAntKalkDto.bgextern = RoundCHF(input.barwert - kalkulation.angAntKalkDto.calcUstzins);
                kalkulation.angAntKalkDto.bgexternbrutto = RoundCHF(round.getGrossValue(input.barwert, ust));
                kalkulation.angAntKalkDto.bginternbrutto = RoundCHF(kalkulation.angAntKalkDto.bgintern);
            }
            else
            {
                kalkulation.angAntKalkDto.bgextern = RoundCHF(input.barwert);
                kalkulation.angAntKalkDto.bgexternbrutto = RoundCHF(round.getGrossValue(input.barwert, ust));
            }
            //---------------------------------------------------ENDE BG* SOWIE TZK SPEZIELLE UST BEHANDLUNG-----------------------------------------
            _log.Debug("Duration B: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            input.istzk = false;
            input.isDiffLeasing = isDiffLeasing;
            result = calcRate(input);
            if (result.rateError)
            {
                calcError += 1;
            }
            // HR 22.8.2011: Anpassung der Umsatzbasis der Provisionen in Absprache mit Stas
            // HR 31.8.2011: Anpassung für TZK in Absprache mit Stas
            // MB 6.12: Stas: bei TZK ist der Provisionsbasis auch so - BGINTERNBRUTTO-SZBRUTTO
            result.provisionsBasis = kalkulation.angAntKalkDto.bginternbrutto - kalkulation.angAntKalkDto.szBrutto;
            input.istzk = istzk;

            kalkulation.angAntKalkDto.rateBrutto = result.rateBrutto; 
            kalkulation.angAntKalkDto.rateBruttoInklAbsicherung = result.rateBrutto+ kalkulation.angAntKalkDto.calcRsvmonat - rsvMitFinBrutto;//add both unrounded
            kalkulation.angAntKalkDto.ersteRateBruttoInklAbsicherung = kalkulation.angAntKalkDto.szBrutto + kalkulation.angAntKalkDto.calcRsvmonat - rsvMitFinBrutto;//add both unrounded
            
            kalkulation.angAntKalkDto.rate = round.getNetValue(kalkulation.angAntKalkDto.rateBrutto, ust);
            double firstRateBruttoUnrounded = kalkulation.angAntKalkDto.rateBrutto;
            double firstRateNettoUnrounded = result.rate;

            kalkulation.angAntKalkDto.zins = zinsen[0];
            kalkulation.angAntKalkDto.zinseff = result.zinseff;
            kalkulation.angAntKalkDto.calcRsvgesamt = RoundCHF(kalkulation.angAntKalkDto.calcRsvgesamt);
            // double zinskosten = RoundCHF(calcZinsKosten(input.ersteRate > 0, input.laufzeit, rateOhneRatenabsicherungGerundet, kalkulation.angAntKalkDto.szBrutto, kalkulation.angAntKalkDto.rwBrutto, bgexternBruttoOhneRatenabsicherung, isLeasing));
            // if (input.zins == 0) zinskosten = 0;

            // Ticket#2011112110000071 Indikative Zinskosten für Express
            if (isExpress)
            {
                // K1 = (Anfangskapital zu Beginn der Geldanlage) * (Zinssatz/100) * ((die Anlagedauer in Tagen)/365 )
                // Ticket#2012072610000022 — Defect #6734 : LZ + 1
                kalkulation.angAntKalkDto.calcZinskosten = kalkulation.angAntKalkDto.bgexternbrutto * (kalkulation.angAntKalkDto.zins / 100) * ((input.laufzeit + 1) * 30) / 365;
            }
            else
            {
                kalkulation.angAntKalkDto.calcZinskosten = RoundCHF(calcZinsKosten(input.ersteRate > 0, input.laufzeit, BankNowCalculator.RoundCHF(result.rateBrutto), kalkulation.angAntKalkDto.szBrutto, kalkulation.angAntKalkDto.rwBrutto, kalkulation.angAntKalkDto.bgexternbrutto, isLeasing));
            }
            if (input.zins == 0)
            {
                kalkulation.angAntKalkDto.calcZinskosten = 0;
            }

            double gesAblFaktor = 0;
            double extAblFaktor = 0;
            double provAblFaktor = 1;//default gesamte rsv prämie
            if (kalkulation.angAntKalkDto.bginternbrutto > 0)
            {
                gesAblFaktor = (gesAbloese / kalkulation.angAntKalkDto.bginternbrutto);
                extAblFaktor = (extAbloese / kalkulation.angAntKalkDto.bginternbrutto);
                //wenn vorvertrag credit classic und rsv hatte, dann rsvprovision anders berechnen: (CR 28601)
                //(Totalbetrag - Eigenablöse1*PPI_Faktor1- Eigenablöse2*PPI_Faktor2... -EigenablöseN*PPI_FaktorN) / Totalbetrag (Eigenablöse + Neugeld + Fremd-ablöse) = Provisionsratio Ratenabsicherungsprovision
                //Summe der Eigenablösen mit PPI-Faktor siehe weiter oben bei intAbloese-Berechnung
                provAblFaktor = ((kalkulation.angAntKalkDto.bginternbrutto-intAbloese) /(kalkulation.angAntKalkDto.bginternbrutto));
            }

            kalkulation.angAntKalkDto.calcRsvzins = RoundCHF(kalkulation.angAntKalkDto.calcZinskosten - zkOhneSicherung);


           
            kalkulation.angAntKalkDto.rate = RoundCHF(kalkulation.angAntKalkDto.rate);
            kalkulation.angAntKalkDto.rateBrutto = RoundCHF(kalkulation.angAntKalkDto.rateBrutto);
            kalkulation.angAntKalkDto.rateBruttoInklAbsicherung = RoundCHF(kalkulation.angAntKalkDto.rateBruttoInklAbsicherung);
            kalkulation.angAntKalkDto.ersteRateBruttoInklAbsicherung = RoundCHF(kalkulation.angAntKalkDto.ersteRateBruttoInklAbsicherung);
            //kalkulation.angAntKalkDto.calcRsvmonat = RoundCHF(kalkulation.angAntKalkDto.calcRsvmonat);

            kalkulation.angAntKalkDto.szUst = kalkulation.angAntKalkDto.szBrutto - kalkulation.angAntKalkDto.sz;
            kalkulation.angAntKalkDto.rateUst = kalkulation.angAntKalkDto.rateBrutto - kalkulation.angAntKalkDto.rate;
            kalkulation.angAntKalkDto.rwUst = kalkulation.angAntKalkDto.rwBrutto - kalkulation.angAntKalkDto.rw;

            if ( (prodCtx.sysvart == 1) && (kalkulation.angAntKalkDto.szBrutto > 0) && (kalkulation.angAntKalkDto.szBrutto < kalkulation.angAntKalkDto.rateBrutto))
            {
                calcError += 16;
            }
            if ((kalkulation.angAntKalkDto.sz + kalkulation.angAntKalkDto.rw) > kalkulation.angAntKalkDto.bgintern)
            {
                calcError += 32;
            }
            //Differenzleasing - bankzins berechnen
            double subventionRatendifferenz = 0;
            if (isDiffLeasing)
            {
                input.zins = zinsen[0];
                if (kalkulation.angAntKalkDto.sz == 0)
                {
                    input.ersteRate = result.rate;
                }
                input.barwert = kalkulation.angAntKalkDto.bgintern;
                List<AngAntVsDto> insurancesBZ = cloneInsurances(kalkulation.angAntVsDto);
                result = calcInsurance(insurancesBZ, kalkulation, prodCtx.perDate, insuranceBo, input, CalcType.DIFFLEASING, isLeasing);
                //----------------
                input.istzk = false;
                result = calcRate(input);
                if (result.rateError)
                {
                    calcError += 2;
                }
                // HR 22.8.2011: Anpassung der Umsatzbasis der Provisionen in Absprache mit Stas
                result.provisionsBasis = kalkulation.angAntKalkDto.bginternbrutto - kalkulation.angAntKalkDto.szBrutto;
                input.istzk = istzk;               
                subventionRatendifferenz = Math.Abs(RoundCHF((input.laufzeit - 1) * (result.rateBrutto - firstRateBruttoUnrounded)));
            }

            _log.Debug("Duration C: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            //PROVISIONS--------------------------------------------------------------------------------------------------------------------------
            List<AngAntProvDto> overwrites = new List<AngAntProvDto>();
            if(kalkulation.angAntProvDto!=null)
            {
                foreach (AngAntProvDto pr in kalkulation.angAntProvDto)//filter the correct provtypes
                {
                    if (pr.flaglocked > 0)
                    {
                        overwrites.Add(pr);
                    }
                }
            }
            kalkulation.angAntProvDto = new List<AngAntProvDto>();
            double provisionsBasisZins = zkOhneSicherung;
            // Änderung 31.08.2011 nach Problem in Abstprache mit Stas
            kalkulation.angAntProvDto = calculateProvisions(provKontext, result.provisionsBasis, provisionsBasisZins, gesAblFaktor, extAblFaktor, input.laufzeit,
                                                            kalkulation.angAntKalkDto.auszahlung, provBo, prodCtx, insurances, kalkulation.angAntAblDto, overwrites, istzk, isDispo, provAblFaktor, rapVal, kalkulation.angAntKalkDto.bginternbrutto);
            //PROVISIONS--------------------------------------------END---------------------------------------------------------------------------

            //Subventionen------------------------------------------------------------------------------------------------------
            if (subventionRatendifferenz > 0)
            {
                iSubventionDto iSubvention = new iSubventionDto();
                iSubvention.calcMode = SubventionCalcMode.IMPLICIT;
                iSubvention.subventionValue = subventionRatendifferenz; //ratendiff
                iSubvention.sourceArea = ExplicitSubventionArea.INTEREST;
                iSubvention.areaId = 22;
                iSubvention.laufzeit = (long)input.laufzeit;
                iSubvention.mwst = input.ust;
                iSubvention.defaultValue = 0;
                iSubvention.roundMode = 1;//CHF Rounding
                iSubvention.perDate = prodCtx.perDate;
                iSubvention.subventionSource = SubventionSourceField.DIFFERENZLEASINGZINS;

                long haendler = obTypDao.getHaendlerByEmployee(prodCtx.sysperole);

                long person = obTypDao.getPersonIDByPEROLE(haendler);

                if (person != 0)
                {
                    iSubvention.sysperson = person;
                }

                kalkulation.angAntSubvDto = subventionBo.calcSubvention(iSubvention, prodCtx);
            }
            //Subventionen------------------------------------------END---------------------------------------------------------

            _log.Debug("Duration D: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            //calculate values for rap min and rap max--------------------------------------------------------------------------
            if (zinsen.Length > 1)
            {
                //RAP MIN
                input.istzk = istzk;
                input.zins = zinsen[1];
                input.barwert = kalkulation.angAntKalkDto.bgintern;
                kalkulation.angAntKalkDto.calcRsvmonatMin = 0;
                kalkulation.angAntKalkDto.calcRsvzinsTmp = 0;
                insurances = cloneInsurances(kalkulation.angAntVsDto);//clone the insurances to keep the result for this calculation
                result = calcInsurance(insurances, kalkulation, prodCtx.perDate, insuranceBo, input, CalcType.MIN, isLeasing);
                provisionsBasisZins = result.zinskostenOhneAbsicherung;
                double mwstTeilzahlung = result != null ? RoundCHF(result.mwstTeilzahlungszuschlag) : 0;
                input.istzk = false;
                result = calcRate(input);
                if (result.rateError)
                {
                    calcError += 4;
                }
                input.istzk = istzk;

                kalkulation.angAntKalkDto.rapratebruttoMin = result.rateBrutto;// +kalkulation.angAntKalkDto.calcRsvmonatMin;//add both unrounded
                kalkulation.angAntKalkDto.rapzinseffMin = result.zinseff;
                double rsvgesamt = RoundCHF(kalkulation.angAntKalkDto.calcRsvmonatMin * input.laufzeit);
                double bwforcalculation = RoundCHF(round.getGrossValue(input.barwert, ust));
                double zinskostenInklAbsicherung = calcZinsKosten(input.ersteRate > 0, input.laufzeit, BankNowCalculator.RoundCHF(result.rateBrutto), kalkulation.angAntKalkDto.szBrutto, kalkulation.angAntKalkDto.rwBrutto, bwforcalculation, isLeasing);
                if (input.zins == 0)
                {
                    zinskostenInklAbsicherung = 0;
                }
                kalkulation.angAntKalkDto.calcZinskostenMin = RoundCHF(zinskostenInklAbsicherung);
                //provisionsBasisZins -= kalkulation.angAntKalkDto.calcRsvzinsTmp;
                kalkulation.angAntKalkDto.rapratebruttoMin = RoundCHF(kalkulation.angAntKalkDto.rapratebruttoMin);
                kalkulation.angAntKalkDto.calcRsvmonatMin = RoundCHF(kalkulation.angAntKalkDto.calcRsvmonatMin);
                kalkulation.angAntProvDtoRapMin = calculateProvisions(provKontext, round.getGrossValue(result.provisionsBasis, ust), provisionsBasisZins, 0, 0, input.laufzeit,
                                                                      kalkulation.angAntKalkDto.auszahlung, provBo, prodCtx, insurances, kalkulation.angAntAblDto, null, istzk, isDispo, 1, rapVal, kalkulation.angAntKalkDto.bginternbrutto);
                _log.Debug("Duration E: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                //RAP MAX
                input.istzk = istzk;
                input.zins = zinsen[2];
                input.barwert = kalkulation.angAntKalkDto.bgintern;
                kalkulation.angAntKalkDto.calcRsvmonatMax = 0;
                kalkulation.angAntKalkDto.calcRsvzinsTmp = 0;
                insurances = cloneInsurances(kalkulation.angAntVsDto);//clone the insurances to keep the result for this calculation
                result = calcInsurance(insurances, kalkulation, prodCtx.perDate, insuranceBo, input, CalcType.MAX, isLeasing);
                provisionsBasisZins = result.zinskostenOhneAbsicherung;
                mwstTeilzahlung = result != null ? RoundCHF(result.mwstTeilzahlungszuschlag) : 0;
                input.istzk = false;
                result = calcRate(input);
                if (result.rateError)
                {
                    calcError += 8;
                }
                input.istzk = istzk;

                kalkulation.angAntKalkDto.rapratebruttoMax = result.rateBrutto;// +kalkulation.angAntKalkDto.calcRsvmonatMax;//add both unrounded
                kalkulation.angAntKalkDto.rapzinseffMax = result.zinseff;
                rsvgesamt = RoundCHF(kalkulation.angAntKalkDto.calcRsvmonatMax * input.laufzeit);
                bwforcalculation = RoundCHF(round.getGrossValue(input.barwert, ust));
                zinskostenInklAbsicherung = calcZinsKosten(input.ersteRate > 0, input.laufzeit, BankNowCalculator.RoundCHF(result.rateBrutto), kalkulation.angAntKalkDto.szBrutto, kalkulation.angAntKalkDto.rwBrutto, bwforcalculation, isLeasing);
                if (input.zins == 0)
                {
                    zinskostenInklAbsicherung = 0;
                }
                kalkulation.angAntKalkDto.calcZinskostenMax = RoundCHF(zinskostenInklAbsicherung);
                //provisionsBasisZins -= kalkulation.angAntKalkDto.calcRsvzinsTmp;
                kalkulation.angAntKalkDto.rapratebruttoMax = RoundCHF(kalkulation.angAntKalkDto.rapratebruttoMax);
                kalkulation.angAntKalkDto.calcRsvmonatMax = RoundCHF(kalkulation.angAntKalkDto.calcRsvmonatMax);
                kalkulation.angAntProvDtoRapMax = calculateProvisions(provKontext, round.getGrossValue(result.provisionsBasis, ust), provisionsBasisZins, 0, 0, input.laufzeit,
                                                                      kalkulation.angAntKalkDto.auszahlung, provBo, prodCtx, insurances, kalkulation.angAntAblDto, null, istzk, isDispo, 1, rapVal, kalkulation.angAntKalkDto.bginternbrutto);

            }
            //calculate values for rap min and rap max------------------END-----------------------------------------------------
            _log.Debug("Duration F: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            kalkulation.angAntKalkDto.sysprhgrp = prodCtx.sysprhgroup;


            ///Restwertabsicherungsmodell
            double rwahk = kalkCtx.grundBrutto + kalkCtx.zubehoerBrutto;
            if(rwahk>0)
                kalkulation.angAntKalkDto.rwKalkBruttoP = Math.Round(kalkulation.angAntKalkDto.rwBrutto / (rwahk) * 100, 2);
            else
                kalkulation.angAntKalkDto.rwKalkBruttoP = 0;
            kalkulation.angAntKalkDto.rwBase = 0;
            if (prodCtx.sysobtyp > 0)
            {
                srvKontextDto srvKontext = new srvKontextDto();
                srvKontext.syskdtyp = prodCtx.syskdtyp;
                srvKontext.sysobart = prodCtx.sysobart;
                srvKontext.sysobtyp = prodCtx.sysobtyp;
                srvKontext.sysprkgroup = prodCtx.sysprkgroup;
                srvKontext.sysprprodukt = kalkulation.angAntKalkDto.sysprproduct;
                IPrismaServiceBo svcBo = new PrismaServiceBo(prismaServiceDao, obTypDao, OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());
                List<AvailableServiceDto> services = svcBo.listAvailableServices(srvKontext, null);

                //Hier wird automatisch eine Restwertabsicherung in die Kalkulation gehängt, falls sie mandatory und noch nicht vorhanden ist:
                if (services != null && services.Count > 0)
                {
                    if (kalkulation.angAntVsDto != null)
                    {
                        foreach (AngAntVsDto fvs in kalkulation.angAntVsDto)
                        {
                            if (fvs.serviceType.Equals(ServiceType.VSTYP))
                            {
                                fvs.serviceType = (from f in services
                                                   where f.sysID == fvs.sysvstyp
                                                   select f.serviceType).FirstOrDefault();
                            }
                        }

                    }

                    bool rwafound = false;
                    AvailableServiceDto rwa = (from f in services
                                               where f.serviceType == ServiceType.RWA
                                               select f).FirstOrDefault();
                    if (rwa != null)//RWA-Versicherung für Produkt vorhanden
                    {
                        
                        // LinQ Error 20190510 kalkulation.angAntVsDt == null
                        AngAntVsDto rwavs = null;
						if (kalkulation.angAntVsDto != null)
						{
							rwavs = (from f in kalkulation.angAntVsDto
										where f.serviceType.Equals(ServiceType.RWA)
										select f).FirstOrDefault();
                            if(rwavs!=null)
                                rwafound = true;
						}
						else
						{
							_log.Debug ("RWA: Insurance found(" + rwa.sysID + "), but angAntVsDto-List IS EMPTY! --> RESETTING List to new AngAntVsDto()");
						}
						if (rwavs == null) rwavs = new AngAntVsDto();
                        rwavs.serviceType = rwa.serviceType;
                        rwavs.sysvstyp = rwa.sysID;
                        bool isOptionalRWA = !(!rwa.editable && rwa.selected);
                        //Die Produktkalkulation prüft, ob zu einer RWA-Objektversicherung auf dem zugeordneten Objekttyp eine gültige Wertegruppe gemäss RWA-Zuordnung vorliegt
                        RestWertSettingsDto rwSettings = new OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxDBDao().getSysVGForVGREFType(DTO.Auskunft.VGRefType.RWA, prodCtx.sysobtyp, prodCtx.sysbrand, prodCtx.sysprhgroup, prodCtx.perDate);
                        if (rwSettings.External)
                        {
                            _log.Debug("RWA: Insurance found(" + rwa.sysID + "), but no vgref defined for brand:" + prodCtx.sysbrand + " sysobtyp:" + prodCtx.sysobtyp + " sysprhgroup:" + prodCtx.sysprhgroup+" vstyp optional: "+ isOptionalRWA);
                            //Wenn keine Zuordnung vorliegt
                            //Wenn die RWA - Objektversicherung optional ist, wird sie ignoriert. Die Kalkulation verhält sich so als ob keine RWA - Objektversicherung vorhanden ist
                            if (isOptionalRWA)//Wenn die RWA-Objektversicherung optional ist, wird sie ignoriert. Die Kalkulation verhält sich so als ob keine RWA-Objektversicherung vorhanden ist
                            {
                                kalkulation.angAntVsDto.Remove(rwavs);
                            }
                            else//Wenn die RWA-Objektversicherung obligatorisch ist, wird die RWA-Objektversicherung in die Kalkulation übernommen, allerdings werden alle relevanten Werte mit 0 befüllt
                            {
                                kalkulation.angAntKalkDto.rwBase = 0;
                                rwavs.deckungssumme = 0;

                                if (!kalkulation.angAntVsDto.Contains(rwavs))
                                    kalkulation.angAntVsDto.Add(rwavs);
                            }
                        }
                        else//Wenn eine RWA-Zuordnung vorliegt, wird die RWA-Objektversicherung in die Kalkulation übernommen
                        {
                            //Falls die RWA-Absicherung greift (der kalkulatorische RW liegt innerhalb RWA-Toleranz) wird die RWA-Indikation im Feld ANTVS:DECKUNGSSUMME als Nettobetrag abgelegt und auf den Vertrag vererbt (VTVS:DECKUNGSSUMME). 

                            String Laufzeit = ""+(getAgeInMonths(kalkCtx.erstzulassung, prodCtx.perDate)+ input.laufzeit);
                            String Laufleistung = ((int)(kalkCtx.ubnahmeKm + (kalkulation.angAntKalkDto.ll * (input.laufzeit / 12.0)))).ToString();
                            double rwProzent = vgdao.getVGValue(rwSettings.sysvgrw, Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null), Laufzeit, Laufleistung, VGInterpolationMode.LINEAR, plSQLVersion.V2);

                            _log.Debug("RWA: Insurance found(" + rwa.sysID + "), using sysvg="+ rwSettings.sysvgrw + " for brand:" + prodCtx.sysbrand + " sysobtyp:" + prodCtx.sysobtyp + " sysprhgroup:" + prodCtx.sysprhgroup + " vstyp optional:" + isOptionalRWA+"  Laufzeit:"+ Laufzeit + " Laufleistung:"+ Laufleistung+" Result rw %:"+rwProzent);

                            kalkulation.angAntKalkDto.rwBase = 0;
                            rwavs.deckungssumme = 0;
                            VSTYP vstyp = prismaServiceDao.getVSTYP().Where(a => a.SYSVSTYP == rwa.sysID).FirstOrDefault();

                            kalkulation.angAntKalkDto.rwBaseBrutto = Math.Round((kalkCtx.grundBrutto + kalkCtx.zubehoerBrutto) * (rwProzent / 100), 2);
                            //convert to Netto
                            kalkulation.angAntKalkDto.rwBase = round.getNetValue(kalkulation.angAntKalkDto.rwBaseBrutto, ust);
                            kalkulation.angAntKalkDto.rwBaseBruttoP = rwProzent;
                            
                            if (VSCalcFactory.Cnst_CALC_RWA_B.Equals(vstyp.CODEMETHOD))
                            {

                                double rwaquote = quoteDao.getQuote(vstyp.SYSQUOTE.GetValueOrDefault());
                                if( Math.Abs(kalkulation.angAntKalkDto.rwBrutto-kalkulation.angAntKalkDto.rwBaseBrutto) <=rwaquote)
                                {
                                    rwavs.deckungssumme = kalkulation.angAntKalkDto.rwBase;
                                }
                            }
                            else if (VSCalcFactory.Cnst_CALC_RWA_P.Equals(vstyp.CODEMETHOD))
                            {
                                double rwaquote = quoteDao.getQuote(vstyp.SYSQUOTE.GetValueOrDefault());
                                if (Math.Abs(kalkulation.angAntKalkDto.rwBrutto - kalkulation.angAntKalkDto.rwBaseBrutto) <= kalkulation.angAntKalkDto.rwBaseBrutto * (rwaquote / 100))
                                {
                                    rwavs.deckungssumme = kalkulation.angAntKalkDto.rwBase;
                                }
                            }

                            if (kalkulation.angAntVsDto == null)
                                kalkulation.angAntVsDto = new List<AngAntVsDto>();

                            if (!rwafound)//when we found one in input, dont add again
								kalkulation.angAntVsDto.Add (rwavs);
                        }
                    }
                    else
                    {
                        if (kalkulation.angAntVsDto != null)//if already given, remove it
                        {
                            AngAntVsDto rwavs = (from f in kalkulation.angAntVsDto
                                     where f.serviceType == ServiceType.RWA
                                     select f).FirstOrDefault();
                            if(rwavs!=null)
                                kalkulation.angAntVsDto.Remove(rwavs);
                        }
                    }
                }
            }
            //End RWA

            // Determine amount of payout--------------------------------------------------START--------------------------------
            
            VartDto vartDto = Mapper.Map<VART, VartDto>(vart);
            AngAntKalkDto KalkDat = kalkulation.angAntKalkDto;

            // Ticket#2012013110000149:
            // jeder Eintrag (egal ob intern oder extern) bei dem flagEpos gesetzt ist, muss als indikativ betrachtet und nicht summiert werden
            double ablSumme = 0;
            if (kalkulation.angAntAblDto != null)
            {
                foreach (AngAntAblDto Abl in kalkulation.angAntAblDto)
                {
                    if (Abl.flagEpos == false)
                    {
                        ablSumme += Abl.betrag;
                    }
                }
            }

            //Subventionssummen berechnen
            double subvSumme = 0;
            if(kalkulation.angAntSubvDto!=null)
            {
                foreach (AngAntSubvDto Subv in kalkulation.angAntSubvDto)
                {
                    if (Subv.syssubvg != 1)
                    {
                        subvSumme += Subv.betragBrutto;
                    }
                }
            }

            //Auszahlungsbetrag abhängig vom Vertriebskanal und der Produktart berechnen
            switch (prodCtx.sysprchannel)
            {
                // Fahrzeugfinanzierung
                case 1:
                    switch (vartDto.code)
                    {
                        // Leasing
                        case "LEASING":
                            if (isDiffLeasing)
                            {
                                if (kalkulation.angAntKalkDto.verrechnungFlag)
                                {
                                    if (KalkDat.szBrutto > 0)
                                    {
                                        KalkDat.auszahlung = KalkDat.bginternbrutto - KalkDat.szBrutto - KalkDat.depot - subvSumme;
                                    }
                                    else
                                    {
                                        KalkDat.auszahlung = KalkDat.bginternbrutto - KalkDat.rateBrutto - KalkDat.depot - subvSumme;
                                    }
                                }
                                else
                                    //TicketID=5810 Leasingrate und die Kaution nicht vom Barkaufpreis abgezogen werden dürfen, wenn VERECHNUNGFLAG mit 0 übergeben wird.
                                {
                                    KalkDat.auszahlung = KalkDat.bginternbrutto - subvSumme;
                                }
                            }
                            else
                            {
                                if (kalkulation.angAntKalkDto.verrechnungFlag)
                                {
                                    if (KalkDat.szBrutto > 0)
                                    {
                                        KalkDat.auszahlung = KalkDat.bginternbrutto - KalkDat.szBrutto - KalkDat.depot;
                                    }
                                    else
                                    {
                                        KalkDat.auszahlung = KalkDat.bginternbrutto - KalkDat.rateBrutto - KalkDat.depot;
                                    }
                                }
                                else
                                    //TicketID=5810 Leasingrate und die Kaution nicht vom Barkaufpreis abgezogen werden dürfen, wenn VERECHNUNGFLAG mit 0 übergeben wird.
                                {
                                    KalkDat.auszahlung = KalkDat.bginternbrutto;
                                }
                            }
                            break;
                        // Klassic Kredit
                        // für alle 3 arten die gleiche Behandlung
                        case "KREDIT_CLASSIC":
                        case "TZK"://gibt es nicht mehr, laut AS 
                        case "TZK_PLUS":
                            // 24.07.2017 Anforderung Stas: Änderung BNRVZ-1532 / Neu darf der Verrechnungsflag bei den FF-Kreditverträgen (Classic und TZK) bei der Berechnung ANTKALK:AUSZAHLUNG nicht berücksichtigt werden.
                            if (KalkDat.szBrutto > 0)
                                // 13.09.2011 Anforderung Stas: Änderung von bginternbrutto in bgintern.
                                // 16.11.2011 Anforderung Stas: Änderung bgintern in bginternbrutto
                            {
                                KalkDat.auszahlung = KalkDat.bginternbrutto + kalkulation.angAntKalkDto.calcUstzins - KalkDat.szBrutto;
                            }
                            else
                            {
                                KalkDat.auszahlung = KalkDat.bginternbrutto + kalkulation.angAntKalkDto.calcUstzins;
                            }
                            break;
                    }
                    break;
                // Kreditfinanzierung
                case 2:
                    switch (vartDto.code)
                    {    // für alle 3 arten die gleiche Behandlung
                        case "KREDIT_CLASSIC":                            
                        case "KREDIT_EXPRESS":
                            KalkDat.auszahlung = KalkDat.bginternbrutto - ablSumme;
                            break;
                        case "KREDIT_DISPO":
                            //Ticket#2011120110000071 - Bei DISPO wird Feld AUSZAHLUNG nicht angefasst und kommt 1:1 zurück.
                            break;
                    }
                    break;
            }
            // Determine amount of payout---------------------------------------------------END---------------------------------

            if (kalkulation.angAntKalkDto.szBrutto != kalkulation.angAntKalkDto.sz)
            {
                kalkulation.angAntKalkDto.sz = round.getNetValue(kalkulation.angAntKalkDto.szBrutto, ust);
            }
            _log.Debug("Duration G: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            return kalkulation;
        }

        /// <summary>
        /// Liefert Alter in Monaten seit erstzulassung
        /// </summary>
        /// <param name="erstzul"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        protected static int getAgeInMonths(DateTime? erstzul, DateTime perDate)
        {
            if (erstzul == null)
                erstzul = perDate;
            int age;
            if (erstzul.Value.Year < 1801) erstzul = perDate;//avoid invalid age

            for (age = 0; erstzul.Value.AddMonths(age + 1).CompareTo(perDate) <= 0; age++) ;
            return age;
        }

        private List<AngAntVsDto> cloneInsurances(List<AngAntVsDto> versicherungen)
        {
            if (versicherungen == null)
            {
                return null;
            }

            List<AngAntVsDto> rval = new List<AngAntVsDto>();
            foreach (AngAntVsDto vs in versicherungen)
            {
                AngAntVsDto nvs = ObjectCloner.Clone<AngAntVsDto>(vs);
                //nvs = Mapper.Map<AngAntVsDto, AngAntVsDto>(vs, nvs);
                rval.Add(nvs);
            }
            return rval;
        }


        /// <summary>
        /// Calculates Insurances
        /// benötigt als Input Werte aus Ratenkalkulation
        /// ändert ggf. Input-Werte für finale Ratenkalkulation             
        /// </summary>
        /// <param name="versicherungen">Versicherungen Liste</param>
        /// <param name="kalkulation">Kalkulations Daten</param>
        /// <param name="perDate">Berechnungsdatum</param>
        /// <param name="insuranceBo">Versicherungs BO</param>
        /// <param name="input">the input parameters of a previous calculation</param>
        /// <param name="type">Typ</param>
        /// <param name="isLeasing">isLeasing</param>
        /// <returns>the altered result of a previous calculation</returns>
        public CalculationOutputParameters calcInsurance(List<AngAntVsDto> versicherungen, KalkulationDto kalkulation, DateTime perDate, IInsuranceBo insuranceBo, CalculationInputParameters input, CalcType type, bool isLeasing)
        {
            if (input == null)
            {

                _log.Error("No Calculation delivered for calcInsurance");
                throw new ApplicationException("No Calculation delivered for calcInsurance");
            }

            //insurance needs ratebrutto as inputvalue, so calculate
            
            CalculationOutputParameters result = calcRate(input);
            
            input.barwert = result.barwert;

            IRounding round = RoundingFactory.createRounding();
            result.zinskostenOhneAbsicherung = RoundCHF(calcZinsKosten(input.ersteRate > 0, input.laufzeit, BankNowCalculator.RoundCHF(result.rateBrutto), kalkulation.angAntKalkDto.szBrutto, kalkulation.angAntKalkDto.rwBrutto, BankNowCalculator.RoundCHF(round.getGrossValue(input.barwert, input.ust)), isLeasing));


            if (versicherungen == null)
            {
                return result;
            }
            //all insurances same inputvalues for banknow
            iInsuranceDto vsParam = new iInsuranceDto();
            InsuranceCalcComponent lzparam = new InsuranceCalcComponent(InsuranceCalcComponentType.Laufzeit, input.laufzeit);
            InsuranceCalcComponent aufschubparam = new InsuranceCalcComponent(InsuranceCalcComponentType.Aufschub, 0);
            vsParam.additionalInputValues = new List<InsuranceCalcComponent>();
            vsParam.additionalInputValues.Add(lzparam);
            vsParam.additionalInputValues.Add(new InsuranceCalcComponent(InsuranceCalcComponentType.RateBruttoUnrounded, result.rateBrutto));
            vsParam.additionalInputValues.Add(new InsuranceCalcComponent(InsuranceCalcComponentType.Zins, input.zins));
            vsParam.additionalInputValues.Add(aufschubparam);
            vsParam.perDate = perDate;

            foreach (AngAntVsDto vs in versicherungen)
            {
                vsParam.sysvstyp = vs.sysvstyp;
                vs.lz = kalkulation.angAntKalkDto.lz;
                vs.lz += kalkulation.angAntKalkDto.aufschub;
                aufschubparam.value = kalkulation.angAntKalkDto.aufschub;
                vs.ppy = 12;
                lzparam.value = vs.lz;
                oInsuranceDto vsResult = insuranceBo.calculateInsurance(vs, vsParam, perDate);
                if (vsResult == null) continue;
                if (vs.mitfinflag>0)
                {
                    vs.ppy = 1;
                }

                if (type == CalcType.DEFAULT)//nur für defaultzins werden diese werte verändert
                {
                    kalkulation.angAntKalkDto.calcRsvmonat += vsResult.getOutputValue(InsuranceResultComponentType.RsvMonat);
                    //kalkulation.angAntKalkDto.calcRsvzins += vsResult.getOutputValue(InsuranceResultComponentType.RsvZins);
                    kalkulation.angAntKalkDto.calcRsvgesamt += vsResult.getOutputValue(InsuranceResultComponentType.RsvGesamt);
                }
                else if (type == CalcType.MIN)
                {
                    kalkulation.angAntKalkDto.calcRsvmonatMin += vsResult.getOutputValue(InsuranceResultComponentType.RsvMonat);
                }
                else if (type == CalcType.MAX)
                {
                    kalkulation.angAntKalkDto.calcRsvmonatMax += vsResult.getOutputValue(InsuranceResultComponentType.RsvMonat);
                }
                if(vs.mitfinflag>0)
                {
                    input.barwert += vsResult.getOutputValue(InsuranceResultComponentType.RsvBarwertAddition);
                    result.rsvMitFinBrutto += vsResult.getOutputValue(InsuranceResultComponentType.RsvMonat);
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates Provisions for Expected Loss Calculations
        /// uses a minimum required input interfaces
        /// </summary>
        /// <param name="prodCtx"></param>
        /// <param name="kundenScore"></param>
        /// <param name="finanzierungsbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <returns></returns>
        override
        public List<AngAntProvDto> calculateProvisionsDirect(prKontextDto prodCtx, String kundenScore,double finanzierungsbetrag, double zinsertrag )
        {
            IRounding round = RoundingFactory.createRounding();
            IZinsBo zinsBo = BOFactory.getInstance().createZinsBo(ZinsBo.CONDITIONS_BANKNOW, isoCode);

            long sysprproduct = prodCtx.sysprproduct;
            
            double umsatzbasis = finanzierungsbetrag;// kalkulation.angAntKalkDto.bginternbrutto - kalkulation.angAntKalkDto.szBrutto;
            double zinsbasis = zinsertrag;// RoundCHF(calcZinsKosten(input.ersteRate > 0, input.laufzeit, BankNowCalculator.RoundCHF(result.rateBrutto), kalkulation.angAntKalkDto.szBrutto, kalkulation.angAntKalkDto.rwBrutto, BankNowCalculator.RoundCHF(round.getGrossValue(input.barwert, input.ust)), isLeasing));



            VART vart = prismaDao.getVertragsart(sysprproduct);
            if (vart == null)
            {
                throw new Exception("Product has no VART:" + sysprproduct);
            }

            //Produkte werden anhand vart unterschieden: es gibt KREDIT, TZK, LEASING
            String Code = vart.CODE.ToUpper();
            
            bool isLeasing = (Code.IndexOf("LEAS") > -1);
            bool isDiffLeasing = prismaDao.isDiffLeasing(sysprproduct);
            

         
            

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
                        List<PRRAPVAL> rapvaluesliste =zinsBo.getRapValues(prrap.SYSPRRAP, prodCtx.sysprkgroup);
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
               
                }
            }
            IPrismaProductBo productBo = PrismaBoFactory.getInstance().createPrismaProductBo(PrismaProductBo.CONDITIONS_BANKNOW, isoCode);
            PRPRODUCT product = productBo.getProduct(sysprproduct);
            if (product == null)
            {
                throw new Exception("Product not found:" + sysprproduct);
            }

            //Provision initialization------------------------------------------------------------------------------------------
            provKontextDto provKontext = new provKontextDto();
            provKontext.sysprproduct = prodCtx.sysprproduct;
            provKontext.perDate = prodCtx.perDate;
            provKontext.sysbrand = prodCtx.sysbrand;
            provKontext.sysobtyp = prodCtx.sysobtyp;
            provKontext.sysperole = prodCtx.sysperole;
            provKontext.sysprhgroup = prodCtx.sysprhgroup;
            provKontext.sysvttyp = product.SYSVTTYP.HasValue ? product.SYSVTTYP.Value : 0;
            provKontext.sysvart = product.SYSVART.HasValue ? product.SYSVART.Value : 0;
            provKontext.sysvarttab = product.SYSVARTTAB.HasValue ? product.SYSVARTTAB.Value : 0;
            provKontext.sysperole = prodCtx.sysperole;
            provKontext.sysprhgroup = prodCtx.sysprhgroup;
            provKontext.sysvstyp = 0;//??
            provKontext.sysabltyp = 0;//??
            //Provision initialization---------------------------------END------------------------------------------------------

            IProvisionBo provBo = PrismaBoFactory.getInstance().createProvisionBo();

            List<AngAntProvDto> overwriteProvisions = new List<AngAntProvDto>();

            //for each supported provision:
            provKontext.sysvstyp = 0;//??
            provKontext.sysabltyp = 0;//??
            List<AngAntProvDto> rval = new List<AngAntProvDto>();
            double neugeldFaktor = 1.0;
            double fremdgeldFaktor = 1.0;
            double gesAblFaktor = 0;
            if (rapVal != null)
            {
                if (rapVal.FAKTOR1.HasValue && rapVal.FAKTOR1.Value > 0)
                {
                    neugeldFaktor = (double)rapVal.FAKTOR1.Value;
                }
                if (rapVal.FAKTOR2.HasValue && rapVal.FAKTOR2.Value > 0)
                {
                    fremdgeldFaktor = (double)rapVal.FAKTOR2.Value;
                }
            }

            long hdperole = obTypDao.getHaendlerByEmployee(provKontext.sysperole);
            //check default handelsgruppe
            bool prhGroup = provBo.isPrhGroup(hdperole, provKontext.sysprhgroup);
            if (!prhGroup)
            {
                provKontext.sysprhgroup = 0;
            }
            //double testbasis = zinsbasis * (auszahlungsBasis) / (auszahlungsBasis + extAbloese + intAbloese);
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_FASZL, 0, provBo, provKontext, prodCtx, overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_ZINS_NEUGELD, zinsbasis * (1 - gesAblFaktor) * neugeldFaktor, provBo, provKontext, prodCtx, overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_UMSATZ_NEUGELD, umsatzbasis * (1 - gesAblFaktor), provBo, provKontext, prodCtx, overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_UMSATZ_GESAMT, umsatzbasis, provBo, provKontext, prodCtx, overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_MARK_STUECK_NEUGELD, 0, provBo, provKontext, prodCtx, overwriteProvisions));


            return rval;
        }

            /// <summary>
            /// Provisionen Berechnen
            /// </summary>
            /// <param name="provKontext">Provisions Kontext</param>
            /// <param name="umsatzbasis">Umsatzbasis</param>
            /// <param name="zinsbasis">Zinsbasis</param>
            /// <param name="gesAblFaktor">Ablösefaktor gesamt</param>
            /// <param name="extAblFaktor">Externer Ablösefaktor</param>
            /// <param name="laufzeit">Laufzeit</param>
            /// <param name="auszahlungsBasis">Auszahlungsbasis</param>
            /// <param name="provBo">Provisions BO</param>
            /// <param name="prodCtx">Produktions Kontext</param>
            /// <param name="versicherungen">Versicherungsliste</param>
            /// <param name="abloesen">Ablösen</param>
            /// <param name="overwriteProvisions">Provisionen Überschreiben-Flag</param>
            /// <param name="istzk">TZK Flag</param>
            /// <param name="isDispo">Dispo Flag</param>
            /// <param name="provRsvFaktor">RSV Provisionsfaktor</param>
            /// <param name="rap">RAP Provisionsinfo</param>
            /// <param name="bginternbrutto">bginternbrutto</param>
            /// <returns></returns>
            public List<AngAntProvDto> calculateProvisions(provKontextDto provKontext, double umsatzbasis, double zinsbasis, double gesAblFaktor, double extAblFaktor, double laufzeit, 
                                                       double auszahlungsBasis, IProvisionBo provBo, prKontextDto prodCtx,  List<AngAntVsDto> versicherungen, List<AngAntAblDto> abloesen, 
                                                       List<AngAntProvDto> overwriteProvisions, bool istzk,  bool isDispo, double provRsvFaktor, CIC.Database.PRISMA.EF6.Model.PRRAPVAL rap, double bginternbrutto)
        {
            //for each supported provision:
            provKontext.sysvstyp = 0;//??
            provKontext.sysabltyp = 0;//??
            List<AngAntProvDto> rval = new List<AngAntProvDto>();
            double neugeldFaktor = 1.0;
            double fremdgeldFaktor = 1.0;
            if(rap!=null)
            {
                if (rap.FAKTOR1.HasValue && rap.FAKTOR1.Value>0)
                {
                    neugeldFaktor = (double)rap.FAKTOR1.Value;
                }
                if (rap.FAKTOR2.HasValue && rap.FAKTOR2.Value > 0)
                {
                    fremdgeldFaktor = (double)rap.FAKTOR2.Value;
                }
            }

            long hdperole = obTypDao.getHaendlerByEmployee(provKontext.sysperole);
            //check default handelsgruppe
            bool prhGroup = provBo.isPrhGroup(hdperole, provKontext.sysprhgroup);
            if (!prhGroup)
            {
                provKontext.sysprhgroup = 0;
            }
            //double testbasis = zinsbasis * (auszahlungsBasis) / (auszahlungsBasis + extAbloese + intAbloese);
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_FASZL, 0, provBo, provKontext, prodCtx, overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_ZINS_NEUGELD, zinsbasis * (1 - gesAblFaktor) * neugeldFaktor, provBo, provKontext, prodCtx, overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_UMSATZ_NEUGELD, umsatzbasis * (1-gesAblFaktor), provBo, provKontext, prodCtx,  overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_UMSATZ_GESAMT, umsatzbasis, provBo, provKontext, prodCtx, overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_MARK_STUECK_NEUGELD, 0, provBo, provKontext, prodCtx,  overwriteProvisions));
            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_DISPO, auszahlungsBasis, provBo, provKontext, prodCtx,  overwriteProvisions));

            if (versicherungen != null)
            {
                foreach (AngAntVsDto vs in versicherungen)
                {
                    double provBase = vs.praemie;//use unrounded insurance value!
                    provKontext.sysvstyp = vs.sysvstyp;
                    if (vs.ppy != 1 && vs.mitfinflag != 1)
                    {
                        provBase *= vs.lz;
                    }
                    
                    String cm = insuranceDao.getVSTYP(vs.sysvstyp).CODEMETHOD;
                    if (VSCalcFactory.Cnst_CALC_AUEUAL_KREDIT.Equals(cm) || VSCalcFactory.Cnst_CALC_AUEUAL_LEASING.Equals(cm))
                    {
                        rval.AddRange(calcProvisions(ProvisionSourceField.PROV_MARK_STUECK_VERSICHERUNG, 0, provBo, provKontext, prodCtx, overwriteProvisions));
                        rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_VERSICHERUNG, provBase * provRsvFaktor, provBo, provKontext, prodCtx, overwriteProvisions));
                    }
                    if (VSCalcFactory.Cnst_CALC_TODESFALL_KREDIT.Equals(cm) || VSCalcFactory.Cnst_CALC_TODESFALL_LEASING.Equals(cm))
                    {
                        rval.AddRange(calcProvisions(ProvisionSourceField.PROV_MARK_STUECK_VERSICHERUNG_RIP, 0, provBo, provKontext, prodCtx, overwriteProvisions));
                        rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_VERSICHERUNG_RIP, provBase, provBo, provKontext, prodCtx, overwriteProvisions));
                    }
                }
            }

            if (abloesen != null)
            {
                provKontext.sysvstyp = 0;
                provKontext.sysabltyp = 0;
                foreach (AngAntAblDto abl in abloesen)
                {
                    provKontext.sysabltyp = abl.sysabltyp;
                    if (provBo.isAbloesetyp(abl.sysabltyp, Abloesetyp.EXTERN))
                    {
                        rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_ZINS_ABLEXTERN, zinsbasis * (abl.betrag / bginternbrutto) * fremdgeldFaktor, provBo, provKontext, prodCtx, overwriteProvisions));
                    }
                    else
                    {
                        rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_ZINS_ABLINTERN, zinsbasis * (abl.betrag / bginternbrutto), provBo, provKontext, prodCtx, overwriteProvisions));
                    }

                    if (!isDispo)
                    {
                        if (provBo.isAbloesetyp(abl.sysabltyp, Abloesetyp.EXTERN))
                        {
                            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_UMSATZ_ABLEXTERN, abl.betrag * (extAblFaktor / gesAblFaktor), provBo, provKontext, prodCtx, overwriteProvisions));
                        }
                        else
                        {
                            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_UMSATZ_ABLINTERN, abl.betrag * ((1 - extAblFaktor) / gesAblFaktor), provBo, provKontext, prodCtx, overwriteProvisions));
                        }
                    }
                    else
                    {
                        if (provBo.isAbloesetyp(abl.sysabltyp, Abloesetyp.EXTERN))
                        {
                            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_UMSATZ_ABLEXTERN, abl.betrag, provBo, provKontext, prodCtx, overwriteProvisions));
                        }
                        else
                        {
                            rval.AddRange(calcProvisions(ProvisionSourceField.PROV_BASE_UMSATZ_ABLINTERN, abl.betrag, provBo, provKontext, prodCtx, overwriteProvisions));
                        }
                    }
                }
            }
            return rval;
        }

        /// <summary>
        /// Iterates over all provisiontypes and all provisioned prflds and creates provisions
        /// the calculation base values are fetched by the prfld-objectmeta-codes that are linked to the values by convention
        /// </summary>
        /// <param name="field">Feld</param>
        /// <param name="baseValue">Basiswert</param>
        /// <param name="provBo">Provisionen BO</param>
        /// <param name="provKontext">Provisionenkontext</param>
        /// <param name="prKontext">Kontext Daten</param>
        /// <param name="overwriteProvisions">Liste zu ueberschreibender Provisionen</param>
        /// <returns></returns>
        public List<AngAntProvDto> calcProvisions(ProvisionSourceField field, double baseValue, IProvisionBo provBo, provKontextDto provKontext, prKontextDto prKontext,  List<AngAntProvDto> overwriteProvisions)
        {
            List<AngAntProvDto> rval = new List<AngAntProvDto>();

            //input: provisionSourceField
            //-> gets ParamDto -> prfld
            //if not in prflds return
            //optional sysvstyp/sysabltyp
            long FieldId = provBo.getPrfldId(field, prKontext);
            if (FieldId == 0)
            {
                return rval;
            }

            List<PRPROVTYPE> provtypes = provBo.getProvisionTypes(FieldId);
            iProvisionDto provParam = new iProvisionDto();

            foreach (PRPROVTYPE ptype in provtypes)
            {
                provParam.sysprprovtype = ptype.SYSPRPROVTYPE;
                provParam.sysprfld = FieldId;
                provParam.provisionInputValue = baseValue;
                provParam.provType = ProvGroupAssignType.AUTO;
                provParam.useProvisionOverwriteValue = false;
                provParam.provisionOverwriteValue = 0;
                //CR 196 - new field mapped
                provParam.defRoleType = (int)ptype.SYSDEFROLETYPE.GetValueOrDefault();
                bool bIgnore = false;

                if (overwriteProvisions != null && overwriteProvisions.Count != 0)
                {
                    foreach (AngAntProvDto ovp in overwriteProvisions)
                    {
                        if (ovp.sysprprovtype == provParam.sysprprovtype && !UsedProv.Contains(provParam.sysprprovtype))
                        {
                            provParam.useProvisionOverwriteValue = true;
                            provParam.provisionOverwriteValue = ovp.provision;
                            if (field == ProvisionSourceField.PROV_BASE_VERSICHERUNG)
                            {
                                this.UsedProv.Add(provParam.sysprprovtype);
                            }
                            break;
                        }
                        else
                            if (ovp.sysprprovtype == provParam.sysprprovtype && UsedProv.Contains(provParam.sysprprovtype))
                            {
                                bIgnore = true;
                            }
                    }
                }
                if (!bIgnore)
                {
                    List<AngAntProvDto> provs = provBo.calculateProvision(provKontext, provParam);
                    if (provs != null)
                    {
                        rval.AddRange(provs);
                    }
                }
            }
            foreach(AngAntProvDto dto in rval)
            {
                dto.provisionOrg = dto.provision;
                dto.provision = RoundCHF(dto.provision);
                dto.provisionBrutto = RoundCHF(dto.provisionBrutto);

                dto.defaultprovision = RoundCHF(dto.defaultprovision);
                dto.defaultprovisionbrutto = RoundCHF(dto.defaultprovisionbrutto);
            }
            return rval;
        }

        
        /// <summary>
        /// solves to Rate, doesnt change input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CalculationOutputParameters calcRate(CalculationInputParameters input)
        {
            IRounding round = RoundingFactory.createRounding();
            CalculationOutputParameters rval = new CalculationOutputParameters();
            bool vorschuessig = true;
            double barwert = input.barwert;
            double laufzeit = input.laufzeit;
            double ust = input.ust;

            // HR 02.08.2011: Anpassung wegen Zeitlicher einpassung Erste Rate in Kredit oder Leasing
            rval.provisionsBasis = input.barwert;
            if (input.isCredit)//Kredit
            {
                vorschuessig = false;
                ust = 0;
                barwert -= input.ersteRate;
            }
            else
            {
                if (input.ersteRate > 0)
                {
                    laufzeit--;
                    // Buchwert nach 1.Monat
                    if(input.isDiffLeasing)
                    {
                        barwert = Kalkulator.calcENDW(barwert, input.ersteRate, input.zins / 12, 1, true);
                    }
                    else
                    {
                        barwert = System.Math.Round(Kalkulator.calcENDW(barwert, input.ersteRate, input.zins / 12, 1, true) / 5, 2) * 5;
                    }
                    rval.provisionsBasis = input.barwert ;
                
                }
            }
            // HR 2.08.2011: Ende der Anpassung

            //Ratenberechnung
            double rate = Kalkulator.calcRATE(barwert, input.zins / 12, laufzeit, input.restwert, vorschuessig);
            rval.barwert = input.barwert;// Kalkulator.calcBARW(rate, input.zins / 12, laufzeit, input.restwert, vorschuessig);
            rval.vorschuessig = vorschuessig;
            rval.barwertOhneAbsicherung = rval.barwert;

            //rechenweg  mwst/rundung:
            //rate ungerundet netto
            //rate inkl. mwst
            //rate chf
            rval.rateBrutto = round.getGrossValue(rate, ust);

            rval.rate = rate;
            rval.zinseff = CalculateEffectiveInterest(input.zins, 12);
            rval.laufzeit = laufzeit;
            if (input.ersteRate > rval.rate) //7.1.10.1 Produkthandbuch
            {
                rval.provisionsBasis = input.barwert - input.ersteRate;
            }

            if (input.istzk)
            {
                CalculationInputParameters input2 = input.clone();
                input2.istzk = false;
                double tzzuschlag = rval.rate * input2.laufzeit + input2.restwert + input.ersteRate - input2.barwert;
                double mwsttz = round.getTaxValue(tzzuschlag, input2.mwst);
                input2.barwert += mwsttz;
                
                CalculationOutputParameters rval2 = calcRate(input2);
                rval2.provisionsBasis = rval.provisionsBasis;
                rval2.mwstTeilzahlungszuschlag = mwsttz;
                return rval2;
            }
            if (rval.rate < 0)
            {
                rval.rateError = true;
            }
            return rval;

        }

       /// <summary>
       /// Zinskosten berechnen
       /// </summary>
       /// <param name="hasErsteRate">erste Rate Flag</param>
       /// <param name="laufzeit">Laufzeit</param>
       /// <param name="rateBrutto">Brutto Rate</param>
       /// <param name="szBrutto">Sonderzahlung Brutto</param>
       /// <param name="rwBrutto">Restwert Brutto</param>
       /// <param name="bwBrutto">Basiswert Brutto</param>
       /// <param name="isLeasing">Ist Leasing-Flag</param>
       /// <returns></returns>
        public double calcZinsKosten(bool hasErsteRate, double laufzeit, double rateBrutto, double szBrutto, double rwBrutto, double bwBrutto, bool isLeasing)
        {
            double lz = laufzeit;
            if (hasErsteRate && isLeasing)
            {
                lz--;
            }
            double rval = RoundCHF(rateBrutto) * lz + szBrutto + rwBrutto - bwBrutto;// -mwstTeilzahlungRounded;// - gesAbloese;
            if (rval < 0)
            {
                rval = 0;
            }
            return rval;
        }

        /// <summary>
        /// Effektivzins berechnen
        /// </summary>
        /// <param name="nominalInterest">Nominalzins</param>
        /// <param name="ppy">Zahlungen pro Jahr</param>
        /// <returns>Effektivzins</returns>
        public static double CalculateEffectiveInterest(double nominalInterest, int ppy)
        {
            int Ppy = ppy;

            double d = System.Math.Pow((double)(1 + (nominalInterest / (100 * Ppy))), (double)Ppy);

            d = (d - 1) * 100;

            return d;
        }

        /// <summary>
        /// Nominalzins berechnen
        /// </summary>
        /// <param name="effectiveInterest">Effektivzins</param>
        /// <param name="ppy">Zahlungen Pro Jahr</param>
        /// <returns>Nominalzins</returns>
        public static double CalculateNominalInterest(double effectiveInterest, int ppy)
        {
            double Ppy = ppy;

            double d = System.Math.Pow((double)((effectiveInterest / 100) + 1), (double)(1 / Ppy));
            d = (d - 1) * 100 * Ppy;

            return d;
        }


        /// <summary>
        /// Rundung Schweitzer Franken
        /// </summary>
        /// <param name="value">Eingangswert ungerundet</param>
        /// <returns>Ausgangswert gerundet</returns>
        public static double RoundCHF(double value)
        {
            IRounding round = RoundingFactory.createRounding();
            return round.RoundCHF(value);
            //Round to 2 places after coma
            //return System.Math.Round(value * 20, 0) / 20;
        }




    }

    /// <summary>
    /// Rechenart
    /// </summary>
    public enum CalcType
    {
        /// <summary>
        /// Standard
        /// </summary>
        DEFAULT,
        /// <summary>
        /// Minimal
        /// </summary>
        MIN,
        /// <summary>
        /// Mximal
        /// </summary>
        MAX,
        /// <summary>
        /// Differenzleasing
        /// </summary>
        DIFFLEASING//wont do anything
    }

    /// <summary>
    /// Kalkulations-Eingabeparameter
    /// </summary>
    public class CalculationInputParameters
    {
        /// <summary>
        /// Barwert
        /// </summary>
        public double barwert { get; set; }
        /// <summary>
        /// Laufzeit
        /// </summary>
        public double laufzeit { get; set; }
        /// <summary>
        /// Erste Rate
        /// </summary>
        public double ersteRate { get; set; }
        /// <summary>
        /// Restwert
        /// </summary>
        public double restwert { get; set; }
        /// <summary>
        /// Kredit-Flag
        /// </summary>
        public bool isCredit { get; set; }
        /// <summary>
        /// Zinssatz
        /// </summary>
        public double zins { get; set; }
        /// <summary>
        /// Umsatzsteuer
        /// </summary>
        public double ust { get; set; }
        /// <summary>
        /// Teilzahlungs-Flag
        /// </summary>
        public bool istzk { get; set; }
        /// <summary>
        /// Differenzleasing-Flag
        /// </summary>
        public bool isDiffLeasing { get; set; }
        /// <summary>
        /// Mehrwertsteuer
        /// </summary>
        public double mwst { get; set; }
        
        /// <summary>
        /// Kalkulationeingaben Klonen
        /// </summary>
        /// <returns>Kopie der Eingaben</returns>
        public CalculationInputParameters clone()
        {
            CalculationInputParameters rval = new CalculationInputParameters();
            rval.barwert = this.barwert;
            rval.laufzeit = this.laufzeit;
            rval.ersteRate = this.ersteRate;
            rval.restwert = this.restwert;
            rval.isCredit = this.isCredit;
            rval.zins = this.zins;
            rval.ust = this.ust;
            rval.istzk = this.istzk;
            rval.mwst = this.mwst;
            rval.isDiffLeasing = this.isDiffLeasing;
            return rval;
        }
    }

    /// <summary>
    /// Kalkulations Ausgabeparameter
    /// </summary>
    public class CalculationOutputParameters
    {
        /// <summary>
        /// Bruttorate
        /// </summary>
        public double rateBrutto { get; set; }
        /// <summary>
        /// Nettorate
        /// </summary>
        public double rate { get; set; }
        /// <summary>
        /// Effektivzins
        /// </summary>
        public double zinseff { get; set; }
        /// <summary>
        /// Laufzeit
        /// </summary>
        public double laufzeit { get; set; }
        /// <summary>
        /// Barwert
        /// </summary>
        public double barwert { get; set; }
        /// <summary>
        /// Barwert ohne Absicherung
        /// </summary>
        public double barwertOhneAbsicherung { get; set; }
        /// <summary>
        /// Vorschüssigkeits - Flag
        /// </summary>
        public bool vorschuessig { get; set; }
        /// <summary>
        /// Mehrwertsteuer Teilzahlungszuschalg
        /// </summary>
        public double mwstTeilzahlungszuschlag { get; set; }
        /// <summary>
        /// Provisionsbasis
        /// </summary>
        public double provisionsBasis { get; set; }
        /// <summary>
        /// Ratenfehler
        /// Rate ist negativ
        /// </summary>
        public bool rateError { get; set; }
        /// <summary>
        /// Barwert ohne Absicherung
        /// </summary>
        public double zinskostenOhneAbsicherung { get; set; }

        /// <summary>
        /// rateBrutto enthält bereits mitfin Versicherungen, calcRsvmonat enthält aber alle VS, mitfin und nicht mitfin, für die Gesamtrate muss also der mitfinanzierte Bestandteil von calcRsvmonat wieder abgezogen werden
        /// </summary>
        public double rsvMitFinBrutto { get; set; }
    }

}

