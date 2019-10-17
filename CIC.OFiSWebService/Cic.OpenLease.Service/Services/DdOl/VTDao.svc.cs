// OWNER BK, 27-04-2009
namespace Cic.OpenLease.Service.DdOl
{
    #region Using
    using Cic.OpenLease.Service.Services.DdOl;
    using Cic.OpenLease.Service.Services.DdOl.BO;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Config;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    #endregion

    /// <summary>
    /// Datenzugriffsobjekt für Verträge
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public sealed partial class VTDao : Cic.OpenLease.ServiceAccess.DdOl.IVTDao
    {
        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        private static CacheDictionary<String, int> searchCache = CacheFactory<String, int>.getInstance().createCache(1000*30);
        private static CacheDictionary<String, int> countCache = CacheFactory<String, int>.getInstance().createCache(1000 * 30);
        private static CacheDictionary<String, List<long>> resultIdCache = CacheFactory<String, List<long>>.getInstance().createCache(1000 * 30);
        #region IVTDao methods


        private class ITVTSearch
        {
            public String NAME { get; set; }
            public String VORNAME { get; set; }
            public String ORT { get; set; }
            public String PLZ { get; set; }
            public String ZUSATZ { get; set; }
        }
        private class ANTOBVTSearch
        {
            public long SYSOB { get; set; }
            public String FABRIKAT { get; set; }
            public String HERSTELLER { get; set; }
            public long? JAHRESKM { get; set; }
            public String KFZ { get; set; }
        }
        private class ANTKALKVTSearch
        {
            public decimal? BGEXTERN { get; set; }
            public decimal? DEPOT { get; set; }
            public int? LZ { get; set; }
            public decimal? RATE { get; set; }
            public decimal? RW { get; set; }
            public decimal? SZ { get; set; }
            public decimal? ANZAHLUNG { get; set; }
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
            return "";
        }

        /// <summary>
        /// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
        /// Das Gesichtsfeld kann berücksichtigt werden.
        /// Mit Benutzer Validierung (RFG.SEHEN erforderlich).
        /// MK
        /// </summary>
        /// <param name="vtSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.VTSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <param name="vtSortData">Sortparametern <see cref="Cic.OpenLease.ServiceAccess.DdOl.VTSortData" />.</param>
        /// <returns>List von <see cref="Cic.OpenLease.ServiceAccess.DdOl.VTDto"/></returns>
        public SearchResult<VTShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.VTSortData[] vtSortData)
        {


            // Check search data
            if (vtSearchData == null)
            {
                // Throw exception
                throw new ArgumentException("vtSearchData");
            }

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Check parameters
                Cic.OpenLease.Service.Helpers.ServiceParametersHelper.CheckTopParameter(searchParameters.Top);
                Cic.OpenLease.Service.Helpers.ServiceParametersHelper.CheckSkipParameter(searchParameters.Skip);
            }
            catch
            {
                // Throw caught exception
                throw;
            }

            try
            {
                List<VTShortDto> VTList = null;
                long resCount = 0;
                SearchResult<VTShortDto> result = new SearchResult<VTShortDto>();
                using (DdOlExtended context = new DdOlExtended())
                {
                    try
                    {
                        // Get raw list
                        VTList = MyDeliverVTList(context, vtSearchData, searchParameters, vtSortData, ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER, out resCount);
                        result.results = VTList;
                        result.searchCountMax = (int)resCount;
                        result.searchCountFiltered = VTList.Count;
                    }
                    catch
                    {
                        // Throw caught exception
                        throw;
                    }

                    // Check list
                    if ((VTList == null) || (VTList.Count == 0))
                        return result;


                    // Loop through list
                    foreach (VTShortDto LoopVT in VTList)
                    {

                        // Check if VT is linked to Bonitaet and BoniposList is not null
                        string query = "select count(*) from bonitaet,bonipos where bonipos.sysbonitaet=bonitaet.sysbonitaet and sysvt=" + LoopVT.VTSYSID;
                        long hasAuflage = context.ExecuteStoreQuery<long>(query, null).FirstOrDefault();
                        if (hasAuflage > 0)
                            LoopVT.AUFLAGEN = "JA";
                        else
                            LoopVT.AUFLAGEN = "NEIN";
                        LoopVT.VTBEGINN = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(LoopVT.VTBEGINN);
                        LoopVT.VTENDE = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(LoopVT.VTENDE);


                        //SYSID VTSYSID, SYSKD VTSYSKD, BEGINN VTBEGINN, ENDE VTENDE, VART VTVART, VERTRAG VTVERTRAG, ZUSTAND VTZUSTAND, SYSBERATADDB, SYSVART
                        ITVTSearch itinfo = context.ExecuteStoreQuery<ITVTSearch>("Select NAME, VORNAME, ORT, PLZ, ZUSATZ from cic.person where sysperson=" + LoopVT.VTSYSKD, null).FirstOrDefault();
                        if (itinfo != null)
                        {
                            LoopVT.PERSONNAME = itinfo.NAME;
                            LoopVT.PERSONVORNAME = itinfo.VORNAME;
                            LoopVT.PERSONORT = itinfo.ORT;
                            LoopVT.PERSONPLZ = itinfo.PLZ;
                            LoopVT.PERSONZUSATZ = itinfo.ZUSATZ;
                        }
                        //Bitte hier für die Ermittlung des Verkäufers VT:SYSVK verwenden. Dieser verweist auf die sysPerson des Verkäufers.
                        itinfo = context.ExecuteStoreQuery<ITVTSearch>("Select NAME, VORNAME, ORT, PLZ, ZUSATZ from cic.person, cic.vt where vt.SYSVK =person.sysperson and vt.sysid=" + LoopVT.VTSYSID, null).FirstOrDefault();
                        if (itinfo != null)
                        {
                            LoopVT.VERKAEUFERNAME = itinfo.NAME;
                            LoopVT.VERKAEUFERVORNAME = itinfo.VORNAME;

                        }
                        //LoopVT.KDTYPNAME = context.ExecuteStoreQuery<String>("select kdtyp.name from kdtyp,person where kdtyp.syskdtyp=person.syskdtyp and person.sysperson = " + LoopVT.VTSYSKD, null).FirstOrDefault();
                        LoopVT.KDTYPNAME = "";
                        decimal TaxRate = LsAddHelper.GetTaxRate(context, (long)LoopVT.SYSVART);
                        LoopVT.KALKSZBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(LoopVT.KALKSZ.GetValueOrDefault(), TaxRate);


                        ANTOBVTSearch antob = context.ExecuteStoreQuery<ANTOBVTSearch>("Select SYSOB, BEZEICHNUNG FABRIKAT, HERSTELLER, JAHRESKM, kennzeichen KFZ FROM OB where sysvt=" + LoopVT.VTSYSID, null).FirstOrDefault();
                        if (antob != null)
                        {
                            LoopVT.OBFABRIKAT = antob.FABRIKAT;
                            LoopVT.OBHERSTELLER = antob.HERSTELLER;
                            LoopVT.OBHALTERKFZ = antob.KFZ;
                            if (LoopVT.OBHALTERKFZ == null)
                                LoopVT.OBHALTERKFZ = "";
                            LoopVT.OBJAHRESKM = antob.JAHRESKM;
                            ANTKALKVTSearch antkalk = context.ExecuteStoreQuery<ANTKALKVTSearch>("Select BGEXTERN,DEPOT,LZ,RATE,RW,SZ,ANZAHLUNG FROM KALK where SYSKALK=" + antob.SYSOB, null).FirstOrDefault();
                            if (antkalk != null)
                            {
                                LoopVT.KALKBGEXTERN = antkalk.BGEXTERN;
                                LoopVT.KALKDEPOT = antkalk.DEPOT;
                                LoopVT.KALKANZAHLUNG = antkalk.ANZAHLUNG;
                                LoopVT.KALKLZ = antkalk.LZ;
                                LoopVT.KALKRATE = antkalk.RATE;
                                LoopVT.KALKRW = antkalk.RW;
                                LoopVT.KALKSZ = antkalk.SZ;
                            }
                        }



                    }

                }
                return result;
            }
            catch
            {
                // Throw caught exception
                throw;
            }


        }

        public bool isExtendable(long sysId)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {

                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {


                    ExtendableValidation extval = Context.ExecuteStoreQuery<ExtendableValidation>("select vt.aktivkz, kalk.rwbase restwert, ob.schwacke,kalk.syskalktyp, vt.ende from vt,ob,kalk where kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + sysId, null).FirstOrDefault();

                    if (extval.AKTIVKZ != 1)
                    {
                        return false;
                    }
                    if (!extval.syskalktyp.HasValue)
                    {
                        _Log.Info("Contract " + sysId + " has no valid syskalktyp! Extension not possible");
                        return false;
                    }
                    if (extval.SCHWACKE == null || extval.SCHWACKE.Length == 0)
                    {
                        _Log.Info("Contract " + sysId + " has no valid SCHWACKE! Extension not possible");
                        return false;
                    }
                    if (!extval.ende.HasValue || extval.ende.Value.Year < 2000)
                    {
                        _Log.Info("Contract " + sysId + " has no valid ENDE! Extension not possible");
                        return false;
                    }
                    long[] kredit = new long[] { 39, 41, 40, 50 };

                    if (extval.RESTWERT < 1 && !kredit.Contains(extval.syskalktyp.Value))
                    {
                        _Log.Info("Contract " + sysId + " has no residual value for leasing! Extension not possible");
                        return false;
                    }

                    /* DateTime ende = extval.ende.HasValue ? extval.ende.Value : DateTime.Now;
                     TimeSpan diff = ende.Subtract(DateTime.Now);
                     int months = diff.Days / 30;//monate bis ende*/
                    //if (months <= 3) return false;
                    if (extval.syskalktyp == 42 && !ServiceValidator.MembershipUserValidationInfo.IsInternalMitarbeiter)
                        return false;
                    //if (months <= 5 && !ServiceValidator.MembershipUserValidationInfo.IsInternalMitarbeiter)
                    //     return false;
                    return true;

                }
            }
            catch (Exception exception)
            {

                // Log the exception
                _Log.Error("Contract extension validation check failed", exception);

                // Throw the exception
                throw exception;
            }
        }
        public Cic.OpenLease.ServiceAccess.DdOl.VTDto Deliver(long sysId)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Resolve the problem with includes
                //Devart.Data.Oracle.Entity.OracleEntityProviderServices.TypedNulls = true;

                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {

                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.VT, sysId))
                        throw new Exception("No Permission to VT");
                    // Query VT
                    var CurrentVT = (from Vertrag in Context.VT
                                     where Vertrag.SYSID == sysId
                                     select Vertrag).FirstOrDefault();

                    // Check if antrag was found
                    if (CurrentVT == null)
                    {
                        // Throw an exception
                        throw new Exception("Could not get data from Vt.");
                    }


                    if (!Context.Entry(CurrentVT).Collection(f => f.OBList).IsLoaded)
                        Context.Entry(CurrentVT).Collection(f => f.OBList).Load();
                     

                    // Get antob
                    OB CurrentOb = CurrentVT.OBList.FirstOrDefault();

                    // Check if antob was found
                    if (CurrentOb == null)
                    {
                        // Throw an exception
                        throw new Exception("Could not get data from AntOb.");
                    }

                    if (CurrentOb.KALK == null)
                        Context.Entry(CurrentOb).Reference(f => f.KALK).Load();
 

                    // Get antob
                    KALK CurrentKalk = CurrentOb.KALK;

                    // Check if antob was found
                    if (CurrentKalk == null)
                    {
                        // Throw an exception
                         _Log.Error("Could not get data from KALK for VT="+sysId);
                    }


                    // Query KD
                    var CurrentPerson = (from Person in Context.PERSON
                                         where Person.SYSPERSON == CurrentVT.SYSKD
                                         select Person).FirstOrDefault();

                    if (CurrentPerson == null)
                    {
                        CurrentPerson = new PERSON();
                    }
                    else
                    {
                        CurrentPerson.SYSPUSER = CurrentVT.SYSKD;
                    }

                    ItInfo itInfo = Context.ExecuteStoreQuery<ItInfo>("select nationalitaet,GESCHLECHT,titel2 as titel, rechtsform, hobby.fluchtausweis,hobby.familienstand from person, hobby where hobby.syshobby=person.sysperson and sysperson=" + CurrentPerson.SYSPERSON, null).FirstOrDefault();
                    if(itInfo!=null)
                        CurrentPerson.SYSLANDNAT = ContractExtensionDao.getSYSLANDNAT(itInfo.NATIONALITAET);

                    // Create an assembler
                    VTAssembler Assembler = new VTAssembler(ServiceValidator.SysPEROLE);

                    KDTYP kdtyp = MyDeliverSYSKDTYP(Context, CurrentVT.SYSKD);
                    Cic.OpenLease.ServiceAccess.DdOl.VTDto rval = Assembler.ConvertToDto(CurrentVT, CurrentKalk, CurrentOb, CurrentPerson, kdtyp, Context);

                    rval.AntObSich = Context.ExecuteStoreQuery<AntObSichDto>("select trim(person.name)||' '||trim(person.vorname)||' '||to_char(person.gebdatum,'dd.MM.yyyy') PersonNameVornameGebDatum from vtobsich,person where person.sysperson=vtobsich.sysmh and bezeichnung='Mitantragsteller' and vtobsich.sysvt=" + sysId, null).ToArray();
                    

                    //#6266
                    rval.ObAbNahmeKm = Context.ExecuteStoreQuery<long>("select ob.ubnahmekm from vt,ob where ob.sysvt=vt.sysid and vt.sysid =" + sysId, null).FirstOrDefault();
                    
                    rval.SERIE = Context.ExecuteStoreQuery<String>("select ob.serie from vt,ob where ob.sysvt=vt.sysid and vt.sysid =" + sysId, null).FirstOrDefault();

                    String mandant = CurrentVT.VERTRIEBSWEG.Trim();
                    if(mandant!=null && mandant.IndexOf(' ')>-1)
                        mandant = mandant.Substring(0, mandant.IndexOf(' ')).Trim();
                    rval.SYSBRAND = Context.ExecuteStoreQuery<long>("select sysbrand from brand where mandant='" + mandant + "'", null).FirstOrDefault();
                    if (CurrentOb.SCHWACKE!=null)
                        rval.SCHWACKE = trim(CurrentOb.SCHWACKE);
                    rval.BAUREIHE = Context.ExecuteStoreQuery<String>("select sklasse from schwacke where schwacke='"+rval.SCHWACKE+"'", null).FirstOrDefault();

                    List<Devart.Data.Oracle.OracleParameter>  parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.SysId});
                    int zinstyp= Context.ExecuteStoreQuery<int>("select vtobsl.variabel from vtobsl where vtobsl.sysvt = :sysvt and vtobsl.syssltyp in (select syssltyp from sltyp where renditeflag = 1) and vtobsl.inaktiv = 0", parameters.ToArray()).FirstOrDefault();
                    rval.ZINSTYP = zinstyp == 0 ? "fix" : "variabel";

                    //Fix #6161
                    Cic.OpenLease.ServiceAccess.DdOl.BRANDDto[] userBrands = Cic.OpenLease.Common.MembershipProvider.listBrands(ServiceValidator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);

                   
                    String hersteller = "";
                    if (userBrands != null && userBrands.Count() > 1)
                    {
                        VehicleDao vd = new VehicleDao();
                        OBTYPDto obInfo = vd.MyDeliverObTyp(Context, rval.SCHWACKE);
						if(obInfo!=null)
						{
							long sysobtyp = Context.ExecuteStoreQuery<long>("select sysobtyp from obtyp where schwacke='" + rval.SCHWACKE + "'", null).FirstOrDefault();
							hersteller = vd.MyDeliverHersteller(Context, sysobtyp, obInfo.DESCRIPTION);


							/*foreach (BRANDDto br in userBrands)
							{
								string[] showonly = null;
								string[] hide = null;
								BRAND brand = BRANDHelper.DeliverBRAND(Context, br.SYSBRAND.Value);
								if (brand.NAME.ToLower().IndexOf("bmw") > -1)
								{
									showonly = new string[2];
									showonly[0] = "bmw";
									showonly[1] = "mini";
								}
								else if (brand.NAME.ToLower().IndexOf("alphera") > -1)
								{
									hide = new string[2];
									hide[0] = "bmw";
									hide[1] = "mini";
								}


								if (showonly != null)
								{
									if (hersteller.ToLower().IndexOf(showonly[0]) < 0 && hersteller.ToLower().IndexOf(showonly[1]) < 0)
									{
										continue;//wrong brand
									}
								}
								else if (hide != null)
								{
									if (hersteller.ToLower().IndexOf(hide[0]) > -1 || hersteller.ToLower().IndexOf(hide[1]) > -1)
									{
										continue;//wrong brand
									}
								}
								rval.SYSBRAND = br.SYSBRAND.Value;
								break;
							}*/
						}
                    }

                    /*VTObligoDto obligo = Context.ExecuteStoreQuery<VTObligoDto>("select RISIKOGR1,RISIKOGR2,RISIKOGR3,RISIKOGR4,RISIKOGR5,RISIKOGR6,RISIKOGR7, OP, STAND from vtobligo where sysvtobligo=" + sysId, null).FirstOrDefault();
                    if (obligo != null)
                    {

                        int anzaufl = 4;

                        DateTime ende = CurrentVT.ENDE.HasValue ? CurrentVT.ENDE.Value : DateTime.Now;
                        //wenn nutzenleasing und nicht interner mitarbeiter:
                        //innerhalb anzBis keine Auflösewerte mehr darstellen
                        long syskalktyp = Context.ExecuteStoreQuery<long>("select kalktyp.syskalktyp from vt,ob,kalk,kalktyp where kalktyp.syskalktyp=kalk.syskalktyp and kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + sysId, null).FirstOrDefault();
                        if (!ServiceValidator.MembershipUserValidationInfo.IsInternalMitarbeiter)
                        {
                            if (syskalktyp == 42)
                            {
                                decimal anzBis = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_AUFLOESEANZEIGEBIS);

                                DateTime today = DateTime.Now;
                                int months = 0;
                                while (today.CompareTo(ende) < 0)
                                {
                                    today = today.AddMonths(1);
                                    months++;
                                }

                                //TimeSpan diff = ende.Subtract(DateTime.Now);
                                //int months = diff.Days / 30;//monate bis ende
                                if (months <= anzBis)
                                    anzaufl = 0;
                                //FIX for BMWABASIS-975-CR
                                anzaufl = months - (int)anzBis;
                                if (anzaufl > 4) anzaufl = 4;
                                if (anzaufl < 0) anzaufl = 0;
                                
                            }
                        }

                        decimal TaxRate = LsAddHelper.GetTaxRate(Context, 100);
                        if (syskalktyp == 50 || syskalktyp == 40 || syskalktyp == 39 || syskalktyp == 41)
                            TaxRate = LsAddHelper.GetTaxRate(Context, 200);//Kredit

                        rval.AUFLOESEWERTE = new VTRueckDto[anzaufl];
                        for (int i = 0; i < anzaufl; i++)
                        {
                            rval.AUFLOESEWERTE[i] = new VTRueckDto();
                            rval.AUFLOESEWERTE[i].datum = DateTimeHelper.getUltimoN(obligo.STAND, i);
                            if (i == 0)
                                rval.AUFLOESEWERTE[i].betrag = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(obligo.RISIKOGR4, TaxRate));
                            else if (i == 1)
                                rval.AUFLOESEWERTE[i].betrag = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(obligo.RISIKOGR5, TaxRate));
                            else if (i == 2)
                                rval.AUFLOESEWERTE[i].betrag = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(obligo.RISIKOGR6, TaxRate));
                            else if (i == 3)
                                rval.AUFLOESEWERTE[i].betrag = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(obligo.RISIKOGR7, TaxRate));
                        }

                        rval.BONUSFF = new BonusDto();
                        rval.BONUSFF.betrag = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundCosting(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(obligo.RISIKOGR1, TaxRate)); 
                        rval.BONUSFF.datum = DateTimeHelper.getUltimo(obligo.STAND);
                        rval.OPOS = new OposDto();
                        rval.OPOS.datum = obligo.STAND;
                        rval.OPOS.betrag = obligo.OP;
                        rval.EUROTAXBLAU = obligo.RISIKOGR2.GetValueOrDefault();
                        rval.EUROTAXGELB = obligo.RISIKOGR3.GetValueOrDefault();
                        rval.EUROTAXVALID = obligo.RISIKOGR3.HasValue && obligo.RISIKOGR2.HasValue;

                        rval.EUROTAXMITTE = (rval.EUROTAXBLAU + rval.EUROTAXGELB) / 2;
                    }*/
                    if (CurrentVT.SYSVART.HasValue)
                        rval.VART = Context.ExecuteStoreQuery<String>("select bezeichnung from vart where sysvart=" + CurrentVT.SYSVART.Value, null).FirstOrDefault();
                    
                   

                    return rval;
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                System.Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.VertragDeliverFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }
        #region Depracated
        /*
        /// <summary>
        /// Die mthode liefert ein Buchwert PDF dokument.
        /// MK
        /// </summary>
        /// <param name="sysVt">Vertrags Id</param>
        /// <param name="language">Vertrags Id</param>
        /// <param name="generateThumbnails">Vertrags Id</param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.VTAssetsValueDto"/></returns>        
        public Cic.OpenLease.ServiceAccess.DdOl.VTAssetsValueDto DeliverAssetValueDocument(long sysVt, string language, bool generateThumbnails)
        {
            Cic.OpenLease.ServiceAccess.Merge.OlClient.HotInputDto HotInputDto;
            Cic.OpenLease.ServiceAccess.Merge.OlClient.HotOutputDto HotOutputDto = null;
            Cic.OpenLease.Client.Executor OlClientExecutor;
            Cic.OpenLease.ServiceAccess.DdOl.VTAssetsValueDto VTAssetsValueDto = null;

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            // Prepare EAIHOT for WEBOFFICE_ORDER
            HotInputDto = new Cic.OpenLease.ServiceAccess.Merge.OlClient.HotInputDto();
            HotInputDto.EAIHOT = Cic.OpenLease.Model.DdOw.EAIHOTHelper.InstanciateEaiHot("SOAP_HOLEBUCHWERTE", "VT", sysVt, language, ServiceValidator.SYSPUSER, ServiceValidator.SYSWFUSER);

            OlClientExecutor = new Cic.OpenLease.Client.Executor();
            HotOutputDto = new Cic.OpenLease.ServiceAccess.Merge.OlClient.HotOutputDto();
            
            try
            {
                HotOutputDto.EAIHOT = OlClientExecutor.Execute(HotInputDto.EAIHOT);
                
                if (HotOutputDto.EAIHOT != null)
                {
                    VTAssetsValueDto = new Cic.OpenLease.ServiceAccess.DdOl.VTAssetsValueDto();

                    if (HotOutputDto.EAIHOT.EAIHFILEList.Count > 0)
                    {
                        VTAssetsValueDto.PdfDocument = HotOutputDto.EAIHOT.EAIHFILEList.FirstOrDefault<Cic.OpenLease.Model.DdOw.EAIHFILE>().EAIHFILE1;
                    }

                    if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(HotOutputDto.EAIHOT.OUTPUTPARAMETER1))
                    {
                        System.DateTime Date;
                        System.DateTime.TryParse(HotOutputDto.EAIHOT.OUTPUTPARAMETER1, out Date);
                        VTAssetsValueDto.Date = Date;
                    }
                   
                    if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(HotOutputDto.EAIHOT.OUTPUTPARAMETER2))
                    {
                        double Value;
                        double.TryParse(HotOutputDto.EAIHOT.OUTPUTPARAMETER2, out Value);
                        VTAssetsValueDto.Value = Value;
                    }
                }
                
            }
            catch
            {
                throw;
            }

            return VTAssetsValueDto;
        }

		/// <summary>
		/// Die methode liefert den Jahresumsatz der angemeldeten Rolle über das Gesichtsfeld.
		/// </summary>
		/// <returns>double</returns>
		public decimal DeliverAnnualSales(int year)
		{
			decimal AnnualSales = 0.0M;

			System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.VT> Query;

			// Validate
			ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			ServiceValidator.ValidateView();

			try
			{
				using (DdOlExtended context = new DdOlExtended())
				{
					try
					{
						// Create query
						Query = context.CreateQuery<Cic.OpenLease.Model.DdOl.VT>(Cic.Basic.Data.Objects.ContextHelper.GetQualifiedEntitySetName(context, typeof(Cic.OpenLease.Model.DdOl.VT)));
					}
					catch
					{
						// Throw caught exception
						throw;
					}

					// Check query
					if (Query != null)
					{
						// Sight fields narrowing
						
						System.Collections.Generic.List<long> SightFieldIds = Cic.OpenLease.Model.DdOl.PEUNIHelper.DeliverSightFieldIds(context, ServiceValidator.SysPEROLE, Cic.OpenLease.Model.DdOl.PEUNIHelper.Areas.VT);

					    // narrow
						Query = Query.Where(Cic.Basic.Data.Objects.EntityFrameworkHelper.BuildContainsExpression<Cic.OpenLease.Model.DdOl.VT, long>(vt => vt.SYSID, SightFieldIds));
						

						// AKTIVKZ
						Query = Query.Where(vt => ((vt.AKTIVKZ != null) && (vt.AKTIVKZ != 0)));
						// DATAKTIV
						Query = Query.Where(vt => ((vt.DATAKTIV != null) && (vt.DATAKTIV.Value.Year == year)));
						// BGEXTERN
						Query = Query.Where(vt => ((vt.BGEXTERN != null) && (vt.BGEXTERN > 0.0M)));

						try
						{
							// Select
							AnnualSales = Query.Select(vt => vt.BGEXTERN.Value).Sum();
						}
						catch
						{
							throw;
						}
					}
				}
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return AnnualSales;
		}
        */
        #endregion
        private static String trim(String str)
        {
            if (str == null) return null;
            String rval = str.Trim();
            if (rval.Length == 0)
                return null;
            return rval;
        }
        #endregion

        #region My methods
        private static string MyCreateQuery(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData, long? sysPERSONInPEROLE, long? sysPUSER, bool count)
        {
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentException("context");
            }

            // Check search data
            if (vtSearchData == null)
            {
                // Throw exception
                throw new ArgumentException("vtSearchData");
            }

            if (count)
            {
                QueryBuilder.Append("SELECT /*+ RULE */ COUNT(*) FROM CIC.VT,CIC.OB WHERE OB.sysvt=vt.sysid ");
            }
            else
            {
                //QueryBuilder.Append("SELECT  /*+ RULE */ SYSID VTSYSID, SYSKD VTSYSKD, BEGINN VTBEGINN, ENDE VTENDE, VART VTVART, VERTRAG VTVERTRAG, ZUSTAND VTZUSTAND, SYSBERATADDB, SYSVART FROM CIC.VT WHERE 1 = 1 ");
                QueryBuilder.Append("SELECT   vt.SYSID  FROM CIC.VT,CIC.OB,cic.person vk WHERE  OB.sysvt=vt.sysid and vt.SYSVK =vk.sysperson(+)");
            }
            // VERTRAG
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.VERTRAG))
            {
                // Where
                QueryBuilder.Append("AND UPPER(VERTRAG) LIKE UPPER(:pVERTRAG) ");
            }

            //SYSID

            // SYSIT
            if ((vtSearchData.SYSIT.HasValue) && (vtSearchData.SYSIT.Value > 0))
            {
                // Where

                QueryBuilder.Append("AND SYSKD IN (SELECT DISTINCT SYSPERSON FROM IT WHERE SYSIT=:pSYSIT) ");
            }


            // CustomerName
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Person))
            {
                //Create query of id's
                string QueryFilteredIdsByPERSON = MyDeliverFilteredSYSIDsByPERSON(context, vtSearchData);

                //Add values to query
                QueryBuilder.Append("AND SYSID IN (" + QueryFilteredIdsByPERSON + ")");

            }

            // VerkaeuferName
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Verkaeufer))
            {
                //Create query of id's
                //string QueryFilteredIdsByVERKAEUFER = MyDeliverFilteredSYSIDsByVERKAEUFER(context, vtSearchData);

                //Add values to query
                //QueryBuilder.Append("AND SYSID IN (" + QueryFilteredIdsByVERKAEUFER + ")");
                QueryBuilder.Append("AND UPPER(vk.name||' '||vk.vorname) LIKE UPPER(:pVERKAEUFER) ");

            }

            // Deliver filtered Ids query by OBhalter if there are search values
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Obhalter))
            {
                

                //Add subquery
                QueryBuilder.Append(" AND UPPER(trim(OB.KENNZEICHEN)) LIKE UPPER(:pOBHALTER) ");
            }


            // Sight fields narrowing
            if (sysPERSONInPEROLE.HasValue)
            {
                // Narrow
                QueryBuilder.Append("AND SYSID IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", 'VT',sysdate))) ");
                //QueryBuilder.Append("AND exists (select cic.CIC_PEROLE_UTILS.ChkObjInPEUNI(" + sysPUSER + ", 'VT', SYSID) from dual) ");
            }

            //Get filter
            string Filter;
            Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "VT", string.Empty);
            

            if (Filter != null && Filter.Length > 0)
            {
                QueryBuilder.Append("AND " + Filter);
            }

            // Return
            return QueryBuilder.ToString();
        }

        /*private static string MyDeliverFilteredSYSIDsByOBHalter(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData)
        {
            System.Text.StringBuilder QueryVTBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryOBBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryOBBriefBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryOBHalterBuilder = new System.Text.StringBuilder();
            System.Collections.Generic.List<long> SysIDList = new System.Collections.Generic.List<long>();

            QueryOBHalterBuilder.Append("SELECT SYSOBBRIEF FROM CIC.OBHALTER WHERE 1 = 1 ");
            QueryOBBriefBuilder.Append("SELECT SYSOBBRIEF FROM CIC.OBBRIEF WHERE 1 = 1 ");
            QueryOBBuilder.Append("SELECT SYSVT FROM CIC.OB WHERE 1 = 1 ");
            QueryVTBuilder.Append("SELECT SYSID FROM CIC.VT WHERE 1 = 1 ");

            System.Collections.Generic.List<OBHALTER> list = new System.Collections.Generic.List<OBHALTER>();

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Obhalter))
            {
                // OBHALTER
                QueryOBHalterBuilder.Append("AND UPPER(KFZ) LIKE UPPER(:pOBHALTER)");
            }

            if (QueryOBHalterBuilder.ToString().Trim() != "SELECT * FROM OBHALTER WHERE 1 = 1")
            {
                //OBBrief
                QueryOBBriefBuilder.Append("AND SYSOBBRIEF IN (" + QueryOBHalterBuilder.ToString() + ")");
            }

            if (QueryOBBriefBuilder.ToString().Trim() != "SELECT SYSOBBRIEF FROM CIC.OBBRIEF WHERE 1 = 1")
            {
                //OB
                QueryOBBuilder.Append("AND SYSOB IN(" + QueryOBBriefBuilder.ToString() + ")");
            }

            if (QueryOBBuilder.ToString().Trim() != "SELECT SYSVT FROM CIC.OB WHERE 1 = 1")
            {
                //VT
                QueryVTBuilder.Append("AND SYSID IN (" + QueryOBBuilder.ToString() + ")");
            }

            if (QueryVTBuilder.ToString().Trim() == "SELECT SYSID FROM CIC.VT WHERE 1 = 1")
            {
                QueryVTBuilder = new System.Text.StringBuilder();
            }

            return QueryVTBuilder.ToString();
        }*/

        private static string MyDeliverFilteredSYSIDsByPERSON(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData)
        {
            System.Text.StringBuilder QueryVTBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryPERSONBuilder = new System.Text.StringBuilder();

            System.Collections.Generic.List<long> SysIDList = new System.Collections.Generic.List<long>();

            QueryPERSONBuilder.Append("SELECT /*+ INDEX(PERSON PERSON_XTD_CONCAT) */ DISTINCT SYSPERSON FROM CIC.PERSON WHERE 1 = 1 ");
            QueryVTBuilder.Append("SELECT  DISTINCT SYSID FROM CIC.VT WHERE 1 = 1 ");/*+ PARALLEL(VT) */

            
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Person))
            {
                // PERSON
                QueryPERSONBuilder.Append("AND (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:pPERSON))");
            }

            //VT
            QueryVTBuilder.Append("AND SYSKD IN (" + QueryPERSONBuilder.ToString() + ")");

            //return
            return QueryVTBuilder.ToString();
        }


        private static string MyDeliverFilteredSYSIDsByVERKAEUFER(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData)
        {
            System.Text.StringBuilder QueryVTBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryPERSONBuilder = new System.Text.StringBuilder();

            System.Collections.Generic.List<long> SysIDList = new System.Collections.Generic.List<long>();

            QueryPERSONBuilder.Append("SELECT /*+ INDEX(PERSON PERSON_XTD_CONCAT) */ DISTINCT SYSPERSON FROM CIC.PERSON WHERE 1 = 1 ");
            QueryVTBuilder.Append("SELECT  DISTINCT SYSID FROM CIC.VT WHERE 1 = 1 ");/*+ PARALLEL(VT) */

            
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Verkaeufer))
            {
                // PERSON
                QueryPERSONBuilder.Append("AND (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:pVERKAEUFER))");
            }

            //VT
            //FEHLT IN SYSBERATADDB IN VT
            QueryVTBuilder.Append("AND SYSBERATADDB IN (" + QueryPERSONBuilder.ToString() + ")");
            // QueryVTBuilder.Append("AND SYSKD IN (" + QueryPERSONBuilder.ToString() + ")");

            //return
            return QueryVTBuilder.ToString();
        }

        private long? MyDeliverSYSKDFromSYSID(DdOlExtended context, long SYSID)
        {
            //Get SYSKD FROM SYSID
            string Query = "SELECT SYSKD FROM CIC.VT WHERE SYSID = " + SYSID;
            return context.ExecuteStoreQuery<long>(Query, null).FirstOrDefault<long>();
        }

        private long? MyDeliverVerkaeuferFromSYSID(DdOlExtended context, long SYSID)
        {
            //Get SYSKD FROM SYSID
            string Query = "SELECT SYSBERATADDB FROM CIC.VT WHERE SYSID = " + SYSID;
            return context.ExecuteStoreQuery<long>(Query, null).FirstOrDefault<long>();
        }
       
        private System.Collections.Generic.List<VTShortDto> MyDeliverVTList(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.VTSortData[] vtSortData, long? sysPERSONInPEROLE, long? sysPUSER, out long count)
        {
            string Query;
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();
            System.Collections.Generic.List<VTShortDto> VTList;

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentException("context");
            }

            // Check search data
            if (vtSearchData == null)
            {
                throw new ArgumentException("vtSearchData");
            }

            // Create new list
            VTList = new System.Collections.Generic.List<VTShortDto>();
            object[] Parameters = MyDeliverQueryParameters(vtSearchData);


            //GET Created Query
            Query = MyCreateQuery(context, vtSearchData, sysPERSONInPEROLE, sysPUSER, false);

            QueryBuilder.Append(Query);
            StringBuilder sortBuilder = new StringBuilder();
            if (vtSortData != null && vtSortData.Length > 0)
            {
                // Order
                int i = 0;
                sortBuilder.Append(" ORDER BY ");
                foreach (Cic.OpenLease.ServiceAccess.DdOl.VTSortData sortDataLoop in vtSortData)
                {
                    String sortField = sortDataLoop.SortDataConstant.ToString();
                    sortBuilder.Append(sortField + " " + sortDataLoop.SortOrderConstant.ToString());
                    if (i != vtSortData.Length - 1)
                    {
                        sortBuilder.Append(", ");
                    }
                    i++;
                }
            }
            else
            {
                // Default Order
                sortBuilder.Append(" ORDER BY AKTIVKZ, DATAKTIV, VERTRAG");
            }
            QueryBuilder.Append(sortBuilder.ToString());
            String vtQuery = QueryBuilder.ToString();


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
                String resultIdCacheKey = getParamKey(Parameters, "IT_" + sysPUSER + sortBuilder.ToString());

                if (!resultIdCache.ContainsKey(resultIdCacheKey))
                {
                    _Log.Debug("VT-Search-SQL: " + vtQuery);
                    resultIdCache[resultIdCacheKey] = context.ExecuteStoreQuery<long>(vtQuery, Parameters).ToList();
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

                vtQuery = "SELECT SYSID VTSYSID, SYSKD VTSYSKD, BEGINN VTBEGINN, ENDE VTENDE, VERTRAG VTVERTRAG, case when VT.aktivkz=1 then 'aktiv' else 'inaktiv' end VTZUSTAND, SYSBERATADDB, VT.SYSVART, BENUTZER VERKAEUFERVORNAME, VART.BEZEICHNUNG VTVART FROM CIC.VT, CIC.VART WHERE VT.SYSVART=VART.SYSVART AND sysid in (" + string.Join(",", ids) + ") " + sortBuilder.ToString();
                _Log.Debug("VT-Search-SQL: " + vtQuery);
                VTList = context.ExecuteStoreQuery<VTShortDto>(vtQuery, null).ToList();
                _Log.Debug("VT-Search Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                return VTList;
            }



            // Check top
            if ((searchParameters != null) && (searchParameters.Top > 0))
            {
                Query = "SELECT /*+ RULE */ * FROM (SELECT rownum rnum, a.* FROM(" + QueryBuilder.ToString() + ") a WHERE rownum <= " + (searchParameters.Skip + searchParameters.Top) + ") WHERE rnum > " + searchParameters.Skip;
            }
            _Log.Debug("VT-Search Query: " + Query);
            try
            {
                // Create list


                double starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;

                int resultCount = 0;
                String countKey = getParamKey(Parameters, Query);
                // count when query changed in the last 2 minutes
                if (!searchCache.ContainsKey(countKey))
                {
                    String countQuery = MyCreateQuery(context, vtSearchData, sysPERSONInPEROLE, sysPUSER, true);
                    resultCount = context.ExecuteStoreQuery<int>(countQuery, Parameters).FirstOrDefault();
                    Parameters = MyDeliverQueryParameters(vtSearchData);
                }
                else
                    resultCount = searchCache[countKey];

                VTList = context.ExecuteStoreQuery<VTShortDto>(Query, Parameters).ToList<VTShortDto>();
                _Log.Debug("VT-Search Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                searchCache[countKey] = resultCount; //touch the cache, another cache lifetime duration for no fetching the count
                count = resultCount;
            }
            catch
            {
                throw;
            }


            // Return
            return VTList;
        }
        private static String getParamKey(object[] par, String prefix)
        {
            StringBuilder sb = new StringBuilder(prefix);
            sb.Append(": ");
            foreach (object o in par)
                sb.Append(((Devart.Data.Oracle.OracleParameter)o).Value.ToString());
            return sb.ToString();
        }
        private object[] MyDeliverQueryParameters(Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData)
        {

            System.Collections.Generic.List<Devart.Data.Oracle.OracleParameter> ParametersList = new System.Collections.Generic.List<Devart.Data.Oracle.OracleParameter>();
            // VERTRAG
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.VERTRAG))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pVERTRAG", Value = "%" + vtSearchData.VERTRAG.Trim() + "%" });
            }

            // CustomerName
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Person))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pPERSON", Value = "%" + (vtSearchData.Person.Trim()).Replace(" ", "%") + "%" });

            }

            // Deliver filtered Ids query by OBhalter if there are search values
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Obhalter))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pOBHALTER", Value = "%" + vtSearchData.Obhalter.Trim() + "%" });
            }

            // Deliver filtered Ids query by Verkaeufer if there are search values
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(vtSearchData.Verkaeufer))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pVERKAEUFER", Value = "%" + (vtSearchData.Verkaeufer.Trim()).Replace(" ", "%") + "%" });
            }

            // Deliver filtered Ids query by SYSIT if there are search values
            if ((vtSearchData.SYSIT.HasValue) && (vtSearchData.SYSIT.Value > 0))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSYSIT", Value = vtSearchData.SYSIT });
            }

            return ParametersList.ToArray();
        }

        private KDTYP MyDeliverSYSKDTYP(DdOlExtended context, long? SYSID)
        {
            //Get SYSKD FROM SYSID

            KDTYP result = null;
            if (!SYSID.HasValue)
                return null;

            var query =
                from p in context.PERSON
                where p.SYSPERSON == SYSID
                select p.KDTYP;

            try
            {
                result = query.FirstOrDefault<KDTYP>();
            }
            catch (Exception e)
            {
                throw new ApplicationException("Could not get KDTYP in VT", e); ;
            }

            return result;
        }

        #endregion
    }
    class ExtendableValidation
    {
        public long? syskalktyp { get; set; }
        public DateTime? ende { get; set; }
        public String SCHWACKE { get; set; }
        public decimal RESTWERT { get; set; }
        public int AKTIVKZ { get; set; }
    }
}
