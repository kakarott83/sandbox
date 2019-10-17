using System;
using System.Reflection;
using System.ServiceModel;
using AutoMapper;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "searchAntragService" in code, svc and config file together.
    /// <summary>
    /// Der Service searchAntrag liefert eine Liste aller Anträge nach bestimmten Filterbedingungen und liefert die Detaills zu einem selectierten Antrag
    /// </summary>
    /// 
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class searchAntragService : IsearchAntragService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static OpenOne.Common.DTO.Prisma.prKontextDto createProductKontext(CredentialContext cctx, DTO.AntragDto antrag)
        {
            OpenOne.Common.DTO.Prisma.prKontextDto pKontext = new OpenOne.Common.DTO.Prisma.prKontextDto();

            pKontext.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            pKontext.sysperole = cctx.getMembershipInfo().sysPEROLE;
            pKontext.sysprproduct = antrag.kalkulation.angAntKalkDto.sysprproduct;
            pKontext.sysbrand = antrag.sysbrand;
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

            return pKontext;
        }

        /// <summary>
        /// Findet Anträge anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchAntragDto</param>
        /// <returns>osearchAntragDto</returns>
        /// 
        public osearchAntragDto searchAntrag(isearchAntragDto input)
        {
            osearchAntragDto rval = new osearchAntragDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input searchAntragDto was sent.");
                }
                if (input.searchInput == null)
                {
                    throw new ArgumentException("No Searchparameter was sent.");
                }
                cctx.validateService();
                IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                IAngAntDao aad = CommonDaoFactory.getInstance().getAngAntDao();
                SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto> bo = new SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>();
                bo.setPermission(cctx.getMembershipInfo().sysPEROLE, true, "PEROLE");
                Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto> sr = bo.search(input.searchInput);
                aad.fetchStates(sr.results);
                Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, DTO.AntragDto> searchMapper = new Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, DTO.AntragDto>();
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
        /// Liefert alle relevanten Antragsdaten
        /// </summary>
        /// <param name="input">igetAntragDetailDto</param>
        /// <returns>ogetAntragDetailDto</returns>
        public ogetAntragDetailDto getAntragDetail(igetAntragDetailDto input)
        {
            ogetAntragDetailDto rval = new ogetAntragDetailDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input getAntragDetailDto was sent.");
                }
                if (input.sysantrag == 0 || input.sysantrag == 0)
                {
                    throw new ArgumentException("No sysantrag was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysantrag, DateTime.Now);
                MembershipUserValidationInfo User = cctx.getMembershipInfo();

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                IKundeBo kundeBo = BOFactory.getInstance().createKundeBo();
                RoleContextListsBo roleBo = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());
                Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antrag = bo.getAntrag(input.sysantrag, cctx.getMembershipInfo().sysPEROLE);

               

                roleBo.setAlertsAsReaded(input.sysantrag, User.sysPUSER);

                try
                {
                    //Kunde by antrag
                    antrag.kunde = kundeBo.getKundeViaAntragID((long)antrag.sysit, antrag.sysid);
                }
                catch (Cic.OpenOne.GateBANKNOW.Common.DAO.KundeDao.PkzUkzException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    //currently the antrag may have no customer, so ignore it
                    _log.Debug("Explicitly unhandled Exception: " + ex.Message);
                }
                try
                {
                    if(antrag.mitantragsteller!=null)
                        antrag.mitantragsteller = kundeBo.getKundeViaAntragID((long)antrag.mitantragsteller.sysit, antrag.sysid);
                }
                catch (Exception ex)
                {
                    //currently the antrag may have no mitantragsteller, so ignore it
                    _log.Debug("Explicitly unhandled Exception: " + ex.Message);
                }

                // Bezeichnungen
                Common.DTO.AntragDto antragDto = bo.getAntragBezeichnungen(antrag);
                rval.antrag = Mapper.Map<Common.DTO.AntragDto, Service.DTO.AntragDto>(antragDto);
                DropListDto[] objektarten = roleBo.listAvailableObjektarten(cctx.getUserLanguange());
                if (objektarten != null && rval.antrag.angAntObDto != null)
                {
                    for (int i = 0; i < objektarten.Length; i++)
                    {
                        if (rval.antrag.angAntObDto.sysobart == objektarten[i].sysID)
                        {
                            rval.antrag.angAntObDto.obArtBezeichnung = objektarten[i].bezeichnung;
                            break;
                        }
                    }
                }
                DropListDto[] nutzungsarten = roleBo.listAvailableNutzungsarten (cctx.getUserLanguange());
                if (nutzungsarten != null && rval.antrag.kalkulation != null && rval.antrag.kalkulation.angAntKalkDto != null)
                {
                    for (int i = 0; i < nutzungsarten.Length; i++)
                    {
                        if (rval.antrag.kalkulation.angAntKalkDto.sysobusetype == nutzungsarten[i].sysID)
                        {
                            rval.antrag.kalkulation.angAntKalkDto.obUseTypeBezeichnung = nutzungsarten[i].bezeichnung;
                            break;
                        }
                    }
                }


                // Auflagen
                rval.antrag.auflagenText = bo.getAuflagen(antrag.sysid, cctx.getUserLanguange());
                rval.antrag.zustaende = bo.getZustaende(antrag.sysid, cctx.getUserLanguange());

                
                bool mitRisikoFilter = true;
                if (input.wsclient != null)
                {
                    mitRisikoFilter = !(input.wsclient == AngAntDao.ERFASSUNGSCLIENT_MA);
                }

                if (rval.antrag.zustand == "Finanzierungsvorschlag" && rval.antrag.ProFinLock > 0 && rval.antrag.sysprchannel==1)
                {

                    ITransactionRisikoBo trbo = BOFactory.getInstance().createTransactionRisikoBO();
                    
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto var1 = trbo.getVariante(antragDto, (int)Cic.OpenOne.GateBANKNOW.Common.DTO.AntragVarianten.GleichbleibendeRate, cctx.getMembershipInfo().sysPEROLE, cctx.getMembershipInfo().ISOLanguageCode, mitRisikoFilter);
                    if (var1 != null)
                        rval.antrag.kalkulation.angAntKalkVar1Dto = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, AngAntKalkDto>(var1);
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto var2 = trbo.getVariante(antragDto, (int)Cic.OpenOne.GateBANKNOW.Common.DTO.AntragVarianten.Mindestanzahlung, cctx.getMembershipInfo().sysPEROLE, cctx.getMembershipInfo().ISOLanguageCode, mitRisikoFilter);
                    if (var2 != null)
                        rval.antrag.kalkulation.angAntKalkVar2Dto = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, AngAntKalkDto>(var2);
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto var3 = trbo.getVariante(antragDto, (int)Cic.OpenOne.GateBANKNOW.Common.DTO.AntragVarianten.FreieKalkulation, cctx.getMembershipInfo().sysPEROLE, cctx.getMembershipInfo().ISOLanguageCode, mitRisikoFilter);
                    if (var3 != null)
                        rval.antrag.kalkulation.angAntKalkVar3Dto = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, AngAntKalkDto>(var3);
                    else
                        rval.antrag.kalkulation.angAntKalkVar3Dto = rval.antrag.kalkulation.angAntKalkDto;
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
                rval.antrag = null;
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }
    }
}