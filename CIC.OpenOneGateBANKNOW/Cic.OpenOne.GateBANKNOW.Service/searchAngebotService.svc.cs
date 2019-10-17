using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using AutoMapper;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Service.BO;


namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "searchAngebotService" in code, svc and config file together.
    /// <summary>
    /// Der Service searchAngebot liefert eine Liste aller Angebote nach bestimmten Filterkriterien. Desweiteren liefert er detaills zu einem Angebot.
    /// </summary>
    /// 
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class searchAngebotService : IsearchAngebotService
    {
        /// <summary>
        /// Findet Angebote anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchAngebotDto</param>
        /// <returns>osearchAngebotDto</returns>
        /// 
        public osearchAngebotDto searchAngebot(isearchAngebotDto input)
        {
            osearchAngebotDto rval = new osearchAngebotDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input searchAngebotDto is send");
                }
                if (input.searchInput == null)
                {
                    throw new ArgumentException("No SearchParameter is send");
                }
                cctx.validateService();
                IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                FilterSearchTransformer fst = new FilterSearchTransformer();
                input.searchInput = fst.filterZustandTransformer(input.searchInput);
                SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto> bo = new SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>();
                bo.setPermission(cctx.getMembershipInfo().sysPEROLE, true, "PEROLE");
                Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto> sr = bo.search(input.searchInput);
                Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, DTO.AngebotDto> searchMapper = new Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, DTO.AngebotDto>();
                searchMapper.setStatusEPOS(sr);
                rval.result = searchMapper.mapSearchResultIT(sr,"sysit","kunde",kundeBo);
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
        /// Liefert alle relevanten Angebotsdaten
        /// </summary>
        /// <param name="input">igetAngebotDetailDto</param>
        /// <returns>ogetAngebotDetailDto</returns>
        public ogetAngebotDetailDto getAngebotDetail(igetAngebotDetailDto input)
        {
            ogetAngebotDetailDto rval = new ogetAngebotDetailDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input getAngebotDetailDto is send");
                }
                if (input.sysangebot == 0 || input.sysangebot == 0)
                {
                    throw new ArgumentException("No sysangebot is set");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, input.sysangebot, DateTime.Now);

                List<KalkulationDto> kalkulationen = new List<KalkulationDto>();
               
                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                //KundeBo kundeBo = new KundeBo(new KundeDao());
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebot = bo.getAngebot(input.sysangebot);

              
                Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto angebotOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto>(angebot);
                //Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto kundeOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto>(kundeBo.getKunde(angebotOutput.sysit));
                if (angebotOutput.kunde != null)
                {
                    angebotOutput.sysit = angebotOutput.kunde.sysit;
                }
                Cic.OpenOne.GateBANKNOW.Service.BO.StatusEPOSBo.setStatusEPOS(angebotOutput);
                rval.angebot = angebotOutput;
                //rval.angebot.kunde = angebotOutput.kunde;
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
    }
}
