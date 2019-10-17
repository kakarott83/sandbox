using AutoMapper;
using Cic.OpenLease.Service;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using System.Data.Entity;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Santander;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateOEM.Service.Contract;
using Cic.OpenOne.GateOEM.Service.DTO;
using System;
using System.Collections.Generic;
using Cic.OpenOne.Common.Util.ExtensionOld;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.Workflow.DAO;
using Cic.One.DTO;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.Model.DdOw;
namespace Cic.One.GateOEM.BO
{
    /// <summary>
    /// Business Object for managing the VAP Interface
    /// creates an it/Angebot from the VAP-Input
    /// returns reference-data to the generated offer
    /// </summary>
    public class VAPBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private MembershipUserValidationInfo getAuthInfo(String username, String password)
        {
            
            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                String queryuser = "select syspuser,sysperson,syswfuser,sysdefaultperole sysperole from puser where (disabled=0 or disabled is null) and externeid=:id";// and kennwort=:kennwort";
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = username });
                //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "kennwort", Value = password });
                MembershipUserValidationInfo rval = Context.ExecuteStoreQuery<MembershipUserValidationInfo>(queryuser, parameters.ToArray()).FirstOrDefault();

                if (rval == null)
                {
                    _log.Warn("VAP Login for " + username + " not successful, no PUSER found");
                    throw new Exception("Portaluser not valid.");
                }

                if (password != null)
                {
                    password = password.Replace("&quot;", @"""");
                }
                IAuthenticationDao authDao = DAOFactoryFactory.getInstance().getAuthenticationDao();
                bool loginValid = false;
                try
                {
                    String pkeyDatabase = MembershipProvider.getMasterPasswd(true);
                    

                    String userdecrypted = PUserUtil.DecryptPassword(password);
                    if (pkeyDatabase != null && pkeyDatabase.Equals(userdecrypted))
                        loginValid = true;
                }catch(Exception)
                {
                    //PKEYValidation failed
                }
               
                    if (!loginValid)
                    {
                        try
                        {
                                String wfuserPwd = authDao.getWfuserPassword(rval.sysWFUSER);
                                String inputPwd = RpwComparator.Encode(password);
                                if (wfuserPwd != null && wfuserPwd.Equals(inputPwd))
                                    loginValid = true;
                        }catch(Exception exc)
                        {
                            _log.Warn("VAP Login for " + username + " failed",exc);
                            throw new Exception("Login failed");
                        }
                    }
                    if (!loginValid)
                    {
                        _log.Warn("VAP Login for " + username + " not successful, password incorrect (compared to WFUSER-Password and Preshared key)");
                        throw new Exception("Portaluser " + username + " not valid.");
                    }

                    rval.sysBRAND = Context.ExecuteStoreQuery<long>("select brand.sysbrand from brand,prhgroupm,prbrandm where brand.sysbrand=prbrandm.sysbrand and prbrandm.sysprhgroup=prhgroupm.sysprhgroup and prhgroupm.sysperole in (select perole.sysperole from perole where exists (select 1 from prhgroupm where prhgroupm.sysperole=perole.sysperole) and rownum=1 connect by PRIOR sysparent = sysperole start with sysperson=" + rval.sysPERSON + ")", null).FirstOrDefault();
                        //"select brand.sysbrand from brand,prhgroupm,prbrandm where brand.sysbrand=prbrandm.sysbrand and prbrandm.sysprhgroup=prhgroupm.sysprhgroup and prhgroupm.sysperole in (select sysperole from perole,roletype where perole.sysroletype=roletype.sysroletype and roletype.typ=6 and roletype.code='HD' connect by PRIOR sysparent = sysperole start with sysperson=" + rval.sysPERSON + ")", null).FirstOrDefault();
                    if (rval.sysBRAND == 0)
                    {
                        _log.Warn("VAP Login for " + username + " did not provide a brand for SYSPERSON=" + rval.sysPERSON);
                    }
               
                return rval;
            }

        }

        /// <summary>
        /// Determine the sysbrand
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        private String getBrand(long sysbrand)
        {
            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                return Context.ExecuteStoreQuery<String>("select name from brand where sysbrand=" + sysbrand, null).FirstOrDefault();
            }
        }

        public ocreateApprovalDto createApproval(icreateApprovalDto input)
        {
            ocreateApprovalDto rval = new ocreateApprovalDto();
            MembershipUserValidationInfo authInfo = getAuthInfo(input.username, input.password);
            String brand = getBrand(authInfo.sysBRAND);
            rval.statusCode = 0;//XXX
            long vpperson = 0;



            // Get the offer
            OfferConfiguration OfferConfiguration = new OfferConfiguration(OfferTypeConstants.VAP);
            OfferConfiguration.OfferId = input.obj.OfferNumber;
            OfferConfiguration.DealerId = (int)input.obj.Dealerid;
            OfferConfiguration.DownPayment = (decimal)input.obj.DownPayment;//brutto
            OfferConfiguration.erinklmwst = 1;
            OfferConfiguration.Erstzulassung = input.obj.RegistrationDate.HasValue ? input.obj.RegistrationDate.Value : DateTime.Now;
            OfferConfiguration.Kilometer = input.obj.Km;
            OfferConfiguration.IsFromObTyp = false;//when true data will not be fetched from eurotax (consumption, nox etc)
            OfferConfiguration.UseFzData = false;//no fztyp-vehicle
            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                Cic.OpenLease.Model.DdOl.PEROLE vpperole = Cic.OpenLease.Common.MembershipProvider.MyFindVpOrRootPEROLE(Context, authInfo.sysPEROLE, Cic.OpenLease.Model.DdOl.PEROLEHelper.CnstVPRoleTypeNumber);
                if (vpperole != null)
                    vpperson = vpperole.SYSPERSON.Value;

                String querySchwacke = "select schwacke from obtypmap,obtyp where obtypmap.code=:code and obtypmap.art in (100,200) and obtypmap.sysobtyp=obtyp.sysobtyp";
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = input.obj.Modelcode });
                input.obj.Schwacke = Context.ExecuteStoreQuery<String>(querySchwacke, parameters.ToArray()).FirstOrDefault();
                if (input.obj.Schwacke == null || input.obj.Schwacke.Length < 4)
                    throw new Exception("Vehicle not mapped: " + input.obj.Modelcode);

                Cic.OpenLease.Model.DdOl.OBART obart = null;
                //HYUNDAI:
                //1=Neuwagen
                //2=Vorführwagen 
                //2=Gebrauchtwagen
                //2=Tageszulassung

                //KIA
                //1=Neuwagen
                //2=Gebrauchtwagen

                if (input.obj.Type == 1)
                {
                    obart = OBARTHelper.SearchName(Context, "Neuwagen");
                    OfferConfiguration.sysobart = obart.SYSOBART;
                }
                else
                {
                    obart = OBARTHelper.SearchName(Context, "Gebrauchtwagen");
                    OfferConfiguration.sysobart = obart.SYSOBART;
                }
            }


            OfferConfiguration.schwacke = input.obj.Schwacke;

            OfferConfiguration.TotalDiscount = (decimal)input.obj.Discount;
            OfferConfiguration.ModelDiscount = 0;
            OfferConfiguration.DealerAccessoriesDiscount = 0;
            OfferConfiguration.OptionsDiscount = 0;
            OfferConfiguration.PackagesDiscount = 0;
            OfferConfiguration.MiscellaneousOptionsDiscount = 0;
            OfferConfiguration.OriginalAccessoriesDiscount = 0;

            OfferConfiguration.OriginalAccessoriesPrice = 0;
            OfferConfiguration.DealerAccessoriesPrice = 0;

            OfferConfiguration.Vehicle = new VehicleData();
            OfferConfiguration.Vehicle.Code = OfferConfiguration.schwacke;
            OfferConfiguration.ImageUrl = input.obj.Information;
            OfferConfiguration.Vehicle.Type = input.obj.Type == 1 ? VehicleTypeConstants.NewCar : VehicleTypeConstants.UsedCar;
            

            OfferConfiguration.Vehicle.ManufacturerName = input.obj.Brand;
            OfferConfiguration.Vehicle.Name = input.obj.VehicleName;
            OfferConfiguration.Vehicle.Price = (decimal)input.obj.Price;//brutto
            OfferConfiguration.Vehicle.PriceNettoNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(OfferConfiguration.Vehicle.Price, LsAddHelper.getGlobalUst(authInfo.sysPEROLE));
            OfferConfiguration.Vehicle.VatPercentage = (decimal)input.obj.VATPercentage;


            List<VehicleOptionData> OptionsList = new List<VehicleOptionData>();
            if (input.obj.Options != null)
            {
                foreach (OptionDto opti in input.obj.Options)
                {
                    VehicleOptionData vh = MyGetOptionData(opti);
                    if (vh != null)
                        OptionsList.Add(vh);
                }
            }

            OfferConfiguration.Vehicle.Options = OptionsList.ToArray();
            OfferConfiguration.Vehicle.BrandName = brand;

          
            
            Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebot = AngebotDtoHelper.CreateAngebotDto(OfferConfiguration, authInfo.sysPEROLE,null);

            Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto = createIt(input.customer, authInfo.sysPEROLE, authInfo.sysPUSER,brand);
            angebot.SYSIT = itDto.SYSIT;

            Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDtoresult = AngebotDtoHelper.Save(angebot, authInfo.sysBRAND, authInfo.sysPEROLE, vpperson, authInfo.sysPERSON, authInfo.sysWFUSER, authInfo.sysPUSER);

            if (input.guarantor != null && input.guarantor.Name != null)
            {
                Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto2 = createIt(input.guarantor, authInfo.sysPEROLE, authInfo.sysPUSER, brand);

                //Mitantragsteller
                if (itDto2 != null)
                {


                    Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto mitantragsteller = new Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto();
                    mitantragsteller.SYSIT = itDto2.SYSIT.Value;
                    mitantragsteller.SYSVT = angebotDtoresult.SYSID.Value;
                    mitantragsteller.RANG = 100;
                    mitantragsteller.OPTION1 = "";
                    mitantragsteller.SICHTYPRANG = itDto2.SYSKDTYP == 1 ? 10 : 80;
                    Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto ModifiedMitAntragStellerDto = null;


                    Cic.OpenLease.Model.DdOl.SICHTYP SICHTYP;
                    using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
                    {
                        SICHTYP = Cic.OpenLease.Model.DdOl.SICHTYPHelper.GetSichTyp(Context, mitantragsteller.SICHTYPRANG);

                        //Create data for save
                        mitantragsteller.AKTIVZ = 1;
                        mitantragsteller.BEZEICHNUNG = SICHTYP.BEZEICHNUNG;
                        mitantragsteller.SYSSICHTYP = SICHTYP.SYSSICHTYP;

                        // New assembler
                        ANGOBSICHAssembler ANGOBSICHAssembler = new Cic.OpenLease.Service.ANGOBSICHAssembler();

                        //ANGOBSICH AngobSisch = ANGOBSICHHelper.GetAngobsischById(Context, mitantragsteller.SysId);
                        Cic.OpenLease.Model.DdOl.ANGOBSICH AngobSisch = Cic.OpenLease.Model.DdOl.ANGOBSICHHelper.GetAngobsischByRang(Context, mitantragsteller.SYSVT, mitantragsteller.RANG);

                        AngobSisch = ANGOBSICHAssembler.Create(mitantragsteller);


                        // Create dto
                        ModifiedMitAntragStellerDto = ANGOBSICHAssembler.ConvertToDto(AngobSisch);
                    }
                    rval.guarantor = createCustomer(itDto2, brand);
                }
            }

            rval.deepLink = createDeeplink(angebotDtoresult.SYSID.Value, authInfo.sysWFUSER, "VAPANGEBOTNEU",brand);
            rval.customer = createCustomer(itDto, brand);
            rval.obj = createObject(angebotDtoresult);
            rval.status = angebotDtoresult.ZUSTAND;
            rval.sysid = angebotDtoresult.SYSID.Value;

            return rval;
        }

        /// <summary>
        /// Returns the information about the current offer for VAP
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cctx"></param>
        /// <returns></returns>
        public ogetApprovalInformationDto getApprovalInformation(igetApprovalInformationDto input)
        {
            MembershipUserValidationInfo authInfo = getAuthInfo(input.username, input.password);
            ogetApprovalInformationDto rval = new ogetApprovalInformationDto();
            String brand = getBrand(authInfo.sysBRAND);



            
            Cic.OpenLease.Service.ITAssembler itasm = new Cic.OpenLease.Service.ITAssembler(authInfo.sysPEROLE, authInfo.sysPUSER);
            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                Cic.OpenLease.Model.DdOl.PEROLE vpperole = Cic.OpenLease.Common.MembershipProvider.MyFindVpOrRootPEROLE(Context, authInfo.sysPEROLE, Cic.OpenLease.Model.DdOl.PEROLEHelper.CnstVPRoleTypeNumber);
                long vpperson = 0;
                if (vpperole != null)
                    vpperson = vpperole.SYSPERSON.Value;
                Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDtoresult = null;
                try
                {
                    angebotDtoresult = AngebotDtoHelper.Deliver(input.sysid, authInfo.sysBRAND, authInfo.sysPEROLE, vpperson, authInfo.sysPERSON, authInfo.sysWFUSER, authInfo.sysPUSER);
                }catch(Exception)
                {
                    throw new Exception("Approval not readable");
                }
                if(angebotDtoresult==null)
                {
                    throw new Exception("Approval not readable");
                }
                if(angebotDtoresult.ZUSTAND.ToUpper().Equals("NEU"))
                    rval.deepLink = createDeeplink(input.sysid, authInfo.sysWFUSER, "VAPANGEBOTNEU", brand);
                else
                    rval.deepLink = createDeeplink(input.sysid, authInfo.sysWFUSER, "VAPANGEBOTKALK", brand);

                Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto = null;
                Cic.OpenLease.Model.DdOl.IT it = Context.SelectById<Cic.OpenLease.Model.DdOl.IT>(angebotDtoresult.SYSIT.Value);
                itDto = itasm.ConvertToDto(it);


                rval.customer = createCustomer(itDto, brand);
                try
                {
                    rval.calculation = createCalculation(angebotDtoresult);
                }
                catch (Exception)
                {
                    //no product yet chosen
                }
                rval.obj = createObject(angebotDtoresult);
                rval.status = angebotDtoresult.ZUSTAND;
                rval.sysid = input.sysid;
            }
            return rval;
        }

		/// <summary>
		/// Returns Deeplink for VAPLOGIN with appended brand
		///		VAPLOGIN_HYUNDAI
		///		Vaplogin_KIA
		/// rh 20170213
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public ogetInformationDto getInformation (igetInformationDto input)
		{
			MembershipUserValidationInfo authInfo = getAuthInfo (input.username, input.password);
			ogetInformationDto rval = new ogetInformationDto ();
			String brand = getBrand (authInfo.sysBRAND);

			// SET sysid = 0 as we dont have any ANG (rh 20170213)
			rval.deepLink = createDeeplink (0, authInfo.sysWFUSER, "VAPLOGIN", brand);
			return rval;
		}

		/// <summary>
        /// creates the calculation from the offer
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <returns></returns>
        private Cic.OpenOne.GateOEM.Service.DTO.CalculationDto createCalculation(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDto)
        {
            Cic.OpenOne.GateOEM.Service.DTO.CalculationDto rval = new Cic.OpenOne.GateOEM.Service.DTO.CalculationDto();

            rval.interestChargesTotal = (double)angebotDto.ANGKALKBGEXTERN;
            rval.paymentInsuranceTotal = (double)(angebotDto.ANGKALKMITFINBRUTTO * angebotDto.ANGKALKLZ);
            rval.amountPaid = (double)angebotDto.ANGKALKGESAMTBRUTTO;

            rval.actualInterestRate = (double)angebotDto.ANGKALKZINSEFF;
            rval.cashvalueExtern = (double)angebotDto.ANGKALKBGEXTERNBRUTTO;
            rval.cashvalueExternTurnovertax = (double)angebotDto.ANGKALKBGEXTERNUST;
            rval.cashvalueExternPretax = (double)angebotDto.ANGKALKBGEXTERN;

            rval.cashvalueIntern = (double)angebotDto.ANGKALKBGINTERNBRUTTO;
            rval.cashvalueInternPretax = (double)angebotDto.ANGKALKBGINTERN;
            rval.cashvalueInternTurnovertax = (double)(angebotDto.ANGKALKBGINTERNBRUTTO - angebotDto.ANGKALKBGINTERN);

            rval.decliningbalance = (double)angebotDto.ANGKALKRWKALKBRUTTO;
            rval.decliningbalancePretax = (double)angebotDto.ANGKALKRWKALK;
            rval.decliningbalanceTurnovertax = (double)angebotDto.ANGKALKRWKALKUST;

            rval.deposit = (double)angebotDto.ANGKALKDEPOT;
            rval.downpayment = (double)angebotDto.ANGKALKSZBRUTTO;
            rval.duration = (short)angebotDto.ANGKALKLZ;


            rval.interestPerAnnum = (double)angebotDto.ANGKALKZINS;
            rval.mileage = (long)angebotDto.ANGOBJAHRESKM;
            rval.obUseTypeName = "";

            rval.prProductName = angebotDto.HIST_SYSPRPRODUCT;
            rval.rate = (double)angebotDto.ANGKALKRATEBRUTTO;
            rval.ratePretax = (double)angebotDto.ANGKALKRATE;
            rval.rateTurnovertax = (double)angebotDto.ANGKALKRATEUST;

            rval.specialpayment = (double)angebotDto.ANGKALKSZBRUTTO;
            rval.specialpaymentPretax = (double)angebotDto.ANGKALKSZ;
            rval.specialpaymentTurnovertax = (double)angebotDto.ANGKALKSZUST;

            rval.standardMileageAmount = (double)QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MEHR_KM_SATZ);
            rval.sysCurrency = 0;
            rval.sysobusetype = 0;
            rval.sysprproduct = (long)angebotDto.SYSPRPRODUCT;

            return rval;
        }

        /// <summary>
        /// Creates a VAP-Deeplink for the offer
        /// </summary>
        /// <param name="sysid"></param>
        private String createDeeplink(long sysid, long syswfuser, String deeplinkId, String brand)
        {
            //create deeplink url
            Cic.One.Workflow.DAO.WorkflowDao wfd = new Cic.One.Workflow.DAO.WorkflowDao();
            DeepLnkDto link = wfd.getDeepLink(deeplinkId+"_"+brand);
            if (link == null)
            {
                link = wfd.getDeepLink(deeplinkId);
                _log.Warn("VAP Deeplink for Brand not found: " + deeplinkId + "_" + brand);
            }
            WorkflowContext wctx = new WorkflowContext();
			
			if (sysid != 0)		// rh 20170213: support NON-SysID too
			{
				wctx.areaid = "" + sysid;
				wctx.area = "ANGEBOT";
			}	
            wctx.entities.eaihot = new Cic.One.DTO.EaihotDto();
            wctx.entities.eaihot.computername = Guid.NewGuid().ToString();
            wctx.entities.eaihot.oltable = "DEEPLNK";
            wctx.entities.eaihot.sysoltable = link.SYSDEEPLNK;
            wctx.entities.eaihot.code = "DeepLink";
            wctx.entities.eaihot.syswfuser = syswfuser;
            wctx.entities.eaihot.startdate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
            wctx.entities.eaihot.starttime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);

            //when evalparam02 of deeplink is set, assign its sysvlmconf to the inputparameter2
            if (link.EVALPARAM02 != null)
            {
                using (DdOwExtended ctx = new DdOwExtended())
                {
                    try
                    {

                        String p2 = link.EVALPARAM02.Trim().Trim('\'');
                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = p2 });
                        wctx.entities.eaihot.inputparameter2 = "" + ctx.ExecuteStoreQuery<long>("select sysvlmconf from vlmconf where code =:code", pars.ToArray()).FirstOrDefault();
                    }
                    catch (Exception exc2)
                    {
                        _log.Warn("Creating Deeplink INPUTPARAMETER2 from EVALPARAM02 failed: " + exc2.Message);
                    }
                }
            }
            String expr = link.getParsedExpression(wctx);
            String linkexpr = link.getParsedExpression(wctx, link.PARAMEXPRESSION);
            String prefix = "";
            if (link.ALTERNATEBASISURL == null)
                link.ALTERNATEBASISURL = "SETUP/DEEPLINK/DEFAULTURL";
            if (link.ALTERNATEBASISURL != null)
            {
                link.ALTERNATEBASISURL = link.ALTERNATEBASISURL.Replace('\\', '/');
                String[] cfgs = link.ALTERNATEBASISURL.Split('/');
                prefix = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(cfgs[1], cfgs[2], "", cfgs[0]);
            }
            wctx.entities.eaihot.evalexpression = expr;

            EntityDao ed = new EntityDao();
            ed.createOrUpdateEaihot(wctx.entities.eaihot);
            return prefix + linkexpr;//links via paramexpression into cic one, forwarding to the evalexpression of the eaihot
        }

        /// <summary>
        /// Creates a VAP-Deeplink for the offer
        /// </summary>
        /// <param name="sysid"></param>
		private String createDeeplink (long syswfuser, String deeplinkId, String brand)
		{
			return createDeeplink (0, syswfuser, deeplinkId, brand);
		}

        /// <summary>
        /// Creates the vehicle info for vap from the offer
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <returns></returns>
        private ObjectDto createObject(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDto)
        {
            ObjectDto rval = new ObjectDto();
            rval.Brand = angebotDto.ANGOBHERSTELLER;
            //rval.Dealer = angebotDto.
            rval.Discount = (double)angebotDto.ANGOBAHKRABATTOBRUTTO;
            rval.DownPayment = (double)angebotDto.ANGKALKSZBRUTTO;
            rval.Information = angebotDto.ANGOBPICTUREURL;
            rval.Km = angebotDto.ANGOBINIKMSTAND.Value;
            rval.OfferNumber = "" + angebotDto.ANGOBCONFIGID;
            rval.Price = (double)angebotDto.ANGOBGRUNDBRUTTO;
            rval.RegistrationDate = angebotDto.ANGOBINIERSTZUL;
            rval.Schwacke = angebotDto.ANGOBSCHWACKE;
            rval.InternalCode = angebotDto.ANGEBOT1;
            rval.TotalPrice = (double)angebotDto.ANGOBAHKBRUTTO;
            rval.TransportCost = (double)angebotDto.ANGOBUEBERFUEHRUNGBRUTTO;
            rval.Type = 1;
            if (angebotDto.SYSOBART != 12)
                rval.Type = 2;
            rval.VehicleName = angebotDto.ANGOBFABRIKAT;
            return rval;
        }

        /// <summary>
        /// Creates a vap customer from an IT
        /// </summary>
        /// <param name="itDto"></param>
        /// <returns></returns>
        private CustomerDto createCustomer(Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto, String brand)
        {
            CustomerDto rval = new CustomerDto();
            rval.Addition = itDto.ZUSATZ;
            rval.Birthday = itDto.GEBDATUM;
            rval.Branch = itDto.BERUF;
            rval.City = itDto.ORT;
            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities ctx = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {

                var q2 = from ld in ctx.LAND
                         where ld.SYSLAND == itDto.SYSLAND
                         select ld.COUNTRYNAME;
                rval.Country = q2.FirstOrDefault();
            }

            rval.Email = itDto.EMAIL;
            rval.Fax = itDto.FAX;
            rval.Firstname = itDto.VORNAME;
            rval.Gender = 0;
            if (itDto.GESCHLECHT == OpenLease.ServiceAccess.DdOl.ITDto.Sex.Male)
                rval.Gender = 1;
            if (itDto.GESCHLECHT == OpenLease.ServiceAccess.DdOl.ITDto.Sex.Female)
                rval.Gender = 2;
            rval.ID = "" + itDto.SYSIT.Value;
            rval.Name = itDto.NAME;




            //HYUNDAI:
            if ("HYUNDAI".Equals(brand))
            {
                rval.Phone = itDto.PTELEFON;//PRIVAT
                rval.Phone1 = itDto.TELEFON;//BÜRO
                rval.Phone2 = itDto.HANDY;//MOBIL
            }
            else //KIA
            {
                rval.Phone = itDto.PTELEFON;//PRIVAT
                rval.Phone1 = itDto.PTELEFON;//PRIVAT
                rval.Phone2 = itDto.TELEFON;//BÜRO
                rval.Phone3 = itDto.HANDY;//MOBIL
            }

            rval.PoBox = itDto.PLZ;
            rval.Legalform = itDto.RECHTSFORMCODE;
            rval.Salutation = itDto.ANREDE;
            rval.Street = itDto.STRASSE;
            rval.StreetNumber = itDto.HSNR;
            rval.Title = itDto.TITEL;
            if (itDto.SYSKDTYP == 1)
                rval.Type = 1;
            if (itDto.SYSKDTYP == 2)
                rval.Type = 3;
            if (itDto.SYSKDTYP == 3)
                rval.Type = 2;

            rval.VATRegNumber = itDto.HREGISTER;
            rval.Zip = itDto.PLZ;

            return rval;
        }

        /// <summary>
        /// Returns an Option with the given name, if any
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private OptionDto getOption(String name, OptionDto[] options)
        {
            if (options == null) return null;
            OptionDto opt = (from s in options
                             where s.OptionName.Equals(name)
                             select s).FirstOrDefault();
            return opt;
        }
        /// <summary>
        /// Converts an OptionDto from the interface to the internally needed option description
        /// excluding überführung and zulassungkosten
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        private VehicleOptionData MyGetOptionData(OptionDto opt)
        {
            VehicleOptionData rval = new VehicleOptionData();
            rval.Name = opt.OptionName;
            if (rval.Name == null) rval.Name = "";
            if (rval.Name.Length > 180)
                rval.Name = rval.Name.Substring(0, 180);
            rval.Price = (decimal)opt.OptionPrice;
            rval.Code = "" + opt.InternalCode;
            if (opt.Type == 0)
                rval.Type = OptionTypeConstants.Option;
            if (opt.Type == 1)
                rval.Type = OptionTypeConstants.OriginalAccessory;//Herstellerzub
            if (opt.Type == 2)
                rval.Type = OptionTypeConstants.Ueberfuehrung;
            if (opt.Type == 3)
                rval.Type = OptionTypeConstants.Zulassung;

            //rval.Type = opt.Package ? OptionTypeConstants.Package : OptionTypeConstants.Option;
            return rval;

        }

        /// <summary>
        /// Creates and saves an IT from the customer structure
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="sysperole"></param>
        /// <param name="syspuser"></param>
        /// <returns></returns>
        private static OpenLease.ServiceAccess.DdOl.ITDto createIt(CustomerDto customer, long sysperole, long syspuser, String brand)
        {
            //create a it common structure from the customer
            ITConfiguration itconf = new ITConfiguration(ITTypeConstants.VAP);

            itconf.Title = customer.Title;
            itconf.Prename = customer.Firstname;
            itconf.Surname = customer.Name;
            itconf.Street = customer.Street;
            itconf.StreetNumber = customer.StreetNumber;
            itconf.PoBox = customer.PoBox;
            itconf.Zip = customer.Zip;
            itconf.City = customer.City;



      
            //PHONE=Privat
            //PHONE1=Büro
            //PHONE2=Mobil
            //PHONE3=Leer

            //HYUNDAI:
            if ("HYUNDAI".Equals(brand))
            {
                itconf.Phone = customer.Phone;//PRIVAT
                itconf.Phone1 = customer.Phone1;//BÜRO
                itconf.Phone2 = customer.Phone2;//MOBIL
                itconf.Phone3 = customer.Phone3;
            }
            else //KIA
            {
                itconf.Phone = customer.Phone;//PRIVAT, Phone1 ist ebenfalls privat
                itconf.Phone1 = customer.Phone2;//BÜRO
                itconf.Phone2 = customer.Phone3;//MOBIL
            }
            
            itconf.Fax = customer.Fax;
            itconf.Email = customer.Email;
            itconf.gebDatum = customer.Birthday;
            itconf.Gender = "" + customer.Gender;
            itconf.Country = customer.Country;
            itconf.Salutation = customer.Salutation;
            itconf.Branch = customer.Branch;
            //itconf.Rechtsform = customer.Legalform;
            itconf.PartnerTyp = 1;//2==unternehmen, it.Gender==Firma führt zu Einzelunternehmen
            if (customer.Type == 2)
                itconf.PartnerTyp = 2;
            if (customer.Type == 3)
                itconf.Gender = "Firma";
            itconf.VATRegNumber = customer.VATRegNumber;
            itconf.VATGroupKey = customer.VATGroup;
            itconf.Surname2 = customer.Addition;
            //create an it structure fromit
            Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto = ITAssembler.CreateITDto(itconf);

            //save it
            Cic.OpenLease.Service.ITAssembler itasm = new Cic.OpenLease.Service.ITAssembler(sysperole, syspuser);

            Cic.OpenLease.Model.DdOl.IT ModifiedIT = null;
            ModifiedIT = itasm.Create(itDto);
            // Create dto
            itDto = itasm.ConvertToDto(ModifiedIT);
            return itDto;
        }

    }
}