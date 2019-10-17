using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using Cic.OpenOne.Common.DTO;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// EAIHOT DAO
    /// </summary>
    public class EaihotDao : IEaihotDao
    {
        const string GETEAIQOUFROMEAIHOTSORT = "select * from eaiqou where syseaihot = :syseaihot order by case when f03=1 then 1 when f12=1 then 2 when f11=1 then 3 when f05=1 then 4 when f04=1 then 5 else 6 end, f06 desc,to_number(f07)";
        const string GETEAIQOUFROMEAIHOT = "select * from eaiqou where syseaihot = :syseaihot order by f06 desc,to_number(f07)";
        const string QUERYBEZEICHNUNGWFTXBYSYSID = "select actualterm1 from vc_ctlut where area = 'WFTX' and SYSCTLANG = :sysctlang and SYSID = :syswftx";

        ILog _log = Log.GetLogger (MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Neuen Eaihot erstellen 
        /// </summary>
        /// <param name="eaihotInput">Eaihot Eingang</param>
        /// <returns>Eaihot Ausgang</returns>
        public EaihotDto createEaihot (EaihotDto eaihotInput)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIHOT eaihotOutput = new EAIHOT();
                owCtx.EAIHOT .Add(eaihotOutput);

                Mapper.Map<EaihotDto, EAIHOT>(eaihotInput, eaihotOutput);
                if (eaihotInput.SYSEAIART != null && eaihotInput.SYSEAIART.HasValue && eaihotInput.SYSEAIART.Value > 0)
                    eaihotOutput.SYSEAIART = eaihotInput.SYSEAIART.Value;
                else
                    eaihotOutput.EAIART = getEaiArt(eaihotInput.CODE, owCtx);
                eaihotOutput.CLIENTART = 1;
                owCtx.SaveChanges();
                return getEaihot(eaihotOutput.SYSEAIHOT);
            }
        }

        /// <summary>
        /// Neuen Eaihot erstellen 
        /// </summary>
        /// <param name="eaihotInput">Eaihot Eingang</param>
        /// <returns>Eaihot Ausgang</returns>
        public EaihotDto createEaihotWithoutEaiArt (EaihotDto eaihotInput) {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIHOT eaihotOutput = new EAIHOT();
                owCtx.EAIHOT .Add(eaihotOutput);

                Mapper.Map<EaihotDto, EAIHOT>(eaihotInput, eaihotOutput);
                if (eaihotInput.SYSEAIART != null && eaihotInput.SYSEAIART.HasValue && eaihotInput.SYSEAIART.Value > 0)
                    eaihotOutput.SYSEAIART = eaihotInput.SYSEAIART.Value;
                else
                    eaihotOutput.EAIART = getEaiArtBasic(eaihotInput.CODE, owCtx);
                eaihotOutput.CLIENTART = 1;
                owCtx.SaveChanges();
                return getEaihotWithoutEaiArt(eaihotOutput.SYSEAIHOT);
            }
        }


        /// <summary>
        /// Bestehendes Eaihot holen
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        /// <returns>Eaihot Ausgang</returns>
        public EaihotDto getEaihotWithoutEaiArt (long syseaihot) {
            EaihotDto rval = null;
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIHOT eaihotOutput = (from e in owCtx.EAIHOT
                                       where e.SYSEAIHOT == syseaihot
                                       select e).FirstOrDefault();

                if (eaihotOutput != null)
                {
                    rval = Mapper.Map<EAIHOT, EaihotDto>(eaihotOutput);
                }
            }
            return rval;
        }


        /// <summary>
        /// activates an eaihot
        /// </summary>
        /// <param name="eaihotInput"></param>
        /// <param name="eve"></param>
        public void activateEaihot (EaihotDto eaihotInput, int eve)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIHOT eaihotOutput = (from e in owCtx.EAIHOT
                                       where e.SYSEAIHOT == eaihotInput.SYSEAIHOT
                                       select e).FirstOrDefault();
                eaihotOutput.EVE = eve;

                owCtx.SaveChanges();
            }
        }

        /// <summary>
        /// EAI Eingangsqueue erzeugen
        /// </summary>
        /// <param name="eaiqinInput">EAIQIN Eingangsdaten</param>
        /// <returns>EAIQIN Daten</returns>
        public EaiqinDto createEaiqin (EaiqinDto eaiqinInput)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIQIN eaiOutput = new EAIQIN()
                {
                    EAIHOT = (from eai in owCtx.EAIHOT
                              where eai.SYSEAIHOT == eaiqinInput.sysEaihot
                              select eai).FirstOrDefault(),
                    F01 = eaiqinInput.F01,
                    F02 = eaiqinInput.F02,
                    F03 = eaiqinInput.F03,
                    F04 = eaiqinInput.F04,
                    F05 = eaiqinInput.F05,
                    F06 = eaiqinInput.F06,
                    F07 = eaiqinInput.F07,
                    F08 = eaiqinInput.F08,
                    F09 = eaiqinInput.F09,
                    F10 = eaiqinInput.F10,
                    F11 = eaiqinInput.F11,
                    F12 = eaiqinInput.F12,
                    F13 = eaiqinInput.F13,
                    F14 = eaiqinInput.F14,
                    F15 = eaiqinInput.F15,
                    F16 = eaiqinInput.F16,
                    F17 = eaiqinInput.F17,
                    F18 = eaiqinInput.F18,
                    F19 = eaiqinInput.F19,
                    F20 = eaiqinInput.F20,
                };
                owCtx.EAIQIN .Add(eaiOutput);
                owCtx.SaveChanges();
                eaiqinInput.SYSEAIQIN = eaiOutput.SYSEAIQIN;
                return eaiqinInput;
            }
        }

        /// <summary>
        /// EAIHFile anlegen
        /// </summary>
        /// <param name="eaihfile">EAIHFILE Daten</param>
        public void createEaihfile (EaihfileDto eaihfile)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIHFILE eaiOutput = new EAIHFILE();
                eaiOutput.CODE = eaihfile.CODE;
                eaiOutput.EAIHFILE1 = eaihfile.EAIHFILE;
                eaiOutput.SYSEAIHOT = eaihfile.SYSEAIHOT;

                owCtx.EAIHFILE .Add(eaiOutput);                
                owCtx.SaveChanges();
            }
        }

		/// <summary>
		/// EAI Eingangsqueue erzeugen
		/// </summary>
		/// <param name="eaiqinInputs">EAIQIN Eingangsdaten</param>
		public void createEaiqin (List<EaiqinDto> eaiqinInputs)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                foreach (EaiqinDto eaiqinInput in eaiqinInputs)
                {
                    EAIQIN eaiOutput = new EAIQIN()
                    {

                        EAIHOT = (from eai in owCtx.EAIHOT
                                  where eai.SYSEAIHOT == eaiqinInput.sysEaihot
                                  select eai).FirstOrDefault(),
                        F01 = eaiqinInput.F01,
                        F02 = eaiqinInput.F02,
                        F03 = eaiqinInput.F03,
                        F04 = eaiqinInput.F04,
                        F05 = eaiqinInput.F05,
                        F06 = eaiqinInput.F06,
                        F07 = eaiqinInput.F07,
                        F08 = eaiqinInput.F08,
                        F09 = eaiqinInput.F09,
                        F10 = eaiqinInput.F10,
                        F11 = eaiqinInput.F11,
                        F12 = eaiqinInput.F12,
                        F13 = eaiqinInput.F13,
                        F14 = eaiqinInput.F14,
                        F15 = eaiqinInput.F15,
                        F16 = eaiqinInput.F16,
                        F17 = eaiqinInput.F17,
                        F18 = eaiqinInput.F18,
                        F19 = eaiqinInput.F19,
                        F20 = eaiqinInput.F20,
                    };
                    owCtx.EAIQIN .Add(eaiOutput);
                }
                owCtx.SaveChanges();
            }
        }

		/// <summary>
		/// EAI Ausgangsqueue erzeugen
		/// </summary>
		/// <param name="eaiqoutInputs">EAIQOU Ausgangsdaten</param>
		public void createEaiqout(List<EaiqoutDto> eaiqoutInputs)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                foreach (EaiqoutDto eaiqoutInput in eaiqoutInputs)
                {
                    EAIQOU eaiOutput = Mapper.Map<EaiqoutDto,EAIQOU>(eaiqoutInput);
                    eaiOutput.SYSEAIHOT=eaiqoutInput.sysEaihot;
                    
                    owCtx.EAIQOU.Add(eaiOutput);
                }
                owCtx.SaveChanges();
            }
        }

        /// <summary>
        /// Bestehendes Eaihot holen
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        /// <returns>Eaihot Ausgang</returns>
        public EaihotDto getEaihot (long syseaihot)
        {
            EaihotDto rval = null;
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIHOT eaihotOutput = (from e in owCtx.EAIHOT
                                       where e.SYSEAIHOT == syseaihot
                                       select e).FirstOrDefault();

                if (eaihotOutput != null)
                {
                    rval = Mapper.Map<EAIHOT, EaihotDto>(eaihotOutput);
                    if (eaihotOutput.EAIART == null)
                        owCtx.Entry(eaihotOutput).Reference(f => f.EAIART).Load();

                    
                    rval.SYSEAIART = eaihotOutput.EAIART.SYSEAIART;
                }
            }
            return rval;
        }

        /// <summary>
        /// returns the eaihot for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public EaihotDto getEaihotByQuery (String query)
        {
            
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                return owCtx.ExecuteStoreQuery<EaihotDto>(query).FirstOrDefault();
            }
            
        }

        /// <summary>
        /// EAIHFILE des EAIHOT auslesen
        /// </summary>
        /// <param name="eaihot">EAIHOT</param>
        /// <returns>EAIHFILE Daten</returns>
        public EaihfileDto getEaiHotFile(long eaihot)
        {
            EaihfileDto rval = null;
            using (DdOwExtended owContext = new DdOwExtended())
            {
                rval = owContext.ExecuteStoreQuery<EaihfileDto>("select eaihfile.* from eaihfile where syseaihot=" + eaihot).FirstOrDefault();
                
            }
            return rval;
        }

        /// <summary>
        /// Returns the eaihfile by direct id
        /// </summary>
        /// <param name="syseaihfile"></param>
        /// <returns></returns>
        public EaihfileDto getEaiHFile (long syseaihfile)
        {
            EaihfileDto rval = null;
            using (DdOwExtended owContext = new DdOwExtended())
            {
                rval = owContext.ExecuteStoreQuery<EaihfileDto>("select eaihfile.* from eaihfile where syseaihfile=" + syseaihfile).FirstOrDefault();
            }
            return rval;
        }

        /// <summary>
        /// Eaihot löschen
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        public bool deleteEaihot(long syseaihot)
        {
            bool result = true;
            return result;
        }

        /// <summary>
        /// EaihotFile zusammen mit EaiHot löschen
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        public bool deleteEaihotFile (long syseaihot)
        {
            bool result = true;
            return result;
        }

        /// <summary>
        /// Eaihot kopieren
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        /// <returns></returns>
        public EaihotDto copyEaihot(long syseaihot)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eaihotDto"></param>
        /// <returns></returns>
        public EaihotDto updateEaihot (EaihotDto eaihotDto)
        {
            EaihotDto rval = null;
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIHOT eaihotOutput = (from e in owCtx.EAIHOT
                                       where e.SYSEAIHOT == eaihotDto.SYSEAIHOT
                                       select e).Single();
                if (eaihotOutput.EAIART == null)
                    owCtx.Entry(eaihotOutput).Reference(f => f.EAIART).Load();
                
                if (eaihotOutput != null)
                {
                    //eaihotOutput = Mapper.Map<EaihotDto, EAIHOT>(eaihotDto);
                    //owCtx.ObjectStateManager.GetObjectStateEntry(eaihotOutput).SetModified();
                    //owCtx.Attach(eaihotOutput);
                    eaihotOutput.CODE = eaihotDto.CODE;
                    eaihotOutput.COMLANGUAGE = eaihotDto.COMLANGUAGE;
                    eaihotOutput.COMPUTERNAME = eaihotDto.COMPUTERNAME;
                    eaihotOutput.EVALEXPRESSION = eaihotDto.EVALEXPRESSION;
                    eaihotOutput.EVE = eaihotDto.EVE;
                    eaihotOutput.EXECPRIORITY = eaihotDto.EXECPRIORITY;
                    eaihotOutput.FILEFLAGIN = eaihotDto.FILEFLAGIN;
                    eaihotOutput.FILEFLAGOUT = eaihotDto.FILEFLAGOUT;
                    eaihotOutput.FINISHDATE = eaihotDto.FINISHDATE;
                    eaihotOutput.FINISHTIME = eaihotDto.FINISHTIME;
                    eaihotOutput.GUILANGUAGE = eaihotDto.GUILANGUAGE;
                    eaihotOutput.HOSTCOMPUTER = eaihotDto.HOSTCOMPUTER;
                    eaihotOutput.INPUTPARAMETER1 = eaihotDto.INPUTPARAMETER1;
                    eaihotOutput.INPUTPARAMETER2 = eaihotDto.INPUTPARAMETER2;
                    eaihotOutput.INPUTPARAMETER3 = eaihotDto.INPUTPARAMETER3;
                    eaihotOutput.INPUTPARAMETER4 = eaihotDto.INPUTPARAMETER4;
                    eaihotOutput.INPUTPARAMETER5 = eaihotDto.INPUTPARAMETER5;
                    eaihotOutput.OLTABLE = eaihotDto.OLTABLE;
                    eaihotOutput.OUTPUTPARAMETER1 = eaihotDto.OUTPUTPARAMETER1;
                    eaihotOutput.OUTPUTPARAMETER2 = eaihotDto.OUTPUTPARAMETER2;
                    eaihotOutput.OUTPUTPARAMETER3 = eaihotDto.OUTPUTPARAMETER3;
                    eaihotOutput.OUTPUTPARAMETER4 = eaihotDto.OUTPUTPARAMETER4;
                    eaihotOutput.OUTPUTPARAMETER5 = eaihotDto.OUTPUTPARAMETER5;
                    eaihotOutput.PROZESSSTATUS = eaihotDto.PROZESSSTATUS;
                    eaihotOutput.RETURNPARAMETER = eaihotDto.RETURNPARAMETER;
                    eaihotOutput.STARTDATE = eaihotDto.STARTDATE;
                    eaihotOutput.STARTTIME = eaihotDto.STARTTIME;
                    eaihotOutput.SUBMITDATE = eaihotDto.SUBMITDATE;
                    eaihotOutput.SUBMITTIME = eaihotDto.SUBMITTIME;
                    eaihotOutput.SYSEAIART = (from art in owCtx.EAIART
                                           where art.SYSEAIART == eaihotDto.SYSEAIART
                                           select art.SYSEAIART).FirstOrDefault();
                    eaihotOutput.SYSEAIHOT = eaihotDto.SYSEAIHOT;
                    eaihotOutput.SYSOLTABLE = eaihotDto.SYSOLTABLE;
                    eaihotOutput.SYSPORTAL = eaihotDto.SYSPORTAL;
                    eaihotOutput.SYSWFEXEC = eaihotDto.SYSWFEXEC;
                    eaihotOutput.SYSWFUSER = eaihotDto.SYSWFUSER;
                    eaihotOutput.TRANSACTIONFLAG = eaihotDto.TRANSACTIONFLAG;
                    eaihotOutput.WFERROR = eaihotDto.WFERROR;
                    eaihotOutput.CLIENTART = 1;
                    owCtx.SaveChanges();
                    rval = Mapper.Map<EAIHOT, EaihotDto>(eaihotOutput);
                    if (eaihotOutput.EAIART != null)
                    {
                        rval.SYSEAIART = eaihotOutput.EAIART.SYSEAIART;
                    }
                }
            }
            return rval;
        }

        /// <summary>
        /// Bereich ID verifizieren
        /// </summary>
        /// <param name="area">Bereich</param>
        /// <param name="sysAreaId">Bereichs ID</param>
        /// <returns>Verifizierung</returns>
        public bool verifyAreaId (AreaConstants area, long sysAreaId)
        {
            // Create the entities
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Assume the id does not exist
                    bool IdExists = false;

                    switch (area)
                    {
                        case AreaConstants.Angebot:
                            var CurrentAngebot = (from Angebot in Entities.ANGEBOT
                                                  where Angebot.SYSID == sysAreaId
                                                  select Angebot).FirstOrDefault();
                            IdExists = CurrentAngebot != null;
                            break;

                        case AreaConstants.Antrag:
                            var CurrentAntrag = (from Antrag in Entities.ANTRAG
                                                 where Antrag.SYSID == sysAreaId
                                                 select Antrag).FirstOrDefault();
                            IdExists = CurrentAntrag != null;
                            break;

                        case AreaConstants.It:
                            var CurrentIt = (from It in Entities.ANTRAG
                                             where It.SYSID == sysAreaId
                                             select It).FirstOrDefault();
                            IdExists = CurrentIt != null;
                            break;
                        case AreaConstants.Vt:
                            var CurrentVt = (from Vt in Entities.VT
                                             where Vt.SYSID == sysAreaId
                                             select Vt).FirstOrDefault();
                            IdExists = CurrentVt != null;
                            break;
                    }
                    // Return the verification result
                    return IdExists;
                }
                catch (Exception exception)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not verify area id.", exception);
                }
            }
        }

        /// <summary>
        /// EAI Art auslesen
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>EAI Art Daten</returns>
        public EAIART getEaiArt (string code)
        {
            EAIART eaiArtOut = new EAIART();
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                eaiArtOut = (from eai in owCtx.EAIART
                             where eai.CODE.ToUpper().Equals(code.ToUpper())
                             select eai).FirstOrDefault();
            }
            if (eaiArtOut != null)
            {
                return eaiArtOut;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// EAIHOT aus altem Tabellencode und Sysart ermittlen
        /// </summary>
        /// <param name="sysid">sysID</param>
        /// <param name="oltable">Alte Tabellen ID</param>
        /// <param name="code">CODE</param>
        /// <param name="syseaiart">EAI Art ID</param>
        /// <returns>EAIHOT Daten</returns>
        public EaihotDto getEaiHotByOltableAndCodeAndSysart (long sysid, string oltable, string code, long syseaiart)
        {
            EaihotDto rval = new EaihotDto();
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EAIHOT eaiHot = (from hot in owCtx.EAIHOT
                                 where hot.SYSOLTABLE == sysid
                                 where hot.OLTABLE.ToUpper().Equals(oltable.ToUpper())
                                 where hot.CODE.ToUpper().Equals(code.ToUpper())
                                 where hot.SYSEAIART == (from art in owCtx.EAIART
                                                      where art.SYSEAIART == syseaiart
                                                      select art.SYSEAIART).FirstOrDefault()
                                 orderby hot.SYSEAIHOT descending
                                 select hot).FirstOrDefault();

                
                rval = Mapper.Map<EAIHOT, EaihotDto>(eaiHot);
            }
            return rval;
        }

        /// <summary>
        /// Liste von EAIHOT aus Code und sysPerson ermittlen
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sysPerson">Die sysId vom Vertriebspartner (sysOLTable)</param>
        /// <returns></returns>
        public List<EaihotDto> listEaiHotForCodeAndPerson (string code, long sysPerson)
        {
            List<EaihotDto> rval = new List<EaihotDto>();
            _log.Debug("eaiHotDao.listEaiHotForCodeAndPerson: code = " + code + ", sysPerson = " + sysPerson);
            using (DdOwExtended owContext = new DdOwExtended())
            {
                List<EAIHOT> eaiHotList = (from hot in owContext.EAIHOT
                                           where hot.SYSOLTABLE == sysPerson &&
                                                 hot.OLTABLE.ToUpper().Equals("PEROLE") &&
                                                 hot.CODE.ToUpper().Equals(code.ToUpper()) &&
                                                 hot.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready &&
                                                 (hot.FILEFLAGOUT == null || hot.FILEFLAGOUT <1)
                                            orderby hot.SYSEAIHOT descending
                                           select hot).ToList<EAIHOT>();
                _log.Debug("eaiHotDao.listEaiHotForCodeAndPerson: eaiHotList.count = " + eaiHotList.Count);
                
                foreach (var eaihot in eaiHotList)
                {
                    rval.Add(Mapper.Map<EAIHOT, EaihotDto>(eaihot));
                }
            }
            _log.Debug("eaiHotDao.listEaiHotForCodeAndPerson: rval.count = " + rval.Count);
            return rval;
        }

		/// <summary>
		/// EAI Queue Out's für EAIHOT ermitteln
		/// </summary>
		/// <param name="syseaihot">syseaihot</param>
		/// <param name="sort">sort</param>
		/// <returns>Liste mit Ausgangsqueues</returns>
		public List<EaiqoutDto> listEaiqouForEaihot (long syseaihot, bool sort)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syseaihot", Value = syseaihot });
                String query = GETEAIQOUFROMEAIHOT;
                if (sort)
                    query = GETEAIQOUFROMEAIHOTSORT;
                List<EAIQOU> data = owCtx.ExecuteStoreQuery<EAIQOU>(query, parameters.ToArray()).ToList();

                List<EaiqoutDto> listEaiqouOut = new List<EaiqoutDto>();
                foreach (var eaihot in data)
                {
                    listEaiqouOut.Add(Mapper.Map<EAIQOU, EaiqoutDto>(eaihot));
                }
                return listEaiqouOut;
            }
        }

		/// <summary>
		/// EAI Queue In für EAIHOT ermittlen
		/// </summary>
		/// <param name="syseaihot">EAIHOT</param>
		/// <returns>Liste mit Eingangsqueues</returns>
		public List<EaiqinDto> listEaiqinForEaihot (long syseaihot)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                List<EaiqinDto> listeaqinOut = new List<EaiqinDto>();
                var query = (from qin in owCtx.EAIQIN
                             where qin.EAIHOT.SYSEAIHOT == syseaihot
                             select qin).OrderBy(a => a.SYSEAIQIN);
                if (query != null)
                {
                    foreach (EAIQIN eaiqin in query)
                    {
                        listeaqinOut.Add(Mapper.Map < EAIQIN, EaiqinDto>(eaiqin));
                    }
                }
                return listeaqinOut;
            }
        }

        /// <summary>
        /// WFTX holen
        /// </summary>
        /// <param name="sysctlang">Sprach ID</param>
        /// <param name="syswftx">WFTX ID</param>
        /// <returns>String</returns>
        public string getWFTX (int sysctlang, int syswftx)
        {
            string rval = null;
            using (DdOlExtended olCtx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysctlang", Value = sysctlang });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswftx", Value = syswftx });

                rval = olCtx.ExecuteStoreQuery<string>(QUERYBEZEICHNUNGWFTXBYSYSID, parameters.ToArray()).FirstOrDefault<string>();
            }
            return rval;
        }

        /// <summary>
        /// EAIHOT's nach alter TableID und SYSART auflisten
        /// </summary>
        /// <param name="sysid">SYSID</param>
        /// <param name="oltable">alte Table ID</param>
        /// <param name="code">CODE</param>
        /// <param name="syseaiart">SYS EAI Art</param>
        /// <returns>Liste mit EAIHOT's</returns>
        public List<EaihotDto> listEaiHotByOltableAndCodeAndSysart (long sysid, string oltable, string code, long syseaiart)
        {
            List<EaihotDto> rval = new List<EaihotDto>();
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                List<EAIHOT> eaiHot = (from hot in owCtx.EAIHOT
                                       where hot.SYSOLTABLE == sysid
                                       where hot.OLTABLE.ToUpper().Equals(oltable)
                                       where hot.CODE.ToUpper().Equals(code.ToUpper())
                                       where hot.SYSEAIART == (from art in owCtx.EAIART
                                                            where art.SYSEAIART == syseaiart
                                                            select art.SYSEAIART).FirstOrDefault()
                                       select hot).ToList();
                
                foreach (EAIHOT hot in eaiHot)
                {
                    rval.Add(Mapper.Map<EAIHOT, EaihotDto>(hot));
                }
            }
            return rval;
        }

        #region Private Methods

        private static EAIART getEaiArt(string code, DdOwExtended owCtx)
        {
            try
            {
                // Query EAIART
                var CurrentEaiArt = (from EaiArt in owCtx.EAIART
                                     where EaiArt.CODE.ToUpper().Equals(code.ToUpper())
                                     select EaiArt).FirstOrDefault();

                // Check if nothing was found
                if (CurrentEaiArt == null)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not find " + code + " in EAIART.");
                }

                // Return the id
                return CurrentEaiArt;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get eai art.", e);
            }
        }


        private static EAIART getEaiArtBasic(string code, DdOwExtended owCtx) {
            try
            {
                // Query EAIART
                var CurrentEaiArt = (from EaiArt in owCtx.EAIART
                                     where EaiArt.CODE.ToUpper().Equals(code.ToUpper())
                                     select EaiArt).FirstOrDefault();

                return CurrentEaiArt;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get eai art.", e);
            }
        }
        #endregion Private Methods
    }
}