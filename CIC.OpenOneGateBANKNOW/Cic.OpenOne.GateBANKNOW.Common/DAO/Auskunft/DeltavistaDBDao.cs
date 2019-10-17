using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using CIC.Database.IC.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Deltavista DB Data Access Object
    /// </summary>
    public class DeltavistaDBDao : IDeltavistaDBDao
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets USERNAME and PASSWORD from AUSKUNFTCFG
        /// </summary>
        /// <returns>IdentityDescriptor, filled with username and password</returns>
        public DAO.Auskunft.DeltavistaRef.IdentityDescriptor GetIdentityDescriptor()
        {
            DAO.Auskunft.DeltavistaRef.IdentityDescriptor idDescriptor = new DeltavistaRef.IdentityDescriptor();
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    idDescriptor.name = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG.ToUpper() == "DELTAVISTAGETADDRESSID").First().USERNAME;
                    idDescriptor.password = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG.ToUpper() == "DELTAVISTAGETADDRESSID").First().KEYVALUE;
                }
                catch (Exception ex)
                {
                    _log.Error("Zugangsdaten für Deltavista konnten nicht geladen werden. ", ex);
                    throw ex;
                }
            }
            return idDescriptor;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DAO.Auskunft.DeltavistaRef.IdentityDescriptor GetIdentityDescriptorArb()
        {
            DAO.Auskunft.DeltavistaRef.IdentityDescriptor idDescriptor = new DeltavistaRef.IdentityDescriptor();
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    idDescriptor.name = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG.ToUpper() == "DELTAVISTAGETADDRESSIDARB").First().USERNAME;
                    idDescriptor.password = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG.ToUpper() == "DELTAVISTAGETADDRESSIDARB").First().KEYVALUE;
                }
                catch (Exception ex)
                {
                    _log.Error("Zugangsdaten für Deltavista konnten nicht geladen werden. ", ex);
                    throw ex;
                }
            }
            return idDescriptor;
        }

        /// <summary>
        /// Saves DVInputaddressIdentification and DVAddressDescription
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="inDto">Input Data</param>
        public void SaveGetIdentifiedAddressInput(long sysAuskunft, DeltavistaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVINPADRID : DVInputaddressIdentification
                    DVINPADRID DVInpAddrIdent = new DVINPADRID();
                    DVInpAddrIdent.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.DVINPADRID.Add(DVInpAddrIdent);
                    context.SaveChanges();

                    // DVADRDESC : DVAddressDescription
                    DVADRDESC DVAddrDescr = MyFillAddrDesc(inDto.AddressDescription);
                    DVAddrDescr.DVINPADRID = DVInpAddrIdent;
                    context.DVADRDESC.Add(DVAddrDescr);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von AddressIdentification Input ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves DVInputCompanyDetails with addressId
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="inDto">Input Data</param>
        public void SaveGetCompanyDetailsInput(long sysAuskunft, DeltavistaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVINPCD : DVInputCompanyDetails
                    DVINPCD DVInpCompDet = new DVINPCD();
                    DVInpCompDet.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    if (inDto.AddressId != 0)
                    {
                        DVInpCompDet.ADDRESSID = inDto.AddressId.ToString();
                    }
                    context.DVINPCD.Add(DVInpCompDet);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von CompanyDetailsInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves DVInputbonitaetsauskunft with addressId
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="inDto">Input Data</param>
        public void SaveGetDebtDetailsInput(long sysAuskunft, DeltavistaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVINPBONI : DVInputbonitaetsauskunft
                    DVINPBONI DVInpBoni = new DVINPBONI();
                    DVInpBoni.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    if (inDto.AddressId != 0)
                    {
                        DVInpBoni.ADDRESSID = inDto.AddressId.ToString();
                    }
                    context.DVINPBONI.Add(DVInpBoni);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von DebtDetailsInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves DVOutputaddressIdentification
        /// Saves DVAddressMatch[0..n] plus DVAddressDescription, DVAddressCorrection, DVKeyValuePair[0..n] for each DVAddressMatch
        /// Saves possible DVTransactionError
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="outDto">Input Data</param>
        public void SaveGetIdentifiedAddressOutput(long sysAuskunft, DeltavistaOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    List<DVAddressMatchDto> OutAddrMatchDtoList = new List<DVAddressMatchDto>();
                    // addrIdent.verificationDecision: ist nie null
                    if (outDto.VerificationDecision == 0)
                    {
                        // falls 0 : candidateListe null, foundAddress nicht null
                        if (outDto.FoundAddress != null)
                            OutAddrMatchDtoList.Add(outDto.FoundAddress);
                    }
                    else
                    {
                        // falls > 0 : foundAddress null, candidateListe nicht null (laut Spezifikation / stimmt aber nicht falls verificationDecision = 2)
                        OutAddrMatchDtoList = outDto.CandidateList;
                    }

                    // DVOUTADRID : DVOutputaddressIdentification
                    DVOUTADRID DVOutAdrId = new DVOUTADRID();
                    DVOutAdrId.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    if (outDto.TransactionError != null)
                    {
                        DVOutAdrId.ERRCODE = outDto.TransactionError.Code;
                        DVOutAdrId.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    DVOutAdrId.VERIFICATIONDECISION = outDto.VerificationDecision;
                    context.DVOUTADRID.Add(DVOutAdrId);
                    context.SaveChanges();

                    if (OutAddrMatchDtoList != null)
                    {
                        foreach (var OutAddrMatchDto in OutAddrMatchDtoList)
                        {
                            // DVADRMATCH : DVAddressMatch
                            DVADRMATCH DVAdrMatch = MyFillAddressMatch(OutAddrMatchDto);
                            DVAdrMatch.DVOUTADRID = DVOutAdrId;
                            context.DVADRMATCH.Add(DVAdrMatch);
                            context.SaveChanges();

                            // DVADRCORR : DVAddressCorrection (keine Liste)
                            if (OutAddrMatchDto.Correction != null)
                            {
                                DVADRCORR DVAdrCorr = MyFillAddrCorr(OutAddrMatchDto.Correction);
                                DVAdrCorr.DVADRMATCH = DVAdrMatch;
                                context.DVADRCORR.Add(DVAdrCorr);
                            }

                            if (OutAddrMatchDto.Address != null)
                            {
                                // DVADRDESC : DVAddressDescription
                                DVADRDESC DVAdrDesc = MyFillAddrDesc(OutAddrMatchDto.Address);
                                DVAdrDesc.DVADRMATCH = DVAdrMatch;
                                context.DVADRDESC.Add(DVAdrDesc);
                            }

                            if (OutAddrMatchDto.KeyValueList != null)
                            {
                                // DVKVP : DVKeyValuePair
                                foreach (var OutDtoKVP in OutAddrMatchDto.KeyValueList)
                                {
                                    DVKVP DVkvp = MyFillKeyValuePair(OutDtoKVP);
                                    DVkvp.DVADRMATCH = DVAdrMatch;
                                    context.DVKVP.Add(DVkvp);
                                }
                            }
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von AddressIdentificationOutput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves DVOutputCompanyDetails with nogaCodes, dateEntry, numberOfShares,...
        /// Saves DVManagementMember[0..n] and DVAddressDescription[0..n] and DVManagementMember[0..n] and DVKeyValuePair[0..n]
        /// Saves possible DVTransactionError
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="outDto">Output Data</param>
        public void SaveGetCompanyDetailsOutput(long sysAuskunft, DeltavistaOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVOUTCD : DVOutputCompanyDetails
                    DVOUTCD DVOutCD = MyFillCompanyDetails(outDto);
                    DVOutCD.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.DVOUTCD.Add(DVOutCD);
                    context.SaveChanges();

                    // DVADRDESC : DVAddressDescription
                    DVADRDESC DVAddrDesc = MyFillAddrDesc(outDto.HqAddress);
                    DVAddrDesc.DVOUTCD = DVOutCD;
                    context.DVADRDESC.Add(DVAddrDesc);
                    context.SaveChanges();

                    DVOutCD.SYSDVADRDESCHQ = DVAddrDesc.SYSDVADRDESC;

                    // DVMGMNTM : DVManagementMember
                    if (outDto.ManagementList != null)
                    {
                        foreach (var OutDVMgmt in outDto.ManagementList)
                        {
                            try
                            {
                                DVMGMNTM DVMgmtM = MyFillManagementMember(OutDVMgmt);
                                DVMgmtM.DVOUTCD = DVOutCD;
                                context.DVMGMNTM.Add(DVMgmtM);
                                context.SaveChanges();


                                // DVADRDESC : DVAddressDescription for DVManagementMember
                                DVAddrDesc = MyFillAddrDesc(OutDVMgmt.Address);
                                DVAddrDesc.DVMGMNTM = DVMgmtM;
                                context.DVADRDESC.Add(DVAddrDesc);
                            }
                            catch (Exception ex)
                            {
                                _log.Debug("Fehler beim Speichern von ManagementMember ", ex);
                            }
                        }
                    }

                    // DVKVP : DVKeyValuePair
                    if (outDto.KeyValueList != null)
                    {
                        foreach (var OutDVkvp in outDto.KeyValueList)
                        {
                            try
                            {
                                DVKVP DVkvp = MyFillKeyValuePair(OutDVkvp);
                                DVkvp.DVOUTCD = DVOutCD;
                                context.DVKVP.Add(DVkvp);
                            }
                            catch (Exception ex)
                            {
                                _log.Debug("Fehler beim Speichern von KeyValues ", ex);
                            }
                        }
                    }

                    // DVCONTACT : ContactList
                    if (outDto.ContactList != null)
                    {
                        foreach (var OutDVContDescr in outDto.ContactList)
                        {
                            try
                            {
                                DVCONTACT DVContDescr = MyFillContact(OutDVContDescr);
                                DVContDescr.DVOUTCD = DVOutCD;
                                context.DVCONTACT.Add(DVContDescr);
                            }
                            catch (Exception ex)
                            {
                                _log.Debug("Fehler beim Speichern von Contacts ", ex);
                            }
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von CompanyDetailsOutput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves DVOutputbonitaetsauskunft and DVDebtEntry[0..n]
        /// Saves possible DVTransactionError
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="outDto">Output Data</param>
        public void SaveGetDebtDetailsOutput(long sysAuskunft, DeltavistaOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVOUTBONI : DVOutputbonitaetsauskunft
                    DVOUTBONI DVOutBoni = new DVOUTBONI();
                    DVOutBoni.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    DVOutBoni.RETURNCODE = outDto.ReturnCode;
                    if (outDto.TransactionError != null)
                    {
                        DVOutBoni.ERRCODE = outDto.TransactionError.Code;
                        DVOutBoni.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.DVOUTBONI.Add(DVOutBoni);
                    context.SaveChanges();

                    if (outDto.DebtList != null)
                    {
                        foreach (var DebtEntry in outDto.DebtList)
                        {
                            DVDEBTENTRY DVDebtEntry = Mapper.Map<DVDebtEntryDto, DVDEBTENTRY>(DebtEntry);
                            DVDebtEntry.DVOUTBONI = DVOutBoni;
                            context.DVDEBTENTRY.Add(DVDebtEntry);
                        }
                        context.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von DebtDetailsOutput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// FindBySysId
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Deltavista Information Data</returns>
        public DeltavistaInDto FindBySysId(long sysAuskunft)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    DeltavistaInDto DeltavistaInDto = new DeltavistaInDto();

                    // Hole den AuskunftTyp
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP AuskunftTyp = AuskunftTypQuery.Single();

                    if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.DeltavistaGetAddressId))
                    {
                        // Address Identification
                        MyFindBySysId_GetAddressId(context, sysAuskunft, DeltavistaInDto);
                    }

                    if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.DeltavistaGetAddressIdArb))
                    {
                        // Address Identification
                        MyFindBySysId_GetAddressId(context, sysAuskunft, DeltavistaInDto);
                    }

                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.DeltavistaGetCompanyDetails))
                    {
                        // Company Details
                        MyFindBySysId_GetCompanyDetails(context, sysAuskunft, DeltavistaInDto);
                    }
                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.DeltavistaGetDebtDetails))
                    {
                        // Bonität
                        MyFindBySysId_GetDebtDetails(context, sysAuskunft, DeltavistaInDto);
                    }
                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.DeltavistaGetReport))
                    {
                        // Handelsregisterauskunft 
                        MyFindBySysId_GetReport(context, sysAuskunft, DeltavistaInDto);
                    }
                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.DeltavistaOrderCresuraReport))
                    {
                        // Betreibungsauskunft 
                        MyFindBySysId_OrderCresuraReport(context, sysAuskunft, DeltavistaInDto);
                    }

                    return DeltavistaInDto;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Laden von Information Data in FindBySysId. Error Message. ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Gets USERNAME and PASSWORD from AUSKUNFTCFG
        /// </summary>
        /// <returns>Identity Descriptor</returns>
        public DeltavistaRef2.IdentityDescriptor GetIdDescriptor()
        {
            DAO.Auskunft.DeltavistaRef2.IdentityDescriptor idDescriptor = new DeltavistaRef2.IdentityDescriptor();
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    var DescriptorQuery = from AuskunftCfg in context.AUSKUNFTCFG
                                          where AuskunftCfg.BEZEICHNUNG.ToUpper() == "DELTAVISTAGETADDRESSID"
                                          select AuskunftCfg;
                    AUSKUNFTCFG AuskunftConfig = DescriptorQuery.Single();
                    idDescriptor.name = AuskunftConfig.USERNAME;
                    idDescriptor.password = AuskunftConfig.KEYVALUE;
                }
                catch (Exception ex)
                {
                    _log.Error("Zugangsdaten für Deltavista konnten nicht geladen werden. ", ex);
                    throw ex;
                }
            }
            return idDescriptor;
        }

        /// <summary>
        /// SaveOrderCresuraReportInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        public void SaveOrderCresuraReportInput(long sysAuskunft, DeltavistaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVINPBA : Betreibungsauskunft
                    DVINPBA DVInpBA = Mapper.Map<DeltavistaInDto, DVINPBA>(inDto);
                    DVInpBA.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.DVINPBA.Add(DVInpBA);
                    context.SaveChanges();

                    // DVADRDESC : AddressDescription
                    DVADRDESC DVAdrDesc = MyFillAddrDesc(inDto.AddressDescription);
                    DVAdrDesc.DVINPBA = DVInpBA;
                    context.DVADRDESC.Add(DVAdrDesc);
                    context.SaveChanges();

                    // DVORDDESC : OrderDescription
                    if (inDto.OrderDescription != null)
                    {
                        DVORDDESC DVOrdDesc = new DVORDDESC();
                        if (inDto.OrderDescription.BAProductId != 0)
                        {
                            DVOrdDesc.BAPRODUCTID = inDto.OrderDescription.BAProductId.ToString();
                        }
                        if (inDto.OrderDescription.cresuraReportId != 0)
                        {
                            DVOrdDesc.CRESURAREPORTID = inDto.OrderDescription.cresuraReportId.ToString();
                        }
                        if (inDto.OrderDescription.EWKProductId != 0)
                        {
                            DVOrdDesc.EWKPRODUCTID = inDto.OrderDescription.EWKProductId.ToString();
                        }
                        DVOrdDesc.DVINPBA = DVInpBA;
                        context.DVORDDESC.Add(DVOrdDesc);
                    }
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von Betreibungsauskunft Input", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveOrderCresuraReportOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        public void SaveOrderCresuraReportOutput(long sysAuskunft, DeltavistaOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVOUTBA : Betreibungsauskunft
                    DVOUTBA DVOutBA = new DVOUTBA();
                    DVOutBA.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    DVOutBA.REFERENZNUMMER = outDto.ReferenceNumber;
                    if (outDto.TransactionError != null)
                    {
                        DVOutBA.ERRCODE = outDto.TransactionError.Code;
                        DVOutBA.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.DVOUTBA.Add(DVOutBA);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von Betreibungsauskunft Output ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveGetReportInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        public void SaveGetReportInput(long sysAuskunft, DeltavistaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVINPHR : DVInputHandelsregisterauskunft 

                    DVINPHR DVInpHr = new DVINPHR();
                    DVInpHr.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    if (inDto.AddressId != 0)
                    {
                        DVInpHr.ADDRESSID = inDto.AddressId.ToString();
                    }
                    if (inDto.ReportId != 0)
                    {
                        DVInpHr.REPORTID = inDto.ReportId.ToString();
                    }
                    DVInpHr.TARGETFORMAT = inDto.TargetFormat;

                    context.DVINPHR.Add(DVInpHr);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von DVInputHandelsregisterauskunft Input", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveGetReportOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        public void SaveGetReportOutput(long sysAuskunft, DeltavistaOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // DVOUTHR : DVInputHandelsregisterauskunft
                    DVOUTHR DVOutHr = new DVOUTHR();
                    DVOutHr.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    DVOutHr.REPORT = outDto.Report;
                    DVOutHr.REPORTBLOB = outDto.ReportBlob;
                    DVOutHr.REPORTBLOBFORMAT = outDto.ReportBlobFormat;
                    if (outDto.TransactionError != null)
                    {
                        DVOutHr.ERRCODE = outDto.TransactionError.Code;
                        DVOutHr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.DVOUTHR.Add(DVOutHr);
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von DVInputHandelsregisterauskunft Output ", ex);
                    throw ex;
                }
            }
        }


        #region Private Methods

        /// <summary>
        /// MyFillAddressMatch
        /// DVADRMATCH : DVAddressMatch
        /// </summary>
        /// <param name="InputAddressMatch"></param>
        /// <returns>DVADRMATCH</returns>
        private DVADRMATCH MyFillAddressMatch(DVAddressMatchDto InputAddressMatch)
        {
            DVADRMATCH DVAdrMatch = new DVADRMATCH();
            if (InputAddressMatch.AddressId != 0)
            {
                DVAdrMatch.ADDRESSID = InputAddressMatch.AddressId.ToString();
            }
            DVAdrMatch.CHARACTER = (long?)InputAddressMatch.Character;
            DVAdrMatch.CONFIDENCE = (long?)InputAddressMatch.Confidence;
            DVAdrMatch.DIFFERENCE = (long?)InputAddressMatch.Difference;
            DVAdrMatch.SIMILARITY = (long?)InputAddressMatch.Similarity;
            // Der Status muss zukünftig in das Feld DVSTATUS geschrieben werden. Das bisherige Feld Status hat eine andere Verwendung.
            DVAdrMatch.DVSTATUS = (long?)InputAddressMatch.Status;
            return DVAdrMatch;
        }

        /// <summary>
        /// MyFillAddrCorr
        /// </summary>
        /// <param name="InputCorrection"></param>
        /// <returns>DVADRCORR</returns>
        private DVADRCORR MyFillAddrCorr(DVAddressCorrectionDto InputCorrection)
        {
            DVADRCORR DVAddrCorr = Mapper.Map<DVAddressCorrectionDto, DVADRCORR>(InputCorrection);
            return DVAddrCorr;
        }

        /// <summary>
        /// MyFillAddrDesc
        /// </summary>
        /// <param name="InputAddrDesc"></param>
        /// <returns>DVADRDESC</returns>
        private DVADRDESC MyFillAddrDesc(DVAddressDescriptionDto InputAddrDesc)
        {
            DVADRDESC DVAddrDesc = Mapper.Map<DVAddressDescriptionDto, DVADRDESC>(InputAddrDesc);
            return DVAddrDesc;
        }

        /// <summary>
        /// MyFillKeyValuePair
        /// </summary>
        /// <param name="outDVkvp"></param>
        /// <returns>DVKVP</returns>
        private DVKVP MyFillKeyValuePair(DVKeyValuePairDto outDVkvp)
        {
            DVKVP DVkvp = Mapper.Map<DVKeyValuePairDto, DVKVP>(outDVkvp);
            return DVkvp;
        }

        /// <summary>
        /// MyFillManagementMember
        /// </summary>
        /// <param name="outDVMgmt"></param>
        /// <returns>DVMGMNTM</returns>
        private DVMGMNTM MyFillManagementMember(DVManagementMemberDto outDVMgmt)
        {
            DVMGMNTM DVMgmntM = Mapper.Map<DVManagementMemberDto, DVMGMNTM>(outDVMgmt);
            if (outDVMgmt.SignatureRight != 0)
            {
                DVMgmntM.SIGNATURERIGHT = outDVMgmt.SignatureRight.ToString();
            }
            return DVMgmntM;
        }

        /// <summary>
        /// MyFillContact
        /// </summary>
        /// <param name="outDVContDescr"></param>
        /// <returns>DVCONTACT</returns>
        private DVCONTACT MyFillContact(DVContactDescriptionDto outDVContDescr)
        {
            DVCONTACT DVContact = Mapper.Map<DVContactDescriptionDto, DVCONTACT>(outDVContDescr);
            return DVContact;
        }

        /// <summary>
        /// MyMapDVAdrDescToDeltavistaInDto
        /// </summary>
        /// <param name="dvAdrDesc"></param>
        /// <returns>DVAddressDescriptionDto</returns>
        private DVAddressDescriptionDto MyMapDVAdrDescToDeltavistaInDto(DVADRDESC dvAdrDesc)
        {
            DVAddressDescriptionDto inDto = Mapper.Map<DVADRDESC, DVAddressDescriptionDto>(dvAdrDesc);
            return inDto;
        }

        /// <summary>
        /// MyFillCompanyDetails
        /// </summary>
        /// <param name="dvOutDto">DVOutDto</param>
        /// <returns>DVOUTCD</returns>
        private DVOUTCD MyFillCompanyDetails(DeltavistaOutDto dvOutDto)
        {
            DVOUTCD dvOutCD = Mapper.Map<DeltavistaOutDto, DVOUTCD>(dvOutDto);

            if (dvOutDto.NogaCode02Description != null)
                if (dvOutDto.NogaCode02Description.Length > 100)
                    dvOutCD.NOGA02DESCRIPTION = dvOutDto.NogaCode02Description.Substring(0, 99) + "+";
                else
                    dvOutCD.NOGA02DESCRIPTION = dvOutDto.NogaCode02Description;
            if (dvOutDto.NogaCode08Description != null)
                if (dvOutDto.NogaCode08Description.Length > 100)
                    dvOutCD.NOGA08DESCRIPTION = dvOutDto.NogaCode08Description.Substring(0, 99) + "+";
                else
                    dvOutCD.NOGA08DESCRIPTION = dvOutDto.NogaCode08Description;

            if (dvOutDto.TransactionError != null)
            {
                dvOutCD.ERRCODE = dvOutDto.TransactionError.Code;
                dvOutCD.ERRTEXT = MyTruncateErrText(dvOutDto.TransactionError.Text);
            }
            return dvOutCD;
        }

        /// <summary>
        /// MyConvertStringToID
        /// </summary>
        /// <param name="StrValue"></param>
        /// <returns>int</returns>
        private int MyConvertStringToID(string StrValue)
        {
            int IdParam = 0;
            Int32.TryParse(StrValue, out IdParam);
            return IdParam;
        }

        /// <summary>
        /// MyFindBySysId_GetAddressId
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysAuskunft"></param>
        /// <param name="InDto"></param>
        private void MyFindBySysId_GetAddressId(DdIcExtended context, long sysAuskunft, DeltavistaInDto InDto)
        {
            var DeltavistaQuery = from DVAdrDesc in context.DVADRDESC                                                                               // Selektiere alle AddrDescr
                                  join DVInpAdrId in context.DVINPADRID on DVAdrDesc.DVINPADRID.SYSDVINPADRID equals DVInpAdrId.SYSDVINPADRID       // für die InpAddrId, die
                                  join Auskunft in context.AUSKUNFT on DVInpAdrId.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT                  // mit Auskunft verbunden sind und
                                  where Auskunft.SYSAUSKUNFT == sysAuskunft                                                                         // der gesuchten sysAuskunft entsprechen.
                                  select DVAdrDesc;
            DVADRDESC DVADRDESC = DeltavistaQuery.Single();
            InDto.AddressDescription = MyMapDVAdrDescToDeltavistaInDto(DVADRDESC);
        }

        /// <summary>
        /// MyFindBySysId_GetCompanyDetails
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysAuskunft"></param>
        /// <param name="InDto"></param>
        private void MyFindBySysId_GetCompanyDetails(DdIcExtended context, long sysAuskunft, DeltavistaInDto InDto)
        {
            var DeltavistaQuery = from DVInpCompDet in context.DVINPCD                                                                               // Selektiere alle CompanyDetails,
                                  join Auskunft in context.AUSKUNFT on DVInpCompDet.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT                  // die mit Auskunft verbunden sind und
                                  where Auskunft.SYSAUSKUNFT == sysAuskunft                                                                         // der gesuchten sysAuskunft entsprechen.
                                  select DVInpCompDet;
            DVINPCD DVInpCD = DeltavistaQuery.Single();

            InDto.AddressId = MyConvertStringToID(DVInpCD.ADDRESSID);
        }

        /// <summary>
        /// MyFindBySysId_GetDebtDetails
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysAuskunft"></param>
        /// <param name="InDto"></param>
        private void MyFindBySysId_GetDebtDetails(DdIcExtended context, long sysAuskunft, DeltavistaInDto InDto)
        {
            var DeltavistaQuery = from DVInpBoni in context.DVINPBONI                                                                               // Selektiere alle DebtEntries,
                                  join Auskunft in context.AUSKUNFT on DVInpBoni.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT                  // die mit Auskunft verbunden sind und
                                  where Auskunft.SYSAUSKUNFT == sysAuskunft                                                                         // der gesuchten sysAuskunft entsprechen.
                                  select DVInpBoni;
            DVINPBONI DVInpBonitaet = DeltavistaQuery.Single();

            InDto.AddressId = MyConvertStringToID(DVInpBonitaet.ADDRESSID);
        }

        /// <summary>
        /// MyFindBySysId_GetReport
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysAuskunft"></param>
        /// <param name="InDto"></param>
        private void MyFindBySysId_GetReport(DdIcExtended context, long sysAuskunft, DeltavistaInDto InDto)
        {
            var DeltavistaQuery = from DVInpHr in context.DVINPHR                                                                               // Selektiere alle DvInpHr
                                  join Auskunft in context.AUSKUNFT on DVInpHr.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT                  // die mit Auskunft verbunden sind und
                                  where Auskunft.SYSAUSKUNFT == sysAuskunft                                                                         // der gesuchten sysAuskunft entsprechen.
                                  select DVInpHr;
            DVINPHR DVInpHandelsReg = DeltavistaQuery.Single();

            InDto.AddressId = MyConvertStringToID(DVInpHandelsReg.ADDRESSID);
            InDto.ReportId = MyConvertStringToID(DVInpHandelsReg.REPORTID);
            InDto.TargetFormat = DVInpHandelsReg.TARGETFORMAT;
        }

        /// <summary>
        /// MyFindBySysId_OrderCresuraReport
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysAuskunft"></param>
        /// <param name="InDto"></param>
        private void MyFindBySysId_OrderCresuraReport(DdIcExtended context, long sysAuskunft, DeltavistaInDto InDto)
        {
            // DVINPBA
            var DeltavistaQuery = from DVInpBA in context.DVINPBA                                                                                // Selektiere alle DvInpBA
                                  join Auskunft in context.AUSKUNFT on DVInpBA.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT                  // die mit Auskunft verbunden sind und
                                  where Auskunft.SYSAUSKUNFT == sysAuskunft                                                                         // der gesuchten sysAuskunft entsprechen.
                                  select DVInpBA;
            DVINPBA DVInpBetAus = DeltavistaQuery.Single();
            InDto.ContactEmail = DVInpBetAus.CONTACTEMAIL;
            InDto.ContactFaxNr = DVInpBetAus.CONTACTFAXNR;
            InDto.ContactName = DVInpBetAus.CONTACTNAME;
            InDto.ContactTelDirect = DVInpBetAus.CONTACTTELDIRECT;
            InDto.Reason = DVInpBetAus.REASON;
            InDto.RefNo = DVInpBetAus.REFNO;

            // Address Description
            var DeltavistaAdrDescQuery = from DVAdrDesc in context.DVADRDESC
                                         join DVInpBA in context.DVINPBA on DVAdrDesc.DVINPBA.SYSDVINPBA equals DVInpBA.SYSDVINPBA
                                         where DVInpBA.SYSDVINPBA == DVInpBetAus.SYSDVINPBA
                                         select DVAdrDesc;
            DVADRDESC DVInpAdrDesc = DeltavistaAdrDescQuery.Single();
            InDto.AddressDescription = MyMapDVAdrDescToDeltavistaInDto(DVInpAdrDesc);

            // Order Description
            var DeltavistaOrdDescQuery = from DVOrdDesc in context.DVORDDESC
                                         join DVInpBA in context.DVINPBA on DVOrdDesc.DVINPBA.SYSDVINPBA equals DVInpBA.SYSDVINPBA
                                         where DVInpBA.SYSDVINPBA == DVInpBetAus.SYSDVINPBA
                                         select DVOrdDesc;
            DVORDDESC DVInpOrdDesc = DeltavistaOrdDescQuery.Single();
            InDto.OrderDescription = new DVOrderDescriptionDto();
            InDto.OrderDescription.BAProductId = MyConvertStringToID(DVInpOrdDesc.BAPRODUCTID);
            InDto.OrderDescription.cresuraReportId = MyConvertStringToID(DVInpOrdDesc.CRESURAREPORTID);
            InDto.OrderDescription.EWKProductId = MyConvertStringToID(DVInpOrdDesc.EWKPRODUCTID);
        }

        /// <summary>
        /// truncateErrText
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string MyTruncateErrText(string s)
        {
            if (s != null)
                if (s.Length > 254)
                    return s.Substring(0, 254) + "+";
                else
                    return s;
            else
                return s;
        }

        #endregion Private Methods
    }
}