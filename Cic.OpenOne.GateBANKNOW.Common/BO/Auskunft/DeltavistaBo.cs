using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DeltavistaRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstract Class: Deltavista Business Object
    /// </summary>
    public class DeltavistaBo : AbstractDeltavistaBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const long codeTechExc = -2;
        private const long codeSerAufExc = -1;
        IdentityDescriptor idDesc;
        DAO.Auskunft.DeltavistaRef2.IdentityDescriptor idDescRef2;
        DeltavistaOutDto outDto;

        /// <summary>
        /// Constructore 
        /// </summary>
        /// <param name="dvWSDao">Deltavista Web Service Data Access Object</param>
        /// <param name="dvDBDao">Deltavista DB Data Access Object</param>
        /// <param name="auskunftDao">Information Data Access Object</param>
        public DeltavistaBo(IDeltavistaWSDao dvWSDao, IDeltavistaDBDao dvDBDao, IAuskunftDao auskunftDao)
            : base(dvWSDao, dvDBDao, auskunftDao)
        {
        }

        /// <summary>
        /// Calls DeltavistaWSDao method getAddressId
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>DeltavistaOutDto </returns>
        public override AuskunftDto getIdentifiedAddress(DeltavistaInDto inDto)
        {
            long code = codeTechExc;

            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.DeltavistaGetAddressId);
            try
            {
                idDesc = dvDBDao.GetIdentityDescriptor();
                AddressDescription addressDesc = MyMapDtoToAddressDescription(inDto.AddressDescription);

                // Save Input
                dvDBDao.SaveGetIdentifiedAddressInput(sysAuskunft, inDto);
                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                AddressIdentificationResponse addrIdent = dvWSDao.getAddressId(idDesc, addressDesc, inDto.AddressDescription.UIDNummer);

                string responseXML = Cic.OpenOne.Common.Util.Serialization.XMLSerializer.SerializeUTF8WithoutNamespace(addrIdent);

                code = codeTechExc;
                outDto = MyMapAddressIdentificationToOutDto(addrIdent);

                // Save Output
                dvDBDao.SaveGetIdentifiedAddressOutput(sysAuskunft, outDto);

                // Update Auskuft
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
                auskunftDto.DeltavistaOutDto = outDto;
                auskunftDto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = dvWSDao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getIdentifiedAddress Webservice!", e);
            }
        }

        /// <summary>
        /// Calls DeltavistaWSDao method getAddressId
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>DeltavistaOutDto </returns>
        public override AuskunftDto getIdentifiedAddressArb(DeltavistaInDto inDto)
        {
            long code = codeTechExc;

            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.DeltavistaGetAddressIdArb);
            try
            {
                idDesc = dvDBDao.GetIdentityDescriptorArb();
                AddressDescription addressDesc = MyMapDtoToAddressDescription(inDto.AddressDescription);

                // Save Input
                dvDBDao.SaveGetIdentifiedAddressInput(sysAuskunft, inDto);

                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                AddressIdentificationResponse addrIdent = dvWSDao.getAddressId(idDesc, addressDesc, inDto.AddressDescription.UIDNummer);

                code = codeTechExc;
                outDto = MyMapAddressIdentificationToOutDto(addrIdent);

                // Save Output
                dvDBDao.SaveGetIdentifiedAddressOutput(sysAuskunft, outDto);

                // Update Auskuft
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
                auskunftDto.DeltavistaOutDto = outDto;

                auskunftDto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getIdentifiedAddressArb Webservice!", e);
            }
        }

        /// <summary>
        /// Calls DeltavistaWSDao method getCompanyDetailsByAddressId
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto getCompanyDetailsByAddressId(DeltavistaInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.DeltavistaGetCompanyDetails);
            try
            {
                idDesc = dvDBDao.GetIdentityDescriptor();

                // Save Input
                dvDBDao.SaveGetCompanyDetailsInput(sysAuskunft, inDto);

                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CompanyDetailsResponse compResp = dvWSDao.getCompanyDetailsByAddressId(idDesc, inDto.AddressId);

                // For report
                string responseXML = Cic.OpenOne.Common.Util.Serialization.XMLSerializer.SerializeUTF8WithoutNamespace(compResp);
                code = codeTechExc;

                outDto = MyMapCompanyDetailsToOutDto(compResp);

                

                // Update Auskuft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
				if(code==0 && outDto!=null && outDto.HqAddress!=null)
                    // Save Output                 
				{
				    this.dvDBDao.SaveGetCompanyDetailsOutput(sysAuskunft, this.outDto);
				}

                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.DeltavistaOutDto = outDto;
                auskunftDto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getCompanyDetailsByAddressId Webservice!", e);
            }
        }

        /// <summary>
        /// Calls DeltavistaWSDao method getDebtDetailsByAddressId
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto getDebtDetailsByAddressId(DeltavistaInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.DeltavistaGetDebtDetails);
            try
            {
                idDesc = dvDBDao.GetIdentityDescriptor();

                // Save Input
                dvDBDao.SaveGetDebtDetailsInput(sysAuskunft, inDto);
                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                DebtDetailsResponse debtResp = dvWSDao.getDebtDetailsByAddressId(idDesc, inDto.AddressId);

                // For report
                string responseXML = Cic.OpenOne.Common.Util.Serialization.XMLSerializer.SerializeUTF8WithoutNamespace(debtResp);

                code = codeTechExc;
                outDto = MyMapDebtDetailsToOutDto(debtResp);

                // Save Output
                dvDBDao.SaveGetDebtDetailsOutput(sysAuskunft, outDto);

                // Update Auskuft
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
                auskunftDto.DeltavistaOutDto = outDto;
                auskunftDto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getDebtDetailsByAddressId!", e);
            }
        }

        /// <summary>
        /// Order Cresura Report
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto orderCresuraReport(long sysAuskunft)
        {
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Get DeltavistaInDto
                DeltavistaInDto DVInDto = dvDBDao.FindBySysId(sysAuskunft);
                auskunftdto.DeltavistaInDto = DVInDto;

                DAO.Auskunft.DeltavistaRef2.AddressDescription Ref2AddrDesc = Mapper.Map<DVAddressDescriptionDto, DAO.Auskunft.DeltavistaRef2.AddressDescription>(DVInDto.AddressDescription);
                DAO.Auskunft.DeltavistaRef2.OrderDescriptor Ref2OrderDesc = Mapper.Map<DVOrderDescriptionDto, DAO.Auskunft.DeltavistaRef2.OrderDescriptor>(DVInDto.OrderDescription);

                idDescRef2 = dvDBDao.GetIdDescriptor();
                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                DAO.Auskunft.DeltavistaRef2.CresuraReportResponse reportResp = dvWSDao.orderCresuraReport(idDescRef2, Ref2AddrDesc, Ref2OrderDesc, DVInDto.RefNo,
                                                                                                          DVInDto.Reason, DVInDto.ContactEmail, DVInDto.ContactFaxNr, DVInDto.ContactName,
                                                                                                          DVInDto.ContactTelDirect, DVInDto.BinaryPOI, DVInDto.BinaryPOItype);

                // For report
                string responseXML = Cic.OpenOne.Common.Util.Serialization.XMLSerializer.SerializeUTF8WithoutNamespace(reportResp);
                code = codeTechExc;

                outDto = new DeltavistaOutDto();
                outDto.ReferenceNumber = reportResp.referenceNumber;
                if (reportResp.transactionError != null)
                {
                    outDto.TransactionError = new DVTransactionErrorDto();
                    outDto.TransactionError.Code = reportResp.transactionError.code;
                    outDto.TransactionError.Text = reportResp.transactionError.text;
                }

                auskunftdto.DeltavistaOutDto = outDto;
                auskunftdto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                // Save Output
                dvDBDao.SaveOrderCresuraReportOutput(sysAuskunft, outDto);

                // Update Auskuft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);

                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in orderCresuraReport!", e);
            }
        }

        /// <summary>
        /// Order Cresura Report
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto orderCresuraReport(DeltavistaInDto inDto)
        {
            long code = codeTechExc;

            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.DeltavistaOrderCresuraReport);
            try
            {
                DAO.Auskunft.DeltavistaRef2.IdentityDescriptor idDesc = dvDBDao.GetIdDescriptor();
                DAO.Auskunft.DeltavistaRef2.AddressDescription addressDesc = Mapper.Map<DVAddressDescriptionDto, DAO.Auskunft.DeltavistaRef2.AddressDescription>(inDto.AddressDescription);
                DAO.Auskunft.DeltavistaRef2.OrderDescriptor orderDesc = Mapper.Map<DVOrderDescriptionDto, DAO.Auskunft.DeltavistaRef2.OrderDescriptor>(inDto.OrderDescription);

                // Save Input
                dvDBDao.SaveOrderCresuraReportInput(sysAuskunft, inDto);
                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                DAO.Auskunft.DeltavistaRef2.CresuraReportResponse reportResp = dvWSDao.orderCresuraReport(idDesc, addressDesc, orderDesc, inDto.RefNo,
                                                                                                          inDto.Reason, inDto.ContactEmail, inDto.ContactFaxNr, inDto.ContactName,
                                                                                                          inDto.ContactTelDirect, inDto.BinaryPOI, inDto.BinaryPOItype);
                // For report
                string responseXML = Cic.OpenOne.Common.Util.Serialization.XMLSerializer.SerializeUTF8WithoutNamespace(reportResp);
                code = codeTechExc;

                outDto = new DeltavistaOutDto();
                outDto.ReferenceNumber = reportResp.referenceNumber;
                if (reportResp.transactionError != null)
                {
                    outDto.TransactionError = new DVTransactionErrorDto();
                    outDto.TransactionError.Code = reportResp.transactionError.code;
                    outDto.TransactionError.Text = reportResp.transactionError.text;
                }

                // Save Output
                dvDBDao.SaveOrderCresuraReportOutput(sysAuskunft, outDto);

                // Update Auskuft
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
                auskunftDto.DeltavistaOutDto = outDto;
                auskunftDto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in orderCresuraReport!", e);
            }

        }

        /// <summary>
        /// Get Report
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto getReport(DeltavistaInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.DeltavistaGetReport);
            try
            {
                DAO.Auskunft.DeltavistaRef2.IdentityDescriptor idDesc = dvDBDao.GetIdDescriptor();

                // Save Input
                dvDBDao.SaveGetReportInput(sysAuskunft, inDto);
                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                DAO.Auskunft.DeltavistaRef2.ReportResponse reportResp = dvWSDao.getReport(idDesc, inDto.AddressId, inDto.ReportId, inDto.TargetFormat);
                code = codeTechExc;

                outDto = new DeltavistaOutDto();

                // outDto.Report bleibt leer
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                if (reportResp.report != null)
                {
                    outDto.ReportBlob = enc.GetBytes(reportResp.report);
                }
                outDto.ReportBlobFormat = inDto.TargetFormat;

                if (reportResp.transactionError != null)
                {
                    outDto.TransactionError = new DVTransactionErrorDto();
                    outDto.TransactionError.Code = reportResp.transactionError.code;
                    outDto.TransactionError.Text = reportResp.transactionError.text;
                }

                // Save Output
                dvDBDao.SaveGetReportOutput(sysAuskunft, outDto);

                // Update Auskuft
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
                auskunftDto.DeltavistaOutDto = outDto;
                auskunftDto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getReport!", e);
            }
        }

        /// <summary>
        /// get Identified Address
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto getIdentifiedAddress(long sysAuskunft)
        {
            long code = codeTechExc;
            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Get DeltavistaInDto
                DeltavistaInDto DVInDto = dvDBDao.FindBySysId(sysAuskunft);
                auskunftdto.DeltavistaInDto = DVInDto;

                idDesc = dvDBDao.GetIdentityDescriptor();
                AddressDescription addressDesc = MyMapDtoToAddressDescription(DVInDto.AddressDescription);
                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                AddressIdentificationResponse addrIdent = dvWSDao.getAddressId(idDesc, addressDesc, DVInDto.AddressDescription.UIDNummer);
                code = codeTechExc;

                DeltavistaOutDto outDto = MyMapAddressIdentificationToOutDto(addrIdent);
                auskunftdto.DeltavistaOutDto = outDto;
                auskunftdto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                // Save Output 
                dvDBDao.SaveGetIdentifiedAddressOutput(sysAuskunft, outDto);

                // Update Auskuft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }

                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);

                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getIdentifiedAddress!", e);
            }
        }

        /// <summary>
        /// getIdentifiedAddress
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto getIdentifiedAddressArb(long sysAuskunft)
        {
            long code = codeTechExc;
            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Get DeltavistaInDto
                DeltavistaInDto DVInDto = dvDBDao.FindBySysId(sysAuskunft);
                auskunftdto.DeltavistaInDto = DVInDto;

                idDesc = dvDBDao.GetIdentityDescriptorArb();
                AddressDescription addressDesc = MyMapDtoToAddressDescription(DVInDto.AddressDescription);

                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                AddressIdentificationResponse addrIdent = dvWSDao.getAddressId(idDesc, addressDesc, DVInDto.AddressDescription.UIDNummer);
                code = codeTechExc;

                DeltavistaOutDto outDto = MyMapAddressIdentificationToOutDto(addrIdent);
                auskunftdto.DeltavistaOutDto = outDto;
                auskunftdto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                // Save Output 
                dvDBDao.SaveGetIdentifiedAddressOutput(sysAuskunft, outDto);

                // Update Auskuft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }

                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);

                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getIdentifiedAddressArb!", e);
            }
        }

        /// <summary>
        /// Get Company Details By Address ID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto getCompanyDetailsByAddressId(long sysAuskunft)
        {
            long code = codeTechExc;
            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Get DeltavistaInDto
                DeltavistaInDto DVInDto = dvDBDao.FindBySysId(sysAuskunft);
                auskunftdto.DeltavistaInDto = DVInDto;

                idDesc = dvDBDao.GetIdentityDescriptor();
                code = codeTechExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                CompanyDetailsResponse compResp = dvWSDao.getCompanyDetailsByAddressId(idDesc, DVInDto.AddressId);
                code = codeSerAufExc;

                outDto = MyMapCompanyDetailsToOutDto(compResp);
                auskunftdto.DeltavistaOutDto = outDto;
                auskunftdto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

               

                // Update Auskuft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                if (code == 0 && outDto != null && outDto.HqAddress != null)
                    // Save Output                 
                {
                    this.dvDBDao.SaveGetCompanyDetailsOutput(sysAuskunft, this.outDto);
                }

                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);

                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getCompanyDetailsByAddressId!", e);
            }
        }

        /// <summary>
        /// Get Debt Details By Address ID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto getDebtDetailsByAddressId(long sysAuskunft)
        {
            long code = codeTechExc;
            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Get DeltavistaInDto
                DeltavistaInDto DVInDto = dvDBDao.FindBySysId(sysAuskunft);
                auskunftdto.DeltavistaInDto = DVInDto;

                idDesc = dvDBDao.GetIdentityDescriptor();
                code = codeSerAufExc;

                //For report
                dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // Send request away
                DebtDetailsResponse debtResp = dvWSDao.getDebtDetailsByAddressId(idDesc, DVInDto.AddressId);
                code = codeTechExc;

                outDto = MyMapDebtDetailsToOutDto(debtResp);
                auskunftdto.DeltavistaOutDto = outDto;
                auskunftdto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

                // Save Output
                dvDBDao.SaveGetDebtDetailsOutput(sysAuskunft, outDto);

                // Update Auskuft
                if (outDto.TransactionError != null)
                {
                    code = (long)outDto.TransactionError.Code;
                }
                else
                {
                    code = 0;
                }
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);

                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in getDebtDetailsByAddressId!", e);
            }
        }

        /// <summary>
        /// Get Report
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto getReport(long sysAuskunft)
        {
            long code = codeTechExc;
            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);

            // Get DeltavistaInDto
            DeltavistaInDto DVInDto = dvDBDao.FindBySysId(sysAuskunft);
            auskunftdto.DeltavistaInDto = DVInDto;

            idDescRef2 = dvDBDao.GetIdDescriptor();

            code = codeTechExc;

            //For report
            dvWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

            // Send request away
            DAO.Auskunft.DeltavistaRef2.ReportResponse reportResp = dvWSDao.getReport(idDescRef2, DVInDto.AddressId, DVInDto.ReportId, DVInDto.TargetFormat);
            code = codeSerAufExc;

            outDto = new DeltavistaOutDto();

            // outDto.Report bleibt leer
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            if (reportResp.report != null)
            {
                outDto.ReportBlob = enc.GetBytes(reportResp.report);
            }
            outDto.ReportBlobFormat = DVInDto.TargetFormat;

            if (reportResp.transactionError != null)
            {
                outDto.TransactionError = new DVTransactionErrorDto();
                outDto.TransactionError.Code = reportResp.transactionError.code;
                outDto.TransactionError.Text = reportResp.transactionError.text;
            }

            auskunftdto.DeltavistaOutDto = outDto;
            auskunftdto.requestXML = dvWSDao.getSoapXMLDto().requestXML;
            auskunftdto.responseXML = dvWSDao.getSoapXMLDto().responseXML;

            // Save Output
            dvDBDao.SaveGetReportOutput(sysAuskunft, outDto);

            // Update Auskuft
            if (outDto.TransactionError != null)
            {
                code = (long)outDto.TransactionError.Code;
            }
            else
            {
                code = 0;
            }
            auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);

            return auskunftdto;
        }

        #region Private Methods

        /// <summary>
        /// Maps AddressIdentificationResponse to DeltavistaOutDto
        /// </summary>
        /// <param name="addrIdent"></param>
        /// <returns>DeltavistaOutDto</returns>
        private DeltavistaOutDto MyMapAddressIdentificationToOutDto(AddressIdentificationResponse addrIdent)
        {
            DeltavistaOutDto outDto = new DeltavistaOutDto();
            // addrIdent.verificationDecision:
            // ist nie null
            // falls 0 : candidateListe null, foundAddress nicht null
            // falls > 0 : foundAddress null, candidateListe nicht null (laut Spezifikation / stimmt aber nicht falls verificationDecision = 2)
            outDto.VerificationDecision = addrIdent.verificationDecision;
            if (addrIdent.verificationDecision > 0)
            {
                if (addrIdent.candidateListe != null)
                {
                    outDto.CandidateList = new List<DVAddressMatchDto>();
                    foreach (DAO.Auskunft.DeltavistaRef.AddressMatch candidate in addrIdent.candidateListe)
                    {
                        DVAddressMatchDto addrMatch = MyMapAddressMatchToDto(candidate);
                        outDto.CandidateList.Add(addrMatch);
                    }
                }
            }
            else if (addrIdent.verificationDecision == 0)
            {
                outDto.FoundAddress = MyMapAddressMatchToDto(addrIdent.foundAddress);
            }
            if (addrIdent.transactionError != null)
            {
                outDto.TransactionError = MyMapTransactionErrorToDto(addrIdent.transactionError);
            }
            return outDto;
        }

        /// <summary>
        /// Maps CompanyDetailsResponse to DeltavistaOutDto
        /// </summary>
        /// <param name="companyDetails"></param>
        /// <returns>DeltavistaOutDto</returns>
        private DeltavistaOutDto MyMapCompanyDetailsToOutDto(CompanyDetailsResponse companyDetails)
        {
            DeltavistaOutDto outDto = Mapper.Map<CompanyDetailsResponse, DeltavistaOutDto>(companyDetails);
            outDto.HqAddress = MyMapAddressDescriptionToDto(companyDetails.hqAddress);

            if (companyDetails.keyValueList != null)
            {
                outDto.KeyValueList = new List<DVKeyValuePairDto>();
                foreach (DAO.Auskunft.DeltavistaRef.KeyValueItem keyValueItem in companyDetails.keyValueList)
                {
                    DVKeyValuePairDto keyValuePairDto = MyMapKeyValueToDto(keyValueItem);
                    outDto.KeyValueList.Add(keyValuePairDto);
                }
            }
            if (companyDetails.managementList != null)
            {
                outDto.ManagementList = new List<DVManagementMemberDto>();
                foreach (DAO.Auskunft.DeltavistaRef.ManagementMember memberItem in companyDetails.managementList)
                {
                    DVManagementMemberDto memberDto = MyMapManagementMemberToDto(memberItem);
                    outDto.ManagementList.Add(memberDto);
                }
            }
            if (companyDetails.contactList != null)
            {
                outDto.ContactList = new List<DVContactDescriptionDto>();
                foreach (DAO.Auskunft.DeltavistaRef.ContactDescription contactItem in companyDetails.contactList)
                {
                    DVContactDescriptionDto contactDto = new DVContactDescriptionDto();
                    contactDto.ContactDetails = contactItem.contactDetails;
                    contactDto.ContactType = contactItem.contactType;
                    contactDto.DateLastVerified = contactItem.dateLastVerified;
                    outDto.ContactList.Add(contactDto);
                }
            }
            if (companyDetails.samePhoneList != null)
            {
                outDto.SamePhoneList = new List<DVAddressDescriptionDto>();
                foreach (DAO.Auskunft.DeltavistaRef.AddressDescription addressItem in companyDetails.samePhoneList)
                {
                    DVAddressDescriptionDto addressDto = MyMapAddressDescriptionToDto(addressItem);
                    outDto.SamePhoneList.Add(addressDto);
                }
            }
            if (companyDetails.transactionError != null)
            {
                outDto.TransactionError = MyMapTransactionErrorToDto(companyDetails.transactionError);
            }
            return outDto;
        }

        /// <summary>
        /// Maps DebtDetailsResponse to DeltavistaOutDto
        /// </summary>
        /// <param name="debtDetails"></param>
        /// <returns>DeltavistaOutDto</returns>
        private DeltavistaOutDto MyMapDebtDetailsToOutDto(DebtDetailsResponse debtDetails)
        {
            DeltavistaOutDto outDto = new DeltavistaOutDto();
            outDto.ReturnCode = debtDetails.returnCode;
            if (debtDetails.debtList != null)
            {
                outDto.DebtList = new List<DVDebtEntryDto>();
                foreach (DAO.Auskunft.DeltavistaRef.DebtEntry debtEntry in debtDetails.debtList)
                {
                    DVDebtEntryDto debtEntryDto = MyMapDebtEntryToDto(debtEntry);
                    outDto.DebtList.Add(debtEntryDto);
                }
            }
            if (debtDetails.transactionError != null)
            {
                outDto.TransactionError = MyMapTransactionErrorToDto(debtDetails.transactionError);
            }
            return outDto;
        }

        /// <summary>
        /// Maps AddressDescription to DTO
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private DVAddressDescriptionDto MyMapAddressDescriptionToDto(DAO.Auskunft.DeltavistaRef.AddressDescription address)
        {
            DVAddressDescriptionDto dto = Mapper.Map<AddressDescription, DVAddressDescriptionDto>(address);
            return dto;
        }

        /// <summary>
        /// Maps AddressCorrection to DTO
        /// </summary>
        /// <param name="correction"></param>
        /// <returns>DVAddressCorrectionDto</returns>
        private DVAddressCorrectionDto MyMapAddressCorrectionToDto(DAO.Auskunft.DeltavistaRef.AddressCorrection correction)
        {
            DVAddressCorrectionDto dto = Mapper.Map<AddressCorrection, DVAddressCorrectionDto>(correction);
            return dto;
        }

        /// <summary>
        /// Maps KeyValuePair to DTO
        /// </summary>
        /// <param name="item"></param>
        /// <returns>DVKeyValuePairDto</returns>
        private DVKeyValuePairDto MyMapKeyValueToDto(DAO.Auskunft.DeltavistaRef.KeyValueItem item)
        {
            DVKeyValuePairDto dto = Mapper.Map<KeyValueItem, DVKeyValuePairDto>(item);
            return dto;
        }

        /// <summary>
        /// Maps AddressMatch to DTO
        /// </summary>
        /// <param name="address"></param>
        /// <returns>DVAddressMatchDto</returns>
        private DVAddressMatchDto MyMapAddressMatchToDto(DAO.Auskunft.DeltavistaRef.AddressMatch address)
        {
            DVAddressMatchDto dto = Mapper.Map<AddressMatch, DVAddressMatchDto>(address);
            return dto;
        }

        /// <summary>
        /// Maps DVTransactionError to DTO
        /// </summary>
        /// <param name="error"></param>
        /// <returns>DVTransactionErrorDto</returns>
        private DVTransactionErrorDto MyMapTransactionErrorToDto(DAO.Auskunft.DeltavistaRef.TransactionError error)
        {
            DVTransactionErrorDto dto = Mapper.Map<TransactionError, DVTransactionErrorDto>(error);
            return dto;
        }

        /// <summary>
        /// Maps ManagementMember to DTO
        /// </summary>
        /// <param name="memberItem"></param>
        /// <returns>DVManagementMemberDto</returns>
        private DVManagementMemberDto MyMapManagementMemberToDto(DAO.Auskunft.DeltavistaRef.ManagementMember memberItem)
        {
            DVManagementMemberDto dto = Mapper.Map<ManagementMember, DVManagementMemberDto>(memberItem);
            return dto;
        }

        /// <summary>
        /// Maps DebtEntry to DTO
        /// </summary>
        /// <param name="debtEntry"></param>
        /// <returns>DVDebtEntryDto</returns>
        private DVDebtEntryDto MyMapDebtEntryToDto(DAO.Auskunft.DeltavistaRef.DebtEntry debtEntry)
        {
            DVDebtEntryDto dto = Mapper.Map<DebtEntry, DVDebtEntryDto>(debtEntry);
            return dto;
        }

        /// <summary>
        /// Maps DVAddressDescriptionDto to DeltavistaRef.AddressDescription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>AddressDescription</returns>
        private DAO.Auskunft.DeltavistaRef.AddressDescription MyMapDtoToAddressDescription(DVAddressDescriptionDto dto)
        {
            DAO.Auskunft.DeltavistaRef.AddressDescription address = Mapper.Map<DVAddressDescriptionDto, DAO.Auskunft.DeltavistaRef.AddressDescription>(dto);
            return address;
        }

        /// <summary>
        /// Maps DVAddressDescriptionDto to DeltavistaRef2.AddressDescription
        /// Used by Service Reference DAO.Auskunft.DeltavistaRef2 (OrderCresuraReport)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>DAO.Auskunft.DeltavistaRef2.AddressDescription</returns>
        private DAO.Auskunft.DeltavistaRef2.AddressDescription MyMapDtoToAddressDescriptionRef2(DVAddressDescriptionDto dto)
        {
            DAO.Auskunft.DeltavistaRef2.AddressDescription address = Mapper.Map<DVAddressDescriptionDto, DAO.Auskunft.DeltavistaRef2.AddressDescription>(dto);
            return address;
        }

        #endregion Private Methods
    }
}