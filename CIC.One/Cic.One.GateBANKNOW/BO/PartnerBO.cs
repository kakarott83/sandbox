using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.One.DTO;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.Model.DdOw;
using System.Data.EntityClient;
using System.Data.Common;
using Dapper;
using Cic.One.GateBANKNOW.DTO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.One.GateBANKNOW.BO
{
    public class PartnerBO
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime nullDate = new DateTime(1800, 1, 1);
        public static readonly String CONFIGSOURCE = AngAntBo.CONFIGSOURCE_PFC;
        public const String DLCODE_ERF = "EPOSPARTNER_ERF";
        public const String DLCODE_DETAIL = "EPOSPARTNER_DETAIL";

        /// <summary>
        /// Authenticates the given Partner VK and HNDL Id
        /// </summary>
        /// <param name="extuserid"></param>
        /// <param name="extdealerid"></param>
        /// <returns></returns>
        private MembershipUserValidationInfo getAuthInfo(String extuserid, String extdealerid)
        {

            using (PrismaExtended  Context = new PrismaExtended())
            {
                String queryuser = "select perole.sysperson,perole.sysperole,perole.sysparent sysbrand from peoption,roletype,perole where roletype.sysroletype=perole.sysroletype and roletype.typ=6 and peoption.SYSID=perole.sysperson  and peoption.str03=:id";
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = extdealerid });
                //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "kennwort", Value = password });
                MembershipUserValidationInfo rvalHndl = Context.ExecuteStoreQuery<MembershipUserValidationInfo>(queryuser, parameters.ToArray()).FirstOrDefault();

                if (rvalHndl == null)
                {
                    _log.Warn("BNPartner F00025_EXTUSER_INVALID Call for " + extdealerid + " not successful, no PUSER found");
                    throw new ServiceBaseException("F00025_EXTUSER_INVALID", "Der Extdealer ist nicht über die externen IDs im System auffindbar", MessageType.Error);
                }
               


                queryuser = "select syspuser,perole.sysperson,syswfuser,perole.sysperole from puser,peoption,roletype,perole where perole.sysperson=puser.sysperson and roletype.sysroletype=perole.sysroletype and roletype.typ=7 and peoption.SYSID=puser.sysperson and (disabled=0 or disabled is null) and peoption.str03=:id and perole.sysparent=:hndlperole";
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = extuserid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "hndlperole", Value = rvalHndl.sysPEROLE });
                
                //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "kennwort", Value = password });
                MembershipUserValidationInfo rval = Context.ExecuteStoreQuery<MembershipUserValidationInfo>(queryuser, parameters.ToArray()).FirstOrDefault();

                if (rval == null)
                {
                    _log.Warn("BNPartner F00025_EXTUSER_INVALID Call for " + extuserid + " not successful, no PUSER found");
                    throw new ServiceBaseException("F00025_EXTUSER_INVALID", "Der Extuser ist nicht dem strategischem Partner zugeordnet oder nicht über die externen IDs im System auffindbar.", MessageType.Error);
                }


                


                rval.sysBRAND = getBrand(rval.sysPEROLE);
                if (rval.sysBRAND == 0)
                {
                    _log.Warn("BNPartner Call for " + extuserid + " did not provide a brand for SYSPERSON=" + rval.sysPERSON);
                }

                return rval;
            }

        }

        /// <summary>
        /// Fetch the brand for the perole
        /// assumes the perole is a direct child of the hndl perole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public long getBrand(long sysperole)
        {
            
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                return olCtx.ExecuteStoreQuery<long>("select distinct sysbrand from CIC.PRHGROUPM,prbrandm where prbrandm.sysprhgroup=prhgroupm.sysprhgroup and prhgroupm.sysperole="+sysperole+" and sysbrand>1").FirstOrDefault();
                /*long sysVpPerole = OpenOne.Common.Model.DdOl.PeRoleUtil.FindRootPEROLEByRoleType(olCtx, sysperole, (long)OpenOne.Common.Model.DdOl.RoleTypeTyp.HAENDLER);
                DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);

                var query = from BRAND in olCtx.BRAND // Selektiere alle BRANDs
                            join PRBRANDM in olCtx.PRBRANDM on BRAND.SYSBRAND equals PRBRANDM.BRAND.SYSBRAND // aus der Liste der Brands al ler
                            join PRHGROUPM in olCtx.PRHGROUPM on PRBRANDM.PRHGROUP.SYSPRHGROUP equals PRHGROUPM.PRHGROUP.SYSPRHGROUP // Handelsgruppen des Verkäufers
                            join PEROLEVP in olCtx.PEROLE on PRHGROUPM.PEROLE.SYSPEROLE equals PEROLEVP.SYSPEROLE // Verkäuferrolle
                            where PEROLEVP.ROLETYPE.TYP == (int)OpenOne.Common.Model.DdOl.RoleTypeTyp.HAENDLER  // Einschränkung für Verkäuferrolle
                            && PEROLEVP.SYSPEROLE == sysVpPerole // Konkreter Verkäufer
                            && PRHGROUPM.ACTIVEFLAG == 1
                            && (PRHGROUPM.VALIDFROM == null || PRHGROUPM.VALIDFROM <= aktuell || PRHGROUPM.VALIDFROM <= nullDate)
                            && (PRHGROUPM.VALIDUNTIL == null || PRHGROUPM.VALIDUNTIL >= aktuell || PRHGROUPM.VALIDUNTIL <= nullDate)
                            orderby PRHGROUPM.DEFAULTFLAG descending, BRAND.SYSBRAND
                            select BRAND;


                foreach (CIC.Database.OL.EF4.Model.BRAND item in query)
                {
                    if (item.ACTIVEFLAG == 1)
                    {
                        return item.SYSBRAND;
                    }
                }
                */

            }
           
        }

        
        /// <summary>
        /// Determine the Brand name for the sysbrand
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        private String getBrandName(long sysbrand)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<String>("select name from brand where sysbrand=" + sysbrand, null).FirstOrDefault();
            }
        }
        
        /// <summary>
        /// Retrieve the proposals Deeplink
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public String getLink(igetLinkDto input)
        {
            long sysid = 0;
            String dlcode = DLCODE_ERF;
            
            using (PrismaExtended ctx = new PrismaExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                sysid= con.Query<long>("select sysid from antrag where extreferenz=:extref order by sysid desc",new { extref = input.extreferenz }).FirstOrDefault();
                //wenn kein Datensatz für input.getextReferenz gefunden, dann leerer Link mit errorcode = I0001_DATA_INVALID  zurückgeben
                if (sysid == 0)
                {
                    _log.Warn("BNPartner I0001_DATA_INVALID for extreferenz " + input.extreferenz);
                    throw new ServiceBaseException("I0001_DATA_INVALID", "", MessageType.Error);
                }
                String zustand = con.Query<String>("select zustand from antrag where sysid=:sysid", new { sysid = sysid }).FirstOrDefault();
                if (!"NEU".Equals(zustand.ToUpper()))
                    dlcode = DLCODE_DETAIL;
            }
            

            MembershipUserValidationInfo authInfo = getAuthInfo(input.extuserid , input.extdealerid);
            String brand = getBrandName(authInfo.sysBRAND);
            //Do not set the wfuser, we want the deeplink use the SSO WFUSER
            return createDeeplink(sysid, 0, dlcode, brand);
        }
        
        /// <summary>
        /// Creates a Deeplink for the offer
        /// </summary>
        /// <param name="sysid"></param>
        private String createDeeplink(long sysid, long syswfuser, String deeplinkId, String brand)
        {
            //create deeplink url
            Cic.One.Workflow.DAO.WorkflowDao wfd = new Cic.One.Workflow.DAO.WorkflowDao();
            DeepLnkDto link = wfd.getDeepLink(deeplinkId);// + "_" + brand);
            if (link == null)
            {
                link = wfd.getDeepLink(deeplinkId);
                _log.Warn("Deeplink for Brand not found: " + deeplinkId + "_" + brand);
                return null;
            }
            WorkflowContext wctx = new WorkflowContext();

            if (sysid != 0)     // rh 20170213: support NON-SysID too
            {
                wctx.areaid = "" + sysid;
                wctx.area = "ANTRAG";
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
        /// Creates the new proposal
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ServiceBaseException createAntrag(icreateAntragDto input)
        {
            MembershipUserValidationInfo authInfo = getAuthInfo(input.extuserid, input.extdealerid);
            long sysperole = authInfo.sysPEROLE;
            AuthenticationBo.validateActivePerole(sysperole);

            using (PrismaExtended ctx = new PrismaExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                long sysid = con.Query<long>("select sysid from antrag where extreferenz=:extref order by sysid desc", new { extref = input.antrag.extreferenz }).FirstOrDefault();
                //bereits mit dieser Referenz angelegt
                if (sysid >0)
                {
                    _log.Warn("BNPartner F0001_DUPLICATE_OPPORTUNITY_ID" + input.antrag.extreferenz);
                    throw new ServiceBaseException("F0001_DUPLICATE_OPPORTUNITY_ID", "", MessageType.Error);
                }
            }
            

            ServiceBaseException rval = null;
            List<ServiceBaseException> rvals = new List<ServiceBaseException>();
            String language = input.ISOLanguageCode;
            if (language == null || language.Length == 0)
                language = "de-CH";
            input.antrag.gueltigBis = null;
            OpenOne.Common.BO.Prisma.IPrismaProductBo productBo = OpenOne.Common.BO.Prisma.PrismaBoFactory.getInstance().createPrismaProductBo(OpenOne.Common.BO.Prisma.PrismaProductBo.CONDITIONS_BANKNOW, language);
            PRPRODUCT product = productBo.getProduct(input.antrag.kalkulation.angAntKalkDto.sysprproduct);
            if (product == null)
            {
                _log.Warn("BNPartner ceateAntrag W00001_PRODUCT_INVALID (not found): " + input.antrag.kalkulation.angAntKalkDto.sysprproduct);
                rvals.Add(new ServiceBaseException("W00001_PRODUCT_INVALID", "", MessageType.Error));                
            }
            VART vart = productBo.getVertragsart(product.SYSPRPRODUCT);
            if (vart == null)
            {
                _log.Warn("BNPartner ceateAntrag W00001_PRODUCT_INVALID (no VART): " + input.antrag.kalkulation.angAntKalkDto.sysprproduct);
                rvals.Add(new ServiceBaseException("W00001_PRODUCT_INVALID", "", MessageType.Error));
            }
            OpenOne.Common.BO.IMwStBo mwstBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createMwstBo();
            OpenOne.Common.BO.IRounding round = OpenOne.Common.BO.RoundingFactory.createRounding();
            String Code = vart.CODE.ToUpper();
            bool istzk = (Code.IndexOf("TZK") > -1);//Teilzahlungskauf ist analog Kredit zu rechnen
            bool isCredit = (Code.IndexOf("KREDIT") > -1) || istzk;
            double ust = mwstBo.getMehrwertSteuer(1, vart.SYSVART, DateTime.Now);
            double mwst = ust; //always mwst, independent of credit
            if (isCredit) ust = 0;
            input.antrag.angAntObDto.ahk=round.getNetValue(input.antrag.angAntObDto.ahkBrutto, ust);
            input.antrag.angAntObDto.ahkUst = input.antrag.angAntObDto.ahkBrutto - input.antrag.angAntObDto.ahk;
            input.antrag.angAntObDto.grund = round.getNetValue(input.antrag.angAntObDto.grundBrutto, ust);
            input.antrag.angAntObDto.grundUst = input.antrag.angAntObDto.grundBrutto - input.antrag.angAntObDto.grund;
            input.antrag.angAntObDto.jahresKm = input.antrag.kalkulation.angAntKalkDto.ll;
            if(input.antrag.angAntObDto.fzart==null)
                input.antrag.angAntObDto.fzart = "100";




            IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
            IAngAntDao aadao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDao();
            bool vinok = false;

            String vin = input.antrag.angAntObDto.brief.fident;
            //falls vin gegeben, diese analysieren
            if(vin!=null&& vin.Length>0 && vin.Length!=17)
            {
                _log.Warn("BNPartner ceateAntrag I0003_VIN_INVALID (length): " + vin);
                rvals.Add(new ServiceBaseException("I0003_VIN_INVALID", "", MessageType.Error));
                input.antrag.angAntObDto.brief.fident = "";
                vin = null;
            }
            if(vin!=null && vin.Trim().Length>3)
            {
                try
                {
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto obInfo = bo.getObjektdatenByVIN(vin, authInfo.sysWFUSER, language);
                    vinok = true;
                    if (obInfo != null && obInfo.brief != null)
                        input.antrag.angAntObDto.brief = obInfo.brief;
                    input.antrag.angAntObDto.fabrikat = obInfo.fabrikat;
                    input.antrag.angAntObDto.hersteller = obInfo.hersteller;
                    input.antrag.angAntObDto.schwacke = obInfo.schwacke;
                }
                catch(Exception e)
                {
                    input.antrag.angAntObDto.brief.fident = "";
                    _log.Warn("BNPartner ceateAntrag I0003_VIN_INVALID (fetch Objektdaten): " + vin,e);
                    rvals.Add(new ServiceBaseException("I0003_VIN_INVALID", "", MessageType.Error));
                }
            }


            String modellcode = input.antrag.angAntObDto.schwacke;
            long sysobtyp = 0;
            if (modellcode != null && modellcode.Trim().Length>3)
            {
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                    sysobtyp = con.Query<long>("select sysobtyp from obtyp where schwacke=:schwacke", new { schwacke = modellcode }).FirstOrDefault();
                    if(sysobtyp==0)
                    {
                        _log.Warn("BNPartner ceateAntrag I0004_MODELLCODE_INVALID: " + modellcode);
                        rvals.Add(new ServiceBaseException("I0004_MODELLCODE_INVALID", "", MessageType.Error));
                    }
                }
                try
                {
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto obInfo = aadao.getObjektdaten(sysobtyp);
                    if (obInfo != null && obInfo.brief != null)
                        input.antrag.angAntObDto.brief = obInfo.brief;
                    input.antrag.angAntObDto.fabrikat = obInfo.fabrikat;
                    input.antrag.angAntObDto.hersteller = obInfo.hersteller;
                    input.antrag.angAntObDto.obTypBezeichnung = obInfo.bezeichnung;
                }
                catch(Exception ex)
                {
                    _log.Warn("BNPartner ceateAntrag I0004_MODELLCODE_INVALID: " + modellcode);
                    rvals.Add(new ServiceBaseException("I0004_MODELLCODE_INVALID", "", MessageType.Error));
                }

            }
            input.antrag.angAntObDto.sysobtyp = sysobtyp;




            long sysbrand = 0;//
            byte rateError = 0;

            
            
            //Fixed Erfassungsclient
            input.antrag.erfassungsclient = Cic.OpenOne.GateBANKNOW.Common.DAO.AngAntDao.ERFASSUNGSCLIENT_B2B;
            input.antrag.angAntObDto.configsource = CONFIGSOURCE;

            IKalkulationBo kalkBo = null;

            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            
            //recalc upon save!
            if (input.antrag.kalkulation != null)
            {
                Cic.OpenOne.Common.DTO.Prisma.prKontextDto pKontext = MyCreateProductKontext(sysperole,sysbrand, input.antrag);
                _log.Debug("Calculating on Antrag Update with context: " + _log.dumpObject(pKontext));


                kalkBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKalkulationBo(language);
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = input.antrag.kalkulation;// Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(input.antrag.kalkulation);
                double rateorg = kalkulationInput.angAntKalkDto.rateBrutto;
                Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext kKontext = new Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext();
                if (input.antrag.angAntObDto != null)
                {
                    kKontext.grundBrutto = input.antrag.angAntObDto.grundBrutto;
                    kKontext.zubehoerBrutto = input.antrag.angAntObDto.zubehoerBrutto;
                }
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = kalkulationInput;
                try
                {
                    kalk =  kalkBo.calculate(kalkulationInput, pKontext, kKontext, language, ref rateError);
                }catch(Exception e)
                {
                    if (e.Message.Contains("Product not found"))
                    {
                        _log.Warn("BNPartner ceateAntrag W00001_PRODUCT_INVALID: " + kalk.angAntKalkDto.sysprproduct);
                        rvals.Add(new ServiceBaseException("W00001_PRODUCT_INVALID", "", MessageType.Error));
                    }
                    else
                    {
                        _log.Warn("BNPartner ceateAntrag I0001_DATA_INVALID - calculate failed ", e);
                        rvals.Add(new ServiceBaseException("I0001_DATA_INVALID", "", MessageType.Error));
                    }
                }
                //replace given kalkulation with the complete one
                input.antrag.kalkulation = kalk;
                if(Math.Abs(rateorg-kalk.angAntKalkDto.rateBrutto)>1)
                {
                    _log.Warn("BNPartner ceateAntrag I00002_RATE_INVALID Rate differs: "+rateorg+"!="+ kalk.angAntKalkDto.rateBrutto);
                    rvals.Add(new ServiceBaseException("I00002_RATE_INVALID", "", MessageType.Error));
                }
                if (input.antrag.angAntObDto != null)
                    input.antrag.angAntObDto.satzmehrKm = kalk.angAntKalkDto.ob_mark_satzmehrkm;
            }
            _log.Debug("Duration Kalkulation: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            
            Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antragInput = input.antrag;
            
            IKundeBo kundeBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
            if (input.antrag.kunde != null)
            {
                
                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kundeInput = input.antrag.kunde;
                if (kundeInput.kontos != null)
                    foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto kto in kundeInput.kontos)
                        kto.sysantrag = input.antrag.sysid;

                antragInput.kunde = kundeBo.createOrUpdateKunde(kundeInput,sysperole);
            }
            _log.Debug("Duration Kunde: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (input.antrag.mitantragsteller != null)
            {
                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kundeInput =input.antrag.mitantragsteller;
                if (kundeInput.kontos != null)
                    foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto kto in kundeInput.kontos)
                        kto.sysantrag = input.antrag.sysid;
                antragInput.mitantragsteller = kundeBo.createOrUpdateKunde(kundeInput, sysperole);
            }
            _log.Debug("Duration MA: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            
            _log.Debug("Duration Antrag: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;


            if (kalkBo != null)
            {
                try
                {
                    kalkBo.throwErrorMessages(rateError);
                }catch(Exception e)
                {
                    _log.Warn("BNPartner ceateAntrag I0005_KALKULATION_INVALID",e);
                    rvals.Add(new ServiceBaseException("I0005_KALKULATION_INVALID", "", MessageType.Error));
                }
            }
            bo.createOrUpdateAntrag(antragInput, sysperole);

            if (rvals.Count > 0)//return first occured problem
                rval = rvals[0];

            return rval;
        }

        /// <summary>
        /// fills the context by proposal data
        /// </summary>
        /// <param name="cctx"></param>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private static OpenOne.Common.DTO.Prisma.prKontextDto MyCreateProductKontext(long sysperole, long sysbrand, OpenOne.GateBANKNOW.Common.DTO.AntragDto antrag)
        {
            OpenOne.Common.DTO.Prisma.prKontextDto pKontext = new OpenOne.Common.DTO.Prisma.prKontextDto();

            pKontext.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            pKontext.sysperole = sysperole;
            pKontext.sysprproduct = antrag.kalkulation.angAntKalkDto.sysprproduct;
            pKontext.sysbrand = sysbrand;
            if (antrag.kunde != null)
            {
                pKontext.syskdtyp = antrag.kunde.syskdtyp;
            }
            if (antrag.angAntObDto != null)
            {
                pKontext.sysobart = antrag.angAntObDto.sysobart;
                pKontext.sysobtyp = antrag.angAntObDto.sysobtyp;
            }
            if(antrag.sysprchannel.HasValue)
                pKontext.sysprchannel = antrag.sysprchannel.Value;
            if(antrag.sysprhgroup.HasValue)
                pKontext.sysprhgroup = antrag.sysprhgroup.Value;
            pKontext.sysprusetype = antrag.kalkulation.angAntKalkDto.sysobusetype;

            return pKontext;
        }
    }
}