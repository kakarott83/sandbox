using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using AutoMapper;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.BO;
using System.Linq;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "getBuchwertService" in code, svc and config file together.
    /// <summary>
    /// Der Service getBuchwert liefert den aktuellen Buchwert eines Vertrages
    /// </summary>
    /// 
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class getBuchwertService : IgetBuchwertService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Eurotax GetForecast Auskunft einholen
        /// </summary>
        /// <param name="input">iEurotaxForecastDto</param>
        /// <returns>oEurotaxForecastDto</returns>        
        public oEurotaxGetForecastDto EurotaxGetForecast(iEurotaxGetForecastDto input)
        {
            oEurotaxGetForecastDto rval = new oEurotaxGetForecastDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("EurotaxGetForcastDto was empty.");
                }
                cctx.validateService();

                EurotaxInDto inDto = new EurotaxInDto();
                inDto.prodKontext = input.prodKontext;
                Mapper.Map<iEurotaxGetForecastDto, EurotaxInDto>(input, inDto);

                IEurotaxBo etbo = AuskunftBoFactory.CreateDefaultEurotaxBo();
                rval.EurotaxForecastList = new List<EurotaxGetForecastDto>();

                try
                {
                    List<EurotaxOutDto> outDto = etbo.GetForecast(inDto);

                    foreach (var forecastDto in outDto)
                    {
                        EurotaxGetForecastDto output = new EurotaxGetForecastDto();
                        
                        AutoMapper.Mapper.Map<EurotaxOutDto, EurotaxGetForecastDto>(forecastDto, output);
                        rval.EurotaxForecastList.Add(output);
                    }
                }
                catch (Exception ex)
                {
                    _log.Warn("No Eurotax Forecast possible for B2B with sysobtyp=" + input.sysobtyp + ": ", ex);
                }
                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (EuroTaxTimeoutException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, " F_01003_EurotaxServiceTimeoutFailure");
                return rval;
            }
            catch (EuroTaxCallException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, " F_01002_EurotaxServiceCallFailure");
                return rval;
            }
            catch (EuroTaxCommException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, " F_01001_EurotaxServiceCommFailure");
                return rval;
            }
            catch (ArgumentException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)      //expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)             //unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// 2.3.2.1	Anzeige Button für indikativen Buchwertberechnung
        /// </summary>
        /// <param name="sysid">Antrags Id</param>
        /// <returns></returns>
        public bool isBuchwertCalculationAllowed(long sysid)
        {
            /*
             * ((vt:endekz<>1)+(Choose(INSTRING('AKTIV',UPPER(VT:Zustand), 1, 1) >0 or IN-STRING('gekündigt',VT:Zustand, 1, 1) >0 , 0, 1)<>0))<>0 and vt:zustand<>'pendent' and _XQL:FETCH('VT_FI_RESTSALDO','CICSQL5:F01')<=0 and VT.SYSVART=1
             * */
            //BRN9 CR 29 p.9

            ogetBuchwertDto rval = new ogetBuchwertDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (sysid == 0)
                {
                    throw new ArgumentException("No input getBuchwertDto was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                return Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createBuchwertBo().isBuchwertCalculationAllowed(sysid);

               

            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return false;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return false;
            }
            catch (ServiceBaseException e)  //expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return false;
            }
            catch (Exception e) //unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return false;
            }
        }


        /// <summary>
        /// 2.3.2.1	Anzeige Button für indikativen Buchwertberechnung
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public oisAllowed isBuchwertCalculationAllowedNew(iisAllowed input)
        {
            /*
             * ((vt:endekz<>1)+(Choose(INSTRING('AKTIV',UPPER(VT:Zustand), 1, 1) >0 or IN-STRING('gekündigt',VT:Zustand, 1, 1) >0 , 0, 1)<>0))<>0 and vt:zustand<>'pendent' and _XQL:FETCH('VT_FI_RESTSALDO','CICSQL5:F01')<=0 and VT.SYSVART=1
             * */
            //BRN9 CR 29 p.9

            oisAllowed rval = new oisAllowed();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input.sysid == 0)
                {
                    throw new ArgumentException("No input getBuchwertDto was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                rval.isallowed = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createBuchwertBo().isBuchwertCalculationAllowed(input.sysid);
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
        /// Liefert den aktuellen Buchwert des Vertrags
        /// </summary>
        /// <param name="input">igetBuchwertDto</param>
        /// <returns>ogetBuchwertDto</returns>
        public ogetBuchwertDto getBuchwert(igetBuchwertDto input)
        {
            ogetBuchwertDto rval = new ogetBuchwertDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input getBuchwertDto was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                

                GateBANKNOW.Common.DTO.igetBuchwertDto ip = Mapper.Map<igetBuchwertDto, GateBANKNOW.Common.DTO.igetBuchwertDto>(input);
                ip.sysPerole = cctx.getMembershipInfo().sysPEROLE;
                rval = Mapper.Map<GateBANKNOW.Common.DTO.ogetBuchwertDto, ogetBuchwertDto>(Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createBuchwertBo().getBuchwert(ip), rval);

                rval.success();
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
        /// Löst eine Kaufofferte aus
        /// </summary>
        /// <param name="input"></param>
        public operformKaufofferte performKaufofferte(iperformKaufofferteDto input)
        {

            operformKaufofferte rval = new operformKaufofferte();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input performKaufofferte was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);
                Cic.OpenOne.GateBANKNOW.Common.DTO.iperformKaufofferteDto inputBo = new Cic.OpenOne.GateBANKNOW.Common.DTO.iperformKaufofferteDto();
                inputBo.herkunft= "B2B";
                inputBo.sysWFUser = cctx.getMembershipInfo().sysPEROLE;
                inputBo.isHaendler = input.isHaendler;
                inputBo.sysid = input.sysid;
                inputBo.sysPerole = cctx.getMembershipInfo().sysPEROLE;
                
                rval = Mapper.Map<GateBANKNOW.Common.DTO.operformKaufofferte, operformKaufofferte>(BOFactory.getInstance().createVertragBo().performKaufofferte(inputBo));
                
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


        public oisAllowed isPerformKaufofferteAllowed(iisAllowed input)
        {
            oisAllowed rval = new oisAllowed();
            CredentialContext cctx = new CredentialContext();
             try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input isPerformKaufofferteAllowed was sent.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();
                IVertragBo bo = BOFactory.getInstance().createVertragBo();
                rval.isallowed = bo.isPerformKaufofferteAllowed(input.sysid);

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
        /// Liefert eine Liste für den angegebenen Listencode
        /// e.g.
        /// input.code = "BW_BERECHNUNG_PER"
        /// 
        /// </summary>
        /// <returns>ogetListItemsDto</returns>
        public ogetListItemsDto getListItems(igetListItems input)
        {
            ogetListItemsDto rval = new ogetListItemsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null||input.code==null)
                {
                    throw new ArgumentException("No input for list-code given.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();

                rval.items= new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).getDdlkpsposDetailsById(input.code,input.domainid);
                //BNRFZ - 1402 - Umstellung Defaultwert Kaufofferte an / Dropdown Buchwert per
                rval.items = (from f in rval.items
                              where f.bezeichnung != null
                              select f).ToArray();

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

    }
}