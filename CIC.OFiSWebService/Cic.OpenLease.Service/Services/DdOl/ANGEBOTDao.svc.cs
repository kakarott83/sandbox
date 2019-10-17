// OWNER MK, 04-02-2009
namespace Cic.OpenLease.Service.DdOl
{
    #region Using
    using Cic.One.DTO.Mediator;
    using Cic.OpenLease.Service.Provision;
    using Cic.OpenLease.Service.Services.DdOl;
    using Cic.OpenLease.Service.Services.DdOl.BO;
    using Cic.OpenLease.Service.Versicherung;
    using Cic.OpenLease.ServiceAccess;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.DAO.Prisma;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Config;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using CIC.Database.OW.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    #endregion

    /// <summary>
    /// Datenzugriffsobjekt für Angebote
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public sealed partial class ANGEBOTDao : Cic.OpenLease.ServiceAccess.DdOl.IANGEBOTDao
    {
        #region Private constants

        private const int CnstFinKzSet = 1;
        private const int CnstFinKzNotSet = 0;

        private static CacheDictionary<String, int> searchCache = CacheFactory<String, int>.getInstance().createCache(1000 * 30);
        private static CacheDictionary<String, int> countCache = CacheFactory<String, int>.getInstance().createCache(1000 * 30);
        private static CacheDictionary<String, List<long>> resultIdCache = CacheFactory<String, List<long>>.getInstance().createCache(1000 * 30);

        private static CacheDictionary<String, PROVTARIFDto[]> tarifeCache = CacheFactory<String, PROVTARIFDto[]>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, ControlDto> prodKdtypCache = CacheFactory<String, ControlDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        

        #endregion

        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region IANGEBOTDao Members

        public static void flushSearchCache()
        {
            searchCache.Clear();
            countCache.Clear();
            resultIdCache.Clear();
        }

        #region Standard

        private class ITRSDVInfo
        {
            public int KDTYP { get; set; }
            public DateTime? GEBDATUM { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printData"></param>
        /// <returns></returns>
        public String createHtmlReport(PrintDto printData)
        {
            if (printData.vtDto == null && !printData.angebot.SYSPRPRODUCT.HasValue) return "";
            HtmlReportBo htmlReportBo = null;
            if (printData.vtDto != null)
            {
                htmlReportBo = new HtmlReportBo(new Cic.OpenLease.Service.Services.DdOl.DAO.AngebotVARTTemplateDao());
                return htmlReportBo.CreateHtmlReport(printData, "vertragsuebersicht");
            }
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            //long sysvpperson = ServiceValidator.VpSysPERSON.GetValueOrDefault();
            long sysvkperson = ServiceValidator.SysPERSON;
            long sysBRAND = ServiceValidator.SysBRAND;

            return quickPrint(printData, sysvkperson, sysBRAND, ServiceValidator.SysPEROLE);
        }

        public static string quickPrint(PrintDto printData, long sysvkperson, long sysBRAND, long sysperole)
        {
            String vartCode = "KREDIT_3WEGEFIN";

            ANGEBOTAssembler.validateInsurances(printData.angebot);
            decimal ust = LsAddHelper.GetTaxRate(printData.angebot.SYSVART);
            decimal nova = printData.angebot.ANGOBININOVA_P.HasValue ? printData.angebot.ANGOBININOVA_P.Value : 0;
            printData.UST = ust;
            printData.SAPAKETEBRUTTO = 0;
            if (printData.angebot.ANGOBPAKETEBRUTTO.HasValue)
                printData.SAPAKETEBRUTTO += printData.angebot.ANGOBPAKETEBRUTTO.Value;
            if (printData.angebot.ANGOBSONZUBBRUTTO.HasValue)
                printData.SAPAKETEBRUTTO += printData.angebot.ANGOBSONZUBBRUTTO.Value;
            printData.UEBZUL = 0;
            if (printData.angebot.ANGOBUEBERFUEHRUNGBRUTTO.HasValue)
                printData.UEBZUL += printData.angebot.ANGOBUEBERFUEHRUNGBRUTTO.Value;
            if (printData.angebot.ANGOBZULASSUNGBRUTTO.HasValue)
                printData.UEBZUL += printData.angebot.ANGOBZULASSUNGBRUTTO.Value;

            if (printData.angebot.SYSPRPRODUCT.HasValue)
            {
                Cic.OpenOne.Common.DAO.Prisma.PrismaDao pdao = new CachedPrismaDao();
                printData.angebot.HIST_SYSPRPRODUCT = pdao.getProduct(printData.angebot.SYSPRPRODUCT.Value, "de-DE").NAME;
            }
            try
            {
                printData.ANGOBKMGESAMT = printData.angebot.ANGOBJAHRESKM.Value * (printData.angebot.ANGKALKLZ.Value / 12.0);
            }
            catch (Exception exc) { }

            printData.GESAMTBETRAG = 0;
            if (printData.angebot.ANGKALKGESAMTBRUTTO.HasValue)
                printData.GESAMTBETRAG += printData.angebot.ANGKALKGESAMTBRUTTO.Value;
            if (printData.angebot.ANGKALKSZBRUTTO.HasValue)
                printData.GESAMTBETRAG += printData.angebot.ANGKALKSZBRUTTO.Value;

            printData.VERZINSUNGSART = "variabel (Anpassung quartalsmäßig, Basis 3-Monats-Euribor)";

            if (printData.angebot.ANGKALKZINSTYP == 1)
                printData.VERZINSUNGSART = "fix";
            DateTime bis = (DateTime.Today).Date.AddDays(30);

            printData.GUELTIGBIS = bis.ToString("dd.MM.yyyy");
            printData.ERSTZULASSUNG = "";
            printData.HEUTE = DateTime.Today.ToString("dd.MM.yyyy");
            if (printData.angebot.ANGOBINIERSTZUL.HasValue)
                printData.ERSTZULASSUNG = printData.angebot.ANGOBINIERSTZUL.Value.ToString("dd.MM.yyyy");
            printData.BRAND = "";

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    VSTYPDao vsdao = new VSTYPDao(context);
                    List<InsuranceDto> resultInsurances = new List<InsuranceDto>();
                    foreach (InsuranceDto ins in printData.angebot.ANGVSPARAM)
                    {
                        try
                        {
                            if (ins.InsuranceParameter.CODEMETHOD == VSCalcFactory.Cnst_CALC_KASKO)
                            {
                                if (!ANGEBOTAssembler.checkFlag(printData.angebot.ANGKALKFSVKFLAG)) continue;
                                printData.KASKONAME = vsdao.getVsTyp(ins.InsuranceParameter.SysVSTYP).BESCHREIBUNG;
                                printData.ZUSATZGESAMT += ins.InsuranceResult.Praemie;

                            }
                            if (ins.InsuranceParameter.CODEMETHOD == VSCalcFactory.Cnst_CALC_HAFTPFLICHT)
                            {
                                if (!ANGEBOTAssembler.checkFlag(printData.angebot.ANGKALKFSHPFLAG)) continue;
                                printData.HPNAME = vsdao.getVsTyp(ins.InsuranceParameter.SysVSTYP).BESCHREIBUNG;
                                printData.ZUSATZGESAMT += ins.InsuranceResult.Praemie;
                            }
                            if (ins.InsuranceParameter.CODEMETHOD == VSCalcFactory.Cnst_CALC_INSASSEN)
                            {
                                if (!ANGEBOTAssembler.checkFlag(printData.angebot.ANGKALKFSINSASSENFLAG)) continue;
                                printData.IUVNAME = vsdao.getVsTyp(ins.InsuranceParameter.SysVSTYP).BESCHREIBUNG;
                                printData.ZUSATZGESAMT += ins.InsuranceResult.Praemie;
                            }
                            if (ins.InsuranceParameter.CODEMETHOD == VSCalcFactory.Cnst_CALC_GAP)
                            {
                                if (!ANGEBOTAssembler.checkFlag(printData.angebot.ANGKALKFSGAPFLAG)) continue;
                                printData.IUVNAME = vsdao.getVsTyp(ins.InsuranceParameter.SysVSTYP).BESCHREIBUNG;
                                printData.ZUSATZGESAMT += ins.InsuranceResult.Praemie;
                            }
                            if (ins.InsuranceParameter.CODEMETHOD == VSCalcFactory.Cnst_CALC_RECHTSSCHUTZ)
                            {
                                if (!ANGEBOTAssembler.checkFlag(printData.angebot.ANGKALKFSRECHTSCHUTZFLAG)) continue;
                                printData.RSVNAME = vsdao.getVsTyp(ins.InsuranceParameter.SysVSTYP).BESCHREIBUNG;
                                printData.ZUSATZGESAMT += ins.InsuranceResult.Praemie;
                            }
                            if (ins.InsuranceParameter.CODEMETHOD == VSCalcFactory.Cnst_CALC_RESTSCHULD)
                            {
                                if (!ANGEBOTAssembler.checkFlag(printData.angebot.ANGKALKFSRSVFLAG)) continue;
                                printData.RSDVNAME = vsdao.getVsTyp(ins.InsuranceParameter.SysVSTYP).BESCHREIBUNG;
                                printData.RSDVPRAEMIE = ins.InsuranceResult.Praemie;
                                //printData.ZUSATZGESAMT += ins.InsuranceResult.Praemie;
                            }
                            resultInsurances.Add(ins);
                        }
                        catch (Exception ve)
                        {
                            _Log.Error("Error printing Insurance", ve);
                        }
                    }
                    printData.angebot.ANGVSPARAM = resultInsurances.ToArray();
                    printData.ZUSATZGESAMT += printData.angebot.ANGKALKRATEBRUTTO.Value;

                    String[] order = new String[] { "KASKO", "HP", "IUV", "RSV", "RSDV", "GAP" };

                    List<InsuranceDto> insurances = new List<InsuranceDto>();
                    foreach (String code in order)
                    {
                        foreach (InsuranceDto ins in printData.angebot.ANGVSPARAM)
                        {
                            if (ins.InsuranceParameter.CODEMETHOD == null)
                            {
                                insurances.Add(ins);
                                break;
                            }
                            if (ins.InsuranceParameter.CODEMETHOD.Equals(code))
                            {
                                insurances.Add(ins);
                                break;
                            }
                        }

                    }
                    printData.angebot.ANGVSPARAM = insurances.ToArray();

                    CalculationDao calcDao = new CalculationDao(context);

                    ObartDTO oa = calcDao.getObArt(printData.angebot.SYSOBART.Value);
                    printData.FZART = oa.NAME;


                }
            }
            catch (Exception ex)
            {
                _Log.Error("Error printing ", ex);
            }
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {

                    //printData.VKINFO = context.ExecuteStoreQuery<VKInfo>("select     vkBerater.VorName  ,vkBerater.name,VP.Name abteilung ,vkBerater.Telefon,vkBerater.Email  from  PERSON  vkBerater, vp where   vp.sysperson=" + sysvpperson + " and vkBerater.sysperson=" + sysvkperson, null).FirstOrDefault();
                    printData.VKINFO = context.ExecuteStoreQuery<VKInfo>("select     vkBerater.VorName  ,vkBerater.name,vkBerater.Telefon,vkBerater.Email  from  PERSON  vkBerater where vkBerater.sysperson=" + sysvkperson, null).FirstOrDefault();
                    if (printData.VKINFO == null)
                        printData.VKINFO = new VKInfo();
                    //Zuerst Filiale
                    printData.VKINFO.ABTEILUNG = context.ExecuteStoreQuery<String>("select person.name abteilung from person,perole, roletype where person.sysperson=perole.sysperson and perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=" + sysperole + ") and roletype.typ=8 and roletype.code='FIL'", null).FirstOrDefault();
                    //Ansonsten Händler
                    if (String.IsNullOrEmpty(printData.VKINFO.ABTEILUNG))
                        printData.VKINFO.ABTEILUNG = context.ExecuteStoreQuery<String>("select person.name abteilung from person,perole, roletype where person.sysperson=perole.sysperson and perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole=" + sysperole + ") and roletype.typ=6 and roletype.code='HD'", null).FirstOrDefault();

                    vartCode = context.ExecuteStoreQuery<String>("select  code from vart where sysvart=" + printData.angebot.SYSVART, null).FirstOrDefault();
                    String brandName = context.ExecuteStoreQuery<String>("select name from brand where sysbrand=" + sysBRAND, null).FirstOrDefault();
                    printData.BRAND = brandName;
                    //printData.BRAND = context.ExecuteStoreQuery<String>("select brand.name from brand,prhgroupm,prbrandm,perole where brand.sysbrand=prbrandm.sysbrand and prbrandm.sysprhgroup=prhgroupm.sysprhgroup and perole.sysperole=prhgroupm.sysperole and perole.sysperson=" + sysvkperson, null).FirstOrDefault();
                    //Automarke des Fahrzeugs
                    printData.BRANDINFO = context.ExecuteStoreQuery<String>("select bezeichnung from obtyp where level=4 and sysobtypp is not null connect by prior sysobtypp=sysobtyp start with sysobtyp=" + printData.angebot.SYSOBTYP, null).FirstOrDefault();


                    if ("LEASING_KILOMETER".Equals(vartCode))
                    {
                        printData.angebot.ANGOBSATZMINDERKM = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MINDER_KM_SATZ);
                        printData.angebot.ANGOBSATZMEHRKM = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MEHR_KM_SATZ);

                        printData.angebot.ANGOBSATZMINDERKMBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(printData.angebot.ANGOBSATZMINDERKM.Value, ust);
                        printData.angebot.ANGOBSATZMEHRKMBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(printData.angebot.ANGOBSATZMEHRKM.Value, ust);
                    }


                }
            }
            catch (Exception ex)
            {
                _Log.Error("Error printing ", ex);
            }

            printData.IMGSUFFIX = printData.BRAND.ToLower();
            HtmlReportBo htmlReportBo = new HtmlReportBo(new Cic.OpenLease.Service.Services.DdOl.DAO.AngebotVARTTemplateDao());
            return htmlReportBo.CreateHtmlReport(printData, vartCode);
        }

        /// <summary>
        /// Validates the RSDV against quoted rules
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<String> validateRSDV(ANGEBOTDto dto)
        {
            List<String> results = new List<String>();

            //do not check for unsaved angebot
            if (!dto.SYSID.HasValue || dto.SYSID.Value == 0)
                return results;
         

            using (DdOlExtended Context = new DdOlExtended())
            {            

                ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                try
                {
                    //Delivers in Queue VSRULE records where F02=Syswfmmemo with the Message
                    One.DTO.QueueResultDto qr = BOS.getQueueData(dto.SYSID.Value, "ANGEBOT", new String[] { "VSRULE" }, "EINREICHDIALOG_VERS_RULESET", null, ServiceValidator.SYSWFUSER, null);
                    if (qr != null && qr.queues != null)
                    {
                        QueueRecordDto[] recs = qr.queues[0].records;

                        if (recs != null)
                        {
                            foreach (QueueRecordDto rec in recs)
                            {
                                if (rec.values != null && rec.values.Count() > 0)
                                {
                                    String syswfmmemo = (from f in rec.values
                                                         where f.VariableName.Equals("F02")
                                                         select f.Value).FirstOrDefault();
                                    if (syswfmmemo != null && syswfmmemo.Length > 0)
                                    {
                                        String rtext = Context.ExecuteStoreQuery<String>("select notizmemo from wfmmemo where syswfmmemo=" + syswfmmemo).FirstOrDefault();
                                        if (rtext != null && rtext.Length > 0)
                                            results.Add(rtext.Trim());
                                    }
                                }
                            }
                        }
                    }
                }catch(Exception e)
                {
                    results.Add("Fehler bei der Auswertung der Versicherungsbedingungen: "+e.Message);
                    _Log.Error("Fehler bei der Auswertung der Versicherungsbedingungen", e);
                }
               
            }
            return results;

        }


        /// <summary>
        /// Validates the GAP against quoted rules
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<String> validateGAP(ANGEBOTDto dto)
        {
            List<String> results = new List<String>();
            return results;
            /*
            if (!dto.ANGKALKFSGAPFLAG.HasValue || (dto.ANGKALKFSGAPFLAG.Value == 0))
                return results;


            using (DdOlExtended Context = new DdOlExtended())
            {

                InsuranceDto ins = (from v in dto.ANGVSPARAM
                                    where v.InsuranceParameter.CODEMETHOD.Equals("GAP")
                                    select v).FirstOrDefault();
                if (ins == null) return results;
                VSTYPDao vsd = new VSTYPDao(Context);
                VSTYP vsType = vsd.getVsTyp(ins.InsuranceParameter.SysVSTYP);
                if (vsType == null) return results;
                String vstypcode = vsType.CODE;


                //validate Fahrzeugalter:
                String maxageCode = QUOTEDao.QUOTE_GAP_MAXEIN_PREFIX + vstypcode;
                String maxage2Code = QUOTEDao.QUOTE_GAP_MAXEND_PREFIX + vstypcode;
                String minageCode = QUOTEDao.QUOTE_GAP_MINEIN_PREFIX + vstypcode;

                decimal maxage = QUOTEDao.deliverQuotePercentValueByName(maxageCode);
                decimal maxage2 = QUOTEDao.deliverQuotePercentValueByName(maxage2Code);
                decimal minage = QUOTEDao.deliverQuotePercentValueByName(minageCode);

                if(maxage==0)
                    maxage = 7 * 12;//antragsbeginn autoalter
                if(maxage2==0)
                 maxage2 = 11 * 12; //antragsende autoalter

                DateTime testDate = dto.ANGOBLIEFERUNG != null && dto.ANGOBLIEFERUNG.HasValue ? dto.ANGOBLIEFERUNG.Value : DateTime.Now;
                if (dto.ANGOBINIERSTZUL != null && dto.ANGOBINIERSTZUL.HasValue)
                    testDate = dto.ANGOBINIERSTZUL.Value;

                //alter Antragstellung:
                int age = 0;
                for (age = 0; testDate.AddMonths(age + 1).CompareTo(DateTime.Now) <= 0; age++) ;


                if (age > maxage)
                    results.Add("Der Abschluss einer GAP-Versicherung ist nur für Fahrzeuge jünger als " + (int)(maxage / 12) + " Jahre bei Antragstellung möglich.<br/>");

                //alter Antragstellung:
                age += (int)ins.InsuranceParameter.Laufzeit;
                if (age > maxage2)
                    results.Add("Der Abschluss einer GAP-Versicherung ist nur für Fahrzeuge jünger als " + (int)(maxage2 / 12) + " Jahre bei Versicherungsende möglich.<br/>");





                long sysquote = DdOlExtended.getKey(vsType.QUOTEAliasReference.EntityKey);
                if (sysquote == 0)
                    results.Add("SYSQUOTE in GAP-Versicherung für max. Finanzrate nicht konfiguriert für VSTYP " + vsType.SYSVSTYP);
                else
                {
                    decimal maxrate = QUOTEDao.deliverQuotePercentValue(sysquote);
                    if (dto.ANGKALKRATEGESAMTBRUTTO > maxrate)
                        results.Add("Der Abschluss der gewählten GAP-Versicherung ist nur für eine max. Finanzrate von " + maxrate + " freigegeben.");
                }

            }
            return results;*/

        }
        /// <summary>
        /// returns the value or 0 when null or no value set
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static decimal getValue(decimal? val)
        {
            if (val == null) return 0;
            if (!val.HasValue) return 0;
            return val.Value;
        }

        /// <summary>
        /// Validate needed Fields for Submit and Print of Offer
        /// </summary>
        /// <param name="sysid">id of the offer</param>
        /// <returns></returns>
        public List<ValidationResultDto> validateAngebotFields(long sysid)
        {
            List<ValidationResultDto> rval = new List<ValidationResultDto>();
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            using (DdOlExtended Context = new DdOlExtended())
            {


                try
                {
                    ANGEBOTDto dto = Deliver(sysid);
                    long syskdtyp = Context.ExecuteStoreQuery<long>("select syskdtyp from it where sysit=" + dto.SYSIT, null).FirstOrDefault();
                    //Bei einem Hauptantragsteller als Unternehmen können bis zu 2 Vertretungsberechtigte erfasst werden.
                    /*
                     * > mindestens ein Vertretungsberechtigter muss erfasst werden
                        > hat der erste Vertretungsberechtigte nur Gesamtvertretungsrechte (ID=GESAMTVERTRETUNG) dann ist auch ein zweiter Vertretungsberechtigter zu erfassen (Pflicht)
                     * */
                    if (syskdtyp == 3)//Unternehmen, ob VB oder nicht wird im FO geregelt, dh. falls wir hier einen VB finden müssen wir nur schauen ob es min 1 ist. Falls es einer ist muss dieser EINZEL sein, sosnt müssen es 2 sein
                    {
                        String queryVBcheck = "select angobsich.option1 from angobsich, sichtyp,angebot,it where angobsich.sysit=it.sysit and sichtyp.syssichtyp=angobsich.syssichtyp and sichtyp.rang in (140) and angebot.sysid=angobsich.sysvt and angebot.sysid=" + sysid;
                        List<String> vb = Context.ExecuteStoreQuery<String>(queryVBcheck, null).ToList<String>();

                        if (vb == null || vb.Count == 0)
                        {
                            ; //all ok, checkd from FO
                            //   rval.Add(new ValidationResultDto() { valid = false, Message = "Unternehmen benötigen zwingend einen Vertretungsberechtigten" });
                        }
                        else if (vb.Contains("EINZELVERTRETUNG") || vb.Contains("EINZELVOLLMACHT"))//einer ist einzelvertretung, oder EINZELVOLLMACHT alles ok
                        {
                        }
                        else if (vb.Count == 1)	//Gemeinsamvertretungsberechtigung - min zwei
                        {
                            rval.Add (new ValidationResultDto() { valid = false, Message = "Zweiter Vertretungsberechtigter benötigt" });
                        }

						if (vb != null)
						{// CHECK if ALL VB-ART fields are filled with valid entries
							bool allVbartValid = true;
							foreach (string vbart in vb)
							{
								if (allVbartValid)
									allVbartValid = (vbart != null) && (vbart.Equals ("GESAMTVERTRETUNG") || vbart.Equals ("EINZELVERTRETUNG") || vbart.Equals ("EINZELVOLLMACHT")); 
							}
							if ( ! allVbartValid)
								rval.Add (new ValidationResultDto () { valid = false, Message = "Es muss für JEDEN Vertretungsberechtigten eine Berechtigungsart definiert sein!" });
						}
                    }
                    VSTYPDao vsdao = new VSTYPDao(Context);

                    #region for all

                    // CALCULATION
                    PRParamDao prparamDao = new PRParamDao(Context);
                    PRPARAMDto parBGIntern = prparamDao.DeliverPrParam(dto.SYSPRPRODUCT.GetValueOrDefault(), dto.SYSOBTYP.GetValueOrDefault(), ServiceValidator.SysPEROLE, dto.SYSBRAND.GetValueOrDefault(), PRParamDao.CnstFinanzierungssummeMin, dto.SYSOBART.GetValueOrDefault(), false);
                    if(parBGIntern!=null)
                    {
                        if(parBGIntern.MINVALN>dto.ANGKALKBGINTERN.Value)
                            rval.Add(new ValidationResultDto() { valid = false, Message = "Minimale Finanzierungssumme für Produkt unterschritten" });
                    }
                    PRPARAMDto parRate = prparamDao.DeliverPrParam(dto.SYSPRPRODUCT.GetValueOrDefault(), dto.SYSOBTYP.GetValueOrDefault(), ServiceValidator.SysPEROLE, dto.SYSBRAND.GetValueOrDefault(), PRParamDao.CnstRateMin, dto.SYSOBART.GetValueOrDefault(), false);
                    if (parRate != null)
                    {
                        if (parRate.MINVALN > dto.ANGKALKRATE.Value)
                            rval.Add(new ValidationResultDto() { valid = false, Message = "Minimale Rate für Produkt unterschritten" });
                    }
                    //if (dto.ANGKALKZINSTYP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Tarifkennzeichen/Version erforderlich" });

                    bool isNoLeasingKredit = (dto.ANGKALKSYSKALKTYP == 49 || dto.ANGKALKSYSKALKTYP == 54);


                    if (dto.ANGOBAHKEXTERNBRUTTO == null || dto.ANGOBAHKEXTERNBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Anschaffungswert brutto erforderlich" });
                    if (dto.ANGKALKBGEXTERNBRUTTO == null || dto.ANGKALKBGEXTERNBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Finanzierungsbetrag erforderlich" });
                    if (dto.ANGOBJAHRESKM == null || dto.ANGOBJAHRESKM.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Jahreskilometer erforderlich" });
                    if (dto.ANGKALKLZ == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Laufzeit des Vertrages erforderlich" });
                    //#4095 if ((dto.ANGKALKRATEBRUTTO == null || dto.ANGKALKRATEBRUTTO == 0) && !isNoLeasingKredit) rval.Add(new ValidationResultDto() { valid = false, Message = "Monatliche Leasing- bzw. Kreditrate inkl. MwSt." });
                    if (dto.ANGKALKRGGEBUEHR == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Rechtsgeschäftsgebühr erforderlich" });

                    //MwStDaten
                    var AngebotMwSt = (from Angebot in Context.ANGEBOT
                                       where Angebot.SYSID == sysid
                                       select Angebot.SYSMWST).FirstOrDefault();
                    if (AngebotMwSt == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MwSt Satz erforderlich" });

                    if (dto.ANGKALKZINSEFF == null || dto.ANGKALKZINS == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Zinssatz der Kalkulation erforderlich" });
                    if (dto.ANGKALKZINSTYP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Zinsart fix/variabel erforderlich" });
                    if (syskdtyp == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Kundenart (GK/EK) in der Kalkulation" });

                    bool isNew = OBARTHelper.isOfType(Context, dto.SYSOBART.GetValueOrDefault(), 0) || OBARTHelper.isOfType(Context, dto.SYSOBART.GetValueOrDefault(), 3);

                    // Vehicle Data
                    if (dto.SYSOBTYP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Objektgruppe/vehicle nature (auto or moto) erforderlich" });
                    if (dto.SYSOBART == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Flag: NW, VZW, GW erforderlich" });
                    if (dto.ANGOBHERSTELLER == null || dto.ANGOBHERSTELLER.Length == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Markenbeschreibung (Hersteller) erforderlich" });
                    if (dto.ANGOBTYP == null || dto.ANGOBTYP.Length == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Fahrzeugbeschreibung erforderlich" });
                    if (dto.ANGOBFABRIKAT == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Modell/Reihe (aus Fahrzeugtabelle) erforderlich" });
                    if (dto.ANGOBNOVA == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Nova Satz des Fahrzeuges erforderlich" });
                    if (dto.ANGOBSONZUBBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Sonderausstattung brutto erforderlich" });
                    if (dto.ANGOBPAKETEBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "SA-Pakete brutto erforderlich" });
                    if (dto.ANGOBSONZUBRABATTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Nachlass Betrag für Sonderausstattung + Fhzg. erforderlich" });
                    if (dto.ANGOBSONZUBRABATTOP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Nachlass in % für Sonderausstattung + Fhzg. erforderlich" });
                    if (dto.ANGOBZUBEHOER == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Zubehör erforderlich" });
                    if (dto.ANGOBZUBEHOERRABATTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Nachlass Betrag für Zubehör erforderlich" });
                    if (dto.ANGOBZUBEHOERRABATTOP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Nachlass in % für Zubehör erforderlich" });

                    if( getValue(dto.ANGKALKVERRECHNUNG)>0 && dto.ANGABLFLAGINTEXT<1)
                    {
                        rval.Add(new ValidationResultDto() { valid = false, Message = "Bei Ablösung eines Vorkredits muß das Kreditinstitut gewählt werden." });
                    }
                    if (getValue(dto.ANGKALKVERRECHNUNG) > 0 && dto.ANGABLFLAGINTEXT >1)
                    {
                        IBANValidator iv = new IBANValidator();
                        if (iv.checkIBAN(dto.ANGABLIBAN).error != IBANValidationErrorType.NoError)
                        {
                            rval.Add(new ValidationResultDto() { valid = false, Message = "Bei Ablösung über ein fremdes Kreditinstitut muß eine gültige IBAN angegeben werden." });
                        }
                    }

                    //#9075
                    if ((getValue(dto.ANGOBAHKEXTERNBRUTTO) - getValue(dto.ANGKALKSZBRUTTO)) <= getValue(dto.ANGKALKRWKALKBRUTTO))
                    {
                        rval.Add(new ValidationResultDto() { valid = false, Message = "Kapital (AHK) minus MVZ bzw. Anzahlung muss größer als der Restwert sein" });
                    }

                    #endregion


                    if (dto.ANGKALKSYSKALKTYP.HasValue && dto.ANGKALKSYSKALKTYP > 0)
                    {
                        switch (dto.ANGKALKSYSKALKTYP)
                        {

                            case (44):  //   RestwertLeasing (KalkTyp.SYSKALKTYP == 44)
                                if (dto.ANGKALKSZBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ inkl. MwSt.erforderlich" });
                                if (dto.ANGKALKSZBRUTTOP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ bzw. Anzahlung in % erforderlich" });
                                if (dto.ANGKALKDEPOT == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Depot (BR, NL, RWL) erforderlich" });
                                if (dto.ANGKALKDEPOTP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "% Depot erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTO == null || dto.ANGKALKRWBASEBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Restwert- bzw. Zielratenvorschlag in € erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTOP == null || dto.ANGKALKRWBASEBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "% Restwert- bzw. Zielratenvorschlag % (Select und RWL) erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTO == null || dto.ANGKALKRWKALKBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Eigener RW/Zielrate in € (Select, Zielratenkr., RWL) = für die Kalkulation verwendeter RW bzw. Zielrate erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTOP == null || dto.ANGKALKRWKALKBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "%Eigener RW/Zielrate % (Select, Zielratenkr., RWL) erforderlich" });
                                if (dto.SYSPROVTARIF == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Variante der Kalkulation ( z.b.: 0,5%) erforderlich" });

                                // Vehicle Data

                                if (dto.ANGOBKW == null) rval.Add(new ValidationResultDto() { valid = false, Message = "KW des Fahrzeuges erforderlich" });
                                if (dto.ANGOBGRUNDBRUTTO == null || dto.ANGOBGRUNDBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Fahrzeuglistenpreis brutto erforderlich" });

                                break;

                            case (42):  // NutzenLeasing (KalkTyp.SYSKALKTYP == 42)
                                if (dto.ANGKALKSZBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ inkl. MwSt.erforderlich" });
                                if (dto.ANGKALKSZBRUTTOP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ bzw. Anzahlung in % erforderlich" });
                                if (dto.ANGKALKDEPOT == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Depot (BR, NL, RWL) erforderlich" });
                                if (dto.ANGKALKDEPOTP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "% Depot erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTO == null || dto.ANGKALKRWBASEBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Restwert- bzw. Zielratenvorschlag in € erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTOP == null || dto.ANGKALKRWBASEBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "% Restwert- bzw. Zielratenvorschlag % (Select und RWL) erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTO == null || dto.ANGKALKRWKALKBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Eigener RW/Zielrate in € (Select, Zielratenkr., RWL) = für die Kalkulation verwendeter RW bzw. Zielrate erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTOP == null || dto.ANGKALKRWKALKBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "%Eigener RW/Zielrate % (Select, Zielratenkr., RWL) erforderlich" });
                                if (dto.ANGOBSATZMEHRKMBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Mehrkilometer inkl. MwSt." });
                                if (dto.SYSPROVTARIF == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Variante der Kalkulation ( z.b.: 0,5%) erforderlich" });

                                // Vehicle Data

                                if (dto.ANGOBKW == null) rval.Add(new ValidationResultDto() { valid = false, Message = "KW des Fahrzeuges erforderlich" });
                                if (!isNew && dto.ANGOBINIKMSTAND == null) rval.Add(new ValidationResultDto() { valid = false, Message = "km-Stand bei Übernahme erforderlich" });
                                if (!isNew && dto.ANGOBINIERSTZUL == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Erstzulassung erforderlich" });
                                if (dto.ANGOBGRUNDBRUTTO == null || dto.ANGOBGRUNDBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Fahrzeuglistenpreis Brutto erforderlich" });




                                break;


                            case (48):  // BUSINESS rent -- Mietvertrag

                                if (dto.ANGKALKDEPOT == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Depot (BR, NL, RWL) erforderlich" });
                                if (dto.ANGKALKDEPOTP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "% Depot erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTO == null || dto.ANGKALKRWBASEBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Restwert- bzw. Zielratenvorschlag in € erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTOP == null || dto.ANGKALKRWBASEBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "% Restwert- bzw. Zielratenvorschlag % (Select und RWL) erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTO == null || dto.ANGKALKRWKALKBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Eigener RW/Zielrate in € (Select, Zielratenkr., RWL) = für die Kalkulation verwendeter RW bzw. Zielrate erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTOP == null || dto.ANGKALKRWKALKBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "%Eigener RW/Zielrate % (Select, Zielratenkr., RWL) erforderlich" });

                                if (dto.ANGOBSATZMEHRKMBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Mehrkilometer inkl. MwSt." });



                                // VERSICHERUNG & SERVICE
                                foreach (InsuranceDto insurance in dto.ANGVSPARAM)
                                {
                                    VSTYP vstyp = vsdao.getVsTyp(insurance.InsuranceParameter.SysVSTYP);


                                    if (vstyp == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Service/Versicherungs-Code ungültig: " + insurance.InsuranceParameter.CODEMETHOD });

                                    /*    if (insurance.InsuranceParameter.CODEMETHOD == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Service/Versicherungs-Code erforderlich" });
                                        if (insurance.InsuranceResult.Praemie == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "monatliche Versicherungs- bzw. Serviceraten (einzelne) erforderlich für " + insurance.InsuranceParameter.CODEMETHOD });
                                        if (insurance.InsuranceParameter.CODEMETHOD != null)
                                        {
                                            //Haftpflichtversicherung
                                            if ("HP".Equals(insurance.InsuranceParameter.CODEMETHOD))
                                            {
                                                if (insurance.InsuranceParameter.Deckungssumme == 0 || insurance.InsuranceParameter.Deckungssumme == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Deckungssumme erforderlich für Haftpflicht" });
                                                if (insurance.InsuranceParameter.Nachlass == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Vorzug Haftpflicht erforderlich" });
                                            }
                                        }*/
                                }



                                //Bonus/Malus Info
                                if (dto.ANGOBNOVAZUAB == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Bonus/malus erforderlich" });


                                // Wartung & Reparatur 
                                if (dto.ANGKALKFSMAINTFIXFLAG == null) rval.Add(new ValidationResultDto() { valid = false, Message = "W&R fix/variabel erforderlich" });
                                if (dto.ANGKALKFSMEHRKMBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "W&R mehr-km erforderlich" });

                                //Reifen- und Felgenersatz 
                                if (dto.ANGKALKFSRIMSCOUNTV == null || dto.ANGKALKFSRIMSCOUNTH == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Anzahl Felgen (Stk.) erforderlich" });
                                if (dto.ANGKALKFSSTIRESCOUNTV == null || dto.ANGKALKFSSTIRESCOUNTH == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Anzahl Sommerreifen (Stk.) erforderlich" });

                                // Vehicle Data

                                if (dto.ANGOBINIMOTORTYP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Motor Typ ( Benzin oder Diesel) erforderlich" });
                                if (dto.ANGOBKW == null) rval.Add(new ValidationResultDto() { valid = false, Message = "KW des Fahrzeuges erforderlich" });
                                if (dto.ANGOBCCM == null) rval.Add(new ValidationResultDto() { valid = false, Message = "ccm erforderlich" });
                                if (dto.ANGOBSCHWACKE == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Eurotax Nummer erforderlich" });
                                if (!isNew && dto.ANGOBINIKMSTAND == null) rval.Add(new ValidationResultDto() { valid = false, Message = "km-Stand bei Übernahme erforderlich" });
                                if (!isNew && dto.ANGOBINIERSTZUL == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Erstzulassung erforderlich" });



                                break;

                            case (40):  // SELECTKREDIT BMW SELECT 
                                if (dto.ANGKALKSZBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ inkl. MwSt.erforderlich" });
                                if (dto.ANGKALKSZBRUTTOP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ bzw. Anzahlung in % erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTO == null || dto.ANGKALKRWBASEBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Restwert- bzw. Zielratenvorschlag in € erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTOP == null || dto.ANGKALKRWBASEBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "% Restwert- bzw. Zielratenvorschlag % (Select und RWL) erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTO == null || dto.ANGKALKRWKALKBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Eigener RW/Zielrate in € (Select, Zielratenkr., RWL) = für die Kalkulation verwendeter RW bzw. Zielrate erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTOP == null || dto.ANGKALKRWKALKBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "%Eigener RW/Zielrate % (Select, Zielratenkr., RWL) erforderlich" });
                                if (dto.ANGOBSATZMEHRKMBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Mehrkilometer inkl. MwSt." });
                                if (dto.SYSPROVTARIF == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Variante der Kalkulation ( z.b.: 0,5%) erforderlich" });


                                // Vehicle Data
                                if (dto.ANGOBCCM == null) rval.Add(new ValidationResultDto() { valid = false, Message = "ccm erforderlich" });
                                if (dto.ANGOBGRUNDBRUTTO == null || dto.ANGOBGRUNDBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Fahrzeuglistenpreis Brutto erforderlich" });


                                break;


                            case (50):  // ZielKredit  ZIELRATENKREDIT
                                if (dto.ANGKALKSZBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ inkl. MwSt.erforderlich" });
                                if (dto.ANGKALKSZBRUTTOP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ bzw. Anzahlung in % erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTO == null || dto.ANGKALKRWBASEBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Restwert- bzw. Zielratenvorschlag in € erforderlich" });
                                if (dto.ANGKALKRWBASEBRUTTOP == null || dto.ANGKALKRWBASEBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "% Restwert- bzw. Zielratenvorschlag % (Select und RWL) erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTO == null || dto.ANGKALKRWKALKBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Eigener RW/Zielrate in € (Select, Zielratenkr., RWL) = für die Kalkulation verwendeter RW bzw. Zielrate erforderlich" });
                                if (dto.ANGKALKRWKALKBRUTTOP == null || dto.ANGKALKRWKALKBRUTTOP.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "%Eigener RW/Zielrate % (Select, Zielratenkr., RWL) erforderlich" });
                                if (dto.SYSPROVTARIF == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Variante der Kalkulation ( z.b.: 0,5%) erforderlich" });


                                // Vehicle Data
                                if (dto.ANGOBCCM == null) rval.Add(new ValidationResultDto() { valid = false, Message = "ccm erforderlich" });
                                if (!isNew && dto.ANGOBINIKMSTAND == null) rval.Add(new ValidationResultDto() { valid = false, Message = "km-Stand bei Übernahme erforderlich" });
                                if (!isNew && dto.ANGOBINIERSTZUL == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Erstzulassung erforderlich" });
                                if (dto.ANGOBGRUNDBRUTTO == null || dto.ANGOBGRUNDBRUTTO.Value == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Fahrzeuglistenpreis brutto erforderlich" });


                                break;




                            case (39):  // STANDARDRATENKREDIT 
                                if (dto.ANGKALKSZBRUTTO == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ inkl. MwSt.erforderlich" });
                                if (dto.ANGKALKSZBRUTTOP == null) rval.Add(new ValidationResultDto() { valid = false, Message = "MVZ bzw. Anzahlung in % erforderlich" });
                                if (dto.SYSPROVTARIF == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Variante der Kalkulation ( z.b.: 0,5%) erforderlich" });


                                // Vehicle Data
                                if (dto.ANGOBCCM == null) rval.Add(new ValidationResultDto() { valid = false, Message = "ccm erforderlich" });
                                if (dto.ANGOBSCHWACKE == null || dto.ANGOBSCHWACKE.Length == 0) rval.Add(new ValidationResultDto() { valid = false, Message = "Eurotax Nummer erforderlich" });
                                if (!isNew && dto.ANGOBINIKMSTAND == null) rval.Add(new ValidationResultDto() { valid = false, Message = "km-Stand bei Übernahme erforderlich" });
                                if (!isNew && dto.ANGOBINIERSTZUL == null) rval.Add(new ValidationResultDto() { valid = false, Message = "Erstzulassung erforderlich" });

                                break;


                        }
                    }

                    else if (!dto.SYSPRPRODUCT.HasValue || dto.SYSPRPRODUCT == 0)
                    {
                        rval.Add(new ValidationResultDto() { valid = false, Message = "Kein Finanzierungsprodukt gewählt" });
                    }

                    int vc = validateIBAN(Context, sysid);
                    if (vc == 1)
                    {
                        rval.Add(new ValidationResultDto() { valid = false, Message = "Bankverbindung vervollständigen" });
                    }
                    else if (vc == 2)
                    {
                        rval.Add(new ValidationResultDto() { validationId = ValidationStatus.DELETEDIBANBIC, valid = true, Message = "Fehlerhafte Bankverbindung wurde entfernt." });
                    }


                }
                catch (System.Exception exception)
                {
                    // Log the exception
                    _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.ValidateAngebotSave + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.ValidateAngebotSave.ToString(), exception);
                    rval.Add(new ValidationResultDto() { valid = false, Message = "Angebot konnte nicht geprüft werden: " + exception.Message });
                }

            }

            return rval;
        }

        /// <summary>
        /// Validates the given validationId's against the Offer
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="validations"></param>
        /// <returns></returns>
        public ValidationResultDto[] validateAngebot(long sysangebot, ValidationStatus[] validations)
        {
            // Validate
            ValidationResultDto[] rval = new ValidationResultDto[validations.Length];
            try
            {

                int i = 0;
                foreach (ValidationStatus stat in validations)
                {
                    if (stat == ValidationStatus.KASKONEEDED)
                    {
                        rval[i++] = PlausibilityCheck.checkValidKaskoPlausibility(sysangebot, "true");
                    }
                }
            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
            return rval;
        }

        /// <summary>
        /// Submits or resubmits the Offer
        /// </summary>
        /// <param name="einreichungDto"></param>
        /// <returns></returns>
        public SubmitStatus Submit(EinreichungDto einreichungDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            ServiceValidator.ValidateEdit();

            try
            {


                using (DdOlExtended Context = new DdOlExtended())
                {

                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, einreichungDto.SYSANGEBOT))
                    {
                        _Log.Warn("Submit of offer not possible. User has no permission to access this offer");
                        return SubmitStatus.NOPERMISSION;
                    }
                        

                    long isValid = ANGEBOTAssembler.getPRODUCTValidFromAngebot(einreichungDto.SYSANGEBOT, Context);
                    if (isValid == 0)
                        return SubmitStatus.PRODUCTEXPIRED;

                    long CONTRACTEXT = Context.ExecuteStoreQuery<long>("select CONTRACTEXT from angebot where sysid ='" + einreichungDto.SYSANGEBOT + "'", null).FirstOrDefault();
                    if (CONTRACTEXT > 0)
                    {
                        return SubmitExtension(einreichungDto);
                    }

                }

                int contracttype = 0;
              
                //Save werbecode
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if(einreichungDto.TRADEONOWNACCOUNT>0)//nur hier hat der user den dialog für die ganzen Flags hier gesehen und bestaetigt
                    { 
                        long sysit = Context.ExecuteStoreQuery<long>("select sysit from angebot where sysid=" + einreichungDto.SYSANGEBOT, null).FirstOrDefault();
                        long syskdtyp = Context.ExecuteStoreQuery<long>("select syskdtyp from it where sysit=" + sysit, null).FirstOrDefault();
                        if (syskdtyp < 3 && einreichungDto.SCHUFAFLAG == 0)
                            return SubmitStatus.SCHUFANEEDED;

                    
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "werbecode", Value = einreichungDto.WERBECODE });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "INFOMAILFLAG", Value = einreichungDto.INFOMAILFLAG });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "INFOSMSFLAG", Value = einreichungDto.INFOSMSFLAG });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "INFOTELFLAG", Value = einreichungDto.INFOTELFLAG });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schufaflag", Value = einreichungDto.SCHUFAFLAG });
                        //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bwazustflag", Value = einreichungDto.BWAZUSTFLAG });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                        Context.ExecuteStoreCommand("update it set INFOTELFLAG=:INFOTELFLAG,werbecode=:werbecode,INFOSMSFLAG=:INFOSMSFLAG,INFOMAILFLAG=:INFOMAILFLAG,schufaflag=:schufaflag where sysit=:sysit", parameters.ToArray());

                        if (syskdtyp != 3)//Privatperson Schufaflag setzen
                        {
                            MitantragstellerDto[] mas = DeliverMitantragsteller(einreichungDto.SYSANGEBOT);
                            if (mas != null)
                            {
                                foreach (MitantragstellerDto ma in mas)
                                {
                                    if (ma.SICHTYPRANG < 100)
                                    {
                                        if (einreichungDto.SCHUFAFLAGMA == null || einreichungDto.SCHUFAFLAGMA.Value == 0)
                                        {
                                            return SubmitStatus.SCHUFANEEDED;
                                        }
                                        parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schufaflag", Value = einreichungDto.SCHUFAFLAGMA });
                                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = ma.SYSIT });
                                        Context.ExecuteStoreCommand("update it set  schufaflag=:schufaflag where sysit=:sysit", parameters.ToArray());
                                    }
                                }
                            }
                        }
                        else if (syskdtyp == 3)
                        {
                            List<long> inhaber = Context.ExecuteStoreQuery<long>("select sysober from itkne where area='ANGEBOT' and relatetypecode in ('INH','GESELLS','KOMPL','PARTNER','VORSTAND','STIFTUNGSV','STIFTB','WB') and sysarea=" + einreichungDto.SYSANGEBOT + " and sysunter = " + sysit).ToList();

                            MitantragstellerDto[] mas = DeliverMitantragsteller(einreichungDto.SYSANGEBOT);
                            if (mas != null)
                            {
                                foreach (MitantragstellerDto ma in mas)
                                {
                                    if (ma.SICHTYPRANG < 100)
                                    {
                                        if (einreichungDto.SCHUFAFLAGMA == null || einreichungDto.SCHUFAFLAGMA.Value == 0)
                                        {
                                            return SubmitStatus.SCHUFANEEDED;
                                        }
                                        parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schufaflag", Value = einreichungDto.SCHUFAFLAGMA });
                                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = ma.SYSIT });
                                        Context.ExecuteStoreCommand("update it set  schufaflag=:schufaflag where sysit=:sysit", parameters.ToArray());

                                        if (inhaber != null)
                                        {
                                            for (int i = 0; i < 10; i++)
                                                inhaber.Remove(ma.SYSIT);
                                        }
                                    }
                                    else//alle anderen schufaflag auf 0!
                                    {
                                        parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schufaflag", Value = 0 });
                                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = ma.SYSIT });
                                        Context.ExecuteStoreCommand("update it set  schufaflag=:schufaflag where sysit=:sysit", parameters.ToArray());
                                    }
                                }
                            }

                            

                            if (einreichungDto.SCHUFAFLAGIHNUM != null)
                            {
                                string[] itids = einreichungDto.SCHUFAFLAGIHNUM.Split(',');
                                
                                foreach (String itid in itids)
                                {
                                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schufaflag", Value = "1" });
                                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = itid });
                                    Context.ExecuteStoreCommand("update it set  schufaflag=:schufaflag where sysit=:sysit", parameters.ToArray());
                                    if (inhaber != null)
                                    {
                                        for(int i=0; i<10; i++)
                                            inhaber.Remove(long.Parse(itid));
                                    }
                                }
                            }
                            if (inhaber != null)//set schufaflag for others to zero
                            {
                                foreach (long sysinh in inhaber)
                                {
                                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schufaflag", Value = "0" });
                                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysinh });
                                    Context.ExecuteStoreCommand("update it set  schufaflag=:schufaflag where sysit=:sysit", parameters.ToArray());
                                }
                            }

                        }
                    }
                }

 
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                   

                    //EinreichungsDaten
                    var CurrentAngebot = (from Angebot in Context.ANGEBOT
                                          where Angebot.SYSID == einreichungDto.SYSANGEBOT
                                          select Angebot).FirstOrDefault();

                    // Check if ANGEBOT was found
                    if (CurrentAngebot == null)
                    {
                        // Throw an exception
                        //throw new Exception("Specified angebot could not be found.");
                        _Log.Warn("Submit of offer couldnt find Offer with id "+einreichungDto.SYSANGEBOT);
                        return SubmitStatus.OFFERNOTFOUND;
                    }

                    if (einreichungDto.RESUBMIT == 0)//Einreichung
                    {
                        // Check if the status is valid
                        if (!ZustandHelper.VerifyAngebotStatus(einreichungDto.SYSANGEBOT, Context, AngebotZustand.Kalkuliert, AngebotZustand.Gedruckt))
                        {
                            // Throw an exception
                            //throw new Exception("Invalid angebot status for Submit().");
                            _Log.Warn("Submit of offer not possible. Offer not in Status Kalkuliert or Gedruckt");
                            return SubmitStatus.INVALIDSTATUS;
                        }

                        //Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, CurrentAngebot);
                        // Write in Angebot
                        CurrentAngebot.GUELTIGBIS = (DateTime.Today).Date.AddDays(90);                        
                        CurrentAngebot.TRADEONOWNACCOUNT = einreichungDto.TRADEONOWNACCOUNT;
                        CurrentAngebot.CONTRACTTYPE = contracttype;
                      
                        try
                        {
                            // Save the changes
                            Context.SaveChanges();
                        }
                        catch (Exception exception)
                        {
                            // Throw an exception
                            _Log.Warn("Submit of offer couldnt save Offer", exception);
                            return SubmitStatus.SAVENOTPOSSIBLE;//throw new Exception("The angebot could not be saved.", exception);
                        }
                        try
                        {
                            AngebotSubmitDao sd = new AngebotSubmitDao();
                            sd.submit(CurrentAngebot, ServiceValidator.ISOLanguageCode, ServiceValidator.SysPEROLE, ServiceValidator.SYSWFUSER, einreichungDto.BWAZUSTFLAG);
                            ZustandHelper.SetAngebotStatus(CurrentAngebot,einreichungDto.SYSANGEBOT, Context, AngebotZustand.Eingereicht);

                        }
                        catch (Exception exception)
                        {

                            _Log.Warn("Submit of offer couldnt change Zustand", exception);
                            return SubmitStatus.NEWSTATENOTPOSSIBLE;// throw new Exception("The Antrag could not be saved. The Angebot will remain in the old State!", exception);
                        }
                    }
                    else//Wiedereinreichung
                    {
                        // Check if ANGEBOT a copy
                        if (CurrentAngebot.ANGEBOT1.IndexOf(".") < 0 || CurrentAngebot.SYSANGEBOT == null)
                        {
                            // Throw an exception
                            throw new Exception("Invalid angebot status for Resubmit().");
                        }
                        if (CurrentAngebot.SYSANGEBOT != null && CurrentAngebot.SYSANGEBOT.HasValue)//#3590
                        {
                            ZustandHelper.SetAngebotStatus(CurrentAngebot,CurrentAngebot.SYSANGEBOT.Value, Context, AngebotZustand.StornoWiedereinreichung);
                        }

                        //Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, CurrentAngebot);
                        // Write in Angebot


                        ZustandHelper.SetAngebotStatus(CurrentAngebot,einreichungDto.SYSANGEBOT, Context, AngebotZustand.Eingereicht);


                        EinreichungDto ed = new EinreichungDto();
                        ed.SYSANGEBOT = einreichungDto.SYSANGEBOT;
                        //if empty look in antrag-mandat
                        updateMandat(ed, Context);

                        try
                        {
                            // Save the changes
                            Context.SaveChanges();
                            AngebotSubmitDao sd = new AngebotSubmitDao();
                            sd.resubmit(CurrentAngebot, ServiceValidator.ISOLanguageCode, ServiceValidator.SysPEROLE, ServiceValidator.SYSWFUSER);
                        }
                        catch (Exception exception)
                        {
                            // Throw an exception
                            throw new Exception("The angebot could not be saved.", exception);
                        }
                    }
               

                    

                }
                flushSearchCache();

                return SubmitStatus.OK;

            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed.ToString(), exception);

                // Throw the exception
                return SubmitStatus.TECHNICALISSUE;
            }
            
        }
        private static int validateIBAN(DdOlExtended Context, long sysid)
        {
            /*BankdatenDto bankdaten = BankdatenDao.DeliverBankdaten(sysid);
            if ((bankdaten.IBAN == null||bankdaten.IBAN.Length==0) && bankdaten.EINZUG == 1) return 1;//muss vorhanden sein! sonst keine Kontoinhaberzuordnung!
            if ((bankdaten.IBAN == null || bankdaten.IBAN.Length == 0) && bankdaten.EINZUG == 0) return 0;//no kontoinfo available to check/delete

            IBANValidator checker = new IBANValidator();
            IBANValidationError checkError = checker.checkIBANandBIC(bankdaten.IBAN, bankdaten.BIC);
            if (checkError.error != IBANValidationErrorType.NoError || checkError.bicwarning)
            {
                if (bankdaten.EINZUG == 0)
                {   //kein einzug, iban löschen
                    if (bankdaten.IBAN == null || bankdaten.IBAN.Length == 0)
                    {
                        return 0; //OK, bereits geleert
                    }
                    Context.ExecuteStoreCommand("update it set iban=null,bic=null where sysit=" + bankdaten.SYSKI, null);
                    Context.SaveChanges();
                    return 2;
                }
                return 1;
            }
            else
            {
                return 0;//IBAN OK
            }*/



            int einzug = Context.ExecuteStoreQuery<int>("select einzug from angebot where  angebot.sysid=" + sysid).FirstOrDefault();
            //potential problem - it might have been updated by second offer already but this offer has a global mandate......
            BLZInfo blzInfo = Context.ExecuteStoreQuery<BLZInfo>("select replace(iban,' ','') iban,replace(bic,' ','') bic,einzug, it.sysit syski from angebot,it where it.sysit=angebot.syski and angebot.sysid=" + sysid).FirstOrDefault();
            if (blzInfo == null)
            {
                blzInfo = Context.ExecuteStoreQuery<BLZInfo>("select replace(iban,' ','') iban,replace(bic,' ','') bic,einzug, it.sysit syski from angebot,it where it.sysit=angebot.sysit and angebot.sysid=" + sysid).FirstOrDefault();
            }

            if (blzInfo == null && einzug == 1) return 1;//muss vorhanden sein! sonst keine Kontoinhaberzuordnung!

            if (blzInfo == null || blzInfo.IBAN == null || blzInfo.IBAN.Length == 0)
                return 0; //no kontoinfo available to check/delete

            IBANValidator checker = new IBANValidator();
            IBANValidationError checkError = checker.checkIBANandBIC(blzInfo.IBAN, blzInfo.BIC);
            if (checkError.error != IBANValidationErrorType.NoError || checkError.bicwarning)
            {
                if (einzug == 0 || !blzInfo.EINZUG.HasValue || blzInfo.EINZUG == 0)
                {   //kein einzug, iban löschen
                    if (blzInfo.IBAN == null || blzInfo.IBAN.Length == 0)
                    {
                        return 0; //OK, bereits geleert
                    }
                    Context.ExecuteStoreCommand("update it set iban=null,bic=null where sysit=" + blzInfo.SYSKI, null);
                    Context.SaveChanges();
                    return 2;
                }
                return 1;
            }
            else
            {
                return 0;//IBAN OK
            }
        }

        /// <summary>
        /// Validates the Mandate, returning a error status when mandat was not valid and also creating a new mandate when an invalid mandate was found
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        public SubmitStatus validateMandat(long sysangebot)
        {
            return SubmitStatus.OK;
            /*
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {
                int einzug = Context.ExecuteStoreQuery<int>("select einzug from angebot where  angebot.sysid=" + sysangebot).FirstOrDefault();
                if (einzug < 1) return SubmitStatus.OK;//kein einzug, keine Prüfung

                ANGEBOTDto ang = Context.ExecuteStoreQuery<ANGEBOTDto>("select syski,einzug,sysit,sysid,sysvart,sysls SYSBRAND,zustand from angebot where  angebot.sysid=" + sysangebot).FirstOrDefault();

                BankdatenDto bankdaten = BankdatenDao.getBankDatenFromAngebot(ang, ang.SYSBRAND.Value, Context, false, false);
                long syskonto = 0;
                long sysmandat = BankdatenDao.findMandat(Context, bankdaten, ref syskonto);
                if (sysmandat == 0)
                {

                    bankdaten.MANDATSORT = Context.ExecuteStoreQuery<String>("select ort from person,angebot where sysperson=angebot.sysvk and angebot.sysid=" + sysangebot, null).FirstOrDefault();


                    BankdatenDao bd = new BankdatenDao();
                    MANDAT m = bd.createOrUpdateMandat(Context, bankdaten, ServiceValidator.VpSysPERSON.GetValueOrDefault());
                    if (m != null)
                    {

                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysangebot });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ref", Value = m.REFERENZ });
                        Context.ExecuteStoreCommand("update angebot set mandatreferenz=:ref where sysid=:sysid", parameters.ToArray());
                        if (ang.ZUSTAND != null && ang.ZUSTAND.ToUpper().IndexOf("GENEHMIGT") > -1)
                        {
                            long sysperson = Context.ExecuteStoreQuery<long>("select sysperson from it where sysit=" + ang.SYSKI
                            , null).FirstOrDefault();
                            if (sysperson > 0)
                            {
                                m.PERSONReference.EntityKey = Context.getEntityKey(typeof(PERSON), sysperson);
                            }
                        }
                        Context.SaveChanges();
                    }
                    return SubmitStatus.MANDATVALIDATIONFAILED;
                }
                else // Defect #9243 - backend mandat wurde geändert, anderes gefunden, angebot vor druck aktualisieren
                {
                    String REFERENZ = Context.ExecuteStoreQuery<String>("select referenz from mandat where sysmandat=" + sysmandat, null).FirstOrDefault();

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysangebot });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ref", Value = REFERENZ });
                    Context.ExecuteStoreCommand("update angebot set mandatreferenz=:ref where sysid=:sysid", parameters.ToArray());
                    Context.SaveChanges();
                }

                return SubmitStatus.OK;
            }*/
        }
        /// <summary>
        /// Updates the mandant date and city and returns false if iban/bic are not correct
        /// </summary>
        /// <param name="einreichungDto"></param>
        /// <param name="Context"></param>
        /// <returns></returns>
        private static SubmitStatus updateMandat(EinreichungDto einreichungDto, DdOlExtended Context)
        {
            int einzug = Context.ExecuteStoreQuery<int>("select einzug from angebot where  angebot.sysid=" + einreichungDto.SYSANGEBOT).FirstOrDefault();
            if (einzug < 1) return SubmitStatus.OK;//kein einzug, keine Prüfung

            BLZInfo blzInfo = Context.ExecuteStoreQuery<BLZInfo>("select iban,bic,einzug from angebot,it where it.sysit=angebot.syski and angebot.sysid=" + einreichungDto.SYSANGEBOT).FirstOrDefault();
            if (blzInfo == null) return SubmitStatus.IBANVALIDATIONFAILED;//muss vorhanden sein! sonst keine Kontoinhaberzuordnung!
            if (blzInfo.EINZUG.HasValue && blzInfo.EINZUG.Value > 0)
            {
                IBANValidator checker = new IBANValidator();
                IBANValidationError checkError = checker.checkIBANandBIC(blzInfo.IBAN, blzInfo.BIC);
                if (checkError.error != IBANValidationErrorType.NoError || checkError.bicwarning)
                    return SubmitStatus.IBANVALIDATIONFAILED;
                //test mandat-status or valid-dates!!!

                ANGEBOTDto ang = Context.ExecuteStoreQuery<ANGEBOTDto>("select syski,einzug,angebot.sysit,sysid,sysvart,sysls SYSBRAND, it.sysperson syskd from angebot,it where it.sysit=angebot.sysit and angebot.sysid=" + einreichungDto.SYSANGEBOT).FirstOrDefault();
                BankdatenDto bankdaten = BankdatenDao.getBankDatenFromAngebot(ang, ang.SYSBRAND.Value, Context, false, true);
                long syskonto = 0;
                long sysmandat = BankdatenDao.findMandat(Context, bankdaten, ref syskonto);

                if (sysmandat == 0)
                    return SubmitStatus.MANDATVALIDATIONFAILED;

                //Defect #9036 - SEPA-2014-LS1 AIDA - Globalmandat bei neuem Angebot darf erst nach Antragsanlage in Open Lease auf die OL Person übertragen werden
                if (ang.SYSKD.HasValue && ang.SYSKD.Value > 0)
                {
                    MANDAT mandat = (from p in Context.MANDAT
                                     where p.SYSMANDAT == sysmandat
                                     select p).FirstOrDefault();
                    mandat.SYSPERSON= ang.SYSKD.GetValueOrDefault();
                }
                //wird immer in angebot-save aktualisiert
                /*
                MANDAT mandat = (from p in Context.MANDAT
                                 where p.AREA.Equals("ANGEBOT") && p.SYSID == einreichungDto.SYSANGEBOT
                                 select p).FirstOrDefault();

              

                //VerkäuferOrt
                String ort = Context.ExecuteStoreQuery<String>("select ort from person,angebot where sysperson=angebot.sysvk and angebot.sysid=" + einreichungDto.SYSANGEBOT).FirstOrDefault();
                if (mandat == null) return false;//EINZUG aber KEIN MANDAT!
                mandat.SIGNCITY = ort;
                if (einreichungDto.UNTERSCHRIFTORT != null && einreichungDto.UNTERSCHRIFTORT.Length > 0)
                {
                    String newOrt = einreichungDto.UNTERSCHRIFTORT;
                    if (newOrt.Length > 40)
                        newOrt = newOrt.Substring(0, 40);
                    mandat.SIGNCITY = newOrt;
                }

                mandat.SIGNDATE = DateTime.Now;
                mandat.VALIDFROM = mandat.SIGNDATE;
                
                //default immer CORE (BMWSEPA-2031) falls nichts gesetzt
                if (!mandat.PAYART.HasValue || mandat.PAYART == 0)
                {
                    mandat.PAYART = 1;
                }*/
            }
            return SubmitStatus.OK;
        }

        /* 
        /// <summary>
        /// Resubmits the offer
        /// possible when kalkuliert or gedruckt AND W on the end
        /// </summary>
        /// <param name="sysId"></param>
       public void Resubmit(EinreichungDto einreichungDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            ServiceValidator.ValidateEdit();

            try
            {

                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, einreichungDto.SYSANGEBOT))
                            throw new Exception("No Permission to ANGEBOT");
                    // Check if the status is valid
                    if (!ZustandHelper.VerifyAngebotStatus(einreichungDto.SYSANGEBOT, Context, AngebotZustand.Kalkuliert, AngebotZustand.Gedruckt))
                    {
                        // Throw an exception
                        throw new Exception("Invalid angebot status for Resubmit().");
                    }
                    

                    
                   
                    //EinreichungsDaten
                    var CurrentAngebot = (from Angebot in Context.ANGEBOT
                                          where Angebot.SYSID == einreichungDto.SYSANGEBOT && Angebot.SYSANGEBOT != null
                                          select Angebot).FirstOrDefault();

                    // Check if ANGEBOT was found
                    if (CurrentAngebot == null)
                    {
                        // Throw an exception
                        throw new Exception("Specified angebot could not be found.");
                    }

                    // Check if ANGEBOT a copy
                    if (CurrentAngebot.ANGEBOT1.IndexOf(".")<0 || CurrentAngebot.SYSANGEBOT == null)
                    {
                        // Throw an exception
                        throw new Exception("Invalid angebot status for Resubmit().");
                    }
                    if (CurrentAngebot.SYSANGEBOT != null && CurrentAngebot.SYSANGEBOT.HasValue)//#3590
                    {
                        ZustandHelper.SetAngebotStatus(CurrentAngebot.SYSANGEBOT.Value, Context, AngebotZustand.StornoWiedereinreichung);
                    }

                    Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, CurrentAngebot);
                    // Write in Angebot


                    ZustandHelper.SetAngebotStatus(einreichungDto.SYSANGEBOT, Context, AngebotZustand.Eingereicht);
                

                    EinreichungDto ed = new EinreichungDto();
                    ed.SYSANGEBOT = einreichungDto.SYSANGEBOT;
                   
                    //if empty look in antrag-mandat
                    updateMandat(ed, Context);

                    try
                    {
                        // Save the changes
                        Context.SaveChanges();
                        AngebotSubmitDao sd = new AngebotSubmitDao();
                        sd.resubmit(CurrentAngebot, ServiceValidator.ISOLanguageCode, ServiceValidator.SysPEROLE, ServiceValidator.SYSWFUSER);
                        

                    }
                    catch (Exception exception)
                    {
                        // Throw an exception
                        throw new Exception("The angebot could not be saved.", exception);
                    }

                  
                }
            }
            catch (System.Exception exception)
            {

                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }*/

        /// <summary>
        /// Copies the offer for re-submit
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public ANGEBOTDto CopyToResubmit(long sysid)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            ServiceValidator.ValidateEdit();

            try
            {
                return copyForResubmit(sysid, ServiceValidator.SYSPUSER, ServiceValidator.SysBRAND, ServiceValidator.SysPEROLE, ServiceValidator.VpSysPERSON.GetValueOrDefault(), ServiceValidator.SysPERSON, ServiceValidator.SYSWFUSER, ServiceValidator.ISOLanguageCode, ServiceValidator.VpSysPEROLE);

            }

            catch (System.Exception exception)
            {

                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed.ToString(), exception);

                ANGEBOTDto rval = new ANGEBOTDto();

                rval.ERRORMESSAGE = "UnknownError";
                return rval;
            }

        }

        public static ANGEBOTDto copyForResubmit(long sysid, long syspuser, long sysbrand, long sysperole, long sysvpperson, long sysperson, long syswfuser, String langcode, long vpsysperole)
        {
            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {

                // Check if the status is valid
                if (!ZustandHelper.VerifyAngebotStatus(sysid, Context, AngebotZustand.AntragsaenderungErforderlich))//#3724
                                                                                                                    //if (antragStatus==null || !"Antragsänderung erfdl.".Equals(antragStatus))
                {
                    ANGEBOTDto rval = new ANGEBOTDto();

                    rval.ERRORMESSAGE = "InvalidStatus";
                    return rval;
                }

                if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, syspuser, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysid))
                    throw new Exception("No Permission to ANGEBOT");

                // Get angebot with specified id
                ANGEBOTDto AngebotDto = AngebotDtoHelper.Deliver(sysid, sysbrand, sysperole, sysvpperson, sysperson, syswfuser, syspuser);

                long syskdtyp = 1;
                if (AngebotDto.SYSIT.HasValue && AngebotDto.SYSIT.Value > 0)
                {
                    syskdtyp = Context.ExecuteStoreQuery<long>("select syskdtyp from it where sysit=" + AngebotDto.SYSIT, null).FirstOrDefault();
                }



                DateTime erstzul = AngebotDto.ANGOBINIERSTZUL.HasValue ? AngebotDto.ANGOBINIERSTZUL.Value : DateTime.Now;
                DateTime pDate = AngebotDto.ANGOBLIEFERUNG.HasValue ? AngebotDto.ANGOBLIEFERUNG.Value : DateTime.Now;
                if (erstzul.Year < 1801) erstzul = DateTime.Now;//avoid invalid age

                Cic.OpenOne.Common.DAO.Prisma.IPrismaDao pDao = Cic.OpenOne.Common.DAO.Prisma.PrismaDaoFactory.getInstance().getPrismaDao();
                Cic.OpenOne.Common.DAO.IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                Cic.OpenOne.Common.DAO.ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                Cic.OpenOne.Common.BO.Prisma.PrismaProductBo bo = new Cic.OpenOne.Common.BO.Prisma.PrismaProductBo(pDao, obDao, transDao, Cic.OpenOne.Common.BO.Prisma.PrismaProductBo.CONDITIONS_HCBE, langcode);
                Cic.OpenOne.Common.DTO.Prisma.prKontextDto kontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();
                kontext.perDate = pDate;
                kontext.sysobtyp = AngebotDto.SYSOBTYP.Value;
                kontext.sysobart = AngebotDto.SYSOBART.Value;
                kontext.sysprchannel = 1;//immer 1
                kontext.sysperole = sysperole;
                kontext.syskdtyp = syskdtyp;
                kontext.sysvpperole = vpsysperole;


                string angebotw = AngebotDto.ANGEBOT1;
                if (angebotw.IndexOf(".") < 0)
                    angebotw = angebotw + ".2";
                else
                {
                    long numold = long.Parse(angebotw.Substring(angebotw.IndexOf(".") + 1));
                    angebotw = angebotw.Substring(0, angebotw.IndexOf("."));
                    angebotw += "." + (numold + 1);
                }
                // Query ANGEBOT
                var CurrentAngebot = (from Angebot in Context.ANGEBOT
                                      where Angebot.ANGEBOT1 == angebotw
                                      select Angebot).FirstOrDefault();

                // Check if ANGEBOT was found
                if (CurrentAngebot != null)
                {
                    AngebotDto = AngebotDtoHelper.Deliver(CurrentAngebot.SYSID, sysbrand, sysperole, sysvpperson, sysperson, syswfuser, syspuser);
                    return AngebotDto;
                }

                //Ticket #3724
                AngebotDto.SPECIALCALCSTATUS = 0;
                AngebotDto.SPECIALCALCCOUNT = 0;


                // Set the id to null - will cause call to create
                AngebotDto.SYSID = null;
                AngebotDto.ERFASSUNG = DateTime.Today;
                // Set parent angebot
                AngebotDto.SYSANGEBOT = sysid;
                AngebotDto.ANGEBOT1 = angebotw;
                AngebotDto.GUELTIGBIS = (DateTime.Today).Date.AddDays(90);
                // Set the statuses
                if (AngebotDto.ANGVSPARAM != null)
                {
                    foreach (InsuranceDto ins in AngebotDto.ANGVSPARAM)
                        ins.SysAngVs = 0;
                }
                AngebotDto.ZUSTANDAM = DateTime.Now;
                AngebotDto.ZUSTAND = ZustandHelper.GetStringFromEnumerator(AngebotZustand.NeuResubmit);
                AngebotDto.TRADEONOWNACCOUNT = null;

                // Save new angebot
                List<MitantragstellerDto> mitAntragsteller = getMitantragsteller(sysid);

                AngebotDto = AngebotDtoHelper.Save(AngebotDto, sysbrand, sysperole, sysvpperson, sysperson, syswfuser, CnstFinKzNotSet, null, syspuser,true);


                // MitAntragsteller copy für das neue Angebot
                if (AngebotDto.SYSID.HasValue)
                {
                    long AngebotSysid = AngebotDto.SYSID.Value;

                    foreach (var maLoop in mitAntragsteller)
                    {
                        maLoop.SYSVT = AngebotSysid;
                        maLoop.SysId = 0;
                        saveMitantragstellerData(maLoop);
                    }
                }
                //Copy picture
                using (DdOwExtended context = new DdOwExtended())
                {
                    AngebotBinaryDao dao = new AngebotBinaryDao(context);
                    if (AngebotDto.SYSID.HasValue)
                        dao.copyPictureData(sysid, AngebotDto.SYSID.Value);

                }


                long sysantrag = Context.ExecuteStoreQuery<long>("select sysid from Antrag where sysangebot=" + sysid, null).FirstOrDefault();


                if(CurrentAngebot.SYSOPPO.HasValue)
                    Context.ExecuteStoreCommand("UPDATE ANGEBOT set sysoppo="+CurrentAngebot.SYSOPPO.Value+" where sysid=" + AngebotDto.SYSID.Value, null);

                //HCEB-1735  - Antrag stornieren
                if (sysantrag > 0)
                {
                    String reason = Context.ExecuteStoreQuery<String>("select DECODE(ID,1,9,2,11,3,17,14) from ddlkppos where code ='ANGEBOT_STORNOGRUND' and value like '%Storno Antrags%'", null).FirstOrDefault();
                    String att = Context.ExecuteStoreQuery<String>("select SYSATTRIBUTDEF from attributdef where ATTRIBUT = 'Storniert'", null).FirstOrDefault();
                    long time = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    long date = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
                    Context.ExecuteStoreCommand("insert into eaihot (code,oltable,sysoltable,syseaiart,inputparameter2,inputparameter3,inputparameter4,hostcomputer,submitdate,submittime,eve,syswfuser,inputparameter5) values ('ANTRAG_CHG_STATUS','ANTRAG'," + sysantrag + ",203,'START_FO','" + att + "','" + reason + "','*'," + date + "," + time + ",1," + syswfuser + ",'" + syswfuser + "')", null);
                }


                return AngebotDto;



            }
        }

        /// <summary>
        /// Cancels an offer (changing its status, if allowed)
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <param name="cancelReason"></param>
        /// <returns></returns>
        public AngebotCancelStatus Cancel(long sysAngebot, string cancelReason)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            ServiceValidator.ValidateEdit();

            try
            {
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {

                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAngebot))
                        throw new Exception("No Permission to ANGEBOT");

                    // Query ANGEBOT
                    var CurrentAngebot = (from Angebot in Context.ANGEBOT
                                          where Angebot.SYSID == sysAngebot
                                          select Angebot).FirstOrDefault();

                    // Check if ANGEBOT was found
                    if (CurrentAngebot == null)
                    {
                        // Throw an exception
                        return AngebotCancelStatus.ANGEBOT_NOT_FOUND;
                    }

                    long sysantrag = Context.ExecuteStoreQuery<long>("select sysid from Antrag where sysangebot=" + sysAngebot, null).FirstOrDefault();

                    // Check if the status is valid
                    if (!ZustandHelper.VerifyAngebotStatus(sysAngebot, Context, AngebotZustand.Neu, AngebotZustand.Kalkuliert, AngebotZustand.Gedruckt, AngebotZustand.Einreichen, AngebotZustand.Eingereicht, AngebotZustand.Genehmigt, AngebotZustand.GenehmigtMitAuflagen, AngebotZustand.AbgelehntMitAuflagen, AngebotZustand.BONITAETSPRUEFUNG, AngebotZustand.ZusatzinformationBenoetigt, AngebotZustand.GenehmigtAutomatisch))
                    {
                        // Throw an exception
                        return AngebotCancelStatus.ANGEBOT_INVALID_STATE;
                    }
                    //wenn antrag nicht einer von folgenden Zuständen hat, Fehler liefern
                    if (sysantrag > 0 && !ZustandHelper.AntragHasStatus(sysAngebot, Context, AntragZustand.NEU, AntragZustand.VORPRÜFUNG, AntragZustand.RISIKOPRÜFUNG, AntragZustand.NACHBEARBEITUNG))
                    {
                        // Throw an exception
                        return AngebotCancelStatus.ANTRAG_INVALID_STATE;
                    }
                    //wenn antrag einen folgender Zustände hat, fehler liefern:
                   /* if (!ZustandHelper.AntragHasNotStatus(sysAngebot, Context, AntragZustand.ABGELEHNT, AntragZustand.ABGELEHNTMITAUFLAGEN, AntragZustand.ABGESCHLOSSEN))
                    {
                        // Throw an exception
                        return AngebotCancelStatus.ANTRAG_INVALID_STATE;
                    }*/



                    // Write the new status and status change time
                    CurrentAngebot.ZUSTAND = ZustandHelper.GetStringFromEnumerator(AngebotZustand.Stornieren);
                    CurrentAngebot.AKTIVKZ = 0;
                    CurrentAngebot.ZUSTANDAM = DateTime.Now;

                    // Write the cancellation reason
                    if (cancelReason != null && cancelReason.Length > 40)
                        cancelReason = cancelReason.Substring(0, 40);
                    CurrentAngebot.ABTRETUNGVON = cancelReason;

                   
                    
                    String reason  = Context.ExecuteStoreQuery<String>("select DECODE(ID,1,9,2,11,3,17,14) from ddlkppos where code ='ANGEBOT_STORNOGRUND' and value like '%" + cancelReason + "%'",null).FirstOrDefault();
                    String att = Context.ExecuteStoreQuery<String>("select SYSATTRIBUTDEF from attributdef where ATTRIBUT = 'Storniert'", null).FirstOrDefault();
                    long time = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    long date = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
                    if(sysantrag>0)
                        Context.ExecuteStoreCommand("insert into eaihot (code,oltable,sysoltable,syseaiart,inputparameter2,inputparameter3,inputparameter4,hostcomputer,submitdate,submittime,eve,syswfuser,inputparameter5) values ('ANTRAG_CHG_STATUS','ANTRAG'," + sysantrag + ",203,'START_FO','" + att + "','" + reason + "','*'," + date + "," + time + ",1," + ServiceValidator.SYSWFUSER + ",'" +ServiceValidator.SYSWFUSER + "')", null);

                    // Save the changes
                    Context.SaveChanges();

                    return AngebotCancelStatus.ANGEBOT_CANCELED_OK;
                }
            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotResubmitFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Changes the state of the Offer
        /// </summary>
        /// <param name="sysAngebot"></param>
        public void AngebotZustandKalkuliert(long sysAngebot)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            ServiceValidator.ValidateEdit();

            try
            {
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAngebot))
                        throw new Exception("No Permission to ANGEBOT");

                    // Query ANGEBOT
                    var CurrentAngebot = (from Angebot in Context.ANGEBOT
                                          where Angebot.SYSID == sysAngebot
                                          select Angebot).FirstOrDefault();
                    // Check if ANGEBOT was found
                    if (CurrentAngebot == null)
                    {
                        // Throw an exception
                        throw new Exception("Specified angebot could not be found.");
                    }

                    // Check if the status is valid
                    if (!ZustandHelper.VerifyAngebotStatus(sysAngebot, Context, AngebotZustand.Storniert, AngebotZustand.Gedruckt, AngebotZustand.Neu))
                    {
                        // Throw an exception
                        throw new Exception("Invalid angebot status for Kalkulert: " + CurrentAngebot.ZUSTAND);
                    }





                    // Write the new status and status change time
                    CurrentAngebot.ZUSTAND = ZustandHelper.GetStringFromEnumerator(AngebotZustand.Kalkuliert);
                    CurrentAngebot.AKTIVKZ = 1;
                    CurrentAngebot.ZUSTANDAM = DateTime.Now;


                    // Save the changes
                    Context.SaveChanges();
                }
            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotZustandKalkuliert, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotZustandKalkuliert + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotZustandKalkuliert.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        private class ITAngebotSearch
        {
            public String NAME { get; set; }
            public String VORNAME { get; set; }
            public String ORT { get; set; }
            public String PLZ { get; set; }
        }
        private class ANGKALKSearchResult
        {
            public long SYSKALK { get; set; }
            public int? LZ { get; set; }
            public decimal? RATEBRUTTO { get; set; }
            public decimal? SZ { get; set; }
            public decimal? SZBRUTTO { get; set; }
            public decimal? DEPOT { get; set; }
            public decimal? AHK { get; set; }
            public decimal? BGEXTERNBRUTTO { get; set; }
            public decimal? RWKALKBRUTTO { get; set; }
        }
        private class ANGOBSearchResult
        {
            public long SYSOB { get; set; }
            public long? JAHRESKM { get; set; }
            public decimal? AHK { get; set; }
            public decimal? AHKBRUTTO { get; set; }
            public String BEZEICHNUNG { get; set; }
        }
        private class PersonSearchResult
        {
            public long SYSVK { get; set; }
            public String VERKAUFERNAME { get; set; }
            public String VERKAUFERVORNAME { get; set; }
        }
        private class UnterlagenResult
        {
            public String BEZEICHNUNG { get; set; }
            public String BESCHREIBUNG { get; set; }
            public long RANG { get; set; }
        }
        /// <summary>
        /// Suche nach Angeboten
        /// </summary>
        /// <param name="angebotSearchData">Suchfilter</param>
        /// <param name="searchParameters">Suchparameter</param>
        /// <param name="angebotSortData">Sortierparameter</param>
        /// <returns>Liste der gefunden Angebote</returns>
        public SearchResult<ANGEBOTShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData angebotSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSortData[] angebotSortData)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {



                if (angebotSearchData == null)
                {
                    throw new ArgumentException("angebotSearchData");
                }


                // Check search parameters
                Cic.OpenLease.Service.Helpers.ServiceParametersHelper.CheckTopParameter(searchParameters.Top);
                Cic.OpenLease.Service.Helpers.ServiceParametersHelper.CheckSkipParameter(searchParameters.Skip);

                SearchResult<ANGEBOTShortDto> result = new SearchResult<ANGEBOTShortDto>();
                System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTShortDto> ANGEBOTShortDtoList = null;

                long resCount = 0;
                using (DdOlExtended Context = new DdOlExtended())
                {
                    FlowDao flowdao = new FlowDao(Context);
                    // Get raw list
                    ANGEBOTShortDtoList = MyDeliverANGEBOTList(Context, angebotSearchData, searchParameters, angebotSortData, ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER, out resCount);
                    result.results = ANGEBOTShortDtoList;
                    result.searchCountMax = (int)resCount;
                    result.searchCountFiltered = ANGEBOTShortDtoList.Count;

                    // Check list
                    if ((ANGEBOTShortDtoList == null) || (ANGEBOTShortDtoList.Count == 0))
                        return result;


                    foreach (ANGEBOTShortDto LoopANGEBOTShortDto in ANGEBOTShortDtoList)
                    {

                        // Check  Angebot ist gueltig 
                        if (LoopANGEBOTShortDto.GUELTIGBIS >= DateTime.Today)

                            LoopANGEBOTShortDto.Gueltig = true;
                        else
                            LoopANGEBOTShortDto.Gueltig = false;

                       
                       LoopANGEBOTShortDto.AUFLAGEN = "NEIN";
                       
                        List<FlowDto> auflagen = flowdao.getMessages("ANG", LoopANGEBOTShortDto.SYSID);
                        if (auflagen != null && auflagen.Count > 0)
                            LoopANGEBOTShortDto.AUFLAGEN = "JA";
                       

                        // Get IT-Data if angebot sysit is not null
                        if (LoopANGEBOTShortDto.SYSIT != null)
                        {
                            string ITQuery = "SELECT NAME, ORT, PLZ, VORNAME FROM CIC.IT WHERE SYSIT = " + LoopANGEBOTShortDto.SYSIT;
                            ITAngebotSearch itang = Context.ExecuteStoreQuery<ITAngebotSearch>(ITQuery, null).FirstOrDefault();
                            if (itang != null)
                            {
                                LoopANGEBOTShortDto.ITNAME = itang.NAME;
                                LoopANGEBOTShortDto.ITORT = itang.ORT;
                                LoopANGEBOTShortDto.ITPLZ = itang.PLZ;
                                LoopANGEBOTShortDto.ITVORNAME = itang.VORNAME;
                            }
                        }

                        //ANGEBOT-Data:
                        LoopANGEBOTShortDto.DATANGEBOT = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(LoopANGEBOTShortDto.DATANGEBOT);
                        LoopANGEBOTShortDto.ERFASSUNG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(LoopANGEBOTShortDto.ERFASSUNG);

                        if (!LoopANGEBOTShortDto.SPECIALCALCSTATUS.HasValue || LoopANGEBOTShortDto.SPECIALCALCSTATUS == 0)
                            LoopANGEBOTShortDto.SPECIALCALCSTATUSTEXT = "";
                        else if (LoopANGEBOTShortDto.SPECIALCALCSTATUS == 1)
                            LoopANGEBOTShortDto.SPECIALCALCSTATUSTEXT = "angefordert";
                        else if (LoopANGEBOTShortDto.SPECIALCALCSTATUS == 2)
                            LoopANGEBOTShortDto.SPECIALCALCSTATUSTEXT = "in Bearbeitung";
                        else if (LoopANGEBOTShortDto.SPECIALCALCSTATUS == 3)
                            LoopANGEBOTShortDto.SPECIALCALCSTATUSTEXT = "durchgeführt";

                        //Get angkalk from angebot and related to angkalk
                        string ANGKALKQuery = "SELECT SYSKALK,LZ,RATEBRUTTO,SZ,SZBRUTTO,DEPOT,AHK,BGEXTERNBRUTTO,RWKALKBRUTTO FROM CIC.ANGKALK WHERE SYSANGEBOT = " + LoopANGEBOTShortDto.SYSID;
                        ANGKALKSearchResult angkalkData = Context.ExecuteStoreQuery<ANGKALKSearchResult>(ANGKALKQuery, null).FirstOrDefault();
                        if (angkalkData != null)
                        {
                            LoopANGEBOTShortDto.ANGKALKLZ = angkalkData.LZ;//int?
                            LoopANGEBOTShortDto.ANGKALKRATE = angkalkData.RATEBRUTTO;//Decimal?
                            LoopANGEBOTShortDto.ANGKALKSZ = angkalkData.SZ;//decimal?
                            LoopANGEBOTShortDto.ANGKALKSZBRUTTO = angkalkData.SZBRUTTO;//decimal?
                            LoopANGEBOTShortDto.ANGKALKDEPOT = angkalkData.DEPOT;//decimal?
                            LoopANGEBOTShortDto.AHKEXTERNBRUTTO = angkalkData.AHK;//decimal?
                            LoopANGEBOTShortDto.ANGKALKBGEXTERNBRUTTO = angkalkData.BGEXTERNBRUTTO;//decimal?
                            LoopANGEBOTShortDto.RWKALKBRUTTO = angkalkData.RWKALKBRUTTO;//decimal?

                            //Get angob
                            string ANGOBQuery = "SELECT JAHRESKM,AHKEXTERNBRUTTO AHK,AHKBRUTTO, HERSTELLER||' '||BEZEICHNUNG BEZEICHNUNG FROM CIC.ANGOB WHERE SYSOB = (SELECT SYSOB FROM CIC.ANGKALK WHERE SYSKALK = " + angkalkData.SYSKALK + ")";
                            ANGOBSearchResult angob = Context.ExecuteStoreQuery<ANGOBSearchResult>(ANGOBQuery, null).FirstOrDefault();
                            if (angob != null)
                            {
                                LoopANGEBOTShortDto.ANGOBJAHRESKM = angob.JAHRESKM;
                                //LoopANGEBOTShortDto.AHKEXTERNBRUTTO = angob.AHK;//decimal?
                                LoopANGEBOTShortDto.ANGKALKBGEXTERNBRUTTO = angob.AHK;
                                LoopANGEBOTShortDto.OBJEKTVT = angob.BEZEICHNUNG;
                                LoopANGEBOTShortDto.ANGKALKBGEXTERNBRUTTO = angob.AHKBRUTTO;//HCEB-697
                                
                            }
                        }

                        if (LoopANGEBOTShortDto.SYSBERATADDB != null && LoopANGEBOTShortDto.SYSBERATADDB > 0)
                        {
                            string PERSONQuery = "SELECT SYSPERSON SYSVK, NAME VERKAUFERNAME, VORNAME VERKAUFERVORNAME FROM CIC.PERSON WHERE SYSPERSON = " + LoopANGEBOTShortDto.SYSBERATADDB;
                            PersonSearchResult pers = Context.ExecuteStoreQuery<PersonSearchResult>(PERSONQuery, null).FirstOrDefault();

                            LoopANGEBOTShortDto.SYSVK = pers.SYSVK;
                            LoopANGEBOTShortDto.VERKAUFERNAME = pers.VERKAUFERNAME;
                            LoopANGEBOTShortDto.VERKAUFERVORNAME = pers.VERKAUFERVORNAME;
                        }
                        if (LoopANGEBOTShortDto.SYSVORVT.HasValue && LoopANGEBOTShortDto.SYSVORVT.Value > 0)
                        {
                            LoopANGEBOTShortDto.VORVERTRAGSNUMMER = Context.ExecuteStoreQuery<String>("select vertrag vorvertragsnummer from vt,kalk,ob where kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + LoopANGEBOTShortDto.SYSVORVT.Value, null).FirstOrDefault();

                        }
                    }

                }

                return result;
            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSearchFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSearchFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSearchFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }



        /// <summary>
        /// Loads an offer
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto Deliver(long sysID)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysID))
                        throw new Exception("No Permission to ANGEBOT");
                }

                bool isIM = Cic.OpenLease.Common.MembershipProvider.isInternalMitarbeiter(ServiceValidator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);
                ANGEBOTDto rval = AngebotDtoHelper.Deliver(sysID, ServiceValidator.SysBRAND, ServiceValidator.SysPEROLE, ServiceValidator.VpSysPERSON.GetValueOrDefault(), ServiceValidator.SysPERSON, ServiceValidator.SYSWFUSER, ServiceValidator.SYSPUSER);

                if (rval == null) throw new Exception("ANGEBOT not readable");

                //set status to work in progress if a im opens a specialcalc requested offer
                if (isIM && rval.SPECIALCALCSTATUS == 1)
                {
                    rval.SPECIALCALCSTATUS = 2;
                    rval.SPECIALCALCSYSWFUSER = ServiceValidator.SYSWFUSER;
                    rval.SPECIALCALCDATE = System.DateTime.Today;
                    try
                    {
                        using (DdOlExtended context = new DdOlExtended())
                        {
                            ANGEBOT originalANGEBOT = (from c in context.ANGEBOT
                                                       where c.SYSID == sysID
                                                       select c).FirstOrDefault();//context.SelectById<ANGEBOT>(sysID);
                            originalANGEBOT.SPECIALCALCSTATUS = rval.SPECIALCALCSTATUS;
                            originalANGEBOT.SPECIALCALCSYSWFUSER = rval.SPECIALCALCSYSWFUSER;
                            originalANGEBOT.SPECIALCALCDATE = rval.SPECIALCALCDATE;
                            context.SaveChanges();
                            //context.Update<ANGEBOT>(originalANGEBOT, null);

                        }
                    }
                    catch (System.Exception e)
                    {

                        // Log the exception
                        _Log.Error("Error saving Specialcalstatus 2", e);

                    }
                }

                //wenn ein interner mitarbeiter ein angebot eines verkäufers öffnet, eine sonderkalkulation draus machen - test!
                if (isIM && (!rval.SPECIALCALCSTATUS.HasValue || rval.SPECIALCALCSTATUS.Value == 0) && rval.SYSBERATADDB != ServiceValidator.SysPEROLE)
                {
                    rval.SPECIALCALCSTATUS = 4;//status for im einreichung for vk
                    rval.SPECIALCALCSYSWFUSER = -1;
                    rval.SPECIALCALCDATE = System.DateTime.Today;
                }
                 using (DdOlExtended context = new DdOlExtended())
                        {
                            rval.ANTRAGSSTATUS = context.ExecuteStoreQuery<String>(@"select  (SELECT extstate.zustand 
FROM attribut,attributdef,state,statedef extstate,statedef intstate,wftable,antrag 
WHERE attribut.sysstate = state.sysstate
and antrag.sysangebot=angebot.sysid
AND attribut.sysattributdef= attributdef.sysattributdef
AND attribut.sysstatedef = extstate.sysstatedef
AND state.sysstatedef =intstate.sysstatedef
AND state.syswftable = wftable.syswftable
AND wftable.syscode = 'ANTRAG' and UPPER(attributdef.attribut) = upper(antrag.attribut) and upper(intstate.zustand)=upper(antrag.zustand))  ANTRAGSSTATUS from angebot where sysid=" +rval.SYSID, null).FirstOrDefault();
                }
                return rval;
            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotDeliverFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotDeliverFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotDeliverFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }

        }

        /// <summary>
        /// Saves Angebot to database 
        /// </summary>
        /// <param name="angebotDto">AngebotDto
        ///<list type="bullet">
        ///<item>Angebot:ANGEBOT1 - ANGEBOT1: Offer identification number (*)<br />Angebotsnummer</item>
        ///<item>Angebot:BEGINN - BEGINN: Offer start date<br /></item>
        ///<item>Angebot:ENDE - ENDE: Offer end date<br /><br /></item>
        ///<item>Angebot:ERFASSUNG - ERFASSUNG: Offer creation date (*)<br /><br /></item>
        ///<item>Angebot:ZUSTAND - ZUSTAND: Offer state<br />Status</item>
        ///<item>Angebot:ZUSTANDAM - ZUSTANDAM: Offer state date<br />/item>
        ///<item>Angkalk:BGEXTERN - ANGKALKBGEXTERN: Total price netto (*)<br />Anschaffungswert Netto</item>
        ///<item>Angkalk:BGEXTERNNACHLBRUTTO - ANGKALKBGEXTERNNACHLBRUTTO: Total price discount brutto (*)<br /></item>
        ///<item>Angkalk:BGEXTERNBRUTTO - ANGKALKBGEXTERNBRUTTO: Total price brutto (*)<br />Anschaffungswert Brutto</item>
        ///<item>Angkalk:BGEXTERNUST - ANGKALKBGEXTERNUST: Income tax (*)<br />Anschaffungswert Umsatzsteuer</item>
        ///<item>Angkalk:DEPOT - ANGKALKDEPOT: Deposit<br />Depotleistung (Betrag)</item>
        ///<item>Angkalk:DEPOTP - ANGKALKDEPOTP: Deposit in percent<br /></item>
        ///<item>Angkalkfs:ANABMLDFLAG - ANGKALKFSANABMLDFLAG: Registration flag<br />An- und Abmeldegebühren: Service JA / NEIN</item>
        ///<item>Angkalkfs:INSASSENFLAG - ANGKALKFSINSASSENFLAG: Passengers insurance flag<br />Insassenunfallversicherung JA / NEIN</item>
        ///<item>Angkalkfs:ANABBRUTTO - ANGKALKFSANABBRUTTO: Registration fee brutto<br />An- und Abmeldegebühren Rate Brutto</item>
        ///<item>Angkalkfs:ANABMELDUNG - ANGKALKFSANABMELDUNG: Registration fee netto<br />An- und Abmeldegebühren Rate Netto</item>
        ///<item>Angkalkfs:ANABUST - ANGKALKFSANABUST: Registration fee tax<br />An- und Abmeldegebühren Rate Umsatzsteuer</item>
        ///<item>Angkalkfs:EXTRASBRUTTO - ANGKALKFSEXTRASBRUTTO: Extras total price<br />Sonstige Dienstleistungen Rate Brutto</item>
        ///<item>Angkalkfs:EXTRASFLAG - ANGKALKFSEXTRASFLAG: Extras flag<br />Sonstige Dienstleistungen JA / NEIN</item>
        ///<item>Angkalkfs:EXTRASPRICE - ANGKALKFSEXTRASPRICE: Extras price netto<br />Sonstige Dienstleistungen Rate Netto</item>
        ///<item>Angkalkfs:EXTRASUST - ANGKALKFSEXTRASUST: Extras tax<br />Sonstige Dienstleistungen Rate Umsatzsteuer</item>
        ///<item>Angkalkfs:FUELBRUTTO - ANGKALKFSFUELBfRUTTO: Fuel price brutto<br />Petrol Rate Brutto</item>
        ///<item>Angkalkfs:FUELFLAG - ANGKALKFSFUELFLAG: Fuel flag<br />Petrolservice JA / NEIN</item>
        ///<item>Angkalkfs:FUELPRICE - ANGKALKFSFUELPRICE: Fuel price netto<br />Petrol Rate Netto</item>
        ///<item>Angkalkfs:FUELUST - ANGKALKFSFUELUST: Fuel tax<br />Petrol Rate Umsatzsteuer</item>
        ///<item>Angkalkfs:HPFLAG - ANGKALKFSHPFLAG: Liability insurance flag<br />Haftpflichtversicherung JA / NEIN</item>
        ///<item>Angkalkfs:MAINTENANCE - ANGKALKFSMAINTENANCE: Maintenance price netto<br />Wartung und Reparatur Netto</item>
        ///<item>Angkalkfs:MAINTENANCEBRUTTO - ANGKALKFSMAINTENANCEBRUTTO: Maintenance price brutto<br />Wartung und Reparatur Brutto</item>
        ///<item>Angkalkfs:MAINTENANCEFLAG - ANGKALKFSMAINTENANCEFLAG: Maintenance flag<br />Wartung und Reparatur JA / NEIN</item>
        ///<item>Angkalkfs:MAINTENANCEUST - ANGKALKFSMAINTENANCEUST: Maintenance tax<br />Wartung und Reparatur Umsatzsteuer</item>
        ///<item>Angkalkfs:MAINTFIXFLAG - ANGKALKFSMAINTFIXFLAG: Maintenance fix flag<br />Wartung und Reparatur: Fixe Ratenkalkulation JA / NEIN</item>
        ///<item>Angkalkfs:MEHRKM - ANGKALKFSMEHRKM: Expected mileage fee<br />Verrechnungssatz Wartung und Rep. pro Mehrkm Netto</item>
        ///<item>Angkalkfs:MINDERKM - ANGKALKFSMINDERKM: Return price for kilometer below expected mileage<br />Minderkm</item>
        ///<item>Angkalkfs:RECHTSCHUTZFLAG - ANGKALKFSRECHTSCHUTZFLAG: Legal protection insurance<br />Rechtschutzversicherung JA / NEIN</item>
        ///<item>Angkalkfs:REPCARCOUNT - ANGKALKFSREPCARCOUNT: Expected repair count<br />Ersatzfahrzeug Anzahl der Tage</item>
        ///<item>Angkalkfs:REPCARFLAG - ANGKALKFSREPCARFLAG: Repair flag<br />Ersatzfahrzeugservice JA / NEIN</item>
        ///<item>Angkalkfs:REPCARPRICE - ANGKALKFSREPCARPRICE: Repair service price<br />Ersatzfahrzeug Preis pro Tag Netto</item>
        ///<item>Angkalkfs:REPCARRATE - ANGKALKFSREPCARRATE: Repair monthly rate<br />Ersatzfahrzeug Rate Netto</item>
        ///<item>Angkalkfs:REPCARRATEBRUTTO - ANGKALKFSREPCARRATEBRUTTO: Repair monthly rate brutto<br />Ersatzfahrzeug Rate Brutto</item>
        ///<item>Angkalkfs:REPCARRATEUST - ANGKALKFSREPCARRATEUST: Repair monthly rate tax<br />Ersatzfahrzeug Rate Umsatzsteuer</item>
        ///<item>Angkalkfs:RIMSCODEH - ANGKALKFSRIMSCODEH: Rear rims code<br />RimsCodeH</item>
        ///<item>Angkalkfs:RIMSCODEV - ANGKALKFSRIMSCODEV: Front rims code<br />Felgen Code vorne</item>
        ///<item>Angkalkfs:RIMSCOUNTH - ANGKALKFSRIMSCOUNTH: Rear rims count<br />Felgen Anzahl hinten</item>
        ///<item>Angkalkfs:RIMSCOUNTV - ANGKALKFSRIMSCOUNTV: Front rims count<br />Felgen Anzahl vorne</item>
        ///<item>Angkalkfs:RIMSPRICEH - ANGKALKFSRIMSPRICEH: Rear rims price<br />Felgen Preis hinten Netto</item>
        ///<item>Angkalkfs:RIMSPRICEV - ANGKALKFSRIMSPRICEV: Front rims price<br />Felgen Preis vorne Netto</item>
        ///<item>Angkalkfs:STIRESCODEH - ANGKALKFSSTIRESCODEH: Rear summer tires code<br />Sommerreifen Code hinten</item>
        ///<item>Angkalkfs:STIRESCODEV - ANGKALKFSSTIRESCODEV: Front summer tires code<br />Sommerreifen Code vorne</item>
        ///<item>Angkalkfs:STIRESCOUNTH - ANGKALKFSSTIRESCOUNTH: Rear summer tires count<br />Sommerreifen Anzahl hinten</item>
        ///<item>Angkalkfs:STIRESCOUNTV - ANGKALKFSSTIRESCOUNTV: Front summer tires count<br />Sommerreifen Anzahl vorne</item>
        ///<item>Angkalkfs:STIRESMODH - ANGKALKFSSTIRESMODH: Rear summer tires extras<br />Sommerreifen Marke hinten</item>
        ///<item>Angkalkfs:STIRESMODV - ANGKALKFSSTIRESMODV: Front summer tires extras<br />Sommerreifen Marke vorne</item>
        ///<item>Angkalkfs:STIRESPRICE - ANGKALKFSSTIRESPRICE: Summer tires price<br />Reifen Gesamtrate Netto</item>
        ///<item>Angkalkfs:STIRESPRICEH - ANGKALKFSSTIRESPRICEH: Rear summer tires price<br />Sommerreifen Preis hinten Netto</item>
        ///<item>Angkalkfs:STIRESPRICEHBRUTTO - ANGKALKFSSTIRESPRICEHBRUTTO: Rear summer tires price brutto<br />Sommerreifen Preis hinten Brutto</item>
        ///<item>Angkalkfs:STIRESPRICEHUST - ANGKALKFSSTIRESPRICEHUST: Rear summer tires tax<br />Sommerreifen Preis hinten Umsatzsteuer</item>
        ///<item>Angkalkfs:STIRESPRICEV - ANGKALKFSSTIRESPRICEV: Front summer tires price<br />Sommerreifen Preis vorne Netto</item>
        ///<item>Angkalkfs:STIRESPRICEVBRUTTO - ANGKALKFSSTIRESPRICEVBRUTTO: Front summer tires price brutto<br />Sommerreifen Preis vorne Brutto</item>
        ///<item>Angkalkfs:STIRESPRICEVUST - ANGKALKFSSTIRESPRICEVUST: Front summer tires tax<br />Sommerreifen Preis vorne Umsatzsteuer</item>
        ///<item>Angkalkfs:TIRESADDITION - ANGKALKFSTIRESADDITION: Additional tires rate<br />Zusatzkosten pro Reifen Netto</item>
        ///<item>Angkalkfs:TIRESADDITIONBRUTTO - ANGKALKFSTIRESADDITIONBRUTTO: Tires extras price brutton<br />Zusatzkosten pro Reifen Brutto</item>
        ///<item>Angkalkfs:TIRESADDITIONUST - ANGKALKFSTIRESADDITIONUST: Tires extras tax<br /></item>
        ///<item>Angkalkfs:TIRESFIXFLAG - ANGKALKFSTIRESFIXFLAG: Tires fix flag<br />Reifen Nachkalkulation ausgeschlossen JA / NEIN</item>
        ///<item>Angkalkfs:TIRESFLAG - ANGKALKFSTIRESFLAG: Tires flag<br />Reifen Limitierung JA / NEIN</item>
        ///<item>Angkalkfs:VKFLAG - ANGKALKFSVKFLAG: Full insurance flag<br />Vollkaskoversicherung JA / NEIN</item>
        ///<item>Angkalkfs:WTIRESCODEH - ANGKALKFSWTIRESCODEH: Reat winter tires code<br />Winterreifen Code hinten</item>
        ///<item>Angkalkfs:WTIRESCODEV - ANGKALKFSWTIRESCODEV: Front winter tires code<br />Winterreifen Code vorne</item>
        ///<item>Angkalkfs:WTIRESCOUNTH - ANGKALKFSWTIRESCOUNTH: Rear winter tires count<br />Winterreifen Anzahl hinten</item>
        ///<item>Angkalkfs:WTIRESCOUNTV - ANGKALKFSWTIRESCOUNTV: Front winter tires count<br />Winterreifen Anzahl vorne</item>
        ///<item>Angkalkfs:WTIRESMODH - ANGKALKFSWTIRESMODH: Rear winter tires extras<br />Winterreifen Marke hinten</item>
        ///<item>Angkalkfs:WTIRESMODV - ANGKALKFSWTIRESMODV: Front winter tires extras<br />Winterreifen Marke vorne</item>
        ///<item>Angkalkfs:WTIRESPRICEH - ANGKALKFSWTIRESPRICEH: Rear winter tires price<br />Winterreifen Preis hinten Netto</item>
        ///<item>Angkalkfs:WTIRESPRICEHBRUTTO - ANGKALKFSWTIRESPRICEHBRUTTO: Rear winter tires price brutto<br />Winterreifen Preis hinten Brutto </item>
        ///<item>Angkalkfs:WTIRESPRICEHUST - ANGKALKFSWTIRESPRICEHUST: Rear winter tires tax<br />Winterreifen Preis hinten Umsatzsteuer</item>
        ///<item>Angkalkfs:WTIRESPRICEV - ANGKALKFSWTIRESPRICEV: Front winter tires price<br />Winterreifen Preis vorne Netto</item>
        ///<item>Angkalkfs:WTIRESPRICEVBRUTTO - ANGKALKFSWTIRESPRICEVBRUTTO: Front winter tires price brutto<br />Winterreifen Preis vorne Brutto</item>
        ///<item>Angkalkfs:WTIRESPRICEVUST - ANGKALKFSWTIRESPRICEVUST: Front winter tires tax<br />Winterreifen Preis vorne Umsatzsteuer\</item>
        ///<item>Angkalk:GEBUEHR - ANGKALKGEBUEHR: Additional fees<br />Bearbeitungsgebühr Netto</item>
        ///<item>Angkalk:GEBUEHRBRUTTO - ANGKALKGEBUEHRBRUTTO: Additional fees brutto<br />Bearbeitungsgebühr Brutto</item>
        ///<item>Angkalk:GRUNDB - ANGKALKGRUND: Total price brutto (*)<br />Listenpreis Netto vor offenen Nachlass</item>
        ///<item>Angkalk:GRUNDBRUTTO - ANGKALKGRUNDBRUTTO: Total price brutto (*)<br />Listenpreis Brutto vor offenen Nachlass</item>
        ///<item>Angkalk:GRUNDNACHLBRUTTO - ANGKALKGRUNDNACHLBRUTTO: Discounts brutto (*)<br /></item>
        ///<item>Angkalk:GRUNDNETTO - ANGKALKGRUNDNETTO: Total price netto (*)<br /></item>
        ///<item>Angkalk:HERZUBBRUTTO - ANGKALKHERZUBBRUTTO: Manufacturer additional price brutto (?)<br />Herstellerzubehör Brutto vor offenen Nachlass</item>
        ///<item>Angkalk:HERZUBNACHLBRUTTO - ANGKALKHERZUBNACHLBRUTTO: Manufacturer additional price discount brutto<br /></item>
        ///<item>Angkalk:HERZUBNETTO - ANGKALKHERZUBNETTO: Manufacturer additional price netto<br /></item>
        ///<item>Angkalk:HERZUBRABO - ANGKALKHERZUBRABO: Open (visible) discount<br /></item>
        ///<item>Angkalk:HERZUBRABOP - ANGKALKHERZUBRABOP: Open (visible) discount in percent<br /></item>
        ///<item>Angob:JAHRESKM - ANGOBJAHRESKM: Expected mileage per year<br />jährliche Laufleistung des Fahrzeugs in km</item>
        ///<item>Angkalk:LZ - ANGKALKLZ: Contract duration<br />Laufzeit (Monate)</item>
        ///<item>Angkalk:MITFINBRUTTO - ANGKALKMITFINBRUTTO: ???<br />Mitfinanzierten Bestandteile Brutto</item>
        ///<item>Angkalk:MITFINUST - ANGKALKMITFINUST: ???<br />Mitfinanzierten Bestandteile Umsatzsteuer</item>
        ///<item>Angkalk:PAKETEBRUTTO - ANGKALKPAKETEBRUTTO: Total packets price brutto (*)<br /></item>
        ///<item>Angkalk:PAKETENETTO - ANGKALKPAKETENETTO: Total packets price netto (*)<br /></item>
        ///<item>Angkalk:PAKETEUST - ANGKALKPAKETEUST: Total packets tax (*)<br /></item>
        ///<item>Angkalk:PAKETENACHLBRUTTO - ANGKALKPAKETENACHLBRUTTO: Packets discount brutto<br /></item>
        ///<item>Angkalk:PAKRABO - ANGKALKPAKRABO: Open (visible) packets discount<br /></item>
        ///<item>Angkalk:PAKRABOP - ANGKALKPAKRABOP: Open (visible) packets discount in percent<br /></item>
        ///<item>Angkalk:PPY - ANGKALKPPY: Periods per year<br />Zahlungsmodus</item>
        ///<item>Angkalk:RABATTO - ANGKALKRABATTO: Total discount netto<br /></item>
        ///<item>Angkalk:RABATTOP - ANGKALKRABATTOP: Total discount in percent<br /></item>
        ///<item>Angkalk:RATE - ANGKALKRATE: Rate<br />berechnete Rate Netto</item>
        ///<item>Angkalk:RATEBRUTTO - ANGKALKRATEBRUTTO: Brutto rate<br />berechnete Rate Brutto</item>
        ///<item>Angkalk:RATEUST - ANGKALKRATEUST: Rate tax<br />berechnete Rate Umsatzsteuer</item>
        ///<item>Angkalk:RGGEBUEHR - ANGKALKRGGEBUEHR: RGG fee<br />Rechtsgeschäftsgebühr (Betrag)</item>
        ///<item>Angkalk:RGGFREI - ANGKALKRGGFREI: RGG free<br />Rechtsgeschäftsgebühr Befreiung JA / NEIN</item>
        ///<item>Angkalk:RGGVERR - ANGKALKRGGVERR: RGG offset<br />RGG-Zahlungsart</item>
        ///<item>Angkalk:RWBASE - ANGKALKRWBASE: Residual value base netto<br />Restwertvorschlag Netto</item>
        ///<item>Angkalk:RWBASEBRUTTO - ANGKALKRWBASEBRUTTO: Residual value base brutto<br />Restwertvorschlag Brutto</item>
        ///<item>Angkalk:RWBASEBRUTTOP - ANGKALKRWBASEBRUTTOP: Residual value base in percent<br />Restwertvorschlag in %</item>
        ///<item>Angkalk:RWBASEUST - ANGKALKRWBASEUST: Residual value base tax<br />Restwertvorschlag Umsatzsteuer</item>
        ///<item>Angob:RWCRV - ANGOBRWCRV: Residual value CRV<br /></item>
        ///<item>Angkalk:RWCRVBRUTTO - ANGKALKRWCRVBRUTTO: Residual value CRV brutto<br />Restwert CRV Brutto</item>
        ///<item>Angkalk:RWCRVBRUTTOP - ANGKALKRWCRVBRUTTOP: Residual value CRV in percent<br />Restwert CRV in %</item>
        ///<item>Angkalk:RWCRVUST - ANGKALKRWCRVUST: Residual value CRV tax<br />Restwert CRV Umsatzsteuer</item>
        ///<item>Angkalk:RWKALK - ANGKALKRWKALK: Calculatory residual value<br />Restwert Netto</item>
        ///<item>Angkalk:RWKALKBRUTTO - ANGKALKRWKALKBRUTTO: Calculatory residual value brutto<br />Restwert Brutto</item>
        ///<item>Angkalk:RWKALKBRUTTOP - ANGKALKRWKALKBRUTTOP: Calculatory residual value in percent<br />Restwert in %</item>
        ///<item>Angkalk:RWKALKUST - ANGKALKRWKALKUST: Calculatory residual value tax<br />Restwert Umsatzsteuer</item>
        ///<item>Angkalk:SONZUBBRUTTO - ANGKALKSONZUBBRUTTO: Special additions price brutto<br /></item>
        ///<item>Angkalk:SONZUBNACHLBRUTTO - ANGKALKSONZUBNACHLBRUTTO: Special additions discount price brutto<br /></item>
        ///<item>Angkalk:SONZUBNETTO - ANGKALKSONZUBNETTO: Special additions price netto<br /></item>
        ///<item>Angkalk:SONZUBRABO - ANGKALKSONZUBRABO: Special additions open (visible) discount<br /></item>
        ///<item>Angkalk:SONZUBRABOP - ANGKALKSONZUBRABOP: Special additions open (visible) discount in percent<br /></item>
        ///<item>Angkalk:SZ - ANGKALKSZ: First rate<br />Anzahlung / Sonderzahlung Netto</item>
        ///<item>Angkalk:SZBRUTTO - ANGKALKSZBRUTTO: First rate brutto<br />Anzahlung / Sonderzahlung Brutto</item>
        ///<item>Angkalk:SZBRUTTOP - ANGKALKSZBRUTTOP: First rate in percent<br />Anzahlung in Prozent des Anschaffungswertes</item>
        ///<item>Angkalk:SZUST - ANGKALKSZUST: First rate tax<br />Anzahlung / Sonderzahlung Umsatzsteuer</item>
        ///<item>Angkalk:ZUBEHOERBRUTTO - ANGKALKZUBEHOERBRUTTO: Regular additions price brutto (*)<br />Händlerzubehör vor offenen Nachlass Brutto</item>
        ///<item>Angkalk:ZUBEHOERNETTO - ANGKALKZUBEHOERNETTO: Regular additions price netto (*)<br /></item>
        ///<item>Angkalk:ZUBEHOEROR - ANGKALKZUBEHOEROR: ???<br /></item>
        ///<item>Angkalk:ZUBEHOERORP - ANGKALKZUBEHOERORP: ???<br /></item>
        ///<item>Angob:AUTOMATIK - ANGOBAUTOMATIK: Automatic transmission flag (*)<br />Getriebeart Automatik</item>
        ///<item>Angob:FABRIKAT - ANGOBFABRIKAT: Brand (*)<br />Fahrzeugmodell, z.B. 3-er Touring</item>
        ///<item>Angob:FARBEA - ANGOBFARBEA: Body color<br />Außenfarbe</item>
        ///<item>Angob:FZART - ANGOBFZART: Vehicle art<br />Fahrzeugart</item>
        ///<item>Angob:FZNR - ANGOBFZNR: Vehicle number<br /></item>
        ///<item>Angob:HERSTELLER - ANGOBHERSTELLER: Manufacturer (*)<br />Hersteller des Fahrzeugs</item>
        ///<item>Angobini:ACTUATION - ANGOBINIACTUATION: Actuation (hybrid or not) (*)<br />Antriebsart</item>
        ///<item>Angob:BAUJAHR - ANGOBBAUJAHR: Production year<br /></item>
        ///<item>Angob:CCM - ANGOBCCM: Capacity (*)<br /></item>
        ///<item>Angobini:CO2 - ANGOBINICO2: Co2 emission (*)<br />Co2-Emissionen in g/km</item>
        ///<item>Angobini:KMSTAND - ANGOBINIKMSTAND: Starting mileage<br />Kilometerstand</item>
        ///<item>Angobini:NOX - ANGOBININOX: Nitrous oxides (*)<br />NoX Emission in mg/km</item>
        ///<item>Angobini:PARTICLES - ANGOBINIPARTICLES: Particles (*)<br />Partikelausstoß in g/km</item>
        ///<item>Angobini:VERBRAUCH_D - ANGOBINIVERBRAUCH_D: Average consumption (*)<br />Durchschnittverbrauch in l/100 km</item>
        ///<item>Angobini:VORBESITZER - ANGOBINIVORBESITZER: Previous owner<br />Anzahl der Vorbesitzer</item>
        ///<item>Angob:KMTOLERANZ - ANGOBKMTOLERANZ: Mileage margin<br />Toleranzgrenze für Mehr/Minder-km</item>
        ///<item>Angob:KW - ANGOBKW: Power (*)<br /></item>
        ///<item>Angob:LIEFERUNG - ANGOBLIEFERUNG: Delivery date<br />Auslieferungsdatum</item>
        ///<item>Angob:NOVA - ANGOBNOVA: Nova rate (*)<br />NoVA-Befreiung</item>
        ///<item>Angob:NOVAP - ANGOBNOVAP: Nova rate in percent (*)<br /></item>
        ///<item>Angob:SERIE - ANGOBSERIE: Series<br />Fahrgestellnummer des Fahrzeugs</item>
        ///<item>Angob:TYP - ANGOBTYP: Type<br />Fahrzeugtyp</item>
        ///<item>IT:HSNR - ITHSNR: Customer house number<br /></item>
        ///<item>IT:NAME - ITNAME: Customer name<br /></item>
        ///<item>IT:ORT - ITORT: Customer city<br /></item>
        ///<item>IT:PLZ - ITPLZ: Customer postal code<br /></item>
        ///<item>IT:STRASSE - ITSTRASSE: Customer street<br /></item>
        ///<item>IT:VORNAME - ITVORNAME: Customer first name<br /></item>
        ///<item>Angebot:OBJEKTVT - OBJEKTVT: Object description (* Brand/Model/Type)<br />Enthält die zusammengesetzten Felder: Hersteller, Typ, Fabrikat</item>
        ///<item>Angkalk:ZINSTYP - ANGKALKZINSTYP: Interest<br />Verzinsungsart</item>
        ///<item>Angkalk:ZINSEFF - ANGKALKZINSEFF: Effective interest<br />Effektivzinssatz</item>
        ///<item>Angob:SCHWACKE - ANGOBSCHWACKE: Eurotax number (*)<br />Eurotax / Schwacke-Code</item>
        ///<item>Angob:BGN - ANGOBBGN: Expected usual life<br /></item>
        ///<item>Angob:SYSKGRUPPE - ANGOBSYSKGRUPPE: Customer group<br /></item>
        ///<item>Angob:USGAAP - ANGOBUSGAAP: US-GAAP<br /></item>
        ///<item>Angkalk:HERZUB - ANGKALKHERZUB: Manufacturer additions price netto (*?)<br />Herstellerzubehör Netto vor offenen Nachlass</item>
        ///<item>Angkalk:NOVAZUABBRUTTO - ANGKALKNOVAZUABBRUTTO: Nova addtional/reduction brutto<br />NoVA Zu-/Abschlag Brutto</item>
        ///<item>Angkalk:HERZUBUST - ANGKALKHERZUBUST: Manufacturer additions price tax (*?)<br /></item>
        ///<item>Angob:NOVAZUAB - ANGOBNOVAZUAB: Nova addtional/reduction netto<br />NoVA Zu-/Abschlag Netto</item>
        ///<item>Angob:INVENTAR - ANGOBINVENTAR: Inventory (???)<br /></item>
        ///<item>Angob:NOVABRUTTO - ANGOBNOVABRUTTO: Nova tax brutto<br />NoVA Brutto</item>
        ///<item>Angob:NOVABETRAG - ANGOBNOVABETRAG: Total nova brutto<br />NoVA Netto</item>
        ///<item>Angkalk:VERRECHNUNG - ANGKALKVERRECHNUNG: Offset<br />Abdeckungsbetrag eines Vorkredits</item>
        ///<item>Angkalk:REFIZINS1 - ANGKALKREFIZINS1: Real interest<br />Refi-Zinssatz</item>
        ///<item>Angkalk:ZINS1 - ANGKALKZINS1: Interest<br /></item>
        ///<item>Angkalk:GEBUEHRUST - ANGKALKGEBUEHRUST: Fee<br />Bearbeitungsgebühr Umsatzsteuer</item>
        ///<item>Angkalk:GEBUEHRINTERNBRUTTO - ANGKALKGEBUEHRINTERNBRUTTO: Internal fee brutto<br />Bearbeitungsgebühr intern Brutto</item>
        ///<item>Angob:SATZMEHRKM - ANGOBSATZMEHRKM: Maximal expected annual mileage<br />Verrechnungssatz für Mehr-KM (Basis Finanzierung) NETTO</item>
        ///<item>Angob:SATZMINDERKM - ANGOBSATZMINDERKM: Minimal expected annual mileage<br />Verrechnungssatz für Minder-KM (Basis Finanzierung) NETTO</item>
        ///<item>Angebot:DATANGEBOT - DATANGEBOT: Offer date<br />Angebotsdatum</item>
        ///<item>Angob:OBJEKT - ANGOBOBJEKT: Object short description<br /></item>
        ///<item>Angebot:SYSVART - SYSVART: Contract art<br /></item>
        ///<item>Angkalk:SYSKALKTYP - ANGKALKSYSKALKTYP: Calculation type<br /></item>
        ///<item>Angebot:SYSVTTYP - SYSVTTYP: Contract type<br /></item>
        ///<item>Angebot:SYSPRPRODUCT - SYSPRPRODUCT: Product<br /></item>
        ///<item>Angebot:RATE - RATE: Rate<br />Rate Netto</item>
        ///<item>Angebot:VART - VART: Vertragsart</item>
        ///</item>
        ///</list>
        ///</param>
        /// <returns>AngebotDto</returns>
        public Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto Save(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                bool isIM = Cic.OpenLease.Common.MembershipProvider.isInternalMitarbeiter(ServiceValidator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);
                // if (!isIM)//CR 4373 Wenn Verkäufer speichert status wieder zurücksetzen
                //   angebotDto.SPECIALCALCSTATUS = 0;

                Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDtobackup = angebotDto;
                _Log.Debug("INFO upon Pre-Save: " + _Log.dumpObject(angebotDto));

                //Avoid SAVE when eingereicht!
                if (angebotDto.SYSID.HasValue && angebotDto.SYSID.Value > 0)
                {
                    using (DdOlExtended Context = new DdOlExtended())
                    {
                        // Check if the status is valid
                        if (!ZustandHelper.VerifyAngebotStatus(angebotDto.SYSID.Value, Context, AngebotZustand.Neu, AngebotZustand.Kalkuliert, AngebotZustand.Gedruckt, AngebotZustand.NeuResubmit))
                        {
                            // Throw an exception
                            throw new Exception("Ungültiger ANGEBOT Status");
                        }
                    }                  
                }


                Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDtoresult = AngebotDtoHelper.Save(angebotDto, ServiceValidator.SysBRAND, ServiceValidator.SysPEROLE, ServiceValidator.VpSysPERSON.GetValueOrDefault(), ServiceValidator.SysPERSON, ServiceValidator.SYSWFUSER, ServiceValidator.SYSPUSER);

                _Log.Debug("INFO Save-Result: " + _Log.dumpObject(angebotDtoresult));
                flushSearchCache();
                return angebotDtoresult;
            }
            catch (Exception exception)
            {

                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSaveFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSaveFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSaveFailed.ToString(), exception);
                if (exception is ServiceException)
                {
                    ServiceException se = (ServiceException)exception;
                    if (se.Code != null && se.Code.Name != null && se.Code.Name.Equals(((int)Cic.OpenLease.ServiceAccess.ServiceCodes.SaveAngebotFailedProduct).ToString()))
                    {
                        ANGEBOTDto rval = new ANGEBOTDto();
                        rval.ERRORMESSAGE = exception.Message;
                        rval.ERRORDETAIL = exception.StackTrace;
                        return rval;
                    }
                }
                // Throw the exception
                throw TopLevelException;

            }
        }

        /// <summary>
        /// Copies an offer for a special calc
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public ANGEBOTDto CopyForSpecialCalc(long sysId)
        {
            return CopyAngebot(sysId, true);
        }

        /// <summary>
        /// Copies an offer
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public ANGEBOTDto Copy(long sysId)
        {
            return CopyAngebot(sysId, false);
        }

        /// <summary>
        /// copy an offer
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="forSpecialcalc">wenn true wird die SK modifiziert, wenn false nur eine vorlage für ein neues Angebot</param>
        /// <returns></returns>
        private ANGEBOTDto CopyAngebot(long sysId, bool forSpecialcalc)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                ANGEBOTDto AngebotDto = copyAngebot(sysId, forSpecialcalc, ServiceValidator.SysBRAND, ServiceValidator.SysPEROLE, ServiceValidator.VpSysPERSON.GetValueOrDefault(), ServiceValidator.SysPERSON, ServiceValidator.SYSWFUSER, ServiceValidator.SYSPUSER);
                // Return angebot
                return AngebotDto;

            }

            catch (Exception exception)
            {

                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotCopyFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotCopyFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotCopyFailed.ToString(), exception);
                if (exception is ServiceException)
                {
                    ServiceException se = (ServiceException)exception;
                    if (se.Code != null && se.Code.Name != null && se.Code.Name.Equals(((int)Cic.OpenLease.ServiceAccess.ServiceCodes.SaveAngebotFailedProduct).ToString()))
                    {
                        ANGEBOTDto rval = new ANGEBOTDto();
                        rval.ERRORMESSAGE = exception.Message;
                        rval.ERRORDETAIL = exception.StackTrace;
                        return rval;
                    }
                }

                // Throw the exception
                throw TopLevelException;
            }
        }

        public static ANGEBOTDto copyAngebot(long sysId, bool forSpecialcalc, long sysbrand, long sysperole, long vpsysperson, long sysperson, long syswfuser, long syspuser)
        {
            ANGEBOTDto AngebotDto = null;
            List<MitantragstellerDto> mitAntragsteller = null;
            long originalSysId = sysId;
            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {

                if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, syspuser, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysId))
                    throw new Exception("No Permission to ANGEBOT");

                // Get angebot with specified id
                AngebotDto = AngebotDtoHelper.Deliver(sysId, sysbrand, sysperole, vpsysperson,sysperson,syswfuser,syspuser);



                // Set the id to null
                AngebotDto.SYSID = null;
                //Ticket #3866 Gebühr nach kopie nicht mehr reduziert
                AngebotDto.ANGKALKGEBUEHR_NACHLASS = 0;
                //unset insurance references to avoid deletion
                if (AngebotDto.ANGVSPARAM != null)
                {
                    foreach (InsuranceDto ins in AngebotDto.ANGVSPARAM)
                        ins.SysAngVs = 0;
                }
                AngebotDto.ERFASSUNG = DateTime.Today;

                long defaultTarif = Context.ExecuteStoreQuery<long>("select sysprovtarif from provtarif where standardflag=1", null).FirstOrDefault();
                

                // Set parent angebot
                //AngebotDto.SYSANGEBOT = sysId;
                if (AngebotDto.SPECIALCALCSTATUS.HasValue && AngebotDto.SPECIALCALCSTATUS.Value > 0)//#172013
                {
                    AngebotDto.SYSPROVTARIF = defaultTarif;
                    AngebotDto.WUNSCHPROVISION = 0;
                }
                AngebotDto.SPECIALCALCSTATUS = 0;
                if (!forSpecialcalc)
                {
                    AngebotDto.SPECIALCALCCOUNT = 0;

                }
                //Laut #3736 soll NICHT ein überschriebener Zinssatz verwendet werden, laut #3835 aber z.B. eine Restwerteingabe erhalten bleiben. 
                //if ( AngebotDto.KALKULATIONSOURCE == BmwCalculationDto.CalculationSources.ZinsEff || AngebotDto.KALKULATIONSOURCE == BmwCalculationDto.CalculationSources.ZinsNominal)
                {
                    AngebotDto.KALKULATIONSOURCE = CalculationDto.CalculationSources.Mietvorauszahlung;//#3736, #4198
                                                                                                       //Reset Provision
                    AngebotDto.SYSPROVTARIF = defaultTarif;
                    AngebotDto.WUNSCHPROVISION = 0;
                }

                AngebotDto.TRADEONOWNACCOUNT = null;
                AngebotDto.WFMMEMOSCALCVKTEXT = null;
                AngebotDto.WFMMEMOSCALCIDTEXT = null;
                AngebotDtoHelper.UpdateAngebotSpecialCalcStatus(AngebotDto);


                mitAntragsteller = getMitantragsteller(originalSysId); 

                //#4152
                AngebotDto.ANGKALKGEBUEHR_NACHLASS = 0;
                AngebotDto.ANGKALKFSANABNACHLASS = 0;
                AngebotDto.ANGKALKFSFUELNACHLASS = 0;
                AngebotDto.ANGKALKFSMAINTNACHLASS = 0;
                AngebotDto.ANGKALKFSTIRESNACHLASS = 0;
                AngebotDto.ANGKALKFSREPCARNACHLASS = 0;
                AngebotDto.ANGKALKFSEXTRASNACHLASS = 0;

                //remove invalid Insurances of copy Defect #4541
                AngebotDtoHelper.validateInsurances(Context, AngebotDto);
            }

            // Save new angebot
            AngebotDto = AngebotDtoHelper.Save(AngebotDto,sysbrand,sysperole,vpsysperson,sysperson,syswfuser, CnstFinKzNotSet, null, syspuser, true);
            


            // MitAntragsteller copy für das neue Angebot
            if (AngebotDto.SYSID.HasValue)
            {
                long AngebotSysid = AngebotDto.SYSID.Value;

                foreach (var maLoop in mitAntragsteller)
                {
                    maLoop.SYSVT = AngebotSysid;
                    maLoop.SysId = 0;
                    saveMitantragstellerData(maLoop);
                }
            }
            //update KNE
            using (DdOwExtended Context = new DdOwExtended())
            {
                ITDto kne = new ITDto();
                kne.KNELIST = Context.ExecuteStoreQuery<ITKNEDto>("select sysober,sysunter,relatetypecode from itkne where area='ANGEBOT' and sysarea=" + sysId + " and sysunter = " + AngebotDto.SYSIT.Value).ToList();
                if (kne.KNELIST != null && kne.KNELIST.Count > 0)
                {
                    foreach (ITKNEDto knedto in kne.KNELIST)
                    {
                        knedto.SYSANGEBOT = AngebotDto.SYSID.Value;
                        kne.KNE = knedto;
                        kne.SYSIT = knedto.SYSOBER;

                        ITAssembler.UpdateKNE(kne);
                    }

                }

            }

            //Copy picture
            using (DdOwExtended context = new DdOwExtended())
            {
                AngebotBinaryDao dao = new AngebotBinaryDao(context);
                if (AngebotDto.SYSID.HasValue)
                    dao.copyPictureData(originalSysId, AngebotDto.SYSID.Value);

                long sysOppo = context.ExecuteStoreQuery<long>("select sysoppo from angebot where sysid=" + originalSysId, null).FirstOrDefault();

                if (sysOppo > 0)
                    context.ExecuteStoreCommand("UPDATE ANGEBOT set sysoppo=" + sysOppo + " where sysid=" + AngebotDto.SYSID.Value, null);

            }

            AngebotSubmitDao.linkDocuments("ANGEBOT", AngebotDto.SYSID.Value, "ANGEBOT", originalSysId);
            if (forSpecialcalc)//for specialcalc, update copy-source specialcalccounter
            {
                using (DdOlExtended Context = new DdOlExtended())
                {
                    ANGEBOT ang = (from a in Context.ANGEBOT
                                   where a.SYSID == sysId
                                   select a).FirstOrDefault();
                    ang.SPECIALCALCCOUNT = 0;
                    Context.SaveChanges();
                }
            }
            flushSearchCache();
            return AngebotDto;
        }


        #endregion


        /// <summary>
        /// Lists all subventions for the offer
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <returns></returns>
        public List<SubventionDto> DeliverSubventionen(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);


            try
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    angebotDto.SYSBRAND = ServiceValidator.SysBRAND;
                    List<SubventionDto> subs = Subvention.DeliverSubventions(angebotDto, ctx, ServiceValidator.VpSysPERSON, ServiceValidator.SysPERSON, ServiceValidator.SysPEROLE);
                    //PEROLE p = PeroleHelper.FindRootPEROLEObjByRoleType(ctx, ServiceValidator.SysPEROLE, PeroleHelper.CnstInternerMitarbeiterRoleTypeNumber);
                    //Log.Debug("Subventionsservice: Interner Mitarbeiter. " + _Log.dumpObject(p));
                    /* if (p == null) //not internal mitarbeiter - filter only subventions of himself
                     {
                         foreach (SubventionDto s in subs)
                         {
                             List<SubventionPosDto> tmpList = new List<SubventionPosDto>();
                             if (s.SUBVENTIONSGEBER != null)
                             {
                                 foreach (SubventionPosDto sp in s.SUBVENTIONSGEBER)
                                 {
                                     if (sp.SYSPERSON == ServiceValidator.SysPERSON)
                                         tmpList.Add(sp);
                                     else _Log.Debug("Filter Subvention " + _Log.dumpObject(sp) + " for non-internal employee");
                                 }
                                 s.SUBVENTIONSGEBER = tmpList.ToArray();
                             }
                         }
                     }*/
                    return subs;
                }
            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverSubventionenFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverSubventionenFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }



        /// <summary>
        /// Validates the product for the given kd type
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="syskdtyp"></param>
        /// <returns></returns>
        public ControlDto ControlProductKdtyp(long sysprproduct, long syskdtyp)
        {
            try
            {
                String key = sysprproduct + "_" + syskdtyp;
                if (!prodKdtypCache.ContainsKey(key))
                {
                    using (DdOlExtended Context = new DdOlExtended())
                    {
                        string Query = "select syskdtyp from prclprktyp where sysprproduct=" + sysprproduct;
                        List<long> kdtyps = Context.ExecuteStoreQuery<long>(Query, null).ToList<long>();
                        if (kdtyps.Count == 0)
                        {
                            prodKdtypCache[key] = null;
                            return null;
                        }
                        foreach (var kdtyp in kdtyps)
                        {
                            if (kdtyp == syskdtyp)
                            {
                                prodKdtypCache[key] = null;
                                return null;
                            }
                        }


                        Query = "select name from kdtyp where syskdtyp in (" + String.Join(",", kdtyps.Select(i => i.ToString()).ToArray()) + ")";
                        List<String> kdtypnames = Context.ExecuteStoreQuery<String>(Query, null).ToList<String>();
                        ControlDto control = new ControlDto();
                        control.CONTROLMESSAGE = "Das ausgewählte Produkt ist für die Kundenart nicht gültig. Bitte wählen Sie ein anderes Finanzierungsprodukt aus.";
                        prodKdtypCache[key] = control;
                    }
                }
                return prodKdtypCache[key];
            }
            catch (Exception e)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotCopyFailed, e);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.SelectionProductInAngebot + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.SelectionProductInAngebot.ToString(), e);

                // Throw the exception
                throw TopLevelException;
            }

        }


        #region Mitantragsteller
        /// <summary>
        /// Removes the mitantragsteller
        /// </summary>
        /// <param name="sysId"></param>
        public void DeleteMitantragsteller(long sysId)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateDelete();

            try
            {
                // Create the context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Delete
                    ANGOBSICHHelper.DeleteAngobsisch(Context, sysId);
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeleteMitantragstellerFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeleteMitantragstellerFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeleteMitantragstellerFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// saves the mitantragsteller
        /// Bei einem Angebot mit Hauptantragsteller als Einzelunternehmern oder Unternehmen gibt es keine Mitantragsteller, sondern nur Bürgen.
        /// 
        ///  Bitte hier die Speicherung im ANGOBSICH-Datensatz entsprechend anpassen (sysSichtyp und Bezeichnung)
        ///  203 = Mitantragsteller - RANG 10
        ///  229 = Bürgschaften/Garantien - RANG 80
        ///  234 = Vertretungsberechtigte - RANG 140
        /// 
        /// </summary>
        /// <param name="mitantragsteller"></param>
        /// <returns></returns>
        public MitantragstellerDto SaveMitantragsteller(MitantragstellerDto mitantragsteller)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();
            MitantragstellerDto ModifiedMitAntragStellerDto = saveMitantragstellerData(mitantragsteller);

            return ModifiedMitAntragStellerDto;
        }

        private static MitantragstellerDto saveMitantragstellerData(MitantragstellerDto mitantragsteller)
        {
            // Create an assembler
            ANGOBSICHAssembler ANGOBSICHAssembler;

            //Cic.OpenLease.Model.DdOl.ANGOBSICH ModifiedANGOBSICH = null;
            Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto ModifiedMitAntragStellerDto = null;

            try
            {
                SICHTYP SICHTYP;
                using (DdOlExtended Context = new DdOlExtended())
                {
                    SICHTYP = SICHTYPHelper.GetSichTyp(Context, mitantragsteller.SICHTYPRANG);
                    //ANGOBSICH = ANGOBSICHHelper.GetAngobisch(Context, sysangebot);


                    //Create data for save
                    mitantragsteller.AKTIVZ = 1;
                    mitantragsteller.BEZEICHNUNG = SICHTYP.BEZEICHNUNG;
                    mitantragsteller.SYSSICHTYP = SICHTYP.SYSSICHTYP;

                    // New assembler
                    ANGOBSICHAssembler = new Cic.OpenLease.Service.ANGOBSICHAssembler();

                    //ANGOBSICH AngobSisch = ANGOBSICHHelper.GetAngobsischById(Context, mitantragsteller.SysId);
                    ANGOBSICH AngobSisch = ANGOBSICHHelper.GetAngobsischByRang(Context, mitantragsteller.SYSVT, mitantragsteller.RANG);

                    // Check if AngobSisch was found
                    if (AngobSisch == null)
                    {
                        // Create new
                        AngobSisch = ANGOBSICHAssembler.Create(mitantragsteller);
                    }
                    else
                    {
                        mitantragsteller.SysId = AngobSisch.SYSID;
                        // Update
                        AngobSisch = ANGOBSICHAssembler.Update(mitantragsteller);
                    }

                    // Create dto
                    ModifiedMitAntragStellerDto = ANGOBSICHAssembler.ConvertToDto(AngobSisch);
                }
            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SaveMitantragstellerFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.SaveMitantragstellerFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.SaveMitantragstellerFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }

            return ModifiedMitAntragStellerDto;
        }

        /// <summary>
        /// load MA of sichtyp rang 10,80,140 (MA, Bürge, Vertretungsberechtigte)
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        public MitantragstellerDto[] DeliverMitantragsteller(long sysAngebot)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            // Create the endorsers list
            List<MitantragstellerDto> Endorsers = new List<MitantragstellerDto>();

            try
            {
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAngebot))
                        throw new Exception("No Permission to ANGEBOT");

                    Endorsers = getMitantragsteller(sysAngebot);

                }


            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitantragstellerFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitantragstellerFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitantragstellerFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }

            return Endorsers.ToArray();
        }
        #endregion

        private static List<MitantragstellerDto>  getMitantragsteller(long sysAngebot)
        {
            using (DdOlExtended Context = new DdOlExtended())
            {
                
                String query = "select sichtyp.rang SICHTYPRANG, angobsich.option1, angobsich.sysid, angobsich.bezeichnung, angebot.sysid sysvt, angobsich.syssichtyp, angobsich.beginn, angobsich.ende, angobsich.sysit, angobsich.aktivflag aktivz, angobsich.rang from angobsich, sichtyp,angebot where  angobsich.sysvt=angebot.sysid and angobsich.syssichtyp=sichtyp.syssichtyp and sichtyp.rang in (10,80,140) and angebot.sysid=:sysid";


                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysAngebot });

                return Context.ExecuteStoreQuery<MitantragstellerDto>(query, parameters.ToArray()).ToList();

            }
        }

        #region Calculations


        /// <summary>
        /// Delivers MitFin for a certain Product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="sysprkgroup"></param>
        /// <param name="syskdtyp"></param>
        /// <param name="sysobart"></param>
        /// <param name="lz"></param>
        /// <param name="ll"></param>
        /// <returns></returns>
        public MitfinanzierteBestandteileDto[] DeliverMitfinanzierteBestandteileProduct(long sysprproduct, long sysobtyp, long sysprkgroup, long syskdtyp, long sysobart, long lz, long ll)
        {
            return DeliverAdditionalServicesForProduct(sysprproduct, sysobtyp, sysprkgroup, syskdtyp, sysobart, lz, ll, true);
        }

        /// <summary>
        /// Delivers Services for a certain Product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="sysprkgroup"></param>
        /// <param name="syskdtyp"></param>
        /// <param name="sysobart"></param>
        /// <param name="lz"></param>
        /// <param name="ll"></param>
        /// <param name="mitfinanziert"></param>
        /// <returns></returns>
        public MitfinanzierteBestandteileDto[] DeliverAdditionalServicesForProduct(long sysprproduct, long sysobtyp, long sysprkgroup, long syskdtyp, long sysobart, long lz, long ll, bool mitfinanziert)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Get the result
                MitfinanzierteBestandteileDto[] Result = FsPreisHelper.GetMitfinanzierteBestandteileProduct(sysprproduct, sysobtyp, sysprkgroup, syskdtyp, sysobart, lz, ll, mitfinanziert);



                // Return the prices
                return Result;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitfinanzierteBestandteileFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitfinanzierteBestandteileFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitfinanzierteBestandteileFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Delivers and calculates all values for the price-gui
        /// </summary>
        /// <param name="bmwPurchasePriceDto"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.PurchasePriceDto DeliverPurchasePrice(Cic.OpenLease.ServiceAccess.DdOl.PurchasePriceDto bmwPurchasePriceDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                VehicleDao vd = new VehicleDao();
                return vd.deliverPurchasePrice(bmwPurchasePriceDto, ServiceValidator.SysPEROLE);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverPurchasePriceFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverPurchasePriceFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverPurchasePriceFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// HCBE Rate Calculation
        /// </summary>
        /// <param name="calculationDto"></param>
        /// <param name="sysPerole"></param>
        /// <param name="sysBrand"></param>
        /// <param name="sysPuser"></param>
        /// <returns></returns>
        public static Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calcRate(Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto,long sysPerole, long sysBrand, long sysPuser)
        {
            using (DdOlExtended Context = new DdOlExtended())
            {
                KALKTYP KalkTyp = PRPRODUCTHelper.DeliverKalkTyp(Context, calculationDto.SysPrProduct);
                GebuehrDao gebuehrDao = new GebuehrDao(Context);
                RVSuggest rvSuggest = new RVSuggest(Context);

                // Get VP PeRole
                long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(Context, sysPerole, PeroleHelper.CnstVPRoleTypeNumber);

                // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                PRHGROUP prHGroup = PeroleHelper.DeliverVPPrHGroupList(Context, sysBrand, sysVpPeRole).FirstOrDefault<PRHGROUP>();
                calculationDto.SysPrkGroup = 0;//not in use 
                calculationDto.SysPrhGroup = prHGroup.SYSPRHGROUP;

                calculationDto.isIM = false;
                if(sysPuser>0)
                    calculationDto.isIM = Cic.OpenLease.Common.MembershipProvider.isInternalMitarbeiter(sysPuser);
               
                VSTYPDao vsTypdao = new VSTYPDao(Context);
                PRParamDao prparamDao = new PRParamDao(Context);
                LsAddDao lsAddDao = new LsAddDao(Context);
                
                CalculationDao calcDao = new CalculationDao(Context);
                KORREKTURDao kDao = new KORREKTURDao(Context);
                QUOTEDao qDao = new QUOTEDao();
                calculationDto.Message = "";
                calculationDto.MessageCode = CalculationDto.MessageCodes.NoError;
                if (calculationDto.Verzinsungsart == 0)
                    calculationDto.Verzinsungsart = 1;
                Cic.OpenLease.ServiceAccess.DdOl.CalculationDto rval = KalkulationHelper.Calculate(Context, sysBrand, sysPerole, calculationDto, calcDao, gebuehrDao, rvSuggest, vsTypdao, prparamDao, lsAddDao,  kDao, qDao);
                calculationDto.ZinsEff = rval.ZinsEff;
                String oMessage = rval.Message;
                Cic.OpenLease.ServiceAccess.DdOl.CalculationDto.MessageCodes ocode = rval.MessageCode;
                if (calculationDto.CalculationSource != CalculationDto.CalculationSources.Rate)
                {
                    rval = KalkulationHelper.Calculate(Context, sysBrand, sysPerole, calculationDto, calcDao, gebuehrDao, rvSuggest, vsTypdao, prparamDao, lsAddDao,  kDao, qDao);
                    if (rval.MessageCode == CalculationDto.MessageCodes.NoError)
                    {
                        rval.MessageCode = ocode;
                        rval.Message = oMessage;
                    }
                }

                return rval;
            }
        }

        /// <summary>
        /// Main rate calculation routine
        /// </summary>
        /// <param name="calculationDto"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.CalculationDto Calculate(Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto)
        {


            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                return calcRate(calculationDto, ServiceValidator.SysPEROLE, ServiceValidator.SysBRAND, ServiceValidator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);
            }
            catch (Exception exception)
            {
                _Log.Error("Calculate failed", exception);

                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.BmwCalculateFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.BmwCalculateFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.BmwCalculateFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }



        public Cic.OpenLease.ServiceAccess.DdOl.MehrMinderKmDto DeliverMehrMinderKm(decimal listenPreis, decimal sonderausstattung, decimal paketeBrutto, long sysprproduct)
        {

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                using (DdOlExtended Context = new DdOlExtended())
                {
                    Tires tires = new Tires(Context);
                    return tires.deliverMehrMinderKm(listenPreis, sonderausstattung, paketeBrutto, sysprproduct);
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMehrMinderKmFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMehrMinderKmFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMehrMinderKmFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public Cic.OpenLease.ServiceAccess.DdOl.MehrMinderKmDto DeliverMehrMinderKmVorvertrag(long sysvorvt)
        {

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                using (DdOlExtended Context = new DdOlExtended())
                {
                    Tires tires = new Tires(Context);
                    return tires.deliverMehrMinderKmVorvertrag(sysvorvt);
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMehrMinderKmFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMehrMinderKmFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMehrMinderKmFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Gets the monthly fuel fee. Can throw an ApplicationException with one of the following inner exceptions:
        /// <list type="bullet">
        /// <item><description>Argument lz is invalid.</description><br /></item>
        /// <item><description>Argument ll is invalid.</description><br /></item>
        /// <item><description>Argument consTot is invalid.</description><br /></item>
        /// <item><description>Fuel type is undefined.</description><br /></item>
        /// <item><description>Could not get the fuel price.</description><br /></item>
        /// </list>
        /// More details may be available in the further inner exceptions.
        /// </summary>
        /// <param name="lz">Contract length</param>
        /// <param name="ll"></param>
        /// <param name="consTot">Average fuel consumption</param>
        /// <param name="fuelType">Fuel type</param>
        /// <param name="sysfstyp">brand</param>
        /// <param name="nachlass">nachlass</param>
        /// <returns>Monthly fee</returns>
        public PetrolPriceDto CalculatePetrol(int lz, long ll, double consTot, FuelTypeConstants fuelType, long sysfstyp, decimal nachlass)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Check if fuel consumption value is valid
                if (consTot < 0)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument average fuel consumption is not valid.");
                }

                // Check if lz value is valid
                if (lz <= 0)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument lz is not valid.");
                }

                // Check if ll value is valid
                if (ll <= 0)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument ll is not valid.");
                }

                // Check if ll value is valid
                if (sysfstyp <= 0)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument sysfstyp is not valid.");
                }

                // Check if fuelType value is valid
                if (fuelType == FuelTypeConstants.Undefined)
                {
                    // Throw an exception
                    throw new ArgumentException("Fuel type is undefined.");
                }

                // Get the latest fuel price
                decimal FuelPrice = FsPreisHelper.GetFuelPrice(fuelType);

                // Check if the returned fuel price is valid
                if (FuelPrice <= 0)
                {
                    throw new ApplicationException("Fuel price is not valid");
                }

                // Create the result
                PetrolPriceDto Result = new PetrolPriceDto();

                // Get the tax rate
                decimal TaxRate = LsAddHelper.GetTaxRate(null);

                // Calculate the overall price
                Result.NetPrice = (decimal)consTot / 100m * (decimal)ll / (decimal)lz * FuelPrice;

                // Get the gross price
                Result.Price_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(Result.NetPrice, TaxRate));
                Result.GrossPrice = Result.Price_Default - nachlass;
                Result.NetPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(Result.GrossPrice, TaxRate));

                // Get the tax rate
                Result.Tax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Result.GrossPrice, TaxRate);

                // Get the unit prices
                Result.NetUnitPrice = FuelPrice;
                Result.GrossUnitPrice = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(FuelPrice, TaxRate);
                Result.UnitTax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Result.GrossUnitPrice, TaxRate);


                // Round the prices
                Result.NetPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.NetPrice);
                Result.GrossPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.GrossPrice);
                Result.Tax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Tax);
                Result.NetUnitPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.NetUnitPrice);
                Result.GrossUnitPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.GrossUnitPrice);
                Result.UnitTax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.UnitTax);
                Result.Price_Subvention = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Price_Subvention);
                Result.Price_SubventionNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Price_SubventionNetto);
                Result.Price_SubventionUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Price_SubventionUst);
                Result.Price_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Price_Default);

                Result.Price_Subvention = 0;
                Result.Price_SubventionNetto = 0;
                Result.Price_SubventionUst = 0;


                // Return the price
                return Result;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculatePetrolFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculatePetrolFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.CalculatePetrolFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }


        public PetrolLieferantDto[] DeliverPetrolLieferanten(FuelTypeConstants fuelType)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Get the result
                PetrolLieferantDto[] Result = FsPreisHelper.GetFuelLieferanten(fuelType);



                // Return the prices
                return Result;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitfinanzierteBestandteileFailed, exception);

                // Log the exception
                _Log.Warn(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitfinanzierteBestandteileFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitfinanzierteBestandteileFailed.ToString(), exception);

                return new PetrolLieferantDto[0];
            }
        }



        /// <summary>
        /// Gets the monthly car unregistration fee. Can throw an ApplicationException with one of the following inner exceptions:
        /// <list type="bullet">
        /// <item><description>Argument lz is invalid.</description><br /></item>
        /// <item><description>Could not get the unregistration price.</description><br /></item>
        /// </list>
        /// More details may be available in the further inner exceptions.
        /// </summary>
        /// <param name="lz">Contract length</param>
        /// <param name="kennzeichenInkl">Include number plates</param>
        /// <param name="sysOBTYP">SysObTyp</param>
        /// <param name="nachlass">nachlass</param>
        /// <returns>Monthly fee</returns>
        public AnAbmeldePriceDto CalculateAnAbmelde(int lz, bool kennzeichenInkl, long sysOBTYP, decimal nachlass)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Check lz parameter
                if (lz <= 0)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument lz is invalid.");
                }

                // Declare the price
                decimal Price;

                //Declare Schwacke
                string Schwacke;

                using (DdOlExtended Context = new DdOlExtended())
                {
                    //Get schwacke
                    Schwacke = RestOfTheHelpers.GetSchwacke(Context, sysOBTYP);
                }


                // Check if number plates are included
                if (kennzeichenInkl)
                {
                    // Get the unregistration price
                    Price = FsPreisHelper.GetUnregistrationPrice(NumberPlateConstants.WithNumberPlate, Schwacke);
                }
                else
                {
                    // Get the unregistration price
                    Price = FsPreisHelper.GetUnregistrationPrice(NumberPlateConstants.WithoutNumberPlate, Schwacke);
                }

                // Create the result
                AnAbmeldePriceDto Result = new AnAbmeldePriceDto();

                // Get the tax rate
                decimal TaxRate = LsAddHelper.GetTaxRate(null);

                // Do the calculation
                Result.NetPrice = Price / (decimal)lz;

                // Get the gross price
                Result.Price_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(Result.NetPrice, TaxRate));
                Result.GrossPrice = Result.Price_Default - nachlass;
                Result.NetPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(Result.GrossPrice, TaxRate));

                // Get the tax
                Result.Tax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Result.GrossPrice, TaxRate);

                // Get the unit prices
                Result.NetUnitPrice = Price;
                Result.GrossUnitPrice = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(Price, TaxRate);
                Result.UnitTax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Result.GrossUnitPrice, TaxRate);

                // Round the prices
                Result.NetPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.NetPrice);
                Result.GrossPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.GrossPrice);
                Result.Tax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Tax);
                Result.NetUnitPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.NetUnitPrice);
                Result.GrossUnitPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.GrossUnitPrice);
                Result.UnitTax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.UnitTax);
                Result.Price_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Price_Default);
                Result.Price_Subvention = 0;
                Result.Price_SubventionNetto = 0;
                Result.Price_SubventionUst = 0;
                // Return the price
                return Result;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateAnAbmeldeFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateAnAbmeldeFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateAnAbmeldeFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public decimal[] DeliverErsatzfahrzeugFsPrices()
        {
            try
            {
                return FsPreisHelper.GetReplacementCarPrices();
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverErsatzfahrzeugFsPricesFailed, exception);

                // Log the exception
                _Log.Warn(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverErsatzfahrzeugFsPricesFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverErsatzfahrzeugFsPricesFailed.ToString()+" "+exception.Message);

                return new decimal[0];
            }
        }

        public decimal[] DeliverManagementFeeFsPrices()
        {
            try
            {
                return FsPreisHelper.GetManagementFees();
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverManagementFeeFsPricesFailed, exception);

                // Log the exception
                _Log.Warn(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverManagementFeeFsPricesFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverManagementFeeFsPricesFailed.ToString() + " " + exception.Message);

                return new decimal[0];
            }
        }



        /// <summary>
        /// Gets the monthly replacement car fee. Can throw an ApplicationException with one of the following inner exceptions:
        /// <list type="bullet">
        /// <item><description>Argument lz is invalid.</description><br /></item>
        /// <item><description>Argument countDays is invalid.</description><br /></item>
        /// <item><description>Could not get the replacement car price.</description><br /></item>
        /// </list>
        /// More details may be available in the further inner exceptions.
        /// </summary>
        /// <param name="lz">Contract length</param>
        /// <param name="countDays">Maximum number of days the car will be supplied for</param>
        /// <param name="price">Price</param>
        /// <param name="nachlass">nachlass</param>
        /// <returns>Monthly fee</returns>
        public ErsatzfahrzeugPriceDto CalculateErsatzfahrzeug(int lz, int countDays, decimal price, decimal nachlass)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Check lz parameter
                if (lz <= 0)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument lz is invalid.");
                }

                // Check countDays parameter
                if (countDays <= 0)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument countDays is invalid.");
                }

                // Create the result
                ErsatzfahrzeugPriceDto Result = new ErsatzfahrzeugPriceDto();

                // Get the tax rate
                decimal TaxRate = LsAddHelper.GetTaxRate(null);

                // Calculate the price
                Result.NetPrice = price * (decimal)countDays / (decimal)lz;

                // Get the gross price
                Result.Price_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(Result.NetPrice, TaxRate));
                Result.GrossPrice = Result.Price_Default - nachlass;
                Result.NetPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(Result.GrossPrice, TaxRate));

                // Get the tax rate
                Result.Tax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Result.GrossPrice, TaxRate);

                // Get the unit prices
                Result.NetUnitPrice = price;
                Result.GrossUnitPrice = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(Result.NetUnitPrice, TaxRate);
                Result.UnitTax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Result.GrossUnitPrice, TaxRate);

                // Round the prices
                Result.NetPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.NetPrice);
                Result.GrossPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.GrossPrice);
                Result.Tax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Tax);
                Result.NetUnitPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.NetUnitPrice);
                Result.GrossUnitPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.GrossUnitPrice);
                Result.UnitTax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.UnitTax);
                Result.Price_Subvention = 0;
                Result.Price_SubventionNetto = 0;
                Result.Price_SubventionUst = 0;



                // Return the price
                return Result;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateErsatzfahrzeugFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateErsatzfahrzeugFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateErsatzfahrzeugFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Gets the monthly management fee. Can throw an ApplicationException with one of the following inner exceptions:
        /// <list type="bullet">
        /// <item><description>Argument lz is invalid.</description><br /></item>
        /// <item><description>Could not get the management fee.</description><br /></item>
        /// </list>
        /// More details may be available in the further inner exceptions.
        /// </summary>
        /// <param name="lz">Contract length</param>
        /// <param name="price">Price</param>
        /// <param name="nachlass">nachlass</param>
        /// <param name="sysprproduct">sysprproduct</param>
        /// <param name="sysobtyp">sysobtyp</param>
        /// <param name="SPECIALCALCSTATUS">SPECIALCALCSTATUS</param>
        /// <returns>Monthly fee</returns>
        public ManagementFeeDto CalculateManagementFee(int lz, decimal price, decimal nachlass, long sysprproduct, long sysobtyp, long? SPECIALCALCSTATUS)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Check lz parameter
                if (lz <= 0)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument lz is invalid.");
                }

                // Create the result
                ManagementFeeDto Result = new ManagementFeeDto();

                // Get the tax rate
                decimal TaxRate = LsAddHelper.GetTaxRate(null);

                // Calculate the net price
                Result.NetPrice = price / (decimal)lz;

                // Get the gross price
                Result.Price_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(Result.NetPrice, TaxRate));
                Result.GrossPrice = Result.Price_Default - nachlass;
                Result.NetPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(Result.GrossPrice, TaxRate));


                // Calculate the tax rate
                Result.Tax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Result.GrossPrice, TaxRate);

                // Get the unit prices
                Result.NetUnitPrice = price;
                Result.GrossUnitPrice = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(Result.NetUnitPrice, TaxRate);
                Result.UnitTax = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Result.GrossUnitPrice, TaxRate);

                // Get the provision
                ProvisionDto Provision = new ProvisionDto();
                Provision.sysBrand = ServiceValidator.SysBRAND;
                Provision.sysPerole = ServiceValidator.SysPEROLE;
                Provision.sysobtyp = sysobtyp;
                Provision.sysprproduct = sysprproduct;
                Provision.bearbeitungsgebuehr = Result.NetUnitPrice;
                Provision.noProvision = Cic.OpenLease.Common.MembershipProvider.isInternalMitarbeiter(ServiceValidator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);
                if (SPECIALCALCSTATUS.HasValue && SPECIALCALCSTATUS.Value > 0)
                    Provision.noProvision = false;
                if (lz < QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_PROV_MIN_LZ))
                    Provision.noProvision = true;

                DeliverBearbeitungsgebuehrProvision(Provision);

                Result.Bearbaitungsgebuhr = new BearbeitungsgebuhrDto();
                Result.Bearbaitungsgebuhr.BasicPrice = Result.NetPrice;
                Result.Bearbaitungsgebuhr.ProvisionValue = Provision.provision;
                Result.Bearbaitungsgebuhr.ProvisionPercentage = 0;
                if (Result.Bearbaitungsgebuhr.BasicPrice > 0)
                    Result.Bearbaitungsgebuhr.ProvisionPercentage = Result.Bearbaitungsgebuhr.ProvisionValue * 100 / Result.Bearbaitungsgebuhr.BasicPrice;

                // Round the prices
                Result.NetPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.NetPrice);
                Result.GrossPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.GrossPrice);
                Result.Tax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Tax);
                Result.NetUnitPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.NetUnitPrice);
                Result.GrossUnitPrice = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.GrossUnitPrice);
                Result.UnitTax = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.UnitTax);
                Result.Price_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Result.Price_Default);
                Result.Price_Subvention = 0;
                Result.Price_SubventionNetto = 0;
                Result.Price_SubventionUst = 0;

                // Return the result
                return Result;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateManagementFeeFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateManagementFeeFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateManagementFeeFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        #endregion

        #region Versicherung

        /// <summary>
        /// Service method to deliver all kinds of insurances (for BMW without customer group)
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="SysKdTyp"></param>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public List<VSARTDto> DeliverVSART(long sysObTyp, long sysObArt, long SysKdTyp, long sysPrProduct)
        {
            return DeliverVSART(0, sysObTyp, sysObArt, SysKdTyp, sysPrProduct);
        }

        /// <summary>
        /// Service method to deliver all kinds of insurances 
        /// </summary>
        /// <param name="SysPRKGroup">Customer group</param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="SysKdTyp"></param>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public List<VSARTDto> DeliverVSART(long SysPRKGroup, long sysObTyp, long sysObArt, long SysKdTyp, long sysPrProduct)
        {
            try
            {
                ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                validator.ValidateView();
                using (DdOlExtended context = new DdOlExtended())
                {
                    VSARTDao dao = new VSARTDao(context);

                    return dao.DeliverAvailableVsArt(SysPRKGroup, sysObTyp, sysObArt, SysKdTyp, sysPrProduct);
                }
            }
            catch (Exception exception)
            {
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSARTFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSARTFailed.ToString(), exception);
                // Throw an exception
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSARTFailed, exception);
            }

        }

        /// <summary>
        /// Service method to deliver all insurance companies (for BMW without customer group)
        /// </summary>
        /// <param name="sysVsArt"></param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="SysKdTyp"></param>
        /// <returns></returns>
        public List<PERSONDto> DeliverVSPERSON(long sysVsArt, long sysObTyp, long sysObArt, long SysKdTyp)
        {
            return DeliverVSPERSON(0, sysVsArt, sysObTyp, sysObArt, SysKdTyp);
        }

        /// <summary>
        /// Service method to deliver all insurance companies
        /// </summary>
        /// <param name="SysPRKGroup">Customer group</param>
        /// <param name="sysVsArt"></param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="SysKdTyp"></param>
        /// <returns></returns>
        public List<PERSONDto> DeliverVSPERSON(long SysPRKGroup, long sysVsArt, long sysObTyp, long sysObArt, long SysKdTyp)
        {

            try
            {
                ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                validator.ValidateView();
                using (DdOlExtended context = new DdOlExtended())
                {
                    VSPERSONDao dao = new VSPERSONDao(context);

                    return dao.DeliverAvailableVs(SysPRKGroup, sysVsArt, sysObTyp, sysObArt, SysKdTyp);
                }
            }
            catch (Exception exception)
            {
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSPERSONFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSPERSONFailed.ToString(), exception);
                // Throw an exception
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSPERSONFailed, exception);
            }

        }

        /// <summary>
        /// Delivers the complete insurance-tree for the given insurance filter parameters
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="sysKdTyp"></param>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public VSARTDto[] DeliverInsuranceTree(long sysObTyp, long sysObArt, long sysKdTyp, long sysPrProduct)
        {
            try
            {
                ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                validator.ValidateView();
                using (DdOlExtended context = new DdOlExtended())
                {



                    VSTYPDao vstyp = new VSTYPDao(context);
                    //NEEDED = Aktiv / nicht aktiv
                    //DISABLED =  readonly /Änderbar/"Nicht schaltbar"
                    //DISABLED==1 && NEEDED=0 -> entfernen

                    //alle versicherungen für das produkt:
                    List<PRVSDto> insurances = vstyp.getAllInsurances(sysPrProduct);
                    //Verfügbarkeiten im VSTYP
                    List<long> availvs = vstyp.getAvailabilites(0, sysObTyp, sysObArt, sysKdTyp);
                    //Filtere Versicherungen mit Verfügbarkeiten
                    var q1 = from c in insurances
                             where availvs.Contains(c.SYSVSTYP)
                             select c;
                    insurances = q1.ToList();


                    List<VSARTDto> vsarten = vstyp.getVSART(insurances);

                    foreach (VSARTDto va in vsarten)
                    {
                        List<PERSONDto> personen = vstyp.getVSPERSON(va.SYSVSART, insurances);
                        va.VSLIST = personen.ToArray();
                        bool hasDefault = false;
                        bool hasvalidvart = false;
                        foreach (PERSONDto person in va.VSLIST)
                        {
                            List<VSTYPDto> vstypen = vstyp.getVSTYPList(va.SYSVSART, (long)person.SYSPERSON, insurances);
                            person.VSTYPLIST = vstypen.ToArray();
                            bool hasvalidvs = false;
                            foreach (VSTYPDto vst in vstypen)
                            {

                                if (vst.NEEDED.HasValue && vst.NEEDED.Value > 0)
                                    person.NEEDED = 1;
                                VSTYP vstypInfo = vstyp.getVsTyp(vst.SYSVSTYP.Value);
                                if (vstypInfo.VALIDUNTIL != null && vstypInfo.VALIDUNTIL.HasValue && (vstypInfo.VALIDUNTIL.Value.CompareTo(DateTime.Now) < 0 && vstypInfo.VALIDUNTIL.Value>VSTYPDao.nullDate))
                                {
                                    vst.DISABLED = 1;
                                    person.NEEDED = 0;
                                    person.DISABLED = 1;
                                }
                                else if (vst.FLAGDEFAULT.HasValue && vst.FLAGDEFAULT.Value)
                                    hasDefault = true;
                                if ((person.NEEDED> 0) || (!vst.DISABLED.HasValue || vst.DISABLED.Value < 1))
                                    hasvalidvs = true;
                            }
                            if (!hasvalidvs)
                            {
                                person.DISABLED = 1;
                                person.NEEDED = 0;
                            }
                            else hasvalidvart = true;
                        }


                        if (!hasDefault)
                        {
                            if (va.VSLIST != null && va.VSLIST.Length > 0 && va.VSLIST[0].VSTYPLIST != null && va.VSLIST[0].VSTYPLIST.Length > 0)
                            {
                                foreach (VSTYPDto vst in va.VSLIST[0].VSTYPLIST)
                                {
                                    if ((vst.DISABLED.HasValue && vst.DISABLED.Value < 1) || !vst.DISABLED.HasValue)
                                    {
                                        vst.FLAGDEFAULT = true;
                                        break;
                                    }

                                }
                            }
                        }
                        if (!hasvalidvart)
                        {
                            va.DISABLED = 1;
                            va.NEEDED = 0;

                        }
                    }


                    return vsarten.ToArray();
                }
            }
            catch (Exception exception)
            {
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSDataFailed.ToString(), exception);
                // Throw an exception
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSDataFailed, exception);
            }

        }

        /// <summary>
        /// service Method to deliver all insurances for BMW (no customer group)
        /// </summary>
        /// <param name="sysVsArt"></param>
        /// <param name="sysVs"></param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="sysKdTyp"></param>
        /// <returns></returns>
        public List<VSTYPDto> DeliverVSTYP(long sysVsArt, long sysVs, long sysObTyp, long sysObArt, long sysKdTyp)
        {
            return DeliverVSTYP(0, sysVsArt, sysVs, sysObTyp, sysObArt, sysKdTyp);
        }

        /// <summary>
        /// service Method to deliver all insurances
        /// </summary>
        /// <param name="SysPRKGroup">Customer group</param>
        /// <param name="sysVsArt"></param>
        /// <param name="sysVs"></param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="sysKdTyp"></param>
        /// <returns></returns>
        public List<VSTYPDto> DeliverVSTYP(long SysPRKGroup, long sysVsArt, long sysVs, long sysObTyp, long sysObArt, long sysKdTyp)
        {
            try
            {
                ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                validator.ValidateView();
                using (DdOlExtended context = new DdOlExtended())
                {
                    VSTYPDao dao = new VSTYPDao(context);

                    return dao.DeliverVsTyp(SysPRKGroup, sysVsArt, sysVs, sysObTyp, sysObArt, sysKdTyp);
                }
            }
            catch (Exception exception)
            {
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSTYPFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSTYPFailed.ToString(), exception);
                // Throw an exception
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSTYPFailed, exception);
            }
        }

        /// <summary>
        /// service method to calculate multiple insurances fee for all kind of service products
        /// </summary>
        /// <param name="insuranceParams"></param>
        /// <returns></returns>
        public List<InsuranceResultDto> DeliverVS(List<InsuranceParameterDto> insuranceParams)
        {
            try
            {
                ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                validator.ValidateView();

                List<InsuranceResultDto> result = new List<InsuranceResultDto>();

                using (DdOlExtended context = new DdOlExtended())
                {

                    foreach (InsuranceParameterDto param in insuranceParams)
                    {

                        if (param.SysVSTYP < 1)
                        {
                            result.Add(new InsuranceResultDto());
                            continue;
                        }
                        param.calcProvision = !Cic.OpenLease.Common.MembershipProvider.isInternalMitarbeiter(validator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);
                        if (param.specialcalcStatus > 0)
                            param.calcProvision = true;
                        if (param.Laufzeit < QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_PROV_MIN_LZ))
                            param.calcProvision = false;
                        try
                        {
                            VSTYPDao dao = new VSTYPDao(context);
                            InsuranceResultDto res = dao.DeliverVSData(validator.SysPEROLE, validator.SysBRAND, param);
                            result.Add(res);
                        }
                        catch (Exception ex)
                        {
                            _Log.Error("Not returning result for Insurance " + _Log.dumpObject(param) + ": " + ex.Message, ex);
                        }
                    }
                }
                return result;
            }
            catch (Exception exception)
            {
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSDataFailed.ToString(), exception);
                // Throw an exception
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSDataFailed, exception);
            }
        }


        #endregion

        #region Provisionen
        private static CacheDictionary<String, PROVTARIFDto[]> abProvCache = CacheFactory<String, PROVTARIFDto[]>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        /// <summary>
        /// Delivers the abschluss provision tarif list
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <param name="sysObtyp"></param>
        /// <returns></returns>
        public PROVTARIFDto[] DeliverAbschlussProvisionsTarife(long sysPrProduct, long sysObtyp)
        {
            ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            validator.ValidateView();
            return DeliverAbschlussProvisionsTarifeForRole(sysPrProduct, sysObtyp, validator.SysPEROLE);
        }

        /// <summary>
        /// Delivers the abschluss provision tarif list
        /// (Dropdown in finance-gui for selection of abschluss provision tarif)
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <param name="sysObtyp"></param>
        /// <param name="sysPerole"></param>
        /// <returns></returns>
        public PROVTARIFDto[] DeliverAbschlussProvisionsTarifeForRole(long sysPrProduct, long sysObtyp, long sysPerole)
        {

            try
            {

                ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                validator.ValidateView();


                using (DdOlExtended context = new DdOlExtended())
                {

                    // Get VP PeRole
                    long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(context, sysPerole,PeroleHelper.CnstVPRoleTypeNumber);

                    // Get PrHGroup - in BMW there can be only one - thats why FirstOrDefault
                    PRHGROUP PrHGroup = PeroleHelper.DeliverVPPrHGroupList(context, validator.SysBRAND, sysVpPeRole).FirstOrDefault<PRHGROUP>();

                    String key = sysPrProduct + "_" + sysObtyp + "_" + sysPerole + "_" + validator.SysBRAND + "_" + PrHGroup.SYSPRHGROUP;
                    if (!abProvCache.ContainsKey(key))
                    {
                        ProvisionDto param = new ProvisionDto();
                        param.sysPerole = sysPerole;
                        param.sysBrand = validator.SysBRAND;
                        param.sysPrhgroup = PrHGroup.SYSPRHGROUP;
                        param.rank = (int)ProvisionTypeConstants.Abschluss;
                        param.sysobtyp = sysObtyp;
                        param.sysprproduct = sysPrProduct;

                        PROVDao dao = new PROVDao(context);

                        List<PROVTARIF> tarife = dao.deliverAdjustedTarife(param);
                        GenericAssembler<PROVTARIFDto, PROVTARIF> assembler = new GenericAssembler<PROVTARIFDto, PROVTARIF>(new PROVTARIFMapper());

                        abProvCache[key] = assembler.ConvertToDto(tarife).ToArray();
                    }
                    return abProvCache[key];
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTarifeFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTarifeFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Delivers all provision rates for the given rank
        /// </summary>
        /// <param name="provsteprank"></param>
        /// <returns></returns>
        public PROVTARIFDto[] DeliverTarife(int provsteprank)
        {
            ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            validator.ValidateView();
            return DeliverTarifeForRole(provsteprank, validator.SysPEROLE);
        }

        /// <summary>
        /// Delivers all provision rates for the given rank and user perole
        /// </summary>
        /// <param name="provsteprank"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public PROVTARIFDto[] DeliverTarifeForRole(int provsteprank, long sysperole)
        {
            try
            {

                ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                validator.ValidateView();
                using (DdOlExtended context = new DdOlExtended())
                {
                    // Get VP PeRole
                    long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(context, sysperole, PeroleHelper.CnstVPRoleTypeNumber);

                    // Get PrHGroup - in BMW there can be only one - thats why FirstOrDefault
                    PRHGROUP PrHGroup = PeroleHelper.DeliverVPPrHGroupList(context, validator.SysBRAND, sysVpPeRole).FirstOrDefault<PRHGROUP>();
                    String key = provsteprank + "_" + sysperole + "_" + validator.SysBRAND + "_" + PrHGroup.SYSPRHGROUP;

                    if (!tarifeCache.ContainsKey(key))
                    {

                        PROVDao dao = new PROVDao(context);
                        List<PROVTARIF> tarife = dao.DeliverProvTarife(sysperole, validator.SysBRAND, PrHGroup.SYSPRHGROUP, provsteprank);

                        GenericAssembler<PROVTARIFDto, PROVTARIF> assembler = new GenericAssembler<PROVTARIFDto, PROVTARIF>(new PROVTARIFMapper());

                        tarifeCache[key] = assembler.ConvertToDto(tarife).ToArray();
                    }
                    return tarifeCache[key];
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTarifeFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverVSDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTarifeFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Calls the Provision Calculator for Abschlussprovision, independent of the param-rank
        /// </summary>
        /// <param name="param">Parameters for the Calculation</param>
        /// <returns>A new ProvisionDto filled with data provided from the calculator</returns>
        public ProvisionDto DeliverAbschlussProvision(ProvisionDto param)
        {
            try
            {
                param.rank = (int)ProvisionTypeConstants.Abschluss;
                return DeliverProvision(param);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAbschlussProvisionFailed, exception);


                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Calls the Provision Calculator for Haftpflichtprovision, independent of the param-rank
        /// </summary>
        /// <param name="param">Parameters for the Calculation</param>
        /// <returns>A new ProvisionDto filled with data provided from the calculator</returns>
        public ProvisionDto DeliverHaftpflichtProvision(ProvisionDto param)
        {
            try
            {
                param.rank = (int)ProvisionTypeConstants.Haftpflicht;
                return DeliverProvision(param);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverHaftpflichtProvisionFailed, exception);


                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Calls the Provision Calculator for Bearbeitungsgebuehrprovision, independent of the param-rank
        /// </summary>
        /// <param name="param">Parameters for the Calculation</param>
        /// <returns>A new ProvisionDto filled with data provided from the calculator</returns>
        public ProvisionDto DeliverBearbeitungsgebuehrProvision(ProvisionDto param)
        {
            try
            {
                param.rank = (int)ProvisionTypeConstants.Bearbeitungsgebuehr;
                return DeliverProvision(param);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBearbeitungsgebuehrProvisionFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Calls the Provision Calculator for Kaskoprovision, independent of the param-rank
        /// </summary>
        /// <param name="param">Parameters for the Calculation</param>
        /// <returns>A new ProvisionDto filled with data provided from the calculator</returns>
        public ProvisionDto DeliverKaskoProvision(ProvisionDto param)
        {
            try
            {
                param.rank = (int)ProvisionTypeConstants.Kasko;
                return DeliverProvision(param);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverKaskoProvisionFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }


        /// <summary>
        /// Calls the Provision Calculator for Restschuldprovision, independent of the param-rank
        /// </summary>
        /// <param name="param">Parameters for the Calculation</param>
        /// <returns>A new ProvisionDto filled with data provided from the calculator</returns>
        public ProvisionDto DeliverRestschuldProvision(ProvisionDto param)
        {
            try
            {
                param.rank = (int)ProvisionTypeConstants.Restschuld;
                return DeliverProvision(param);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverRestschuldProvisionFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Calls the Provision Calculator for Wartungreparaturprovision, independent of the param-rank
        /// </summary>
        /// <param name="param">Parameters for the Calculation</param>
        /// <returns>A new ProvisionDto filled with data provided from the calculator</returns>
        public ProvisionDto DeliverWartungReparaturProvision(ProvisionDto param)
        {
            try
            {
                param.rank = (int)ProvisionTypeConstants.WartungReparatur;
                return DeliverProvision(param);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverWartungReparaturProvisionFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }


        /// <summary>
        /// Generic Service for calculation of the provision
        /// the type of provision is determined by param.rank
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ProvisionDto DeliverProvision(ProvisionDto param)
        {

            try
            {
                ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                validator.ValidateView();
                using (DdOlExtended context = new DdOlExtended())
                {
                    PROVDao dao = new PROVDao(context);
                    if (param.sysBrand == 0)
                        param.sysBrand = validator.SysBRAND;
                    if (param.sysPerole == 0)
                        param.sysPerole = validator.SysPEROLE;
                    //param.isIM = validator.MembershipUserValidationInfo.IsInternalMitarbeiter;

                    return dao.DeliverProvision(param);
                }
            }
            catch (Exception exception)
            {
                // Log the exception
                _Log.Error(exception.Message, exception);

                // Throw the exception
                throw exception;
            }
        }
        #endregion

        #region Eurotax
        /// <summary>
        /// Liefert Liste möglicher Fahrzeuge aus SA3-XML
        /// Aida-GUI öffnet Popup und lässt ein Fahrzeug wählen.
        /// SA3XML und gewählte Eurotaxnummer kommen dann über deliverAngebotDtoFromSa3FromSchwacke wieder herein
        /// </summary>
        /// <param name="sa3XmlData"></param>
        /// <returns></returns>
        public List<FzConfiguration> GetSa3FzConfiguration(string sa3XmlData)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {

                return Sa3Helper.GetSa3FzConfiguration(sa3XmlData);

            }
            catch (Exception exception)
            {

                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverFzDataFromSa3Failed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverFzDataFromSa3Failed + " " + exception.Message, exception);

                throw TopLevelException;

            }
        }

        public ANGEBOTDto getAngebotFromVertrag(long sysvt)
        {
            return null;
        }

        /// <summary>
        /// Creates Angebot-Object from a object-id (used by carconfigurator hek)
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public ANGEBOTDto DeliverAngebotDtoFromCarConfiguratorSysob(long sysob, ANGEBOTDto AngebotDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Get the offer
                OfferConfiguration OfferConfiguration = ConfigurationManagerHelper.GetObjectConfigurationSysob(sysob, ServiceValidator.SysPEROLE);

                // Create and return angebot
                ANGEBOTDto angebot= AngebotDtoHelper.CreateAngebotDto(OfferConfiguration,ServiceValidator.SysPEROLE,AngebotDto);
                return angebot;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        /// <summary>
        /// Creates Angebot-Object from a object-id (used by carconfigurator)
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public ANGEBOTDto DeliverAngebotDtoFromCarConfigurator(long sysobtyp,ANGEBOTDto AngebotDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Get the offer
                OfferConfiguration OfferConfiguration = ConfigurationManagerHelper.GetObjectConfiguration(sysobtyp, ServiceValidator.SysPEROLE);

                // Create and return angebot
                return AngebotDtoHelper.CreateAngebotDto(OfferConfiguration, ServiceValidator.SysPEROLE,AngebotDto);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Erzeugt aus SA3-BMW Input ein ANGEBOT als Ausgangsdatenstruktur für die Weboberfläche
        /// </summary>
        /// <param name="xmlData">BMW SA3-XML-Daten</param>
        /// <param name="schwacke"></param>
        /// <returns></returns>
        public ANGEBOTDto DeliverAngebotDtoFromSa3FromSchwacke(string xmlData, string schwacke,ANGEBOTDto AngebotDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            _Log.Debug("SA-3-Data: " + xmlData);

            try
            {
                // Get the offer
                OfferConfiguration OfferConfiguration = Sa3Helper.GetSa3OfferConfiguration(xmlData, schwacke);



                // Create and return angebot
                ANGEBOTDto rval = AngebotDtoHelper.CreateAngebotDto(OfferConfiguration, ServiceValidator.SysPEROLE,AngebotDto);

                //Nova2014
                //introduce a netto-netto LP that is saved and never altered
                //then in deliver purchase-price deliver always this original netto (at least for sa3)
                //set ANGOBGRUNDBRUTTO negativ and to pricenetto -> in deliverPurchasePrise then refetch all based on this netto
                rval.ANGOBGRUNDBRUTTO = OfferConfiguration.Vehicle.PriceNettoNetto * -1;
                rval.ANGOBGRUNDEXKLN = OfferConfiguration.Vehicle.PriceNettoNetto;
                return rval;
            }
            catch (Exception exception)
            {
                ANGEBOTDto rval = new ANGEBOTDto();
                rval.ERRORMESSAGE = exception.Message;
                rval.ERRORDETAIL = exception.StackTrace;
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromSa3Failed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromSa3Failed + " " + exception.Message, exception);
                _Log.Error("SA-3-Data: " + xmlData);
                // Throw the exception
                //throw TopLevelException;
                return rval;
            }
        }






        /// <summary>
        /// Used from AIDA Gui to determine technical Data, called once per vehicle
        /// vehicle may be selected through cc, manual et or sa3
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="BmwTechnicalDataDto"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.TechnicalDataDto DeliverTechnicalDataExtendedFromObTyp(long sysObTyp, long sysObArt, Cic.OpenLease.ServiceAccess.DdOl.TechnicalDataDto BmwTechnicalDataDto)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            try
            {
                VehicleDao vd = new VehicleDao();
                return vd.deliverTechnicalDataExtendedFromObTyp(sysObTyp, sysObArt, BmwTechnicalDataDto, ServiceValidator.SysPEROLE);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }


        /// <summary>
        /// returns true, if the IT has a currently submitted offer or is a sichtyp of another submitted offer
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public bool hasSubmittedOffer(long sysit)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {

                return ITDao.hasSubmittedOffer(sysit);

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.IsBrandDealer, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.IsBrandDealer + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.IsBrandDealer.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }

        }

        /// <summary>
        /// returns true, if the loginuser is a dealer for the given brand
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public bool isBrandDealer(String brand)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {


                // Get database context and search for description
                using (DdOlExtended context = new DdOlExtended())
                {

                    Cic.OpenLease.ServiceAccess.DdOl.BRANDDto[] userBrands = Cic.OpenLease.Common.MembershipProvider.listBrands(ServiceValidator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);

                    if (userBrands.Count() > 0)
                    {
                        foreach (BRANDDto b in userBrands)
                            if (b.NAME.ToLower().IndexOf(brand.ToLower()) > -1)
                                return true;
                    }
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.IsBrandDealer, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.IsBrandDealer + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.IsBrandDealer.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
            return false;
        }

        /// <summary>
        /// Liefert Hersteller/Fahrzeugtextinformationen
        /// used in AIDA-Gui:
        ///  * fill Kategorie-List (from getObjectArts)
        ///  * after carconfig or when from sa3
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <param name="isFromSa3"></param>
        /// <param name="checkMultifranchise"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.ObjectContextDto DeliverObjectContextFromObTyp(long sysObTyp, bool isFromSa3, bool checkMultifranchise, int contractext)
        {

            ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);


            try
            {
                VehicleDao vd = new VehicleDao();
                return vd.deliverObjectContextFromObTyp(sysObTyp, isFromSa3, checkMultifranchise, validator.SysBRAND, validator.MembershipUserValidationInfo, contractext);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverObjectContextFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverObjectContextFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverObjectContextFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Liefert Hersteller/Fahrzeugtextinformationen
        /// used in AIDA-Gui:
        ///  * after manual eurotax-number 
        /// </summary>
        /// <param name="eurotaxNr"></param>
        /// <param name="isFromSa3"></param>
        /// <param name="checkMultifranchise"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.ObjectContextDto DeliverObjectContext(string eurotaxNr, bool isFromSa3, bool checkMultifranchise, int contractext)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

            VehicleDao vd = new VehicleDao();
            return vd.deliverObjectContext(eurotaxNr, isFromSa3, checkMultifranchise, ServiceValidator.SysBRAND, ServiceValidator.MembershipUserValidationInfo, contractext);
        }


        public string DeliverEurotaxNr(string okaCode, string optionCode1, string optionCode2)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Execute the helper's method and return its value
                return EurotaxNumberHelper.GetEurotaxNumberFromOka(okaCode, optionCode1, optionCode2);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverEurotaxNrFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverEurotaxNrFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverEurotaxNrFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        #endregion

        #region Tires

        /// <summary>
        /// Returns all data for the Tire-GUI
        /// </summary>
        /// <param name="eurotaxNr"></param>
        /// <param name="reifencodeVorne"></param>
        /// <param name="reifencodeHinten"></param>
        /// <param name="reifencodeVorneSommer"></param>
        /// <param name="reifencodeHintenSommer"></param>
        /// <returns></returns>
        public TireInfoDto deliverTireGUIData(String eurotaxNr, String reifencodeVorne, String reifencodeHinten, String reifencodeVorneSommer, String reifencodeHintenSommer)
        {

            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                eurotaxNr = eurotaxNr.Trim();
                using (DdOlExtended Context = new DdOlExtended())
                {
                    Tires tires = new Tires(Context);
                    return tires.getTireData(eurotaxNr, reifencodeVorne, reifencodeHinten, reifencodeVorneSommer, reifencodeHintenSommer);
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTiresPricesFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTiresPricesFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTiresPricesFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }


        /// <summary>
        /// Calculates the current tire-configuration
        /// </summary>
        /// <param name="bmwTiresAndRimsCalculationDto"></param>
        /// <returns></returns>
        public TiresAndRimsCalculationDto CalculateTires(TiresAndRimsCalculationDto bmwTiresAndRimsCalculationDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                using (DdOlExtended Context = new DdOlExtended())
                {
                    Tires tires = new Tires(Context);
                    return tires.calculateTires(bmwTiresAndRimsCalculationDto);
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateTiresFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateTiresFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.CalculateTiresFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        #endregion

        #region WRRATE
        public WRRateDto DeliverWRRATE(long sysObTyp, long lz, decimal ll, int variabel, decimal rn, System.DateTime perDate, decimal nachlass, long sysprproduct, int? SPECIALCALCSTATUS)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                WRRateDto WRRateDto = null;
                WRRate WRRate;
                WRRateAssembler WRRateAssembler = new WRRateAssembler();
                decimal Provision = 0;
                bool serviceVariabel = variabel == 0 ? true : false;
                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Get the tax rate
                    decimal TaxRate = LsAddHelper.GetTaxRate(Context, null);
                    FSTYPDto fstyp = FsPreisHelper.getFsTyp(FsPreisHelper.CnstWartungFsArt);
                    if (variabel > 1 && fstyp != null)
                    {
                        if (fstyp.FIXVARDEFAULT == 2)
                            serviceVariabel = true;
                        else serviceVariabel = false;
                    }


                    //Create new WRRate
                    WRRate = new WRRate(Context);

                    try
                    {
                        //Calculate
                        WRRate.calculateWRRate(sysObTyp, lz, ll, serviceVariabel, rn, perDate, TaxRate);

                    }
                    catch
                    {
                        //Write 0 on exception
                        //WRRate.excessKMCharge = 0;
                        //WRRate.KMCharge = 0;
                        //WRRate.recessKMCharge = 0;
                        //WRRate.Rate = 0;
                        throw;
                    }

                    Provision = 0;
                    if (nachlass == 0)
                    {
                        // Provisionsberechnung
                        try
                        {
                            ProvisionDto paramprovision = new ProvisionDto();
                            paramprovision.rank = (int)ProvisionTypeConstants.WartungReparatur;
                            paramprovision.sysBrand = ServiceValidator.SysBRAND;
                            paramprovision.sysPerole = ServiceValidator.SysPEROLE;
                            paramprovision.sysobtyp = sysObTyp;
                            paramprovision.sysprproduct = sysprproduct;
                            paramprovision.noProvision = Cic.OpenLease.Common.MembershipProvider.isInternalMitarbeiter(ServiceValidator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);
                            if (SPECIALCALCSTATUS.HasValue && SPECIALCALCSTATUS.Value > 0)
                                paramprovision.noProvision = false;
                            PROVDao dao = new PROVDao(Context);
                            paramprovision = dao.DeliverProvision(paramprovision);
                            Provision = paramprovision.provision;
                        }
                        catch (Exception ie)
                        {
                            throw new InvalidOperationException("Provision für WartungReparatur(" + ProvisionTypeConstants.WartungReparatur + ") nicht konfiguriert: " + ie.Message, ie);
                        }
                    }




                    if (WRRate != null)
                    {


                        //Convert to dto
                        WRRateDto = WRRateAssembler.ConvertToDto(WRRate);

                        // Calculate gross and tax values
                        WRRateDto.RateDefault = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(WRRateDto.RateNetto, TaxRate));
                        WRRateDto.RateBrutto = WRRateDto.RateDefault;


                        WRRateDto.RateBrutto -= nachlass;
                        WRRateDto.RateNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(WRRateDto.RateBrutto, TaxRate));

                        // Add Provision to Dto
                        WRRateDto.Provision = Provision;


                        if (fstyp != null)
                        {
                            WRRateDto.FIXVARDEFAULT = fstyp.FIXVARDEFAULT;
                            WRRateDto.FIXVAROPTION = fstyp.FIXVAROPTION;
                        }
                        _Log.Debug("Fix-Variabel Default: " + WRRateDto.FIXVARDEFAULT + " Variabel: " + variabel);

                        /*   if (fstyp != null)
                           {
                               //Subvention Gebühr--------------------
                               Subvention sub = new Subvention(Context);
                               sub.generateImplicitSubvention(WRRateDto.RateBrutto,0,sysprproduct,sysPrFld,
                               decimal subvention = WRRateDto.RateBrutto - sub.deliverSubvention(WRRateDto.RateBrutto, sysprproduct, Subvention.CnstAREA_SERVICE, (long)fstyp.SYSFSTYP, lz);
                               WRRateDto.RateBrutto -= subvention;
                               WRRateDto.Subvention = subvention;
                               _Log.Info("Subvention für Wartung: " + subvention);
                               //Ende Subvention-------------------------------------
                               WRRateDto.RateNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(WRRateDto.RateBrutto, TaxRate);
                           }*/

                        WRRateDto.RateUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromNetValue(WRRateDto.RateNetto, TaxRate);

                        // Format
                        WRRateDto.excessKMCharge = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(WRRateDto.excessKMCharge);
                        WRRateDto.KMCharge = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(WRRateDto.KMCharge);
                        WRRateDto.Provision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(WRRateDto.Provision);

                        WRRateDto.recessKMCharge = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(WRRateDto.recessKMCharge);

                    }

                }


                //Return
                return WRRateDto;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverWRRATEFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverWRRATEFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverWRRATEFailed.ToString(), exception);

                WRRateDto rval = new WRRateDto();
                rval.Provision = 0;
                rval.RateBrutto = 0;
                rval.RateNetto = 0;
                rval.RateUst = 0;
                rval.recessKMCharge = 0;
                rval.KMCharge = 0;
                rval.excessKMCharge = 0;
                return rval;
            }
        }

        #endregion

        #region Approval
        public ApprovalDto DeliverApproval(long sysAngebot)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAngebot))
                        throw new Exception("No Permission to ANGEBOT");

                    // Query APPROVAL
                    var CurrentApproval = (from Approval in Context.APPROVAL
                                           where Approval.ANGEBOT.SYSID == sysAngebot
                                           select Approval).FirstOrDefault();

                    // Check if the approval was found
                    if (CurrentApproval == null)
                    {
                        // Throw an exception
                        throw new Exception("Sepcified approval could not be found");
                    }

                    // Return the entity converted to DTO
                    return null;// ApprovalAssembler.ConvertToDto(CurrentApproval);
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverApprovalFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverApprovalFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverApprovalFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Returns all Guardean Messages for the Offer
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        public FlowDto[] DeliverGuardeanMessages(long sysAngebot)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                if (sysAngebot == 0)
                    return new FlowDto[0];

                using (DdOlExtended Context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAngebot))
                        throw new Exception("No Permission to ANGEBOT");
                }
                FlowDao dao = new FlowDao(new DdOlExtended());

                return dao.getMessages("ANG", sysAngebot).ToArray();
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverGuardeanMessages, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverGuardeanMessages + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverGuardeanMessages.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public AuflagenInfoDto[] DeliverAuflagen(long sysAngebot)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                List<AuflagenInfoDto> rval = new List<AuflagenInfoDto>();

                using (DdOlExtended olCtx = new DdOlExtended())
                {
                    
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(olCtx, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAngebot))
                        throw new Exception("No Permission to ANGEBOT");
                    using (DdOwExtended Context = new DdOwExtended())
                    {
                        /*WFMMEMO wfmmemo = WFMMEMOHelper.DeliverWfmmemoFromAngebot(Context, sysAngebot, WFMKATHelper.CnstWfmmkatNameAngebotSubmit);
                        if (wfmmemo != null)
                        {
                            AuflagenInfoDto a = new AuflagenInfoDto();
                            a.ERFASSER = "";
                            if (wfmmemo.CREATEDATE.HasValue && wfmmemo.CREATEDATE.Value.Year > 1800)
                                a.Datum = (DateTime)wfmmemo.CREATEDATE;
                            if (wfmmemo.EDITDATE.HasValue && wfmmemo.EDITDATE.Value.Year > 1800)
                                a.Datum = (DateTime)wfmmemo.EDITDATE;

                            long? timestamp = wfmmemo.CREATETIME;
                            if (wfmmemo.EDITTIME > 0)
                                timestamp = wfmmemo.EDITTIME;
                            if(timestamp.HasValue)
                                a.Datum = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDate(a.Datum, (long)timestamp);

                            a.ERFASSER = wfmmemo.STR02;
                            //a.ERFASSER = getUserName(wfmmemo.CREATEUSER);

                            a.PRUEFER = getUserName(wfmmemo.EDITUSER);
                            
                            a.isComment = true;
                            a.Bezeichnung = Cic.OpenOne.Common.Util.StringConversionHelper.ClarionByteToString(wfmmemo.NOTIZMEMO);
                            rval.Add(a);

                        }*/
                        long sysWftable = WFTABLEHelper.DeliverSyswftableForAngebot(Context);

                        var WfmmemoQuery = from wf in Context.WFMMEMO
                                           where wf.SYSWFMTABLE == sysWftable && !wf.STR10.Contains("Interne Guardean") && wf.SYSLEASE == sysAngebot

                                           select wf;

                        if (WfmmemoQuery.Count() > 0)
                        {
                            List<WFMMEMO> memos = WfmmemoQuery.ToList<WFMMEMO>();


                            foreach (WFMMEMO m in memos)
                            {
                                //m.WFMMKATReference.Load();
                                //if (m.WFMMKAT != null) continue;
                                /*if (wfmmemo != null)
                                {
                                    if (m.SYSWFMMEMO == wfmmemo.SYSWFMMEMO) continue;
                                }*/

                                AuflagenInfoDto a = new AuflagenInfoDto();
                                a.ERFASSER = "";

                                if (m.CREATEDATE.HasValue && m.CREATEDATE.Value.Year > 1800)
                                    a.Datum = (DateTime)m.CREATEDATE.Value;
                                if (m.EDITDATE.HasValue && m.EDITDATE.Value.Year > 1800)
                                    a.Datum = (DateTime)m.EDITDATE;

                                long? timestamp = m.CREATETIME;
                                if (m.EDITTIME > 0)
                                    timestamp = m.EDITTIME;
                                if (timestamp.HasValue && a.Datum != null)
                                    a.Datum = Cic.OpenOne.Common.Util.DateTimeHelper.CreateDate(a.Datum, (long)timestamp).Value;

                                a.ERFASSER = m.STR02;// getUserName(m.CREATEUSER);

                                a.PRUEFER = getUserName(m.EDITUSER);

                                a.isComment = true;
                                a.Bezeichnung =  (m.NOTIZMEMO);
                                rval.Add(a);
                            }
                        }

                    }
                }
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {


                    string query = "select bonipos.* from bonitaet,bonipos,angebot where bonipos.sysbonitaet=bonitaet.sysbonitaet and angebot.sysid=" + sysAngebot + " and angebot.sysantrag=bonitaet.sysantrag and rownum<10";
                    //"select bonipos.* from bonitaet,bonipos where bonipos.sysbonitaet=bonitaet.sysbonitaet and sysangebot=" + sysAngebot + "and rownum<10";
                    List<AuflagenInfoDto> rval2 = Context.ExecuteStoreQuery<AuflagenInfoDto>(query, null).ToList<AuflagenInfoDto>();

                    foreach (AuflagenInfoDto bp in rval2)
                    {

                        if (bp.Erham.HasValue && bp.Erham.Value.Year > 1990)
                        {
                            bp.STATUS = "erhalten";

                            if (bp.Erham.HasValue)
                                bp.Datum = (DateTime)bp.Erham;
                        }
                        else
                        {
                            bp.STATUS = "offen";
                            if (bp.AngefAm.HasValue)
                                bp.Datum = (DateTime)bp.AngefAm;
                        }
                        bp.ERFASSER = bp.AngefVon;
                        bp.PRUEFER = bp.Form;//XXX
                    }
                    rval.AddRange(rval2);
                }

                return rval.ToArray();
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBonitaetFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBonitaetFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBonitaetFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        private string getUserName(long? uid)
        {
            if (uid.HasValue && uid > 0)
            {
                using (DdOlExtended Context = new DdOlExtended())
                {
                    try
                    {
                        PERSON p = PERSONHelper.SelectBySysPERSONWithoutException(Context, (long)uid);
                        if (p != null)
                            return p.NAME + " " + p.VORNAME;
                    }
                    catch (Exception)
                    {
                        //may not find the person
                    }
                }
            }
            return "";
        }
        public BonitaetDto[] DeliverBonitaet(long sysAngebot)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAngebot))
                        throw new Exception("No Permission to ANGEBOT");

                    // Query BONITAET
                    var BoniTaets = from BoniTaet in Context.BONITAET.Include("BONIPOSList")
                                    where BoniTaet.ANGEBOT.SYSID == sysAngebot
                                    select BoniTaet;

                    // Create BoniTaet list
                    List<BonitaetDto> BoniTaetList = new List<BonitaetDto>();
/*
                    // Create an assembler
                    BoniTaetAssembler Assembler = new BoniTaetAssembler();

                    // Loop through all rows
                    foreach (var LoopBoniTaet in BoniTaets)
                    {
                        // Add BoniTaet to the list
                        BoniTaetList.Add(Assembler.ConvertToDto(LoopBoniTaet));
                    }
                    */
                    // Return the list
                    return BoniTaetList.ToArray();
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBonitaetFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBonitaetFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBonitaetFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        #endregion

        public ANGEBOTDto CloneAngebot(ANGEBOTDto AngebotDto)
        {
            return AngebotDto;
        }

        #region SpecialCalculation
        public ANGEBOTDto RequestSpecialCalculation(ANGEBOTDto AngebotDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                if (AngebotDto != null && ZustandHelper.GetEnumeratorFromString(AngebotDto.ZUSTAND) != AngebotZustand.Gedruckt)
                {
                    AngebotDto.SPECIALCALCSTATUS = 1;
                    if (AngebotDto.SPECIALCALCCOUNT == null)
                        AngebotDto.SPECIALCALCCOUNT = 0;
                    else AngebotDto.SPECIALCALCCOUNT++;

                    AngebotDto = AngebotDtoHelper.Save(AngebotDto, ServiceValidator.SysBRAND, ServiceValidator.SysPEROLE, ServiceValidator.VpSysPERSON.GetValueOrDefault(), ServiceValidator.SysPERSON, ServiceValidator.SYSWFUSER, ServiceValidator.SYSPUSER);
                    return AngebotDto;
                }
                else throw new Exception("Special Calculation not allowed!");


            }
            catch (Exception exception)
            {

                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SCalcRequestFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.SCalcRequestFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.SCalcRequestFailed.ToString(), exception);
                if (exception is ServiceException)
                {
                    ServiceException se = (ServiceException)exception;
                    if (se.Code != null && se.Code.Name != null && se.Code.Name.Equals(((int)Cic.OpenLease.ServiceAccess.ServiceCodes.SaveAngebotFailedProduct).ToString()))
                    {
                        ANGEBOTDto rval = new ANGEBOTDto();
                        rval.ERRORMESSAGE = exception.Message;
                        rval.ERRORDETAIL = exception.StackTrace;
                        return rval;
                    }
                }
                // Throw the exception
                throw TopLevelException;
            }


        }


        public ANGEBOTDto PerformSpecialCalculation(ANGEBOTDto AngebotDto)
        {

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                AngebotDto.SPECIALCALCSTATUS = 3;
                AngebotDto.SPECIALCALCSYSWFUSER = ServiceValidator.SYSWFUSER;
                AngebotDto.SPECIALCALCDATE = System.DateTime.Today;

                //#3450 add Specialcalcuser to memo
                using (DdOlExtended Context = new DdOlExtended())
                {
                    PersonDto vkper = PeroleHelper.DeliverPerson(Context, ServiceValidator.SysPEROLE);
                    if (vkper != null)
                    {
                        AngebotDto.WFMMEMOSCALCVKTEXT += " / " + vkper.VORNAME + " " + vkper.NAME;
                    }
                }

                AngebotDto = AngebotDtoHelper.Save(AngebotDto, ServiceValidator.SysBRAND, ServiceValidator.SysPEROLE, ServiceValidator.VpSysPERSON.GetValueOrDefault(), ServiceValidator.SysPERSON, ServiceValidator.SYSWFUSER, ServiceValidator.SYSPUSER);
                return AngebotDto;
            }
            catch (Exception exception)
            {

                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SCalcPerformedFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.SCalcPerformedFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.SCalcPerformedFailed.ToString(), exception);


                if (exception is ServiceException)
                {
                    ServiceException se = (ServiceException)exception;
                    if (se.Code != null && se.Code.Name != null && se.Code.Name.Equals(((int)Cic.OpenLease.ServiceAccess.ServiceCodes.SaveAngebotFailedProduct).ToString()))
                    {
                        ANGEBOTDto rval = new ANGEBOTDto();
                        rval.ERRORMESSAGE = exception.Message;
                        rval.ERRORDETAIL = exception.StackTrace;
                        return rval;
                    }
                }
                // Throw the exception
                throw TopLevelException;
            }



        }
        #endregion

        #region TransferVendors
        /// <summary>
        /// Delivers a list of all vendors in the same or deeper level as the current logged in role
        /// </summary>
        /// <returns></returns>
        public TransferVendorDto[] DeliverTransferVendors()
        {

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    VendorDao vd = new VendorDao(context);
                    return vd.DeliverTransferVendors(ServiceValidator.SysPEROLE);
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTransferVendorsFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTransferVendorsFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTransferVendorsFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }

        }

        /// <summary>
        /// Delivers a list of all vendors in the same or deeper level as the given person
        /// </summary>
        /// <returns></returns>
        public TransferVendorDto[] DeliverTransferVendorsProvision(long sysberataddb)
        {

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    VendorDao vd = new VendorDao(context);
                    return vd.DeliverTransferVendorsProvision(sysberataddb);
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTransferVendorsFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTransferVendorsFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverTransferVendorsFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }

        }


        /// <summary>
        /// Connects the offer to a different vendor
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="param"></param>
        public void TransferOfferToVendor(long sysid, TransferVendorDto param)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    // Open the connection
                    //context.Connection.Open();

                    // Create a transaction
                    //TRANSACTIONS
                    //DbTransaction OlTransaction = context.Connection.BeginTransaction();

                    VendorDao vd = new VendorDao(context);
                    vd.transferOfferToVendor(sysid, param, ServiceValidator.SYSPUSER);
                    vd.transferOfferProposalsToVendor(sysid, param, ServiceValidator.SYSPUSER);

                    context.SaveChanges();
                    // OlTransaction.Commit();

                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.TransferOfferToVendorFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.TransferOfferToVendorFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.TransferOfferToVendorFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        #endregion

        #region binary features
        public byte[] deliverBinaryData(long SYSWFDADOC)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                using (DdOwExtended context = new DdOwExtended())
                {
                    AngebotBinaryDao dao = new AngebotBinaryDao(context);
                    return dao.loadDataById(SYSWFDADOC);

                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBinaryDataFailed, exception);

                // Log the exception
                _Log.Warn(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBinaryDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBinaryDataFailed.ToString());

                // Throw the exception
                throw TopLevelException;
            }
        }
        public long saveBinaryData(byte[] data)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                using (DdOwExtended context = new DdOwExtended())
                {
                    AngebotBinaryDao dao = new AngebotBinaryDao(context);
                    return dao.saveAngebotData(data);

                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SaveBinaryDataFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.SaveBinaryDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.SaveBinaryDataFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        #endregion



        #endregion

        #region My Methods






        
        /// <summary>
        /// Searches for offers with the given filter parameters
        /// </summary>
        /// <param name="context"></param>
        /// <param name="angebotSearchData"></param>
        /// <param name="searchParameters"></param>
        /// <param name="angebotSortData"></param>
        /// <param name="sysPERSONInPEROLE"></param>
        /// <param name="sysPUSER"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<ANGEBOTShortDto> MyDeliverANGEBOTList(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData angebotSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSortData[] angebotSortData, long? sysPERSONInPEROLE, long? sysPUSER, out long count)
        {
            string Query;
            System.Collections.Generic.List<ANGEBOTShortDto> ANGEBOTList;
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();

            if (context == null)
            {
                throw new ArgumentException("context");
            }

            if (angebotSearchData == null)
            {
                throw new ArgumentException("angebotSearchData");
            }

            // Create new list
            ANGEBOTList = new System.Collections.Generic.List<ANGEBOTShortDto>();
            object[] Parameters = MyDeliverQueryParameters(angebotSearchData);


            //GET Created Query
            Query = MyCreateQuery(context, angebotSearchData, sysPERSONInPEROLE, sysPUSER, false);

            QueryBuilder.Append(Query);
            StringBuilder sortBuilder = new StringBuilder();
            if (angebotSortData != null && angebotSortData.Length > 0)
            {
                int i = 0;
                // Order
                sortBuilder.Append(" ORDER BY ");
                foreach (Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSortData angebotSortDataLoop in angebotSortData)
                {
                    String sortField = angebotSortDataLoop.SortDataConstant.ToString();
                    if (angebotSortDataLoop.SortDataConstant == ANGEBOTSortData.SortDataConstants.ANGEBOT1)
                        sortField = "ANGEBOT";
                    sortBuilder.Append(sortField + " " + angebotSortDataLoop.SortOrderConstant.ToString());
                    if (i != angebotSortData.Length - 1)
                    {
                        sortBuilder.Append(", ");
                    }
                    i++;
                }
            }
            else
            {
                // Default Order
                sortBuilder.Append(" ORDER BY AKTIVKZ, DATAKTIV, ANGEBOT1");
            }
            QueryBuilder.Append(sortBuilder.ToString());
            String anQuery = QueryBuilder.ToString();


            bool superOptimizedSearchEnabled = true;
            if (superOptimizedSearchEnabled)
            {
                //optimization-idea
                //when unknown query:
                //  now outer rows-query/conditions
                //  fetch only primary key
                //  generate a cached id-list for this query
                //always:
                //  fetch the section to use from the id-list by given parameters skip and pageSize
                //  use inner query with joins and just one search-condition: primary-key in in(<section-ids>) for the detail-fetching of all fields
                double starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                String resultIdCacheKey = getParamKey(Parameters, "IT_" + sysPUSER + sortBuilder.ToString()) + angebotSearchData.SPECIALCALC;

                if (!resultIdCache.ContainsKey(resultIdCacheKey))
                {
                    _Log.Debug("ANGEBOT-Search-SQL: " + anQuery);
                    resultIdCache[resultIdCacheKey] = context.ExecuteStoreQuery<long>(anQuery, Parameters).ToList();
                }
                List<long> ids = resultIdCache[resultIdCacheKey];


                count = ids.Count;

                //avoid range-fetching errors:
                int useSize = searchParameters.Top;
                if (searchParameters.Skip > ids.Count)
                {
                    searchParameters.Skip = ids.Count - 1 - useSize;

                }
                if (searchParameters.Skip < 0) searchParameters.Skip = 0;
                if (searchParameters.Skip + useSize >= ids.Count)
                {
                    useSize = ids.Count - searchParameters.Skip;
                }
                ids = ids.GetRange(searchParameters.Skip, useSize);
                //avoid error on empty list
                if (ids.Count == 0)
                    ids.Add(-1);
                //build the simple id fech query

                anQuery = @"SELECT 
(SELECT extstate.zustand 
FROM attribut,attributdef,state,statedef extstate,statedef intstate,wftable,antrag 
WHERE attribut.sysstate = state.sysstate
and antrag.sysangebot=angebot.sysid
AND attribut.sysattributdef= attributdef.sysattributdef
AND attribut.sysstatedef = extstate.sysstatedef
AND state.sysstatedef =intstate.sysstatedef
AND state.syswftable = wftable.syswftable
AND wftable.syscode = 'ANTRAG' and UPPER(attributdef.attribut) = upper(antrag.attribut) and upper(intstate.zustand)=upper(antrag.zustand))  ANTRAGSSTATUS,
ANGEBOT.SYSVORVT, ANGEBOT.CONTRACTEXT, ANGEBOT.SPECIALCALCSTATUS,ANGEBOT.SPECIALCALCCOUNT,ANGEBOT.SYSID, ANGEBOT.VERTRAG, ANGEBOT.HAUPTVERTRAG, ANGEBOT.ANTRAG, ANGEBOT.ANGEBOT AS ANGEBOT1, ANGEBOT.SYSAGB, ANGEBOT.SYSLS, ANGEBOT.SYSVT, ANGEBOT.SYSANTRAG, ANGEBOT.SYSANGEBOT, ANGEBOT.SYSFANK, ANGEBOT.SYSDS, ANGEBOT.ZUSTAND, ANGEBOT.ZUSTANDAM, ANGEBOT.ERFASSUNG, ANGEBOT.BEARBEITUNG, ANGEBOT.BENUTZER, ANGEBOT.OK, ANGEBOT.AKTIVKZ, ANGEBOT.ENDEKZ, ANGEBOT.ENDEAM, ANGEBOT.FINKZ, ANGEBOT.AKTIV, ANGEBOT.SYSADM, ANGEBOT.SYSBERATADDA, ANGEBOT.SYSBERATADDB, ANGEBOT.SYSKD, ANGEBOT.SYSVK, ANGEBOT.SYSBN, ANGEBOT.RANGBN, ANGEBOT.SYSKI, ANGEBOT.RANGKI, ANGEBOT.SYSGA, ANGEBOT.SYSLF, ANGEBOT.SYSRA, ANGEBOT.SYSUN, ANGEBOT.SYSVS, ANGEBOT.STANDORT, ANGEBOT.INVENTAR, ANGEBOT.FABRIKAT, ANGEBOT.OBJEKTVT, ANGEBOT.OBJEKTKZ, ANGEBOT.SERIE, ANGEBOT.LFORM, ANGEBOT.NUTZUNG, ANGEBOT.VART, ANGEBOT.VTYP, ANGEBOT.KONSTELLATION, ANGEBOT.FFORM, ANGEBOT.FSATZ, ANGEBOT.BRUTTOKREDIT, ANGEBOT.GRUND, ANGEBOT.GESAMT, ANGEBOT.BGEXTERN, ANGEBOT.AHK, ANGEBOT.BGINTERN, ANGEBOT.DISAGIO, ANGEBOT.SZ, ANGEBOT.SZ2, ANGEBOT.PROVISION, ANGEBOT.GEBUEHR, ANGEBOT.RSV, ANGEBOT.RSV2, ANGEBOT.RW, ANGEBOT.RWHIST, ANGEBOT.RWKALK, ANGEBOT.DB, ANGEBOT.ZINS, ANGEBOT.ZINSEFF, ANGEBOT.BASISZINS, ANGEBOT.LZ, ANGEBOT.RLZ, ANGEBOT.GLZ, ANGEBOT.ALZ, ANGEBOT.MLZ, ANGEBOT.PPY, ANGEBOT.MODUS, ANGEBOT.RATE, ANGEBOT.EXTNOM, ANGEBOT.EXTPANGV, ANGEBOT.EXTEFF, ANGEBOT.EXTNOM2, ANGEBOT.INTZINS1, ANGEBOT.INTZINS2, ANGEBOT.INTZINS3, ANGEBOT.INTZINS4, ANGEBOT.REFIZINS1, ANGEBOT.REFIZINS2, ANGEBOT.BWMARGE1, ANGEBOT.BWMARGEP1, ANGEBOT.BWMARGE2, ANGEBOT.BWMARGEP2, ANGEBOT.BEGINN, ANGEBOT.ENDE, ANGEBOT.ERSTERATE, ANGEBOT.LETZTERATE, ANGEBOT.LTSOLL, ANGEBOT.STAND, ANGEBOT.UEBERNAHME, ANGEBOT.RUECKGABE, ANGEBOT.VALUTATAG, ANGEBOT.FINSATZ, ANGEBOT.ZAHLBONITAET, ANGEBOT.LOCKED, ANGEBOT.VALUTATAGE, ANGEBOT.SYSVART, ANGEBOT.STRUKTUR, ANGEBOT.ABTRETUNG, ANGEBOT.ABTRETUNGVON, ANGEBOT.VERWDRITTER, ANGEBOT.VERWDRITTERBIND, ANGEBOT.VERWLIEF, ANGEBOT.VERWLIEFWERT, ANGEBOT.VERWRCKNAHME, ANGEBOT.VERTRIEBSWEG, ANGEBOT.ZUSENDUNG, ANGEBOT.STOPFAKT, ANGEBOT.DATANGEBOT, ANGEBOT.DATABSCHLUSS, ANGEBOT.DATEINREICHUNG, ANGEBOT.DATAKTIV, ANGEBOT.SYSVPFIL, ANGEBOT.VPANGEBOT, ANGEBOT.VPVERTRAG, ANGEBOT.VPERTRERW, ANGEBOT.VPEINVERG, ANGEBOT.SYSBERATER, ANGEBOT.SYSRVT, ANGEBOT.DATKUENDIGUNG, ANGEBOT.REDUZIERUNG, ANGEBOT.ABLOESE, ANGEBOT.SCHLUSSZAHL, ANGEBOT.SYSVTTYP, ANGEBOT.SYSMWST, ANGEBOT.SYSWAEHRUNG, ANGEBOT.SYSRNWAEHRUNG, ANGEBOT.BONUSFLAG, ANGEBOT.NETTINGFLAG, ANGEBOT.SPERRE, ANGEBOT.SYSKOSTSTEL, ANGEBOT.SYSKOSTTRAE, ANGEBOT.SYSKGRUPPE, ANGEBOT.EINZUG, ANGEBOT.ZAHLSPERRE, ANGEBOT.AUSZAHLSPERRE, ANGEBOT.AUSZAHLSPERREBIS, ANGEBOT.MAHNSPERRE, ANGEBOT.MSPERREBIS, ANGEBOT.ERINKLMWST, ANGEBOT.FAKTURIERUNG, ANGEBOT.BLOCKRATE, ANGEBOT.ESRFLAG, ANGEBOT.IBORSATZ, ANGEBOT.IBORAUFSCHLAG, ANGEBOT.IBORFRIST, ANGEBOT.IBORBEZUG, ANGEBOT.SYSIBOR, ANGEBOT.AKTOBLIGO, ANGEBOT.AKTOPOS, ANGEBOT.SYSPARTNER, ANGEBOT.RWREDUZ, ANGEBOT.LZVERL, ANGEBOT.SYSEVL, ANGEBOT.ZINSCUST, ANGEBOT.ZINSCUST2, ANGEBOT.SYSVARTTAB, ANGEBOT.SYSGST, ANGEBOT.ZINSCUST3, ANGEBOT.MANFEE, ANGEBOT.ANZAHLOB, ANGEBOT.SYSFBN, ANGEBOT.DIFFERENZ, ANGEBOT.DIFFERENZP, ANGEBOT.EXTRATINGCODE, ANGEBOT.BESCHRDEUTSCH, ANGEBOT.BESCHRENGLISCH, ANGEBOT.ZEKGESUCHSID, ANGEBOT.ATTRIBUT, ANGEBOT.ATTRIBUTAM, ANGEBOT.SYSIT, ANGEBOT.PTYP, ANGEBOT.SYSWFUSER, ANGEBOT.SYSPEROLCONT, ANGEBOT.SYSPEROLSIGN, ANGEBOT.SYSPEROLSIGN2, ANGEBOT.SYSADRESSEP, ANGEBOT.SYSADRESSEL, ANGEBOT.SYSLANGOFFER, ANGEBOT.CHECKEDON, ANGEBOT.LOCKEDON, ANGEBOT.RWDUEON, ANGEBOT.RWBASE, ANGEBOT.RWCRV, ANGEBOT.SYSBRAND, ANGEBOT.GUELTIGBIS, ANGEBOT.SYSPRPRODUCT, ANGEBOT.MITFINB, vart.bezeichnung PRPRODUCTNAME, wfuser.name SCALCUSERNAME, wfuser.vorname SCALCUSERVORNAME  FROM CIC.ANGEBOT left outer join vart on vart.sysvart=angebot.sysvart left outer join wfuser on wfuser.syswfuser=specialcalcsyswfuser where angebot.sysid in (" + string.Join(",", ids) + ") " + sortBuilder.ToString();
                _Log.Debug("ANGEBOT-Search-SQL: " + anQuery);
                ANGEBOTList = context.ExecuteStoreQuery<ANGEBOTShortDto>(anQuery, null).ToList();
                _Log.Debug("ANGEBOT-Search Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                return ANGEBOTList;
            }


            // Check top
            if ((searchParameters != null) && (searchParameters.Top > 0))
            {
                Query = "SELECT /*+ RULE */ * FROM (SELECT rownum rnum, a.* FROM(" + QueryBuilder.ToString() + ") a WHERE rownum <= " + (searchParameters.Skip + searchParameters.Top) + ") WHERE rnum > " + searchParameters.Skip;
            }
            _Log.Debug("Angebot-Search Query: " + Query);



            try
            {
                // Create list

                double starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;

                int resultCount = 0;
                String countKey = getParamKey(Parameters, Query + angebotSearchData.SPECIALCALC);
                // count when query changed in the last 2 minutes
                if (!searchCache.ContainsKey(countKey))
                {
                    String countQuery = MyCreateQuery(context, angebotSearchData, sysPERSONInPEROLE, sysPUSER, true);
                    resultCount = context.ExecuteStoreQuery<int>(countQuery, Parameters).FirstOrDefault();
                    Parameters = MyDeliverQueryParameters(angebotSearchData);
                }
                else
                    resultCount = searchCache[countKey];

                ANGEBOTList = context.ExecuteStoreQuery<ANGEBOTShortDto>(Query, Parameters).ToList<ANGEBOTShortDto>();
                _Log.Debug("Angebot-Search Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                searchCache[countKey] = resultCount; //touch the cache, another cache lifetime duration for no fetching the count
                count = resultCount;
            }
            catch
            {
                throw;
            }

            //return
            return ANGEBOTList;
        }
        private static String getParamKey(object[] par, String prefix)
        {
            StringBuilder sb = new StringBuilder(prefix);
            sb.Append(": ");
            foreach (object o in par)
                sb.Append(((Devart.Data.Oracle.OracleParameter)o).Value.ToString());
            return sb.ToString();
        }
        private static string MyCreateQuery(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData angebotSearchData, long? sysPERSONInPEROLE, long? sysPUSER, bool count)
        {
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();

            if (context == null)
            {
                throw new ArgumentException("context");
            }

            if (angebotSearchData == null)
            {
                throw new ArgumentException("angebotSearchData");
            }

            if (count)
            {
                QueryBuilder.Append("SELECT /*+ RULE */ COUNT(*) FROM CIC.ANGEBOT WHERE 1 = 1 ");
            }
            else
            {
                //QueryBuilder.Append("SELECT /*+ RULE */ ANGEBOT.SPECIALCALCSTATUS,ANGEBOT.SPECIALCALCCOUNT,ANGEBOT.SYSID, ANGEBOT.VERTRAG, ANGEBOT.HAUPTVERTRAG, ANGEBOT.ANTRAG, ANGEBOT.ANGEBOT AS ANGEBOT1, ANGEBOT.SYSAGB, ANGEBOT.SYSLS, ANGEBOT.SYSVT, ANGEBOT.SYSANTRAG, ANGEBOT.SYSANGEBOT, ANGEBOT.SYSFANK, ANGEBOT.SYSDS, ANGEBOT.ZUSTAND, ANGEBOT.ZUSTANDAM, ANGEBOT.ERFASSUNG, ANGEBOT.BEARBEITUNG, ANGEBOT.BENUTZER, ANGEBOT.OK, ANGEBOT.AKTIVKZ, ANGEBOT.ENDEKZ, ANGEBOT.ENDEAM, ANGEBOT.FINKZ, ANGEBOT.AKTIV, ANGEBOT.SYSADM, ANGEBOT.SYSBERATADDA, ANGEBOT.SYSBERATADDB, ANGEBOT.SYSKD, ANGEBOT.SYSVK, ANGEBOT.SYSBN, ANGEBOT.RANGBN, ANGEBOT.SYSKI, ANGEBOT.RANGKI, ANGEBOT.SYSGA, ANGEBOT.SYSLF, ANGEBOT.SYSRA, ANGEBOT.SYSUN, ANGEBOT.SYSVS, ANGEBOT.STANDORT, ANGEBOT.INVENTAR, ANGEBOT.FABRIKAT, ANGEBOT.OBJEKTVT, ANGEBOT.OBJEKTKZ, ANGEBOT.SERIE, ANGEBOT.LFORM, ANGEBOT.NUTZUNG, ANGEBOT.VART, ANGEBOT.VTYP, ANGEBOT.KONSTELLATION, ANGEBOT.FFORM, ANGEBOT.FSATZ, ANGEBOT.BRUTTOKREDIT, ANGEBOT.GRUND, ANGEBOT.GESAMT, ANGEBOT.BGEXTERN, ANGEBOT.AHK, ANGEBOT.BGINTERN, ANGEBOT.DISAGIO, ANGEBOT.SZ, ANGEBOT.SZ2, ANGEBOT.PROVISION, ANGEBOT.GEBUEHR, ANGEBOT.RSV, ANGEBOT.RSV2, ANGEBOT.RW, ANGEBOT.RWHIST, ANGEBOT.RWKALK, ANGEBOT.DB, ANGEBOT.ZINS, ANGEBOT.ZINSEFF, ANGEBOT.BASISZINS, ANGEBOT.LZ, ANGEBOT.RLZ, ANGEBOT.GLZ, ANGEBOT.ALZ, ANGEBOT.MLZ, ANGEBOT.PPY, ANGEBOT.MODUS, ANGEBOT.RATE, ANGEBOT.EXTNOM, ANGEBOT.EXTPANGV, ANGEBOT.EXTEFF, ANGEBOT.EXTNOM2, ANGEBOT.INTZINS1, ANGEBOT.INTZINS2, ANGEBOT.INTZINS3, ANGEBOT.INTZINS4, ANGEBOT.REFIZINS1, ANGEBOT.REFIZINS2, ANGEBOT.BWMARGE1, ANGEBOT.BWMARGEP1, ANGEBOT.BWMARGE2, ANGEBOT.BWMARGEP2, ANGEBOT.BEGINN, ANGEBOT.ENDE, ANGEBOT.ERSTERATE, ANGEBOT.LETZTERATE, ANGEBOT.LTSOLL, ANGEBOT.STAND, ANGEBOT.UEBERNAHME, ANGEBOT.RUECKGABE, ANGEBOT.VALUTATAG, ANGEBOT.FINSATZ, ANGEBOT.ZAHLBONITAET, ANGEBOT.LOCKED, ANGEBOT.VALUTATAGE, ANGEBOT.SYSVART, ANGEBOT.STRUKTUR, ANGEBOT.ABTRETUNG, ANGEBOT.ABTRETUNGVON, ANGEBOT.VERWDRITTER, ANGEBOT.VERWDRITTERBIND, ANGEBOT.VERWLIEF, ANGEBOT.VERWLIEFWERT, ANGEBOT.VERWRCKNAHME, ANGEBOT.VERTRIEBSWEG, ANGEBOT.ZUSENDUNG, ANGEBOT.STOPFAKT, ANGEBOT.DATANGEBOT, ANGEBOT.DATABSCHLUSS, ANGEBOT.DATEINREICHUNG, ANGEBOT.DATAKTIV, ANGEBOT.SYSVPFIL, ANGEBOT.VPANGEBOT, ANGEBOT.VPVERTRAG, ANGEBOT.VPERTRERW, ANGEBOT.VPEINVERG, ANGEBOT.SYSBERATER, ANGEBOT.SYSRVT, ANGEBOT.DATKUENDIGUNG, ANGEBOT.REDUZIERUNG, ANGEBOT.ABLOESE, ANGEBOT.SCHLUSSZAHL, ANGEBOT.SYSVTTYP, ANGEBOT.SYSMWST, ANGEBOT.SYSWAEHRUNG, ANGEBOT.SYSRNWAEHRUNG, ANGEBOT.BONUSFLAG, ANGEBOT.NETTINGFLAG, ANGEBOT.SPERRE, ANGEBOT.SYSKOSTSTEL, ANGEBOT.SYSKOSTTRAE, ANGEBOT.SYSKGRUPPE, ANGEBOT.EINZUG, ANGEBOT.ZAHLSPERRE, ANGEBOT.AUSZAHLSPERRE, ANGEBOT.AUSZAHLSPERREBIS, ANGEBOT.MAHNSPERRE, ANGEBOT.MSPERREBIS, ANGEBOT.ERINKLMWST, ANGEBOT.FAKTURIERUNG, ANGEBOT.BLOCKRATE, ANGEBOT.ESRFLAG, ANGEBOT.IBORSATZ, ANGEBOT.IBORAUFSCHLAG, ANGEBOT.IBORFRIST, ANGEBOT.IBORBEZUG, ANGEBOT.SYSIBOR, ANGEBOT.AKTOBLIGO, ANGEBOT.AKTOPOS, ANGEBOT.SYSPARTNER, ANGEBOT.RWREDUZ, ANGEBOT.LZVERL, ANGEBOT.SYSEVL, ANGEBOT.ZINSCUST, ANGEBOT.ZINSCUST2, ANGEBOT.SYSVARTTAB, ANGEBOT.SYSGST, ANGEBOT.ZINSCUST3, ANGEBOT.MANFEE, ANGEBOT.ANZAHLOB, ANGEBOT.SYSFBN, ANGEBOT.DIFFERENZ, ANGEBOT.DIFFERENZP, ANGEBOT.EXTRATINGCODE, ANGEBOT.BESCHRDEUTSCH, ANGEBOT.BESCHRENGLISCH, ANGEBOT.ZEKGESUCHSID, ANGEBOT.ATTRIBUT, ANGEBOT.ATTRIBUTAM, ANGEBOT.SYSIT, ANGEBOT.PTYP, ANGEBOT.SYSWFUSER, ANGEBOT.SYSPEROLCONT, ANGEBOT.SYSPEROLSIGN, ANGEBOT.SYSPEROLSIGN2, ANGEBOT.SYSADRESSEP, ANGEBOT.SYSADRESSEL, ANGEBOT.SYSLANGOFFER, ANGEBOT.CHECKEDON, ANGEBOT.LOCKEDON, ANGEBOT.RWDUEON, ANGEBOT.RWBASE, ANGEBOT.RWCRV, ANGEBOT.SYSBRAND, ANGEBOT.GUELTIGBIS, ANGEBOT.SYSPRPRODUCT, ANGEBOT.MITFINB, prproduct.name PRPRODUCTNAME, wfuser.name SCALCUSERNAME, wfuser.vorname SCALCUSERVORNAME  FROM CIC.ANGEBOT left outer join prproduct on prproduct.sysprproduct=angebot.sysprproduct left outer join wfuser on wfuser.syswfuser=specialcalcsyswfuser WHERE 1 = 1 ");
                QueryBuilder.Append("SELECT ANGEBOT.SYSID FROM CIC.ANGEBOT left outer join prproduct on prproduct.sysprproduct=angebot.sysprproduct left outer join wfuser on wfuser.syswfuser=specialcalcsyswfuser WHERE 1 = 1 ");
            }
            // ANGEBOT
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.ANGEBOT1))
            {
                // Where
                QueryBuilder.Append("AND UPPER(ANGEBOT) LIKE UPPER(:pANGEBOT) ");
            }

            // InteressentID
            if ((angebotSearchData.SYSIT.HasValue) && (angebotSearchData.SYSIT.Value > 0))
            {
                QueryBuilder.Append("AND ANGEBOT.SYSIT = :pSYSIT ");
            }

            // VerkauferName
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.PersonVerkaufer))
            {
                // Where
                QueryBuilder.Append("AND SYSBERATADDB IN(SELECT SYSPERSON FROM CIC.PERSON WHERE (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:pVERKAEUFER))) ");
            }


            // Interessent

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.PersonIt))
            {
                QueryBuilder.Append("AND SYSIT IN (SELECT SYSIT FROM CIC.IT WHERE (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:pPERSON))) ");
            }

            // ZUSTAND
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.ZUSTAND))
            {
                // Where
                QueryBuilder.Append("AND UPPER(ZUSTAND) LIKE UPPER(:pZUSTAND) ");
            }

            // SONDERKALKULATION
            if (angebotSearchData.SPECIALCALC)
            {
                // Where
                QueryBuilder.Append("AND ((UPPER(ZUSTAND) = 'NEU' OR UPPER(ZUSTAND) = 'KALKULIERT') AND (SPECIALCALCSTATUS > 0 AND SPECIALCALCSTATUS<3)) ");
            }



            // Finanzierungsart
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.Finanzierungsart))
            {
                // Where
                QueryBuilder.Append("AND ANGEBOT.SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
            }

            // FzKENNZEICHEN

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.FzKENNZEICHEN))
            {
                QueryBuilder.Append("AND SYSID IN (SELECT SYSANGEBOT FROM ANGKALK WHERE SYSOB IN( SELECT DISTINCT SYSOB FROM CIC.ANGOB WHERE SYSOB IS NOT NULL AND SYSOB <> 0  AND UPPER(KENNZEICHEN) LIKE UPPER(:pFzKENNZEICHEN) ))");
            }

            // FzBEZEICHNUNG
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.FzBEZEICHNUNG))
            {
                QueryBuilder.Append("AND UPPER(OBJEKTVT) LIKE UPPER(:pFzBEZEICHNUNG) ");
            }


            // AngVon
            if (angebotSearchData.AngVon.HasValue)
            {

                // Angebot
                QueryBuilder.Append("AND to_char(DATANGEBOT,'yyyy-mm-dd') >= :pAngVon AND DATANGEBOT IS NOT NULL ");


            }

            // AngAntVtBis
            if (angebotSearchData.AngBis.HasValue)
            {
                // Angebot
                QueryBuilder.Append("AND to_char(DATANGEBOT,'yyyy-mm-dd') <= :pAngBis AND DATANGEBOT IS NOT NULL ");
            }



            // Sight fields narrowing
            if (sysPERSONInPEROLE.HasValue)
            {

                QueryBuilder.Append("AND SYSID IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", 'ANGEBOT',sysdate))) ");
            }


            //Get filter
            string Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "ANGEBOT", string.Empty);
            

            if (Filter != null && Filter.Length > 0)
            {
                QueryBuilder.Append("AND " + Filter);
            }

            //Return
            return QueryBuilder.ToString();
        }






        private object[] MyDeliverQueryParameters(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData angebotSearchData)
        {

            System.Collections.Generic.List<Devart.Data.Oracle.OracleParameter> ParametersList = new System.Collections.Generic.List<Devart.Data.Oracle.OracleParameter>();
            // ANGEBOT
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.ANGEBOT1))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pANGEBOT", Value = "%" + angebotSearchData.ANGEBOT1.Trim() + "%" });
            }

            // Interessent
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.PersonIt))
            {

                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pPERSON", Value = "%" + (angebotSearchData.PersonIt.Trim()).Replace(" ", "%") + "%" });
            }

            // Zustand
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.ZUSTAND))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pZUSTAND", Value = angebotSearchData.ZUSTAND });
            }

            // Veraufer
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.PersonVerkaufer))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pVERKAEUFER", Value = "%" + (angebotSearchData.PersonVerkaufer.Trim()).Replace(" ", "%") + "%" });
            }

            // InteressentID
            if ((angebotSearchData.SYSIT.HasValue) && (angebotSearchData.SYSIT.Value > 0))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSYSIT", Value = angebotSearchData.SYSIT });
            }

            // Interessent
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.Finanzierungsart))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pFinanzierungsart", Value = "%" + angebotSearchData.Finanzierungsart.Trim() + "%" });
            }

            // Interessent
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.FzBEZEICHNUNG))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pFzBEZEICHNUNG", Value = "%" + angebotSearchData.FzBEZEICHNUNG.Trim() + "%" });
            }

            // Interessent
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebotSearchData.FzKENNZEICHEN))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pFzKENNZEICHEN", Value = "%" + angebotSearchData.FzKENNZEICHEN.Trim() + "%" });
            }

            //AngAntVtVon
            if (angebotSearchData.AngVon.HasValue)
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pAngVon", Value = angebotSearchData.AngVon.Value.Date.ToString("yyyy-MM-dd") });
            }

            if (angebotSearchData.AngBis.HasValue)
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pAngBis", Value = angebotSearchData.AngBis.Value.Date.ToString("yyyy-MM-dd") });
            }

            return ParametersList.ToArray();
        }

        /*/// <summary>
        /// @Deprecated
        /// Creates the Angebot-Data object from the CarConfigurator information
        /// </summary>
        /// <param name="configurationIdentifier"></param>
        /// <returns></returns>
        public ANGEBOTDto DeliverAngebotDtoFromConfigurationManager2(Guid configurationIdentifier)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Get the offer
                OfferConfiguration OfferConfiguration = ConfigurationManagerHelper.GetCmOfferConfiguration2(configurationIdentifier);

                // Create and return angebot
                return AngebotDtoHelper.CreateAngebotDto(OfferConfiguration);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed + " " +Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAngebotDtoFromConfigurationManagerFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }*/

        /// <summary>
        /// service method to calculate the insurance fee for all kind of service products
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public InsuranceResultDto DeliverVSData(InsuranceParameterDto param)
        {
            List<InsuranceParameterDto> insuranceParams = new List<InsuranceParameterDto>();
            insuranceParams.Add(param);
            List<InsuranceResultDto> results = DeliverVS(insuranceParams);
            return results[0];
        }

        #endregion

        /// <summary>
        /// Validates a contract before extension
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        public List<ValidationResultDto> validateVTExtension(long sysvt, int contractext)
        {
            try
            {
                ServiceValidator svc = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

                ContractExtensionDao ced = new ContractExtensionDao();
                using (DdOlExtended Context = new DdOlExtended())
                {

                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, svc.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.VT, sysvt))
                        throw new Exception("No Permission to VT");
                }
                return ced.validateVTExtension(sysvt, contractext);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.validateVTExtensionFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.validateVTExtensionFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.validateVTExtensionFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Creates a new Extension Offer from a Contract
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        public ANGEBOTDto getAngebotFromVertrag(long sysvt, int contractext)
        {
            try
            {
                // Validate
                ServiceValidator svc = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

                using (DdOlExtended Context = new DdOlExtended())
                {

                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, svc.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.VT, sysvt))
                        throw new Exception("No Permission to VT");
                }
                ContractExtensionDao ced = new ContractExtensionDao();
                return ced.getAngebotFromVertrag(sysvt, contractext, svc.SysPEROLE, svc.SYSPUSER);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.getAngebotFromVertragFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.getAngebotFromVertragFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.getAngebotFromVertragFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        /// <summary>
        /// load MA
        /// </summary>
        /// <param name="sysvt"></param>
        /// <returns></returns>
        public MitantragstellerDto[] DeliverMitantragstellerFromVt(long sysvt)
        {
            // Validate
            ServiceValidator svc = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            svc.ValidateView();

            // Create the endorsers list
            List<MitantragstellerDto> Endorsers = new List<MitantragstellerDto>();

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(context, svc.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.VT, sysvt))
                        throw new Exception("No Permission to VT");

                    ContractExtensionDao ced = new ContractExtensionDao();

                    //Mitantragsteller
                    Endorsers = context.ExecuteStoreQuery<MitantragstellerDto>("select vtobsich.bezeichnung, sichtyp.syssichtyp as syssichtyp, vtobsich.beginn, vtobsich.ende, vtobsich.aktivflag aktivz, vtobsich.rang, vtobsich.sysmh as sysit from sichtyp,vtobsich where sichtyp.rang in (10,80) and vtobsich.rang>=100 and vtobsich.rang<105 and vtobsich.aktivflag=1 and vtobsich.sysvt=" + sysvt, null).ToList();

                    if (Endorsers != null)
                    {

                        //Für jede alte sicherheit einen neuen it anlegen:
                        foreach (MitantragstellerDto ma in Endorsers)
                        {
                            long sysitma = ced.createItFromPerson(ma.SYSIT, context, svc.SysPEROLE, svc.SYSPUSER, sysvt);
                            ma.SYSIT = sysitma; //neuer sysit

                        }


                    }
                }


            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitantragstellerFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitantragstellerFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitantragstellerFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }

            return Endorsers.ToArray();
        }

        /// <summary>
        /// Returns the id of previous contracts for the offer
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        public String[] getVorvertraege(long sysangebot)
        {
            // Validate
            ServiceValidator validator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            validator.ValidateView();
            try
            {
                using (DdOlExtended Context = new DdOlExtended())
                {

                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, validator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysangebot))
                        throw new Exception("No Permission to ANGEBOT");
                }
                ContractExtensionDao ced = new ContractExtensionDao();
                return ced.getVorvertraege(sysangebot, validator.SYSPUSER);
            }
            catch (Exception e)
            {
                throw new Exception("Could not load previous contract ids", e);
            }
        }

        /// <summary>
        /// Submits an extension offer
        /// </summary>
        /// <param name="einreichungDto"></param>
        /// <returns></returns>
        public SubmitStatus SubmitExtension(EinreichungDto einreichungDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            ServiceValidator.ValidateEdit();
            try
            {

                ContractExtensionDao ced = new ContractExtensionDao();
                if (!ced.validateSubmit(einreichungDto.SYSANGEBOT))
                    return SubmitStatus.MAXMVZEXCEEDED;
                if (!ced.validateExtensionMonth(einreichungDto.SYSANGEBOT))
                    return SubmitStatus.EXTENSIONONLYENDMONTH;

                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {

                 
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, einreichungDto.SYSANGEBOT))
                            throw new Exception("No Permission to ANGEBOT");

                    long isValid = ANGEBOTAssembler.getPRODUCTValidFromAngebot(einreichungDto.SYSANGEBOT, Context);
                    if (isValid == 0)
                        return SubmitStatus.PRODUCTEXPIRED;

                    // Check if the status is valid
                    if (!ZustandHelper.VerifyAngebotStatus(einreichungDto.SYSANGEBOT, Context, AngebotZustand.Kalkuliert, AngebotZustand.Gedruckt))
                    {
                        // Throw an exception
                        throw new Exception("Invalid angebot status for Submit().");
                    }


                    //EinreichungsDaten
                    var CurrentAngebot = (from Angebot in Context.ANGEBOT
                                          where Angebot.SYSID == einreichungDto.SYSANGEBOT
                                          select Angebot).FirstOrDefault();

                    // Check if ANGEBOT was found
                    if (CurrentAngebot == null)
                    {
                        // Throw an exception
                        throw new Exception("Specified angebot could not be found.");
                    }
                    //Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, CurrentAngebot);
                    // Write in Angebot
                    if (CurrentAngebot.SYSVART == 500)
                        return SubmitStatus.SERVICEEXTENSIONNOTSUPPORTED;

                    CurrentAngebot.GUELTIGBIS = (DateTime.Today).Date.AddDays(90);                


                    SubmitStatus s = updateMandat(einreichungDto, Context);
                    if (s != SubmitStatus.OK)
                        return s;


                    // Change the status
                    // Versicherung (sysvart==700), Service (sysvart == 500)
                    if (CurrentAngebot.SYSVART == 700)
                        ZustandHelper.SetAngebotStatus(CurrentAngebot,einreichungDto.SYSANGEBOT, Context, AngebotZustand.Eingereicht);
                    else
                        ZustandHelper.SetAngebotStatus(CurrentAngebot,einreichungDto.SYSANGEBOT, Context, AngebotZustand.Einreichen);


                    try
                    {
                        // Save the changes
                        Context.SaveChanges();
                    }
                    catch (Exception exception)
                    {
                        // Throw an exception
                        throw new Exception("The angebot could not be saved.", exception);
                    }

                }
            }
            catch (System.Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.AngebotSubmitFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
            return SubmitStatus.OK;
        }

        #region SEPA
        /// <summary>
        /// Returns a descriptor for the GUI to prevalidate/layout the iban depending on country-code
        /// </summary>
        /// <returns></returns>
        public List<IBANInfoDto> getIBANInformation()
        {
            IBANValidator iban = new IBANValidator();
            return iban.getIBANInformation();
        }

        ///<summary>
        /// Validates IBAN
        /// Extracts BLZ from IBAN
        /// Searches Bankname from BLZ
        /// Validates bic to blz
        /// </summary>
        /// <param name="iban"></param>
        /// <param name="bic"></param>
        /// <returns></returns>
        public IBANValidationError checkIBANandBIC(String iban, String bic)
        {
            IBANValidator ibanV = new IBANValidator();
            return ibanV.checkIBANandBIC(iban, bic);
        }

        #endregion
    }
}