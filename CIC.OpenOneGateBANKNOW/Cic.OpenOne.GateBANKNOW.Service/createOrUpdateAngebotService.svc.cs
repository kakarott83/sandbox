using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using AutoMapper;
using AutoMapper.Mappers;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using AutoMapper.Configuration;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "createOrUpdateAngebotService" in code, svc and config file together.
    /// <summary>
    /// Der Service createOrUpdateAngebot liefert Listen zu verschiedenen Arten und Typen sowie Produkten. Außerdem lassen sich Werte einstelleun und Speichern. 
    /// Desweiteren kann man das Angebot zu einem Antrag einreichen.
    /// </summary>
    /// <remarks>Die Methode callKonfigurator ist im b2b_steuerung_1_1.pdf nicht aufgelistet.</remarks>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class createOrUpdateAngebotService : IcreateOrUpdateAngebotService
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
                DdOlExtended ctx = new DdOlExtended();
                long sysperole = PeRoleUtil.FindRootPEROLEByRoleType(ctx , cctx.getMembershipInfo().sysPEROLE, (int)RoleTypeTyp.HAENDLER);
                rval.objekttypen = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableObjekttypen(cctx.getUserLanguange(),sysperole);
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
        /// Liefert eine Liste der verfügbaren Objektarten im Kontext
        /// </summary>
        /// <returns>olistAvailableObjektartenDto</returns>
        public olistAvailableObjektartenDto listAvailableObjektarten()
        {
            olistAvailableObjektartenDto rval = new olistAvailableObjektartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
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
                    throw new ArgumentException("No input (ilistAvailableProdukteDto) was sent.");
                }
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                cctx.validateService();
                _log.Debug("Duration Validate: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                DdOlExtended ctx = new DdOlExtended();
                if (input.kontext.sysperole == 0)
                    input.kontext.sysperole = PeRoleUtil.FindRootPEROLEByRoleType(ctx , cctx.getMembershipInfo().sysPEROLE, (int)RoleTypeTyp.HAENDLER);
                else
                    input.kontext.sysperole = PeRoleUtil.FindRootPEROLEByRoleType(ctx , input.kontext.sysperole, (int)RoleTypeTyp.HAENDLER);
                if (input.kontext.sysbrand == 0) input.kontext.sysbrand = cctx.getMembershipInfo().sysBRAND;
                _log.Debug("Duration A: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                Cic.OpenOne.GateBANKNOW.Common.DAO.IPruefungDao pruefundDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getPruefungDao();
                ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                PrismaProductBo bo = new PrismaProductBo(pDao, obDao, transDao, PrismaProductBo.CONDITIONS_BANKNOW, cctx.getUserLanguange());
                Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo pruefungbo = new Cic.OpenOne.GateBANKNOW.Common.BO.PruefungBo(pDao, obDao, transDao, pruefundDao);
                //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
                List<CIC.Database.PRISMA.EF6.Model.PRPRODUCT> products = bo.listAvailableProducts(input.kontext);
                _log.Debug("Duration B: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                IEnumerable<long> alleids = (from a in products
                                             select a.SYSPRPRODUCT).ToList();
                _log.Debug("Products vor Sortierung:" + String.Join(",", alleids.ToArray()));


                AvailableProduktDto[] sortresult = bo.listSortedAvailableProducts(products, input.kontext.sysprbildwelt).ToArray();

                IEnumerable<long> alleidssortresult = (from a in sortresult 
                                             select a.sysID).ToList();
                _log.Debug("Products nach Sortierung:" + String.Join(",", alleidssortresult.ToArray()));


                _log.Debug("Duration C: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                JokerPruefungDto resultJokerPruefung = pruefungbo.analyzeJokerProducts(sortresult, input.kontext);
                _log.Debug("Duration D: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
              
                rval.produkte = resultJokerPruefung.products.ToArray();

                IEnumerable<long> jokerproducte = (from a in rval.produkte
                                                       select a.sysID).ToList();
                _log.Debug("Jokerproducte:" + String.Join(",", jokerproducte.ToArray()));


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

            
            string[] PRIVAT = new string[] {"2","3"};
            try
            {
                DropListDto[] nutzungsarten = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableNutzungsarten(cctx.getUserLanguange());
                int i = 0;
                rval.nutzungsarten = new  DropListDto[2];
                foreach (DropListDto art in nutzungsarten)
                    if (PRIVAT.Contains(art.sysID.ToString()))
                    {
                        DropListDto part= new DropListDto();
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
                    throw new ArgumentException("No input (ilistAvailableServiceDto) was sent.");
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
                    throw new ArgumentException("No input (igetParameterDto) was sent.");
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
        /// Erzeugt/Ändert Persistenzobjekt der Angebotsvariante
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
                    throw new ArgumentException("No input (icreateKalkulationDto) was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());

                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto inputVar = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto>(input.angVar);
                Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto KalkulationOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto>(bo.createOrUpdateKalkulation(inputVar));
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
            catch (AutoMapperConfigurationException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_MapperConfigurationError");
                return rval;
            }
            catch (AutoMapperMappingException e)
            {
                cctx.fillBaseDto(rval, e.InnerException, "F_00002_MappingError");
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
        /// Kopiert Persistenzobjekt der Angebotsvariante
        /// </summary>
        /// <param name="input">icopyKalkulationDto</param>
        /// <returns>ocopyKalkulationDto</returns>
        public ocopyKalkulationDto copyKalkulation(icopyKalkulationDto input)
        {
            ocopyKalkulationDto rval = new ocopyKalkulationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input (icopyKalkulationDto) was sent.");
                }
                if (input.angVar == null)
                {
                    throw new ArgumentException("No AngVar to copy from was sent.");
                }
                cctx.validateService();
                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());

                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto inputVar = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto>(input.angVar);
                Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto KalkulationOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto>(bo.copyKalkulation(inputVar));
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
        /// Löscht Persistenzobjekt der Angebotsvariante
        /// </summary>
        /// <param name="input">ideleteKalkulationDto</param>
        /// <returns>odeleteKalkulationDto</returns>
        public odeleteKalkulationDto deleteKalkulation(ideleteKalkulationDto input)
        {
            odeleteKalkulationDto rval = new odeleteKalkulationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input (ideleteKalkulationDto) was sent.");
                }
                if (input.sysID == 0 || input.sysID == 0)
                {
                    throw new ArgumentException("No sysid was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());
                bo.deleteKalkulation(input.sysID);
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
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                IKalkulationBo kalkBo = null;

                if (input.angebot.angAntVars != null)
                {
                    foreach (DTO.AngAntVarDto variante in input.angebot.angAntVars)
                    {
                        Cic.OpenOne.Common.DTO.Prisma.prKontextDto pKontext = MyCreateProductKontext(cctx, input.angebot, variante);

                        _log.Debug("Calculating Variant on Angebot Update with context: " + _log.dumpObject(pKontext));
                        kalkBo = BOFactory.getInstance().createKalkulationBo(cctx.getUserLanguange());
                        Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulationInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>(variante.kalkulation);
                        Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalk = kalkBo.calculate(kalkulationInput, pKontext, MyCreateKalkKontext(input.angebot), cctx.getUserLanguange(), ref rateError);
                        variante.kalkulation = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>(kalk);
                    }
                }

                Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebotInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>(input.angebot);
                if (input.angebot.kunde != null)
                {
                    //only save when a fresh kunde was sent 
                    //or when the existing customer has zusatzdaten
                    //reason: avoid updating a customer with empty data when only a few of the existing kunde fields come in
                    if (input.angebot.kunde.sysit == 0
                        || (input.angebot.kunde.zusatzdaten!=null&& input.angebot.kunde.zusatzdaten.Length>0&& input.angebot.kunde.zusatzdaten[0].pkz!=null && input.angebot.kunde.zusatzdaten[0].pkz.Length>0)
                        || (input.angebot.kunde.zusatzdaten != null && input.angebot.kunde.zusatzdaten.Length > 0 && input.angebot.kunde.zusatzdaten[0].ukz != null && input.angebot.kunde.zusatzdaten[0].ukz.Length > 0)
                        )
                    {
                        
                        IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                        angebotInput.kunde = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.angebot.kunde), cctx.getMembershipInfo().sysPEROLE);
                    }
                }
                if (input.angebot.mitantragsteller != null)
                {
                    if (input.angebot.mitantragsteller.sysit == 0
                        || (input.angebot.mitantragsteller.zusatzdaten != null && input.angebot.mitantragsteller.zusatzdaten.Length > 0 && input.angebot.mitantragsteller.zusatzdaten[0].pkz != null && input.angebot.mitantragsteller.zusatzdaten[0].pkz.Length > 0)
                        || (input.angebot.mitantragsteller.zusatzdaten != null && input.angebot.mitantragsteller.zusatzdaten.Length > 0 && input.angebot.mitantragsteller.zusatzdaten[0].ukz != null && input.angebot.mitantragsteller.zusatzdaten[0].ukz.Length > 0)
                        )
                    {

                        IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                        angebotInput.mitantragsteller = kundeBo.createOrUpdateKunde(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.angebot.mitantragsteller), cctx.getMembershipInfo().sysPEROLE);
                    }
                }
                Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto angebotOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto>(bo.createOrUpdateAngebot(angebotInput, cctx.getMembershipInfo().sysPEROLE));
                Cic.OpenOne.GateBANKNOW.Service.BO.StatusEPOSBo.setStatusEPOS(angebotOutput);
                rval.angebot = angebotOutput;

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
        /// Speichert Persistenzobjekte des Angebots und aller Angebotsvarianten
        /// </summary>
        /// <param name="input">isaveAngebotDto</param>
        /// <returns>osaveAngebotDto</returns>
        public osaveAngebotDto saveAngebot(isaveAngebotDto input)
        {
            osaveAngebotDto rval = new osaveAngebotDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input (saveAngebotDto) was sent.");
                }
                if (input.angebot == null)
                {
                    throw new ArgumentException("No Angebot was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();

                Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebotInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>(input.angebot);
                Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto angebotOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto>(bo.createOrUpdateAngebot(angebotInput, cctx.getMembershipInfo().sysPEROLE));

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
                if (input.angAntVs != null && input.angAntVs.Count()>0)
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

        private static kalkKontext MyCreateKalkKontext(DTO.AngebotDto antrag)
        {
            //B2B
            //this.rateBruttoInklAbsicherung = 0D;
            //this.ersteRateBruttoInklAbsicherung = 0D;
            kalkKontext kkontext = new kalkKontext();
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
        /// MyCreateProductKontext
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
        /// Übernimmt Angebot in Antrag
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>oprocessAngebotToAntragDto</returns>
        public oprocessAngebotToAntragByIdDto processAngebotToAntragById(long sysid)
        {
            Service.DTO.oprocessAngebotToAntragByIdDto rval = new Service.DTO.oprocessAngebotToAntragByIdDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (sysid == 0 || sysid == 0)
                {
                    throw new ArgumentException("No Sysid was sent. Sysid must be > 0.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebotInput = bo.getAngebot(sysid);

                
                //Create AntragDto from AngebotDto - Antrag not yet saved!
                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antrag = bo.processAngebotToAntrag(angebotInput);

                antrag = bo.createOrUpdateAntrag(antrag, cctx.getMembershipInfo().sysPEROLE);

                // Ticket#2012083110000047 — Übernahme Memos in den Antrag (nur MAClient)
                bo.copyNotizenAngebotToAntrag(angebotInput.sysid, antrag.sysid, antrag.erfassungsclient);

                //BNRZEHN-1574 übernahme Dokumente in den Antrag
                bo.copyDms(angebotInput.sysid, antrag.sysid);

                rval.sysid = antrag.sysid;

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
        /// Übernimmt Angebot in Antrag
        /// Die Primary Key's sind in der neuen Struktur ausgenullt, da ja noch nicht in der DB
        /// </summary>
        /// <param name="input">iprocessAngebotToAntragDto</param>
        /// <returns>oprocessAngebotToAntragDto</returns>
        public Service.DTO.oprocessAngebotToAntragDto processAngebotToAntrag(Service.DTO.iprocessAngebotToAntragDto input)
        {
            Service.DTO.oprocessAngebotToAntragDto rval = new Service.DTO.oprocessAngebotToAntragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input (iprocessAngebotToAntragDto) was sent.");
                }
                if (input.angebot == null)
                {
                    throw new ArgumentException("No Angebot was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, input.angebot.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();

                Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebotInput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>(input.angebot);

                rval.antrag = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto>(bo.processAngebotToAntrag(angebotInput));

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
                rval.objekt = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObDto>(angAntBo.getObjektdatenByVIN(key,cctx.getMembershipInfo().sysWFUSER, cctx.getUserLanguange()));

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
        /// Löst eine Kaufofferte aus
        /// </summary>
        /// <param name="input"></param>
        public Cic.OpenOne.GateBANKNOW.Service.DTO.oPreisschildDruckDto preisschildDruck(Cic.OpenOne.GateBANKNOW.Service.DTO.iPreisschildDruckDto input)
        {

            Cic.OpenOne.GateBANKNOW.Service.DTO.oPreisschildDruckDto rval = new Cic.OpenOne.GateBANKNOW.Service.DTO.oPreisschildDruckDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input preisschildDruck was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);
                Cic.OpenOne.GateBANKNOW.Common.DTO.iPreisschildDruckDto inputBo = new Cic.OpenOne.GateBANKNOW.Common.DTO.iPreisschildDruckDto();
                inputBo.herkunft = "B2B";
                inputBo.ISOLanguageCode= input.ISOLanguageCode;
                inputBo.sysid = input.sysid;
                inputBo.sysAngVar = input.sysAngVar;
                inputBo.preisInklusive = input.preisInklusive;



                rval = Mapper.Map<GateBANKNOW.Common.DTO.oPreisschildDruckDto, Cic.OpenOne.GateBANKNOW.Service.DTO.oPreisschildDruckDto>(BOFactory.getInstance().createAngAntBo().preisschildDruck(inputBo));

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

        /// <summary>
        /// bonus calculation for salesman
        /// </summary>
        /// <param name="input">igetVTProvDto</param>
        /// <returns>ogetVTProvDto</returns>
        public ogetVTProvDto getVTProv(igetVTProvDto input)
        {
            CredentialContext cctx = new CredentialContext();
            ogetVTProvDto rval = new ogetVTProvDto();
            try
            {
                
                
                Common.BO.IIncentivierungBo incentive = BOFactory.getInstance().createIncentivierungBo();
                incentive.createProvisions(input.Kontext);
                //Common.DTO.VertragDto vertrag = BOFactory.getInstance().createVertragBo().getVertrag(input.Kontext.sysvt);
                //incentive.ContractConcluded(input.Kontext.sysperole, vertrag);

                rval.success();
               


            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
               
            }
            catch (ServiceBaseException e)  //expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
               
            }
            catch (Exception e) //unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                
            }

            rval.message.stacktrace = null;
            return rval;
        }

        /// <summary>
        /// get the current user's bonus status in the incentivation program
        /// </summary>
        /// <param name="input">igetMyPocketDataDto</param>
        /// <returns>incentivation status</returns>
        public ogetMyPocketDataDto getMyPocketData(igetMyPocketDataDto input)
        {
            ogetMyPocketDataDto rval = new ogetMyPocketDataDto();

            CredentialContext cctx = new CredentialContext();
            try
            {
                
                IIncentivierungBo incentive = BOFactory.getInstance().createIncentivierungBo();
                rval.MyPocketData = Mapper.Map<Common.DTO.MyPocketDto, Service.DTO.MyPocketDto>(incentive.GetPocket(cctx.getMembershipInfo().sysPEROLE));
                rval.success();
               

            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
               
            }
            catch (ArgumentException e)
            {
                rval.message.type = OpenOne.Common.DTO.MessageType.Info;
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                
            }
            catch (ServiceBaseException e)  //expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
               
            }
            catch (Exception e) //unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
               
            }

            rval.message.stacktrace = null;
            return rval;

        }


        /// <summary>
        /// Attaches the given disclaimer to the given area/id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateDisclaimerDto createDisclaimer(icreateDisclaimerDto input)
        {
            ocreateDisclaimerDto rval = new ocreateDisclaimerDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);


                BOFactory.getInstance().createDisclaimerBo().createDisclaimer(input.area.ToString(), input.disclaimerType, input.sysid, cctx.getMembershipInfo().sysWFUSER, input.inhalt);

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
    }
}