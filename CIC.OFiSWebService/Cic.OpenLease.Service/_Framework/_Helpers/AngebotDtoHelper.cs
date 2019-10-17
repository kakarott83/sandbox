namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.Service.Services.DdOl;
    using Cic.OpenLease.Service.Versicherung;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdEurotax;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using CIC.Database.OW.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    #endregion

    public class AngebotDtoHelper
    {
        private const string CnstEtgTypeImportTableName = "ETGTYPE";
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Methods
        public static void UpdateAngebotSpecialCalcStatus(ANGEBOTDto AngebotDto)
        {
            try
            {
                if (!AngebotDto.SPECIALCALCSTATUS.HasValue || AngebotDto.SPECIALCALCSTATUS == 0)
                    AngebotDto.SPECIALCALCSTATUSTEXT = "keine";
                else if (AngebotDto.SPECIALCALCSTATUS == 1)
                    AngebotDto.SPECIALCALCSTATUSTEXT = "angefordert";
                else if (AngebotDto.SPECIALCALCSTATUS == 2)
                    AngebotDto.SPECIALCALCSTATUSTEXT = "in Bearbeitung";
                else if (AngebotDto.SPECIALCALCSTATUS == 3)
                    AngebotDto.SPECIALCALCSTATUSTEXT = "durchgeführt";

                if (AngebotDto.SPECIALCALCSYSWFUSER > 0)
                {
                    using (DdOlExtended context = new DdOlExtended())
                    {
                        AngebotDto.SPECIALCALCUSER = context.ExecuteStoreQuery<string>("select concat(name,concat(' ',vorname)) name from wfuser where syswfuser=" + AngebotDto.SPECIALCALCSYSWFUSER, null).FirstOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error("Error updating specialcalcstatus", ex);
            }
        }

        /// <summary>
        /// Fills the default values for a new offer
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        public static ANGEBOTDto CreateAngebotDto(OfferConfiguration offer, long sysperole, ANGEBOTDto AngebotDto)
        {
            // Create new angebot
            if(AngebotDto==null)
                AngebotDto = new ANGEBOTDto();
            
            try
            {
                //Avoid SAVE when eingereicht!
                if (AngebotDto.SYSID.HasValue && AngebotDto.SYSID.Value > 0)
                {
                    using (DdOlExtended Context = new DdOlExtended())
                    {
                        // Check if the status is valid
                        if (!ZustandHelper.VerifyAngebotStatus(AngebotDto.SYSID.Value, Context, AngebotZustand.Neu, AngebotZustand.Kalkuliert, AngebotZustand.Gedruckt, AngebotZustand.NeuResubmit))
                        {
                            // Throw an exception
                            throw new Exception("Ungültiger ANGEBOT Status");
                        }
                    }
                }
                AngebotDto.ANGOBINIMOTORTYP = "UNDEFINED";


                // Get the tax rate
                decimal TaxRate = LsAddHelper.getGlobalUst(sysperole);

                //POLSTERCODE, POLSTERTEXT in ANGOB
                AngebotDto.ANGOBPOLSTERCODE = offer.PolsterCode;
                AngebotDto.ANGOBPOLSTERTEXT = offer.PolsterText;

                // Farbea in ANGOB
                AngebotDto.ANGOBFARBEA = offer.Farbea;
                // Set the properties
                AngebotDto.ANGOBCONFIGSOURCE = (Cic.OpenLease.ServiceAccess.OfferTypeConstants)offer.Type;
                AngebotDto.ANGOBCONFIGID = offer.OfferId;
                //AngebotDto.ANGEBOT1 //empty because filled upon save
                
                AngebotDto.ANGKALKSZBRUTTO = 0;
                //AngebotDto.ANGKALKHERZUBRABO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.TotalDiscount);
                AngebotDto.ERFASSUNG = DateTime.Today;
                AngebotDto.SYSOBART = offer.sysobart;
                if (offer.sysobart == 0)
                {
                    using (DdOlExtended context = new DdOlExtended())
                    {
                        String obartname = "Neuwagen";
                        if (offer.Vehicle.AdditionalType == 1)
                            obartname = "Vorführwagen";
                        OBART obart = OBARTHelper.SearchName(context, obartname);
                        if (obart != null)
                            AngebotDto.SYSOBART = obart.SYSOBART;
                    }
                }
                AngebotDto.ANGOBPICTUREURL = offer.ImageUrl;
                AngebotDto.ANGOBHERSTELLER = offer.Vehicle.BrandName;
                AngebotDto.ANGOBFABRIKAT = offer.Vehicle.Name;
                AngebotDto.ANGOBINIKMSTAND = offer.Kilometer;
                AngebotDto.ANGOBINIERSTZUL = offer.Erstzulassung;



                //NOVANEU informativ
                AngebotDto.ANGKALKBGEXTERN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(offer.TotalPrice, TaxRate));
                AngebotDto.ANGKALKBGEXTERNBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(AngebotDto.ANGOBGRUNDBRUTTO.GetValueOrDefault());
                AngebotDto.ANGKALKBGEXTERNUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(TaxRate);
                AngebotDto.ANGKALKGRUNDNETTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(AngebotDto.ANGKALKBGEXTERN.GetValueOrDefault());
                AngebotDto.ANGKALKPAKETENETTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(offer.PackagesPrice, TaxRate));
                AngebotDto.ANGKALKPAKETEUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(AngebotDto.ANGKALKBGEXTERNUST.GetValueOrDefault());

                // Set the options discounts
                AngebotDto.ANGOBGRUNDRABATTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.ModelDiscount);
                AngebotDto.ANGOBSONZUBRABATTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.OptionsDiscount);
                
                AngebotDto.ANGOBAHKRABATTOBRUTTO = 0;
                decimal gesrab = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.TotalDiscount);
                if (gesrab > 0 && (offer.ModelDiscount + offer.OptionsDiscount) == 0)
                {
                    //AngebotDto.ANGOBGRUNDRABATTO = gesrab;
                    //AngebotDto.ANGOBSONZUBRABATTO = 0;
                    AngebotDto.ANGOBAHKRABATTOBRUTTO = gesrab;
                }

                AngebotDto.ANGOBPAKETERABATTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.PackagesDiscount);
                AngebotDto.ANGOBHERZUBRABATTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.OriginalAccessoriesDiscount);
                AngebotDto.ANGOBZUBEHOERRABATTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.DealerAccessoriesDiscount + offer.MiscellaneousOptionsDiscount);

                // Set the options prices
                AngebotDto.ANGOBGRUNDBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.Vehicle.Price);
                //nettonetto
                AngebotDto.ANGOBGRUNDEXKLN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(offer.Vehicle.Price, TaxRate));
                //incl Nova!
                AngebotDto.ANGOBSONZUBRV = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.Vehicle.SARVPriceNetNoNova);
                AngebotDto.ANGOBSONZUBUSER = 0;
                AngebotDto.ANGOBSONZUBDEFAULT = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.OptionsPrice);
                AngebotDto.ANGOBSONZUBBRUTTO = AngebotDto.ANGOBSONZUBDEFAULT;
                AngebotDto.ANGOBPAKETEBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.PackagesPrice);

               
                AngebotDto.ANGOBZULASSUNGBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.ZulassungBrutto);
                AngebotDto.ANGOBUEBERFUEHRUNGBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.UeberfuehrungBrutto);

                //rabattierfähiger betrag
                AngebotDto.ANGOBGRUNDEXTERN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(AngebotDto.ANGOBPAKETEBRUTTO.Value + AngebotDto.ANGOBSONZUBBRUTTO.Value + AngebotDto.ANGOBGRUNDBRUTTO.Value);


                AngebotDto.ANGOBHERZUBBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.OriginalAccessoriesPrice);
                AngebotDto.ANGKALKZUBEHOERBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(offer.DealerAccessoriesPrice + offer.MiscellaneousOptionsPrice);
                AngebotDto.ANGKALKZUBEHOERNETTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)AngebotDto.ANGKALKZUBEHOERBRUTTO.GetValueOrDefault(), TaxRate));
                AngebotDto.ANGOBZUBEHOERBRUTTO = AngebotDto.ANGKALKZUBEHOERBRUTTO;
                AngebotDto.ANGOBZUBEHOER = AngebotDto.ANGKALKZUBEHOERNETTO;

                AngebotDto.ANGOBSONZUBBRUTTOEXKLNOVA = AngebotDto.ANGOBSONZUBBRUTTO;
                AngebotDto.ANGOBPAKETEBRUTTOEXKLNOVA = AngebotDto.ANGOBPAKETEBRUTTO;
                AngebotDto.ANGOBGRUNDBRUTTOEXKLNOVA = AngebotDto.ANGOBGRUNDBRUTTO;
                if (offer.Nova != 0)
                {
                    AngebotDto.ANGOBSONZUBBRUTTOEXKLNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice((decimal)AngebotDto.ANGOBSONZUBBRUTTO / offer.Nova);
                    AngebotDto.ANGOBPAKETEBRUTTOEXKLNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice((decimal)AngebotDto.ANGOBPAKETEBRUTTO / offer.Nova);
                    AngebotDto.ANGOBGRUNDBRUTTOEXKLNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice((decimal)AngebotDto.ANGOBGRUNDBRUTTO / offer.Nova);
                }

                AngebotDto.ANGOBAHKEXTERNBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(AngebotDto.ANGOBGRUNDEXTERN.Value + AngebotDto.ANGOBHERZUBBRUTTO.Value + AngebotDto.ANGOBZUBEHOERBRUTTO.Value + AngebotDto.ANGOBUEBERFUEHRUNGBRUTTO.Value + AngebotDto.ANGOBZULASSUNGBRUTTO.Value);
                AngebotDto.ANGOBGRUNDEXTERNBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(AngebotDto.ANGOBGRUNDEXTERN.Value - AngebotDto.ANGOBAHKRABATTOBRUTTO.Value);
                AngebotDto.ANGOBAHKBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(AngebotDto.ANGOBAHKEXTERNBRUTTO.Value - AngebotDto.ANGOBAHKRABATTOBRUTTO.Value);
                // Check if the offer is from ObTyp
                if (offer.IsFromObTyp || offer.UseFzData)
                {
                    // Get data from ObTyp
                    MyFillAngebotFromObTyp(offer, AngebotDto);

                }
                else
                {
                    // Get data from Eurotax
                    if(offer.Vehicle.Code!=null)
                    MyFillAngebotFromEurotax(offer, AngebotDto);
                }
                AngebotDto.ANGOBSCHWACKE = offer.Vehicle.Code;
                AngebotDto.ANGOBSERIE = offer.serie;
                if (offer.erinklmwst.HasValue)
                    AngebotDto.ANGOBERINKLMWST = offer.erinklmwst;
                AngebotDto.ANGKALKSZBRUTTO = offer.DownPayment;
                AngebotDto.ANGOBBAUJAHR = offer.baujahr;
                AngebotDto.ZUSTAND = "Neu";
                AngebotDto.SYSVART = 15;
                AngebotDto.OBJEKTVT = "neues VAP-Angebot"; //offer.Vehicle.BrandName + " " + AngebotDto.ANGOBTYP;// offer.Vehicle.Name;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Änderung des Fahrzeugs nicht möglich: "+e.Message, e);
            }

            // Return angebot
            return AngebotDto;
        }

        /// <summary>
        /// Validates the integrity ofan insurance FLAG with the given ANGVSPARAM-Contents
        /// angebotDto will be changed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="angebotDto"></param>
        public static void validateInsurances(DdOlExtended context, ANGEBOTDto angebotDto)
        {

            VSTYPDao vsd = new VSTYPDao(context);

            //alle aus ANGVSPARAM entfernen, die nicht angeflagt sind
            List<InsuranceDto> resultInsurances = new List<InsuranceDto>();
            foreach (InsuranceDto ins in angebotDto.ANGVSPARAM)
            {
                VSTYP vstyp = vsd.getVsTyp(ins.InsuranceParameter.SysVSTYP);
                if (vstyp.VALIDUNTIL != null && vstyp.VALIDUNTIL.HasValue && (vstyp.VALIDUNTIL.Value.CompareTo(DateTime.Now) < 0 && vstyp.VALIDUNTIL.Value > VSTYPDao.nullDate))
                {
                    //no more valid because of validuntil! - disable the insurance
                    if (checkFlag(angebotDto.ANGKALKFSRSVFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("RSDV"))
                        angebotDto.ANGKALKFSRSVFLAG = 0;
                    else if (checkFlag(angebotDto.ANGKALKFSVKFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("KASKO"))
                        angebotDto.ANGKALKFSVKFLAG = 0;
                    else if (checkFlag(angebotDto.ANGKALKFSHPFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("HP"))
                        angebotDto.ANGKALKFSHPFLAG = 0;
                    else if (checkFlag(angebotDto.ANGKALKFSINSASSENFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("IUV"))
                        angebotDto.ANGKALKFSINSASSENFLAG = 0;
                    else if (checkFlag(angebotDto.ANGKALKFSGAPFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("GAP"))
                        angebotDto.ANGKALKFSGAPFLAG = 0;
                    else if (checkFlag(angebotDto.ANGKALKFSRECHTSCHUTZFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("RSV"))
                        angebotDto.ANGKALKFSRECHTSCHUTZFLAG = 0;
                }
                else
                    resultInsurances.Add(ins);
            }
            angebotDto.ANGVSPARAM = resultInsurances.ToArray();

        }
        private static bool checkFlag(int? v)
        {
            if (v == null) return false;
            if (!v.HasValue) return false;
            if (v.Value < 1) return false;
            return true;
        }
        public static ANGEBOTDto Deliver(long sysId, long sysBrand, long sysPeRole, long vpSysPERSON, long sysPERSON, long sysWfuser, long sysPUser)
        {
            Cic.OpenLease.Service.ANGEBOTAssembler ANGEBOTAssembler;
            Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto ANGEBOTDto = null;
            ANGEBOT ANGEBOT = null;
            ANGKALK ANGKALK = null;
            ANGOB ANGOB = null;
            ANGOBOPTION ANGOPTION = null;
            ANGOBINI ANGOBINI = null;
            ANGEBOTITDto IT = null;
			ANGABL abl = null;
			ANGABL ablhhr = null;
			ANGKALKFS ANGKALKFS = null;
            ANGOBAUST[] ANGOBAUST = null;
            WFMMEMO WFMMEMOvk = null, WFMMEMOi = null, WFMMEMOSubmit = null;

            bool isMotorrad = false;

            if (sysId < 0)
            {
                throw new ArgumentException("Less than zero", "SYSID");
            }
            ANGEBOTDto vorvtInfo = null;
            using (DdOlExtended Context = new DdOlExtended())
            {
                ANGEBOT = (from c in Context.ANGEBOT
                           where c.SYSID == sysId
                           select c).FirstOrDefault();// Context.SelectById<ANGEBOT>(sysId);
                //Get IT
                if (ANGEBOT != null)
                {
                    IT = Context.ExecuteStoreQuery<ANGEBOTITDto>(ANGEBOTITDto.QUERY + ANGEBOT.SYSIT, null).FirstOrDefault();

                    if (IT == null)
                    {
                        IT = new ANGEBOTITDto();
                    }
                }
                else
                {
                    IT = new ANGEBOTITDto();
                }

                //Get ANGKALK and related
                if (ANGEBOT != null)
                {
                    if (ANGEBOT.SYSVORVT.HasValue && ANGEBOT.SYSVORVT.Value > 0)
                    {
                        vorvtInfo = Context.ExecuteStoreQuery<ANGEBOTDto>("select vertrag vorvertragsnummer,kalk.syskalktyp as SYSKALKTYPVORVT from vt,kalk,ob where kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + ANGEBOT.SYSVORVT.Value, null).FirstOrDefault();

                    }
                    //Get WFMMEMO
                    using (DdOwExtended ContextDdOw = new DdOwExtended())
                    {
                        WFMMEMOvk = WFMMEMOHelper.DeliverWfmmemoFromAngebot(ContextDdOw, ANGEBOT.SYSID, WFMKATHelper.CnstWfmmkatNameSonderkalkulationVerkaeufer);
                        WFMMEMOi =  WFMMEMOHelper.DeliverWfmmemoFromAngebot(ContextDdOw, ANGEBOT.SYSID, WFMKATHelper.CnstWfmmkatNameSonderkalkulationInnendiest);
                        //WFMMEMOSubmit = Cic.OpenLease.Model.DdOw.WFMMEMOHelper.DeliverWfmmemoFromAngebot(ContextDdOw, ANGEBOT.SYSID, WFMKATHelper.CnstWfmmkatNameAngebotSubmit);
                    }

                    ANGKALK = ANGEBOTHelper.GetAngkalkFromAngebot(Context, ANGEBOT.SYSID);
                    if (ANGKALK != null) //ANGKALK is loaded
                    {
                        if (ANGKALK.ANGOB == null)
                            Context.Entry(ANGKALK).Reference(f => f.ANGOB).Load();
                         
                        ANGOB = ANGKALK.ANGOB;

                        if (ANGKALK.ANGOB == null)
                            Context.Entry(ANGKALK).Reference(f => f.ANGOB).Load();
                        if (ANGOB.OBKAT == null)
                            Context.Entry(ANGOB).Reference(f => f.OBKAT).Load();
                        if (ANGOB.OBART == null)
                            Context.Entry(ANGOB).Reference(f => f.OBART).Load();

                       
                        ANGOPTION = Context.ExecuteStoreQuery<ANGOBOPTION>("select sysid, PDEC1501,PDEC1502 from angoboption where sysid=" + ANGOB.SYSOB, null).FirstOrDefault();
                        isMotorrad = HaftpflichtCalculator.isMotorrad( ANGOB.SYSOBTYP.GetValueOrDefault(), Context);
                        ANGOBINI = ANGOBHelper.GetAngobiniFromAngob(Context, ANGOB.SYSOB);
                        if (ANGOBINI == null)
                        {
                            ANGOBINI = new ANGOBINI();
                        }
                        ANGKALKFS = ANGKALKHelper.GetAngkalkfsFromAngkalk(Context, ANGKALK.SYSKALK);
                        if (ANGKALKFS == null)
                        {
                            ANGKALKFS = new ANGKALKFS();
                        }

                        ANGOBAUST = ANGOBHelper.GetAngobaustFromAngob(Context, ANGOB.SYSOB);

						//abl_ALT = (from a in Context.ANGABL where (a.SYSANGEBOT == (long)ANGEBOT.SYSID && a.SYSABLTYP != 43) select a).FirstOrDefault();
						abl = Context.ExecuteStoreQuery<ANGABL> ("SELECT * FROM ANGABL WHERE SYSANGEBOT = " + ANGEBOT.SYSID + " AND (SYSABLTYP is null OR SYSABLTYP <> 43)", null).FirstOrDefault ();

						//ablhhr_ALT = (from a in Context.ANGABL where (a.SYSANGEBOT == (long) ANGEBOT.SYSID && a.SYSABLTYP == 43) select a).FirstOrDefault ();
						ablhhr = Context.ExecuteStoreQuery<ANGABL> ("SELECT * FROM ANGABL WHERE SYSANGEBOT = " + ANGEBOT.SYSID + " AND SYSABLTYP = 43", null).FirstOrDefault ();

					}
					else //ANGKALK is not loaded
                    {
                        ANGKALK = new ANGKALK();
                        ANGOB = new ANGOB();
                        ANGOBINI = new ANGOBINI();
                        ANGKALKFS = new ANGKALKFS();
                        ANGOPTION = new ANGOBOPTION();
                    }
                }
                else
                {
                    ANGKALK = new ANGKALK();
                    ANGOB = new ANGOB();
                    ANGOBINI = new ANGOBINI();
                    ANGKALKFS = new ANGKALKFS();
                    ANGOPTION = new ANGOBOPTION();
                }
                // Check sight field
                if (ANGEBOT != null)
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, sysPUser, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysId))
                    {
                        // Not exists on the list
                        ANGEBOT = null;
                    }
                }

            }

            if (ANGEBOT != null)
            {
                // New assembler
                ANGEBOTAssembler = new Cic.OpenLease.Service.ANGEBOTAssembler(sysPeRole, sysBrand, vpSysPERSON, sysPERSON, sysWfuser, sysPUser);



                // Create dto
                ANGEBOTDto = ANGEBOTAssembler.ConvertToDto(ANGEBOT, ANGKALK, ANGKALKFS, ANGOB, ANGOBINI, IT, ANGOBAUST, ANGOPTION);
                if (vorvtInfo != null)
                {
                    ANGEBOTDto.VORVERTRAGSNUMMER = vorvtInfo.VORVERTRAGSNUMMER;
                    ANGEBOTDto.SYSKALKTYPVORVT = vorvtInfo.SYSKALKTYPVORVT;
                }
                if (abl != null)
                {
                    ANGEBOTDto.ANGABLIBAN = abl.IBAN;
                    ANGEBOTDto.ANGABLFREMDVERTRAG = abl.FREMDVERTRAG;
                    ANGEBOTDto.ANGABLFLAGINTEXT = abl.FLAGINTEXT.HasValue ? abl.FLAGINTEXT.Value : 0;
                    ANGEBOTDto.ANGABLBANK = abl.BANK;
					ANGEBOTDto.ANGABLAKTUELLERATE = abl.AKTUELLERATE;
				}

				if (ablhhr != null) // rh 20180412
				{
					// ANGEBOTDto.ANGABLIBAN = ablhhr.IBAN;
					ANGEBOTDto.VVTAKTUELLERATE = ablhhr.AKTUELLERATE;      
					ANGEBOTDto.VVTDATKALKPER = ablhhr.DATKALKPER;          
				}

				// Check  Angebot ist gueltig 
				if (ANGEBOTDto.GUELTIGBIS >= DateTime.Today)

                    ANGEBOTDto.Gueltig = true;
                else
                    ANGEBOTDto.Gueltig = false;

                // MEMO TO DTO

                if (WFMMEMOSubmit != null)
                    ANGEBOTDto.WFMSubmitKommentar =  (WFMMEMOSubmit.NOTIZMEMO);
                else
                    ANGEBOTDto.WFMSubmitKommentar = "";
                if (WFMMEMOi != null)
                    ANGEBOTDto.WFMMEMOSCALCIDTEXT =  (WFMMEMOi.NOTIZMEMO);
                else
                    ANGEBOTDto.WFMMEMOSCALCIDTEXT = "";
                if (WFMMEMOvk != null)
                    ANGEBOTDto.WFMMEMOSCALCVKTEXT =  (WFMMEMOvk.NOTIZMEMO);
                else
                    ANGEBOTDto.WFMMEMOSCALCVKTEXT = "";




                using (DdOwExtended context = new DdOwExtended())
                {
                    AngebotBinaryDao dao = new AngebotBinaryDao(context);
                    ANGEBOTDto.SYSWFDADOC = dao.getPictureIdFromAngebot(sysId);



                }
                using (DdOlExtended Context = new DdOlExtended())
                {
                    long sysls = LsAddHelper.getMandantByPEROLE(Context, sysPeRole);

                    BankdatenDao bdao = new BankdatenDao();
                    BankdatenDto bankdaten = BankdatenDao.getBankDatenFromAngebot(ANGEBOTDto, sysls, Context, false, true);//von gespeichertem angebot also mandat laden
                    long syskonto = 0;
                    long sysmandat = BankdatenDao.findMandat(Context, bankdaten, ref syskonto);
                    if (sysmandat > 0)//current mandat signcity
                    {
                        ANGEBOTDto.UNTERSCHRIFTORT = Context.ExecuteStoreQuery<String>("select signcity from mandat where sysmandat=" + sysmandat, null).FirstOrDefault();
                    }
                    if (ANGEBOTDto.UNTERSCHRIFTORT == null || ANGEBOTDto.UNTERSCHRIFTORT.Length == 0)
                    {//preselected signcity if not yet in mandate
                        ANGEBOTDto.UNTERSCHRIFTORT = Context.ExecuteStoreQuery<String>("select ort from person,angebot where sysperson=angebot.sysvk and angebot.sysid=" + bankdaten.SYSANGEBOT, null).FirstOrDefault();
                    }


                }
                ANGEBOTDto.isMotorrad = isMotorrad;
            }
            
            return ANGEBOTDto;
        }

        public static ANGEBOTDto Save(ANGEBOTDto angebotDto, long sysBrand, long sysPeRole, long vpSysPERSON, long sysPERSON, long sysWfuser, long sysPUSER)
        {
            return Save(angebotDto, sysBrand, sysPeRole, vpSysPERSON, sysPERSON, sysWfuser, null, null, sysPUSER, false);
        }

        public static ANGEBOTDto Save(ANGEBOTDto angebotDto, long sysBrand, long sysPeRole, long vpSysPERSON, long sysPERSON, long sysWfuser, int? finKz, long? sysAngebot, long sysPUSER, bool noValidation)
        {
            Cic.OpenLease.Service.ANGEBOTAssembler ANGEBOTAssembler;
            ANGEBOT ModifiedANGEBOT = null;
            ANGKALK ModifiedANGKALK = null;
            ANGOB ModifiedANGOB = null;
            ANGOBINI ModifiedANGOBINI = null;
            ANGOBOPTION ModifiedANGOPTION = null;
            ANGKALKFS ModifiedANGKALKFS = null;
            ANGOBAUST[] ModifiedANGOBAUST = null;
            Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto ModifiedANGEBOTDto = null;

            String vvtnummer = angebotDto.VORVERTRAGSNUMMER;

            if (angebotDto.SPECIALCALCSYSWFUSER < 0)
            {
                angebotDto.SPECIALCALCSTATUS = 0;
                angebotDto.SPECIALCALCSYSWFUSER = 0;
                angebotDto.SPECIALCALCDATE = null;
            }

            if (angebotDto == null)
            {
                throw new ArgumentException("angebotDto");
            }
            using (DdOlExtended Context = new DdOlExtended())
            {

                if (!noValidation && !isPrproductValidIt(Context, angebotDto.SYSPRPRODUCT, angebotDto.SYSIT))
                {
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SaveAngebotFailedProduct, "Das ausgewählte Produkt ist für die Kundenart nicht gültig. Bitte wählen Sie ein anderes Finanzierungsprodukt aus.");
                }

            }
            // New assembler
            ANGEBOTAssembler = new Cic.OpenLease.Service.ANGEBOTAssembler(sysPeRole, sysBrand, vpSysPERSON, sysPERSON, sysWfuser, sysPUSER);
            angebotDto.ANGOBFABRIKAT = fixLength(angebotDto.ANGOBFABRIKAT, 40);
            angebotDto.ANGOBHERSTELLER = fixLength(angebotDto.ANGOBHERSTELLER, 40);
            angebotDto.ANGOBSERIE = fixLength(angebotDto.ANGOBSERIE, 40);
            angebotDto.ANGOBFARBEA = fixLength(angebotDto.ANGOBFARBEA, 20);
            angebotDto.ANGOBTYP = fixLength(angebotDto.ANGOBTYP, 60);

            // Check dto
            if (!ANGEBOTAssembler.IsValid(angebotDto))
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralNotValid, Cic.OpenLease.Service.DtoAssemblerHelper.DeliverErrorMessage(ANGEBOTAssembler.Errors));
            }

            try
            {

                if (angebotDto.SYSID == null)
                {
                    // Create
                    ANGEBOTAssembler.Create(angebotDto, out ModifiedANGEBOT, out ModifiedANGKALK, out ModifiedANGKALKFS, out ModifiedANGOB, out ModifiedANGOBINI, out ModifiedANGOBAUST, out ModifiedANGOPTION);

                }
                else
                {
                    // Update
                    ANGEBOTAssembler.Update(angebotDto, out ModifiedANGEBOT, out ModifiedANGKALK, out ModifiedANGKALKFS, out ModifiedANGOB, out ModifiedANGOBINI, out ModifiedANGOBAUST, finKz, sysAngebot, out ModifiedANGOPTION);
                }

                bool isMotorrad = false;
                ANGEBOTITDto IT = null;
                ANGEBOTDto vorvtInfo = null;
                using (DdOlExtended Context = new DdOlExtended())
                {
                    //Get IT
                    IT = Context.ExecuteStoreQuery<ANGEBOTITDto>(ANGEBOTITDto.QUERY + ModifiedANGEBOT.SYSIT, null).FirstOrDefault();
                    if(angebotDto.SYSOBTYP.HasValue)
                        isMotorrad = HaftpflichtCalculator.isMotorrad(angebotDto.SYSOBTYP.Value, Context);
                    //IT = ANGEBOTHelper.GetITFromAngebot(Context, ModifiedANGEBOT.SYSIT);
                    if (IT == null)
                    {
                        IT = new ANGEBOTITDto();
                    }
                    if (angebotDto.SYSVORVT.HasValue && angebotDto.SYSVORVT.Value > 0)
                    {
                        vorvtInfo = Context.ExecuteStoreQuery<ANGEBOTDto>("select vertrag vorvertragsnummer,kalk.syskalktyp as SYSKALKTYPVORVT from vt,kalk,ob where kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + angebotDto.SYSVORVT.Value, null).FirstOrDefault();

                    }
                }
                if (angebotDto.SYSWFDADOC != null && angebotDto.SYSWFDADOC > 0)
                {
                    using (DdOwExtended context = new DdOwExtended())
                    {
                        AngebotBinaryDao dao = new AngebotBinaryDao(context);
                        dao.linkToAngebot((long)angebotDto.SYSWFDADOC, ModifiedANGEBOT.SYSID);
                    }
                }



                // Create dto
                ModifiedANGEBOTDto = ANGEBOTAssembler.ConvertToDto(ModifiedANGEBOT, ModifiedANGKALK, ModifiedANGKALKFS, ModifiedANGOB, ModifiedANGOBINI, IT, ModifiedANGOBAUST, ModifiedANGOPTION);

                using (DdOlExtended Context = new DdOlExtended())
                {
					// SYSABLTYP != 43
					//ANGABL abl_ALT = (from a in Context.ANGABL where (a.SYSANGEBOT == (long) ModifiedANGEBOT.SYSID && (a.SYSABLTYP.HasValue && a.SYSABLTYP != 43))  select a).FirstOrDefault();
					ANGABL abl = Context.ExecuteStoreQuery<ANGABL> ("SELECT * FROM ANGABL WHERE SYSANGEBOT = " + ModifiedANGEBOT.SYSID + " AND (SYSABLTYP is null OR SYSABLTYP <> 43)", null).FirstOrDefault ();
					if (abl != null)
                    {
						
                        ModifiedANGEBOTDto.ANGABLIBAN = abl.IBAN;
                        ModifiedANGEBOTDto.ANGABLFREMDVERTRAG = abl.FREMDVERTRAG;
                        ModifiedANGEBOTDto.ANGABLFLAGINTEXT = abl.FLAGINTEXT.HasValue ? abl.FLAGINTEXT.Value : 0;
                        ModifiedANGEBOTDto.ANGABLBANK = abl.BANK;
                        ModifiedANGEBOTDto.ANGABLAKTUELLERATE = abl.AKTUELLERATE;
					}

					// SYSABLTYP == 43 (rh 20180412)
					//ANGABL ablhhr = (from a in Context.ANGABL where (a.SYSANGEBOT == (long) ModifiedANGEBOT.SYSID && (a.SYSABLTYP.HasValue && a.SYSABLTYP == 43)) select a).FirstOrDefault ();
					ANGABL ablhhr = Context.ExecuteStoreQuery<ANGABL> ("SELECT * FROM ANGABL WHERE SYSANGEBOT = " + ModifiedANGEBOT.SYSID + " AND SYSABLTYP = 43", null).FirstOrDefault ();
					if (ablhhr != null)
					{
						ModifiedANGEBOTDto.VVTAKTUELLERATE = ablhhr.AKTUELLERATE;      
						ModifiedANGEBOTDto.VVTDATKALKPER = ablhhr.DATKALKPER;          
					}
				}

				// OriginalOPTION = Context.ExecuteStoreQuery<ANGOBOPTION>("select sysid, PDEC1501,PDEC1502 from angoboption where sysid=" + OriginalANGOB.SYSOB, null).FirstOrDefault();

				ModifiedANGEBOTDto.isMotorrad = isMotorrad;
                ModifiedANGEBOTDto.ANGKALKRATE_SUBVENTION = angebotDto.ANGKALKRATE_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKRATE_SUBVENTION2 = angebotDto.ANGKALKRATE_SUBVENTION2;
                ModifiedANGEBOTDto.ANGKALKRGGEBUEHR_SUBVENTION = angebotDto.ANGKALKRGGEBUEHR_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKGEBUEHR_SUBVENTION = angebotDto.ANGKALKGEBUEHR_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKRWKALK_SUBVENTION = angebotDto.ANGKALKRWKALK_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKMITFIN_SUBVENTION = angebotDto.ANGKALKMITFIN_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKFSFUELPRICE_SUBVENTION = angebotDto.ANGKALKFSFUELPRICE_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKFSANABMELDUNG_SUBVENTION = angebotDto.ANGKALKFSANABMELDUNG_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKFSREPCARRATE_SUBVENTION = angebotDto.ANGKALKFSREPCARRATE_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKFSSTIRESPRICE_SUBVENTION = angebotDto.ANGKALKFSSTIRESPRICE_SUBVENTION;
                ModifiedANGEBOTDto.ANGKALKFSMAINTENANCE_SUBVENTION = angebotDto.ANGKALKFSMAINTENANCE_SUBVENTION;
                ModifiedANGEBOTDto.WFMMEMOSCALCIDTEXT = angebotDto.WFMMEMOSCALCIDTEXT;
                ModifiedANGEBOTDto.WFMMEMOSCALCVKTEXT = angebotDto.WFMMEMOSCALCVKTEXT;
                if (vorvtInfo != null)
                {
                    ModifiedANGEBOTDto.VORVERTRAGSNUMMER = vorvtInfo.VORVERTRAGSNUMMER;
                    ModifiedANGEBOTDto.SYSKALKTYPVORVT = vorvtInfo.SYSKALKTYPVORVT;
                }

            }
            catch (System.Exception ex)
            {
                _Log.Error("Error upon Save", ex);
                Exception i1 = ex.InnerException;
                if (i1 != null)
                {
                    _Log.Error("Error upon Save - Detail1: ", i1);
                    Exception i2 = i1.InnerException;
                    if (i2 != null)
                        _Log.Error("Error upon Save - Detail2: ", i2);
                }
                throw;
            }
            ModifiedANGEBOTDto.VORVERTRAGSNUMMER = vvtnummer;


            try
            {

                using (DdOlExtended Context = new DdOlExtended())
                {
                    decimal Ust = LsAddHelper.GetTaxRate(Context, angebotDto.SYSVART);
                    long obarttyp = Context.ExecuteStoreQuery<long>("select typ from obart where sysobart=" + angebotDto.SYSOBART, null).FirstOrDefault();
                    String vartcode = Context.ExecuteStoreQuery<String>("select code from vart where sysvart=" + angebotDto.SYSVART, null).FirstOrDefault();

                    decimal UstVoll = LsAddHelper.getGlobalUst(sysPeRole);
                    decimal UstKP = UstVoll;
                    if (ModifiedANGOB.ERINKLMWST.HasValue && ModifiedANGOB.ERINKLMWST == 0)
                        UstKP = 0;

                    List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "rabatto", Value = ModifiedANGOB.AHKRABATTO });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "rabattop", Value = ModifiedANGOB.AHKRABATTOP });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = ModifiedANGOB.SYSOB });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "rabattierfaehigbrutto", Value = ModifiedANGOB.GRUNDEXTERN });
                    decimal rabattierfaehignetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)ModifiedANGOB.GRUNDEXTERN, UstKP) ;
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "rabattierfaehig", Value =  rabattierfaehignetto });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "werkabgabepreisexternbrutto", Value = ModifiedANGOB.GRUNDEXTERNBRUTTO });
                    decimal werknetto =  Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)ModifiedANGOB.GRUNDEXTERNBRUTTO, UstKP) ;
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "werkabgabepreisextern", Value =werknetto});
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "werkabgabepreisexternust", Value = ModifiedANGOB.GRUNDEXTERNBRUTTO-werknetto });
                    
                    
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ahkexternbrutto", Value = ModifiedANGOB.AHKEXTERNBRUTTO });
                    decimal ahknetto = rabattierfaehignetto + Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)ModifiedANGOB.HERZUBBRUTTO, UstKP)
                        + Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)ModifiedANGOB.ZUBEHOERBRUTTO, UstVoll)
                        + Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)ModifiedANGOB.UEBERFUEHRUNGBRUTTO, UstVoll)
                        + Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)ModifiedANGOB.ZULASSUNGBRUTTO, UstKP);
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ahkextern", Value =ahknetto});
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "gesamtust", Value = ModifiedANGOB.AHKEXTERNBRUTTO-ahknetto });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "fztyp", Value = obarttyp });

                    decimal minderkmsatz = 0;
                    decimal mehrkmsatz = 0;
                    decimal kmtoleranz = 0;
                    decimal minderkmbrutto = 0;
                    decimal mehrkmbrutto = 0;
                    if ("LEASING_KILOMETER".Equals(vartcode))
                    {
                        minderkmsatz = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MINDER_KM_SATZ);
                        mehrkmsatz = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MEHR_KM_SATZ);
                        kmtoleranz = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_KM_TOLERANZGRENZE);
                        minderkmbrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(minderkmsatz, Ust);
                        mehrkmbrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(mehrkmsatz, Ust);
                    }


                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "satzmehrkm", Value = mehrkmsatz });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SatzMehrKmBrutto", Value = mehrkmbrutto });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SatzMinderKm", Value = minderkmsatz });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SatzMinderKmBrutto", Value = minderkmbrutto });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "KMToleranz", Value = kmtoleranz });


                    Context.ExecuteStoreCommand("update angob set bezeichnung=(select bezeichnung from vc_obtyp5 where id5=sysobtyp), satzmehrkm=:satzmehrkm,SatzMehrKmBrutto=:SatzMehrKmBrutto,KMToleranz=:KMToleranz,SatzMinderKmBrutto=:SatzMinderKmBrutto,SatzMinderKm=:SatzMinderKm, fztyp=:fztyp,werkabgabepreisexternbrutto=:werkabgabepreisexternbrutto, werkabgabepreisextern=:werkabgabepreisextern, werkabgabepreisexternust=:werkabgabepreisexternust, werkabgabepreisbrutto=:rabattierfaehigbrutto, werkabgabepreis=:rabattierfaehig, gesamtbrutto=:ahkexternbrutto, gesamt=:ahkextern,gesamtust=:gesamtust, werkabgabepreisrabatto=:rabatto,werkabgabepreisrabattop=:rabattop where sysob=:sysob", parameters2.ToArray());
                }
            }
            catch (System.Exception ex)
            {
                _Log.Error("Error upon Save", ex);
                Exception i1 = ex.InnerException;
                if (i1 != null)
                {
                    _Log.Error("Error upon Save - Detail1: ", i1);
                    Exception i2 = i1.InnerException;
                    if (i2 != null)
                        _Log.Error("Error upon Save - Detail2: ", i2);
                }
            }
            return ModifiedANGEBOTDto;
        }

        private static String fixLength(String field, int length)
        {
            if (field != null)
            {
                field = field.Trim();
                if (field.Length > length)
                    field = field.Substring(0, length);
            }
            return field;
        }
        #endregion

        #region My methods
        /// <summary>
        /// Called when car selected from carconfigurator with manual configured vehicle (non-eurotax, noextid=1)
        /// see also ANGEBOTDao.DeliverBmwTechnicalDataExtendedFromObTyp
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="angebotDto"></param>
        private static void MyFillAngebotFromObTyp(OfferConfiguration offer, ANGEBOTDto angebotDto)
        {
            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {

                // Get ObTyp
                // Parse the code
                long SysObTyp = 0;
                long.TryParse(offer.Vehicle.Code, out SysObTyp);//from car configurator

                var CurrentObTyp = (from ObTyp in Context.OBTYP
                                    where ObTyp.SYSOBTYP == SysObTyp
                                    select ObTyp).FirstOrDefault();

                if (offer.UseFzData)//from sa3 when obtyp manually
                {
                    CurrentObTyp = (from ObTyp in Context.OBTYP
                                    where ObTyp.SCHWACKE == offer.Vehicle.Code
                                    select ObTyp).FirstOrDefault();
                }



                // Check if anything was found
                if (CurrentObTyp == null)
                {


                    // Throw an exception
                    throw new Exception("Type not found: ObTyp/SCHWACKE: " + SysObTyp + ".");

                }
                if (CurrentObTyp.SCHWACKE != null && CurrentObTyp.SCHWACKE.Length > 0)
                    offer.Vehicle.Code = CurrentObTyp.SCHWACKE;

                if (CurrentObTyp.FZTYP == null)
                    Context.Entry(CurrentObTyp).Reference(f => f.FZTYP).Load();
                

                // Check if FzTyp exists
                if (CurrentObTyp.FZTYP != null)//manuell erfasstes Fahrzeug
                {
                    angebotDto.ANGOBINICO2 = (long)Math.Round(CurrentObTyp.FZTYP.CO2EMI.GetValueOrDefault());
                    angebotDto.ANGOBINIPARTICLES = (double)CurrentObTyp.FZTYP.PARTIKEL.GetValueOrDefault();
                    angebotDto.ANGOBININOX = (double)CurrentObTyp.FZTYP.NOX.GetValueOrDefault();
                    angebotDto.ANGOBINIVERBRAUCH_D = CurrentObTyp.FZTYP.VERBRAUCH.GetValueOrDefault();
                    angebotDto.ANGOBINIKW = CurrentObTyp.FZTYP.LEISTUNG.GetValueOrDefault();
                    angebotDto.ANGOBININOVA_P = CurrentObTyp.FZTYP.NOVA;
                    angebotDto.ANGOBNOVA = 0;// not yet known if customer has not to pay nova (value = 1)
                    angebotDto.ANGOBCCM = CurrentObTyp.FZTYP.HUBRAUM.GetValueOrDefault();
                    angebotDto.ANGOBKW = (long)angebotDto.ANGOBINIKW;


                    long sysprmart = CurrentObTyp.FZTYP.SYSPRMART.GetValueOrDefault();
                    if (sysprmart > 0)
                    {

                        NoVA n = new NoVA(Context);

                        angebotDto.ANGOBINIMOTORTYP = NoVA.getMotortyp(n.getAntriebsartMART(sysprmart));
                    }
                    //CO2 Reifen
                    //Defaults, they wont be changed ever,                    
                    angebotDto.ANGOBINICO2DEF = angebotDto.ANGOBINICO2;
                    angebotDto.ANGOBININOXDEF = angebotDto.ANGOBININOX;
                    angebotDto.ANGOBINIVERBRAUCH_DDEF = angebotDto.ANGOBINIVERBRAUCH_D;
                    angebotDto.ANGOBINIPARTICLESDEF = angebotDto.ANGOBINIPARTICLES;

                    angebotDto.ANGOBINIACTUATION = 0;
                    if (CurrentObTyp.FZTYP.MART != null)
                    {
                        String mart = CurrentObTyp.FZTYP.MART;
                        if (mart.Contains("Hybrid"))
                            angebotDto.ANGOBINIACTUATION = 1;

                        if (sysprmart == 0 && CurrentObTyp.FZTYP.MART != null)//not yet configured correctly
                            angebotDto.ANGOBINIMOTORTYP = NoVA.getMotortyp(NoVA.getAntriebsartFZ(CurrentObTyp.FZTYP.MART));
                    }
                    angebotDto.ANGOBINIACTUATIONDEF = angebotDto.ANGOBINIACTUATION;
                    angebotDto.ANGOBAUTOMATIK = (CurrentObTyp.FZTYP.GART == "Automatik") ? 1 : 0;
                    angebotDto.ANGOBNOVAP = (decimal)CurrentObTyp.FZTYP.NOVA;
                    angebotDto.ANGOBNOVAPDEF = angebotDto.ANGOBNOVAP;

                }
                if (CurrentObTyp.FZTYP == null || offer.Type == OfferTypeConstants.Sa3)//Manuell und kommt über SA3
                {
                    //CO2-Besteuerung changes:
                    angebotDto.ANGOBINICO2 = (long)offer.Vehicle.co2overal;
                    angebotDto.ANGOBINIPARTICLES = (double)offer.Vehicle.particlemass;
                    angebotDto.ANGOBININOX = (double)offer.Vehicle.nox;
                    angebotDto.ANGOBINIVERBRAUCH_D = (decimal)offer.Vehicle.consumptionoveral;
                    angebotDto.ANGOBNOVAZUAB = (decimal)offer.Vehicle.novaoekobetrag;

                    if (offer.Type == OfferTypeConstants.Sa3)
                    {
                        angebotDto.ANGOBININOVA_P = (decimal)offer.Nova * 100 - 100;
                        angebotDto.ANGOBNOVAP = (decimal)offer.Nova * 100 - 100;
                    }
                    else
                    {
                        angebotDto.ANGOBININOVA_P = (decimal)offer.Nova;
                        angebotDto.ANGOBNOVAP = (decimal)offer.Nova;
                    }
                    angebotDto.ANGOBNOVA = 0;// not yet known if customer has not to pay nova (value = 1)
                    angebotDto.ANGOBINIACTUATION = 0;

                    using (DdEurotaxExtended Entities = new DdEurotaxExtended())//kann hier eigentlich nie rein, da et über andere Methode läuft und hier nur manuelle ohne ET reinkommen ?!
                    {
                        // Query ETGTYPE
                        var CurrentType = (from Type in Entities.ETGTYPE
                                           where Type.NATCODE == offer.Vehicle.Code
                                           select Type).FirstOrDefault();

                        // Check if there is such a type
                        if (CurrentType != null)
                        {
                            // Get the engine capacity and power
                            angebotDto.ANGOBCCM = (long)CurrentType.CAPTECH;
                            angebotDto.ANGOBKW = (long)CurrentType.KW;
                            angebotDto.ANGOBINIKW = (long)angebotDto.ANGOBKW;
                            angebotDto.ANGOBAUTOMATIK = NoVA.hasAutoTransmission(CurrentType.TXTTRANSTYPECD2) ? 1 : 0;
                            // Get the hybrid flag
                            angebotDto.ANGOBINIACTUATION = NoVA.isHybrid(CurrentType.SECFUELTYPCD2) ? 1 : 0;

                            if ("UNDEFINED".Equals(angebotDto.ANGOBINIMOTORTYP))
                            {
                                // Query ETGTXTTABEL
                                angebotDto.ANGOBINIMOTORTYP = NoVA.getMotortyp(NoVA.getAntriebsartEurotax(CurrentType.TXTFUELTYPECD2));
                            }
                        }
                    }

                    //Defaults , they wont be changed ever,
                    angebotDto.ANGOBNOVAPDEF = angebotDto.ANGOBNOVAP;
                    angebotDto.ANGOBINICO2DEF = angebotDto.ANGOBINICO2;
                    angebotDto.ANGOBININOXDEF = angebotDto.ANGOBININOX;
                    angebotDto.ANGOBINIVERBRAUCH_DDEF = angebotDto.ANGOBINIVERBRAUCH_D;
                    angebotDto.ANGOBINIPARTICLESDEF = angebotDto.ANGOBINIPARTICLES;
                    //
                    angebotDto.ANGOBINIACTUATIONDEF = angebotDto.ANGOBINIACTUATION;
                    //ever! ;)
                }
                angebotDto.SYSOBTYP = CurrentObTyp.SYSOBTYP;
                angebotDto.ANGOBTYP = CurrentObTyp.BEZEICHNUNG;
                if (angebotDto.ANGOBINIKW != null)
                    angebotDto.ANGOBTYP += ", " + angebotDto.ANGOBINIKW + " kW";//Detail im Typ-Feld in Maske

                angebotDto.ANGOBFABRIKAT = Context.ExecuteStoreQuery<String>("select bezeichnung from obtyp where importtable='ETGMODEL' start with sysobtyp=" + CurrentObTyp.SYSOBTYP + "  connect by prior sysobtypp=sysobtyp", null).FirstOrDefault();
            }
        }


        /// <summary>
        /// Called when car selected from carconfigurator with eurotax attached for manual et entry or SA3
        /// see also ANGEBOTDao.MyDeliverBmwTechnicalDataExtended
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="angebotDto"></param>
        private static void MyFillAngebotFromEurotax(OfferConfiguration offer, ANGEBOTDto angebotDto)
        {
            // Get the Eurotax National Code
            string NatCode = offer.Vehicle.Code;
            // Create the entities
            using (DdEurotaxExtended Entities = new DdEurotaxExtended())
            {


                // Query ETGTYPE
                var CurrentType = (from Type in Entities.ETGTYPE
                                   where Type.NATCODE == NatCode
                                   select Type).FirstOrDefault();

                // Check if there is such a type
                if (CurrentType == null)
                    throw new ApplicationException("Cannot find vehicle with specified Eurotax number");

                // Get the engine capacity and power
                angebotDto.ANGOBCCM = (long)CurrentType.CAPTECH;
                angebotDto.ANGOBKW = (long)CurrentType.KW;
                angebotDto.ANGOBINIKW = (long)angebotDto.ANGOBKW;


                angebotDto.ANGOBAUTOMATIK = NoVA.hasAutoTransmission(CurrentType.TXTTRANSTYPECD2) ? 1 : 0;
                // Get the hybrid flag
                angebotDto.ANGOBINIACTUATION = NoVA.isHybrid(CurrentType.SECFUELTYPCD2) ? 1 : 0;

                // Query ETGTXTTABEL
                var CurrentFuel = (from Fuel in Entities.ETGTXTTABEL
                                   where Fuel.CODE == CurrentType.TXTFUELTYPECD2
                                   select Fuel).FirstOrDefault();
                if (CurrentFuel != null)
                    angebotDto.ANGOBINIMOTORTYP = NoVA.getMotortyp(NoVA.getAntriebsartEurotax(CurrentFuel.CODE));


                // Query ETGTYPEAT
                var CurrentTypeAt = (from TypeAt in Entities.ETGTYPEAT
                                     where TypeAt.NATCODE == NatCode
                                     select TypeAt).FirstOrDefault();

                // Check if there is a result
                if (CurrentTypeAt == null)
                    angebotDto.ANGOBNOVAP = 0;
                //throw new ApplicationException("Cannot retrieve NoVA rate");

                // Get nova tax
                else angebotDto.ANGOBNOVAP = CurrentTypeAt.NOVA2;
                angebotDto.ANGOBNOVA = 0;// not yet known if customer has not to pay nova (value = 1)


                // Query ETGCONSUMER
                var CurrentConsumer = (from Consumer in Entities.ETGCONSUMER
                                       where Consumer.NATCODE == NatCode
                                       select Consumer).FirstOrDefault();

                // Check if there is a result
                if (CurrentConsumer == null)
                {
                    _Log.Debug("Cannot find emission information for NATCODE=" + NatCode + " defaulting to null");
                }
                else
                {
                    // Set the consumption and emission
                    angebotDto.ANGOBINIVERBRAUCH_D = CurrentConsumer.CONSTOT;
                    angebotDto.ANGOBINIPARTICLES = (double)CurrentConsumer.PART.GetValueOrDefault();
                    angebotDto.ANGOBININOX = (double)CurrentConsumer.NOX.GetValueOrDefault();
                    angebotDto.ANGOBINICO2 = CurrentConsumer.CO2EMI;
                }



                //CO2-Besteuerung changes:
                angebotDto.ANGOBINICO2 = (long)offer.Vehicle.co2overal;
                angebotDto.ANGOBINIPARTICLES = (double)offer.Vehicle.particlemass;
                angebotDto.ANGOBININOX = (double)offer.Vehicle.nox;
                angebotDto.ANGOBINIVERBRAUCH_D = (decimal)offer.Vehicle.consumptionoveral;
                angebotDto.ANGOBNOVAZUAB = (decimal)offer.Vehicle.novaoekobetrag;


                angebotDto.ANGOBININOVA_P = 0;
                angebotDto.ANGOBNOVAP = 0;
                if (offer.Nova > 0)
                {
                    angebotDto.ANGOBININOVA_P = (decimal)offer.Nova * 100 - 100;
                    angebotDto.ANGOBNOVAP = (decimal)offer.Nova * 100 - 100;
                }
                angebotDto.ANGOBNOVA = 0;// not yet known if customer has not to pay nova (value = 1)

                //Defaults from SA3, they wont be changed ever,
                angebotDto.ANGOBNOVAPDEF = angebotDto.ANGOBNOVAP;
                angebotDto.ANGOBINICO2DEF = angebotDto.ANGOBINICO2;
                angebotDto.ANGOBININOXDEF = angebotDto.ANGOBININOX;
                angebotDto.ANGOBINIVERBRAUCH_DDEF = angebotDto.ANGOBINIVERBRAUCH_D;
                angebotDto.ANGOBINIPARTICLESDEF = angebotDto.ANGOBINIPARTICLES;
                //
                angebotDto.ANGOBINIACTUATIONDEF = angebotDto.ANGOBINIACTUATION;
                //ever! ;)

                // Create options list
                List<ANGOBAUSDto> OptionsList = new List<ANGOBAUSDto>();

                // For each options from offer...
                foreach (var LoopOption in offer.Vehicle.Options)
                {
                    // Create AngobAus
                    ANGOBAUSDto AngobAus = new ANGOBAUSDto();

                    // Set the properties
                    AngobAus.BESCHREIBUNG = LoopOption.Name;
                    AngobAus.SNR = LoopOption.Code;
                    AngobAus.FLAGPACKET = LoopOption.Type == OptionTypeConstants.Package ? 1 : 0;
                    AngobAus.BETRAG = LoopOption.Price;

                    // Add to the list
                    OptionsList.Add(AngobAus);
                }


                // Set the options
                angebotDto.ANGOBAUST = OptionsList.ToArray();



            }

            angebotDto.SYSOBTYP = offer.sysobtyp;
            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {

                // Get ObTyp
                var CurrentObTyp = (from ObTyp in Context.OBTYP
                                    where ObTyp.SCHWACKE == NatCode && ObTyp.IMPORTTABLE == CnstEtgTypeImportTableName
                                    select ObTyp).FirstOrDefault();
                if (CurrentObTyp != null)
                {
                    angebotDto.SYSOBTYP = CurrentObTyp.SYSOBTYP;

                }
            }
        }



        public static bool isPrproductValidIt(DdOlExtended context, long? sysprproduct, long? SYSIT)
        {

            KDTYP result = null;
            try
            {
                if(!sysprproduct.HasValue || sysprproduct.Value==0)
                    return true;
                string Query = "select syskdtyp from prclprktyp where sysprproduct=" + sysprproduct;
                List<long> kdtypids = context.ExecuteStoreQuery<long>(Query, null).ToList<long>();
                if (kdtypids.Count == 0) return true;


                var query =
                    from kdtyp in context.KDTYP
                    where kdtyp.SYSKDTYP ==
                    (
                     from j in context.IT
                     where j.SYSIT == SYSIT
                     select j.SYSKDTYP
                     ).FirstOrDefault()
                    select kdtyp;


                result = query.FirstOrDefault<KDTYP>();



                foreach (var kdtypid in kdtypids)
                {
                    if (kdtypid == result.SYSKDTYP) return true;
                }




            }
            catch (Exception e)
            {
                throw new ApplicationException("Could not get KDTYP in IT", e); ;
            }



            return false;

        }
        #endregion
    }
}