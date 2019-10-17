using AutoMapper;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Transactions;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    /// <summary>
    /// Endpoint for B2B/B2C 3rd-Party Frontend Interfaces 
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class b2xService : Ib2xService/*,Ib2xServiceV2,Ib2xServiceV3*/
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Delivers the perole for the given username
        /// username is puser.externeid
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns>SYSPEROLE</returns>
        public ogetDealerDto getDealerID(String userid, String password)
        {

            ogetDealerDto rval = new ogetDealerDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (userid == null)
                {
                    throw new ArgumentException("No input was sent.");
                }
                AuthenticationBo authBo = new AuthenticationBo();
                rval.sysPEROLE = authBo.getDealerId(userid,password);

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)      // expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)                 // unhandled exception
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Delivers the object information from a eurotax id
        /// </summary>
        /// <param name="id">Schluessel</param>
        /// <returns>Fahrzeugspezifische Daten aus Eurotax</returns>
        public oGetObjektDatenDto getObjektDaten(String id)
        {
            oGetObjektDatenDto rval = new oGetObjektDatenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (id == null)
                {
                    throw new ArgumentException("No input was sent.");
                }
                cctx.validateService();

                long sysobtyp = 0;
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = id });
                    sysobtyp = ctx.ExecuteStoreQuery<long>("select sysobtyp from obtyp where schwacke=:schwacke", parameters.ToArray()).FirstOrDefault();
                }

                IAngAntBo angAntBo = BOFactory.getInstance().createAngAntBo();
                rval.objekt = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObDto>(angAntBo.getObjektdaten(sysobtyp));

                rval.success();

                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Adjusts the srv/prod Kontexts' long values by configurations inside SETUP.NET/OVERWRITE/VARNAME
        /// Enabled only if web.config contains Cic.OpenOne.GateBANKNOW.Common.Settings setting name ContextOverride value true
        /// </summary>
        /// <param name="ctx"></param>
        private void alterKontext(object ctx)
        {
            if (Cic.OpenOne.GateBANKNOW.Common.Settings.Default.ContextOverride != null && "true".Equals(Cic.OpenOne.GateBANKNOW.Common.Settings.Default.ContextOverride.ToLower()))
            {
                foreach (PropertyInfo pi in ctx.GetType().GetProperties())
                {
                    if (pi.PropertyType != typeof(long))
                        continue;

                    pi.SetValue(ctx, AppConfig.Instance.GetCfgEntry("SETUP.NET", "OVERWRITE", pi.Name.ToUpper(), (long)pi.GetValue(ctx)));
                }
            }
        }

        /// <summary>
        /// Delivers a list of all products and its parameters for the given context
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public olistAvailableProduktInfoDto listAvailableProduktInfo(ilistAvailableProduktInfoDto input)
        {
            
            olistAvailableProduktInfoDto rval = new olistAvailableProduktInfoDto();
            CredentialContext cctx = new CredentialContext();
           

            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input (ilistAvailableProduktInfoDto) was sent.");
                }
                alterKontext(input.kontext);
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                
                cctx.validateService();
                _log.Debug("Duration Validate: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    if (input.kontext.sysperole == 0)
                        input.kontext.sysperole = PeRoleUtil.FindRootPEROLEByRoleType(ctx , cctx.getMembershipInfo().sysPEROLE, (int)RoleTypeTyp.HAENDLER);
                    else
                        input.kontext.sysperole = PeRoleUtil.FindRootPEROLEByRoleType(ctx , input.kontext.sysperole, (int)RoleTypeTyp.HAENDLER);
                }


                if (input.kontext.sysbrand == 0) input.kontext.sysbrand = cctx.getMembershipInfo().sysBRAND;
                _log.Debug("Duration A: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                Cic.OpenOne.GateBANKNOW.Common.DAO.IPruefungDao pruefundDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getPruefungDao();

                ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                PrismaProductBo bo = new PrismaProductBo(pDao, obDao, transDao, PrismaProductBo.CONDITIONS_BANKNOW, cctx.getUserLanguange());
                IPrismaParameterBo paramBo = new PrismaParameterBo(pDao, obDao, PrismaParameterBo.CONDITIONS_BANKNOW);

                Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo pruefungbo = new Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo(pDao, obDao, transDao, pruefundDao);
                //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
                input.kontext.prprodtype = Cic.OpenOne.Common.DTO.Prisma.Prprodtype.B2C;
                List<CIC.Database.PRISMA.EF6.Model.PRPRODUCT> products = bo.listAvailableProducts(input.kontext);
                _log.Debug("Duration B: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                _log.Debug("Duration C: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                AvailableProduktDto[] sortresult = bo.listSortedAvailableProducts(products, input.kontext.sysprbildwelt).ToArray();
                _log.Debug("Duration D: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                Cic.OpenOne.GateBANKNOW.Common.DTO.JokerPruefungDto resultJokerPruefung = pruefungbo.analyzeJokerProducts(sortresult, input.kontext);
                _log.Debug("Duration E: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                
                rval.produktinfos = new AvailableProduktInfoDto[sortresult.Length];
                int t = 0;
                IKalkulationBo cbo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());
                foreach (AvailableProduktDto p in sortresult)
                {
                    AvailableProduktInfoDto item = new AvailableProduktInfoDto();
                    item.produkt = p;

                    //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
                    input.kontext.sysperole = obDao.getHaendlerByEmployee(input.kontext.sysperole);
                    input.kontext.sysprproduct = p.sysID;

                    CIC.Database.PRISMA.EF6.Model.PRPRODUCT prod = (from s in products
                                                                  where s.SYSPRPRODUCT == Math.Abs(input.kontext.sysprproduct)
                                                                  select s).FirstOrDefault();

                    if (prod != null)
                    {
                        input.kontext.sysvart = (prod.SYSVART != null && prod.SYSVART.HasValue) ? prod.SYSVART.Value : 0;
                        input.kontext.sysvttyp = (prod.SYSVART != null && prod.SYSVTTYP.HasValue) ? prod.SYSVTTYP.Value : 0;
                    }
                    //input.kontext.sysprchannel = 0;

                    List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> prparams = paramBo.listAvailableParameter(input.kontext);

                    //add a virtual rapzins parameter:
                    prparams.Add(cbo.getRap(p.sysID));

                    item.parameters = Mapper.Map<Cic.OpenOne.Common.DTO.Prisma.ParamDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.ParamDto[]>(prparams.ToArray());
                    rval.produktinfos[t++] = item;
                }

                //WARMUP FOR create ANGEBOT
                try
                {
                    Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto ktest = new Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto();
                    ktest.angAntKalkDto = new AngAntKalkDto();
                    ktest.angAntKalkDto.sysprproduct = input.kontext.sysprproduct;
                    ktest.angAntKalkDto.lz = 36;
                    ktest.angAntKalkDto.bginternbrutto = 30000;
                    byte rateError = 0;
                    Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(ktest);
                    IKalkulationBo kalkBo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());
                    Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = kalkBo.calculate(kalkulationInput, input.kontext, new Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext(), cctx.getUserLanguange(), ref rateError);
                    KundeBo kundeBo = new KundeBo(new Cic.OpenOne.GateBANKNOW.Common.DAO.KundeDao());
                    kundeBo.getKunde(1);
                }
                catch(Exception e)
                {
                    _log.Warn("Warmup of Calculator failed", e);
                }
                 try
                {
                    using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            CIC.Database.OL.EF6.Model.IT testIt = new CIC.Database.OL.EF6.Model.IT();
                            ctx.IT.Add(testIt);
                            testIt.NAME = "WARMUP";
                            ctx.SaveChanges();
                        }
                    }
                }catch(Exception excep)
                {
                    _log.Warn("Warmup Issue: " + excep.Message);
                }
                _log.Debug("Duration F: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }


        private static Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext MyCreateKalkKontext(DTO.AngebotDto antrag)
        {
            //B2B
            //this.rateBruttoInklAbsicherung = 0D;
            //this.ersteRateBruttoInklAbsicherung = 0D;
            Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext kkontext = new Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext();
            if (antrag.angAntObDto != null)
            {
                kkontext.grundBrutto = antrag.angAntObDto.grundBrutto;
                kkontext.ubnahmeKm = antrag.angAntObDto.ubnahmeKm;
                kkontext.erstzulassung = antrag.angAntObDto.erstzulassung;
                kkontext.zubehoerBrutto = antrag.angAntObDto.zubehoerBrutto;
            }
            kkontext.zinsNominal = 0;
            return kkontext;
        }

        /// <summary>
        /// Prüft die Kalkulation
        /// </summary>
        /// <param name="input">icheckAngebotDto</param>
        /// <returns>ocheckAngebotDto</returns>
        public Service.DTO.ocheckAntAngDto checkKalkulation(icheckKalkulationDto input)
        {
            Service.DTO.ocheckAntAngDto rval = new Service.DTO.ocheckAntAngDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                if (input == null)
                {
                    throw new ArgumentException("Input is null.");
                }
                if (input.kalkulation == null)
                {
                    throw new ArgumentException("Kalkulation is null.");
                }
                if (input.prodKontext == null)
                {
                    throw new ArgumentException("ProdKontext is null.");
                }
                cctx.validateService();
                alterKontext(input.prodKontext);
                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                Common.DTO.KalkulationDto calc = new Common.DTO.KalkulationDto();
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>(input.kalkulation);
                calc.angAntKalkDto = kalkulationInput; List<Cic.OpenOne.Common.DTO.AngAntVsDto> versicherungen = new List<OpenOne.Common.DTO.AngAntVsDto>();
                if (input.angAntVs != null)
                    foreach (Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto vs in input.angAntVs)
                        versicherungen.Add(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>(vs));
                calc.angAntVsDto = versicherungen;
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObSmallDto antAntObDto = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObSmallDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObSmallDto>(input.angAntOb);
                rval = Mapper.Map<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>(bo.checkAngebot(calc, input.prodKontext, cctx.getUserLanguange(), antAntObDto));

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Löscht die aktuelle Kalkulation auf und liefert die Berechnungsergebnisse
        /// </summary>
        /// <param name="input">isolveKalkulationDto</param>
        /// <returns>osolveKalkulationDto</returns>
        public osolveKalkulationDto solveKalkulation(isolveKalkulationDto input)
        {
            osolveKalkulationDto rval = new osolveKalkulationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                byte rateError = 0;
                if (input == null)
                {
                    throw new ArgumentException("No input (isolveKalkulationDto) was sent.");
                }
                cctx.validateService();
                alterKontext(input.prodKontext);
                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());

                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(input.kalkulation);
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = bo.calculate(kalkulationInput, input.prodKontext, input.kalkKontext, cctx.getUserLanguange(), ref rateError);
                Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto kalkulationOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>(kalk);
                KalkulationServicesBo kservice = new KalkulationServicesBo();
                rval.zusatzinformationen = kservice.aggregateZusatzinformation(kalk);

                rval.kalkulation = kalkulationOutput;
                bo.throwErrorMessages(rateError);

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }


        /// <summary>
        /// Liefert eine Liste der verfügbaren Services im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableServicesDto</param>
        /// <returns>olistAvailableServicesDto</returns>
        public olistAvailableServicesDto listAvailableServices(ilistAvailableServicesDto input)
        {
            olistAvailableServicesDto rval = new olistAvailableServicesDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listAvailableServiceDto was sent.");
                }
                alterKontext(input.kontext);
                IPrismaServiceBo bo = PrismaBoFactory.getInstance().createPrismaServiceBo();
                List<AvailableServiceDto> services = bo.listAvailableServices(input.kontext, cctx.getUserLanguange());
                rval.services = services.ToArray();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt des Angebots
        /// </summary>
        /// <param name="input">icreateAngebotDto</param>
        /// <returns>ocreateAngebotDto</returns>
        public ocreateAngebotDto createOrUpdateAngebot(icreateAngebotDto input)
        {
            ocreateAngebotDto rval = new ocreateAngebotDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                byte rateError = 0;
                if (input == null)
                {
                    throw new ArgumentException("No input (icreateAngebotDto) was sent.");
                }
                if (input.angebot == null)
                {
                    throw new ArgumentException("No Angebot was sent.");
                }
                cctx.validateService();
                long syscictlog=-1;
                if (input.angebot.sysid == 0)
                    syscictlog = LogUtil.addTLog("ANGEBOT", 0, input, cctx.getMembershipInfo().sysWFUSER);
                else
                    LogUtil.addTLog("ANGEBOT", input.angebot.sysid, input, cctx.getMembershipInfo().sysWFUSER);
                 
                long measure = DateTime.Now.TimeOfDay.Milliseconds;
                
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                IKalkulationBo kalkBo = null;
                _log.Debug("Duration B2C A " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                measure = DateTime.Now.TimeOfDay.Milliseconds;
                if (input.angebot.angAntVars != null)
                {
                    foreach (DTO.AngAntVarDto variante in input.angebot.angAntVars)
                    {
                        Cic.OpenOne.Common.DTO.Prisma.prKontextDto pKontext = MyCreateProductKontext(cctx, input.angebot, variante);
                        
                        _log.Debug("Calculating Variant on Angebot Update with context: " + _log.dumpObject(pKontext));
                        kalkBo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());
                        Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(variante.kalkulation);
                        Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = kalkBo.calculate(kalkulationInput, pKontext, MyCreateKalkKontext(input.angebot), cctx.getUserLanguange(), ref rateError);
                        _log.Debug("Duration B2C B " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                        measure = DateTime.Now.TimeOfDay.Milliseconds;
                        variante.kalkulation = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>(kalk);
                    }
                }
                _log.Debug("Duration B2C C " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                measure = DateTime.Now.TimeOfDay.Milliseconds;
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebotInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>(input.angebot);
                _log.Debug("Duration B2C D " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                measure = DateTime.Now.TimeOfDay.Milliseconds;
                if (input.angebot.kunde != null)
                {
                    if (input.angebot.kunde.sysit == 0)
                    {
                        
                        IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                        _log.Debug("Duration B2C E " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                        measure = DateTime.Now.TimeOfDay.Milliseconds;
                        angebotInput.kunde = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.angebot.kunde), cctx.getMembershipInfo().sysPEROLE);
                    }
                    
                }
                _log.Debug("Duration B2C F " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                measure = DateTime.Now.TimeOfDay.Milliseconds;
                if (input.angebot.mitantragsteller != null)
                {
                    if (input.angebot.mitantragsteller.sysit == 0)
                    {
                        
                        IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                        angebotInput.mitantragsteller = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.angebot.mitantragsteller), cctx.getMembershipInfo().sysPEROLE);
                    }
                }
                angebotInput.erfassungsclient = Cic.OpenOne.GateBANKNOW.Common.DAO.AngAntDao.ERFASSUNGSCLIENT_B2C;

                if (angebotInput.angAntVars != null && angebotInput.angAntVars.Count > 0 && angebotInput.angAntVars[0].kalkulation != null && angebotInput.angAntVars[0].kalkulation.angAntKalkDto.aboppy > 0)
                    angebotInput.angAntVars[0].kalkulation.angAntKalkDto.aboppy = 12 / angebotInput.angAntVars[0].kalkulation.angAntKalkDto.aboppy;

                Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebot = bo.createOrUpdateAngebot(angebotInput, cctx.getMembershipInfo().sysPEROLE);
                Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto angebotOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto>(angebot);
                bo.updateVttypinAngebot(angebotOutput.sysid);
                if (input.angebot.kunde != null)
                {
                    angebot.kunde.bestandsKunde = input.angebot.kunde.bestandsKunde;
                }
                if (syscictlog > 0)
                    LogUtil.updateTLog(syscictlog, angebotOutput.sysid);

                //bo.processAngebotEinreichung(angebot, cctx.getMembershipInfo().sysWFUSER, cctx.getUserLanguange());


                Cic.OpenOne.GateBANKNOW.Service.BO.StatusEPOSBo.setStatusEPOS(angebotOutput);
                rval.angebot = angebotOutput;

                BOFactory.getInstance().createEaihotBo().createEAIBosCall(B2CBOSAdapterFactory.CMD_B2C_NEWANGEBOT, "ANGEBOT", rval.angebot.sysid, cctx.getMembershipInfo().sysWFUSER, null, null, null);


                if (kalkBo != null)
                    kalkBo.throwErrorMessages(rateError);

                

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// creates/updates the proposal
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateAntragDto createOrUpdateAntrag(icreateAntragDto input)
        {
            ocreateAntragDto rval = new ocreateAntragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                string joker = "Joker";
                bool pruefundJokerneeded = true;
                byte rateError = 0;
                if (input == null)
                {
                    throw new ArgumentException("No input createAntragDto was sent.");
                }
                if (input.antrag == null)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();

                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);
                input.antrag.erfassungsclient = Cic.OpenOne.GateBANKNOW.Common.DAO.AngAntDao.ERFASSUNGSCLIENT_B2C;

                IKalkulationBo kalkBo = null;

                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                IPruefungBo pruefungbo = BOFactory.getInstance().createPruefungBo();
                //recalc upon save!
                if (input.antrag.kalkulation != null)
                {
                    Cic.OpenOne.Common.DTO.Prisma.prKontextDto pKontext = MyCreateProductKontext(cctx, input.antrag);
                    _log.Debug("Calculating on Antrag Update with context: " + _log.dumpObject(pKontext));

                    if (input.antrag.kalkulation.angAntAblDto != null)
                        foreach (Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntAblDto abl in input.antrag.kalkulation.angAntAblDto)
                            if (abl.sysabltyp <= 0)
                            {
                                throw new ArgumentException("Einen zugelassenen Wert im Dropdown Art der Finanzierung auszuwählen", "sysabltyp");

                            }





                    if (pruefungbo.isJokerProduct(pKontext.sysprproduct))
                        if (pruefungbo.isJokerWithAntrag(pKontext.sysprproduct, input.antrag.sysid) != true)
                        {
                            if (!pruefungbo.isJokerFree(pKontext.sysprproduct, cctx.getMembershipInfo().sysPEROLE, input.antrag.sysprjoker, DateTime.Now))
                            {
                                throw new ApplicationException("Joker steht nicht mehr zur Verfügung. Bitte ein anderes Produkt auswählen");
                            }
                        }
                        else
                            pruefundJokerneeded = false;

                    kalkBo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());
                    Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(input.antrag.kalkulation);
                    Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext kKontext = new Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext();
                    if (input.antrag.angAntObDto != null)
                    {
                        kKontext.grundBrutto = input.antrag.angAntObDto.grundBrutto;
                        kKontext.zubehoerBrutto = input.antrag.angAntObDto.zubehoerBrutto;
                        kKontext.ubnahmeKm = input.antrag.angAntObDto.ubnahmeKm;
                        kKontext.erstzulassung = input.antrag.angAntObDto.erstzulassung;
                    }
                    Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = kalkBo.calculate(kalkulationInput, pKontext, kKontext, cctx.getUserLanguange(), ref rateError);
                    input.antrag.kalkulation = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>(kalk);
                    if (input.antrag.angAntObDto != null)
                        input.antrag.angAntObDto.satzmehrKm = kalk.angAntKalkDto.ob_mark_satzmehrkm;
                }
                _log.Debug("Duration Kalkulation: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antragInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>(input.antrag);
                IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                if (input.antrag.kunde != null)
                {
                    
                    Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kundeInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.antrag.kunde);
                    if (kundeInput.kontos != null)
                        foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto kto in kundeInput.kontos)
                            kto.sysantrag = input.antrag.sysid;

                    antragInput.kunde = kundeBo.createOrUpdateKunde(kundeInput, cctx.getMembershipInfo().sysPEROLE);
                }
                _log.Debug("Duration Kunde: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                if (input.antrag.mitantragsteller != null)
                {
                    
                    Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kundeInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.antrag.mitantragsteller);
                    if (kundeInput.kontos != null)
                        foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto kto in kundeInput.kontos)
                            kto.sysantrag = input.antrag.sysid;
                    antragInput.mitantragsteller = kundeBo.createOrUpdateKunde(kundeInput, cctx.getMembershipInfo().sysPEROLE);
                }
                _log.Debug("Duration MA: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto antragOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto>(bo.createOrUpdateAntrag(antragInput, cctx.getMembershipInfo().sysPEROLE));
                _log.Debug("Duration Antrag: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                rval.antrag = antragOutput;
                
                if (antragOutput.kalkulation != null && antragOutput.kalkulation.angAntKalkDto != null && pruefundJokerneeded)
                {

                    long jokerpruefung = pruefungbo.jokerPruefung(antragOutput.sysid, antragOutput.kalkulation.angAntKalkDto.sysprproduct, joker, cctx.getMembershipInfo().sysPEROLE, input.antrag.sysprjoker);
                    antragOutput.sysprjoker = jokerpruefung;
                }

                if (kalkBo != null)
                    kalkBo.throwErrorMessages(rateError);

                rval.success();
                return rval;
            }
            catch (ApplicationException e)
            {
                cctx.fillBaseDto(rval, e, "F_000010_JokerException");
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                if (e.ParamName!=null && e.ParamName.Equals("sysabltyp"))
                    cctx.fillBaseDto(rval, e, "F_000020_AbltypException");
                else
                    cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// fills the context by proposal data
        /// </summary>
        /// <param name="cctx"></param>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private static OpenOne.Common.DTO.Prisma.prKontextDto MyCreateProductKontext(CredentialContext cctx, DTO.AntragDto antrag)
        {
            OpenOne.Common.DTO.Prisma.prKontextDto pKontext = new OpenOne.Common.DTO.Prisma.prKontextDto();

            pKontext.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            pKontext.sysperole = cctx.getMembershipInfo().sysPEROLE;
            pKontext.sysprproduct = antrag.kalkulation.angAntKalkDto.sysprproduct;
            pKontext.sysbrand = 0;// antrag.sysbrand;
            if (antrag.kunde != null)
            {
                pKontext.syskdtyp = antrag.kunde.syskdtyp;
            }
            if (antrag.angAntObDto != null)
            {
                pKontext.sysobart = antrag.angAntObDto.sysobart;
                pKontext.sysobtyp = antrag.angAntObDto.sysobtyp;
            }
            pKontext.sysprchannel = antrag.sysprchannel;
            pKontext.sysprhgroup = antrag.sysprhgroup;
            pKontext.sysprusetype = antrag.kalkulation.angAntKalkDto.sysobusetype;
            //inputContext.kontext.sysprinttyp
            //inputContext.kontext.sysprkgroup

            return pKontext;
        }

        /// <summary>
        /// fills the context by offer calculation data
        /// </summary>
        /// <param name="cctx"></param>
        /// <param name="angebot"></param>
        /// <param name="variante"></param>
        /// <returns></returns>
        private static Cic.OpenOne.Common.DTO.Prisma.prKontextDto MyCreateProductKontext(CredentialContext cctx, Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto angebot, DTO.AngAntVarDto variante)
        {
            Cic.OpenOne.Common.DTO.Prisma.prKontextDto pKontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();

            if (variante == null)
            {
                if (angebot.angAntVars == null || angebot.angAntVars.Count == 0)
                {
                    throw new ApplicationException("No active calculation.");
                }
                variante = angebot.angAntVars[0];
                if (angebot.angAntVars.Count > 1)
                {
                    variante = (from k in angebot.angAntVars
                                where k.inantrag > 0
                                select k).FirstOrDefault();
                }
            }

            if (variante == null)
            {
                throw new ApplicationException("No active calculation.");
            }
            DTO.KalkulationDto activeKalk = variante.kalkulation;
            // Kontext erstellen
            igetParameterDto inputContext = new igetParameterDto();
            pKontext = new OpenOne.Common.DTO.Prisma.prKontextDto();

            pKontext.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            pKontext.sysperole = cctx.getMembershipInfo().sysPEROLE;
            pKontext.sysprproduct = activeKalk.angAntKalkDto.sysprproduct;
            pKontext.sysbrand = 0;// angebot.sysbrand;
            if (angebot.kunde != null)
            {
                pKontext.syskdtyp = angebot.kunde.syskdtyp;
            }
            if (angebot.angAntObDto != null)
            {
                pKontext.sysobart = angebot.angAntObDto.sysobart;
                pKontext.sysobtyp = angebot.angAntObDto.sysobtyp;
            }
            pKontext.sysprchannel = angebot.sysprchannel;
            pKontext.sysprhgroup = angebot.sysprhgroup;
            pKontext.sysprusetype = activeKalk.angAntKalkDto.sysobusetype;
            //inputContext.kontext.sysprinttyp
            //inputContext.kontext.sysprkgroup
            return pKontext;
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Dokumente im Kontext (20100914_Logik_Druckmatrix_v2)
        /// </summary>
        /// <param name="input">ilistAvailableDokumenteDto</param>
        /// <returns>olistAvailableDokumenteDto</returns>
        /// 
        public olistAvailableDokumenteDto listAvailableDokumente(ilistAvailableDokumenteDto input)
        {
            olistAvailableDokumenteDto rval = new olistAvailableDokumenteDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listAvailableDokumenteDto was sent.");
                }
                if (input.kontext == null)
                {
                    throw new ArgumentException("No Kontext was sent.");
                }
                if (input.kontext.sysID == 0 || input.kontext.sysID == 0)
                {
                    throw new ArgumentException("No Angebot sysID was sent.");
                }
                cctx.validateService();

                try
                {
                    IPrintAngAntBo bo = BOFactory.getInstance().createPrintAngAntBo();
                    String subArea = input.kontext.subarea.ToString();
                    if (input.kontext.subarea == DocumentSubArea.All)
                    {
                        subArea = String.Empty;
                    }
                    Common.DTO.DokumenteDto[] commonDto = bo.listAvailableDokumente(Common.DTO.Enums.EaiHotOltable.Angebot, input.kontext.sysID,
                                                                                    cctx.getMembershipInfo().sysPERSON, cctx.getUserLanguange(), subArea, cctx.getMembershipInfo().sysPEROLE);
                    rval.dokumente = Mapper.Map<Common.DTO.DokumenteDto[], Service.DTO.DokumenteDto[]>(commonDto);
                    rval.success();
                }
                catch (ApplicationException ex)
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Timeout";
                    rval.message.detail = "Timeout during EAIRequest";
                    rval.message.message = ex.ToString();
                }
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Erstellt Druckauftrag für ausgewählte Dokumente
        /// </summary>
        /// <param name="input">iprintCheckedDokumenteDto</param>
        /// <returns>oprintCheckedDokumenteDto</returns>
        public oprintCheckedDokumenteDto printCheckedDokumente(iprintCheckedDokumenteDto input)
        {
            oprintCheckedDokumenteDto rval = new oprintCheckedDokumenteDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input printCheckedDokumenteDto was sent.");
                }
                if (input.sysid == 0)
                {
                    throw new ArgumentException("No Angebot Sysid was sent.");
                }
                if (input.dokumente == null)
                {
                    throw new ArgumentException("No Dokumentelist was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                try
                {
                    IPrintAngAntBo bo = BOFactory.getInstance().createPrintAngAntBo();
                    rval.file = bo.printCheckedDokumente(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.DokumenteDto[], Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto[]>(input.dokumente), input.sysid, input.variantenid,Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotOltable.Angebot, cctx.getMembershipInfo().sysPERSON, input.eCodeEintragung);
                    if (rval.file == null)
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "No PDF generated";
                        rval.message.detail = "Print PDF didn´t work correctly";
                    }
                    else
                    {
                        rval.success();
                    }
                }
                catch (ApplicationException ex)
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Timeout";
                    rval.message.detail = "Timeout during EAIRequest";
                    rval.message.message = ex.ToString();
                }
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// KREMO Budgetcalculation
        /// </summary>
        /// <param name="input"></param>
        /// <returns>budget</returns>
        public okremoGetBudget kremoGetBudget(ikremoGetBudgetDto input)
        {
            okremoGetBudget rval = new okremoGetBudget();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input was sent.");
                }
                cctx.validateService();

                String param = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry("GETBUDGET", "EINKOMMEN_MAX", "1000000", "KREMO");
                double mValue = Double.Parse(param);
                if (input.budget.Einknetto > mValue)
                    input.budget.Einknetto = mValue;
                if (input.budget.Einmalzahlung > mValue)
                    input.budget.Einmalzahlung = mValue;
                if (input.budget.Nebeneinknetto > mValue)
                    input.budget.Nebeneinknetto = mValue;

                KREMOInDto kin = Mapper.Map<BudgetDto, KREMOInDto>(input.budget);
                kin.GebDatum = input.budget.GebDatum.Value.Year * 10000 + input.budget.GebDatum.Value.Month * 100 + input.budget.GebDatum.Value.Day;
                //Kantoncode wie im Antrag beim input, korrigieren:
                kin.Kantoncode = input.budget.getKantoncodeInternal();

                //einmalzahlung auf Einkommen
                kin.Einknetto += input.budget.Einmalzahlung / 12.0;

                rval.budget = input.budget;
                rval.budget.limit = AuskunftBoFactory.CreateDefaultKREMOBo().getBudget(kin);

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Document upload Service for a certain AREA
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateUploadDto createOrUpdateUpload(icreateUploadDto input)
        {
            ocreateUploadDto rval = new ocreateUploadDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                input.file.syscrtuser = cctx.getMembershipInfo().sysPEROLE;
                IFileBo bo = BOFactory.getInstance().createFileBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto fileInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.FileDto, Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto>(input.file);
                fileInput.syswfuser = cctx.getMembershipInfo().sysWFUSER;
                Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto fileOutput = bo.createOrUpdateFileAngebot(fileInput);
                rval.file = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto, Cic.OpenOne.GateBANKNOW.Service.DTO.FileDto>(fileOutput);
                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }

        }

        /// <summary>
        /// Data upload/assignment a certain AREA
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateExtraValueDto createOrUpdateExtraValue(icreateExtraValueDto input)
        {
            ocreateExtraValueDto rval = new ocreateExtraValueDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                if (input.extraval == null)
                {
                    throw new Exception("No CONTENT to save!");
                 
                }
                cctx.validateService();
                long colid = 0;
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = input.extraval.colcode});
                    colid = ctx.ExecuteStoreQuery<long>("select sysddlkpcol from ddlkpcol where code=:code", pars.ToArray()).FirstOrDefault();
                }

                if (colid < 1)
                    throw new Exception("No DDLKPCOL with CODE " + input.extraval.colcode + " found!");

                Encoding iso = Encoding.GetEncoding("Windows-1252");
                IDictionaryListsBo bo = BOFactory.getInstance().createDictionaryListsBo(cctx.getMembershipInfo().ISOLanguageCode);
                DdlkpsposDto[] ival = new DdlkpsposDto[1];
                ival[0] = new DdlkpsposDto();
                ival[0].area = input.extraval.area;
                ival[0].sysid = input.extraval.sysId;
                ival[0].sysddlkpspos = input.extraval.sysExtraValue;
                if(input.extraval.content!=null)
                 ival[0].content = iso.GetString(input.extraval.content);
                ival[0].sysddlkpcol = colid;
                ival[0].activeFlag = 1;
                
                ival = bo.createOrUpdateDdlkpspos(ival);

                rval.extraval = input.extraval;
                
                if(ival.Length>0)
                    rval.extraval.sysExtraValue = ival[0].sysddlkpspos ;
                
                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }

        }

        /// <summary>
        /// returns all GUI-Field Translations
        /// by this query
        /// select ctfoid.frontid,typ,verbaldescription MASTER, replaceterm TRANSLATION,replaceblob,isocode 
        /// FROM ctfoid,cttfoid,ctlang where ctfoid.frontid=cttfoid.frontid and ctlang.sysctlang=cttfoid.sysctlang and flagtranslate=1 order by ctfoid.frontid;
        /// </summary>
        /// <returns>oTranslationDto</returns>
        public oTranslationDto getTranslations()
        {
            oTranslationDto rval = new oTranslationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                ITranslateBo Translator = new TranslateBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());
                rval.translations = Translator.GetStaticList();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)      // expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)                 // unhandled exception
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Delivers Service State information like response time, db version etc
        /// </summary>
        /// <returns></returns>
        public ServiceInformation DeliverServiceInformation()
        {
            ServiceInformation Info = new ServiceInformation();
            CredentialContext cctx = new CredentialContext();
            try
            {
                StateServiceBo bo = new StateServiceBo();

                bo.getServiceInformation(Info);

                Info.success();
                return Info;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(Info, e, "F_00004_DatabaseUnreachableException");
                return Info;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(Info, e);
                return Info;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(Info, e, "F_00001_GeneralError");
                return Info;
            }
        }

        /// <summary>
        /// Liefert alle Quoten incl. deren zeitlicher Gültigkeiten
        /// </summary>
        /// <returns></returns>
        public olistQuoteInfoDto getQuotes()
        {
            olistQuoteInfoDto rval = new olistQuoteInfoDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.quotes = CommonDaoFactory.getInstance().getQuoteDao().getQuotes();
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
            }
            catch (ServiceBaseException e)
            {
                cctx.fillBaseDto(rval, e);
            }
            catch (Exception e)
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
            }

            return rval;
        }

       /* public long createOrUpdateExtraValue(double input)
        {
            return 33393;
        }

        public okremoGetBudgetV3 kremoGetBudget(ikremoGetBudgetDtoV3 input)
        {
            return new okremoGetBudgetV3();
        }*/
    }
}
