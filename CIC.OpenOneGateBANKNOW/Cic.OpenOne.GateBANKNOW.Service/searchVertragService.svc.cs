using System;
using System.ServiceModel;
using AutoMapper;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "searchVertragService" in code, svc and config file together.
    /// <summary>
    /// Der Service searchVertrag liefert eine Liste von Verträgen sowie die Detaills eines bestimmten Vertrags
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class searchVertragService : IsearchVertragService
    {
        /// <summary>
        /// Findet Verträge anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchVertragDto</param>
        /// <returns>osearchVertragDto</returns>
        /// 
        public osearchVertragDto searchVertrag(isearchVertragDto input)
        {
            osearchVertragDto rval = new osearchVertragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input searchVertragDto was sent.");
                }
                if (input.searchInput == null)
                {
                    throw new ArgumentException("No searchparameter was sent.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();
                IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                
                SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.VertragDto> bo = new SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.VertragDto>();
                bo.setPermission(cctx.getMembershipInfo().sysPEROLE, true, "PEROLE");
                Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<Cic.OpenOne.GateBANKNOW.Common.DTO.VertragDto> sr = bo.search(input.searchInput);
                //BRNEUN Cr 29
                sr = BOFactory.getInstance().createVertragBo().zustandPruefung(sr, user.sysPEROLE);
                Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.VertragDto, DTO.VertragDto> searchMapper = new Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.VertragDto, DTO.VertragDto>();
                rval.result = searchMapper.mapSearchResultIT(sr, "sysit", "kunde", kundeBo);


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
        /// Liefert alle relevanten Vertagsdaten
        /// </summary>
        /// <param name="input">igetVertragDetailDto</param>
        /// <returns>ogetVertragDetailDto</returns>
        public ogetVertragDetailDto getVertragDetail(igetVertragDetailDto input)
        {
            ogetVertragDetailDto rval = new ogetVertragDetailDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input getVertragDetailDto was sent.");
                }
                if (input.sysid == 0)
                {
                    throw new ArgumentException("No sysid of Vertrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                MembershipUserValidationInfo User = cctx.getMembershipInfo();

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                RoleContextListsBo roleBo = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());
                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antrag = bo.getAntrag(input.sysid, cctx.getMembershipInfo().sysPEROLE);
                // Bezeichnungen
                Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto antragOutput = Mapper.Map<Common.DTO.AntragDto, Service.DTO.AntragDto>(bo.getAntragBezeichnungen(antrag));
                if (antragOutput != null)
                {
                    if (antragOutput.kunde != null)
                    {
                        antragOutput.sysit = antragOutput.kunde.sysit;
                        if (antragOutput.sysit == 0)
                        {
                            throw new ArgumentException("Der Vertrag mit der Sysid: " + input.sysid + " besitzt keinen gültigen Interessenten.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Der Vertrag mit der Sysid: " + input.sysid + " besitzt keinen gültigen Interessenten.");
                    }
                }

                rval.antrag = antragOutput;
                rval.vertrag = Mapper.Map<Common.DTO.VertragDto, Service.DTO.VertragDto>(bo.getVertrag(antragOutput.sysvt, cctx.getMembershipInfo().sysPEROLE));


                // Kunde (1. AS)
                Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto kundeOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto>(kundeBo.getKundeViaAntragID(antragOutput.sysit,antragOutput.sysid));
                rval.antrag.kunde = kundeOutput;

                // Mitantragsteller (2. AS)
                if (antragOutput.mitantragsteller != null)
                {
                    kundeOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto>(kundeBo.getKunde(antragOutput.mitantragsteller.sysit));
                    rval.antrag.mitantragsteller = kundeOutput;
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



        /// <summary>
        /// Exportiert eine Liste von laufenden Verträgen 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateListenExportDto createListenExport(icreateListenExportDto input)
        {
            ocreateListenExportDto rval = new ocreateListenExportDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input createListenExportDto was sent.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();

                IVertragsListenBo vlistBo = BOFactory.getInstance().createVertragsListenBo();
                Common.DTO.icreateListenExportDto commonDto = Mapper.Map<Service.DTO.icreateListenExportDto, Common.DTO.icreateListenExportDto>(input);
                commonDto.sysWFUser = user.sysWFUSER;
                commonDto.sysPeRole = user.sysPEROLE;
                Common.DTO.ocreateListenExportDto listen = vlistBo.createListenExport(commonDto);

                rval.sysEaiHot = listen.sysEaiHot;
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
        /// Listet Dateien mit Vertragslisten auf
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public olistInboxServicesDto listInboxServices(ilistInboxServicesDto input)
        {
            olistInboxServicesDto rval = new olistInboxServicesDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listInboxServicesDto was sent.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();

                IVertragsListenBo vlistBo = BOFactory.getInstance().createVertragsListenBo();
                rval.eaiHotListe = vlistBo.listInboxServices(input.code, input.sysPerson);

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
            catch (Exception e)         //unhandled exception
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Holt eine Fertige Liste als Excel-Datei 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ogetListeExportDto getListeExport(igetListeExportDto input)
        {
            ogetListeExportDto rval = new ogetListeExportDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input getListeExportDto was sent.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();

                IVertragsListenBo vlistBo = BOFactory.getInstance().createVertragsListenBo();
                rval.eaiHFile = vlistBo.getListeExport(input.sysEaiHot, user.sysPEROLE);

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
            catch (Exception e)         //unhandled exception 
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Löscht eine einzelne Vertragsliste 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public odeleteListeExportDto deleteListeExport(ideleteListeExportDto input)
        {
            odeleteListeExportDto rval = new odeleteListeExportDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input deleteListeExportDto was sent.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();

                IVertragsListenBo vlistBo = BOFactory.getInstance().createVertragsListenBo();
                rval.result = vlistBo.deleteListeExport(input.sysEaiHot,user.sysPEROLE);

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
            catch (Exception e)         //unhandled exception 
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// 2.3.4 Umsetzung der Änderung des Restwertrechnungsempfänger
        /// true when change of restwert rechnung empfänger allowed
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public bool isRREChangeAllowed(long sysid)
        {
            bool rval = false;
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (sysid == 0)
                {
                    throw new ArgumentException("No input getListeExportDto was sent.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();


                rval = BOFactory.getInstance().createVertragBo().isRREChangeAllowed(sysid, user.sysPEROLE);
               
                return rval;
            }
           catch (System.Data.Entity.Core.EntityException)
            {
               // cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException)
            {
                //cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException)      //expected service exceptions
            {
               // cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception)         //unhandled exception 
            {
               // cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }

        }


    }
}