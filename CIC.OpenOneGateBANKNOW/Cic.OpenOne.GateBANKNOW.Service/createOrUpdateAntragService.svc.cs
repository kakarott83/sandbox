using AutoMapper;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Affinity;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "createOrUpdateAntragService" in code, svc and config file together.
    /// <summary>
    /// Der Service createOrUpdateAntrag liefert Listen zu verschiedenen Arten Typen und Produkten. Desweiteren lassen sich Informationen editieren und speichern. Außerdem kann man über diesen Service den Antrag einreichen.
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class createOrUpdateAntragService : IcreateOrUpdateAntragService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Objekttypen im Kontext
        /// </summary>
        /// <returns>olistAvailableObjekttypenDto</returns>
        public olistAvailableObjekttypenDto listAvailableObjekttypen()
        {
            olistAvailableObjekttypenDto rval = new olistAvailableObjekttypenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    long sysperole = PeRoleUtil.FindRootPEROLEByRoleType(ctx , cctx.getMembershipInfo().sysPEROLE, (int)RoleTypeTyp.HAENDLER);
                    rval.objekttypen = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableObjekttypen(cctx.getUserLanguange(), sysperole);

                    rval.success();
                }
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
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
        /// Liefert eine Liste der verfügbaren Objektarten im Kontext
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        public olistAvailableObjektartenDto listAvailableObjektarten()
        {
            olistAvailableObjektartenDto rval = new olistAvailableObjektartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.objektarten = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableObjektarten(cctx.getUserLanguange());

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
        /// Liefert eine Matrix der Produkte(X)/Laufzeiten(Y) für die Kreditlimits im angegebenen Kontext
        /// </summary>
        /// <param name="input">igetCreditLimitsDto</param>
        /// <returns>ogetCreditLimitsDto</returns>
        public ogetCreditLimitsDto getCreditLimits(DTO.igetCreditLimitsDto input)
        {
            ogetCreditLimitsDto rval = new ogetCreditLimitsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input igetCreditLimitsDto was sent.");
                }
                cctx.validateService();


                
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysantrag, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                long user = cctx.getMembershipInfo().sysWFUSER;

                IKundenRisikoBo bo = BOFactory.getInstance().createKundenRisikoBO(cctx.getMembershipInfo().ISOLanguageCode);
                List<Cic.OpenOne.GateBANKNOW.Common.DTO.ProductCreditInfoDto> cls = bo.getCreditLimits(input.kontext, input.sysantrag, input.sysvart, cctx.getUserLanguange(), cctx.getMembershipInfo().sysWFUSER);
                rval.products = Mapper.Map<List<Cic.OpenOne.GateBANKNOW.Common.DTO.ProductCreditInfoDto>, List<Cic.OpenOne.GateBANKNOW.Service.DTO.ProductCreditInfoDto>>(cls);
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
        /// Liefert eine Liste der verfügbaren Produkte im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableProdukteDto</param>
        /// <returns>olistAvailableProdukteDto</returns>
        public olistAvailableProdukteDto listAvailableProdukte(DTO.ilistAvailableProdukteDto input)
        {
            olistAvailableProdukteDto rval = new olistAvailableProdukteDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listAvailableProdukteDto was sent.");
                }
                cctx.validateService();

                if (input.kontext.sysperole == 0)
                    input.kontext.sysperole = cctx.getMembershipInfo().sysPEROLE;
                if (input.kontext.sysbrand == 0) input.kontext.sysbrand = cctx.getMembershipInfo().sysBRAND;

                IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                PrismaProductBo bo = new PrismaProductBo(pDao, obDao, transDao, PrismaProductBo.CONDITIONS_BANKNOW, cctx.getUserLanguange());
                Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo pruefungbo = new Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo(pDao, obDao, transDao, Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getPruefungDao());
                //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
                List<CIC.Database.PRISMA.EF6.Model.PRPRODUCT> products = bo.listAvailableProducts(input.kontext);



                AvailableProduktDto[] sortresult = bo.listSortedAvailableProducts(products, input.kontext.sysprbildwelt).ToArray();
                Common.DTO.JokerPruefungDto resultJokerPruefung = pruefungbo.analyzeJokerProducts(sortresult, input.kontext);

                rval.produkte = resultJokerPruefung.products.ToArray();


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
        /// Liefert eine Liste der verfügbaren Kundentypen im Kontext
        /// </summary>
        /// <returns>olistAvailableKundentypenDto</returns>
        public olistAvailableKundentypenDto listAvailableKundentypen()
        {
            olistAvailableKundentypenDto rval = new olistAvailableKundentypenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.kundentypen = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableKundentypen(cctx.getUserLanguange());

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
        /// Liefert eine Liste der verfügbaren Nutzungsarten im Kontext
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        public olistAvailableNutzungsartenDto listAvailableNutzungsarten()
        {
            olistAvailableNutzungsartenDto rval = new olistAvailableNutzungsartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.nutzungsarten = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableNutzungsarten(cctx.getUserLanguange());

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
        /// Liefert eine Liste der verfügbaren Nutzungsarten im Kontext
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        public olistAvailableNutzungsartenDto listAvailableNutzungsartenPrivat() {
            olistAvailableNutzungsartenDto rval = new olistAvailableNutzungsartenDto();
            CredentialContext cctx = new CredentialContext();

            
            string[] PRIVAT = new string[] { "2", "3" };
            try
            {
                DropListDto[] nutzungsarten = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableNutzungsarten(cctx.getUserLanguange());
                int i = 0;
                rval.nutzungsarten = new DropListDto[2];
                foreach (DropListDto art in nutzungsarten)
                    if (PRIVAT.Contains(art.sysID.ToString()))
                    {
                        DropListDto part = new DropListDto();
                        part = art;
                        rval.nutzungsarten[i] = art;
                        i++;
                    }
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
        /// Liefert eine Liste der verfügbaren Nutzungsarten im Kontext
        /// </summary>
        /// <returns>olistAvailableNutzungsartenDto</returns>
        public olistAvailableNutzungsartenDto listAvailableNutzungsartenFirma() {
            olistAvailableNutzungsartenDto rval = new olistAvailableNutzungsartenDto();
            CredentialContext cctx = new CredentialContext();

            
            string[] PRIVAT = new string[] { "3", "4" };
            try
            {
                DropListDto[] nutzungsarten = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableNutzungsarten(cctx.getUserLanguange());
                int i = 0;
                rval.nutzungsarten = new DropListDto[2];
                foreach (DropListDto art in nutzungsarten)
                    if (PRIVAT.Contains(art.sysID.ToString()))
                    {
                        DropListDto part = new DropListDto();
                        part = art;
                        rval.nutzungsarten[i] = art;
                        i++;
                    }
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
        /// Liefert eine Liste aller Parameter und Eckwerte des Produkts im Kontext
        /// </summary>
        /// <param name="input">igetParameterDto</param>
        /// <returns>ogetParameterDto</returns>
        public ogetParameterDto getParameter(igetParameterDto input)
        {
            ogetParameterDto rval = new ogetParameterDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input getParameterDto was sent.");
                }
                cctx.validateService();
                if (input.kontext.sysperole == 0)
                    input.kontext.sysperole = cctx.getMembershipInfo().sysPEROLE;
                if (input.kontext.sysbrand == 0) input.kontext.sysbrand = cctx.getMembershipInfo().sysBRAND;

                IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();

                IPrismaParameterBo bo = new PrismaParameterBo(pDao, obDao, PrismaParameterBo.CONDITIONS_BANKNOW);
                //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
                input.kontext.sysperole = obDao.getHaendlerByEmployee(input.kontext.sysperole);
                List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> prparams = bo.listAvailableParameter(input.kontext);

                rval.parameters = new Cic.OpenOne.GateBANKNOW.Service.DTO.ParamDto[prparams.Count];
                rval.parameters = Mapper.Map<Cic.OpenOne.Common.DTO.Prisma.ParamDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.ParamDto[]>(prparams.ToArray());

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
        /// Löst die aktuelle Kalkulation auf und liefert die Berechnungsergebnisse
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
                    throw new ArgumentException("No input solveKalkulationDto was sent.");
                }
                cctx.validateService();

                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(input.kalkulation);
                Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = bo.calculate(kalkulationInput, input.prodKontext, input.kalkKontext, cctx.getUserLanguange(), ref rateError);

                KalkulationServicesBo kservice = new KalkulationServicesBo();
                rval.zusatzinformationen = kservice.aggregateZusatzinformation(kalk);
                Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto kalkulationOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>(kalk);
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
        /// Calculates Provisions for Expected Loss Calculation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocalculateProvisionsDto calculateProvisions(icalculateProvisionsDto input)
        {
            ocalculateProvisionsDto rval = new ocalculateProvisionsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
             
                if (input == null)
                {
                    throw new ArgumentException("No input icalculateProvisionsDto was sent.");
                }
                cctx.validateService();

                
                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());

                if (input.prodKontext.sysprhgroup == 0)
                {
                    IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                    BankNowCalculator.autoAssignPrhgroup(input.prodKontext, obDao, PrismaDaoFactory.getInstance().getPrismaDao());
                }

                List<OpenOne.Common.DTO.AngAntProvDto>  provs = bo.calculateProvisionsDirect(input.prodKontext, input.kundenScore, input.finanzierungsbetrag, input.zinsertrag);

                rval.provisions = Mapper.Map<List<OpenOne.Common.DTO.AngAntProvDto>, List<DTO.AngAntProvDto>>(provs);


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
        /// Speichert Persistenzobjekt des Antrags
        /// </summary>
        /// <param name="input">isaveAntragDto</param>
        /// <returns>osaveAntragDto</returns>
        public osaveAntragDto saveAntrag(isaveAntragDto input)
        {
            osaveAntragDto rval = new osaveAntragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input saveAntragDto was sent.");
                }
                if (input.antrag == null)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }

                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();

                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antragInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>(input.antrag);
                // in createOrUpdateAntrag wird der Antrag bereits gespeichert (entweder als Neuanlage oder Update)
                // saveAntrag ist also identisch mit createOrUpdateAntrag
                bo.createOrUpdateAntrag(antragInput, cctx.getMembershipInfo().sysPEROLE);

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
        /// Prüft den Antrag
        /// </summary>
        /// <param name="input">icheckAntragDto</param>
        /// <returns>ocheckAntragDto</returns>
        public Service.DTO.ocheckAntAngDto checkAntrag(icheckAntragDto input)
        {
            Service.DTO.ocheckAntAngDto rval = new Service.DTO.ocheckAntAngDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);

                
                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                rval = Mapper.Map<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>(bo.checkAntragByIdErweiterung(input.antrag.sysid, 0, cctx.getUserLanguange(), true, false, 0, cctx.getMembershipInfo().sysWFUSER, cctx.getMembershipInfo().sysPEROLE));

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
        /// Prüft den Antrag
        /// </summary>
        /// <param name="sysid">id des Antrags</param>
        /// <param name="sysvart">id der Vertragsart</param>
        /// <returns>Status der Antragsprüfung (rot, grün, gelb) und/oder Fehler</returns>
        public Service.DTO.ocheckAntAngDto checkAntragById(long sysid, long sysvart)
        {
            Service.DTO.ocheckAntAngDto rval = new Service.DTO.ocheckAntAngDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, sysid, DateTime.Now);

                
                IAngAntBo bo = BOFactory.getInstance().createAngAntBoMA();

                rval = Mapper.Map<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>(bo.checkAntragByIdErweiterung(sysid, sysvart, cctx.getUserLanguange(), false, true, 0, cctx.getMembershipInfo().sysWFUSER, cctx.getMembershipInfo().sysPEROLE));

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
        /// Prüft den Antrag
        /// </summary>
        /// <param name="sysid">id des Antrags</param>
        /// <param name="sysvart">id der Vertragsart</param>
        /// <param name="sysprproduct">id der Prproduct</param>
        /// <returns>Status der Antragsprüfung (rot, grün, gelb) und/oder Fehler</returns>
        public Service.DTO.ocheckAntAngDto checkAntragById2(long sysid, long sysvart, long sysprproduct)
        {
            Service.DTO.ocheckAntAngDto rval = new Service.DTO.ocheckAntAngDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, sysid, DateTime.Now);

                
                IAngAntBo bo = BOFactory.getInstance().createAngAntBoMA();

                rval = Mapper.Map<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>(bo.checkAntragByIdErweiterung(sysid, sysvart, cctx.getUserLanguange(), false, true, sysprproduct, cctx.getMembershipInfo().sysWFUSER, cctx.getMembershipInfo().sysPEROLE));

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
        /// Prüft den Antrag Flag
        /// </summary>
        /// <param name="sysid">id des Antrags</param>
        /// <param name="sysvart">id der Vertragsart</param>
        /// <param name="nurallgemeine">nur allgemeine Prüfung</param>
        /// <returns>Status der Antragsprüfung (rot, grün, gelb) und/oder Fehler</returns>
        public Service.DTO.ocheckAntAngDto checkAntragByIdFlag(long sysid, long sysvart, bool nurallgemeine)
        {
            Service.DTO.ocheckAntAngDto rval = new Service.DTO.ocheckAntAngDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, sysid, DateTime.Now);

                
                IAngAntBo bo = BOFactory.getInstance().createAngAntBoMA();

                rval = Mapper.Map<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>(bo.checkAntragByIdErweiterung(sysid, sysvart, cctx.getUserLanguange(), false, nurallgemeine, 0, cctx.getMembershipInfo().sysWFUSER, cctx.getMembershipInfo().sysPEROLE));

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
        /// Prüft den Antrag Flag
        /// </summary>
        /// <param name="sysid">id des Antrags</param>
        /// <param name="sysvart">id der Vertragsart</param>
        /// <param name="nurallgemeine">nur allgemeine Prüfung</param>
        /// <param name="sysprproduct">id der Prproduct</param>
        /// <returns>Status der Antragsprüfung (rot, grün, gelb) und/oder Fehler</returns>
        public Service.DTO.ocheckAntAngDto checkAntragByIdFlag2(long sysid, long sysvart, bool nurallgemeine, long sysprproduct)
        {
            Service.DTO.ocheckAntAngDto rval = new Service.DTO.ocheckAntAngDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, sysid, DateTime.Now);

                
                IAngAntBo bo = BOFactory.getInstance().createAngAntBoMA();

                Service.DTO.ocheckAntAngDto rval2 = Mapper.Map<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>(bo.checkAntragByIdErweiterung(sysid, sysvart, cctx.getUserLanguange(), false, nurallgemeine, sysprproduct, cctx.getMembershipInfo().sysWFUSER, cctx.getMembershipInfo().sysPEROLE));
                //assign values, else the duration will be lost
                rval.code = rval2.code;
                rval.errortext = rval2.errortext;
                rval.frontid = rval2.frontid;
                rval.status = rval2.status;

                if (rval.status == Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_GREEN)
                {
                    // TransactionRisiko OK
                    // SimulationRisikoprüfung OK
                    // Die Produktprüfung ist erfolgreich und sie erhalten innerhalb kürzester Zeit die Antragsentscheidung 	CHECK_OK
                    // User kann erneut kalkulieren? nein


                    rval.frontid = "CHECK_OK";

                    rval.success();


                }

                 else
                    if ((rval.status == Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_RED ||rval.status == Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_YELLOW)  && String.Join(",",rval.code).Equals("FEL1"))
                    {
                        // produktprüfung OK
                        

                        ITransactionRisikoBo boT = BOFactory.getInstance().createTransactionRisikoBO();
                        

                        Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto outputTR = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckTrRiskDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto>(boT.checkTrRiskBySysid(sysid, cctx.getMembershipInfo().sysPEROLE, cctx.getUserLanguange(), cctx.getMembershipInfo().sysWFUSER));
                        

                        // TransactionRisiko OK?// TransactionRisiko OK?
                        if (outputTR.frontid != null && (outputTR.frontid.Equals("CHECK_OK") || outputTR.frontid.Equals("CHECK_DE_OK") || outputTR.frontid.Equals("CHECK_AUSW_OK")))
                        {
                            
                            if (outputTR.frontid.Equals("CHECK_AUSW_OK"))
                            {
                                rval.errortext = new List<string>();
                                rval.code = new List<string>();
                                rval.status = Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_GREEN;
                            }
                            //  SimulationRisikoprüfung OK??
                            //  ja: 
                                    //B2BzustandExtern = "Finanzierungsvorschlag";
                                    rval.frontid = "CHECK_OK";


                            //  nein: 
                            //rval.frontid = "CHECK_DE_NOK";

                        }
                        else
                            // nein :
                            rval.frontid = "CHECK_TR_NOK";
                                // User kann erneut kalkulieren? JA
                                


                    }
                    else
                    {
                        
                        rval.frontid = "CHECK_NOK";

                    }


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
        /// Reicht den Antrag ein und gibt den Vertrag zurück
        /// 
        /// nicht vertrag zurückgeben (nichts zurückgeben)
        /// ZUSTAND+ATTRIBUT (==Status) auf antrag muss gesetzt werden (so dass er stas in inbox erscheint)
        ///
        /// select * from wftzust;-> syslease = antragsid, syswftable = antrag
        /// select * from wftzvar; von bestimmtem typ -> anlegen
        /// 
        /// </summary>
        /// <param name="input">iprocessAntragToVertragDto</param>
        /// <returns>oprocessAntragToVertragDto</returns>
        public oprocessAntragToVertragDto processAntragEinreichung(iprocessAntragToVertragDto input)
        {
            oprocessAntragToVertragDto rval = new oprocessAntragToVertragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input processAntragToVertragDto was sent.");
                }
                if (input.antrag == null)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                long syswfuser = cctx.getMembershipInfo().sysWFUSER;

                LogUtil.addTLog("ANTRAG", input.antrag.sysid, input, cctx.getMembershipInfo().sysWFUSER);

                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antragInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>(input.antrag);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();

                // Dummy für die Unterstrukturen
                //Mapper.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.VertragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.VertragDto>();
                bo.processAntragEinreichung(antragInput, syswfuser, cctx.getUserLanguange());


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
        /// createOrUpdateAntrag
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateAntragDto createOrUpdateAntrag(icreateAntragDto input)
        {
            ocreateAntragDto rval = new ocreateAntragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input createAntragDto was sent.");
                }
                if (input.antrag == null)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();

                
                if (input.CRIFReset > 0 && input.antrag.sysid > 0)
                {
                    CRIFBo cbo = new CRIFBo();
                    cbo.resetAuskunfStatus(input.antrag.sysid);//update auskunft set statusnum=1 für area/id von Antrag
                }
                
                Common.DTO.AntragDto antrag = Mapper.Map<Service.DTO.AntragDto, Common.DTO.AntragDto>(input.antrag);

                IKalkulationBo kalkBo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());
                antrag = kalkBo.calculateAntrag(cctx.getMembershipInfo(), antrag);//that also saves with createOrUpdateAntrag inside as last step!


                rval.antrag = Mapper.Map<Common.DTO.AntragDto, Service.DTO.AntragDto>(antrag);

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
        /// createOrUpdateAntrag
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateAntragFromNkkDto createAntragFromNkk(icreateAntragFromNkkDto input)
        {
            ocreateAntragFromNkkDto rval = new ocreateAntragFromNkkDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input icreateAntragFromNKKDto was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo angAntBo = BOFactory.getInstance().createAngAntBo();

                long syswfuser = cctx.getMembershipInfo().sysWFUSER;
                long sysperole = cctx.getMembershipInfo().sysPEROLE;
                string languageCode = cctx.getUserLanguange();

                var antrag = angAntBo.createAntragFromNkk(input.SysNkk, syswfuser, sysperole, languageCode);

                if (antrag != null)
                {
                    rval.SysId = antrag.sysid;
                }

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
        /// Delivers the object information from a car configurator id
        /// </summary>
        /// <param name="key">Schluessel</param>
        /// <returns>Fahrzeugspezifische Daten aus Eurotax</returns>
        public oGetObjektDatenDto getObjektDaten(String key)
        {
            oGetObjektDatenDto rval = new oGetObjektDatenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (key == null)
                {
                    throw new ArgumentException("No input key was sent.");
                }
                cctx.validateService();

                IAngAntBo angAntBo = BOFactory.getInstance().createAngAntBo();
                rval.objekt = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObDto>(angAntBo.getObjektdaten(key));

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
        /// Delivers the object information from a car VIN-Number
        /// </summary>
        /// <param name="key">VIN Number</param>
        /// <returns>Fahrzeugspezifische Daten aus Eurotax map2ETG</returns>
        public oGetObjektDatenDto getObjektDatenByVIN(String key)
        {
            oGetObjektDatenDto rval = new oGetObjektDatenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (key == null)
                {
                    throw new ArgumentException("No input key was sent.");
                }
                cctx.validateService();

                IAngAntBo angAntBo = BOFactory.getInstance().createAngAntBo();
                rval.objekt = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObDto>(angAntBo.getObjektdatenByVIN(key, cctx.getMembershipInfo().sysWFUSER, cctx.getUserLanguange()));

                rval.success();

                return rval;
            }

            catch (ApplicationException e)
            {

                rval.objekt = null;
                cctx.fillBaseDto(rval, e, e.Message);
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
                    throw new ArgumentException("Input was null.");
                }
                if (input.kalkulation == null)
                {
                    throw new ArgumentException("Kalkulation was null.");
                }
                if (input.prodKontext == null)
                {
                    throw new ArgumentException("ProdKontext was null.");
                }

                cctx.validateService();

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>(input.kalkulation);
                List<Cic.OpenOne.Common.DTO.AngAntVsDto> versicherungen = new List<OpenOne.Common.DTO.AngAntVsDto>();
                if (input.angAntVs != null)
                    foreach (Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto vs in input.angAntVs)
                        versicherungen.Add(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>(vs));
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObSmallDto angAntObDto = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObSmallDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObSmallDto>(input.angAntOb);
                Common.DTO.KalkulationDto calc = new Common.DTO.KalkulationDto();
                calc.angAntKalkDto = kalkulationInput;
                calc.angAntVsDto = versicherungen;

                rval = Mapper.Map<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>(bo.checkAngebot(calc, input.prodKontext, cctx.getUserLanguange(), angAntObDto));

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
        /// Gibt alle Optionen für den Lenker zurück
        /// </summary>
        /// <returns>olistAvailableLenkerDto</returns>
        public olistAvailableLenkerDto listAvailableLenker()
        {
            olistAvailableLenkerDto rval = new olistAvailableLenkerDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();

                IDictionaryListsBo bo = BOFactory.getInstance().createDictionaryListsBo(cctx.getUserLanguange());
                rval.lenker = bo.listByCode(DDLKPPOSType.LENKER);

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
        /// listAvailableInsurance
        /// </summary>
        /// <returns></returns>
        public olistAvailableInsuranceDto listAvailableInsurance()
        {
            olistAvailableInsuranceDto rval = new olistAvailableInsuranceDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();

                IDictionaryListsBo bo = BOFactory.getInstance().createDictionaryListsBo(cctx.getUserLanguange());
                
                Cic.OpenOne.GateBANKNOW.Service.DTO.InsuranceDto[] versicherungen = Mapper.Map<Cic.OpenOne.Common.DTO.InsuranceDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.InsuranceDto[]>(bo.listInsurance());

                rval.versicherung = versicherungen;
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
        /// Löst eine Änderung des Restwertrechnungsempfängers aus
        /// </summary>
        /// <param name="input"></param>
        public ochangeRRReceiverDto changeRRReceiver(ichangeRRReceiverDto input)
        {
            ochangeRRReceiverDto rval = new ochangeRRReceiverDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                IVertragBo vertragBo = BOFactory.getInstance().createVertragBo();
                
                rval = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ochangeRRReceiver, Cic.OpenOne.GateBANKNOW.Service.DTO.ochangeRRReceiverDto>(vertragBo.changeRRReceiver(input.sysid));

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
        /// searches for uploaded files
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public osearchUploadDto searchUpload(isearchUploadDto input)
        {
            osearchUploadDto rval = new osearchUploadDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto> bo = new SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto>();
                bo.setPermission(cctx.getMembershipInfo().sysPEROLE, true, "PEROLE");
                Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto> sr = bo.search(input.searchInput);
                Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto, DTO.FileDto> searchMapper = new Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto, DTO.FileDto>();
                rval.result = searchMapper.mapSearchResult(sr);
                foreach (Cic.OpenOne.GateBANKNOW.Service.DTO.FileDto f in rval.result.results)
                {
                    IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                    if (f.syscrtuser > 0)
                    {
                        CIC.Database.OL.EF6.Model.PERSON person = obDao.getPersonByPEROLE(f.syscrtuser);
                        f.username = person.NAME;
                    }
                    if (f.fileName.Contains('.'))
                    {
                        string[] t = f.fileName.Split(new Char[] { '.' });
                        f.format = t.Last();
                    }
                }
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
        /// createOrUpdateUpload
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
                if (input.file == null || input.file.content==null || input.file.content.Length == 0)
                    throw new ArgumentException("Missing File data");

                input.file.syscrtuser = cctx.getMembershipInfo().sysPEROLE;

                //HOTFIX Gesichtskreis
                if (input.file != null && input.file.sysId > 0)
                {
                    using (PrismaExtended ctx = new PrismaExtended())
                    {

                        String query = "SELECT sysid FROM peuni, perolecache WHERE area ='ANTRAG' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = :sysperole and sysid=:sysid";


                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = cctx.getMembershipInfo().sysPEROLE });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = input.file.sysId });
                        long sysidantrag = ctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).FirstOrDefault();
                        if (sysidantrag == 0)
                            throw new ArgumentException("No Permission to upload file.");
                    }
                }
                IFileBo bo = BOFactory.getInstance().createFileBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto fileInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.FileDto, Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto>(input.file);
                fileInput.syswfuser = cctx.getMembershipInfo().sysWFUSER;
                
                Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto fileOutput = bo.createOrUpdateFile(fileInput);
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
        /// Liefert ein Upload-Objekt
        /// </summary>
        /// <param name="input">igetUploadDetailDto</param>
        /// <returns>ogetUploadDetailDto</returns>
        public ogetUploadDetailDto getUploadDetail(igetUploadDetailDto input)
        {
            ogetUploadDetailDto rval = new ogetUploadDetailDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input getUploadDetailDto was sent.");
                }
                if (input.sysfile == 0)
                {
                    throw new ArgumentException("No sysatt was sent.");
                }
                cctx.validateService();
                IFileBo bo = BOFactory.getInstance().createFileBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto file = bo.getFile(input.sysfile);

                rval.file = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.FileDto, Cic.OpenOne.GateBANKNOW.Service.DTO.FileDto>(file);

                //HOTFIX Gesichtskreis
                if(rval.file!=null &&rval.file.sysId>0)
                using (PrismaExtended ctx = new PrismaExtended())
                {

                    String query = "SELECT sysid FROM peuni, perolecache WHERE area ='ANTRAG' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = :sysperole and sysid=:sysid";


                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = cctx.getMembershipInfo().sysPEROLE });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = rval.file.sysId });
                    long sysidantrag = ctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).FirstOrDefault();
                    if(sysidantrag==0)
                        throw new ArgumentException("No Permission to download file.");
                }
               

                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                if (rval.file.syscrtuser > 0)
                {
                    CIC.Database.OL.EF6.Model.PERSON person = obDao.getPersonByPEROLE(rval.file.syscrtuser);
                    rval.file.username = person.NAME;
                }
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
            catch (ServiceBaseException e)      //expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)         //unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }


        #region BNR11_CR_11

        /// <summary>
        /// Finanzierungsvorschlag Einreichen
        /// </summary>
        /// <param name="input">iprocessAntragToVertragDto</param>
        /// <returns>oprocessAntragToVertragDto</returns>
        public Cic.OpenOne.GateBANKNOW.Service.DTO.ofinVorEinreichungDto processFinVorEinreichung(Cic.OpenOne.GateBANKNOW.Service.DTO.ifinVorEinreichungDto input)
        {
            Cic.OpenOne.GateBANKNOW.Service.DTO.ofinVorEinreichungDto rval = new Cic.OpenOne.GateBANKNOW.Service.DTO.ofinVorEinreichungDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input processAntragToVertragDto was sent.");
                }
                if (input.antrag.sysid == 0)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                long user = cctx.getMembershipInfo().sysWFUSER;



                ITransactionRisikoBo TBo = BOFactory.getInstance().createTransactionRisikoBO();

                Cic.OpenOne.GateBANKNOW.Common.DTO.ifinVorEinreichungDto inputBO = new Cic.OpenOne.GateBANKNOW.Common.DTO.ifinVorEinreichungDto();
                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antragInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>(input.antrag);
                inputBO.antrag = antragInput;
                inputBO.user = user;
                inputBO.isocode = cctx.getUserLanguange();
                inputBO.finanzierungsVariante = input.finanzierungsVariante;

                TBo.processFinVorEinreichung(inputBO);


                // Dummy 
                rval.frontid = "THANKS_TEXT2";

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
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public oautomatischePruefungDto automatischePruefung(iautomatischePruefungDto input)
        {
            Cic.OpenOne.GateBANKNOW.Service.DTO.oautomatischePruefungDto rval = new Cic.OpenOne.GateBANKNOW.Service.DTO.oautomatischePruefungDto();
            CredentialContext cctx = new CredentialContext();
            //WENN Produktprüfung Grün ist außer FEL1

            //DANN status = Transaktionsrisiko in Prüfung


            // WENN Produktprüfung mit FEL1 GELB
            //DANN Risikoprüfung durchführen 
            //Wenn Risikoprüfung OK (ohne Stopp)

            //DANN Ändern Produktdaten in ePOS für Händler zulassen
            //Status Antrag in ePOS: Transaktionsrisiko prüfen

            //Wenn Risikoprüfung NOK (Stopps vorhanden)
            //DANN Ändern Produktdaten in ePOS für Händler nicht zulassen, der Antrag muss durch den BN MA bearbeitet werden
            //Status Antrag in MA Client: Produktprüfung NOK
            //Status Antrag in ePOS: Manuelle Prüfung notwendig
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input processAntragToVertragDto was sent.");
                }
                if (input.sysid == 0)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                long user = cctx.getMembershipInfo().sysWFUSER;

                ITransactionRisikoBo TBo = BOFactory.getInstance().createTransactionRisikoBO();

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();


                //Produktprüfung validiert
                //WENN erfolgreicher Produktprüfung wird der Antrag weitergereicht und geprüft

                Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckAntAngDto produktprüfungErgebniss = Mapper.Map<Common.DTO.ocheckAntAngDto, Service.DTO.ocheckAntAngDto>(bo.checkAntragByIdErweiterung(input.sysid, 0, cctx.getUserLanguange(), true, false, 0, cctx.getMembershipInfo().sysWFUSER, cctx.getMembershipInfo().sysPEROLE));
               
                
                if (produktprüfungErgebniss.status == Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_GREEN)
                {
                    // TransactionRisiko OK
                    // SimulationRisikoprüfung OK
                    // Die Produktprüfung ist erfolgreich und sie erhalten innerhalb kürzester Zeit die Antragsentscheidung 	CHECK_OK
                    // User kann erneut kalkulieren? nein
                    

                    rval.frontid = "CHECK_OK";

                    rval.success();


                }
                else
                    if (produktprüfungErgebniss.status == Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_RED && String.Join(",",produktprüfungErgebniss.code).Equals("FEL1"))
                    {
                        // produktprüfung OK
                        

                        ITransactionRisikoBo boT = BOFactory.getInstance().createTransactionRisikoBO();
                        Cic.OpenOne.GateBANKNOW.Common.DTO.icheckTrRiskDto dto = new Cic.OpenOne.GateBANKNOW.Common.DTO.icheckTrRiskDto();

                        Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto outputTR = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckTrRiskDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto>(boT.checkTrRiskBySysid(input.sysid, cctx.getMembershipInfo().sysPEROLE, cctx.getUserLanguange(), cctx.getMembershipInfo().sysWFUSER));
                        
                        // TransactionRisiko OK?// TransactionRisiko OK?
                        if (outputTR.frontid.Equals("CHECK_TR_OK"))
                        {

                            //  SimulationRisikoprüfung OK??
                            //  ja: 
                                    //B2BzustandExtern = "Finanzierungsvorschlag";
                                    //rval.frontid = "CHECK_OK";


                            //  nein: 
                            //rval.frontid = "CHECK_NOK";

                        }
                        else
                            // nein :
                            rval.frontid = "CHECK_NOK";
                                // User kann erneut kalkulieren? JA
                                


                    }
                    else
                    {
                        //Falls die Produktprüfung nicht erfolgreich ist so wird der Antrag zur Bearbeitung freigegeben
                        //Anzeige der getroffenen Regel gem. bestehender Logik

                        rval.checkAntAngDto = produktprüfungErgebniss;
                        rval.frontid = "CHECK_NOK";

                    }



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
        /// Varianten rechnen
        /// Sollte eine und/oder beiden Kalkulationen nicht OK sein, müssen die verschiedenen Varianten gerechnet werden 
        /// Deprecated?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Cic.OpenOne.GateBANKNOW.Service.DTO.osolveKalkVariantenDto solveKalkVarianten(Cic.OpenOne.GateBANKNOW.Service.DTO.isolveKalkVariantenDto input)
        {
            Cic.OpenOne.GateBANKNOW.Service.DTO.osolveKalkVariantenDto rval = new Cic.OpenOne.GateBANKNOW.Service.DTO.osolveKalkVariantenDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input processAntragToVertragDto was sent.");
                }
                if (input.antrag.sysid == 0)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                long user = cctx.getMembershipInfo().sysWFUSER;



                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();



                //bo.solveKalkVarianten(input, input.prodKontext, input.kalkKontext, cctx.getUserLanguange() user, cctx.getUserLanguange();


                // Dummy Werte
                rval.antrag.kalkulation.angAntKalkVar1Dto = input.antrag.kalkulation.angAntKalkDto;
                rval.antrag.kalkulation.angAntKalkVar2Dto = input.antrag.kalkulation.angAntKalkDto;
                rval.antrag.kalkulation.angAntKalkVar3Dto = input.antrag.kalkulation.angAntKalkDto;

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
        /// TransactionRisikoprüfung durchführen 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto checkTrRisk(Cic.OpenOne.GateBANKNOW.Service.DTO.icheckTrRiskDto input)
        {
            Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto rval = new Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto();
            CredentialContext cctx = new CredentialContext();

      

            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input processAntragToVertragDto was sent.");
                }
                if (input.antrag.sysid == 0)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                long user = cctx.getMembershipInfo().sysWFUSER;

                //zum TEST ausgeschaltet
                //checkAntragByIdErweiterung(sysid, sysvart, cctx.getUserLanguange(), false, true, sysprproduct)
                ITransactionRisikoBo bo = BOFactory.getInstance().createTransactionRisikoBO();
                Cic.OpenOne.GateBANKNOW.Common.DTO.icheckTrRiskDto dto = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.icheckTrRiskDto, Cic.OpenOne.GateBANKNOW.Common.DTO.icheckTrRiskDto>(input);

                //
                
                rval = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckTrRiskDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskDto>(bo.checkTrRisk(dto, cctx.getMembershipInfo().sysPEROLE, cctx.getUserLanguange(),false, user));

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                _log.Error(e.Message);
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (ArgumentException e)
            {
                _log.Error(e.Message);
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                _log.Error(e.Message);
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (Exception)//unhandled exception - should not happen!
            {
                  _log.Error("F_00001_GeneralError");
                rval.frontid = "CHECK_NOK";
                return rval;
            }
        }

        /// <summary>
        /// TransactionRisikoprüfung durchführen 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskByIdDto checkTrRiskById(Cic.OpenOne.GateBANKNOW.Service.DTO.icheckTrRiskByIdDto input)
        {
            Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskByIdDto rval = new Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskByIdDto();
            CredentialContext cctx = new CredentialContext();



            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input processAntragToVertragDto was sent.");
                }
                if (input.sysid == 0)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                long user = cctx.getMembershipInfo().sysWFUSER;

                //zum TEST ausgeschaltet
                //checkAntragByIdErweiterung(sysid, sysvart, cctx.getUserLanguange(), false, true, sysprproduct)
                ITransactionRisikoBo bo = BOFactory.getInstance().createTransactionRisikoBO();
                rval = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckTrRiskDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ocheckTrRiskByIdDto>(bo.checkTrRiskBySysid(input.sysid, cctx.getMembershipInfo().sysPEROLE, cctx.getUserLanguange(), cctx.getMembershipInfo().sysWFUSER));

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                rval.frontid = "CHECK_NOK";
                return rval;
            }
        }

        /// <summary>
        /// TransactionRisikoprüfung durchführen 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public orisikoSimulationDto risikoSimulation(Cic.OpenOne.GateBANKNOW.Service.DTO.irisikoSimIODto input)
        {
            orisikoSimulationDto rval = new orisikoSimulationDto();
            CredentialContext cctx = new CredentialContext();



            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input processAntragToVertragDto was sent.");
                }
                if (input.sysid == 0)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                ITransactionRisikoBo bo = BOFactory.getInstance().createTransactionRisikoBO();
                Common.DTO.orisikoSimIODto orisiko = ((TransactionRisikoBO)bo).risikoSim2(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.irisikoSimIODto, Cic.OpenOne.GateBANKNOW.Common.DTO.irisikoSimIODto>(input), cctx.getMembershipInfo().sysPEROLE);
                rval.simulationDERules = Mapper.Map<String[], String[]>(orisiko.simulationDERules);

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                rval.error = true;
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                rval.error = true;
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                rval.error = true;
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                rval.error = true;
                return rval;
            }
        }

        /// <summary>
        /// Risikoprüfung simulieren
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Cic.OpenOne.GateBANKNOW.Service.DTO.orisikoSimDto risikoSim(Cic.OpenOne.GateBANKNOW.Service.DTO.irisikoSimDto input)
        {
            Cic.OpenOne.GateBANKNOW.Service.DTO.orisikoSimDto rval = new Cic.OpenOne.GateBANKNOW.Service.DTO.orisikoSimDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input processAntragToVertragDto was sent.");
                }
                if (input.antrag.sysid == 0)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                long user = cctx.getMembershipInfo().sysWFUSER;



                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();


                //
                //bo.risikoSim(input, user, cctx.getUserLanguange());


                // Dummy 
                //Der Antrag muss durch BANK-now manuell entschie-den werden.
                rval.frontid = "CHECK_DE_NOK";

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                rval.frontid = "CHECK_NOK";
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                rval.frontid = "CHECK_NOK";
                return rval;
            }
        }


        /// <summary>
        ///FinanzierungsVarianten Drucken
        /// </summary>
        /// <param name="input"></param>
        public Cic.OpenOne.GateBANKNOW.Service.DTO.oFinVariantenDruckenDto finanzierungsvariantenDrucken(Cic.OpenOne.GateBANKNOW.Service.DTO.iFinVariantenDruckenDto input)
        {

            Cic.OpenOne.GateBANKNOW.Service.DTO.oFinVariantenDruckenDto rval = new Cic.OpenOne.GateBANKNOW.Service.DTO.oFinVariantenDruckenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input FinVariantenDrucken was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                Cic.OpenOne.GateBANKNOW.Common.DTO.iFinVariantenDruckenDto inputBo = new Cic.OpenOne.GateBANKNOW.Common.DTO.iFinVariantenDruckenDto();
                
                inputBo.ISOLanguageCode = input.ISOLanguageCode;
                inputBo.sysid = input.sysid;




                rval = Mapper.Map<GateBANKNOW.Common.DTO.oFinVariantenDruckenDto, Cic.OpenOne.GateBANKNOW.Service.DTO.oFinVariantenDruckenDto>(BOFactory.getInstance().createAngAntBo().finanzierungsvariantenDrucken(inputBo));

                rval.message.detail = cctx.getUserLanguange();




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
            catch (ServiceBaseException e)  //expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e) //unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        #endregion BNR11_CR_11

        /// <summary>
        /// Erstellt einen neuen Vertrag, der eine Restwertverlängerung des gegebenen Vertrags darstellt
        /// </summary>
        /// <param name="input">icreateExtendedContract</param>
        /// <returns>ocreateExtendedContract</returns>
        public ocreateExtendedContract createExtendedContract(icreateExtendedContract input)
        {
            //string query = "(select 1 from dual where  (select count(*) from antrag where sysvorvt=vt.sysid)=0  and vt.rw>7000 and vt.ende>=sysdate and add_months(sysdate,6)>=vt.ende and vt.sysvart=1 and vt.attribut!='saldiert') extendable";

            ocreateExtendedContract rval = new ocreateExtendedContract();
            CredentialContext cctx = new CredentialContext();
            int wsclient = AngAntDao.ERFASSUNGSCLIENT_B2B; //Default B2B
            if (input.wsclient != null)
                wsclient = (int)input.wsclient;

            try
            {
                IAngAntBo bo;
                if (wsclient == AngAntDao.ERFASSUNGSCLIENT_MA)
                    bo = BOFactory.getInstance().createAngAntBoMA();
                else
                    bo = BOFactory.getInstance().createAngAntBo();

                
                Common.DTO.AntragDto antrag = bo.createExtendedContract(cctx, input.sysVorvertrag, wsclient);
                rval.ExtendedContract = Mapper.Map<Common.DTO.AntragDto, Service.DTO.AntragDto>(antrag);

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ApplicationException e)
            {
                cctx.fillBaseDto(rval, e, e.Message);
                return rval;
            }
            catch (ServiceBaseException e)      //expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)         //unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Fetches the CRIF Control Ownership 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DTO.ogetControlPersonBusinessDto getControlPersonBusiness(igetControlPersonBusinessDto input)
        {
            DTO.ogetControlPersonBusinessDto rval = new DTO.ogetControlPersonBusinessDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();

                CRIFBo cbo = new CRIFBo();
                Common.DTO.ogetControlPersonBusinessDto rv1 = cbo.getControlPersonBusiness(input.sysantrag, input.adressid, cctx.getMembershipInfo().sysPEROLE);
               
                rval = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ogetControlPersonBusinessDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ogetControlPersonBusinessDto>(rv1);

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
        /// KREMO Budgetcalculation
        /// </summary>
        /// <param name="input"></param>
        /// <returns>budget</returns>
        public ogetKremoBudget getKremoBudget (igetKremoBudgetDto input)
        {
            ogetKremoBudget rval = new ogetKremoBudget();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null || input.budget1 == null || input.budget1.pkz == null)
                {
                    throw new ArgumentException ("No input was sent.");
                }
                cctx.validateService();

				LogUtil.addDLog("PEROLE", cctx.getMembershipInfo().sysPEROLE, cctx.getMembershipInfo().sysWFUSER);
                 
                if (input.budget2 != null)
                    rval.pkz2 = input.budget2.pkz;
                Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto budget1 = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KremoBudgetDto,Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto>(input.budget1);
                Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto budget2 = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KremoBudgetDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto>(input.budget2);
                rval = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ogetKremoBudget, ogetKremoBudget>(AuskunftBoFactory.CreateDefaultKREMOBo().getKremoBudget(cctx.getMembershipInfo().sysWFUSER,budget1,budget2,input.sysprproduct));
                

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
        /// Returns a link to an external insurance company appliaction for the given offer id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ogetVSLinkDto getVSLink(igetVSLinkDto input)
        {
            ogetVSLinkDto rval = new ogetVSLinkDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null )
                {
                    throw new ArgumentException("No input was sent.");
                }
                cctx.validateService();


                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                //IAngAntBo bo = BOFactory.getInstance().createAngAntBo();

                VSLinkBo vsl = new VSLinkBo();
                rval.deepLink = vsl.getVSLink(input.sysid, input.extvscode, cctx.getMembershipInfo().sysPEROLE, cctx.getMembershipInfo().ISOLanguageCode);
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
    }
}