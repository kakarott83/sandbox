using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;


namespace Cic.OpenOne.GateBANKNOW.Service
{
    /// <summary>
    /// Der Service AuskunftService ruft die im Auskunfttypen angegebene Schnittstellenmethode auf
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class AuskunftService : IAuskunftService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        
        /// <summary>
        /// Web interface to call Mail sending
        /// </summary>
        /// <param name="To">Recipient</param>
        /// <param name="From">Sender</param>
        /// <param name="Subject">Subject of the mail</param>
        /// <param name="Data">Data body data (intended to be a PDF datastream)</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        public oMessagingDto sendMail(string To, string From, string Subject, byte[] Data)
        {
            oMessagingDto rval = new oMessagingDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (To == null || To == "")
                {
                    throw new ArgumentException("No receiver Information delivered.");
                }
                if (From == null || From == "")
                {
                    throw new ArgumentException("No sender Information delivered.");
                }
                if (Subject == null)
                {
                    throw new ArgumentException("No subject Information delivered.");
                }

                cctx.validateService();

                INotificationGatewayBo Gateway = BOFactory.getInstance().createNotificationGateway();

                int Code = Gateway.sendMail(To, From, Subject, Data, ConfigurationBO.getServerDaten());

                if (Code == 2)
                {
                    rval.message.code = "True";
                    rval.message.detail = "E-Mail sent!";
                    rval.Output = true;
                    rval.success();
                }
                else
                {
                    switch (Code)
                    {
                        case 3:
                            throw (new Exception("Preparing E-Mail failed!"));
                        case 4:
                            throw (new Exception("Transmission to Server failed!"));
                        default:
                            throw (new Exception("Error occurred sending E-Mail!"));
                    }
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
        /// Web interface to call the Fax sending
        /// </summary>
        /// <param name="To">Recipient</param>
        /// <param name="From">Sender</param>
        /// <param name="Data">Fax Body (PDF datastream)</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        public oMessagingDto sendFax(string To, string From, byte[] Data)
        {
            oMessagingDto rval = new oMessagingDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (To == null || To == "")
                {
                    throw new ArgumentException("No receiver Information delivered.");
                }
                if (From == null || From == "")
                {
                    throw new ArgumentException("No sender Information delivered.");
                }
                cctx.validateService();
                INotificationGatewayBo Gateway = BOFactory.getInstance().createNotificationGateway();

                int Code = Gateway.sendFax(To, From, Data, ConfigurationBO.getServerDaten());
                if (Code == 2)
                {
                    rval.message.code = "True";
                    rval.message.detail = "Fax sent!";
                    rval.success();
                }
                else
                {
                    switch (Code)
                    {
                        case 3:
                            throw (new Exception("Preparing Fax failed!"));
                        case 4:
                            throw (new Exception("Transmission to Server failed!"));
                        default:
                            throw (new Exception("Error occured sending Fax!"));
                    }
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
        /// Web Interface to send an SMS
        /// </summary>
        /// <param name="To">Recipient</param>
        /// <param name="From">Sender</param>
        /// <param name="Body">SMS Text as a String</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        public oMessagingDto sendSms(string To, string From, string Body)
        {
            oMessagingDto rval = new oMessagingDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (To == null || To == "")
                {
                    throw new ArgumentException("No receiver Information delivered.");
                }
                if (From == null || From == "")
                {
                    throw new ArgumentException("No sender Information delivered.");
                }

                cctx.validateService();
                INotificationGatewayBo Gateway = BOFactory.getInstance().createNotificationGateway();

                int Code = Gateway.sendSms(To, From, Body, ConfigurationBO.getServerDaten());

                if (Code == 2)
                {
                    rval.message.code = "True";
                    rval.message.detail = "SMS sent!";
                    rval.success();
                }
                else
                {
                    switch (Code)
                    {
                        case 3:
                            throw (new Exception("Preparing SMS failed!"));
                        case 4:
                            throw (new Exception("Transmission to Server failed!"));
                        default:
                            throw (new Exception("Error occured sending SMS!"));
                    }
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
        /// Web Interface to send an E-Mail, SMS or Fax via DB settings
        /// </summary>
        /// <param name="ID">DB ID</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        public oMessagingDto sendDbNotification(int ID)
        {
            oMessagingDto rval = new oMessagingDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (ID == 0)
                {
                    throw new ArgumentException("No DatabaseID was sent.");
                }
                cctx.validateService();

                INotificationGatewayBo Gateway = BOFactory.getInstance().createNotificationGateway();

                int Code = Gateway.sendEAINotification(ID, ConfigurationBO.getServerDaten());

                if (Code == 2)
                {
                    rval.message.code = "True";
                    rval.message.detail = "DB-based Notification sent!";
                    rval.success();
                }
                else
                {
                    switch (Code)
                    {
                        case 3:
                            throw (new Exception("Preparing DB Notification failed!"));
                        case 4:
                            throw (new Exception("Transmission to Server failed!"));
                        default:
                            throw (new Exception("Error occured sending DB Notification!"));
                    }
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
        /// Die Methode instantiiert die im Auskunfttypen angegebene ServiceFacade und ruft deren Methode doAuskunft auf
        /// </summary>
        /// <param name="sysAuskunft">Datenbank ID des Auskunftssatzes</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        public oMessagingDto doAuskunft(long sysAuskunft)
        {
            oMessagingDto rval = new oMessagingDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (sysAuskunft == 0)
                {
                    throw new ArgumentException("SysAuskunft ist 0.");
                }
                cctx.validateService();
                IAuskunftDao dao = new AuskunftDao();
                String auskunftBez = dao.GetAuskunfttypBezeichng(sysAuskunft);
                AuskunftBoFactoryFactory.getFactory(auskunftBez).performAuskunft(sysAuskunft);
                

                rval.Output = true; 
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
                    throw new ArgumentException("EurotaxGetForcastDto is empty.");
                }
                cctx.validateService();

                EurotaxInDto inDto = new EurotaxInDto();
                inDto.prodKontext = input.prodKontext;
                AutoMapper.Mapper.Map<iEurotaxGetForecastDto, EurotaxInDto>(input, inDto);

                List<EurotaxOutDto> outDto = AuskunftBoFactory.CreateDefaultEurotaxBo().GetForecast(inDto);

                rval.EurotaxForecastList = new List<EurotaxGetForecastDto>();

                foreach (var forecastDto in outDto)
                {
                    EurotaxGetForecastDto output = new EurotaxGetForecastDto();
                    
                    AutoMapper.Mapper.Map<EurotaxOutDto, EurotaxGetForecastDto>(forecastDto, output);
                    rval.EurotaxForecastList.Add(output);
                    rval.source = forecastDto.source;
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
        /// Eurotax GetForecast Auskunft einholen
        /// </summary>
        /// <param name="input">iEurotaxForecastDto</param>
        /// <returns>oEurotaxForecastDto</returns>        
        public oEurotaxGetForecastDto EurotaxGetRemo(iEurotaxGetForecastDto input)
        {
            oEurotaxGetForecastDto rval = new oEurotaxGetForecastDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("EurotaxGetForcastDto is empty.");
                }
                cctx.validateService();

                EurotaxInDto inDto = new EurotaxInDto();
                inDto.prodKontext = input.prodKontext;
                AutoMapper.Mapper.Map<iEurotaxGetForecastDto, EurotaxInDto>(input, inDto);

                List<EurotaxOutDto> outDto = AuskunftBoFactory.CreateDefaultEurotaxBo().GetRemo(inDto);

                rval.EurotaxForecastList = new List<EurotaxGetForecastDto>();

                foreach (var forecastDto in outDto)
                {
                    EurotaxGetForecastDto output = new EurotaxGetForecastDto();
                    
                    AutoMapper.Mapper.Map<EurotaxOutDto, EurotaxGetForecastDto>(forecastDto, output);
                    rval.source = forecastDto.source;
                    rval.EurotaxForecastList.Add(output);
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
        /// Eurotax GetValuation Auskunft einholen
        /// </summary>
        /// <param name="input">iEurotaxValuationDto</param>
        /// <returns>oEurotaxValuationDto</returns>
        public oEurotaxGetValuationDto EurotaxGetValuation(iEurotaxGetValuationDto input)
        {
            oEurotaxGetValuationDto rval = new oEurotaxGetValuationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("EurotaxGetValuationDto is empty.");
                }
                cctx.validateService();
                EurotaxInDto inDto = new EurotaxInDto();
                inDto.prodKontext = input.prodKontext;
                inDto.NationalVehicleCode = input.NationalVehicleCode;
                inDto.RegistrationDate = input.RegistrationDate;
                inDto.ISOCountryCodeValuation = input.ISOCountryCode;
                inDto.ISOCurrencyCodeValuation = input.ISOCurrencyCode;
                inDto.ISOLanguageCodeValuation = input.ISOLanguageCode;
                inDto.Mileage = input.Mileage;
                inDto.sysobtyp = input.sysobtyp;

                EurotaxOutDto outDto = AuskunftBoFactory.CreateDefaultEurotaxBo().GetValuation(inDto);

                
                rval.EurotaxGetValuationDto = new EurotaxGetValuationDto();
                AutoMapper.Mapper.Map<EurotaxOutDto, EurotaxGetValuationDto>(outDto, rval.EurotaxGetValuationDto);
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
                cctx.fillBaseDto(rval, new ServiceBaseException("F_01001_EurotaxServiceCommFailure", e.Message, MessageType.Warn));
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
        /// get answer from S1 (via Simple Services)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public oMessagingDto getAuskunftS1(S1AnswerInputData input)
        {
            oMessagingDto rval = new oMessagingDto();
            CredentialContext cctx = new CredentialContext();
            List<S1GetResponseDto> listInBO = new List<S1GetResponseDto>();

            try
            {

                if (input.SysAuskunft == 0)
                {
                    throw new ArgumentException("getAuskunftS1: SysAuskunftssatz is empty or 0");
                }

                RISKEWBS1DBDao s1DBDao = new RISKEWBS1DBDao(input.SysAuskunft);

                if (input.ErrorCode > 0)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorS1BatchProgram, "");

                    ArgumentException e = new ArgumentException(input.ErrorText);
                    _log.Error("getAuskunftS1: error from Simple Services", e);
                    throw new ArgumentException("getAuskunftS1: Fehler S1/Batchprogram: " + input.ErrorText);
                }

                if (input.OutputString == "" || input.OutputString == null)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorS1BatchProgram, "");

                    ArgumentException e = new ArgumentException("Input string from Siple Services is empy");
                    _log.Error("getAuskunftS1", e);
                    throw new ArgumentException("Input string is empty");
                }

                cctx.validateService();

                // parsing imputString (received from s1) into list of the S1GetResponseDto
                S1Bo s1Bo = new S1Bo(input.OutputString);
                s1Bo.GetList(listInBO);

                // check the data received from s1
                if (s1DBDao.ValidateAnswerData(listInBO) == 1)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorSimpleService, "");
                    ArgumentException e = new ArgumentException(s1DBDao.ErrorText);
                    _log.Error("getAuskunftS1: error by ValidateAnswareData", e);
                    throw new ArgumentException(s1DBDao.ErrorText);
                }

                // save in DB the data received from s1
                if (s1DBDao.SaveDataFromS1(listInBO) == 1)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorSimpleService, "");
                    ArgumentException e = new ArgumentException(s1DBDao.ErrorText);
                    _log.Error("getAuskunftS1: Error by SaveDataFromS1", e);
                    throw new ArgumentException(s1DBDao.ErrorText);
                }

                rval.message.code = "True";
                rval.message.detail = "getAuskunftS1 OK!";
                rval.Output = true;
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

        public ogetELDto getEL(long sysantrag)
        {
            ogetELDto rval = new ogetELDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (sysantrag == 0)
                {
                    throw new ArgumentException("EurotaxGetValuationDto is empty.");
                }
                cctx.validateService();
                EurotaxInDto inDto = new EurotaxInDto();

                //Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ELOutDto outDto = AuskunftBoFactory.CreateDefaultEurotaxBo().getEL(sysantrag);
                //AutoMapper.Mapper.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ELOutDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ELOutDto>();
                //rval.eLOutDto = new Cic.OpenOne.GateBANKNOW.Service.DTO.ELOutDto();
                //AutoMapper.Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ELOutDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ELOutDto>(outDto, rval.eLOutDto);


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