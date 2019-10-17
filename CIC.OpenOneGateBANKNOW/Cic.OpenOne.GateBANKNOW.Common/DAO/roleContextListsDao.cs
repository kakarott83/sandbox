using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdCt;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OL.EF6.Model;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Data access object for role context lists
    /// </summary>
    public class RoleContextListsDao : IRoleContextListsDao
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<String, List<StateInfo>> stateCache = CacheFactory<String, List<StateInfo>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private DateTime nullDate = new DateTime(1800, 1, 1);
        /// <summary>
        /// SQL
        /// </summary>
        private static string QUERYOBTYPEN = "select * from vc_obtyp1 order by bezeichnung";
        private static String QUERYALERTS = @"SELECT * FROM (
                              SELECT /*+ INDEX(statealrt, STATEALRT_AREALEASE)*/statealrt.*, antrag.antrag
                              FROM statealrt, antrag
                              WHERE statealrt.SYSLEASE = antrag.sysid AND statealrt.area = 'ANTRAG' AND statealrt.reader IS NULL
                                AND antrag.sysid IN (SELECT peuni.sysid FROM peuni WHERE peuni.sysperole IN (SELECT sysid FROM TABLE(cic.CIC_PEROLE_UTILS.GetTabROLEIDFromPEROLE(:sysperole))) AND peuni.area = 'ANTRAG')
                                AND statealrt.ZUSTAND NOT IN ('In Prüfung')
                              UNION ALL
                              SELECT statealrt.*, a.antrag
                              FROM statealrt, antrag a
                              WHERE statealrt.syslease = a.sysid AND statealrt.area = 'ANTRAG' AND statealrt.reader IS NULL
                                AND statealrt.sysstatealrt IN (
                                  SELECT MIN (statealrt.sysstatealrt)
                                  FROM statealrt
                                  WHERE statealrt.area = 'ANTRAG' AND statealrt.syslease IN (SELECT peuni.sysid FROM peuni WHERE peuni.sysperole IN (SELECT sysid FROM TABLE(cic.CIC_PEROLE_UTILS.GetTabROLEIDFromPEROLE(:sysperole))) AND peuni.area = 'ANTRAG')
                                    AND statealrt.zustand  IN ('In Prüfung')
                                  GROUP BY statealrt.syslease
                                  )
                            ) a
                            ORDER BY a.alertdate DESC
                            ";
        private static string QUERYSTATES = "select zustand, cttstatedef.replacezustand, isocode from cttstatedef, statedef, ctlang where cttstatedef.sysstatedef = statedef.sysstatedef and cttstatedef.sysctlang = ctlang.sysctlang";

        /*private static string QUERYPROFINLOCK = "SELECT COUNT(*) " +
                                                "FROM wftzust, " +
                                                "wftzvar " +
                                                "WHERE wftzvar.SYSWFTZUST  = wftzust.SYSWFTZUST " +
                                                "AND upper(wftzust.Gebiet) = 'ANTRAG' " +
                                                "AND wftzust.STATE         = 'AUTOCHECK' " +
                                                "AND (wftzvar.value        = 'Start' or WFTZUST.status = 0) " +
                                                "AND wftzust.syslease      = :psysid ";
                                                */


        //Holds Brand Fields
        class ObTypFields
        {
            public string bezeichnung { get; set; }
            public long id1 { get; set; }
            public string art { get; set; }
        }

        /// <summary>
        /// get the available Brands
        /// </summary>
        /// <param name="sysPEROLE">Perole of the user</param>
        /// <returns>Array of Brands</returns>
        public DropListDto[] getBrands(long sysPEROLE)
        {
            DropListDto[] dropListDto;

            _log.Debug("Create OlEntities context");
            using (DdOlExtended olCtx = new DdOlExtended())
            {
                _log.Debug("Created OlEntities context");
                long sysVpPerole = PeRoleUtil.FindRootPEROLEByRoleType(olCtx , sysPEROLE, (long)RoleTypeTyp.HAENDLER);
                if (sysVpPerole == 0)
                {
                    _log.Error("No Händler found to this User: " + sysPEROLE);
                    throw new ApplicationException("No Händler found to this User: " + sysPEROLE);
                }

                DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);

                var query = from BRAND in olCtx.BRAND // Selektiere alle BRANDs
                            join PRBRANDM in olCtx.PRBRANDM on BRAND.SYSBRAND equals PRBRANDM.BRAND.SYSBRAND // aus der Liste der Brands al ler
                            join PRHGROUPM in olCtx.PRHGROUPM on PRBRANDM.PRHGROUP.SYSPRHGROUP equals PRHGROUPM.PRHGROUP.SYSPRHGROUP // Handelsgruppen des Verkäufers
                            join PEROLEVP in olCtx.PEROLE on PRHGROUPM.PEROLE.SYSPEROLE equals PEROLEVP.SYSPEROLE // Verkäuferrolle
                            where PEROLEVP.ROLETYPE.TYP == (int)RoleTypeTyp.HAENDLER  // Einschränkung für Verkäuferrolle
                            && PEROLEVP.SYSPEROLE == sysVpPerole // Konkreter Verkäufer
                            && PRHGROUPM.ACTIVEFLAG == 1
                            && (PRHGROUPM.VALIDFROM == null || PRHGROUPM.VALIDFROM <= aktuell || PRHGROUPM.VALIDFROM <= nullDate)
                            && (PRHGROUPM.VALIDUNTIL == null || PRHGROUPM.VALIDUNTIL >= aktuell || PRHGROUPM.VALIDUNTIL <= nullDate)
                            orderby PRHGROUPM.DEFAULTFLAG descending, BRAND.SYSBRAND
                            select BRAND;

                List<DropListDto> dropListDtoList = new List<DropListDto>();
                foreach (BRAND item in query)
                {
                    if (item.ACTIVEFLAG == 1)
                    {
                        dropListDtoList.Add(
                            new DropListDto()
                            {
                                code = item.SYSBRAND.ToString(),
                                sysID = (long)item.SYSBRAND,
                                beschreibung = item.DESCRIPTION,
                                bezeichnung = item.NAME
                            });
                    }
                }
                dropListDto = dropListDtoList.ToArray();
            }
            return dropListDto;
        }

        /// <summary>
        /// get the available Channels
        /// </summary>
        /// <param name="sysPEROLE">Role of the user</param>
        /// <returns>Array of Channels</returns>
        public DropListDto[] getChannels(long sysPEROLE)
        {
            DropListDto[] dropListDto;

            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                _log.Debug("Created OlEntities context");
                long sysVpPerole = PeRoleUtil.FindRootPEROLEByRoleType(olCtx , sysPEROLE, (long)RoleTypeTyp.HAENDLER);

                if (sysVpPerole == 0)
                {
                    _log.Error("No Händler found to this User: " + sysPEROLE);
                    throw new ApplicationException("No Haendler found to this User: " + sysPEROLE);
                }

                DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);

                var query = from BCHANNEL in olCtx.BCHANNEL
                            join PRCHANNELM in olCtx.PRCHANNELM on BCHANNEL.SYSBCHANNEL equals PRCHANNELM.BCHANNEL.SYSBCHANNEL
                            join VPPEROLE in olCtx.PEROLE on PRCHANNELM.PEROLE.SYSPEROLE equals VPPEROLE.SYSPEROLE
                            where VPPEROLE.ROLETYPE.TYP == (long)RoleTypeTyp.HAENDLER
                            && VPPEROLE.SYSPEROLE == sysVpPerole
                            && PRCHANNELM.ACTIVEFLAG == 1
                            && (PRCHANNELM.VALIDFROM == null || PRCHANNELM.VALIDFROM <= aktuell || PRCHANNELM.VALIDFROM <= nullDate)
                            && (PRCHANNELM.VALIDUNTIL == null || PRCHANNELM.VALIDUNTIL >= aktuell || PRCHANNELM.VALIDUNTIL <= nullDate)
                            orderby BCHANNEL.DESCRIPTION
                            select BCHANNEL;

                List<DropListDto> dropListDtoList = new List<DropListDto>();
                foreach (BCHANNEL item in query)
                {
                    if (item.ACTIVEFLAG == 1)
                    {
                        dropListDtoList.Add(
                            new DropListDto()
                            {
                                code = item.SYSBCHANNEL.ToString(),
                                sysID = (long)item.SYSBCHANNEL,
                                beschreibung = item.DESCRIPTION,
                                bezeichnung = item.NAME
                            });
                    }
                }
                dropListDto = dropListDtoList.ToArray();
            }
            return dropListDto;
        }

        /// <summary>
        /// Get the available Kundentypen
        /// </summary>
        /// <returns>Array of Kundentypen</returns>
        public DropListDto[] getKundentypen()
        {
            DropListDto[] dropListDto;
            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended context = new DdOlExtended())
            {
                _log.Debug("Created OlEntities context");

                // Ticket#2012082110000048 - Sortierung nach syskdtyp, die privaten sollen zuerst erscheinen
                // Kundentypen werden zunächst ohne Rollenbezug implementiert
                var query = from KDTYP in context.KDTYP
                            orderby KDTYP.SYSKDTYP ascending
                            select KDTYP;

                List<DropListDto> dropListDtoList = new List<DropListDto>();
                foreach (KDTYP item in query)
                {
                    if (item.ACTIVEFLAG == 1)
                    {
                        dropListDtoList.Add(new DropListDto()
                                            {
                                                sysID = (long)item.SYSKDTYP,
                                                code = item.SYSKDTYP.ToString(),        // changed, because of mix-up in GUI and Webservices 
                                                beschreibung = item.DESCRIPTION,
                                                bezeichnung = item.NAME
                                            });
                    }
                }
                dropListDto = dropListDtoList.ToArray();
            }
            return dropListDto;
        }

        /// <summary>
        /// get the available nutzungsarten
        /// </summary>
        /// <returns>Array of Nutzungsarten</returns>
        public DropListDto[] getNutzungsarten()
        {
            DropListDto[] dropListDto;

            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                _log.Debug("Created OlEntities context");
                var query = from OBUSETYPE in olCtx.OBUSETYPE
                            orderby OBUSETYPE.DESCRIPTION descending
                            select OBUSETYPE;

                List<DropListDto> dropListDtoList = new List<DropListDto>();
                foreach (OBUSETYPE item in query)
                {
                    if (item.ACTIVEFLAG == 1)
                    {
                        dropListDtoList.Add(
                            new DropListDto()
                            {
                                code = item.USETYPE.ToString(),
                                sysID = (long)item.SYSOBUSETYPE,
                                beschreibung = item.DESCRIPTION,
                                bezeichnung = item.NAME
                            });
                    }
                }
                dropListDto = dropListDtoList.ToArray();
            }
            return dropListDto;
        }

        /// <summary>
        ///  get the available objektarten
        /// </summary>
        /// <returns>Array of Objektarten</returns>
        public DropListDto[] getObjektarten()
        {
            DropListDto[] dropListDto;

            _log.Debug("Create OlEntities context");
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                _log.Debug("Created OlEntities context");
                var query = from OBART in olCtx.OBART // zunächst ohne rollenkontext
                            orderby OBART.DESCRIPTION
                            select OBART;
                //var query2 = from OB_ART in query
                //             select new queryResultDto() // bug..
                //                {
                //                    message = new Message(),
                //                    sysID = OB_ART.SYSOBART,
                //                    beschreibung = OB_ART.DESCRIPTION,
                //                    bezeichnung = OB_ART.NAME
                //                };
                //dropListDto = MyMapQueryResultDtoToDropListDto(query2.ToArray());

                List<DropListDto> dropListDtoList = new List<DropListDto>();
                foreach (OBART item in query)
                {
                    if (item.ACTIVEFLAG == 1)
                    {
                        dropListDtoList.Add(
                            new DropListDto()
                            {
                                code = item.TYP.ToString(),
                                sysID = (long)item.SYSOBART,
                                beschreibung = item.DESCRIPTION,
                                bezeichnung = item.NAME
                            });
                    }
                }
                dropListDto = dropListDtoList.ToArray();
            }
            return dropListDto;
        }

        /// <summary>
        /// get the available objekttypen
        /// </summary>
        /// <returns>Array of Objekttypen</returns>
        public DropListDto[] getObjekttypen(bool withOthers)
        {
            DropListDto[] dropListDto;

            _log.Debug("Create OlEntities context");
            using (DdCtExtended olCtx = new DdCtExtended())
            {
                _log.Debug("Created OlEntities context");
                List<long> artOrder = new List<long>();
                //PW, LW, WW, MOT, Boot, Zub, Andere
                artOrder.Add(100);
                artOrder.Add(120);
                artOrder.Add(2600);
                artOrder.Add(130);
                artOrder.Add(700);
                artOrder.Add(2400);
                artOrder.Add(2900);

                List<ObTypFields> valuesOrg = olCtx.ExecuteStoreQuery<ObTypFields>(QUERYOBTYPEN, null).ToList();
                List<ObTypFields> values = new List<ObTypFields>();
                foreach (long art in artOrder)
                {
                    foreach (ObTypFields item in valuesOrg)
                    {
                        if (long.Parse(item.art) == art)
                        {
                            if (!withOthers && long.Parse(item.art) == 2900 && item.bezeichnung == "Andere") continue;//Defect 5766, dont show this
                            values.Add(item);
                            break;
                        }
                    }
                }

                List<DropListDto> dropListDtoList = new List<DropListDto>();
                foreach (ObTypFields item in values)
                {
                    dropListDtoList.Add(
                           new DropListDto()
                           {
                               code = item.art,
                               sysID = (long)item.id1,
                               beschreibung = item.bezeichnung,
                               bezeichnung = item.bezeichnung
                           });
                }


                dropListDto = dropListDtoList.ToArray();
            }
            return dropListDto;
        }


        /// <summary>
        /// get the available objekttypen
        /// </summary>
        /// <returns>Array of Objekttypen</returns>
        public DropListDto[] getObjekttypen(bool withOthers,long sysPEROLE) {
            DropListDto[] dropListDto;

            _log.Debug("Create OlEntities context");
            using (DdCtExtended olCtx = new DdCtExtended())
            {
                _log.Debug("Created OlEntities context");
                //BNRVZ-537 CR345 bei BankNow nicht über DDLKPPOS 
                String VPAuspraegungquery = "SELECT " +
                               " case when SUM(DECODE(sysROLEATTRIB,9,1,0)) <= 0 AND  SUM(DECODE(sysROLEATTRIB,25,1,0)) > 0  then 'MOTORAD' "+
                               " ELSE (  " +
                               " case when SUM(DECODE(sysROLEATTRIB,21,1,0)) > 0  then 'BOOT' "+
                               " ELSE 'GARAGE' end) "+
                               " end AS PREGUNG "+
                               " from ROLEATTRIBM where sysperole = " + sysPEROLE;
                string VPAuspraegung = olCtx.ExecuteStoreQuery<string>(VPAuspraegungquery, null).FirstOrDefault();

                List<long> artOrder = new List<long>();
                //PW, LW, WW, MOT, Boot, Zub, Andere
                artOrder.Add(100);
                artOrder.Add(120);
                artOrder.Add(2600);
                artOrder.Add(130);
                artOrder.Add(700);
                artOrder.Add(2400);
                artOrder.Add(2900);

                List<ObTypFields> valuesOrg = olCtx.ExecuteStoreQuery<ObTypFields>(QUERYOBTYPEN, null).ToList();
                List<ObTypFields> values = new List<ObTypFields>();
                List<ObTypFields> valuesSort = new List<ObTypFields>();
                ObTypFields itemFirst = new ObTypFields(); 
                foreach (long art in artOrder)
                {
                    foreach (ObTypFields item in valuesOrg)
                    {
                        if (long.Parse(item.art) == art)
                        {

                            if (!withOthers && long.Parse(item.art) == 2900 && item.bezeichnung == "Andere") continue;//Defect 5766, dont show this
                            if (long.Parse(item.art) == 100 && VPAuspraegung.Equals("GARAGE")) itemFirst = item;
                            if (long.Parse(item.art) == 130 && VPAuspraegung.Equals("MOTORAD")) itemFirst = item;
                            if (long.Parse(item.art) == 700 && VPAuspraegung.Equals("BOOT")) itemFirst = item; 
                            else valuesSort.Add(item);
                            break;
                        }
                    }
                }
                values.Add(itemFirst);
                values.AddRange(valuesSort);
                List<DropListDto> dropListDtoList = new List<DropListDto>();
                foreach (ObTypFields item in values)
                {
                    dropListDtoList.Add(
                           new DropListDto()
                           {
                               code = item.art,
                               sysID = (long)item.id1,
                               beschreibung = item.bezeichnung,
                               bezeichnung = item.bezeichnung
                           });
                }


                dropListDto = dropListDtoList.ToArray();
            }
            return dropListDto;
        }
        /*private DropListDto[] MyMapQueryResultDtoToDropListDto(queryResultDto[] queryResultDto)
        {
            List<DropListDto> dropListDtoList;
            dropListDtoList = new List<DropListDto>();

            foreach(queryResultDto queryResult in queryResultDto)
            {
                dropListDtoList.Add(
                    new DropListDto()
                    {
                        message = queryResult.message,
                        sysID = (long)queryResult.sysID,
                        beschreibung = queryResult.beschreibung,
                        bezeichnung = queryResult.bezeichnung
                    });
            }
            return dropListDtoList.ToArray();
        }*/

        

        /// <summary>
        /// Verfügbare Alarmmeldungen auflisten
        /// </summary>
        /// <returns>Alarmliste</returns>
        public AvailableAlertsDto[] listAvailableAlerts(string isoCode, long sysperole)
        {
            List<StateDefDto> allAlerts;
            List<AvailableAlertsDto> rval = new List<AvailableAlertsDto>();
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended olCtx = new DdOwExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });

                allAlerts = olCtx.ExecuteStoreQuery<StateDefDto>(QUERYALERTS, parameters.ToArray()).ToList();
            }

            if (allAlerts == null)
                allAlerts = new List<StateDefDto>();

            if (!stateCache.ContainsKey("K"))
            {
                using (Cic.OpenOne.Common.Model.DdCt.DdCtExtended context = new DdCtExtended())
                {
                    stateCache["K"] = context.ExecuteStoreQuery<StateInfo>(QUERYSTATES, null).ToList();
                }
            }
            List<StateInfo> states = stateCache["K"];


            int count = 0;
            foreach (StateDefDto alrt in allAlerts)
            {
                //int proFinLock = 0;
                if (count == 15) break;

                String zustand = (from s in states
                                  where s.isocode.Equals(isoCode) && s.zustand.Equals(alrt.ZUSTAND)
                                  select s.replacezustand).FirstOrDefault();
                if (zustand == null)
                    zustand = alrt.ZUSTAND;
                String zustandalt = (from s in states
                                     where s.isocode.Equals(isoCode) && s.zustand.Equals(alrt.ZUSTANDALT)
                                     select s.replacezustand).FirstOrDefault();
                if (zustandalt == null)
                    zustandalt = alrt.ZUSTANDALT;


                if (!alrt.ALERTTIME.HasValue)
                    alrt.ALERTTIME = 0;

                //if (zustand.Equals("Finanzierungsvorschlag"))
                   // proFinLock = getProFinLock((long)alrt.SYSLEASE);

                rval.Add(new AvailableAlertsDto
                {
                    datum = alrt.ALERTDATE.Value,
                    kunde = alrt.INFO02,
                    marke = alrt.INFO06,
                    modell = alrt.INFO05,
                    ort = alrt.INFO03,
                    statusAlt = zustand,
                    statusNeu = zustandalt,
                    sysID = (long)alrt.SYSLEASE,
                    telefon = alrt.INFO04,
                    vertragsart = alrt.INFO01,
                    zeit = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDateTime(Convert.ToInt32(alrt.ALERTTIME)),
                    antragsNummer = alrt.ANTRAG
                    //proFinLock = proFinLock
                });
                count++;

            }
            return rval.ToArray();
        }


      /*  public int getProFinLock(long sysid)
        {
            int profinlock = 0;
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended olCtx = new DdOwExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid});

                profinlock = olCtx.ExecuteStoreQuery<int>(QUERYPROFINLOCK, parameters.ToArray()).FirstOrDefault();
                return profinlock;
            }


        }*/

        /// <summary>
        /// Alarmmeldungen als gelesen markieren
        /// </summary>
        /// <param name="antrag">Antrag ID</param>
        /// <param name="userid">Benutzer ID</param>
        public void setAlertsAsReaded(long antrag, long userid)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended olCtx = new DdOwExtended())
            {
                var allAlerts = from statealrt in olCtx.STATEALRT // vorerst ohne rollenkontext
                                where statealrt.SYSLEASE == antrag
                                && statealrt.AREA.Equals("ANTRAG")
                                && statealrt.READER == null
                                select statealrt;
                foreach (STATEALRT alrt in allAlerts)
                {
                    alrt.READDATE = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    alrt.READER = userid.ToString();
                    alrt.READTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    olCtx.SaveChanges();
                }
            }
        }
    }

    class StateInfo
    {
        public String zustand { get; set; }
        public String replacezustand { get; set; }
        public String isocode { get; set; }
    }
}