using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Dapper;
using System.Data.Common;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Model.DdCt;
using CIC.Database.IC.EF6.Model;
using Devart.Data.Oracle;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{

    public class DEERROROUTDto
    {

        //public long SYSDEERROROUT { get; set; }

        public long? SYSDEWBRKL { get; set; }

        public DateTime? INQUIRYDATE { get; set; }

        public long? INQUIRYTIME { get; set; }

        public long? CODE { get; set; }

        public string DESCRIPTION { get; set; }

        public string ENGINEVERSION { get; set; }

        public string ENGINESTACKTRACE { get; set; }

        public string JAVASTACKTRACE { get; set; }

    }

    public class DEWBSCOREDto
    {



        //public long SYSDEWBSCORE { get; set; }

        public long? SYSDEWBRKL { get; set; }

        public int? RANK { get; set; }

        public string SCORETYP { get; set; }

        public string BEZEICHNUNG { get; set; }

        public decimal? RESULTATWERT { get; set; }

        public string EINGABEWERT { get; set; }

    }

    public class DEENVOUTDto
    {

        public int? LAYOUTVERSION { get; set; }

        public long? SYSDEWBRKL { get; set; }

        public string SYSTEMDECISIONGROUP { get; set; }

        public string SYSTEMDECISION { get; set; }

        public string INQUIRYTIME { get; set; }

        public DateTime? INQUIRYDATE { get; set; }

        public int? PROCESSVERSION { get; set; }

        public string ORGANIZATIONCODE { get; set; }

        public string PROCESSCODE { get; set; }

        public string INQUIRYCODE { get; set; }

        public long? SYSDEOUTEXEC { get; set; }

        //public long SYSDEENVOUT { get; set; }

    }
    public class S1GetResponseDBDto
    {
        public S1GetResponseDBDto()
        {
            wbscores = new List<DEWBSCOREDto>();
        }
        public DEWBRKLDto dewbrkl { get; set; }

        //DEEnvOut.SYSDEWBRKL = sysdewbrkl;
        public DEENVOUTDto deenvout { get; set; }

        //DEErrorOou.SYSDEWBRKL = sysdewbrkl;
        public DEERROROUTDto deerrout { get; set; }

        //wbscore.SYSDEWBRKL = sysdewbrkl;
        public List<DEWBSCOREDto> wbscores { get; set; }

        public AuskunftStatus status { get; set; }

        public void insertToDb(DbCommand wbrkl, DbCommand envcmd, DbCommand errcmd, DbCommand scorecmd)
        {
            wbrkl.Parameters["SYSDEOUTEXEC"].Value = dewbrkl.SYSDEOUTEXEC;
            wbrkl.Parameters["SYSKD"].Value = dewbrkl.SYSKD;
            wbrkl.Parameters["SYSVT"].Value = dewbrkl.SYSVT;
            wbrkl.Parameters["RKLVT"].Value = dewbrkl.RKLVT;
            wbrkl.Parameters["WBKAPITAL"].Value = dewbrkl.WBKAPITAL;
            wbrkl.Parameters["WBKAPITALACP"].Value = dewbrkl.WBKAPITALACP;
            wbrkl.Parameters["WBZINS"].Value = dewbrkl.WBZINS;
            wbrkl.Parameters["WBZINSACP"].Value = dewbrkl.WBZINSACP;
            wbrkl.Parameters["PARAMETERID"].Value = dewbrkl.PARAMETERID;
            wbrkl.Parameters["PDT1"].Value = dewbrkl.PDT1;
            wbrkl.Parameters["PDT1FRAC"].Value = dewbrkl.PDT1FRAC;
            wbrkl.Parameters["FEHLERCODE"].Value = dewbrkl.FEHLERCODE;
            wbrkl.Parameters["FEHLERBESCHREIBUNG"].Value = dewbrkl.FEHLERBESCHREIBUNG;
            wbrkl.Parameters["PDEC901"].Value = dewbrkl.PDEC901;
            wbrkl.Parameters["PDEC902"].Value = dewbrkl.PDEC902;
            wbrkl.Parameters["PDEC903"].Value = dewbrkl.PDEC903;
            wbrkl.Parameters["DAT01"].Value = dewbrkl.DAT01;
            wbrkl.Parameters["DAT02"].Value = dewbrkl.DAT02;
            wbrkl.Parameters["DAT03"].Value = dewbrkl.DAT03;
            wbrkl.Parameters["STR01"].Value = dewbrkl.STR01;
            wbrkl.Parameters["STR02"].Value = dewbrkl.STR02;
            wbrkl.Parameters["STR03"].Value = dewbrkl.STR03;
            wbrkl.Parameters["LGD"].Value = dewbrkl.LGD;
            wbrkl.Parameters["EAD"].Value = dewbrkl.EAD;
            wbrkl.Parameters["EADRATIO"].Value = dewbrkl.EADRATIO;
            wbrkl.Parameters["PD_LT"].Value = dewbrkl.PD_LT;
            wbrkl.Parameters["RKL_LT"].Value = dewbrkl.RKL_LT;
            wbrkl.Parameters["WBKAPITAL_LT"].Value = dewbrkl.WBKAPITAL_LT;
            wbrkl.Parameters["WBZINS_LT"].Value = dewbrkl.WBZINS_LT;
            wbrkl.Parameters["SCOREBEZEICHNUNG"].Value = dewbrkl.SCOREBEZEICHNUNG;
            wbrkl.Parameters["SCOREVERSION"].Value = dewbrkl.SCOREVERSION;
            wbrkl.Parameters["SCOREWERT"].Value = dewbrkl.SCOREWERT;
            wbrkl.ExecuteNonQuery();

            int syswbrkl = Convert.ToInt32(wbrkl.Parameters["Id"].Value);
            deenvout.SYSDEWBRKL = syswbrkl;


            envcmd.Parameters["SYSDEWBRKL"].Value = deenvout.SYSDEWBRKL;
            envcmd.Parameters["INQUIRYCODE"].Value = deenvout.INQUIRYCODE;
            envcmd.Parameters["ORGANIZATIONCODE"].Value = deenvout.ORGANIZATIONCODE;
            envcmd.Parameters["PROCESSVERSION"].Value = deenvout.PROCESSVERSION;
            envcmd.Parameters["LAYOUTVERSION"].Value = deenvout.LAYOUTVERSION;
            envcmd.Parameters["INQUIRYDATE"].Value = deenvout.INQUIRYDATE;
            envcmd.Parameters["INQUIRYTIME"].Value = deenvout.INQUIRYTIME;
            envcmd.ExecuteNonQuery();

            if (deerrout != null)
            {
                deerrout.SYSDEWBRKL = syswbrkl;
                errcmd.Parameters["SYSDEWBRKL"].Value = deerrout.SYSDEWBRKL;
                errcmd.Parameters["INQUIRYDATE"].Value = deerrout.INQUIRYDATE;
                errcmd.Parameters["INQUIRYTIME"].Value = deerrout.INQUIRYTIME;
                errcmd.Parameters["CODE"].Value = deerrout.CODE;
                errcmd.Parameters["DESCRIPTION"].Value = deerrout.DESCRIPTION;
                errcmd.Parameters["ENGINEVERSION"].Value = deerrout.ENGINEVERSION;
                errcmd.Parameters["ENGINESTACKTRACE"].Value = deerrout.ENGINESTACKTRACE;
                errcmd.Parameters["JAVASTACKTRACE"].Value = deerrout.JAVASTACKTRACE;
                errcmd.ExecuteNonQuery();
            }
            foreach (DEWBSCOREDto score in wbscores)
            {
                score.SYSDEWBRKL = syswbrkl;

                scorecmd.Parameters["SYSDEWBRKL"].Value = score.SYSDEWBRKL;
                scorecmd.Parameters["RANK"].Value = score.RANK;
                scorecmd.Parameters["SCORETYP"].Value = score.SCORETYP;
                scorecmd.Parameters["BEZEICHNUNG"].Value = score.BEZEICHNUNG;
                scorecmd.Parameters["RESULTATWERT"].Value = score.RESULTATWERT;
                scorecmd.Parameters["EINGABEWERT"].Value = score.EINGABEWERT;
                scorecmd.ExecuteNonQuery();
            }
        }
    }
    public class DEWBRKLDto
    {


        //
        // Zusammenfassung:
        //     Description: Reserve


        public DateTime? DAT01 { get; set; }
        //
        // Zusammenfassung:
        //     Description: Reserve


        public string STR01 { get; set; }
        //
        // Zusammenfassung:
        //     Description: Reserve


        public string STR02 { get; set; }
        //
        // Zusammenfassung:
        //     Description: Reserve


        public string STR03 { get; set; }
        //
        // Zusammenfassung:
        //     Description: LGD


        public decimal? LGD { get; set; }
        //
        // Zusammenfassung:
        //     Description: EAD


        public decimal? EAD { get; set; }
        //
        // Zusammenfassung:
        //     Description: PD_LT


        public decimal? PD_LT { get; set; }
        //
        // Zusammenfassung:
        //     Description: Risikoklasse_LT


        public int? RKL_LT { get; set; }
        //
        // Zusammenfassung:
        //     Description: Kapital_WB_LT


        public decimal? WBKAPITAL_LT { get; set; }
        //
        // Zusammenfassung:
        //     Description: Zins_WB_LT


        public decimal? WBZINS_LT { get; set; }
        //
        // Zusammenfassung:
        //     Description: Scorebezeichnung


        public string SCOREBEZEICHNUNG { get; set; }
        //
        // Zusammenfassung:
        //     Description: Scoreversion


        public string SCOREVERSION { get; set; }
        //
        // Zusammenfassung:
        //     Description: Scorewert


        public int? SCOREWERT { get; set; }

        //
        // Zusammenfassung:
        //     Description: Reserve


        public DateTime? DAT03 { get; set; }
        //
        // Zusammenfassung:
        //     Description: Reserve


        public DateTime? DAT02 { get; set; }


        public decimal? PDEC903 { get; set; }
        //
        // Zusammenfassung:
        //     Primary Key: True


        // public long SYSDEWBRKL { get; set; }
        //
        // Zusammenfassung:
        //     Foreign key: True Navigation property: DEOUTEXEC Description: Verknüpfung zur
        //     DEOUTEXEC


        public long? SYSDEOUTEXEC { get; set; }
        //
        // Zusammenfassung:
        //     Description: Verknüpfung zum Kunden


        public long? SYSKD { get; set; }
        //
        // Zusammenfassung:
        //     Description: Verknüpfung zum Vertrag


        public long? SYSVT { get; set; }
        //
        // Zusammenfassung:
        //     Description: Verknüpfung zur Risikoklasse des Vertrags


        public long? RKLVT { get; set; }
        //
        // Zusammenfassung:
        //     Description: Wertberichtigung Kapital


        public decimal? WBKAPITAL { get; set; }
        //
        // Zusammenfassung:
        //     Description: Wertberichtigung Kapital ACP


        public decimal? WBKAPITALACP { get; set; }

        //
        // Zusammenfassung:
        //     Description: Wertberichtigung Zins


        public decimal? WBZINS { get; set; }
        //
        // Zusammenfassung:
        //     Description: Parameter-ID


        public long? PARAMETERID { get; set; }
        //
        // Zusammenfassung:
        //     Description: Probability of Default t1


        public decimal? PDT1 { get; set; }
        //
        // Zusammenfassung:
        //     Description: Probability of Default t1 frac


        public decimal? PDT1FRAC { get; set; }
        //
        // Zusammenfassung:
        //     Description: Error-Code


        public long? FEHLERCODE { get; set; }
        //
        // Zusammenfassung:
        //     Description: Error-Description


        public string FEHLERBESCHREIBUNG { get; set; }
        //
        // Zusammenfassung:
        //     Description: Reserve


        public decimal? PDEC901 { get; set; }
        //
        // Zusammenfassung:
        //     Description: Reserve


        public decimal? PDEC902 { get; set; }
        //
        // Zusammenfassung:
        //     Description: Wertberichtigung Zins ACP


        public decimal? WBZINSACP { get; set; }

        public decimal? EADRATIO { get; set; }

    }

    /// <summary>
    /// Status from AUSKUNFT:Status
    /// </summary>
    public enum AuskunftStatus : int
    {
        /// <summary>
        /// Fachlicher Fehler in einem der Verträge
        /// </summary>
        ErrorInData = 1,

        /// <summary>
        /// Technischer Fehler in einem der Verträge
        /// </summary>
        ErrorTech = 2,

        /// <summary>
        /// correctly sent data from BOS to Simple Services
        /// </summary>
        OKSendToSS = 3,

        /// <summary>
        /// data sent from BOS to Simple Services.  Answare from Simple Services: Error in structure of the data
        /// </summary>
        OKSendToSSErrorInData = 4,

        /// <summary>
        /// Aufruf erfolgreich
        /// </summary>
        OK = 0,

        /// <summary>
        /// Fehler S1/Batchprogram
        /// </summary>
        ErrorS1BatchProgram = -1,

        /// <summary>
        /// SimpleService/CIC Webservice nicht erreichbar
        /// </summary>
        ErrorSimpleService = -2,

        /// <summary>
        /// Strukturen noch nicht vollständig angelegt (vor Aufruf der Schnittstelle, während Anlage der Daten)
        /// </summary>
        ErrorStructur = -3,

        /// <summary>
        /// Strukturen vollständig angelegt, bereit zur Verarbeitung
        /// </summary>
        StrucktOKAndReady = -4
    }

    /// <summary>
    /// S1 DB Data Access Object
    /// </summary>
    public class RISKEWBS1DBDao : IRISKEWBS1DBDao
    {

        #region fields and Enum

        /// <summary>
        /// ErrorText
        /// </summary>
        public string ErrorText
        {
            get
            {
                return errorText;
            }
        }

        /// <summary>
        /// sys WfSUer
        /// </summary>
        public long SysWFUSER;



        /// <summary>
        /// _log
        /// </summary>
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// sysid from sysAuskunf
        /// </summary>
        private long sysAuskunft;

        /// <summary>
        /// local var for Error Text
        /// </summary>
        private string errorText;

        #endregion and Enum

        #region  Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SysAuskunft"></param>
        public RISKEWBS1DBDao(long SysAuskunft)
        {
            this.sysAuskunft = SysAuskunft;
            this.errorText = string.Empty;
            this.SysWFUSER = 0;

            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // AUSKUNFT
                    AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    if (Auskunft.SYSWFUSER.HasValue)
                    {
                        this.SysWFUSER = (long)Auskunft.SYSWFUSER;
                    }
                    else
                    {
                        this.SysWFUSER = 0;
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Error by Create RISKEWBS1DBDao", ex);
                    throw ex;
                }
            }

        }
        #endregion

        #region public methods

        /// <summary>
        /// 0 - ok, 1 - error - DEPRECATED, 
        /// </summary>
        /// <param name="listS1InDto"></param>
        /// <returns></returns>
        public long GetDataFromDB(ref List<RISKEWBS1DataDto> listS1InDto)
        {
            long ret = 1;
            using (DdCtExtended context = new DdCtExtended())
            {
                errorText = "Error by RISKEWBS1DBDao.GetDataFromDB";
                String queryString = String.Empty;

                queryString =
                    "SELECT 1 as CallNumber, 1 as RandomNumber, deenvinp.processcode DecisionProcessCode, InquiryCode, OrganizationCode, ProcessVersion, InquiryDate, InquiryTime, " +
                        "Str01 I_C_VARCHAR_F01, Str02 I_C_VARCHAR_F02, Str03 I_C_VARCHAR_F03, Str04 I_C_VARCHAR_F04, Str05 I_C_VARCHAR_F05, Str06 I_C_VARCHAR_F06, Str07 I_C_VARCHAR_F07, Str08 I_C_VARCHAR_F08, Str09 I_C_VARCHAR_F09, Str10 I_C_VARCHAR_F10, Str11 I_C_VARCHAR_F11, Str12 I_C_VARCHAR_F12, Str13 I_C_VARCHAR_F13, Str14 I_C_VARCHAR_F14, Str15 I_C_VARCHAR_F15, " +
                        "Dat01 I_C_DATE_F16, Dat02 I_C_DATE_F17, Dat03 I_C_DATE_F18, Dat04 I_C_DATE_F19, Dat05 I_C_DATE_F20, Dat06 I_C_DATE_F21, Dat07 I_C_DATE_F22, Dat08 I_C_DATE_F23, Dat09 I_C_DATE_F24, Dat10 I_C_DATE_F25, Dat11 I_C_DATE_F26, Dat12 I_C_DATE_F27, Dat13 I_C_DATE_F28, Dat14 I_C_DATE_F29, Dat15 I_C_DATE_F30, Dat16 I_C_DATE_F31, Dat17 I_C_DATE_F32, Dat18 I_C_DATE_F33, Dat19 I_C_DATE_F34, Dat20 I_C_DATE_F35, " +
                        "Pdec1501 I_C_NUMBER_152_F36, Pdec1502 I_C_NUMBER_152_F37, Pdec1503 I_C_NUMBER_152_F38, Pdec1504 I_C_NUMBER_152_F39, Pdec1505 I_C_NUMBER_152_F40, Pdec1506 I_C_NUMBER_152_F41, Pdec1507 I_C_NUMBER_152_F42, Pdec1508 I_C_NUMBER_152_F43, Pdec1509 I_C_NUMBER_152_F44, Pdec1510 I_C_NUMBER_152_F45, Pdec1511 I_C_NUMBER_152_F46, Pdec1512 I_C_NUMBER_152_F47, Pdec1513 I_C_NUMBER_152_F48, Pdec1514 I_C_NUMBER_152_F49, Pdec1515 I_C_NUMBER_152_F50, Pdec1516 I_C_NUMBER_152_F51, Pdec1517 I_C_NUMBER_152_F52, Pdec1518 I_C_NUMBER_152_F53, Pdec1519 I_C_NUMBER_152_F54, Pdec1520 I_C_NUMBER_152_F55, Pdec1521 I_C_NUMBER_152_F56, Pdec1522 I_C_NUMBER_152_F57, Pdec1523 I_C_NUMBER_152_F58, Pdec1524 I_C_NUMBER_152_F59, Pdec1525 I_C_NUMBER_152_F60, " +
                        "Int01 I_C_NUMBER_120_F61, Int02 I_C_NUMBER_120_F62, Int03 I_C_NUMBER_120_F63, Int04 I_C_NUMBER_120_F64, Int05 I_C_NUMBER_120_F65, Int06 I_C_NUMBER_120_F66, Int07 I_C_NUMBER_120_F67, Int08 I_C_NUMBER_120_F68, Int09 I_C_NUMBER_120_F69, Int10 I_C_NUMBER_120_F70, " +
                        "Flag01 I_C_NUMBER_030_F71, Flag02 I_C_NUMBER_030_F72, Flag03 I_C_NUMBER_030_F73, Flag04 I_C_NUMBER_030_F74, Flag05 I_C_NUMBER_030_F75, Flag06 I_C_NUMBER_030_F76, Flag07 I_C_NUMBER_030_F77, Flag08 I_C_NUMBER_030_F78, Flag09 I_C_NUMBER_030_F79, Flag10 I_C_NUMBER_030_F80 " +
                    "FROM cic.dewbin, cic.deenvinp " +
                    "WHERE cic.dewbin.sysdewbin = cic.deenvinp.sysdewbin AND cic.dewbin.sysdeinpexec IN (SELECT sysdeinpexec FROM cic.deinpexec WHERE sysauskunft = :pSysAuskunft)";

                Cic.OpenOne.CarConfigurator.DAO.EaiparDao eaiParDao = new Cic.OpenOne.CarConfigurator.DAO.EaiparDao();
                queryString = eaiParDao.getEaiParFileByCode("RISKEWB_DB2S1", queryString);

                List<object> parameters = new List<object>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysAuskunft", Value = sysAuskunft });
                listS1InDto = context.ExecuteStoreQuery<RISKEWBS1DataDto>(queryString, parameters.ToArray()).ToList();
                ret = 0;
            }
            return ret;
        }



        public static object getPropertyValue(object srcObj, Type t, string propertyName)
        {
            if (srcObj == null)
            {
                return null;
            }
            PropertyInfo pi = srcObj.GetType().GetProperty(propertyName.Replace("[]", ""));
            if (pi == null)
            {
                return null;
            }
            return pi.GetValue(srcObj);
        }

        private void addWBSCORE(DdIcExtended context, S1GetResponseDto s1Dto, DEWBRKL DEWbRkl)
        {
            Type t = s1Dto.GetType();

            for (int i = 1; i <= 20; i++)
            {

                DEWBSCORE wbscore = new DEWBSCORE();
                wbscore.DEWBRKL = DEWbRkl;

                wbscore.RANK = Convert.ToInt32(RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_ID_" + i));
                wbscore.SCORETYP = (String)RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_Scoretyp_" + i);
                wbscore.BEZEICHNUNG = (String)RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_Bezeichnung_" + i);
                wbscore.RESULTATWERT = Convert.ToDecimal(RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_Resultatwert_" + i));
                wbscore.EINGABEWERT = (String)RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_Eingabewert_" + i);

                context.DEWBSCORE.Add(wbscore);

            }


        }
        private void addWBSCORE2(S1GetResponseDto s1Dto, S1GetResponseDBDto result)
        {
            Type t = s1Dto.GetType();

            for (int i = 1; i <= 20; i++)
            {

                try
                {
                    DEWBSCOREDto wbscore = new DEWBSCOREDto();
                    wbscore.RANK = Convert.ToInt32(RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_ID_" + i));

                    //skip rank <1
                    if (wbscore.RANK == null || !wbscore.RANK.HasValue || wbscore.RANK.Value == 0)
                        continue;

                    wbscore.SCORETYP = (String)RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_Scoretyp_" + i);
                    wbscore.BEZEICHNUNG = (String)RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_Bezeichnung_" + i);
                    wbscore.RESULTATWERT = Convert.ToDecimal(RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_Resultatwert_" + i));
                    wbscore.EINGABEWERT = (String)RISKEWBS1DBDao.getPropertyValue(s1Dto, t, "O_SC_Eingabewert_" + i);
                    result.wbscores.Add(wbscore);
             
                }catch (Exception e)
                {
                    _log.Error("Failed to process DEWBSCORE #"+i, e);
                }
            }


        }

        /// <summary>
        /// add one line of the stream, converted to a dto to the database
        /// </summary>
        /// <param name="SYSDEOUTEXEC"></param>
        /// <param name="s1Dto"></param>
        /// <returns></returns>
        public S1GetResponseDBDto addStreamedS1Response(long SYSDEOUTEXEC, S1GetResponseDto s1Dto)
        {
            S1GetResponseDBDto result = new S1GetResponseDBDto();
            try
            {
                result.status = AuskunftStatus.OK;
                if (s1Dto.O_ErrorCode != 0)
                {
                    result.status = AuskunftStatus.ErrorInData; 
                }

                // DEERROROUT
                if (s1Dto.S1Error_Code != "" && s1Dto.S1Error_Code != null)
                {
                    result.status = AuskunftStatus.ErrorTech; 

                    DEERROROUTDto DEErrorOou = new DEERROROUTDto();
                    this.myFillDEERROUT(DEErrorOou, s1Dto);
                    result.deerrout = DEErrorOou;
                }


                DEWBRKLDto DEWbRkl = new DEWBRKLDto();
                DEWbRkl.SYSDEOUTEXEC = SYSDEOUTEXEC;
                this.myFillDEWBRKL2(DEWbRkl, s1Dto);
                result.dewbrkl = DEWbRkl;

                // DEENVOUT
                DEENVOUTDto DEEnvOut = new DEENVOUTDto();
                this.myFillDEENVOUT2(DEEnvOut, s1Dto);
                result.deenvout = DEEnvOut;
                addWBSCORE2(s1Dto, result);


               

            }
            catch (Exception e)
            {
                _log.Error("Failed to insert DEENVOUT", e);
                result.status = AuskunftStatus.ErrorTech; ;

            }
            return result;

        }

        /// <summary>
        /// check data from s1
        /// </summary>
        /// <param name="listS1InDto"></param>
        /// <returns></returns>
        public long ValidateAnswerData(List<S1GetResponseDto> listS1InDto)
        {
            long ret = 1;
            int _fehlerCode = (int)AuskunftStatus.OKSendToSS;

            using (DdIcExtended context = new DdIcExtended())
            {

                // AUSKUNFT
                AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                if (Auskunft.FEHLERCODE != _fehlerCode.ToString())
                {
                    if (Auskunft.FEHLERCODE == null)
                    {
                        errorText = "Invalid value AUSKUNFT.FEHLERCODE = NULL for sysAuskunf = " + sysAuskunft;
                    }
                    else
                    {
                        errorText = "Invalid value AUSKUNFT.FEHLERCODE = " + Auskunft.FEHLERCODE + " for sysAuskunf = " + sysAuskunft;
                    }

                    return ret;
                }

                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listS1InDto"></param>
        /// <returns></returns>
        public long SaveDataFromS1(List<S1GetResponseDto> listS1InDto)
        {
            long ret = 1;

            using (DdIcExtended context = new DdIcExtended())
            {
                Boolean errorTech = false;
                Boolean errorVT = false;
                int _int;
                try
                {
                    // AUSKUNFT
                    AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                    // DEOUTEXEC
                    DEOUTEXEC DEOutExec = new DEOUTEXEC();
                    DEOutExec.AUSKUNFT = Auskunft;
                    context.DEOUTEXEC.Add(DEOutExec);
                    int i = 0;
                    foreach (S1GetResponseDto s1Dto in listS1InDto)
                    {
                        // DEWBRKL
                        if (s1Dto.O_ErrorCode != 0)
                        {
                            errorVT = true;
                        }

                        DEWBRKL DEWbRkl = new DEWBRKL();
                        DEWbRkl.DEOUTEXEC = DEOutExec;
                        this.myFillDEWBRKL(DEWbRkl, s1Dto);
                        context.DEWBRKL.Add(DEWbRkl);

                        // DEENVOUT
                        DEENVOUT DEEnvOut = new DEENVOUT();
                        DEEnvOut.DEWBRKL = DEWbRkl;
                        this.myFillDEENVOUT(DEEnvOut, s1Dto);
                        context.DEENVOUT.Add(DEEnvOut);

                        addWBSCORE(context, s1Dto, DEWbRkl);

                        // DEERROROUT
                        if (s1Dto.S1Error_Code != "" && s1Dto.S1Error_Code != null)
                        {
                            errorTech = true;
                            DEERROROUT DEErrorOou = new DEERROROUT();
                            DEErrorOou.DEWBRKL = DEWbRkl;
                            this.myFillDEWBRKL(DEErrorOou, s1Dto);
                            context.DEERROROUT.Add(DEErrorOou);
                        }
                        if (i % 10000 == 0)
                        {
                            context.SaveChanges();
                        }
                        i++;
                    }
                    context.SaveChanges();

                    // AUSKUNFT: status + fahlercode
                    if (errorTech == true)
                    {
                        _int = (int)AuskunftStatus.ErrorTech;
                        Auskunft.FEHLERCODE = _int.ToString();
                        //Auskunft.STATUS = status;
                    }
                    else
                    {
                        if (errorVT == true)
                        {
                            //_int = (int)AuskunftStatus.ErrorStructur;
                            _int = (int)AuskunftStatus.ErrorInData;
                            Auskunft.FEHLERCODE = _int.ToString();
                            //Auskunft.STATUS = status;
                        }
                        else
                        {
                            _int = (int)AuskunftStatus.OK;
                            Auskunft.FEHLERCODE = _int.ToString();
                            //Auskunft.STATUS = status;
                        }

                    }

                    context.SaveChanges();
                    ret = 0;
                }
                catch (Exception ex)
                {
                    errorText = ex.Message;
                }
            }

            return ret;
        }

        /// <summary>
        /// Save Status of the Auskunft
        /// </summary>
        /// <param name="FehlerCode"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public long SaveAuskunftStatus(AuskunftStatus FehlerCode, string status)
        {
            long ret = 0;

            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // AUSKUNFT
                    AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                    if (Auskunft.AUSKUNFTTYP == null)
                        context.Entry(Auskunft).Reference(f => f.AUSKUNFTTYP).Load();

                    string bezeichnungTyp = "";
                    if (Auskunft.AUSKUNFTTYP != null)
                    {
                        bezeichnungTyp = Auskunft.AUSKUNFTTYP.BEZEICHNUNG;
                    }

                    int _int = (int)FehlerCode;
                    Auskunft.FEHLERCODE = _int.ToString();

                    if (FehlerCode >= 0)
                        Auskunft.STATUS = bezeichnungTyp + " - Aufruf erfolgreich";
                    else
                        Auskunft.STATUS = bezeichnungTyp + " - Aufruf nicht erfolgreich";

                    context.SaveChanges();
                    ret = 1;
                }
                catch (Exception ex)
                {
                    _log.Error("Error by SaveAuskunftStatus", ex);
                    throw ex;
                }
            }

            return ret;
        }

        #endregion

        #region local methods

        /// <summary>
        /// get amount recors in DEWBIN for Auskunf
        /// </summary>
        /// <returns></returns>
        private long myGetAmountSendRecInAuskunf()
        {
            long ret = 0;
            using (DdCtExtended context = new DdCtExtended())
            {
                String queryString = String.Empty;
                queryString = "SELECT COUNT(1) FROM cic.deinpexec, cic.deenvinp WHERE deinpexec.sysdeinpexec = deenvinp.sysdeinpexec AND deinpexec.sysauskunft = :pSysAuskunft";
                //queryString = "SELECT COUNT(1) FROM cic.deinpexec, cic.deenvinp WHERE deinpexec.sysdeinpexec = deenvinp.sysdeinpexec AND deinpexec.sysauskunft = 899";

                //List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysAuskunft", Value = sysAuskunft });

                DbParameter[] parameters = { new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysAuskunft", Value = sysAuskunft }, };

                ret = context.ExecuteStoreQuery<long>(queryString, parameters).FirstOrDefault();
                //ret = context.ExecuteStoreQuery<long>(queryString, null).FirstOrDefault();
            }

            return ret;
        }

        private void myFillDEENVOUT(DEENVOUT deEnvOut, S1GetResponseDto s1Dto)
        {
            deEnvOut.INQUIRYCODE = s1Dto.InquiryCode;
            deEnvOut.ORGANIZATIONCODE = s1Dto.OrganizationCode;
            deEnvOut.PROCESSVERSION = (int)s1Dto.ProcessVersion;
            deEnvOut.LAYOUTVERSION = (int)s1Dto.LayoutVersion;
            if (s1Dto.InquiryDate.Date.Year == 1)
            {
                deEnvOut.INQUIRYDATE = null;
                deEnvOut.INQUIRYTIME = null;
            }
            else
            {
                deEnvOut.INQUIRYDATE = s1Dto.InquiryDate.Date;
                deEnvOut.INQUIRYTIME = DateTimeHelper.DateTimeToClarionTime(s1Dto.InquiryDate).ToString();
            }

        }
        private void myFillDEENVOUT2(DEENVOUTDto deEnvOut, S1GetResponseDto s1Dto)
        {
            try { 
                deEnvOut.INQUIRYCODE = s1Dto.InquiryCode;
                deEnvOut.ORGANIZATIONCODE = s1Dto.OrganizationCode;
                deEnvOut.PROCESSVERSION = (int)s1Dto.ProcessVersion;
                deEnvOut.LAYOUTVERSION = (int)s1Dto.LayoutVersion;
                if (s1Dto.InquiryDate.Date.Year == 1)
                {
                    deEnvOut.INQUIRYDATE = null;
                    deEnvOut.INQUIRYTIME = null;
                }
                else
                {
                    deEnvOut.INQUIRYDATE = s1Dto.InquiryDate.Date;
                    deEnvOut.INQUIRYTIME = DateTimeHelper.DateTimeToClarionTime(s1Dto.InquiryDate).ToString();
                }
            }
            catch (Exception e)
            {
                _log.Error("Failed to process DEENVOUT ", e);
            }
        }

        private void myFillDEWBRKL(DEWBRKL deWbRkl, S1GetResponseDto s1Dto)
        {
            deWbRkl.SYSKD = (int)s1Dto.O_syskd;
            deWbRkl.SYSVT = (int)s1Dto.O_sysVT;
            deWbRkl.RKLVT = (int)s1Dto.O_DEC_Risikoklasse_VT;
            deWbRkl.WBKAPITAL = s1Dto.O_DEC_Kapital_WB;
            deWbRkl.WBKAPITALACP = s1Dto.O_DEC_Kapital_WB_ACP;
            deWbRkl.WBZINS = s1Dto.O_DEC_Zins_WB;
            deWbRkl.WBZINSACP = s1Dto.O_DEC_Zins_WB_ACP;
            deWbRkl.PARAMETERID = (int)s1Dto.O_DEC_Parameter_ID;
            deWbRkl.PDT1 = s1Dto.O_DEC_PDt1;
            deWbRkl.PDT1FRAC = s1Dto.O_DEC_PDt1_frac;
            deWbRkl.FEHLERCODE = (int)s1Dto.O_ErrorCode;
            deWbRkl.FEHLERBESCHREIBUNG = s1Dto.O_ErrorDescription;
            deWbRkl.PDEC901 = s1Dto.O_Reserve_Number_1;
            deWbRkl.PDEC902 = s1Dto.O_Reserve_Number_2;
            deWbRkl.PDEC903 = s1Dto.O_Reserve_Number_3;
            deWbRkl.DAT01 = s1Dto.O_Reserve_Date_1;
            deWbRkl.DAT02 = s1Dto.O_Reserve_Date_2;
            deWbRkl.DAT03 = s1Dto.O_Reserve_Date_3;
            deWbRkl.STR01 = s1Dto.O_Reserve_Text_1;
            deWbRkl.STR02 = s1Dto.O_Reserve_Text_2;
            deWbRkl.STR03 = s1Dto.O_Reserve_Text_3;


            deWbRkl.LGD = s1Dto.O_DEC_LGD;
            deWbRkl.EAD = s1Dto.O_DEC_EAD;
            deWbRkl.EADRATIO = s1Dto.O_DEC_EAD_RATIO;
            deWbRkl.PD_LT = s1Dto.O_DEC_PD_LT;
            deWbRkl.RKL_LT = (int)s1Dto.O_DEC_Risikoklasse_LT;
            deWbRkl.WBKAPITAL_LT = s1Dto.O_DEC_Kapital_WB_LT;
            deWbRkl.WBZINS_LT = s1Dto.O_DEC_Zins_WB_LT;
            deWbRkl.SCOREBEZEICHNUNG = s1Dto.O_DEC_Scorebezeichnung;
            deWbRkl.SCOREVERSION = s1Dto.O_DEC_Scoreversion;
            deWbRkl.SCOREWERT = (int)s1Dto.O_DEC_Scorewert;

        }
        private void myFillDEWBRKL2(DEWBRKLDto deWbRkl, S1GetResponseDto s1Dto)
        {
            try
            {
                deWbRkl.SYSKD = (int)s1Dto.O_syskd;
                deWbRkl.SYSVT = (int)s1Dto.O_sysVT;
                deWbRkl.RKLVT = (int)s1Dto.O_DEC_Risikoklasse_VT;
                deWbRkl.WBKAPITAL = s1Dto.O_DEC_Kapital_WB;
                deWbRkl.WBKAPITALACP = s1Dto.O_DEC_Kapital_WB_ACP;
                deWbRkl.WBZINS = s1Dto.O_DEC_Zins_WB;
                deWbRkl.WBZINSACP = s1Dto.O_DEC_Zins_WB_ACP;
                deWbRkl.PARAMETERID = (int)s1Dto.O_DEC_Parameter_ID;
                deWbRkl.PDT1 = s1Dto.O_DEC_PDt1;
                deWbRkl.PDT1FRAC = s1Dto.O_DEC_PDt1_frac;
                deWbRkl.FEHLERCODE = (int)s1Dto.O_ErrorCode;
                deWbRkl.FEHLERBESCHREIBUNG = s1Dto.O_ErrorDescription;
                deWbRkl.PDEC901 = s1Dto.O_Reserve_Number_1;
                deWbRkl.PDEC902 = s1Dto.O_Reserve_Number_2;
                deWbRkl.PDEC903 = s1Dto.O_Reserve_Number_3;
                deWbRkl.DAT01 = s1Dto.O_Reserve_Date_1;
                deWbRkl.DAT02 = s1Dto.O_Reserve_Date_2;
                deWbRkl.DAT03 = s1Dto.O_Reserve_Date_3;
                deWbRkl.STR01 = s1Dto.O_Reserve_Text_1;
                deWbRkl.STR02 = s1Dto.O_Reserve_Text_2;
                deWbRkl.STR03 = s1Dto.O_Reserve_Text_3;


                deWbRkl.LGD = s1Dto.O_DEC_LGD;
                deWbRkl.EAD = s1Dto.O_DEC_EAD;
                deWbRkl.EADRATIO = s1Dto.O_DEC_EAD_RATIO;
                deWbRkl.PD_LT = s1Dto.O_DEC_PD_LT;
                deWbRkl.RKL_LT = (int)s1Dto.O_DEC_Risikoklasse_LT;
                deWbRkl.WBKAPITAL_LT = s1Dto.O_DEC_Kapital_WB_LT;
                deWbRkl.WBZINS_LT = s1Dto.O_DEC_Zins_WB_LT;
                deWbRkl.SCOREBEZEICHNUNG = s1Dto.O_DEC_Scorebezeichnung;
                deWbRkl.SCOREVERSION = s1Dto.O_DEC_Scoreversion;
                deWbRkl.SCOREWERT = (int)s1Dto.O_DEC_Scorewert;
            }catch(Exception e)
            {
                _log.Error("Failed to process DEWBRKL ", e);
            }
        }

        private void myFillDEWBRKL(DEERROROUT deErrorOut, S1GetResponseDto s1Dto)
        {
            try { 
                deErrorOut.INQUIRYDATE = s1Dto.S1Error_InquiryDate.Date;
                deErrorOut.INQUIRYTIME = DateTimeHelper.DateTimeToClarionTime(s1Dto.S1Error_InquiryDate);
                if (s1Dto.S1Error_Code != "")
                {
                    deErrorOut.CODE = Convert.ToInt32(s1Dto.S1Error_Code);
                }
                deErrorOut.DESCRIPTION = s1Dto.S1Error_Description;
                deErrorOut.ENGINEVERSION = s1Dto.S1Error_EngineVersion;
                deErrorOut.ENGINESTACKTRACE = s1Dto.S1Error_EngineStackTrace;
                deErrorOut.JAVASTACKTRACE = s1Dto.S1Error_JavaStackTrace;
            }catch(Exception e)
            {
                _log.Error("Failed to process DEERROROUT ", e);
            }
}
        private void myFillDEERROUT(DEERROROUTDto deErrorOut, S1GetResponseDto s1Dto)
        {
            deErrorOut.INQUIRYDATE = s1Dto.S1Error_InquiryDate.Date;
            deErrorOut.INQUIRYTIME = DateTimeHelper.DateTimeToClarionTime(s1Dto.S1Error_InquiryDate);
            if (s1Dto.S1Error_Code != "")
            {
                deErrorOut.CODE = Convert.ToInt32(s1Dto.S1Error_Code);
            }
            deErrorOut.DESCRIPTION = Truncate(s1Dto.S1Error_Description, 255);
            deErrorOut.ENGINEVERSION = Truncate(s1Dto.S1Error_EngineVersion, 255);
            deErrorOut.ENGINESTACKTRACE = Truncate(s1Dto.S1Error_EngineStackTrace, 255);
            deErrorOut.JAVASTACKTRACE = Truncate(s1Dto.S1Error_JavaStackTrace, 255);
        }

        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        #endregion

    }
}
