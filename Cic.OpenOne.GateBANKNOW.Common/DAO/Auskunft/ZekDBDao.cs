using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using CIC.Database.OD.EF6.Model;
using CIC.Database.IC.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// ZEK Data Access Object
    /// </summary>
    public class ZekDBDao : IZekDBDao
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets USERNAME and PASSWORD from AUSKUNFTCFG
        /// </summary>
        /// <returns>IdentityDescriptor, filled with username and password</returns>
        public ZEKRef.IdentityDescriptor GetIdentityDescriptor()
        {
            DAO.Auskunft.ZEKRef.IdentityDescriptor idDescriptor = new ZEKRef.IdentityDescriptor();
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    idDescriptor.name = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG == "ZEK").First().USERNAME;
                    idDescriptor.password = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG == "ZEK").First().KEYVALUE;
                }
                catch (Exception ex)
                {
                    _log.Error("Zugangsdaten für Zek konnten nicht geladen werden. ", ex);
                    throw ex;
                }
            }
            return idDescriptor;
        }

        /// <summary>
        /// Saves KreditgesuchNeuInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveKreditgesuchNeuInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPEC1 zekInpEC1 = new ZEKINPEC1();

                    zekInpEC1.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKINPEC1.Add(zekInpEC1);

                    zekInpEC1.ZIELVEREIN = inDto.Zielverein;
                    zekInpEC1.ANFRAGEGRUND = inDto.Anfragegrund;
                    zekInpEC1.PREVIOUSKREDITGESUCHID = inDto.PreviousKreditgesuchID;

                    if (inDto.RequestEntities != null && inDto.RequestEntities.Count != 0)
                    {
                        foreach (ZekRequestEntityDto zekerInDto in inDto.RequestEntities)
                        {
                            ZEKREQEN zekreqen = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(zekerInDto);
                            context.ZEKREQEN.Add(zekreqen);
                            
                            zekreqen.SYSZEKINPEC1 = zekInpEC1.SYSZEKINPEC1;

                            if (zekerInDto.AddressDescription != null)
                            {
                                ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                                zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekerInDto.AddressDescription);
                                context.ZEKADRDESC.Add(zekadrdesc);
                                zekadrdesc.ZEKREQEN = zekreqen;
                            }
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveKreditgesuchNeuInput ", ex);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Saves InformativanfrageInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveInformativanfrageInput(long sysAuskunft, ZekInDto inDto)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                
                        ZEKINPEC2 zekInpEC2 = new ZEKINPEC2();

                        zekInpEC2.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                        context.ZEKINPEC2.Add(zekInpEC2);

                        zekInpEC2.ZIELVEREIN = inDto.Zielverein;
                        zekInpEC2.ANFRAGEGRUND = inDto.Anfragegrund;

                        if (inDto.RequestEntity != null)
                        {
                            ZEKREQEN zekreqen = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(inDto.RequestEntity);
                            context.ZEKREQEN.Add(zekreqen);
                            zekInpEC2.ZEKREQEN = zekreqen;
                            if (inDto.RequestEntity.AddressDescription != null)
                            {
                                ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                                zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(inDto.RequestEntity.AddressDescription);
                                context.ZEKADRDESC.Add(zekadrdesc);
                                zekadrdesc.ZEKREQEN = zekreqen;
                            }
                        }
                        context.SaveChanges();
                        if (zekInpEC2.ZEKREQEN == null)
                            context.Entry(zekInpEC2).Reference(f => f.ZEKREQEN).Load();
                    
                         if (zekInpEC2.ZEKREQEN.REFNO == 0 || zekInpEC2.ZEKREQEN.REFNO == null)
                                zekInpEC2.ZEKREQEN.REFNO = zekInpEC2.ZEKREQEN.SYSZEKREQEN;
                        context.SaveChanges();
                    }

                   
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveInformativanfrageInput ", ex);
                    throw ex;
                }
        }
        /// <summary>
        /// Saves KreditgesuchAblehnenInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveKreditgesuchAblehnenInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPEC3 zekInpEC3 = new ZEKINPEC3();

                    zekInpEC3.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKINPEC3.Add(zekInpEC3);

                    zekInpEC3.DATUMABLEHNUNG = inDto.DatumAblehnung;
                    zekInpEC3.ABLEHNUNGSGRUND = inDto.Ablehnungsgrund;
                    zekInpEC3.KREDITGESUCHID = inDto.KreditgesuchID;

                    if (inDto.RequestEntity != null)
                    {
                        ZEKREQEN zekreqen = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(inDto.RequestEntity);
                        context.ZEKREQEN.Add(zekreqen);
                        zekInpEC3.ZEKREQEN = zekreqen;

                        if (inDto.RequestEntity.AddressDescription != null)
                        {
                            ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                            zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(inDto.RequestEntity.AddressDescription);
                            context.ZEKADRDESC.Add(zekadrdesc);
                            zekadrdesc.ZEKREQEN = zekreqen;
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveKreditgesuchAblehnenInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves KreditgesuchNeuOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveKreditgesuchNeuOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                
                try
                {
                    ZEKOUTEC1 zekOutEC1 = new ZEKOUTEC1();

                    zekOutEC1.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTEC1.Add(zekOutEC1);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutEC1.ZEKCMR = zekcmr;

                    if (outDto.Responses != null)
                    {
                        foreach (ZekResponseDescriptionDto zekResponseDescriptionDto in outDto.Responses)
                        {
                            saveZekResponseDescription(zekResponseDescriptionDto, zekcmr, context);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveKreditgesuchNeuOutput: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves InformativanfrageOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveInformativanfrageOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTEC2 zekOutEC2 = new ZEKOUTEC2();

                    zekOutEC2.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTEC2.Add(zekOutEC2);

                    ZEKINFR zekinfr = new ZEKINFR();
                    if (outDto.ReturnCode != null)
                    {
                        zekinfr.RETCODE = outDto.ReturnCode.Code;
                        zekinfr.RETTEXT = MyTruncateErrText(outDto.ReturnCode.Text);
                    }
                    context.ZEKINFR.Add(zekinfr);

                    if (outDto.TransactionError != null)
                    {
                        zekinfr.ERRCODE = outDto.TransactionError.Code;
                        zekinfr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }

                    if (outDto.FoundPerson != null)
                    {
                        ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                        zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(outDto.FoundPerson);
                        context.ZEKADRDESC.Add(zekadrdesc);
                        zekadrdesc.ZEKINFR = zekinfr;
                    }

                    if (outDto.Synonyms != null)
                    {
                        foreach (ZekAddressDescriptionDto zekAddressDescriptionDto in outDto.Synonyms)
                        {
                            ZEKADRDESC zekadrdescsy = new ZEKADRDESC();
                            zekadrdescsy = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekAddressDescriptionDto);
                            context.ZEKADRDESC.Add(zekadrdescsy);
                            zekadrdescsy.ZEKINFR = zekinfr;
                        }
                    }

                    zekOutEC2.ZEKINFR = zekinfr;

                    if (outDto.FoundContracts != null)
                        zekinfr.ZEKFC = saveZekFoundContracts(outDto.FoundContracts, context);

                    context.SaveChanges();

                    if (zekinfr.ZEKFC != null  && outDto.FoundContracts.ECode178Contracts!=null)
                    {
                        saveEcode178Contracts(zekinfr.ZEKFC.SYSZEKFC, outDto.FoundContracts.ECode178Contracts);
                    }

                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveInformativanfrageOutput ", ex);
                    throw ex;
                }
            }
        }


        //R11 CR

        /// <summary>
        /// Saves InformativanfrageOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public ZekOutDto getDBInformativanfrageOutput(long sysAuskunft)
        {


            ZekOutDto outDto = new ZekOutDto();

            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {


                    ZEKOUTEC2 zekOutEC2  = context.ZEKOUTEC2.Where(par => par.AUSKUNFT.SYSAUSKUNFT == sysAuskunft).Single();
                    if (zekOutEC2.ZEKINFR == null)
                        context.Entry(zekOutEC2).Reference(f => f.ZEKINFR).Load();
                    
                    ZEKINFR zekinfr = zekOutEC2.ZEKINFR;
                    ZekReturnCodeDto returnCode = new ZekReturnCodeDto();
                    outDto.ReturnCode = returnCode;
                    if (zekinfr != null)
                    {
                            if (zekinfr.RETCODE != null)
                            {
                                outDto.ReturnCode.Code = (int)zekinfr.RETCODE;
                                outDto.ReturnCode.Text = zekinfr.RETTEXT;
                            }

                            ZekTransactionErrorDto transactionError = new ZekTransactionErrorDto();
                            outDto.TransactionError = transactionError;
                            if (zekinfr.ERRCODE != null)
                            {
                                outDto.TransactionError.Code = (int)zekinfr.ERRCODE;
                                outDto.TransactionError.Text = zekinfr.ERRTEXT;
                            }


                            ZEKADRDESC zekadrdesc = context.ZEKADRDESC.Where(par => par.ZEKINFR.SYSZEKINFR == zekinfr.SYSZEKINFR).Single();

                            ZekAddressDescriptionDto foundPerson = new ZekAddressDescriptionDto();
                            if (zekadrdesc != null)
                            {
                                outDto.FoundPerson = foundPerson;
                                outDto.FoundPerson = Mapper.Map<ZEKADRDESC,ZekAddressDescriptionDto>(zekadrdesc);
                            }

                            List<ZekAddressDescriptionDto> synonyme = new List<ZekAddressDescriptionDto>();
                            if (zekinfr.ZEKADRDESCList.Count>0)
                    
                                foreach (ZEKADRDESC item in zekinfr.ZEKADRDESCList)
                                {
                                   ZekAddressDescriptionDto zekAddressDescriptionDto = new ZekAddressDescriptionDto();
                                   zekAddressDescriptionDto = Mapper.Map<ZEKADRDESC,ZekAddressDescriptionDto>(item);
                                   synonyme.Add(zekAddressDescriptionDto);
                                }


                        if (zekinfr.ZEKFC == null)
                            context.Entry(zekinfr).Reference(f => f.ZEKFC).Load();
                        
                            if (zekinfr.ZEKFC != null)
                                outDto.FoundContracts = getZekFoundContracts(zekinfr.ZEKFC, context);
                    
                    }
                    return outDto;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveInformativanfrageOutput ", ex);
                    throw ex;
                }
            }
        }

        private ZekFoundContractsDto getZekFoundContracts(ZEKFC zekfc, DdIcExtended context)
        {
            ZekFoundContractsDto zekfoundContractsDto = new ZekFoundContractsDto();
            if (zekfc != null)
            {
                zekfoundContractsDto.GesamtEngagement = (int?)zekfc.GESAMTENGAGEMENT;
                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKAICList).IsLoaded)
                    context.Entry(zekfc).Collection(f => f.ZEKAICList).Load();
                
                if (zekfc.ZEKAICList.Count > 0)
                {
                    List<ZekAmtsinformationDescriptionDto> zekaicList = new List<ZekAmtsinformationDescriptionDto>();
                    foreach (ZEKAIC zekaic in zekfc.ZEKAICList)
                    {
                        ZekAmtsinformationDescriptionDto zekAmtsinformationDescriptionDto = new ZekAmtsinformationDescriptionDto();
                        zekAmtsinformationDescriptionDto = Mapper.Map<ZEKAIC,ZekAmtsinformationDescriptionDto>(zekaic);
                        zekaicList.Add(zekAmtsinformationDescriptionDto);
                        zekfoundContractsDto.AmtsinformationContracts = zekaicList;
                    }
                }
                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKBDCList).IsLoaded)
                    context.Entry(zekfc).Collection(f => f.ZEKBDCList).Load();
                
                if (zekfc.ZEKBDCList.Count > 0)
                {
                    List<ZekBardarlehenDescriptionDto> zekbdcList = new List<ZekBardarlehenDescriptionDto>();
                    foreach (ZEKBDC zekbdc  in zekfc.ZEKBDCList)
                    {
                        ZekBardarlehenDescriptionDto  zekBardarlehenDescriptionDto = new ZekBardarlehenDescriptionDto();
                        zekBardarlehenDescriptionDto = Mapper.Map<ZEKBDC,ZekBardarlehenDescriptionDto>(zekbdc);
                        zekbdcList.Add(zekBardarlehenDescriptionDto);
                        zekfoundContractsDto.BardarlehenContracts = zekbdcList;
                    }
                }

                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKFKCList).IsLoaded)
                    context.Entry(zekfc).Collection(f => f.ZEKFKCList).Load();
                
                if (zekfc.ZEKFKCList.Count > 0)
                {
                    List<ZekFestkreditDescriptionDto> zekfkcList = new List<ZekFestkreditDescriptionDto>();
                    foreach (ZEKFKC zekfkc in zekfc.ZEKFKCList)
                    {
                        ZekFestkreditDescriptionDto zekFestkreditDescriptionDto = new ZekFestkreditDescriptionDto();
                        zekFestkreditDescriptionDto  = Mapper.Map<ZEKFKC,ZekFestkreditDescriptionDto>(zekfkc);
                        zekfkcList.Add(zekFestkreditDescriptionDto);
                        zekfoundContractsDto.FestkreditContracts = zekfkcList;
                    }
                }

                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKKECList).IsLoaded)
                    context.Entry(zekfc).Collection(f => f.ZEKKECList).Load();
                
                if (zekfc.ZEKKECList.Count > 0)
                {
                    List<ZekKartenengagementDescriptionDto> zekkecList = new List<ZekKartenengagementDescriptionDto>();
                    foreach (ZEKKEC zekkec in zekfc.ZEKKECList)
                    {
                        ZekKartenengagementDescriptionDto zekKartenengagementDescriptionDto = new ZekKartenengagementDescriptionDto();
                        zekKartenengagementDescriptionDto = Mapper.Map<ZEKKEC,ZekKartenengagementDescriptionDto>(zekkec);
                        zekkecList.Add(zekKartenengagementDescriptionDto);
                        zekfoundContractsDto.KartenengagementContracts = zekkecList;
                    }
                }

                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKKICList).IsLoaded)
                    context.Entry(zekfc).Collection(f => f.ZEKKICList).Load();
                
                if (zekfc.ZEKKICList.Count > 0)
                {
                    List<ZekKarteninformationDescriptionDto> zekkicListe = new List<ZekKarteninformationDescriptionDto>();
                    foreach (ZEKKIC zekkic in zekfc.ZEKKICList)
                    {
                        ZekKarteninformationDescriptionDto zekKarteninformationDescriptionDto = new ZekKarteninformationDescriptionDto();
                        zekKarteninformationDescriptionDto = Mapper.Map<ZEKKIC,ZekKarteninformationDescriptionDto>(zekkic);
                        zekkicListe.Add(zekKarteninformationDescriptionDto);
                        zekfoundContractsDto.KarteninformationContracts = zekkicListe;
                    }
                }
                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKKKCList).IsLoaded)
                    context.Entry(zekfc).Collection(f => f.ZEKKKCList).Load();
                
                if (zekfc.ZEKKKCList.Count > 0)
                {
                    List<ZekKontokorrentkreditDescriptionDto> zekkkcList  = new List<ZekKontokorrentkreditDescriptionDto>();
                    foreach (ZEKKKC zekkkc in zekfc.ZEKKKCList)
                    {
                        ZekKontokorrentkreditDescriptionDto zekKontokorrentkreditDescriptionDto = new ZekKontokorrentkreditDescriptionDto();
                        zekKontokorrentkreditDescriptionDto = Mapper.Map<ZEKKKC,ZekKontokorrentkreditDescriptionDto>(zekkkc);
                        zekkkcList.Add(zekKontokorrentkreditDescriptionDto);
                        zekfoundContractsDto.KontokorrentkreditContracts = zekkkcList;
                    }
                }
                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKKGCList).IsLoaded)
                context.Entry(zekfc).Collection(f => f.ZEKKGCList).Load();
                
                if (zekfc.ZEKKGCList.Count > 0)
                {
                    List<ZekKreditgesuchDescriptionDto> zekkgcList = new List<ZekKreditgesuchDescriptionDto>();
                    foreach (ZEKKGC zekkgc in zekfc.ZEKKGCList)
                    {
                        ZekKreditgesuchDescriptionDto zekKreditgesuchDescriptionDto = new ZekKreditgesuchDescriptionDto();
                        zekKreditgesuchDescriptionDto = Mapper.Map<ZEKKGC,ZekKreditgesuchDescriptionDto>(zekkgc);
                        zekkgcList.Add(zekKreditgesuchDescriptionDto);
                        zekfoundContractsDto.KreditgesuchContracts = zekkgcList;
                    }
                }
                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKLMCList).IsLoaded)
                context.Entry(zekfc).Collection(f => f.ZEKLMCList).Load();
                
                if (zekfc.ZEKLMCList.Count >0)
                {
                    List<ZekLeasingMietvertragDescriptionDto> zeklmcList = new List<ZekLeasingMietvertragDescriptionDto>();
                    foreach (ZEKLMC zeklmc in zekfc.ZEKLMCList)
                    {
                        ZekLeasingMietvertragDescriptionDto zekLeasingMietvertragDescriptionDto = new ZekLeasingMietvertragDescriptionDto();
                        zekLeasingMietvertragDescriptionDto = Mapper.Map<ZEKLMC,ZekLeasingMietvertragDescriptionDto>(zeklmc);
                        zeklmcList.Add(zekLeasingMietvertragDescriptionDto);
                        zekfoundContractsDto.LeasingMietvertragContracts = zeklmcList;
                    }
                }
                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKSSCList).IsLoaded)
                context.Entry(zekfc).Collection(f => f.ZEKSSCList).Load();
                
                if (zekfc.ZEKSSCList.Count > 0)
                {
                    List<ZekSolidarschuldnerDescriptionDto>  zeksscList = new List<ZekSolidarschuldnerDescriptionDto>();
                    foreach (ZEKSSC zekssc in zekfc.ZEKSSCList)
                    {
                        ZekSolidarschuldnerDescriptionDto zekSolidarschuldnerDescriptionDto = new ZekSolidarschuldnerDescriptionDto();
                        zekSolidarschuldnerDescriptionDto = Mapper.Map<ZEKSSC,ZekSolidarschuldnerDescriptionDto>(zekssc);
                        zeksscList.Add(zekSolidarschuldnerDescriptionDto);
                        zekfoundContractsDto.SolidarschuldnerContracts = zeksscList;
                    }
                }
                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKTKCList).IsLoaded)
                context.Entry(zekfc).Collection(f => f.ZEKTKCList).Load();
                
                if (zekfc.ZEKTKCList.Count > 0)
                {
                    List<ZekTeilzahlungskreditDescriptionDto> zektkcList = new  List<ZekTeilzahlungskreditDescriptionDto>();
                    foreach (ZEKTKC zektkc in zekfc.ZEKTKCList)
                    {
                        ZekTeilzahlungskreditDescriptionDto zekTeilzahlungskreditDescriptionDto = new ZekTeilzahlungskreditDescriptionDto();
                        zekTeilzahlungskreditDescriptionDto  = Mapper.Map<ZEKTKC,ZekTeilzahlungskreditDescriptionDto>(zektkc);
                        zektkcList.Add(zekTeilzahlungskreditDescriptionDto);
                        zekfoundContractsDto.TeilzahlungskreditContracts = zektkcList;
                    }
                }
                if (zekfc != null && !context.Entry(zekfc).Collection(f => f.ZEKUKCList).IsLoaded)
                context.Entry(zekfc).Collection(f => f.ZEKUKCList).Load();
                
                if (zekfc.ZEKUKCList.Count > 0)
                {
                    List<ZekUeberziehungskreditDescriptionDto> zekuckList = new List<ZekUeberziehungskreditDescriptionDto>();
                    foreach (ZEKUKC zekukc in zekfc.ZEKUKCList)
                    {
                        ZekUeberziehungskreditDescriptionDto zekUeberziehungskreditDescriptionDto = new ZekUeberziehungskreditDescriptionDto();
                        zekUeberziehungskreditDescriptionDto = Mapper.Map<ZEKUKC,ZekUeberziehungskreditDescriptionDto>(zekukc);
                        zekuckList.Add(zekUeberziehungskreditDescriptionDto);
                        zekfoundContractsDto.UeberziehnungskreditContracts = zekuckList;
                    }
                }
                return zekfoundContractsDto;
            }
            return null;
        }
      

        /// <summary>
        /// Saves KreditgesuchAblehnenOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveKreditgesuchAblehnenOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTEC3 zekOutEC3 = new ZEKOUTEC3();
                    zekOutEC3.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTEC3.Add(zekOutEC3);

                    ZEKCCRR zekccrr = new ZEKCCRR();
                    if (outDto.ReturnCode != null)
                    {
                        zekccrr.RETCODE = (long?)outDto.ReturnCode.Code;
                        zekccrr.RETTEXT = MyTruncateErrText(outDto.ReturnCode.Text);
                    }
                    if (outDto.TransactionError != null)
                    {
                        zekccrr.ERRCODE = outDto.TransactionError.Code;
                        zekccrr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCCRR.Add(zekccrr);

                    if (outDto.Synonyms != null && outDto.Synonyms.Count != 0)
                    {
                        foreach (ZekAddressDescriptionDto zekAddressDescriptionDto in outDto.Synonyms)
                        {
                            ZEKADRDESC zekadrdescsy = new ZEKADRDESC();
                            zekadrdescsy = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekAddressDescriptionDto);
                            context.ZEKADRDESC.Add(zekadrdescsy);
                            zekadrdescsy.ZEKCCRR = zekccrr;
                        }
                    }
                    zekOutEC3.ZEKCCRR = zekccrr;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveKreditgesuchAblehnenOutput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves VertragsanmeldungInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveVertragsanmeldungInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPEC4 zekInpEC4 = new ZEKINPEC4();

                    zekInpEC4.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP auskunftTyp = AuskunftTypQuery.Single();
                    context.ZEKINPEC4.Add(zekInpEC4);

                    zekInpEC4.ZIELVEREIN = inDto.Zielverein;
                    zekInpEC4.ART = (int)zekInpEC4.AUSKUNFT.AUSKUNFTTYP.SYSAUSKUNFTTYP;
                    zekInpEC4.KREDITGESUCHID = inDto.KreditgesuchID;
                    if (inDto.RequestEntities != null && inDto.RequestEntities.Count != 0)
                    {
                        foreach (ZekRequestEntityDto zekerInDto in inDto.RequestEntities)
                        {
                            ZEKREQEN zekreqen = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(zekerInDto);
                            context.ZEKREQEN.Add(zekreqen);
                            zekreqen.SYSZEKINPEC4 = zekInpEC4.SYSZEKINPEC4;

                            if (zekerInDto.AddressDescription != null)
                            {
                                ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                                zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekerInDto.AddressDescription);
                                context.ZEKADRDESC.Add(zekadrdesc);
                                zekadrdesc.ZEKREQEN = zekreqen;
                            }
                        }
                    }
                    if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKRegisterBardarlehen))
                    {
                        ZEKBDC zekbdc = new ZEKBDC();
                        zekbdc = Mapper.Map<ZekBardarlehenDescriptionDto, ZEKBDC>(inDto.Bardarlehen);
                        context.ZEKBDC.Add(zekbdc);
                        zekInpEC4.ZEKBDC = zekbdc;
                    }
                    if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKRegisterFestkredit))
                    {
                        ZEKFKC zekfkc = new ZEKFKC();
                        zekfkc = Mapper.Map<ZekFestkreditDescriptionDto, ZEKFKC>(inDto.Festkredit);
                        context.ZEKFKC.Add(zekfkc);
                        zekInpEC4.ZEKFKC = zekfkc;
                    }
                    if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKRegisterKontokorrentkredit))
                    {
                        ZEKKKC zekkkc = new ZEKKKC();
                        zekkkc = Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZEKKKC>(inDto.Kontokorrent);
                        context.ZEKKKC.Add(zekkkc);
                        zekInpEC4.ZEKKKC = zekkkc;
                    }
                    if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKRegisterLeasingMietvertrag))
                    {
                        ZEKLMC zeklmc = new ZEKLMC();
                        zeklmc = Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZEKLMC>(inDto.LeasingMietvertrag);
                        context.ZEKLMC.Add(zeklmc);
                        zekInpEC4.ZEKLMC = zeklmc;
                    }
                    if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKRegisterTeilzahlungskredit))
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZEKTKC>(inDto.Teilzahlung);
                        context.ZEKTKC.Add(zektkc);
                        zekInpEC4.ZEKTKC = zektkc;
                    }
                    if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKRegisterTeilzahlungsvertrag))
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungsvertragDescriptionDto, ZEKTKC>(inDto.Teilzahlungvertrag);
                        context.ZEKTKC.Add(zektkc);
                        zekInpEC4.ZEKTKC = zektkc;
                    }
                    if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKMeldungKartenengagement))
                    {
                        ZEKKEC zekkec = new ZEKKEC();
                        zekkec = Mapper.Map<ZekKartenengagementDescriptionDto, ZEKKEC>(inDto.Kartenengagement);
                        context.ZEKKEC.Add(zekkec);
                        zekInpEC4.ZEKKEC = zekkec;
                    }
                    if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKMeldungUeberziehungskredit))
                    {
                        ZEKUKC zekukc = new ZEKUKC();
                        zekukc = Mapper.Map<ZekUeberziehungskreditDescriptionDto, ZEKUKC>(inDto.Ueberziehungskredit);
                        context.ZEKUKC.Add(zekukc);
                        zekInpEC4.ZEKUKC = zekukc;
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveVertragsanmeldungInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves VertragsanmeldungOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveVertragsanmeldungOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTEC4 zekOutEC4 = new ZEKOUTEC4();
                    zekOutEC4.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTEC4.Add(zekOutEC4);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutEC4.ZEKCMR = zekcmr;

                    if (outDto.Responses != null)
                    {
                        foreach (ZekResponseDescriptionDto zekResponseDescriptionDto in outDto.Responses)
                        {
                            saveZekResponseDescription(zekResponseDescriptionDto, zekcmr, context);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveVertragsanmeldungOutput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves Addressänderung Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveUpdateAddressInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPEC6 zekInpEC6 = new ZEKINPEC6();
                    zekInpEC6.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKINPEC6.Add(zekInpEC6);

                    if (inDto.RequestEntity != null)
                    {
                        ZEKREQEN zekreqen = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(inDto.RequestEntity);
                        context.ZEKREQEN.Add(zekreqen);
                        zekInpEC6.ZEKREQEN = zekreqen;

                        if (inDto.RequestEntity.AddressDescription != null)
                        {
                            ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                            zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(inDto.RequestEntity.AddressDescription);
                            context.ZEKADRDESC.Add(zekadrdesc);
                            zekadrdesc.ZEKREQEN = zekreqen;
                        }
                    }

                    if (inDto.RequestEntityNew != null)
                    {
                        ZEKREQEN zekreqennew = new ZEKREQEN();
                        zekreqennew = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(inDto.RequestEntityNew);
                        context.ZEKREQEN.Add(zekreqennew);
                        context.SaveChanges();
                        zekInpEC6.SYSZEKREQENNEW = zekreqennew.SYSZEKREQEN;

                        if (inDto.RequestEntityNew.AddressDescription != null)
                        {
                            ZEKADRDESC zekadrdescnew = new ZEKADRDESC();
                            zekadrdescnew = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(inDto.RequestEntityNew.AddressDescription);
                            context.ZEKADRDESC.Add(zekadrdescnew);
                            zekadrdescnew.ZEKREQEN = zekreqennew;
                        }

                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveUpdateAddressInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves Addressänderung Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveUpdateAddressOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTEC6 zekOutEC6 = new ZEKOUTEC6();
                    zekOutEC6.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTEC6.Add(zekOutEC6);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutEC6.ZEKCMR = zekcmr;

                    if (outDto.ReturnCode != null)
                    {
                        ZEKRESDESC zekResDesc = new ZEKRESDESC();
                        zekResDesc.REFNO = 0;
                        zekResDesc.RETCODE = outDto.ReturnCode.Code;
                        zekResDesc.RETTEXT = MyTruncateErrText(outDto.ReturnCode.Text);
                        zekResDesc.ZEKCMR = zekcmr;
                        context.ZEKRESDESC.Add(zekResDesc);
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveUpdateAddressOutput: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves Vertragsänderung Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveUpdateVertragInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPEC7 zekInpEC7 = new ZEKINPEC7();

                    zekInpEC7.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP AuskunftTyp = AuskunftTypQuery.Single();
                    context.ZEKINPEC7.Add(zekInpEC7);

                    zekInpEC7.ART = (int)zekInpEC7.AUSKUNFT.AUSKUNFTTYP.SYSAUSKUNFTTYP;
                    if (inDto.RequestEntities != null && inDto.RequestEntities.Count != 0)
                    {
                        foreach (ZekRequestEntityDto zekerInDto in inDto.RequestEntities)
                        {
                            ZEKREQEN zekreqen = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(zekerInDto);
                            context.ZEKREQEN.Add(zekreqen);
                            zekreqen.SYSZEKINPEC7 = zekInpEC7.SYSZEKINPEC7;

                            if (zekerInDto.AddressDescription != null)
                            {
                                ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                                zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekerInDto.AddressDescription);
                                context.ZEKADRDESC.Add(zekadrdesc);
                                zekadrdesc.ZEKREQEN = zekreqen;
                            }
                        }
                    }
                    if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateBardarlehen))
                    {
                        ZEKBDC zekbdc = new ZEKBDC();
                        zekbdc = Mapper.Map<ZekBardarlehenDescriptionDto, ZEKBDC>(inDto.Bardarlehen);
                        context.ZEKBDC.Add(zekbdc);
                        zekInpEC7.ZEKBDC = zekbdc;

                        ZEKBDC zekbdcnew = new ZEKBDC();
                        zekbdcnew = Mapper.Map<ZekBardarlehenDescriptionDto, ZEKBDC>(inDto.BardarlehenNew);
                        context.ZEKBDC.Add(zekbdcnew);
                        context.SaveChanges();
                        zekInpEC7.SYSZEKBDCNEW = zekbdcnew.SYSZEKBDC;
                    }
                    if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateFestkredit))
                    {
                        ZEKFKC zekfkc = new ZEKFKC();
                        zekfkc = Mapper.Map<ZekFestkreditDescriptionDto, ZEKFKC>(inDto.Festkredit);
                        context.ZEKFKC.Add(zekfkc);
                        zekInpEC7.ZEKFKC = zekfkc;

                        ZEKFKC zekfkcnew = new ZEKFKC();
                        zekfkcnew = Mapper.Map<ZekFestkreditDescriptionDto, ZEKFKC>(inDto.FestkreditNew);
                        context.ZEKFKC.Add(zekfkcnew);
                        context.SaveChanges();
                        zekInpEC7.SYSZEKFKCNEW = zekfkcnew.SYSZEKFKC;
                    }
                    if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateKontokorrentkredit))
                    {
                        ZEKKKC zekkkc = new ZEKKKC();
                        zekkkc = Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZEKKKC>(inDto.Kontokorrent);
                        context.ZEKKKC.Add(zekkkc);
                        zekInpEC7.ZEKKKC = zekkkc;

                        ZEKKKC zekkkcnew = new ZEKKKC();
                        zekkkcnew = Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZEKKKC>(inDto.KontokorrentNew);
                        context.ZEKKKC.Add(zekkkcnew);
                        context.SaveChanges();
                        zekInpEC7.SYSZEKKKCNEW = zekkkcnew.SYSZEKKKC;
                    }
                    if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateLeasingMietvertrag))
                    {
                        ZEKLMC zeklmc = new ZEKLMC();
                        zeklmc = Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZEKLMC>(inDto.LeasingMietvertrag);
                        context.ZEKLMC.Add(zeklmc);
                        zekInpEC7.ZEKLMC = zeklmc;

                        ZEKLMC zeklmcnew = new ZEKLMC();
                        zeklmcnew = Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZEKLMC>(inDto.LeasingMietvertragNew);
                        context.ZEKLMC.Add(zeklmcnew);
                        context.SaveChanges();
                        zekInpEC7.SYSZEKLMCNEW = zeklmcnew.SYSZEKLMC;
                    }
                    if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateTeilzahlungskredit))
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZEKTKC>(inDto.Teilzahlung);
                        context.ZEKTKC.Add(zektkc);
                        zekInpEC7.ZEKTKC = zektkc;

                        ZEKTKC zektkcnew = new ZEKTKC();
                        zektkcnew = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZEKTKC>(inDto.Teilzahlung);
                        context.ZEKTKC.Add(zektkcnew);
                        context.SaveChanges();
                        zekInpEC7.SYSZEKTKCNEW = zektkcnew.SYSZEKTKC;
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveUpdateVertragInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves Vertragsänderung Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveUpdateVertragOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTEC7 zekOutEC7 = new ZEKOUTEC7();
                    zekOutEC7.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTEC7.Add(zekOutEC7);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutEC7.ZEKCMR = zekcmr;

                    if (outDto.Responses != null)
                    {
                        foreach (ZekResponseDescriptionDto zekResponseDescriptionDto in outDto.Responses)
                        {
                            saveZekResponseDescription(zekResponseDescriptionDto, zekcmr, context);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveUpdateVertragOutput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves Vertrags-Abmeldung Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveVertragsabmeldungInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPEC5 zekInpEC5 = new ZEKINPEC5();

                    zekInpEC5.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP AuskunftTyp = AuskunftTypQuery.Single();
                    context.ZEKINPEC5.Add(zekInpEC5);

                    zekInpEC5.ART = (int)zekInpEC5.AUSKUNFT.AUSKUNFTTYP.SYSAUSKUNFTTYP;
                    if (inDto.RequestEntities != null && inDto.RequestEntities.Count != 0)
                    {
                        foreach (ZekRequestEntityDto zekerInDto in inDto.RequestEntities)
                        {
                            ZEKREQEN zekreqen = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(zekerInDto);
                            context.ZEKREQEN.Add(zekreqen);
                            zekreqen.SYSZEKINPEC5 = zekInpEC5.SYSZEKINPEC5;

                            if (zekerInDto.AddressDescription != null)
                            {
                                ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                                zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekerInDto.AddressDescription);
                                context.ZEKADRDESC.Add(zekadrdesc);
                                zekadrdesc.ZEKREQEN = zekreqen;
                            }
                        }
                    }
                    if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKCloseBardarlehen))
                    {
                        ZEKBDC zekbdc = new ZEKBDC();
                        zekbdc = Mapper.Map<ZekBardarlehenDescriptionDto, ZEKBDC>(inDto.Bardarlehen);
                        context.ZEKBDC.Add(zekbdc);
                        zekInpEC5.ZEKBDC = zekbdc;
                    }
                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKCloseFestkredit))
                    {
                        ZEKFKC zekfkc = new ZEKFKC();
                        zekfkc = Mapper.Map<ZekFestkreditDescriptionDto, ZEKFKC>(inDto.Festkredit);
                        context.ZEKFKC.Add(zekfkc);
                        zekInpEC5.ZEKFKC = zekfkc;
                    }
                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKCloseKontokorrentkredit))
                    {
                        ZEKKKC zekkkc = new ZEKKKC();
                        zekkkc = Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZEKKKC>(inDto.Kontokorrent);
                        context.ZEKKKC.Add(zekkkc);
                        zekInpEC5.ZEKKKC = zekkkc;
                    }
                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKCloseLeasingMietvertrag))
                    {
                        ZEKLMC zeklmc = new ZEKLMC();
                        zeklmc = Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZEKLMC>(inDto.LeasingMietvertrag);
                        context.ZEKLMC.Add(zeklmc);
                        zekInpEC5.ZEKLMC = zeklmc;
                    }
                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKCloseTeilzahlungskredit))
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZEKTKC>(inDto.Teilzahlung);
                        context.ZEKTKC.Add(zektkc);
                        zekInpEC5.ZEKTKC = zektkc;
                    }
                    else if (AuskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKCloseTeilzahlungsvertrag))
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungsvertragDescriptionDto, ZEKTKC>(inDto.Teilzahlungvertrag);
                        context.ZEKTKC.Add(zektkc);
                        zekInpEC5.ZEKTKC = zektkc;
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveVertragsabmeldungInput: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves Vertragsabmeldung Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveVertragsabmeldungOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTEC5 zekOutEC5 = new ZEKOUTEC5();
                    zekOutEC5.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTEC5.Add(zekOutEC5);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutEC5.ZEKCMR = zekcmr;

                    if (outDto.Responses != null)
                    {
                        foreach (ZekResponseDescriptionDto zekResponseDescriptionDto in outDto.Responses)
                        {
                            saveZekResponseDescription(zekResponseDescriptionDto, zekcmr, context);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveVertragsabmeldungOutput: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Finds ZEK Input data by sysAuskunft and maps it to ZekInDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>ZekInDto, filled with input for ZEK WS</returns>
        public ZekInDto FindBySysId(long sysAuskunft)
        {
            ZekInDto rval = new ZekInDto();

            //comment
            using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended contextOd = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
            {
                //DDLKPSPOS
                DDLKPSPOS spos = contextOd.DDLKPSPOS.Where((p) => p.AREA == "AUSKUNFT" && p.SYSID == sysAuskunft).FirstOrDefault();
                if (spos != null)
                    rval.Bemerkung = spos.VALUE;
            }

            //Regular ZEK input data
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP auskunftTyp = AuskunftTypQuery.Single();

                    // ZEKKreditgesuchNeu EC1
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKKreditgesuchNeu)
                    {
                        var kreditgesuchNeuQuery = from zekinpec1 in context.ZEKINPEC1
                                                   join Auskunft in context.AUSKUNFT on zekinpec1.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                                                   where Auskunft.SYSAUSKUNFT == sysAuskunft
                                                   select zekinpec1;
                        ZEKINPEC1 zekInpEC1 = kreditgesuchNeuQuery.Single();
                        rval = Mapper.Map<ZEKINPEC1, ZekInDto>(kreditgesuchNeuQuery.Single());

                        if (zekInpEC1 != null && !context.Entry(zekInpEC1).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC1).Collection(f => f.ZEKREQENList).Load();
                        
                        if (zekInpEC1.ZEKREQENList != null && zekInpEC1.ZEKREQENList.Count != 0)
                        {
                            rval.RequestEntities = getRequestEntities(zekInpEC1.ZEKREQENList.ToList(), context);
                        }
                        return rval;
                    }

                    // ZEKInformativabfrage EC2
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKInformativabfrage)
                    {
                        var informativabfrageQuery = from zekinpec2 in context.ZEKINPEC2
                                                     join Auskunft in context.AUSKUNFT on zekinpec2.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                                                     where Auskunft.SYSAUSKUNFT == sysAuskunft
                                                     select zekinpec2;
                        ZEKINPEC2 zekInpEC2 = informativabfrageQuery.Single();
                        rval = Mapper.Map<ZEKINPEC2, ZekInDto>(zekInpEC2);
                        
                        if (zekInpEC2.ZEKREQEN == null)
                            context.Entry(zekInpEC2).Reference(f => f.ZEKREQEN).Load();
                        rval.RequestEntity = getRequestEntity(zekInpEC2.ZEKREQEN, context);

                        return rval;
                    }

                    // ZEKKreditgesuchAblehnen EC3
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKKreditgesuchAblehnen)
                    {
                        var kreditgesuchAblehnenQuery = from zekinpec3 in context.ZEKINPEC3
                                                        join Auskunft in context.AUSKUNFT on zekinpec3.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                                                        where Auskunft.SYSAUSKUNFT == sysAuskunft
                                                        select zekinpec3;
                        ZEKINPEC3 zekInpEC3 = kreditgesuchAblehnenQuery.Single();

                        if (zekInpEC3.ZEKREQEN == null)
                            context.Entry(zekInpEC3).Reference(f => f.ZEKREQEN).Load();
                        
                        rval = Mapper.Map<ZEKINPEC3, ZekInDto>(zekInpEC3);
                        if (zekInpEC3.ZEKREQEN == null)
                            context.Entry(zekInpEC3).Reference(f => f.ZEKREQEN).Load();
                        
                        rval.RequestEntity = rval.RequestEntity = getRequestEntity(zekInpEC3.ZEKREQEN, context);
                        return rval;
                    }

                    // ZEKUpdateAddress EC6
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKUpdateAddress)
                    {
                        var updateAddressQuery = from zekinpec6 in context.ZEKINPEC6
                                                 join Auskunft in context.AUSKUNFT on zekinpec6.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                                                 where Auskunft.SYSAUSKUNFT == sysAuskunft
                                                 select zekinpec6;
                        ZEKINPEC6 zekInpEC6 = updateAddressQuery.Single();

                        rval = Mapper.Map<ZEKINPEC6, ZekInDto>(zekInpEC6);
                        
                        if (zekInpEC6.ZEKREQEN == null)
                            context.Entry(zekInpEC6).Reference(f => f.ZEKREQEN).Load();
                        rval.RequestEntity = getRequestEntity(zekInpEC6.ZEKREQEN, context);
                        if (zekInpEC6.SYSZEKREQENNEW != null)
                        {
                            var reqenQuery = from reqen in context.ZEKREQEN
                                             where reqen.SYSZEKREQEN == zekInpEC6.SYSZEKREQENNEW
                                             select reqen;
                            rval.RequestEntityNew = getRequestEntity(reqenQuery.Single(), context);
                        }
                        return rval;
                    }

                    // ZEKRegisterBardarlehen EC4
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKRegisterBardarlehen)
                    {
                        ZEKINPEC4 zekInpEC4 = getZekInpEC4(sysAuskunft, context);

                        rval = Mapper.Map<ZEKINPEC4, ZekInDto>(zekInpEC4);

                        if (zekInpEC4 != null && !context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC4.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC4.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        if (zekInpEC4.ZEKBDC == null)
                            context.Entry(zekInpEC4).Reference(f => f.ZEKBDC).Load();
                        
                        if (zekInpEC4.ZEKBDC != null)
                            rval.Bardarlehen = Mapper.Map<ZEKBDC, ZekBardarlehenDescriptionDto>(zekInpEC4.ZEKBDC);

                        return rval;
                    }

                    // ZEKRegisterFestkredit EC4   
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKRegisterFestkredit)
                    {
                        ZEKINPEC4 zekInpEC4 = getZekInpEC4(sysAuskunft, context);

                        rval = Mapper.Map<ZEKINPEC4, ZekInDto>(zekInpEC4);

                        if (zekInpEC4 != null && !context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC4.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC4.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        if (zekInpEC4.ZEKFKC == null)
                            context.Entry(zekInpEC4).Reference(f => f.ZEKFKC).Load();
                        
                        if (zekInpEC4.ZEKFKC != null)
                            rval.Festkredit = Mapper.Map<ZEKFKC, ZekFestkreditDescriptionDto>(zekInpEC4.ZEKFKC);

                        return rval;
                    }

                    // ZEKRegisterLeasingMietvertrag EC4
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKRegisterLeasingMietvertrag)
                    {
                        ZEKINPEC4 zekInpEC4 = getZekInpEC4(sysAuskunft, context);

                        rval = Mapper.Map<ZEKINPEC4, ZekInDto>(zekInpEC4);

                        if (zekInpEC4 != null && !context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC4.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC4.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC4.ZEKLMC == null)
                            context.Entry(zekInpEC4).Reference(f => f.ZEKLMC).Load();
                        if (zekInpEC4.ZEKLMC != null)
                            rval.LeasingMietvertrag = Mapper.Map<ZEKLMC, ZekLeasingMietvertragDescriptionDto>(zekInpEC4.ZEKLMC);

                        return rval;
                    }

                    // ZEKRegisterTeilzahlungskredit EC4
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKRegisterTeilzahlungskredit)
                    {
                        ZEKINPEC4 zekInpEC4 = getZekInpEC4(sysAuskunft, context);

                        rval = Mapper.Map<ZEKINPEC4, ZekInDto>(zekInpEC4);

                        if (zekInpEC4 != null && !context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC4.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC4.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        if (zekInpEC4.ZEKTKC == null)
                            context.Entry(zekInpEC4).Reference(f => f.ZEKTKC).Load();
                        
                        if (zekInpEC4.ZEKTKC != null)
                            rval.Teilzahlung = Mapper.Map<ZEKTKC, ZekTeilzahlungskreditDescriptionDto>(zekInpEC4.ZEKTKC);

                        return rval;
                    }


                    // ZEKRegisterTeilzahlungsvertrag EC4
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKRegisterTeilzahlungsvertrag)
                    {
                        ZEKINPEC4 zekInpEC4 = getZekInpEC4(sysAuskunft, context);

                        rval = Mapper.Map<ZEKINPEC4, ZekInDto>(zekInpEC4);

                        if (zekInpEC4 != null && !context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC4.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC4.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC4.ZEKTKC == null)
                            context.Entry(zekInpEC4).Reference(f => f.ZEKTKC).Load();
                        if (zekInpEC4.ZEKTKC != null)
                            rval.Teilzahlungvertrag = Mapper.Map<ZEKTKC, ZekTeilzahlungsvertragDescriptionDto>(zekInpEC4.ZEKTKC);

                        return rval;
                    }

                    // ZEKRegisterKontokorrentkredit EC4
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKRegisterKontokorrentkredit)
                    {
                        ZEKINPEC4 zekInpEC4 = getZekInpEC4(sysAuskunft, context);

                        rval = Mapper.Map<ZEKINPEC4, ZekInDto>(zekInpEC4);

                        if (zekInpEC4 != null && !context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC4.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC4.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC4.ZEKKKC == null)
                            context.Entry(zekInpEC4).Reference(f => f.ZEKKKC).Load();
                        if (zekInpEC4.ZEKKKC != null)
                            rval.Kontokorrent = Mapper.Map<ZEKKKC, ZekKontokorrentkreditDescriptionDto>(zekInpEC4.ZEKKKC);

                        return rval;
                    }

                    // ZEKMeldungKartenengagement EC4
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKMeldungKartenengagement)
                    {
                        ZEKINPEC4 zekInpEC4 = getZekInpEC4(sysAuskunft, context);

                        rval = Mapper.Map<ZEKINPEC4, ZekInDto>(zekInpEC4);

                        if (zekInpEC4 != null && !context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC4.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC4.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        if (zekInpEC4.ZEKKEC == null)
                            context.Entry(zekInpEC4).Reference(f => f.ZEKKEC).Load();
                        
                        if (zekInpEC4.ZEKKEC != null)
                            rval.Kartenengagement = Mapper.Map<ZEKKEC, ZekKartenengagementDescriptionDto>(zekInpEC4.ZEKKEC);

                        return rval;
                    }

                    // ZEKMeldungUeberziehungskredit EC4
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKMeldungUeberziehungskredit)
                    {
                        ZEKINPEC4 zekInpEC4 = getZekInpEC4(sysAuskunft, context);

                        rval = Mapper.Map<ZEKINPEC4, ZekInDto>(zekInpEC4);

                        if (zekInpEC4 != null && !context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC4).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC4.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC4.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC4.ZEKUKC == null)
                            context.Entry(zekInpEC4).Reference(f => f.ZEKUKC).Load();
                        if (zekInpEC4.ZEKUKC != null)
                            rval.Ueberziehungskredit = Mapper.Map<ZEKUKC, ZekUeberziehungskreditDescriptionDto>(zekInpEC4.ZEKUKC);

                        return rval;
                    }

                    // ZEK Close Bardarlehen EC5 (Vertragsabmeldung)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKCloseBardarlehen)
                    {
                        ZEKINPEC5 zekInpEC5 = getZekInpEC5(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC5, ZekInDto>(zekInpEC5);

                        if (zekInpEC5 != null && !context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC5.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC5.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC5.ZEKBDC == null)
                            context.Entry(zekInpEC5).Reference(f => f.ZEKBDC).Load();
                        if (zekInpEC5.ZEKBDC != null)
                            rval.Bardarlehen = Mapper.Map<ZEKBDC, ZekBardarlehenDescriptionDto>(zekInpEC5.ZEKBDC);

                        return rval;
                    }

                    // ZEK Close LeasingMietvertrag EC5 (Vertragsabmeldung)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKCloseLeasingMietvertrag)
                    {
                        ZEKINPEC5 zekInpEC5 = getZekInpEC5(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC5, ZekInDto>(zekInpEC5);

                        if (zekInpEC5 != null && !context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC5.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC5.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC5.ZEKLMC == null)
                            context.Entry(zekInpEC5).Reference(f => f.ZEKLMC).Load();
                        if (zekInpEC5.ZEKLMC != null)
                            rval.LeasingMietvertrag = Mapper.Map<ZEKLMC, ZekLeasingMietvertragDescriptionDto>(zekInpEC5.ZEKLMC);

                        return rval;
                    }

                    // ZEK Close Festkredit EC5 (Vertragsabmeldung)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKCloseFestkredit)
                    {
                        ZEKINPEC5 zekInpEC5 = getZekInpEC5(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC5, ZekInDto>(zekInpEC5);

                        if (zekInpEC5 != null && !context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).Load();
                        
                        if (zekInpEC5.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC5.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC5.ZEKFKC == null)
                            context.Entry(zekInpEC5).Reference(f => f.ZEKFKC).Load();
                        if (zekInpEC5.ZEKFKC != null)
                            rval.Festkredit = Mapper.Map<ZEKFKC, ZekFestkreditDescriptionDto>(zekInpEC5.ZEKFKC);

                        return rval;
                    }

                    // ZEK Close Teilzahlungskredit EC5 (Vertragsabmeldung)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKCloseTeilzahlungskredit)
                    {
                        ZEKINPEC5 zekInpEC5 = getZekInpEC5(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC5, ZekInDto>(zekInpEC5);


                        if (zekInpEC5 != null && !context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC5.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC5.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC5.ZEKTKC == null)
                            context.Entry(zekInpEC5).Reference(f => f.ZEKTKC).Load();
                        if (zekInpEC5.ZEKTKC != null)
                            rval.Teilzahlung = Mapper.Map<ZEKTKC, ZekTeilzahlungskreditDescriptionDto>(zekInpEC5.ZEKTKC);

                        return rval;
                    }


                    // ZEK Close Teilzahlungsvertrag EC5 (Vertragsabmeldung)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKCloseTeilzahlungsvertrag)
                    {
                        ZEKINPEC5 zekInpEC5 = getZekInpEC5(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC5, ZekInDto>(zekInpEC5);

                        if (zekInpEC5 != null && !context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).Load();
                        
                        if (zekInpEC5.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC5.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC5.ZEKTKC == null)
                            context.Entry(zekInpEC5).Reference(f => f.ZEKTKC).Load();
                        if (zekInpEC5.ZEKTKC != null)
                            rval.Teilzahlungvertrag = Mapper.Map<ZEKTKC, ZekTeilzahlungsvertragDescriptionDto>(zekInpEC5.ZEKTKC);

                        return rval;
                    }

                    // ZEK Close Kontokorrentkredit EC5 (Vertragsabmeldung)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKCloseKontokorrentkredit)
                    {
                        ZEKINPEC5 zekInpEC5 = getZekInpEC5(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC5, ZekInDto>(zekInpEC5);


                        if (zekInpEC5 != null && !context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC5).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC5.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC5.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC5.ZEKKKC == null)
                            context.Entry(zekInpEC5).Reference(f => f.ZEKKKC).Load();
                        if (zekInpEC5.ZEKKKC != null)
                            rval.Kontokorrent = Mapper.Map<ZEKKKC, ZekKontokorrentkreditDescriptionDto>(zekInpEC5.ZEKKKC);

                        return rval;
                    }

                    // ZEKUpdateBardarlehen EC7
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKUpdateBardarlehen)
                    {
                        ZEKINPEC7 zekInpEC7 = getZekInpEC7(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC7, ZekInDto>(zekInpEC7);


                        if (zekInpEC7 != null && !context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC7.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC7.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        if (zekInpEC7.ZEKBDC == null)
                            context.Entry(zekInpEC7).Reference(f => f.ZEKBDC).Load();
                        if (zekInpEC7.ZEKBDC != null)
                            rval.Bardarlehen = Mapper.Map<ZEKBDC, ZekBardarlehenDescriptionDto>(zekInpEC7.ZEKBDC);

                        if (zekInpEC7.SYSZEKBDCNEW != null)
                        {
                            var bdcQuery = from zekbdc in context.ZEKBDC
                                           where zekbdc.SYSZEKBDC == zekInpEC7.SYSZEKBDCNEW
                                           select zekbdc;
                            rval.BardarlehenNew = Mapper.Map<ZEKBDC, ZekBardarlehenDescriptionDto>(bdcQuery.Single());
                        }
                        return rval;
                    }

                    // ZEKUpdateFestkredit EC7
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKUpdateFestkredit)
                    {
                        ZEKINPEC7 zekInpEC7 = getZekInpEC7(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC7, ZekInDto>(zekInpEC7);


                        if (zekInpEC7 != null && !context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC7.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC7.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        if (zekInpEC7 != null && zekInpEC7.ZEKFKC == null)
                            context.Entry(zekInpEC7).Reference(f => f.ZEKFKC).Load();
                        
                        if (zekInpEC7.ZEKFKC != null)
                            rval.Festkredit = Mapper.Map<ZEKFKC, ZekFestkreditDescriptionDto>(zekInpEC7.ZEKFKC);

                        if (zekInpEC7.SYSZEKFKCNEW != null)
                        {
                            var fkcQuery = from zekfkc in context.ZEKFKC
                                           where zekfkc.SYSZEKFKC == zekInpEC7.SYSZEKFKCNEW
                                           select zekfkc;
                            rval.FestkreditNew = Mapper.Map<ZEKFKC, ZekFestkreditDescriptionDto>(fkcQuery.Single());
                        }

                        return rval;
                    }

                    // ZEKUpdateLeasingMietvertrag
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKUpdateLeasingMietvertrag)
                    {
                        ZEKINPEC7 zekInpEC7 = getZekInpEC7(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC7, ZekInDto>(zekInpEC7);


                        if (zekInpEC7 != null && !context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC7.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC7.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }
                        if (zekInpEC7 != null && zekInpEC7.ZEKLMC == null)
                            context.Entry(zekInpEC7).Reference(f => f.ZEKLMC).Load();
                        
                        if (zekInpEC7.ZEKLMC != null)
                            rval.LeasingMietvertrag = Mapper.Map<ZEKLMC, ZekLeasingMietvertragDescriptionDto>(zekInpEC7.ZEKLMC);

                        if (zekInpEC7.SYSZEKLMCNEW != null)
                        {
                            var kmlcQuery = from zeklkc in context.ZEKLMC
                                            where zeklkc.SYSZEKLMC == zekInpEC7.SYSZEKLMCNEW
                                            select zeklkc;
                            rval.LeasingMietvertragNew = Mapper.Map<ZEKLMC, ZekLeasingMietvertragDescriptionDto>(kmlcQuery.Single());

                        }
                        return rval;
                    }

                    // ZEKUpdateKontokorrentkredit
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKUpdateKontokorrentkredit)
                    {
                        ZEKINPEC7 zekInpEC7 = getZekInpEC7(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC7, ZekInDto>(zekInpEC7);


                        if (zekInpEC7 != null && !context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC7.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC7.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC7 != null && zekInpEC7.ZEKKKC == null)
                            context.Entry(zekInpEC7).Reference(f => f.ZEKKKC).Load();
                        if (zekInpEC7.ZEKKKC != null)
                            rval.Kontokorrent = Mapper.Map<ZEKKKC, ZekKontokorrentkreditDescriptionDto>(zekInpEC7.ZEKKKC);

                        if (zekInpEC7.SYSZEKKKCNEW != null)
                        {
                            var kkcQuery = from zekkkc in context.ZEKKKC
                                           where zekkkc.SYSZEKKKC == zekInpEC7.SYSZEKKKCNEW
                                           select zekkkc;
                            rval.KontokorrentNew = Mapper.Map<ZEKKKC, ZekKontokorrentkreditDescriptionDto>(kkcQuery.Single());
                        }
                        return rval;
                    }

                    // ZEKUpdateTeilzahlungskredit EC7
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKUpdateTeilzahlungskredit)
                    {
                        ZEKINPEC7 zekInpEC7 = getZekInpEC7(sysAuskunft, context);
                        rval = Mapper.Map<ZEKINPEC7, ZekInDto>(zekInpEC7);


                        if (zekInpEC7 != null && !context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).IsLoaded)
                            context.Entry(zekInpEC7).Collection(f => f.ZEKREQENList).Load();
                        if (zekInpEC7.ZEKREQENList != null)
                        {
                            List<ZekRequestEntityDto> requestEntities = getRequestEntities(zekInpEC7.ZEKREQENList.ToList(), context);
                            rval.RequestEntities = requestEntities;
                        }

                        
                        if (zekInpEC7 != null && zekInpEC7.ZEKTKC  == null)
                            context.Entry(zekInpEC7).Reference(f => f.ZEKTKC).Load();
                        if (zekInpEC7.ZEKTKC != null)
                            rval.Teilzahlung = Mapper.Map<ZEKTKC, ZekTeilzahlungskreditDescriptionDto>(zekInpEC7.ZEKTKC);

                        if (zekInpEC7.SYSZEKTKCNEW != null)
                        {
                            var tkcQuery = from zektkc in context.ZEKTKC
                                           where zektkc.SYSZEKTKC == zekInpEC7.SYSZEKTKCNEW
                                           select zektkc;
                            rval.TeilzahlungNew = Mapper.Map<ZEKTKC, ZekTeilzahlungskreditDescriptionDto>(tkcQuery.Single());
                        }
                        return rval;
                    }

                    // ZEKECODE178 Abfrage || Anmeldung
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKeCode178Abfragen || auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKeCode178Abmelden ||
                        auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKeCode178Mutieren || auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKeCode178Anmelden)
                    {
                        var ecode178Query = from zekinpcode178 in context.ZEKINPCODE178
                                            join Auskunft in context.AUSKUNFT on zekinpcode178.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                                            where Auskunft.SYSAUSKUNFT == sysAuskunft
                                            select zekinpcode178;
                        ZEKINPCODE178 inpcode178 = ecode178Query.FirstOrDefault();

                        if (inpcode178.SYSZEKCODE178.HasValue && inpcode178.SYSZEKCODE178.Value > 0)
                        {
                            ZEKCODE178 zc178 = (from z in context.ZEKCODE178
                                                where z.SYSZEKCODE178 == inpcode178.SYSZEKCODE178.Value
                                                select z).FirstOrDefault();
                            rval.ZekeCode178Dto = Mapper.Map<ZEKCODE178, ZekeCode178Dto>(zc178);
                        }

                        if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKeCode178Anmelden)
                        {
                            
                            if (inpcode178 != null && inpcode178.ZEKREQEN == null)
                                context.Entry(inpcode178).Reference(f => f.ZEKREQEN).Load();
                            if (inpcode178.ZEKREQEN != null)
                                rval.RequestEntity = getRequestEntity(inpcode178.ZEKREQEN, context);
                            rval.KreditgesuchID = inpcode178.KREDITGESUCHID;
                            rval.ContractId = inpcode178.CONTRACTID;
                        }
                        return rval;
                    }

                    // ZEKGetARMs
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKgetARMs)
                    {
                        var zekinparmQuery = from zekinparm in context.ZEKINPARM
                                             join Auskunft in context.AUSKUNFT on zekinparm.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                                             where Auskunft.SYSAUSKUNFT == sysAuskunft
                                             select zekinparm;
                        ZEKINPARM inparm = zekinparmQuery.FirstOrDefault();
                        rval.DateLastSuccessfullRequest = inparm.DATELASTSUCCESSFULLREQUEST;
                        return rval;
                    }
                    return rval;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler in ZekDBDao.FindBySysId: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveeCode178AnmeldenInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        public void SaveeCode178AnmeldenInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPCODE178 zekInpeCode178 = new ZEKINPCODE178();
                    zekInpeCode178.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                    ZEKCODE178 zekecode178 = new ZEKCODE178();
                    zekecode178 = Mapper.Map<ZekeCode178Dto, ZEKCODE178>(inDto.ZekeCode178Dto);
                    context.ZEKCODE178.Add(zekecode178);
                    context.SaveChanges();
                    zekInpeCode178.SYSZEKCODE178 = zekecode178.SYSZEKCODE178;
                    zekInpeCode178.KREDITGESUCHID = inDto.KreditgesuchID;
                    zekInpeCode178.CONTRACTID = inDto.ContractId;

                    ZEKREQEN zekreqen = Mapper.Map<ZekRequestEntityDto, ZEKREQEN>(inDto.RequestEntity);
                    context.ZEKREQEN.Add(zekreqen);
                    zekInpeCode178.ZEKREQEN = zekreqen;
                    context.ZEKINPCODE178.Add(zekInpeCode178);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von eCode178AnmeldenInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveeCode178AnmeldenOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        public void SaveeCode178AnmeldenOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTCODE178 zekOutECode178 = new ZEKOUTCODE178();

                    zekOutECode178.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTCODE178.Add(zekOutECode178);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutECode178.ZEKCMR = zekcmr;

                    if (outDto.Responses != null)
                    {
                        foreach (ZekResponseDescriptionDto zekResponseDescriptionDto in outDto.Responses)
                        {
                            saveZekResponseDescription(zekResponseDescriptionDto, zekcmr, context);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von eCode178AnmeldenOutput: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveeCode178MutierenInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        public void SaveeCode178MutierenInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPCODE178 zekInpeCode178 = new ZEKINPCODE178();
                    zekInpeCode178.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    ZEKCODE178 zekecode178 = new ZEKCODE178();
                    zekecode178 = Mapper.Map<ZekeCode178Dto, ZEKCODE178>(inDto.ZekeCode178Dto);
                    context.ZEKCODE178.Add(zekecode178);
                    context.SaveChanges();
                    zekInpeCode178.SYSZEKCODE178 = zekecode178.SYSZEKCODE178;
                    context.ZEKINPCODE178.Add(zekInpeCode178);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von eCode178MutierenInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveeCode178MutierenOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        public void SaveeCode178MutierenOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTCODE178 zekOutECode178 = new ZEKOUTCODE178();

                    zekOutECode178.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTCODE178.Add(zekOutECode178);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutECode178.ZEKCMR = zekcmr;

                    if (outDto.Responses != null)
                    {
                        foreach (ZekResponseDescriptionDto zekResponseDescriptionDto in outDto.Responses)
                        {
                            saveZekResponseDescription(zekResponseDescriptionDto, zekcmr, context);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von eCode178MutierenOutput: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveeCode178AbmeldenInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        public void SaveeCode178AbmeldenInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPCODE178 zekInpeCode178 = new ZEKINPCODE178();
                    zekInpeCode178.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    ZEKCODE178 zekecode178 = new ZEKCODE178();
                    zekecode178 = Mapper.Map<ZekeCode178Dto, ZEKCODE178>(inDto.ZekeCode178Dto);
                    context.ZEKCODE178.Add(zekecode178);
                    context.SaveChanges();
                    zekInpeCode178.SYSZEKCODE178 = zekecode178.SYSZEKCODE178;
                    context.ZEKINPCODE178.Add(zekInpeCode178);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von eCode178AbmeldenInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveeCode178AbmeldenOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        public void SaveeCode178AbmeldenOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTCODE178 zekOutECode178 = new ZEKOUTCODE178();

                    zekOutECode178.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTCODE178.Add(zekOutECode178);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutECode178.ZEKCMR = zekcmr;

                    if (outDto.Responses != null)
                    {
                        foreach (ZekResponseDescriptionDto zekResponseDescriptionDto in outDto.Responses)
                        {
                            saveZekResponseDescription(zekResponseDescriptionDto, zekcmr, context);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveeCode178AbmeldenOutput: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveeCode178AbfrageInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        public void SaveeCode178AbfrageInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPCODE178 zekInpeCode178 = new ZEKINPCODE178();
                    zekInpeCode178.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    ZEKCODE178 zekecode178 = new ZEKCODE178();
                    zekecode178 = Mapper.Map<ZekeCode178Dto, ZEKCODE178>(inDto.ZekeCode178Dto);
                    context.ZEKCODE178.Add(zekecode178);
                    context.SaveChanges();
                    zekInpeCode178.SYSZEKCODE178 = zekecode178.SYSZEKCODE178;
                    context.ZEKINPCODE178.Add(zekInpeCode178);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von eCode178AbfrageInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveeCode178AbfrageOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        public void SaveeCode178AbfrageOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTCODE178 zekOutECode178 = new ZEKOUTCODE178();

                    zekOutECode178.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.ZEKOUTCODE178.Add(zekOutECode178);

                    ZEKCMR zekcmr = new ZEKCMR();
                    zekcmr.KREDITGESUCHID = outDto.KreditgesuchID;
                    zekcmr.KREDITVERTRAGID = outDto.KreditVertragID;
                    zekcmr.ECODEID = outDto.eCodeId;

                    if (outDto.TransactionError != null)
                    {
                        zekcmr.ERRCODE = outDto.TransactionError.Code;
                        zekcmr.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }
                    context.ZEKCMR.Add(zekcmr);
                    zekOutECode178.ZEKCMR = zekcmr;

                    if (outDto.Responses != null)
                    {
                        foreach (ZekResponseDescriptionDto zekResponseDescriptionDto in outDto.Responses)
                        {
                            saveZekResponseDescription(zekResponseDescriptionDto, zekcmr, context);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von eCode178AbfrageOutput: ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveGetARMsInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveGetARMsInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPARM zekInpArm = new ZEKINPARM();
                    zekInpArm.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    zekInpArm.DATELASTSUCCESSFULLREQUEST = inDto.DateLastSuccessfullRequest;
                    context.ZEKINPARM.Add(zekInpArm);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveGetARMsInputInput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SaveGetARMsOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveGetARMsOutput(long sysAuskunft, ZekOutDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKOUTARM zekOutArm = new ZEKOUTARM();

                    zekOutArm.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    zekOutArm.REQUESTDATE = outDto.armResponse.requestDate;
                    context.ZEKOUTARM.Add(zekOutArm);

                    if (outDto.TransactionError != null)
                    {
                        zekOutArm.ERRCODE = outDto.TransactionError.Code;
                        zekOutArm.ERRTEXT = MyTruncateErrText(outDto.TransactionError.Text);
                    }

                    if (outDto.ReturnCode != null)
                    {
                        zekOutArm.RETCODE = outDto.ReturnCode.Code;
                        zekOutArm.RETTEXT = outDto.ReturnCode.Text;
                    }

                    foreach (ZekArmItemDto item in outDto.armResponse.armList)
                    {
                        ZEKARMRESPONSE zekarmresponse = new ZEKARMRESPONSE();
                        zekarmresponse = Mapper.Map<ZekArmItemDto, ZEKARMRESPONSE>(item);
                        zekarmresponse.ZEKOUTARM = zekOutArm;
                        context.ZEKARMRESPONSE.Add(zekarmresponse);
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveGetARMsOutput: ", ex);
                    throw ex;
                }
            }
        }


        public void SaveBemerkungInformativabfrage(long sysAuskunft, string bemerkung, string vertragnummer, string antragnummer, string vpnummer)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    //DDLKPSPOS
                    
                        using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended contextOd = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
                        {

                           
                            if (bemerkung!=null && bemerkung.Length > 0)
                            {
                                //DDLKPSPOS Eintrag anlegen
                                DDLKPCOL colBemerkung = contextOd.DDLKPCOL.Where((p) => p.CODE == "BEMERKUNG" && p.DDLKPRUB.CODE == "INFO_ZEK").FirstOrDefault();
                                DDLKPSPOS sposB = contextOd.DDLKPSPOS.Where((p) => p.AREA == "AUSKUNFT" && p.SYSID == sysAuskunft && p.DDLKPCOL.SYSDDLKPCOL == colBemerkung.SYSDDLKPCOL).FirstOrDefault();
                                if (sposB == null)
                                    sposB = new  DDLKPSPOS();
                                sposB.AREA = "AUSKUNFT";
                                sposB.SYSID = sysAuskunft;
                                sposB.ACTIVEFLAG = 1;
                                sposB.VALUE = bemerkung;
                                sposB.DDLKPCOL = colBemerkung;
                                contextOd.DDLKPSPOS.Add(sposB);
                            }
                            else
                            {
                                //DDLKPSPOS Eintrag löschen, falls vorhanden
                                //DDLKPSPOS Eintrag anlegen
                                DDLKPCOL colBemerkung = contextOd.DDLKPCOL.Where((p) => p.CODE == "BEMERKUNG" && p.DDLKPRUB.CODE == "INFO_ZEK").FirstOrDefault();
                                DDLKPSPOS sposB = contextOd.DDLKPSPOS.Where((p) => p.AREA == "AUSKUNFT" && p.SYSID == sysAuskunft && p.DDLKPCOL.SYSDDLKPCOL == colBemerkung.SYSDDLKPCOL).FirstOrDefault();
                             
                                if (sposB != null && sposB.SYSDDLKPSPOS > 0)
                                    contextOd.DeleteObject(sposB);
                            }


                            if (antragnummer != null && antragnummer.Length > 0)
                            {
                                //DDLKPSPOS ANTRAGSNR anlegen
                                DDLKPCOL colAntragsnr = contextOd.DDLKPCOL.Where((p) => p.CODE == "ANTRAGSNR" && p.DDLKPRUB.CODE == "INFO_ZEK").FirstOrDefault();
                                DDLKPSPOS sposAnt = contextOd.DDLKPSPOS.Where((p) => p.AREA == "AUSKUNFT" && p.SYSID == sysAuskunft && p.DDLKPCOL.SYSDDLKPCOL == colAntragsnr.SYSDDLKPCOL).FirstOrDefault();
                                if (sposAnt == null)
                                    sposAnt = new  DDLKPSPOS();
                                sposAnt.AREA = "AUSKUNFT";
                                sposAnt.SYSID = sysAuskunft;
                                sposAnt.ACTIVEFLAG = 1;
                                sposAnt.VALUE = antragnummer;
                                sposAnt.DDLKPCOL = colAntragsnr;
                                contextOd.DDLKPSPOS.Add(sposAnt);
                            }
                            if (vertragnummer != null && vertragnummer.Length > 0)
                            {
                                //DDLKPSPOS VERTRAGSNR anlegen
                                 DDLKPCOL colVertragsnr = contextOd.DDLKPCOL.Where((p) => p.CODE == "VERTRAGSNR" && p.DDLKPRUB.CODE == "INFO_ZEK").FirstOrDefault();
                                 DDLKPSPOS sposVnr = contextOd.DDLKPSPOS.Where((p) => p.AREA == "AUSKUNFT" && p.SYSID == sysAuskunft && p.DDLKPCOL.SYSDDLKPCOL == colVertragsnr.SYSDDLKPCOL).FirstOrDefault();
                                if (sposVnr == null)
                                    sposVnr = new  DDLKPSPOS();
                                sposVnr.AREA = "AUSKUNFT";
                                sposVnr.SYSID = sysAuskunft;
                                sposVnr.ACTIVEFLAG = 1;
                                sposVnr.VALUE = vertragnummer;
                                sposVnr.DDLKPCOL = colVertragsnr;
                                contextOd.DDLKPSPOS.Add(sposVnr);
                            }
                            if (vpnummer != null && vpnummer.Length > 0)
                            {
                                //DDLKPSPOS VPNUMMER anlegen
                                DDLKPCOL colVpnummer = contextOd.DDLKPCOL.Where((p) => p.CODE == "VPNUMMER" && p.DDLKPRUB.CODE == "INFO_ZEK").FirstOrDefault();
                                DDLKPSPOS sposVpnr = contextOd.DDLKPSPOS.Where((p) => p.AREA == "AUSKUNFT" && p.SYSID == sysAuskunft && p.DDLKPCOL.SYSDDLKPCOL == colVpnummer.SYSDDLKPCOL).FirstOrDefault();
                                if (sposVpnr == null)
                                    sposVpnr = new  DDLKPSPOS();
                                sposVpnr.AREA = "AUSKUNFT";
                                sposVpnr.SYSID = sysAuskunft;
                                sposVpnr.ACTIVEFLAG = 1;
                                sposVpnr.VALUE = vpnummer;

                                sposVpnr.DDLKPCOL = colVpnummer;
                                contextOd.DDLKPSPOS.Add(sposVpnr);
                            }
                           
                            contextOd.SaveChanges();
                        }
                    
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von SaveInformativanfrageBemerkung ", ex);
                throw ex;
            }
        }

        #region Private Methods

        private void saveZekResponseDescription(ZekResponseDescriptionDto zekResponseDescriptionDto, ZEKCMR zekcmr, DdIcExtended context)
        {
            ZEKRESDESC zekresdesc = new ZEKRESDESC();
            zekresdesc = Mapper.Map<ZekResponseDescriptionDto, ZEKRESDESC>(zekResponseDescriptionDto);
            context.ZEKRESDESC.Add(zekresdesc);
            zekresdesc.ZEKCMR = zekcmr;

            if (zekResponseDescriptionDto.FoundPerson != null)
            {
                ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekResponseDescriptionDto.FoundPerson);
                context.ZEKADRDESC.Add(zekadrdesc);
                zekadrdesc.ZEKRESDESC = zekresdesc;
            }

            if (zekResponseDescriptionDto.Synonyms != null)
            {
                foreach (ZekAddressDescriptionDto zekAddressDescriptionDto in zekResponseDescriptionDto.Synonyms)
                {
                    ZEKADRDESC zekadrdescsy = new ZEKADRDESC();
                    zekadrdescsy = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekAddressDescriptionDto);
                    context.ZEKADRDESC.Add(zekadrdescsy);
                    zekadrdescsy.ZEKRESDESC = zekresdesc;
                }
            }

            if (zekResponseDescriptionDto.FoundContracts != null)
                zekresdesc.ZEKFC = saveZekFoundContracts(zekResponseDescriptionDto.FoundContracts, context);
        }


        private ZEKFC saveZekFoundContracts(ZekFoundContractsDto zekfoundContractsDto, DdIcExtended context)
        {
            if (zekfoundContractsDto != null)
            {
                ZEKFC zekfc = new ZEKFC();
                zekfc.GESAMTENGAGEMENT = (decimal?)zekfoundContractsDto.GesamtEngagement;
                context.ZEKFC.Add(zekfc);

                if (zekfoundContractsDto.AmtsinformationContracts != null)
                {
                    foreach (ZekAmtsinformationDescriptionDto zekAmtsinformationDescriptionDto in zekfoundContractsDto.AmtsinformationContracts)
                    {
                        ZEKAIC zekaic = new ZEKAIC();
                        zekaic = Mapper.Map<ZekAmtsinformationDescriptionDto, ZEKAIC>(zekAmtsinformationDescriptionDto);
                        context.ZEKAIC.Add(zekaic);
                        zekaic.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.BardarlehenContracts != null)
                {
                    foreach (ZekBardarlehenDescriptionDto zekBardarlehenDescriptionDto in zekfoundContractsDto.BardarlehenContracts)
                    {
                        ZEKBDC zekbdc = new ZEKBDC();
                        zekbdc = Mapper.Map<ZekBardarlehenDescriptionDto, ZEKBDC>(zekBardarlehenDescriptionDto);
                        context.ZEKBDC.Add(zekbdc);
                        zekbdc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.FestkreditContracts != null)
                {
                    foreach (ZekFestkreditDescriptionDto zekFestkreditDescriptionDto in zekfoundContractsDto.FestkreditContracts)
                    {
                        ZEKFKC zekfkc = new ZEKFKC();
                        zekfkc = Mapper.Map<ZekFestkreditDescriptionDto, ZEKFKC>(zekFestkreditDescriptionDto);
                        context.ZEKFKC.Add(zekfkc);
                        zekfkc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.GesamtEngagement != null)
                {
                }

                if (zekfoundContractsDto.KartenengagementContracts != null)
                {
                    foreach (ZekKartenengagementDescriptionDto zekKartenengagementDescriptionDto in zekfoundContractsDto.KartenengagementContracts)
                    {
                        ZEKKEC zekkec = new ZEKKEC();
                        zekkec = Mapper.Map<ZekKartenengagementDescriptionDto, ZEKKEC>(zekKartenengagementDescriptionDto);
                        context.ZEKKEC.Add(zekkec);
                        zekkec.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.KarteninformationContracts != null)
                {
                    foreach (ZekKarteninformationDescriptionDto zekKarteninformationDescriptionDto in zekfoundContractsDto.KarteninformationContracts)
                    {
                        ZEKKIC zekkic = new ZEKKIC();
                        zekkic = Mapper.Map<ZekKarteninformationDescriptionDto, ZEKKIC>(zekKarteninformationDescriptionDto);
                        context.ZEKKIC.Add(zekkic);
                        zekkic.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.KontokorrentkreditContracts != null)
                {
                    foreach (ZekKontokorrentkreditDescriptionDto zekKontokorrentkreditDescriptionDto in zekfoundContractsDto.KontokorrentkreditContracts)
                    {
                        ZEKKKC zekkkc = new ZEKKKC();
                        zekkkc = Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZEKKKC>(zekKontokorrentkreditDescriptionDto);
                        context.ZEKKKC.Add(zekkkc);
                        zekkkc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.KreditgesuchContracts != null)
                {
                    foreach (ZekKreditgesuchDescriptionDto zekKreditgesuchDescriptionDto in zekfoundContractsDto.KreditgesuchContracts)
                    {
                        ZEKKGC zekkgc = new ZEKKGC();
                        zekkgc = Mapper.Map<ZekKreditgesuchDescriptionDto, ZEKKGC>(zekKreditgesuchDescriptionDto);
                        context.ZEKKGC.Add(zekkgc);
                        zekkgc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.LeasingMietvertragContracts != null)
                {
                    foreach (ZekLeasingMietvertragDescriptionDto zekLeasingMietvertragDescriptionDto in zekfoundContractsDto.LeasingMietvertragContracts)
                    {
                        ZEKLMC zeklmc = new ZEKLMC();
                        zeklmc = Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZEKLMC>(zekLeasingMietvertragDescriptionDto);
                        context.ZEKLMC.Add(zeklmc);
                        zeklmc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.SolidarschuldnerContracts != null)
                {
                    foreach (ZekSolidarschuldnerDescriptionDto zekSolidarschuldnerDescriptionDto in zekfoundContractsDto.SolidarschuldnerContracts)
                    {
                        ZEKSSC zekssc = new ZEKSSC();
                        zekssc = Mapper.Map<ZekSolidarschuldnerDescriptionDto, ZEKSSC>(zekSolidarschuldnerDescriptionDto);
                        context.ZEKSSC.Add(zekssc);
                        zekssc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.TeilzahlungskreditContracts != null)
                {
                    foreach (ZekTeilzahlungskreditDescriptionDto zekTeilzahlungskreditDescriptionDto in zekfoundContractsDto.TeilzahlungskreditContracts)
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZEKTKC>(zekTeilzahlungskreditDescriptionDto);
                        context.ZEKTKC.Add(zektkc);
                        zektkc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.TeilzahlungsvertragContracts != null)
                {
                    foreach (ZekTeilzahlungsvertragDescriptionDto zekTeilzahlungsvertragDescriptionDto in zekfoundContractsDto.TeilzahlungsvertragContracts)
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungsvertragDescriptionDto, ZEKTKC>(zekTeilzahlungsvertragDescriptionDto);
                        context.ZEKTKC.Add(zektkc);
                        zektkc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.UeberziehnungskreditContracts != null)
                {
                    foreach (ZekUeberziehungskreditDescriptionDto zekUeberziehungskreditDescriptionDto in zekfoundContractsDto.UeberziehnungskreditContracts)
                    {
                        ZEKUKC zekukc = new ZEKUKC();
                        zekukc = Mapper.Map<ZekUeberziehungskreditDescriptionDto, ZEKUKC>(zekUeberziehungskreditDescriptionDto);
                        context.ZEKUKC.Add(zekukc);
                        zekukc.ZEKFC = zekfc;
                    }
                }

                return zekfc;
            }
            return null;
        }


        /// <summary>
        /// saveEcode178Contracts / WORKAROUND TODO EDMX 
        /// </summary>
        /// <param name="sysZEKFC"></param>
        /// <param name="ZekeCode178Contracts"></param>
        private void  saveEcode178Contracts(long sysZEKFC, List<ZekeCode178Dto> ZekeCode178Contracts)
        {    
            using (DdIcExtended context = new DdIcExtended())
            {
                string INSERTZEKCODE178 = " Insert into ZEKCODE178 (ECODE178ID,FZSTAMMNUMMER,ECODESTATUS,HAENDLERNUMMER,CHASSISNUMMER,DATUMGUELTIGBIS,DATUMGUELTIGAB ,NEXTECODESTATE,STVANUMMER,SYSZEKFC) " +
                                          " values  (:ECODE178ID,:FZSTAMMNUMMER,:ECODESTATUS,:HAENDLERNUMMER,:CHASSISNUMMER,:DATUMGUELTIGBIS,:DATUMGUELTIGAB,:NEXTECODESTATE,:STVANUMMER,:sysZEKFC) ";
                foreach (ZekeCode178Dto zekcode178 in ZekeCode178Contracts)
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ECODE178ID", Value = zekcode178.Ecode178id });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "FZSTAMMNUMMER", Value = zekcode178.Fzstammnummer });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ECODESTATUS", Value = zekcode178.Ecodestatus });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "HAENDLERNUMMER", Value = zekcode178.Haendlenummer });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "CHASSISNUMMER", Value = zekcode178.Chassisnummer });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "DATUMGUELTIGBIS", Value = zekcode178.Datumgueltigbis });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "DATUMGUELTIGAB", Value = zekcode178.Datumgueltigab });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "NEXTECODESTATE", Value = zekcode178.NextEcodeState });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "STVANUMMER", Value = zekcode178.StvaNummer });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysZEKFC", Value = sysZEKFC });

                    context.ExecuteStoreCommand(INSERTZEKCODE178, parameters.ToArray());
                    parameters.Clear();

                }
              
            }

        }

        private ZEKINPEC4 getZekInpEC4(long sysAuskunft, DdIcExtended context)
        {
            ZEKINPEC4 zekInpEC4 = new ZEKINPEC4();
            var ec4Query = from zekinpec4 in context.ZEKINPEC4
                           join Auskunft in context.AUSKUNFT on zekinpec4.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                           where Auskunft.SYSAUSKUNFT == sysAuskunft
                           select zekinpec4;
            zekInpEC4 = ec4Query.Single();
            return zekInpEC4;
        }

        private ZEKINPEC5 getZekInpEC5(long sysAuskunft, DdIcExtended context)
        {
            ZEKINPEC5 zekInpEC5 = new ZEKINPEC5();
            var ec5Query = from zekinpec5 in context.ZEKINPEC5
                           join Auskunft in context.AUSKUNFT on zekinpec5.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                           where Auskunft.SYSAUSKUNFT == sysAuskunft
                           select zekinpec5;
            zekInpEC5 = ec5Query.Single();
            return zekInpEC5;
        }

        private ZEKINPEC7 getZekInpEC7(long sysAuskunft, DdIcExtended context)
        {
            ZEKINPEC7 zekInpEC7 = new ZEKINPEC7();
            var ec7Query = from zekinpec7 in context.ZEKINPEC7
                           join Auskunft in context.AUSKUNFT on zekinpec7.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                           where Auskunft.SYSAUSKUNFT == sysAuskunft
                           select zekinpec7;
            zekInpEC7 = ec7Query.Single();
            return zekInpEC7;
        }

        private List<ZekRequestEntityDto> getRequestEntities(List<ZEKREQEN> zekreqenList, DdIcExtended context)
        {
            List<ZekRequestEntityDto> requestEntities = null;

            if (zekreqenList != null && zekreqenList.Count != 0)
            {
                requestEntities = new List<ZekRequestEntityDto>();
                foreach (ZEKREQEN zekreqen in zekreqenList)
                {
                    ZekRequestEntityDto zekreqenDto = Mapper.Map<ZEKREQEN, ZekRequestEntityDto>(zekreqen);

                    var addescQuery = from zekadrdesc in context.ZEKADRDESC // Selektiere alle AddrDescr
                                      where zekadrdesc.ZEKREQEN.SYSZEKREQEN == zekreqen.SYSZEKREQEN// der gesuchten sysAuskunft entsprechen.
                                      select zekadrdesc;
                    ZEKADRDESC adrdesc = addescQuery.Single();
                    zekreqenDto.AddressDescription = Mapper.Map<ZEKADRDESC, ZekAddressDescriptionDto>(adrdesc);
                    //Plausibilisierung ZEK Meldungen /BNRSIEBEN-1316
                    if (zekreqenDto.AddressDescription != null)
                    {
                        zekreqenDto.AddressDescription.FoundingDate = MyFoundingDate(zekreqenDto.AddressDescription.LegalForm, zekreqenDto.AddressDescription.FoundingDate);
                        zekreqenDto.AddressDescription.DatumWohnhaftSeit = MyWohnhaftSeit(zekreqenDto.AddressDescription.LegalForm, zekreqenDto.AddressDescription.DatumWohnhaftSeit);
                    }
                    requestEntities.Add(zekreqenDto);
                }
            }
            return requestEntities;
        }

        private ZekRequestEntityDto getRequestEntity(ZEKREQEN zekreqen, DdIcExtended context)
        {
            ZekRequestEntityDto requestEntity = Mapper.Map<ZEKREQEN, ZekRequestEntityDto>(zekreqen);
            var addescQuery = from zekadrdesc in context.ZEKADRDESC  // Selektiere alle AddrDescr
                              where zekadrdesc.ZEKREQEN.SYSZEKREQEN == zekreqen.SYSZEKREQEN // der gesuchten sysAuskunft entsprechen.
                              select zekadrdesc;
            ZEKADRDESC adrdesc = addescQuery.Single();

            requestEntity.AddressDescription = Mapper.Map<ZEKADRDESC, ZekAddressDescriptionDto>(adrdesc);
            if (requestEntity.AddressDescription != null)
            {
                requestEntity.AddressDescription.FoundingDate = MyFoundingDate(requestEntity.AddressDescription.LegalForm, requestEntity.AddressDescription.FoundingDate);
                requestEntity.AddressDescription.DatumWohnhaftSeit = MyWohnhaftSeit(requestEntity.AddressDescription.LegalForm, requestEntity.AddressDescription.DatumWohnhaftSeit);
            }
            return requestEntity;
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

        /// <summary>
        /// MyFoundingDate
        /// </summary>
        /// <param name="legalForm"></param>
        /// <param name="foundingDate"></param>
        /// <returns></returns>
        private string MyFoundingDate(int legalForm, string foundingDate)
        {
            if (legalForm <= 1)
                return null;
            return foundingDate;
        }

        /// <summary>
        /// MyWohnhaftSeit
        /// </summary>
        /// <param name="legalForm"></param>
        /// <param name="wohnhaftSeit"></param>
        /// <returns></returns>
        private string MyWohnhaftSeit(int legalForm, string wohnhaftSeit)
        {
            if (legalForm > 1)
                return null;
            return wohnhaftSeit;
        }

        #endregion Private Methods

    }
}