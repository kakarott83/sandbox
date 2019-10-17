using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using System.Collections.Generic;
using CIC.Database.IC.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Auskunft Data Access Object
    /// </summary>
    public class AuskunftDao : IAuskunftDao
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const int AUSKUNFTSTATUSLEN = 128;
        private const string QUERYHOSTCOMPUTER = "SELECT hostcomputer from auskunft where sysauskunft=:psysauskunft";
        private const string QUERYUPDATEHOSTCOMPUTER = "UPDATE auskunft SET hostcomputer = :phostcomputer where sysauskunft = :psysauskunft";

        private const string QUERYUNPROCESSEDBA =
            "SELECT * from auskunft where auskunft.FEHLERCODE = :pfehlercode AND sysauskunfttyp in (Select sysauskunfttyp from auskunfttyp where bezeichnung in (:pbezeichnung1, :pbezeichnung2)) and auskunft.hostcomputer = :phostcomputer";

        private const string QUERYENTITYSOAPAUSKUNFT =
            "select auskunft.area entity, auskunft.sysid sysid, auskunfttyp.bezeichnung||'_'||auskunft.sysauskunft||'_' code,auskunfttyp.debugflag logDumpFlag from auskunft left outer join auskunfttyp  on  auskunfttyp.sysauskunfttyp=auskunft.sysauskunfttyp where sysauskunft = :psysauskunft";

        private const string QUERYUPDATEWFUSER = "UPDATE auskunft SET syswfuser = :psyswfuser where sysauskunft = :psysauskunft";
        private const string QUERYUPDATEAREAUNDSYSID = "UPDATE auskunft SET sysid = :psysid, area = :parea where sysauskunft = :psysauskunft";
        private const string CFGQUERY = "select url,username,keyvalue,syseaipar,bezeichnung,beschreibung,sysauskunftcfg,utl_raw.cast_to_varchar2(dbms_lob.substr(dataobject)) dataobject from auskunftcfg where bezeichnung=:bez";

        /// <summary>
        /// Holt die Auskunft-Sätze mit Status -5 (für ZekBatch)
        /// </summary>
        /// <returns></returns>
        public AuskunftDto getUnprocessedBatchAuskunft()
        {
            AuskunftDto unprocessedBatchAuskunft = null;
            using (DdIcExtended context = new DdIcExtended())
            {
                String fehlerCode = ((long) AuskunftErrorCode.BatchRequestSent).ToString();
                var query = from auskunft in context.AUSKUNFT
                            where auskunft.FEHLERCODE.Equals(fehlerCode) &&
                                  (auskunft.AUSKUNFTTYP.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKBatchCloseContracts) ||
                                   auskunft.AUSKUNFTTYP.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKBatchUpdateContracts))
                            select auskunft;
                AUSKUNFT auskunftBatch = query.FirstOrDefault();
                unprocessedBatchAuskunft = Mapper.Map<AUSKUNFT, AuskunftDto>(auskunftBatch);
            }
            return unprocessedBatchAuskunft;
        }

        /// <summary>
        /// Holt die Auskunft-Sätze mit Status -5 (für ZekBatch)
        /// </summary>
        /// <returns></returns>
        public List<AuskunftDto> getUnprocessedBatchAuskunfte()
        {
            string hostcomputer = System.Net.Dns.GetHostName();
            List<AuskunftDto> unprocessedBatchAuskunfte = new List<AuskunftDto>();
            using (DdIcExtended context = new DdIcExtended())
            {
                String fehlerCode = ((long) AuskunftErrorCode.BatchRequestSent).ToString();
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "pfehlercode", Value = fehlerCode});
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "pbezeichnung1", Value = AuskunfttypDao.ZEKBatchCloseContracts});
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "pbezeichnung2", Value = AuskunfttypDao.ZEKBatchUpdateContracts});
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "phostcomputer", Value = hostcomputer});
                List<AUSKUNFT> auskunfte = context.ExecuteStoreQuery<AUSKUNFT>(QUERYUNPROCESSEDBA, parameters.ToArray()).ToList();
                //Hostcomputer ist noch nicht in edmx

                /*        List<AUSKUNFT> auskunfte = (from auskunft in context.AUSKUNFT
                                            where auskunft.FEHLERCODE.Equals(fehlerCode) &&
                                          (auskunft.AUSKUNFTTYP.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKBatchCloseContracts) ||
                                           auskunft.AUSKUNFTTYP.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKBatchUpdateContracts))
                                    select auskunft).ToList();*/

                foreach (AUSKUNFT auskunftBatch in auskunfte)
                {
                    unprocessedBatchAuskunfte.Add(Mapper.Map<AUSKUNFT, AuskunftDto>(auskunftBatch));
                }
            }
            return unprocessedBatchAuskunfte;
        }

        /// <summary>
        /// Das DebugFlag von AuskunftTyp holen (für SoapLoggingAuskunft)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public bool getLogdumpFlag(long sysAuskunft)
        {
            bool logDumpFlag = false;
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    int? debugFlag = (from auskunftTyp in context.AUSKUNFTTYP
                                      join auskunft in context.AUSKUNFT on auskunftTyp.SYSAUSKUNFTTYP equals auskunft.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                      where auskunft.SYSAUSKUNFT == sysAuskunft
                                      select auskunftTyp).Single().DEBUGFLAG;
                    logDumpFlag = (debugFlag.HasValue && debugFlag != 0);
                }
                catch (Exception ex)
                {
                    _log.Error("getLogdumpFlag failed: ", ex);
                    throw ex;
                }
            }
            return logDumpFlag;
        }

        /// <summary>
        /// etEntitySoapLogEurotax(
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="bezeichnung"></param>
        /// <returns></returns>
        public Cic.OpenOne.Common.DTO.SoapXMLDto getEntitySoapLogEurotax(long sysid, string bezeichnung)
        {
            Cic.OpenOne.Common.DTO.SoapXMLDto soapXmlDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();
            bool logDumpFlag = false;
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    var auskunfttyp = (from auskunftTyp in context.AUSKUNFTTYP
                                       where auskunftTyp.BEZEICHNUNG == bezeichnung
                                       select auskunftTyp).FirstOrDefault();
                    if (auskunfttyp != null)

                        logDumpFlag = (auskunfttyp.DEBUGFLAG.HasValue && auskunfttyp.DEBUGFLAG != 0);
                }
                catch (Exception ex)
                {
                    _log.Error("getLogdumpFlag failed: ", ex);
                    throw ex;
                }
            }
            soapXmlDto.logDumpFlag = logDumpFlag;
            soapXmlDto.entity = "ANTOB";
            if (sysid == 0)
                sysid = 250; // sysid == sysobtyp wenn sysid=0 default 250??
            soapXmlDto.sysid = sysid;
            soapXmlDto.code = bezeichnung + "_";
            return soapXmlDto;
        }

        /// <summary>
        /// getEntitySoapLogEurotaxVin(
        /// </summary>
        /// <param name="vincode"></param>
        /// <param name="bezeichnung"></param>
        /// <returns></returns>
        public Cic.OpenOne.Common.DTO.SoapXMLDto getEntitySoapLogEurotaxVin(long sysid, string bezeichnung)
        {
            Cic.OpenOne.Common.DTO.SoapXMLDto soapXmlDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();
            bool logDumpFlag = false;
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    var auskunfttyp = (from auskunftTyp in context.AUSKUNFTTYP
                                       where auskunftTyp.BEZEICHNUNG == bezeichnung
                                       select auskunftTyp).FirstOrDefault();
                    if (auskunfttyp != null)

                        logDumpFlag = (auskunfttyp.DEBUGFLAG.HasValue && auskunfttyp.DEBUGFLAG != 0);
                }
                catch (Exception ex)
                {
                    _log.Error("getLogdumpFlag failed: ", ex);
                    throw ex;
                }
            }
            soapXmlDto.logDumpFlag = logDumpFlag;
            soapXmlDto.entity = "AUSKUNFT";
            soapXmlDto.sysid = sysid;
            soapXmlDto.code = bezeichnung + "_" + sysid;
            return soapXmlDto;
        }

        /// <summary>
        /// Das Area und sysarea  von Auskunft holen (für SoapLoggingAuskunft)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public Cic.OpenOne.Common.DTO.SoapXMLDto getEntitySoapLog(long sysAuskunft)
        {
            Cic.OpenOne.Common.DTO.SoapXMLDto soapXmlDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();
            //string entity = "";
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "psysauskunft", Value = sysAuskunft});
                    soapXmlDto = context.ExecuteStoreQuery<Cic.OpenOne.Common.DTO.SoapXMLDto>(QUERYENTITYSOAPAUSKUNFT, parameters.ToArray()).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _log.Error("getLogdumpFlag failed: ", ex);
                    throw ex;
                }
            }
            return soapXmlDto;
        }

        /// <summary>
        /// Creates new AUSKUNFT and links it with AUSKUNFTTYP 
        /// </summary>
        /// <param name="auskunfttyp">Bezeichnung des Auskunfttyps</param>
        /// <returns>SYSAUSKUNFT</returns>
        public long SaveAuskunft(string auskunfttyp)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    AUSKUNFT Auskunft = new AUSKUNFT
                    {
                        STATUS = "Neuer Auskunftssatz (" + auskunfttyp + ")",
                        AUSKUNFTTYP = context.AUSKUNFTTYP.Where(par => par.BEZEICHNUNG == auskunfttyp).FirstOrDefault()
                    };

                    DateTime datetime = DateTime.Now;
                    Auskunft.ANFRAGEDATUM = datetime;
                    Auskunft.ANFRAGEUHRZEIT = datetime.Hour*3600*100 + datetime.Minute*60*100 + datetime.Second*100 + 1;
                    context.AUSKUNFT.Add(Auskunft);

                    context.SaveChanges();

                    return Auskunft.SYSAUSKUNFT;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern der Auskunft. ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Updates AUSKUNFT
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="returnCode"></param>
        public void UpdateAuskunft(long sysAuskunft, long returnCode)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                    DateTime datetime = DateTime.Now;
                    Auskunft.ANFRAGEDATUM = datetime;
                    Auskunft.ANFRAGEUHRZEIT = datetime.Hour*3600*100 + datetime.Minute*60*100 + datetime.Second*100 + 1;
                    if (Auskunft.AUSKUNFTTYP == null)
                        context.Entry(Auskunft).Reference(f => f.AUSKUNFTTYP).Load();

                    string bezeichnungTyp = "";
                    if (Auskunft.AUSKUNFTTYP != null)
                    {
                        bezeichnungTyp = Auskunft.AUSKUNFTTYP.BEZEICHNUNG;
                    }

                    if (returnCode >= 0)
                    {
                        Auskunft.STATUS = bezeichnungTyp + " - Aufruf erfolgreich";
                    }
                    else
                    {
                        Auskunft.STATUS = bezeichnungTyp + " - Aufruf nicht erfolgreich";
                    }

                    // Auskunft.STATUS hat nur 128 Zeichen in der DB
                    if (Auskunft.STATUS.Length > AUSKUNFTSTATUSLEN)
                    {
                        Auskunft.STATUS = Auskunft.STATUS.Remove(AUSKUNFTSTATUSLEN);
                    }

                    Auskunft.FEHLERCODE = returnCode.ToString();

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Update der Auskunft. ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// UpdateAuskunftDtoAuskunft
        /// </summary>
        /// <param name="auskunftDto">auskunftDto</param>
        /// <param name="returnCode">returnCode</param>
        public void UpdateAuskunftDtoAuskunft(AuskunftDto auskunftDto, long returnCode)
        {
            long sysAuskunft = auskunftDto.sysAuskunft;

            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                    DateTime datetime = DateTime.Now;
                    Auskunft.ANFRAGEDATUM = datetime;
                    Auskunft.ANFRAGEUHRZEIT = datetime.Hour*3600*100 + datetime.Minute*60*100 + datetime.Second*100 + 1;
                    if (Auskunft.AUSKUNFTTYP == null)
                        context.Entry(Auskunft).Reference(f => f.AUSKUNFTTYP).Load();

                    string bezeichnungTyp = "";
                    if (Auskunft.AUSKUNFTTYP != null)
                    {
                        bezeichnungTyp = Auskunft.AUSKUNFTTYP.BEZEICHNUNG;
                    }

                    if (auskunftDto.Status != null && auskunftDto.Status.Contains(bezeichnungTyp))
                    {
                        auskunftDto.Status = String.Empty;
                    }
                    if (Auskunft.AREA == null || Auskunft.AREA.Length == 0)
                        Auskunft.AREA = auskunftDto.area;
                    if (Auskunft.SYSID == 0 && auskunftDto.sysid > 0)
                        Auskunft.SYSID = auskunftDto.sysid;

                    Auskunft.STATUS = bezeichnungTyp + " - ";
                    if (!String.IsNullOrEmpty(auskunftDto.Status))
                    {
                        Auskunft.STATUS += auskunftDto.Status;
                    }
                    else
                    {
                        if (returnCode >= 0)
                        {
                            Auskunft.STATUS += "Aufruf erfolgreich";
                        }
                        else
                        {
                            Auskunft.STATUS += "Aufruf nicht erfolgreich";
                        }
                    }

                    // Auskunft.STATUS hat nur 128 Zeichen in der DB
                    if (Auskunft.STATUS.Length > AUSKUNFTSTATUSLEN)
                    {
                        Auskunft.STATUS = Auskunft.STATUS.Remove(AUSKUNFTSTATUSLEN);
                    }

                    Auskunft.FEHLERCODE = returnCode.ToString();

                    context.SaveChanges();

                    auskunftDto.Fehlercode = Auskunft.FEHLERCODE;
                    auskunftDto.Status = Auskunft.STATUS;
                    auskunftDto.Anfragedatum = (DateTime) Auskunft.ANFRAGEDATUM;
                    auskunftDto.Anfrageuhrzeit = (long) Auskunft.ANFRAGEUHRZEIT;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler bei UpdateAuskunftDtoAuskunft. ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// UpdateAuskunftDtoAuskunft
        /// </summary>
        /// <param name="auskunftDto"></param>
        /// <param name="returnCode"></param>
        /// <param name="text"></param>
        public void UpdateAuskunftDtoAuskunft(AuskunftDto auskunftDto, long returnCode, string text)
        {
            long sysAuskunft = auskunftDto.sysAuskunft;

            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                    DateTime datetime = DateTime.Now;
                    Auskunft.ANFRAGEDATUM = datetime;
                    Auskunft.ANFRAGEUHRZEIT = datetime.Hour*3600*100 + datetime.Minute*60*100 + datetime.Second*100 + 1;
                    if (Auskunft.AUSKUNFTTYP == null)
                        context.Entry(Auskunft).Reference(f => f.AUSKUNFTTYP).Load();
                     

                    string bezeichnungTyp = "";
                    if (Auskunft.AUSKUNFTTYP != null)
                    {
                        bezeichnungTyp = Auskunft.AUSKUNFTTYP.BEZEICHNUNG;
                    }

                    if (auskunftDto.Status != null && auskunftDto.Status.Contains(bezeichnungTyp))
                    {
                        auskunftDto.Status = String.Empty;
                    }
                    if (Auskunft.AREA == null || Auskunft.AREA.Length == 0)
                        Auskunft.AREA = auskunftDto.area;
                    if (Auskunft.SYSID == 0 && auskunftDto.sysid > 0)
                        Auskunft.SYSID = auskunftDto.sysid;

                    Auskunft.STATUS = bezeichnungTyp + " - ";
                    if (!String.IsNullOrEmpty(auskunftDto.Status))
                    {
                        Auskunft.STATUS += auskunftDto.Status;
                    }
                    else
                    {
                        if (returnCode >= 0)
                        {
                            Auskunft.STATUS += "Aufruf erfolgreich " + text;
                        }
                        else
                        {
                            Auskunft.STATUS += "Aufruf nicht erfolgreich";
                        }
                    }

                    // Auskunft.STATUS hat nur 128 Zeichen in der DB
                    if (Auskunft.STATUS.Length > AUSKUNFTSTATUSLEN)
                    {
                        Auskunft.STATUS = Auskunft.STATUS.Remove(AUSKUNFTSTATUSLEN);
                    }

                    Auskunft.FEHLERCODE = returnCode.ToString();

                    context.SaveChanges();

                    auskunftDto.Fehlercode = Auskunft.FEHLERCODE;
                    auskunftDto.Status = Auskunft.STATUS;
                    auskunftDto.Anfragedatum = (DateTime) Auskunft.ANFRAGEDATUM;
                    auskunftDto.Anfrageuhrzeit = (long) Auskunft.ANFRAGEUHRZEIT;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler bei UpdateAuskunftDtoAuskunft. ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Gets AUSKUNFT by SysId and maps it to AuskunftDto
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns>AuskunftDto</returns>
        public AuskunftDto FindBySysId(long sysId)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    AUSKUNFT Auskunft = context.AUSKUNFT.First(par => par.SYSAUSKUNFT == sysId);
                     
                    if (Auskunft.AUSKUNFTTYP == null)
                        context.Entry(Auskunft).Reference(f => f.AUSKUNFTTYP).Load();

                    AuskunftDto auskunftDto = Mapper.Map<AUSKUNFT, AuskunftDto>(Auskunft);
                    return auskunftDto;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden der Auskunft mit der Sysid: " + sysId.ToString(), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Sets the statusnum Field to 1 (invalid) for all suitable auskunft entries
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="area"></param>
        /// <param name="auskunfttyp"></param>
        public void invalidateAuskunft(long sysId, string area, string auskunfttyp)
        {
            try
            {
                long auskunfttypId = GetAuskunfttypId(auskunfttyp);
                using (DdIcExtended context = new DdIcExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysId });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "typ", Value = auskunfttypId });
                    context.ExecuteStoreCommand("update auskunft set statusnum=1 where area=:area and sysid=:sysid and sysauskunfttyp=:typ", parameters.ToArray());
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Invalidieren der Auskunft für Typ " + auskunfttyp + " mit Gebiet=" + area + " und Sysid: " + sysId.ToString(), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Sets the auskunft STATUSNUM Field
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="status"></param>
        public void setAuskunfStatusNum(long sysAuskunft, short status)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysAuskunft });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "status", Value = status });
                    context.ExecuteStoreCommand("update auskunft set statusnum=:status where sysauskunft=:sysid", parameters.ToArray());
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Setzen des Auskunftstatus für sysauskunft="+sysAuskunft, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets AUSKUNFT by SysId, area, asukunfttyp  and maps it to AuskunftDto
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="area"></param>
        /// <param name="auskunfttyp"></param>
        /// <returns></returns>
        public AuskunftDto FindByAreaSysId(long sysId, string area, string auskunfttyp)
        {
            try
            {
                long auskunfttypId = GetAuskunfttypId(auskunfttyp);
                using (DdIcExtended context = new DdIcExtended())
                {
                    AUSKUNFT Auskunft =
                        context.AUSKUNFT.Where(
                            par => par.SYSID == sysId && par.AREA == area && par.AUSKUNFTTYP.SYSAUSKUNFTTYP == auskunfttypId && par.FEHLERCODE == "0").First();
                    if (Auskunft != null)
                    {
                        
                        if (Auskunft.AUSKUNFTTYP == null)
                            context.Entry(Auskunft).Reference(f => f.AUSKUNFTTYP).Load();

                        AuskunftDto auskunftDto = Mapper.Map<AUSKUNFT, AuskunftDto>(Auskunft);
                        return auskunftDto;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden der Auskunft für Typ "+auskunfttyp+" mit Gebiet="+area+" und Sysid: " + sysId.ToString(), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets AUSKUNFTTYP by sysAuskunft 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AUSKUNFTTYP.BEZEICHNUNG</returns>
        public string GetAuskunfttypBezeichng(long sysAuskunft)
        {
            AUSKUNFTTYP auskunftTyp;
            using (DdIcExtended context = new DdIcExtended())
            {
                var query = from Auskunft in context.AUSKUNFT
                            where Auskunft.SYSAUSKUNFT == sysAuskunft
                            select Auskunft.AUSKUNFTTYP;
                auskunftTyp = query.FirstOrDefault();
            }
            if (auskunftTyp == null)
            {
                throw new ArgumentException("Es existiert kein Auskunftssatz mit sysAuskunft = " + sysAuskunft);
            }
            return auskunftTyp.BEZEICHNUNG;
        }

        /// <summary>
        /// GetAuskunfttypId
        /// </summary>
        /// <param name="auskunfttyp"></param>
        /// <returns>SYSAUSKUNFTTYP</returns>
        public long GetAuskunfttypId(string auskunfttyp)
        {
           
            using (DdIcExtended context = new DdIcExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bez", Value = auskunfttyp });
                long rval = context.ExecuteStoreQuery<long>("select sysauskunfttyp from auskunfttyp where bezeichnung=:bez", parameters.ToArray()).FirstOrDefault();
                if(rval == 0)
                {
                    throw new ArgumentException("Es existiert kein Auskunfttypssatz mit auskunfttyp = " + auskunfttyp);
                }
                return rval;
            }
            
        }

        /// <summary>
        /// setAuskunftHostcomputer
        /// </summary>
        /// <param name="sysAuskunft"></param>
        public void setAuskunftHostcomputer(long sysAuskunft)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                string hostcomputer = System.Net.Dns.GetHostName();
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "phostcomputer", Value = hostcomputer});
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "psysauskunft", Value = sysAuskunft});
                context.ExecuteStoreCommand(QUERYUPDATEHOSTCOMPUTER, parameters.ToArray());
            }
        }

        /// <summary>
        /// setAuskunftHostcomputer
        /// </summary>
        /// <param name="sysAuskunft"></param>
        public void setAuskunftWfuser(long sysAuskunft, long syswfuser)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                string hostcomputer = System.Net.Dns.GetHostName();
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "psyswfuser", Value = syswfuser});
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "psysauskunft", Value = sysAuskunft});
                context.ExecuteStoreCommand(QUERYUPDATEWFUSER, parameters.ToArray());
            }
        }

        /// <summary>
        /// setArea und sysID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        public void setAuskunfAreaUndId(long sysAuskunft, string area, long sysid)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                string hostcomputer = System.Net.Dns.GetHostName();
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "parea", Value = area});
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "psysid", Value = sysid});
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "psysauskunft", Value = sysAuskunft});
                context.ExecuteStoreCommand(QUERYUPDATEAREAUNDSYSID, parameters.ToArray());
            }
        }

        /// <summary>
        /// getAuskunftHostcomputer
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public string getAuskunftHostcomputer(long sysAuskunft)
        {
            string hostcomputer = null;
            using (DdIcExtended context = new DdIcExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "psysauskunft", Value = sysAuskunft});
                hostcomputer = context.ExecuteStoreQuery<string>(QUERYHOSTCOMPUTER, parameters.ToArray()).FirstOrDefault();
            }

            return hostcomputer;
        }

        /// <summary>
        /// Delivers the auskunft configuration
        /// </summary>
        /// <param name="bezeichnung"></param>
        /// <returns></returns>
        public AuskunftCFGDto getConfiguration(String bezeichnung)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bez", Value = bezeichnung });
                    AuskunftCFGDto rval = context.ExecuteStoreQuery< AuskunftCFGDto>(CFGQUERY,parameters.ToArray()).FirstOrDefault();
                    return rval;
                }
                catch (Exception ex)
                {
                    _log.Error("Zugangsdaten für AUSKUNFT "+ bezeichnung+" konnten nicht geladen werden. ", ex);
                    throw ex;
                }
            }
        }
    }
}