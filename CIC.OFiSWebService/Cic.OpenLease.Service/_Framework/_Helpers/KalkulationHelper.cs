using System;

namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.One.Util.IO;
    using Cic.One.Utils.BO;
    using Cic.One.Utils.DTO;
    using Cic.OpenLease.Service.Provision;
    using Cic.OpenLease.Service.Services.DdOl;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using System.Reflection;
    #endregion

    // Dear maintainer:
    // 
    // Once you are done trying to 'optimize' this routine,
    // and have realized what a terrible mistake that was,
    // please increment the following counter as a warning
    // to the next guy:
    // 
    // total_hours_wasted_here = 128
    // 
    public static class KalkulationHelper
    {
        public const string CnstBearbeitungsgebuehrCode = "Bearb_Geb";
        public const string CnstGebuhrenParam = "Bearbeitungsgebühr Vertrag";
        private const int CnstDefaultZins = 7;
        private const int CnstMaxMietvorauszahlungBruttoP = 30;

        private const String maxZinsMsg = "Zinssatz wurde auf zulässigen Maximalwert reduziert!";
        private const String minZinsMsg = "Zinssatz wurde auf zulässigen Minimalwert erhöht!";

        public static bool debug = true;

        private const int CnstPeriodsPerYear = 12;
        private const int CnstRank = 10;

        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods
        public static ProvisionDto DeliverAbschlussProvision(DdOlExtended context, ProvisionDto param, long sysBRAND, long sysPEROLE)
        {
            param.rank = (int)ProvisionTypeConstants.Abschluss;

            PROVDao dao = new PROVDao(context);
            if (param.sysBrand == 0)
                param.sysBrand = sysBRAND;
            if (param.sysPerole == 0)
                param.sysPerole = sysPEROLE;

            //if (debug) _Log.Debug("Calc Abschluss Provision with " + _Log.dumpObject(param));

            return dao.DeliverProvision(param);
        }
        public static ProvisionDto DeliverRestschuldProvision(DdOlExtended context, ProvisionDto param, long sysBRAND, long sysPEROLE)
        {
            param.rank = (int)ProvisionTypeConstants.Restschuld;


            PROVDao dao = new PROVDao(context);
            if (param.sysBrand == 0)
                param.sysBrand = sysBRAND;
            if (param.sysPerole == 0)
                param.sysPerole = sysPEROLE;

            //if (debug) _Log.Debug("Calc RSV Provision with " + _Log.dumpObject(param));

            return dao.DeliverProvision(param);
        }
        public static ProvisionDto DeliverGAPProvision(DdOlExtended context, ProvisionDto param, long sysBRAND, long sysPEROLE)
        {
            param.rank = (int)ProvisionTypeConstants.GAP;


            PROVDao dao = new PROVDao(context);
            if (param.sysBrand == 0)
                param.sysBrand = sysBRAND;
            if (param.sysPerole == 0)
                param.sysPerole = sysPEROLE;

            //if (debug) _Log.Debug("Calc GAP Provision with " + _Log.dumpObject(param));

            return dao.DeliverProvision(param);
        }


        public static void updateFinanzierungssummeFinal(CalculationDto calculationDto, decimal Ust, bool isCredit, decimal UstErinkl)
        {
            if (isCredit) return;
            if(UstErinkl!=Ust)
            {
                
                calculationDto.AnschaffungswertNetto = calculationDto.KaufpreisNetto;
                calculationDto.AnschaffungswertBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.KaufpreisNetto, UstErinkl);
                calculationDto.AnschaffungswertUst = calculationDto.AnschaffungswertBrutto - calculationDto.AnschaffungswertNetto;
                calculationDto.FinanzierungssummeNetto = calculationDto.KaufpreisNetto - calculationDto.MietvorauszahlungNetto + Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.Verrechnung, Ust);
                calculationDto.FinanzierungssummeBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.KaufpreisNetto, UstErinkl) - calculationDto.MietvorauszahlungBrutto + calculationDto.Verrechnung;
                calculationDto.FinanzierungssummeUst = calculationDto.FinanzierungssummeBrutto - calculationDto.FinanzierungssummeNetto;
                calculationDto.KaufpreisBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.KaufpreisNetto, UstErinkl);

            }
        }
        /// <summary>
        /// Updates the mvz netto/brutto percentages and ust depending on the source
        /// expects the values of the source (netto or brutto value, not percent!) already to be correct
        /// </summary>
        /// <param name="calculationDto"></param>
        /// <param name="fromNetto"></param>
        /// <param name="Ust"></param>
        public static void updateMVZ(CalculationDto calculationDto, bool fromNetto, decimal Ust, bool isCredit)
        {
            if (isCredit)
                Ust = 0;
            if(!fromNetto)//update Netto, brutto was changed
            {
                calculationDto.MietvorauszahlungNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.MietvorauszahlungBrutto, Ust);
                calculationDto.MietvorauszahlungP = CalculateMietvorauszahlungP(calculationDto.MietvorauszahlungNetto, calculationDto.KaufpreisNetto);
                calculationDto.MietvorauszahlungBruttoP = CalculateMietvorauszahlungP(calculationDto.MietvorauszahlungBrutto, calculationDto.KaufpreisBruttoOrg);
            }
            else//netto was entered,update brutto 
            {
                calculationDto.MietvorauszahlungBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.MietvorauszahlungNetto, Ust);
                calculationDto.MietvorauszahlungP = CalculateMietvorauszahlungP(calculationDto.MietvorauszahlungNetto, calculationDto.KaufpreisNetto);
                calculationDto.MietvorauszahlungBruttoP = CalculateMietvorauszahlungP(calculationDto.MietvorauszahlungBrutto, calculationDto.KaufpreisBruttoOrg);
            }
            calculationDto.MietvorauszahlungUst = calculationDto.MietvorauszahlungBrutto - calculationDto.MietvorauszahlungNetto;
        }
        public static void updateFinanzierungssumme(CalculationDto calculationDto, decimal Ust, bool isCredit, decimal NovaAufschlagSatz, bool isGebraucht, decimal UstErinkl)
        {
            calculationDto.FinanzierungssummeBrutto = 0;
            

            calculationDto.FinanzierungssummeBrutto = calculationDto.AnschaffungswertBrutto - calculationDto.MietvorauszahlungBrutto + calculationDto.Verrechnung;
            if (calculationDto.hasRSV && isCredit && calculationDto.IgnoreInsurances<1)//&& (calculationDto.SubventionCalcMode < 2 || calculationDto.SubventionCalcMode > 3)
                calculationDto.FinanzierungssummeBrutto += calculationDto.RestschuldVersicherung;


            if (isCredit)
            {
                calculationDto.FinanzierungssummeNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.FinanzierungssummeBrutto, Ust);
                calculationDto.FinanzierungssummeUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.FinanzierungssummeBrutto, Ust);
            }
            else {
                calculationDto.FinanzierungssummeNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.AnschaffungswertBrutto, UstErinkl) - Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.MietvorauszahlungBrutto, Ust) + Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.Verrechnung, Ust);
                calculationDto.FinanzierungssummeUst = calculationDto.FinanzierungssummeBrutto - calculationDto.FinanzierungssummeNetto;
            }
            if (calculationDto.hasGAP && isCredit && calculationDto.IgnoreInsurances < 1)//&& (calculationDto.SubventionCalcMode < 2 || calculationDto.SubventionCalcMode > 3) 
            {
                calculationDto.FinanzierungssummeBrutto += calculationDto.GAPVersicherung;
                calculationDto.FinanzierungssummeNetto += calculationDto.GAPVersicherung;
            }

            if (isCredit)
            {
                calculationDto.FinanzierungssummeNetto = calculationDto.FinanzierungssummeBrutto;
                calculationDto.FinanzierungssummeUst = 0;
            }
         

        }

        public static Cic.OpenLease.ServiceAccess.DdOl.CalculationDto Calculate(DdOlExtended Context, long sysBRAND, long sysPEROLE, Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, CalculationDao calcDao, GebuehrDao gebuehrDao, RVSuggest rvSuggest, VSTYPDao vs, PRParamDao prparamDao, LsAddDao lsaddDao, KORREKTURDao kDao, QUOTEDao qDao)
        {

            decimal Interest = CnstDefaultZins;

            
            KALKTYP KalkTyp = new KALKTYP();
            KalkTyp.SYSKALKTYP = 10;
            KalkTyp.MODUS = 0;

            decimal NovaForDepot;
            decimal Ust;
            decimal UstErinkl =0;


           
            calculationDto.Message = string.Empty;
            calculationDto.MessageCode = CalculationDto.MessageCodes.NoError;
            //if (debug) _Log.Debug("calculate called with:" + System.Environment.NewLine + _Log.dumpObject(calculationDto));

            double measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Inputvalidation
            //Validate input values
            if (calculationDto.Wunschprovision < 0)
                calculationDto.Wunschprovision = 0;
            if (calculationDto.SysPrProduct <= 0)
            {
                calculationDto.MessageCode = CalculationDto.MessageCodes.PrProd001;
                calculationDto.Message += "SysPrProduct not defined" + System.Environment.NewLine;
                return calculationDto;
            }

            if (calculationDto.SysObTyp <= 0)
            {
                calculationDto.MessageCode = CalculationDto.MessageCodes.ObTyp001;
                calculationDto.Message += "SysObTyp not defined" + System.Environment.NewLine;
                return calculationDto;
            }

            if (calculationDto.SysObArt <= 0)
            {
                calculationDto.MessageCode = CalculationDto.MessageCodes.ObArt001;
                calculationDto.Message += "SysObArt not defined" + System.Environment.NewLine;
                return calculationDto;
            }

        

            if (calculationDto.Laufzeit <= 0)
            {
                calculationDto.MessageCode = CalculationDto.MessageCodes.Lz001;
                calculationDto.Message += "Laufzeit not defined" + System.Environment.NewLine;
                return calculationDto;
            }

        
            #endregion
            

            calculationDto.hasPouvoirMessage = false;
            calculationDto.pouvoirEnteredValue = 0;
            calculationDto.pouvoirMax = 0;
            calculationDto.pouvoirMin = 0;

            #region Zinsermittlung
            CalculationDto calcInfoSubv = null;
            if (calculationDto.SubventionCalcMode == 0)
                calcInfoSubv = ObjectCloner.Clone(calculationDto);


            bool calcSubvention = calculationDto.SubventionCalcMode > 0;
            //if true, zins will be without subventions
            int excludeSubventionZins = 0;
            if (calculationDto.SubventionCalcMode == 1 || calculationDto.SubventionCalcMode == 2)
                excludeSubventionZins = 1;
            bool zinsPouvoirCheck = false;

            decimal targetZinsEff = calculationDto.ZinsEff;
            decimal depotZins = 0;

            _Log.Debug("Duration A " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;

            try
            {
                double ctime = DateTime.Now.TimeOfDay.TotalMilliseconds;


                if ( calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Verzinsungsart || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Laufzeit || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ProductChange)
                {
                    //Gen Zins
                    Interest = ZinsHelper.DeliverZins(Context, calculationDto.SysPrProduct, calculationDto.Laufzeit, System.Convert.ToInt32(calculationDto.AnschaffungswertBrutto), calculationDto.SysObTyp, calculationDto.SysObArt, calculationDto.SysPrkGroup, calculationDto.SysPrhGroup, sysBRAND, sysPEROLE, calculationDto.Verzinsungsart, 0);

                    if(calculationDto.SubventionCalcMode==4)//DefaultZins
                        Interest = ZinsHelper.DeliverZinsBasis(Context,calculationDto.SysPrProduct, calculationDto.Laufzeit,(double) calculationDto.AnschaffungswertBrutto);
                    else if(calculationDto.SubventionCalcMode==5)//nur subventionszins
                        Interest = ZinsHelper.DeliverZins(Context,calculationDto.SysPrProduct, calculationDto.Laufzeit, System.Convert.ToInt32(calculationDto.AnschaffungswertBrutto), calculationDto.SysObTyp, calculationDto.SysObArt, calculationDto.SysPrkGroup, calculationDto.SysPrhGroup, sysBRAND, sysPEROLE, calculationDto.Verzinsungsart, 2);
                    
                    //interpret interest as eff zins, so calc back to nominal
                    Interest = CalculateNominalInterest(Interest, CnstPeriodsPerYear);
                    
                    calculationDto.ZinsNominal_Default = Interest;
                }
                else
                {
                    Interest = calculationDto.Zins;
                }

                depotZins = DeliverVerZinsung(qDao, prparamDao, calculationDto, sysPEROLE, sysBRAND, Interest);
                //Change zins to default if is 0, (may be 0 with subvention)
                /*if (Interest == 0 && excludeSubventionZins)
                {
                    Interest = CnstDefaultZins;
                    calculationDto.MessageCode = CalculationDto.MessageCodes.Zns001;
                    calculationDto.Message = "Zins was 0. Changed to " + CnstDefaultZins + System.Environment.NewLine;
                }*/

                if (!calculationDto.isECOM)
                {
                    decimal refizins = 0;
                    /*int zinstyp = ZinsHelper.DeliverZinsTyp(calculationDto.Verzinsungsart);
                    if (zinstyp == 0)
                        refizins = ZinsHelper.DeliverRefiZinsFix();
                    else refizins = ZinsHelper.DeliverRefiZinsVariabel();*/

                   // _Log.Debug("Refizins " + refizins + " Verzinsungsart: " + calculationDto.Verzinsungsart + " Zinstyp: " + zinstyp);
                    calculationDto.Zinssatz = refizins;
                }

                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsNominal)
                {
                    if (calcSubvention)//wenn bei einer zinsänderung der subventionierte Zins berechnet werden soll
                    {
                        calculationDto.Zins = Interest;
                        calculationDto.ZinsNominal = Interest;
                    }
                    else
                    {
                        //Wird über Zins übergeben :(
                        calculationDto.ZinsNominal = calculationDto.Zins;

                        calculationDto.ZinsNominal = getZinsPouvoir(calculationDto.ZinsNominal, calculationDto.Zinssatz, PRParamDao.CnstZinsNominalFieldMeta, sysBRAND, sysPEROLE, calculationDto, prparamDao);
                        calculationDto.Zins = calculationDto.ZinsNominal;
                        Interest = calculationDto.ZinsNominal;
                        
                        depotZins = DeliverVerZinsung(qDao, prparamDao, calculationDto, sysPEROLE, sysBRAND, Interest);
                    }
                }
                else
                    calculationDto.Zins = Interest;
                _Log.Debug("Interest Duration: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - ctime));
            }
            catch (System.Exception e)
            {
                calculationDto.Message = e.Message + System.Environment.NewLine;
                calculationDto.MessageCode = CalculationDto.MessageCodes.Zns001;
            }
            #endregion
            _Log.Debug("Duration B " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Assignment and Validation of Input
           
           

            Boolean iscredit = false;

            //Get Ust
            VartDTO va = calcDao.getVART(calculationDto.SysPrProduct);
            long sysvart = va.SYSVART;
            Ust = lsaddDao.GetTaxRate(sysvart);
            if (calculationDto.KaufpreisBruttoOrg==0)
                calculationDto.KaufpreisBruttoOrg = calculationDto.KaufpreisBrutto;

            if (calculationDto.KaufpreisNetto != 0)
            {
                
                UstErinkl = calculationDto.KaufpreisBrutto / calculationDto.KaufpreisNetto;
                if (UstErinkl == 0)
                    UstErinkl = 1;
                UstErinkl = (UstErinkl * 100) - 100;
                if (UstErinkl < Ust && (sysvart == 13 || sysvart == 14))//we have a nettocalculation to perform
                {
                    if (calculationDto.erinklmwst == 0)
                    {
                        calculationDto.KaufpreisNetto = calculationDto.KaufpreisBrutto;// Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.KaufpreisBrutto, Ust);
                        Ust = 0;
                        UstErinkl = Ust;

                    }
                    else
                    {
                        //calc the brutto from netto, later we have the correct brutto-rate
                        calculationDto.KaufpreisBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.KaufpreisNetto, Ust);
                    }
                }
                else UstErinkl = Ust;
            }
            long SysINTSTRCT = 0;
            LSADD lsadd = lsaddDao.getLSADDByVART(sysvart);
            if (lsadd != null && lsadd.INTSTRCT != null)
                SysINTSTRCT = lsadd.INTSTRCT.SYSINTSTRCT;

            if (va.CODE.IndexOf("KREDIT")>-1)
                iscredit = true;

            ObartDTO oa = calcDao.getObArt(calculationDto.SysObArt);
            bool isBarkredit = oa.TYP == 3;
            bool isUsedCar = oa.TYP == 1;
            

            if (calculationDto.useNettoSource)//when set, convert all potential netto inputs back to brutto
            {
                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Restwert)
                {
                    calculationDto.RestwertBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.RestwertNetto, iscredit?0:Ust);
                }
                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Rate)
                {
                    calculationDto.MonatlicheRate = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.MonatlicheRateNetto, iscredit ? 0 : Ust);
                }
                /*  if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.RestwertP)
                  {
                      calculationDto.RestwertBrutto =CalculateRW(calculationDto.R Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.RestwertNetto, Ust);
                  }*/
                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.MietvorauszahlungP)//netto percent
                {
                    calculationDto.MietvorauszahlungNetto = KalkulationHelper.CalculateMietvorauszahlung(calculationDto.MietvorauszahlungP, calculationDto.KaufpreisNetto);
                    updateMVZ(calculationDto, true, Ust, iscredit);
                }
                updateMVZ(calculationDto, true, Ust, iscredit);
            }
            decimal wunschRate = calculationDto.MonatlicheRate;

            //NOVANEU
            calculationDto.NovaAufschlag = 0;
            decimal NovaAufschlagSatz = 0;
            decimal sonderminderung = 0;
          
            NovaType nt = new NovaType(Ust, calculationDto.NovaSatz, NovaAufschlagSatz,0);
          
            //KaufpreisBrutto=ANGOBAHKEXTERNBRUTTO
            //MitfinanzierterBestandteilBrutto=ANGKALKMITFINBRUTTO
            calculationDto.AnschaffungswertBrutto = calculationDto.KaufpreisBrutto;// +calculationDto.MitfinanzierterBestandteilBrutto;
          
            //Get Nova
            NovaForDepot = 0M;

            decimal TaxGeb = Ust;
            if (iscredit)
                TaxGeb = 0;

            //initial calc set defaults
            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ProductChange)
            {

                if (calculationDto.MietvorauszahlungBruttoP == 0 && !calculationDto.isVAP)//init SZ only when not from vap
                {
                    PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstMietvorauszahlungFieldMeta, calculationDto.SysObArt, false);
                    if (par != null)
                    {
                        if (par.DEFVALP.HasValue)
                        {
                            calculationDto.MietvorauszahlungBruttoP = (decimal)par.DEFVALP;
                            calculationDto.MietvorauszahlungBrutto = KalkulationHelper.CalculateMietvorauszahlung(calculationDto.MietvorauszahlungBruttoP, calculationDto.KaufpreisBruttoOrg);
                            updateMVZ(calculationDto, false, Ust, iscredit);
                        }
                    }
                }
                if (calculationDto.DepotBruttoP == 0 && calculationDto.CalculationSource != ServiceAccess.DdOl.CalculationDto.CalculationSources.DepotP)
                {
                    PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstDepotFieldMeta, calculationDto.SysObArt, false);
                    if (par != null)
                    {
                        if (par.DEFVALP.HasValue)
                        {
                            calculationDto.DepotBruttoP = (decimal)par.DEFVALP;
                            calculationDto.DepotBrutto = KalkulationHelper.CalculateDepot(calculationDto.DepotBruttoP, calculationDto.AnschaffungswertBrutto);
                        }
                    }
                }
                calculationDto.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.Laufzeit;
            }

            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEff || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEffRecursion)
            {
                calculationDto.ZinsNominal = CalculateNominalInterest(calculationDto.ZinsEff, CnstPeriodsPerYear);
            }
           
           
            bool isZielKredit = sysvart == 11;
            bool noRestwert = (sysvart == 15 || sysvart == 7);
            bool hasZielrate = (sysvart == 11);
           

            CalculationMode Mode = CalculationMode.Begin;
           
            if (!iscredit)
                Mode = CalculationMode.Begin;
            else
                Mode = CalculationMode.End;



            _Log.Debug("Kalkulation: UST: " + Ust + " TaxGeb: " + TaxGeb + " KREDIT: " + iscredit + " Mode: " + Mode + " Nova: " + NovaForDepot + " Source: " + calculationDto.CalculationSource.ToString());

            //Kaufpreis
           // calculationDto.KaufpreisNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.KaufpreisBrutto, iscredit?0:Ust);
            calculationDto.KaufpreisUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.KaufpreisBrutto, Ust);

            //MitfinanzierterBestandteil
            calculationDto.MitfinanzierterBestandteilNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.MitfinanzierterBestandteilBrutto, Ust);
            calculationDto.MitfinanzierterBestandteilUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.MitfinanzierterBestandteilBrutto, Ust);

            //Anschaffungswert
            calculationDto.AnschaffungswertNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.AnschaffungswertBrutto, UstErinkl);
            calculationDto.AnschaffungswertUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.AnschaffungswertBrutto, UstErinkl);

            //Mietvorauszahlung
            //updateMVZ(calculationDto, false, Ust);
            //calculationDto.MietvorauszahlungNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.MietvorauszahlungBrutto, Ust);
            //calculationDto.MietvorauszahlungUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.MietvorauszahlungBrutto, Ust);

            //Finanzierungssumme
            calculationDto.RestschuldVersicherung = 0;


            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Mietvorauszahlung || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.MietvorauszahlungP)
            {
                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.MietvorauszahlungP)
                {
                    calculationDto.MietvorauszahlungBrutto = KalkulationHelper.CalculateMietvorauszahlung(calculationDto.MietvorauszahlungBruttoP, calculationDto.KaufpreisBruttoOrg);
                }

                updateMVZ(calculationDto, false, Ust, iscredit);
            }
            else //BMWAT-1291 - case: after a given MVZ and then changing the inputsource to e.g. Laufzeit the user changes the Kaufpreis, then the mvz amount wont be valid (eg 20% of lower KP will not be the valid amount as currently in mvz-amount, that will result in invalid BGINTERN)
            {
                //always assume the percentage is the master-value for MVZ
                calculationDto.MietvorauszahlungBrutto = KalkulationHelper.CalculateMietvorauszahlung(calculationDto.MietvorauszahlungBruttoP, calculationDto.KaufpreisBruttoOrg);
            }
            ValidateMVZ(Context, sysBRAND, sysPEROLE, calculationDto, prparamDao, iscredit, Ust);
            recalcGAP(Context, sysBRAND, sysPEROLE, calculationDto, vs, Ust, iscredit, isUsedCar, NovaAufschlagSatz, UstErinkl);
            updateFinanzierungssumme(calculationDto, Ust, iscredit, NovaAufschlagSatz, isUsedCar, UstErinkl);

            bool initRestwert = false;
            //DepotNetto
            calculationDto.DepotNetto = calculationDto.DepotBrutto;
            if (calculationDto.Lieferdatum == null)
                calculationDto.Lieferdatum = System.DateTime.Now;

            if (!calculationDto.isECOM)
            {
                validateLaufleistung(sysBRAND, sysPEROLE, calculationDto, prparamDao);
                validateLaufzeit(sysBRAND, sysPEROLE, calculationDto, prparamDao);
            }

            //NOVANEU - hier muss die berechnungsgrundlage für die Restwertermittlung noch anders ausgerechnet werden
            nt = new NovaType(Ust, calculationDto.NovaSatz, NovaAufschlagSatz, sonderminderung);
            nt.setBruttoInklNova(calculationDto.ListenpreisBrutto);
            decimal ListenPreisNetto = nt.netto;
            nt = new NovaType(Ust, calculationDto.NovaSatz, NovaAufschlagSatz, 0);
            nt.setBruttoInklNova(calculationDto.SonzubBrutto + calculationDto.PaketeBrutto);
            decimal szNetto = nt.netto;
            #endregion
            _Log.Debug("Duration C " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region RESTWERT

            _Log.Debug("Init RV with " + ListenPreisNetto + "/" + szNetto + "/" + calculationDto.SARVNETTO + "/" +
                calculationDto.NovaBonusMalus + "/" + Ust + "/" + calculationDto.NovaSatz + "/" + sonderminderung);

            rvSuggest.initialize (ListenPreisNetto, szNetto, calculationDto.SARVNETTO, calculationDto.NovaBonusMalus, Ust, calculationDto.NovaSatz, sonderminderung, va.CODE, oa.TYP , oa.SYSOBART, calculationDto.KaufpreisBrutto);
            bool triggerNewRestwert = false;
            bool setRw = false;
            if (!calcSubvention && !calculationDto.skipRWCheck && ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEffRecursion != calculationDto.CalculationSource && ServiceAccess.DdOl.CalculationDto.CalculationSources.RateRecursion != calculationDto.CalculationSource)
            {
                //Restwertvorschlag
                if (noRestwert)
                {
                    calculationDto.RestwertBrutto = 0;
                    calculationDto.RestwertNetto = 0;
                    calculationDto.RestwertUst = 0;
                    calculationDto.RestwertBruttoP = 0;
                    calculationDto.RestwertvorschlagBrutto = 0;
                    calculationDto.RestwertvorschlagNetto = 0;
                    calculationDto.RestwertvorschlagtUst = 0;
                    calculationDto.RestwertvorschlagBruttoP = 0;
                }
                else
                {
                    try
                    {
                        _Log.Debug("RV: Ust: " + Ust + " ListenpreisBrutto: " + calculationDto.ListenpreisBrutto + " Bonus: " + calculationDto.NovaBonusMalus + " ListenpreisNetto: " + ListenPreisNetto + " Sonderzubehoer: " + szNetto + " Laufleistung: " + calculationDto.Laufleistung + " Laufzeit: " + calculationDto.Laufzeit + " ez-Datum: " + calculationDto.Erstzulassungsdatum + " sa3: " + calculationDto.isSA3);
                        rvSuggest.calculateRV(KalkTyp.SYSKALKTYP, calculationDto.SysObTyp, ListenPreisNetto, szNetto, calculationDto.UbNahmeKm, calculationDto.Laufleistung, calculationDto.Laufzeit, calculationDto.Erstzulassungsdatum, calculationDto.Lieferdatum, calculationDto.isSA3, calculationDto.KaufpreisNetto, calculationDto.SARVNETTO,calculationDto.KaufpreisBrutto);
                        _Log.Debug("SF_Base: " + rvSuggest.sfBase + " (" + rvSuggest.sfBaseprozent + "%/" + rvSuggest.sfBaseprozentLP + "%LP) CRV: " + rvSuggest.crv + " (" + rvSuggest.crvprozent + "%/" + rvSuggest.crvprozentLP + "%LP) ");

                        decimal RWVorschlag = rvSuggest.crv > rvSuggest.sfBase ? rvSuggest.crv : rvSuggest.sfBase;//Netto!!!
                        decimal rpercent = rvSuggest.crvprozentLP > rvSuggest.sfBaseprozentLP ? rvSuggest.crvprozentLP : rvSuggest.sfBaseprozentLP;// crvprozent;
                        _Log.Debug("RWVorschlag: " + RWVorschlag + " (" + rpercent + "%)");


                        decimal deckelungRW = calculationDto.AnschaffungswertBrutto+ calculationDto.Verrechnung;
                      
                        deckelungRW = rvSuggest.getRestwertFromBrutto((decimal)deckelungRW).netto;

                        if (RWVorschlag > deckelungRW)//Deckelung
                        {
                            
                            RWVorschlag = deckelungRW;
                            rpercent = rvSuggest.getRestwertPercent(deckelungRW);
                            
                        }


                        //Subvention Restwert--------------------
                        if (!calculationDto.isECOM)
                        {
                            //Die Erhöhung selbst wird über eine Korrekturtabelle ermittelt in der die selben Dimensionen wie in der Restwerttabelle abgetragen werden. 
                            //Die Verbindung zwischen dem Produkt und der Korrekturtabelle erfolgt dahingehend, dass die Bezeichnung der Korrekturtabelle in das Feld "Interner Name" im Produkt einzutragen ist.
                            bool isOverflow = false;
                            decimal subventionRWPercent = rvSuggest.calculateSubvention(calculationDto.SysPrProduct, calculationDto.UbNahmeKm, calculationDto.Laufleistung, calculationDto.Laufzeit, calculationDto.Erstzulassungsdatum, System.DateTime.Now, ref isOverflow);
                            decimal rwcbase = ListenPreisNetto;
                            if (isOverflow)
                                rwcbase = calculationDto.KaufpreisNetto;
                            //die Restwerterhöhungen werden dann  nur auf den CRV aufgeschlagen
                            decimal subventionPercent = rpercent + subventionRWPercent;//rvSuggest.crvprozent + subventionRWPercent;
                            NovaType subvRW = rvSuggest.getRestwertNetto(subventionPercent);
                            decimal subventionRWNetto = subvRW.netto;

                            if (subventionRWNetto > deckelungRW)//Deckelung
                            {
                                subventionRWNetto = deckelungRW;
                                rpercent = rvSuggest.getRestwertPercent(deckelungRW);
                                subvRW = rvSuggest.getValuesFromNetto(subventionRWNetto);
                            }


                            //brutto in subvention:
                            calculationDto.Subvention_Restwert = 0;
                            if (subventionRWPercent > 0)
                            {
                                NovaType subvCRV = rvSuggest.getValuesFromNetto(rvSuggest.crv);

                                calculationDto.Subvention_Restwert = subvRW.bruttoInklNova - subvCRV.bruttoInklNova;
                                //Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(subventionRWNetto, Ust) - Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rvSuggest.crv, Ust);
                                _Log.Debug("Subvention Restwert: " + subventionRWPercent + "% == " + calculationDto.Subvention_Restwert);
                                calculationDto.Subvention_Restwert /= calculationDto.Laufzeit;
                            }
                            if (subventionPercent > rpercent)
                            {
                                RWVorschlag = subventionRWNetto;
                                rpercent = subventionPercent;
                            }
                            //Ende Subvention Restwert



                        }





                        NovaType tmpVSRW = rvSuggest.getValuesFromNetto(RWVorschlag);
                        calculationDto.RestwertvorschlagBrutto = tmpVSRW.bruttoInklNova;
                        calculationDto.RestwertvorschlagtUst = tmpVSRW.ust;
                        calculationDto.RestwertvorschlagNetto = tmpVSRW.netto + tmpVSRW.nova + tmpVSRW.bonusmalusexklaufschlag;//alt:RWVorschlag
                        calculationDto.RestwertvorschlagBruttoP = rpercent;

                        NovaType tmpSFRW = rvSuggest.getValuesFromNetto(rvSuggest.sfBase);
                        calculationDto.sfBaseProzent = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rvSuggest.sfBaseprozentLP);
                        calculationDto.sfBaseBrutto = tmpSFRW.bruttoInklNova;
                        calculationDto.sfBaseNetto = tmpSFRW.netto + tmpSFRW.nova + tmpSFRW.bonusmalusexklaufschlag;//alt: tmpSFRW.netto;
                        calculationDto.sfBaseUst = tmpSFRW.ust;

                        triggerNewRestwert = calculationDto.CrvProzent != Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rvSuggest.crvprozentLP);



                        NovaType tmpCRVRW = rvSuggest.getValuesFromNetto(rvSuggest.crv);
                        calculationDto.CrvProzent = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rvSuggest.crvprozentLP);
                        calculationDto.CrvBrutto = tmpCRVRW.bruttoInklNova;
                        calculationDto.CrvNetto = tmpCRVRW.netto + tmpCRVRW.nova + tmpCRVRW.bonusmalusexklaufschlag;//alt: tmpCRVRW.netto
                        calculationDto.CrvUst = tmpCRVRW.ust;



                        //wenn noch kein restwert oder ein einflussfaktor des restwertvorschlags geändert wird, restwert neu zuweisen
                        if (triggerNewRestwert || (((calculationDto.RestwertBruttoP == 0 || calculationDto.RestwertBrutto == 0) && calculationDto.RestwertBruttoP_Default==0) || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Laufzeit || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Laufleistung))
                        {
                           
                            {
                                PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstRestwertFieldMeta, calculationDto.SysObArt, false);
                                if (par != null && par.DEFVALP.HasValue)
                                {
                                    decimal defRPercent = (decimal)par.DEFVALP;
                                    if (defRPercent >= 0)
                                    {
                                        NovaType tmpDevRW = rvSuggest.getRestwertNetto(defRPercent);
                                        calculationDto.RestwertBruttoP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(defRPercent);
                                        calculationDto.RestwertBrutto = tmpDevRW.bruttoInklNova;
                                        calculationDto.RestwertNetto = tmpDevRW.netto + tmpDevRW.nova + tmpDevRW.bonusmalusexklaufschlag;
                                        calculationDto.RestwertUst = tmpDevRW.ust;
                                        setRw = true;
                                    }
                                }
                            }
                            if (!setRw)
                            {
                                calculationDto.RestwertBrutto = calculationDto.RestwertvorschlagBrutto;
                                calculationDto.RestwertNetto = calculationDto.RestwertvorschlagNetto;
                                calculationDto.RestwertUst = calculationDto.RestwertvorschlagtUst;
                                calculationDto.RestwertBruttoP = calculationDto.RestwertvorschlagBruttoP;
                            }
                            if (calculationDto.DepotBrutto > calculationDto.RestwertvorschlagBrutto)//Deckelung
                                calculationDto.DepotBrutto = calculationDto.RestwertvorschlagNetto;

                            //HCE-5825
                            initRestwert = true;

                        }
                    }
                    catch (System.Exception e)
                    {
                        _Log.Error("RW Vorschlag failed: " + e.Message, e);
                        
                        if (calculationDto.RestwertBrutto == 0)
                            initRestwert = true;
                    }
                }
            }
            #endregion
            _Log.Debug("Duration D " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Validate because of new Restwert
            ValidateAHKMVZ(calculationDto,Ust,iscredit);
            ValidateRW(calculationDto, rvSuggest);
            //Validate MVZ/Depot when LZ was changed but dont cause an error if value was changed
            if (triggerNewRestwert || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Laufzeit)
            {
                ///// AnschaffungswertNetto,RestwertBrutto,RestwertNetto,MietvorauszahlungBrutto,DepotBrutto
                ValidateMVZ(Context, sysBRAND, sysPEROLE, calculationDto, prparamDao, iscredit, Ust);
                
                if (calculationDto.MessageCode == ServiceAccess.DdOl.CalculationDto.MessageCodes.Dpt001
                    || calculationDto.MessageCode == ServiceAccess.DdOl.CalculationDto.MessageCodes.Dpt002
                  || calculationDto.MessageCode == ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001)
                {
                    calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.NoError;
                    calculationDto.Message = "";
                }
            }
            #endregion
            _Log.Debug("Duration E " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Ratenberechnung Var Deklaration
            //Parameters for rate calculation
            decimal Base;
            decimal FirstPayment;
            decimal Term;
            decimal Rate;
            decimal RateInput;
            decimal ResidualValue;
            decimal ZinsEff;

            //Base = calculationDto.FinanzierungssummeBrutto;
            FirstPayment = 0;
            Term = calculationDto.Laufzeit;
            Rate = calculationDto.MonatlicheRate;
            decimal userInputRate = calculationDto.MonatlicheRate;
            RateInput = calculationDto.MonatlicheRate;
            ResidualValue = calculationDto.RestwertBrutto;
            ZinsEff = Interest;

            calculationDto.AbschlussProvision = new AbschlussProvisionDto();
            calculationDto.AbschlussProvision.ProvisionValue = 0;
            calculationDto.AbschlussProvision.BasicPrice = 0;
            calculationDto.AbschlussProvision.ProvisionPercentage = 0;
            // Create zugang provision
            calculationDto.ZugangProvision = new ZugangProvisionDto();
            calculationDto.ZugangProvision.ProvisionValue = 0;
            calculationDto.ZugangProvision.BasicPrice = 0;
            calculationDto.ZugangProvision.ProvisionPercentage = 0;
            #endregion
            _Log.Debug("Duration F " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Validierung erneut
            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Mietvorauszahlung || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.MietvorauszahlungP)
            {
                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.MietvorauszahlungP)
                {
                    calculationDto.MietvorauszahlungBrutto = KalkulationHelper.CalculateMietvorauszahlung(calculationDto.MietvorauszahlungBruttoP, calculationDto.KaufpreisBruttoOrg);
                }
                updateMVZ(calculationDto, false, Ust, iscredit);
                ValidateMVZ(Context, sysBRAND, sysPEROLE, calculationDto, prparamDao, iscredit, Ust);
            
            }
            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Depot || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.DepotP)
            {

                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.DepotP)
                {
                    //Calculate DepotBrutto
                    calculationDto.DepotBrutto = KalkulationHelper.CalculateDepot(calculationDto.DepotBruttoP, calculationDto.AnschaffungswertBrutto);
                }
                else
                    calculationDto.DepotBruttoP = KalkulationHelper.CalculateDepotP(calculationDto.DepotBrutto, calculationDto.AnschaffungswertBrutto);

                if (calculationDto.DepotBrutto > Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.RestwertBrutto, Ust))//Deckelung
                {
                    calculationDto.DepotBrutto = calculationDto.RestwertNetto;
                    calculationDto.DepotBruttoP = KalkulationHelper.CalculateDepotP(calculationDto.DepotBrutto, calculationDto.AnschaffungswertBrutto);
                }
                

            }
            #endregion
            _Log.Debug("Duration G " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            //Restkaufpreis - ohne RSV
            calculationDto.Restkaufpreis = calculationDto.FinanzierungssummeBrutto;
            recalcRSV(Context, sysBRAND, sysPEROLE, calculationDto, vs, Ust, iscredit, isUsedCar, NovaAufschlagSatz, UstErinkl);
            if (calculationDto.Bearbeitungsgebuehr == 0)
                calculateBearbeitungsgebuehr(sysBRAND, sysPEROLE, calculationDto, gebuehrDao);

            _Log.Debug("Duration H " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            //Provisionsberechnung--------------------------------------------------------------------
            /*
             * Provisions
             *  dependent of
             *   calculationDto.AnschaffungswertNetto - calculationDto.MietvorauszahlungNetto + calculationDto.Verrechnung - calculationDto.DepotBrutto;
             *  impact on
             *    Zins
             */
            #region Provisions

            decimal finchange = 0;
           
            #endregion
            _Log.Debug("Duration I " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;


            updateFinanzierungssumme(calculationDto, Ust, iscredit, NovaAufschlagSatz, isUsedCar, UstErinkl);

            #region Ratenberechnungen
            Base = calculationDto.FinanzierungssummeBrutto;
            FirstPayment = 0;
            //Mietvorauszahlung, changes FinanzierungssummeBrutto
            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Mietvorauszahlung || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.MietvorauszahlungP)
            {


                Base = calculationDto.FinanzierungssummeBrutto;
                FirstPayment = 0;

                calcRate(calculationDto, ref Interest, Ust, SysINTSTRCT, Mode, ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref ZinsEff, true, iscredit, depotZins);
                updateFinanzierungssumme(calculationDto, Ust, iscredit, NovaAufschlagSatz, isUsedCar, UstErinkl);

                Base = calculationDto.FinanzierungssummeBrutto;
                FirstPayment = 0;
            }
            //Depot
            else if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Depot || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.DepotP)
            {

                Base = calculationDto.FinanzierungssummeBrutto;
                FirstPayment = 0;

                calcRate(calculationDto, ref Interest, Ust, SysINTSTRCT, Mode, ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref ZinsEff, true, iscredit, depotZins);

            }

            //ZinsEffRecurse
            else if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEffRecursion)
            {
                //Calculate restwert values

                Interest = CalculateNominalInterest(calculationDto.ZinsEff, CnstPeriodsPerYear);

                Base = calculationDto.FinanzierungssummeBrutto;
                FirstPayment = 0;

                if (finchange != 0)
                {
                    //Pflichtenheft 4.3.9.1
                    //Aufaddieren der beiden Beträge(abschluss+sfAufschlag) auf den Barwert (Finanzierungssumme) wenn NICHT Standardprovision

                    decimal FinanzierungssummeNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(Base, Ust) + finchange;
                    Base = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(FinanzierungssummeNetto, Ust);

                }

                calcRate(calculationDto, ref Interest, Ust, SysINTSTRCT, Mode, ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref ZinsEff, false, iscredit, depotZins);
                calculationDto.Zins = Interest;
            }
           /* //RateRekursion
            else if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.RateRecursion)
            {
                //Calculate restwert values
                Interest = CalculateNominalInterest(calculationDto.ZinsEff, CnstPeriodsPerYear);
                Base = calculationDto.FinanzierungssummeBrutto;
                FirstPayment = 0;
                if (finchange != 0)
                {
                    //Pflichtenheft 4.3.9.1
                    //Aufaddieren der beiden Beträge(abschluss+sfAufschlag) auf den Barwert (Finanzierungssumme) wenn NICHT Standardprovision
                    decimal FinanzierungssummeNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(Base, Ust) + finchange;
                    Base = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(FinanzierungssummeNetto, Ust);
                }
               
                calcRate(calculationDto, ref Interest, Ust, SysINTSTRCT, Mode, ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref ZinsEff, false, iscredit, depotZins);
                calculationDto.Zins = Interest;
            }*/


            //ZinsNominal
            else if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsNominal)
            {
                //Calculate restwert values

                Interest = calculationDto.ZinsNominal;
                calculationDto.Zins = Interest;

                Base = calculationDto.FinanzierungssummeBrutto;

             

                FirstPayment = 0;

                calcRate(calculationDto, ref Interest, Ust, SysINTSTRCT, Mode, ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref ZinsEff, false, iscredit, depotZins);

            }

           

            //Restwert
            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Restwert || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.RestwertP || initRestwert)
            {
                #region RW Validation
                bool pouvoirAllowed = true;//if the pouvoir is applied
                bool changeAllowed = true;//if a change of restwert is allowed at all

                //min/max borders mathematical:
                decimal maxPercent = 100;
                decimal minPercent = 1;

                String maxMsg = "Restwert wurde auf zulässigen Maximalwert reduziert!";
                String minMsg = "Restwert wurde auf zulässigen Minimalwert erhöht!";
                PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstRestwertFieldMeta, calculationDto.SysObArt, false);
                decimal minProduct = 1;
                decimal maxProduct = 100;

                if (par != null && par.MINVALP.HasValue)
                {
                    minPercent = (decimal)par.MINVALP;
                    minProduct = minPercent;
                }
                if (par != null && par.MAXVALP.HasValue)
                {
                    maxPercent = (decimal)par.MAXVALP;
                    maxProduct = maxPercent;
                }
                bool isactionrw = false;
                if (par != null && par.DEFVALP.HasValue)
                {
                    isactionrw = (minProduct == maxProduct) && (minProduct == par.DEFVALP.Value);
                }

                


                decimal maxRestwertNetto = rvSuggest.getRestwertNetto(maxPercent).netto;//ListenPreisNetto, maxPercent, szNetto);
                
                //never ever the rw percent might be higher than this, its the cars value!


                decimal deckelungRW = calculationDto.AnschaffungswertBrutto + calculationDto.Verrechnung;               
                deckelungRW = rvSuggest.getRestwertFromBrutto((decimal)deckelungRW).netto;

                decimal maxPercentAHK = rvSuggest.getRestwertPercent(deckelungRW); 
               
                if (!changeAllowed)
                {
                    maxPercent = calculationDto.CrvProzent > calculationDto.sfBaseProzent ? calculationDto.CrvProzent : calculationDto.sfBaseProzent;
                    minPercent = maxPercent;
                }
                if (isactionrw)
                {
                    changeAllowed = false;
                    pouvoirAllowed = false;
                    minPercent = minProduct;
                    maxPercent = maxProduct;
                }
                _Log.Debug("Restwert-Änderung: changeAllowed: " + changeAllowed + " applyPouvoirs: " + pouvoirAllowed + " minPercent: " + minPercent + " maxPercent: " + maxPercent + " iM: " + calculationDto.isIM + " kalkTyp: " + KalkTyp.SYSKALKTYP);

                bool restwertgarantie = calculationDto.restwertgarantie && calculationDto.isIM;

                //----------Pouvoir Restwert ----------------------------------
                if (pouvoirAllowed)//bezieht sich immer auf den höheren aus SF-Base und CRV
                {
                    //aktueller Vorschlag ist in maxPercent/minPercent
                    maxPercent = calculationDto.CrvProzent > calculationDto.sfBaseProzent ? calculationDto.CrvProzent : calculationDto.sfBaseProzent;
                    minPercent = maxPercent;

                    if (restwertgarantie)
                    {
                        //keine Senkung erlaubt
                        maxPercent = maxProduct;
                    }
                    else
                    {
                        decimal cpercent = maxPercent;// calculationDto.CrvProzent - calculationDto.sfBaseProzent;
                        if (cpercent < 0) cpercent = 0;

                      
                        
                        PRPARAMDto parRWP = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstRestwertPFieldMeta, calculationDto.SysObArt, false);


                        if (parRWP != null)//Abweichungsparameter vorhanden, prüfen
                        {

                            decimal userMaxPercent = (parRWP.MAXVALP.HasValue ? (decimal)parRWP.MAXVALP : 0);
                            decimal userMinPercent = (parRWP.MINVALP.HasValue ? (decimal)parRWP.MINVALP : 0);

                            minPercent = cpercent + userMinPercent;// (cpercent / 100.0M * userMinPercent); //assumed a negative minPercent value!
                            maxPercent = cpercent + userMaxPercent;// (cpercent / 100.0M * userMaxPercent);

                            //Produkt RWKALK gilt immer
                            if (minPercent < minProduct) minPercent = minProduct;
                            if (maxPercent > maxProduct) maxPercent = maxProduct;

                            _Log.Debug("Percentage Difference Restwert: Max: " + parRWP.MAXVALP + " Min: " + parRWP.MINVALP + " Max Netto RW: " + maxRestwertNetto + " max %: " + maxPercent);
                        }
                        else _Log.Debug("No RW Percentage Difference defined for Product " + calculationDto.SysPrProduct + " Field " + PRParamDao.CnstRestwertFieldMeta);
                    }
                }

                //end Pouvoir Restwert-----------------------------------------------------



                //Calculate restwert values

                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Restwert)
                {
                    calculationDto.RestwertBruttoP = rvSuggest.getRestwertPercentFromBrutto(calculationDto.RestwertBrutto);
                    calculationDto.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.RestwertP;
                }
                decimal orgPercent = calculationDto.RestwertBruttoP;
                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.RestwertP || initRestwert)
                {
                    decimal percent = calculationDto.RestwertBruttoP;

                    if (!calculationDto.skipRWCheck)
                    {
                        if (maxPercent > maxPercentAHK)//avoid a rw higher than ahk
                            maxPercent = maxPercentAHK;
                        if (minPercent > maxPercentAHK)//avoid a rw higher than ahk
                            minPercent = maxPercentAHK;

                        if (percent > maxPercent)
                        {
                            setPouvoir(calculationDto, minPercent, maxPercent, percent);
                            percent = maxPercent;
                            calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Rw002;
                            calculationDto.Message = maxMsg + System.Environment.NewLine;

                        }
                        if (percent < minPercent)
                        {
                            setPouvoir(calculationDto, minPercent, maxPercent, percent);
                            percent = minPercent;
                            calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Rw002;
                            calculationDto.Message = minMsg + System.Environment.NewLine;

                        }

                        calculationDto.RestwertBrutto = rvSuggest.getRestwertNetto(percent).bruttoInklNova;
                        calculationDto.RestwertBruttoP = percent;
                    }
                }

                NovaType tempRwValues = rvSuggest.getRestwertFromBrutto(calculationDto.RestwertBrutto);
                calculationDto.RestwertNetto = tempRwValues.netto + tempRwValues.nova + tempRwValues.bonusmalusexklaufschlag; //tempRwValues.netto;
                calculationDto.RestwertUst = tempRwValues.ust;


                //Falls kein Restwert bzw. am Produkt 0 eingestellt ist, den RW-Vorschlag setzen
                if (calculationDto.RestwertBrutto <= 0 || calculationDto.RestwertBruttoP <= 0)
                {
                    if (!calculationDto.skipRWCheck)
                    {
                        calculationDto.RestwertBrutto = calculationDto.RestwertvorschlagBrutto;
                        calculationDto.RestwertNetto = calculationDto.RestwertvorschlagNetto;
                        calculationDto.RestwertUst = calculationDto.RestwertvorschlagtUst;
                        calculationDto.RestwertBruttoP = calculationDto.RestwertvorschlagBruttoP;
                    }
                }
                if (orgPercent == calculationDto.RestwertBruttoP)//wenn gewünschter RW zwar zu einer Meldung führt,aber das ergebnis wegen der RW-Vorschlags-Regel dem eingegebenen entspricht, dann keine Meldung bringen
                {
                    calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.NoError;
                    calculationDto.Message = "";
                }
                #endregion
                //check mvz/Depot
                ValidateAHKMVZ(calculationDto,Ust,iscredit);//mvz may be too high now
         

                //Change parameter for calculate rate
                ResidualValue = calculationDto.RestwertBrutto;

                Base = calculationDto.FinanzierungssummeBrutto;
                FirstPayment = 0;

                calcRate(calculationDto, ref Interest, Ust, SysINTSTRCT, Mode, ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref ZinsEff, true, iscredit, depotZins);
            }


            //Laufzeit
            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Laufzeit || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.RGGebuehr || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEff || calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Wunschprovision)//Eff because of calculating original rate when changing eff
            {

                NovaType tempRwValues = rvSuggest.getRestwertFromBrutto(calculationDto.RestwertBrutto);
                calculationDto.RestwertNetto = tempRwValues.netto + tempRwValues.nova + tempRwValues.bonusmalusexklaufschlag; //tempRwValues.netto;
                calculationDto.RestwertUst = tempRwValues.ust;



                //Change parameter for calculate rate
                if (!setRw)
                {
                    ResidualValue = KalkulationHelper.ChangeResiudalValue(calculationDto.RestwertBrutto, calculationDto.RestwertvorschlagBrutto);
                    calculationDto.RestwertBrutto = ResidualValue;
                }

                

                Base = calculationDto.FinanzierungssummeBrutto;
                FirstPayment = 0;
                if (!zinsPouvoirCheck)//dont call when having a rate for effzins pouvoir check
                    calcRate(calculationDto, ref Interest, Ust, SysINTSTRCT, Mode, ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref ZinsEff, true, iscredit, depotZins);

            }

            //Laufleistung
            else if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Laufleistung)
            {

                NovaType tempRwValues = rvSuggest.getRestwertFromBrutto(calculationDto.RestwertBrutto);
                calculationDto.RestwertNetto = tempRwValues.netto + tempRwValues.nova + tempRwValues.bonusmalusexklaufschlag; //tempRwValues.netto;
                calculationDto.RestwertUst = tempRwValues.ust;


                //Change parameter for calculate rate
                if (!setRw)
                {
                    ResidualValue = KalkulationHelper.ChangeResiudalValue(calculationDto.RestwertBrutto, calculationDto.RestwertvorschlagBrutto);
                    calculationDto.RestwertBrutto = ResidualValue;
                }

                calculationDto.MonatlicheRate = KalkulationHelper.CalculateRateWithVorteil(calculationDto.DepotBrutto, calculationDto.Laufzeit, Base, calculationDto.MonatlicheRate, Ust, SysINTSTRCT, depotZins);

                Base = calculationDto.FinanzierungssummeBrutto;
                FirstPayment = 0;

                calcRate(calculationDto, ref Interest, Ust, SysINTSTRCT, Mode, ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref ZinsEff, true, iscredit, depotZins);

            }
            #endregion

           
            //Bearbeitungsgebuehr
            _Log.Debug("Duration J " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Gebührberechnung
           
            decimal geb = 0;// calculationDto.Bearbeitungsgebuehr;

            
            calculationDto.RgGebBrutto = 0;
            calculationDto.RgGebNetto = 0;
            calculationDto.RgGebUst = 0;
            calculationDto.RgGeb_Default = 0;
            calculationDto.Subvention_RGG = 0;
           
            #endregion
            _Log.Debug("Duration K " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Zinsberechnung
            decimal nominalzins = Interest;
            _Log.Debug("Zins Nominal: " + Interest + " Eff: " + calculationDto.ZinsEff);
            //if (iscredit)
            {
                //-(Anschaffungswert-MVZ  -Teilentgelt+     RGGmitfin-RGG1.rate+rsv    -BGG+Vorkredit)
                try
                {
                    _Log.Debug("Kreditzins effektiv old: " + CalculateEffectiveInterest(nominalzins, CnstPeriodsPerYear));
                    decimal mitfin = calculationDto.RestschuldVersicherung + calculationDto.GAPVersicherung;
                    if (!iscredit)
                        mitfin = 0;
                    double zbase = (double)(mitfin+calculationDto.AnschaffungswertBrutto  - calculationDto.MietvorauszahlungBrutto + calculationDto.Verrechnung);
                    double mrate = iscredit ? (double)calculationDto.MonatlicheRateKredit : (double)calculationDto.MonatlicheRate;
                    mrate = (double)calculationDto.MonatlicheRate;
                    decimal nominalzins2 = (decimal)((12.0 * Kalkulator.calcZINS(zbase, mrate, (double)calculationDto.Laufzeit, (double)(calculationDto.RestwertBrutto), iscredit ? Kalkulator.ZAHLMODUS_NACHSCHUESSIG : Kalkulator.ZAHLMODUS_VORSCHUESSIG)));

                    _Log.Debug("Kreditzins effektiv new: " + CalculateEffectiveInterest(nominalzins2, CnstPeriodsPerYear));
                    nominalzins = nominalzins2;

                    decimal rggmitfin = calculationDto.RgGebVersion == ServiceAccess.RgGebVersion.Mitfinanziert ? calculationDto.RgGebBrutto : 0;
                    double zinsbasis = (double)(mitfin+calculationDto.AnschaffungswertBrutto - calculationDto.MietvorauszahlungBrutto + calculationDto.Verrechnung);
                    calculationDto.Zins2 = (decimal)((12.0 * Kalkulator.calcZINS(zinsbasis, mrate, (double)calculationDto.Laufzeit, (double)(calculationDto.RestwertBrutto), iscredit ? Kalkulator.ZAHLMODUS_NACHSCHUESSIG : Kalkulator.ZAHLMODUS_VORSCHUESSIG)));
                }
                catch (Exception ex)
                {
                    _Log.Warn("Zinscalc failed: "+ex.Message);
                    //_Log.Error("Calculate called with:" + System.Environment.NewLine + _Log.dumpObject(calculationDto));
                }
            }
            calculationDto.ZinsEff_Default = CalculateEffectiveInterest(calculationDto.ZinsNominal_Default, CnstPeriodsPerYear);
            calculationDto.ZinsEff = CalculateEffectiveInterest(nominalzins, CnstPeriodsPerYear);
            calculationDto.ZinsNominal = nominalzins;
            if (calculationDto.ZinsEff > 99.0M)
                calculationDto.ZinsEff = 99.0M;
            _Log.Debug("Final Zins Nominal: " + Interest + " Eff: " + calculationDto.ZinsEff);
            #endregion
            _Log.Debug("Duration L " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Ergebniszuweisung und Rundung
            

            calculationDto = KalkulationHelper.CalculatePricesFromPercent(calculationDto, iscredit?0:Ust);

            //damit leasing/kredit rate gleich bleibt beim nettorechner immer ohne diesen aufschlag als berechnungsgrundlage die rate kalkulieren
            //calculationDto.AnschaffungswertBrutto -= calculationDto.NovaAufschlag;
            calculationDto.MitfinanzierterBestandteilBrutto = 0;
            if (calculationDto.hasRSV && iscredit && calculationDto.IgnoreInsurances < 1)//&& (calculationDto.SubventionCalcMode < 2||calculationDto.SubventionCalcMode >3)
                calculationDto.MitfinanzierterBestandteilBrutto += calculationDto.RestschuldVersicherung;
            if (calculationDto.hasGAP && iscredit && calculationDto.IgnoreInsurances < 1)//&& (calculationDto.SubventionCalcMode < 2||calculationDto.SubventionCalcMode >3)
            {
                calculationDto.MitfinanzierterBestandteilBrutto += calculationDto.GAPVersicherung;
            }

            //Round values
            calculationDto.AnschaffungswertBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.AnschaffungswertBrutto);
            calculationDto.AnschaffungswertNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.AnschaffungswertNetto);
            calculationDto.AnschaffungswertUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.AnschaffungswertBrutto - calculationDto.AnschaffungswertNetto);

            calculationDto.DepotBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.DepotBrutto);
            calculationDto.DepotBruttoP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(calculationDto.DepotBruttoP);

            calculationDto.FinanzierungssummeBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.FinanzierungssummeBrutto);
            calculationDto.FinanzierungssummeNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.FinanzierungssummeNetto);
            calculationDto.FinanzierungssummeUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.FinanzierungssummeUst);

            calculationDto.Restkaufpreis = calculationDto.FinanzierungssummeBrutto - calculationDto.RestschuldVersicherung;

            calculationDto.KaufpreisBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.KaufpreisBrutto);
            calculationDto.KaufpreisNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.KaufpreisNetto);
            calculationDto.KaufpreisUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.KaufpreisUst);


            updateMVZ(calculationDto, false, Ust,iscredit);

            calculationDto.MietvorauszahlungBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MietvorauszahlungBrutto);
            calculationDto.MietvorauszahlungBruttoP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(calculationDto.MietvorauszahlungBruttoP);
            calculationDto.MietvorauszahlungNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MietvorauszahlungNetto);
            calculationDto.MietvorauszahlungUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MietvorauszahlungUst);
            

            calculationDto.MitfinanzierterBestandteilBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MitfinanzierterBestandteilBrutto);
            calculationDto.MitfinanzierterBestandteilNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MitfinanzierterBestandteilNetto);
            calculationDto.MitfinanzierterBestandteilUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MitfinanzierterBestandteilUst);

            calculationDto.MonatlicheRateRaw = calculationDto.MonatlicheRate;
            calculationDto.MonatlicheRate = Cic.OpenLease.Service.RoundingFacade.getInstance().CutPrice(calculationDto.MonatlicheRate);

            //Calculate Rate Netto and Ust
            calculationDto.MonatlicheRateNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.MonatlicheRate, iscredit ? 0 : Ust);
            calculationDto.MonatlicheRateUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.MonatlicheRate, iscredit ? 0 : Ust);


            calculationDto.MonatlicheRateNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MonatlicheRateNetto);
            calculationDto.MonatlicheRateUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MonatlicheRateUst);


            NovaType tempRwVal = rvSuggest.getRestwertFromBrutto(calculationDto.RestwertBrutto);
            calculationDto.RestwertNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.RestwertBrutto, Ust); ;// tempRwVal.netto + tempRwVal.nova + tempRwVal.bonusmalusexklaufschlag; //tempRwValues.netto;//tempRwVal.netto;
            calculationDto.RestwertUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.RestwertBrutto, Ust);//tempRwVal.ust;
            if (iscredit)//Fix #4280
            {
                calculationDto.RestwertNetto = calculationDto.RestwertBrutto;
                calculationDto.RestwertUst = 0;
            }

            if (calcInfoSubv != null)
            {
                calcInfoSubv.RestwertBrutto = calculationDto.RestwertBrutto;
                calcInfoSubv.RestwertvorschlagBrutto = calculationDto.RestwertvorschlagBrutto;
            }
            calculationDto.RestwertBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RestwertBrutto);
            calculationDto.RestwertBruttoP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(calculationDto.RestwertBruttoP);
            calculationDto.RestwertNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RestwertNetto);
            calculationDto.RestwertUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RestwertUst);

            calculationDto.RestwertvorschlagBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RestwertvorschlagBrutto);
            calculationDto.RestwertvorschlagBruttoP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(calculationDto.RestwertvorschlagBruttoP);
            calculationDto.RestwertvorschlagNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RestwertvorschlagNetto);
            calculationDto.RestwertvorschlagtUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RestwertvorschlagtUst);



            calculationDto.RgGebBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RgGebBrutto);
            calculationDto.RgGebNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RgGebNetto);
            calculationDto.RgGebUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RgGebUst);

            calculationDto.Zins = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(calculationDto.Zins);
            calculationDto.Restkaufpreis = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.Restkaufpreis);
            calculationDto.ZinsEff = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(calculationDto.ZinsEff);
            calculationDto.Kreditbetrag = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.Kreditbetrag);


            //Calculate GesamtKosten
            decimal mrategk = calculationDto.MonatlicheRate;
            decimal bwfehler = (calculationDto.MonatlicheRateRaw - mrategk) * calculationDto.Laufzeit;
            calculationDto.GesamtKostenBrutto = bwfehler+KalkulationHelper.CalculateGesamtKosten(calculationDto.MietvorauszahlungBrutto, ref mrategk, calculationDto.Laufzeit, calculationDto.RestwertBrutto, geb, calculationDto.FinanzierungssummeBrutto, calculationDto.RestschuldVersicherung, calculationDto.GAPVersicherung);
            calculationDto.MonatlicheRate = mrategk;
          //  if(calculationDto.ANGKALKBWFEHLER<=0.0001M)
                calculationDto.ANGKALKBWFEHLER = bwfehler;
            calculationDto.MonatlicheRateNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.MonatlicheRate, iscredit?0:Ust);
            calculationDto.MonatlicheRateUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.MonatlicheRate, iscredit ? 0 : Ust);
            calculationDto.MonatlicheRateNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MonatlicheRateNetto);
            calculationDto.MonatlicheRateUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MonatlicheRateUst);

            calculationDto.GesamtKostenNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.GesamtKostenBrutto, Ust);
            calculationDto.GesamtKostenUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.GesamtKostenBrutto, Ust);


            calculationDto.GesamtbelastungBrutto = calculationDto.GesamtKostenBrutto + calculationDto.FinanzierungssummeBrutto;
            calculationDto.GesamtbelastungNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.GesamtbelastungBrutto, Ust);
            calculationDto.GesamtbelastungUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(calculationDto.GesamtbelastungBrutto, Ust);


            calculationDto.GesamtKostenBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.GesamtKostenBrutto);
            calculationDto.GesamtKostenNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.GesamtKostenNetto);
            calculationDto.GesamtKostenUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.GesamtKostenUst);

            calculationDto.GesamtbelastungBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.GesamtbelastungBrutto);
            calculationDto.GesamtbelastungNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.GesamtbelastungNetto);
            calculationDto.GesamtbelastungUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.GesamtbelastungUst);


            // Kreditbetrag
            calculationDto.Kreditbetrag = calculationDto.GesamtKostenBrutto + calculationDto.FinanzierungssummeBrutto - calculationDto.RestschuldVersicherung;
            #endregion
            _Log.Debug("Duration M " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
            #region Zinseffektiv-Änderung-Rekursion
            //ZinsEff - recursive call
            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEff)
            {


                decimal targetZinsIn = targetZinsEff;
                Cic.OpenLease.ServiceAccess.DdOl.CalculationDto.MessageCodes tmpCode = calculationDto.MessageCode;
                String tmpMsg = calculationDto.Message;

                PRPARAMDto pEff = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstZinsEffFieldMeta, calculationDto.SysObArt, false);
                PRPARAMDto pNom = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstZinsNominalFieldMeta, calculationDto.SysObArt, false);
                if(pEff!=null)
                    targetZinsEff = getZinsPouvoir(targetZinsIn, calculationDto.Zinssatz, PRParamDao.CnstZinsEffFieldMeta, sysBRAND, sysPEROLE, calculationDto, prparamDao);
                else if(pNom!=null)
                {
                    //we have to check zinseffborder via zinsnominal-border
                    decimal ctgtnom = CalculateNominalInterest(targetZinsIn, CnstPeriodsPerYear);
                    ctgtnom = getZinsPouvoir(ctgtnom, calculationDto.ZinsNominal, PRParamDao.CnstZinsNominalFieldMeta, sysBRAND, sysPEROLE, calculationDto, prparamDao);
                    //now we have the nominal inside correct bounds, convert back to eff:
                    targetZinsEff = CalculateEffectiveInterest(ctgtnom, CnstPeriodsPerYear);
                }
                
                
                
                if (zinsPouvoirCheck)
                {
                    targetZinsEff = targetZinsIn;
                }
                else
                {
                    tmpCode = calculationDto.MessageCode;
                    tmpMsg = calculationDto.Message;
                }
                decimal zinsdif = 10000;
                decimal mrate = calculationDto.MonatlicheRate;
                int count = 0;
                CalculationDto calcInfo = calculationDto;
                while (Math.Abs(zinsdif) > 0.000001M && count < 10)
                {
                    calcInfo = ObjectCloner.Clone(calculationDto);
                    calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEffRecursion;
                    calcInfo.ZinsEff = targetZinsIn;
                    calcInfo.MonatlicheRate = mrate;
                    
                    calcInfo = Calculate(Context, sysBRAND, sysPEROLE, calcInfo, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                    zinsdif = targetZinsEff - calcInfo.ZinsEff;
                    targetZinsIn = targetZinsIn + zinsdif;
                    mrate = calcInfo.MonatlicheRate;
                    count++;
                }
                //at least once again to update monthly rate for the final value to calculate LRV
                calcInfo = ObjectCloner.Clone(calculationDto);
                calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEffRecursion;
                calcInfo.ZinsEff = targetZinsIn;
                calcInfo.MonatlicheRate = mrate;

                calcInfo = Calculate(Context, sysBRAND, sysPEROLE, calcInfo, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                zinsdif = targetZinsEff - calcInfo.ZinsEff;
                targetZinsIn = targetZinsIn + zinsdif;
                mrate = calcInfo.MonatlicheRate;
                

                calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEff;
                calculationDto = calcInfo;
                calculationDto.Message = tmpMsg;
                calculationDto.MessageCode = tmpCode;
            }
            #endregion

            #region Rate-Änderung-Rekursion
            //Rateänderung - recursive call
            if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.Rate)
            {
                decimal bound = 0;
                PRPARAMDto pRate = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstRateMin , calculationDto.SysObArt, false);
                if (checkParam(pRate, wunschRate, ref bound))
                {
                    calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                    calculationDto.Message = "Die monatliche Rate wurde aufgrund der Unter-/Überschreitung der minimalen/maximalen Rate auf den nächstmöglichen Wert angepasst!" + System.Environment.NewLine;
                    calculationDto.MonatlicheRate = bound;
                    wunschRate = bound;
                }
                

                //remember original values
                Cic.OpenLease.ServiceAccess.DdOl.CalculationDto.MessageCodes tmpCode = calculationDto.MessageCode;
                String tmpMsg = "";// calculationDto.Message;
                tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.NoError;


                decimal addDiff = calculationDto.KaufpreisBrutto;//maximal suitable value for sz/rw/rabatt in ANY CASE
                if (!iscredit)
                    addDiff = calculationDto.KaufpreisNetto;

                if (calculationDto.MonatlicheRateCalced < wunschRate)//rate wird erhöht, initialen Input invertieren
                    addDiff *= -1;

                decimal iterationInput = 0;
                if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Mietvorauszahlung)
                {
                    iterationInput = calculationDto.MietvorauszahlungBrutto;
                }
                else if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Restwert)
                {
                    iterationInput = calculationDto.RestwertBrutto;
                }
                else if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Rabatt)
                {
                    iterationInput = calculationDto.KaufpreisBrutto;
                    if(!iscredit)
                        iterationInput = calculationDto.KaufpreisNetto;
                }

                decimal rabattmax = calculationDto.ANGOBGRUNDEXTERN;
                decimal kaufpreisMax = calculationDto.ANGOBAHKEXTERNBRUTTO;//bei min rabatt
                decimal kaufpreisMin = calculationDto.ANGOBAHKEXTERNBRUTTO-rabattmax;//bei max rabatt
                
                if(!iscredit)
                {
                    rabattmax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.ANGOBGRUNDEXTERN, UstErinkl);
                    kaufpreisMax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.ANGOBAHKEXTERNBRUTTO, UstErinkl);
                    kaufpreisMin = kaufpreisMax - rabattmax;
                }


                decimal originalInput = iterationInput;
                decimal iterationInputOld = iterationInput;
                
                int count = 0;
                decimal wdiff = 10, lastwdiff=0;
                bool iterate = false;
                CalculationDto calcInfo = calculationDto;
                //test if wunschrate already suits the current values
                calcInfo = ObjectCloner.Clone(calculationDto);
                calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.Mietvorauszahlung;
                calcInfo = Calculate(Context, sysBRAND, sysPEROLE, calcInfo, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                //do not iterate when value already ok
                if (Math.Abs(calcInfo.MonatlicheRate - wunschRate) > 0.01M)
                    iterate = true;

                int MAXITER = 18;
                while (iterate)
                {
                    calcInfo = ObjectCloner.Clone(calculationDto);
                    
                    
                    if(calculationDto.CalculationTarget==CalculationDto.CalculationTargets.Mietvorauszahlung)
                    {
                        calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.Mietvorauszahlung;
                        iterationInput += addDiff;//wenn der Wert steigt, sinkt die Rate
                        if (iterationInput < 0)
                            iterationInput = 0;
                        if (iterationInput > calculationDto.KaufpreisBrutto)//kann nie rabattierfähigen Betrag übersteigen
                            iterationInput = calculationDto.KaufpreisBrutto;
                        iterationInput = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(iterationInput);
                        calcInfo.MietvorauszahlungBrutto = iterationInput;
                        calcInfo.skipMVZCheck = true;


                    }
                    else if (calculationDto.CalculationTarget==CalculationDto.CalculationTargets.Restwert)
                    {
                        calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.Restwert;
                        iterationInput += addDiff;//wenn der Wert steigt, sinkt die Rate
                        if (iterationInput < 0)
                            iterationInput = 0;
                        if (iterationInput > calculationDto.KaufpreisBrutto)
                            iterationInput = calculationDto.KaufpreisBrutto;
                        iterationInput = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(iterationInput);
                        calcInfo.RestwertBrutto = iterationInput;
                        calcInfo.skipRWCheck = true;
                         
                    }
                    else if(calculationDto.CalculationTarget==CalculationDto.CalculationTargets.Rabatt)
                    {
                        calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.Mietvorauszahlung;
                        iterationInput -= addDiff;//wenn der Wert sinkt, sinkt die Rate
                        if (iterationInput < kaufpreisMin)
                            iterationInput = kaufpreisMin;
                        if (iterationInput > kaufpreisMax)
                            iterationInput = kaufpreisMax;
                        iterationInput = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(iterationInput);

                        if (!iscredit)
                        {
                            calcInfo.KaufpreisNetto = iterationInput;
                            calcInfo.KaufpreisBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calcInfo.KaufpreisNetto, UstErinkl);
                        }
                        else
                            calcInfo.KaufpreisBrutto = iterationInput;
                        calcInfo.skipMVZCheck = true;

                    }

                    calcInfo.useNettoSource = false;
                    calcInfo = Calculate(Context, sysBRAND, sysPEROLE, calcInfo, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                    addDiff = Math.Abs(iterationInputOld - iterationInput)/2.0M;
                    iterationInputOld = iterationInput;
                    wdiff = Math.Abs(calcInfo.MonatlicheRateRaw - wunschRate);
                    if (calcInfo.MonatlicheRateRaw > wunschRate) //rate muss sinken, also wert erhöhen
                    {
                        //durch abs bereits positiv   
                    }
                    else //rate muss steigen, also wert verringern
                    {
                        
                        addDiff *= -1.0M;
                    }
                    //if (lastwdiff == wdiff) break;//no change anymore
                    count++;

                    _Log.Debug("Iteration " + count + " calculated: " + calcInfo.MonatlicheRateRaw + " wanted: " + wunschRate + " diff: " + wdiff + " addition: " + addDiff+" input: "+ iterationInput);
                    if (Math.Abs(Cic.OpenLease.Service.RoundingFacade.getInstance().CutPrice(calcInfo.MonatlicheRateRaw)-wunschRate) < 0.01M) break;//wunschrate to calced rate differs less than 0.5 cents
                    if (Math.Abs(addDiff) < 0.01M) break;//less than 1 cent difference in inputs
                    if (count > MAXITER) break;//max iteration count reached
                    lastwdiff = wdiff;
                }
                calcInfo.skipRWCheck = false;
                calcInfo.skipMVZCheck = false;
                calcInfo.useNettoSource = calculationDto.useNettoSource;
                //wenn fertig, Grenzprüfung, falls Grenze, erneut rechnen
                if (count> MAXITER)
                {
                    tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                    tmpMsg = "Die monatliche Rate konnte nicht erreicht werden und wurde auf den nächstmöglichen Wert angepasst!" + System.Environment.NewLine;
                }
                bool recalcBound = false;
                if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Mietvorauszahlung)
                {
                    PRPARAMDto pSZ = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstMietvorauszahlungFieldMeta, calculationDto.SysObArt, false);
                    calcInfo.MietvorauszahlungBruttoP = KalkulationHelper.CalculateMietvorauszahlungP(calcInfo.MietvorauszahlungBrutto, calcInfo.KaufpreisBruttoOrg);
                    if (checkParam(pSZ, calcInfo.MietvorauszahlungBruttoP, ref bound))
                    {
                        calcInfo.MietvorauszahlungBruttoP = bound;
                        calcInfo.MietvorauszahlungBrutto = KalkulationHelper.CalculateMietvorauszahlung(calcInfo.MietvorauszahlungBrutto, calcInfo.KaufpreisBruttoOrg); 
                        recalcBound = true;
                        iterationInput = calcInfo.MietvorauszahlungBrutto;
                        tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                        tmpMsg = "Die monatliche Rate wurde aufgrund der Unter-/Überschreitung der minimalen/maximalen Anzahlung auf den nächstmöglichen Wert angepasst!" + System.Environment.NewLine;
                    }

                }
                if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Rabatt)
                {
                    PRPARAMDto pKSum = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstFinanzierungssummeMin, calculationDto.SysObArt, false);
                    if (checkParam(pKSum, calcInfo.FinanzierungssummeBrutto, ref bound))
                    {
                        decimal rabattDiff = Math.Abs(calcInfo.FinanzierungssummeBrutto - bound);
                        rabattDiff = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rabattDiff);
                        if (bound > calcInfo.FinanzierungssummeBrutto)//rabatt war zu gross, reduzieren
                            calcInfo.KaufpreisBrutto -= rabattDiff;
                        else if(bound < calcInfo.FinanzierungssummeBrutto)//rabatt war zu klein, erhöhen
                            calcInfo.KaufpreisBrutto += rabattDiff;
                        iterationInput = calcInfo.AnschaffungswertBrutto;
                        recalcBound = true;
                        tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                        tmpMsg = "Die monatliche Rate wurde aufgrund der Unter-/Überschreitung der minimalen/maximalen Kreditsumme auf den nächstmöglichen Wert angepasst!" + System.Environment.NewLine;
                    }
                  

                }
                if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Restwert)
                {
                    
                    PRPARAMDto pRW = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstRestwertFieldMeta, calculationDto.SysObArt, false);
                    if (checkParam(pRW, calcInfo.RestwertBruttoP, ref bound))
                    {
                        calcInfo.RestwertBruttoP = bound;
                        calcInfo.RestwertBrutto = rvSuggest.getRestwertNetto(bound).bruttoInklNova;
                        iterationInput = calcInfo.RestwertBrutto;
                        recalcBound = true;
                        tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                        tmpMsg = "Die monatliche Rate wurde aufgrund der Unter-/Überschreitung der Restwertgrenzen auf den nächstmöglichen Wert angepasst!" + System.Environment.NewLine;
                    }
                    
                }
                /* if (calcInfo.MessageCode != CalculationDto.MessageCodes.NoError)
                 {
                     tmpMsg += " " + calcInfo.Message;
                     tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                 }*/
                //always check for other warnings now
                _Log.Debug("Iteration final Result: " + calcInfo.MonatlicheRate);
                calcInfo.ANGKALKBWFEHLER = 0;
                calcInfo = Calculate(Context, sysBRAND, sysPEROLE, calcInfo, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                _Log.Debug("Iteration final Calculation: " + calcInfo.MonatlicheRate);
                /* calcInfo.Message = tmpMsg+" "+calcInfo.Message;
                 if (calcInfo.MessageCode==CalculationDto.MessageCodes.NoError)//use the general error above, else the specific one
                 {
                     calcInfo.MessageCode = tmpCode;
                 }*/
                if (Math.Abs(calcInfo.MonatlicheRate - wunschRate) >= 0.01M && tmpMsg.Length == 0)//obwohl die rate zur Wunschrate abweicht keine Meldung -> Meldung bringen
                {
                    if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Mietvorauszahlung)
                    {
                        tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                        tmpMsg = "Die monatliche Rate wurde aufgrund der Unter-/Überschreitung der minimalen/maximalen Anzahlung auf den nächstmöglichen Wert angepasst!" + System.Environment.NewLine;
                    }
                    if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Rabatt)
                    {
                        tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                        tmpMsg = "Die monatliche Rate wurde aufgrund der Unter-/Überschreitung der minimalen/maximalen Kreditsumme auf den nächstmöglichen Wert angepasst!" + System.Environment.NewLine;
                    }
                    if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Restwert)
                    {
                        tmpCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                        tmpMsg = "Die monatliche Rate wurde aufgrund der Unter-/Überschreitung der Restwertgrenzen auf den nächstmöglichen Wert angepasst!" + System.Environment.NewLine;
                    }
                }
                //calcInfo.ANGKALKBWFEHLER = (wunschRate-calcInfo.MonatlicheRate)*calcInfo.Laufzeit;
                calcInfo.Message = tmpMsg;
                calcInfo.MessageCode = tmpCode;
                calcInfo.hasPouvoirMessage = false; //disable boundary message

                //calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.Mietvorauszahlung;
                calcInfo.CalculationSource = ServiceAccess.DdOl.CalculationDto.CalculationSources.Rate;

                if (calculationDto.CalculationTarget == CalculationDto.CalculationTargets.Rabatt)
                {
                    if (iscredit)
                    {
                        calcInfo.Rabatt = originalInput - iterationInput;
                        calcInfo.Rabatt = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calcInfo.Rabatt);
                        calcInfo.KaufpreisBruttoOrg = calcInfo.KaufpreisBrutto;
                        calcInfo.KaufpreisNetto -= calcInfo.Rabatt;
                    }
                    else
                    {
                        calcInfo.Rabatt = calcInfo.KaufpreisBruttoOrg - calcInfo.KaufpreisBrutto;
                        calcInfo.Rabatt = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calcInfo.Rabatt);
                        calcInfo.KaufpreisBruttoOrg = calcInfo.KaufpreisBrutto;
                        calcInfo.KaufpreisNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calcInfo.KaufpreisBrutto, UstErinkl);
                    }
                }
                
                calculationDto = calcInfo;
                if (calcInfoSubv != null)
                    calcInfoSubv.CalculationSource = calcInfo.CalculationSource;
                
            }
#endregion

            _Log.Debug("Duration N " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;
#region Subventionsberechnungs-Rekursion
            switch (calculationDto.SubventionCalcMode)
            {
                case (-1):
                   
                    
                    if (debug)
                    {
                        /*_Log.Debug("Calculate returns with:" + _Log.dumpObject(calculationDto));
                        _Log.Debug("AbschlussProvision:" + _Log.dumpObject(calculationDto.AbschlussProvision));
                        _Log.Debug("ZugangsProvision:" + _Log.dumpObject(calculationDto.ZugangProvision));*/
                    }
                    break;
                case (0):
                    //Zins
                    

                    calcInfoSubv.SubventionCalcMode = 1;
                    //subventionsberechnungen ermitteln den restwert nicht erneut, sollen aber evtl rw-anpassungen schon übernehmen aus dem ersten unsubventionierten rechendurchlauf
                    calcInfoSubv.RestwertBrutto = calculationDto.RestwertBrutto;
                    calcInfoSubv.RestwertNetto = calculationDto.RestwertNetto;
                    calcInfoSubv.RestwertUst = calculationDto.RestwertUst;
                    calcInfoSubv.RestwertBruttoP = calculationDto.RestwertBruttoP;
                    calcInfoSubv.RestwertvorschlagBrutto = calculationDto.RestwertvorschlagBrutto;
                    calcInfoSubv.RestwertvorschlagNetto = calculationDto.RestwertvorschlagNetto;
                    calcInfoSubv.RestwertvorschlagtUst = calculationDto.RestwertvorschlagtUst;
                    calcInfoSubv.RestwertvorschlagBruttoP = calculationDto.RestwertvorschlagBruttoP;
                    if (calcInfoSubv.CalculationSource == CalculationDto.CalculationSources.Restwert || calcInfoSubv.CalculationSource == CalculationDto.CalculationSources.RestwertP)
                        calcInfoSubv.CalculationSource = CalculationDto.CalculationSources.Laufzeit;
                    //--------------

                    decimal aktCalcRateWithInputs = calculationDto.MonatlicheRate;
                    decimal newZinsNom = calculationDto.ZinsNominal;
                    
                    if (calcInfoSubv.CalculationSource != ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEffRecursion && calcInfoSubv.CalculationSource != ServiceAccess.DdOl.CalculationDto.CalculationSources.RateRecursion)
                        {
                            
                            
                            if (calculationDto.SubventionCalc==0)
                            {
                                calculationDto.Subvention_Zins = 0;
                                calcInfoSubv.SubventionCalc = 1;
                                CalculationDto r = Calculate(Context, sysBRAND, sysPEROLE, calcInfoSubv, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                                calculationDto.Subvention_Zins = (r.MonatlicheRate - aktCalcRateWithInputs) * r.Laufzeit;//subvention explizit über aktion, nicht user-änderung der rate
                                calculationDto.Subvention_RGG = (r.Subvention_RGG);
                                calculationDto.MonatlicheRate_Default = r.MonatlicheRate;
                                
                                calculationDto.ZinsNominal = Interest;
                                updateDefaults(calculationDto);

                           
                                CalculationDto calcInfoSubv2 = ObjectCloner.Clone(calcInfoSubv);
                                calcInfoSubv.CalculationSource = CalculationDto.CalculationSources.Laufzeit;

                                //calculationDto.SubventionCalcMode == 4)//DefaultZins
                                //calculationDto.SubventionCalcMode == 5)//nur subventionszins

                                //values without expl. zins and without user zins change:
                                calcInfoSubv2.IgnoreInsurances = 1;
                                calcInfoSubv2.SubventionCalcMode = 4;
                                calcInfoSubv2.CalculationSource = CalculationDto.CalculationSources.Laufzeit;//assigns defaultzins
                                calcInfoSubv2.SubventionCalc = 1;
                                CalculationDto rDefault = Calculate(Context, sysBRAND, sysPEROLE, calcInfoSubv2, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                                decimal defaultRate = rDefault.MonatlicheRateRaw;//Zeile 31 xls
                                decimal ausgZinsNom = rDefault.Zins;
                                calculationDto.ZinsBasis = CalculateEffectiveInterest(rDefault.Zins, CnstPeriodsPerYear);
                                calculationDto.MonatlicheRate_Default = defaultRate;
                               // decimal orgBW = rDefault.FinanzierungssummeBrutto;//ohne rsv+gap
                               // decimal zinsvorallensubs = rDefault.ZinsNominal;


                                //calculate rate with vk-entered-zins, without gap/rsv/expl zins
                                calcInfoSubv2 = ObjectCloner.Clone(calcInfoSubv);
                                calcInfoSubv2.IgnoreInsurances = 1;
                                calcInfoSubv2.SubventionCalcMode = 3;
                                calcInfoSubv2.CalculationSource = CalculationDto.CalculationSources.Mietvorauszahlung;//does NOT assign defaultzins
                                calcInfoSubv2.Zins = newZinsNom;
                                calcInfoSubv2.SubventionCalc = 1;
                                rDefault = Calculate(Context, sysBRAND, sysPEROLE, calcInfoSubv2, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                                decimal rateFromVkZins = rDefault.MonatlicheRateRaw;//rate ohne rsv/gap durch userzinsänderung - Zeile 37 xls
                                decimal zinsVk = rDefault.Zins;

                                //calculate rate with vk-entered-zins, without gap/rsv/expl zins
                                calcInfoSubv2 = ObjectCloner.Clone(calcInfoSubv);
                                calcInfoSubv2.IgnoreInsurances = 1;
                                calcInfoSubv2.SubventionCalcMode = 5;//only aktionszins
                                calcInfoSubv2.CalculationSource = CalculationDto.CalculationSources.Laufzeit;//assigns defaultzins
                                calcInfoSubv2.SubventionCalc = 1;
                                rDefault = Calculate(Context, sysBRAND, sysPEROLE, calcInfoSubv2, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                                decimal rateFromAktionszins = rDefault.MonatlicheRateRaw;//rate ohne rsv/gap durch userzinsänderung - Zeile 33 xls
                                decimal zielZinsNom = rDefault.Zins;
                                calculationDto.ZinsAktion = CalculateEffectiveInterest(rDefault.Zins,CnstPeriodsPerYear);

                                //calculate zinsexpl (configured zins reductions)
                                calcInfoSubv2 = ObjectCloner.Clone(calcInfoSubv);
                                calcInfoSubv2.IgnoreInsurances = 1;
                                calcInfoSubv2.SubventionCalcMode =  3;
                                calcInfoSubv2.CalculationSource = CalculationDto.CalculationSources.Laufzeit;//assigns defaultzins
                                calcInfoSubv2.SubventionCalc = 1;
                                rDefault = Calculate(Context, sysBRAND, sysPEROLE, calcInfoSubv2, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                                decimal defaultRateExpl = rDefault.MonatlicheRateRaw;//rate mit konfigurierter zinssubvention - Zeile 35 xls
                                decimal zinsExpl = rDefault.Zins;
                                //  decimal bwExpl = rDefault.FinanzierungssummeBrutto;//ohne rsv+gap
                                /* decimal zinsexpl = rDefault.ZinsNominal;


                                 decimal tmpNewRateZinsExplWithoutGAPRSV = (decimal)Cic.One.Utils.BO.Kalkulator.calcRATE((double)orgBW, (double)(zinsexpl / 12.0M), (double)calculationDto.Laufzeit, (double)calculationDto.RestwertBrutto, Mode == CalculationMode.Begin);
                                 decimal newBW2 = (decimal)Cic.One.Utils.BO.Kalkulator.calcBARW((double)tmpNewRateZinsExplWithoutGAPRSV, (double)(zinsvorallensubs / 12.0M), (double)calculationDto.Laufzeit, (double)calculationDto.RestwertBrutto, Mode == CalculationMode.Begin);


                                 //rate von zins von usereingabe
                                 decimal tmpNewRateWithoutGAPRSV = (decimal)Cic.One.Utils.BO.Kalkulator.calcRATE((double)newBW2, (double)(newZinsNom / 12.0M), (double)calculationDto.Laufzeit, (double)calculationDto.RestwertBrutto, Mode == CalculationMode.Begin);
                                 //barwert bei dieser rate mit zins nach expl. zins (also mit 3.99 anstatt 4.99)
                                 decimal newBW = (decimal)Cic.One.Utils.BO.Kalkulator.calcBARW((double)tmpNewRateWithoutGAPRSV, (double)(calculationDto.ZinsNominal_Default / 12.0M), (double)calculationDto.Laufzeit, (double)calculationDto.RestwertBrutto, Mode == CalculationMode.Begin);
                                 */

                                calculationDto.ZinsNominal = Interest;
                                updateDefaults(calculationDto);

                                // calcInfoSubv.SubventionCalcMode = 3;//calculate with all subventions
                                // CalculationDto r = Calculate(Context, sysBRAND, sysPEROLE, calcInfoSubv, calcDao, gebuehrDao, rvSuggest, vs, prparamDao, lsaddDao, kDao, qDao);
                                // decimal subvDefaultRate = r.MonatlicheRate;
                                //HIER zu korrigieren für zinssubvention explizit
                                //calculationDto.Subvention_Zins = (defaultRate - subvDefaultRate) * r.Laufzeit;//subvention explizit über aktion, nicht user-änderung der rate
                                //calculationDto.Subvention_RGG = (r.Subvention_RGG);
                               /* decimal defKredValue = (defaultRate * calculationDto.Laufzeit);
                                decimal explKredValue = (defaultRateExpl * calculationDto.Laufzeit);
                                decimal testSubvExplZins = defKredValue - explKredValue;
                                decimal userKredValue = (rateFromVkZins * calculationDto.Laufzeit);
                                decimal testUserProvision = userKredValue - explKredValue;*/
                                //HCE CORR
                                calculationDto.Subvention_Zins = 0;
                                if (defaultRate > rateFromAktionszins)
                                {
                                    //calculationDto.Subvention_Zins = (defaultRate - rateFromAktionszins) * calculationDto.Laufzeit;
                                    calculationDto.Subvention_Zins = (decimal)Cic.One.Utils.BO.Kalkulator.calcBARW((double)(defaultRate - rateFromAktionszins),(double)( (ausgZinsNom - zielZinsNom)  / 12.0M), calculationDto.Laufzeit, 0.0, !iscredit);
                                }
                                calculationDto.Subvention_Zins = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.Subvention_Zins);


                                calculationDto.Subvention_Zins2 = 0;
                                if (defaultRateExpl > rateFromVkZins)
                                {
                                    calculationDto.Subvention_Zins2 = (defaultRateExpl - rateFromVkZins) * calculationDto.Laufzeit;
                                //calculationDto.Subvention_Zins2 = (decimal)Cic.One.Utils.BO.Kalkulator.calcBARW((double)(defaultRateExpl - rateFromVkZins), (double)((zinsExpl-zinsVk)  / 12.0M), calculationDto.Laufzeit, 0.0, !iscredit);
                            }
                            calculationDto.Subvention_Zins2 = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.Subvention_Zins2);

                                decimal userProvision = 0;
                                if (rateFromVkZins > defaultRateExpl)
                                    userProvision = (rateFromVkZins - defaultRateExpl) * calculationDto.Laufzeit;
                                else 
                                    userProvision = (rateFromVkZins - defaultRateExpl) * calculationDto.Laufzeit;

                                // calculationDto.Subvention_RGG = 0;
                                calculationDto.ZugangProvision.ProvisionValue = 0;
                                if (calculationDto.Laufzeit >= QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_PROV_MIN_LZ))
                                {
                                    calculationDto.ZugangProvision.ProvisionValue = userProvision; 
                                    calculationDto.ZugangProvision.ProvisionValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.ZugangProvision.ProvisionValue);
                                    if (calculationDto.ZugangProvision.ProvisionValue > 0)
                                    {
                                        PRPARAMDto parHDNLPROV = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstHndlProvPFieldMeta, calculationDto.SysObArt, false);


                                        if (parHDNLPROV != null) //Abweichungsparameter vorhanden, prüfen
                                        {
                                            decimal pprov = parHDNLPROV.DEFVALP.HasValue ? parHDNLPROV.DEFVALP.Value : 0;
                                            if (pprov > 100)
                                                pprov = 100;
                                            if (pprov < 0)
                                                pprov = 0;
                                            calculationDto.ZugangProvision.ProvisionValue = calculationDto.ZugangProvision.ProvisionValue/100.0M*pprov;
                                            calculationDto.ZugangProvision.ProvisionValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.ZugangProvision.ProvisionValue);
                                            calculationDto.ZugangProvision.ProvisionPercentage = pprov;
                                        }
                                        else
                                        {
                                            calculationDto.ZugangProvision.ProvisionValue = 0;
                                            calculationDto.ZugangProvision.ProvisionPercentage = 0;
                                        }
                                    }
                                }
                            }
                            //set netto into this field:
                            calculationDto.ZugangProvision.BasicPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.ZugangProvision.ProvisionValue, Ust));

                       
                    }
                    
                    if(iscredit)
                    {
                        calculationDto.MitfinanzierterBestandteilNetto = calculationDto.MitfinanzierterBestandteilBrutto;
                    }

                    updateFinanzierungssummeFinal(calculationDto, Ust, iscredit, UstErinkl);

                    //Round values
                    calculationDto.AnschaffungswertBrutto = calculationDto.KaufpreisBrutto;//Fix for AnschaffungswertBrutto already contained MitFin
                    calculationDto.AnschaffungswertNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.AnschaffungswertBrutto, UstErinkl);
                    
                    calculationDto.AnschaffungswertBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.AnschaffungswertBrutto + calculationDto.MitfinanzierterBestandteilBrutto);
                    calculationDto.AnschaffungswertNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.AnschaffungswertNetto + calculationDto.MitfinanzierterBestandteilBrutto);
                    calculationDto.AnschaffungswertUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.AnschaffungswertBrutto - calculationDto.AnschaffungswertNetto);


                    // CALCULATION FINAL Parameter check
                    PRPARAMDto parBGIntern = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstFinanzierungssummeMin, calculationDto.SysObArt, false);
                    if (parBGIntern != null)
                    {
                        if (parBGIntern.MINVALN > calculationDto.FinanzierungssummeNetto)//ANGKALKBGINTERN
                        {
                            calculationDto.MessageCode = CalculationDto.MessageCodes.Rt001;
                            calculationDto.Message = "Minimale Finanzierungssumme für Produkt unterschritten";
                        }
                    }
                    PRPARAMDto parRate = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstRateMin, calculationDto.SysObArt, false);
                    if (parRate != null)
                    {
                        if (parRate.MINVALN > calculationDto.MonatlicheRateNetto)//ANGKALKRATE
                        {
                            calculationDto.MessageCode = CalculationDto.MessageCodes.Rt001;
                            calculationDto.Message = "Minimale Rate für Produkt unterschritten";
                        }
                    }

                    //_Log.Debug("Calculate returns with:" + _Log.dumpObject(calculationDto));
                    break;
            }
#endregion
            _Log.Debug("Duration O " + (DateTime.Now.TimeOfDay.TotalMilliseconds - measure)); measure = DateTime.Now.TimeOfDay.TotalMilliseconds;

          
            return calculationDto;

        }

        private static void calculateBearbeitungsgebuehr(long sysBRAND, long sysPEROLE, Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, GebuehrDao gebuehrDao)
        {
            bool noProvision = ((calculationDto.SPECIALCALCSTATUS.HasValue && calculationDto.SPECIALCALCSTATUS.Value > 0) && calculationDto.BearbeitungsgebuehrNachlass > 0);
            if (calculationDto.Laufzeit < QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_PROV_MIN_LZ))
                noProvision = true;
            if ((!calculationDto.SPECIALCALCSTATUS.HasValue || calculationDto.SPECIALCALCSTATUS.Value == 0) && calculationDto.isIM)//Fix #6069
                noProvision = true;
            GebuhrenDto gebDto = gebuehrDao.calcGebuehr(calculationDto.FinanzierungssummeBrutto, calculationDto.SysPrProduct, calculationDto.SysObTyp, calculationDto.SysObArt, calculationDto.BearbeitungsgebuehrNachlass, sysPEROLE, sysBRAND, noProvision);
            calculationDto.Gebuehren = gebDto;
            calculationDto.Bearbeitungsgebuehr = gebDto.GebuhrenBrutto;
            
            calculationDto.Bearbeitungsgebuehr_Default = gebDto.Gebuhren_Default;
        }

        private static ProvisionDto recalcRSV(DdOlExtended Context, long sysBRAND, long sysPEROLE, Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, VSTYPDao vs, decimal Ust, Boolean iscredit, bool isUsedCar, decimal NovaAufschlagSatz, decimal UstErinkl)
        {
            ProvisionDto ProvisionDto = new ProvisionDto();
            /*-----------------------------------------------------------------------------------------------------------
            * RSV
            *  dependent of
            *    FinanzierungssummeBrutto
            *    Laufzeit
            *    Zins
            *    RestwertBrutto
            *  impact on
            *    FinanzierungssummeBrutto
            *    MonatlicheRate
            */
#region RSV
            calculationDto.RestschuldVersicherung = 0;
            calculationDto.RSVProvision = 0;
            calculationDto.rsdvParam = null;
            calculationDto.rsdvResult = null;
            if (calculationDto.hasRSV && calculationDto.SYSVSTYPRSV > 0)
            {
                _Log.Debug("RSV with Zielrate " + calculationDto.RestwertBrutto + " Finanzierungssumme " + calculationDto.FinanzierungssummeBrutto + " Laufzeit " + calculationDto.Laufzeit + " Zinssatz: " + (calculationDto.Zins / 100) + " SysVSTYP " + calculationDto.SYSVSTYPRSV + " prproduct " + calculationDto.SysPrProduct);
                InsuranceParameterDto insurance = new InsuranceParameterDto();
                //Defect #4544 
                decimal rsvZins = calculationDto.Zins;
                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsNominal)
                    rsvZins = calculationDto.ZinsNominal;
                if (calculationDto.CalculationSource == ServiceAccess.DdOl.CalculationDto.CalculationSources.ZinsEffRecursion)
                    rsvZins = CalculateNominalInterest(calculationDto.ZinsEff, CnstPeriodsPerYear);

                // Finanzierungssumme, Zielrate, Laufzeit, Zinssatz
                insurance.Finanzierungssumme = calculationDto.Restkaufpreis;
                insurance.Zielrate = calculationDto.RestwertBrutto;
                insurance.Laufzeit = calculationDto.Laufzeit;
                insurance.Zinssatz = rsvZins / 100;
                insurance.LaufzeitFinanzierung = calculationDto.Laufzeit;
                insurance.ZinssatzDefault = calculationDto.ZinsNominal_Default;
                insurance.ZinssatzNominal = calculationDto.ZinsNominal;
                insurance.SysVSTYP = calculationDto.SYSVSTYPRSV;
                insurance.isCredit = iscredit;
                insurance.sysPrProduct = calculationDto.SysPrProduct;
                insurance.Nachlass = 0;
                insurance.calcProvision = !calculationDto.isIM;
                insurance.sysKdTyp = calculationDto.sysKdTyp;
                insurance.MonatsrateBrutto = calculationDto.MonatlicheRate;
                if(calculationDto.sysKdTyp > 1 && !iscredit)
                {
                    //use nettorate
                    insurance.MonatsrateBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.MonatlicheRate, iscredit ? 0 : Ust);
                }
                if(!iscredit)
                {
                    insurance.GAPSumme = calculationDto.lzgap * calculationDto.GAPVersicherung;
                }
                if (calculationDto.SPECIALCALCSTATUS.HasValue && calculationDto.SPECIALCALCSTATUS.Value > 0)
                    insurance.calcProvision = true;
                if (calculationDto.Laufzeit < QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_PROV_MIN_LZ))
                    insurance.calcProvision = false;
                try
                {
                    double rsvmeasure = DateTime.Now.TimeOfDay.TotalMilliseconds;

                    InsuranceResultDto vsr = vs.DeliverVSData(sysPEROLE, sysBRAND, insurance);
                    _Log.Debug("Duration RSV1 " + (DateTime.Now.TimeOfDay.TotalMilliseconds - rsvmeasure)); rsvmeasure = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    calculationDto.rsdvParam = insurance;
                    calculationDto.rsdvResult = vsr;
                    calculationDto.RestschuldVersicherung = vsr.Praemie;
                    calculationDto.RestschuldVersicherung = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RestschuldVersicherung);
                    calculationDto.RestschuldVersicherungDefault = vsr.Praemie_Default;
                    _Log.Debug("RSV changes Rate from " + calculationDto.MonatlicheRate + " to " + vsr.rateNeu);
                    calculationDto.MonatlicheRate = vsr.rateNeu;
                    calculationDto.Subvention_RSV = vsr.Praemie_Subvention;
                    _Log.Debug("RSV Result: Prämie: " + vsr.Praemie + " Monatliche Rate: " + vsr.rateNeu + " Subvention: " + calculationDto.Subvention_RSV+" anteil: "+vsr.Gesamtfinanzierungsrate);

                    //Bei RSV wird diese der FinanzierungssummeBrutto hinzugerechnet
                    updateFinanzierungssumme(calculationDto, Ust, iscredit, NovaAufschlagSatz, isUsedCar, UstErinkl);


                    ProvisionDto.sysPerole = sysPEROLE;
                    ProvisionDto.sysBrand = sysBRAND;
                    ProvisionDto.sysProvTarif = calculationDto.SysProvTariff;
                    ProvisionDto.rank = (int)ProvisionTypeConstants.Restschuld;
                    ProvisionDto.sysobtyp = calculationDto.SysObTyp;
                    ProvisionDto.sysprproduct = calculationDto.SysPrProduct;
                    ProvisionDto.versicherungspraemiegesamt = calculationDto.RestschuldVersicherung;
                    ProvisionDto.noProvision = calculationDto.isIM;
                    ProvisionDto.sysVstyp = insurance.SysVSTYP;
                    if (calculationDto.SPECIALCALCSTATUS.HasValue && calculationDto.SPECIALCALCSTATUS.Value > 0)
                        ProvisionDto.noProvision = false;
                    if (calculationDto.Laufzeit < QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_PROV_MIN_LZ))
                        ProvisionDto.noProvision = true;
                    ProvisionDto.laufzeit = calculationDto.Laufzeit;
                    ProvisionDto = DeliverRestschuldProvision(Context, ProvisionDto, sysBRAND, sysPEROLE);
                    _Log.Debug("Duration RSV2 " + (DateTime.Now.TimeOfDay.TotalMilliseconds - rsvmeasure)); rsvmeasure = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    calculationDto.RSVProvision = ProvisionDto.provision;
                    calculationDto.rsdvProvision = ProvisionDto;
                }
                catch (Exception r)
                {
                    _Log.Error("RSV-Calculation failed: " + r.Message, r);
                }
            }
            else if (calculationDto.hasRSV && calculationDto.SYSVSTYPRSV < 1)
                _Log.Debug("RSV not calculable: No SYSVSTYP given!");
            //END OF RSV-----------------------------------------------------------------------------------------------------------
#endregion
            return ProvisionDto;
        }
        /// <summary>
        /// Calculates GAP insurance
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="sysBRAND"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="calculationDto"></param>
        /// <param name="vs"></param>
        /// <param name="Ust"></param>
        /// <param name="iscredit"></param>
        /// <param name="isUsedCar"></param>
        /// <param name="NovaAufschlagSatz"></param>
        /// <returns></returns>
        private static ProvisionDto recalcGAP(DdOlExtended Context, long sysBRAND, long sysPEROLE, Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, VSTYPDao vs, decimal Ust, Boolean iscredit, bool isUsedCar, decimal NovaAufschlagSatz, decimal UstErinkl)
        {
            ProvisionDto ProvisionDto = new ProvisionDto();
            /*-----------------------------------------------------------------------------------------------------------
            * GAP
            *  dependent of
            *    FinanzierungssummeBrutto
            *    Laufzeit
            *    Zins
            *    RestwertBrutto
            *  impact on
            *    FinanzierungssummeBrutto
            *    MonatlicheRate
            */
#region GAP
            calculationDto.GAPVersicherung = 0;
            calculationDto.GAPProvision = 0;
            calculationDto.gapParam = null;
            calculationDto.gapResult = null;
            if (calculationDto.hasGAP && calculationDto.SYSVSTYPGAP > 0)
            {
                _Log.Debug("GAP" + calculationDto.SYSVSTYPGAP + " Laufzeit " + calculationDto.lzgap +" Produkt:"+ calculationDto.SysPrProduct);
                InsuranceParameterDto insurance = new InsuranceParameterDto();
                insurance.SysVSTYP = calculationDto.SYSVSTYPGAP;
                insurance.Nachlass = 0;
                insurance.sysPrProduct = calculationDto.SysPrProduct;
                insurance.Laufzeit = calculationDto.lzgap;
                insurance.Finanzierungssumme = calculationDto.KaufpreisBrutto;
                insurance.FinanzierungssummeNetto = calculationDto.KaufpreisNetto;
                insurance.LaufzeitFinanzierung = calculationDto.Laufzeit;
                insurance.ZinssatzDefault = calculationDto.ZinsNominal_Default;
                insurance.ZinssatzNominal = calculationDto.ZinsNominal;
                insurance.isCredit = iscredit;
                insurance.sysKdTyp = calculationDto.sysKdTyp;
                insurance.calcProvision = !calculationDto.isIM;
                if (calculationDto.SPECIALCALCSTATUS.HasValue && calculationDto.SPECIALCALCSTATUS.Value > 0)
                    insurance.calcProvision = true;
                if (calculationDto.Laufzeit < QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_PROV_MIN_LZ))
                    insurance.calcProvision = false;
                try
                {
                    double rsvmeasure = DateTime.Now.TimeOfDay.TotalMilliseconds;

                    InsuranceResultDto vsr = vs.DeliverVSData(sysPEROLE, sysBRAND, insurance);
                    
                    calculationDto.gapParam = insurance;
                    calculationDto.gapResult = vsr;
                    calculationDto.GAPVersicherung = vsr.Praemie;
                    calculationDto.GAPVersicherung = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.GAPVersicherung);
                    calculationDto.GAPVersicherungDefault = vsr.Praemie_Default;
                    

                    //Bei gap wird diese der FinanzierungssummeNetto hinzugerechnet
                    updateFinanzierungssumme(calculationDto, Ust, iscredit, NovaAufschlagSatz, isUsedCar, UstErinkl);


                    ProvisionDto.sysPerole = sysPEROLE;
                    ProvisionDto.sysBrand = sysBRAND;
                    ProvisionDto.sysProvTarif = calculationDto.SysProvTariff;
                    ProvisionDto.rank = (int)ProvisionTypeConstants.GAP;
                    ProvisionDto.sysobtyp = calculationDto.SysObTyp;
                    ProvisionDto.sysprproduct = calculationDto.SysPrProduct;
                    ProvisionDto.versicherungspraemiegesamt = calculationDto.GAPVersicherung;
                    ProvisionDto.noProvision = calculationDto.isIM;
                    ProvisionDto.sysVstyp = insurance.SysVSTYP;
                    if (calculationDto.SPECIALCALCSTATUS.HasValue && calculationDto.SPECIALCALCSTATUS.Value > 0)
                        ProvisionDto.noProvision = false;
                    if (calculationDto.Laufzeit < QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_PROV_MIN_LZ))
                        ProvisionDto.noProvision = true;
                    ProvisionDto.laufzeit = calculationDto.Laufzeit;
                    ProvisionDto = DeliverGAPProvision(Context, ProvisionDto, sysBRAND, sysPEROLE);
                    
                    calculationDto.GAPProvision = ProvisionDto.provision;
                    calculationDto.gapProvisionDto = ProvisionDto;
                }
                catch (Exception r)
                {
                    _Log.Error("GAP-Calculation failed: " + r.Message, r);
                }
            }
            else if (calculationDto.hasGAP && calculationDto.SYSVSTYPGAP < 1)
                _Log.Debug("GAP not calculable: No SYSVSTYP given!");
            //END OF GAP-----------------------------------------------------------------------------------------------------------
#endregion
            return ProvisionDto;
        }

        private static void calcRate(Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, ref decimal Interest, decimal Ust, long SysINTSTRCT, CalculationMode Mode, ref decimal Base, ref decimal FirstPayment, ref decimal Term, ref decimal Rate, ref decimal ResidualValue, ref decimal ZinsEff, bool setDefault, bool isCredit, decimal depotZins)
        {
            //Calculate rate
            try
            {
                _Log.Debug("Calc Rate: zins: "+Interest+" ust: "+Ust+" instrct: "+SysINTSTRCT+" mode: "+Mode+" Base: "+Base+" Erste Rate: "+FirstPayment+" LZ: "+Term+" Rate: "+Rate+" RW: "+ResidualValue+" ZinsEff: "+ZinsEff+" DepotZins: "+depotZins+" setDefault: "+setDefault);
                //For Kredit-Calculations, calc rate nachschüssig, too
                if (isCredit)
                {
                    calculationDto.MonatlicheRateKredit = KalkulationHelper.CalculateRate(ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref Interest, ref ZinsEff, CnstPeriodsPerYear, CalculationTargets.CalculateRate, CalculationMode.End);
                    calculationDto.MonatlicheRateKredit = KalkulationHelper.CalculateRateWithVorteil(calculationDto.DepotBrutto, calculationDto.Laufzeit, Base, calculationDto.MonatlicheRateKredit, Ust, SysINTSTRCT, depotZins);
                }

                //vorschüssig
                calculationDto.MonatlicheRate = KalkulationHelper.CalculateRate(ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref Interest, ref ZinsEff, CnstPeriodsPerYear, CalculationTargets.CalculateRate, Mode);

                calculationDto.MonatlicheRate = KalkulationHelper.CalculateRateWithVorteil(calculationDto.DepotBrutto, calculationDto.Laufzeit, Base, calculationDto.MonatlicheRate, Ust, SysINTSTRCT, depotZins);
                if (setDefault)
                {
                    calculationDto.MonatlicheRate_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().CutPrice(calculationDto.MonatlicheRate);
                    calculationDto.ZinsNominal = Interest;
                    updateDefaults(calculationDto);
                }
            }
            catch (System.Exception e)
            {
                calculationDto.MessageCode = CalculationDto.MessageCodes.Rw001;
                calculationDto.Message = e.Message + System.Environment.NewLine;
            }
        }

        private static CalculationDto updateDefaults(CalculationDto calculationDto)
        {
            //dont set the defaults when internal employee calcs sonderkalkulation
            bool imcalcingspecialcalc = calculationDto.isIM && calculationDto.SPECIALCALCSTATUS >= 2;
            if (imcalcingspecialcalc) return calculationDto;

            calculationDto.RestwertBrutto_Default = calculationDto.RestwertBrutto;
            calculationDto.RestwertBruttoP_Default = calculationDto.RestwertBruttoP;
            
            return calculationDto;
        }

        /// <summary>
        /// Clears fields that may not be saved for stand alone products
        /// </summary>
        /// <param name="calculationDto"></param>
        /// <returns></returns>
        private static CalculationDto clearStandAlone(CalculationDto calculationDto)
        {
            calculationDto.CrvBrutto = 0;
            calculationDto.CrvNetto = 0;
            calculationDto.CrvProzent = 0;
            calculationDto.CrvUst = 0;
            calculationDto.FinanzierungssummeBrutto = 0;
            calculationDto.FinanzierungssummeNetto = 0;
            calculationDto.FinanzierungssummeUst = 0;

            calculationDto.MonatlicheRate = 0;
            calculationDto.MonatlicheRateNetto = 0;
            calculationDto.MonatlicheRateUst = 0;

            calculationDto.Gebuehren.Gebuhren_Default = 0;
            calculationDto.Gebuehren.GebuhrenBrutto = 0;
            calculationDto.Gebuehren.GebuhrenUst = 0;
            calculationDto.Gebuehren.Provision = 0;
            calculationDto.Gebuehren.Subvention = 0;
            calculationDto.GesamtbelastungBrutto = 0;
            calculationDto.GesamtbelastungNetto = 0;
            calculationDto.GesamtbelastungUst = 0;
            calculationDto.GesamtKostenBrutto = 0;
            calculationDto.GesamtKostenNetto = 0;
            calculationDto.GesamtKostenUst = 0;
            calculationDto.Kreditbetrag = 0;
            calculationDto.Restkaufpreis = 0;
            calculationDto.RestschuldVersicherung = 0;
            calculationDto.RestschuldVersicherungDefault = 0;
            calculationDto.RestwertBrutto = 0;
            calculationDto.RestwertBruttoP = 0;
            calculationDto.RestwertNetto = 0;
            calculationDto.RestwertUst = 0;
            calculationDto.RestwertvorschlagBrutto = 0;
            calculationDto.RestwertvorschlagBruttoP = 0;
            calculationDto.RestwertvorschlagNetto = 0;
            calculationDto.RestwertvorschlagtUst = 0;
            calculationDto.RgGeb_Default = 0;
            calculationDto.RgGebBrutto = 0;
            calculationDto.RgGebNetto = 0;
            calculationDto.RgGebUst = 0;
            calculationDto.RSVProvision = 0;
            calculationDto.sfBaseBrutto = 0;
            calculationDto.sfBaseNetto = 0;
            calculationDto.sfBaseProzent = 0;
            calculationDto.sfBaseUst = 0;
            calculationDto.Subvention_Restwert = 0;
            calculationDto.Subvention_RGG = 0;
            calculationDto.Subvention_RSV = 0;
            calculationDto.Subvention_Zins = 0;
            calculationDto.Zins = 0;
            calculationDto.ZinsEff = 0;
            calculationDto.ZinsNominal = 0;
            calculationDto.Zinssatz = 0;
            return calculationDto;
        }

        private static void setPouvoir(CalculationDto dto, decimal min, decimal max, decimal current)
        {
            dto.hasPouvoirMessage = true;
            dto.pouvoirMin = min;
            dto.pouvoirMax = max;
            dto.pouvoirEnteredValue = current;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetvalue">user entered interest value</param>
        /// <param name="defaultValue">default interest value</param>
        /// <param name="fieldid"></param>
        /// <param name="sysBRAND"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="calculationDto"></param>
        /// <param name="prparamDao"></param>
        /// <returns>the interest value, reduced to the pouvoir bounds of the user</returns>
        private static decimal getZinsPouvoir(decimal targetvalue, decimal defaultValue, string fieldid, long sysBRAND, long sysPEROLE, Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, PRParamDao prparamDao)
        {
            decimal targetZins = targetvalue;
            decimal mineff = defaultValue;
            decimal maxeff = defaultValue;
            //bool hasPouvoir = false;
            PRPARAMDto param = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, fieldid, calculationDto.SysObArt, false);
            if (param != null)
            {
                if (param.TYP == 1)
                {
                    if (param.MINVALP.HasValue)
                    {
                        mineff = mineff + param.MINVALP.Value;
                        if (mineff < 0)
                            mineff = 0;
                    }
                    if (param.MAXVALP.HasValue)
                    {
                        decimal newmaxeff = maxeff + param.MAXVALP.Value;

                        if (newmaxeff > maxeff) maxeff = newmaxeff;
                    }
                    //hasPouvoir = true;
                }

            }
            PRPOUVOIR effpouvoir = prparamDao.DeliverPrPouvoir(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, fieldid, calculationDto.SysObArt);
            if (effpouvoir != null)
            {
                if (effpouvoir.TYP == 1)
                {
                    if (effpouvoir.ADJMINP.HasValue)
                    {
                        mineff = mineff + effpouvoir.ADJMINP.Value;
                        if (mineff < 0)
                            mineff = 0;
                    }
                    if (effpouvoir.ADJMAXP.HasValue)
                    {
                        decimal newmaxeff = maxeff + effpouvoir.ADJMAXP.Value;

                        if (newmaxeff > maxeff) maxeff = newmaxeff;
                    }
                    //hasPouvoir = true;
                }

            }
            _Log.Debug("Zinspouvoir für " + fieldid + ": min: " + mineff + " max: " + maxeff + " default: " + defaultValue + " entered: " + targetvalue);

            if (targetZins < mineff)
            {
                targetZins = mineff;
                calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Rw002;
                calculationDto.Message = minZinsMsg + System.Environment.NewLine;
                
            }
            if (targetZins > maxeff)
            {
                targetZins = maxeff;
                calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Rw002;
                calculationDto.Message = maxZinsMsg + System.Environment.NewLine;
                
            }

            return targetZins;
        }

        private static void validateLaufzeit(long sysBRAND, long sysPEROLE, Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, PRParamDao prparamDao)
        {
            int ageDays;
            DateTime ezul = calculationDto.Erstzulassungsdatum;
            if (calculationDto.Lieferdatum == null || calculationDto.Lieferdatum.Year < 2000) calculationDto.Lieferdatum = DateTime.Now;
            if (ezul.Year < 1801) ezul = calculationDto.Lieferdatum;//avoid invalid age
            PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstLaufzeitMAXFieldMeta, calculationDto.SysObArt, false);
            PRPARAMDto step = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstLaufzeitFieldMeta, calculationDto.SysObArt, false);
            if (par == null || step == null)
            {
                _Log.Warn("No " + PRParamDao.CnstLaufzeitMAXFieldMeta + "/" + PRParamDao.CnstLaufzeitFieldMeta + " Fields defined for Product " + calculationDto.SysPrProduct);
                return;
            }

            for (ageDays = 0; ezul.AddDays(ageDays + 1).CompareTo(calculationDto.Lieferdatum) <= 0; ageDays++) ;
            int months = (int)Math.Floor(ageDays / 30.0);
            int maxlz = (int)par.MAXVALN - months;//max months for lz allowed

            if (calculationDto.Laufzeit > maxlz)
            {
                calculationDto.Laufzeit = maxlz;
                while (calculationDto.Laufzeit % step.STEPSIZE != 0)
                    calculationDto.Laufzeit--;//= (int)par.STEPSIZE;

                if (calculationDto.Laufzeit < 1)
                    calculationDto.Laufzeit = 1;

                if (calculationDto.MessageCode != CalculationDto.MessageCodes.Lz002)
                    calculationDto.Message = "";

                calculationDto.MessageCode = CalculationDto.MessageCodes.Lz002;
                calculationDto.Message += " Die max. Laufzeit von " + par.MAXVALN + " Monaten darf nicht überschritten werden!";

            }
            else if (calculationDto.Laufzeit < par.MINVALN)
            {
                calculationDto.Laufzeit = (int)par.MINVALN;
                if (calculationDto.MessageCode != CalculationDto.MessageCodes.Lz002)
                    calculationDto.Message = "";

                calculationDto.MessageCode = CalculationDto.MessageCodes.Lz002;
                calculationDto.Message += " Die min. Laufzeit von " + par.MINVALN + " Monaten darf nicht unterschritten werden!";
            }
        }
        private static void validateLaufleistung(long sysBRAND, long sysPEROLE, Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, PRParamDao prparamDao)
        {

            PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstLaufleistungMAXFieldMeta, calculationDto.SysObArt, false);
            PRPARAMDto step = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstLaufleistungFieldMeta, calculationDto.SysObArt, false);
            if (par == null || step == null)
            {
                _Log.Warn("No " + PRParamDao.CnstLaufleistungMAXFieldMeta + "/" + PRParamDao.CnstLaufleistungFieldMeta + " Fields defined for Product " + calculationDto.SysPrProduct);
                return;
            }
            long cll = calculationDto.UbNahmeKm + (long)(calculationDto.Laufleistung * calculationDto.Laufzeit / 12);


            long tmp = (long)par.MAXVALN - calculationDto.UbNahmeKm;
            if (tmp < 0) tmp = 0;

            long topLL = (long)((tmp) / (calculationDto.Laufzeit / (1.0M * 12)));//pro jahr
            topLL = (long)(((long)Math.Floor((topLL - (decimal)step.MINVALN) / (decimal)step.STEPSIZE)) * (decimal)step.STEPSIZE) + (long)step.MINVALN;
            if (topLL < step.MINVALN)
            {
                topLL = (long)step.MINVALN;
                calculationDto.Laufzeit = (int)Math.Floor((decimal)(tmp / topLL)) * 12;
            }

            tmp = (long)par.MINVALN - calculationDto.UbNahmeKm;
            if (tmp < 0) tmp = 0;

            long bottomLL = 0;
            if (calculationDto.Laufzeit > 0)
                bottomLL = (long)((tmp) / (calculationDto.Laufzeit / (1.0M * 12)));
            bottomLL = (long)(((long)Math.Ceiling((bottomLL - (decimal)step.MINVALN) / (decimal)step.STEPSIZE)) * (decimal)step.STEPSIZE) + (long)step.MINVALN;
            if (bottomLL < step.MINVALN)
                bottomLL = (long)step.MINVALN;

            if (cll > par.MAXVALN)
            {
                calculationDto.Laufleistung = topLL;


                calculationDto.MessageCode = CalculationDto.MessageCodes.Lz002;
                calculationDto.Message = " Die max. Laufleistung von " + par.MAXVALN + " KM darf nicht überschritten werden!";
            }
            else if (cll < par.MINVALN)
            {
                calculationDto.Laufleistung = bottomLL;


                calculationDto.MessageCode = CalculationDto.MessageCodes.Lz002;
                calculationDto.Message = " Die min. Laufleistung von " + par.MINVALN + " KM darf nicht unterschritten werden!";
            }
        }
        /// <summary>
        /// validates depot, needs
        /// AnschaffungswertNetto,RestwertBrutto,RestwertNetto,MietvorauszahlungBrutto,DepotBrutto
        /// </summary>
        /// <param name="calculationDto"></param>
        /// <param name="Nova"></param>
        /// <param name="kalktyp"></param>
        /// <param name="iscredit"></param>
        /// <param name="Ust"></param>
        /// <param name="sysBRAND"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="prparamDao"></param>
        /// <param name="rvSuggest"></param>
        private static void ValidateDepot(Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, decimal Nova, long kalktyp, bool iscredit, decimal Ust,
            long sysBRAND, long sysPEROLE, PRParamDao prparamDao, RVSuggest rvSuggest)
        {
            if (iscredit) return;//nur leasing!

            //Calculate Max value for DepotBrutto
            NovaType tempRwVals = rvSuggest.getRestwertFromBrutto(calculationDto.RestwertBrutto);
            calculationDto.RestwertNetto = tempRwVals.netto + tempRwVals.nova + tempRwVals.bonusmalusexklaufschlag; //tempRwVals.netto;

            //Die Depotleistung darf 50 % des Netto-Anschaffungswertes inkl. NoVA nicht übersteigen
            decimal maxp = 50;
            PRPARAMDto depPar = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstDepotMaxValPFieldMeta, calculationDto.SysObArt, false);
            if (depPar != null && depPar.MAXVALP.HasValue)
            {
                maxp = depPar.MAXVALP.GetValueOrDefault(maxp);
            }


            decimal MaxValueFirstCondition = KalkulationHelper.CalculateDepotFirstCondition(calculationDto.AnschaffungswertNetto, calculationDto.RestwertNetto, Nova, maxp);
            _Log.Debug("Changed Depot: " + PRParamDao.CnstDepotMaxValPFieldMeta + ": " + maxp + " DepotBrutto: " + calculationDto.DepotBrutto + " Anschaffungswert Netto: " + calculationDto.AnschaffungswertNetto + " MVZ: " + calculationDto.MietvorauszahlungBrutto + " FirstCond: " + MaxValueFirstCondition);

            //Check if is more than max value - condition 1
            if (calculationDto.DepotBrutto > MaxValueFirstCondition)
            {
                calculationDto.DepotBrutto = MaxValueFirstCondition;


                if (kalktyp == 42 || kalktyp == 44)
                {
                    calculationDto.Message = "Depot wurde auf zulässigen Maximalwert reduziert!" + System.Environment.NewLine;
                    calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Dpt001;
                }
                
            }

            // Get the tax rate
            decimal MaxValueSecondCondition = KalkulationHelper.CalculateDepotSecondCondition(calculationDto.MietvorauszahlungBrutto, calculationDto.DepotBrutto);
            _Log.Debug("DepotBrutto: " + calculationDto.DepotBrutto + " SecondCond: " + MaxValueSecondCondition);
            //Check if is more than max value - condition 2

            PRPARAMDto pareig = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstDepotMaxEigenPFieldMeta, calculationDto.SysObArt);
            decimal defmaxpercent = 50;
            if (pareig == null)
            {
                _Log.Warn("No PRPARAM " + PRParamDao.CnstDepotMaxEigenPFieldMeta + " defined for Product " + calculationDto.SysPrProduct + " - MVZ+Depot Validation set to " + defmaxpercent + "%!");
            }
            else
            {
                defmaxpercent = pareig.MAXVALP.GetValueOrDefault(defmaxpercent);
            }

            defmaxpercent /= 100.0M;

            decimal b2 = (calculationDto.AnschaffungswertNetto * defmaxpercent);
            if (Math.Round(MaxValueSecondCondition, 1) > Math.Round(b2, 1))
            {
                //Recalculate MietvorauszahlungBrutto
                updateMVZ(calculationDto, false, Ust, iscredit);

                //Write correct values
                calculationDto.DepotBrutto = b2 - calculationDto.MietvorauszahlungBrutto;
                calculationDto.DepotBruttoP = KalkulationHelper.CalculateDepotP(calculationDto.DepotBrutto, calculationDto.AnschaffungswertBrutto);

                //Add message
                calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Dpt002;

                String deptString = "Anzahlung";
                if (kalktyp == 42 || kalktyp == 44)
                    deptString = "MVZ";

                if (kalktyp == 40 || kalktyp == 39)
                {
                    calculationDto.Message = "Die " + deptString + " ist zu hoch und wird auf den Maximalwert zurückgesetzt!" + System.Environment.NewLine;
                }
                
            }
            _Log.Debug("DepotBrutto: " + calculationDto.DepotBrutto + " MVZ: " + calculationDto.MietvorauszahlungBrutto);
            //Calculate DepotBruttoP
            calculationDto.DepotBruttoP = KalkulationHelper.CalculateDepotP(calculationDto.DepotBrutto, calculationDto.AnschaffungswertBrutto);


            //validate upper/lower bounds
            PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstDepotFieldMeta, calculationDto.SysObArt);
            if (par != null)
            {
                decimal max = par.MAXVALP.GetValueOrDefault(100);
                decimal min = par.MINVALP.GetValueOrDefault(0);
                //Check if is more than max value
                if (calculationDto.DepotBruttoP > max)
                {
                    //Write correct values
                    calculationDto.DepotBruttoP = max;
                    calculationDto.DepotBrutto = KalkulationHelper.CalculateDepot(calculationDto.DepotBruttoP, calculationDto.AnschaffungswertBrutto);

                    //Add message
                    calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Dpt002;
                    calculationDto.Message = "Depotbetrag größer als " + max + "% wurde reduziert!" + System.Environment.NewLine;
                }
                else if (calculationDto.DepotBruttoP < min)
                {
                    //Write correct values
                    calculationDto.DepotBruttoP = min;
                    calculationDto.DepotBrutto = KalkulationHelper.CalculateDepot(calculationDto.DepotBruttoP, calculationDto.AnschaffungswertBrutto);

                    //Add message
                    calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Dpt002;
                    calculationDto.Message = "Depotbetrag kleiner als " + min + "% wurde erhöht!" + System.Environment.NewLine;
                }
            }
            calculationDto.DepotNetto = calculationDto.DepotBrutto;
        }


        /// <summary>
        /// Validates MVZ boundaries
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="sysBRAND"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="calculationDto"></param>
        /// <param name="prparamDao"></param>
        /// <param name="isCredit"></param>
        /// <param name="Ust"></param>
        private static void ValidateMVZ(DdOlExtended Context, long sysBRAND, long sysPEROLE, Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, PRParamDao prparamDao, bool isCredit, decimal Ust)
        {
            if (calculationDto.skipMVZCheck) return;
            PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstMietvorauszahlungFieldMeta, calculationDto.SysObArt);
            if (par == null)
            {
                _Log.Error("No PRPARAM " + PRParamDao.CnstMietvorauszahlungFieldMeta + " defined for Product " + calculationDto.SysPrProduct + " - MVZ Validation not possible!");
                return;
            }
            decimal mvmax = par.MAXVALP.GetValueOrDefault(CnstMaxMietvorauszahlungBruttoP);  

            //Check if is more than max value
            decimal tmpmvzbp = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MietvorauszahlungBruttoP);
            if (tmpmvzbp > mvmax)
            {
                //Write correct values
                calculationDto.MietvorauszahlungBruttoP = mvmax;
                calculationDto.MietvorauszahlungBrutto = KalkulationHelper.CalculateMietvorauszahlung(calculationDto.MietvorauszahlungBruttoP, calculationDto.KaufpreisBruttoOrg);
                updateMVZ(calculationDto, false, Ust, isCredit);
                //Add message
                calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                calculationDto.Message = "Mietvorauszahlung größer als " + mvmax + " wurde reduziert!" + System.Environment.NewLine;
            }
           
            if (calculationDto.MietvorauszahlungBrutto > 0)
            {
                ContractExtensionDao ced = new ContractExtensionDao();
                ced.validateVTExtension(calculationDto, isCredit, Ust);
            }
        }

        /// <summary>
        /// Validates AHK-RW>MVZ boundary
        /// </summary>
        /// <param name="calculationDto"></param>
        private static void ValidateAHKMVZ(Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, decimal Ust, bool isCredit)
        {
            decimal mvmax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.AnschaffungswertBrutto) - Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.RestwertBrutto);
            
            if (mvmax < 0) mvmax = 0;
            //Check if is more than max value
            decimal tmpmvzb = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(calculationDto.MietvorauszahlungBrutto);
            if (tmpmvzb > mvmax)
            {
                //Write correct values
                calculationDto.MietvorauszahlungBrutto = mvmax;
                calculationDto.MietvorauszahlungBruttoP = KalkulationHelper.CalculateMietvorauszahlungP(calculationDto.MietvorauszahlungBrutto, calculationDto.KaufpreisBruttoOrg);
                updateMVZ(calculationDto, false, Ust, isCredit);
                //Add message
                calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                calculationDto.Message = "Mietvorauszahlung größer als AHK-RW=" + mvmax + " wurde reduziert!" + System.Environment.NewLine;
            }
        }
        /// <summary>
        /// Validates the rw after a rabatt
        /// </summary>
        /// <param name="calculationDto"></param>
        /// <param name="rvSuggest"></param>
        private static void ValidateRW(CalculationDto calculationDto, RVSuggest rvSuggest)
        {
			decimal deckelungRW = calculationDto.AnschaffungswertBrutto + calculationDto.Verrechnung;               
            deckelungRW = rvSuggest.getRestwertFromBrutto((decimal)deckelungRW).netto;

            decimal maxPercentAHK = rvSuggest.getRestwertPercent(deckelungRW); 
			if(Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(calculationDto.RestwertBruttoP) > Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(maxPercentAHK))
			{
				calculationDto.RestwertBruttoP=Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(maxPercentAHK);
                calculationDto.RestwertBrutto = rvSuggest.getRestwertNetto(maxPercentAHK).bruttoInklNova;
					
                    calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Mvz001;
                    calculationDto.Message = "Restwert größer als AHK wurde reduziert!" + System.Environment.NewLine;
			}
        }

        /// <summary>
        /// recalulate all prices
        /// </summary>
        /// <param name="calculationDto"></param>
        /// <param name="TaxRate"></param>
        /// <returns></returns>
        public static CalculationDto CalculatePricesFromPercent(CalculationDto calculationDto, decimal TaxRate)
        {
            //Calculate Depot
            if (calculationDto.DepotBruttoP > 0)
            {
                calculationDto.DepotBrutto = CalculateDepot(calculationDto.DepotBruttoP, calculationDto.KaufpreisBrutto);
            }



            return calculationDto;
        }

        public static decimal CalculateMehrKMSatz(decimal listenPreis, decimal sonderausstattung, decimal paketeBrutto, decimal pValue)
        {
            decimal Result;

            Result = (listenPreis + sonderausstattung + paketeBrutto) * (pValue / 100);
            return Result;
        }

        public static decimal CalculateMinderKMSatz(decimal mehrkmsatz, decimal minderKmPValue)
        {
            decimal Result;

            Result = mehrkmsatz * (minderKmPValue / 100);
            return Result;
        }

        public static decimal CalculateExternBrutto(decimal bruttoValue, decimal rabatto)
        {
            return bruttoValue - rabatto;
        }

        public static decimal CalculateBruttoFromRabattOP(decimal bruttoValue, decimal rabattOP)
        {
            decimal Result;
            Result = bruttoValue - ((bruttoValue * rabattOP) / 100);
            return Result;
        }

        public static decimal CalculateBruttoFromRabattO(decimal bruttoValue, decimal rabattO)
        {
            decimal Result;
            Result = bruttoValue - rabattO;
            return Result;
        }

        public static decimal CalculateRabattO(decimal bruttoValue, decimal rabattOP)
        {
            decimal Result;
            Result = (rabattOP * bruttoValue) / 100;
            return Result;
        }

        public static decimal CalculateRabattoP(decimal bruttoValue, decimal rabattO)
        {
            decimal Result = 0;
            if (bruttoValue != 0)
            {
                Result = (rabattO * 100) / bruttoValue;
            }
            return Result;
        }

        public static decimal CalculateRabattOFromNewBrutto(decimal bruttoValue, decimal newBruttoValue)
        {
            decimal Result;
            Result = bruttoValue - newBruttoValue;
            return Result;
        }

        public static decimal CalculateRabattoPFromNewBrutto(decimal bruttoValue, decimal newBruttoValue)
        {
            decimal Result;
            Result = CalculateRabattoP(bruttoValue, CalculateRabattOFromNewBrutto(bruttoValue, newBruttoValue));
            return Result;
        }

        public static decimal CalculateMietvorauszahlungP(decimal mietvorauszahlung, decimal anschaffungswertBrutto)
        {
            decimal Result = 0;
            if (anschaffungswertBrutto != 0)
            {
                Result = (mietvorauszahlung * 100) / anschaffungswertBrutto;
            }
            return Result;
        }

        /// <summary>
        /// bei einer Kombination der beiden Eigenleistungsformen (MVZ + Depot) sind nicht mehr als 30 % Mietvorauszahlung möglich
        /// </summary>
        /// <param name="mietvorauszahlungP"></param>
        /// <param name="anschaffungswertBrutto"></param>
        /// <returns></returns>
        public static decimal CalculateMietvorauszahlung(decimal mietvorauszahlungP, decimal anschaffungswertBrutto)
        {
            decimal Result;
            Result = (anschaffungswertBrutto * mietvorauszahlungP) / 100;
            return Result;
        }

        public static decimal CalculateDepotP(decimal depot, decimal anschaffungswertBrutto)
        {
            decimal Result = 0;
            if (anschaffungswertBrutto != 0)
            {
                Result = (depot * 100) / anschaffungswertBrutto;
            }
            return Result;
        }

        public static decimal CalculateDepot(decimal depotP, decimal anschaffungswertBrutto)
        {
            decimal Result;
            Result = (anschaffungswertBrutto * depotP) / 100;
            return Result;
        }
        public static decimal CalculateRW(decimal rwP, decimal rw)
        {
            decimal Result;
            Result = (rw * rwP) / 100;
            return Result;
        }

        public static decimal CalculateKreditbetrag(decimal finnanzierungssumme, decimal restschuldVersicherung)
        {
            decimal Result;

            Result = finnanzierungssumme + restschuldVersicherung;

            return Result;
        }



        public static decimal CalculateGesamtKosten(decimal mietvorauszahlung, ref decimal rate, int laufzeit, decimal restwert, decimal gebuhren, decimal finanzierungssumme, decimal restschuldVersicherung, decimal gapVersicherung)
        {
            decimal Result;

            Result = (rate * laufzeit) + restwert + gebuhren - finanzierungssumme;//gap ist in finsumme enthalten!
                

            if(Result<0)
            {
                //rate += 0.01M;
                Result = (rate * laufzeit) + restwert + gebuhren - finanzierungssumme;//gap ist in finsumme enthalten!
            }

            return Result;
        }

        /*  public static decimal CalculateRestwertP(decimal restwert, decimal anschaffungswertBrutto)
          {
              decimal Result = 0;
              if (anschaffungswertBrutto != 0)
              {
                  Result = (restwert * 100) / anschaffungswertBrutto;
              }
              return Result;
          }*/

        /* public static decimal CalculateRestwert(decimal restwertP, decimal anschaffungswertBrutto)
         {
             decimal Result;
             Result = (anschaffungswertBrutto * restwertP) / 100;
             return Result;
         }
         */
        public static decimal CalculateRate(ref decimal aquisitionCost, ref decimal firstPayment, ref decimal term, ref decimal rate, ref decimal residualValue, ref decimal interest, ref decimal zinsEff, int ppy, CalculationTargets art, CalculationMode mode)
        {
            if (interest == 0)
            {
                if (aquisitionCost - residualValue == 0)
                {
                    rate = 0;
                    return 0;
                }
                rate = (aquisitionCost - residualValue) / term;
                return rate;
            }

            decimal Result;
            // CalculateRateCore(ref aquisitionCost, ref firstPayment, ref term, ref rate, ref residualValue, ref interest, ref zinsEff, ppy, art, mode);

            bool zahlmodus = true;
            if (mode == CalculationMode.End)
                zahlmodus = false;
            double trate = Kalkulator.calcRATE((double)(aquisitionCost - firstPayment), (double)interest / 12.0, (double)term, (double)residualValue, zahlmodus);
            _Log.Debug("Test Rate: " + rate + " == " + trate + "?");
            rate = (decimal)trate;
            Result = rate;
            return Result;
        }
        /*
        public static decimal CalculateZins(ref decimal aquisitionCost, ref decimal firstPayment, ref decimal term, ref decimal rate, ref decimal residualValue, ref decimal interest, ref decimal zinsEff, int ppy, CalculationTargets art, CalculationMode mode, decimal Zins)
        {
            decimal Result;
            CalculateRateCore(ref aquisitionCost, ref firstPayment, ref term, ref rate, ref residualValue, ref interest, ref zinsEff, ppy, art, mode);
            Result = interest;
            return Result;
        }*/

        public static decimal CalculateRateWithVorteil(decimal depot, int term, decimal amount, decimal rateBrutto, decimal Ust, long SysINTSTRCT, decimal depotZins)
        {
            decimal JahreZins;
            decimal Vorteil = 0;

            if (depotZins != 0)
            {
                JahreZins = (depotZins / 100) / 12;
                Vorteil = (depot * JahreZins);
            }

            decimal rval = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(rateBrutto, Ust) - Vorteil;
            rval = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval, Ust);
            _Log.Debug("Vorteil: " + Vorteil + " DepotZins: " + depotZins + " RateBrutto: " + rateBrutto + " rateBrutto mit Vorteil: " + rval);
            return rval;
        }

        public static decimal ChangeResiudalValue(decimal restwertBrutto, decimal restwertvorschlagBrutto)
        {
            decimal Result;

            if (restwertBrutto <= 0)
            {
                Result = restwertvorschlagBrutto;
            }
            else
            {
                Result = restwertBrutto;
            }

            return Result;
        }

        /// <summary>
        /// Die Depotleistung darf 50 % des Netto-Anschaffungswertes inkl. NoVA sowie den Restwert exklusive MWSt. nicht übersteigen
        /// </summary>
        /// <param name="anschaffungswertnetto"></param>
        /// <param name="restwertnetto"></param>
        /// <param name="nova"></param>
        /// <param name="maxp"></param>
        /// <returns></returns>
        public static decimal CalculateDepotFirstCondition(decimal anschaffungswertnetto, decimal restwertnetto, decimal nova, decimal maxp)
        {
            decimal Result;
            Result = ((anschaffungswertnetto) * maxp) / 100;

            if (Result > restwertnetto)
            {
                Result = restwertnetto;
            }

            return Result;
        }

        /// <summary>
        /// ... die Gesamtsumme von Mietvorauszahlung und Depotleistung 50 % des Netto-Anschaffungswertes nicht überschreiten darf
        /// und auch bei einer Kombination der beiden Eigenleistungsformen nicht mehr als 30 % Mietvorauszahlung möglich sind.
        /// </summary>
        /// <param name="mietvorauszahlung"></param>
        /// <param name="depot"></param>
        /// <returns></returns>
        public static decimal CalculateDepotSecondCondition(decimal mietvorauszahlung, decimal depot)
        {
            return mietvorauszahlung + depot;
        }


        public static decimal CalculateEffectiveInterest(decimal nominalInterest, int ppy)
        {
            decimal Ppy = ppy;

            double d = System.Math.Pow((double)(1 + (nominalInterest / (100 * Ppy))), (double)Ppy);

            d = (d - 1) * 100;

            return Convert.ToDecimal(d);
        }

        public static decimal CalculateNominalInterest(decimal effectiveInterest, int ppy)
        {
            double Ppy = ppy;

            double d = System.Math.Pow((double)((effectiveInterest / 100) + 1), (double)(1 / Ppy));
            d = (d - 1) * 100 * Ppy;

            return Convert.ToDecimal(d);
        }

        private static decimal DeliverVerZinsung(QUOTEDao qDao, PRParamDao prparamDao, CalculationDto calculationDto, long sysPEROLE, long sysBRAND, decimal defaultZins)
        {
            decimal depotAbschlag = (decimal)qDao.getQuote(QUOTEDao.QUOTE_LEAKALK_DEPOTABSCHLAG);
            decimal rval = defaultZins;
            PRPARAMDto par = prparamDao.DeliverPrParam(calculationDto.SysPrProduct, calculationDto.SysObTyp, sysPEROLE, sysBRAND, PRParamDao.CnstDepotAbschlagFieldMeta, calculationDto.SysObArt, false);
            if (par != null && par.DEFVALP.HasValue)
            {
                depotAbschlag = par.DEFVALP.Value;
                _Log.Debug("Depot-Verzinsung Abschlag über Produkt-Parameter: " + par.DEFVALP.Value);
            }
            rval = defaultZins - depotAbschlag;
            _Log.Debug("Default-Zins: " + defaultZins + " DepotZins: " + rval + " Abschlag: " + depotAbschlag);
            if (rval < 0)
            {
                rval = 0;
                _Log.Warn("Default-Zins negative, set to 0!");
            }
            return rval;

        }

        public static decimal CalculatePercent(decimal? baseValue, decimal? value)
        {
            decimal ResultValue;
            ResultValue = 0.0M;

            if (baseValue != null && baseValue != 0)
            {
                ResultValue = value.GetValueOrDefault(0.0M) * 100M / baseValue.Value;
            }

            return ResultValue;
        }
        public static decimal CalculateValue(decimal? baseValue, decimal? value)
        {
            decimal ResultValue;
            ResultValue = 0.0M;

            ResultValue = ((value.GetValueOrDefault(0.0M) / 100M) * baseValue.GetValueOrDefault(0.0M));

            return ResultValue;
        }
        /// <summary>
        /// Returns true when the value exceeds the boundaries, returning the missed bound
        /// </summary>
        /// <param name="pRate"></param>
        /// <param name="currentValue"></param>
        /// <param name="bound"></param>
        /// <returns></returns>
        private static bool checkParam(PRPARAMDto pRate, decimal currentValue, ref decimal bound)
        {
            if (pRate != null)
            {
                if (pRate.TYP == 1)
                {

                    if (currentValue < pRate.MINVALP)
                    {
                        bound = pRate.MINVALP.Value;
                        return true;
                    }
                    else if (currentValue > pRate.MAXVALP)
                    {
                        bound = pRate.MAXVALP.Value;
                        return true;
                    }
                }
                else
                {
                    if (currentValue < pRate.MINVALN)
                    {
                        bound = pRate.MINVALN.Value;
                        return true;
                    }
                    else if (currentValue > pRate.MAXVALN)
                    {
                        bound = pRate.MAXVALN.Value;
                        return true;
                    }
                }
            }
            return false;
        }
#endregion
    }
}