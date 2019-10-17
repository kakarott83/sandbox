using System;
using System.Globalization;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using Cic.OpenOne.GateBANKNOW.Common.Resources;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// BuchwertException-Klasse
    /// </summary>
    public class BuchwertException : ServiceBaseException
    {
        /// <summary>
        /// BuchwertException
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public BuchwertException(String code, String message, MessageType type)
            : base(code, message, type)
        {
        }
    }

    /// <summary>
    /// Buchwert Business Objekt
    /// </summary>
    public class BuchwertBo : AbstractBuchwertBo
    {
        private const string CnstBuchwertCode = "SOAP_HOLEBUCHWERTE_NFE";
        private const string CnstBuchwertPDFCode = "SOAP_HOLEBUCHWERTEPDF_NFE";
        public const String BW_PAR_FILE_CODE = "BUCHWERT_ALLOWED_WEB";
        public const String BW_CALC_ALLOWED = @" CASE
                                    WHEN (SELECT SUM (
                                        CASE
                                          WHEN NVL(
                                            (SELECT rn.sysrntyp
                                            FROM RN
                                            WHERE RN.sysid =
                                              (SELECT MAX(rn.sysid)
                                              FROM RN
                                              WHERE RN.SysRNTYP IN (85, 97,102, 131,140, 141, 261, 318, 327, 328, 303)
                                              AND RN.Text NOT LIKE '%Restrate'
                                              AND RN.StornoKZ = 0
                                              AND RN.SysVT    = VT.SysID
                                              )
                                            ),0) NOT IN (0,303)
                                          THEN 1
                                          ELSE 0
                                        END) cnt
                                      FROM rn
                                      WHERE rn.sysvt=vt.sysid)=0
                                      AND vt.sysvart=1
                                      AND UPPER(vt.zustand) in('AKTIV','GEKÜNDIGT')
                                      AND (vt.endekz is null or vt.endekz=0)
                                    THEN 1
                                    ELSE 0
                                  END";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eaihotDao"></param>
        public BuchwertBo(IEaihotDao eaihotDao, IObTypDao obTypDao)
            : base(eaihotDao, obTypDao)
        {
        }

        /// <summary>
        /// getBuchwert
        /// wenn currentSysVtruek gesetzt - > pdf aus jener Rückrechnung liefern
        /// 
        /// </summary>
        /// <param name="inputBw"></param>
        /// <returns></returns>
        override
        public ogetBuchwertDto getBuchwert(igetBuchwertDto inputBw)
        {
            try
            {
                VertragDao vertragDao = new VertragDao();

                long sysAreaid = vertragDao.getVertragSysId(inputBw.sysid);

                if (!eaihotDao.verifyAreaId(AreaConstants.Vt, sysAreaid))
                {
                    throw new ApplicationException("SysAreaId=" + sysAreaid + " not found in area " + AreaConstants.Vt + ".");
                }
                EaihotDto eaihotOutput = null;
                ogetBuchwertDto outputBw = new ogetBuchwertDto();
                bool sprache= (inputBw.sprache != null && inputBw.sprache.Trim().Length > 0) ;

                //new fast-return of existing vtruek Buchwerte
                if (!sprache && inputBw.currentSysVtruek > 0)
                {
                    try
                    {
                        eaihotOutput = eaihotDao.getEaihotByQuery("select * from eaihot where code = 'SOAP_HOLEBUCHWERTE_NFE' and oltable = 'VT' and outputparameter5 ='" + inputBw.currentSysVtruek + "' order by syseaihot desc");
                        if (eaihotOutput == null) throw new Exception("no vtruek found");
                    }catch(Exception)
                    {
                        throw new Exception("No recent Buchwertkalkulation for the given id " + inputBw.currentSysVtruek + " was found");
                    }
                }
                else
                {
                    EaihotDto eaihot = new EaihotDto();

                    // Set the table
                    eaihot.OLTABLE = AreaConstants.Vt.ToString().ToUpper(); ;

                    // Set the area id
                    eaihot.SYSOLTABLE = sysAreaid;

                    // Set the event engine enabled
                    eaihot.EVE = 1;
                    eaihot.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    eaihot.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));

                    // Set the document name, Angebotsnummer, Kundenname, Fahrzeug, Finanzierungsprodukt and Rate
                    //      eaihot.INPUTPARAMETER1 = DateTimeHelper.DateTimeToClarionDate(inputBw.perDatum).ToString();
                    eaihot.PROZESSSTATUS = (int)EaiHotStatusConstants.Pending;
                    eaihot.HOSTCOMPUTER = "*";
                    
                    eaihot.CODE = CnstBuchwertCode;
                    eaihot.INPUTPARAMETER2 = inputBw.perDatumCode;
                    
                    eaihot.INPUTPARAMETER3 = obTypDao.getPersonIDByPEROLE(inputBw.sysPerole).ToString();
                    if (sprache)
                    {
                        eaihot.CODE = CnstBuchwertPDFCode;

                        // SpracheId ermitteln
                        IDictionaryListsDao dictionaryListDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao();
                        DropListDto[] languageCode = dictionaryListDao.deliverCTLANG();
                        int sprachId = new int();

                        foreach (DropListDto spracheAusListe in languageCode)
                        {
                            if (spracheAusListe.code.Equals(inputBw.sprache))
                            {
                                sprachId = (int)spracheAusListe.sysID;
                                break;
                            }
                        }
                        if (sprachId == 0)
                        {
                            throw new Exception("Keine gültige Sprache gefunden für Dokumentensprache: " + inputBw.sprache + " für VertragId : " + inputBw.sysid);
                        }
                        eaihot.INPUTPARAMETER1 = sprachId.ToString();
                        eaihot.INPUTPARAMETER5 = ""+inputBw.currentSysVtruek;
                    }

                    eaihotOutput = eaihotDao.createEaihot(eaihot);

                    DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    TimeSpan timeOut = new TimeSpan(0, 0, 0, 120);
                    while (eaihotOutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                    {
                        eaihotOutput = eaihotDao.getEaihot(eaihotOutput.SYSEAIHOT);
                        System.Threading.Thread.Sleep(500);

                    }
                }

                if (eaihotOutput.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {
                    outputBw.BuchwertDto = new BuchwertDto();

                    
                    if (eaihotOutput.OUTPUTPARAMETER2 != null)
                    {
                        outputBw.BuchwertDto.buchwertBrutto = double.Parse(eaihotOutput.OUTPUTPARAMETER2.Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    if (eaihotOutput.OUTPUTPARAMETER1 != null)
                    {
                        outputBw.BuchwertDto.perDatum = DateTime.ParseExact(eaihotOutput.OUTPUTPARAMETER1.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        outputBw.BuchwertDto.perDatum = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    }

                    outputBw.BuchwertDto.vertrag = eaihotOutput.OUTPUTPARAMETER3;//vertragDao.getVertragNummer(inputBw.sysid); 
                    if (outputBw.BuchwertDto.vertrag != null)
                        outputBw.BuchwertDto.vertrag = outputBw.BuchwertDto.vertrag.Trim();

                    EaihfileDto eaiHFile = null;
                    if(inputBw.activeOfferSysEaihfile>0)//if given from outside use that eaihfile
                        eaiHFile = eaihotDao.getEaiHFile(inputBw.activeOfferSysEaihfile);
                    else
                        eaiHFile = eaihotDao.getEaiHotFile(eaihotOutput.SYSEAIHOT);

                    if (eaiHFile != null)
                    {
                        outputBw.BuchwertDto.hfile = eaiHFile.EAIHFILE;
                    }

                    if (eaihotOutput.OUTPUTPARAMETER4 != null && eaihotOutput.OUTPUTPARAMETER4.Length > 0)
                    {
                        if ("FALSE_DATE".Equals(eaihotOutput.OUTPUTPARAMETER4.Trim()))
                        {
                            throw new BuchwertException(ExceptionMessages.E_20002_BuchwertDateFailed, ExceptionMessages.E_20002_BuchwertDateFailed, MessageType.Info);
                        }
                        throw new BuchwertException(ExceptionMessages.E_20001_BuchwertFailed, ExceptionMessages.E_20001_BuchwertFailed, MessageType.Info);
                    }
                    
                    
                }
                else
                {
                    // Throw an exception
                    throw new ArgumentException("Could not get Buchwert. EAIHOT timeout");
                }
                return outputBw;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Could not get Buchwert. EAIHOT timeout", ex);
            }
            catch (BuchwertException ex)
            {
                throw ex;
            }
            catch (Exception exception)
            {
                // Throw an exception
                throw new ApplicationException("Could not get Buchwert", exception);
            }
        }

        /// <summary>
        /// Prüfung auf Timeout
        /// </summary>
        /// <param name="oldDate">Alter Zeitstempel</param>
        /// <param name="timeOut">NeuerZeitstempel</param>
        /// <returns>Timeout true= Timeout/false = kein Timeout</returns>
        public static bool isTimeOut(DateTime oldDate, TimeSpan timeOut)
        {
            TimeSpan ts = DateTime.Now - oldDate;

            if (ts > timeOut)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  isBuchwertCalculationAllowed
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        override public bool isBuchwertCalculationAllowed(long sysid)
        {
            VertragDao vertragDao = new VertragDao();
            VertragDto vertrag = new VertragDto();
            long sysAreaid = vertragDao.getVertragSysId(sysid);
            vertrag = vertragDao.getVertragDetails(sysAreaid);
            string zustand="";
            
            bool vertragInRechnung = vertragDao.restsaldoInRechnung(sysAreaid);
           
            //BRN9 CR 29 p.9
            //Der Vetrag hat kein EndeKZ und ist im aktiven Zustand. Der Zustand ist nicht pendent oder gekündigt. Der Restsaldo (inkl. dem Restwert) des Vertrags wurde noch nicht in Rechnung gestellt
            if (vertrag.zustand != null)
            {
                zustand = vertrag.zustand.ToUpper();
            }
            if ((vertrag.endekz != 1 || zustand.Contains("AKTIV") || zustand.Contains("GEKÜNDIGT")) && (zustand != "" && !zustand.Equals("PENDENT")) && vertrag.sysvart == 1 && !vertragInRechnung)
            {
                return true;
            }

            return false;

        }


    }
}