using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using Cic.OpenOne.Common.DAO;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Angebot/ANtrag Drucken
    /// </summary>
    public class PrintAngAntBo : AbstractPrintAngAntBo
    {
        const int PRINTTIMEOUT = 60;
        const int LISTDOCTIMEOUT = 45;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eaihotDao">EAIHOT Daten</param>
        /// <param name="angAntDao">Angebot/Antrag Daten</param>
        /// <param name="dictionaryListDao">Dictionary List data</param>
        public PrintAngAntBo(IEaihotDao eaihotDao, IAngAntDao angAntDao, IDictionaryListsDao dictionaryListDao)
            : base(eaihotDao, angAntDao, dictionaryListDao)
        {
        }

        /// <summary>
        /// listAvailableDokumente
        /// </summary>
        /// <param name="type">Typ</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="sysperson">Personen ID</param>
        /// <param name="userLanguage">userLanguage</param>
        /// <param name="subArea">subArea</param>
        /// <returns></returns>
        public override DokumenteDto[] listAvailableDokumente(DTO.Enums.EaiHotOltable type, long sysid, long sysperson, String userLanguage, String subArea, long sysperole)
        {
            return listAvailableDokumente(type, sysid, sysperson, userLanguage, subArea, false,sysperole);
        }
        public override DokumenteDto[] listAvailableDokumente(DTO.Enums.EaiHotOltable type, long sysid, long sysperson, String userLanguage, String subArea, bool sort, long sysperole)
        {
            List<DokumenteDto> dokumente = new List<DokumenteDto>();
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                EaihotDto eaioutput = new EaihotDto();
                EAIART art = eaihotDao.getEaiArt("DOK_LIST_FE");
                if (art != null)
                {
                    if (type == DTO.Enums.EaiHotOltable.Antrag)
                    {
                        AntragDto antrag = angAntDao.getAntrag(sysid,sysperole);
                        if (antrag == null)
                        {
                            throw new Exception("Antrag not found:" + sysid);
                        }
                        if (antrag.Drucksperre >= 1)
                        {
                            return null;
                        }
                        eaioutput = new EaihotDto()
                        {
                            CODE = "DOK_LIST_FE",
                            OLTABLE = "ANTRAG",
                            SYSOLTABLE = sysid,
                            SYSEAIART = art.SYSEAIART,
                            SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                            SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                            EVE = 1,
                            PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                            INPUTPARAMETER1 = sysperson.ToString(),
                            INPUTPARAMETER2 = subArea,
                            HOSTCOMPUTER = "*"
                        };
                        eaioutput = eaihotDao.createEaihot(eaioutput);
                    }
                    else if (type == DTO.Enums.EaiHotOltable.Angebot)
                    {
                        AngebotDto angebot = angAntDao.getAngebot(sysid);
                        if (angebot == null)
                        {
                            throw new Exception("Angebot not found:" + sysid);
                        }
                        if (angebot.Drucksperre >= 1)
                        {
                            return null;
                        }
                        eaioutput = new EaihotDto()
                        {
                            CODE = "DOK_LIST_FE",
                            OLTABLE = "ANGEBOT",
                            SYSOLTABLE = sysid,
                            SYSEAIART = art.SYSEAIART,
                            SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                            SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                            EVE = 1,
                            PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                            INPUTPARAMETER1 = sysperson.ToString(),
                            INPUTPARAMETER2 = subArea,
                            HOSTCOMPUTER = "*"
                        };
                        eaioutput = eaihotDao.createEaihot(eaioutput);
                    }
                }

                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 0, MyGetTimeOutValue("LISTDOCTIMEOUT", LISTDOCTIMEOUT));
                while (eaioutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    eaioutput = eaihotDao.getEaihot(eaioutput.SYSEAIHOT);
                    System.Threading.Thread.Sleep(500);
                }

                if (eaioutput.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {
                    EAIHOT eaihotOutput = Mapper.Map<EaihotDto, EAIHOT>(eaioutput);

                    DropListDto[] languageCode = dictionaryListDao.deliverCTLANG_PRINT();
                    int userLangId = 0;
                    foreach (DropListDto spracheAusListe in languageCode)
                    {
                        if (spracheAusListe.code.Equals(userLanguage))
                        {
                            userLangId = (int)spracheAusListe.sysID;
                            break;
                        }
                    }

                    foreach (EaiqoutDto eaiqou in eaihotDao.listEaiqouForEaihot(eaihotOutput.SYSEAIHOT,sort))
                    {
                        string sprache = "";
                        foreach (DropListDto spracheAusListe in languageCode)
                        {
                            if (spracheAusListe.sysID == Convert.ToUInt32(eaiqou.F02))
                            {
                                sprache = spracheAusListe.code;
                                break;
                            }
                        }
                        if (sprache == "")
                        {
                            sprache = "de-CH";
                            foreach (DropListDto spracheAusListe in languageCode)
                            {
                                if (spracheAusListe.code == sprache)
                                {
                                    eaiqou.F02 = spracheAusListe.sysID.ToString();
                                    break;
                                }
                            }
                        }
                        dokumente.Add(new DokumenteDto()
                        {
                            Bezeichnung = eaihotDao.getWFTX(userLangId, Convert.ToInt32(eaiqou.F01)),
                            sysEaiquo = eaiqou.SYSEAIQOU,
                            DokumentenID = Convert.ToUInt32(eaiqou.F01),
                            DefaultSprache = sprache,
                            AdditionalSprache = eaiqou.F15,
                            KundenExemplar = Convert.ToInt32(eaiqou.F03),
                            VertriebspatnerExemplar = Convert.ToInt32(eaiqou.F04),
                            BnowMitarbeiterExemplar = Convert.ToInt32(eaiqou.F05),
                            Druck = Convert.ToInt32(eaiqou.F06),
                            MAExemplar = Convert.ToInt32(eaiqou.F12),
                            BGExemplar = Convert.ToInt32(eaiqou.F11)
                        });
                    }
                }
                else
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get dokumentlist (timeout).");
                }
            }
            return dokumente.ToArray();
        }

        /// <summary>
        /// Drucken ausgewähler Dokumente
        /// </summary>
        /// <param name="dokumente">Dokumentenliste</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="type">Typ</param>
        /// <param name="sysperson">Person ID</param>
        /// <param name="eCodeEintragund">eCodeEintragund</param>
        /// <returns>Binaerdaten</returns>
        public override byte[] printCheckedDokumente(DokumenteDto[] dokumente, long sysid, long? variantenid, DTO.Enums.EaiHotOltable type, long sysperson, bool eCodeEintragung)
        {
            long syseaihot = 0;
            return printCheckedDokumente(dokumente, sysid, variantenid, type, sysperson, eCodeEintragung,ref syseaihot);
        }
        public override byte[] printCheckedDokumente(DokumenteDto[] dokumente, long sysid, long? variantenid, DTO.Enums.EaiHotOltable type, long sysperson, bool eCodeEintragung, ref long syseaihot)
        {
            byte[] rval = null;
            EAIART eaiArt = eaihotDao.getEaiArt("DRUCKAUFTRAG_FE");
            string oltable = "";
            if (type == EaiHotOltable.Antrag)
            {
                oltable = "ANTRAG";
            }
            else if (type == EaiHotOltable.Angebot)
            {
                oltable = "ANGEBOT";
            }

            EaihotDto eaihot = null;
            if (eaiArt != null)
            {
                eaihot = new EaihotDto()
                {
                    CODE = "DRUCKAUFTRAG_FE",
                    OLTABLE = oltable,
                    SYSOLTABLE = sysid,
                    SYSEAIART = eaiArt.SYSEAIART,
                    SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 0,
                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    INPUTPARAMETER1 = sysperson.ToString(),
                    HOSTCOMPUTER = "*",
                    INPUTPARAMETER2 = eCodeEintragung.ToString()
                    
                    
                };
                if (variantenid != null)
                {
                    eaihot.INPUTPARAMETER3 = variantenid.ToString();
                }

                eaihot = eaihotDao.createEaihot(eaihot);

                DropListDto[] languageCode = dictionaryListDao.deliverCTLANG_PRINT();
                int? sprachid = new int();

                foreach (DokumenteDto dokument in dokumente)
                {
                    sprachid = 0;
                    foreach (DropListDto spracheAusListe in languageCode)
                    {
                        if (spracheAusListe.code == dokument.DefaultSprache)
                        {
                            sprachid = (int)spracheAusListe.sysID;
                        }
                    }
                    if (sprachid == 0)
                    {
                        throw new ArgumentException("Keine gültige Sprache gefunden für Dokumentensprache: " + dokument.DefaultSprache + " vom Dokument: " + dokument.DokumentenID);
                    }
                    eaihotDao.createEaiqin(new EaiqinDto()
                    {
                        sysEaihot = eaihot.SYSEAIHOT,
                        F20 = dokument.sysEaiquo.ToString(),
                        F01 = dokument.DokumentenID.ToString(),
                        F02 = sprachid.ToString(),
                        F03 = dokument.KundenExemplar.ToString(),
                        F04 = dokument.VertriebspatnerExemplar.ToString(),
                        F05 = dokument.BnowMitarbeiterExemplar.ToString(),
                        F06 = dokument.Druck.ToString(),
                    });
                }
                eaihot.EVE = 1;
                eaihot = eaihotDao.updateEaihot(eaihot);
                syseaihot = eaihot.SYSEAIHOT;
                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 0, MyGetTimeOutValue("PRINTTIMEOUT", PRINTTIMEOUT));
                while (eaihot.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    eaihot = eaihotDao.getEaihot(eaihot.SYSEAIHOT);
                    System.Threading.Thread.Sleep(500);
                }
                if (eaihot.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {

                    EaihfileDto fileOutput = eaihotDao.getEaiHotFile(eaihot.SYSEAIHOT);
                    if (fileOutput != null)
                    {
                        if (fileOutput.EAIHFILE != null)
                        {
                            rval = fileOutput.EAIHFILE;
                        }
                    }
                }
                else
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get dokumentfile (timeout).");
                }
            }
            return rval;
        }

        /// <summary>
        /// Drucken ausgewähler Dokumente
        /// </summary>
        /// <param name="dokumente">Dokumentenliste</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="type">Typ</param>
        /// <param name="sysperson">Person ID</param>
        /// <returns>Binaerdaten</returns>
        public override byte[] checkPrintCheckedDokumente(DokumenteDto[] dokumente, long sysid, long? variantenid, DTO.Enums.EaiHotOltable type, long sysperson)
        {
            byte[] rval = null;
            string typ = "";
            EAIART art = eaihotDao.getEaiArt("DRUCKAUFTRAG_FE");
            if (DTO.Enums.EaiHotOltable.Angebot == type)
            {
                typ = "ANGEBOT";
            }
            else if (DTO.Enums.EaiHotOltable.Antrag == type)
            {
                typ = "ANTRAG";
            }
            if (art != null && typ != "")
            {
                List<EaihotDto> eaihots = eaihotDao.listEaiHotByOltableAndCodeAndSysart(sysid, typ, "DRUCKAUFTRAG_FE", art.SYSEAIART);
                EaihotDto eaihot = (from eai in eaihots
                                    orderby eai.SYSEAIHOT descending
                                    select eai).First();
                if (eaihot.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready)
                {
                    DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    TimeSpan timeOut = new TimeSpan(0, 0, 0, MyGetTimeOutValue("PRINTTIMEOUT", PRINTTIMEOUT));
                    while (eaihot.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                    {
                        eaihot = eaihotDao.getEaihot(eaihot.SYSEAIHOT);
                        System.Threading.Thread.Sleep(500);
                    }
                    if (eaihot.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready)
                    {
                        // Throw an exception
                        throw new ApplicationException("Could not get dokumentfile (timeout).");
                    }
                }
                EaihfileDto fileOutput = eaihotDao.getEaiHotFile(eaihot.SYSEAIHOT);
                if (fileOutput != null)
                {
                    if (fileOutput.EAIHFILE != null)
                    {
                        rval = fileOutput.EAIHFILE;
                    }
                }
            }
            return rval;
        }

        /// <summary>
        /// Prüfung auf Timeout
        /// </summary>
        /// <param name="oldDate">Alter Zeitstempel</param>
        /// <param name="timeOut">NeuerZeitstempel</param>
        /// <returns>Timeout true= Timeout/false = kein Timeout</returns>
        public static bool isTimeOut(DateTime oldDate, TimeSpan timeOut)
        {
            TimeSpan ts = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) - oldDate;

            if (ts > timeOut)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Holt den Timeout-Parameter für den Print-Service aus der CFG
        /// </summary>
        /// <returns>Timeout-Wert</returns>
        private static int MyGetTimeOutValue(String timeoutKey, int defaultValue)
        {
            int retValue = 0;
            String cfgParam = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry("WEBSERVICES", timeoutKey, defaultValue.ToString(), "SETUP.NET");
            Int32.TryParse(cfgParam, out retValue);

            return retValue;
        }
    }
}