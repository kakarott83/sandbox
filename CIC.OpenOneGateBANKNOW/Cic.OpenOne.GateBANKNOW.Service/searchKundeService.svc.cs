using AutoMapper;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    using Common.DAO.Auskunft;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "searchKundeService" in code, svc and config file together.
    /// <summary>
    /// Der Service searchKunde liefert eine Liste aller Kunden und liefert die Detaills eines Kunden.
    /// </summary>
    /// 
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class searchKundeService : IsearchKundeService
    {
        /// <summary>
        /// Findet Händlerkunden anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchKundeDto</param>
        /// <returns>osearchKundeDto</returns>
        /// 
        public osearchKundeDto searchKunde(isearchKundeDto input)
        {
            osearchKundeDto rval = new osearchKundeDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if(input == null)
                {
                    throw new ArgumentException("No input searchKundeDto is send");
                }
                if (input.searchInput == null)
                {
                    throw new ArgumentException("No Searchparameter is send");
                }
              
                cctx.validateService();
                SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto> bo = new SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>();
                bo.setPermission(cctx.getMembershipInfo().sysPEROLE, true, "PEROLE");
                Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto> sr = bo.search(input.searchInput);
                Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, DTO.KundeDto> searchMapper = new Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, DTO.KundeDto>();
                rval.result = searchMapper.mapSearchResult(sr);

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
        /// Liefert alle relevanten Kundenstamm- und Zusatzdaten
        /// </summary>
        /// <param name="input">igetKundenDetailDto</param>
        /// <returns>ogetKundenDetailDto</returns>
        public ogetKundeDetailDto getKundeDetail(igetKundeDetailDto input)
        {
            ogetKundeDetailDto rval = new ogetKundeDetailDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input getKundeDetailDto is send");
                }
                if (input.syskunde == 0 || input.syskunde == 0)
                {
                    throw new ArgumentException("No syskunde is set");
                }
                cctx.validateService();

                

                AuthenticationBo.validateUserPermission(ValidationType.IT, cctx.getMembershipInfo().sysPUSER, input.syskunde, DateTime.Now);

                IKundeBo bo = BOFactory.getInstance().createKundeBo();

                if (input.mitantragsteller > 0)
                {
                    input.syskunde = bo.getMitantragsteller(input.syskunde);
                    if(input.syskunde==0)
                        throw new ArgumentException("No Mitantragsteller found");
                    AuthenticationBo.validateUserPermission(ValidationType.IT, cctx.getMembershipInfo().sysPUSER, input.syskunde, DateTime.Now);
                }

                
                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kunde = bo.getKunde(input.syskunde);
                Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto[] adressen = bo.getAdressen(input.syskunde);
                Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto[] konten = bo.getKonten(input.syskunde);
 
                
                Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto kundeOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto>(kunde);
                Cic.OpenOne.GateBANKNOW.Service.DTO.AdresseDto[] adressenOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.AdresseDto[]>(adressen);
                Cic.OpenOne.GateBANKNOW.Service.DTO.KontoDto[] kontenOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.KontoDto[]>(konten);
                kundeOutput.adressen = adressenOutput;
                kundeOutput.kontos = kontenOutput;

                rval.kunde = kundeOutput;

                if (kundeOutput.zusatzdaten != null && kundeOutput.zusatzdaten.Length > 0)
                {
                    if (kundeOutput.zusatzdaten[0].pkz != null && kundeOutput.zusatzdaten[0].pkz.Length > 0)
                    {
                        bool qstFlag = kundeOutput.zusatzdaten[0].pkz[0].quellensteuerFlag;
                        kundeOutput.zusatzdaten[0].pkz[0].quellensteuerFlag = false;
                        if (qstFlag)
                        {
                            kundeOutput.zusatzdaten[0].pkz[0].einkbrutto = 0;
                            kundeOutput.zusatzdaten[0].pkz[0].einknetto = 0;
                            kundeOutput.zusatzdaten[0].pkz[0].jbonusbrutto = 0;
                            kundeOutput.zusatzdaten[0].pkz[0].jbonusnetto = 0;

                            kundeOutput.zusatzdaten[0].pkz[0].zulageausbildung = 0;
                            kundeOutput.zusatzdaten[0].pkz[0].zulagekind = 0;
                            kundeOutput.zusatzdaten[0].pkz[0].zulagesonst = 0;
                        }
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
        /// Findet externe Kunden anhand von bestimmten Sucheingaben
        /// </summary>
        /// <param name="input">isearchKundeExternNonGenericDto</param>
        /// <returns>osearchKundeExternNonGenericDto</returns>
        public osearchKundeExternNonGenericDto searchKundeExternNonGeneric(isearchKundeExternNonGenericDto input)
        {
            osearchKundeExternNonGenericDto rval = new osearchKundeExternNonGenericDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input searchKundeDto is send");
                }
                if (input.searchInput == null)
                {
                    throw new ArgumentException("No Searchparameter is send");
                }

                cctx.validateService();
                var bo = new SearchKundeBo(new CrifWSDao(), new CrifDBDao());

                
                var searchInput = Mapper.Map(input.searchInput, new Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternDto());

                var boResult = bo.searchKundeExternNonGeneric(searchInput);

                
                Mapper.Map(boResult, rval);

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
        ///  PKZ/UKZ aus dem letzten Antrag im Zustand 'Vertrag aktiviert' 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ogetZusatzDatenAktivDto getZusatzDatenAktiv(igetZusatzDatenAktivDto input)
        {
            ogetZusatzDatenAktivDto rval = new ogetZusatzDatenAktivDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
               
                if (input.sysit == 0 )
                {
                    throw new ArgumentException("No syskunde is set");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.IT, cctx.getMembershipInfo().sysPUSER, input.sysit, DateTime.Now);


                IZusatzdatenBo bo = BOFactory.getInstance().createZusatzdatenBo();


    
                Cic.OpenOne.GateBANKNOW.Service.DTO.ZusatzdatenDto zusatzdatenOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ZusatzdatenDto>(bo.getZusatzdatenAktiv(input.sysit, input.kdtyp));

                rval.zusatzdaten = zusatzdatenOutput;
                rval.zusatzdaten.kdtyp = input.kdtyp;

                
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
        /// searchKundeExtern
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public osearchKundeExternDto searchKundeExtern(DTO.isearchKundeExternDto input) 
        {
            osearchKundeExternDto rval = new osearchKundeExternDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input searchKundeDto is send");
                }
                if (input.input == null)
                {
                    throw new ArgumentException("No Searchparameter is send");
                }

                cctx.validateService();
                Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternGUIDto> osearchDto = new Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternGUIDto>();
                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternGUIDto[] res = new Common.DTO.KundeExternGUIDto[1];
                res[0] = new Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternGUIDto();
                osearchDto.results = res;
                osearchDto.results[0].strasse = "Neugasse";
                osearchDto.results[0].landBezeichnung = "CHE";
                osearchDto.results[0].hsnr = "39";
                osearchDto.results[0].name = "BANK-now";
                Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternGUIDto, DTO.KundeExternGUIDto> searchMapper = new Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeExternGUIDto, DTO.KundeExternGUIDto>();
                Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<KundeExternGUIDto> result = new Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<KundeExternGUIDto>();
                result = searchMapper.mapSearchResult(osearchDto);
                rval.result = result; 
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
