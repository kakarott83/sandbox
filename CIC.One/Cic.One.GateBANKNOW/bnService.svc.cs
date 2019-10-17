using AutoMapper;
using Cic.One.DTO;
using Cic.One.DTO.BN;
using Cic.One.GateBANKNOW.BO;
using Cic.One.GateBANKNOW.Contract;
using Cic.One.Web.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Linq;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.One.Web.DAO;

namespace Cic.One.GateBANKNOW
{
    /// <summary>
    /// Service-Endpoint for BANKNOW Specific Cic.One Functions
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class bnService : IbnService
    {

        /// <summary>
        /// Creates a Antrag from NKK
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateAntragFromNkkDto createAntragFromNkk(icreateAntragFromNkkDto input)
        {
            ServiceHandler<icreateAntragFromNkkDto, ocreateAntragFromNkkDto> serviceHandler = new ServiceHandler<icreateAntragFromNkkDto, ocreateAntragFromNkkDto>(input);
            return serviceHandler.process(delegate(icreateAntragFromNkkDto inp, ocreateAntragFromNkkDto rval, CredentialContext cctx)
            {
                if (input == null)
                    throw new ArgumentException("No search input");

                IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
                var antrag = bo.createAntragFromNkk(inp.sysnkk, cctx.getMembershipInfo().sysWFUSER, cctx.getMembershipInfo().sysPEROLE, cctx.getMembershipInfo().ISOLanguageCode);

                if (antrag == null)
                    throw new Exception("Antrag not created from NKK " + input.sysnkk);
                rval.sysantrag = antrag.sysid;

                rval.success();
               
            });

           
        }
        /// <summary>
        /// createOrUpdateAngebot
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateOrUpdateBNAngebotDto createOrUpdateBNAngebot(icreateOrUpdateBNAngebotDto input)
        {

            Cic.One.Web.BO.ServiceHandler<icreateOrUpdateBNAngebotDto, ocreateOrUpdateBNAngebotDto> ew = new Cic.One.Web.BO.ServiceHandler<icreateOrUpdateBNAngebotDto, ocreateOrUpdateBNAngebotDto>(input);
            return ew.process(delegate(icreateOrUpdateBNAngebotDto param, ocreateOrUpdateBNAngebotDto rval, CredentialContext cctx)
            {

                IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
                

                Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebotInput = Mapper.Map<BNAngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>(param.angebot);
                if (param.angebot.kunde != null)
                {
                    IKundeBo kundeBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
                    angebotInput.kunde = kundeBo.createOrUpdateKunde(Mapper.Map<BNKundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(param.angebot.kunde), cctx.getMembershipInfo().sysPEROLE);
                    angebotInput.syskd = param.angebot.kunde.syskd;
                    angebotInput.sysit = param.angebot.kunde.sysit;
                }
                if (param.angebot.mitantragsteller != null)
                {
                    IKundeBo kundeBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
                    
                    angebotInput.mitantragsteller = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.One.DTO.BNKundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(param.angebot.mitantragsteller), cctx.getMembershipInfo().sysPEROLE);
                }
                /*if (param.angebot.haendler != null && )
                {
                    IKundeBo kundeBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
                    angebotInput.sysVM = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.One.DTO.BNKundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(param.angebot.haendler), cctx.getMembershipInfo().sysPEROLE).syskd;
                }*/
                if (angebotInput.erfassungsclient == 0)//Do not overwrite an already set erfassungsclient when one web opens eg ma or dmr erfassungsclient offer
                {
                    //IMPORTANT: At some time it might be important to change the erfassungsclient depending on the original erfassungsclient
                    angebotInput.erfassungsclient = Cic.OpenOne.GateBANKNOW.Common.DAO.AngAntDao.ERFASSUNGSCLIENT_ONE;
                }

                BNAngebotDto angebotOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, BNAngebotDto>(bo.createOrUpdateAngebot(angebotInput, cctx.getMembershipInfo().sysPEROLE));

                if (angebotOutput.sysVM > 0)//must load haendler, not done in createorupdate
                {
                    IKundeDao kddao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getKundeDao();
                    angebotOutput.haendler = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, BNKundeDto>(kddao.getKundeBySysKd(angebotOutput.sysVM));
                }

                rval.angebot = angebotOutput;
                if (angebotOutput.mitantragsteller != null)
                {
                    rval.angebot.sysMa = angebotOutput.mitantragsteller.syskd;
                    rval.angebot.sysItMa = angebotOutput.mitantragsteller.sysit;
                }
                if(angebotOutput.kunde!=null)
                {
                    rval.angebot.syskd = angebotOutput.kunde.syskd;
                    rval.angebot.sysit = angebotOutput.kunde.sysit;
                }

                IEntityDao ed = DAOFactoryFactory.getInstance().getEntityDao();
                ed.setSysPerole(cctx.getMembershipInfo().sysPEROLE);
                ed.setISOLanguage(cctx.getMembershipInfo().ISOLanguageCode);
                rval.angebot.produkt = ed.getProduktInfoAngebotDetails(angebotOutput.getEntityId());
                if (rval.angebot.produkt != null)
                {
                    rval.angebot.produkt.versicherungen = ed.getAngebotVersicherung(angebotOutput.getEntityId());
                }

                rval.success();
            });
        }

        /// <summary>
        /// createOrUpdateAngebot
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateOrUpdateBNAntragDto createOrUpdateBNAntrag(icreateOrUpdateBNAntragDto input)
        {

            Cic.One.Web.BO.ServiceHandler<icreateOrUpdateBNAntragDto, ocreateOrUpdateBNAntragDto> ew = new Cic.One.Web.BO.ServiceHandler<icreateOrUpdateBNAntragDto, ocreateOrUpdateBNAntragDto>(input);
            return ew.process(delegate(icreateOrUpdateBNAntragDto param, ocreateOrUpdateBNAntragDto rval, CredentialContext cctx)
            {

                IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();

                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antragInput = Mapper.Map<BNAntragDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>(param.antrag);
                if (param.antrag.kunde != null)
                {
                    IKundeBo kundeBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
                    antragInput.kunde = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.One.DTO.BNKundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(param.antrag.kunde), cctx.getMembershipInfo().sysPEROLE);
                }
                if (param.antrag.mitantragsteller != null)
                {
                    IKundeBo kundeBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
                    antragInput.mitantragsteller = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.One.DTO.BNKundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(param.antrag.mitantragsteller), cctx.getMembershipInfo().sysPEROLE);
                }

                BNAntragDto antragOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, BNAntragDto>(bo.createOrUpdateAntrag(antragInput, cctx.getMembershipInfo().sysPEROLE));
                rval.antrag = antragOutput;
                rval.success();
            });
        }

        /// <summary>
        /// delivers a list of Zek data
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchZekDto searchZek(iSearchDto iSearch, ZekDto request)
        {
            ServiceHandler<iSearchDto, oSearchZekDto> serviceHandler = new ServiceHandler<iSearchDto, oSearchZekDto>(iSearch);
            return serviceHandler.process(delegate(iSearchDto input, oSearchZekDto rval, CredentialContext cctx)
            {
                if (input == null)
                    throw new ArgumentException("No search input");
                if (request.nummer != null)
                {
                    request.nummer = request.nummer.Trim();
                }
                ZekBo zekInfo = (new BO.BOFactory()).getZekBO(cctx.getMembershipInfo().sysWFUSER);
                zekInfo.ZekData = request;
                var auskunft = zekInfo.DoRequest();

                if (rval == null)
                    rval = new oSearchZekDto();
                if (rval.result == null)
                    rval.result = new oSearchDto<ZekDto>();
                rval.result.results = auskunft.ToArray();
                rval.result.searchCountFiltered = auskunft.Count;
                rval.result.searchCountMax = auskunft.Count;
                if (input.pageSize == 0)
                    rval.result.searchNumPages = 1;
                else
                    rval.result.searchNumPages = (int)Math.Ceiling(auskunft.Count * 1.0M / (1.0M * input.pageSize));
            });
        }

        public ocreateOrUpdateBNKundenIdentifikationDto createOrUpdateBNKundenIdentifikation(icreateOrUpdateBNKundenIdentifikationDto kunde)
        {

            ServiceHandler<icreateOrUpdateBNKundenIdentifikationDto, ocreateOrUpdateBNKundenIdentifikationDto> ew = new ServiceHandler<icreateOrUpdateBNKundenIdentifikationDto, ocreateOrUpdateBNKundenIdentifikationDto>(kunde);
            return ew.process(delegate(icreateOrUpdateBNKundenIdentifikationDto input, ocreateOrUpdateBNKundenIdentifikationDto rval, CredentialContext cctx)
            {

                if (input == null || input.kunden == null)
                    throw new ArgumentException("No valid input");


                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kd = null;
                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto ma = null;

                if (input.kunden.kunde != null)
                {
                    IKundeBo kundeBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
                    kd = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.One.DTO.BNKundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.kunden.kunde), cctx.getMembershipInfo().sysPEROLE);
                }
                if (input.kunden.mitantragsteller != null)
                {
                    IKundeBo kundeBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
                    ma = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.One.DTO.BNKundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.kunden.mitantragsteller), cctx.getMembershipInfo().sysPEROLE);
                }

                if (rval.kunden == null)
                    rval.kunden = new BNKundenIdentifikationDto();
                rval.kunden.kunde = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.One.DTO.BNKundeDto>(kd);
                rval.kunden.mitantragsteller = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.One.DTO.BNKundeDto>(ma);
                rval.success();

            });

        }

        /// <summary>
        /// update customer legitimation
        /// </summary>
        /// <param name="legMethodeInput"></param>
        public oupdateLegMethodeDto updateLegitimationsmethode(iupdateLegMethodeDto legMethodeInput)
        {
            ServiceHandler<iupdateLegMethodeDto, oupdateLegMethodeDto> ew = new ServiceHandler<iupdateLegMethodeDto, oupdateLegMethodeDto>(legMethodeInput);
            return ew.process(delegate(iupdateLegMethodeDto input, oupdateLegMethodeDto rval, CredentialContext cctx)
            {

                if (input == null || input.sysAngebot <= 0)
                    throw new ArgumentException("No valid input");
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(cctx.getMembershipInfo());

               
                bo.updateLegitimationMethode(legMethodeInput.sysAngebot, legMethodeInput.sysWfuser, legMethodeInput.sysIt,legMethodeInput.legitimationsMethode);
                rval.geupdated = true;
                rval.success();

            });

        }

        /// <summary>
        /// update abwicklungsort for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public oupdateAbwicklungsortDto updateAbwicklungsort(iupdateAbwicklungsortDto input)
        {
            ServiceHandler<iupdateAbwicklungsortDto, oupdateAbwicklungsortDto> ew = new ServiceHandler<iupdateAbwicklungsortDto, oupdateAbwicklungsortDto>(input);
            return ew.process(delegate(iupdateAbwicklungsortDto inp, oupdateAbwicklungsortDto rval, CredentialContext cctx)
            {

                if (inp == null)
                    throw new ArgumentException("No valid input");
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(cctx.getMembershipInfo());

                rval.updated = bo.updateAbwicklungsort(inp);
                rval.success();

            });

        }

        /// <summary>
        /// update smstext for ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public oupdateSMSTextDto updateSMSText(iupdateSMSTextDto input)
        {
            ServiceHandler<iupdateSMSTextDto, oupdateSMSTextDto> ew = new ServiceHandler<iupdateSMSTextDto, oupdateSMSTextDto>(input);
            return ew.process(delegate(iupdateSMSTextDto inp, oupdateSMSTextDto rval, CredentialContext cctx)
            {

                if (inp == null)
                    throw new ArgumentException("No valid input");
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(cctx.getMembershipInfo());

                rval.updated = bo.updateSMSText(inp);
                rval.success();

            });

        }

        /// <summary>
        /// get anciliary details for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public ogetAnciliaryDetailDto getAnciliaryDetail(igetAnciliaryDetailDto input)
        {
            ServiceHandler<igetAnciliaryDetailDto, ogetAnciliaryDetailDto> ew = new ServiceHandler<igetAnciliaryDetailDto, ogetAnciliaryDetailDto>(input);
            return ew.process(delegate(igetAnciliaryDetailDto inp, ogetAnciliaryDetailDto rval, CredentialContext cctx)
            {

                if (inp == null)
                    throw new ArgumentException("No valid input");
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(cctx.getMembershipInfo());

                ogetAnciliaryDetailDto output = bo.getAnciliaryDetail(inp);
                rval.sysberater = output.sysberater;
                rval.sysabwicklung = output.sysabwicklung;
                rval.smstext = output.smstext;
                rval.nummer = output.nummer;
                rval.locked = output.locked;
                rval.success();

            });

        }

        /// <summary>
        /// accept EPOS Conditions of current user
        /// </summary>
        public oacceptEPOSConditionsDto acceptEPOSConditions()
        {

            ServiceHandler<long, oacceptEPOSConditionsDto> ew = new ServiceHandler<long, oacceptEPOSConditionsDto>(0);
            return ew.process(delegate(long inp, oacceptEPOSConditionsDto rval, CredentialContext cctx)
            {

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(cctx.getMembershipInfo());

                bo.acceptEPOSConditions();
                rval.success();

            });

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
                AbstractPrismaServiceBo bo = new PrismaServiceBo(PrismaDaoFactory.getInstance().getPrismaServiceDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());
                List<AvailableServiceDto> services = bo.listAvailableServices(input.kontext, cctx.getUserLanguange());
                rval.services = services.ToArray();

                rval.success();
                return rval;
            }
            catch (System.Data.EntityException e)
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

        /// <returns></returns>
        /// <summary>
        /// Liefert eine Liste der verfügbaren News im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableNewsDto</param>
        /// <returns>olistAvailableNewsDto</returns>
        public olistAvailableNewsDto listAvailableNews(ilistAvailableNewsDto input)
        {
            olistAvailableNewsDto rval = new olistAvailableNewsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listAvailableNewsDto is send");
                }
                cctx.validateService();

                IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                PrismaNewsBo bo = new PrismaNewsBo(pDao, obDao, PrismaNewsBo.CONDITIONS_BANKNOW, cctx.getUserLanguange());

                //create context
                Cic.OpenOne.Common.DTO.Prisma.prKontextDto kontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();
                kontext.sysbrand = input.sysbrand;
                kontext.sysprchannel = input.sysprchannel;
                kontext.sysperole = cctx.getMembershipInfo().sysPEROLE;

                List<Cic.OpenOne.Common.DTO.AvailableNewsDto> news = bo.listAvailableNews(kontext, cctx.getUserLanguange(), input.binaryData);
                rval.news = Mapper.Map<Cic.OpenOne.Common.DTO.AvailableNewsDto[], Cic.One.DTO.AvailableNewsDto[]>(news.ToArray());
                rval.news = (from s in rval.news
                             select s).OrderByDescending(x=>x.datum).ToArray();
                rval.success();
                return rval;
            }
            catch (System.Data.EntityException e)
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
                // Entsorgt die nicht verwendung der Exception
                //String message = e.Message;
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                // Entsorgt die nicht verwendung der Exception
                String message = e.Message;
                //cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Produkte im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableServicesDto</param>
        /// <returns>olistAvailableServicesDto</returns>
        public olistAvailableProductsDto listAvailableProducts(ilistAvailableProductsDto input)
        {
            olistAvailableProductsDto rval = new olistAvailableProductsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input ilistAvailableProductsDto was sent.");
                }
                alterKontext(input.kontext);

                //for FZ (sysprchannel 1) you need sysperole, sysobtyp and sysprusetype

                IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                Cic.OpenOne.GateBANKNOW.Common.DAO.IPruefungDao pruefDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getPruefungDao();

                ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                PrismaProductBo bo = new PrismaProductBo(pDao, obDao, transDao, PrismaProductBo.CONDITIONS_BANKNOW, cctx.getMembershipInfo().ISOLanguageCode);
                //IPrismaParameterBo paramBo = new PrismaParameterBo(pDao, obDao, PrismaParameterBo.CONDITIONS_BANKNOW);

                //Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo pruefungbo = new Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo(pDao, obDao, transDao, pruefDao);
                //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
                
                List<Cic.OpenOne.Common.Model.Prisma.PRPRODUCT> products = bo.listAvailableProducts(input.kontext);
                AvailableProduktDto[] sortresult = bo.listSortedAvailableProducts(products, input.kontext.sysprbildwelt).ToArray();
                //Cic.OpenOne.GateBANKNOW.Common.DTO.JokerPruefungDto resultJokerPruefung = pruefungbo.analyzeJokerProducts(sortresult, kontext);

                List<DropListDto> availableProducts = new List<DropListDto>();
                foreach (AvailableProduktDto p in sortresult)
                {
                    DropListDto dp = new DropListDto();
                    dp.sysID = p.sysID;
                    dp.beschreibung = p.beschreibung;
                    dp.bezeichnung = p.bezeichnung;
                    if (!String.IsNullOrEmpty(p.vttypcode))
					{
						if (p.vttypcode.Equals ("CREDIT-NOW CASA FLEX"))
							dp.code = "FLEX";       // due to new vttyp.UNTERTYP (rh 20171027)
						else if (p.vttypcode.Equals ("CREDIT-NOW DIPLOMA"))
							dp.code = "DIPLOMA";
						else if (p.vttypcode.Equals ("CREDIT-NOW FLEX"))
							dp.code = "DISPO";
						else if (p.vttypcode.IndexOf ("CREDIT-NOW CASA") > -1)
							dp.code = "CASA";
						else
							dp.code = p.vttypcode; //original without mapping
                    }
                    else
                    {
                        Cic.OpenOne.Common.Model.Prisma.VART vart = pDao.getVertragsart(p.sysID);
                        if (vart != null)
                        {
                            if (vart.CODE.ToUpper().Equals("KREDIT_CLASSIC"))
                                dp.code = "CLASSIC";
                            else if (vart.CODE.ToUpper().Equals("KREDIT_DISPOPLUS"))
                                dp.code = "CARD";
                            else if (vart.CODE.ToUpper().Equals("KREDIT_DISPO"))
                                dp.code = "DISPO";
                            else if (vart.CODE.ToUpper().Equals("LEASING"))
                                dp.code = "LEASE";
                            else if (vart.CODE.ToUpper().Equals("KREDIT_FIX"))
                                dp.code = "CREDIT";
                            else if (vart.CODE.ToUpper().Equals("KREDIT_EXPRESS"))
                                dp.code = "EXPRESS";
                            else if (vart.CODE.ToUpper().Equals("TZK_PLUS"))
                                dp.code = "CAR";
                            else
                                dp.code = vart.CODE; //original without mapping
                        }
                    }
                    if (dp.code != null)
                        availableProducts.Add(dp);
                }
                rval.products = availableProducts.ToArray();

                rval.success();
                return rval;
            }
            catch (System.Data.EntityException e)
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
        /// Adjusts the srv/prod Kontexts' long values by configurations inside SETUP.NET/OVERWRITE/VARNAME
        /// Enabled only if web.config contains Cic.OpenOne.GateBANKNOW.Common.Settings setting name ContextOverride value true
        /// </summary>
        /// <param name="ctx"></param>
        private void alterKontext(object ctx)
        {
            if (Settings.Default.ContextOverride != null && "true".Equals(Settings.Default.ContextOverride.ToLower()))
            {
                foreach (PropertyInfo pi in ctx.GetType().GetProperties())
                {
                    if (pi.PropertyType != typeof(long))
                        continue;

                    pi.SetValue(ctx, AppConfig.Instance.GetCfgEntry("SETUP.NET", "OVERWRITE", pi.Name.ToUpper(), (long)pi.GetValue(ctx, null)), null);
                }
            }
        }


        
        /// <summary>
        /// returns ewk/bwk/ga details
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ogetAuskunftDetailDto getAuskunftDetail(igetAuskunftDetailDto inp)
        {
            ServiceHandler<igetAuskunftDetailDto, ogetAuskunftDetailDto> ew = new ServiceHandler<igetAuskunftDetailDto, ogetAuskunftDetailDto>(inp);
            return ew.process(delegate(igetAuskunftDetailDto input, ogetAuskunftDetailDto rval, CredentialContext cctx)
            {
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(cctx.getMembershipInfo());
                if (bo is Cic.One.GateBANKNOW.BO.EntityBo)
                {
                    ((Cic.One.GateBANKNOW.BO.EntityBo)bo).getAuskunftDetail(input, rval);
                }
                rval.success();
            });
        }

        /// <summary>
        /// person info
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ogetBNKundeDetailDto getBNKundeDetail(igetBNKundeDetailDto inp)
        {
            ServiceHandler<igetBNKundeDetailDto, ogetBNKundeDetailDto> ew = new ServiceHandler<igetBNKundeDetailDto,ogetBNKundeDetailDto>(inp);
            return ew.process(delegate(igetBNKundeDetailDto input, ogetBNKundeDetailDto rval, CredentialContext cctx)
            {
                // rval.kunde = Mapper.Map<ItDto, BNKundeDto>(input.it);
                long itid = 0;
                bool newIt = false;
                bool wasPerson = false;
                
                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kd = null;
                if (input.it != null && input.it.sysit > 0)
                    itid = input.it.sysit;
                else if (input.kd != null && "IT".Equals(input.kd.area))
                    itid = input.kd.entityId;
                else if (input.kd != null)
                {
                    IEntityDao ed = DAOFactoryFactory.getInstance().getEntityDao();
                    ed.setSysPerole (cctx.getMembershipInfo().sysPEROLE);
					/* itid = ed.getItIdFromPerson (input.kd.entityId, true);
                    if (itid == 0)
                    {
                        newIt = true;													//create new It
                        itid = ed.getItIdFromPerson(input.kd.entityId, false);
                        if (itid == 0)													// no it for PersonDto available
                        {
                            IKundeDao kddao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getKundeDao();
                            kd = kddao.getKundeBySysKd(input.kd.entityId);
                            kd.sysit = 0;
                            kd.syskd = input.kd.entityId;
                            // kddto = kddao.createKunde(kddto, cctx.getMembershipInfo().sysPEROLE);
                            // itid = kddto.sysit;
                        }
                    }
                    * */
                    itid = ed.getItIdFromPerson (input.kd.entityId, true);
                    IKundeDao kddao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getKundeDao();
                    kd = kddao.getKundeBySysKd (input.kd.entityId);
                    kd.sysit = itid;
                    kd.syskd = input.kd.entityId;
                    kd.kontos = null;
                    wasPerson = true;
                    // falls it zu person eine korradresse hat, die ID füllen
                    if (kd.sysit > 0)
                    {
                        Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto[] adr = kddao.getAdressen(kd.sysit);
                        if (adr != null && adr.Length > 0)
                            kd.sysitkorradresse = adr[0].sysadresse;
                    }
                }

                IKundeBo kdBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundeBo();
                if (kd == null)
                    kd = kdBo.getKunde(itid);
                if (newIt)
                {
                    kd.sysit = 0;
                }
                if (wasPerson)
                {
                    kd.syskd = input.kd.entityId;
                    // aktuellste Personen-Zusatzdaten holen
                    ZusatzdatenDao zdao = new ZusatzdatenDao();
                    kd.zusatzdaten = zdao.getPersonZusatzdaten(input.kd.entityId);
                    if (newIt)
					{// delete references to force new it
                        kd.sysit = 0;
                        if (kd.zusatzdaten != null && kd.zusatzdaten.Length > 0)
                        {
                            if (kd.zusatzdaten[0].pkz != null && kd.zusatzdaten[0].pkz.Length > 0)
                            {
                                kd.zusatzdaten[0].pkz[0].syspkz = 0;
                                kd.zusatzdaten[0].pkz[0].sysantrag = 0;
                                kd.zusatzdaten[0].pkz[0].sysangebot = 0;                              
                            }
                            if (kd.zusatzdaten[0].ukz != null && kd.zusatzdaten[0].ukz.Length > 0)
                            {
                                kd.zusatzdaten[0].ukz[0].sysukz = 0;
                                kd.zusatzdaten[0].ukz[0].sysantrag = 0;
                                kd.zusatzdaten[0].ukz[0].sysangebot = 0;                               
                            }
                        }
                    }
                    if (kd.zusatzdaten != null && kd.zusatzdaten.Length > 0)
                    {
                        if (kd.zusatzdaten[0].pkz != null && kd.zusatzdaten[0].pkz.Length > 0)
                        {
							// BNRVZ-1370 FLAG soll erhalten bleiben, NUR die legitMethod ausnullen: kd.zusatzdaten[0].pkz[0].kdIdentFlag = 0;
                            kd.zusatzdaten[0].pkz[0].legitMethodCode=null;
                            kd.zusatzdaten[0].pkz[0].legitAbnehmer=null;
                            kd.zusatzdaten[0].pkz[0].legitDatum = null;
                        }
                        if (kd.zusatzdaten[0].ukz != null && kd.zusatzdaten[0].ukz.Length > 0)
                        {
							// BNRVZ-1370 FLAG soll erhalten bleiben, NUR die legitMethod ausnullen: kd.zusatzdaten[0].ukz[0].kdIdentFlag = 0;
                            kd.zusatzdaten[0].ukz[0].legitMethodCode = null;
                            kd.zusatzdaten[0].ukz[0].legitAbnehmer = null;
                            kd.zusatzdaten[0].ukz[0].legitDatum = null;
                        }
                    }
                }
                // BNRVZ-1039 PKZ daten LEEREN für alte bestandsdaten
                
                if (kd.zusatzdaten != null && kd.zusatzdaten.Length > 0)
                {
					if (kd.zusatzdaten[0].pkz != null && kd.zusatzdaten[0].pkz.Length > 0)
					{
						bool qstFlag = kd.zusatzdaten[0].pkz[0].quellensteuerFlag;
						kd.zusatzdaten[0].pkz[0].quellensteuerFlag = false;
						if (qstFlag)
						{
							kd.zusatzdaten[0].pkz[0].einkbrutto = 0;
							kd.zusatzdaten[0].pkz[0].einknetto = 0;
							kd.zusatzdaten[0].pkz[0].jbonusbrutto = 0;
							kd.zusatzdaten[0].pkz[0].jbonusnetto = 0;

							kd.zusatzdaten[0].pkz[0].zulageausbildung = 0;
							kd.zusatzdaten[0].pkz[0].zulagekind = 0;
							kd.zusatzdaten[0].pkz[0].zulagesonst = 0;
						}

						// BNRVZ-1370 auch PKZ-legitMethodCode LEEREN für alte Bestandsdaten (rh: 20170707)
						// BNRVZ-1370 FLAG soll erhalten bleiben, NUR die legitMethod ausnullen: kd.zusatzdaten[0].pkz[0].kdIdentFlag = 0;
						kd.zusatzdaten[0].pkz[0].legitMethodCode = null;
						kd.zusatzdaten[0].pkz[0].legitAbnehmer = null;
						kd.zusatzdaten[0].pkz[0].legitDatum = null;
					}
					else
					{// BNRVZ-1370: UKZ-legitMethodCode LEEREN für alte Bestandsdaten (rh: 20170707)
						if (kd.zusatzdaten[0].ukz != null && kd.zusatzdaten[0].ukz.Length > 0)
						{
							// BNRVZ-1370 FLAG soll erhalten bleiben, NUR die legitMethod ausnullen: kd.zusatzdaten[0].ukz[0].kdIdentFlag = 0;
							kd.zusatzdaten[0].ukz[0].legitMethodCode = null;
							kd.zusatzdaten[0].ukz[0].legitAbnehmer = null;
							kd.zusatzdaten[0].ukz[0].legitDatum = null;
						}
					}
                }
                

                rval.kunde = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.One.DTO.BNKundeDto>(kd);
                
            });
        }

        /// <summary>
        /// Bank-Now Ratenkalkulator
        /// löscht die aktuelle Kalkulation auf und liefert die Berechnungsergebnisse
        /// </summary>
        /// <param name="input">isolveKalkulationDto</param>
        /// <returns>osolveKalkulationDto</returns>
        public Cic.One.DTO.BN.osolveBNKalkulationDto solveBNKalkulation(Cic.One.DTO.BN.isolveBNKalkulationDto inp)
        {
            ServiceHandler<Cic.One.DTO.BN.isolveBNKalkulationDto, Cic.One.DTO.BN.osolveBNKalkulationDto> ew = new ServiceHandler<Cic.One.DTO.BN.isolveBNKalkulationDto, Cic.One.DTO.BN.osolveBNKalkulationDto>(inp);
            return ew.process(delegate(Cic.One.DTO.BN.isolveBNKalkulationDto input, Cic.One.DTO.BN.osolveBNKalkulationDto rval, CredentialContext cctx)
            {
                byte rateError = 0;
                if (input == null)
                {
                    throw new ArgumentException("No input (isolveKalkulationDto) was sent.");
                }

                IKalkulationBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());

                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = Mapper.Map<Cic.One.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(input.kalkulation);
                Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext kalkKontext = Mapper.Map<Cic.One.DTO.BN.kalkKontext, Cic.OpenOne.GateBANKNOW.Common.DTO.kalkKontext>(input.kalkKontext);
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = bo.calculate(kalkulationInput, input.prodKontext, kalkKontext, cctx.getUserLanguange(), ref rateError);
                Cic.One.DTO.KalkulationDto kalkulationOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.One.DTO.KalkulationDto>(kalk);
                //KalkulationServicesBo kservice = new KalkulationServicesBo();
                //rval.zusatzinformationen = kservice.aggregateZusatzinformation(kalk);

                rval.kalkulation = kalkulationOutput;
                rval.success();
            });

        }


        /// <summary>
        /// Prüft die Kalkulation
        /// </summary>
        /// <param name="input">icheckAngebotDto</param>
        /// <returns>ocheckAngebotDto</returns>
        public ocheckAntAngDto checkKalkulation(icheckKalkulationDto inp)
        {
            ServiceHandler<icheckKalkulationDto, ocheckAntAngDto> ew = new ServiceHandler<icheckKalkulationDto, ocheckAntAngDto>(inp);
            return ew.process(delegate(icheckKalkulationDto input, ocheckAntAngDto rval, CredentialContext cctx)
            {

                IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto calc = new Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto();
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto kalkulationInput = Mapper.Map<Cic.One.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>(input.kalkulation);
                calc.angAntKalkDto = kalkulationInput;

                List<Cic.OpenOne.Common.DTO.AngAntVsDto> versicherungen = new List<OpenOne.Common.DTO.AngAntVsDto>();
                if (input.angAntVs != null && input.angAntVs.Length > 0)
                {
                    foreach (Cic.OpenOne.Common.DTO.AngAntVsDto vs in input.angAntVs)
                    {
                        versicherungen.Add(vs);
                    }
                }
                calc.angAntVsDto = versicherungen;
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObSmallDto antAntObDto = Mapper.Map<Cic.One.DTO.BN.AngAntObSmallDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObSmallDto>(input.angAntOb);
                Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto, ocheckAntAngDto>(bo.checkAngebot(calc, input.prodKontext, cctx.getUserLanguange(), antAntObDto),rval);

                rval.success();
            });
        }




        /// <summary>
        /// delivers Mailmsg detail
        /// </summary>
        /// <param name="mailmsg"></param>
        /// <returns></returns>
        public osendRiskmailDto sendRiskmail(isendRiskmailDto riskmail) {
            ServiceHandler<isendRiskmailDto, osendRiskmailDto> ew = new ServiceHandler<isendRiskmailDto, osendRiskmailDto>(riskmail);
            return ew.process(delegate(isendRiskmailDto input, osendRiskmailDto rval, CredentialContext cctx) {
                if (input == null)
                    throw new ArgumentException("No valid input");
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(cctx.getMembershipInfo());
                if (bo != null)
                {
                    rval = bo.sendRiskmail(riskmail);
                }

                rval.success();

            });
        }
         
        }
       
}
