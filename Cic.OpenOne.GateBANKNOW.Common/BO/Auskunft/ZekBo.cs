using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// ZekBo implements AbstractZekBo
    /// </summary>
    public class ZekBo : AbstractZekBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const long codeTechExc = -2;
        private const long codeSerAufExc = -1;
        
        DAO.Auskunft.ZEKRef.IdentityDescriptor idDesc;

        List<RequestEntity> requestEntities;
        CommonMultiResponse response;
        ZekOutDto outDto;

        /// <summary>
        /// Konstruktor, IZEKWSDao, IZEKDBDao, IAuskunfDao werden initialisert
        /// </summary>
        /// <param name="zekWSDao"></param>
        /// <param name="zekDBDao"></param>
        /// <param name="auskunftDao"></param>
        public ZekBo(IZekWSDao zekWSDao, IZekDBDao zekDBDao, IAuskunftDao auskunftDao)
            : base(zekWSDao, zekDBDao, auskunftDao)
        {
        }

        #region EC1
        /// <summary>
        /// Saves Auskunft and Zekinput, sends kreditgesuchNeu request (EC1) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto kreditgesuchNeu(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKKreditgesuchNeu);
            try
            {
                // Save KreditgesuchNeu Input
                zekDBDao.SaveKreditgesuchNeuInput(sysAuskunft, inDto);

                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                code = codeSerAufExc;

                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.kreditgesuchNeu(idDesc, requestEntities.ToArray(), inDto.Zielverein, inDto.Anfragegrund, inDto.PreviousKreditgesuchID);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save KreditgesuchNeu Output
                zekDBDao.SaveKreditgesuchNeuOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in kreditgesuchNeu!");
                throw new ApplicationException("Unexpected Exception in kreditgesuchNeu!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, sends kreditgesuchNeu request (EC1) away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto kreditgesuchNeu(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntities to ZekRef.requestEntities
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                code = codeSerAufExc;

                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                response = zekWSDao.kreditgesuchNeu(idDesc, requestEntities.ToArray(), inDto.Zielverein, inDto.Anfragegrund, inDto.PreviousKreditgesuchID);
                
                code = codeTechExc;
                
                // Map WS response to ZekOutDto
                outDto = MyMapCommonMultiResponseToOutDto(response);
                
                // Save outDto to database
                zekDBDao.SaveKreditgesuchNeuOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;

                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in kreditgesuchNeu!");
                throw new ApplicationException("Unexpected Exception in kreditgesuchNeu!", e);
            }

        }
        #endregion

        #region EC2
        /// <summary>
        /// Saves Auskunft and Zekinput, sends informativabfrage request (EC2) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto informativabfrage(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKInformativabfrage);
            // Save Informativanfrage Input
            try
            {
                zekDBDao.SaveInformativanfrageInput(sysAuskunft, inDto);
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                RequestEntity requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                InfoResponse infoResponse = zekWSDao.informativabfrage(idDesc, requestEntity, inDto.Zielverein, inDto.Anfragegrund);
                code = codeTechExc;
                outDto = MyMapInfoResponseToOutDto(infoResponse);
                // Save Informativanfrage Output
                zekDBDao.SaveInformativanfrageOutput(sysAuskunft, outDto);
                // Save Bemerkung
                if (outDto.FoundContracts!= null)
                {
                    zekDBDao.SaveBemerkungInformativabfrage(sysAuskunft, inDto.Bemerkung, inDto.vertragnummer, inDto.antragnummer, inDto.vpnummer);
                }
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in informativabfrage!");
                throw new ApplicationException("Unexpected Exception in informativabfrage!", e);
            }
        }


        /// <summary>
        /// Saves Auskunft and Zekinput, sends informativabfrage request (EC2) away and saves response und logdump 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto informativabfrageLogDump(ZekInDto inDto, string area, long sysAreaid, long syswfuser)
        {
           
                     
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKInformativabfrage);
            auskunftDao.setAuskunfAreaUndId(sysAuskunft, area, sysAreaid);
            auskunftDao.setAuskunftWfuser(sysAuskunft, syswfuser);
            // Save Informativanfrage Input
            try
            {
                zekDBDao.SaveInformativanfrageInput(sysAuskunft, inDto);

                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                RequestEntity requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                InfoResponse infoResponse = zekWSDao.informativabfrage(idDesc, requestEntity, inDto.Zielverein, inDto.Anfragegrund);
                code = codeTechExc;
                outDto = MyMapInfoResponseToOutDto(infoResponse);
                // Save Informativanfrage Output
                zekDBDao.SaveInformativanfrageOutput(sysAuskunft, outDto);
                // Save Bemerkung
                if (outDto.FoundContracts != null)
                {
                    zekDBDao.SaveBemerkungInformativabfrage(sysAuskunft, inDto.Bemerkung, inDto.vertragnummer, inDto.antragnummer, inDto.vpnummer);
                }
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in informativabfrage!");
                throw new ApplicationException("Unexpected Exception in informativabfrage!", e);
            }
        }


        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to informativabfrage request (EC2), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto informativabfrage(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntity to ZekRef.RequestEntity
                RequestEntity requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                InfoResponse infoResponse = zekWSDao.informativabfrage(idDesc, requestEntity, inDto.Zielverein, inDto.Anfragegrund);
                code = codeTechExc;
                // Map WS response to ZekOutDto
                outDto = MyMapInfoResponseToOutDto(infoResponse);
                // Save outDto to database
                zekDBDao.SaveInformativanfrageOutput(sysAuskunft, outDto);
                // Save Bemerkung
                if (outDto.FoundContracts != null)
                {
                    this.zekDBDao.SaveBemerkungInformativabfrage(sysAuskunft, inDto.Bemerkung, inDto.vertragnummer, inDto.antragnummer, inDto.vpnummer);
                }
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in informativabfrage!");
                throw new ApplicationException("Unexpected Exception in informativabfrage!", e);
            }
        }
        #endregion


        //BNR11 CR
        #region EC2FOROL
        /// <summary>
        /// Saves Auskunft and Zekinput, sends informativabfrage request (EC2) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftOLDto informativabfrageOL(AuskunftOLDto inDto)
        {
            ZekInDto zekInDto = new ZekInDto();
            AuskunftDto auskunftDto = new AuskunftDto();
            AuskunftOLDto auskunftOLDto = new AuskunftOLDto();
            ZekOLOutDto zekOLOutDto = new ZekOLOutDto();
            auskunftOLDto.ZekOLOutDto = zekOLOutDto;
            Cic.OpenOne.Common.DTO.Message message = new OpenOne.Common.DTO.Message();
            bool externeAbrage = inDto.externeAbrage;
            try
            {
                if (inDto.externeAbrage)
                {
                    zekInDto = Mapper.Map<ZekOLInDto, ZekInDto>(inDto.ZekOLInDto);
                    zekInDto.Bemerkung = inDto.bemerkung;
                    zekInDto.antragnummer = inDto.antragnummer;
                    zekInDto.vertragnummer = inDto.vertragnummer;
                    zekInDto.vpnummer = inDto.vpnummer;

                    try
                    {

                        auskunftDto = informativabfrageLogDump(zekInDto, inDto.area, inDto.sysAreaid, inDto.syswfuser);

                        if (auskunftDto.ZekOutDto.TransactionError != null)
                        {
                            message.code = "ZEK_TRANSACTIONERROR_" + auskunftDto.ZekOutDto.TransactionError.Code;
                            message.detail = auskunftDto.ZekOutDto.TransactionError.Text;
                        }
                    }
                    catch (Exception e)
                    {
                        message.code = "ABFRAGE_EXTERN_NOK";
                        message.detail = e.Message;
                        externeAbrage = false;

                        if (inDto.area != "" && inDto.sysAreaid > 0)
                        {
                            // DB lessen bei area und sysid

                            AuskunftDto auskunftdto = auskunftDao.FindByAreaSysId(inDto.sysAreaid, inDto.area, AuskunfttypDao.ZEKInformativabfrage.ToString());
                            if (auskunftdto != null && auskunftdto.sysAuskunft > 0)
                            {
                                try
                                {
                                    inDto.sysAuskunft = auskunftdto.sysAuskunft;
                                    ZekOutDto zekOutDto = zekDBDao.getDBInformativanfrageOutput(inDto.sysAuskunft);
                                    auskunftDto.ZekOutDto = zekOutDto;
                                    message.code = "ABFRAGE_EXTERN_NOK_INTERNE_RESPONSE_OK";
                                }
                                catch (Exception e1)
                                {
                                    message.detail = e1.Message;

                                }
                            }
                            else
                            {
                                message.code = "ABFRAGE_EXTERN_NOK_INTERNE_RESPONSE_NICHTVERFUEGBAR";
                            }
                        }
                    }
                }

                else if (inDto.sysAuskunft > 0)
                {
                    try
                    {
                        ZekOutDto zekOutDto = zekDBDao.getDBInformativanfrageOutput(inDto.sysAuskunft);
                        auskunftDto.ZekOutDto = zekOutDto;
                        message.code = "ABFRAGE_INTERN_OK";
                        //DB lessen bei sysauskunft

                    }
                    catch
                    {
                        message.code = "ABFRAGE_INTERN_NOK";
                    }


                }
                else if (inDto.area != "" && inDto.sysAreaid > 0)
                {
                    // DB lessen bei area und sysid
                    AuskunftDto auskunftdto = auskunftDao.FindByAreaSysId(inDto.sysAreaid, inDto.area, AuskunfttypDao.ZEKInformativabfrage.ToString());
                    if (auskunftdto != null && auskunftdto.sysAuskunft > 0)
                    {
                        inDto.sysAuskunft = auskunftdto.sysAuskunft;
                        ZekOutDto zekOutDto = zekDBDao.getDBInformativanfrageOutput(inDto.sysAuskunft);
                        auskunftDto.ZekOutDto = zekOutDto;
                        message.code = "ABFRAGE_INTERN_OK";
                    }
                    else
                    {
                        message.code = "ABFRAGE_INTERN_NOK";
                    }
                }

                auskunftOLDto = Mapper.Map<AuskunftDto, AuskunftOLDto>(auskunftDto);
                auskunftOLDto.ZekOLOutDto = MyMapZekOLOutDto(auskunftDto.ZekOutDto);
                auskunftOLDto.ZekOLInDto = inDto.ZekOLInDto;
                auskunftOLDto.bemerkung = inDto.bemerkung;
                auskunftOLDto.antragnummer = inDto.antragnummer;
                auskunftOLDto.vertragnummer = inDto.vertragnummer;
                auskunftOLDto.vpnummer = inDto.vpnummer;

                
                if (message.code != "")
                {
                    auskunftOLDto.ZekOLOutDto.message = message;
                }
                else
                {
                    auskunftOLDto.ZekOLOutDto.success();
                }

                return auskunftOLDto;
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                message.code = "F_00004_DatabaseUnreachableException";
                auskunftOLDto.ZekOLOutDto.message = message;
                return auskunftOLDto;
            }
            catch (ArgumentException)
            {
                message.code = "F_00003_ArgumentException";
                auskunftOLDto.ZekOLOutDto.message = message;
                return auskunftOLDto;
            }
            catch (Exception)//unhandled exception - should not happen!
            {
                message.code = "F_00001_GeneralError";
                auskunftOLDto.ZekOLOutDto.message = message;
                return auskunftOLDto;
            }
        }
       
        #endregion

        #region EC3
        /// <summary>
        /// Saves Auskunft and Zekinput, sends kreditgesuchAblehnen request (EC3) away and saves response 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto kreditgesuchAblehnen(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKKreditgesuchAblehnen);
            try
            {
                // Save ZEK Input
                zekDBDao.SaveKreditgesuchAblehnenInput(sysAuskunft, inDto);
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                RequestEntity requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CreditClaimRejectionResponse response = zekWSDao.kreditgesuchAblehnen(idDesc, requestEntity, inDto.DatumAblehnung, inDto.Ablehnungsgrund, inDto.KreditgesuchID);
                code = codeTechExc;
                outDto = MyMapCreditClaimRejectionResponseToOutDto(response);
                zekDBDao.SaveKreditgesuchAblehnenOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in kreditgesuchAblehnen!");
                throw new ApplicationException("Unexpected Exception in kreditgesuchAblehnen!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to kreditgesuchAblehnen request (EC3), 
        /// sends request away and maps response to ZekOutDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto kreditgesuchAblehnen(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntity to ZekRef.RequestEntity
                RequestEntity requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away   zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                CreditClaimRejectionResponse response = zekWSDao.kreditgesuchAblehnen(idDesc, requestEntity, inDto.DatumAblehnung, inDto.Ablehnungsgrund, inDto.KreditgesuchID);
                code = codeTechExc;
                outDto = MyMapCreditClaimRejectionResponseToOutDto(response);
                // Save outDto to database
                zekDBDao.SaveKreditgesuchAblehnenOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in kreditgesuchAblehnen!");
                throw new ApplicationException("Unexpected Exception in kreditgesuchAblehnen!", e);
            }
        }
        #endregion

        #region EC4
        /// <summary>
        /// Saves Auskunft and Zekinput, sends registerBardarlehen request (EC4) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto registerBardarlehen(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKRegisterBardarlehen);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsanmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.BardarlehenDescription bardarlehen = MyMapFromDtoToBardarlehen(inDto.Bardarlehen);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerBardarlehen(idDesc, requestEntities.ToArray(), bardarlehen, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerBardarlehen!");
                throw new ApplicationException("Unexpected Exception in registerBardarlehen!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to registerBardarlehen request (EC4), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto registerBardarlehen(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.BardarlehenDescription bardarlehen = MyMapFromDtoToBardarlehen(inDto.Bardarlehen);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerBardarlehen(idDesc, requestEntities.ToArray(), bardarlehen, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerBardarlehen!");
                throw new ApplicationException("Unexpected Exception in registerBardarlehen!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends registerFestkredit request (EC4) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto registerFestkredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKRegisterFestkredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsanmeldungInput(sysAuskunft, inDto);
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                List<RequestEntity> requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.Festkredit festkredit = MyMapFromDtoToFestkredit(inDto.Festkredit);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerFestkredit(idDesc, requestEntities.ToArray(), festkredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerFestkredit!");
                throw new ApplicationException("Unexpected Exception in registerFestkredit!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to registerFestkredit request (EC4), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto registerFestkredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.Festkredit festkredit = MyMapFromDtoToFestkredit(inDto.Festkredit);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
        
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerFestkredit(idDesc, requestEntities.ToArray(), festkredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerFestkredit!");
                throw new ApplicationException("Unexpected Exception in registerFestkredit!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends registerKontokorrentkredit request (EC4) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto registerKontokorrentkredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKRegisterKontokorrentkredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsanmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kredit = MyMapFromDtoToKontokorrentkredit(inDto.Kontokorrent);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerKontokorrentkredit(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save Vertragsanmeldung Output
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerKontokorrentkredit!");
                throw new ApplicationException("Unexpected Exception in registerKontokorrentkredit!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to registerKontokorrentkredit request (EC4), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto registerKontokorrentkredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kredit = MyMapFromDtoToKontokorrentkredit(inDto.Kontokorrent);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerKontokorrentkredit(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerKontokorrentkredit!");
                throw new ApplicationException("Unexpected Exception in registerKontokorrentkredit!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends registerLeasingMietvertrag request (EC4) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto registerLeasingMietvertrag(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKRegisterLeasingMietvertrag);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsanmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.LeasingMietvertragDescription kredit = MyMapFromDtoToLeasingMietvertrag(inDto.LeasingMietvertrag);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerLeasingMietvertrag(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save Vertragsanmeldung Output
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerLeasingMietvertrag!");
                throw new ApplicationException("Unexpected Exception in registerLeasingMietvertrag!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to registerLeasingMietvertrag request (EC4), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto registerLeasingMietvertrag(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.LeasingMietvertragDescription kredit = MyMapFromDtoToLeasingMietvertrag(inDto.LeasingMietvertrag);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerLeasingMietvertrag(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerLeasingMietvertrag!");
                throw new ApplicationException("Unexpected Exception in registerLeasingMietvertrag!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends registerTeilzahlungskredit request (EC4) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto registerTeilzahlungskredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKRegisterTeilzahlungskredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsanmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                //DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription kredit = MyMapFromDtoToTeilzahlungskredit(inDto.Teilzahlung);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlung);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                // CommonMultiResponse multiResponse = zekWSDao.registerTeilzahlungskredit(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                CommonMultiResponse multiResponse = zekWSDao.registerTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag, inDto.KreditgesuchID, inDto.Zielverein);
                

                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save Vertragsanmeldung Output
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                // Update Auskunft
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerTeilzahlungskredit!");
                throw new ApplicationException("Unexpected Exception in registerTeilzahlungskredit!", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto registerTeilzahlungsvertrag(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKRegisterTeilzahlungsvertrag);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsanmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlungvertrag);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag, inDto.KreditgesuchID, inDto.Zielverein);

                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save Vertragsanmeldung Output
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                // Update Auskunft
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerTeilzahlungsvertrag!");
                throw new ApplicationException("Unexpected Exception in registerTeilzahlungsvertrag!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to registerTeilzahlungskredit request (EC4), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto registerTeilzahlungskredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                //DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription kredit = MyMapFromDtoToTeilzahlungskredit(inDto.Teilzahlung);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlung);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away 
                // CommonMultiResponse multiResponse = zekWSDao.registerTeilzahlungskredit(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                CommonMultiResponse multiResponse = zekWSDao.registerTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag, inDto.KreditgesuchID, inDto.Zielverein);

                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerTeilzahlungskredit!");
                throw new ApplicationException("Unexpected Exception in registerTeilzahlungskredit!", e);
            }
        }

        public override AuskunftDto registerTeilzahlungsvertrag(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlungvertrag);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.registerTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag, inDto.KreditgesuchID, inDto.Zielverein);

                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in registerTeilzahlungsvertrag!");
                throw new ApplicationException("Unexpected Exception in registerTeilzahlungsvertrag!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends meldungKartenengagement request (EC4) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto meldungKartenengagement(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKMeldungKartenengagement);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsanmeldungInput(sysAuskunft, inDto);
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                KartenengagementDescription kredit = MyMapFromDtoToKartenengagement(inDto.Kartenengagement);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.meldungKartenengagement(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save Vertragsanmeldung Output
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in meldungKartenengagement!");
                throw new ApplicationException("Unexpected Exception in meldungKartenengagement!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to meldungKartenengagement request (EC4), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto meldungKartenengagement(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                KartenengagementDescription kredit = MyMapFromDtoToKartenengagement(inDto.Kartenengagement);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.meldungKartenengagement(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in meldungKartenengagement!");
                throw new ApplicationException("Unexpected Exception in meldungKartenengagement!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends meldungUeberziehungskredit request (EC4) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto meldungUeberziehungskredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKMeldungUeberziehungskredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsanmeldungInput(sysAuskunft, inDto);
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                UeberziehungskreditDescription kredit = MyMapFromDtoToUeberziehungskredit(inDto.Ueberziehungskredit);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                
                // Send request away 
                CommonMultiResponse multiResponse = zekWSDao.meldungUeberziehungskredit(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save Vertragsanmeldung Output
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in meldungUeberziehungskredit!");
                throw new ApplicationException("Unexpected Exception in meldungUeberziehungskredit!", e);
            }
        }
        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to meldungUeberziehungskredit request (EC4), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto meldungUeberziehungskredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                UeberziehungskreditDescription kredit = MyMapFromDtoToUeberziehungskredit(inDto.Ueberziehungskredit);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.meldungUeberziehungskredit(idDesc, requestEntities.ToArray(), kredit, inDto.KreditgesuchID, inDto.Zielverein);

                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsanmeldungOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in meldungUeberziehungskredit!");
                throw new ApplicationException("Unexpected Exception in meldungUeberziehungskredit!", e);
            }
        }
        #endregion

        #region EC5

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to closeBardarlehen request, 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto closeBardarlehen(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;

                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.BardarlehenDescription bardarlehen = MyMapFromDtoToBardarlehen(inDto.Bardarlehen);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeBardarlehen(idDesc, requestEntities.ToArray(), bardarlehen);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeBardarlehen!");
                throw new ApplicationException("Unexpected Exception in closeBardarlehen!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends closeBardarlehen request away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto closeBardarlehen(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKCloseBardarlehen);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsabmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.BardarlehenDescription bardarlehen = MyMapFromDtoToBardarlehen(inDto.Bardarlehen);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeBardarlehen(idDesc, requestEntities.ToArray(), bardarlehen);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeBardarlehen!");
                throw new ApplicationException("Unexpected Exception in closeBardarlehen!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to closeLeasingMietvertrag request, 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto closeLeasingMietvertrag(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;

                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.LeasingMietvertragDescription leasingMietvertrag = MyMapFromDtoToLeasingMietvertrag(inDto.LeasingMietvertrag);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeLeasingMietvertrag(idDesc, requestEntities.ToArray(), leasingMietvertrag);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeLeasingMietvertrag!");
                throw new ApplicationException("Unexpected Exception in closeLeasingMietvertrag!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends closeLeasingMietvertrag request away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto closeLeasingMietvertrag(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKCloseLeasingMietvertrag);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsabmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.LeasingMietvertragDescription leasingMietvertrag = MyMapFromDtoToLeasingMietvertrag(inDto.LeasingMietvertrag);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away 
                CommonMultiResponse multiResponse = zekWSDao.closeLeasingMietvertrag(idDesc, requestEntities.ToArray(), leasingMietvertrag);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeLeasingMietvertrag!");
                throw new ApplicationException("Unexpected Exception in closeLeasingMietvertrag!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to closeFestkredit request (EC5), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto closeFestkredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.Festkredit circumstantialCreditDescription = MyMapFromDtoToFestkredit(inDto.Festkredit);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeFestkredit(idDesc, requestEntities.ToArray(), circumstantialCreditDescription);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeFestkredit!");
                throw new ApplicationException("Unexpected Exception in closeFestkredit!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends closeFestkredit request (EC5) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto closeFestkredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKCloseFestkredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsabmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.Festkredit circumstantialCreditDescription = MyMapFromDtoToFestkredit(inDto.Festkredit);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeFestkredit(idDesc, requestEntities.ToArray(), circumstantialCreditDescription);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeFestkredit!");
                throw new ApplicationException("Unexpected Exception in closeFestkredit!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to closeTeilzahlungskredit request (EC5), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto closeTeilzahlungskredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                //DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription teilzahlungskredit = MyMapFromDtoToTeilzahlungskredit(inDto.Teilzahlung);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlung);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeTeilzahlungskredit!");
                throw new ApplicationException("Unexpected Exception in closeTeilzahlungskredit!", e);
            }
        }

        public override AuskunftDto closeTeilzahlungsvertrag(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription teilzahlungsvertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlungvertrag);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), teilzahlungsvertrag);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeTeilzahlungsvertrag!");
                throw new ApplicationException("Unexpected Exception in closeTeilzahlungsvertrag!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends closeTeilzahlungskredit request (EC5) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto closeTeilzahlungskredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKCloseTeilzahlungskredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsabmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                //DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription teilzahlungskredit = MyMapFromDtoToTeilzahlungskredit(inDto.Teilzahlung);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlung);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeTeilzahlungskredit!");
                throw new ApplicationException("Unexpected Exception in closeTeilzahlungskredit!", e);
            }
        }


        public override AuskunftDto closeTeilzahlungsvertrag(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKCloseTeilzahlungsvertrag);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsabmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription teilzahlungsvertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlungvertrag);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), teilzahlungsvertrag);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeTeilzahlungsvertrag!");
                throw new ApplicationException("Unexpected Exception in closeTeilzahlungsvertrag!", e);
            }
        }


        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to closeKontokorrentkredit request (EC5), 
        /// sends request away and saves response
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto closeKontokorrentkredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntites to ZekRef.RequestEntites
                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kontokorrentkredit = MyMapFromDtoToKontokorrentkredit(inDto.Kontokorrent);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeKontokorrentkredit(idDesc, requestEntities.ToArray(), kontokorrentkredit);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeKontokorrentkredit!");
                throw new ApplicationException("Unexpected Exception in closeKontokorrentkredit!", e);
            }
        }

        /// <summary>
        /// Saves Auskunft and Zekinput, sends closeKontokorrentkredit request (EC5) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto closeKontokorrentkredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKCloseKontokorrentkredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveVertragsabmeldungInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kontokorrentkredit = MyMapFromDtoToKontokorrentkredit(inDto.Kontokorrent);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.closeKontokorrentkredit(idDesc, requestEntities.ToArray(), kontokorrentkredit);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveVertragsabmeldungOutput(sysAuskunft, outDto);

                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeKontokorrentkredit!");
                throw new ApplicationException("Unexpected Exception in closeKontokorrentkredit!", e);
            }
        }

        #endregion

        #region EC6
        /// <summary>
        /// Saves Auskunft and Zekinput, sends updateAddress request (EC6) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>ZekOutDto</returns>
        public override AuskunftDto updateAddress(ZekInDto inDto)
        {
            long code = codeTechExc;
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKUpdateAddress);
            try
            {
                zekDBDao.SaveUpdateAddressInput(sysAuskunft, inDto);
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                RequestEntity requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                RequestEntity requestEntityNew = MyMapFromDtoToRequestEntity(inDto.RequestEntityNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                UpdateAddressResponse response = zekWSDao.updateAddress(idDesc, requestEntity, requestEntityNew);
                code = codeTechExc;
                outDto = MyMapUpdateAddressResponseToOutDto(response);
                zekDBDao.SaveUpdateAddressOutput(sysAuskunft, outDto);
                
                if (outDto.TransactionError != null)
                {
                    code = (long)this.outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }

                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateAddress!");
                throw new ApplicationException("Unexpected Exception in updateAddress!", e);
            }

        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to updateAddress request (EC6), 
        /// sends request away and maps response to ZekOutDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto updateAddress(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                RequestEntity requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                RequestEntity requestEntityNew = MyMapFromDtoToRequestEntity(inDto.RequestEntityNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                UpdateAddressResponse response = zekWSDao.updateAddress(idDesc, requestEntity, requestEntityNew);
                code = codeTechExc;
                outDto = MyMapUpdateAddressResponseToOutDto(response);

                zekDBDao.SaveUpdateAddressOutput(sysAuskunft, outDto);
                
                if (outDto.TransactionError != null)
                {
                    code = (long)this.outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }

                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateAddress!");
                throw new ApplicationException("Unexpected Exception in updateAddress!", e);
            }
        }
        #endregion

        #region EC7
        /// <summary>
        /// Saves Auskunft and Zekinput, sends updateBardarlehen request (EC7) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto updateBardarlehen(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKUpdateBardarlehen);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveUpdateVertragInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.BardarlehenDescription bardarlehen = MyMapFromDtoToBardarlehen(inDto.Bardarlehen);
                DAO.Auskunft.ZEKRef.BardarlehenDescription bardarlehenNew = MyMapFromDtoToBardarlehen(inDto.BardarlehenNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateBardarlehen(idDesc, requestEntities.ToArray(), bardarlehen, bardarlehenNew);

                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateBardarlehen!");
                throw new ApplicationException("Unexpected Exception in updateBardarlehen!", e);
            }
        }
        /// <summary>
        /// Saves Auskunft and Zekinput, sends updateFestkredit request (EC7) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto updateFestkredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKUpdateFestkredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveUpdateVertragInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.Festkredit kredit = MyMapFromDtoToFestkredit(inDto.Festkredit);
                DAO.Auskunft.ZEKRef.Festkredit kreditNew = MyMapFromDtoToFestkredit(inDto.Festkredit);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateFestkredit(idDesc, requestEntities.ToArray(), kredit, kreditNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateFestkredit!");
                throw new ApplicationException("Unexpected Exception in updateFestkredit!", e);
            }
        }
        /// <summary>
        /// Saves Auskunft and Zekinput, sends updateKontokorrentkredit request (EC7) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto updateKontokorrentkredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKUpdateKontokorrentkredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveUpdateVertragInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }

                DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kredit = MyMapFromDtoToKontokorrentkredit(inDto.Kontokorrent);
                DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kreditNew = MyMapFromDtoToKontokorrentkredit(inDto.KontokorrentNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateKontokorrentkredit(idDesc, requestEntities.ToArray(), kredit, kreditNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateKontokorrentkredit!");
                throw new ApplicationException("Unexpected Exception in updateKontokorrentkredit!", e);

            }

        }
        /// <summary>
        /// Saves Auskunft and Zekinput, sends updateLeasingMietvertrag request (EC7) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto updateLeasingMietvertrag(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKUpdateLeasingMietvertrag);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveUpdateVertragInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.LeasingMietvertragDescription kredit = MyMapFromDtoToLeasingMietvertrag(inDto.LeasingMietvertrag);
                DAO.Auskunft.ZEKRef.LeasingMietvertragDescription kreditNew = MyMapFromDtoToLeasingMietvertrag(inDto.LeasingMietvertragNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateLeasingMietvertragkredit(idDesc, requestEntities.ToArray(), kredit, kreditNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateLeasingMietvertrag!");
                throw new ApplicationException("Unexpected Exception in updateLeasingMietvertrag!", e);
            }
        }
        /// <summary>
        /// Saves Auskunft and Zekinput, sends updateTeilzahlungskredit request (EC7) away and saves response
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto updateTeilzahlungskredit(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKUpdateTeilzahlungskredit);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveUpdateVertragInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                // DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription kredit = MyMapFromDtoToTeilzahlungskredit(inDto.Teilzahlung);
               // DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription kreditNew = MyMapFromDtoToTeilzahlungskredit(inDto.TeilzahlungNew);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlung);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertragNew = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlung);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                // CommonMultiResponse multiResponse = zekWSDao.updateTeilzahlungskredit(idDesc, requestEntities.ToArray(), kredit, kreditNew);
                CommonMultiResponse multiResponse = zekWSDao.updateTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag, vertragNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateTeilzahlungskredit!");
                throw new ApplicationException("Unexpected Exception in updateTeilzahlungskredit!", e);
            }
        }

        public override AuskunftDto updateTeilzahlungsvertrag(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKUpdateTeilzahlungsvertrag);
            try
            {
                // Save Vertragsanmeldung Input
                zekDBDao.SaveUpdateVertragInput(sysAuskunft, inDto);
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlungvertrag);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertragNew = MyMapFromDtoToTeilzahlungsvertrag(inDto.TeilzahlungvertragNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag, vertragNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateTeilzahlungsvertrag!");
                throw new ApplicationException("Unexpected Exception in updateTeilzahlungsvertrag!", e);
            }
        }

        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to updateBardarlehen request (EC7), 
        /// sends request away and maps response to ZekOutDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto updateBardarlehen(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.BardarlehenDescription bardarlehen = MyMapFromDtoToBardarlehen(inDto.Bardarlehen);
                DAO.Auskunft.ZEKRef.BardarlehenDescription bardarlehenNew = MyMapFromDtoToBardarlehen(inDto.BardarlehenNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateBardarlehen(idDesc, requestEntities.ToArray(), bardarlehen, bardarlehenNew);

                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateBardarlehen!");
                throw new ApplicationException("Unexpected Exception in updateBardarlehen!", e);
            }
        }
        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to updateFestkredit request (EC7), 
        /// sends request away and maps response to ZekOutDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto updateFestkredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.Festkredit kredit = MyMapFromDtoToFestkredit(inDto.Festkredit);
                DAO.Auskunft.ZEKRef.Festkredit kreditNew = MyMapFromDtoToFestkredit(inDto.Festkredit);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateFestkredit(idDesc, requestEntities.ToArray(), kredit, kreditNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateFestkredit!");
                throw new ApplicationException("Unexpected Exception in updateFestkredit!", e);
            }
        }
        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to updateKontokorrentkredit request (EC7), 
        /// sends request away and maps response to ZekOutDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto updateKontokorrentkredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kredit = MyMapFromDtoToKontokorrentkredit(inDto.Kontokorrent);
                DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kreditNew = MyMapFromDtoToKontokorrentkredit(inDto.KontokorrentNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateKontokorrentkredit(idDesc, requestEntities.ToArray(), kredit, kreditNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateKontokorrentkredit!");
                throw new ApplicationException("Unexpected Exception in updateKontokorrentkredit!", e);
            }
        }
        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to updateLeasingMietvertrag request (EC7), 
        /// sends request away and maps response to ZekOutDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto updateLeasingMietvertrag(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.LeasingMietvertragDescription kredit = MyMapFromDtoToLeasingMietvertrag(inDto.LeasingMietvertrag);
                DAO.Auskunft.ZEKRef.LeasingMietvertragDescription kreditNew = MyMapFromDtoToLeasingMietvertrag(inDto.LeasingMietvertragNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateLeasingMietvertragkredit(idDesc, requestEntities.ToArray(), kredit, kreditNew);
                
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);

                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateLeasingMietvertrag!");
                throw new ApplicationException("Unexpected Exception in updateLeasingMietvertrag!", e);
            }
        }
        /// <summary>
        /// Collects input from database by SysId, maps it to ZektInDto, maps ZekInDto to updateTeilzahlungskredit request (EC7), 
        /// sends request away and maps response to ZekOutDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto updateTeilzahlungskredit(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                //DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription kredit = MyMapFromDtoToTeilzahlungskredit(inDto.Teilzahlung);
                //DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription kreditNew = MyMapFromDtoToTeilzahlungskredit(inDto.TeilzahlungNew);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlung);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertragNew = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlung);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                //CommonMultiResponse multiResponse = zekWSDao.updateTeilzahlungskredit(idDesc, requestEntities.ToArray(), kredit, kreditNew);
                CommonMultiResponse multiResponse = zekWSDao.updateTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag, vertragNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in Teilzahlungskredit!");
                throw new ApplicationException("Unexpected Exception in Teilzahlungskredit!", e);
            }
        }


        public override AuskunftDto updateTeilzahlungsvertrag(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();

                requestEntities = new List<RequestEntity>();
                if (inDto.RequestEntities != null)
                {
                    foreach (ZekRequestEntityDto dto in inDto.RequestEntities)
                    {
                        this.requestEntities.Add(MyMapFromDtoToRequestEntity(dto));
                    }
                }
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = MyMapFromDtoToTeilzahlungsvertrag(inDto.Teilzahlungvertrag);
                DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertragNew = MyMapFromDtoToTeilzahlungsvertrag(inDto.TeilzahlungvertragNew);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.updateTeilzahlungsvertrag(idDesc, requestEntities.ToArray(), vertrag, vertragNew);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveUpdateVertragOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in Teilzahlungsvertrag!");
                throw new ApplicationException("Unexpected Exception in Teilzahlungsvertrag!", e);
            }
        }
        #endregion

        #region ECODE178

        /// <summary>
        /// eCode178Anmelden
        /// </summary>
        /// <param name="inDto">zekInDto</param>
        /// <returns></returns>
        public override AuskunftDto eCode178Anmelden(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKeCode178Anmelden);
            try
            {
                // Save KreditgesuchNeu Input
                zekDBDao.SaveeCode178AnmeldenInput(sysAuskunft, inDto);

                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();
                 // Map inDto.RequestEntity to ZekRef.RequestEntity
                RequestEntity requestEntity = null;
                if (inDto.RequestEntity != null)
                {
                    requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                }
                eCode178 eCode178 = MyMapFromDtoToeCode178(inDto.ZekeCode178Dto);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.eCode178Anmelden(idDesc, requestEntity, eCode178, inDto.KreditgesuchID, inDto.ContractId);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save KreditgesuchNeu Output
                zekDBDao.SaveeCode178AnmeldenOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178Anmelden!");
                throw new ApplicationException("Unexpected Exception in eCode178Anmelden!", e);
            }
        }

        /// <summary>
        /// eCode178Anmelden
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public override AuskunftDto eCode178Anmelden(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {   
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Map inDto.RequestEntity to ZekRef.RequestEntity
                RequestEntity requestEntity = null;
                if (inDto.RequestEntity != null)
                {
                    requestEntity = MyMapFromDtoToRequestEntity(inDto.RequestEntity);
                }
                eCode178 eCode178 = MyMapFromDtoToeCode178(inDto.ZekeCode178Dto);
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.eCode178Anmelden(idDesc, requestEntity, eCode178, inDto.KreditgesuchID, inDto.ContractId); ;
                code = codeTechExc;
                // Map WS response to ZekOutDto
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveeCode178AnmeldenOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178Anmelden!");
                throw new ApplicationException("Unexpected Exception in eCode178Anmelden!", e);
            }
        }

        /// <summary>
        /// eCode178Mutieren
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public override AuskunftDto eCode178Mutieren(ZekInDto inDto)
        {
           long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKeCode178Mutieren);
            try
            {
                // Save KreditgesuchNeu Input
                zekDBDao.SaveeCode178MutierenInput(sysAuskunft, inDto);

                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();
                eCode178 eCode178= MyMapFromDtoToeCode178(inDto.ZekeCode178Dto);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.eCode178Mutieren(idDesc, eCode178);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save KreditgesuchNeu Output
                zekDBDao.SaveeCode178MutierenOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178Mutieren!");
                throw new ApplicationException("Unexpected Exception in eCode178Mutienren!", e);
            }
        }

        /// <summary>
        /// eCode178Mutieren
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public override AuskunftDto eCode178Mutieren(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                eCode178 ecode178 = MyMapFromDtoToeCode178(inDto.ZekeCode178Dto);
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.eCode178Mutieren(idDesc, ecode178);
                code = codeTechExc;
                // Map WS response to ZekOutDto
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveeCode178MutierenOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178Mutieren!");
                throw new ApplicationException("Unexpected Exception in eCode178Mutienren!", e);
            }
        }

        /// <summary>
        /// eCode178Abmelden
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public override AuskunftDto eCode178Abmelden(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKeCode178Abmelden);
            try
            {
                // Save KreditgesuchNeu Input
                zekDBDao.SaveeCode178AbmeldenInput(sysAuskunft, inDto);

                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();
                eCode178 eCode178 = MyMapFromDtoToeCode178(inDto.ZekeCode178Dto);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.eCode178Abmelden(idDesc,  eCode178);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save KreditgesuchNeu Output
                zekDBDao.SaveeCode178AbmeldenOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178Abmelden!");
                throw new ApplicationException("Unexpected Exception in eCode178Abmelden!", e);
            }
        }


        /// <summary>
        /// eCode178Abmelden
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public override AuskunftDto eCode178Abmelden(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {

                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                eCode178 eCode178 = MyMapFromDtoToeCode178(inDto.ZekeCode178Dto);
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.eCode178Abmelden(idDesc, eCode178);
                code = codeTechExc;
                // Map WS response to ZekOutDto
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveeCode178AbmeldenOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178Abnmelden!");
                throw new ApplicationException("Unexpected Exception in eCode178Abmelden!", e);
            }
        }

        /// <summary>
        /// eCode178Abfrage
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public override AuskunftDto eCode178Abfrage(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKeCode178Abfragen);
            try
            {
                // Save KreditgesuchNeu Input
                zekDBDao.SaveeCode178AbfrageInput(sysAuskunft, inDto);

                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();
                eCode178 eCode178 = MyMapFromDtoToeCode178(inDto.ZekeCode178Dto);
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CommonMultiResponse multiResponse = zekWSDao.eCode178Abfrage(idDesc,  eCode178);
                code = codeTechExc;
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save KreditgesuchNeu Output
                zekDBDao.SaveeCode178AbfrageOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178Abfrage!");
                throw new ApplicationException("Unexpected Exception in eCode178Abfrage!", e);
            }
        }


        /// <summary>
        /// eCode178Abfragen
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public override AuskunftDto eCode178Abfrage(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                eCode178 eCode178 = MyMapFromDtoToeCode178(inDto.ZekeCode178Dto);
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away 
                CommonMultiResponse multiResponse = zekWSDao.eCode178Abfrage(idDesc, eCode178);
                code = codeTechExc;
                // Map WS response to ZekOutDto
                outDto = MyMapCommonMultiResponseToOutDto(multiResponse);
                // Save outDto to database
                zekDBDao.SaveeCode178AbfrageOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178Abfrage!");
                throw new ApplicationException("Unexpected Exception in eCode178Abfrage!", e);
            }
        }

        #endregion

        #region getARMS

        /// <summary>
        /// eCode178Abfrage
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        public override AuskunftDto getARMs(ZekInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKgetARMs);
            try
            {
                // Save KreditgesuchNeu Input
                zekDBDao.SaveGetARMsInput(sysAuskunft, inDto);

                // Get username and password
                idDesc = zekDBDao.GetIdentityDescriptor();
                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                ArmResponse response = zekWSDao.getARMs(idDesc, inDto.DateLastSuccessfullRequest);
                code = codeTechExc;
                outDto = MyMapArmResponseToOutDto(response);
                // Save KreditgesuchNeu Output
                zekDBDao.SaveGetARMsOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.ZekOutDto = outDto;
                auskunftDto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in getARMs!");
                throw new ApplicationException("Unexpected Exception in getARMs!", e);
            }
        }

        /// <summary>
        /// getARMs
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        public override AuskunftDto getARMs(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekDBDao.FindBySysId(auskunftdto.sysAuskunft);
                auskunftdto.ZekInDto = inDto;
                // Get username and password for Zek WS
                idDesc = zekDBDao.GetIdentityDescriptor();

                code = codeSerAufExc;
                //For report
                zekWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                // Send request away
                ArmResponse response = zekWSDao.getARMs(idDesc, inDto.DateLastSuccessfullRequest); 
                code = codeTechExc;
                // Map WS response to ZekOutDto
                outDto = MyMapArmResponseToOutDto(response);
                // Save outDto to database
                zekDBDao.SaveGetARMsOutput(sysAuskunft, outDto);
                // Update Auskunft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.ZekOutDto = outDto;
                auskunftdto.requestXML = zekWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = zekWSDao.getSoapXMLDto().responseXML;
                
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in eCode178bnmelden!");
                throw new ApplicationException("Unexpected Exception in eCode178Abfrage!", e);
            }
        }

        #endregion

        #region MyMethods

        /// <summary>
        /// Maps List of ZekRequestEntityDto to RequestEntity Array
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Array of RequestEntities</returns>
        private RequestEntity MyMapFromDtoToRequestEntity(ZekRequestEntityDto dto)
        {
            RequestEntity entity = new RequestEntity();
            if (dto.DebtorRole != null)
            {
                entity.debtorRole = (int)dto.DebtorRole;
            }
            entity.forceNewAddress = dto.ForceNewAddress;
            entity.previousReturnCode = dto.PreviousReturnCode;
            entity.refno = dto.RefNo;
            if (dto.AddressDescription != null)
            {
                entity.addressDescription = Mapper.Map<ZekAddressDescriptionDto, AddressDescription>(dto.AddressDescription);
                //BNRACHT-1103
                if (entity.addressDescription.name != null)
                {  
                    entity.addressDescription.name = entity.addressDescription.name.TrimEnd();
                    if (entity.addressDescription.name.Length > 36)
                    {
                        entity.addressDescription.name = entity.addressDescription.name.Substring(0, 36);
                        entity.addressDescription.name = entity.addressDescription.name.TrimEnd();
                    }
                 }

                if (entity.addressDescription.firstname != null)
                {
                    entity.addressDescription.firstname = entity.addressDescription.firstname.TrimEnd();
                    if (entity.addressDescription.firstname.Length > 24)
                    {
                        entity.addressDescription.firstname = entity.addressDescription.firstname.Substring(0, 24);
                        entity.addressDescription.firstname = entity.addressDescription.firstname.TrimEnd();
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// Maps CommonMultiResponse to ZEKOutDto
        /// </summary>
        /// <param name="response"></param>
        /// <returns>ZekOutDto</returns>
        private ZekOutDto MyMapCommonMultiResponseToOutDto(CommonMultiResponse response)
        {
            ZekOutDto outDto = new ZekOutDto();
            outDto.KreditgesuchID = response.kreditGesuchID;
            outDto.KreditVertragID = response.kreditVertragID;
            outDto.eCodeId = response.eCodeId;

            if (response.responses != null)
            {
                outDto.Responses = new List<ZekResponseDescriptionDto>();
                foreach (ResponseDescription responseDescription in response.responses)
                {
                    outDto.Responses.Add(MyMapResponseDescriptionToDto(responseDescription));
                }

                if (String.IsNullOrEmpty(outDto.KreditVertragID))
                {
                    outDto.KreditVertragID = MyGetKreditVertragID(outDto.Responses);
                }
            }

            if (response.transactionError != null)
            {
                outDto.TransactionError = Mapper.Map<DAO.Auskunft.ZEKRef.TransactionError, ZekTransactionErrorDto>(response.transactionError);
            }
            return outDto;
        }


        /// <summary>
        /// Holt die kreditVertragID, um sie im OutDto zu speichern, um sie in ZekCmr zu speichern.
        /// </summary>
        /// <param name="responseDtos"></param>
        /// <returns></returns>
        private string MyGetKreditVertragID(List<ZekResponseDescriptionDto> responseDtos)
        {
            string kreditVertragID = String.Empty;

            // if (responseDtos != null)
            foreach (var response in responseDtos)
            {
                if (response.FoundContracts != null)
                {
                    if (response.FoundContracts.BardarlehenContracts != null)
                    {
                        return response.FoundContracts.BardarlehenContracts[0].kreditVertragID;
                    }
                    if (response.FoundContracts.FestkreditContracts != null)
                    {
                        return response.FoundContracts.FestkreditContracts[0].KreditVertragID;
                    }
                    if (response.FoundContracts.KartenengagementContracts != null)
                    {
                        return response.FoundContracts.KartenengagementContracts[0].KreditVertragID;
                    }
                    if (response.FoundContracts.KontokorrentkreditContracts != null)
                    {
                        return response.FoundContracts.KontokorrentkreditContracts[0].kreditVertragID;
                    }
                    if (response.FoundContracts.KreditgesuchContracts != null)
                    {
                        return response.FoundContracts.KreditgesuchContracts[0].KreditVertragID;
                    }
                    if (response.FoundContracts.LeasingMietvertragContracts != null)
                    {
                        return response.FoundContracts.LeasingMietvertragContracts[0].KreditVertragID;
                    }
                    if (response.FoundContracts.SolidarschuldnerContracts != null)
                    {
                        return response.FoundContracts.SolidarschuldnerContracts[0].KreditVertragID;
                    }
                    if (response.FoundContracts.TeilzahlungskreditContracts != null)
                    {
                        return response.FoundContracts.TeilzahlungskreditContracts[0].KreditVertragID;
                    }
                    if (response.FoundContracts.UeberziehnungskreditContracts != null)
                    {
                        return response.FoundContracts.UeberziehnungskreditContracts[0].KreditVertragID;
                    }
                }
            }
            return kreditVertragID;
        }


        /// <summary>
        /// Maps InfoResponse to ZekOutDto
        /// </summary>
        /// <param name="response"></param>
        /// <returns>ZekOutDto</returns>
        private ZekOutDto MyMapInfoResponseToOutDto(InfoResponse response)
        {
            ZekOutDto outDto = new ZekOutDto();

            if (response.returnCode != null)
            {
                outDto.ReturnCode = Mapper.Map<DAO.Auskunft.ZEKRef.ReturnCode, ZekReturnCodeDto>(response.returnCode);
            }
            if (response.foundPerson != null)
            {
                outDto.FoundPerson = Mapper.Map<AddressDescription, ZekAddressDescriptionDto>(response.foundPerson);
            }
            if (response.synonymes != null)
            {
                outDto.Synonyms = new List<ZekAddressDescriptionDto>();
                foreach (AddressDescription address in response.synonymes)
                {
                    outDto.Synonyms.Add(Mapper.Map<AddressDescription, ZekAddressDescriptionDto>(address));
                }
            }
            if (response.foundContracts != null)
            {
                outDto.FoundContracts = MyMapFoundContractsToDto(response.foundContracts);
            }
            if (response.transactionError != null)
            {
                outDto.TransactionError = Mapper.Map<DAO.Auskunft.ZEKRef.TransactionError, ZekTransactionErrorDto>(response.transactionError);
            }
            return outDto;
        }

        /// <summary>
        /// Maps CreditClaimRejectionResponse to ZekOutDto
        /// </summary>
        /// <param name="response"></param>
        /// <returns>ZekOutDto</returns>
        private ZekOutDto MyMapCreditClaimRejectionResponseToOutDto(CreditClaimRejectionResponse response)
        {
            ZekOutDto outDto = new ZekOutDto();
            if (response.returnCode != null)
            {
                outDto.ReturnCode = Mapper.Map<DAO.Auskunft.ZEKRef.ReturnCode, ZekReturnCodeDto>(response.returnCode);
            }
            if (response.transactionError != null)
            {
                outDto.TransactionError = Mapper.Map<DAO.Auskunft.ZEKRef.TransactionError, ZekTransactionErrorDto>(response.transactionError);
            }
            if (response.synonymes != null)
            {
                outDto.Synonyms = new List<ZekAddressDescriptionDto>();
                foreach (AddressDescription address in response.synonymes)
                {
                    outDto.Synonyms.Add(Mapper.Map<AddressDescription, ZekAddressDescriptionDto>(address));
                }
            }
            return outDto;
        }

        /// <summary>
        /// Maps UpdateAddressResponse to ZekOutDto
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private ZekOutDto MyMapUpdateAddressResponseToOutDto(UpdateAddressResponse response)
        {
            ZekOutDto outDto = new ZekOutDto();
            if (response.returnCode != null)
            {
                outDto.ReturnCode = Mapper.Map<DAO.Auskunft.ZEKRef.ReturnCode, ZekReturnCodeDto>(response.returnCode);
            }
            if (response.transactionError != null)
            {
                outDto.TransactionError = Mapper.Map<DAO.Auskunft.ZEKRef.TransactionError, ZekTransactionErrorDto>(response.transactionError);
            }
            if (response.synonymes != null)
            {
                outDto.Synonyms = new List<ZekAddressDescriptionDto>();
                foreach (AddressDescription address in response.synonymes)
                {
                    outDto.Synonyms.Add(Mapper.Map<AddressDescription, ZekAddressDescriptionDto>(address));
                }
            }
            if (response.synonymesNew != null)
            {
                outDto.SynonymsNew = new List<ZekAddressDescriptionDto>();
                foreach (AddressDescription address in response.synonymesNew)
                {
                    outDto.SynonymsNew.Add(Mapper.Map<AddressDescription, ZekAddressDescriptionDto>(address));
                }
            }
            return outDto;
        }

        /// <summary>
        /// Maps ResponseDescription to ZekResponseDescritptionDto
        /// </summary>
        /// <param name="response"></param>
        /// <returns>ZekResponseDescriptionDto</returns>
        private ZekResponseDescriptionDto MyMapResponseDescriptionToDto(ResponseDescription response)
        {
            ZekResponseDescriptionDto dto = new ZekResponseDescriptionDto();
            dto.RefNo = response.refNo;

            // FOUND PERSON
            if (response.foundPerson != null)
            {
                dto.FoundPerson = Mapper.Map<AddressDescription, ZekAddressDescriptionDto>(response.foundPerson);
            }

            // SYNONYMS
            if (response.synonymes != null)
            {
                dto.Synonyms = new List<ZekAddressDescriptionDto>();
                foreach (AddressDescription address in response.synonymes)
                {
                    dto.Synonyms.Add(Mapper.Map<AddressDescription, ZekAddressDescriptionDto>(address));
                }
            }

            // RETURNCODE
            if (response.returnCode != null)
            {
                dto.ReturnCode = Mapper.Map<DAO.Auskunft.ZEKRef.ReturnCode, ZekReturnCodeDto>(response.returnCode);
            }

            // FOUND CONTRACTS
            if (response.foundContracts != null)
            {
                dto.FoundContracts = MyMapFoundContractsToDto(response.foundContracts);
            }
            return dto;
        }

        /// <summary>
        /// Maps FoundContracts to ZekFoundContractsDto
        /// </summary>
        /// <param name="foundContracts"></param>
        /// <returns>ZekFoundContractsDto</returns>
        private ZekFoundContractsDto MyMapFoundContractsToDto(FoundContracts foundContracts)
        {
            ZekFoundContractsDto dto = new ZekFoundContractsDto();
            dto.GesamtEngagement = foundContracts.gesamtEngagement;
            if (foundContracts.amtsinformationContracts != null)
            {
                dto.AmtsinformationContracts = new List<ZekAmtsinformationDescriptionDto>();

                foreach (AmtsinformationDescription description in foundContracts.amtsinformationContracts)
                {
                    dto.AmtsinformationContracts.Add(Mapper.Map<AmtsinformationDescription, ZekAmtsinformationDescriptionDto>(description));
                }
            }
            if (foundContracts.bardarlehenContracts != null)
            {
                dto.BardarlehenContracts = new List<ZekBardarlehenDescriptionDto>();

                foreach (DAO.Auskunft.ZEKRef.BardarlehenDescription description in foundContracts.bardarlehenContracts)
                {
                    dto.BardarlehenContracts.Add(Mapper.Map<DAO.Auskunft.ZEKRef.BardarlehenDescription, ZekBardarlehenDescriptionDto>(description));
                }
            }
            if (foundContracts.festkreditContracts != null)
            {
                dto.FestkreditContracts = new List<ZekFestkreditDescriptionDto>();

                foreach (DAO.Auskunft.ZEKRef.Festkredit description in foundContracts.festkreditContracts)
                {
                    dto.FestkreditContracts.Add(Mapper.Map<DAO.Auskunft.ZEKRef.Festkredit, ZekFestkreditDescriptionDto>(description));
                }
            }
            if (foundContracts.kartenengagementContracts != null)
            {
                dto.KartenengagementContracts = new List<ZekKartenengagementDescriptionDto>();

                foreach (KartenengagementDescription description in foundContracts.kartenengagementContracts)
                {
                    dto.KartenengagementContracts.Add(Mapper.Map<KartenengagementDescription, ZekKartenengagementDescriptionDto>(description));
                }
            }
            if (foundContracts.karteninformationContracts != null)
            {
                dto.KarteninformationContracts = new List<ZekKarteninformationDescriptionDto>();

                foreach (KarteninformationDescription description in foundContracts.karteninformationContracts)
                {
                    dto.KarteninformationContracts.Add(Mapper.Map<KarteninformationDescription, ZekKarteninformationDescriptionDto>(description));
                }
            }
            if (foundContracts.kontokorrentkreditContracts != null)
            {
                dto.KontokorrentkreditContracts = new List<ZekKontokorrentkreditDescriptionDto>();

                foreach (DAO.Auskunft.ZEKRef.KontokorrentkreditDescription description in foundContracts.kontokorrentkreditContracts)
                {
                    dto.KontokorrentkreditContracts.Add(Mapper.Map<DAO.Auskunft.ZEKRef.KontokorrentkreditDescription, ZekKontokorrentkreditDescriptionDto>(description));
                }
            }
            if (foundContracts.kreditGesuchContracts != null)
            {
                dto.KreditgesuchContracts = new List<ZekKreditgesuchDescriptionDto>();

                foreach (KreditGesuchDescription description in foundContracts.kreditGesuchContracts)
                {
                    dto.KreditgesuchContracts.Add(Mapper.Map<KreditGesuchDescription, ZekKreditgesuchDescriptionDto>(description));
                }
            }
            if (foundContracts.leasingMietvertragContracts != null)
            {
                dto.LeasingMietvertragContracts = new List<ZekLeasingMietvertragDescriptionDto>();

                foreach (DAO.Auskunft.ZEKRef.LeasingMietvertragDescription description in foundContracts.leasingMietvertragContracts)
                {
                    dto.LeasingMietvertragContracts.Add(Mapper.Map<DAO.Auskunft.ZEKRef.LeasingMietvertragDescription, ZekLeasingMietvertragDescriptionDto>(description));
                }
            }
            if (foundContracts.solidarschuldnerContracts != null)
            {
                dto.SolidarschuldnerContracts = new List<ZekSolidarschuldnerDescriptionDto>();

                foreach (SolidarschuldnerDescription description in foundContracts.solidarschuldnerContracts)
                {
                    dto.SolidarschuldnerContracts.Add(Mapper.Map<SolidarschuldnerDescription, ZekSolidarschuldnerDescriptionDto>(description));
                }
            }
            if (foundContracts.teilzahlungskreditContracts != null)
            {
                dto.TeilzahlungskreditContracts = new List<ZekTeilzahlungskreditDescriptionDto>();

                foreach (DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription description in foundContracts.teilzahlungskreditContracts)
                {
                    dto.TeilzahlungskreditContracts.Add(Mapper.Map<DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription, ZekTeilzahlungskreditDescriptionDto>(description));
                }
            }
            if (foundContracts.teilzahlungsvertragContracts != null)
            {
                dto.TeilzahlungsvertragContracts = new List<ZekTeilzahlungsvertragDescriptionDto>();

                foreach (DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription description in foundContracts.teilzahlungsvertragContracts)
                {
                    dto.TeilzahlungsvertragContracts.Add(Mapper.Map<DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription, ZekTeilzahlungsvertragDescriptionDto>(description));
                }
            }
            if (foundContracts.ueberziehungskreditContracts != null)
            {
                dto.UeberziehnungskreditContracts = new List<ZekUeberziehungskreditDescriptionDto>();

                foreach (UeberziehungskreditDescription description in foundContracts.ueberziehungskreditContracts)
                {
                    dto.UeberziehnungskreditContracts.Add(Mapper.Map<UeberziehungskreditDescription, ZekUeberziehungskreditDescriptionDto>(description));
                }
            }
            if (foundContracts.eCodes178 != null)
            {
                dto.ECode178Contracts = new List<ZekeCode178Dto>();

                foreach (eCode178 description in foundContracts.eCodes178)
                {
                    dto.ECode178Contracts.Add(Mapper.Map<eCode178, ZekeCode178Dto>(description));
                }
            }
            return dto;
        }

        /// <summary>
        /// Maps ZekBardarlehenDescriptionDto to BardarlehenDescription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DAO.Auskunft.ZEKRef.BardarlehenDescription MyMapFromDtoToBardarlehen(ZekBardarlehenDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.BardarlehenDescription bd = Mapper.Map<ZekBardarlehenDescriptionDto, DAO.Auskunft.ZEKRef.BardarlehenDescription>(dto);
            return bd;
        }

        /// <summary>
        /// Maps ZekFestkreditDescriptionDto to Festkredit
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DAO.Auskunft.ZEKRef.Festkredit MyMapFromDtoToFestkredit(ZekFestkreditDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.Festkredit kredit = Mapper.Map<ZekFestkreditDescriptionDto, DAO.Auskunft.ZEKRef.Festkredit>(dto);
            return kredit;
        }

        /// <summary>
        /// Maps ZekKontokorrentkreditDescriptionDto to KontokorrentkreditDescription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DAO.Auskunft.ZEKRef.KontokorrentkreditDescription MyMapFromDtoToKontokorrentkredit(ZekKontokorrentkreditDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.KontokorrentkreditDescription kredit = Mapper.Map<ZekKontokorrentkreditDescriptionDto, DAO.Auskunft.ZEKRef.KontokorrentkreditDescription>(dto);
            return kredit;
        }

        /// <summary>
        /// Maps ZekLeasingMietvertragDescriptionDto to LeasingMietvertragDescription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DAO.Auskunft.ZEKRef.LeasingMietvertragDescription MyMapFromDtoToLeasingMietvertrag(ZekLeasingMietvertragDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.LeasingMietvertragDescription kredit = Mapper.Map<ZekLeasingMietvertragDescriptionDto, DAO.Auskunft.ZEKRef.LeasingMietvertragDescription>(dto);
            return kredit;
        }

        /// <summary>
        /// Maps ZekTeilzahlungskreditDescriptionDto to TeilzahlungskreditDescription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription MyMapFromDtoToTeilzahlungskredit(ZekTeilzahlungskreditDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription kredit = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, DAO.Auskunft.ZEKRef.TeilzahlungskreditDescription>(dto);
            return kredit;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription MyMapFromDtoToTeilzahlungsvertrag(ZekTeilzahlungsvertragDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = Mapper.Map<ZekTeilzahlungsvertragDescriptionDto, DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription>(dto);
            return vertrag;
        }

        private DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription MyMapFromDtoToTeilzahlungsvertrag(ZekTeilzahlungskreditDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription vertrag = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, DAO.Auskunft.ZEKRef.TeilzahlungsvertragDescription>(dto);
            return vertrag;
        }

        /// <summary>
        /// Maps ZekKartenengagementDescriptionDto to KartenengagementDescription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DAO.Auskunft.ZEKRef.KartenengagementDescription MyMapFromDtoToKartenengagement(ZekKartenengagementDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.KartenengagementDescription kredit = Mapper.Map<ZekKartenengagementDescriptionDto, DAO.Auskunft.ZEKRef.KartenengagementDescription>(dto);
            return kredit;
        }

        /// <summary>
        /// Maps ZekUeberziehungskreditDescriptionDto to UeberziehungskreditDescription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DAO.Auskunft.ZEKRef.UeberziehungskreditDescription MyMapFromDtoToUeberziehungskredit(ZekUeberziehungskreditDescriptionDto dto)
        {
            DAO.Auskunft.ZEKRef.UeberziehungskreditDescription kredit = Mapper.Map<ZekUeberziehungskreditDescriptionDto, DAO.Auskunft.ZEKRef.UeberziehungskreditDescription>(dto);
            return kredit;
        }

        /// <summary>
        /// MyMapFromDtoToeCode178
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private eCode178 MyMapFromDtoToeCode178(ZekeCode178Dto dto)
        {
            DAO.Auskunft.ZEKRef.eCode178 eCode178 = Mapper.Map<ZekeCode178Dto, DAO.Auskunft.ZEKRef.eCode178>(dto);
            string ecodeid = dto.Ecode178id;

            if (ecodeid != null)
            {
                ecodeid = ecodeid.Trim();
                if (ecodeid.Length > 12)
                {
                    ecodeid = ecodeid.Substring(0, 12);
                }
            }
            eCode178.eCode178Id = ecodeid;

            return eCode178;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// MyMapArmResponseToOutDto
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private ZekOutDto MyMapArmResponseToOutDto(ArmResponse response)
        {
            ZekOutDto outDto = new ZekOutDto();
            if (response.returnCode != null)
            {
                outDto.ReturnCode = Mapper.Map<DAO.Auskunft.ZEKRef.ReturnCode, ZekReturnCodeDto>(response.returnCode);
            }
            if (response.transactionError != null)
            {
                outDto.TransactionError = Mapper.Map<DAO.Auskunft.ZEKRef.TransactionError, ZekTransactionErrorDto>(response.transactionError);
            }
            outDto.armResponse = new ZekArmResponseDto();
            outDto.armResponse.requestDate = response.requestDate;

            if (response.armList != null)
            {
                outDto.armResponse.armList = new List<ZekArmItemDto>();
                foreach (ArmItem item in response.armList)
                {
                    outDto.armResponse.armList.Add(Mapper.Map<ArmItem, ZekArmItemDto>(item));
                }
            }
            return outDto;
        }

      

        /// <summary>
        /// MyMapZekOLOutDto
        /// </summary>
        /// <param name="zekOutDto"></param>
        /// <returns></returns>
        private ZekOLOutDto MyMapZekOLOutDto(ZekOutDto zekOutDto)
        {
            ZekOLOutDto output = new ZekOLOutDto();
            ZekOLOutDto zekOLOutDto = new ZekOLOutDto();
            zekOLOutDto = Mapper.Map<ZekOutDto, ZekOLOutDto>(zekOutDto);
            if (zekOutDto.FoundContracts != null)
            { 
                    ZekOLFoundContractsDto foundOLContracts = new ZekOLFoundContractsDto();
                    zekOLOutDto.FoundContracts = foundOLContracts;

                    if (zekOLOutDto.FoundContracts != null)
                    {
                        zekOLOutDto.FoundContracts.GesamtEngagement = zekOutDto.FoundContracts.GesamtEngagement;
                    }

                if (zekOutDto.FoundContracts.AmtsinformationContracts != null)
                    {
                        List<ZekOLAmtsinformationDescriptionDto> AmtsinformationContracts = new List<ZekOLAmtsinformationDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.AmtsinformationContracts)
                        {
                            ZekOLAmtsinformationDescriptionDto amtsinformationContract = new ZekOLAmtsinformationDescriptionDto();
                            amtsinformationContract = Mapper.Map<ZekAmtsinformationDescriptionDto,ZekOLAmtsinformationDescriptionDto>(contract);
                            amtsinformationContract.artOfContract = ArtOfContract.amtsinformationContracts;
                            AmtsinformationContracts.Add(amtsinformationContract);
                        }
                        zekOLOutDto.FoundContracts.AmtsinformationContracts = AmtsinformationContracts;

                    }
                    if (zekOutDto.FoundContracts.BardarlehenContracts != null)
                    {
                        List<ZekOLBardarlehenDescriptionDto> BardarlehenContracts = new List<ZekOLBardarlehenDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.BardarlehenContracts)
                        {
                            ZekOLBardarlehenDescriptionDto bardarlehenContract = new ZekOLBardarlehenDescriptionDto();
                            bardarlehenContract = Mapper.Map<ZekBardarlehenDescriptionDto, ZekOLBardarlehenDescriptionDto>(contract);
                            bardarlehenContract.artOfContract = ArtOfContract.bardarlehenContracts;
                            BardarlehenContracts.Add(bardarlehenContract);
                        }
                        zekOLOutDto.FoundContracts.BardarlehenContracts = BardarlehenContracts;
                    }
                    if (zekOutDto.FoundContracts.FestkreditContracts != null)
                    {
                        List<ZekOLFestkreditDescriptionDto> FestkreditContracts = new List<ZekOLFestkreditDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.FestkreditContracts)
                        {
                            ZekOLFestkreditDescriptionDto festkreditContract = new ZekOLFestkreditDescriptionDto();
                            festkreditContract = Mapper.Map<ZekFestkreditDescriptionDto, ZekOLFestkreditDescriptionDto>(contract);
                            festkreditContract.artOfContract = ArtOfContract.festkreditContracts;
                            FestkreditContracts.Add(festkreditContract);
                   
                        }
                        zekOLOutDto.FoundContracts.FestkreditContracts = FestkreditContracts;
                    }
                    if (zekOutDto.FoundContracts.KartenengagementContracts != null)
                    {
                        List<ZekOLKartenengagementDescriptionDto> KartenengagementContracts = new List<ZekOLKartenengagementDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.KartenengagementContracts)
                        {
                            ZekOLKartenengagementDescriptionDto kartenengagementContract = new ZekOLKartenengagementDescriptionDto();
                            kartenengagementContract = Mapper.Map<ZekKartenengagementDescriptionDto, ZekOLKartenengagementDescriptionDto>(contract);
                            kartenengagementContract.artOfContract = ArtOfContract.kartenengagementContracts;
                            KartenengagementContracts.Add(kartenengagementContract);
                        }
                        zekOLOutDto.FoundContracts.KartenengagementContracts = KartenengagementContracts;
                    }
                    if (zekOutDto.FoundContracts.KarteninformationContracts != null)
                    {
                        List<ZekOLKarteninformationDescriptionDto> KarteninformationContracts = new List<ZekOLKarteninformationDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.KarteninformationContracts)
                        {
                            ZekOLKarteninformationDescriptionDto karteninformationContract = new ZekOLKarteninformationDescriptionDto();
                            karteninformationContract = Mapper.Map<ZekKarteninformationDescriptionDto, ZekOLKarteninformationDescriptionDto>(contract);
                            karteninformationContract.artOfContract = ArtOfContract.karteninformationContracts;
                            KarteninformationContracts.Add(karteninformationContract);
                        }
                        zekOLOutDto.FoundContracts.KarteninformationContracts = KarteninformationContracts;
                    }
                    if (zekOutDto.FoundContracts.KontokorrentkreditContracts != null)
                    {
                        List<ZekOLKontokorrentkreditDescriptionDto> KontokorrentkreditContracts = new List<ZekOLKontokorrentkreditDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.KontokorrentkreditContracts)
                        {
                            ZekOLKontokorrentkreditDescriptionDto kontokorrentkreditContract = new ZekOLKontokorrentkreditDescriptionDto();
                            kontokorrentkreditContract = Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZekOLKontokorrentkreditDescriptionDto>(contract);
                            kontokorrentkreditContract.artOfContract = ArtOfContract.kontokorrentkreditContracts;
                            KontokorrentkreditContracts.Add(kontokorrentkreditContract);
                        }
                        zekOLOutDto.FoundContracts.KontokorrentkreditContracts = KontokorrentkreditContracts;
                    }
                    if (zekOutDto.FoundContracts.KreditgesuchContracts!= null)
                    {
                        List<ZekOLKreditgesuchDescriptionDto> KreditgesuchContracts = new List<ZekOLKreditgesuchDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.KreditgesuchContracts)
                        {
                            ZekOLKreditgesuchDescriptionDto kreditgesuchContract = new ZekOLKreditgesuchDescriptionDto();
                            kreditgesuchContract = Mapper.Map<ZekKreditgesuchDescriptionDto, ZekOLKreditgesuchDescriptionDto>(contract);
                            kreditgesuchContract.artOfContract = ArtOfContract.kreditGesuchContracts;
                            KreditgesuchContracts.Add(kreditgesuchContract);
                        }
                        zekOLOutDto.FoundContracts.KreditgesuchContracts = KreditgesuchContracts;
                    }
                    if (zekOutDto.FoundContracts.LeasingMietvertragContracts != null)
                    {
                        List<ZekOLLeasingMietvertragDescriptionDto> LeasingMietvertragContracts = new List<ZekOLLeasingMietvertragDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.LeasingMietvertragContracts)
                        {
                            ZekOLLeasingMietvertragDescriptionDto leasingMietvertragContract = new ZekOLLeasingMietvertragDescriptionDto();
                            leasingMietvertragContract = Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZekOLLeasingMietvertragDescriptionDto>(contract);
                            leasingMietvertragContract.artOfContract = ArtOfContract.amtsinformationContracts;
                            LeasingMietvertragContracts.Add(leasingMietvertragContract);
                        } 
                        zekOLOutDto.FoundContracts.LeasingMietvertragContracts = LeasingMietvertragContracts;
                    }
                    if (zekOutDto.FoundContracts.SolidarschuldnerContracts != null)
                    { 
                        List<ZekOLSolidarschuldnerDescriptionDto> SolidarschuldnerContracts = new List<ZekOLSolidarschuldnerDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.SolidarschuldnerContracts)
                        {
                            ZekOLSolidarschuldnerDescriptionDto solidarschuldnerContract = new ZekOLSolidarschuldnerDescriptionDto();
                            solidarschuldnerContract = Mapper.Map<ZekSolidarschuldnerDescriptionDto, ZekOLSolidarschuldnerDescriptionDto>(contract);
                            solidarschuldnerContract.artOfContract = ArtOfContract.amtsinformationContracts;
                            SolidarschuldnerContracts.Add(solidarschuldnerContract);
                        }
                        zekOLOutDto.FoundContracts.SolidarschuldnerContracts = SolidarschuldnerContracts;
                    }
                    if (zekOutDto.FoundContracts.TeilzahlungskreditContracts != null)
                    {
                        List<ZekOLTeilzahlungskreditDescriptionDto> TeilzahlungskreditContracts = new List<ZekOLTeilzahlungskreditDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.TeilzahlungskreditContracts)
                        {
                            ZekOLTeilzahlungskreditDescriptionDto teilzahlungskreditContract = new ZekOLTeilzahlungskreditDescriptionDto();
                            teilzahlungskreditContract = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZekOLTeilzahlungskreditDescriptionDto>(contract);
                            teilzahlungskreditContract.artOfContract = ArtOfContract.teilzahlungskreditContracts;
                            TeilzahlungskreditContracts.Add(teilzahlungskreditContract); 
                        }
                        zekOLOutDto.FoundContracts.TeilzahlungskreditContracts = TeilzahlungskreditContracts;
                    }
                    if (zekOutDto.FoundContracts.TeilzahlungsvertragContracts != null)
                    {
                        List<ZekOLTeilzahlungsvertragDescriptionDto> TeilzahlungsvertragContracts = new List<ZekOLTeilzahlungsvertragDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.TeilzahlungsvertragContracts)
                        {
                            ZekOLTeilzahlungsvertragDescriptionDto teilzahlungsvertragContract = new ZekOLTeilzahlungsvertragDescriptionDto();
                            teilzahlungsvertragContract = Mapper.Map<ZekTeilzahlungsvertragDescriptionDto, ZekOLTeilzahlungsvertragDescriptionDto>(contract);
                            teilzahlungsvertragContract.artOfContract = ArtOfContract.teilzahlungskreditContracts;
                            TeilzahlungsvertragContracts.Add(teilzahlungsvertragContract);
                        }
                        zekOLOutDto.FoundContracts.TeilzahlungsvertragContracts = TeilzahlungsvertragContracts;
                    }
                    if (zekOutDto.FoundContracts.UeberziehnungskreditContracts != null)
                    {
                
                        List<ZekOLUeberziehungskreditDescriptionDto> UeberziehnungskreditContracts = new List<ZekOLUeberziehungskreditDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.UeberziehnungskreditContracts)
                        {
                            ZekOLUeberziehungskreditDescriptionDto ueberziehnungskreditContract = new ZekOLUeberziehungskreditDescriptionDto();
                            ueberziehnungskreditContract = Mapper.Map<ZekUeberziehungskreditDescriptionDto, ZekOLUeberziehungskreditDescriptionDto>(contract);
                            ueberziehnungskreditContract.artOfContract = ArtOfContract.ueberziehungskreditContracts;
                            UeberziehnungskreditContracts.Add(ueberziehnungskreditContract);
                        }
                        zekOLOutDto.FoundContracts.UeberziehnungskreditContracts = UeberziehnungskreditContracts;   
                    }

                    if (zekOutDto.FoundContracts.ECode178Contracts != null)
                    {

                        List<ZekOLeCode178DtoDescriptionDto> eCode178DtoContracts = new List<ZekOLeCode178DtoDescriptionDto>();
                        foreach (var contract in zekOutDto.FoundContracts.ECode178Contracts)
                        {
                            ZekOLeCode178DtoDescriptionDto  eCode178DtokreditContract = new ZekOLeCode178DtoDescriptionDto ();
                            eCode178DtokreditContract = Mapper.Map<ZekeCode178Dto,ZekOLeCode178DtoDescriptionDto>(contract);
                            eCode178DtokreditContract.artOfContract = ArtOfContract.eCode178DtoContracts;
                            eCode178DtoContracts.Add(eCode178DtokreditContract);
                        }
                        zekOLOutDto.FoundContracts.ECode178Contracts = eCode178DtoContracts;
                    }
            }
            return zekOLOutDto;
        }
        #endregion

    }
}