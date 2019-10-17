using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using System.Linq;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "createSchnellkalkulationService" in code, svc and config file together.
    /// <summary>
    /// Der Service Schnellkalkulation kann Schnellkalkulationen anlegen, kalkLieren, speichern und weitere Informationen liefern.
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class createSchnellkalkulationService : IcreateOrUpdateSchnellkalkulationService
    {
        //Methode saveSchnellkalkulation fehlt im Dokument b2b_steuerung_1_1.pdf -> saveKalLation!

        /// <summary>
        /// Liefert eine Liste der verfügbaren Produkte im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableProdukteDto</param>
        /// <returns>olistAvailableProdukteDto</returns>
        public olistAvailableProdukteDto listAvailableProdukte(ilistAvailableProdukteDto input)
        {
            olistAvailableProdukteDto rval = new olistAvailableProdukteDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listAvailableProdukteDto is send");
                }
                cctx.validateService();
                IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                PrismaProductBo bo = new PrismaProductBo(pDao, obDao, transDao, PrismaProductBo.CONDITIONS_BANKNOW, cctx.getUserLanguange());
                //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
                //Einschränken nach VART Schnellkalkulation
                input.kontext.prprodtype = Cic.OpenOne.Common.DTO.Prisma.Prprodtype.SCHNELLCALC;
                List<CIC.Database.PRISMA.EF6.Model.PRPRODUCT > products = bo.listAvailableProducts(input.kontext);

                rval.produkte = bo.listSortedAvailableProducts(products, input.kontext.sysprbildwelt).ToArray();

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
        /// Liefert eine Liste der verfügbaren Services im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableServicesDto</param>
        /// <returns>olistAvailableServciesDto</returns>
        public olistAvailableServicesDto listAvailableServices(ilistAvailableServicesDto input)
        {
            olistAvailableServicesDto rval = new olistAvailableServicesDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listAvailableServicesDto is send");
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
        /// Liefert eine Liste aller Parameter und Eckwerte des Produkts
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
                    throw new ArgumentException("No input getParameterDto is send");
                }
                cctx.validateService();
                if (input.kontext.sysperole == 0) input.kontext.sysperole = cctx.getMembershipInfo().sysPEROLE;
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
        /// Löst die aktuelle Schnellkalkulation auf und liefert die Berechnungsergebnisse
        /// </summary>
        /// <param name="input">isolveKalkulationDto</param>
        /// <returns>osoleKalkulationDto</returns>
        public osolveSchnellkalkulationDto solveKalkulation(isolveSchnellkalkulationDto input)
        {
            osolveSchnellkalkulationDto rval = new osolveSchnellkalkulationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                byte rateError = 0;
                if (input == null)
                {
                    throw new ArgumentException("No input solveSchnellkalkulationDto is send");
                }
                cctx.validateService();
                rval.kalkulationen = new List<osolveKalkulationDto>();
                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());

                foreach (isolveKalkulationDto iSolve in input.kalkulationen)
                {
                    Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(iSolve.kalkulation);

                    Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = bo.calculate(kalkulationInput, iSolve.prodKontext, iSolve.kalkKontext, cctx.getUserLanguange(), ref rateError);
                    Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto kalkulationOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>(kalk);
                    KalkulationServicesBo kservice = new KalkulationServicesBo();

                    osolveKalkulationDto result = new osolveKalkulationDto();
                    result.zusatzinformationen = kservice.aggregateZusatzinformation(kalk);

                    result.kalkulation = kalkulationOutput;
                    rval.kalkulationen.Add(result);
                }
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
        /// Erzeugt/Ändert Persistenzobjekt der Schnellkalkulation
        /// </summary>
        /// <param name="input">icreateKalkulationDto</param>
        /// <returns>ocreateKalkulationDto</returns>
        public ocreateKalkulationDto createOrUpdateKalkulation(icreateKalkulationDto input)
        {
            ocreateKalkulationDto rval = new ocreateKalkulationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input createKalkulationDto is send");
                }
                cctx.validateService();

                ISchnellKalkulationBo bo = new SchnellKalkulationBo(new SchnellkalkulationDao());
                rval.kalkulation = new Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto();
                Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto KalkulationOutput = new DTO.AngAntVarDto();

                KalkulationOutput.kalkulation = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>(bo.createOrUpdateSchnellkalkulation(input.angVar.sysangebot));
                rval.kalkulation = KalkulationOutput;
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
        /// Speichert eine Kalkulation
        /// </summary>
        /// <param name="input">isaveKalkulationDto</param>
        /// <returns>osaveKalkulationDto</returns>
        public osaveKalkulationDto saveKalkulation(isaveKalkulationDto input)
        {
            osaveKalkulationDto rval = new osaveKalkulationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input saveKalkulationDto is send");
                }
                if (input.kalkualtion == null)
                {
                    throw new ArgumentException("No Kalkulation is send");
                }
                cctx.validateService();
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);
                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());

                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto>(input.kalkualtion);
                Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto kalkulationOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto>(bo.updateKalkulation(kalkulationInput));
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

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                Common.DTO.KalkulationDto calc = new Common.DTO.KalkulationDto();
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>(input.kalkulation);
                calc.angAntKalkDto = kalkulationInput;

                List<Cic.OpenOne.Common.DTO.AngAntVsDto> versicherungen = new List<OpenOne.Common.DTO.AngAntVsDto>();
                if (input.angAntVs != null && input.angAntVs.Count() > 0)
                {
                    foreach (Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto vs in input.angAntVs)
                    {
                        versicherungen.Add(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>(vs));
                    }
                }
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
    }
}